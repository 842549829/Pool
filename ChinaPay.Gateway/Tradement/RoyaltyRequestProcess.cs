using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// 分账
    /// </summary>
    public class RoyaltyRequestProcess : RequestProcessor
    {
        public RoyaltyRequestProcess(string payTradeNo, string royalties)
        {
            PayTradeNo = payTradeNo;
            Royalties = royalties;
        }

        /// <summary>
        /// 支付交易流水号
        /// </summary>
        public string PayTradeNo { get; set; }

        /// <summary>
        /// 分账信息
        /// </summary>
        public string Royalties { get; set; }

        protected override string Target
        {
            get { return "Royalty.aspx"; }
        }

        protected override bool RequireResquest
        {
            get { return true; }
        }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.Royalty; }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"payTradeNo", PayTradeNo},
                    {"royalties", (Royalties)}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc) { }
    }
}