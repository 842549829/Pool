using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.SystemSetting;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class MemberManager : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                Bind();
            }
        }

        protected void dataSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "del")
            {
                try
                {
                    OnLineCustomerService.DeleteMember(Guid.Parse(e.CommandArgument.ToString()),PublishRoles.平台, this.CurrentUser.Name);
                    Bind();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"删除");
                }
            }
        }

        private void Bind()
        {
            string devidGroupId = Request.QueryString["devideGroupId"];
            if (!string.IsNullOrWhiteSpace(devidGroupId))
            {
                var list = from item in OnLineCustomerService.QueryMembers(Guid.Parse(devidGroupId))
                           select new
                           {
                               Id = item.Id,
                               Remark = item.Remark,
                               QQ = item.QQ.Join(",")
                           };
                this.dataSource.DataSource = list;
                this.dataSource.DataBind();
                if (list.Any())
                {
                    dataSource.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string devideGroupId = Request.QueryString["devideGroupId"];
            Response.Redirect("MermberAddOrUpdate.aspx?devideGroupId="+devideGroupId);
        }
    }
}
