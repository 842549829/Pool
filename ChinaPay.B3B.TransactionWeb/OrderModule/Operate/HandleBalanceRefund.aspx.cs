using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using RefundFlight = ChinaPay.B3B.Service.Order.Domain.Applyform.RefundFlight;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.B3B.DataTransferObject.Policy;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class HandleBalanceRefund : BasePage
    {
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";

        protected bool IsSpeical
        {
            get
            {
                if (ViewState["IsSpeical"] == null) return false;
                return (bool)ViewState["IsSpeical"];
            }
            set { ViewState["IsSpeical"] = value; }
        }

        private readonly Dictionary<int, string> CNIndex = new Dictionary<int, string>
            {
                {1, "一"},
                {2, "二"},
                {3, "三"},
                {4, "四"},
                {5, "五"},
                {6, "六"},
                {7, "七"},
                {8, "八"},
                {9, "九"},
                {10, "十"},
            };

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal applyformId;
                string errorMsg = string.Empty;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    
                    if (Lock(applyformId, LockRole.Platform, "处理差额退款",out errorMsg))
                    {
                        var applyform = Service.ApplyformQueryService.QueryBalanceRefundApplyform(applyformId);
                        if (applyform == null)
                        {
                            showErrorMessage("差额退款申请单不存在");
                        }
                        else
                        {
                            
                            bindData(applyform);
                            bindApplyInfo(applyform);
                            form1.Visible = true;
                        }
                    }
                    else
                    {
                        showErrorMessage(errorMsg);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindData(Service.Order.Domain.Applyform.BalanceRefundApplyform applyform)
        {
            bindHeader(applyform);
            bindVoyages(applyform.Applyform);
            bindPassengers(applyform.Applyform);
            bindApplyAndProcessInfo(applyform.Applyform);
            bindBill(applyform.Applyform);
            bindProcessInfo(applyform.Applyform);
        }

        private void bindProcessInfo(RefundOrScrapApplyform applyform)
        {
            var passenagerCount = applyform.Passengers.Count();
            hdPassengerCount.Value = passenagerCount.ToString();
            ChinaPay.B3B.Service.Order.Domain.Order order = applyform.Order;
            decimal CommissionRate = applyform.Order.Bill.PayBill.Provider.Source.Fare == 0
                                         ? 0
                                         : Math.Abs(applyform.Order.Bill.PayBill.Provider.Source.Commission) / applyform.Order.Bill.PayBill.Provider.Source.Fare;
            var flightsPassengerRelation = applyform.Flights.OrderBy(f => f.OriginalFlight.Serial)
                .Select(flightInfo =>
                {
                    var bill = applyform.RefundBill.Purchaser.Source.Details.First(f => f.Flight.Id == flightInfo.OriginalFlight.ReservateFlight);
                    var maxRufundFee = bill != null ? bill.Anticipation : short.MaxValue;
                    return
                        new
                        {
                            Refunded = bill.Amount,
                            RefundFee = Math.Abs(bill.RefundFee),
                            RefundReate = (bill.RefundRate*100).ToString("0.00"),
                            Flight = flightInfo,
                            flightId = flightInfo.OriginalFlight.Id,
                            Departure = flightInfo.OriginalFlight.Departure.Name,
                            Arrival = flightInfo.OriginalFlight.Arrival.Name,
                            Carrier = flightInfo.OriginalFlight.Carrier.Code,
                            flightInfo.OriginalFlight.FlightNo,
                            TicketPrice = flightInfo.OriginalFlight.Price.Fare,
                            flightInfo.OriginalFlight.Id,
                            Bunk = flightInfo.OriginalFlight.Bunk.Code,
                            TakeoffTime =
                                flightInfo.OriginalFlight.TakeoffTime.ToString("yyyy-MM-dd"),
                            Rate = string.Empty,
                            Fee = string.Empty,
                            Total = string.Empty,
                            PassengerCount = passenagerCount,
                            EI = getEI(flightInfo.OriginalFlight, applyform.Order),
                            TripType = order.TripType.GetDescription(),
                            RenderServiceCharge = IsSpeical && !order.IsThirdRelation,
                            Seaial = CNIndex[flightInfo.OriginalFlight.Serial],
                            Passengers = from p in applyform.Passengers
                                         let serviceCharge = getServiceCharge(p)
                                         let ticket = p.Tickets.First(
                                                     t => t.Flights
                                                         .Any(f => f.ReservateFlight == flightInfo.OriginalFlight.ReservateFlight))
                                         select new
                                         {
                                             first = bill.Anticipation,
                                             p.Name,
                                             No = ticket == null ? string.Empty : ticket.SettleCode + "-" + ticket.No,
                                             PassengerType =
                                         p.PassengerType.GetDescription(),
                                             flightInfo.OriginalFlight.Price.AirportFee,
                                             flightInfo.OriginalFlight.Price.BAF,
                                             TicketPrice =
                                         flightInfo.OriginalFlight.Price.Fare,
                                             YingShou =
                                         flightInfo.OriginalFlight.Price.Total -
                                         (CommissionRate *
                                          flightInfo.OriginalFlight.Price.Fare),
                                             Rate = string.Empty,
                                             Fee = string.Empty,
                                             AirportPair = string.Format("{0}-{1}",
                                                 flightInfo.OriginalFlight.Departure.Code,
                                                 flightInfo.OriginalFlight.Arrival.Code),
                                             TotalRefund = string.Empty,
                                             RenderServiceCharge =
                                         IsSpeical && !order.IsThirdRelation,
                                             p.CredentialsType,
                                             RefundServiceCharge =
                                         flightInfo.RefundServiceCharge == 0
                                             ? string.Format("{0}(不退)", serviceCharge)
                                             : serviceCharge ==
                                               flightInfo.RefundServiceCharge
                                                   ? string.Format("{0}(全退)",
                                                       flightInfo.RefundServiceCharge ?? 0)
                                                   : string.Format("{0}(退{1})",
                                                       serviceCharge,
                                                       flightInfo.
                                                         RefundServiceCharge ?? 0),
                                             Commission =
                                         IsSpeical
                                             ? "0"
                                             : (CommissionRate *
                                                flightInfo.OriginalFlight.Price.Fare).
                                                   TrimInvaidZero(),
                                             //是否是特殊票,民航基金，燃油，佣金
                                             StrFee =
                                         string.Format(
                                             "parameters={{IsSpeical:{0},AirportFee:{1},BAF:{2},Commission:{3},Price:{4},ServiceCharge:{5},maxRufundFee:{6}}}",
                                             IsSpeical ? 1 : 0,
                                             flightInfo.OriginalFlight.Price.AirportFee,
                                             flightInfo.OriginalFlight.Price.BAF,
                                             (IsSpeical
                                                  ? 0
                                                  : CommissionRate *
                                                    flightInfo.OriginalFlight.Price.Fare).
                                         TrimInvaidZero(),
                                             flightInfo.OriginalFlight.Price.Fare,
                                            0,
                                            maxRufundFee
                                            )
                                         }
                        };
                }
                );
            RefundInfos.DataSource = flightsPassengerRelation;
            RefundInfos.DataBind();
        }

        protected void RefundInfos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var Passengers = e.Item.FindControl("Passengers") as Repeater;
            if (Passengers != null)
            {
                Passengers.DataSource = DataBinder.Eval(e.Item.DataItem, "Passengers");
                Passengers.DataBind();
            }
        }

        private string getEI(Flight flight, ChinaPay.B3B.Service.Order.Domain.Order order)
        {
            switch (order.Product.ProductType)
            {
                case ProductType.Promotion:
                    return getProvision(order.Provider.Product.RefundAndReschedulingProvision);
                case ProductType.Special:
                    if (order.IsThirdRelation && GetOrderRole(order) != OrderRole.Supplier)
                        return getProvision(order.Supplier.Product.RefundAndReschedulingProvision);
                    else
                        return getProvision(order.Provider == null ? null : order.Provider.Product.RefundAndReschedulingProvision);
                default:
                    var eiTemplate = new Regex("退票规定:(?<RefundRegulation>.+?)改签规定:(?<ChangeRegulation>.+?)签转规定:(?<EndorseRegulation>.+?)备注:(?<Remarks>.+)");
                    Match bunkEI = eiTemplate.Match(flight.Bunk.EI);
                    if (bunkEI.Success)
                    {
                        return GetRegulations(bunkEI.Groups["RefundRegulation"].Value, bunkEI.Groups["ChangeRegulation"].Value,
                            bunkEI.Groups["EndorseRegulation"].Value, bunkEI.Groups["Remarks"].Value);
                    }
                    return flight.Bunk.EI;
            }
        }

        private string getProvision(RefundAndReschedulingProvision provision)
        {
            if (provision == null) return string.Empty;
            return string.Format("作废规定：{0}<br />改签规定：{1}<br />签转规定：{2}<br />退票规定：{3}",
                provision.Scrap,
                provision.Alteration,
                provision.Transfer,
                provision.Refund);
        }

        public static string GetRegulations(string refundRegulation, string changeRegulation, string endorseRegulation, string remarks)
        {
            var result = new StringBuilder();
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", refundRegulation);
            result.AppendFormat("<p><span class=b>作废规定：</span>{0}</p>", changeRegulation);
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", endorseRegulation);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", remarks);
            return result.ToString();
        }

        private decimal getServiceCharge(Passenger passenger)
        {
            decimal result = 0M;
            if (IsSpeical)
            {
                result += (from ticket in passenger.Tickets
                           from flight in ticket.Flights
                           where flight.Bunk is SpecialBunk
                           select (flight.Bunk as SpecialBunk).ServiceCharge).Sum();
            }
            return result;
        }

        private void bindApplyInfo(BalanceRefundApplyform applyform)
        {
            var submitTime = applyform.RefundBill.TradeTime;
            var remark = applyform.RefundBill.Remark;
            var InfoHTML = new StringBuilder();
            InfoHTML.Append("<table><tr><th>提交时间</th><th>差错备注</th></tr>");
            InfoHTML.Append("<tr>");
            InfoHTML.AppendFormat("<td>{0}</td><td>{1}</td>", submitTime, remark);
            InfoHTML.Append("</table>");
            this.divApplication.InnerHtml = InfoHTML.ToString();
        }
        private void bindHeader(Service.Order.Domain.Applyform.BalanceRefundApplyform applyform)
        {
            this.lblApplyformId.Text = applyform.Id.ToString();
            this.linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            this.linkOrderId.InnerText = applyform.OrderId.ToString();
            this.lblApplyType.Text = applyform.ToString();
            var product = applyform.Order.IsThirdRelation ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription();
            }
            this.lblStatus.Text = Service.Order.StatusService.GetBalanceRefundStatus(applyform.BalanceRefundStatus, DataTransferObject.Order.OrderRole.Platform);
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
            this.linkPurchaser.InnerText = applyform.PurchaserName;
            this.linkPurchaser.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + applyform.PurchaserId.ToString();
            this.linkProvider.InnerText = applyform.ProviderName;
            this.linkProvider.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + applyform.ProviderId.ToString();
        }


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

        List<PNRPair> RenderedPNR = new List<PNRPair>();


        private void bindVoyages(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.voyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight));
        }
        private void bindPassengers(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.passengers.InitData(applyform.Order, applyform.Passengers, applyform.Flights.Select(f => f.OriginalFlight));
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
            if (applyform.Status == DataTransferObject.Order.RefundApplyformStatus.Refunded)
            {
                var refundInfoHTML = new StringBuilder();
                refundInfoHTML.Append("<table><tr><th>航段</th><th>手续费率</th><th>手续费</th><th>退款金额</th><th>交易流水号</th><th>交易时间</th></tr>");
                var index = 0;
                var bill = CompanyType.Platform == CurrentCompany.CompanyType ? applyform.RefundBill.Provider : getUserRoleBill(applyform.RefundBill);
                var flightRefundFees = applyform.OriginalFlights.Join(bill.Source.Details, f => f.ReservateFlight, f => f.Flight.Id, (f1, f2) => new
                {
                    flight = f1,
                    fee = f2
                }).ToList();
                foreach (var item in flightRefundFees)
                {
                    refundInfoHTML.Append("<tr>");
                    refundInfoHTML.AppendFormat("<td>{0} - {1}</td>", item.flight.Departure.City, item.flight.Arrival.City);
                    refundInfoHTML.AppendFormat("<td>{0}%</td>", (item.fee.RefundRate * 100).TrimInvaidZero());
                    refundInfoHTML.AppendFormat("<td>{0}</td>", Math.Abs(item.fee.RefundFee).TrimInvaidZero());
                    if (index == 0)
                    {
                        refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", flightRefundFees.Count, bill.Source.Details.Sum(p => p.Anticipation).TrimInvaidZero());
                    }
                    refundInfoHTML.AppendFormat("<td>{0}</td>", applyform.RefundBill.Tradement.TradeNo);
                    refundInfoHTML.AppendFormat("<td>{0}</td>", applyform.RefundBill.Purchaser.Time);
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
            return null;
        }

        private void bindBill(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            if (applyform.Status == ChinaPay.B3B.DataTransferObject.Order.RefundApplyformStatus.Refunded)
            {
                this.bill.InitData(applyform.RefundBill);
            }
            else
            {
                this.bill.Visible = false;
            }
        }
        private void setBackButton()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "ApplyformList.aspx";
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
        
        /// <summary>
        /// 平台同意差额退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProcess_click(object sender, EventArgs e) {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    var feeInfo = GetBalanceRefundFee();

                    ApplyformProcessService.PlatformAgreeBalanceRefund(applyformId, feeInfo, CurrentUser.UserName, CurrentUser.Name);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('提交成功！');location.href='" + ReturnUrl + "'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "同意差额退款");
            }
        }

        private List<BalanceRefundFeeView> GetBalanceRefundFee()
        {
            var result = new List<BalanceRefundFeeView>();
            foreach (RepeaterItem item in RefundInfos.Items)
            {
                var feeTypeContainer = item.FindControl("FeeType") as DropDownList;
                var feeInputer = item.FindControl("balance") as TextBox;
                var voyageIdContainer = item.FindControl("voyageId") as HiddenField;
                if (feeTypeContainer == null || feeInputer == null || voyageIdContainer==null) return null;
                var balanceRefundFeeView = new BalanceRefundFeeView()
                    {
                        Voyage = Guid.Parse(voyageIdContainer.Value),
                    };
                int passenagerCount = int.Parse(hdPassengerCount.Value);
                if (feeTypeContainer.SelectedIndex == 1)
                {
                    balanceRefundFeeView.Rate = feeInputer.Text.ToDecimal() * passenagerCount;
                }
                else
                {
                    balanceRefundFeeView.Fee = feeInputer.Text.ToDecimal() * passenagerCount;
                }
                result.Add(balanceRefundFeeView);
            }
            return result;
        }

        protected string ReturnUrl
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

        /// <summary>
        /// 平台拒绝差额退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_click(object sender, EventArgs e) {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    ApplyformProcessService.PlatformNotAgreeBalanceRefund(applyformId, CurrentUser.UserName, CurrentUser.Name,refuseReason.Value);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('拒绝差额退款成功！');location.href='" + ReturnUrl + "'", false);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "拒绝差额退款");
            }
        }
    }
}