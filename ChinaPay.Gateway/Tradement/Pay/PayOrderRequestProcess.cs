using System;
using System.Configuration;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement.Pay
{
    public class PayOrderRequestProcess : PayRequestProcess
    {
        public PayOrderRequestProcess(decimal orderId, decimal amount, string payAccount, string payeeAccount, string subject, string note, string extraParams, string channel,
            string bank) : base(amount, payAccount, payeeAccount, subject, note, extraParams, channel, bank) { OrderId = orderId; }

        public decimal OrderId { get; set; }

        public override decimal OutTradeNo
        {
            get { return OrderId; }
        }

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
            get { return ConfigurationManager.AppSettings["PoolPayNotifyUrl"] + "OrderPayNotify.ashx"; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.Pay; }
        }

        protected override void SaveLog(string request, string response, DateTime requestTime)
        {
            var log = new TradementLog
                {
                    OrderId = OrderId,
                    Type = TradementBusinessType.Pay,
                    Request = request,
                    Response = response,
                    Time = requestTime,
                    Remark = Note,
                };
            LogService.SaveTradementLog(log);
        }
    }
}