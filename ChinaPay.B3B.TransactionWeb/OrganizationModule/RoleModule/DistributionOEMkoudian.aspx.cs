using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule
{
    public partial class DistributionOEMkoudian : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                chkAirlist.DataSource = from item in FoundationService.Airlines select new { Text = item.Name, Value = item.Code.Value};
                chkAirlist.DataTextField = "Text";
                chkAirlist.DataValueField = "Value";
                //txtDepartureAirports.InitData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}