using System;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// ReturnUrl 的摘要说明
    /// </summary>
    public class ReturnUrl : BaseHandler
    {
        private string m_url = @"/Login.aspx?account={0}&type={1}&time={2}&sign={3}&domainName=http://{4}:{5}";
        public string GetUrl(string accountNo, string type)
        {
            try
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["poolpayUrl"] + m_url;
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                Signature signature = new Signature(time);
                return string.Format(url, accountNo, type, time, signature.GetSign(accountNo, type), BasePage.DomainName, System.Web.HttpContext.Current.Request.Url.Port);
            }
            catch (Exception)
            { 
                return string.Empty; 
            }
        }
    }
}