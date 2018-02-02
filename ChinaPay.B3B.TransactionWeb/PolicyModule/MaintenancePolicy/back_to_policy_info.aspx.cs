using System;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class back_to_policy_info : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckIsVail();
            if (!IsPostBack)
            {
                InitlblData();
            }
        }

        private void CheckIsVail()
        {

            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("base_policy_manage.aspx");
            }
            try
            {
                Guid.Parse(Request.QueryString["id"]);
            }
            catch (Exception)
            {
                Response.Redirect("base_policy_manage.aspx");
            }
        }

        private void InitlblData()
        {
            var roundTrip = PolicyManageService.GetRoundTripPolicy(Guid.Parse(Request.QueryString["id"]));
            if (roundTrip == null) return;
            hidIds.Value = Request.QueryString["id"];
            lblAirline.Text = roundTrip.Airline;
            lblArrival.Text = roundTrip.Arrival;
            lblVoyage.Text = roundTrip.VoyageType.GetDescription();
            lblOffice.Text = roundTrip.OfficeCode;
            lblTicket.Text = roundTrip.TicketType.GetDescription();
            lblDeparture.Text = roundTrip.Departure;
            lblDepartureDate.Text = (roundTrip.DepartureDateStart.ToString("yyyy-MM-dd")) + "-" + (roundTrip.DepartureDateEnd.ToString("yyyy-MM-dd"));
            lblArrivalDate.Text = (roundTrip.ReturnDateStart == null ? "" : ((DateTime)roundTrip.ReturnDateStart).ToString("yyyy-MM-dd")) + "-" + (roundTrip.ReturnDateEnd == null ? "" : ((DateTime)roundTrip.ReturnDateEnd).ToString("yyyy-MM-dd"));
            lblCreateTime.Text = roundTrip.StartPrintDate.ToString("yyyy-MM-dd");
            lblBunks.Text = roundTrip.Berths;
            lblDepartureFilght.Text = roundTrip.DepartureFlightsFilterType == LimitType.None ? "所有" : (roundTrip.DepartureFlightsFilterType == LimitType.Include ? "适用以下航班：" + roundTrip.DepartureFlightsFilter : "不适用以下航班：" + roundTrip.DepartureFlightsFilter);
            lblRetnrnFilght.Text = roundTrip.ReturnFlightsFilterType == LimitType.None ? "所有" : (roundTrip.ReturnFlightsFilterType == LimitType.Include ? "适用以下航班：" + roundTrip.ReturnFlightsFilter : "不适用以下航班：" + roundTrip.ReturnFlightsFilter);
            lblDepartureFilghtDate.Text = roundTrip.DepartureDatesFilterType == DateMode.Date ? roundTrip.DepartureDatesFilter : StringOperation.TransferToChinese(roundTrip.DepartureDatesFilter);
            lblReturnFilghtDate.Text = roundTrip.ReturnDatesFilterType == DateMode.Date ? roundTrip.ReturnDatesFilter : StringOperation.TransferToChinese(roundTrip.ReturnDatesFilter);
            lblNeiBu.Text = (roundTrip.InternalCommission * 100).TrimInvaidZero() + "%";
            lblXiaJi.Text = (roundTrip.SubordinateCommission * 100).TrimInvaidZero() + "%";
            lblTongHang.Text = (roundTrip.ProfessionCommission * 100).TrimInvaidZero() + "%";
            lblLock.Text = roundTrip.Freezed == true ? "锁定" : "未锁定";
            lblRemaek.Text = roundTrip.Remark;
            lblPrice.Text = "￥" + roundTrip.Price.TrimInvaidZero();
            lblChang.Text = roundTrip.ChangePNR ? "需要" : "不需要";
            lblLvyou.Text = roundTrip.TravelDays + "天";
            lblDrawerCondition.Text = roundTrip.DrawerCondition;
            if (roundTrip.Freezed)
            {
                btnunlock.Visible = true;
                btnlock.Visible = false;
                unlock.Visible = true;
                @lock.Visible = false;
            }
            else
            {
                btnunlock.Visible = false;
                btnlock.Visible = true;
                unlock.Visible = false;
                @lock.Visible = true;
            }
        }

        protected void btnlock_Click(object sender, EventArgs e)
        {
            CheckIsVail();
            PolicyManageService.LockPolicy(PolicyType.RoundTrip, this.CurrentUser.UserName, this.txtlockReason.Text, Guid.Parse(Request.QueryString["id"]));
            Response.Redirect("./back_to_policy_manage.aspx");
        }

        protected void btnunlock_Click(object sender, EventArgs e)
        {
            CheckIsVail();
            PolicyManageService.UnLockPolicy(PolicyType.RoundTrip, this.CurrentUser.UserName, this.txtunlockReason.Text, Guid.Parse(Request.QueryString["id"]));
            Response.Redirect("./back_to_policy_manage.aspx");
        }

        protected void btnLog_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PolicyOperatorLog.aspx?id=" + hidIds.Value);
        }
    }
}