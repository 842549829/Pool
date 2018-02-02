using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaPay.B3B.Interface.Processor
{
    class OrderPay : RequestProcessor
    {
        private string _key = System.Configuration.ConfigurationManager.AppSettings["ChinaPayKey"];
        private string _url = System.Configuration.ConfigurationManager.AppSettings["ChinaPayUrl"];
        protected override string ExecuteCore()
        {
            string id = Context.GetParameterValue("id");
            string time = DateTime.Now.ToString("yyyyMMddhhmmss");
            string userName = Employee.UserName;
            string type = "1";
            //生成签名验证值
            string key = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(userName + id + type + time + _key, "utf-8");

            string url = _url + "?userName=" + userName + "&orderId=" + id + "&type=" + type + "&time=" + time + "&key=" + key;
            return "<payUrl>" + System.Web.HttpUtility.UrlEncode(url) + "</payUrl>";
        }
    }
}