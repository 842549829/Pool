using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.Data_ManageModule
{
    public partial class IpLimitationManager :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.hfdOwner.Value = this.CurrentCompany.CompanyId.ToString();
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryEmployee(pagination);
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
                queryEmployee(pagination);
            }
            else
            {
                pager.CurrentPageIndex = 1;
            }
        }

        private void queryEmployee(Pagination pagination)
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
                    p.Cellphone,
                    p.Enabled,
                    p.Id,
                    p.IpLimitation
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
            condition.IpLimitation = this.txtIp.Text.Trim();
            return condition;
        }
    }
}