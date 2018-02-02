using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// 自动代扣签约查询
    /// </summary>
    public class QuerySignProtocolStatusProcess : RequestProcessor
    {
        public QuerySignProtocolStatusProcess(string account, string channel)
        {
            Account = account;
            Channel = channel;
        }

        /// <summary>
        /// 扣款帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 通道编号
        /// </summary>
        public string Channel { get; set; }

        protected override string Target
        {
            get { return "QuerySignProtocolStatus.aspx"; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.QuerySignStatus; }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"account", Account},
                    {"channel", Channel}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc) { }

        protected override void SaveLog(string request, string response, System.DateTime time)
        {
            LogService.SavePoolpayInvokeLog(new poolpayInvokeLog
            {
                BusinessId = string.Empty,
                Type = BusinessType,
                Request = request,
                Response = response,
                Time = DateTime.Now,
                Remark = string.Empty
            });
        }
    }
}