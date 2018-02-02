using System;
using System.Configuration;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement.Pay
{
    public class PaySMSRequestProcess : PayRequestProcess
    {
        public PaySMSRequestProcess(decimal orderId, decimal amount, string payAccount, string payeeAccount, string subject, string note, string extraParams, string channel,
            string bank)
            : base(amount, payAccount, payeeAccount, subject, note, extraParams, channel, bank) { OrderId = orderId; }

        public decimal OrderId { get; set; }

        /// <summary>
        /// 支付地址
        /// </summary>
        public string PayUrl
        {
            get { return Result; }
        }

        /// <summary>
        /// 通知地址
        /// </summary>
        public override string NotifyUrl
        {
            get { return ConfigurationManager.AppSettings["PoolPayNotifyUrl"] + "SMSPayNotify.ashx"; }
        }

        public override decimal OutTradeNo
        {
            get { return OrderId; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.Pay; }
        }

        protected override void SaveLog(string request, string response, System.DateTime requestTime)
        {
            LogService.SavePoolpayInvokeLog(new poolpayInvokeLog
            {
                BusinessId = OrderId.ToString(),
                Type = BusinessType,
                Request = request,
                Response = response,
                Time = requestTime,
                Remark = string.Empty
            });
        }
    }
}