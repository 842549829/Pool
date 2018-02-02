using System.Linq;

namespace ChinaPay.B3B.Service.Order {
    class RemindHelper {
        public static void RemindOrder(Domain.Order order) {
            try {
                switch(order.Status) {
                    case DataTransferObject.Order.OrderStatus.Applied:
                        var appliedAcceptor = order.IsThirdRelation ? order.Supplier.CompanyId : order.Provider.CompanyId;
                        Remind.OrderRemindService.Save(order.Id, Remind.Model.RemindStatus.AppliedForConfirm, getCarrier(order), appliedAcceptor);
                        break;
                    case DataTransferObject.Order.OrderStatus.Ordered:
                        Remind.OrderRemindService.Save(order.Id, Remind.Model.RemindStatus.OrderedForPay, getCarrier(order), order.Purchaser.CompanyId);
                        break;
                    case DataTransferObject.Order.OrderStatus.PaidForSupply:
                        var supplyAcceptor = order.IsThirdRelation ? order.Supplier.CompanyId : order.Provider.CompanyId;
                        Remind.OrderRemindService.Save(order.Id, Remind.Model.RemindStatus.PaidForSupply, getCarrier(order), supplyAcceptor);
                        break;
                    case DataTransferObject.Order.OrderStatus.PaidForETDZ:
                        Remind.OrderRemindService.Save(order.Id, Remind.Model.RemindStatus.PaidForETDZ, getCarrier(order), order.Provider.CompanyId,order.CustomNo);
                        break;
                    case DataTransferObject.Order.OrderStatus.ConfirmFailed:
                    case DataTransferObject.Order.OrderStatus.DeniedWithSupply:
                    case DataTransferObject.Order.OrderStatus.DeniedWithETDZ:
                    case DataTransferObject.Order.OrderStatus.Canceled:
                    case DataTransferObject.Order.OrderStatus.Finished:
                        Remind.OrderRemindService.Delete(order.Id);
                        break;
                }
            } catch(System.Exception ex) {
                LogService.SaveExceptionLog(ex, "处理订单提醒信息");
            }
        }
        public static void RemindApplyform(Domain.Applyform.RefundOrScrapApplyform applyform) {
            if(applyform.RequireSeparatePNR) return;
            try {
                switch(applyform.Status) {
                    case DataTransferObject.Order.RefundApplyformStatus.DeniedByProviderTreasurer:
                    case DataTransferObject.Order.RefundApplyformStatus.AppliedForProvider:
                        var status = applyform is Domain.Applyform.RefundApplyform ? Remind.Model.RemindStatus.AppliedForRefund : Remind.Model.RemindStatus.AppliedForScrap;
                        Remind.OrderRemindService.Save(applyform.Id, status, getCarrier(applyform), applyform.ProviderId);
                        break;
                    case DataTransferObject.Order.RefundApplyformStatus.AgreedByProviderBusiness:
                        Remind.OrderRemindService.Save(applyform.Id, Remind.Model.RemindStatus.AgreedForReturnMoney, getCarrier(applyform), applyform.ProviderId);
                        break;
                    case DataTransferObject.Order.RefundApplyformStatus.DeniedByProviderBusiness:
                    case DataTransferObject.Order.RefundApplyformStatus.Refunded:
                        Remind.OrderRemindService.Delete(applyform.Id);
                        break;
                }
            } catch(System.Exception ex) {
                LogService.SaveExceptionLog(ex, "处理退废票申请提醒信息");
            }
        }
        public static void RemindApplyform(Domain.Applyform.PostponeApplyform applyform) {
            if(applyform.RequireSeparatePNR) return;
            try {
                switch(applyform.Status) {
                    case DataTransferObject.Order.PostponeApplyformStatus.Agreed:
                        Remind.OrderRemindService.Save(applyform.Id, Remind.Model.RemindStatus.AgreedForPostponeFee, getCarrier(applyform), applyform.PurchaserId);
                        break;
                    case DataTransferObject.Order.PostponeApplyformStatus.Cancelled:
                    case DataTransferObject.Order.PostponeApplyformStatus.Paid:
                        Remind.OrderRemindService.Delete(applyform.Id);
                        break;
                }
            } catch(System.Exception ex) {
                LogService.SaveExceptionLog(ex, "处理改期申请提醒信息");
            }
        }
        static string getCarrier(Domain.Order order) {
            return order.PNRInfos.First().Flights.First().Carrier.Code;
        }
        static string getCarrier(Domain.Applyform.RefundOrScrapApplyform applyform) {
            return applyform.Flights.First().OriginalFlight.Carrier.Code;
        }
        static string getCarrier(Domain.Applyform.PostponeApplyform applyform) {
            return applyform.Flights.First().OriginalFlight.Carrier.Code;
        }
    }
}