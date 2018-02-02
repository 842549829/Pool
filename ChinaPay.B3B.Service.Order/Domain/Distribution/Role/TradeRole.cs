using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Postpone;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Postpone;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    /// <summary>
    /// 交易角色
    /// </summary>
    public abstract class TradeRole : BaseRole {
        decimal? _rate = null;

        protected TradeRole(Guid id)
            : base(id) {
        }
        protected TradeRole(Guid id, string account, decimal rate)
            : base(id, account) {
            _rate = rate;
        }

        public abstract TradeRoleType RoleType { get; }
        /// <summary>
        /// 交易手续费率
        /// </summary>
        public decimal Rate {
            get {
                if(!_rate.HasValue) {
                    throw new InvalidOperationException("缺少交易费率");
                }
                return _rate.Value;
            }
        }
        /// <summary>
        /// 获取交易手续费率
        /// </summary>
        protected abstract decimal GetTradeRate(TradeInfo trade);
        /// <summary>
        /// 获取改期时的交易手续费
        /// </summary>
        protected virtual decimal GetPostponeTradeFee() {
            return 0;
        }
        /// <summary>
        /// 计算交易手续费
        /// </summary>
        protected virtual decimal CalcTradeFee(decimal anticipation) {
            return ProcessTradeFee(anticipation * Rate * -1);
        }
        protected virtual decimal ProcessTradeFee(decimal tradeFee) {
            return Utility.Calculator.Ceiling(tradeFee, -2);
        }
        protected decimal GetTradeRate(SpecialProductType productType, Data.DataMapping.CompanyParameter parameter) {
            switch(productType) {
                case SpecialProductType.Singleness:
                    return parameter.SinglenessRate;
                case SpecialProductType.Disperse:
                    return parameter.DisperseRate;
                case SpecialProductType.CostFree:
                    return parameter.CostFreeRate;
                case SpecialProductType.Bloc:
                    return parameter.BlocRate;
                case SpecialProductType.Business:
                    return parameter.BusinessRate;
                case SpecialProductType.OtherSpecial:
                    return parameter.OtherSpecialRate;
                case SpecialProductType.LowToHigh:
                    return parameter.LowToHighRate;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 生成支付账单
        /// </summary>
        internal NormalPayRoleBill MakePayBill(TradeInfo trade, Deduction deduction) {
            _rate = GetTradeRate(trade);
            var detailBills = (from flight in trade.Flights
                               from passenger in trade.Passengers
                               select makePayDetailBill(trade.IsThirdRelation, passenger, flight, deduction)).ToList();
            var roleBill = new NormalPayRoleBill(this) {
                Source = new NormalPayRoleBillSource {
                    Details = detailBills,
                    Rebate = deduction == null ? 0 : deduction.Rebate,
                },
            };
            return roleBill;
        }
        internal void RefreshPrice(TradeInfo trade, NormalPayRoleBill roleBill) {
            var rebate = roleBill.Source.Rebate;
            var firstPassengerId = roleBill.Source.Details.First().Passenger;
            var singleDetailBills = roleBill.Source.Details.Where(d => d.Passenger == firstPassengerId);
            var singleTotalCommission = singleDetailBills.Sum(d => d.Commission);
            var singleTotalTradeFee = singleDetailBills.Sum(d => d.TradeFee);
            var detailBills = new List<NormalPayDetailBill>();
            var totalFlightCount = trade.Flights.Count();
            foreach(var passenger in trade.Passengers) {
                var residualCommission = singleTotalCommission;
                var residualTradeFee = singleTotalTradeFee;
                var flightIndex = 1;
                foreach(var flight in trade.Flights) {
                    var increasing = roleBill.Source.Details.First(b => b.Passenger == passenger && b.Flight.Id == flight.Id).Increasing;
                    var detailBill = makePayDetailBill(trade.IsThirdRelation, passenger, flight, rebate, increasing, residualCommission, residualTradeFee, flightIndex == totalFlightCount);
                    residualCommission -= detailBill.Commission;
                    residualTradeFee -= detailBill.TradeFee;
                    detailBills.Add(detailBill);
                    flightIndex++;
                }
            }
            roleBill.Source.Details = detailBills;
        }
        internal void RefreshReleaseFare(TradeInfo trade, NormalPayRoleBill roleBill) {
            foreach(var flight in trade.Flights) {
                var flightBills = roleBill.Source.Details.Where(d => d.Flight.Id == flight.Id);
                foreach(var fb in flightBills) {
                    fb.Flight.ReleasedFare = flight.ReleasedFare;
                    fb.Commission = CalcCommission(flight, roleBill.Source.Rebate);
                    fb.ServiceCharge = GetServiceCharge(trade.IsThirdRelation, fb.Flight);
                    fb.Anticipation = GetAnticipation(flight, fb.ServiceCharge, fb.Commission, fb.Increasing);
                    fb.TradeFee = CalcTradeFee(fb.Anticipation);
                    fb.RefreshAmount();
                }
            }
        }
        internal void RefreshFare(TradeInfo trade, NormalPayRoleBill roleBill) {
            foreach(var flight in trade.Flights) {
                var flightBills = roleBill.Source.Details.Where(d => d.Flight.Id == flight.Id);
                foreach(var fb in flightBills) {
                    fb.Flight.Fare = flight.Fare;
                    fb.ServiceCharge = GetServiceCharge(trade.IsThirdRelation, fb.Flight);
                }
            }
        }
        /// <summary>
        /// 生成支付账单
        /// </summary>
        internal PostponePayRoleBill MakePayBill(IEnumerable<Order.Domain.Applyform.PostponeFlight> flights, IEnumerable<Order.Domain.Passenger> passengers) {
            _rate = 0;
            var totalTradeFee = Utility.Calculator.Round(GetPostponeTradeFee(), -2);

            var billSerial = 1;
            var detailBills = new List<PostponePayDetailBill>();
            foreach(var flight in flights) {
                foreach(var passenger in passengers) {
                    var detailBill = makePayDetailBill(passenger.Id, flight);
                    if(billSerial == 1) {
                        detailBill.TradeFee = Math.Abs(totalTradeFee) * -1;
                    }
                    detailBills.Add(detailBill);
                    billSerial++;
                }
            }
            var roleBill = new PostponePayRoleBill(this) {
                Source = new PostponePayRoleBillSource {
                    Details = detailBills
                }
            };
            return roleBill;
        }

        /// <summary>
        /// 生成支付账单明细
        /// </summary>
        private NormalPayDetailBill makePayDetailBill(bool isThirdRelation, Guid passenger, Flight flight, Deduction deduction) {
            var bill = new NormalPayDetailBill(passenger, flight) {
                Commission = deduction == null ? 0 : CalcCommission(flight, deduction.Rebate),
                Increasing = deduction == null ? 0 : GetIncreasing(deduction),
                ServiceCharge = GetServiceCharge(isThirdRelation, flight)
            };
            bill.Anticipation = GetAnticipation(flight, bill.ServiceCharge, bill.Commission, bill.Increasing);
            bill.TradeFee = CalcTradeFee(bill.Anticipation);
            return bill;
        }
        /// <summary>
        /// 生成支付账单明细
        /// </summary>
        private NormalPayDetailBill makePayDetailBill(bool isThirdRelation, Guid passenger, Flight flight, decimal rebate, decimal increasing, decimal maxCommission, decimal maxTradeFee, bool isLastFlight) {
            var bill = new NormalPayDetailBill(passenger, flight) {
                Commission = CalcCommission(flight, rebate),
                Increasing = increasing,
                ServiceCharge = GetServiceCharge(isThirdRelation, flight)
            };
            if(isLastFlight || Math.Abs(bill.Commission) > Math.Abs(maxCommission)) bill.Commission = maxCommission;
            bill.Anticipation = GetAnticipation(flight, bill.ServiceCharge, bill.Commission, increasing);
            bill.TradeFee = CalcTradeFee(bill.Anticipation);
            if(isLastFlight || Math.Abs(bill.TradeFee) > Math.Abs(maxTradeFee)) bill.TradeFee = maxTradeFee;
            return bill;
        }
        /// <summary>
        /// 生成支付账单明细
        /// </summary>
        private PostponePayDetailBill makePayDetailBill(Guid passenger, Order.Domain.Applyform.PostponeFlight flight) {
            var bill = new PostponePayDetailBill(passenger, Flight.GetFlight(flight.OriginalFlight)) {
                PostponeFee = GetPostponeFee(flight)
            };
            bill.Anticipation = bill.PostponeFee;
            return bill;
        }

        /// <summary>
        /// 计算佣金
        /// </summary>
        protected virtual decimal CalcCommission(Flight flight, decimal rebate) {
            return ProcessCommission(flight.Fare * rebate);
        }
        protected virtual decimal ProcessCommission(decimal commission) {
            return Utility.Calculator.Round(commission, -2);
        }
        /// <summary>
        /// 获取加价金额
        /// </summary>
        protected virtual decimal GetIncreasing(Deduction deduction) {
            return Utility.Calculator.Round(deduction.Increasing, -2);
        }
        /// <summary>
        /// 获取服务费
        /// </summary>
        protected abstract decimal GetServiceCharge(bool isThirdRelation, Flight flight);
        /// <summary>
        /// 获取应收/付金额
        /// </summary>
        protected abstract decimal GetAnticipation(Flight flight, decimal serviceCharge, decimal commission, decimal increasing);
        /// <summary>
        /// 获取改期费
        /// </summary>
        protected abstract decimal GetPostponeFee(Order.Domain.Applyform.PostponeFlight flight);

        /// <summary>
        /// 生成退款账单
        /// </summary>
        internal NormalRefundRoleBill MakeRefundBill(NormalPayRoleBill payBill, RefundInfo refundInfo, IEnumerable<NormalRefundRoleBill> refundedNormalBills) {
            var refundDetailBills = (from payDetail in payBill.Source.Details
                                     where hasApply(refundInfo, payDetail.Flight.Id, payDetail.Passenger)
                                     let refundedDetails = getRefundedDetailBills(refundedNormalBills, payDetail.Flight.Id, payDetail.Passenger)
                                     select makeRefundDetailBill(payDetail, refundInfo, refundedDetails)).ToList();
            var bill = new NormalRefundRoleBill(this) {
                PayRoleBill = payBill,
                Source = new NormalRefundRoleBillSource {
                    Details = refundDetailBills
                }
            };
            return bill;
        }
        /// <summary>
        /// 生成差错退款账单
        /// </summary>
        internal NormalRefundRoleBill MakeErrorRefundBill(NormalPayRoleBill payBill, ErrorRefundInfo refundInfo, IEnumerable<NormalRefundRoleBill> refundedNormalBills) {
            var refundDetailBills = (from payDetail in payBill.Source.Details
                                     let flightId = payDetail.Flight.Id
                                     let passenger = payDetail.Passenger
                                     where hasApply(refundInfo, flightId, passenger)
                                     let refundedDetails = getRefundedDetailBills(refundedNormalBills, flightId, passenger)
                                     select makeRefundDetailBill(payDetail, refundInfo.GetFlight(flightId), refundedDetails)).ToList();
            var bill = new NormalRefundRoleBill(this) {
                PayRoleBill = payBill,
                Source = new NormalRefundRoleBillSource {
                    Details = refundDetailBills
                }
            };
            return bill;
        }
        /// <summary>
        /// 生成退款账单
        /// </summary>
        internal PostponeRefundRoleBill MakeRefundBill(PostponePayRoleBill payBill) {
            var bill = new PostponeRefundRoleBill(this) {
                PayRoleBill = payBill,
                Source = new PostponeRefundRoleBillSource {
                    Details = payBill.Source.Details.Select(makeRefundDetailBill).ToList()
                }
            };
            return bill;
        }
        /// <summary>
        /// 判断某人有没有申请某航段的退款
        /// </summary>
        private bool hasApply(RefundInfo refundInfo, Guid flight, Guid passenger) {
            if(refundInfo == null) {
                return true;
            } else {
                return refundInfo.Contains(flight, passenger);
            }
        }
        /// <summary>
        /// 判断某人有没有申请某航段的差错退款
        /// </summary>
        private bool hasApply(ErrorRefundInfo refundInfo, Guid flight, Guid passenger) {
            return refundInfo.Contains(flight, passenger);
        }
        /// <summary>
        /// 生成退款账单明细
        /// </summary>
        private NormalRefundDetailBill makeRefundDetailBill(NormalPayDetailBill payBill, RefundInfo refundInfo, IEnumerable<NormalRefundDetailBill> refundedBills) {
            var refundFlight = refundInfo == null ? null : refundInfo.GetFlight(payBill.Flight.Id);
            var result = new NormalRefundDetailBill(payBill.Passenger, payBill.Flight) {
                RefundRate = refundFlight == null ? 0 : refundFlight.Rate,
                RefundFee = refundFlight == null ? 0 : GetRefundFee(payBill.Flight, payBill.Passenger, refundFlight)
            };
            result.Commission = GetRefundCommission(payBill);
            reviseRefundCommission(payBill, result, refundedBills);
            result.Increasing = refundFlight == null ? payBill.Increasing * -1 : GetRefundIncreasing(payBill, refundFlight.RefundServiceCharge.HasValue);
            reviseRefundIncreasing(payBill, result, refundedBills);
            result.ServiceCharge = refundFlight == null ? payBill.ServiceCharge * -1 : GetRefundServiceCharge(refundInfo.HasSupplier, refundFlight);
            reviseRefundServiceCharge(payBill, result, refundedBills);
            result.Anticipation = refundFlight == null ? payBill.Anticipation * -1 : GetRefundAnticipation(payBill, result.Commission, result.Increasing, result.ServiceCharge, result.RefundRate, result.RefundFee);
            reviseRefundAnticipation(payBill, result, refundedBills);
            result.TradeFee = CalcTradeFee(result.Anticipation);
            reviseRefundTradeFee(payBill, result, refundedBills);
            return result;
        }
        /// <summary>
        /// 生成差错退款账单明细
        /// </summary>
        private NormalRefundDetailBill makeRefundDetailBill(NormalPayDetailBill payBill, ErrorRefundFlight refundFlight, IEnumerable<NormalRefundDetailBill> refundedBills) {
            var result = new NormalRefundDetailBill(payBill.Passenger, payBill.Flight);
            result.Anticipation = GetErrorRefundAnticipation(refundFlight);
            reviseRefundAnticipation(payBill, result, refundedBills);
            result.TradeFee = CalcTradeFee(result.Anticipation);
            reviseRefundTradeFee(payBill, result, refundedBills);
            return result;
        }
        /// <summary>
        /// 生成退款账单明细
        /// </summary>
        private PostponeRefundDetailBill makeRefundDetailBill(PostponePayDetailBill payBill) {
            return new PostponeRefundDetailBill(payBill.Passenger, payBill.Flight) {
                Anticipation = payBill.PostponeFee * -1,
                TradeFee = payBill.TradeFee * -1
            };
        }
        /// <summary>
        /// 获取航段和人对应的明细上，已经退过的数据
        /// </summary>
        private IEnumerable<NormalRefundDetailBill> getRefundedDetailBills(IEnumerable<NormalRefundRoleBill> refundedBills, Guid flight, Guid passenger) {
            return from roleBill in refundedBills
                   from detail in roleBill.Source.Details
                   where detail.Flight.Id == flight && detail.Passenger == passenger
                   select detail;
        }

        #region "修正退款时，相关金额信息"
        /// <summary>
        /// 修改应该退还的佣金
        /// </summary>
        private void reviseRefundCommission(NormalPayDetailBill payBill, NormalRefundDetailBill refundBill, IEnumerable<NormalRefundDetailBill> refundedBills) {
            var refundedCommission = refundedBills.Sum(item => item.Commission);
            if(Math.Abs(payBill.Commission) < Math.Abs(refundBill.Commission + refundedCommission)) {
                refundBill.Commission = (payBill.Commission + refundedCommission) * -1;
            }
        }
        /// <summary>
        /// 修正退还的加价金额
        /// </summary>
        private void reviseRefundIncreasing(NormalPayDetailBill payBill, NormalRefundDetailBill refundBill, IEnumerable<NormalRefundDetailBill> refundedBills) {
            var refundedIncreasing = refundedBills.Sum(item => item.Increasing);
            if(Math.Abs(payBill.Increasing) < Math.Abs(refundBill.Increasing + refundedIncreasing)) {
                refundBill.Increasing = (payBill.Increasing + refundedIncreasing) * -1;
            }
        }
        /// <summary>
        /// 修改应该退还的服务费
        /// </summary>
        private void reviseRefundServiceCharge(NormalPayDetailBill payBill, NormalRefundDetailBill refundBill, IEnumerable<NormalRefundDetailBill> refundedBills) {
            var refundedServiceCharge = refundedBills.Sum(item => item.ServiceCharge);
            if(Math.Abs(payBill.ServiceCharge) < Math.Abs(refundBill.ServiceCharge + refundedServiceCharge)) {
                refundBill.ServiceCharge = (payBill.ServiceCharge + refundedServiceCharge) * -1;
            }
        }
        /// <summary>
        /// 修正退款的应收/退金额
        /// </summary>
        private void reviseRefundAnticipation(NormalPayDetailBill payBill, NormalRefundDetailBill refundBill, IEnumerable<NormalRefundDetailBill> refundedBills) {
            var refundedAmount = refundedBills.Sum(item => item.Anticipation);
            if(Math.Abs(payBill.Anticipation) < Math.Abs(refundBill.Anticipation + refundedAmount)) {
                refundBill.Anticipation = (payBill.Anticipation + refundedAmount) * -1;
            }
        }
        /// <summary>
        /// 修改退款时的交易手续费
        /// </summary>
        private void reviseRefundTradeFee(NormalPayDetailBill payBill, NormalRefundDetailBill refundBill, IEnumerable<NormalRefundDetailBill> refundedBills) {
            var refundedTradeFee = refundedBills.Sum(item => item.TradeFee);
            var refundedAnticipation = refundedBills.Sum(b => b.Anticipation);
            var refundAnticipation = refundedAnticipation + refundBill.Anticipation;
            // 退款金额与收款金额相同时，直接全退手续费
            // 退还的手续费不能超过收取的手续费
            if(Math.Abs(payBill.Anticipation) == Math.Abs(refundAnticipation)
                || Math.Abs(payBill.TradeFee) < Math.Abs(refundBill.TradeFee + refundedTradeFee)) {
                refundBill.TradeFee = (payBill.TradeFee + refundedTradeFee) * -1;
            }
        }
        #endregion

        /// <summary>
        /// 获取应该退还的佣金
        /// </summary>
        protected virtual decimal GetRefundCommission(NormalPayDetailBill payBill) {
            return payBill.Commission * -1;
        }
        /// <summary>
        /// 获取应该退还的加价金额
        /// </summary>
        protected virtual decimal GetRefundIncreasing(NormalPayDetailBill payBill, bool providerRefundServiceCharge) {
            return payBill.Increasing * -1;
        }
        /// <summary>
        /// 获取应该退还的服务费
        /// </summary>
        protected abstract decimal GetRefundServiceCharge(bool isThirdRelation, RefundFlight refundFlight);
        /// <summary>
        /// 获取退票手续费
        /// </summary>
        protected abstract decimal GetRefundFee(Flight flight, Guid passenger, RefundFlight refundFlight);
        /// <summary>
        /// 获取应退/收金额
        /// </summary>
        protected abstract decimal GetRefundAnticipation(NormalPayDetailBill payBill, decimal refundCommission, decimal refundIncreasing, decimal refundServiceCharge, decimal refundRate, decimal refundFee);
        /// <summary>
        /// 获取差错退款的应退/收金额
        /// </summary>
        protected abstract decimal GetErrorRefundAnticipation(ErrorRefundFlight refundFlight);
    }
}