using System;
using System.Linq;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule {
    public partial class UpdateCredentials : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                var id = Request.QueryString["id"];
                decimal orderId;
                if(decimal.TryParse(id, out orderId)) {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if(order != null) {
                        this.hidOrderId.Value = id;
                        bindPassengers(order);
                        return;
                    }
                }
                this.divError.Visible = true;
                this.divError.InnerHtml = "<h2>订单不存在</h2>";
                form1.Visible = false;
            }
        }
        private void bindPassengers(Service.Order.Domain.Order order) {
            var passengers = from pnr in order.PNRInfos
                             from passenger in pnr.Passengers
                             select new
                             {
                                 Name = passenger.Name,
                                 Type = passenger.PassengerType.GetDescription(),
                                 Credentials = passenger.Credentials
                             };
            this.passengerContents.DataSource = passengers;
            this.passengerContents.DataBind();
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
    }
}