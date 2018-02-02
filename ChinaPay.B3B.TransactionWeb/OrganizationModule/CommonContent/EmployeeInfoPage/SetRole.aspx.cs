using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.Service.Permission;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage
{
    public partial class SetRole : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                this.setBackButton();
                this.BindUserRole();
                this.BindAccount();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PermissionRoleService.UpdateRolesOfUser(Guid.Parse(Request.QueryString["EmployeeId"]), this.GetPermissionRoles(), this.CurrentUser.UserName);
                RegisterScript("alert('保存成功');window.location.href='StaffInfoMgr.aspx';");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "设置角色错误");
            }
        }
        private IEnumerable<Guid> GetPermissionRoles()
        {
            IList<Guid> lists = new List<Guid>();
            for (int i = 0; i < this.chklUserRole.Items.Count; i++)
                if (this.chklUserRole.Items[i].Selected)
                    lists.Add(Guid.Parse(this.chklUserRole.Items[i].Value));
            return lists;
        }
        private void BindAccount() {
            this.lblAccountNo.Text = Request.QueryString["UserName"];
        }
        private void BindUserRole()
        {
            var userRoles = PermissionRoleService.QueryPerssionRolesOfUser(Guid.Parse(Request.QueryString["EmployeeId"])).ToDictionary(item => item.Key);
            foreach (PermissionRoleView item in PermissionRoleService.QueryPermissionRoles(this.CurrentCompany.CompanyId).Where(linq => linq.Valid))
            {
                var controlItem = new ListItem(item.Name, item.Id.ToString());
                if (userRoles.ContainsKey(item.Id))
                {
                    controlItem.Selected = true;
                }
                this.chklUserRole.Items.Add(controlItem);
            }
        }
        private void setBackButton()
        {
            this.btnGoBack.Attributes.Add("onclick", "window.location.href='" + (Request.UrlReferrer ?? Request.Url).PathAndQuery + "';return false;");
        }
    }
}