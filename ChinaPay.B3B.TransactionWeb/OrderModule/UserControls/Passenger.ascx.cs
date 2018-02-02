using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.UserControls
{
    public partial class Passenger : System.Web.UI.UserControl
    {
        private Service.Order.Domain.Order m_order;
        private IEnumerable<Service.Order.Domain.Passenger> m_passengers;
        private Mode m_mode;
        private string m_ETDZCondition;
        private IEnumerable<Flight> m_Flgiht;
        private OrderRole _CurrentOrderRole;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindPassengers();
            }
        }
        public string Tip
        {
            get
            {
                return this.lblTip.Text;
            }
            set
            {
                this.lblTip.Text = value;
            }
        }
        public void InitData(Order order, IEnumerable<Service.Order.Domain.Passenger> passengers, IEnumerable<Flight> flights)
        {
            InitData(order, passengers, Mode.Normal,flights);
        }
        public void InitData(Order order, IEnumerable<Service.Order.Domain.Passenger> passengers, Mode mode, IEnumerable<Flight> flights, bool showServiceCharge = false)
        {
            this.m_order = order;
            this.m_passengers = passengers;
            this.m_mode = mode;
            this.m_ETDZCondition = order.IsThirdRelation ? order.Supplier.Product.Condition:order.Provider.Product.Condition;
            m_Flgiht = flights;
            this.showServiceCharge = showServiceCharge;
        }
        private void bindPassengers()
        {
            if (this.m_order != null && this.m_passengers != null)
            {
                _CurrentOrderRole = BasePage.GetOrderRole(m_order);
                var passengersInfo = "<table>" + drawTitle() + drawContents() + "</table>";
                if (this.m_mode == Mode.Itinerary)
                {
                    passengersInfo += "<div><input type='button' value='打印行程告知单' class='btn class1' style='float:right;'/></div>";
                }
                if (!string.IsNullOrEmpty(m_ETDZCondition) && _CurrentOrderRole != OrderRole.Provider) passengersInfo +=
                "<div id=\"Condition\"><h3 class=\"titleBg\"><span >出票条件</span></h3></div><div ><div>" + m_ETDZCondition + "</div></div>";
                this.divPassengers.InnerHtml = passengersInfo;
            }
        }
        private string drawTitle()
        {
            string title ="<tr><th>乘机人姓名</th><th>乘机人类型</th>";
            if (OrderRole.Platform == _CurrentOrderRole || OrderRole.Purchaser == _CurrentOrderRole)
            {
                title += "<th>联系方式</th>";
            }
            title += "<th>证件号</th>";
            if (isShowTicketNo())
            {
                title += "<th>票号</th>";
            }
            title += "<th>票面价</th>";
            if (this.m_mode != Mode.Supply)
            {
                if (isShowServiceCharge())
                {
                    title += "<th>服务费</th>";
                }
                if (isShowRebate())
                {
                    title += "<th>返点</th>";
                }
                else if (isShowAllRebate())
                {
                    title += "<th>卖出返点</th><th>买入返点</th>";
                }
            }
            title += "<th>民航基金/燃油税</th><th>总价</th>";
            if (this.m_mode == Mode.Apply)
            {
                title += "<th/>";
            }
            else if (this.m_mode == Mode.ETDZ)
            {
                title += "<th>票号</th>";
            }
            if (this.m_mode == Mode.Itinerary)
            {
                title += "<th><input type='checkbox' value='{0}'/></th>";
            }
            title += "</tr>";
            return title;
        }
        private string drawContents()
        {
            if (this.m_order != null && this.m_passengers != null)
            {
                var content = new StringBuilder();
                int index = 1;
                foreach (var item in this.m_passengers)
                {
                    content.Append("<tr>");
                    content.AppendFormat("<td>{0}</td>", item.Name);
                    content.AppendFormat("<td><span>{0}</span><span class='h' style='display:none'>{1}</span><span class='h' style='display:none'></span></td>", item.PassengerType.GetDescription(), (int)item.PassengerType);
                    if (OrderRole.Platform == _CurrentOrderRole || OrderRole.Purchaser == _CurrentOrderRole)
                    {
                        content.AppendFormat("<td>{0}</td>",item.Phone);
                    }
                    content.AppendFormat("<td><span>{0}</span><span class='h' style='display:none'>{1}</span></td>", item.Credentials, (int)item.CredentialsType);
                    if (isShowTicketNo())
                    {
                    //    content.AppendFormat("<td><span id='NO{2}'>{0}</span> {1}</td>", 
                    //        FromatTicket(item.Tickets, item.Tickets.Count() > 1), 
                    //        _CurrentOrderRole == OrderRole.Provider && this.Page is Provide.OrderDetail ? 
                    //        "<a href='javascript:EditTick(\"NO" + index + "\")'>修改</a>" : string.Empty, index);
                        content.AppendFormat("<td>{0}</td>", FromatTicket(item.Tickets,_CurrentOrderRole == OrderRole.Provider && this.Page is Provide.OrderDetail));
                    }
                    if (this.m_mode == Mode.Supply)
                    {
                        content.AppendFormat("<td><span class='price releasedFare'>{0}</span><input type='text' class='text newFare' style='display:none;'/></td>",
                            getReleasedFare(item).TrimInvaidZero());
                    }
                    else
                    {
                        content.AppendFormat("<td><span class='ticketPrice'>{0}</span></td>", m_Flgiht.Sum(f=>f.Price.Fare).TrimInvaidZero());
                        if (isShowServiceCharge())
                        {
                            content.AppendFormat("<td><span>{0}</span></td>", getServiceCharge(item).TrimInvaidZero());
                        }
                        if (isShowRebate())
                        {
                            content.AppendFormat("<td>{0}%</td>", (getRebate() * 100).TrimInvaidZero());
                        }
                        else if (isShowAllRebate())
                        {
                            content.AppendFormat("<td>{0}%</td>", (getProvideRebate() * 100).TrimInvaidZero());
                            content.AppendFormat("<td>{0}%</td>", (getPurchaseRebate() * 100).TrimInvaidZero());
                        }
                    }
                    content.AppendFormat("<td><span class='price'>{0}</span> / <span class='price'>{1}</span></td>", m_Flgiht.Sum(f=>f.AirportFee).TrimInvaidZero(),m_Flgiht.Sum(f=>f.BAF).TrimInvaidZero());
                    content.AppendFormat("<td><span class='price zongjia'>{0}</span></td>", (m_Flgiht.Sum(f => f.Price.Total) + (isShowServiceCharge() ? getServiceCharge(item) : 0)).TrimInvaidZero());
                    if (this.m_mode == Mode.Apply||m_mode==Mode.Itinerary)
                    {
                        content.AppendFormat("<td><input type='checkbox' value='{0}'/></td>", item.Id);
                    }
                    else if (this.m_mode == Mode.ETDZ)
                    {
                        content.AppendFormat("<td><input type='text' class='text settleCode' value='{0}' /> <input type='text' class='text ticketNo parentTicketNo'/> {1}</td>",
                            item.Tickets.First().SettleCode,
                            item.Tickets.Count() > 1 ? "<span>-</span> <input type='text' class='text ticketNOend' /><span class='addTicketNo'>+</span>" : string.Empty);
                        //content.Append("<td>");
                        //for (int i = 0; i < item.Tickets.Count(); i++)
                        //{
                        //    content.Append("<input type='text' class='text ticketNo'/><br/>");
                        //}
                        // content.Append("</td>");
                   }
                    content.Append("</tr>");
                    index++;
                }
                return content.ToString();
            }
            return string.Empty;
        }

        private string FromatTicket(IEnumerable<Ticket> tickets,bool isModify) {
            //var firstTicket = tickets.First();
            //是否连续票号
            //bool isSeries = multiTickets && (Convert.ToInt32(tickets.Last().No.Substring(8))  -  Convert.ToInt32(tickets.First().No.Substring(8)) + 1 == tickets.Count());
            //if (multiTickets)
            //{
            //    return string.Format("<span id='{1}'>{0}-{1}{2}</span>{3}",
            //            firstTicket.SettleCode,
            //            firstTicket.No,
            //            multiTickets ? "-" + tickets.Last().No.Substring(8) : string.Empty,
            //            isModify ? "<a href='javascript:EditTick(" + firstTicket.No + ")'>修改</a>" : string.Empty
            //            );
            //}
            //else {
            StringBuilder sb = new StringBuilder();
            foreach (var item in tickets)
            {
                sb.AppendFormat("<span id='{1}'>{0}-{1}</span>{2}",
                    item.SettleCode,
                    item.No,
                    isModify ? "<a href='javascript:EditTick("+item.No+")'>修改</a>" : string.Empty);
                sb.Append("</br>");
            }
            return sb.ToString();
            //}
        }

        private decimal getServiceCharge(Service.Order.Domain.Passenger passenger)
        {
            var result = 0M;
            bool showIncreasing = _CurrentOrderRole == OrderRole.Purchaser;
            if (this.m_order.IsSpecial)
            {
                result += (from ticket in passenger.Tickets
                           from flight in ticket.Flights
                           where flight.Bunk is Service.Order.Domain.Bunk.SpecialBunk
                           select (flight.Bunk as Service.Order.Domain.Bunk.SpecialBunk).ServiceCharge + (showIncreasing?flight.Increasing:0)).Sum();
            }
            return result;
        }
        private decimal getReleasedFare(Service.Order.Domain.Passenger passenger)
        {
            var result = 0M;
            if (this.m_order.IsSpecial)
            {
                result += (from ticket in passenger.Tickets
                           from flight in ticket.Flights
                           where flight.Bunk is Service.Order.Domain.Bunk.SpecialBunk
                           select (flight.Bunk as Service.Order.Domain.Bunk.SpecialBunk).ReleasedFare).Sum();
            }
            return result;
        }
        private decimal getRebate()
        {
            if (!m_order.IsSpecial && BasePage.LogonCompany.CompanyId == m_order.Purchaser.CompanyId)
                return getPurchaseRebate();
            if (m_order.Provider != null && BasePage.LogonCompany.CompanyId == m_order.Provider.CompanyId)
                return getProvideRebate();
            if (m_order.Supplier != null && BasePage.LogonCompany.CompanyId == m_order.Supplier.CompanyId)
                return getSupplyRebate();
            return 0;
        }
        private decimal getPurchaseRebate()
        {
            return this.m_order.Purchaser.Rebate;
        }
        private decimal getProvideRebate()
        {
            return this.m_order.Provider == null ? 0 : this.m_order.Provider.Rebate;
        }
        private decimal getSupplyRebate()
        {
            return this.m_order.Supplier == null ? 0 : this.m_order.Supplier.Rebate;
        }
        private bool isShowRebate()
        {
            if (!m_order.IsSpecial && BasePage.LogonCompany.CompanyId == m_order.Purchaser.CompanyId)
                return true;
            if (m_order.Provider != null && BasePage.LogonCompany.CompanyId == m_order.Provider.CompanyId)
                return true;
            if (m_order.Supplier != null && BasePage.LogonCompany.CompanyId == m_order.Supplier.CompanyId)
                return true;
            return false;
        }
        private bool isShowAllRebate()
        {
            return BasePage.LogonCompany.CompanyType == Common.Enums.CompanyType.Platform;
        }
        private bool isShowTicketNo()
        {
            return this.m_order.Status == DataTransferObject.Order.OrderStatus.Finished;
        }
        private bool isShowServiceCharge()
        {
            if (this.m_order.IsSpecial)
            {
                if (this.m_order.IsThirdRelation && BasePage.LogonCompany.CompanyType == CompanyType.Provider||showServiceCharge)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool showServiceCharge { get; set; }
    }
}