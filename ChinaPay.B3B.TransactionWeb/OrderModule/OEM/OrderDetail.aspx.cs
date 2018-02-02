using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.TransactionWeb.OrderModule.UserControls;
using System.Web.UI.HtmlControls;


namespace ChinaPay.B3B.TransactionWeb.OrderModule.OEM
{
    public partial class OrderDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");

            if (!IsPostBack)
            {
                setBackButton();
                var id = Request.QueryString["id"];
                decimal orderId;
                if (decimal.TryParse(id, out orderId))
                {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if (order == null)
                    {
                        showErrorMessage("订单不存在");
                    }
                    else
                    {
                        bindOrder(order);
                        setButtons(order);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindOrder(Service.Order.Domain.Order order)
        {
            bindOrderHeader(order);
            bindPNRGroups(order);
            bindOrderBills(order);
            bindOrderContactInfo(order.Contact);
        }
        private void bindOrderHeader(Service.Order.Domain.Order order)
        {
            this.lblOrderId.Text = order.Id.ToString();
            this.lblStatus.Text = Service.Order.StatusService.GetOrderStatus(order.Status, DataTransferObject.Order.OrderRole.Purchaser);
            this.lblAmount.Text = order.Purchaser.Amount.ToString("F2");
            var product = order.IsThirdRelation ? order.Supplier.Product : order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = order.Product.ProductType.GetDescription();
            }
            if (order.Provider != null && order.Provider.Product is Service.Order.Domain.CommonProductInfo)
            {
                this.lblTicketType.Text = (order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            }
            else
            {
                this.lblTicketType.Text = "-";
            }
            this.lblOriginalOrderId.Text = order.AssociateOrderId.HasValue ? order.AssociateOrderId.Value.ToString() : "-";
            this.lblProducedTime.Text = order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblPayTime.Text = order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            this.lblETDZTime.Text = order.ETDZTime.HasValue ? order.ETDZTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            lblProducter.Text = order.Purchaser.OperatorAccount;
            switch (order.Status)
            {
                case OrderStatus.ConfirmFailed:
                case OrderStatus.DeniedWithSupply:
                case OrderStatus.DeniedWithETDZ:
                case OrderStatus.Canceled:
                    this.divFailed.Visible = true;
                    this.lblFailedReason.Text = order.Remark;
                    break;
            }
        }
        private void bindPNRGroups(Service.Order.Domain.Order order)
        {
            bool IsShowPNR = order.Source == OrderSource.CodeImport ||
                order.Source == OrderSource.ContentImport ||
                order.Source == OrderSource.InterfaceOrder ||
                (order.Status > OrderStatus.PaidForSupply && order.Status != OrderStatus.Canceled);
            divPNRGroups.Visible = true;
            foreach (var item in order.PNRInfos)
            {
                var pnrInfo = LoadControl(ResolveUrl("~/OrderModule/UserControls/PNRInfo.ascx")) as OrderModule.UserControls.PNRInfo;
                pnrInfo.ShowPNR = IsShowPNR;
                if (BasePage.GetOrderRole(order) == OrderRole.Purchaser && order.Status == OrderStatus.Finished)
                {
                    pnrInfo.InitData(order, item, Mode.Itinerary);
                }
                else
                {
                    pnrInfo.InitData(order, item);
                }
                this.divPNRGroups.Controls.Add(pnrInfo);
                hidRenderPrice.Value = item.Flights.Any() && item.Flights.First().Bunk is FreeBunk || item.Flights.Any() && item.Flights.First().Price.Fare != 0 ? "1" : "0";
            }
        }
        private void bindOrderBills(Service.Order.Domain.Order order)
        {
            switch (order.Status)
            {
                case OrderStatus.PaidForSupply:
                case OrderStatus.DeniedWithSupply:
                case OrderStatus.PaidForETDZ:
                case OrderStatus.DeniedWithETDZ:
                case OrderStatus.Canceled:
                case OrderStatus.Finished:
                    this.bill.InitData(order.Bill);
                    this.bill.Visible = true;
                    break;
                default:
                    this.bill.Visible = false;
                    break;
            }
        }
        private void bindOrderContactInfo(Contact contact)
        {
            this.lblContact.Text = contact.Name;
            this.lblContactMobile.Text = contact.Mobile;
            this.lblContactEmail.Text = contact.Email;
        }
        private void setButtons(Service.Order.Domain.Order order)
        {
            if (order.Status == OrderStatus.Finished)
            {
                //// 订单历史记录
                //setRequestUrl("../OrderHistoryRecord.aspx", order.Id, this.btnOrderHistory);
                //// 进行中的申请
                //setRequestUrl("../ProcessingApplyform.aspx", order.Id, this.btnProcessingApplyforms);
                //// 申请退改签
                //setRequestUrl("Apply.aspx", order.Id, this.btnApply);
            }
            else if (order.Status == OrderStatus.Ordered)
            {
                //setRequestUrl("OrderPay.aspx", order.Id, this.btnPay);
            }
        }
        private void setBackButton()
        {
            // 返回
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "OrderList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void setRequestUrl(string targetPage, decimal orderId, HtmlButton sender)
        {
            var requestUrl = string.Format("{0}?id={1}&returnUrl={2}&role=Purchase", targetPage, orderId.ToString(), HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            sender.Attributes.Add("onclick", "window.location.href='" + requestUrl + "';");
            sender.Visible = true;
        }
        private void showErrorMessage(string message)
        {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }
    }
}