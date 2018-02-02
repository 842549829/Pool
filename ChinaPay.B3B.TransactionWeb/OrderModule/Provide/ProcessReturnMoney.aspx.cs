using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using RefundFlight = ChinaPay.B3B.Service.Order.Domain.Applyform.RefundFlight;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide {
    public partial class ProcessReturnMoney : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                decimal applyformId;
                if(decimal.TryParse(Request.QueryString["id"], out applyformId)) {
                    string lockErrorMsg;
                    if(Lock(applyformId, LockRole.Provider, "退款", out lockErrorMsg)) {
                        RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                        if(applyform == null) {
                            showErrorMessage("退/废票申请单不存在");
                        } else {
                            bindData(applyform);
                        }
                    } else {
                        showErrorMessage("锁定申请单失败。原因:" + lockErrorMsg);
                    }
                } else {
                    showErrorMessage("参数错误");
                }
            }
        }

        private void bindData(RefundOrScrapApplyform applyform) {
            BindHeader(applyform);
            bindVoyages(applyform);
            bindPassengers(applyform);
            bindPolicyRemark(applyform);
            bindApplyAndProcessInfo(applyform);
            bindBill(applyform);
        }

        private void BindHeader(RefundOrScrapApplyform applyform) {
            RefundApplyform refundForm = applyform as RefundApplyform;
            OrderRole orderRole = BasePage.GetOrderRole(applyform.Order);
            lblApplyformId.Text = applyform.Id.ToString();
            linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            linkOrderId.InnerText = applyform.OrderId.ToString();
            this.lblApplyType.Text = string.Format("{0} {1}", applyform,
                refundForm != null ? (string.Format("({0})", refundForm.RefundType.GetDescription())) : string.Empty);
            //var product = applyform.Order.IsThirdRelation ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            var product = applyform.Order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = product.ProductType.GetDescription();
            }
            lblStatus.Text = StatusService.GetRefundApplyformStatus(applyform.Status, orderRole);
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


        private void bindVoyages(RefundOrScrapApplyform applyform) { voyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight)); }
        private void bindPassengers(RefundOrScrapApplyform applyform) { passengers.InitData(applyform.Order, applyform.Passengers,applyform.Flights.Select(f=>f.OriginalFlight)); }

        private void bindPolicyRemark(RefundOrScrapApplyform applyform) {
            var product = applyform.Order.Provider.Product;
            lblPolicyRemark.Text = product.Remark;
        }

        private void bindApplyAndProcessInfo(RefundOrScrapApplyform applyform) {
            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm");
            lblAppliedReason.Text = applyform.ApplyRemark;
            if(applyform.ProcessedTime.HasValue) {
                lblProcessedTime.Text = applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm");
                lblProcessedResult.Text = applyform.ProcessStatus == ApplyformProcessStatus.Finished ? StatusService.GetRefundApplyformStatus(applyform.Status, GetOrderRole(applyform.Order)) + " " + applyform.ProcessedFailedReason : string.Empty;
            }
            if (CurrentCompany.CompanyType == CompanyType.Provider && applyform.Status == RefundApplyformStatus.AgreedByProviderBusiness)
            {
                var refundInfoHTML = new StringBuilder();
                refundInfoHTML.Append("<table><tr><th>航段</th><th>手续费率</th><th>手续费</th><th>应退金额</th><th>实退金额</th></tr>");
                int index = 0;
                //if(applyform is RefundApplyform)
                //{
                    NormalRefundRoleBill bill = applyform.RefundBill.Provider;
                    var flightRefundFees = applyform.OriginalFlights.Join(bill.Source.Details, f => f.ReservateFlight,
                        f => f.Flight.Id, (f1, f2) => new { flight = f1, fee = f2 });
                    foreach (var item in flightRefundFees)
                    {
                        refundInfoHTML.Append("<tr>");
                        refundInfoHTML.AppendFormat("<td>{0} - {1}</td>", item.flight.Departure.City, item.flight.Arrival.City);
                        refundInfoHTML.AppendFormat("<td>{0}%</td>", (item.fee.RefundRate * 100).TrimInvaidZero());
                        refundInfoHTML.AppendFormat("<td>{0}</td>", Math.Abs(item.fee.RefundFee).TrimInvaidZero());
                        if (index == 0)
                        {
                            refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", applyform.Flights.Count()*applyform.Passengers.Count(), bill.Source.Anticipation.TrimInvaidZero());
                            refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", applyform.Flights.Count() * applyform.Passengers.Count(), bill.Amount.TrimInvaidZero());
                        }
                        refundInfoHTML.Append("</tr>");
                        index++;
                    }
                //}
                //else
                //{
                //    NormalRefundRoleBill bill = applyform.RefundBill.Provider;
                //    foreach (var item in applyform.Flights)
                //    {
                //        refundInfoHTML.Append("<tr>");
                //        refundInfoHTML.AppendFormat("<td>{0} - {1}</td>", item.OriginalFlight.Departure.AirportCode, item.OriginalFlight.Arrival.AirportCode);
                //        refundInfoHTML.AppendFormat("<td>{0}%</td>", (item.RefundFeeInfo.Rate * 100).TrimInvaidZero());
                //        refundInfoHTML.AppendFormat("<td>{0}</td>", item.RefundFeeInfo.Fee.TrimInvaidZero());
                //        if (index == 0)
                //        {
                //            refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", applyform.Flights.Count(), bill.Source.Anticipation.TrimInvaidZero());
                //        }
                //        refundInfoHTML.Append("</tr>");
                //        index++;
                //    }
                //}
                refundInfoHTML.Append("</table>");
                divRefundFeeInfo.InnerHtml = refundInfoHTML.ToString();
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


        private void bindBill(RefundOrScrapApplyform applyform) {
            if(BasePage.GetOrderRole(applyform.Order) == OrderRole.Supplier && applyform.Status == RefundApplyformStatus.Refunded) {
                bill.Visible = true;
                bill.InitData(applyform.RefundBill);
            } else {
                bill.Visible = false;
            }
        }

        private void showErrorMessage(string message) {
            divError.Visible = true;
            divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        protected void btnReleaseLock_Click(object sender, EventArgs e) {
            decimal applyformId;
            if(decimal.TryParse(Request.QueryString["id"], out applyformId)) {
                ReleaseLock(applyformId);
                Response.Redirect("/OrderModule/Provide/ReturnMoneyList.aspx");
            }
        }

        protected void denyRefund_Submited(object sender, EventArgs e) {
            try {
                decimal applyformId;
                if(decimal.TryParse(Request.QueryString["id"], out applyformId)) {
                    ApplyformProcessService.DenyReturnMoneyByProviderTreasurer(applyformId, txtReason.Text.Trim(), CurrentUser.UserName);
                    ReleaseLock(applyformId);
                    RegisterScript("alert('拒绝退款成功');window.location.href='ReturnMoneyList.aspx';");
                } else {
                    showErrorMessage("参数错误");
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "拒绝退款");
            }
        }

        protected void btnAgreeRefund_Click(object sender, EventArgs e) {
            try {
                decimal applyformId;
                if(decimal.TryParse(Request.QueryString["id"], out applyformId)) {
                   ApplyformProcessService.AgreeReturnMoneyByProviderTreasurer(applyformId, CurrentUser.UserName);
                    ReleaseLock(applyformId);
                    RegisterScript("alert('同意退款成功');window.location.href='ReturnMoneyList.aspx';");
                } else {
                    showErrorMessage("参数错误");
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "同意退款");
            }
        }
    }
}