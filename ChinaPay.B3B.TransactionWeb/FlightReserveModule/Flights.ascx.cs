using System;
using System.Collections.Generic;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.FlightQuery;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public partial class Flights : System.Web.UI.UserControl {
        IEnumerable<DataTransferObject.FlightQuery.FlightView> m_flights;
        private bool m_showEI;
        private PolicyView m_policy;
        private bool m_IsSpecial {
            get {
                return m_policy != null && m_policy.Type == PolicyType.Special;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                bindFlights();
            }
        }
        public void InitData(bool showEI, IEnumerable<DataTransferObject.FlightQuery.FlightView> flights,PolicyView policy) {
            this.m_showEI = showEI;
            this.m_flights = flights;
            this.m_policy = policy;
        }
        private void bindFlights() {
            var flightsHTML = new StringBuilder();
            flightsHTML.Append("<table><tr><th>行程</th><th>航空公司</th><th>航班号</th><th>机型</th><th>舱位</th><th>出发</th><th>到达</th></tr>");
            if(m_flights != null) {
                foreach(var item in m_flights) {
                    flightsHTML.Append("<tr>");
                    flightsHTML.AppendFormat("<td>第{0}程</td>", getSerial(item.Serial));
                    flightsHTML.AppendFormat("<td>{0}</td>", item.AirlineName);
                    flightsHTML.AppendFormat("<td>{0}</td>", item.AirlineCode + item.FlightNo);
                    flightsHTML.AppendFormat("<td>{0}</td>", item.Aircraft);
                    flightsHTML.AppendFormat("<td><div class='el-block inline'>{0}", item.BunkCode);
                    if(item.Discount.HasValue&&item.RenderDiscount.HasValue) {
                        flightsHTML.AppendFormat("/{0}", ((m_IsSpecial?item.RenderDiscount.Value:item.Discount.Value) * 100).TrimInvaidZero());
                    }
                    flightsHTML.Append("</div>");
                    if(m_showEI) {
                        flightsHTML.AppendFormat("<a target='_blank' href='/Index.aspx?redirectUrl=/SystemSettingModule/Role/AirlineRetreatChangeNew.aspx?Carrier={0}' class='el-text flightEI'", item.AirlineCode);
                        flightsHTML.AppendFormat("'>退改签</a><div class='tgq'>{0}</div>", item.EI);
                    }
                    flightsHTML.Append("</td>");
                    flightsHTML.AppendFormat("<td>{0} {1}{2} {3}</td>", item.Departure.Time.ToString("yyyy年MM月dd日 HH:mm"), item.Departure.City, item.Departure.Name,item.Departure.Terminal);
                    flightsHTML.AppendFormat("<td>{0} {1}{2} {3}</td>", item.Arrival.Time.ToString("yyyy年MM月dd日 HH:mm"), item.Arrival.City, item.Arrival.Name,item.Arrival.Terminal);
                    flightsHTML.Append("</tr>");
                }
            }
            flightsHTML.Append("</table>");
            this.divFlights.InnerHtml = flightsHTML.ToString();
        }
        private string getSerial(int serial) {
            if(serial == 1) {
                return "一";
            } else if(serial == 2) {
                return "二";
            } else if(serial == 3) {
                return "三";
            } else if(serial == 4) {
                return "四";
            } else if(serial == 5) {
                return "五";
            } else if(serial == 6) {
                return "六";
            } else if(serial == 7) {
                return "七";
            } else if(serial == 8) {
                return "八";
            } else if(serial == 9) {
                return "九";
            }
            return serial.ToString();
        }
    }
}