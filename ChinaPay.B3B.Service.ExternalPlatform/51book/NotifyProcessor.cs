using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.ExternalPlatform._51book {
    abstract class NotifyProcessor : Processor.INotifyProcessor {
        protected System.Web.HttpContext Context;

        protected NotifyProcessor(System.Web.HttpContext context) {
            Context = context;
        }

        public static NotifyProcessor CreateProcessor(System.Web.HttpContext context) {
            var notifyType = GetParameter(context, "type");
            if(notifyType == "0") {
                return new DenyETDZNotify(context);
            } else if(notifyType == "1") {
                return new ETDZSuccessNotify(context);
            } else if(notifyType == "2") {
                return new ETDZFailedNotify(context);
            } else {
                saveNotifyLog(getAllParameterString(context), "未知通知", "");
                throw new CustomException("未知通知");
            }
        }

        public ExternalPlatformNotifyView Execute() {
            var result = ExecuteCore();
            result.Id = GetParameter(BusinessKey);
            result.Response = "S";
            result.Valid = true;
            saveNotifyLog(getAllParameterString(Context), result.Response, NotifyType);
            return result;
        }

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

        protected abstract ExternalPlatformNotifyView ExecuteCore();

        protected virtual string BusinessKey { get { return "sequenceNo"; } }

        protected abstract string NotifyType { get; }

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
            return new PaySuccessNotifyView {
                Payment = new Payment {
                    PayInterface = PayInterface.Alipay,
                    IsAutoPay = false,
                    PayTime = DateTime.Now
                }
            };
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
            var etdzPNRCode = GetParameter("pnrNo");
            var settleCode = string.Empty;
            if(!string.IsNullOrWhiteSpace(etdzPNRCode)) {
                result.Ticket.NewPNR = new PNRPair(etdzPNRCode, string.Empty);
            }
            var passengers = GetParameter("passengerNames").Split(',');
            var ticketNos = GetParameter("ticketNos").Split(',');
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
            result.Ticket.TicketNos = ticketNoViews;
            return result;
        }

        protected override string NotifyType {
            get { return "出票成功"; }
        }
    }

    class DenyETDZNotify : NotifyProcessor {
        public DenyETDZNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            return new DenyETDZNotifyView {
                Reason = GetParameter("reason")
            };
        }

        protected override string NotifyType {
            get { return "拒绝出票"; }
        }
    }

    class ETDZFailedNotify : NotifyProcessor {
        public ETDZFailedNotify(System.Web.HttpContext context)
            : base(context) {
        }

        protected override ExternalPlatformNotifyView ExecuteCore() {
            return new ETDZFailedNotifyView {
                Reason = "暂时不能出票"
            };
        }

        protected override string BusinessKey {
            get { return "orderNo"; }
        }

        protected override string NotifyType {
            get { return "出票失败"; }
        }
    }
}