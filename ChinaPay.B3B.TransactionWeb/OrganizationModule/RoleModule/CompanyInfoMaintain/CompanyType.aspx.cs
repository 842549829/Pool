using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain
{
    public partial class CompanyType : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (this.CurrentCompany.CompanyType)
                {
                    case ChinaPay.B3B.Common.Enums.CompanyType.Provider:
                        Response.Redirect("./AgentMaintain.aspx",false);
                        break;
                    case ChinaPay.B3B.Common.Enums.CompanyType.Purchaser:
                        Response.Redirect("./ProviderMaintain.aspx", false);
                        break;
                    case ChinaPay.B3B.Common.Enums.CompanyType.Supplier:
                        Response.Redirect("./ProductMaintain.aspx", false);
                        break;
                    case ChinaPay.B3B.Common.Enums.CompanyType.Platform:
                        Response.Redirect("./AgentMaintain.aspx", false);
                        break;
                }
            }
        }
    }
}