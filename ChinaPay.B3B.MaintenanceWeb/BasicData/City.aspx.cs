using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class City : BasePage
    {
        readonly int m_pageSize = 10;

        #region 加载数据
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                this.Pagerl.Visible = false;
                LoadCondition("City");
                this.btnSelect_Click(this, e);
            }
            this.Pagerl.PageSize = m_pageSize;
            this.Pagerl.CurrentPageChanged += Pager1_CurrentPageChanged;
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        private void DropList()
        {
            this.ddlProvinceName.DataSource = FoundationService.Provinces;
            this.ddlProvinceName.Items.Clear();
            this.ddlProvinceName.DataValueField = "Code";
            this.ddlProvinceName.DataTextField = "Name";
            this.ddlProvinceName.DataBind();
            this.ddlProvinceName.Items.Insert(0, new ListItem("-所有-", "-所有-"));
        }
        #endregion

        #region 查询
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = IsLoacCondition ? Pagerl.CurrentPageIndex : Pagerl.CurrentPageIndex = 1,
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
                PageIndex = this.Pagerl.CurrentPageIndex,
                GetRowCount = true
            };
            bindDatas(pagination);
        }
        void bindDatas(Pagination pagination) {
            try {
                var totalDatas = from item in FoundationService.Cities
                                 where isValidData(item)
                                 select item;
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                var datas = totalDatas.Take(endRow).Skip(startRow);
                this.gvCity.DataSource = datas;
                this.gvCity.DataBind();
                if(datas.Count() > 0) {
                    this.Pagerl.Visible = true;
                    if(pagination.GetRowCount) {
                        this.Pagerl.RowCount = totalDatas.Count();
                    }
                } else {
                    this.Pagerl.Visible = false;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }
        bool isValidData(Service.Foundation.Domain.City city) {
            if (!string.IsNullOrEmpty(this.txtCityCode.Text.Trim()) && this.txtCityCode.Text.Trim() != city.Code.Trim()) return false;
            if (!string.IsNullOrEmpty(this.txtCityName.Text.Trim()) && this.txtCityName.Text.Trim() != city.Name.Trim()) return false;
            if (this.ddlProvinceName.SelectedIndex > 0 && this.ddlProvinceName.SelectedValue.Trim() != city.ProvinceCode.Trim()) return false;
            return true;
        }
        #endregion

        #region 删除
        protected void gvCity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LinkButton linkButton = this.gvCity.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if (linkButton != null)
            {
                try
                {
                    FoundationService.DeleteCity(linkButton.CommandArgument, CurrentUser.UserName);
                    RegisterScript("alert('删除成功！'); window.location.href='City.aspx?Search=Back';");
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
