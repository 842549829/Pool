using System;
using System.Web.UI;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class base_policy_info : BasePage
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
            NormalPolicy normal = PolicyManageService.GetNormalPolicy(Guid.Parse(Request.QueryString["id"].ToString()));
            if (normal != null)
            {
                if (normal.VoyageType == VoyageType.OneWay)
                {
                    returnFilght.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lblDepartureShowOrHide.Style.Add(HtmlTextWriterStyle.Display, "none");
                    if (normal.IsPeer)
                    {
                        btnTiedian.Visible = true;
                        //btnKoudian.Visible = true;
                    }
                }
                if (normal.VoyageType == VoyageType.OneWayOrRound)
                {
                    if (normal.IsPeer)
                    {
                        btnTiedian.Visible = true;
                        //btnKoudian.Visible = true;
                    }
                }
                hidIds.Value = Request.QueryString["id"];
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
                if (normal.Freezed)
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
        }

        protected void btnlock_Click(object sender, EventArgs e)
        {
            CheckIsVail();
            PolicyManageService.LockPolicy(PolicyType.Normal, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, Guid.Parse(Request.QueryString["id"].ToString()));
            Response.Redirect("./base_policy_manage.aspx");
        }

        protected void btnunlock_Click(object sender, EventArgs e)
        {
            CheckIsVail();
            PolicyManageService.UnLockPolicy(PolicyType.Normal, this.CurrentUser.UserName, this.txtunlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, Guid.Parse(Request.QueryString["id"].ToString()));
            Response.Redirect("./base_policy_manage.aspx");
        }

        protected void btnLog_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PolicyOperatorLog.aspx?id=" + hidIds.Value, true);
        }

        protected void btnTiedian_Click(object sender, EventArgs e)
        {
            Response.Redirect("./tkdian_policy.aspx?id=" + hidIds.Value, true);
        }
    }
}