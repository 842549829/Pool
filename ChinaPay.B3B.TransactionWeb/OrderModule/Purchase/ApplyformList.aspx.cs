using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core.Extension;
using System.Web.UI;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase {
    public partial class ApplyformList : BasePage {
        protected bool IsFirstLoad
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartDate.Text = this.txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
            }  
        }
        private void initData() {
            //改期状态
            var postPoneStatus = Service.Order.StatusService.GetRolePostponeApplyformStatus(OrderRole.Purchaser).Values.Distinct();
            string strPosponeStatus = 	"<li><a href='javascript:void(0)' class='curr'>全部</a></li>";
            foreach (var item in postPoneStatus)
            {
                strPosponeStatus += "<li><a href='javascript:void(0)'>"+item+"</a></li>";
            }
            this.posponeStauts.InnerHtml = strPosponeStatus;
            //退/废票状态
            var refundStatus = Service.Order.StatusService.GetRoleRefundApplyformStatus(OrderRole.Purchaser).Values.Distinct();
            string strRefundStatus = "<li><a href='javascript:void(0)' class='curr'>全部</a></li>";
            foreach (var item in refundStatus)
            {
                strRefundStatus += "<li><a href='javascript:void(0)'>" + item + "</a></li>";
            }
            this.refundStatus.InnerHtml = strRefundStatus;

            //差错退款状态
            var balanceRefundStatus = Service.Order.StatusService.GetBalanceRefundStatus(OrderRole.Purchaser).Values.Distinct();
            string strBalanceRefundStatus = "<li><a href='javascript:void(0)' class='curr'>全部</a></li>";
            foreach (var item in balanceRefundStatus)
            {
                strBalanceRefundStatus += "<li><a href='javascript:void(0)'>" + item + "</a></li>";
            }
            this.balanceRefundstatus.InnerHtml = strBalanceRefundStatus;

            //申请单状态
            var applyTypeValues = Enum.GetValues(typeof(ApplyformType)) as ApplyformType[];
            string strApplyType = "<li><a href='javascript:void(0)'>综合查询</a></li>";
            foreach(var item in applyTypeValues) {
                strApplyType += "<li><a href='javascript:void(0)'>"+item.GetDescription()+"</a></li>";
            }
            this.applyType.InnerHtml = strApplyType;

            this.hfdApplyformType.Value = "综合查询";
            this.posponeStauts.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.refundStatus.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.balanceRefundstatus.Style.Add(HtmlTextWriterStyle.Display, "none");
        }
    }
}