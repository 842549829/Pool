using System;
using System.Collections.Generic;
using System.Xml;

namespace ChinaPay.Gateway.Tradement
{
    public abstract class PayRequestProcess : RequestProcessor
    {
        internal const string TicketOrderPayType = "Order";
        internal const string PostponeApplyformPayType = "Postpone";
        internal const string SMSOrderPayType = "SMS";

        protected PayRequestProcess(decimal amount, string payAccount, string payeeAccount, string subject, string note, string extraParams, string channel, string bank)
        {
            Amount = amount;
            PayAccount = payAccount;
            PayeeAccount = payeeAccount;
            Subject = subject;
            Note = note;
            ExtraParams = extraParams;
            Channel = channel;
            Bank = bank;
        }

        /// <summary>
        /// 外部交易号
        /// </summary>
        public virtual decimal OutTradeNo { get; private set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 付款账号
        /// </summary>
        public string PayAccount { get; set; }

        /// <summary>
        /// 收款账号
        /// </summary>
        public string PayeeAccount { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string ExtraParams { get; set; }

        /// <summary>
        /// 通道编号
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 银行代码
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public virtual string NotifyUrl
        {
            get { return string.Empty; }
        }

        protected override string Target
        {
            get { return "Pay.aspx"; }
        }

        protected override bool RequireResquest
        {
            get { return false; }
        }

        protected string PaymentType
        {
            get
            {
                string[] extraParamArray = ExtraParams.Split('|');
                if (extraParamArray.Length >= 3)
                {
                    return extraParamArray[0];
                }
                return string.Empty;
            }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"outTradeNo", OutTradeNo.ToString()},
                    {"amount", Amount.ToString("F2")},
                    {"payAccount", PayAccount},
                    {"payeeAccount", PayeeAccount},
                    {"subject", Subject},
                    {"note", Subject},
                    {"extraParams", ExtraParams},
                    {"channel", Channel},
                    {"bank", Bank},
                    {"notifyUrl", NotifyUrl}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc) { }

        protected override void SaveLog(string request, string response, DateTime requestTime) { }
    }
}