using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// 交易解冻结
    /// </summary>
    public class TradeUnFreezeRequestProcess : RequestProcessor
    {
        public TradeUnFreezeRequestProcess(string freezeNo, decimal amount)
        {
            FreezeNo = freezeNo;
            Amount = amount;
        }

        /// <summary>
        /// 冻结号
        /// </summary>
        public string FreezeNo { get; set; }

        /// <summary>
        /// 解冻金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 解冻结号
        /// </summary>
        public string unFreezeNo { get; set; }

        protected override string Target
        {
            get { return "TradeUnFreeze.aspx"; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.Unfreeze; }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"freezeNo", FreezeNo},
                    {"amount", Amount.ToString("F2")}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc) { unFreezeNo = GetXmlNodeValue(doc.SelectSingleNode("/poolpay/unFreezeNo")); }

        //protected override void SaveLog(string request, string response, DateTime time)
        //{
        //    var log = new TradementLog
        //    {
        //        OrderId = OrderId,
        //        Type = TradementBusinessType.Pay,
        //        Request = request,
        //        Response = response,
        //        Time = time,
        //        Remark = string.Format("解冻{0},冻结号:{1}", Success ? "成功" : "失败", unFreezeNo)
        //    };
        //    LogService.SaveTradementLog(log);
        //}
    }
}