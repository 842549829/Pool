using System;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class LicenseQuery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string type = Request.QueryString["type"];
                string companyId = Request.QueryString["companyId"];
                this.imgLencense.Src = "/OrganizationHandlers/OutPutImage.ashx/OutPutStram?companyId="+companyId+"&type="+type;
            }
        }
    }
}