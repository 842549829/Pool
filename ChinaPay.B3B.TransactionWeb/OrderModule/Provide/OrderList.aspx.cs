using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class OrderList : BasePage
    {
        protected bool IsSupplier;

        protected bool IsFirstLoad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            IsSupplier = CurrentCompany.CompanyType == CompanyType.Supplier;
            if (!IsPostBack)
            {
                txtStartDate.Text = txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                var ordersStatus = Service.Order.StatusService.GetRoleOrderStatus(IsSupplier ? OrderRole.Supplier : OrderRole.Provider);
                ChinaPay.B3B.TransactionWeb.PublicClass.Order.BindOrdersStatus(ulOrderStatus, ordersStatus);
                var productValues = (Enum.GetValues(typeof(ProductType)) as ProductType[]).
                    Where(p => !IsSupplier || p == ProductType.Special).OrderBy(o => (int)o);
                ChinaPay.B3B.TransactionWeb.PublicClass.Order.BindProductType(ulProduct, productValues);
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
                divEfficiency.Visible = !IsSupplier;
            }
        }
    }
}