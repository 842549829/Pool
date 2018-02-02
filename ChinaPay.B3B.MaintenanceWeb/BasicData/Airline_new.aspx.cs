using System;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Airline_new : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"].ToString() == "add")
                    {
                        this.txtErCode.Enabled = true;
                    }
                    else
                    {
                        this.txtErCode.Enabled = false;
                        Refresh(Request.QueryString["code"].ToString());
                    }
                }
            }
        }
        private void Refresh(string code)
        {
            ChinaPay.B3B.Service.Foundation.Domain.Airline airLine = FoundationService.QueryAirline(code);
            if (airLine == null) return;
            this.txtErCode.Text = airLine.Code.Value;
            this.txtAirlineName.Text = airLine.Name;
            this.txtAirlineShortName.Text = airLine.ShortName;
            this.txtJsCode.Text = airLine.SettleCode;
            this.ddlAirlineStatus.SelectedValue = airLine.Valid == true ? "T" : "F";
        } 
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                AirlineView airlineView = new AirlineView()
                {
                    Code = this.txtErCode.Text.Trim(),
                    Name = this.txtAirlineName.Text.Trim(),
                    SettleCode = this.txtJsCode.Text.Trim(),
                    ShortName = this.txtAirlineShortName.Text.Trim(),
                    Valid = this.ddlAirlineStatus.SelectedValue == "T" ? true : false
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddAirline(airlineView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='Airline.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateAirline(airlineView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='Airline.aspx?Search=Back'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion
    }
}
