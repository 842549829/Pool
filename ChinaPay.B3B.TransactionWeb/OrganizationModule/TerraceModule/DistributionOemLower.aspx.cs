using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.TransactionWeb.UserControl;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOemLower : BasePage
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
        private Guid CompanyId
        {
            get {
                return Guid.Parse(Request.QueryString["CompanyId"]);
            }
        }
        private void initData()
        {
            ddlRelationType.Items.Add(new ListItem("下级采购", ((int)RelationshipType.Distribution).ToString()));
            var companyParameter = CompanyService.GetCompanyParameter(CompanyId);
            if (companyParameter.CanHaveSubordinate)
                ddlRelationType.Items.Add(new ListItem("组织机构", ((int)RelationshipType.Organization).ToString()));
            ddlRelationType.Items.Insert(0, new ListItem("全部", ""));
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
                PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                GetRowCount = true
            };
            queryLowerCompanys(pagination);
        }

        private void queryLowerCompanys(Pagination pagination)
        {
            try
            {
                var companys = CompanyService.GetAllSubordinates(getSearchParameter(), pagination);
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
                                          RelationType = company.RelationshipType == RelationshipType.Organization
                                          ? "内部机构"
                                          : company.RelationshipType == RelationshipType.Distribution ? "下级采购" : string.Empty,
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
            param.Superior = CompanyId;
            if (!string.IsNullOrEmpty(ddlAccountType.SelectedValue))
                param.AccountType = (AccountBaseType)byte.Parse(ddlAccountType.SelectedValue);

            if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue.Trim()))
            {
                param.Enabled = ddlStatus.SelectedValue == "1";
            }
            if (!string.IsNullOrWhiteSpace(this.ddlRelationType.SelectedValue))
                param.RelationshipType = (RelationshipType)int.Parse(this.ddlRelationType.SelectedValue);
            return param;
        }
    }
}