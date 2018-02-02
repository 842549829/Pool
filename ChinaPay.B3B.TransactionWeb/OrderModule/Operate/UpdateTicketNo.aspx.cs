using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate {
    public partial class UpdateTicketNo : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                var id = Request.QueryString["id"];
                decimal orderId;
                if(decimal.TryParse(id, out orderId)) {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if(order == null) {
                        showErrorMessage("订单不存在");
                    }else{
                        this.hidOrderId.Value = id;
                        bindTickets(order);
                    }
                } else {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindTickets(Service.Order.Domain.Order order) {
            var tickets = from pnr in order.PNRInfos
                          from passenger in pnr.Passengers
                          from ticket in passenger.Tickets
                          select new
                          {
                              Name = passenger.Name,
                              Type = passenger.PassengerType.GetDescription(),
                              Credentials = passenger.Credentials,
                              SettleCode = ticket.SettleCode,
                              Original = ticket.No
                          };
            this.ticketContents.DataSource = tickets;
            this.ticketContents.DataBind();
        }

        private string FromatTicket(IEnumerable<Ticket> tickets, bool multiTickets)
        {
            var firstTicket = tickets.First();
            return string.Format("{0}-{1}{2}", firstTicket.SettleCode, firstTicket.No, multiTickets ? "-" + tickets.Last().No.Substring(8) : string.Empty);
        }

        private void setBackButton() {
            // 返回
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void showErrorMessage(string message) {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }
    }
}