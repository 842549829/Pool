using System;
using System.Linq;
using ChinaPay.B3B.Service;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule {
    public partial class ApplyformLog : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");

            if(!IsPostBack) {
                var id = Request.QueryString["id"];
                decimal applyformId;
                if(decimal.TryParse(id, out applyformId)) {
                    this.lblApplyformId.Text = id;
                    bindOrderLogs(applyformId);
                } else {
                    this.lblApplyformId.Text = "申请单号错误";
                }
                setBackUrl();
            }
        }
        private void bindOrderLogs(decimal applyformId) {
            var applyform = ApplyformQueryService.QueryApplyform(applyformId);
            var orderRole = GetOrderRole(applyform.Order);
            var logs = from item in Service.LogService.QueryApplyformLog(applyformId)
                       where item.IsVisible(orderRole, CurrentCompany.CompanyId)
                       select new
                       {
                           Keyword = item.Keyword,
                           OperateTime = item.Time.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                           Detail = item.Content,
                           Operator = getOperator(item)
                       };
            logContent.DataSource = logs;
            logContent.DataBind();
        }
        private void setBackUrl() {
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = Request.UrlReferrer.PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private string getOperator(Service.Log.Domain.OrderLog log) {
            switch(CurrentCompany.CompanyType) {
                case Common.Enums.CompanyType.Platform:
                    return log.Role.GetDescription() + "<br />" + log.Account;
                default:
                    return isCurrentCompany(log.Company) ? log.Account : log.Role.GetDescription();
            }
        }
        private bool isCurrentCompany(Guid operateCompany) {
            return CurrentCompany.CompanyId == operateCompany;
        }
    }
}