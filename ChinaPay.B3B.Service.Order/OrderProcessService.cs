using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.Domain.Ticket;
using ChinaPay.B3B.Service.Distribution.Domain;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Order.Repository;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.Core;
using ChinaPay.Core.Exception;
using ChinaPay.Core.Extension;
using ChinaPay.SMS.Service.Domain;
using Deduction = ChinaPay.B3B.Service.Distribution.Domain.Deduction;
using Flight = ChinaPay.B3B.Service.Order.Domain.Flight;
using FlightView = ChinaPay.SMS.Service.Templete.FlightView;
using PassengerView = ChinaPay.B3B.DataTransferObject.Order.PassengerView;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 订单处理服务
    /// </summary>
    public static class OrderProcessService {

        #region "订单"

        public static Order.Domain.Order ProduceOrder(OrderView orderView, MatchedPolicy matchedPolicy, EmployeeDetailInfo employee, Guid OEMID, bool forbidChnagePnr,
            AuthenticationChoise choise = AuthenticationChoise.NoNeedAUTH) {
            var today = DateTime.Today;
            if(SystemManagement.SystemParamService.UnOrderableTimeZones.Any(item => item.Lower.Date <= today && today <= item.Upper.Date)) {
                var forbiddenOrderTime = SystemManagement.SystemParamService.UnOrderableTimeZones.FirstOrDefault(item => item.Lower.Date <= today && today <= item.Upper.Date);
                throw new CustomException(string.Format("{0:yyyy年MM月dd日 HH点mm分} 到 {1:yyyy年MM月dd日 HH点mm分} 期间不能生成订单", forbiddenOrderTime.Lower, forbiddenOrderTime.Upper));
            }

            if(orderView == null) throw new ArgumentNullException("orderView");
            if(employee == null) throw new ArgumentNullException("employee");
            if(matchedPolicy == null) throw new ArgumentNullException("matchedPolicy");
            if(matchedPolicy.Provider == employee.Owner) throw new CustomException("不能购买自己发布的产品");
            Order.Domain.Order order;
            if(!matchedPolicy.IsExternal) {
                order = Order.Domain.Order.NewOrder(orderView, matchedPolicy, employee, forbidChnagePnr, OEMID, choise);
            } else {
                order = ExternalOrder.NewExternalOrder(orderView, matchedPolicy, employee, choise);
                var extOrder = ExternalPlatform.OrderService.Produce(order.Id, orderView, orderView.PnrContent, orderView.PatContent, matchedPolicy.OriginalExternalPolicy);
                if(extOrder.Success) {
                    var anticipation = (orderView.Flights.Sum(p => orderView.PATPrice.AirportTax + orderView.PATPrice.BunkerAdjustmentFactor) +
                                        matchedPolicy.ParValue * (1 - matchedPolicy.OriginalExternalPolicy.OriginalRebate)) *
                                       orderView.Passengers.Count();
                    if(Math.Abs(anticipation - extOrder.Result.Amount) > orderView.Passengers.Count()) {

                        foreach(var flight in orderView.Flights) {
                            LogService.SaveFareErrorLog(new FareErrorLog {
                                Carrier = flight.Airline,
                                Departure = flight.Departure,
                                Arrival = flight.Arrival,
                                Bunk = flight.Bunk,
                                FlightDate = flight.TakeoffTime,
                                Fare = extOrder.Result.Amount,
                                Time = DateTime.Now
                            });
                        }
                    }
                    var externalOrder = order as ExternalOrder;
                    externalOrder.ECommission = matchedPolicy.OriginalExternalPolicy.OriginalRebate;
                    externalOrder.PayStatus = PayStatus.NoPay;
                    externalOrder.Platform = matchedPolicy.OriginalExternalPolicy.Platform;
                    externalOrder.ExternalOrderId = extOrder.Result.Id;
                    externalOrder.Amount = extOrder.Result.Amount;
                    SaveExternalPolicyCopy(matchedPolicy.OriginalExternalPolicy, order.Id);
                } else {
                    LogService.SaveExceptionLog(new CustomException("生成外平台政策订单失败"),
                        string.Format("生成{0}订单失败,原因{1}，订单号：{2},政策所在平台：{3}", matchedPolicy.OriginalExternalPolicy.Platform.GetDescription(), extOrder.ErrMessage,
                            order.Id, matchedPolicy.OriginalExternalPolicy.Platform.GetDescription()));
                    throw new CustomException(extOrder.ErrMessage);
                }
            }
            var specialPolicyInfo = matchedPolicy.OriginalPolicy as SpecialPolicyInfo;
            PNRInfo pnrInfo = order.PNRInfos.First();
            var tradeInfo = new TradeInfo() {
                Id = order.Id,
                IsThirdRelation = order.IsThirdRelation,
                IsSpecialProduct = order.IsSpecial,
                SpecialProductType = specialPolicyInfo != null ? specialPolicyInfo.Type : SpecialProductType.OtherSpecial,
                ProviderRelationWithPurchaser = matchedPolicy.RelationType,
                Passengers = pnrInfo.Passengers.Select(p => p.Id),
                Flights = pnrInfo.Flights.Select(Distribution.Domain.Flight.GetFlight),
            };
            var isRebate = matchedPolicy.OemInfo.ProfitType == OemProfitType.Discount;
            var tradeDeduction = new TradeDeduction() {
                Purchaser = new UserDeduction() {
                    Owner = employee.Owner,
                    Deduction = new Deduction {
                        Rebate = isRebate ? matchedPolicy.Commission : 0,
                        Increasing = isRebate ? 0 : (matchedPolicy.OemInfo.TotalProfit * -1)
                    }
                }
            };

            foreach(OemProfit profit in matchedPolicy.OemInfo.Profits) {
                tradeDeduction.AddRoyalty(new UserDeduction {
                    Owner = profit.CompanyId,
                    Deduction = new Deduction {
                        Rebate = isRebate ? profit.Value : 0,
                        Increasing = isRebate ? 0 : profit.Value
                    }
                });
            }
            var payBill = DistributionProcessService.ProducePurchasePayBill(tradeInfo, tradeDeduction);
            if(!order.IsThirdRelation) {
                tradeDeduction.Provider = new UserDeduction {
                    Owner = order.Provider.CompanyId,
                    Deduction = new Deduction {
                        Rebate = isRebate ? order.Provider.Rebate : 0,
                        Increasing = 0
                    }
                };
                tradeDeduction.Platform = new Deduction {
                    Rebate =
                        order.Provider.Rebate - order.Purchaser.Rebate - (matchedPolicy.OemInfo.ProfitType == OemProfitType.Discount ? matchedPolicy.OemInfo.TotalProfit : 0),
                    Increasing = 0
                };
                payBill = DistributionProcessService.ProduceProvidePayBill(tradeInfo, tradeDeduction, payBill, false);
            }
            order.SetBill(payBill);
            if(order.IsInterior && !order.RequireConfirm) {
                order.PaySuccess(null);
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.InsertOrder(order);
                    distributionRepository.SaveNormalPayBill(payBill);
                    if(order.IsCustomerResource) {
                        // 扣减资源数
                        var productId = order.IsThirdRelation ? order.Supplier.Product.Id : order.Provider.Product.Id;
                        var passengerCount = order.PNRInfos.Sum(item => item.Passengers.Count());
                        Policy.PolicyManageService.Decrease(productId, passengerCount);
                    }
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    var extOrder = order as ExternalOrder;
                    if(extOrder != null && extOrder.ExternalOrderId != null) {
                        ExternalPlatform.OrderService.Cancel(extOrder.Platform, 0, extOrder.ExternalOrderId, extOrder.PNRInfos.First().Passengers.Select(p => p.Name),
                            "B3B平台订单生成失败");
                    }
                    throw;
                }
            }

            Order.LogHelper.SaveProduceOrderLog(order);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
            return order;
        }

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="releasedFare">价格</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void ReviseReleasedFare(decimal orderId, decimal releasedFare, string operatorAccount) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            var originalPurchaserAmount = order.Purchaser.Amount;
            var originalReleasedFare = order.ReviseReleasedFare(releasedFare);

            var pnrInfo = order.PNRInfos.First();
            var product = order.Product as SpeicalProductInfo;
            var tradeInfo = new TradeInfo() {
                Id = order.Id,
                IsThirdRelation = order.IsThirdRelation,
                IsSpecialProduct = order.IsSpecial,
                SpecialProductType = product != null ? product.SpeicalProductType : SpecialProductType.OtherSpecial,
                ProviderRelationWithPurchaser = order.Provider == null ? RelationType.Brother : order.Provider.PurchaserRelationType,
                Passengers = pnrInfo.Passengers.Select(p => p.Id),
                Flights = pnrInfo.Flights.Select(Distribution.Domain.Flight.GetFlight),
            };
            var payBill = DistributionProcessService.RefreshReleaseFare(tradeInfo);

            order.SetBill(payBill);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateOrderForReviseReleasedFare(order);
                    distributionRepository.ReSaveNormalPayBill(payBill);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            decimal increasing = pnrInfo.Flights.Sum(f => f.Increasing);
            Order.LogHelper.SaveSupplierReviseReleasedFareLog(order.Id, originalReleasedFare, releasedFare, originalPurchaserAmount, order.Purchaser.Amount,
                increasing, increasing * pnrInfo.Passengers.Count(), order.Supplier.CompanyId, order.IsThirdRelation, operatorAccount);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
        }

        /// <summary>
        /// 资源方确认成功 并提供资源
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="pnrCode">资源信息</param>
        /// <param name="patPrice"> </param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="matchedPolicy"> </param>
        /// <param name="flightNo"> </param>
        /// <param name="flightDate"> </param>
        public static string SupplierConfirmSuccessful(Order.Domain.Order order, PNRPair pnrCode, decimal? patPrice, string operatorAccount, MatchedPolicy matchedPolicy, Guid oemId) {
            if(order == null) throw new NotFoundException("订单不存在");
            Checker.CheckOrderLocked(order.Id, operatorAccount);
            //var order = OrderQueryService.QueryOrder(orderId);

            var officeNo = string.Empty;
            var fareRevised = order.ConfirmResourceSuccessful(pnrCode, patPrice, oemId);
            var specialPolicyInfo = order.Product as SpeicalProductInfo;
            PNRInfo pnrInfo = order.PNRInfos.First();
            var tradeInfo = new TradeInfo() {
                Id = order.Id,
                IsThirdRelation = order.IsThirdRelation,
                IsSpecialProduct = order.IsSpecial,
                SpecialProductType = specialPolicyInfo != null ? specialPolicyInfo.SpeicalProductType : SpecialProductType.OtherSpecial,
                Passengers = pnrInfo.Passengers.Select(p => p.Id),
                Flights = pnrInfo.Flights.Select(Distribution.Domain.Flight.GetFlight),
            };
            tradeInfo.ProviderRelationWithPurchaser = order.IsThirdRelation ? matchedPolicy.RelationType : order.Provider.PurchaserRelationType;
            if(order.IsThirdRelation) {
                var policy = matchedPolicy;
                order.MatchProvider(policy);
                order.UpdateProvision(policy);
                officeNo = policy.OfficeNumber;

                var tradeDeduction = new TradeDeduction() {
                    Provider = new UserDeduction {
                        Owner = order.Provider.CompanyId,
                        Deduction = new Deduction {
                            Rebate = order.Provider.Rebate,
                            Increasing = 0
                        }
                    },
                    Platform = new Deduction {
                        Rebate =
                            order.Provider.Rebate - order.Purchaser.Rebate - (order.Supplier == null ? 0 : order.Supplier.Rebate) -
                            (matchedPolicy.OemInfo.ProfitType == OemProfitType.Discount ? matchedPolicy.OemInfo.TotalProfit : 0)
                                                 ,
                        Increasing = 0
                    }
                };
                if(order.IsThirdRelation) {
                    tradeDeduction.Supplier = new UserDeduction() {
                        Owner = order.Supplier.CompanyId,
                        Deduction = new Deduction {
                            Rebate = order.Supplier.Rebate,
                            Increasing = 0
                        }
                    };
                }
                var payBill = DistributionProcessService.ProduceProvidePayBill(tradeInfo, tradeDeduction, false);
                order.SetBill(payBill);
            }
            if(fareRevised) {
                DistributionProcessService.RefreshFare(tradeInfo, order.Bill.PayBill);
            }
            if(order.IsInterior) {
                order.PaySuccess(null);
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateOrderForSupplyResource(order);
                    if(order.IsThirdRelation) {
                        distributionRepository.SaveRoyaltiesPayBill(order.Bill.PayBill);
                    }
                    if(fareRevised) {
                        distributionRepository.UpdatePayBillFare(order.Bill.PayBill);
                    }
                    orderRepository.UpdateRemindStatus(order.Id);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveSupplierConfirmSuccessfulLog(order, patPrice, operatorAccount);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
            return officeNo;
        }

        /// <summary>
        /// 资源方确认失败
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="reason">原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void SupplierConfirmFailed(decimal orderId, string reason, string operatorAccount) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null)
                throw new NotFoundException(orderId.ToString(), "订单不存在");
            order.ConfirmResourceFailed(reason);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.UpdateStatus(order.Id, order.Status, order.Remark);
                    // 恢复相应的资源数
                    ResumeResource(order);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveSupplierConfirmFailedLog(order, reason, operatorAccount);
            Order.StatisticHelper.Statistic(order);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
        }

        /// <summary>
        /// 提供资源
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="pnrCode">资源信息</param>
        /// <param name="patPrice"> </param>
        /// <param name="matchedPolicy"> </param>
        /// <param name="operatorAccount">操作员账号</param>
        public static string SupplyResource(Order.Domain.Order order, PNRPair pnrCode, decimal? patPrice, MatchedPolicy matchedPolicy, string operatorAccount, Guid oemId) {
            if(order == null) throw new NotFoundException("订单不存在");
            Checker.CheckOrderLocked(order.Id, operatorAccount);
            //var order = OrderQueryService.QueryOrder(orderId);

            var officeNo = string.Empty;
            var fareRevised = order.SupplyResource(pnrCode, patPrice, oemId);
            var specialPolicyInfo = order.Product as SpeicalProductInfo;
            PNRInfo pnrInfo = order.PNRInfos.First();
            var tradeInfo = new TradeInfo() {
                Id = order.Id,
                IsThirdRelation = order.IsThirdRelation,
                IsSpecialProduct = order.IsSpecial,
                SpecialProductType = specialPolicyInfo != null ? specialPolicyInfo.SpeicalProductType : SpecialProductType.OtherSpecial,
                Passengers = pnrInfo.Passengers.Select(p => p.Id),
                Flights = pnrInfo.Flights.Select(Distribution.Domain.Flight.GetFlight),
            };
            tradeInfo.ProviderRelationWithPurchaser = !order.IsThirdRelation
                                                          ? order.Provider.PurchaserRelationType
                                                          : matchedPolicy.RelationType;
            if(order.IsThirdRelation) {
                var policy = matchedPolicy;
                order.MatchProvider(policy);
                order.UpdateProvision(policy);
                officeNo = policy.OfficeNumber;


                var tradeDeduction = new TradeDeduction() {
                    Provider = new UserDeduction {
                        Owner = order.Provider.CompanyId,
                        Deduction = new Deduction {
                            Rebate = order.Provider.Rebate,
                            Increasing = 0
                        }
                    },
                    Platform = new Deduction {
                        Rebate =
                            order.Provider.Rebate - order.Purchaser.Rebate - (order.Supplier == null ? 0 : order.Supplier.Rebate) -
                            (matchedPolicy.OemInfo.ProfitType == OemProfitType.Discount ? matchedPolicy.OemInfo.TotalProfit : 0),
                        Increasing = 0
                    }
                };
                if(order.IsThirdRelation) {
                    tradeDeduction.Supplier = new UserDeduction() {
                        Owner = order.Supplier.CompanyId,
                        Deduction = new Deduction {
                            Rebate = order.Supplier.Rebate,
                            Increasing = 0
                        }
                    };
                }
                var payBill = DistributionProcessService.ProduceProvidePayBill(tradeInfo, tradeDeduction, false);
                order.SetBill(payBill);
            }
            if(fareRevised) {
                DistributionProcessService.RefreshFare(tradeInfo, order.Bill.PayBill);
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateOrderForSupplyResource(order);
                    if(order.IsThirdRelation) {
                        distributionRepository.SaveRoyaltiesPayBill(order.Bill.PayBill);
                    }
                    if(fareRevised) {
                        distributionRepository.UpdatePayBillFare(order.Bill.PayBill);
                    }
                    orderRepository.UpdateRemindStatus(order.Id);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveSupplyResourceLog(order, patPrice, operatorAccount);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
            return officeNo;
        }

        /// <summary>
        /// 拒绝提供资源
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="reason">原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DenySupplyResource(decimal orderId, string reason, string operatorAccount) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            order.DenySupplyResource(reason);
            var refundBill = DistributionProcessService.ProduceNormalRefundBill(orderId);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateStatus(order.Id, order.Status, order.Remark);
                    distributionRepository.SaveRefundBill(refundBill);
                    // 恢复相应的资源数
                    ResumeResource(order);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }


            Order.LogHelper.SaveDenySupplyResourceLog(order, reason, operatorAccount);
            Order.StatisticHelper.Statistic(order);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
            // 退款是否成功，不影响业务流程
            // 如果失败，会由其他地方来单独处理
            if(!order.IsInterior) {
                try {
                    var refundResult = Tradement.RefundmentService.TradeRefund(refundBill, RefundBusinessType.DenySupply);
                    if(refundResult != null && refundResult.Success) {
                        DistributionProcessService.NormalRefundSuccess(refundBill, new[] { refundResult });
                        using(var command = Factory.CreateCommand()) {
                            var distributionRepository = Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                        } 
                            var orderNotifier = new Order.Notify.OrderNotifier(order);
                            orderNotifier.SendRefundSuccessNotify(); 
                    }
                } catch(Exception ex) {
                    LogService.SaveExceptionLog(ex);
                }
            }
        }

        /// <summary>
        /// 能否支付
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="message">不能支付的原因</param>
        public static bool Payable(decimal orderId, out string message) {
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            if(!order.IsB3BOrder) {
                var externalOrderInfo = OrderQueryService.QueryOrderExternalInfo(order.Id);
                if(externalOrderInfo == null) throw new NotFoundException(orderId.ToString(), "获取订单信息失败");
                PNRInfo pnrInfo = order.PNRInfos.First();
                IEnumerable<Flight> flights = pnrInfo.Flights;
                var anticipation = flights.Sum(p => p.Price.Fare * (1 - externalOrderInfo.ECommission) + p.Price.BAF + p.Price.AirportFee) * pnrInfo.Passengers.Count();
                if(Math.Abs(externalOrderInfo.Amount.Value - anticipation) > pnrInfo.Passengers.Count()) {
                    message = "订单中存在航段运价变动，不能进行支付";
                    string lockMsg = string.Empty;
                    LockService.Lock(new LockInfo(orderId.ToString()) {
                        Account = order.Purchaser.OperatorAccount,
                        Company = order.Purchaser.CompanyId

                    }, out lockMsg);
                    CancelOrder(orderId, order.Purchaser.OperatorAccount, "外部订单中存在航段运价变动,已经不能再进行支付了");
                    return false;
                }
            }
            return order.IsPayable(out message);
        }

        /// <summary>
        /// 支付成功
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="payAccount">支付账号</param>
        /// <param name="payTradeNo">流水号</param>
        /// <param name="payTime">支付时间</param>
        /// <param name="payInterface">支付接口</param>
        /// <param name="payAccountType">支付账号类型</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void PaySuccess(decimal orderId, string payAccount, string payTradeNo,string channelTradeNo, DateTime payTime, PayInterface payInterface, PayAccountType payAccountType, string operatorAccount) {
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            if(order.Status == OrderStatus.Canceled) {
                processTimeoutPaySuccess(order, payAccount, payTradeNo, channelTradeNo, payInterface, payAccountType, payTime);
                return;
            } else if(order.Status != OrderStatus.Ordered) return;

            order.PaySuccess(payTime);
            var payBill = DistributionProcessService.PurchaserPaySuccessForOrder(orderId, payAccount, payTradeNo, payInterface, payAccountType, payTime, channelTradeNo);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    var autoPayRepository = Factory.CreateAutoPayRepository(command);
                    orderRepository.UpdateOrderForPaySuccess(order);
                    distributionRepository.UpdatePayBillForPurchaserPaySuccess(payBill);
                    //修改代扣状态 2013-3-29 wangsl
                    autoPayRepository.UpdateSuccess(order.Id);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            Order.LogHelper.SavePaySuccessLog(order, payAccount, payTradeNo, payTime, operatorAccount);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
            LockService.UnLockForcibly(orderId.ToString());
            if(!order.IsB3BOrder) {
                System.Threading.ThreadPool.QueueUserWorkItem(obj => {
                                                                      try {
                                                                          AutoPayExternalOrder((decimal)obj);
                                                                      } catch(Exception ex) {
                                                                          LogService.SaveTextLog(ex.Message + ex.Source + ex.StackTrace);
                                                                      }
                                                                  }, order.Id);
            }
        }

        /// <summary>
        /// 调用接口支付第三方订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单扩展信息，注意返回对象中不包含订单详细信息</returns>
        public static ExternalOrder AutoPayExternalOrder(decimal orderId) {
            var externalOrderInfo = OrderQueryService.QueryOrderExternalInfo(orderId);
            if(externalOrderInfo.PayStatus == PayStatus.Paied) {
                throw new CustomException("请不要重复支付！");
            }
            var payResult = ExternalPlatform.OrderService.Pay(externalOrderInfo.Platform, orderId, externalOrderInfo.ExternalOrderId, externalOrderInfo.Amount.Value);
            if(payResult.Success && payResult.Result.Success) {
                externalOrderInfo.PayStatus = PayStatus.Paied;
                externalOrderInfo.PayTime = payResult.Result.Payment.PayTime;
                externalOrderInfo.PayTradNO = payResult.Result.Payment.TradeNo;
                externalOrderInfo.IsAutoPay = payResult.Result.Payment.IsAutoPay;
                externalOrderInfo.FaildInfo = string.Empty;
            } else {
                externalOrderInfo.PayStatus = PayStatus.PayFail;
                externalOrderInfo.PayTime = DateTime.Now;
                externalOrderInfo.IsAutoPay = true;
                externalOrderInfo.FaildInfo = payResult.Result == null ? payResult.ErrMessage : payResult.Result.ErrorMessage ?? string.Empty;
            }
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.PayExternalOrderSuccess(externalOrderInfo);
                    command.CommitTransaction();
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex, "外部订单支付失败");
                    throw;
                }
            }
            if(!payResult.Success && payResult.ErrMessage == "代扣超时,请稍后再试！") {
                //查询支付信息
                QueryExternalPlatformPaymentStatus(orderId);
            }
            return externalOrderInfo;
        }

        /// <summary>
        /// 出票成功
        /// 回填票号
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="ticketNoView">票号信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="operatorName">操作员名称</param>
        public static void OutTicket(decimal orderId, TicketNoView ticketNoView, string operatorAccount, string operatorName, Guid oemId) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            ETDZ(ticketNoView, operatorAccount, operatorName, order, oemId);
        }

        internal static void ETDZ(TicketNoView ticketNoView, string operatorAccount, string operatorName, Order.Domain.Order order, Guid oemId) {
            var originalTicketType = order.Provider.Product.TicketType.ToString();
            var orginalOfficeNo = order.Provider.Product.OfficeNo;
            if(order == null) throw new NotFoundException(order.Id.ToString(), "订单不存在");
            if(!PNRPair.IsNullOrEmpty(ticketNoView.ETDZPNR) && !PNRPair.IsNullOrEmpty(order.ReservationPNR)) {
                if(PNRPair.Equals(ticketNoView.ETDZPNR, order.ReservationPNR) ||
                    PNRPair.Equals(ticketNoView.ETDZPNR.PNR, order.ReservationPNR.BPNR) ||
                    PNRPair.Equals(ticketNoView.ETDZPNR.BPNR, order.ReservationPNR.PNR))
                    throw new CustomException("小编码不能为空且新编码不能与原编码相同");
            }
            order.FillTicketNo(DateTime.Now, ticketNoView, operatorAccount, operatorName, oemId);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.UpdateOrderForETDZ(order);
                    orderRepository.UpdateRemindStatus(order.Id);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            if(!string.IsNullOrEmpty(order.Contact.Mobile)) {
                //验证系统是否需要自动给用户发出票成功短信
                CompanySmsParam accountParam = ChinaPay.SMS.Service.SMSCompanySmsParamService.Query(AccountType.Payment, order.Purchaser.CompanyId);
                if(accountParam != null && (accountParam.B3BReceiveSms & ChinaPay.SMS.DataTransferObject.CompanyB3BReceiveSms.Ticket) > 0) {
                    string domainName, servicePhone;
                    if(order.IsOEMOrder) {
                        var oeminfo = OEMService.QueryOEMById(order.OEMID.Value);
                        domainName = oeminfo.DomainName;
                        servicePhone = oeminfo.Contract.ServicePhone;
                    } else {
                        domainName = ChinaPay.B3B.Service.SystemManagement.SystemParamService.B3BDefalutLogonUrl;
                        servicePhone = ChinaPay.B3B.Service.Organization.Domain.OEMContract.B3BDefault.ServicePhone;
                    }
                    var pnr = order.PNRInfos.First();
                    SMS.Service.SMSSendService.SendB3BTicketSuccess(order.Contact.Mobile, new Account(order.Purchaser.CompanyId, order.Purchaser.OperatorAccount),
                        pnr.Flights.Select(f => new FlightView {
                            Departure = f.Departure.Name,
                            Arrival = f.Arrival.Name,
                            FlightNo = f.FlightNo,
                            Airline = f.Carrier.Name,
                            TakeoffTime = f.TakeoffTime,
                            Bunk = f.Bunk.Code
                        }), pnr.Passengers.Select(p => p.Name), domainName, servicePhone);
                }
            }
            // 处理分润与积分,内部机构的不管
            if(!order.IsInterior) {
                // 分润
                try {
                    ProcessRoyaltyFailedOrder(order.Id);
                } catch(Exception ex) {
                    LogService.SaveExceptionLog(ex);
                }
                // 处理积分
                try {
                    // 取采购时的票面价，如果是特殊票，取发布价，其他的就取真实票面价
                    var payBill = order.Bill.PayBill;
                    var purchaserFare = payBill.Purchaser.Source.Details.Sum(d => d.Flight.ReleasedFare);
                    bool isCredit = payBill.Tradement.IsPoolpay && payBill.Tradement.PayAccountType == PayAccountType.Credit;
                    Service.Integral.IntegralServer.InsertIntergralByMoney(order.Purchaser.OperatorAccount, order.Purchaser.CompanyId,
                        order.Purchaser.Name, purchaserFare,
                        payBill.Tradement.IsPoolpay, order.Id.ToString(), true, isCredit);
                } catch(Exception ex) {
                    LogService.SaveExceptionLog(ex);
                }
            }

            Order.LogHelper.SaveOutTicketLog(order, ticketNoView, operatorAccount, originalTicketType, orginalOfficeNo);
            Order.StatisticHelper.Statistic(order);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
        }

        /// <summary>
        /// 修改价格信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="priceViews">价格信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void RevisePrice(decimal orderId, IEnumerable<PriceView> priceViews, string operatorAccount) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            order.ReviseFare(priceViews);
            var product = order.Product as SpeicalProductInfo;
            PNRInfo pnrInfo = order.PNRInfos.First();
            var tradeInfo = new TradeInfo() {
                Id = order.Id,
                IsThirdRelation = order.IsThirdRelation,
                IsSpecialProduct = order.IsSpecial,
                SpecialProductType = product != null ? product.SpeicalProductType : SpecialProductType.OtherSpecial,
                ProviderRelationWithPurchaser = order.Provider == null ? RelationType.Brother : order.Provider.PurchaserRelationType,
                Passengers = pnrInfo.Passengers.Select(p => p.Id),
                Flights = pnrInfo.Flights.Select(Distribution.Domain.Flight.GetFlight),
            };
            DistributionProcessService.RefreshPayBill(tradeInfo, order.Bill.PayBill);
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    var billRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateOrderForReviseFare(order);
                    var applyform = order.Applyforms.FirstOrDefault();
                    if(applyform != null) {
                        applyformRepository.UpdateApplyformForRevicePrice(applyform);
                    }
                    billRepository.UpdatePayBillPriceInfo(order.Bill.PayBill);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveRevisePriceLog(order, operatorAccount);
            foreach(var applyform in order.Applyforms) {
                if(applyform is Order.Domain.Applyform.RefundOrScrapApplyform) {
                    Order.RemindHelper.RemindApplyform(applyform as Order.Domain.Applyform.RefundOrScrapApplyform);
                } else {
                    Order.RemindHelper.RemindApplyform(applyform as Order.Domain.Applyform.PostponeApplyform);
                }
            }
        }

        /// <summary>
        /// 拒绝出票
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="reason">拒绝原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DenyOutticket(decimal orderId, string reason, string operatorAccount) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            DenyETDZ(reason, operatorAccount, order);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
            if(!order.IsB3BOrder) {
                //拒绝出票时候取消外平台订单
                var extOrderInfo = OrderQueryService.QueryOrderExternalInfo(orderId);
                ExternalPlatform.OrderService.Cancel(extOrderInfo.Platform, order.Id, extOrderInfo.ExternalOrderId, order.PNRInfos.First().Passengers.Select(p => p.Name), reason);
            }
        }

        private static void DenyETDZ(string reason, string operatorAccount, Order.Domain.Order order) {
            order.DenyOutticket(reason);
            Distribution.Domain.Bill.Refund.NormalRefundBill refundBill = null;
            if(order.IsInterior) {
                order.Cancel();
                refundBill = DistributionProcessService.ProduceNormalRefundBill(order.Id);
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateStatus(order.Id, order.Status, order.Remark);
                    if(order.IsInterior) {
                        distributionRepository.SaveRefundBill(refundBill);
                        if(order.IsSpecial) {
                            // 恢复相应资源数
                            ResumeResource(order);
                        }
                    }
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
                //if (order.Source == OrderSource.InterfaceOrder)
                //{
                //    //发取消出票退款成功通知
                //    var orderNotifier = new Order.Notify.OrderNotifier(order);
                //    orderNotifier.SendCancelRefundNotify();
                //}
            }

            Order.LogHelper.SaveDenyOutticketLog(order, reason, operatorAccount);
            Order.StatisticHelper.Statistic(order);
            Order.RemindHelper.RemindOrder(order);
        }

        /// <summary>
        /// 转到资源方，重新处理资源
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void ReSupplyResource(decimal orderId, string operatorAccount) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null)
                throw new NotFoundException(orderId.ToString(), "订单不存在");
            order.ReSupplyResource();

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.UpdateStatus(order.Id, order.Status, order.Remark);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveReSupplyResourceLog(order, operatorAccount);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
        }

        /// <summary>
        /// 转到出票方，重新出票
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void ReOutticket(decimal orderId, string operatorAccount) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null)
                throw new NotFoundException(orderId.ToString(), "订单不存在");
            order.ReOutticket();

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.UpdateStatus(order.Id, order.Status, order.Remark);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveReOutticketLog(order, operatorAccount);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();
        }

        /// <summary>
        /// 换出票方出票
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="matchedPolicy">新政策信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="forbidChangePNR"> </param>
        public static Order.Domain.Order ChangeProvider(decimal orderId, MatchedPolicy matchedPolicy, string operatorAccount, bool forbidChangePNR,bool needAUTH) {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            if(matchedPolicy == null) throw new ArgumentNullException("matchedPolicy");
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");

            string originalProvider = order.Provider.Name;
            order.ChangeProvider(matchedPolicy, forbidChangePNR);
            order.NeedAUTH = needAUTH;
            var specialPolicyInfo = order.Product as SpeicalProductInfo;
            PNRInfo pnrInfo = order.PNRInfos.First();
            var reproduceRoyaltyBill = false;
            var tradeInfo = new TradeInfo() {
                Id = order.Id,
                IsThirdRelation = order.IsThirdRelation,
                IsSpecialProduct = order.IsSpecial,
                SpecialProductType = specialPolicyInfo != null ? specialPolicyInfo.SpeicalProductType : SpecialProductType.OtherSpecial,
                ProviderRelationWithPurchaser = matchedPolicy.RelationType,
                Passengers = pnrInfo.Passengers.Select(p => p.Id),
                Flights = pnrInfo.Flights.Select(Distribution.Domain.Flight.GetFlight),
            };
            var tradeDeduction = new TradeDeduction() {
                Provider = new UserDeduction {
                    Owner = order.Provider.CompanyId,
                    Deduction = new Deduction {
                        Rebate = matchedPolicy.Rebate,
                        Increasing = 0
                    }
                },
                Platform = new Deduction {
                    Increasing = 0,
                    Rebate =
                        order.Provider.Rebate - order.Purchaser.Rebate - (order.Supplier == null ? 0 : order.Supplier.Rebate) -
                        (matchedPolicy.OemInfo.ProfitType == OemProfitType.Discount ? matchedPolicy.OemInfo.TotalProfit : 0)
                }
            };
            if(order.IsThirdRelation) {
                tradeDeduction.Supplier = new UserDeduction() {
                    Owner = order.Supplier.CompanyId,
                    Deduction = new Deduction {
                        Rebate = order.Supplier.Rebate,
                        Increasing = 0
                    }
                };
            } else {
                reproduceRoyaltyBill = true;
                var isRebate = matchedPolicy.OemInfo.ProfitType == OemProfitType.Discount;
                foreach(var profit in matchedPolicy.OemInfo.Profits) {
                    tradeDeduction.AddRoyalty(new UserDeduction {
                        Owner = profit.CompanyId,
                        Deduction = new Deduction {
                            Rebate = isRebate ? profit.Value : 0,
                            Increasing = isRebate ? 0 : profit.Value
                        }
                    });
                }
            }
            var payBill = DistributionProcessService.ProduceProvidePayBill(tradeInfo, tradeDeduction, reproduceRoyaltyBill);
            order.SetBill(payBill);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateOrderForProviderChanged(order);
                    distributionRepository.SaveRoyaltiesPayBill(payBill);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveChangeProviderLog(order, originalProvider, operatorAccount);
            Order.RemindHelper.RemindOrder(order);
            //var notifier = new Order.Notify.OrderNotifier(order);
            //notifier.Execute();
            return order;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="reason">原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void CancelOrder(decimal orderId, string operatorAccount, string reason = "") {
            Checker.CheckOrderLocked(orderId, operatorAccount);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            order.Cancel(reason);
            Distribution.Domain.Bill.Refund.NormalRefundBill refundBill = null;
            if(order.IsInterior || order.Bill.PayBill.PaySucceed) {
                refundBill = DistributionProcessService.ProduceNormalRefundBill(orderId);
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    orderRepository.UpdateStatus(order.Id, order.Status, order.Remark);
                    if(refundBill != null) {
                        distributionRepository.SaveRefundBill(refundBill);
                    }
                    if(order.IsSpecial) {
                        // 恢复相应资源数
                        ResumeResource(order);
                    }
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            Order.LogHelper.SaveCancelOrderLog(order, operatorAccount);
            Order.StatisticHelper.Statistic(order);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();

            // 退款是否成功，不影响业务流程
            // 如果失败，会由其他地方来单独处理
            if(refundBill != null && !order.IsInterior) {
                try {
                    var refundResult = Tradement.RefundmentService.TradeRefund(refundBill, RefundBusinessType.Cancel);
                    if(refundResult != null && refundResult.Success) {
                        DistributionProcessService.NormalRefundSuccess(refundBill, new[] { refundResult });
                        using(var command = Factory.CreateCommand()) {
                            var distributionRepository = Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                        }
                            var orderNotifier = new Order.Notify.OrderNotifier(order);
                            orderNotifier.SendRefundSuccessNotify();
                    }
                } catch(Exception ex) {
                    LogService.SaveExceptionLog(ex);
                }
            }

        }

        /// <summary>
        /// 回滚资源数
        /// </summary>
        private static void ResumeResource(Order.Domain.Order order) {
            if(order.IsCustomerResource) {
                try {
                    var productId = order.IsThirdRelation ? order.Supplier.Product.Id : order.Provider.Product.Id;
                    var passengerCount = order.PNRInfos.Sum(item => item.Passengers.Count());
                    Policy.PolicyManageService.Increase(productId, passengerCount);
                } catch(Exception ex) {
                    LogService.SaveExceptionLog(ex);
                }
            }
        }

        /// <summary>
        /// 提交退改签申请
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="applyformView">申请信息</param>
        /// <param name="employee">操作员</param>
        public static BaseApplyform Apply(decimal orderId, ApplyformView applyformView, EmployeeDetailInfo employee, Guid oemId) {
            if(applyformView is UpgradeApplyformView) throw new InvalidOperationException("调用方法错啦");
            checkApplyTime();
            Checker.CheckOrderLocked(orderId, employee.UserName);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");

            var applyform = order.Apply(applyformView, oemId);
            if(applyformView is RefundOrScrapApplyformView) {
                var app = applyformView as RefundOrScrapApplyformView;
                var orderflightCount = order.PNRInfos.Sum(f => f.Flights.Count());
                var orderPassengers = order.PNRInfos.Sum(p => p.Passengers.Count());
                if(app.DelegageCancelPNR &&
                    (orderflightCount != applyformView.Voyages.Count() ||
                     orderPassengers != applyformView.Passengers.Count())) {
                    throw new CustomException("只有取消所有航段，所有人时才能委托平台取消编码");
                }
            }

            applyform.ApplierAccount = employee.UserName;
            applyform.ApplierAccountName = employee.Name;
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Insert(applyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            if(applyform is RefundOrScrapApplyform) {
                var refundOrScrapApplyform = applyform as Order.Domain.Applyform.RefundOrScrapApplyform;
                try {
                    FreezeService.Freeze(refundOrScrapApplyform);
                } catch(Exception ex) {
                    LogService.SaveExceptionLog(ex);
                }
                Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            } else if(applyform is Order.Domain.Applyform.PostponeApplyform) {
                Order.RemindHelper.RemindApplyform(applyform as Order.Domain.Applyform.PostponeApplyform);
            }
            Order.LogHelper.SaveApplyLog(order, applyform, employee.UserName);
            return applyform;
        }

        /// <summary>
        /// 提交退改签申请
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="applyformView">申请信息</param>
        /// <param name="employee">操作员</param>
        public static BaseApplyform Apply(decimal orderId, BalanceRefundApplyView applyformView, EmployeeDetailInfo employee, Guid oemId)
        {
            Checker.CheckOrderLocked(orderId, employee.UserName);
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");

            var applyform = order.Apply(applyformView, oemId);
            applyform.ApplierAccount = employee.UserName;
            applyform.ApplierAccountName = employee.Name;
            using (var command = Factory.CreateCommand())
            {
                command.BeginTransaction();
                try
                {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Insert(applyform);
                    applyformRepository.UpdateFlag(applyformView.AssociateApplyformId);
                    command.CommitTransaction();
                }
                catch (System.Data.Common.DbException ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            if (applyform is RefundOrScrapApplyform)
            {
                var refundOrScrapApplyform = applyform as RefundOrScrapApplyform;
                try
                {
                    FreezeService.Freeze(refundOrScrapApplyform);
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex);
                }
                Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            }
            else if (applyform is Order.Domain.Applyform.PostponeApplyform)
            {
                Order.RemindHelper.RemindApplyform(applyform as Order.Domain.Applyform.PostponeApplyform);
            }
            else if (applyform is BalanceRefundApplyform)
            {
                var balanceRefundApplyform = applyform as BalanceRefundApplyform;
                try
                {
                    FreezeService.Freeze(balanceRefundApplyform);
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex);
                }
                //Order.RemindHelper.RemindApplyform(balanceRefundApplyform);
            }
            Order.LogHelper.SaveApplyLog(order, applyform, employee.UserName);
            return applyform;
        }

        /// <summary>
        /// 提交退改签申请验证
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="applyformView">申请信息</param>
        /// <param name="employee">操作员</param>
        public static BaseApplyform ApplyValidate(decimal orderId, ApplyformView applyformView, EmployeeDetailInfo employee, Guid oemId)
        {
            if (applyformView is UpgradeApplyformView) throw new InvalidOperationException("调用方法错啦");
            checkApplyTime();
            Checker.CheckOrderLocked(orderId, employee.UserName);
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");

            var applyform = order.Apply(applyformView, oemId);

            applyform.ApplierAccount = employee.UserName;
            applyform.ApplierAccountName = employee.Name;
            return applyform;
        }


        private static void checkApplyTime() {
            var today = DateTime.Today;
            if(SystemManagement.SystemParamService.NotApplyTimeZones.Any(item => item.Lower.Date <= today && today <= item.Upper.Date)) {
                var forbiddenApplyTime = SystemManagement.SystemParamService.NotApplyTimeZones.FirstOrDefault(item => item.Lower.Date <= today && today <= item.Upper.Date);
                throw new CustomException(string.Format("{0:yyyy年MM月dd日 HH点mm分} 到 {1:yyyy年MM月dd日 HH点mm分} 期间不能申请退改签", forbiddenApplyTime.Lower, forbiddenApplyTime.Upper));
            }
        }

        /// <summary>
        /// 申请升舱
        /// </summary>
        public static decimal Apply(decimal orderId, UpgradeApplyformView applyformView, PolicyMatch.MatchedPolicy matchedPolicy, EmployeeDetailInfo employee, Guid oemId) {
            checkApplyTime();
            Checker.CheckOrderLocked(orderId, employee.UserName);
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");

            var applyform = order.Apply(applyformView, oemId);
            applyform.ApplierAccount = employee.UserName;
            applyform.ApplierAccountName = employee.Name;
            var upgradeApplyform = applyform as Order.Domain.Applyform.UpgradeApplyform;
            var passengers = upgradeApplyform.Passengers.Select(passenger => new PassengerView {
                Name = passenger.Name,
                Credentials = passenger.Credentials,
                CredentialsType = passenger.CredentialsType,
                PassengerType = passenger.PassengerType,
                Phone = passenger.Phone
            }).ToList();
            var orderView = new OrderView {
                FdSuccess = false,
                PNR = upgradeApplyform.NewPNR,
                AssociateOrderId = order.Id,
                AssociatePNR = upgradeApplyform.OriginalPNR,
                Contact = order.Contact,
                Source = upgradeApplyform.Source,
                Passengers = passengers,
                Flights = upgradeApplyform.Flights,
                PATPrice = upgradeApplyform.PATPrice
            };
            var newOrder = ProduceOrder(orderView, matchedPolicy, employee, oemId, false);
            upgradeApplyform.NewOrderId = newOrder.Id;
            Order.LogHelper.SaveApplyLog(order, applyform, employee.UserName);
            return newOrder.Id;
        }

        private static void processTimeoutPaySuccess(Order.Domain.Order order, string payAccount, string payTradeNo, string channelTradeNo, PayInterface payInterface, PayAccountType payAccountType, DateTime payTime)
        {
            Distribution.Domain.Bill.Refund.NormalRefundBill refundBill;
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    // 写支付账单
                    var payBill = DistributionProcessService.PurchaserPaySuccessForOrder(order.Id, payAccount, payTradeNo, payInterface, payAccountType, payTime, channelTradeNo);
                    distributionRepository.UpdatePayBillForPurchaserPaySuccess(payBill);
                    // 写退款账单
                    refundBill = DistributionProcessService.ProduceNormalRefundBill(payBill, "支付超时");
                    distributionRepository.SaveRefundBill(refundBill);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            // 提交退款
            try {
                var refundResult = Tradement.RefundmentService.TradeRefund(refundBill, RefundBusinessType.PayTimeout);
                if(refundResult != null && refundResult.Success) {
                    DistributionProcessService.NormalRefundSuccess(refundBill, new[] { refundResult });
                    using(var command = Factory.CreateCommand()) {
                        var distributionRepository = Factory.CreateDistributionRepository(command);
                        distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                    }
                }
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex);
            }
        }

        public static IEnumerable<MatchedPolicy> matchPolicys(Order.Domain.Order order, string pnrCode, Guid oemId) {
            //order.ConfirmResourceSuccessful(new PNRPair(pnrCode, string.Empty), null);
            var firstPNRInfo = order.PNRInfos.First();
            decimal releasedFare = getReleasedFare(order, firstPNRInfo.Passengers.First());
            firstPNRInfo.Flights.First().Bunk.SetYBPrice(releasedFare);
            order.PNRInfos.First().UpdateContentForResource(new PNRPair(pnrCode, string.Empty),
                (order.Product as SpeicalProductInfo).SpeicalProductType != SpecialProductType.OtherSpecial, null, order.IsStandby, oemId, false, true);
            var policyFilterCondition = new PolicyMatch.Domain.PolicyFilterConditions() {
                Purchaser = order.Purchaser.CompanyId,
                PolicyType = PolicyType.Normal
            };
            var voyageFilterInfos = getVoyageFilterInfos(order);
            policyFilterCondition.Voyages.AddRange(voyageFilterInfos);
            policyFilterCondition.VoyageType = GetVoyageType(order.TripType);
            policyFilterCondition.PatPrice = releasedFare;
            policyFilterCondition.PatContent = firstPNRInfo.PatContent;
            policyFilterCondition.PnrContent = firstPNRInfo.PNRContent;
            policyFilterCondition.PnrPair = firstPNRInfo.Code;
            return Service.PolicyMatch.PolicyMatchServcie.MatchBunkForSpecial(policyFilterCondition, false);
        }

        public static decimal getReleasedFare(Order.Domain.Order order, Service.Order.Domain.Passenger passenger) {
            var result = 0M;
            if(order.IsSpecial) {
                result += (from ticket in passenger.Tickets
                           from flight in ticket.Flights
                           where flight.Bunk is Service.Order.Domain.Bunk.SpecialBunk
                           select (flight.Bunk as Service.Order.Domain.Bunk.SpecialBunk).ReleasedFare).Sum();
            }
            return result;
        }

        private static IEnumerable<PolicyMatch.Domain.VoyageFilterInfo> getVoyageFilterInfos(Order.Domain.Order order) {
            return (from pnrInfo in order.PNRInfos
                    from item in pnrInfo.Flights
                    select new Service.PolicyMatch.Domain.VoyageFilterInfo {
                        Flight = new Service.PolicyMatch.Domain.FlightFilterInfo {
                            Airline = item.Carrier.Code,
                            Departure = item.Departure.Code,
                            Arrival = item.Arrival.Code,
                            FlightDate = item.TakeoffTime.Date,
                            FlightNumber = item.FlightNo,
                            StandardPrice = item.YBPrice,
                            IsShare = item.IsShare
                        },
                        Bunk = new Service.PolicyMatch.Domain.BunkFilterInfo {
                            Code = item.Bunk.Code,
                            Discount = item.Bunk.Discount,
                            Type = order.IsSpecial ? BunkType.Economic : item.Bunk.Type
                        }
                    }).ToList();
        }

        private static VoyageType GetVoyageType(DataTransferObject.Command.PNR.ItineraryType type) {
            switch(type) {
                case DataTransferObject.Command.PNR.ItineraryType.OneWay:
                    return VoyageType.OneWay;
                case DataTransferObject.Command.PNR.ItineraryType.Roundtrip:
                    return VoyageType.RoundTrip;
                case DataTransferObject.Command.PNR.ItineraryType.Conjunction:
                    return VoyageType.TransitWay;
                case DataTransferObject.Command.PNR.ItineraryType.Notch:
                    return VoyageType.Notch;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion

        #region "辅助功能"

        /// <summary>
        /// 修改票号
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="originalTicketNo">原票号</param>
        /// <param name="newTicketNo">新票号</param>
        /// <param name="settleCode"> </param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="isPlatform">是否是平台修改 </param>
        public static void UpdateTicketNo(decimal orderId, string originalTicketNo, string[] newTicketNo, string operatorAccount, bool isPlatform = true, string settleCode = "") {
            var order = OrderQueryService.QueryOrder(orderId);
            var numbers = parseTicketNnumbers(originalTicketNo);
            if(numbers.Length != newTicketNo.Length) throw new CustomException("票号数量和原票号数量不一致");
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");

            var passenger = order.UpdateTicketNo(numbers, newTicketNo, settleCode);

            var ticketList = new List<Ticket>();
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    for(int i = 0; i < numbers.Length; i++) {
                        var ticket = passenger[i].GetTicket(newTicketNo[i]);
                        ticketList.Add(ticket);
                        var orderRepository = Factory.CreateOrderRepository(command);
                        orderRepository.UpdateTicketNo(passenger[i].Id, ticket.Serial, newTicketNo[i], ticket.SettleCode);
                    }
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveUpdateTicketNoLog(orderId, passenger, ticketList, originalTicketNo, operatorAccount, isPlatform ? OperatorRole.Platform : OperatorRole.Provider);
        }

        /// <summary>
        /// 从票号合并字符串值中解析票号系列
        /// </summary>
        /// <param name="originalTicketNo"></param>
        /// <returns></returns>
        private static string[] parseTicketNnumbers(string originalTicketNo) {
            if(originalTicketNo.Length == 14 || originalTicketNo.Length == 10) return new string[] { originalTicketNo };
            var firstNo = originalTicketNo.Substring(0, 10);
            var end = originalTicketNo.Substring(11, 2);
            var firstNumber = Int64.Parse(firstNo);
            var endNumber = firstNumber - firstNumber % 100 + Int64.Parse(end);
            if(firstNumber > endNumber) endNumber += 100;
            var result = new string[endNumber - firstNumber + 1];
            for(int i = 0; i <= endNumber - firstNumber; i++) {
                result[i] = (firstNumber + i).ToString();
            }
            return result;
        }

        /// <summary>
        /// 修改证件号
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="passengerName">乘机人姓名</param>
        /// <param name="originalCredentials">原证件号</param>
        /// <param name="newCredentials">新证件号</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="isPlatform">是否平台运营人员操作</param>
        /// <returns>
        /// 表示操作编码是否成功
        /// 只有编码操作成功，才会修改订单中的证件号信息
        /// </returns>
        public static bool UpdateCredentials(decimal orderId, string passengerName, string originalCredentials, string newCredentials, string operatorAccount, bool isPlatform,
            Guid oemId) {
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");

            bool success = false;
            var passenger = order.UpdateCredentitials(passengerName, originalCredentials, newCredentials, true, out success, oemId);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    if(success) {
                        orderRepository.UpdateCredentitals(passenger.Id, newCredentials);
                    }
                    orderRepository.SaveCredentialsUpdateInfo(order, passenger, originalCredentials, newCredentials, success,
                        isPlatform ? OperatorRole.Platform : OperatorRole.Purchaser, operatorAccount);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            if(success) {
                Order.LogHelper.SaveUpdateCredentialsLog(order, passenger, originalCredentials, isPlatform, operatorAccount);
            }
            return success;
        }

        /// <summary>
        /// 运营方处理修改失败的证件号记录
        /// </summary>
        /// <param name="credentialsUpdateInfoId">证件号修改信息Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void HandlingCredentialsForUpdateFailed(Guid credentialsUpdateInfoId, string operatorAccount, Guid oemId) {
            var credentialsUpdateInfo = OrderQueryService.QueryCredentialsUdpateInfo(credentialsUpdateInfoId);
            if(credentialsUpdateInfo == null)
                throw new NotFoundException(credentialsUpdateInfoId.ToString(), "未找到该修改记录");
            if(credentialsUpdateInfo.Success)
                return;
            var order = OrderQueryService.QueryOrder(credentialsUpdateInfo.OrderId);
            if(order == null)
                throw new NotFoundException(credentialsUpdateInfo.OrderId.ToString(), "原订单不存在");
            bool success = false;
            var passenger = order.UpdateCredentitials(credentialsUpdateInfo.Passenger, credentialsUpdateInfo.OriginalCredentials, credentialsUpdateInfo.NewCredentials, false,
                out success, oemId);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.UpdateCredentitals(passenger.Id, credentialsUpdateInfo.NewCredentials);
                    orderRepository.SaveCredentialsUpdateInfo(order, passenger, credentialsUpdateInfo.OriginalCredentials, credentialsUpdateInfo.NewCredentials, success,
                        OperatorRole.Platform, operatorAccount);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveUpdateCredentialsLog(order, passenger, credentialsUpdateInfo.OriginalCredentials, true, operatorAccount);
        }

        /// <summary>
        /// 处理分润失败的订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static void ProcessRoyaltyFailedOrder(decimal orderId) {
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString());
            var payBill = order.Bill.PayBill;
            if(payBill.RequireSubPay && !payBill.SubPaySucceed) {
                if(!Tradement.RoyaltyService.SubPay(order, payBill)) return;
                DistributionProcessService.SubPaySuccess(payBill, DateTime.Now);
                using(var command = Factory.CreateCommand()) {
                    command.BeginTransaction();
                    var repository = Factory.CreateDistributionRepository(command);
                    try {
                        repository.UpdatePayBillForRoyaltiesTradeSuccess(payBill);
                        command.CommitTransaction();
                    } catch(System.Data.Common.DbException ex) {
                        command.RollbackTransaction();
                        LogService.SaveExceptionLog(ex);
                        throw new Exception("系统错误", ex);
                    } catch(Exception ex) {
                        command.RollbackTransaction();
                        LogService.SaveExceptionLog(ex);
                        throw;
                    }
                }
            }
            if(Tradement.RoyaltyService.Royalty(order, payBill)) {
                DistributionProcessService.RoyaltiesTradeSuccess(payBill, DateTime.Now);
                using(var command = Factory.CreateCommand()) {
                    command.BeginTransaction();
                    var repository = Factory.CreateDistributionRepository(command);
                    try {
                        repository.UpdatePayBillForRoyaltiesTradeSuccess(payBill);
                        command.CommitTransaction();
                    } catch(System.Data.Common.DbException ex) {
                        command.RollbackTransaction();
                        LogService.SaveExceptionLog(ex);
                        throw new Exception("系统错误", ex);
                    } catch(Exception ex) {
                        command.RollbackTransaction();
                        LogService.SaveExceptionLog(ex);
                        throw;
                    }
                }
            }
        }

        public static void ProcessRefundFailedRecord(decimal applyformId) {
            var repository = Factory.CreateRefundRepository();
            var refundInfo = repository.Query(applyformId);
            if(refundInfo != null) {
                switch(refundInfo.BusinessType) {
                    case RefundBusinessType.Cancel:
                    case RefundBusinessType.DenySupply:
                        Tradement.RefundmentService.ProcessTradeRefundemnt(refundInfo);
                        break;
                    case RefundBusinessType.PayTimeout:
                        Tradement.RefundmentService.ProcessTradeRefundemnt(refundInfo);
                        break;
                    case RefundBusinessType.Refund:
                    case RefundBusinessType.Scrap:
                        Tradement.RefundmentService.ProcessOrderRefundment(refundInfo);
                        break;
                    case RefundBusinessType.DenyPostpone:
                        Tradement.RefundmentService.ProcessPostponeRefundment(refundInfo);
                        break;
                }
            }
        }

        /// <summary>
        /// 判断该编码是否需要取消
        /// </summary>
        /// <param name="pnrCode">编码内容</param>
        /// <param name="producedTime">编码生成时间</param>
        public static bool RequireCancelPNR(string pnrCode, DateTime producedTime) {
            if(string.IsNullOrWhiteSpace(pnrCode)) throw new ArgumentNullException("pnrCode");
            if(producedTime == DateTime.MinValue) throw new ArgumentNullException("producedTime");

            // 如果未超过取消编码时限，则不用处理
            if(producedTime.AddMinutes(Service.SystemManagement.SystemParamService.CancelPnrLimit) > DateTime.Now) return false;
            using(var command = Factory.CreateCommand()) {
                var orderRepository = Factory.CreateOrderRepository(command);
                var order = orderRepository.QueryOrder(pnrCode, producedTime);
                // 如果未生成订单，需要取消
                if(order == null) return true;
                // 如果是导入的，则不管
                if(order.Source == OrderSource.CodeImport || order.Source == OrderSource.ContentImport || order.Source == OrderSource.InterfaceOrder) return false;
                // 不管特殊票
                if(order.IsSpecial) return false;
                // 如果是平台产生的编码，并且订单未支付，则需要取消
                switch(order.Status) {
                    case OrderStatus.Applied:
                    case OrderStatus.ConfirmFailed:
                    case OrderStatus.Ordered:
                    case OrderStatus.Canceled:
                        return true;
                    case OrderStatus.Finished:
                        return order.ReservationPNR != null && PNRPair.Equals(order.ReservationPNR.PNR, pnrCode) && !PNRPair.Equals(order.ReservationPNR, order.ETDZPNR);
                    case OrderStatus.PaidForSupply:
                    case OrderStatus.DeniedWithSupply:
                    case OrderStatus.PaidForETDZ:
                    case OrderStatus.DeniedWithETDZ:
                        return false;
                    default:
                        throw new NotSupportedException("未知订单状态");
                }
            }
        }

        /// <summary>
        /// 处理待支付订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static void ProcessWaitForPayOrder(decimal orderId) {
            var order = OrderQueryService.QueryOrder(orderId);
            if(order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            if(!order.IsPayTimeout) return;
            order.Cancel("超出支付时限");

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.UpdateStatus(order.Id, order.Status, order.Remark);
                    // 恢复相应资源数
                    ResumeResource(order);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveOrderTimeoutLog(order);
            Order.StatisticHelper.Statistic(order);
            Order.RemindHelper.RemindOrder(order);
            var notifier = new Order.Notify.OrderNotifier(order);
            notifier.Execute();

            LockService.UnLockForcibly(orderId.ToString());
            if(!order.IsB3BOrder) {
                //拒绝出票时候取消外平台订单
                var extOrderInfo = OrderQueryService.QueryOrderExternalInfo(orderId);
                ExternalPlatform.OrderService.Cancel(extOrderInfo.Platform, order.Id, extOrderInfo.ExternalOrderId, order.PNRInfos.First().Passengers.Select(p => p.Name), "超过支付时限");
            }
        }

        public static void SendMessageToPassenger(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var orderRepository = Factory.CreateOrderRepository(command);
                orderRepository.SetPassengerMsgSended(orderId);
            }
        }

        /// <summary>
        /// 记录采购方发送催单信息
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="remindContent">提醒内容</param>
        public static void Reminded(decimal orderId, string remindContent) {
            using(var command = Factory.CreateCommand()) {
                var orderRepository = Factory.CreateOrderRepository(command);
                orderRepository.Reminded(orderId, remindContent);
            }
        }

        /// <summary>
        /// 订单协调后，将采购催单状态改为false，即不需要催单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        public static void UpdateRemindStatus(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var orderRepository = Factory.CreateOrderRepository(command);
                orderRepository.UpdateRemindStatus(orderId);
            }
        }

        /// <summary>
        /// 保存外平台的生成订单的政策副本
        /// </summary>
        /// <param name="externalPolicy">外平台政策信息</param>
        /// <param name="orderId"> </param>
        /// <returns></returns>
        private static bool SaveExternalPolicyCopy(ExternalPolicyView externalPolicy, decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var orderRepository = Factory.CreateOrderRepository(command);
                return orderRepository.SaveExternalPolicyCopy(externalPolicy, orderId);
            }
        }

        /// <summary>
        /// 查询外部订单的外部政策
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static ExternalPolicyView LoadExternalPolicy(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var orderRepository = Factory.CreateOrderRepository(command);
                return orderRepository.QueryExternalPolicy(orderId);
            }
        }

        #endregion

        /// <summary>
        /// 外部订单支付成功
        /// </summary>
        /// <param name="notifyInfo"></param>
        public static void ExternalPaySucess(PaySuccessNotifyView notifyInfo) {
            var externalOrderInfo = OrderQueryService.QueryOrderExternalInfo(notifyInfo.Id);
            if(notifyInfo.Valid && notifyInfo.Payment != null) {
                externalOrderInfo.PayStatus = PayStatus.Paied;
                externalOrderInfo.PayTime = notifyInfo.Payment.PayTime;
                externalOrderInfo.PayTradNO = notifyInfo.Payment.TradeNo;
                externalOrderInfo.IsAutoPay = notifyInfo.Payment.IsAutoPay;
                externalOrderInfo.FaildInfo = string.Empty;
            } else {
                externalOrderInfo.PayStatus = PayStatus.PayFail;
                externalOrderInfo.PayTime = DateTime.Now;
                externalOrderInfo.FaildInfo = "支付通知信息验证失败";
            }
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.PayExternalOrderSuccess(externalOrderInfo);
                    command.CommitTransaction();
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex, "外部订单支付失败");
                    throw;
                }
            }
        }

        public static void ExternalPlatformETDZ(PNRPair pnrPair, string externalOrderId, IEnumerable<TicketNoView.Item> ticketNos, string settleCode, Guid oemId) {
            var externalOrder = OrderQueryService.QueryExternalOrder(externalOrderId);
            if(externalOrder == null) {
                throw new CustomException("订单不存在！");
            }
            var ticketNoView = new TicketNoView() {
                ETDZPNR = pnrPair,
                Mode = ETDZMode.Manual,
                Items = ticketNos,
                NewSettleCode = settleCode
            };
            var setting = ExternalPlatform.Processor.PlatformBase.GetPlatform(externalOrder.Platform);
            ETDZ(ticketNoView, setting.Setting.ProviderAccount, externalOrder.Platform.GetDescription(), externalOrder, oemId);
        }

        /// <summary>
        /// 接收外平台拒绝出票的通知
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="externalOrderId"></param>
        public static void ExternalOrderDenyETDZ(string reason, string externalOrderId) {
            var externalOrder = OrderQueryService.QueryExternalOrder(externalOrderId);
            if(externalOrder == null) {
                throw new CustomException("订单不存在！");
            }
            var setting = ExternalPlatform.Processor.PlatformBase.GetPlatform(externalOrder.Platform);
            DenyETDZ(reason, setting.Setting.ProviderAccount, externalOrder);
        }

        /// <summary>
        /// 查询订单外平台支付订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static ExternalOrder QueryExternalPlatformPaymentStatus(decimal orderId) {
            var extOrder = OrderQueryService.QueryExternalOrder(orderId);
            if(extOrder == null) {
                throw new CustomException("订单未找到！");
            }
            var paymentQueryResult = ExternalPlatform.OrderService.QueryPayment(extOrder.Platform, extOrder.Id, extOrder.ExternalOrderId);
            if(paymentQueryResult.Success && paymentQueryResult.Result != null) {
                extOrder.PayStatus = PayStatus.Paied;
                extOrder.PayTime = paymentQueryResult.Result.PayTime;
                extOrder.PayTradNO = paymentQueryResult.Result.TradeNo;
                extOrder.IsAutoPay = paymentQueryResult.Result.IsAutoPay;
                extOrder.FaildInfo = string.Empty;
            } else {
                extOrder.PayStatus = PayStatus.PayFail;
                extOrder.PayTime = DateTime.Now;
                extOrder.FaildInfo = "获取支付信息失败,未能查询到订单支付状态";

            }
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var orderRepository = Factory.CreateOrderRepository(command);
                    orderRepository.PayExternalOrderSuccess(extOrder);
                    command.CommitTransaction();
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex, "外部订单支付失败");
                    throw;
                }
            }
            return extOrder;
        }
        /// <summary>
        /// 验证订单信息是否满足退票条件
        /// </summary>
        /// <param name="order"></param>
        /// <param name="passgners"></param>
        /// <param name="voyages"></param>
        /// <param name="pnr"></param>
        /// <param name="delegageCancelPNR"></param>
        /// <param name="omeId"></param>
        /// <returns>Item1 编码已取消  Item2 票号未使用   Item3  票号未打印   Item4 乘机人姓名一致   Item5 所有指令均调用成功 </returns>
        public static Tuple<bool, bool, bool, bool, bool> CheckRefundCondition(Order.Domain.Order order, List<Guid> passgners, List<Guid> voyages, PNRPair pnr, bool delegageCancelPNR,
            Guid omeId) {
            var PNRCancled = true;
            var TicketUnUse = true;
            var isNotPrinted = true;
            var _isSameName = true;
            var allSuccess = true;

            var refundPassengers = new List<Passenger>();
            //var flgihts = new List<Flight>();
            //if (voyages != null)
            //{
            //    var pnrInfo = order.PNRInfos.First(item => item.IsSamePNR(pnr));
            //    foreach (var voyage in voyages)
            //    {
            //        if (voyage != null)
            //        {
            //            var flight = pnrInfo.Flights.FirstOrDefault(item => item.Id == voyage);
            //            if (flight == null) throw new NotFoundException("原编码中不存在航段信息。");
            //            flgihts.Add(flight);
            //        }
            //    }
            //}
            //if (!flgihts.Any()) throw new CustomException("缺少航段信息");

            if(passgners != null) {
                var pnrInfo = order.PNRInfos.First(item => item.IsSamePNR(pnr));
                foreach(var passenger in passgners) {
                    var originalPassenger = pnrInfo.Passengers.FirstOrDefault(item => item.Id == passenger);
                    if(originalPassenger == null) throw new NotFoundException("原编码中不存在乘机人信息。");
                    refundPassengers.Add(originalPassenger);
                }
                if(!refundPassengers.Any()) throw new CustomException("缺少乘机人信息");
            }
            var validatedTickets = new List<string>();
            foreach(Passenger refundPassenger in refundPassengers) {
                foreach(var ticket in refundPassenger.Tickets) {
                    if(validatedTickets.Any(t => t == ticket.No)) continue;
                    validatedTickets.Add(ticket.No);
                    string ticketNumber = ticket.SettleCode + "-" + ticket.No;
                    var validateResult = CommandService.GetTicketDetails(ticketNumber, omeId);
                    foreach (var flight in ticket.Flights)
                    {
                        if (!voyages.Contains(flight.Id)) continue;
                        var departureCode = flight.Departure.Code;
                        if (validateResult.Success)
                        {
                            PNRCancled &= validateResult.Result.ElectronicTicket.PnrCodeCancelled(departureCode);
                            TicketUnUse &= validateResult.Result.ElectronicTicket.TicketStatusIsOpen(departureCode);
                            isNotPrinted &= validateResult.Result.JourneySheet.IsNotUsed();
                            _isSameName &= validateResult.Result.ElectronicTicket.Name == refundPassenger.Name;
                        }
                        else if (validateResult.Result.Status != DetrErrorStatus.Authority)
                        {
                            PNRCancled = false;
                            TicketUnUse = false;
                            isNotPrinted = false;
                            _isSameName = false;
                        }
                        else
                        {
                            allSuccess = false;
                        }
                    }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
                }
            }
            PNRCancled = PNRCancled || delegageCancelPNR ||
                ((order.Source == OrderSource.PlatformOrder || order.Source == OrderSource.InterfaceReservaOrder) && order.ETDZPNR.Equals(order.ReservationPNR) );
            return new Tuple<bool, bool, bool, bool,bool>(PNRCancled, TicketUnUse, isNotPrinted, _isSameName,allSuccess);
        }
    }
}