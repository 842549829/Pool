using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Log.Domain;

namespace ChinaPay.Gateway.Tradement
{
    /// <summary>
    /// �˿�
    /// </summary>
    public class RefundRequestProcess : RequestProcessor
    {
        public RefundRequestProcess(string payTradeNo, string refundBatchNo, string refundReason, string payer = "", string subPayment = "", string royalties = "")
        {
            PayTradeNo = payTradeNo;
            RefundBatchNo = refundBatchNo;
            RefundReason = refundReason;
            _payer = payer;
            _subPayment = subPayment;
            _royalties = royalties;
        }

        /// <summary>
        /// ֧��������ˮ��
        /// </summary>
        public string PayTradeNo { get; set; }

        /// <summary>
        /// �˿����κ�
        /// </summary>
        public string RefundBatchNo { get; set; }

        /// <summary>
        /// �˿�ԭ��
        /// </summary>
        public string RefundReason { get; set; }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        public string _payer { get; set; }

        /// <summary>
        /// �ӽ�����Ϣ
        /// </summary>
        public string _subPayment { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string _royalties { get; set; }

        protected override string Target
        {
            get { return "Refund.aspx"; }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string outRoyalties { get; set; }

        /// <summary>
        /// �ӽ�����Ϣ
        /// </summary>
        public string outSubPayment { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string outPayer { get; set; }

        public override TradementBusinessType BusinessType
        {
            get { return TradementBusinessType.Refund; }
        }

        protected override Dictionary<string, string> ConstructParametersCore()
        {
            return new Dictionary<string, string>
                {
                    {"payTradeNo", PayTradeNo},
                    {"refundBatchNo", RefundBatchNo},
                    {"refundReason", (RefundReason)},
                    {"payer", (_payer)},
                    {"subPayment", (_subPayment)},
                    {"royalties", (_royalties)}
                };
        }

        protected override void ParseResponseCore(XmlDocument doc)
        {
            outPayer = GetXmlNodeValue(doc.SelectSingleNode("/poolpay/payer"));
            outSubPayment = GetXmlNodeValue(doc.SelectSingleNode("/poolpay/subPayment"));
            outRoyalties = GetXmlNodeValue(doc.SelectSingleNode("/poolpay/royalties"));
        }

        /// <summary>
        /// �����˿���Ϣ
        /// </summary>
        /// <param name="refund"></param>
        /// <returns></returns>
        public static RefundInfo ParseRefundInfo(string refund)
        {
            string[] infos = refund.Split('|');
            if (infos.Length != 4) return null;
            return new RefundInfo
                {
                    Account = infos[0],
                    RefundStatus = (RefundStatus) int.Parse(infos[1]),
                    RefundTime = DateTime.Parse(infos[2]),
                    RefundRemark = infos[3]
                };
        }

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
        //        Remark = string.Format("�˿�{0},������ˮ��:{1},�˿����κ�:{2}",Success?"�ɹ�":"ʧ��",PayTradeNo,RefundBatchNo)
        //    };
        //    LogService.SaveTradementLog(log);
        //}

        #region Nested type: RefundInfo

        public class RefundInfo
        {
            /// <summary>
            /// �ʺ�
            /// </summary>
            public string Account { get; set; }

            /// <summary>
            /// �˿�״̬
            /// </summary>
            public RefundStatus RefundStatus { get; set; }

            /// <summary>
            /// �˿�ʱ��
            /// </summary>
            public DateTime? RefundTime { get; set; }

            /// <summary>
            /// �˿���Ϣ
            /// </summary>
            public string RefundRemark { get; set; }
        }

        #endregion
    }
}