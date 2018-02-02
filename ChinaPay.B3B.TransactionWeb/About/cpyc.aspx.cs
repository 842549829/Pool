using System;
using System.Configuration;
using System.Web.UI;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class cpyc : UnAuthBasePage
    {
        protected string ToolUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["FileWeb"] + ConfigurationManager.AppSettings["RemindToolUrl"];
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("core.css");
        }
    }
}