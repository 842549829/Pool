using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// ����֧��
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
        /// �ⲿ���׺�
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// �����˺�
        /// </summary>
        public string PayAccount { get; set; }

        /// <summary>
        /// �տ��˺�
        /// </summary>
        public string PayeeAccount { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// ��ע
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
                    Remark = string.Format("�Զ�����{0},��ˮ��{1}", Success ? "�ɹ�" : "ʧ��", PayResult.PayTradeNo)
                };
            LogService.SaveTradementLog(log);
        }

        #region Nested type: PayInfo

        public class PayInfo
        {
            /// <summary>
            /// ֧��������ˮ��
            /// </summary>
            public string PayTradeNo { get; set; }

            /// <summary>
            /// �����˺�
            /// </summary>
            public string PayAccount { get; set; }

            /// <summary>
            /// �����˺�����
            /// </summary>
            public string PayAccountType { get; set; }

            /// <summary>
            /// ֧��ʱ��
            /// </summary>
            public DateTime PayTime { get; set; }
        }

        #endregion
    }
}