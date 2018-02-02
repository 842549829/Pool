using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using System.Web.UI.HtmlControls;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase
{
    public partial class RefundApplyformDetail : BasePage
    {
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";
        private readonly List<PNRPair> RenderedPNR = new List<PNRPair>();
        decimal applyformId;

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    if (applyform == null)
                    {
                        showErrorMessage("退/废票申请单不存在");
                    }
                    else
                    {
                        if (applyform.RefundBill != null)
                        {
                            var fare = applyform.RefundBill.Purchaser.Amount;
                            BalanceRefundError.Attributes.Add("add", applyformId + "|" + fare);
                        }
                        var applyerAccount = CurrentUser.Name;
                        var attachment = ApplyformQueryService.QueryApplyAttachmentView(applyform.Id);
                        if (attachment.Any())
                        {
                            ucOutPutImage.IsPlatform = false;
                            ucOutPutImage.ApplyAttachment = attachment;
                        }
                        else
                        {
                            divApplyAttachment.Visible = false;
                        }
                        setButton(applyform);
                        bindData(applyform);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }

        private void setButton(RefundOrScrapApplyform applyform)
        {
            var order = applyform.Order;
            var isComplete = applyform.ProcessStatus.GetDescription();
            var isRefund = applyform.Status.GetDescription();
            var isSubmit = applyform.HasBalanceRefund;
            var form = applyform as RefundApplyform;
            if (form != null && (form.RefundType==RefundType.SpecialReason||form.RefundType==RefundType.Voluntary)&& isComplete == "已处理" && isRefund == "已退/废票" && !applyform.HasBalanceRefund)
            {
                BalanceRefundError.Visible = true;
            }
            else
            {
                BalanceRefundError.Visible = false;
            }
            // 订单历史记录
            setRequestUrl("../OrderHistoryRecord.aspx", order.Id, this.btnOrderHistory);
            // 进行中的申请
            setRequestUrl("../ProcessingApplyform.aspx", order.Id, this.btnProcessingApplyforms);
        }

        private void setRequestUrl(string targetPage, decimal orderId, System.Web.UI.WebControls.LinkButton sender)
        {
            var requestUrl = string.Format("{0}?id={1}&returnUrl={2}&role=Operate", targetPage, orderId.ToString(), HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            sender.Attributes.Add("onclick", "window.location.href='" + requestUrl + "';return false;");
            sender.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    var remark = this.txtErrorRemork.Text;
                    OrderProcessService.Apply(applyform.OrderId, new BalanceRefundApplyView(applyform.Flights.Select(f=>f.OriginalFlight.Id),applyformId,remark), CurrentUser, OwnerOEMId);
                    ReleaseLock(applyform.OrderId);
                    RegisterScript(this, "alert('差错退款申请成功！');location.href='" + ReturnUrl + "'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "差错退款");
            }
        }

        private void bindData(RefundOrScrapApplyform applyform)
        {
            bindHeader(applyform);
            bindVoyages(applyform);
            bindPassengers(applyform);
            bindApplyAndProcessInfo(applyform);
            bindApplyFormBill(applyform);
            bindOrderContactInfo(applyform);
        }

        private void bindOrderContactInfo(RefundOrScrapApplyform applyform)
        {
            var orderId = applyform.Id;
            var order = Service.OrderQueryService.QueryOrder(orderId);
            if (order != null)
            {
                var contact = order.Contact;
                this.lblContact.Text = contact.Name;
                this.lblContactMobile.Text = contact.Mobile;
                this.lblContactEmail.Text = contact.Email;
            }
        }

        private void bindApplyFormBill(RefundOrScrapApplyform applyform) {
            switch (applyform.Status)
            {
                case  RefundApplyformStatus.Refunded:
                    bill.Visible = true;
                    bill.InitData(applyform.RefundBill);

                    break;
                    default:
                    bill.Visible = false;
                    break;
            }
        }

        private void bindHeader(RefundOrScrapApplyform applyform)
        {
            var refundForm = applyform as RefundApplyform;
            lblApplyformId.Text = applyform.Id.ToString();
            linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            linkOrderId.InnerText = applyform.OrderId.ToString();
            lblApplyType.Text = string.Format("{0} {1}", applyform,
                refundForm != null ? (string.Format("({0})", refundForm.RefundType.GetDescription())) : string.Empty);
            ProductInfo product = applyform.Order.IsThirdRelation ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                lblProductType.Text = applyform.Order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                lblProductType.Text = applyform.Order.Product.ProductType.GetDescription();
            }
            lblStatus.Text = StatusService.GetRefundApplyformStatus(applyform.Status, OrderRole.Purchaser);
            if (applyform.Order.Provider != null && applyform.Order.Provider.Product is CommonProductInfo)
            {
                lblTicketType.Text = (applyform.Order.Provider.Product as CommonProductInfo).TicketType.ToString();
            }
            else
            {
                lblTicketType.Text = "-";
            }

            lblPNR.Text = AppendPNR(applyform.NewPNR, string.Empty);
            lblPNR.Text += AppendPNR(applyform.OriginalPNR, string.IsNullOrWhiteSpace(lblPNR.Text) ? string.Empty : "原编码：");
        }

        private string AppendPNR(PNRPair pnr, string tip)
        {
            if (PNRPair.IsNullOrEmpty(pnr)) return string.Empty;
            if (RenderedPNR.Any(pnr.Equals)) return string.Empty;
            var result = new StringBuilder(" ");
            result.Append(tip);
            if (!string.IsNullOrWhiteSpace(pnr.PNR)) result.AppendFormat(PNRFORMAT, pnr.PNR.ToUpper(), "小");
            result.Append(" ");
            if (!string.IsNullOrWhiteSpace(pnr.BPNR)) result.AppendFormat(PNRFORMAT, pnr.BPNR.ToUpper(), "大");
            RenderedPNR.Add(pnr);
            return result.ToString();
        }


        private void bindVoyages(RefundOrScrapApplyform applyform) { voyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight)); }
        private void bindPassengers(RefundOrScrapApplyform applyform) { passengers.InitData(applyform.Order, applyform.Passengers, applyform.Flights.Select(f => f.OriginalFlight)); }

        private void bindApplyAndProcessInfo(RefundOrScrapApplyform applyform)
        {
            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm:ss");
            lblAppliedReason.Text = applyform.ApplyRemark;
            if (applyform.ProcessedTime.HasValue)
            {
                lblProcessedTime.Text = applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                lblProcessedResult.Text = applyform.ProcessStatus == ApplyformProcessStatus.Finished
                                              ? StatusService.GetRefundApplyformStatus(applyform.Status, GetOrderRole(applyform.Order)) + " " + applyform.ProcessedFailedReason
                                              : string.Empty;
            }
            if (applyform.Status == RefundApplyformStatus.Refunded)
            {
                var refundInfoHTML = new StringBuilder();
                refundInfoHTML.Append("<table><tr><th>航段</th><th>手续费率</th><th>手续费</th><th>退款金额</th></tr>");
                int index = 0;
                var flightRefundFees = applyform.OriginalFlights.Join(applyform.RefundBill.Purchaser.Source.Details, f => f.ReservateFlight, f => f.Flight.Id,
                    (f1, f2) => new {flight = f1, fee = f2});
                foreach (var item in flightRefundFees)
                {
                    refundInfoHTML.Append("<tr>");
                    refundInfoHTML.AppendFormat("<td>{0} - {1}</td>", item.flight.Departure.City, item.flight.Arrival.City);
                    refundInfoHTML.AppendFormat("<td>{0}%</td>", (item.fee.RefundRate*100).TrimInvaidZero());
                    refundInfoHTML.AppendFormat("<td>{0}</td>", Math.Abs(item.fee.RefundFee).TrimInvaidZero());
                    if (index == 0)
                    {
                        refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", flightRefundFees.Count(), applyform.RefundBill.Purchaser.Amount.TrimInvaidZero());
                    }
                    refundInfoHTML.Append("</tr>");
                    index++;
                }
                refundInfoHTML.Append("</table>");
                divRefundFeeInfo.InnerHtml = refundInfoHTML.ToString();
            }
        }

        private void setBackButton()
        {
            string returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "ApplyformList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }

        private void showErrorMessage(string message)
        {
            divError.Visible = true;
            divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        private string ReturnUrl
        {
            get
            {
                var returnUrl = Request.QueryString["returnUrl"];
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = "ApplyformList.aspx";
                }
                if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
                return returnUrl;
            }
        }
    }
}