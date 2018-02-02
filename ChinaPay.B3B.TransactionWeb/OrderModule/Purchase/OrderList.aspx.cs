using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.SMS.Service;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.SystemManagement;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase
{
    public partial class OrderList :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartDate.Text = txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                var ordersStatus = Service.Order.StatusService.GetRoleOrderStatus(OrderRole.Purchaser);
                ChinaPay.B3B.TransactionWeb.PublicClass.Order.BindOrdersStatus(ulOrderStatus, ordersStatus);
                var productValues = (Enum.GetValues(typeof(ProductType)) as ProductType[]).OrderBy(o => (int)o);
                ChinaPay.B3B.TransactionWeb.PublicClass.Order.BindProductType(ulProduct, productValues);
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
                this.hfdOperatorAccount.Value = this.CurrentUser.UserName;
                this.movedTime.InnerHtml = SystemParamService.PurchaseReminderTime.ToString();
            }
        }

        protected bool IsFirstLoad { get; set; }
    }
}