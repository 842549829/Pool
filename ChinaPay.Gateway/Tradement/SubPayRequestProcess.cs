using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// ֧������(�ӽ���)
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
        /// ֧��������ˮ��
        /// </summary>
        public string PayTradeNo { get; set; }

        /// <summary>
        /// �ⲿ�ӽ��׺�
        /// </summary>
        public string OutSubTradeNo { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// �����˺�
        /// </summary>
        public string PayAccount { get; set; }

        /// <summary>
        /// ��ע
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