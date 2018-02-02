using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.UserControls {
    public partial class Voyage : System.Web.UI.UserControl {
        private Service.Order.Domain.Order m_order;
        private IEnumerable<Service.Order.Domain.Flight> m_flights;
        private Mode m_mode;

        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                bindFlights();
            }
        }
        public string Tip {
            get { return this.lblTip.Text; }
            set { this.lblTip.Text = value; }
        }
        public void InitData(Service.Order.Domain.Order order, IEnumerable<Service.Order.Domain.Flight> flights) {
            InitData(order, flights, Mode.Normal);
        }
        public void InitData(Service.Order.Domain.Order order, IEnumerable<Service.Order.Domain.Flight> flights, Mode mode) {
            this.m_order = order;
            this.m_flights = flights;
            this.m_mode = mode;
        }

        private void bindFlights() {
            if(this.m_flights != null) {
                this.divFlight.InnerHtml = "<table>" + drawTitle() + drawContent() + "</table>";
            }
        }
        private string drawTitle() {
            var title = "<tr><th>航空公司</th><th>航班号</th><th>机型</th>";
            if(this.m_mode != Mode.Supply) {
                title += "<th>舱位/折扣</th>";
            }
            title += "<th>出发</th><th>到达</th><th>航班日期</th><th>起抵时间</th>";
            if(this.m_mode == Mode.Apply) {
                title += "<th/>";
            }
            title += "</tr>";
            return title;
        }
        private string drawContent() {
            var content = new StringBuilder();
            foreach(var item in this.m_flights) {
                content.Append("<tr>");
                content.AppendFormat("<td>{0}</td>", item.Carrier.Name);
                content.AppendFormat("<td><span class='h'>{0}</span><span>{1}</span></td>", item.Carrier.Code, item.FlightNo);
                content.AppendFormat("<td>{0}</td>", item.AirCraft);
                if(this.m_mode != Mode.Supply) {
                    if(string.IsNullOrWhiteSpace(item.Bunk.Code)) {
                        content.Append("<td>-<br />");
                    } else {
                        content.AppendFormat("<td><label>{0}</label> / <label>{1}</label><br />",
                            item.Bunk.Code, (item.Bunk.Discount * 100).TrimInvaidZero());
                    }
                    content.AppendFormat("<a target='_blank' class='flightEI' href='/Index.aspx?redirectUrl=/SystemSettingModule/Role/AirlineRetreatChangeNew.aspx?Carrier={0}'>退改签</a>" +
                            "<div class='tgq' style='display:none;'>{1}</div></td>",item.Carrier.Code ,getEI(item));
                }
                content.AppendFormat("<td><label>{0}</label> <label>{1}</label><label class='h'>{2}</label> {3}</td>", item.Departure.City, item.Departure.Name, item.Departure.Code,item.DepartureTerminal);
                content.AppendFormat("<td><label>{0}</label> <label>{1}</label><label class='h'>{2}</label> {3}</td>", item.Arrival.City, item.Arrival.Name, item.Arrival.Code,item.ArrivalTerminal);
                content.AppendFormat("<td>{0}</td>", item.TakeoffTime.ToString("yyyy-MM-dd"));
                content.AppendFormat("<td>{0}</td>", item.TakeoffTime.ToString("HH:mm") + "-" + item.LandingTime.ToString("HH:mm"));
                if(this.m_mode == Mode.Apply) {
                    content.AppendFormat("<td><input type='checkbox' value='{0}'/></td>", item.Id);
                }
                content.Append("</tr>");
            }
            return content.ToString();
        }
        private string getEI(Service.Order.Domain.Flight flight) {
            switch(m_order.Product.ProductType) {
                case ProductType.Promotion:
                    if (string.IsNullOrWhiteSpace(m_order.Provider.Product.RefundAndReschedulingProvision.Refund)
                        && string.IsNullOrWhiteSpace(m_order.Provider.Product.RefundAndReschedulingProvision.Scrap)
                        && string.IsNullOrWhiteSpace(m_order.Provider.Product.RefundAndReschedulingProvision.Transfer))
                         return GetBunksRegulations(flight);
                    else
                        return GetGeneralBunkRegulation(m_order.Provider.Product.RefundAndReschedulingProvision);
                case ProductType.Special:
                    if (m_order.IsThirdRelation && BasePage.GetOrderRole(m_order) != OrderRole.Provider)
                        return GetRegulation(m_order.Supplier.Product.RefundAndReschedulingProvision);
                    else if (!m_order.IsThirdRelation)
                        return GetBunkRegulation(m_order.Provider == null ? null : m_order.Provider.Product.RefundAndReschedulingProvision,m_order);
                    else
                        return GetBunksRegulations(flight);
                default:
                   return GetBunksRegulations(flight);
            }
        }

        public static string GetBunkRegulation(RefundAndReschedulingProvision bunk,Service.Order.Domain.Order order)
        {
            if (bunk == null) return string.Empty;
            StringBuilder result = new StringBuilder();
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", bunk.Refund);
            if(!order.IsThirdRelation)
            result.AppendFormat("<p><span class=b>废票规定：</span>{0}</p>", bunk.Scrap);
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", bunk.Alteration);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", bunk.Transfer);
            return result.ToString();
        }

        public static string GetGeneralBunkRegulation(RefundAndReschedulingProvision bunk)
        {
            if (bunk == null) return string.Empty;
            StringBuilder result = new StringBuilder();
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", bunk.Refund);
            result.AppendFormat("<p><span class=b>废票规定：</span>{0}</p>", bunk.Scrap);
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", bunk.Alteration);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", bunk.Transfer);
            return result.ToString();
        }

        public static string GetRegulation(RefundAndReschedulingProvision regulation)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", regulation.Alteration);
            result.AppendFormat("<p><span class=b>作废规定：</span>{0}</p>", regulation.Scrap);
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", regulation.Refund);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", regulation.Transfer);
            return result.ToString();
        }

        public static string GetRegulations(string refundRegulation, string changeRegulation, string endorseRegulation, string remarks)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", changeRegulation);
          //  result.AppendFormat("<p><span class=b>作废规定：</span>{0}</p>", changeRegulation);
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", refundRegulation);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", endorseRegulation);
            return result.ToString();
        }

        public static string GetBunksRegulations(Service.Order.Domain.Flight flight)
        {
            Regex eiTemplate = new Regex("退票规定:(?<RefundRegulation>.*?)改签规定:(?<ChangeRegulation>.*?)签转规定:(?<EndorseRegulation>.*?)备注:(?<Remarks>.*)");
            var bunkEI = eiTemplate.Match(flight.Bunk.EI);
            if (bunkEI.Success)
            {
                return GetRegulations(bunkEI.Groups["RefundRegulation"].Value, bunkEI.Groups["ChangeRegulation"].Value,
                    bunkEI.Groups["EndorseRegulation"].Value, bunkEI.Groups["Remarks"].Value);
            }
            return flight.Bunk.EI;
        }
    }
}