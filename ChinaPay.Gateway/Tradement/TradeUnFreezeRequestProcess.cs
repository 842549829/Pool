using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// ���׽ⶳ��
    /// </summary>
    public class TradeUnFreezeRequestProcess : RequestProcessor
    {
        public TradeUnFreezeRequestProcess(string freezeNo, decimal amount)
        {
            FreezeNo = freezeNo;
            Amount = amount;
        }

        /// <summary>
        /// �����
        /// </summary>
        public string FreezeNo { get; set; }

        /// <summary>
        /// �ⶳ���
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// �ⶳ���
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
        //        Remark = string.Format("�ⶳ{0},�����:{1}", Success ? "�ɹ�" : "ʧ��", unFreezeNo)
        //    };
        //    LogService.SaveTradementLog(log);
        //}
    }
}