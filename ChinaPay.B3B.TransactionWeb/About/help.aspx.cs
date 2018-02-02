using System;
using ChinaPay.B3B.Service.SystemManagement;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class help : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("core.css");
            if (!IsPostBack)
            {
                BindSpecialPolicyHelpInfos();
            }


        }

        private void BindSpecialPolicyHelpInfos() {
            var source = SpecialProductService.Query();
            SpecialPolicyInfos.DataSource = source;
            SpecialPolicTitleList.DataSource = source;
            SpecialPolicyInfos.DataBind();
            SpecialPolicTitleList.DataBind();
        }
    }
}