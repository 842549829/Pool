using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Notify
{
    class PostponeApplyformNotifier : Notifier
    {
        public PostponeApplyformNotifier(Domain.Applyform.PostponeApplyform applyform)
        {
            if (applyform == null) throw new ArgumentNullException("applyform");
            this.Applyform = applyform;
        }

        public Domain.Applyform.PostponeApplyform Applyform
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
                LogService.SaveExceptionLog(ex, "接口改期票通知");
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
        void interfaceNotify()
        {
            switch (Applyform.Status)
            {
                case PostponeApplyformStatus.Denied:
                    SendRefuseChangeNotify();
                    break;
                case PostponeApplyformStatus.Agreed:
                    SendAgreedNotify();
                    break;
                case PostponeApplyformStatus.Paid:
                    SendReschPaymentNotify();
                    break;
                case PostponeApplyformStatus.Postponed:
                    SendReschedulingtNotify();
                    break;
            }
        }
        /// <summary>
        /// 同意改期通知
        /// </summary>
        private void SendAgreedNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.AgreedAddress))
            {
                var parameters = new Dictionary<string, string>
                                                 {
                                                     { "type", "9" },
                                                     { "id", Applyform.Id.ToString() },  
                                                     { "feeInfo",Applyform.Flights.Join("^",item => item.OriginalFlight.Departure.Code+item.OriginalFlight.Arrival.Code + "|" +item.PostponeFee) },
                                                     { "amount", Applyform.PayBill == null ?"0": Applyform.PayBill.Tradement.Amount.ToString()  }
                                                 };
                SendNotifyRequest("同意改期", interfaceSetting.AgreedAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        /// <summary>
        /// 拒绝改期通知
        /// </summary>
        private void SendRefuseChangeNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.RefuseChangeAddress))
            {
                var parameters = new Dictionary<string, string>
                                                 {
                                                     { "type", "10" },
                                                     { "id", Applyform.Id.ToString() },  
                                                     { "reason",Applyform.ProcessedFailedReason }
                                                 };
                SendNotifyRequest("拒绝改期", interfaceSetting.RefuseChangeAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        /// <summary>
        /// 拒绝改期退款成功通知
        /// </summary>
        public void SendRefundApplySuccessNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.ReturnTicketSuccessAddress))
            {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "14" },
                                                 { "id", Applyform.Id.ToString() },
                                                 { "amount", Applyform.PayBill == null ?"0": Applyform.PayBill.Tradement.Amount.ToString() },
                                                 { "refundTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
                                             };
                SendNotifyRequest("拒绝改期退款成功", interfaceSetting.RefundApplySuccessAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        /// <summary>
        /// 改期支付成功通知
        /// </summary>
        private void SendReschPaymentNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.ReschPaymentAddress))
            {
                var parameters = new Dictionary<string, string>
                                                 {
                                                     { "type", "11" },
                                                     { "id", Applyform.Id.ToString() },  
                                                     { "amount", Applyform.PayBill == null ?"0":Applyform.PayBill.Tradement.Amount.ToString() },  
                                                     { "channelTradeNo", Applyform.PayBill == null ?"": Applyform.PayBill.Tradement.ChannelTradeNo },
                                                     { "tradeNo", Applyform.PayBill == null ?"":Applyform.PayBill.Tradement.TradeNo },  
                                                     { "payTime",DateTime.Now.ToString() }
                                                 };
                SendNotifyRequest("改期支付成功", interfaceSetting.ReschPaymentAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        /// <summary>
        /// 改期成功通知
        /// </summary>
        private void SendReschedulingtNotify()
        {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Applyform.Purchaser.CompanyId);
            if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.ReschedulingAddress))
            {
                var parameters = new Dictionary<string, string>
                                                 {
                                                     { "type", "12" },
                                                     { "id", Applyform.Id.ToString() },  
                                                     { "feeInfo",Applyform.Flights.Join("^",item => item.OriginalFlight.Departure.Code+item.OriginalFlight.Arrival.Code + "|" +item.PostponeFee) },
                                                     { "amount", Applyform.PayBill == null ?"0": Applyform.PayBill.Tradement.Amount.ToString()  }
                                                 };
                SendNotifyRequest("改期成功", interfaceSetting.ReschedulingAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        private void SendNotifyRequest(string type, string address, string securityCode, Dictionary<string, string> parameters)
        {
            SendNotifyRequest(Applyform.Id, type, address, securityCode, parameters);
        }

    }
}