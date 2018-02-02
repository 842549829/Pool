using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage
{
    public partial class ExtendCompanyList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.lblAddress.Text = System.Configuration.ConfigurationManager.AppSettings["SpreadAddress"] + "?key=" + this.CurrentUser.UserName;
                initData();
                btnQuery_Click(this, e);
                LoadCondition("ExtendCompany");
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void pager_CurrentPageChanged(Pager sender, int newpage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newpage,
                GetRowCount = true
            };
            queryCompays(pagination);
        }

        private void queryCompays(Pagination pagination)
        {
            try
            {
                var companys = CompanyService.GetSpreadingList(getCondition(),pagination);
                datalist.DataSource = from company in companys
                                      select new
                                       {
                                           company.AbbreviateName,
                                           company.Admin,
                                           company.Contact,
                                           company.ContactCellphone,
                                           company.Enabled,
                                           CompanyType = company.Type,
                                           Type = company.Type.GetDescription(),
                                           ID = company.Id,
                                           company.OperatorAccount,
                                           DisplayAudit = company.Type!=CompanyType.Purchaser,
                                           AccountType = company.AccountType.GetDescription()
                                       };

                datalist.DataBind();
                if (companys.Any())
                {
                    pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private SpreadingQueryParameter getCondition()
        {
            var parameter = new SpreadingQueryParameter();
            parameter.AbbreviateName = txtAbbreviateName.Text.Trim();
            parameter.UserNo = txtAccount.Text.Trim();
            parameter.Contact = txtContact.Text.Trim();
            if (!string.IsNullOrWhiteSpace(ddlCompanyType.SelectedValue))
                parameter.Type = (CompanyType)int.Parse(ddlCompanyType.SelectedValue);
            if (!string.IsNullOrEmpty(ddlStatus.SelectedValue.Trim()))
                parameter.Enabled = ddlStatus.SelectedValue == "1";
            if (!string.IsNullOrEmpty(ddlAccountType.SelectedValue))
                parameter.AccountType = (AccountBaseType)byte.Parse(ddlAccountType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(ddlEmployeNo.SelectedValue))
                parameter.OperatorAccount = ddlEmployeNo.SelectedValue;
            parameter.Initiator = CurrentCompany.CompanyId;
            return parameter;
        }


        private void initData()
        {
            #region LoadCompanyType
            var companyTypes = GetCompanyTypes();
            ddlCompanyType.DataTextField = "Value";
            ddlCompanyType.DataValueField = "Key";
            ddlCompanyType.DataSource = companyTypes;
            ddlCompanyType.DataBind();
            #endregion

            //#region LoadCompanyStatus

            //var companyStatus = GetCompanyStatus();
            //ddlStatus.DataTextField = "Value";
            //ddlStatus.DataValueField = "Key";
            //ddlStatus.DataSource = companyStatus;
            //ddlStatus.DataBind();
            //#endregion

            this.ddlEmployeNo.DataSource = from item in EmployeeService.QueryEmployees(this.CurrentCompany.CompanyId)
                                           select new
                                           {
                                                Text = item.UserName+"-"+item.Name,
                                                Value =item.UserName
                                           };
            this.ddlEmployeNo.DataTextField = "Text";
            this.ddlEmployeNo.DataValueField = "Value";
            this.ddlEmployeNo.DataBind();
            this.ddlEmployeNo.Items.Insert(0, new ListItem("全部", ""));
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
                var pagination = new Pagination()
                {
                    PageSize = pager.PageSize,
                    PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex=1,
                    GetRowCount = true
                };
                queryCompays(pagination);
        }





        /// <summary>
        /// 获取所有的公司类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetCompanyTypes()
        {
            var companyTypes = Enum.GetValues(typeof(CompanyType)) as CompanyType[];
            if (companyTypes == null) return null;
            return
                companyTypes.Where(item => item != CompanyType.Platform).Select(
                    item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
        }

    }
}