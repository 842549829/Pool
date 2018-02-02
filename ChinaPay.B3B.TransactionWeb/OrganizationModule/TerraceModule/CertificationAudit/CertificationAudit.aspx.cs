using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CertificationAudit
{
    public partial class CertificationAudit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                txtBeginTime.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
                btnSearch_Click(this, e);
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }
        private void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            Pagination pagination = new Pagination() {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryCompanys(pagination);
        }
        private void queryCompanys(Pagination pagination)
        {
            try
            {
                var companys = AccountCombineService.GetNeedAuditCompanies(getCondition(),pagination);
                this.datalist.DataSource = companys.Select(item => new {
                    Account = item.UserNo,
                    CompanyName = item.AbbreviateName,
                    AccountType = item.CompanyType.GetDescription() +"("+ item.AccountType.GetDescription()+")",
                    CompanyTypeValue = (byte)item.CompanyType,
                    AccountTypeValue = (byte)item.AccountType,
                    Time =item.ApplyTime,
                    Id = item.CompanyId,
                    AuditType = item.AuditType,
                    AuditTypeValue = item.AuditType =="普通审核"?0:1,
                    SourceType = item.SpreadId == Guid.Empty ? item.SourceType : "<a href='../CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId="+item.SpreadId.ToString()+"'>" + item.SourceType + "</a>",
                });
                datalist.DataBind();
                if (companys.Any())
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
        private CompanyAuditQueryCondition getCondition()
        {
            CompanyAuditQueryCondition parameter = new CompanyAuditQueryCondition();
            if (!string.IsNullOrWhiteSpace(this.txtAccount.Text))
                parameter.UserNo = this.txtAccount.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.ddlRoleType.SelectedValue))
                parameter.CompanyType = (CompanyType)byte.Parse(this.ddlRoleType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(this.ddlAccountType.SelectedValue))
                parameter.AccountType = (AccountBaseType)byte.Parse(this.ddlAccountType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(this.txtCompayName.Text))
                parameter.CompanyName = this.txtCompayName.Text.Trim();
            parameter.ApplyTimeStart = DateTime.Parse(this.txtBeginTime.Text);
            parameter.ApplyTimeEnd = DateTime.Parse(this.txtEndTime.Text).AddDays(1).AddMilliseconds(-3);
            if (!string.IsNullOrWhiteSpace(this.ddlAuditType.SelectedValue))
            {
                parameter.AuditType = this.ddlAuditType.SelectedValue.Trim();
            }
            if (!string.IsNullOrWhiteSpace(this.ddlRegisterType.SelectedValue))
            {
                parameter.SourceType = this.ddlRegisterType.SelectedValue.Trim();
            }
            return parameter;
        }
        private void initData() {
            IEnumerable<KeyValuePair<int, string>> companyTypes = GetCompanyType();
            ddlRoleType.DataTextField = "Value";
            ddlRoleType.DataValueField = "Key";
            ddlRoleType.DataSource = companyTypes;
            ddlRoleType.DataBind();
            ddlRoleType.Items.Insert(0, new ListItem("全部", ""));
        }
        private IEnumerable<KeyValuePair<int, string>> GetCompanyType() { 
            var companyType = Enum.GetValues(typeof(CompanyType)) as CompanyType[];
            if (companyType == null) return null;
            return companyType.Where(item =>item != CompanyType.Platform).Select(item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                queryCompanys(pagination);
            }
            else
            {
                pager.CurrentPageIndex = 1;
            }
        }
    }
}