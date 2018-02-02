using System;
using System.Linq;
using System.Web;
using ChinaPay.Gateway.Tradement;

namespace ChinaPay.B3B.Interface.Processor {
    /// <summary>
    /// 在网站上支付
    /// </summary>
    internal class PayUtility {
        private static readonly string SignKey = System.Configuration.ConfigurationManager.AppSettings["ChinaPayKey"];
        private static readonly string PayAddress = System.Configuration.ConfigurationManager.AppSettings["ChinaPayUrl"];

        public static string GetPayUrl(string businessId, string payType, string userName) {
            var signTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            //生成签名验证值
            var sign = Utility.MD5EncryptorService.MD5FilterZero(userName + businessId + payType + signTime + SignKey, "utf-8");

            var url = PayAddress + "?userName=" + userName + "&orderId=" + businessId + "&type=" + payType + "&time=" + signTime + "&key=" + sign;
            return "<payUrl>" + HttpUtility.UrlEncode(url) + "</payUrl>";
        }

        public static string GetPayInterface(string interfaceCode) {
            PayChannelQueryProcessor processor = new PayChannelQueryProcessor();
            if (processor.Execute())
            {
                interfaceCode = interfaceCode == "0" ? "255" : interfaceCode;
                var channels = processor.Channels.FirstOrDefault(item => item.Code == interfaceCode);
                if (channels == null) throw new InterfaceInvokeException("1", "支付通道");
                return string.Format("{0}|{1}|{2}", channels.Code, string.Empty, channels.Name);
            }
            return string.Empty;
        }
    }
}