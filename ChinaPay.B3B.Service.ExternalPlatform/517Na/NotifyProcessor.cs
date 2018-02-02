using System;
using System.Collections.Generic;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.ExternalPlatform._517Na {
    abstract class NotifyProcessor : _517Na.ProcessorBase, Processor.INotifyProcessor {
        protected System.Web.HttpContext Context;

        protected NotifyProcessor(System.Web.HttpContext context) {
            Context = context;
        }

        public static NotifyProcessor CreateProcessor(System.Web.HttpContext context) {
            var notifyType = GetParameter(context, "DrawABillFlag");
            if(notifyType == "0") {
                return new ETDZSuccessNotify(context);
            } else if(notifyType == "1") {
                return new ETDZFailedNotify(context);
            } else {
                saveNotifyLog(getAllParameterString(context), "未知通知", "");
                throw new CustomException("未知通知");
            }
        }

        public ExternalPlatformNotifyView Execute() {
            if(ValidateSign()) {   
                var result = ExecuteCore();
                result.Id = GetParameter("OrderId");
                result.Response = "SUCCESS";
                result.Valid = true;
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
            var signKey = "Sign";
            var parameters = GetParameterCollection(Context);
            foreach(var key in parameters.AllKeys) {
                if(key == signKey) continue;
                requestContents.Add(key, parameters[key]);
            }
            var sign = Sign(requestContents);
            var externalSign = parameters[signKey];
            return sign == externalSign;
        }

        private string Sign(Dictionary<string, string> parameters) {
            var signContent = parameters.Join("", p => p.Key + "=" + p.Value) + DateTime.Today.ToString("yyyy-MM-dd") + Platform.NotifyCode;
            return Utility.MD5EncryptorService.MD5(signContent);
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

    class ETDZSuccessNotify : NotifyProcessor {
        public ETDZSuccessNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            var result = new ETDZSuccessNotifyView {
                Ticket = new TicketInfo()
            };
            var newPNRCode = GetParameter("NewPnr");
            if(!string.IsNullOrWhiteSpace(newPNRCode)) {
                result.Ticket.NewPNR = new PNRPair(newPNRCode, string.Empty);
            }
            var settleCode = string.Empty;
            var ticketNoViews = new List<TicketNoView.Item>();
            foreach(var passenger in GetParameter("TicketNos").Split(',')) {
                if(!string.IsNullOrWhiteSpace(passenger)) {
                    var dataArray = passenger.Split('|');
                    if(dataArray.Length == 5) {
                        var name = dataArray[4];
                        if(string.IsNullOrWhiteSpace(settleCode)) {
                            settleCode = dataArray[0];
                        }
                        var ticketNo = dataArray[1];
                        ticketNoViews.Add(new TicketNoView.Item {
                            Name = name,
                            TicketNos = new[] { ticketNo }
                        });
                    }
                }
            }
            result.Ticket.SettleCode = settleCode;
            result.Ticket.TicketNos = ticketNoViews;
            return result;
        }

        protected override string NotifyType {
            get { return "出票成功"; }
        }
    }
    class ETDZFailedNotify : NotifyProcessor {
        public ETDZFailedNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            return new ETDZFailedNotifyView {
                Reason = GetParameter("DrawABillRemark")
            };
        }

        protected override string NotifyType {
            get { return "出票失败"; }
        }
    }
}