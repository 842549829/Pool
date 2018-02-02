using System;
using System.Web.UI;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class notch_policy_info : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
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
                Guid.Parse(Request.QueryString["id"].ToString());
            }
            catch (Exception)
            {
                Response.Redirect("base_policy_manage.aspx");
            }
        }

        private void InitlblData()
        {
            NotchPolicy notch = PolicyManageService.GetNotchPolicy(Guid.Parse(Request.QueryString["id"].ToString()));
            if (notch != null)
            { 
                hidIds.Value = Request.QueryString["id"];
                string str = "";
                foreach (var item in notch.DepartureArrival)
                {
                    str += (item.IsAllowable ? "适用航段：" : "排除航段：") + item.Departure + " 至 " + item.Arrival + " <br />";
                }
                lblDeparture.InnerHtml = str;
                lblAirline.Text = notch.Airline;
                //lblArrival.Text = notch.Arrival;
                lblVoyage.Text = notch.VoyageType.GetDescription();
                lblOffice.Text = notch.OfficeCode;
                lblCutomerCode.Text = notch.CustomCode;
                //lblExceptAirlines.Text = notch.ExceptAirways;
                lblExceptDay.Text = notch.DepartureDateFilter;
                lblDepartureWeekFilter.Text = PublicClass.StringOperation.TransferToChinese(notch.DepartureWeekFilter);
                lblTicket.Text = notch.TicketType.GetDescription();
                //lblDeparture.Text = notch.Departure;
                lblDepartureDate.Text = (notch.DepartureDateStart.ToString("yyyy-MM-dd")) + "至" + (notch.DepartureDateEnd.ToString("yyyy-MM-dd"));
                lblCreateTime.Text = notch.StartPrintDate.ToString("yyyy-MM-dd");
                lblBunks.Text = notch.Berths;
                lblDepartureFilght.Text = notch.DepartureFlightsFilterType == LimitType.None ? "不限" : (notch.DepartureFlightsFilterType == LimitType.Include ? "适用：" + notch.DepartureFlightsFilter : "不适用：" + notch.DepartureFlightsFilter);
                lblXiaJi.Text = (notch.SubordinateCommission * 100).TrimInvaidZero() == "-1" ? "" : (notch.SubordinateCommission * 100).TrimInvaidZero() + "%";
                if (notch.IsInternal)
                {
                    lblNeiBu.Text = (notch.InternalCommission * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    this.internalTitle.Visible = false;
                    this.internalValue.Visible = false;
                }
                if (notch.IsPeer)
                {
                    lblTongHang.Text = ((notch.ProfessionCommission) * 100).TrimInvaidZero() + "%";
                }
                else
                {
                    this.proffessionTitle.Visible = false;
                    this.proffessionValue.Visible = false;
                }
                lblLock.Text = notch.Freezed ? "锁定" : "未锁定";
                lblRemaek.Text = notch.Remark;
                lblChang.Text = notch.ChangePNR ? "需要" : "不需要";
                lblPrintBeforeTwoHours.Text = notch.PrintBeforeTwoHours ? "可以" : "不可以";
                lblDrawerCondition.Text = notch.DrawerCondition;
                if (notch.Freezed)
                {
                    this.btnunlock.Visible = true;
                    this.btnlock.Visible = false;
                    this.unlock.Visible = true;
                    this.@lock.Visible = false;
                }
                else
                {
                    this.btnunlock.Visible = false;
                    this.btnlock.Visible = true;
                    this.unlock.Visible = false;
                    this.@lock.Visible = true;
                }
                //if (notch.VoyageType == VoyageType.OneWayOrRound || notch.VoyageType == VoyageType.RoundTrip || notch.VoyageType == VoyageType.TransitWay)
                //{
                //    this.lblRetnrnFilght.Text = notch.ReturnFlightsFilterType == LimitType.None ? "所有" : (notch.ReturnFlightsFilterType == LimitType.Include ? "适用以下航班：" + notch.ReturnFlightsFilter : "不适用以下航班：" + notch.ReturnFlightsFilter);
                //    lblSuitReduce.Text = notch.SuitReduce ? "适用" : "不适用";
                //}
                //else
                //{
                //    suitBerthTitle.Visible = false;
                //    suitBerthValue.Visible = false;
                //}
                //if (notch.VoyageType == VoyageType.TransitWay)
                //{
                //    this.lblVoyageType.Text = "联程";
                //    this.lblDepartureShowOrHide.Text = "第一程";
                //    this.lblArrivalShowOrHide.Text = "第二程";
                //    this.transit.Visible = true;
                //    this.lblTransit.Text = notch.Transit;
                //    lblMultiSuitReduce.Text = notch.MultiSuitReduce ? "适用" : "不适用";
                //}
                //else
                //{
                //    duoduanTitle.Visible = false;
                //    duoduanValue.Visible = false;
                //}
            }
        }

        protected void btnlock_Click(object sender, EventArgs e)
        {
            CheckIsVail();
            PolicyManageService.LockPolicy(PolicyType.Notch, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, Guid.Parse(Request.QueryString["id"].ToString()));
            Response.Redirect("./notch_policy_manage.aspx");
        }

        protected void btnunlock_Click(object sender, EventArgs e)
        {
            CheckIsVail();
            PolicyManageService.UnLockPolicy(PolicyType.Notch, this.CurrentUser.UserName, this.txtunlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, Guid.Parse(Request.QueryString["id"].ToString()));
            Response.Redirect("./notch_policy_manage.aspx");
        }

        protected void btnLog_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PolicyOperatorLog.aspx?id=" + hidIds.Value, true);
        }
         
    }
}