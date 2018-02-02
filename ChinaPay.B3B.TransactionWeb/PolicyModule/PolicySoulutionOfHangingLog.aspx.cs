using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule
{
    public partial class PolicySoulutionOfHangingLog : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                this.txtStartTime.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                this.txtEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
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
        void QueryPolicy(Pagination pagination)
        {
            try
            {
                var querylist = PolicyManageService.GetSuspendOperation(GetCondition(), pagination);
                var list = from item in querylist
                           select new
                           {
                               OptionTime = item.OperateTime,
                               Option = item.CompanyId == Guid.Parse("00000000-0000-0000-0000-000000000001") ? item.CompanyName + "(" + item.Operator + ")" : "<a href='/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + item.CompanyId + "'>" + item.CompanyName + "(" + item.Operator + ")" + " </a>",
                               OptionOwnerType = item.OperatorRoleType.GetDescription(),
                               OptionType = item.OperateType.GetDescription(),
                               OptionContent = item.OperateType.GetDescription() + "：" + item.Airlines,
                               OptionReason = item.Reason,
                               IP = item.IP,
                               companyid = item.CompanyId,
                               companyName = item.CompanyName
                           };
                grvlog.DataSource = list;
                grvlog.DataBind();
                if (pagination.RowCount > 0)
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    showempty.Visible = false;
                    grvlog.HeaderRow.TableSection = TableRowSection.TableHeader;
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

        private SuspendOperationQueryParameter GetCondition()
        {
            SuspendOperationQueryParameter parameter = new SuspendOperationQueryParameter
            {
                OperateDate = new ChinaPay.Core.Range<DateTime> { Lower = DateTime.Parse(txtStartTime.Text), Upper = DateTime.Parse(txtEndTime.Text).AddDays(1).AddMilliseconds(-1) },
            };
            if (Request.QueryString["id"] != null)
            {
                parameter.Company = Guid.Parse(Request.QueryString["id"]);
            }
            return parameter;
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

    }
}