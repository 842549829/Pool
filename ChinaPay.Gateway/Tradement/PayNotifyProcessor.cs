using System;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    public class PayNotifyProcessor : NotifyProcessor
    {
        public PayNotifyProcessor(HttpRequest request)
            : base(request)
        {
        }

        /// <summary>
        /// 业务单号
        /// </summary>
        public decimal BusinessId
        {
            get;
            private set;
        }

        /// <summary>
        /// 国付通交易流水号
        /// </summary>
        public string PoolPayTradeNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 支付账号
        /// </summary>
        public string PayAccount
        {
            get;
            private set;
        }

        /// <summary>
        /// 支付账号类型
        /// </summary>
        public PayAccountType PayAccountType
        {
            get;
            private set;
        }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 支付通道
        /// </summary>
        public PayInterface PayChannel
        {
            get;
            private set;
        }

        /// <summary>
        /// 支付通道交易流水号
        /// </summary>
        public string ChannelTradeNo
        {
            get;
            private set;
        }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string ExtraParams
        {
            get;
            private set;
        }

        public override TradementBusinessType BusinessType
        {
            get
            {
                return TradementBusinessType.Notify;
            }
        }

        protected override void ParseCore()
        {
            BusinessId = decimal.Parse(Parameters["outTradeNo"]);
            PoolPayTradeNo = Parameters["payTradeNo"];
            PayAccount = Parameters["payAccount"];
            PayAccountType = ParsePayAccountType(Parameters["payAccountType"]);
            PayTime = DateTime.Parse(Parameters["payTime"]);
            PayChannel = ParsePayInterface(Parameters["channel"]);
            ChannelTradeNo = Parameters["channelPayTradeNo"];
            ExtraParams = Parameters["extraParams"];
        }

        protected override void SaveLog(string request, string response)
        {
            var tradementLog = new TradementLog
                {
                    OrderId = BusinessId,
                    Request = request,
                    Response = response,
                    Time = DateTime.Now,
                    Remark = "支付通知",
                };
            LogService.SaveTradementLog(tradementLog);

        }

        public static PayInterface ParsePayInterface(string code)
        {
            if (code == "Alipay")
                return PayInterface.Alipay;
            if (code == "ChinaPnr")
                return PayInterface.ChinaPnr;
            if (code == "Tenpay")
                return PayInterface.Tenpay;
            if (code == "99Bill")
                return PayInterface._99Bill;
            return PayInterface.Virtual;
        }

        public static PayAccountType ParsePayAccountType(string code)
        {
            if (code == "1")
                return PayAccountType.Credit;
            return PayAccountType.Cash;
        }
    }
}