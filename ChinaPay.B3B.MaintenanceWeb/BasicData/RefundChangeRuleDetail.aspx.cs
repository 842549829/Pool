using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class RefundChangeRuleDetail : BasePage
    {
        readonly int m_pageSize = 10;

        public string AirlineCode
        {
            get { return Request.QueryString["Code"]??string.Empty;}
        }

        public string ReturnUrl
        {
            get {
                return Request.UrlReferrer.OriginalString;
            }
        }

        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Pager1.Visible = false;
                btnSelect_Click(sender, e);
            }
            this.Pager1.PageSize = m_pageSize;
            this.Pager1.CurrentPageChanged += Pager1_CurrentPageChanged;
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
                RefundAndReschedulingBase detail = FoundationService.QueryRefundAndReschedulingNewBase(AirlineCode);

                if (detail!=null)
                {
                    lblAirline.Text = string.Format("{0}({1})", detail.Airline.Name, detail.AirlineCode);
                    lblCondition.Text = detail.Condition;
                    lblScrapRules.Text = detail.Scrap;
                    lblUpgradRules.Text = detail.Upgrade;
                    lblRemark.Text = detail.Remark;
                    lblPhone.Text = detail.AirlineTel;

                    if (detail.RefundAndReschedulingDetail != null)
                    {
                        var totalDatas = detail.RefundAndReschedulingDetail;
                        var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                        var endRow = pagination.PageSize * pagination.PageIndex;
                        var datas = totalDatas.Skip(startRow > 0 ? startRow : 0).Take(pagination.PageSize);
                        this.gvRefundRules.DataSource = datas;
                        this.gvRefundRules.DataBind();
                        if (datas.Any())
                        {
                            this.Pager1.Visible = true;
                            if (pagination.GetRowCount)
                            {
                                Pager1.RowCount = totalDatas.Count();
                            }
                        }
                        else
                        {
                            Pager1.Visible = false;
                        }
                    }
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        #endregion

        #region 删除
        protected void gvChildTicketClassInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LinkButton linkButton = this.gvRefundRules.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if (linkButton != null)
            {
                try
                {
                    FoundationService.DeleteRefundAndReshedulingNewDetail(new Guid(linkButton.CommandArgument), CurrentUser.UserName);
                    RegisterScript("alert('删除成功！');");
                    Pager1_CurrentPageChanged(Pager1, Pager1.CurrentPageIndex);
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
