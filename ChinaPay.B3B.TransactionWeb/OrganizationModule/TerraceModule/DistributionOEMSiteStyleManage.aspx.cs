using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOEMSiteStyleManage :UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");

        }
    }
}