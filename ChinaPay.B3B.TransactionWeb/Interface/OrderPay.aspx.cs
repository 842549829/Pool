using System;
using System.Web;

namespace ChinaPay.B3B.TransactionWeb.Interface {
    /// <summary>
    /// 无需登录直接支付
    /// </summary>
    public partial class OrderPay : System.Web.UI.Page {
        /// <summary>
        /// 加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                var userName = Request.QueryString["userName"];
                var orderId = Request.QueryString["orderId"];
                var time = Request.QueryString["time"];
                var sign = Request.QueryString["key"];
                var type = Request.QueryString["type"];
                string message;
                if(validateArgs(userName, orderId, type, time, sign, out message) && LogonUtility.Logon(userName, out message)) {
                    var urlParameter = HttpUtility.UrlEncode(string.Format("/OrderModule/Purchase/OrderPay.aspx?id={0}&type={1}", orderId, type));
                    Response.Redirect(string.Format("/Index.aspx?redirectUrl={0}", urlParameter));
                } else {
                    Response.Write(message);
                }
            }
        }
        private bool validateArgs(string userName, string orderId, string type, string time, string sign, out string message) {
            return VerificationEmpty(userName, orderId, sign, out message) &&
                   VerificationSign(userName, orderId, type, time, sign, out message);
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        private bool VerificationSign(string userName, string orderId, string type, string time, string sign, out string message) {
            message = string.Empty;
            string key = System.Configuration.ConfigurationManager.AppSettings["ChinaPayKey"];
            if(string.IsNullOrWhiteSpace(key)) {
                message = "签名失败";
                return false;
            }
            string md5Sign = Utility.MD5EncryptorService.MD5FilterZero(userName + orderId + type + time + key, "utf-8");
            if(md5Sign.ToUpper() != sign.ToUpper()) {
                message = "签名错误";
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证参数是否为空
        /// </summary>
        private bool VerificationEmpty(string userName, string orderId, string sign, out string message) {
            message = string.Empty;
            if(string.IsNullOrWhiteSpace(userName)) {
                message = "用户名为空";
                return false;
            }
            if(string.IsNullOrWhiteSpace(orderId)) {
                message = "订单号为空";
                return false;
            }
            if(string.IsNullOrWhiteSpace(sign)) {
                message = "签名为空";
                return false;
            }
            return true;
        }
    }
}