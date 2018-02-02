using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Airport_new : BasePage
    {
        #region 加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"].ToString() == "add")
                    {
                        this.txtAirportCode.Enabled = true;
                    }
                    else
                    {
                        this.txtAirportCode.Enabled = false;
                        Refresh(Request.QueryString["code"].ToString());
                    }
                }
            }
        }
        private void Refresh(string code)
        {
            ChinaPay.B3B.Service.Foundation.Domain.Airport airport = FoundationService.QueryAirport(code);
            if (airport == null) return;
            this.txtAirportCode.Text = airport.Code.Value;
            this.txtAirportName.Text = airport.Name;
            this.txtAirportShortName.Text = airport.ShortName;
            this.ddlAirportStatus.SelectedValue = airport.Valid == true ? "T" : "F";
            if (airport.IsMain == true)this.rdoOK.Checked = true;else this.rdoNo.Checked = true;
            if (airport.LocationLevel == LocationLevel.County)
            {
                var county = airport.Location as Service.Foundation.Domain.County;
                if (county == null) return;
                this.ddlCity.Visible = true;
                this.ddlCounty.Visible = true;
                if (county.City.ProvinceCode != null)
                    this.ddlProvince.SelectedValue = county.City.ProvinceCode;
                this.GetCity(county.City.ProvinceCode);
                if (county.CityCode != null)
                    this.ddlCity.SelectedValue = county.CityCode;
                this.GetCounty(county.CityCode);
                if (county.Code != null)
                    this.ddlCounty.SelectedValue = county.Code;
            }
            else if (airport.LocationLevel == LocationLevel.City)
            {
                var city = airport.Location as Service.Foundation.Domain.City;
                if (city == null) return;
                this.ddlCity.Visible = true;
                if (city.ProvinceCode != null)
                    this.ddlProvince.SelectedValue = city.ProvinceCode;
                this.GetCity(city.ProvinceCode);
                if(city.Code!=null) this.ddlCity.SelectedValue = city.Code;
            }
        } 
        private void DropList()
        {
            var province = FoundationService.Provinces;
            foreach (var item in province)
            {
                this.ddlProvince.Items.Add(new ListItem(item.Name,item.Code));
            }
        }
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
             if (Request.QueryString["action"] != null)
            {
                string locationCode = string.Empty;
                LocationLevel locationLevel = LocationLevel.City;
                if (this.ddlCounty.SelectedValue != "0")
                {
                    locationCode = this.ddlCounty.SelectedValue;
                    locationLevel = LocationLevel.County;
                }
                else {
                    locationCode = this.ddlCity.SelectedValue;
                }
                AirportView airportView = new AirportView()
                {
                    Code = this.txtAirportCode.Text.Trim(),
                    Name = this.txtAirportName.Text.Trim(),
                    ShortName = this.txtAirportShortName.Text.Trim(),
                    Valid = this.ddlAirportStatus.SelectedValue == "T" ? true : false,
                    LocationCode = locationCode,
                    LocationLevel = locationLevel,
                    IsMain = this.rdoOK.Checked == true ? true : false
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddAirport(airportView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='Airport.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateAirport(airportView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='Airport.aspx?Search=Back'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion

        #region 所属地方
        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string provinceCode = this.ddlProvince.SelectedValue;

            GetCity(provinceCode);
        }
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cityCode = this.ddlCity.SelectedValue;
            GetCounty(cityCode);
        }
        private void GetCounty(string cityCode)
        {
            this.ddlCounty.Items.Clear();
            this.ddlCounty.Items.Insert(0, new ListItem("-请选择-", "0"));
            if (cityCode != "0")
            {
                this.ddlCounty.Visible = true;
                var cityAll = FoundationService.Counties;
                var city = from item in cityAll
                           where item.CityCode == cityCode
                           select item;
                foreach (var item in city)
                {
                    this.ddlCounty.Items.Add(new ListItem(item.Name, item.Code));
                }
            }
            else
            {
                this.ddlCounty.Visible = false;
            }
        }
        private void GetCity(string provinceCode)
        {
            this.ddlCity.Items.Clear();
            this.ddlCounty.Items.Clear();
            this.ddlCity.Items.Insert(0, new ListItem("-请选择-", "0"));
            this.ddlCounty.Items.Insert(0, new ListItem("-请选择-", "0"));
            if (provinceCode != "0")
            {
                this.ddlCity.Visible = true;
                var cityAll = FoundationService.Cities;
                var city = from item in cityAll
                           where item.ProvinceCode == provinceCode
                           select item;
                foreach (var item in city)
                {
                    this.ddlCity.Items.Add(new ListItem(item.Name, item.Code));
                }
            }
            else
            {
                this.ddlCity.Visible = false;
                this.ddlCounty.Visible = false;
            }
        }
        #endregion
    }
}
