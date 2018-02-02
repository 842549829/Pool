using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule
{
    public partial class ConfigurationList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                lblEnterpriseQQ.Text = CurrenContract.EnterpriseQQ;
                lblServicePhone.Text = CurrenContract.ServicePhone;
                InitData();
            }
        }

        protected void rptConfig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                try
                {
                    ConfigurationService.DeleteConfiguration(this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                    InitData();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"删除");
                }
            }
        }

        private void InitData()
        {
            var list = ConfigurationService.QueryConfigurations(this.CurrentCompany.CompanyId);
            this.rptConfig.DataSource = list;
            this.rptConfig.DataBind();
            if (list.Any())
            {
                rptConfig.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }
}