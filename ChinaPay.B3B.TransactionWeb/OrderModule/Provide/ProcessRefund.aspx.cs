using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class ProcessRefund : BasePage
    {
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";
        private static readonly Regex _numReg = new Regex("\\d+(\\.\\d+)?");

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

        private readonly List<PNRPair> RenderedPNR = new List<PNRPair>();

        protected bool IsSpeical
        {
            get
            {
                if (ViewState["IsSpeical"] == null) return false;
                return (bool) ViewState["IsSpeical"];
            }
            set { ViewState["IsSpeical"] = value; }
        }

        protected bool RequireSeparatePNR
        {
            get
            {
                if (ViewState["RequireSeparatePNR"] == null)
                {
                    return false;
                }
                return (bool) ViewState["RequireSeparatePNR"];
            }
            set { ViewState["RequireSeparatePNR"] = value; }
        }

        private string ReturnUrl
        {
            get
            {
                string returnUrl = Request.QueryString["returnUrl"];
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = "ChangeProcessList.aspx";
                }
                if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
                return returnUrl;
            }
        }

        protected bool FeeInputEnabled { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                lblRefunPhone.Text = CurrenContract.RefundPhone;
                setBackButton();
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    string lockErrorMsg;
                    if (Lock(applyformId, LockRole.Provider, "退票处理", out lockErrorMsg))
                    {
                        if (applyform == null)
                        {
                            showErrorMessage("退/废票申请单不存在");
                        }
                        else
                        {
                            var attachment = ApplyformQueryService.QueryApplyAttachmentView(applyform.Id);
                            if(attachment.Any())
                            {
                                ucOutPutImage.IsPlatform = false;
                                ucOutPutImage.ApplyAttachment = attachment;
                            }
                            else
                            {
                                divApplyAttachment.Visible = false;
                            }
                            IsSpeical = !applyform.Order.IsThirdRelation && applyform.IsSpecial;
                            RequireSeparatePNR = applyform.RequireSeparatePNR;
                            bindData(applyform);
                        }
                    }
                    else
                    {
                        showErrorMessage("锁定申请单失败。原因:" + lockErrorMsg);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }

        private void bindData(RefundOrScrapApplyform applyform)
        {
            bindHeader(applyform);
            bindPassengers((RefundApplyform) applyform);
            bindPolicyRemark(applyform);
            bindProcessInfo(applyform);
            bindETEDCondition(applyform);
        }

        private void bindETEDCondition(RefundOrScrapApplyform applyform)
        {
            string condition = applyform.Order.IsThirdRelation ? applyform.Order.Supplier.Product.Condition : applyform.Order.Provider.Product.Condition;
            if (string.IsNullOrEmpty(condition)) return;
            divETDZCondition.Visible = true;
            divConditionContent.InnerText = condition;
        }

        private void bindProcessInfo(RefundOrScrapApplyform applyform)
        {
            ProcessInfo.DataSource = applyform.Operations.Where(p => p.Company == CurrentCompany.CompanyId);
            ProcessInfo.DataBind();
            Reason.Text = applyform.ProcessedFailedReason;
        }

        private void bindHeader(RefundOrScrapApplyform applyform)
        {
            var refundForm = applyform as RefundApplyform;
            OrderRole orderRole = OrderRole.Provider;
            lblApplyformId.Text = applyform.Id.ToString();
            linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" +
                               HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            linkOrderId.InnerText = applyform.OrderId.ToString();
            lblApplyType.Text = string.Format("{0} {1}", applyform,
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

            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd");
            lblAppliedReason.Text = applyform.ApplyRemark;

            if (applyform.Order.Provider != null && applyform.Order.Provider.PurchaserRelationType != RelationType.Brother)
            {
                lblRelation.Text = applyform.Order.Provider.PurchaserRelationType.GetDescription() + "-";
                hrefPurchaseName.InnerHtml = applyform.Order.Purchaser.Company.UserName + "（" + applyform.Order.Purchaser.Name + "）";
                hrefPurchaseName.HRef = "/OrganizationModule/RoleModule/ExtendCompanyManage/LowerComapnyInfoUpdate/LowerCompanyDetailInfo.aspx?CompanyId="
                                        + applyform.Order.Purchaser.CompanyId.ToString() +
                                        "&Type=" + (applyform.Order.Provider.PurchaserRelationType == RelationType.Interior ? "Organization" : "Junion");
            }
            else
            {
                lblRelation.Text = "平台采购";
                hrefPurchaseName.Visible = false;
            }
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

        private void showErrorMessage(string message)
        {
            divError.Visible = true;
            divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        private void bindPassengers(RefundApplyform applyform)
        {
            int passenagerCount = applyform.Passengers.Count();
            Order order = applyform.Order;
            bool returnServiceCharge = IsSpeical && !order.IsThirdRelation &&
                                       (applyform.RefundType == RefundType.Involuntary || (order.IsSpecial && applyform.RefundType == RefundType.SpecialReason));
            MutilFlightTip.Visible = order.TripType != ItineraryType.OneWay; //多段航程显示价格修改提示
            decimal CommissionRate = applyform.Order.Bill.PayBill.Provider.Source.Fare == 0
                                         ? 0
                                         : Math.Abs(applyform.Order.Bill.PayBill.Provider.Source.Commission)/applyform.Order.Bill.PayBill.Provider.Source.Fare;
            var flightsPassengerRelation = applyform.Flights.OrderBy(f => f.OriginalFlight.Serial)
                .Select(flightInfo =>
                            {
                                var bill = applyform.Order.Bill.PayBill.Provider.Source.Details.First(f=>f.Flight.Id==flightInfo.OriginalFlight.ReservateFlight);
                                var maxRufundFee = bill!=null?bill.Anticipation:short.MaxValue;
                                return
                                    new
                                        {
                                            Flight = flightInfo,
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
                                            RefundFee = string.Empty,
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
                                                                 p.Name,
                                                                 No = ticket==null?string.Empty:ticket.SettleCode+"-"+ticket.No,
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
                                                                           flightInfo.RefundServiceCharge??0)
                                                                       : string.Format("{0}(退{1})",
                                                                           serviceCharge,
                                                                           flightInfo.
                                                                             RefundServiceCharge??0),
                                                                 Commission =
                                                             IsSpeical
                                                                 ? "0"
                                                                 : (CommissionRate *
                                                                    flightInfo.OriginalFlight.Price.Fare).
                                                                       TrimInvaidZero(),
                                                                 returnServiceCharge,
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
                                                                 returnServiceCharge
                                                                     ? flightInfo.RefundServiceCharge??0
                                                                     : 0, maxRufundFee)
                                                             }
                                        };
                            }
                );


            RefundInfos.DataSource = flightsPassengerRelation;
            RefundInfos.DataBind();
            FeeInputEnabled = applyform.RefundType != RefundType.Involuntary && applyform.RefundType != RefundType.Upgrade;
            this.hfdTradeFee.Value = order.Bill.PayBill.Provider.Owner.Rate.ToString();
        }

        private void bindPolicyRemark(RefundOrScrapApplyform applyform)
        {
            divPolicyRemark.Visible = true;
            ProductInfo productInfo = applyform.Order.Provider.Product;
            divPolicyRemarkContent.InnerHtml = string.Format("{0} <span class='systemEndFix'>{1}</span>", productInfo == null ? string.Empty : productInfo.Remark,
                SystemParamService.PolicyRemark);
        }

        protected void btnAgree_Click(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    CheckPNR();
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    IEnumerable<RefundFeeView> feeInfos = CollectFeeInfo();
                    if (!feeInfos.Any())
                    {
                        throw new Exception("费率输入不正确！");
                    }
                    string pnr = txtPNR.Text.Trim();
                    string bpnr = txtBPNR.Text.Trim();
                    var refundProcessView = new RefundProcessView
                        {
                            NewPNR = new PNRPair(pnr, bpnr),
                            Items = feeInfos
                        };
                    ApplyformProcessService.AgreeRefundOrScrapByProvider(applyformId, refundProcessView, CurrentUser.UserName, CurrentUser.Name);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('同意退废票成功');location.href='/OrderModule/Provide/ChangeProcessList.aspx'");
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "同意退/废票");
            }
        }

        private void CheckPNR()
        {
            if (!RequireSeparatePNR) return;
            if (string.IsNullOrEmpty(txtPNR.Text.Trim()) && string.IsNullOrEmpty(txtBPNR.Text.Trim())) throw new VerificationException("没有输入分离编码！");
            var pnrReg = new Regex(@"^[\w\d]{6}$");
            if (!string.IsNullOrEmpty(txtPNR.Text.Trim()) && !pnrReg.IsMatch(txtPNR.Text.Trim())) throw new VerificationException("小编码格式不正确");
            if (!string.IsNullOrEmpty(txtBPNR.Text.Trim()) && !pnrReg.IsMatch(txtBPNR.Text.Trim())) throw new VerificationException("大编码格式不正确");
        }

        private IEnumerable<RefundFeeView> CollectFeeInfo()
        {
            var result = new List<RefundFeeView>();
            foreach (RepeaterItem item in RefundInfos.Items)
            {
                var Passengers = item.FindControl("Passengers") as Repeater;
                if (Passengers != null)
                {
                    foreach (RepeaterItem repeaterItem in Passengers.Items)
                    {
                        var txtReate = repeaterItem.FindControl("Reate") as TextBox;
                        var txtFee = repeaterItem.FindControl("Fee") as TextBox;
                        var voyage = repeaterItem.FindControl("Voyage") as HiddenField;
                        if (Validates(txtReate.Text.Trim()) && Validates(txtFee.Text.Trim()))
                        {
                            string[] voyageItems = voyage.Value.Split('-');
                            var flightVoyage = new AirportPair(voyageItems[0], voyageItems[1]);
                            result.Add(new RefundFeeView(flightVoyage, decimal.Parse(txtReate.Text.Trim())/100,
                                decimal.Parse(txtFee.Text.Trim())));
                        }
                    }
                }
            }
            return result;
        }

        private bool Validates(string numInput, string regError = "请输入正确的费率信息！")
        {
            if (numInput == string.Empty)
            {
                throw new Exception("请输入完整的费率信息和编码！");
            }
            if (!_numReg.IsMatch(numInput))
            {
                throw new Exception(regError);
            }
            return true;
        }

        protected void btnDeny_Click(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    ApplyformProcessService.DenyRefundOrScrapByProvider(applyformId, Reason.Text.Trim(),
                        CurrentUser.UserName);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('拒绝成功！');location.href='" + ReturnUrl + "'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "拒绝退/废票");
            }
        }

        protected void btnReleaseLockAndBack_Click(object sender, EventArgs e)
        {
            decimal applyformId;
            if (decimal.TryParse(Request.QueryString["id"], out applyformId))
            {
                ReleaseLock(applyformId);
                RegisterScript(this, "location.href='" + ReturnUrl + "'", true);
            }
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

        private void setBackButton()
        {
            string returnUrl = ReturnUrl;
            btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }

        private string getEI(Flight flight, Order order)
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

        public static string GetRegulations(string refundRegulation, string changeRegulation, string endorseRegulation, string remarks)
        {
            var result = new StringBuilder();
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", refundRegulation);
            result.AppendFormat("<p><span class=b>作废规定：</span>{0}</p>", changeRegulation);
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", endorseRegulation);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", remarks);
            return result.ToString();
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

        #region Nested type: RenderTempClass

        private class RenderTempClass
        {
            public RenderTempClass(RefundFlight flight, Passenger passenger, Ticket ticket)
            {
                Flight = flight;
                Passenger = passenger;
                Ticket = ticket;
            }

            public RefundFlight Flight { get; set; }
            public Passenger Passenger { get; set; }
            public Ticket Ticket { get; set; }
        }

        #endregion
    }
}