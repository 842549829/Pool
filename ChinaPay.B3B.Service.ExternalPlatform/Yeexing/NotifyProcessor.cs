using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.Core;
using System.Text;

namespace ChinaPay.B3B.Service.ExternalPlatform.Yeexing {
    abstract class NotifyProcessor : ProcessorBase, Processor.INotifyProcessor {
        protected System.Web.HttpContext Context;

        protected NotifyProcessor(System.Web.HttpContext context) {
            Context = context;
        }

        public static NotifyProcessor CreateProcessor(System.Web.HttpContext context) {
            var notifyType = GetParameter(context, "type");
            if(notifyType == "1") {
                return new ETDZSuccessNotify(context);
            } else if(notifyType == "2") {
                return new PaySuccessNotify(context);
            } else if(notifyType == "3") {
                return new CancelSuccessNotify(context);
            } else if(notifyType == "6") {
                return new ETDZFailedNotify(context);
            } else {
                saveNotifyLog(getAllParameterString(context), "未知通知", "");
                throw new CustomException("未知通知");
            }
        }

        public ExternalPlatformNotifyView Execute() {
            if(ValidateSign()) {
                var result = ExecuteCore();
                var id = GetParameter("orderid");
                result.Id = id;
                result.Response = "RECV_ORDID_" + id;
                saveNotifyLog(getAllParameterString(Context), result.Response, NotifyType);
                return result;
            } else {
                saveNotifyLog(getAllParameterString(Context), "签名无效", NotifyType);
                throw new CustomException("签名无效");
            }
        }

        protected abstract ExternalPlatformNotifyView ExecuteCore();

        protected abstract string NotifyType { get; }

        protected static string GetParameter(System.Web.HttpContext context, string name) {
            var parameters = GetParameterCollection(context);
            return parameters[name];
        }

        protected string GetParameter(string name) {
            var parameters = GetParameterCollection(Context);
            return parameters[name];
        }

        protected static System.Collections.Specialized.NameValueCollection GetParameterCollection(System.Web.HttpContext context) {
            if(context.Request.HttpMethod == "POST") {
                return context.Request.Form;
            } else if(context.Request.HttpMethod == "GET") {
                return context.Request.QueryString;
            } else {
                return context.Request.Params;
            }
        }
        private static string getAllParameterString(System.Web.HttpContext context) {
            var result = new StringBuilder();
            var parameters = GetParameterCollection(context);
            foreach(var key in parameters.AllKeys) {
                result.AppendFormat("{0}={1}&", key, parameters.Get(key));
            }
            if(result.Length > 0) result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        protected virtual bool ValidateSign() {
            var requestContents = new Dictionary<string, string>();
            var signKey = "sign";
            var parameters = GetParameterCollection(Context);
            foreach(var key in parameters.AllKeys) {
                if(key == signKey) continue;
                requestContents.Add(key, parameters[key]);
            }
            var sign = Sign(requestContents);
            var externalSign = parameters[signKey];
            return sign == externalSign;
        }
        private static void saveNotifyLog(string request, string response, string remark) {
            var log = new Log.Domain.ExternalPlatformAlternatingLog {
                Platform = Platform.Instance.PlatformInfo,
                Type = "通知",
                Request = request,
                Response = response,
                Remark = remark,
                Time = DateTime.Now
            };
            LogService.SaveExternalPlatformAlternatingLog(log);
        }
    }

    class PaySuccessNotify : NotifyProcessor {
        public PaySuccessNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            var payPlatform = GetParameter("payPlatform");
            PayInterface payInterface;
            if(payPlatform == "1") {
                payInterface = PayInterface.Alipay;
            } else if(payPlatform == "2") {
                payInterface = PayInterface.ChinaPnr;
            } else if(payPlatform == "6") {
                payInterface = PayInterface.Virtual;
            } else {
                return new PaySuccessNotifyView {
                    Valid = false,
                    Response = "未知支付方式"
                };
            }
            return new PaySuccessNotifyView {
                Payment = new Payment {
                    TradeNo = GetParameter("payid"),
                    PayInterface = payInterface,
                    IsAutoPay = GetParameter("payType") == "1",
                    PayTime = DateTime.Now
                },
                Valid = true
            };
        }

        protected override bool ValidateSign() {
            var parameters = GetParameterCollection(Context);
            var contents = parameters["orderid"] + parameters["payType"] + parameters["payid"] +
                           parameters["payplatform"] + parameters["totalPrice"] + parameters["type"];
            var sign = Sign(contents);
            var externalSign = parameters["sign"];
            return sign == externalSign;
        }

        protected override string NotifyType {
            get { return "支付成功"; }
        }
    }

    class ETDZSuccessNotify : NotifyProcessor {
        public ETDZSuccessNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            var result = new ETDZSuccessNotifyView() {
                Valid = true,
                Ticket = new TicketInfo()
            };
            var newPNRCode = GetParameter("newPnr");
            var settleCode = string.Empty;
            if(!string.IsNullOrWhiteSpace(newPNRCode)) {
                result.Ticket.NewPNR = new PNRPair(newPNRCode, string.Empty);
            }
            var passengers = GetParameter("passengerName").Split('^');
            var ticketNos = GetParameter("airId").Split('^');
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
                    var ticketNo = ticketFullNo.Substring(3, ticketFullNo.Length - 3);
                    ticketNoForPassenger.TicketNos = new[] { ticketNo.TrimStart('-') };
                }
                ticketNoViews.Add(ticketNoForPassenger);
                index++;
            }
            result.Ticket.SettleCode = settleCode;
            result.Ticket.TicketNos = ticketNoViews;
            return result;
        }

        protected override string NotifyType {
            get { return "出票成功"; }
        }
    }

    class CancelSuccessNotify : NotifyProcessor {
        public CancelSuccessNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            return new CancelOrderNotifyView {
                Valid = true
            };
        }

        protected override string NotifyType {
            get { return "取消订单"; }
        }
    }

    class ETDZFailedNotify : NotifyProcessor {
        public ETDZFailedNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            return new ETDZFailedNotifyView {
                Reason = GetParameter("refuseMemo"),
                Valid = true
            };
        }

        protected override string NotifyType {
            get { return "出票失败"; }
        }
    }
}