using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class bargain_defaultpolicy_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitData();
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryBargainDefaultPolicy(pagination);
        }

        private void InitData()
        {
            this.ddlAirlines.DataSource = from item in FoundationService.Airlines
                                          select new
                                          {
                                              ShortName = item.Code + "-" + item.ShortName,
                                              Code = item.Code
                                          };
            ddlAirlines.DataTextField = "ShortName";
            ddlAirlines.DataValueField = "Code";
            this.ddlAirlines.DataBind();
            ddlAirlines.Items.Insert(0, new ListItem("-请选择-", ""));
            //AdultAgentCompany.InitCompanies(CompanyService.GetCompanies( CompanyType.Provider ));
            AdultAgentCompany.SetCompanyType(CompanyType.Provider);
            ddlProvince.DataSource = from item in FoundationService.Provinces
                                     select new
                                     {
                                         Name = item.Name,
                                         Code = item.Code
                                     };
            ddlProvince.DataTextField = "Name";
            ddlProvince.DataValueField = "Code";
            ddlProvince.DataBind();
            ddlProvince.Items.Insert(0, new ListItem("-请选择-", ""));
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination()
                {
                    GetRowCount = true,
                    PageIndex = 1,
                    PageSize = pager.PageSize
                };
                queryBargainDefaultPolicy(pagination);
            }
            else
            {
                this.pager.CurrentPageIndex = 1;
            }
        }

        private void queryBargainDefaultPolicy(Pagination pagination)
        {
            try
            {
                var queryList = PolicyManageService.GetBargainDefaultPolicies(getCondition(), pagination);
                var list = from item in queryList
                           select new
                           {
                               Airline = item.Airline,
                               AdultProviderAbbreviateName = item.AdultProviderAbbreviateName,
                               AdultCommission = double.Parse((item.AdultCommission * 100).ToString()).ToString() + "%",
                               Province = FoundationService.QueryProvice(item.ProvinceCode).Name,
                               ProvinceCode = item.ProvinceCode
                           };
                this.dataSource.DataSource = list;
                this.dataSource.DataBind();
                if (pagination.RowCount>0)
                {
                    this.pager.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    dataSource.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private BargainDefaultPolicyQueryParameter getCondition()
        {
            BargainDefaultPolicyQueryParameter parameter = new BargainDefaultPolicyQueryParameter();
            if (!string.IsNullOrWhiteSpace(ddlAirlines.SelectedValue))
            {
                parameter.Airline = ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(ddlProvince.SelectedValue))
                parameter.ProvinceCode = ddlProvince.SelectedValue;
            if (AdultAgentCompany.CompanyId.HasValue)
                parameter.AdultProviderId = AdultAgentCompany.CompanyId;
            return parameter;
        }

        protected void dataSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] condition = e.CommandArgument.ToString().Split(',');
            string airline = condition[0];
            string provinceCode = condition[1];
            try
            {
                PolicyManageService.DeleteBargainDefaultPolicy(airline, provinceCode,this.CurrentUser.UserName);
                ShowMessage("删除成功！");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"删除");
            }
            btnQuery_Click(sender,e);
        }
    }
}