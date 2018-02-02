using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule
{
    public partial class DistributionOEMSiteSet : BasePage
    {
        private static string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var oem = OEMService.QueryOEM(CurrentCompany.CompanyId);
                if (oem != null)
                {
                    lblName.Text = oem.SiteName;
                    lblDomain.Text = oem.DomainName;
                    lblEmail.Text = oem.ManageEmail;
                    txtICP.Text = oem.ICPRecord;
                    //lblLogo.Text = oem.LogoPath;
                    txtEmbedCode.Text = oem.EmbedCode;
                    lblEnable.Text = oem.Enabled ? "启用" : "不启用";
                    lblReg.Text = oem.AllowSelfRegex ? "允许" : "不允许";
                    imgUrl.Value = FileWeb + "/" + oem.LogoPath;
                }
            }
        }
    }
}