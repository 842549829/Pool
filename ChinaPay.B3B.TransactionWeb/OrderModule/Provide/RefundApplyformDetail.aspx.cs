using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;
using RefundFlight = ChinaPay.B3B.Service.Order.Domain.Applyform.RefundFlight;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class RefundApplyformDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    var applyform = Service.ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    if (applyform == null)
                    {
                        showErrorMessage("退/废票申请单不存在");
                    }
                    else
                    {
                        var attachment = ChinaPay.B3B.Service.ApplyformQueryService.QueryApplyAttachmentView(applyform.Id);
                        if (attachment.Any())
                        {
                            ucOutPutImage.IsPlatform = false;
                            ucOutPutImage.ApplyAttachment = attachment;
                        }
                        else
                        {
                            divApplyAttachment.Visible = false;
                        }
                        bindData(applyform);
                        setButtons(applyform);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindData(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            bindHeader(applyform);
            bindVoyages(applyform);
            bindPassengers(applyform);
            bindPolicyRemark(applyform);
            bindApplyAndProcessInfo(applyform);
            bindBill(applyform);
        }
        private void bindHeader(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            RefundApplyform refundForm = applyform as RefundApplyform;
            var orderRole = BasePage.GetOrderRole(applyform.Order);
            this.lblApplyformId.Text = applyform.Id.ToString();
            this.linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            this.linkOrderId.InnerText = applyform.OrderId.ToString();
            this.lblApplyType.Text = string.Format("{0} {1}", applyform,
                refundForm != null ? (string.Format("({0})", refundForm.RefundType.GetDescription())) : string.Empty);
            var product = applyform.Order.IsThirdRelation && orderRole!=OrderRole.Provider ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = product.ProductType.GetDescription();
            }
            this.lblStatus.Text = Service.Order.StatusService.GetRefundApplyformStatus(applyform.Status, orderRole);
            if (applyform.Order.Provider != null && applyform.Order.Provider.Product is Service.Order.Domain.CommonProductInfo)
            {
                this.lblTicketType.Text = (applyform.Order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            }
            else
            {
                this.lblTicketType.Text = "-";
            }

            lblPNR.Text = AppendPNR(applyform.NewPNR, string.Empty);
            lblPNR.Text += AppendPNR(applyform.OriginalPNR, string.IsNullOrWhiteSpace(lblPNR.Text) ? string.Empty : "原编码：");

            if (applyform.Order.Provider != null && applyform.Order.Provider.PurchaserRelationType != Common.Enums.RelationType.Brother)
            {
                lblRelation.Text = applyform.Order.Provider.PurchaserRelationType.GetDescription() + "-";
                hrefPurchaseName.InnerHtml = applyform.Order.Purchaser.Company.UserName + "（" + applyform.Order.Purchaser.Name + "）";
                this.hrefPurchaseName.HRef = "/OrganizationModule/RoleModule/ExtendCompanyManage/LowerComapnyInfoUpdate/LowerCompanyDetailInfo.aspx?CompanyId="
                                             + applyform.Order.Purchaser.CompanyId.ToString() +
                                             "&Type=" + (applyform.Order.Provider.PurchaserRelationType == Common.Enums.RelationType.Interior ? "Organization" : "Junion");
            }
            else
            {
                lblRelation.Text = "平台采购";
                hrefPurchaseName.Visible = false;
            }
        }

        List<PNRPair> RenderedPNR = new List<PNRPair>();
        string AppendPNR(PNRPair pnr, string tip)
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
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";


        private void bindVoyages(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.voyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight));
        }
        private void bindPassengers(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.passengers.InitData(applyform.Order, applyform.Passengers,applyform.Flights.Select(f=>f.OriginalFlight));
        }
        private void bindPolicyRemark(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.divPolicyRemark.Visible = true;
            var product = BasePage.GetOrderRole(applyform.Order) == OrderRole.Supplier ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            this.divPolicyRemarkContent.InnerHtml = string.Format("{0} <span class='systemEndFix'>{1}</span>", product.Remark, SystemParamService.PolicyRemark);
        }
        private void bindApplyAndProcessInfo(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm");
            this.lblAppliedReason.Text = applyform.ApplyRemark;
            if (applyform.ProcessedTime.HasValue)
            {
                this.lblProcessedTime.Text = applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm");
                this.lblProcessedResult.Text = StatusService.GetRefundApplyformStatus(applyform.Status, GetOrderRole(applyform.Order)) + " " + applyform.ProcessedFailedReason;
            }
            if (CurrentCompany.CompanyType == CompanyType.Provider && applyform.Status == DataTransferObject.Order.RefundApplyformStatus.Refunded)
            {
                var refundInfoHTML = new StringBuilder();
                refundInfoHTML.Append("<table><tr><th>航段</th><th>手续费率</th><th>手续费</th><th>退款金额</th></tr>");
                var index = 0;
                NormalRefundRoleBill bill = getUserRoleBill(applyform.RefundBill);
                var flightRefundFees = applyform.OriginalFlights.Join(bill.Source.Details, f => f.ReservateFlight, f => f.Flight.Id, (f1, f2) => new { flight = f1, fee = f2 });
                foreach (var item in flightRefundFees)
                {
                    refundInfoHTML.Append("<tr>");
                    refundInfoHTML.AppendFormat("<td>{0} - {1}</td>", item.flight.Departure.City, item.flight.Arrival.City);
                    refundInfoHTML.AppendFormat("<td>{0}%</td>", (item.fee.RefundRate * 100).TrimInvaidZero());
                    refundInfoHTML.AppendFormat("<td>{0}</td>", Math.Abs(item.fee.RefundFee).TrimInvaidZero());
                    if (index == 0)
                    {
                        refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", flightRefundFees.Count(), bill.Source.Anticipation.TrimInvaidZero());
                    }
                    refundInfoHTML.Append("</tr>");
                    index++;
                }


                refundInfoHTML.Append("</table>");
                this.divRefundFeeInfo.InnerHtml = refundInfoHTML.ToString();
            }
        }

        private NormalRefundRoleBill getUserRoleBill(NormalRefundBill bill)
        {
            if (bill.Purchaser.Owner.Id == BasePage.LogonCompany.CompanyId)
            {
                return bill.Purchaser;
            }
            else if (bill.Provider != null && bill.Provider.Owner.Id == BasePage.LogonCompany.CompanyId)
            {
                return bill.Provider;
            }
            else if (bill.Supplier != null && bill.Supplier.Owner.Id == BasePage.LogonCompany.CompanyId)
            {
                return bill.Supplier;
            }
            return bill.Platform.Deduction;
        }


        private void bindBill(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            OrderRole orderRole = GetOrderRole(applyform.Order);
            if (orderRole == OrderRole.Supplier && applyform.Status == RefundApplyformStatus.Refunded||
                (orderRole == OrderRole.Provider&&(applyform.Status==RefundApplyformStatus.Refunded||applyform.Status==RefundApplyformStatus.AgreedByProviderBusiness))
                )
            {
                this.bill.Visible = true;
                this.bill.InitData(applyform.RefundBill);
            }
            else
            {
                this.bill.Visible = false;
            }
        }
        private void setButtons(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            if (CurrentCompany.CompanyType == Common.Enums.CompanyType.Provider)
            {
                switch (applyform.Status)
                {
                    case ChinaPay.B3B.DataTransferObject.Order.RefundApplyformStatus.AppliedForProvider:
                    case ChinaPay.B3B.DataTransferObject.Order.RefundApplyformStatus.DeniedByProviderTreasurer:
                        this.btnProcess.Visible = !applyform.RequireRevisePrice;
                        this.btnProcess.Attributes.Add("onclick", string.Format("window.location.href='{0}?id={1}&returnUrl={2}';return false;",
                            applyform is Service.Order.Domain.Applyform.ScrapApplyform ? "Abolish.aspx" : "ProcessRefund.aspx", applyform.Id.ToString(), HttpUtility.UrlEncode(Request.Url.PathAndQuery)));
                        break;
                }
            }
        }
        private void setBackButton()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "ApplyList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void showErrorMessage(string message)
        {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }
    }
}