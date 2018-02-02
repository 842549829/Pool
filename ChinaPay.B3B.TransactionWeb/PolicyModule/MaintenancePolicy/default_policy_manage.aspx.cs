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
    public partial class default_policy_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
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
                ddlAirlines.Items.Insert(0,new ListItem("-请选择-", ""));

                ucAudltProvider.SetCompanyType(CompanyType.Provider);
                ucChildProvider.SetCompanyType(CompanyType.Provider);

            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryDefaultPolicy(pagination);
        }

        private void queryDefaultPolicy(Pagination pagination)
        {
            try{
                var queryList = PolicyManageService.GetDefaultPolicies(getCondition(), pagination);
                var list = from item in queryList
                           select new
                           {
                               Airline = item.Airline,
                               AdultProviderAbbreviateName = item.AdultProviderAbbreviateName,
                               AdultCommission = double.Parse((item.AdultCommission*100).ToString()).ToString() +"%",
                               ChildProviderAbbreviateName = item.ChildProviderAbbreviateName,
                               ChildCommission = double.Parse((item.ChildCommission*100).ToString()).ToString() +"%"
                           };
                this.dataSource.DataSource = list;
                this.dataSource.DataBind();
                if (pagination.RowCount > 0)
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
            catch(Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private DefaultPolicyQueryParameter getCondition()
        {
            DefaultPolicyQueryParameter parameter = new DefaultPolicyQueryParameter();
            if (!string.IsNullOrWhiteSpace(ddlAirlines.SelectedValue))
            {
                parameter.Airline = ddlAirlines.SelectedValue;
            }
            parameter.AdultProviderId = ucAudltProvider.CompanyId;
            parameter.ChildProviderId = ucChildProvider.CompanyId;
            return parameter;
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
                queryDefaultPolicy(pagination);
            }
            else
            {
                this.pager.CurrentPageIndex = 1;
            }
        }

        //protected void dataSource_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName.ToString() == "del")
        //    {
        //        try
        //        {
        //            PolicyManageService.DeleteDefaultPolicy(e.CommandArgument.ToString());
        //            btnQuery_Click(sender,e);
        //        }
        //        catch (Exception)
        //        {
        //            ShowMessage("系统异常");
        //        }
        //    }
        //}
    }
}