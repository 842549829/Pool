using System;
using System.Text;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    /// <summary>
    /// 
    /// </summary>
    class PayApplyform : BaseProcessor
    {
        public string _orderId { get; set; }
        private string _key = System.Configuration.ConfigurationManager.AppSettings["ChinaPayKey"];
        private string _url = System.Configuration.ConfigurationManager.AppSettings["ChinaPayUrl"];
        public PayApplyform(string orderId, string userName, string sign)
            : base(userName, sign)
        {
            _orderId = orderId;
        }

        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("orderId", _orderId);
            return collection;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrEmpty(_orderId)) throw new InterfaceInvokeException("1", "订单号");
        }

        protected override string ExecuteCore()
        {
            string time = DateTime.Now.ToString("yyyyMMddhhmmss");
            string userName = Employee.UserName;
            string type = "2";
            //生成签名验证值
            string key = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(userName + _orderId + type + time + _key, "utf-8");
            //待确定
            string url = _url + "?userName=" + userName + "&orderId=" + _orderId + "&type=" + type + "&time=" + time + "&key=" + key;
            return "<payUrl>" + System.Web.HttpUtility.UrlEncode(url) + "</payUrl>";
        }
    }
}