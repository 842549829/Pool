using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.TransactionWeb.OrderModule {
    public partial class CredentialsUpdateLog : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");

            if(!IsPostBack) {
                var id = Request.QueryString["order"];
                var passenger = Request.QueryString["passenger"];
                decimal orderId;
                if(decimal.TryParse(id, out orderId)) {
                    this.lblOrderId.Text = id;
                    bindLogs(orderId, passenger);
                } else {
                    this.lblOrderId.Text = "订单编号错误";
                }
                setBackUrl();
            }
        }
        private void bindLogs(decimal orderId, string passengerIdString) {
            IEnumerable<DataTransferObject.Order.CredentialsUpdateRecordView> records = null;
            Guid passengerId;
            if(Guid.TryParse(passengerIdString, out passengerId)) {
                records = Service.OrderQueryService.QueryCredentialsUpdateRecords(orderId, passengerId);
            } else {
                records = Service.OrderQueryService.QueryCredentialsUpdateRecords(orderId);
            }
            this.logContent.DataSource = from item in records
                                         orderby item.CommitTime descending
                                         select new
                                         {
                                             OperateTime = item.CommitTime,
                                             Operator = item.CommitAccount,
                                             Passenger = item.Passenger,
                                             Original = item.OriginalCredentials,
                                             New = item.NewCredentials,
                                             Status = item.Success ? "成功" : "失败"
                                         };
            this.logContent.DataBind();
        }
        private void setBackUrl() {
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = Request.UrlReferrer.PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
    }
}