using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Announce;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Integral;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.FlightReserveModule;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb {
    public partial class PurchaseDefault : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("booking.css");
            if (!IsPostBack)
            {
                bindUpdateLog();
                BindAnnouncement();
                BindHeader();
                BindDefaultAirPort();
                ClientScript.RegisterClientScriptInclude(GetType(), "annoucement", ResolveClientUrl("/Scripts/EmergencyAnnounce.js"));
                ServiceTelephone.Text = CurrenContract.ServicePhone;
            }
        }

        private void BindDefaultAirPort() {
            WorkingSetting workingSetting = CompanyService.GetWorkingSetting(CurrentCompany.CompanyId);
            if(workingSetting != null && (!string.IsNullOrWhiteSpace(workingSetting.DefaultDeparture) || !string.IsNullOrWhiteSpace(workingSetting.DefaultDeparture))) {
                // 默认出发城市
                BindDefaultAirportInfo(workingSetting.DefaultDeparture, txtDepartureValue, txtDeparture);
                // 默认到达城市
                BindDefaultAirportInfo(workingSetting.DefaultArrival, txtArrivalValue, txtArrival);
                // 默认出发日期为三天后
                txtGoDate.Value = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            }
            txtIntegral.Text = IntegralServer.GetIntegralByAccountIdZong(CurrentUser.Owner).IntegralAvailable.ToString();
        }

        private void BindDefaultAirportInfo(string code, HtmlInputHidden valueControl, HtmlInputText textControl) {
            if(!string.IsNullOrWhiteSpace(code)) {
                Airport airport = FoundationService.QueryAirport(code);
                if(airport != null && airport.Location != null) {
                    valueControl.Value = airport.Code.Value;
                    textControl.Value = airport.Location.Name + "[" + airport.Code + "]";
                }
            }
        }

        private void BindAnnouncement() {
            var pagination = new Pagination { PageSize = 8, PageIndex = 1 };
            dataList.DataSource = AnnounceService.UserQuery(BasePage.IsOEM ? BasePage.OEM.CompanyId : this.CurrentCompany.IsOem ? this.CurrentCompany.CompanyId : Guid.Empty, IsOEM, this.CurrentCompany.IsOem, pagination);
            dataList.DataBind();
        }

        protected void btnPNRCodeImport_Click(object sender, EventArgs e) {
            bool needImputPat = false;
            try {
                if(radChildrenPNR.Checked) {
                    needImputPat = ImportHelper.ImportByPNRCode(txtAdultPNRCode.Value.Trim(), txtChildrenPNRCode.Value.Trim(), PassengerType.Child, txtPATContent.Value, HttpContext.Current).NeedPAT;
                } else {
                    needImputPat = ImportHelper.ImportByPNRCode(txtPNRCode.Text.Trim(), string.Empty, PassengerType.Adult, txtPATContent.Value, HttpContext.Current).NeedPAT;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "PNR编码导入");
                return;
            }
            if(needImputPat) showPatInput();
            else gotoNextStep();
        }

        private void gotoNextStep() {
            RegisterScript("window.top.location='/FlightReserveModule/ChoosePolicyWithImport.aspx?source=" + ChoosePolicy.ImportSource + "';");
        }

        private void BindHeader() {
            string headerHTML = string.Format("<strong>{0}好，{1}({2})</strong>", getTimezone(), CurrentUser.Name, CurrentUser.UserName);
            if(CurrentUser.LastLoginTime.HasValue) {
                headerHTML += string.Format(" <span>上次登录：{0} 登录地址：{1}({2})</span>",
                    CurrentUser.LastLoginTime.Value.ToString("yyyy年MM月dd日 HH:mm"), CurrentUser.LastLoginLocation, CurrentUser.LastLoginIP);
                if((CurrentCompany.CompanyType == CompanyType.Provider || CurrentCompany.CompanyType == CompanyType.Supplier) &&
                    (CurrentCompany.PeriodEndOfUse.HasValue && CurrentCompany.PeriodEndOfUse.Value.Date < DateTime.Today.Date))
                    headerHTML = " <a href='/About/lxwm.aspx'><strong style='color:red;'>[ 账号使用期限已到，请与平台联系 ]</strong></a>";
            } else {
                headerHTML += " 您是第一次登录";
            }
            userinfor.InnerHtml = headerHTML;
        }

        private string getTimezone() {
            int hour = DateTime.Now.Hour;
            if(hour < 5) {
                return "凌晨";
            }
            if(hour < 8) {
                return "早上";
            }
            if(hour < 11) {
                return "上午";
            }
            if(hour < 13) {
                return "中午";
            }
            if(hour < 17) {
                return "下午";
            }
            return "晚上";
        }

        private void showPatInput() {
            RegisterScript("$(function(){setTimeout(function(){$(\"#popWinTrigger\").click();},200);})");
        }

        protected string PatReg {
            get {
                return Regex.Replace(Service.Command.RegexUtil.PATCmdRegex, @"\?\<.+\>|\(\?:.+\)", string.Empty);
            }
        }

        private void bindUpdateLog() {
            var updateLogs = Service.ReleaseNote.ReleaseNoteService.Query(new Pagination { PageSize = 1 }, null, null, CurrentCompany.CompanyType, ChinaPay.B3B.Common.Enums.ReleaseNoteType.B3BVisible);
            if(updateLogs.Any()) {
                if(updateLogs.FirstOrDefault().UpdateTime.AddDays(3).Date >= DateTime.Now.Date) {
                    newRelease.Visible = true;
                    this.newRelease.HRef = "/Index.aspx?redirectUrl=/ReleaseNoteModule/Releasenote.aspx";
                }
            }
        }
    }
}