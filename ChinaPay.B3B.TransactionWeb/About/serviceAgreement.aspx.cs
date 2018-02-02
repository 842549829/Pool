
using System;
namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class serviceAgreement:UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("core.css");
            if (!IsPostBack)
            {
                hfdPlatformName.Value = BasePage.PlatformName;
                hfdDomainName.Value = BasePage.DomainName;
            }
        }
    }
}