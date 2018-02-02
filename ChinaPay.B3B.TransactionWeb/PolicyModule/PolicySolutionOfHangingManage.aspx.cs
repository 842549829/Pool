using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule
{
    public partial class PolicySolutionOfHangingManage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                ucCompany.SetCompanyType(CompanyType.Provider | CompanyType.Supplier);
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
            QueryPolicy(pagination);
        }

        void QueryPolicy(Pagination pagination)
        {
            try
            {
                var querylist = PolicyManageService.GetSuspendInfos(GetCondition(pagination),
                                                                    item => item.AbbreviateName);
                var list = from item in querylist
                           select new
                           {
                               CompanyType = item.CompanyType.GetDescription(),
                               item.CompanyId,
                               item.AbbreviateName,
                               SuspendByCompany = item.SuspendByCompany.Join(","),
                               SuspendByPlatform = item.SuspendByPlatform.Join(",")
                           };
                grvhang.DataSource = list;
                grvhang.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = querylist.RowCount;
                    }
                    showempty.Visible = false;
                    grvhang.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    showempty.Visible = true;
                    this.pager.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private SuspendInfoQueryParameter GetCondition(Pagination pagination)
        {
            var parameter = new SuspendInfoQueryParameter
            {
                Company = ucCompany.CompanyId,
                PageSize = pager.PageSize,
                PageIndex = pagination.PageIndex
            };
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = 1,
                GetRowCount = true
            };
            QueryPolicy(pagination);
        }
    }
}