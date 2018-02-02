using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class IncomeGroupList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitData();
                queryInit(1);
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            queryInit(newPage);
        }

        private void queryInit(int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryIncomeGroup(pagination);
        }

        private void queryIncomeGroup(Pagination pagination)
        {
            try
            {
                var global = CompanyService.QueryLimitationType(this.CurrentCompany.CompanyId);
                var type = CompanyService.QueryGlobalPurchaseIncome(this.CurrentCompany.CompanyId);

                var setting = IncomeGroupService.QueryIncomeGroupDeductGlobalByCompanyId(CurrentCompany.CompanyId);

                var incomeGroup = from item in IncomeGroupService.QueryIncomeGroup(this.CurrentCompany.CompanyId, pagination)
                                  select new
                                  {
                                      item.Id,
                                      item.Company,
                                      item.Name,
                                      item.Description,
                                      item.CreateTime,
                                      item.UserCount,
                                      purchaseRestriction = global == Common.Enums.PurchaseLimitationType.Each ? " <a href='PurchaseRestrictionSetting.aspx?IncomeGroupId=" + item.Id + "'>采买设置</a> | " : "",
                                      incomeGlobal = type == Common.Enums.IncomeGroupLimitType.Each ? " <a href='Shouyishezhi.aspx?id=" + item.Id + "'>收益设置</a> | " : "" 
                                  };
                this.rptIncomeGroup.DataSource = incomeGroup;
                this.rptIncomeGroup.DataBind();
                if (incomeGroup.Count() > 0)
                {
                    this.pager.Visible = true;
                    this.showOrHide.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.showOrHide.Visible = false;
                    this.rptIncomeGroup.Visible = false;
                    this.pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"加载");
            }
        }

        private void InitData()
        {
            var incomeGroup = IncomeGroupService.QueryIncomeGroup(this.CurrentCompany.CompanyId, null);
            this.ddlIncomeGroup.DataSource = incomeGroup;
            this.ddlIncomeGroup.DataTextField = "Name";
            this.ddlIncomeGroup.DataValueField = "Id";
            this.ddlIncomeGroup.DataBind();
            this.ddlIncomeGroup.Items.Insert(0, new ListItem("-请选择-", ""));
        }

    }
}