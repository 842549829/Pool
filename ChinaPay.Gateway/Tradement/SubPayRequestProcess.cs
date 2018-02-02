using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// 支付补差(子交易)
    /// </summary>
    public class SubPayRequestProcess : RequestProcessor
    {
        public SubPayRequestProcess(string payTradeNo, string outSubTradeNo, decimal amount, string payAccount, string note)
        {
            PayTradeNo = payTradeNo;
            OutSubTradeNo = outSubTradeNo;
            Amount = amount;
            PayAccount = payAccount;
            Note = note;
        }

        /// <summary>
        /// 支付交易流水号
        /// </summary>
        public string PayTradeNo { get; set; }

        /// <summary>
        /// 外部子交易号
        /// </summary>
        public string OutSubTradeNo { get; set; }

        /// <summary>
        /// 补差金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 付款账号
        /// </summary>
        public string PayAccount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        protected override string Target
        {
            get { return "SubPay.aspx"; }
        }

        protected override bool RequireResquest
        {
            get { return true; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.SubPay; }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"payTradeNo", PayTradeNo},
                    {"outSubTradeNo", OutSubTradeNo},
                    {"amount", Amount.ToString("F2")},
                    {"payAccount", (PayAccount)},
                    {"note", (Note)},
                };
        }

        protected override void ParseResponseCore(XmlDocument doc) { }

    }
}