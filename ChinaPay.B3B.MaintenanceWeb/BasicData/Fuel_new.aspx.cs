using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Fuel_new : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"].ToString() == "update")
                        Refresh(Request.QueryString["Id"].ToString());
                }
            }
        }
        private void Refresh(string code) 
        {
            ChinaPay.B3B.Service.Foundation.Domain.BAF baf = FoundationService.QueryBAF(new Guid(code));
            if(baf == null) return;
            if (baf.AirlineCode.Value != null) this.ddlAirline.SelectedValue = baf.AirlineCode.Value;
            if (baf.EffectiveDate !=null) this.txtStartDate.Text = baf.EffectiveDate.ToString("yyyy-MM-dd");
            if (baf.ExpiredDate!=null) this.txtStopDate.Text = Convert.ToDateTime(baf.ExpiredDate).ToString("yyyy-MM-dd");
            this.txtAdult.Text = baf.Adult.ToString();
            this.txtChild.Text = baf.Child.ToString();
            this.txtStartMileage.Text = baf.Mileage.ToString();
        }
        private void DropList()
        {
            foreach (var item in FoundationService.Airlines)
            {
                this.ddlAirline.Items.Add(new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value)); 
            }
            this.ddlAirline.Items.Insert(0, new ListItem("-请选择-", "0"));
        } 
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                BAFView bafView = new BAFView()
                {
                    Airline = this.ddlAirline.SelectedValue,
                    Mileage = Convert.ToDecimal(this.txtStartMileage.Text.Trim()),
                    EffectiveDate = Convert.ToDateTime(this.txtStartDate.Text),
                    Adult = Convert.ToDecimal(this.txtAdult.Text.Trim()),
                    Child = Convert.ToDecimal(this.txtChild.Text.Trim())
                };
                if (!string.IsNullOrEmpty(this.txtStopDate.Text))
                {
                    bafView.ExpiredDate = Convert.ToDateTime(this.txtStopDate.Text);
                }
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddBAF(bafView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='Fuel.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateBAF(new Guid(Request.QueryString["Id"].ToString()), bafView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='Fuel.aspx?Search=Back'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        }
        #endregion
    }
}
