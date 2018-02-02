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
    public partial class ProcessBalanceRefundReturnMoney : BasePage
    {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal applyformId;
                if(decimal.TryParse(Request.QueryString["id"], out applyformId)) {
                    string lockErrorMsg;
                    if(Lock(applyformId, LockRole.Provider, "退款", out lockErrorMsg)) {
                        BalanceRefundApplyform applyform = ApplyformQueryService.QueryBalanceRefundApplyform(applyformId);
                        if(applyform == null) {
                            showErrorMessage("差额退款申请单不存在");
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

        private void bindData(BalanceRefundApplyform applyform) {
            BindHeader(applyform);
            bindVoyages(applyform.Applyform);
            bindPassengers(applyform.Applyform);
            bindPolicyRemark(applyform);
            bindApplyAndProcessInfo(applyform);
            //bindBill(applyform);
            bindBalanceRefund(applyform);
            bindRefundInfo(applyform.Applyform);
        }

        private void BindHeader(BalanceRefundApplyform applyform) {
            OrderRole orderRole = BasePage.GetOrderRole(applyform.Order);
            lblApplyformId.Text = applyform.Id.ToString();
            linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            linkOrderId.InnerText = applyform.OrderId.ToString();
            this.lblApplyType.Text = "差额退款";
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
            lblStatus.Text = StatusService.GetBalanceRefundStatus(applyform.BalanceRefundStatus, orderRole);
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

        private void bindPolicyRemark(BalanceRefundApplyform applyform) {
            var product = applyform.Order.Provider.Product;
            lblPolicyRemark.Text = product.Remark;
        }

        private void bindApplyAndProcessInfo(BalanceRefundApplyform applyform) {
            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm");
            lblAppliedReason.Text = applyform.ApplyRemark;
            if(applyform.ProcessedTime.HasValue) {
                lblProcessedTime.Text = applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm");
                lblProcessedResult.Text = applyform.BalanceRefundStatus == BalanceRefundProcessStatus.Finished ? StatusService.GetBalanceRefundStatus(applyform.BalanceRefundStatus, GetOrderRole(applyform.Order)) + " " + applyform.ProcessedFailedReason : string.Empty;
            }
            
                var refundInfoHTML = new StringBuilder();
                refundInfoHTML.Append("<table><tr><th>航段</th><th>手续费率</th><th>手续费</th><th>应退金额</th><th>实退金额</th></tr>");
                int index = 0;
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
                            refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", applyform.Applyform.Flights.Count()*applyform.Passengers.Count(), bill.Source.Anticipation.TrimInvaidZero());
                            refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", applyform.Applyform.Flights.Count() * applyform.Passengers.Count(), bill.Amount.TrimInvaidZero());
                        }
                        refundInfoHTML.Append("</tr>");
                        index++;
                    }
                
                refundInfoHTML.Append("</table>");
                divRefundFeeInfo.InnerHtml = refundInfoHTML.ToString();
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


        //private void bindBill(BalanceRefundApplyform applyform) {
        //    if(BasePage.GetOrderRole(applyform.Order) == OrderRole.Supplier && applyform.Status == RefundApplyformStatus.Refunded) {
        //        bill.Visible = true;
        //        bill.InitData(applyform.RefundBill);
        //    } else {
        //        bill.Visible = false;
        //    }
        //}

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
                    ApplyformProcessService.ProviderTreasurerNotAgreeBalanceRefund(applyformId, txtReason.Text.Trim(), CurrentUser.UserName,CurrentUser.Name);
                    ReleaseLock(applyformId);
                    RegisterScript("alert('拒绝退款成功');window.location.href='BalanceRefundReturnMoneyList.aspx';");
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
                   ApplyformProcessService.ProviderTreasurerAgreeBalanceRefund(applyformId, CurrentUser.UserName,CurrentUser.Name);
                    ReleaseLock(applyformId);
                    RegisterScript("alert('同意退款成功');window.location.href='ReturnMoneyList.aspx';");
                } else {
                    showErrorMessage("参数错误");
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "同意退款");
            }
        }

        private void bindBalanceRefund(BalanceRefundApplyform applyform)
        {
            var refundInfoHTML = new StringBuilder();
            refundInfoHTML.Append("<h3 class=\"titleBg\">差错退款信息</h3><table>	<tr><th>姓名</th><th>类型</th><th>票号</th><th>票面价</th><th>名航基金</th><th>燃油</th><th>返佣</th><th>收款金额</th><th>首次退款金额</th><th>本次退款金额</th><th>退款金额总计</th>    </tr>");
            var index = 0;
            var bill = CompanyType.Platform == CurrentCompany.CompanyType ? applyform.RefundBill.Provider : getUserRoleBill(applyform.RefundBill);
            var commission = applyform.Order.Bill.PayBill.Provider.Source.Rebate;
            int passengerCount = applyform.Applyform.Passengers.Count();
            decimal refundedFee = applyform.Applyform.RefundBill.Provider.Amount / passengerCount;
            decimal balanceFee = (decimal)(applyform.Applyform.Flights.Sum(f => f.BanlanceFare) / passengerCount);
            foreach (var passenger in applyform.Applyform.Passengers)
            {
                refundInfoHTML.Append("<tr>");
                refundInfoHTML.AppendFormat("<td>{0}</td>", passenger.Name);
                refundInfoHTML.AppendFormat("<td>{0}</td>", passenger.PassengerType.GetDescription());
                refundInfoHTML.AppendFormat("<td>{0}</td>", string.Join("<br />", passenger.Tickets.Select(t => t.SettleCode + "-" + t.No)));
                refundInfoHTML.AppendFormat("<td>{0}</td>", passenger.Price.Fare);
                refundInfoHTML.AppendFormat("<td>{0:0.00}</td>", passenger.Price.AirportFee);
                refundInfoHTML.AppendFormat("<td>{0:0.00}</td>", passenger.Price.BAF);
                refundInfoHTML.AppendFormat("<td>{0:0.0}%</td>", commission * 100);
                refundInfoHTML.AppendFormat("<td>{0:0.00}</td>",
                    applyform.Applyform.Flights.Sum(f => f.OriginalFlight.Price.Fare * (1 - commission)));
                refundInfoHTML.AppendFormat("<td>{0}</td>", refundedFee);
                refundInfoHTML.AppendFormat("<td class='price'>{0:0.00}</td>", balanceFee);
                refundInfoHTML.AppendFormat("<td>{0}</td>", refundedFee + balanceFee);
                refundInfoHTML.Append("</tr>");
                index++;
            }
            refundInfoHTML.Append("</table>");
            this.BalanceRefundInfo.InnerHtml = refundInfoHTML.ToString();
            BalanceRefundInfo.Visible = true;

        }

        /// <summary>
        /// 加载退票处理信息
        /// </summary>
        /// <param name="applyform"></param>
        private void bindRefundInfo(RefundOrScrapApplyform applyform)
        {
            var refundInfoHTML = new StringBuilder();
            refundInfoHTML.Append("<table><tr> <th>提交时间</th> <th>处理时间</th> <th>退票原因</th> <th>处理结果</th> <th>航段</th> <th>手续费率</th> <th>手续费</th> <th>退款金额</th> <th>交易账号</th> <th>交易流水号</th> <th>交易时间</th></tr>");
            var index = 0;
            var bill = CompanyType.Platform == CurrentCompany.CompanyType ? applyform.RefundBill.Provider : getUserRoleBill(applyform.RefundBill);
            var flightRefundFees = applyform.OriginalFlights.Join(bill.Source.Details, f => f.ReservateFlight, f => f.Flight.Id, (f1, f2) => new
            {
                flight = f1,
                fee = f2
            });
            var tradeTime = applyform.RefundBill.TradeTime;
            var tradeNo = applyform.RefundBill.TradementBase.TradeNo;
            var payAccount = applyform.RefundBill.Provider.PayRoleBill.Owner.Account;
            var processStatus = applyform.Status.GetDescription();
            var applyRemark = applyform.ApplyRemark;
            var processedTime = applyform.ProcessedTime;
            var appliedTime = applyform.AppliedTime;
            foreach (var item in flightRefundFees)
            {
                refundInfoHTML.Append("<tr>");
                refundInfoHTML.AppendFormat("<td>{0:yyyy-MM-dd}</td>", appliedTime);
                refundInfoHTML.AppendFormat("<td>{0:yyyy-MM-dd}</td>", processedTime);
                refundInfoHTML.AppendFormat("<td>{0}</td>", applyRemark);
                refundInfoHTML.AppendFormat("<td>{0}</td>", processStatus);
                refundInfoHTML.AppendFormat("<td>{0}-{1}</td>", item.flight.Departure.City, item.flight.Arrival.City);
                refundInfoHTML.AppendFormat("<td>{0:0.00}</td>", item.fee.RefundRate);
                refundInfoHTML.AppendFormat("<td>{0:0.00}</td>", item.fee.RefundFee);
                refundInfoHTML.AppendFormat("<td>{0:0.00}</td>", item.fee.Amount);
                refundInfoHTML.AppendFormat("<td>{0}</td>", payAccount);
                refundInfoHTML.AppendFormat("<td>{0}</td>", tradeNo);
                refundInfoHTML.AppendFormat("<td>{0:yyyy-MM-dd}</td>", tradeTime);
                refundInfoHTML.Append("</tr>");
                index++;
            }
            refundInfoHTML.Append("</table>");
            this.RefundInfo.InnerHtml = refundInfoHTML.ToString();

        }

        private void setBackButton()
        {
            string returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "BalanceRefundReturnMoneyList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }

    }
}