using System;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class PlaneType_new : BasePage
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
                        this.txtPlaneTypeCode.Enabled = true;
                    }
                    else
                    {
                        this.txtPlaneTypeCode.Enabled = false;
                        Refresh(Request.QueryString["Id"].ToString());
                    }
                }
            }
        }
        private void Refresh(string code)
        {
            ChinaPay.B3B.Service.Foundation.Domain.AirCraft airCraft = FoundationService.QueryAirCraft(new Guid(code));
            if (airCraft.Code.Value != null) this.txtPlaneTypeCode.Text = airCraft.Code.Value;
            if (airCraft.Name != null) this.txtPlaneTypeName.Text = airCraft.Name;
            if (airCraft.Manufacturer != null) this.txtMake.Text = airCraft.Manufacturer;
            if (airCraft.Description != null) this.ttDescription.InnerText = airCraft.Description;
            this.txtAirportPrice.Text = airCraft.AirportFee.ToString();
            this.ddlStatus.SelectedValue = airCraft.Valid == true ? "T" : "F";
        } 
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                AirCraftView airCraftView = new AirCraftView()
                {
                    Code = this.txtPlaneTypeCode.Text.Trim(),
                    Name = this.txtPlaneTypeName.Text.Trim(),
                    AirportFee = Convert.ToDecimal(this.txtAirportPrice.Text.Trim()),
                    Manufacturer = this.txtMake.Text.Trim(),
                    Description = this.ttDescription.InnerText.Trim(),
                    Valid = this.ddlStatus.SelectedValue == "T" ? true : false
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddAirCraft(airCraftView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='PlaneType.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateAirCraft(new Guid(Request.QueryString["Id"].ToString()), airCraftView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='PlaneType.aspx?Search=Back'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion
    }
}
