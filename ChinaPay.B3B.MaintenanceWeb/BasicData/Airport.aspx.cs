using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Airport : BasePage {
        readonly int m_pageSize = 10;

        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                this.pagerl.Visible = false;
                LoadCondition("Airport");
                this.btnSelect_Click(this, e);
            }
            this.pagerl.PageSize = m_pageSize;
            this.pagerl.CurrentPageChanged += Pager1_CurrentPageChanged;
        }
        //航空公司状态
        public string GetState(string enabled)
        {
            return Convert.ToBoolean(enabled) == true ? "启用" : "禁用";
        }
        //操作状态
        public string ButtonState(string enabled)
        {
            return Convert.ToBoolean(enabled) == true ? "禁用" : "启用";
        }
        public string GetMain(string ismain) {
            return Convert.ToBoolean(ismain) == true ? "是" : "否";
        }
        #endregion

        #region 查询
        protected void btnSelect_Click(object sender, EventArgs e) {
            var pagination = new Pagination() {
                PageSize = m_pageSize,
                PageIndex = IsLoacCondition ? pagerl.CurrentPageIndex : pagerl.CurrentPageIndex=1,
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
                var totalDatas = from item in FoundationService.Airports
                                 where isValidData(item)
                                 select item;
                var startRow = pagination.PageSize * (pagination.PageIndex - 1);
                var endRow = pagination.PageSize * pagination.PageIndex;
                var datas = totalDatas.Take(endRow).Skip(startRow);
                this.gvAirport.DataSource = datas;
                this.gvAirport.DataBind();
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
        private bool isValidData(ChinaPay.B3B.Service.Foundation.Domain.Airport airport)
        {
            if (!string.IsNullOrEmpty(this.txtAirportCode.Text.Trim().ToUpper()) && this.txtAirportCode.Text.Trim().ToUpper() != airport.Code.Value) return false;
            if (this.ddlAirportStatus.SelectedIndex > 0 && Convert.ToBoolean(this.ddlAirportStatus.SelectedValue) != airport.Valid) return false;
            return true;
        }
        #endregion

        #region 删除
       protected void gvAirport_RowDeleting(object sender, GridViewDeleteEventArgs e)
       {
           LinkButton linkButton = this.gvAirport.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
           if (linkButton != null)
           {
               try
               {
                   FoundationService.DeleteAirport(linkButton.CommandArgument, CurrentUser.UserName);
                   RegisterScript("alert('删除成功！'); window.location.href='Airport.aspx?Search=Back';");
               } catch(Exception ex) {
                   ShowExceptionMessage(ex, "删除");
                   return;
               }
               refresh();
           }
       } 
       #endregion

        #region 状态控制
       protected void gvAirport_RowCommand(object sender, GridViewCommandEventArgs e)
       {
           if(e.CommandName == "opdate") {
               string code = e.CommandArgument.ToString();
               ChinaPay.B3B.Service.Foundation.Domain.Airport air = FoundationService.QueryAirport(code);
               if(air == null)
                   return;
               AirportView airportView = new AirportView() {
                   Code = code.Trim(),
                   Valid = air.Valid == true ? false : true,
                   Name = air.Name,
                   ShortName = air.ShortName,
                   LocationCode = air.LocationCode,
                   IsMain = air.IsMain,
                   LocationLevel = air.LocationLevel
               };
               try {
                   FoundationService.UpdateAirport(airportView, CurrentUser.UserName);
                   if(air.Valid)
                       RegisterScript("alert('禁用成功！'); window.location.href='Airport.aspx?Search=Back';");
                   else
                       RegisterScript("alert('启用成功！'); window.location.href='Airport.aspx?Search=Back';");
               } catch(Exception ex) {
                   ShowExceptionMessage(ex, air.Valid ? "禁用" : "启用");
                   return;
               }
               refresh();
           }
       } 
       #endregion
    }
}
