using System;
using System.Web.UI;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class base_policy_info : BasePage
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
            NormalPolicy normal = PolicyManageService.GetNormalPolicy(Guid.Parse(Request.QueryString["id"].ToString()));
            if (normal != null)
            {
                if (normal.VoyageType == VoyageType.OneWay)
                {
                    returnFilght.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblDepartureShowOrHide.Style.Add(HtmlTextWriterStyle.Display, "none");
                }

                lblAirline.Text = normal.Airline;
                lblArrival.Text = normal.Arrival;
                lblVoyage.Text = normal.VoyageType.GetDescription();
                lblOffice.Text = normal.OfficeCode;
                lblCutomerCode.Text = normal.CustomCode;
                lblExceptAirlines.Text = normal.ExceptAirways;
                lblExceptDay.Text = normal.DepartureDateFilter;
                lblDepartureWeekFilter.Text = PublicClass.StringOperation.TransferToChinese(normal.DepartureWeekFilter);
                lblTicket.Text = normal.TicketType.GetDescription();
                lblDeparture.Text = normal.Departure;
                lblDepartureDate.Text = (normal.DepartureDateStart.ToString("yyyy-MM-dd")) + "至" + (normal.DepartureDateEnd.ToString("yyyy-MM-dd"));
                lblCreateTime.Text = normal.StartPrintDate.ToString("yyyy-MM-dd");
                lblBunks.Text = normal.Berths;
                lblDepartureFilght.Text = normal.DepartureFlightsFilterType == LimitType.None ? "不限" : (normal.DepartureFlightsFilterType == LimitType.Include ? "适用：" + normal.DepartureFlightsFilter : "不适用：" + normal.DepartureFlightsFilter);
                lblXiaJi.Text = (normal.SubordinateCommission * 100).TrimInvaidZero() == "-1" ? "" : (normal.SubordinateCommission * 100).TrimInvaidZero() + "%";
                if (normal.IsInternal)
                {
                    lblNeiBu.Text = (normal.InternalCommission * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    this.internalTitle.Visible = false;
                    this.internalValue.Visible = false;
                }
                if (normal.IsPeer)
                {
                    lblTongHang.Text = ((normal.ProfessionCommission) * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    this.proffessionTitle.Visible = false;
                    this.proffessionValue.Visible = false;
                }
                lblLock.Text = normal.Freezed ? "锁定" : "未锁定";
                lblRemaek.Text = normal.Remark;
                lblChang.Text = normal.ChangePNR ? "需要" : "不需要";
                lblPrintBeforeTwoHours.Text = normal.PrintBeforeTwoHours ? "可以" : "不可以";
                lblDrawerCondition.Text = normal.DrawerCondition;
                if (normal.VoyageType == VoyageType.OneWayOrRound || normal.VoyageType == VoyageType.RoundTrip || normal.VoyageType == VoyageType.TransitWay)
                {
                    this.lblRetnrnFilght.Text = normal.ReturnFlightsFilterType == LimitType.None ? "所有" : (normal.ReturnFlightsFilterType == LimitType.Include ? "适用以下航班：" + normal.ReturnFlightsFilter : "不适用以下航班：" + normal.ReturnFlightsFilter);
                    lblSuitReduce.Text = normal.SuitReduce ? "适用" : "不适用";
                }
                else
                {
                    suitBerthTitle.Visible = false;
                    suitBerthValue.Visible = false;
                }
                if (normal.VoyageType == VoyageType.TransitWay)
                {
                    this.lblVoyageType.Text = "联程";
                    this.lblDepartureShowOrHide.Text = "第一程";
                    this.lblArrivalShowOrHide.Text = "第二程";
                    this.transit.Visible = true;
                    this.lblTransit.Text = normal.Transit;
                    lblMultiSuitReduce.Text = normal.MultiSuitReduce ? "适用" : "不适用";
                }
                else
                {
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