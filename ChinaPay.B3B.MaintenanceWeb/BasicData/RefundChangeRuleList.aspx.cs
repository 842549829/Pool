using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class RefundChangeRuleList : BasePage
    {
        readonly int m_pageSize = 10;

        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                this.Pager1.Visible = false;
            }
            this.Pager1.PageSize = m_pageSize;
            this.Pager1.CurrentPageChanged += Pager1_CurrentPageChanged;
        }
        public decimal CountDiscount(object discount) {
            return Convert.ToDecimal(discount) * 100;
        }
        private void DropList()
        {
            foreach (var item in FoundationService.Airlines)
            {
                this.ddlAirline.Items.Add(new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value));
            }
            this.ddlAirline.Items.Insert(0, new ListItem("-所有-", ""));
        } 
        #endregion

        #region 查询
        protected void btnSelect_Click(object sender, EventArgs e) {
            if(this.Pager1.CurrentPageIndex == 1) {
                var pagination = new Pagination() {
                    PageSize = m_pageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                bindDatas(pagination);
            } else {
                this.Pager1.CurrentPageIndex = 1;
            }
        }

        void refresh() {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = this.Pager1.CurrentPageIndex,
                GetRowCount = true
            };
            bindDatas(pagination);
        }

        void Pager1_CurrentPageChanged(UserControl.Pager sender, int newPage) {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            bindDatas(pagination);
        }

        void bindDatas(Pagination pagination) {
            try {
                string airlineCode = null;
                if (!string.IsNullOrWhiteSpace(ddlAirline.SelectedValue))
                {
                    airlineCode = ddlAirline.SelectedValue.Trim().ToUpper();
                }
                var totalDatas = FoundationService.QueryAllRefundAndReschedulings(airlineCode);
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                this.gvChildTicketClassInfo.DataSource = totalDatas.Skip(startRow>0?startRow:0).Take(pagination.PageSize).Select(r => new
                {
                    Carrier = r.AirlineCode,
                    CarrierName = r.AirlineName,
                    HasRules = r.HasRules  ? "已添加" : "未添加",
                    RulesCount = r.RulesCount
                });
                this.gvChildTicketClassInfo.DataBind();
                if (totalDatas.Any())
                {
                    this.Pager1.Visible = true;
                    if(pagination.GetRowCount) {
                        this.Pager1.RowCount = totalDatas.Count();
                    }
                } else {
                    this.Pager1.Visible = false;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        #endregion

    }
}
