using System;
using System.Linq;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.TransactionWeb.OrderModule.UserControls;

namespace ChinaPay.B3B.TransactionWeb.OrderModule {
    public partial class OrderHistoryRecord : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                decimal orderId;
                if(decimal.TryParse(Request.QueryString["id"], out orderId)) {
                    LoadOrderInfoHis(orderId);
                } else {
                    ShowMessage("参数错误");
                }
                setBackButton();
            }
        }

        private void LoadOrderInfoHis(decimal orderId) {
            var orderInfo = OrderQueryService.QueryOrder(orderId);
            if(orderInfo == null) {
                ShowMessage("订单不存在！");
                return;
            }
            this.lblOrderId.Text = orderId.ToString();
            applyFormList.DataSource = orderInfo.FinishedApplyforms;
            applyFormList.DataBind();
        }

        protected void applyFormList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e) {
            try
            {
                var ucRefundFormView = e.Item.FindControl("ucRefundFormView") as RefundFormView;
                var ucPostPoneView = e.Item.FindControl("ucPostPoneView") as PostPoneView;
                if (ucRefundFormView == null || ucPostPoneView == null)
                {
                    return;
                }
                if (e.Item.DataItem is PostponeApplyform)
                {
                    var applyform = e.Item.DataItem as PostponeApplyform;
                    ucPostPoneView.InitPostPoneView(applyform, CurrentCompany.CompanyType);
                    ucRefundFormView.Visible = false;
                }
                if (e.Item.DataItem is RefundOrScrapApplyform)
                {
                    var applyform = e.Item.DataItem as RefundOrScrapApplyform;
                    ucRefundFormView.InitRefundFormView(applyform, CurrentCompany.CompanyType);
                    ucPostPoneView.Visible = false;
                }
            }
            catch (Exception ex)
            {
                 ShowExceptionMessage(ex,"查找订单历史");
            }
        }
        private void setBackButton() {
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = Request.UrlReferrer.PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
    }
}