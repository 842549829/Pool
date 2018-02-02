using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service.Announce;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb {
    public partial class TicketDefault : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                bindHeader();
                bindUpdateLog();
                bindRemind();
                bindSuspendedInfo();
                bindAnnounce();
            }
        }

        private void bindHeader() {
            var headerHTML = string.Format("<strong>{0}好，{1}({2})</strong>", getTimezone(), CurrentUser.Name, CurrentUser.UserName);
            if(CurrentUser.LastLoginTime.HasValue) {
                headerHTML += string.Format(" <span>上次登录：{0} 登录地址：{1}({2})</span>",
                    CurrentUser.LastLoginTime.Value.ToString("yyyy年MM月dd日 HH:mm"), CurrentUser.LastLoginLocation, CurrentUser.LastLoginIP);
                if((this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Provider || this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Supplier) && (this.CurrentCompany.PeriodEndOfUse.HasValue && this.CurrentCompany.PeriodEndOfUse.Value.Date < DateTime.Today.Date))
                    headerHTML += " <a target='_blank' href='/About/lxwm.aspx'><strong style='color:red;'>[ 账号使用期限已到，请与平台联系 ]</strong></a>";
            } else {
                headerHTML += " 您是第一次登录";
            }
            this.divHeader.InnerHtml = headerHTML;
        }
        private string getTimezone() {
            var hour = DateTime.Now.Hour;
            if(hour < 5) {
                return "凌晨";
            } else if(hour < 8) {
                return "早上";
            } else if(hour < 11) {
                return "上午";
            } else if(hour < 13) {
                return "中午";
            } else if(hour < 17) {
                return "下午";
            } else {
                return "晚上";
            }
        }
        private void bindRemind() {
            IEnumerable<KeyValuePair<string, KeyValuePair<string, int>>> remindInfo = null;
            switch(CurrentCompany.CompanyType) {
                case Common.Enums.CompanyType.Provider:
                    remindInfo = getProviderRemind(CurrentCompany.CompanyId);
                    divSwf.Visible = false;
                    break;
                case Common.Enums.CompanyType.Supplier:
                    remindInfo = getSupplierRemind(CurrentCompany.CompanyId);
                    divSwf.Visible = false;
                    break;
                default:
                    this.divRemind.Visible = false;
                    divSwf.Visible = true;
                    return;
            }
            var remindHTML = new StringBuilder();
            remindHTML.Append("<h3>订单提醒</h3>");
            foreach(var item in remindInfo) {
                remindHTML.AppendFormat("<p><a href='{1}'>{0}：<span  class='obvious'>{2}</span></a></p>", item.Key, item.Value.Key, item.Value.Value);
            };
            if (CurrentCompany.CompanyType==CompanyType.Provider)
            {
                var lastWeekSpeedStatInfo = Service.Report.ReportService.QueryProviderETDZSpeedStatistics(new ETDZSpeedStatCondition()
                {
                    Provider = CurrentCompany.CompanyId,
                    StartStatTime = DateTime.Today.AddDays(-7),
                    EndStatTime = DateTime.Today,
                    StatGroup = new SpeedStatGroup()
                }, new Pagination
                {
                    GetRowCount = true,
                    PageSize = 10,
                    PageIndex = 1
                });
                var speed = lastWeekSpeedStatInfo.Rows.Count > 0 ? (Math.Round((int)lastWeekSpeedStatInfo.Rows[0]["Speed"] / 60.0)).ToString() : "0";
                remindHTML.AppendFormat("<p><a href='{1}'>{0}：<span  class='obvious'>平均{2}分钟</span></a></p>", "最近一周出票效率", "/ReportModule/ProvideETDZSpeedStatisticReport.aspx", speed);
            }

            this.divRemind.InnerHtml = remindHTML.ToString();
        }
        private IEnumerable<KeyValuePair<string, KeyValuePair<string, int>>> getProviderRemind(Guid provider) {
            var companySetting = CompanyService.GetWorkingSetting(CurrentCompany.CompanyId);
            bool isWorkOnCustomerNo = companySetting != null && companySetting.IsImpower;
            var result = new List<KeyValuePair<string, KeyValuePair<string, int>>>();
            var remindInfo = Service.Remind.OrderRemindService.QueryProviderRemindInfo(provider);
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetOrderStatus(OrderStatus.Ordered, OrderRole.Provider), new KeyValuePair<string, int>("/OrderModule/Purchase/PayOrderList.aspx", remindInfo.PayOrder)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetPostponeApplyformStatus(PostponeApplyformStatus.Agreed, OrderRole.Provider), new KeyValuePair<string, int>("/OrderModule/Purchase/ApplyformList.aspx", remindInfo.PayPostponeFee)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetOrderStatus(OrderStatus.PaidForETDZ, OrderRole.Provider), new KeyValuePair<string, int>("/OrderModule/Provide/WaitOrderListNew.aspx", isWorkOnCustomerNo ? Service.Order.StatusService.GetOrderTodoCount(OrderStatus.PaidForETDZ, CurrentUser.Id, CurrentCompany.CompanyId) : remindInfo.ETDZ)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetRefundApplyformStatus(RefundApplyformStatus.AppliedForProvider, OrderRole.Provider), new KeyValuePair<string, int>("/OrderModule/Provide/ChangeProcessList.aspx", remindInfo.Scrap)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetRefundApplyformStatus(RefundApplyformStatus.AgreedByProviderBusiness, OrderRole.Provider), new KeyValuePair<string, int>("/OrderModule/Provide/ReturnMoneyList.aspx", remindInfo.ReturnMoney)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetOrderStatus(OrderStatus.Applied, OrderRole.Provider), new KeyValuePair<string, int>("/OrderModule/Provide/WaitOrderListNew.aspx?type=Applied", remindInfo.Confirm)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetOrderStatus(OrderStatus.PaidForSupply, OrderRole.Provider), new KeyValuePair<string, int>("/OrderModule/Provide/WaitOrderListNew.aspx?type=PaidForSupply", remindInfo.Supply)));
            return result;
        }
        private IEnumerable<KeyValuePair<string, KeyValuePair<string, int>>> getSupplierRemind(Guid supplier) {
            var result = new List<KeyValuePair<string, KeyValuePair<string, int>>>();
            var remindInfo = Service.Remind.OrderRemindService.QuerySupplierRemindInfo(supplier);
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetOrderStatus(OrderStatus.Applied, OrderRole.Supplier), new KeyValuePair<string, int>("/OrderModule/Provide/ResourceList.aspx?type=Applied", remindInfo.Confirm)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetOrderStatus(OrderStatus.PaidForSupply, OrderRole.Supplier), new KeyValuePair<string, int>("/OrderModule/Provide/ResourceList.aspx?type=PaidForSupply", remindInfo.Supply)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetOrderStatus(OrderStatus.Ordered, OrderRole.Supplier), new KeyValuePair<string, int>("/OrderModule/Purchase/PayOrderList.aspx", remindInfo.PayOrder)));
            result.Add(new KeyValuePair<string, KeyValuePair<string, int>>(Service.Order.StatusService.GetPostponeApplyformStatus(PostponeApplyformStatus.Agreed, OrderRole.Supplier), new KeyValuePair<string, int>("/OrderModule/Purchase/PayOrderList.aspx", remindInfo.PayPostponeFee)));
            return result;
        }
        private void bindSuspendedInfo() {
            switch(CurrentCompany.CompanyType) {
                case Common.Enums.CompanyType.Provider:
                case Common.Enums.CompanyType.Supplier:
                    bindSuspendedPolicies();
                    break;
                default:
                    this.divSuspendedPolicy.Visible = false;
                    break;
            }
        }
        private void bindSuspendedPolicies() {
            var suspendPolicies = Service.Policy.PolicyManageService.GetSuspendInfo(CurrentCompany.CompanyId);
            var suspendPolicyHTML = "<ul>";
            suspendPolicyHTML += string.Format("<li><h2>平台挂起政策：</h2><div><p class='obvious'>{0}</p></div></li>",
                suspendPolicies.SuspendByPlatform.Join(" "));
            suspendPolicyHTML += string.Format("<li><h2>本单位挂起政策：</h2><div><p class='obvious'>{0}</p></div></li>",
                suspendPolicies.SuspendByCompany.Join(" "));
            suspendPolicyHTML += "</ul>";
            this.divSuspendedPolicy.InnerHtml = suspendPolicyHTML;
        }
        private void bindAnnounce() {
            if(CurrentCompany.CompanyType == Common.Enums.CompanyType.Platform) {
                this.right.Visible = false;
            } else {
                var announceHTML = new StringBuilder();
                announceHTML.Append("<ul class='info-list'>");
                //Guid company = AnnounceService.QueryPlatForm();
                //if(IsOEM)
                //    company = OEM.CompanyId;
                //var announceList = AnnounceService.UserQuery(company, IsOEM);

                var pagination = new Pagination { PageSize = 10, PageIndex = 1 };
                var announceList = AnnounceService.UserQuery(BasePage.IsOEM ? BasePage.OEM.CompanyId : this.CurrentCompany.IsOem ? this.CurrentCompany.CompanyId : Guid.Empty, IsOEM, this.CurrentCompany.IsOem, pagination);

                foreach(var item in announceList) {
                    announceHTML.AppendFormat("<li title='{3}'><a href='SystemSettingModule/Role/AnnounceInfo.aspx?Id={0}'>{1}</a><span class='date'>{2}</span></li>", item.Id, item.Title.Length > 15 ? item.Title.Substring(0, 15) + "..." : item.Title, item.PublishTime.Date.ToShortDateString(), item.Title);
                }
                announceHTML.Append("<li><a href='SystemSettingModule/Role/AnnounceList.aspx' style='float:right'>更多......</a>");
                announceHTML.Append("</ul>");
                this.divAnnounce.InnerHtml = announceHTML.ToString();
            }
        }
        private void bindUpdateLog() {
            var updateLogs = Service.ReleaseNote.ReleaseNoteService.Query(new Pagination { PageSize = 1 }, null, null, CurrentCompany.CompanyType, Common.Enums.ReleaseNoteType.B3BVisible);
            if(updateLogs.Any()) {
                if(updateLogs.FirstOrDefault().UpdateTime.AddDays(3).Date >= DateTime.Now.Date) {
                    newRelease.Visible = true;
                    this.newRelease.HRef = "/Index.aspx?redirectUrl=/ReleaseNoteModule/Releasenote.aspx";
                }
            }
        }
    }
}