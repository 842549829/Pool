using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Notify
{
    class RefundApplyformNotifier : Notifier
    {
        public RefundApplyformNotifier(Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            if (applyform == null) throw new ArgumentNullException("applyform");
            this.Applyform = applyform;
        }

        public Domain.Applyform.RefundOrScrapApplyform Applyform
        {
            get;
            private set;
        }

        public override void Execute()
        {
            try
            {
                interfaceNotify();
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex, "接口退废票通知");
            }
        }

        public void RefundSuccess()
        {
            try
            {
                SendRefundSuccessNotify();
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex, "退废票退款成功通知");
            }
        }
        void interfaceNotify()
        {
            switch (Applyform.Status)
            {
                case RefundApplyformStatus.Refunded:
                    SendReturnTicketSuccessNotify();
                    break;
                case RefundApplyformStatus.Denied:
                    SendRefuseTicketNotify();
                    break;
            }
        }
        private IEnumerable<SMS.Service.Templete.FlightView> getFlights()
        {
            return Applyform.Flights.Select(f => GetFlightView(f.OriginalFlight));
        }
        private string getPurchaserPhone()
        {
            return Applyform.Order.Contact.Mobile;
        }

        /// <summary>
        /// 退废票退款成功
        /// </summary>
        private void SendRefundSuccessNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.RefundSuccessAddress))
            {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "6" },
                                                 { "id", Applyform.Id.ToString() },
                                                 { "amount", Applyform.RefundBill.Purchaser.Amount.ToString() },
                                                 { "refundTime", Applyform.RefundBill.Purchaser.Time.HasValue ? Applyform.RefundBill.Purchaser.Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "" }
                                             };
                SendNotifyRequest("退废票退款成功", interfaceSetting.RefundSuccessAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        /// <summary>
        /// 退废票处理成功
        /// </summary>
        private void SendReturnTicketSuccessNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.ReturnTicketSuccessAddress))
            {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "7" },
                                                 { "id", Applyform.Id.ToString() },
                                                 { "feeInfo",Applyform.Flights.Join("^",item => item.OriginalFlight.Departure.Code+item.OriginalFlight.Arrival.Code+"|"+item.OriginalFlight.FlightNo+"|"+item.OriginalFlight.TakeoffTime.ToString("yyyy-MM-dd") +"|"+ item.RefundRate + "|" +(item.RefundServiceCharge??0)) },
                                                 { "amount", Applyform.RefundBill.Purchaser.Amount.ToString()  }
                                             };
                SendNotifyRequest("退废票处理成功", interfaceSetting.ReturnTicketSuccessAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        /// <summary>
        /// 拒绝退废票通知
        /// </summary>
        private void SendRefuseTicketNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.RefuseTicketAddress))
            {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "8" },
                                                 { "id", Applyform.Id.ToString() },
                                                 { "reason",Applyform.ProcessedFailedReason }
                                             };
                SendNotifyRequest("拒绝退废票", interfaceSetting.RefuseTicketAddress, interfaceSetting.SecurityCode, parameters);
            }
        }

        private void SendNotifyRequest(string type, string address, string securityCode, Dictionary<string, string> parameters)
        {
            SendNotifyRequest(Applyform.Id, type, address, securityCode, parameters);
        }
    }

    internal class BalanceRefundApplyformNotifier : Notifier
    {
        public BalanceRefundApplyformNotifier(BalanceRefundApplyform balanceRefundApplyform) { 
        }

        public override void Execute() { 
        }

        public void RefundSuccess() { 
        }
    }
}