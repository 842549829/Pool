using System;
using System.Linq;
using ChinaPay.B3B.Service;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;
using System.Text;

namespace ChinaPay.B3B.TransactionWeb.OrderModule
{
    public partial class OrderLog : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                var id = Request.QueryString["id"];
                decimal orderId;
                if (decimal.TryParse(id, out orderId))
                {
                    this.lblOrderId.Text = id;
                    bindOrderLogs(orderId);
                }
                else
                {
                    this.lblOrderId.Text = "订单号错误";
                }
                setBackUrl();
            }
        }
        private void bindOrderLogs(decimal orderId)
        {
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null) return;
            var orderRole = GetOrderRole(order);
            var logs = from item in LogService.QueryOrderLog(orderId)
                       where item.IsVisible(orderRole, CurrentCompany.CompanyId)
                       select new
                       {
                           item.Keyword,
                           OperateTime = item.Time.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                           Detail = Detail(item.Content),
                           Operator = getOperator(item),
                           Applyform = item.ApplyformId
                       };
            logContent.DataSource = logs;
            logContent.DataBind();
        }
        private string Detail(string conten)
        {
            int index = conten.IndexOf("#");
            if (index != -1)
            {
                string first = conten.Substring(0, index);
                string last = conten.Substring(index + 1);
                string[] lastArray = last.Split('|');
                string lastHtml = string.Empty;
                if (lastArray.Length > 1)
                {
                    lastHtml = string.Format("<a href='/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId={0}'>{1}</a>", lastArray[1], lastArray[0]);
                }
                return first + lastHtml;
            }
            else {
                return conten;
            }
            
        }
        private void setBackUrl()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.UrlReferrer.PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private string getOperator(Service.Log.Domain.OrderLog log)
        {
            if (log.Role == OperatorRole.System) return log.Role.GetDescription();
            switch (CurrentCompany.CompanyType)
            {
                case Common.Enums.CompanyType.Platform:
                    return log.Role.GetDescription() + "<br />" + log.Account;
                default:
                    return isCurrentCompany(log.Company) ? log.Account : log.Role.GetDescription();
            }
        }
        private bool isCurrentCompany(Guid operateCompany)
        {
            return CurrentCompany.CompanyId == operateCompany;
        }
    }
}