using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role {
    public partial class AirlineRetreatChangeNew : UnAuthBasePage{
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                BindAirline();
            }
        }
        private void BindAirline() {
            string carrier = Request.QueryString["Carrier"];
            var airlineList = FoundationService.RefundAndReschedulings.OrderBy(item => item.Level).Where(w => w.Airline != null && !string.IsNullOrEmpty(w.AirlineCode.Value));
            hdfAirlineCode.Value = string.IsNullOrEmpty(carrier) && airlineList.Any() ? airlineList.FirstOrDefault().AirlineCode.Value : carrier;
            airlines.InnerHtml = string.Empty;
            foreach(var item in airlineList) {
                airlines.InnerHtml += "<a value='" + item.AirlineCode.Value + "'>" + item.Airline.ShortName + "</a>";
            }
        }
    }
}