using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.Service.Permission;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleInfoAddModify
{
    public partial class System_RoleMenusList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            var list = PermissionRoleService.QueryPermissionRoles(this.CurrentCompany.CompanyId); 
            showempty.Visible = !list.Any();
            grvRole.DataSource = list;
            grvRole.DataBind();
            if (list.Any())
            {
                grvRole.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void grvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "enable")
            {
                PermissionRoleView view = PermissionRoleService.QueryPermissionRole(Guid.Parse(e.CommandArgument.ToString()));
                //取得当前行 
                if (view.Valid)
                {
                    //禁用角色
                    PermissionRoleService.DisablePermissionRole(Guid.Parse(e.CommandArgument.ToString()), this.CurrentUser.UserName);
                }
                else
                {
                    //启用角色
                    PermissionRoleService.EnablePermissionRole(Guid.Parse(e.CommandArgument.ToString()), this.CurrentUser.UserName);
                }
                Bind();
            }
            if (e.CommandName == "del")
            {
                //删除角色
                PermissionRoleService.DeletePermissionRole(Guid.Parse(e.CommandArgument.ToString()), this.CurrentUser.UserName);
                Bind();
            }
        }
    }
}