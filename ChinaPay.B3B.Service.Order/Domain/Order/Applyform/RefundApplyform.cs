using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core;
using ChinaPay.Core.Exception;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    /// <summary>
    /// 退票申请
    /// </summary>
    public class RefundApplyform : RefundOrScrapApplyform {
        
        internal RefundApplyform(decimal orderId, decimal applyformId)
            : base(orderId, applyformId) {
        }
        internal RefundApplyform(Order order, RefundApplyformView refundApplyformView, Guid oemId)
            : base(order, refundApplyformView,oemId) {
            RefundType = refundApplyformView.RefundType;
            ApplyAttachment = refundApplyformView.ApplyAttachmentView;
            if(IsSpecial) {
                if(RefundType == RefundType.SpecialReason&&!refundApplyformView.DelegageCancelPNR) {
                    Status = RefundApplyformStatus.AppliedForPlatform;
                }
                foreach(var flight in Flights) {
                    var specialBunk = flight.OriginalFlight.Bunk as Bunk.SpecialBunk;
                    if(specialBunk != null) {
                        switch(RefundType) {
                            // 非自愿退票，服务费全退
                            case RefundType.Involuntary:
                                flight.RefundServiceCharge = specialBunk.ServiceCharge;
                                break;
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 附件
        /// </summary>
        public List<ApplyAttachmentView> ApplyAttachment { get; set; }
        /// <summary>
        /// 退票类型
        /// </summary>
        public RefundType RefundType { get; internal set; }

        public override string ToString() {
            return "退票";
        }

        /// <summary>
        /// 平台处理是否可退票
        /// </summary>
        internal void ProcessByPlatform(IEnumerable<PlatformProcessRefundView> refundViews) {
            if(IsSpecial) {
                if(RefundType == RefundType.SpecialReason) {
                    if(Status == RefundApplyformStatus.AppliedForPlatform || Status == RefundApplyformStatus.DeniedByProviderBusiness) {
                        if(refundViews == null) throw new ArgumentNullException("refundViews");
                        foreach(var flight in Flights) {
                            var refundFlightProcessView = refundViews.FirstOrDefault(item => flight.OriginalFlight.IsSameVoyage(item.AirportPair));
                            if(refundFlightProcessView == null)
                                throw new CustomException("缺少航段的处理信息。航段：" + flight.OriginalFlight.Departure.Code + "-" + flight.OriginalFlight.Arrival.Code);
                            if(refundFlightProcessView.ServiceCharge < 0) throw new CustomException("退还的服务费不能小于0");
                            if(refundFlightProcessView.ServiceCharge > (flight.OriginalFlight.Bunk as Bunk.SpecialBunk).ServiceCharge)
                                throw new CustomException("退还的服务费不能超过服务费总额");
                            flight.RefundServiceCharge = refundFlightProcessView.ServiceCharge;
                        }
                        Status = RefundApplyformStatus.AppliedForProvider;
                        Processed();
                    } else {
                        throw new StatusException(Id.ToString());
                    }
                } else {
                    throw new CustomException("平台不能处理非特殊原因退票");
                }
            } else {
                throw new CustomException("正常订单,平台不能处理");
            }
        }

        protected override IEnumerable<Service.Distribution.Domain.Bill.Refund.RefundFlight> AgreeByProviderExecuteCore(RefundProcessView processView) {
            if(processView.Items == null) throw new CustomException("缺少退票处理信息");
            foreach(var flight in Flights) {
                var refundRate = 0M;
                var refundFee = 0M;
                if(RefundType == RefundType.Involuntary) {
                    refundRate = 0;
                    refundFee = 0;
                } else {
                    var refundFlightProcessView = processView.Items.FirstOrDefault(item => flight.OriginalFlight.IsSameVoyage(item.AirportPair));
                    if(refundFlightProcessView == null)
                        throw new CustomException("缺少航段的处理信息。航段：" + flight.OriginalFlight.Departure.Code + "-" + flight.OriginalFlight.Arrival.Code);
                    refundRate = refundFlightProcessView.Rate;
                    refundFee = Utility.Calculator.Round(refundFlightProcessView.Fee, 0);
                }
                flight.RefundRate = refundRate;
                yield return new Service.Distribution.Domain.Bill.Refund.RefundFlight(flight.OriginalFlight.ReservateFlight) {
                    Rate = refundRate,
                    FeeForProvider = refundFee,
                    FeeForPurchaser = refundFee,
                    FeeForSupplier = 0,
                    RefundServiceCharge = flight.RefundServiceCharge
                };
            }
        }
    }
}