using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.Gateway.Tradement
{
    public class QueryPaymentProcess : RequestProcessor
    {
        public QueryPaymentProcess(string outTradeNo) { OutTradeNo = outTradeNo; }
        public string OutTradeNo { get; private set; }

        /// <summary>
        /// ֧���ɹ�
        /// </summary>
        public bool PaySuccess { get; set; }

        /// <summary>
        /// ҵ�񵥺�
        /// </summary>
        public decimal BusinessId { get; private set; }

        /// <summary>
        /// ����ͨ������ˮ��
        /// </summary>
        public string PayTradeNo { get; private set; }

        /// <summary>
        /// ֧���˺�
        /// </summary>
        public string PayAccount { get; private set; }

        /// <summary>
        /// ֧���˺�����
        /// </summary>
        public PayAccountType PayAccountType { get; private set; }

        /// <summary>
        /// ֧��ʱ��
        /// </summary>
        public DateTime PayTime { get; private set; }

        /// <summary>
        /// ֧��ͨ��
        /// </summary>
        public PayInterface PayChannel { get; private set; }

        /// <summary>
        /// ֧��ͨ��������ˮ��
        /// </summary>
        public string ChannelTradeNo { get; private set; }

        /// <summary>
        /// ������
        /// </summary>
        protected decimal Acmount { get; private set; }

        /// <summary>
        /// ��չ��Ϣ
        /// </summary>
        public string ExtraParams { get; private set; }

        protected override string Target
        {
            get { return "QueryPayment.aspx"; }
        }

        protected override bool RequireResquest
        {
            get { return true; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.QueryPayment; }
        }


        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"outTradeNo", OutTradeNo}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("/poolpay/payment");
            if (node != null)
            {
                BusinessId = GetXmlNodeValue(node.SelectSingleNode("outTradeNo")).ToDecimal();
                Acmount = GetXmlNodeValue(node.SelectSingleNode("amount")).ToDecimal();
                PayTradeNo = GetXmlNodeValue(node.SelectSingleNode("payTradeNo"));
                PayAccount = GetXmlNodeValue(node.SelectSingleNode("payAccount"));
                PayAccountType = PayNotifyProcessor.ParsePayAccountType(GetXmlNodeValue(node.SelectSingleNode("/payAccountType")));
                PayTime = DateTime.Parse(GetXmlNodeValue(node.SelectSingleNode("payTime")));
                PayChannel = PayNotifyProcessor.ParsePayInterface(GetXmlNodeValue(node.SelectSingleNode("/channel")));
                ChannelTradeNo = GetXmlNodeValue(node.SelectSingleNode("channelPayTradeNo"));
                ExtraParams = GetXmlNodeValue(node.SelectSingleNode("extraParams"));
                PaySuccess = true;
            }
            else
            {
                PaySuccess = false;
            }
        }
    }
}