using System;
using System.Linq;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule
{
    public partial class FlightQueryNew : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitSearchData();
            }
        }

        private void InitSearchData()
        {
            string departureCode = Request.QueryString["departure"];
            string arrivalCode = Request.QueryString["arrival"];
            if (Page is FlightQueryBackResult)
            {
                arrivalCode = departureCode;
                departureCode = Request.QueryString["arrival"];
            }
            Airport departure = FoundationService.Airports.FirstOrDefault(p => String.Compare(p.Code.Value, departureCode, StringComparison.OrdinalIgnoreCase) == 0);
            Airport arrival = FoundationService.Airports.FirstOrDefault(p => String.Compare(p.Code.Value, arrivalCode, StringComparison.OrdinalIgnoreCase) == 0);
            string goDate = Request.QueryString["goDate"];
            string backDate = Request.QueryString["backDate"];
            string airLineCode = Request.QueryString["goDate"];
            Airline airLine = FoundationService.Airlines.FirstOrDefault(p => p.Code.Equals(airLineCode));

            string registerOption =
                string.Format(
                    "var searchOption = {{departureCode:'{0}',departure:'{1}',arrivalCode:'{2}',arrival:'{3}',goDate:'{4}',backDate:'{5}',airLineCode:'{6}',airLine:'{7}'}}"
                    , departureCode, departure == null ? string.Empty : departure.Location.Name + "[" + departureCode + "]",
                    arrivalCode, arrival == null ? string.Empty : arrival.Location.Name + "[" + arrivalCode + "]",
                    goDate, backDate, airLineCode, airLine == null ? string.Empty : airLine.Name
                    );
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "parm", registerOption, true);
        }
    }
}