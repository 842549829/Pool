using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using System.Text;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Order.Notify {
    class OrderNotifier : Notifier {
        public OrderNotifier(Domain.Order order) {
            if(order == null) throw new InvalidOperationException("order");
            Order = order;
        }

        public Domain.Order Order {
            get;
            private set;
        }

        public override void Execute() {
            try {
                smsNotify();
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "订单短信通知");
            }
            try {
                interfaceNotify();
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "接口订单通知");
            }
        }
        private void smsNotify() {
            switch(Order.Status) {
                case DataTransferObject.Order.OrderStatus.Applied:
                    SendAppliedMessage();
                    break;
                case DataTransferObject.Order.OrderStatus.Ordered:
                    SendConfirmSuccessMessage();
                    break;
                case DataTransferObject.Order.OrderStatus.PaidForSupply:
                    SendPaidForSupplyMessage();
                    break;
            }
        }
        private void interfaceNotify() {
            if (Order.Source == OrderSource.InterfaceOrder || Order.Source == OrderSource.InterfaceReservaOrder)
            {
                switch (Order.Status)
                {
                    case OrderStatus.ConfirmFailed:
                        SendConfirmFailedNotify();
                        break;
                    case OrderStatus.Ordered:
                        if (Order.IsSpecial && Order.RequireConfirm)
                        {
                            SendConfirmSuccessNotify();
                        }
                        break;
                    case OrderStatus.PaidForSupply:
                        SendPaidNotify();
                        break;
                    case OrderStatus.PaidForETDZ:
                        if (!Order.IsSpecial)
                        {
                            SendPaidNotify();
                        }
                        break;
                        //case OrderStatus.DeniedWithSupply:
                    case OrderStatus.Canceled:
                        //case OrderStatus.DeniedWithETDZ:
                        SendCanceledNotify();
                        break;
                    case OrderStatus.Finished:
                        SendETDZSuccessNotify();
                        break;
                }
            }
        }
        private string PlatformName {
            get {
                return Order.IsOEMOrder ? Order.OemInfo.SiteName : ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultPlatformName;
            }
        }
        private void SendAppliedMessage() {
            if(!Order.RevisedPrice) {
                SMS.Service.SMSSendService.SendB3BAppliedForConfirm(getSupplierPhone(), getFlights(), getPassengers(), PlatformName);
            }
        }
        private void SendConfirmSuccessMessage() {
            if(Order.IsSpecial && Order.RequireConfirm) {
                SMS.Service.SMSSendService.SendB3BConfirmSuccess(getPurchaserPhone(), getFlights(), getPassengers(), PlatformName);
            }
        }
        private void SendPaidForSupplyMessage() {
            SMS.Service.SMSSendService.SendB3BPaidForSupply(getSupplierPhone(), getFlights(), getPassengers(), PlatformName);
        }

        private IEnumerable<string> getPassengers() {
            return Order.PNRInfos.First().Passengers.Select(GetPassenger);
        }
        private IEnumerable<SMS.Service.Templete.FlightView> getFlights() {
            return Order.PNRInfos.First().Flights.Select(GetFlightView);
        }
        private string getPurchaserPhone() {
            return Order.Contact.Mobile;
        }
        private string getSupplierPhone() {
            return Order.IsThirdRelation ? Order.Supplier.Company.ContactPhone : Order.Provider.Company.ContactPhone;
        }

        private void SendConfirmFailedNotify() {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Order.Purchaser.CompanyId);
            if(interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.ConfirmFailAddress)) {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "2" },
                                                 { "id", Order.Id.ToString() },
                                                 { "reason", Order.Remark }
                                             };
                SendNotifyRequest("确认失败", interfaceSetting.ConfirmFailAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        private void SendConfirmSuccessNotify() {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Order.Purchaser.CompanyId);
            if(interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.PaySuccessAddress)) {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "1" },
                                                 { "id", Order.Id.ToString() },
                                                 { "pnr", Order.ReservationPNR.BPNR + "|" + Order.ReservationPNR.PNR },
                                                 { "fare", Order.PNRInfos.First().SinglePrice.Fare.ToString("F2") },
                                                 { "confirmTime", Order.SupplyTime.Value.ToString("yyyy-MM-dd HH:mm:ss") }
                                             };
                SendNotifyRequest("确认成功", interfaceSetting.ConfirmSuccessAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        private void SendPaidNotify() {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Order.Purchaser.CompanyId);
            if(interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.PaySuccessAddress)) {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "3" },
                                                 { "id", Order.Id.ToString() },
                                                 { "amount", Order.Bill.PayBill.Purchaser.Amount.ToString("F2") },
                                                 { "tradeNo", Order.Bill.PayBill.Tradement.TradeNo },
                                                 { "channelTradeNo", Order.Bill.PayBill.Tradement.ChannelTradeNo },
                                                 { "payTime", Order.Purchaser.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") }
                                             };
                SendNotifyRequest("支付成功", interfaceSetting.PaySuccessAddress, interfaceSetting.SecurityCode, parameters);
            }

        }
        private void SendCanceledNotify() {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Order.Purchaser.CompanyId);
            if(interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.PaySuccessAddress)) {
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "5" },
                                                 { "id", Order.Id.ToString() },
                                                 { "reason", Order.Remark },
                                                 { "subType",Order.Purchaser.PayTime.HasValue ? "2" : "1"}
                                             };
                SendNotifyRequest("取消出票", interfaceSetting.RefuseAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        /// <summary>
        /// 发送订单退款成功通知
        /// </summary>
        public void SendRefundSuccessNotify(){
            if (Order.Source == OrderSource.InterfaceOrder || Order.Source == OrderSource.InterfaceReservaOrder)
            {
                try
                {
                    var interfaceSetting = Organization.ExternalInterfaceService.Query(Order.Purchaser.CompanyId);
                    if (interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.RefundSuccessAddress))
                    {
                        var parameters = new Dictionary<string, string>
                            {
                                {"type", "13"},
                                {"id", Order.Id.ToString()},
                                {"amount", Order.Bill.PayBill.Purchaser.Amount.ToString()},
                                {"refundTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
                            };
                        SendNotifyRequest("订单退款成功", interfaceSetting.CanceldulingAddress, interfaceSetting.SecurityCode, parameters);
                    }
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex, "订单退款成功通知");
                }
            }
        }
        private void SendETDZSuccessNotify() {
            var interfaceSetting = Organization.ExternalInterfaceService.Query(Order.Purchaser.CompanyId);
            if(interfaceSetting != null && !string.IsNullOrWhiteSpace(interfaceSetting.PaySuccessAddress)) {
                var pnr = Order.PNRInfos.First();
                var tickets = pnr.Passengers.Join("^", p => p.Name + ":" + p.Tickets.Join("-", t => t.No));
                var parameters = new Dictionary<string, string>
                                             {
                                                 { "type", "4" },
                                                 { "id", Order.Id.ToString() },
                                                 { "settleCode", pnr.Passengers.First().Tickets.First().SettleCode },
                                                 { "tickets", tickets },
                                                 { "etdzTime", Order.ETDZTime.Value.ToString("yyyy-MM-dd HH:mm:ss") }
                                             };
                SendNotifyRequest("出票成功", interfaceSetting.DrawSuccessAddress, interfaceSetting.SecurityCode, parameters);
            }
        }
        private void SendNotifyRequest(string type, string address, string securityCode, Dictionary<string, string> parameters) {
            SendNotifyRequest(Order.Id,type,address,securityCode,parameters);
        }
    }
}