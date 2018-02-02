using System;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public partial class FlightQueryResult : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("ticket.css");

            if(!IsPostBack)
            {
                try
                {
                    this.hidFlightValidyMinutes.Value = Service.SystemManagement.SystemParamService.FlightValidityMinutes.ToString();
                    this.hidDepartureName.Value = Service.FoundationService.QueryCityNameByAirportCode(Request.QueryString["departure"]);
                    this.hidArrivalName.Value = Service.FoundationService.QueryCityNameByAirportCode(Request.QueryString["arrival"]);
                    if (Request.QueryString["goDate"] != null) this.hidDate.Value = DateTime.Parse(Request.QueryString["goDate"]).ToString("MM月dd日 dddd");
                }
                catch (Exception ex)
                {
                    LogService.SaveTextLog(string.Format("请求地址：{0}，登陆公司名称：{1},员工帐号{2},发生时间:{3},异常信息：{4}",Request.Url.OriginalString,CurrentCompany.CompanyName,CurrentUser.UserName,DateTime.Now.ToLocalTime(),ex.Message));
                }
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }
        }
    }
}