using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage
{
    public partial class StaffInfoMgr : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            pager.CurrentPageChanged += pager_CurrentPageChanged;
            if (!IsPostBack)
            {
                LoadCondition("StaffInfo");
                btnQuery_Click(this, e);
            }
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryOrders(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                    GetRowCount = true
                };
                queryOrders(pagination);
            }
            else {
                pager.CurrentPageIndex = 1;
            }
        }

        private void queryOrders(Pagination pagination)
        {
            try
            {
                var condition = getCondition();
                var staffInfos = EmployeeService.QueryEmployees(condition, pagination);
                datalist.DataSource = staffInfos.Select(p => new
                {
                    p.Name,
                    p.UserName,
                    Gender = p.Gender.GetDescription(),
                    UserRoles = p.RoleName,
                    p.Email,
                    p.Cellphone,
                    p.Enabled,
                    p.Remark,
                    p.Id,
                    p.IsAdministrator
                });
                datalist.DataBind();
                if (staffInfos.Any())
                {
                    pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }
        private EmployeeQueryParameter getCondition()
        {
            var condition = new EmployeeQueryParameter();
            condition.Name = this.txtName.Text.Trim();
            condition.Login = this.txtUserName.Text.Trim();
            condition.Owner = CurrentCompany.CompanyId;
            if (!string.IsNullOrWhiteSpace(this.ddlEnabled.SelectedValue))
            {
                condition.Enabled = this.ddlEnabled.SelectedValue == "1";
            }
            return condition;
        }

        protected void ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Disable")
            {
                try
                {
                    Guid employeeId = new Guid(e.CommandArgument.ToString());
                    if (EmployeeService.DisableEmployee(employeeId,this.CurrentUser.UserName)) ShowMessage("禁用成功！");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "禁用");
                }
            }
            if (e.CommandName == "Enable")
            {
                try
                {
                    Guid employeeId = new Guid(e.CommandArgument.ToString());
                    if(EmployeeService.EnableEmployee(employeeId, this.CurrentUser.UserName)) ShowMessage("启用成功！");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "启用");
                }
            }

            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = pager.CurrentPageIndex,
                GetRowCount = true
            };
            queryOrders(pagination);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeService.ResetPassword(Guid.Parse(hdStaffId.Value), Reason.Text.Trim(), this.CurrentUser.UserName);
                ShowMessage(this, "重置密码成功\n\r默认密码" +ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultPassword);
                Reason.Text = string.Empty;
            }
            catch (Exception)
            {
                ShowMessage(this, "重置密码失败");
            }
        }
    }
}