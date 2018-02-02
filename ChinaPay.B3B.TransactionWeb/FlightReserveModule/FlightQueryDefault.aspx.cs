using System;
using System.Web;
using System.Web.UI.HtmlControls;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public partial class FlightQueryDefault : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                var workingSetting = Service.Organization.CompanyService.GetWorkingSetting(CurrentCompany.CompanyId);
                if(workingSetting != null) {
                    // 默认出发城市
                    bindDefaultAirportInfo(workingSetting.DefaultDeparture, this.txtDepartureValue, this.txtDeparture);
                    // 默认到达城市
                    bindDefaultAirportInfo(workingSetting.DefaultArrival, this.txtArrivalValue, this.txtArrival);
                }
                // 默认出发日期为第二天
                this.txtGoDate.Value = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            }
        }
        private void bindDefaultAirportInfo(string code, HtmlInputControl valueControl, HtmlInputControl textControl) {
            if(!string.IsNullOrWhiteSpace(code)) {
                var airport = Service.FoundationService.QueryAirport(code);
                if(airport != null && airport.Location != null) {
                    valueControl.Value = airport.Code.Value;
                    textControl.Value = airport.Location.Name + "[" + airport.Code + "]";
                }
            }
        }

        protected void btnPNRCodeImport_Click(object sender, EventArgs e) {
            bool needImputPat = false;
            try {
                if(this.radChildrenPNR.Checked) {
                   needImputPat = ImportHelper.ImportByPNRCode(this.txtAdultPNRCode.Text.Trim(), this.txtChildrenPNRCode.Text.Trim(), Common.Enums.PassengerType.Child, null, HttpContext.Current).NeedPAT;
                } else {
                   needImputPat = ImportHelper.ImportByPNRCode(this.txtPNRCode.Text.Trim(), string.Empty, Common.Enums.PassengerType.Adult, null, HttpContext.Current).NeedPAT;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "PNR编码导入");
                return;
            }
            if (needImputPat)
            {
                showPatInput();
            }
            else
            {
                RegisterScript("window.top.location='/FlightReserveModule/ChoosePolicy.aspx?source=" + FlightReserveModule.ChoosePolicy.ImportSource + "';");
            }
        }

        private void showPatInput() {
            RegisterScript("");
        }
    }
}