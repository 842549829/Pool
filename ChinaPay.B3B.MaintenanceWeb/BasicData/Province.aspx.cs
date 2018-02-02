using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData {
    public partial class Province : BasePage {
        readonly int m_pageSize = 10;

        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                this.Pagerl.Visible = false;
                initArea();
                LoadCondition("Province");
                this.btnSelect_Click(this, e);
            }
            this.Pagerl.PageSize = m_pageSize;
            this.Pagerl.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(Pager1_CurrentPageChanged);
        }

        private void initArea() {
            this.ddlArea.DataSource = FoundationService.Areas;
            this.ddlArea.DataTextField = "Name";
            this.ddlArea.DataValueField = "Code";
            this.ddlArea.DataBind();
            this.ddlArea.Items.Insert(0, new ListItem("-所有-", ""));
        }

        protected void btnSelect_Click(object sender, EventArgs e) {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = IsLoacCondition ? Pagerl.CurrentPageIndex : Pagerl.CurrentPageIndex = 1,
                GetRowCount = true
            };
            bindDatas(pagination);
        }

        void refresh() {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = Pagerl.CurrentPageIndex,
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
                var totalDatas = from item in FoundationService.Provinces
                                 where isValidData(item)
                                 select item;
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                var datas = totalDatas.Take(endRow).Skip(startRow);
                this.gvProvice.DataSource = datas;
                this.gvProvice.DataBind();
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

        bool isValidData(Service.Foundation.Domain.Province province) {
            if(!string.IsNullOrWhiteSpace(this.txtProvinceCode.Text) && this.txtProvinceCode.Text.Trim() != province.Code.Trim())
                return false;
            if(!string.IsNullOrWhiteSpace(this.txtProvinceName.Text) && this.txtProvinceName.Text.Trim() != province.Name.Trim())
                return false;
            if(this.ddlArea.SelectedValue != "" && this.ddlArea.SelectedValue != province.AreaCode){
                return false;
            }
            return true;
        }

        protected void gvProvice_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            LinkButton linkButton = this.gvProvice.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if(linkButton != null) {
                try {
                    List<ChinaPay.B3B.Service.Foundation.Domain.City> cityList = FoundationService.Cities.Select(c => c.Code == linkButton.CommandArgument) as List<ChinaPay.B3B.Service.Foundation.Domain.City>;
                    if(cityList != null && cityList.Count() > 0) {
                        ShowMessage("该省份含有城市信息，不能删除！");
                        return;
                    }
                    FoundationService.DeleteProvince(linkButton.CommandArgument, CurrentUser.UserName);
                    RegisterScript("alert('删除成功！'); window.location.href='Province.aspx?Search=Back';");
                } catch(Exception ex) {
                    ShowExceptionMessage(ex, "删除");
                    return;
                }
                refresh();
            }
        }
    }
}
