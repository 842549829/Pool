using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    /// <summary>
    /// 废票申请
    /// </summary>
    public class ScrapApplyform : RefundOrScrapApplyform {
        internal ScrapApplyform(decimal orderId, decimal applyformId)
            : base(orderId, applyformId) {
        }
        internal ScrapApplyform(Order order, ScrapApplyformView scrapApplyformView, Guid oemId)
            : base(order, scrapApplyformView,oemId) {
        }

        public override string ToString() {
            return "废票";
        }

        protected override IEnumerable<Service.Distribution.Domain.Bill.Refund.RefundFlight> AgreeByProviderExecuteCore(RefundProcessView processView) {
            var supplierFee = SystemManagement.SystemParamService.ResourcerRate;
            var providerFee = SystemManagement.SystemParamService.ProviderRate;
            var providerFeePerTicket = IsSpecial && !Order.IsThirdRelation ? (providerFee + supplierFee) : providerFee;
            var supplierFeePerTicket = IsSpecial && Order.IsThirdRelation ? supplierFee : 0;
            var purchaserFeePerTicket = providerFeePerTicket + supplierFeePerTicket;
            foreach(var flight in Flights) {
                var flightCount = flight.OriginalFlight.Ticket.Flights.Count();
                var fee = purchaserFeePerTicket / flightCount;
                flight.RefundRate = 0;
                decimal? refundServiceCharge = null;
                var specialBunk = flight.OriginalFlight.Bunk as Bunk.SpecialBunk;
                if(specialBunk != null) {
                    // 废票时，服务费全退
                    refundServiceCharge = specialBunk.ServiceCharge;
                }
                yield return new Service.Distribution.Domain.Bill.Refund.RefundFlight(flight.OriginalFlight.ReservateFlight) {
                    Rate = 0,
                    FeeForPurchaser = fee,
                    FeeForProvider = providerFeePerTicket / flightCount,
                    FeeForSupplier = supplierFeePerTicket / flightCount,
                    RefundServiceCharge = refundServiceCharge
                };
            }
        }
    }
}