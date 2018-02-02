using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Fuel : BasePage {
        readonly int m_pageSize = 10;

        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                this.pagerl.Visible = false;
                LoadCondition("Fuel");
                this.btnSelect_Click(this, e);
            }
            this.pagerl.PageSize = m_pageSize;
            this.pagerl.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(Pager1_CurrentPageChanged);
        }
        #endregion

        #region 查询
        protected void btnSelect_Click(object sender, EventArgs e) {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = IsLoacCondition?pagerl.CurrentPageIndex:pagerl.CurrentPageIndex =1,
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
        void refresh() {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = this.pagerl.CurrentPageIndex,
                GetRowCount = true
            };
            bindDatas(pagination);
        }
        void bindDatas(Pagination pagination) {
            try {
                var totalDatas = from item in FoundationService.BAFs
                                 where isValidData(item)
                                 select item;
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                var datas = totalDatas.Take(endRow).Skip(startRow);
                this.gvFuel.DataSource = datas;
                this.gvFuel.DataBind();
                if(datas.Count() > 0) {
                    this.pagerl.Visible = true;
                    if(pagination.GetRowCount) {
                        this.pagerl.RowCount = totalDatas.Count();
                    }
                } else {
                    this.pagerl.Visible = false;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }
        bool isValidData(ChinaPay.B3B.Service.Foundation.Domain.BAF BAF) 
        {
            if (!string.IsNullOrEmpty(this.txtStartMileage.Text.Trim()) && this.txtStartMileage.Text.Trim() != BAF.Mileage.ToString()) return false;
            if (!string.IsNullOrEmpty(this.txtStartDate.Text) && this.txtStartDate.Text != BAF.EffectiveDate.ToString("yyyy-MM-dd")) return false;
            if (!string.IsNullOrEmpty(this.txtStopDate.Text) && this.txtStopDate.Text != Convert.ToDateTime(BAF.ExpiredDate).ToString("yyyy-MM-dd")) return false;
            if (!string.IsNullOrEmpty(this.ucAriline.Code) && this.ucAriline.Code != BAF.AirlineCode.Value) return false;
            return true;
        }
        #endregion

        #region 删除
        protected void gvFuel_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
           LinkButton linkButton = this.gvFuel.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
           if (linkButton != null)
           {
               try
               {
                   FoundationService.DeleteBAF(new Guid(linkButton.CommandArgument), CurrentUser.UserName);
                   RegisterScript("alert('删除成功！'); window.location.href='Fuel.aspx';");
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
