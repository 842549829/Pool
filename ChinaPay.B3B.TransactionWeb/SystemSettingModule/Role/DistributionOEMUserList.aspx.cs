using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class DistributionOEMUserList : BasePage
    {
        private const string DateFromat = "yyyy-MM-dd";

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                initData();
                string incomeGroupId = Request.QueryString["IncomeGroupId"];
                if (!string.IsNullOrWhiteSpace(incomeGroupId))
                {
                    this.ddlIncomeGroup.SelectedValue = incomeGroupId;
                    btnQuery_Click(sender,e);
                }
            }
          pager.CurrentPageChanged+=new CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        private void initData()
        {
            var incomeGroup = IncomeGroupService.QueryIncomeGroup(this.CurrentCompany.CompanyId, null);
            this.ddlIncomeGroup.DataSource = incomeGroup;
            this.ddlIncomeGroup.DataTextField = "Name";
            this.ddlIncomeGroup.DataValueField = "Id";
            this.ddlIncomeGroup.DataBind();
            this.ddlIncomeGroup.Items.Insert(0, new ListItem("所有",""));
            var companyParameter = CompanyService.GetCompanyParameter(CurrentCompany.CompanyId);
            btnCreateSubCompany.PostBackUrl =companyParameter == null|| !companyParameter.CanHaveSubordinate
                                                  ? "/OrganizationModule/CommonContent/AddAccount/ExtendOpenAccount.aspx?Type=Purchaser"
                                                  : "/OrganizationModule/RoleModule/ExtendCompanyManage/AddLower.aspx";
            LoadCondition("DistributionOEMUserList");
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryDistributionOEMUser(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                GetRowCount = true
            };
            queryDistributionOEMUser(pagination);
        }

        private void queryDistributionOEMUser(Pagination pagination)
        {
            try
            {
                var distributionOemUser = from item in DistributionOEMService.QueryDistributionOEMUserList(getCondition(), pagination)
                                          select new
                                          {
                                              item.RegisterTime,
                                              item.Login,
                                              item.AbbreviateName,
                                              item.IncomeGroupName,
                                              Type = item.Type.GetDescription(),
                                              AccountType = item.AccountType.GetDescription(),
                                              item.ContactName,
                                              item.Enabled,
                                              AccountTypeValue = (byte)item.AccountType,
                                              item.CompanyId,
                                              IncomeGroupId = item.IncomeGroupId.HasValue? item.IncomeGroupId.ToString():string.Empty
                                          };
                datalist.DataSource = distributionOemUser;
                datalist.DataBind();
                if (distributionOemUser.Count()>0)
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

        private DistributionOEMUserCondition getCondition()
        {
            var parameter = new DistributionOEMUserCondition();
            parameter.CompanyId = this.CurrentCompany.CompanyId;
            if (!string.IsNullOrWhiteSpace(this.txtBeginTime.Text))
                parameter.RegisterBeginTime = DateTime.Parse(this.txtBeginTime.Text);
            if (!string.IsNullOrWhiteSpace(this.txtEndTime.Text))
                parameter.RegisterEndTime = DateTime.Parse(this.txtEndTime.Text).AddDays(1).AddMilliseconds(-3);
            if (!string.IsNullOrWhiteSpace(this.txtUserNo.Text))
                parameter.UserNo = this.txtUserNo.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.ddlStatus.Text))
                parameter.Enable = this.ddlStatus.Text == "1";
            if(!string.IsNullOrWhiteSpace(this.txtAbbreviateName.Text))
               parameter.AbbreviateName = txtAbbreviateName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtContactName.Text))
                parameter.ContactName = this.txtContactName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.ddlIncomeGroup.SelectedValue))
                parameter.IncomeGroup = Guid.Parse(this.ddlIncomeGroup.SelectedValue);
            return parameter;
        }

        private bool valiate()
        {
           
            return true;
        }
    }
}