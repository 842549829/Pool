using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class AbolishList : BasePage
    {
        protected bool IsFirstLoad { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                txtAppliedDateStart.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                txtAppliedDateEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
            }
        }

    }
}