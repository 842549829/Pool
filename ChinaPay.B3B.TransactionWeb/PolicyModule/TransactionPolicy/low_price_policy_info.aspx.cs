using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class low_price_policy_info : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitlblData();
                setBackButton();
            }
        }

        private void InitlblData()
        {
            BargainPolicy Bargain = PolicyManageService.GetBargainPolicy(Guid.Parse(Request.QueryString["id"].ToString()));
            if (Bargain != null)
            {
                lblAirline.Text = Bargain.Airline;
                lblArrival.Text = Bargain.Arrival;
                lblVoyage.Text = Bargain.VoyageType.GetDescription();
                lblOffice.Text = Bargain.OfficeCode;
                lblCutomerCode.Text = Bargain.CustomCode;
                lblExceptDay.Text = Bargain.DepartureDateFilter;
                lblTicket.Text = Bargain.TicketType.GetDescription();
                lblDeparture.Text = Bargain.Departure;
                lblDepartureDate.Text = (Bargain.DepartureDateStart.ToString("yyyy-MM-dd")) + "至" + (Bargain.DepartureDateEnd.ToString("yyyy-MM-dd"));
                lblCreateTime.Text = Bargain.StartPrintDate.ToString("yyyy-MM-dd");
                lblBunks.Text = Bargain.Berths;
                lblDepartureFilght.Text = Bargain.DepartureFlightsFilterType == LimitType.None ? "不限" : (Bargain.DepartureFlightsFilterType == LimitType.Include ? "适用以下航班：" + Bargain.DepartureFlightsFilter : "不适用以下航班：" + Bargain.DepartureFlightsFilter);
                lblDepartureDateFilght.Text = PublicClass.StringOperation.TransferToChinese(Bargain.DepartureWeekFilter);
                lblRetreat.Text = "作废规定：" + Bargain.InvalidRegulation + "<br />"
                               + "改签规定：" + Bargain.ChangeRegulation + "<br />"
                               + "签转规定：" + Bargain.EndorseRegulation + "<br />"
                               + "退票规定：" + Bargain.RefundRegulation + "<br />";
                if (Bargain.IsInternal)
                {
                    lblNeiBu.Text = (Bargain.InternalCommission * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    this.internalTitle.Visible = false;
                    this.internalValue.Visible = false;
                }
                lblXiaJi.Text = (Bargain.SubordinateCommission * 100).TrimInvaidZero() == "-1" ? "" : (Bargain.SubordinateCommission * 100).TrimInvaidZero() + "%";
                if (Bargain.IsPeer)
                {
                    lblTongHang.Text = (Bargain.ProfessionCommission * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    professionTitle.Visible = false;
                    professionValue.Visible = false;
                }
                lblLock.Text = Bargain.Freezed == true ? "锁定" : "未锁定";
                lblRemaek.Text = Bargain.Remark;
                lblChang.Text = Bargain.ChangePNR ? "需要" : "不需要";
                lblDays.Text = "最少提前天数：" + (Bargain.BeforehandDays > -1 ? Bargain.BeforehandDays + "天" : "");
                lblDays.Text += "最多提前天数：" + (Bargain.MostBeforehandDays > -1 ? Bargain.MostBeforehandDays + "天" : "无");
                lblPrintBeforeTwoHours.Text = Bargain.PrintBeforeTwoHours ? "可以" : "不可以";
                lblDrawerCondition.Text = Bargain.DrawerCondition;
                if (Bargain.VoyageType == VoyageType.OneWay)
                {
                    lblDepartuerShowOrHide.Visible = false;
                    returnFilght.Visible = false;
                }
                else
                {
                    this.lblRetnrnFilght.Text = Bargain.ReturnFlightsFilterType == LimitType.None ? "所有" : (Bargain.ReturnFlightsFilterType == LimitType.Include ? "适用：" + Bargain.ReturnFlightsFilter : "不适用：" + Bargain.ReturnFlightsFilter);
                }

                if (Bargain.VoyageType == VoyageType.RoundTrip)
                {
                    exceptAirlinesTitle.Visible = false;
                    this.travelDayTitle.Visible = true;
                    this.travelDayValue.Visible = true;
                    this.lblTravelDays.Text = Bargain.TravelDays.ToString();
                }
                else
                {
                    this.lblExceptAirlines.Text = Bargain.ExceptAirways;
                }
                if (Bargain.VoyageType == VoyageType.TransitWay)
                {
                    priceTitle.Visible = false;
                    priceValue.Visible = false;
                    transit.Visible = true;
                    lblTransit.Text = Bargain.Transit;
                    lblDepartuerShowOrHide.Text = "第一程";
                    lblArrivalShowOrHide.Text = "第二程";
                    lblMultiSuitReduce.Text = Bargain.MultiSuitReduce ? "适用" : "不适用";
                }
                else
                {
                    lblPice.Text = Bargain.PriceType == PriceType.Discount ? (Bargain.Price * 100).TrimInvaidZero() + "折" : (Bargain.PriceType == PriceType.Commission ? "按返佣" : (Bargain.Price > 0 ? (Bargain.Price.TrimInvaidZero() + "元") : ""));
                    duoduanTitle.Visible = false;
                    duoduanValue.Visible = false;
                }
            }
            else
            {
                ShowMessage("该政策已不存在");
            }
        }

        private void setBackButton()
        {
            // 返回
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
    }
}