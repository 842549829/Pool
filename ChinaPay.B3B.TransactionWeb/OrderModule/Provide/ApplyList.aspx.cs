using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class ApplyList : BasePage
    {
        protected bool IsSupplier;

        protected bool IsFirstLoad
        {
            get;
            set;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            IsSupplier = CurrentCompany.CompanyType == CompanyType.Supplier;
            this.hfdCompanyType.Value = CurrentCompany.CompanyType == CompanyType.Supplier ? "产品方" : "出票方";
            if (!IsPostBack)
            {
                txtAppliedDateStart.Text = txtAppliedDateEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
            }
        }

        private void initData()
        {
            //申请单状态
            var applyTypeValues = Enum.GetValues(typeof(ApplyformType)) as ApplyformType[];
            string strApplyType ="<li><a href='javascript:void(0)'>综合查询</a></li>";
            foreach (var item in applyTypeValues)
            {
                if(item != ApplyformType.Postpone)
                strApplyType += "<li><a href='javascript:void(0)'>" + item.GetDescription() + "</a></li>";
            }
            this.applyType.InnerHtml = strApplyType;

            //退/废票状态
            var refundStatus = Service.Order.StatusService.GetRoleRefundApplyformStatus(this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier).Values.Distinct();
            string strRefundStatus = "<li><a href='javascript:void(0)' class='curr'>全部</a></li>";
            foreach (var item in refundStatus)
            {
                strRefundStatus += "<li><a href='javascript:void(0)'>" + item + "</a></li>";
            }
            this.refundStatus.InnerHtml = strRefundStatus;

            this.hfdApplyformType.Value = "综合查询";
            this.refundStatus.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

    }
}