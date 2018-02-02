using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class FixedNavigation : BasePage
    {
        readonly int m_pageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Pager.Visible = false;
            }
            this.Pager.PageSize = m_pageSize;
            this.Pager.CurrentPageChanged += Pager_CurrentPageChanged;
        }
        void Pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            bindDatas(pagination);
        }
        void bindDatas(Pagination pagination)
        {
            try
            {
                var totalDatas = from item in FoundationService.FixedNavigations
                                 where isValidData(item)
                                 select item;
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                var datas = totalDatas.Take(endRow).Skip(startRow);
                this.gvFixedNavigation.DataSource = datas;
                this.gvFixedNavigation.DataBind();
                if (datas.Count() > 0)
                {
                    this.Pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.Pager.RowCount = totalDatas.Count();
                    }
                }
                else
                {
                    this.Pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }
        private bool isValidData(FixedNavigationView view)
        {
            if (!string.IsNullOrEmpty(ucDepartures.Code) && ucDepartures.Code != view.Departure) return false;
            if (!string.IsNullOrEmpty(ucArrivals.Code) && ucArrivals.Code != view.Arrival) return false;
            return true;
        }
        void refresh()
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = this.Pager.CurrentPageIndex,
                GetRowCount = true
            };
            bindDatas(pagination);
        }
        protected void gvFixedNavigation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LinkButton linkButton = this.gvFixedNavigation.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            Label label = this.gvFixedNavigation.Rows[e.RowIndex].FindControl("lblArrival") as Label;
            if (linkButton != null)
            {
                try
                {
                    FoundationService.DeleteFixedNavigation(new FixedNavigationView(){ Departure=linkButton.CommandArgument,Arrival=label.Text}, CurrentUser.UserName);
                    ShowMessage("删除成功！");
                    refresh();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除");
                }
            }
        }
        protected void btnSelect_Click(object sender, EventArgs e) {
            if (this.Pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination()
                {
                    PageSize = m_pageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                bindDatas(pagination);
            }
            else
            {
                this.Pager.CurrentPageIndex = 1;
            }
        }
        #region 添加
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Checking();
                var fixedNavigation = new FixedNavigationView();
                fixedNavigation.Departure = ucDeparture.Code;
                fixedNavigation.Arrival = ucArrival.Code;
                FoundationService.AddFixedNavigation(fixedNavigation, CurrentUser.UserName);
                ShowMessage("保存成功！");
                refresh();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "保存");
            }
        }
        private void Checking()
        {
            if (string.IsNullOrEmpty(ucDeparture.Code))
            {
                 throw new ChinaPay.Core.Exception.KeyRepeatedException("出发地不能为空");
            }
            if (string.IsNullOrEmpty(ucArrival.Code))
            {
                throw new ChinaPay.Core.Exception.KeyRepeatedException("到达地不能为空");
            }
            if (ucArrival.Code == ucDeparture.Code)
            {
                throw new ChinaPay.Core.Exception.KeyRepeatedException("出发地不能跟到达地相同");
            }
        } 
        #endregion
    }
}