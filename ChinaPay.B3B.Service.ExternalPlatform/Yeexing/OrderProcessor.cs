using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.ExternalPlatform.Yeexing {
    class OrderProcessor : RequestProcessorBase, Processor.IOrderProcessor {
        public RequestResult<ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, DataTransferObject.Policy.ExternalPolicyView policy) {
            RequestResult<ExternalOrderView> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address
                };
                var pnr = orderView.PNR.PNR;
                var yeexingPolicy = policy as DataTransferObject.Policy.YeexingPolicyView;
                var signValue = new Dictionary<string, string>
                                {
                                    {"userName", Platform.UserName},
                                    {"pnr", pnr},
                                    {"plcid", policy.Id},
                                    {"ibePrice", yeexingPolicy.IBEPrice},
                                    {"out_orderid", orderId.ToString()},
                                    {"disc", yeexingPolicy.Disc},
                                    {"extReward", yeexingPolicy.extReward}
                                };
                var signText = Sign(signValue);
                request = GetRequestValue(signValue, signText);
                response = platform.PnrBook(Platform.UserName, pnr, policy.Id, yeexingPolicy.IBEPrice, orderId.ToString(), yeexingPolicy.Disc, yeexingPolicy.extReward, signText);
                result = parseProduceResponse(response);
            } catch(Exception ex) {
                result = new RequestResult<ExternalOrderView> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                LogService.SaveExceptionLog(ex, "易行生成订单");
                response = ex.Message;
            }
            SaveRequestLog(response, request, "生成订单");
            return result;
        }

        public RequestResult<ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, string pnrContent, string patContent, DataTransferObject.Policy.ExternalPolicyView policy) {
            if(orderView.UseBPNR) return Produce(orderId, orderView, policy);
            RequestResult<ExternalOrderView> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address
                };
                var pnr = orderView.UseBPNR?orderView.PNR.BPNR:orderView.PNR.PNR;
                var pnrText = GetPnrParameter(pnrContent, patContent);
                var yeexingPolicy = policy as DataTransferObject.Policy.YeexingPolicyView;
                var signValue = new Dictionary<string, string>
                                {
                                    {"userName", Platform.UserName},
                                    {"pnr", pnr},
                                    {"pnrText", pnrText},
                                    {"plcid", policy.Id},
                                    {"ibePrice", yeexingPolicy.IBEPrice},
                                    {"out_orderid", orderId.ToString()},
                                    {"disc", yeexingPolicy.Disc},
                                    {"extReward", yeexingPolicy.extReward}
                                };
                var signText = Sign(signValue);
                request = GetRequestValue(signValue, signText);
                response = platform.ParsePnrBook(Platform.UserName, pnr, pnrText, policy.Id, yeexingPolicy.IBEPrice, orderId.ToString(), yeexingPolicy.Disc, yeexingPolicy.extReward, signText);
                result = parseProduceResponse(response);
            } catch(Exception ex) {
                result = new RequestResult<ExternalOrderView> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                LogService.SaveExceptionLog(ex, "易行生成订单");
                response = ex.Message;
            }
            SaveRequestLog(response, request, "生成订单");
            return result;
        }

        public RequestResult<AutoPayResult> AutoPay(decimal orderId, string externalOrderId, PayInterface payInterface, decimal amount) {
            RequestResult<AutoPayResult> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address
                };
                var amountText = amount.ToString("F2");
                var payInterfaceValue = Platform.GetPayInterfaceValue(payInterface);
                var signValue = new Dictionary<string, string>
                                    {
                                        {"userName", Platform.UserName},
                                        {"orderid", externalOrderId},
                                        {"totalPrice", amountText},
                                        {"payPlatform", payInterfaceValue},
                                        {"pay_notify_url", Platform.NotifyUrl},
                                        {"out_notify_url", Platform.NotifyUrl}
                                    };
                var signText = Sign(externalOrderId + Platform.NotifyUrl + payInterfaceValue + Platform.NotifyUrl + amountText + Platform.UserName);
                request = GetRequestValue(signValue, signText);
                response = platform.PayOut(Platform.UserName, externalOrderId, amountText, payInterfaceValue, Platform.NotifyUrl, Platform.NotifyUrl, signText);
                result = parseAutoPayResponse(response);
                if(result.Success) {
                    result.Result.Id = orderId;
                    result.Result.Payment.PayInterface = payInterface;
                    result.Result.Payment.IsAutoPay = true;
                    result.Result.Payment.PayTime = DateTime.Now;
                }
            } catch(Exception ex) {
                result = new RequestResult<AutoPayResult> {
                    Success = false,
                    ErrMessage = ex.Message=="The operation has timed out"?"代扣超时,请稍后再试！":"系统错误，请联系平台"
                };
                LogService.SaveExceptionLog(ex, "易行" + payInterface.GetDescription() + "代扣");
                response = ex.Message;
            }
            SaveRequestLog(response, request, "代扣");
            return result;
        }

        public RequestResult<string> ManualPay(decimal orderId, string externalOrderId, PayInterface payInterface, decimal amount) {
            RequestResult<string> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address
                };
                var amountText = amount.ToString("F2");
                var payInterfaceValue = Platform.GetPayInterfaceValue(payInterface);
                var signValue = new Dictionary<string, string>
                                {
                                    {"userName", Platform.UserName},
                                    {"orderid", externalOrderId},
                                    {"totalPrice", amountText},
                                    {"payPlatform", payInterfaceValue},
                                    {"pay_notify_url", Platform.NotifyUrl},
                                    {"out_notify_url", Platform.NotifyUrl}
                                };
                var signText = Sign(externalOrderId + Platform.NotifyUrl + payInterfaceValue + Platform.NotifyUrl + amountText + Platform.UserName);
                request = GetRequestValue(signValue, signText);
                response = platform.Pay(Platform.UserName, externalOrderId, amountText, payInterfaceValue, Platform.NotifyUrl, Platform.NotifyUrl, signText);
                result = parseManualPayResponse(response);
            } catch(Exception ex) {
                result = new RequestResult<string> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                LogService.SaveExceptionLog(ex, "易行" + payInterface.GetDescription() + "手动支付");
                response = ex.Message;
            }
            SaveRequestLog(response, request, "手动支付");
            return result;
        }

        public RequestResult<bool> Cancel(decimal orderId, string externalOrderId, IEnumerable<string> passengers, string reason) {
            if(passengers == null || !passengers.Any()) throw new ArgumentNullException("passengers");

            RequestResult<bool> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address
                };
                var passengerNames = string.Join("^", passengers);
                var canceledNotifyUrl = Platform.NotifyUrl;
                var signValue = new Dictionary<string, string>
                                {
                                    {"userName", Platform.UserName},
                                    {"orderid", externalOrderId},
                                    {"passengerName", passengerNames},
                                    {"cancel_notify_url", canceledNotifyUrl}
                                };
                var signText = Sign(signValue);
                request = GetRequestValue(signValue, signText);
                response = platform.CancelTicket(Platform.UserName, externalOrderId, passengerNames, canceledNotifyUrl, signText);
                result = parseCancelResponse(response);
            } catch(Exception ex) {
                result = new RequestResult<bool> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                LogService.SaveExceptionLog(ex, "易行取消订单");
                response = ex.Message;
            }
            SaveRequestLog(response, request, "取消订单");
            return result;
        }

        public RequestResult<Payment> QueryPayment(decimal orderId, string externalOrderId) {
            RequestResult<Payment> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address
                };
                var signValue = new Dictionary<string, string>
                                    {
                                        {"userName", Platform.UserName},
                                        {"orderid", externalOrderId},
                                        {"out_orderid", orderId.ToString()}
                                    };
                var signText = Sign(signValue);
                request = GetRequestValue(signValue, signText);
                response = platform.Order_Query(Platform.UserName, externalOrderId, orderId.ToString(), signText);
                result = parseQueryPaymentResponse(response);
            } catch(Exception ex) {
                result = new RequestResult<Payment> {
                    Success = false,
                    ErrMessage = ex.Message == "The operation has timed out" ? "操作超时,请稍后再试！" : "系统错误，请联系平台"
                };
                LogService.SaveExceptionLog(ex, "获取易行订单信息");
                response = ex.Message;
            }
            SaveRequestLog(response, request, "查询支付信息");
            return result;
        }

        public RequestResult<TicketInfo> QueryTicketNo(decimal orderId, string externalOrderId) {
            RequestResult<TicketInfo> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"userName", Platform.UserName},
                                    {"orderid", externalOrderId},
                                    {"out_orderid", orderId.ToString()}
                                };
                var signText = Sign(signValue);
                request = GetRequestValue(signValue, signText);
                response = platform.Order_QueryAirid(Platform.UserName, externalOrderId, orderId.ToString(), signText);
                result = parseTicketInfo(response);
            } catch(Exception ex) {
                result = new RequestResult<TicketInfo> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                LogService.SaveExceptionLog(ex, "易行查询票号");
                response = ex.Message;
            }
            SaveRequestLog(response, request, "查询票号");
            return result;
        }

        private RequestResult<ExternalOrderView> parseProduceResponse(string produceResponse) {
            var result = new RequestResult<ExternalOrderView>();
            var doc = new XmlDocument();
            doc.LoadXml(produceResponse);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                result.Success = true;
                result.Result = new ExternalOrderView {
                    Id = GetAttributeValue(doc.SelectSingleNode("/result/orderInfo"), "ordered"),
                    Amount = decimal.Parse(GetAttributeValue(doc.SelectSingleNode("/result/price"), "totalPrice"))
                };
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }

        private RequestResult<AutoPayResult> parseAutoPayResponse(string payResponse) {
            var result = new RequestResult<AutoPayResult>();
            var doc = new XmlDocument();
            doc.LoadXml(payResponse);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                result.Success = true;
                result.Result = new AutoPayResult {
                    ExternalId = doc.SelectSingleNode("/result/orderid").InnerText,
                    Payment = new Payment {
                        TradeNo = doc.SelectSingleNode("/result/payid").InnerText
                    },
                    Success = doc.SelectSingleNode("/result/pay_status").InnerText == "T"
                };
                if(!result.Result.Success) {
                    result.Result.ErrorMessage = doc.SelectSingleNode("/result/error").InnerText;
                }
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }

        private RequestResult<string> parseManualPayResponse(string payResponse) {
            var result = new RequestResult<string>();
            var doc = new XmlDocument();
            doc.LoadXml(payResponse);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                result.Success = true;
                result.Result = doc.SelectSingleNode("/result/payurl").InnerText;
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }

        private RequestResult<bool> parseCancelResponse(string cancelRespose) {
            var result = new RequestResult<bool>();
            var doc = new XmlDocument();
            doc.LoadXml(cancelRespose);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                result.Success = result.Result = true;
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }

        private RequestResult<Payment> parseQueryPaymentResponse(string queryResponse) {
            var result = new RequestResult<Payment>();
            var doc = new XmlDocument();
            doc.LoadXml(queryResponse);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                result.Success = true;
                var payPlatform = GetAttributeValue(doc.SelectSingleNode("/result/orderInfo"), "payType");
                if(!string.IsNullOrWhiteSpace(payPlatform)) {
                    result.Result = new Payment {
                        PayTime = DateTime.Now,
                        PayInterface = Platform.GetPayInterface(payPlatform)
                    };
                }
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }

        private RequestResult<TicketInfo> parseTicketInfo(string ticketResponse) {
            var result = new RequestResult<TicketInfo>();
            var doc = new XmlDocument();
            doc.LoadXml(ticketResponse);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                var ticketNoText = doc.SelectSingleNode("/result/airId").InnerText;
                if(string.IsNullOrWhiteSpace(ticketNoText) || ticketNoText.ToLower().Contains("null")) {
                    result.Success = false;
                    result.ErrMessage = "无票号信息";
                } else {
                    var passengers = doc.SelectSingleNode("/result/passengerName").InnerText.Split('^');
                    var ticketNos = ticketNoText.Split('^');
                    result.Success = true;
                    result.Result = new TicketInfo();
                    var newPNRCode = doc.SelectSingleNode("/result/newPnr").InnerText;
                    var settleCode = string.Empty;
                    if(!string.IsNullOrWhiteSpace(newPNRCode)) {
                        result.Result.NewPNR = new PNRPair(newPNRCode, string.Empty);
                    }
                    var ticketNoViews = new List<DataTransferObject.Order.TicketNoView.Item>();
                    var index = 0;
                    foreach(var passenger in passengers) {
                        var ticketNoForPassenger = new DataTransferObject.Order.TicketNoView.Item {
                            Name = passenger
                        };
                        if(ticketNos.Length > index) {
                            var ticketFullNo = ticketNos[index];
                            if(settleCode.Length == 0) {
                                settleCode = ticketFullNo.Substring(0, 3);
                            }
                            ticketNoForPassenger.TicketNos = new[] { ticketFullNo.Substring(3, ticketFullNo.Length - 3) };
                        }
                        ticketNoViews.Add(ticketNoForPassenger);
                        index++;
                    }
                    result.Result.SettleCode = settleCode;
                    result.Result.TicketNos = ticketNoViews;
                }
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }
    }
}