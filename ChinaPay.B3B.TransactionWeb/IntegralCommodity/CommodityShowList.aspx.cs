using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Integral;
using ChinaPay.B3B.Service.Integral.Domain;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodity
{
    public partial class CommodityShowList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                IntegralCount count = IntegralServer.GetIntegralByAccountIdZong(this.CurrentUser.Owner);
                if (count!=null)
                {
                    lblKeYong.Text = count.IntegralAvailable + "";
                    lblZong.Text = count.IntegralCounts + "";
                }
                else
                {
                    lblKeYong.Text = "0";
                    lblZong.Text = "0";
                }
            }
        }
         
    }
}