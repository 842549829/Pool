using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Service;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule
{
    public partial class PolicyOperatorLog : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                this.txtStartTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                this.txtEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                Query(1);
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Query(1);
        }
        void Query(int index)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = index,
                GetRowCount = true
            };
            var condition = new OperationLogQueryCondition
            {
                Module = OperationModule.政策,
                AssociateKey = Request.QueryString["id"] ?? "",
                LogStartDate = txtStartTime.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtStartTime.Text),
                LogEndDate = txtEndTime.Text == "" ? (DateTime?)null : Convert.ToDateTime(txtEndTime.Text).AddDays(1)
            };
            var queryList = LogService.QueryOperationLogs(condition, pagination);
            var list = from item in queryList
                       select new
                                  {
                                      OptionTime = item.Time,
                                      Option = item.Account,
                                      OptionContent = item.Content,
                                      Role = item.Role.GetDescription()
                                  };
            grvlog.DataSource = list;
            grvlog.DataBind();
            if (queryList != null && queryList.Any())
            {
                pager.RowCount = pagination.RowCount;
                pager.Visible = true;
                showempty.Visible = false;
                grvlog.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                pager.Visible = false;
                showempty.Visible = true;
            }
        }
        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            Query(newPage);
            pager.CurrentPageIndex = newPage;
        }
    }
}