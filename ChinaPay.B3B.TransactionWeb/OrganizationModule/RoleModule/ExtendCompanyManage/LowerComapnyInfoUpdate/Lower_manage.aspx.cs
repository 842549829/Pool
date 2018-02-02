using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate
{
    public partial class LowerManage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                initData();
            BtnQuery_Click(this, e);
            LoadCondition("LowerCompany");
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void initData()
        {
            #region LoadCompanyStatus

            //IEnumerable<KeyValuePair<int, string>> companyStatus = GetCompanyStatuss();
            //ddlStatus.DataTextField = "Value";
            //ddlStatus.DataValueField = "Key";
            //ddlStatus.DataSource = companyStatus;
            //ddlStatus.DataBind();
            //ddlStatus.Items.Insert(0, "全部");

            btnCreateSubCompany.PostBackUrl = CompanyService.GetCompanyParameter(CurrentCompany.CompanyId).CanHaveSubordinate
                                                  ? "/OrganizationModule/RoleModule/ExtendCompanyManage/AddLower.aspx"
                                                  : "/OrganizationModule/CommonContent/AddAccount/ExtendOpenAccount.aspx?Type=Purchaser";

            #endregion
            ddlRelationType.Items.Add( new ListItem("下级采购",((int)RelationshipType.Distribution).ToString()));
            var companyParameter = CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId);
            if (companyParameter.CanHaveSubordinate)
                ddlRelationType.Items.Add(new ListItem("组织机构",((int)RelationshipType.Organization).ToString()));
            ddlRelationType.Items.Insert(0, new ListItem("全部",""));
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryLowerCompanys(pagination);
        }

        protected void BtnQuery_Click(object sender, EventArgs e)
        {
                var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex=1,
                    GetRowCount = true
                };
                queryLowerCompanys(pagination);
        }

        private void queryLowerCompanys(Pagination pagination)
        {
            try
            {
                var companys = CompanyService.GetAllSubordinates(getSearchParameter(),pagination);
                datalist.DataSource = from company in companys
                                      select new
                                          {
                                              company.AbbreviateName,
                                              company.UserNo,
                                              company.Contact,
                                              company.Enabled,
                                              company.CompanyId,
                                              Status = company.Enabled ? "启用" : "禁用",
                                              company.Group,
                                              RelationType =
                                          company.RelationshipType == RelationshipType.Organization
                                              ? "内部机构"
                                              : company.RelationshipType == RelationshipType.Distribution ? "下级采购" : string.Empty,
                                              ResetPWDEnable = company.RelationshipType != RelationshipType.Distribution,
                                              RelationTypeStr = company.RelationshipType,
                                              AccountType = company.AccountType.GetDescription()
                                          };

                datalist.DataBind();
                if (companys.Count() > 0)
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

        private SubordinateQueryParameter getSearchParameter()
        {
            var param = new SubordinateQueryParameter();
            if (!string.IsNullOrWhiteSpace(txtAbbreviateName.Text))
                param.AbbreviateName = txtAbbreviateName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtUserName.Text))
                param.UserNo = txtUserName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtContact.Text))
                param.Contact = txtContact.Text.Trim();
            param.Superior = CurrentCompany.CompanyId;
            if (!string.IsNullOrEmpty(ddlAccountType.SelectedValue))
                param.AccountType = (AccountBaseType)byte.Parse(ddlAccountType.SelectedValue);

            if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue.Trim()))
            {
                if (ddlStatus.SelectedValue == "1")
                {
                    param.Enabled = true;
                }
                else
                {
                    param.Enabled = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.ddlRelationType.SelectedValue))
                param.RelationshipType = (RelationshipType)int.Parse(this.ddlRelationType.SelectedValue);
                return param;
        }

        /// <summary>
        /// 获取所有的公司状态
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetCompanyStatuss()
        {
            var companyTypes = Enum.GetValues(typeof(CompanyStatus)) as CompanyStatus[];
            if (companyTypes == null) return null;
            return
                companyTypes.Select(
                    item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
        }

        /// <summary>
        /// 禁用下级公司
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Disable")
            {
                try
                {
                    var companyId = new Guid(e.CommandArgument.ToString());
                    PolicyType policyType = Service.Policy.PolicyManageService.CheckIfHasDefaultPolicy(companyId);
                    if (PolicyType.Unknown != policyType) throw new CustomException("该供应商在平台存在默认政策指向，请调整后再行操作");
                    //if (CompanyService.Disable(companyId,this.CurrentUser.UserName))
                    //{
                    //    ShowMessage("禁用成功");
                    //}
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
                    var companyId = new Guid(e.CommandArgument.ToString());
                    if (CompanyService.Enable(companyId,this.CurrentUser.UserName))
                    {
                        ShowMessage("启用成功");
                    }
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
            queryLowerCompanys(pagination);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var result = EmployeeService.ResetPassword(hdCompanyAccount.Value.Trim(), Reason.Text.Trim(), this.CurrentUser.UserName);
                if (result)
                {
                    ShowMessage("密码重置成功！");
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "重置密码");
            }
        }
    }
}