using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class ChildTicketMaintain : BasePage {
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
            this.ddlAirline.Items.Insert(0, new ListItem("-所有-", "-所有-"));
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
                var totalDatas = from item in FoundationService.ChildOrderableBunks
                                 where isValidData(item)
                                 select item;
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                var datas = totalDatas.Take(endRow).Skip(startRow);
                this.gvChildTicketClassInfo.DataSource = datas;
                this.gvChildTicketClassInfo.DataBind();
                if(datas.Any()) {
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
        private bool isValidData(ChinaPay.B3B.Service.Foundation.Domain.ChildOrderableBunk childOrderableBunks)
        {
            if (this.ddlAirline.SelectedIndex > 0 && this.ddlAirline.SelectedValue != childOrderableBunks.AirlineCode.Value) return false;
            if (!string.IsNullOrEmpty(this.txtCwCode.Text.Trim()) && this.txtCwCode.Text.Trim().ToUpper() != childOrderableBunks.BunkCode.Value.ToUpper()) return false;
            return true;
        }
        #endregion

        #region 删除
        protected void gvChildTicketClassInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LinkButton linkButton = this.gvChildTicketClassInfo.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if (linkButton != null)
            {
                try
                {
                    FoundationService.DeleteChildrenOrderableBunk(new Guid(linkButton.CommandArgument), CurrentUser.UserName);
                    RegisterScript("alert('删除成功！'); window.location.href='ChildTicketMaintain.aspx';");
                } catch(Exception ex) {
                    ShowExceptionMessage(ex, "删除");
                    return;
                }
                refresh();
            }
        } 
        #endregion
    }
}
