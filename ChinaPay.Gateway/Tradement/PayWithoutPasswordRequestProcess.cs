using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// 无密支付
    /// </summary>
    public class PayWithoutPasswordRequestProcess : RequestProcessor
    {
        public PayWithoutPasswordRequestProcess(string outTradeNo, decimal amount, string payAccount, string payeeAccount, string subject, string note = "")
        {
            OutTradeNo = outTradeNo;
            Amount = amount;
            PayAccount = payAccount;
            PayeeAccount = payeeAccount;
            Subject = subject;
            Note = note;
        }

        /// <summary>
        /// 外部交易号
        /// </summary>
        public string OutTradeNo { get; set; }

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

        protected override string Target
        {
            get { return "PayWithoutPassword.aspx"; }
        }

        public PayInfo PayResult { get; set; }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.PayWithPassword; }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"outTradeNo", OutTradeNo},
                    {"amount", Amount.ToString("F2")},
                    {"payAccount", PayAccount},
                    {"payeeAccount", PayeeAccount},
                    {"subject", Subject},
                    {"note", Note}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc)
        {
            PayResult = new PayInfo
                {
                    PayTradeNo = GetXmlNodeValue(doc.SelectSingleNode("PayTradeNo")),
                    PayAccount = GetXmlNodeValue(doc.SelectSingleNode("PayAccount")),
                    PayAccountType = GetXmlNodeValue(doc.SelectSingleNode("PayAccountType")),
                    PayTime = DateTime.Parse(GetXmlNodeValue(doc.SelectSingleNode("PayTime"))),
                };
        }

        protected override void SaveLog(string request, string response, DateTime time)
        {
            var log = new TradementLog
                {
                    OrderId = OutTradeNo.ToDecimal(),
                    Type = TradementBusinessType.Pay,
                    Request = request,
                    Response = response,
                    Time = time,
                    Remark = string.Format("自动代扣{0},流水号{1}", Success ? "成功" : "失败", PayResult.PayTradeNo)
                };
            LogService.SaveTradementLog(log);
        }

        #region Nested type: PayInfo

        public class PayInfo
        {
            /// <summary>
            /// 支付交易流水号
            /// </summary>
            public string PayTradeNo { get; set; }

            /// <summary>
            /// 付款账号
            /// </summary>
            public string PayAccount { get; set; }

            /// <summary>
            /// 付款账号类型
            /// </summary>
            public string PayAccountType { get; set; }

            /// <summary>
            /// 支付时间
            /// </summary>
            public DateTime PayTime { get; set; }
        }

        #endregion
    }
}