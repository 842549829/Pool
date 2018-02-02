using System;
using System.Web.UI;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class team_policy_info : BasePage
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
            TeamPolicy team = PolicyManageService.GetTeamPolicy(Guid.Parse(Request.QueryString["id"].ToString()));
            if (team != null)
            {
                if (team.VoyageType == VoyageType.OneWay)
                {
                    returnFilght.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblDepartureShowOrHide.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
               
                lblAirline.Text = team.Airline;
                lblArrival.Text = team.Arrival;
                lblVoyage.Text = team.VoyageType.GetDescription();
                lblOffice.Text = team.OfficeCode;
                lblCutomerCode.Text = team.CustomCode;
                lblExceptAirlines.Text = team.ExceptAirways;
                lblExceptDay.Text = team.DepartureDateFilter;
                lblDepartureWeekFilter.Text = PublicClass.StringOperation.TransferToChinese(team.DepartureWeekFilter);
                lblTicket.Text = team.TicketType.GetDescription();
                lblDeparture.Text = team.Departure;
                lblDepartureDate.Text = (team.DepartureDateStart.ToString("yyyy-MM-dd")) + "至" + (team.DepartureDateEnd.ToString("yyyy-MM-dd"));
                lblCreateTime.Text = team.StartPrintDate.ToString("yyyy-MM-dd");
                lblBunks.Text = team.Berths;
                lblDepartureFilght.Text = team.DepartureFlightsFilterType == LimitType.None ? "所有" : (team.DepartureFlightsFilterType == LimitType.Include ? "适用：" + team.DepartureFlightsFilter : "不适用：" + team.DepartureFlightsFilter);
                lblXiaJi.Text = (team.SubordinateCommission * 100).TrimInvaidZero() + "%";
                lblPrintBeforeTwoHours.Text = team.PrintBeforeTwoHours ? "可以" : "不可以";
                if (team.IsInternal)
                {
                    lblNeiBu.Text = (team.InternalCommission * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    this.internalTitle.Visible = false;
                    this.internalValue.Visible = false;
                }
                if (team.IsPeer)
                {
                    lblTongHang.Text = ((team.ProfessionCommission) * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    this.proffessionTitle.Visible = false;
                    this.proffessionValue.Visible = false;
                }
                lblLock.Text = team.Freezed ? "锁定" : "未锁定";
                lblRemaek.Text = team.Remark;
                lblChang.Text = team.ChangePNR ? "需要" : "不需要";

                lblDrawerCondition.Text = team.DrawerCondition;
                if (team.VoyageType == VoyageType.OneWayOrRound || team.VoyageType == VoyageType.RoundTrip || team.VoyageType == VoyageType.TransitWay)
                {
                    this.lblRetnrnFilght.Text = team.ReturnFlightsFilterType == LimitType.None ? "不限" : (team.ReturnFlightsFilterType == LimitType.Include ? "适用以下航班：" + team.ReturnFlightsFilter : "不适用以下航班：" + team.ReturnFlightsFilter);
                    lblSuitReduce.Text = team.SuitReduce ? "适用" : "不适用";
                }
                else
                {
                    suitBerthTitle.Visible = false;
                    suitBerthValue.Visible = false;
                }
                if (team.VoyageType == VoyageType.TransitWay)
                {
                    this.lblVoyageType.Text = "联程";
                    this.lblDepartureShowOrHide.Text = "第一程";
                    this.lblArrivalShowOrHide.Text = "第二程";
                    this.transit.Visible = true;
                    this.lblTransit.Text = team.Transit;
                    lblMultiSuitReduce.Text = team.MultiSuitReduce ? "适用" : "不适用";
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