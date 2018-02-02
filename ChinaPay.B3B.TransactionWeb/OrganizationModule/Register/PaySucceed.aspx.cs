using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Register
{
    public partial class PaySucceed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string account = Request.QueryString["AccountNo"];
                if (!string.IsNullOrEmpty(account)) this.lblAccounNo.InnerText = account;
                Session.Remove("Info");
            }
        }
    }
}