using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// ���׶���
    /// </summary>
    public class TradeFreezeRequestProcess : RequestProcessor
    {
        public TradeFreezeRequestProcess(decimal orderId, decimal applyformId, string payTradeNo, string account, decimal amount)
        {
            OrderId = orderId;
            ApplyformId = applyformId;
            PayTradeNo = payTradeNo;
            Account = account;
            Amount = amount;
        }

        public decimal OrderId { get; set; }
        public decimal ApplyformId { get; set; }

        /// <summary>
        /// ֧��������ˮ��
        /// </summary>
        public string PayTradeNo { get; set; }

        /// <summary>
        /// �����˺�
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public string FreezeNo { get; set; }

        protected override string Target
        {
            get { return "TradeFreeze.aspx"; }
        }

        protected override bool RequireResquest
        {
            get { return true; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.Freeze; }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"payTradeNo", PayTradeNo},
                    {"account", Account},
                    {"amount", Amount.ToString("F2")}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc) { FreezeNo = GetXmlNodeValue(doc.SelectSingleNode("/poolpay/freezeNo")); }

        //protected override void SaveLog(string request, string response, DateTime time)
        //{
        //    var log = new TradementLog
        //    {
        //        OrderId = OrderId,
        //        ApplyformId = ApplyformId,
        //        Type = TradementBusinessType.Pay,
        //        Request = request,
        //        Response = response,
        //        Time = time,
        //        Remark = string.Format("����{0},�����:{1}", Success ? "�ɹ�" : "ʧ��", FreezeNo)
        //    };
        //    LogService.SaveTradementLog(log);
        //}
    }
}