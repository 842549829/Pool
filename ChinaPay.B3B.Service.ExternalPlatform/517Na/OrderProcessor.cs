using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.B3B.DataTransferObject.Policy;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.ExternalPlatform._517Na {
    class OrderProcessor : ProcessorBase, Processor.IOrderProcessor {
        public RequestResult<ExternalOrderView> Produce(decimal orderId, OrderView orderView, ExternalPolicyView policy) {
            throw new NotImplementedException();
        }

        public RequestResult<ExternalOrderView> Produce(decimal orderId, OrderView orderView, string pnrContent, string patContent, ExternalPolicyView policy) {
            RequestResult<ExternalOrderView> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var signValue = new Dictionary<string, string>
                                {
                                    {"pnrcontent", GetPnrParameter(pnrContent)},
                                    {"bigpnr", orderView.PNR.BPNR},
                                    {"benefitid", policy.Id},
                                    {"linker", Platform.Contact},
                                    {"linktel", Platform.ContactPhone},
                                    {"splitbenefitid", (policy as _517NaPolicyView).SubId},
                                    {"patcontent", GetPnrParameter(patContent)}
                                };
                request = GetRequest("create_order_pnrcontent", signValue);
                var platform = new _517Na.BenefitInterface {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout,
                    RequestEncoding = Platform.Encoding
                };
                response = platform.InterfaceFacade(request);
                result = parseProduceResponse(response);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "517Na生成订单");
                result = new RequestResult<ExternalOrderView> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
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
                var signValue = new Dictionary<string, string>
                                {
                                    {"orderid", externalOrderId},
                                    {"amount", amount.ToString("F2")}
                                };
                request = GetRequest("pay_order", signValue);
                var platform = new _517Na.BenefitInterface {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout,
                    RequestEncoding = Platform.Encoding
                };
                response = platform.InterfaceFacade(request);
                result = parseAutoPayResponse(response);
                if(result.Success) {
                    result.Result.Id = orderId;
                    result.Result.ExternalId = externalOrderId;
                    result.Result.Payment.PayInterface = payInterface;
                    result.Result.Payment.IsAutoPay = true;
                    result.Result.Payment.PayTime = DateTime.Now;
                }
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "517Na代扣");
                result = new RequestResult<AutoPayResult> {
                    Success = false,
                    ErrMessage = ex.Message == "The operation has timed out" ? "代扣超时,请稍后再试！" : "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "代扣");
            return result;
        }

        public RequestResult<string> ManualPay(decimal orderId, string externalOrderId, PayInterface payInterface, decimal amount) {
            throw new NotImplementedException();
        }

        public RequestResult<bool> Cancel(decimal orderId, string externalOrderId, IEnumerable<string> passengers, string reason) {
            RequestResult<bool> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var signValue = new Dictionary<string, string>
                                {
                                    {"orderid", externalOrderId}
                                };
                request = GetRequest("cancel_order", signValue);
                var platform = new _517Na.BenefitInterface {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout,
                    RequestEncoding = Platform.Encoding
                };
                response = platform.InterfaceFacade(request);
                result = parseCancelOrderResponse(response);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "517Na取消订单");
                result = new RequestResult<bool> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
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
                var signValue = new Dictionary<string, string>
                                {
                                    {"orderid", externalOrderId}
                                };
                request = GetRequest("get_orderinfo", signValue);
                var platform = new _517Na.BenefitInterface {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout,
                    RequestEncoding = Platform.Encoding
                };
                response = platform.InterfaceFacade(request);
                result = parseQueryPaymentResponse(response);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "获取517Na订单信息");
                result = new RequestResult<Payment> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
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
                var signValue = new Dictionary<string, string>
                                {
                                    {"orderid", externalOrderId}
                                };
                request = GetRequest("get_orderinfo", signValue);
                var platform = new _517Na.BenefitInterface {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout,
                    RequestEncoding = Platform.Encoding
                };
                response = platform.InterfaceFacade(request);
                result = parseQueryTicketNoResponse(response);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "获取517Na订单信息");
                result = new RequestResult<TicketInfo> {
                    Success = false,
                    ErrMessage = ex.Message == "The operation has timed out" ? "操作超时,请稍后再试！" : "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "查询票号信息");
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
                    Id = GetAttributeValue(doc.SelectSingleNode("/OrderInfo"), "OrderId"),
                    Amount = decimal.Parse(GetAttributeValue(doc.SelectSingleNode("/OrderInfo"), "TotlePirce"))
                };
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }

        private RequestResult<AutoPayResult> parseAutoPayResponse(string response) {
            var result = new RequestResult<AutoPayResult>();
            var doc = new XmlDocument();
            doc.LoadXml(response);
            string errorMessage;
            if(ResponseSuccess(doc, out errorMessage)) {
                result.Success = true;
                var payResultNode = doc.SelectSingleNode("/PayResult");
                var paySuccessText = GetAttributeValue(payResultNode, "PaySuccess");
                result.Result = new AutoPayResult {
                    Payment = new Payment {
                        TradeNo = string.Empty
                    }
                };
                if(paySuccessText == "True") {
                    result.Result.Success = true;
                } else {
                    result.Result.Success = false;
                    result.Result.ErrorMessage = GetAttributeValue(payResultNode, "ErrorDescription");
                }
            } else {
                result.Success = false;
                result.ErrMessage = errorMessage;
            }
            return result;
        }

        private RequestResult<bool> parseCancelOrderResponse(string response) {
            var result = new RequestResult<bool>();
            var doc = new XmlDocument();
            doc.LoadXml(response);
            string errorMessage;
            if(ResponseSuccess(doc, out errorMessage)) {
                result.Success = true;
                result.Result = GetAttributeValue(doc.SelectSingleNode("/Result"), "SUCCESS") == "TRUE";
            } else {
                result.Success = false;
                result.ErrMessage = errorMessage;
            }
            return result;
        }

        private RequestResult<Payment> parseQueryPaymentResponse(string response) {
            var result = new RequestResult<Payment>();
            var doc = new XmlDocument();
            doc.LoadXml(response);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                result.Success = true;
                var payTimeText = GetAttributeValue(doc.SelectSingleNode("/Order/OrderInfo"), "PayTime");
                if(!string.IsNullOrWhiteSpace(payTimeText)) {
                    result.Result = new Payment {
                        PayTime = DateTime.Parse(payTimeText)
                    };
                }
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }

        private RequestResult<TicketInfo> parseQueryTicketNoResponse(string response) {
            var result = new RequestResult<TicketInfo>();
            var doc = new XmlDocument();
            doc.LoadXml(response);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                var outTicketTimeText = GetAttributeValue(doc.SelectSingleNode("/Order/OrderInfo"), "OutTicketTime");
                if(!string.IsNullOrWhiteSpace(outTicketTimeText)) {
                    result.Success = true;
                    var passengersNode = doc.SelectSingleNode("/Order/PassengersInfo");
                    string settlCode = null;
                    var ticketNoViews = new List<TicketNoView.Item>();
                    foreach(XmlNode passengerNode in passengersNode.ChildNodes) {
                        var ticketNoText = GetAttributeValue(passengerNode, "TicketNo");
                        var ticketNoMatch = Regex.Match(ticketNoText, @"(?<Code>\d+)-(?<No>\d+)");
                        if(ticketNoMatch.Success) {
                            var passengerName = GetAttributeValue(passengerNode, "Name");
                            if(string.IsNullOrWhiteSpace(settlCode)) {
                                settlCode = ticketNoMatch.Groups["Code"].Value;
                            }
                            ticketNoViews.Add(new TicketNoView.Item {
                                Name = passengerName,
                                TicketNos = new[] { ticketNoMatch.Groups["No"].Value }
                            });
                        }
                    }
                    result.Result = new TicketInfo {
                        SettleCode = settlCode,
                        TicketNos = ticketNoViews
                    };
                } else {
                    result.Success = false;
                    result.ErrMessage = "无票号信息";
                }
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }
    }
}