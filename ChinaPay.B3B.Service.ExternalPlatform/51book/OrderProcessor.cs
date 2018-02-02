using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.ExternalPlatform._51book {
    class OrderProcessor : RequestProcessorBase, Processor.IOrderProcessor {
        public RequestResult<DataTransferObject.Order.External.ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, DataTransferObject.Policy.ExternalPolicyView policy) {
            throw new NotImplementedException();
        }

        public RequestResult<DataTransferObject.Order.External.ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, string pnrContent, string patContent, DataTransferObject.Policy.ExternalPolicyView policy) {
            RequestResult<DataTransferObject.Order.External.ExternalOrderView> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                pnrContent = pnrContent.Trim().TrimEnd('>');
                patContent = patContent.Trim().TrimEnd('>');
                var requestModel = new _51bookCreateOrder.createOrderByRtPatRequest() {
                    agencyCode = Platform.UserName,
                    pnrTxt = pnrContent,
                    pataTxt = patContent,
                    outOrderNo = orderId.ToString(),
                    policyIdSpecified = true,
                    policyId = int.Parse(policy.Id),
                    allowSwitchPolicySpecified = true,
                    allowSwitchPolicy = 0,
                    needProductTypeSpecified = true,
                    needProductType = 2,
                    allowSwitchPnrSpecified = true,
                    allowSwitchPnr = 1,
                    doPaySpecified = true,
                    payTypeSpecified = true,
                    linkMan = Platform.Contact,
                    linkPhone = Platform.ContactPhone,
                    createdBy = Platform.UserName,
                    ticketNotifiedUrl = Platform.ETDZNotifyUrl,
                    paymentReturnUrl = Platform.PayReturnUrl
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"agencyCode", Platform.UserName},
                                    {"createdBy", requestModel.createdBy},
                                    {"doPay", requestModel.doPay.ToString()},
                                    {"linkMan", requestModel.linkMan},
                                    {"linkPhone", requestModel.linkPhone},
                                    {"payBank", requestModel.payBank},
                                    {"payType", requestModel.payType.ToString()},
                                    {"policyId", requestModel.policyId.ToString()}
                                };
                requestModel.sign = Sign(signValue);
                var platform = new _51bookCreateOrder.CreateOrderByRtPatServiceImpl_1_0Service {
                    Url = Platform.Address_CreateOrderByPnrText
                };
                request = GetModelString(requestModel);
                var responseModel = platform.createOrderByRtPat(requestModel);
                response = GetModelString(responseModel);
                result = parseProduceResponse(responseModel);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "51book生成订单");
                result = new RequestResult<DataTransferObject.Order.External.ExternalOrderView> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "生成订单");
            return result;
        }

        public RequestResult<DataTransferObject.Order.External.AutoPayResult> AutoPay(decimal orderId, string externalOrderId, PayInterface payInterface, decimal amount) {
            RequestResult<DataTransferObject.Order.External.AutoPayResult> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var requestModel = new _51bookAutoPay.payRequest {
                    agencyCode = Platform.UserName,
                    orderNo = externalOrderId,
                    payType = "1",
                    payerLoginName = Platform.UserName,
                    param1 = Platform.GetPayInterfaceValue(payInterface)
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"agencyCode", Platform.UserName},
                                    {"orderNo", requestModel.orderNo},
                                    {"payerLoginName", requestModel.payerLoginName},
                                    {"payType", requestModel.payType}
                                };
                requestModel.sign = Sign(signValue);
                var platform = new _51bookAutoPay.PayServiceImpl_1_0Service {
                    Url = Platform.Address_AutoPayOrder
                };
                request = GetModelString(requestModel);
                var responseModel = platform.pay(requestModel);
                response = GetModelString(responseModel);
                result = parseAutoPayResponse(responseModel);
                if(result.Success) {
                    result.Result.Id = orderId;
                    result.Result.Payment.PayInterface = payInterface;
                    result.Result.Payment.IsAutoPay = true;
                }
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "51book" + payInterface.GetDescription() + "代扣");
                result = new RequestResult<DataTransferObject.Order.External.AutoPayResult> {
                    Success = false,
                    ErrMessage = ex.Message == "The operation has timed out" ? "代扣超时,请稍后再试！" : "系统错误，请联系平台"
                };
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
                var requestModel = new _51bookManualPay.getPaymentUrlRequest {
                    agencyCode = Platform.UserName,
                    orderNo = externalOrderId,
                    payType = Platform.GetPayInterfaceValue(payInterface),
                    payBank = string.Empty
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"agencyCode", Platform.UserName},
                                    {"orderNo", requestModel.orderNo},
                                    {"payType", requestModel.payType},
                                    {"payBank", requestModel.payBank}
                                };
                requestModel.sign = Sign(signValue);
                var platform = new _51bookManualPay.GetPaymentUrlServiceImpl_1_0Service {
                    Url = Platform.Address_ManualPayOrder
                };
                request = GetModelString(requestModel);
                var responseModel = platform.getPaymentUrl(requestModel);
                response = GetModelString(responseModel);
                result = parseManualPayResponse(responseModel);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "51book" + payInterface.GetDescription() + "手动支付");
                result = new RequestResult<string> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "手动支付");
            return result;
        }

        public RequestResult<bool> Cancel(decimal orderId, string externalOrderId, IEnumerable<string> passengers, string reason) {
            RequestResult<bool> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var requestModel = new _51bookCancelOrder.cancelOrderRequest {
                    agencyCode = Platform.UserName,
                    canclePNR = 0,
                    canclePNRSpecified = true,
                    orderNo = externalOrderId
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"agencyCode", Platform.UserName},
                                    {"canclePNR", requestModel.canclePNR.ToString()},
                                    {"orderNo", requestModel.orderNo}
                                };
                requestModel.sign = Sign(signValue);
                var platform = new _51bookCancelOrder.CancelOrderServiceImpl_1_0Service {
                    Url = Platform.Address_CancelOrder
                };
                request = GetModelString(requestModel);
                var responseModel = platform.cancelOrder(requestModel);
                response = GetModelString(responseModel);
                result = parseCancelResponse(responseModel);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "51book取消订单");
                result = new RequestResult<bool> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "取消订单");
            return result;
        }

        public RequestResult<DataTransferObject.Order.External.Payment> QueryPayment(decimal orderId, string externalOrderId) {
            RequestResult<DataTransferObject.Order.External.Payment> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var requestModel = new _51bookOrderDetail.getOrderByOrderNoRequest {
                    agencyCode = Platform.UserName,
                    orderNo = externalOrderId
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"agencyCode", Platform.UserName},
                                    {"orderNo", requestModel.orderNo}
                                };
                requestModel.sign = Sign(signValue);
                var platform = new _51bookOrderDetail.GetOrderByOrderNoServiceImpl_1_0Service {
                    Url = Platform.Address_CancelOrder
                };
                request = GetModelString(requestModel);
                var responseModel = platform.getOrderByOrderNo(requestModel);
                response = GetModelString(responseModel);
                result = parseQueryPaymentResponse(responseModel);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "获取51book订单信息");
                result = new RequestResult<DataTransferObject.Order.External.Payment> {
                    Success = false,
                    ErrMessage = ex.Message == "The operation has timed out" ? "操作超时,请稍后再试！" : "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "查询支付信息");
            return result;
        }

        public RequestResult<DataTransferObject.Order.External.TicketInfo> QueryTicketNo(decimal orderId, string externalOrderId) {
            RequestResult<DataTransferObject.Order.External.TicketInfo> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var requestModel = new _51bookOrderDetail.getOrderByOrderNoRequest {
                    agencyCode = Platform.UserName,
                    orderNo = externalOrderId
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"agencyCode", Platform.UserName},
                                    {"orderNo", requestModel.orderNo}
                                };
                requestModel.sign = Sign(signValue);
                var platform = new _51bookOrderDetail.GetOrderByOrderNoServiceImpl_1_0Service {
                    Url = Platform.Address_CancelOrder
                };
                request = GetModelString(requestModel);
                var responseModel = platform.getOrderByOrderNo(requestModel);
                response = GetModelString(responseModel);
                result = parseQueryTicketNoResponse(responseModel);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "获取51book订单信息");
                result = new RequestResult<DataTransferObject.Order.External.TicketInfo> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "查询票号信息");
            return result;
        }

        private RequestResult<DataTransferObject.Order.External.ExternalOrderView> parseProduceResponse(_51bookCreateOrder.createOrderByRtPatReply response) {
            var result = new RequestResult<DataTransferObject.Order.External.ExternalOrderView>();
            if(response.returnCode == "S") {
                result.Success = true;
                result.Result = new DataTransferObject.Order.External.ExternalOrderView {
                    Id = response.policyOrder.liantuoOrderNo,
                    Amount = (decimal)response.policyOrder.paymentInfo.totalPay
                };
            } else {
                result.Success = false;
                result.ErrMessage = response.returnMessage;
            }
            return result;
        }

        private RequestResult<DataTransferObject.Order.External.AutoPayResult> parseAutoPayResponse(_51bookAutoPay.payReply response) {
            var result = new RequestResult<DataTransferObject.Order.External.AutoPayResult>();
            if(response.returnCode == "S") {
                result.Success = true;
                DateTime payTime;
                if(!DateTime.TryParse(response.paymentInfo.payTime, out payTime)) {
                    payTime = DateTime.Now;
                }
                result.Result = new DataTransferObject.Order.External.AutoPayResult {
                    ExternalId = response.orderNo,
                    Success = true,
                    Payment = new DataTransferObject.Order.External.Payment {
                        PayTime = payTime,
                        TradeNo = response.paymentInfo.payTradeNo
                    }
                };
            } else {
                result.Success = false;
                result.ErrMessage = response.returnMessage;
            }
            return result;
        }

        private RequestResult<string> parseManualPayResponse(_51bookManualPay.getPaymentUrlReply response) {
            var result = new RequestResult<string>();
            if(response.returnCode == "S") {
                result.Success = true;
                result.Result = response.paymentUrl;
            } else {
                result.Success = false;
                result.ErrMessage = response.returnMessage;
            }
            return result;
        }

        private RequestResult<bool> parseCancelResponse(_51bookCancelOrder.cancelOrderReply response) {
            var result = new RequestResult<bool>();
            if(response.returnCode == "S") {
                result.Success = result.Result = true;
            } else {
                result.Success = false;
                result.ErrMessage = response.returnMessage;
            }
            return result;
        }

        private RequestResult<DataTransferObject.Order.External.Payment> parseQueryPaymentResponse(_51bookOrderDetail.getOrderByOrderNoReply response) {
            var result = new RequestResult<DataTransferObject.Order.External.Payment>();
            if(response.returnCode == "S") {
                result.Success = true;
                if(!string.IsNullOrWhiteSpace(response.policyOrder.paymentInfo.paymentUrl)) {
                    DateTime payTime;
                    if(!DateTime.TryParse(response.policyOrder.paymentInfo.payTime, out payTime)) {
                        payTime = DateTime.Now;
                    }
                    result.Result = new DataTransferObject.Order.External.Payment {
                        PayTime = payTime,
                        TradeNo = response.policyOrder.paymentInfo.payTradeNo,
                        PayInterface = Platform.GetPayInterface(response.policyOrder.paymentInfo.paymentUrl)
                    };
                }
            } else {
                result.Success = false;
                result.ErrMessage = response.returnMessage;
            }
            return result;
        }

        private RequestResult<DataTransferObject.Order.External.TicketInfo> parseQueryTicketNoResponse(_51bookOrderDetail.getOrderByOrderNoReply response) {
            var result = new RequestResult<DataTransferObject.Order.External.TicketInfo>();
            if(response.returnCode == "S") {
                result.Success = true;
                var ticketNoViews = new List<DataTransferObject.Order.TicketNoView.Item>();
                result.Result = new DataTransferObject.Order.External.TicketInfo {
                    TicketNos = ticketNoViews
                };
                if(!string.IsNullOrWhiteSpace(response.policyOrder.pnrNo)) {
                    result.Result.NewPNR = new PNRPair(response.policyOrder.pnrNo, string.Empty);
                }
                var passengerInfos = response.policyOrder.passengerList;
                if(passengerInfos != null && passengerInfos.Length > 0 && !string.IsNullOrWhiteSpace(passengerInfos[0].ticketNo)) {
                    foreach(var passenger in passengerInfos) {
                        ticketNoViews.Add(new DataTransferObject.Order.TicketNoView.Item {
                            TicketNos = new[] { passenger.ticketNo },
                            Name = passenger.name
                        });
                    }
                }
            } else {
                result.Success = false;
                result.ErrMessage = response.returnMessage;
            }
            return result;
        }
    }
}