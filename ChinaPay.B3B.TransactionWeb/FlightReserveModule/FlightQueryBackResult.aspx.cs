using System;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public partial class FlightQueryBackResult : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("ticket.css");
            if(!IsPostBack) {
                try
                {
                    this.hidFlightValidyMinutes.Value = Service.SystemManagement.SystemParamService.FlightValidityMinutes.ToString();
                    this.hidDepartureName.Value = Service.FoundationService.QueryCityNameByAirportCode(Request.QueryString["departure"]);
                    this.hidArrivalName.Value = Service.FoundationService.QueryCityNameByAirportCode(Request.QueryString["arrival"]);
                    if (Request.QueryString["goDate"] != null) this.hidDate.Value = DateTime.Parse(Request.QueryString["goDate"]).ToString("MM月dd日 dddd");
                this.flightsArgs.Value = Request.Form["flightsArgs"];
                this.policyArgs.Value = Request.Form["policyArgs"];
                }
                catch (Exception ex)
                {
                    LogService.SaveTextLog(string.Format("请求地址：{0}，登陆公司名称：{1},员工帐号{2},发生时间:{3},异常信息：{4}", Request.Url.OriginalString, CurrentCompany.CompanyName, CurrentUser.UserName, DateTime.Now.ToLocalTime(), ex.Message));

                }
                bindGoFlightInfo();
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();    
            }
        }
        private void bindGoFlightInfo() {
            var goFlightInfo = DataTransferObject.FlightQuery.FlightView.Parse(Request.Form["flightsArgs"]);
            var policyInfo = DataTransferObject.FlightQuery.PolicyView.Parse(Request.Form["policyArgs"]);
            txtAirLineName.Text = goFlightInfo.AirlineName;
            txtAirCraftType.Text = goFlightInfo.Aircraft;
            txtAirLineCode.Text = goFlightInfo.AirlineCode+goFlightInfo.FlightNo+ " "+goFlightInfo.BunkCode;
            txtTakeOffTime.Text = goFlightInfo.Departure.Time.ToString("yyyy-MM-dd HH:mm");
            txtLanddingTime.Text = goFlightInfo.Arrival.Time.ToString("yyyy-MM-dd HH:mm");
            txtGoAirPort.Text = string.Format("{0}{1}{2}", goFlightInfo.Departure.City, goFlightInfo.Departure.Name, goFlightInfo.Departure.Terminal);
            txtBackAirPort.Text = string.Format("{0}{1}{2}", goFlightInfo.Arrival.City, goFlightInfo.Arrival.Name, goFlightInfo.Arrival.Terminal);
            lblPrice.Text = string.Format("￥{0}", policyInfo.PublishFare);
            txtEI.Text = goFlightInfo.EI.Replace("lt;","<").Replace("rt;",">");
            txtFare.Text = string.Format("{0}+{1}",goFlightInfo.AirportFee,goFlightInfo.BAF);
        }
    }
}