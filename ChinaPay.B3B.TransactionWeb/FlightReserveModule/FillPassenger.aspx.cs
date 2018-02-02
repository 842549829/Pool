using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.FlightQuery;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public partial class FillPassenger : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");

            if(!IsPostBack) {
                FlightHandlers.FlightQuery.ClearFlightQuerySession();
                saveParameters();
                if(Request.QueryString["source"] == ChoosePolicy.UpgradeByQueryFlightSource) {
                    try {
                        commitPassengerInfos();
                        Response.Redirect(string.Format("/FlightReserveModule/ChoosePolicyWithUpgrade.aspx?source={0}&orderId={1}&provider={2}",
                            ChoosePolicy.UpgradeByQueryFlightSource, Request.QueryString["orderId"], Request.QueryString["provider"]));
                    } catch {
                        RegisterScript("alert('订座失败，请重新操作');window.location='/Index.aspx?redirectUrl=/OrderModule/Purchase/Apply.aspx?id=" + Request.QueryString["orderId"] + "';");
                        //form1.Visible = false;
                    }
                } else {
                    bindData();
                    Response.Buffer = true;
                    Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                    Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                    Response.Expires = 0;
                    Response.CacheControl = "no-cache";
                    Response.Cache.SetNoStore();
                }
            }
        }
        private void bindData() {
            var policy = getPolicy();
            var flights = getFlights();
            bindFlights(flights, getPolicy());
            bindTickets(flights, policy);
            bindPassengerInfo(flights);
            this.hidSeatCount.Value = flights.Min(item => item.SeatCount).ToString();
            bindDefaultContact();
        }
        private IEnumerable<DataTransferObject.FlightQuery.FlightView> getFlights() {
            return Session["ReservedFlights"] as IEnumerable<DataTransferObject.FlightQuery.FlightView>;
        }
        private DataTransferObject.FlightQuery.PolicyView getPolicy() {
            return Session["FlightPolicy"] as DataTransferObject.FlightQuery.PolicyView;
        }
        private void saveParameters() {
            var flightsArgs = Request.Form["flightsArgs"];
            var policyArgs = Request.Form["policyArgs"];
            if(string.IsNullOrWhiteSpace(flightsArgs) || string.IsNullOrWhiteSpace(policyArgs)) {
                var formParameters = Request.Form.AllKeys.Join(",", p => p + ":" + Request.Form[p]);
                Service.LogService.SaveTextLog("填写乘机人页面参数信息错误：Get参数 " + Request.QueryString + " Post参数 " + formParameters);
                RegisterScript("alert('参数信息错误，请重新查询');window.location='/PurchaseDefault.aspx'", true);
            }
            flightsArgs = flightsArgs.Replace("lt;", "<").Replace("rt;", ">");
            var flights = (from arg in flightsArgs.Split('$')
                           let flightView = DataTransferObject.FlightQuery.FlightView.Parse(arg)
                           where flightView != null
                           select flightView).ToList();
            var policy = DataTransferObject.FlightQuery.PolicyView.Parse(policyArgs);
            Session["ReservedFlights"] = flights;
            Session["FlightPolicy"] = policy;
        }
        private void bindFlights(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights,PolicyView policy) {
            ucFlights.InitData(true, flights,policy);
        }
        private void bindTickets(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights, DataTransferObject.FlightQuery.PolicyView policy) {
            var ticketsHTML = new StringBuilder();
            var flight = flights.Last();
            ticketsHTML.Append("<table><tr><th>票面价</th>");
            // 特殊票时，是没返点的
            if(flight.Rebate.HasValue) {
                ticketsHTML.Append("<th>返点</th>");
            }
            ticketsHTML.Append("<th>结算价</th><th>民航基金/燃油税</th><th>预计总价</th></tr>");
            // 往返产品舱时，单独处理
            if(policy.Type == PolicyType.Bargain && flight.BunkType == BunkType.Production) {
                // 由于往返产品舱的价格,去程和回程是在一起的，所以除了民航基金和燃油是在各航班上，其他都是取最后一程的即可
                ticketsHTML.AppendFormat("<td>{0}</td>", flight.Fare.TrimInvaidZero());
                if(flight.Rebate.HasValue) {
                    ticketsHTML.AppendFormat("<td>{0}%</td>", (flight.Rebate.Value * 100).TrimInvaidZero());
                }
                ticketsHTML.AppendFormat("<td>{0}</td>", flight.SettleAmount.TrimInvaidZero());
                ticketsHTML.AppendFormat("<td>{0}/{1}</td>", flights.Sum(item => item.AirportFee).TrimInvaidZero(), flights.Sum(item => item.BAF).TrimInvaidZero());
                ticketsHTML.AppendFormat("<td>{0}</td>", (flights.Sum(item => item.AirportFee + item.BAF) + flight.SettleAmount).TrimInvaidZero());
                ticketsHTML.Append("</tr>");
            } else {
                var isShowPrice = Request.QueryString["ShowPrice"]==null||Request.QueryString["ShowPrice"]=="true";
                var isFreePolicy = flight.BunkType != null && flight.YBPrice == 0 && flight.BunkType.Value == BunkType.Free;

                ticketsHTML.AppendFormat("<td>{0}</td>", !isShowPrice ? "出票后可见" : isFreePolicy ? "0" : flights.Sum(item => item.Fare).TrimInvaidZero());
                if(flight.Rebate.HasValue) {
                    // 返点始终取最后一程的返点
                    ticketsHTML.AppendFormat("<td>{0}%</td>", (flight.Rebate.Value * 100).TrimInvaidZero());
                }
                var settleAmount = getSettleAmount(flights);
                ticketsHTML.AppendFormat("<td>{0}</td>", settleAmount.TrimInvaidZero());
                ticketsHTML.AppendFormat("<td>{0}/{1}</td>", flights.Sum(item => item.AirportFee).TrimInvaidZero(), flights.Sum(item => item.BAF).TrimInvaidZero());
                ticketsHTML.AppendFormat("<td>{0}</td>", flights.Sum(item => item.AirportFee + item.BAF) + settleAmount);
                ticketsHTML.Append("</tr>");
            }
            ticketsHTML.Append("</table>");
            divTickets.InnerHtml = ticketsHTML.ToString();
        }
        private void bindPassengerInfo(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights) {
            ddlPassengerType.Items.Clear();
            ddlPassengerType.Items.Add(new ListItem("成人", ((int)PassengerType.Adult).ToString()));
            if(childrenOrderable(flights)) {
                ddlPassengerType.Items.Add(new ListItem("儿童", ((int)PassengerType.Child).ToString()));
            }
        }
        private void bindDefaultContact() {
            txtContact.Text = CurrentCompany.Contact;
            txtMobile.Text = CurrentCompany.ContactPhone;
            txtEmail.Text = CurrentCompany.ContactEmail;
        }
        private decimal getSettleAmount(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights) {
            if(flights.Count() > 1) {
                var rebate = flights.Last().Rebate;
                if(rebate.HasValue) {
                    return flights.Sum(item => item.Fare - Utility.Calculator.Round(item.Fare * rebate.Value, -2));
                } else {
                    return flights.Sum(item => item.Fare);
                }
            }
            return flights.Sum(item => item.SettleAmount);
        }
        private bool childrenOrderable(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            return getPolicy().Type != PolicyType.Special && flights.All(item => item.SuportChild);
        }

        private void commitPassengerInfos() {
            var passengers = Session["UpgradePassengers"] as IEnumerable<PassengerView>;
            var flights = getFlights();
            var applyform = Session["UpgradeApplyformView"] as UpgradeApplyformView;
            applyform.PNRSource = OrderSource.PlatformOrder;
            foreach(var item in applyform.Items) {
                item.Flight = ReserveViewConstuctor.GetOrderFlightView(flights.First(f => f.Departure.Code == item.Flight.Departure && f.Arrival.Code == item.Flight.Arrival));
            }
            applyform.NewPNR = PNRHelper.ReserveSeat(flights, passengers);
            if(PNRHelper.RequirePat(flights, PolicyType.Normal)) {
                applyform.PATPrice = PNRHelper.Pat(applyform.NewPNR, flights, passengers.First().PassengerType);
            }
            Session.Remove("UpgradePassengers");
            Session.Remove("UpgradeApplyformView");
            Session["ApplyformView"] = applyform;
            Session["Passengers"] = passengers;
        }
    }
}