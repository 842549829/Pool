using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class Abolish : BasePage
    {
        #region Attribute

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

        protected string Carrier
        {
            get
            {
                if (ViewState["Carrier"] == null)
                {
                    return string.Empty;
                }
                return (string) ViewState["Carrier"];
            }
            set { ViewState["Carrier"] = value; }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                hfdServicePhone.Value = CurrenContract.ServicePhone;
                lblScrapPhone.Text = CurrenContract.ScrapPhone;
                SetBackButton();
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    string lockErrorMsg;
                    if (Lock(applyformId, LockRole.Provider, "退票处理", out lockErrorMsg))
                    {
                        if (applyform == null)
                        {
                            ShowErrorMessage("退/废票申请单不存在");
                        }
                        else
                        {
                            IsSpeical = !applyform.Order.IsThirdRelation && applyform.IsSpecial;
                            RequireSeparatePNR = applyform.RequireSeparatePNR;
                            Carrier = applyform.Flights.First().OriginalFlight.Carrier.Code;
                            BindData(applyform);
                        }
                    }
                    else
                    {
                        ShowErrorMessage("锁定申请单失败。原因:" + lockErrorMsg);
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
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

        protected void btnAgree_Click(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    CheckPNR();
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    string pnr = txtPNR.Text.Trim();
                    string bpnr = txtBPNR.Text.Trim();
                    var refundProcessView = new RefundProcessView
                        {
                            NewPNR = new PNRPair(pnr, bpnr)
                        };
                    ApplyformProcessService.AgreeRefundOrScrapByProvider(applyformId, refundProcessView, CurrentUser.UserName, CurrentUser.Name);
                    ReleaseLock(applyformId);
                    RegisterScript(this, "alert('同意退废票成功');location.href='" + ReturnUrl + "'");
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "同意废票");
            }
        }

        #endregion

        #region BindDate

        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";
        private readonly List<PNRPair> RenderedPNR = new List<PNRPair>();

        private void BindData(RefundOrScrapApplyform applyform)
        {
            BindHeader(applyform);
            BindPolicyRemark(applyform);
            BindProcessInfo(applyform);
            BindPassengers(applyform);
            BindFlight(applyform);
        }

        private void BindHeader(RefundOrScrapApplyform applyform)
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
            if (applyform.Order.Provider != null && applyform.Order.Provider.PurchaserRelationType != RelationType.Brother)
            {
                lblRelation.Text = applyform.Order.Provider.PurchaserRelationType.GetDescription() + "-";
                hrefPurchaseName.InnerHtml = applyform.Order.Purchaser.Company.UserName + "（" + applyform.Order.Purchaser.Name + "）";
                hrefPurchaseName.HRef = "/OrganizationModule/RoleModule/ExtendCompanyManage/LowerComapnyInfoUpdate/LowerCompanyDetailInfo.aspx?CompanyId="
                                        + applyform.Order.Purchaser.CompanyId.ToString();
            }
            else
            {
                lblRelation.Text = "平台采购";
                hrefPurchaseName.Visible = false;
            }
            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:MM:ss");
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

        private void BindPolicyRemark(RefundOrScrapApplyform applyform)
        {
            divPolicyRemark.Visible = true;
            ProductInfo productInfo = applyform.Order.Provider.Product;
            divPolicyRemarkContent.InnerHtml = string.Format("<p class='policyTips'>{0}<span class='obvious'>{1}</span></p>" +
                                                             "<p class='reason'><span class='obvious'>废票申请理由：</span>{2}</p>",
                productInfo == null ? string.Empty : productInfo.Remark, SystemParamService.PolicyRemark, applyform.ApplyRemark);
        }

        private void BindProcessInfo(RefundOrScrapApplyform applyform)
        {
            ProcessInfo.DataSource = applyform.Operations.Where(p => p.Company == CurrentCompany.CompanyId);
            ProcessInfo.DataBind();
            Reason.Text = applyform.ProcessedFailedReason;
        }

        private void BindFlight(RefundOrScrapApplyform applyform)
        {
            FlightInfos.DataSource = applyform.Flights.OrderBy(f => f.OriginalFlight.Serial).Select(flightInfo =>
                                                                                                    new
                                                                                                        {
                                                                                                            Departure =
                                                                                                        flightInfo.OriginalFlight.Departure.City + " " +
                                                                                                        flightInfo.OriginalFlight.Departure.Name,
                                                                                                            Arrival =
                                                                                                        flightInfo.OriginalFlight.Arrival.City + " " +
                                                                                                        flightInfo.OriginalFlight.Arrival.Name,
                                                                                                            TakeoffTime =
                                                                                                        flightInfo.OriginalFlight.TakeoffTime.ToString("yyyy-MM-dd HH:mm"),
                                                                                                            LandingTime =
                                                                                                        flightInfo.OriginalFlight.LandingTime.ToString("yyyy-MM-dd HH:mm"),
                                                                                                            FightNo =
                                                                                                        flightInfo.OriginalFlight.Carrier.Code + flightInfo.OriginalFlight.FlightNo,
                                                                                                            Bunk = flightInfo.OriginalFlight.Bunk.Code,
                                                                                                            Discount =
                                                                                                        (flightInfo.OriginalFlight.Bunk.Discount*100).TrimInvaidZero()
                                                                                                        }
                );
            FlightInfos.DataBind();
        }

        private void BindPassengers(RefundOrScrapApplyform applyform)
        {
            decimal providerScrapFee = SystemParamService.ProviderRate;
            decimal resourcerScrapFee = SystemParamService.ResourcerRate;
            MutilFlightTip.Visible = applyform.Order.TripType != ItineraryType.OneWay; //多段航程显示价格修改提示
            decimal tradeRate = applyform.Order.Bill.PayBill.Provider.Owner.Rate;
            string rebate = (applyform.Order.Bill.PayBill.Provider.Source.Rebate * 100).TrimInvaidZero();
            var passengerInfos = (from passenger in applyform.Passengers
                                  let details = (from detail in applyform.Order.Bill.PayBill.Provider.Source.Details
                                                 join flight in applyform.OriginalFlights on detail.Flight.Id equals flight.ReservateFlight
                                                 where detail.Passenger == passenger.Id
                                                 select detail)
                                  let ticketCount = passenger.Tickets.Count()
                                  let scrapFeePerTicket = applyform.IsSpecial && !applyform.Order.IsThirdRelation ? (providerScrapFee + resourcerScrapFee) : providerScrapFee
                                  let scrapFee = scrapFeePerTicket*ticketCount
                                  let fare = details.Sum(d => d.Flight.Fare)
                                  let airportFee = details.Sum(d => d.Flight.AirportFee)
                                  let baf = details.Sum(d => d.Flight.BAF)
                                  let serviceCharge = details.Sum(d => d.Flight.ReleasedFare) - fare
                                  let commission = Math.Abs(details.Sum(d => d.Commission))
                                  let anticipation = details.Sum(d => d.Anticipation)
                                  let refundAnticipation = anticipation - scrapFee
                                  let refundFee = ChinaPay.Utility.Calculator.Ceiling(refundAnticipation * tradeRate, -2)
                                  select new
                                      {
                                          PassengerType = passenger.PassengerType.GetDescription(),
                                          passenger.Name,
                                          passenger.Credentials,
                                          TicketNo = passenger.Tickets.Join("<br/>", t => t.SettleCode + "-" + t.No),
                                          Fare = fare.TrimInvaidZero(),
                                          AirportFeeAndBAF = airportFee.TrimInvaidZero() + "/" + baf.TrimInvaidZero(),
                                          HandlingFee = scrapFee.TrimInvaidZero(),
                                          RefundAmount = refundAnticipation.TrimInvaidZero(),
                                          Commission = string.Format("{0}%({1})", rebate, commission.TrimInvaidZero()),
                                          Anticipation = anticipation.TrimInvaidZero(),
                                          RealRefundAmount = (refundAnticipation - refundFee).TrimInvaidZero(),
                                          HandlingFeeDigital = scrapFee,
                                          RefundAmountDigital = refundAnticipation,
                                          ServiceCharge = serviceCharge.TrimInvaidZero()
                                      }).ToList();
            PassengerInfos.DataSource = passengerInfos;
            PassengerInfos.DataBind();
            RateAll.InnerText = passengerInfos.Sum(f => f.HandlingFeeDigital).TrimInvaidZero();
            RefundALL.InnerText = passengerInfos.Sum(f => f.RefundAmountDigital).TrimInvaidZero();
        }

        #endregion

        #region Method

        private void CheckPNR()
        {
            if (!RequireSeparatePNR) return;
            if (string.IsNullOrEmpty(txtPNR.Text.Trim()) && string.IsNullOrEmpty(txtBPNR.Text.Trim())) throw new VerificationException("没有输入分离编码！");
            var pnrReg = new Regex(@"^[\w\d]{6}$");
            if (!string.IsNullOrEmpty(txtPNR.Text.Trim()) && !pnrReg.IsMatch(txtPNR.Text.Trim())) throw new VerificationException("小编码格式不正确");
            if (!string.IsNullOrEmpty(txtBPNR.Text.Trim()) && !pnrReg.IsMatch(txtBPNR.Text.Trim())) throw new VerificationException("大编码格式不正确");
        }

        private void ShowErrorMessage(string message)
        {
            divError.Visible = true;
            divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        private void SetBackButton()
        {
            string returnUrl = ReturnUrl;
            btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }

        #endregion
    }
}