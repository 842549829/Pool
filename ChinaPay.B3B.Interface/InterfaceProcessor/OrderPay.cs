using System;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    internal class OrderPay : BaseProcessor
    {
        private string _Id;
        private string _key = System.Configuration.ConfigurationManager.AppSettings["ChinaPayKey"];
        private string _url = System.Configuration.ConfigurationManager.AppSettings["ChinaPayUrl"];
        public OrderPay(string Id, string userName, string sign)
            : base(userName, sign)
        {
            _Id = Id;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrWhiteSpace(_Id)) throw new InterfaceInvokeException("1", "订单号");
        }

        protected override string ExecuteCore()
        {
            string time = DateTime.Now.ToString("yyyyMMddhhmmss");
            string userName = Employee.UserName;
            string type = "1";
            //生成签名验证值
            string key = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(userName + _Id + type + time + _key, "utf-8");

            string url = _url + "?userName=" + userName + "&orderId=" + _Id + "&type=" + type + "&time=" + time + "&key=" + key;
            return "<payUrl>" + System.Web.HttpUtility.UrlEncode(url) + "</payUrl>";
        }

        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("Id", _Id);
            return collection;
        }
    }
}