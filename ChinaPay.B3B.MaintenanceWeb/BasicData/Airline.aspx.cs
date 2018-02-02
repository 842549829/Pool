using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData {
    public partial class Airline : BasePage {
        readonly int m_pageSize = 10;

        #region 数据加载
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                this.Pagerl.Visible = false;
                LoadCondition("Airline");
                this.btnSelect_Click(this, e);
            }
            this.Pagerl.PageSize = m_pageSize;
            this.Pagerl.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(Pager1_CurrentPageChanged);
        }
        //航空公司状态
        public string GetState(string enabled) {
            return Convert.ToBoolean(enabled) == true ? "启用" : "禁用";
        }
        //操作状态
        public string ButtonState(string enabled) {
            return Convert.ToBoolean(enabled) == true ? "禁用" : "启用";
        }
        #endregion

        #region 查询
        protected void btnSelect_Click(object sender, EventArgs e) {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = IsLoacCondition ? Pagerl.CurrentPageIndex : Pagerl.CurrentPageIndex=1,
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
                var totalDatas = from item in FoundationService.Airlines
                                 where isValidData(item)
                                 select item;
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                var datas = totalDatas.Take(endRow).Skip(startRow);
                this.gvAirline.DataSource = datas;
                this.gvAirline.DataBind();
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
        private bool isValidData(ChinaPay.B3B.Service.Foundation.Domain.Airline airline) {
            if(!string.IsNullOrEmpty(this.txtErCode.Text.Trim().ToUpper()) && this.txtErCode.Text.Trim().ToUpper() != airline.Code.Value)
                return false;
            if(!string.IsNullOrEmpty(this.txtErName.Text.Trim()) && this.txtErName.Text.Trim() != airline.Name)
                return false;
            if(!string.IsNullOrEmpty(this.txtErShortName.Text.Trim()) && this.txtErShortName.Text.Trim() != airline.ShortName)
                return false;
            if(!string.IsNullOrEmpty(this.txtErSettleCode.Text.Trim()) && this.txtErSettleCode.Text.Trim() != airline.SettleCode)
                return false;
            if(this.ddlAirportStatus.SelectedIndex > 0 && Convert.ToBoolean(this.ddlAirportStatus.SelectedValue) != airline.Valid)
                return false;
            return true;
        }
        #endregion

        #region 状态控制
        protected void gvAirline_RowCommand(object sender, GridViewCommandEventArgs e) {
            if(e.CommandName == "opdate") {
                string code = e.CommandArgument.ToString();
                ChinaPay.B3B.Service.Foundation.Domain.Airline air = FoundationService.QueryAirline(code);
                if(air == null)
                    return;
                AirlineView airlineView = new AirlineView() {
                    Code = code.Trim(),
                    Valid = !air.Valid,
                    Name = air.Name,
                    ShortName = air.ShortName,
                    SettleCode = air.SettleCode
                };
                try {
                    FoundationService.UpdateAirline(airlineView, CurrentUser.UserName);
                    if(air.Valid)
                        RegisterScript("alert('禁用成功！'); window.location.href='Airline.aspx?Search=Back';");
                    else
                        RegisterScript("alert('启用成功！'); window.location.href='Airline.aspx?Search=Back';");
                } catch(Exception ex) {
                    ShowExceptionMessage(ex, air.Valid ? "禁用" : "启用");
                    return;
                }
                refresh();
            }
        }
        #endregion

        #region 删除
        protected void gvAirline_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            LinkButton linkButton = this.gvAirline.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if(linkButton != null) {
                try {
                    FoundationService.DeleteAirline(linkButton.CommandArgument, CurrentUser.UserName);
                    RegisterScript("alert('删除成功！'); window.location.href='Airline.aspx?Search=Back';");
                } catch(Exception ex) {
                    ShowExceptionMessage(ex, "删除");
                }
                refresh();
            }
        }
        #endregion
    }
}
