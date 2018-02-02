using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    public class PayChannelQueryProcessor : RequestProcessor
    {
        /// <summary>
        /// 支付通道
        /// </summary>
        public IEnumerable<PayChannel> Channels { get; private set; }

        /// <summary>
        /// 网银
        /// </summary>
        public IEnumerable<Bank> Banks { get; private set; }

        protected override string Target
        {
            get { return "QueryChannel.aspx"; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.QueryPayChannel; }
        }

        protected override Dictionary<string, string> ConstructParametersCore() { return new Dictionary<string, string>(); }

        protected override void ParseResponseCore(XmlDocument doc)
        {
            XmlNode channelsNode = doc.SelectSingleNode("/poolpay/channels");
            if (channelsNode != null)
            {
                Channels = (from XmlNode node in channelsNode
                            let code = GetXmlNodeValue(node.SelectSingleNode("code"))
                            let name = GetXmlNodeValue(node.SelectSingleNode("name"))
                            select new PayChannel(code, name)).ToList();
            }
            XmlNode banksNode = doc.SelectSingleNode("/poolpay/banks");
            if (banksNode != null)
            {
                Banks = (from XmlNode node in banksNode
                         let channel = GetXmlNodeValue(node.SelectSingleNode("channel"))
                         let code = GetXmlNodeValue(node.SelectSingleNode("code"))
                         let name = GetXmlNodeValue(node.SelectSingleNode("name"))
                         let bankType = GetXmlNodeValue(node.SelectSingleNode("bankType"))
                         let bankChannnel = GetXmlNodeValue(node.SelectSingleNode("bankchannel"))
                         select new Bank(channel, code, name, bankType, bankChannnel)).ToList();
            }
        }

    }

    public class PayChannel
    {
        internal PayChannel(string code, string name)
        {
            Code = code;
            Name = name;
        }

        /// <summary>
        /// 通道代码
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 通道名称
        /// </summary>
        public string Name { get; private set; }
    }

    public class Bank
    {
        internal Bank(string channel, string code, string name, string bankType, string bankChannel)
        {
            Channel = channel;
            Code = code;
            Name = name;
            BankType = bankType;
            BankChannel = bankChannel;
        }

        /// <summary>
        /// 通道代码
        /// </summary>
        public string Channel { get; private set; }

        /// <summary>
        /// 银行代码
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 银行类型
        /// </summary>
        public string BankType { get; set; }

        /// <summary>
        /// 银行通道简码
        /// </summary>
        public string BankChannel { get; set; }
    }
}