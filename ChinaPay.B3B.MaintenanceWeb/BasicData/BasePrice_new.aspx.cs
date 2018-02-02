using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class BasePrice_new : BasePage
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
        private void Refresh(string id) 
        {
            ChinaPay.B3B.Service.Foundation.Domain.BasicPrice basicPrice = FoundationService.QueryBasicPriceNew(new Guid(id));
            if (basicPrice == null) return;
            if (basicPrice.AirlineCode.Value != null) this.ddlAirline.SelectedValue = basicPrice.AirlineCode.Value;
            if (basicPrice.DepartureCode.Value != null) this.drpDepartAirport.SelectedValue = basicPrice.DepartureCode.Value;
            if (basicPrice.ArrivalCode.Value != null) this.drpArriveAirport.SelectedValue = basicPrice.ArrivalCode.Value;
            this.txtHbDate.Text = basicPrice.FlightDate.ToString("yyyy-MM-dd");
            this.txtCpDate.Text = basicPrice.ETDZDate.ToString("yyyy-MM-dd");
            this.txtPrice.Text = basicPrice.Price.ToString();
            this.txtMileage.Text = basicPrice.Mileage.ToString();
        }
        private void DropList()
        {
            foreach (var item in FoundationService.Airlines)
            {
                this.ddlAirline.Items.Add(new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value));   
            }
            this.ddlAirline.Items.Insert(0, new ListItem("-请选择-", "0"));
            foreach (var item in FoundationService.Airports)
            {
                string name = item.Code.Value + "-" + item.ShortName;
                this.drpDepartAirport.Items.Add(new ListItem(name, item.Code.Value));
                this.drpArriveAirport.Items.Add(new ListItem(name, item.Code.Value));
            }
            this.drpArriveAirport.Items.Insert(0, new ListItem("-请选择-", "0"));
            this.drpDepartAirport.Items.Insert(0, new ListItem("-请选择-", "0"));
        }
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                
                BasicPriceView basePriceView = new BasicPriceView()
                {
                      Airline = this.ddlAirline.SelectedValue == "0" ? string.Empty : this.ddlAirline.SelectedValue,
                      Arrival = this.drpArriveAirport.SelectedValue,
                      Departure = this.drpDepartAirport.SelectedValue,
                      FlightDate = Convert.ToDateTime(this.txtHbDate.Text),
                      ETDZDate = Convert.ToDateTime(this.txtCpDate.Text),   
                      Price = Convert.ToDecimal(this.txtPrice.Text.Trim()),
                      Mileage = Convert.ToDecimal(this.txtMileage.Text.Trim())
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddBasicPrice(basePriceView, CurrentUser.UserName);
                        ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, "alert('添加成功！'); window.location.href='BasePrice.aspx';", true);
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateBasicPrice(new Guid(Request.QueryString["Id"].ToString()), basePriceView, CurrentUser.UserName);
                        ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, "alert('修改成功！'); window.location.href='BasePrice.aspx?Search=Back';", true);
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion
    }
}
