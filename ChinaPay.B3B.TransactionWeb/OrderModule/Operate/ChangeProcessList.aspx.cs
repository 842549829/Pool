using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using Enumerable = System.Linq.Enumerable;
using System.Web.UI;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class ChangeProcessList : BasePage
    {
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
            //退票状态
            var refundTypeValues = Enum.GetValues(typeof(RefundType)) as RefundType[];
            string strApplyType = "<li><a href='javascript:void(0)'>综合查询</a></li>";
            foreach (var item in refundTypeValues)
            {
                strApplyType += "<li><a href='javascript:void(0)'>" + item.GetDescription() + "</a></li>";
            }
            this.refundType.InnerHtml = strApplyType;

            this.hfdRefundType.Value = "综合查询";
        }

        protected bool IsFirstLoad { get; set; }
    }
}