using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.OEM
{
    public partial class ApplyformList : BasePage
    {
        protected bool IsFirstLoad;

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                txtAppliedDateStart.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                txtAppliedDateEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
            }
        }

        private void initData()
        {
            //申请单状态
            var applyTypeValues = Enum.GetValues(typeof(ApplyformType)) as ApplyformType[];
            string strApplyType = "<li><a href='javascript:void(0)'>综合查询</a></li>";
            foreach (var item in applyTypeValues)
            {
                strApplyType += "<li><a href='javascript:void(0)'>" + item.GetDescription() + "</a></li>";
            }
            this.applyType.InnerHtml = strApplyType;

            //改期状态
            var postPoneStatus = Service.Order.StatusService.GetRolePostponeApplyformStatus(OrderRole.OEMOwner).Values.Distinct();
            string strPosponeStatus = "<li><a href='javascript:void(0)' class='curr'>全部</a></li>";
            foreach (var item in postPoneStatus)
            {
                strPosponeStatus += "<li><a href='javascript:void(0)'>" + item + "</a></li>";
            }
            this.posponeStauts.InnerHtml = strPosponeStatus;
            //退票类型
            var refundTypeValue = Enum.GetValues(typeof(RefundType)) as RefundType[];
            string strRefundType = "<li><a href='javascript:void(0)' class='curr'>全部</a></li>";
            foreach (var item in refundTypeValue)
            {
                strRefundType += "<li><a href='javascript:void(0)'>" + item.GetDescription() + "</a></li>";
            }
            refundType.InnerHtml = strRefundType;
            //退/废票状态
            var refundStatus = Service.Order.StatusService.GetRoleRefundApplyformStatus(OrderRole.Platform).Values.Distinct();
            string strRefundStatus = "<li><a href='javascript:void(0)' class='curr'>全部</a></li>";
            foreach (var item in refundStatus)
            {
                strRefundStatus += "<li><a href='javascript:void(0)'>" + item + "</a></li>";
            }
            this.refundStatus.InnerHtml = strRefundStatus;
            this.hfdRefundType.Value = "全部";
            this.hfdApplyformType.Value = "综合查询";
            this.refundType.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.posponeStauts.Style.Add(HtmlTextWriterStyle.Display, "none");
            this.refundStatus.Style.Add(HtmlTextWriterStyle.Display, "none");
            //var companies = CompanyService.GetCompanies(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier);
            //AgentCompany.SetCompanyType(CompanyType.Provider);
            PurchaseCompany.SetCompanyType(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier);
           // SupplierCompany.SetCompanyType(CompanyType.Supplier);
        }

        private string getDiscountText(decimal? discount)
        {
            if (discount.HasValue)
            {
                return (discount.Value * 100).TrimInvaidZero();
            }
            return "-";
        }
    }
}