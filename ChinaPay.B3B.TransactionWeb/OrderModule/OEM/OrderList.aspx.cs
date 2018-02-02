using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using System.Web.UI.WebControls;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.OEM
{
    public partial class OrderList : BasePage
    {
        protected bool IsFirstLoad { get; set; }

        protected string OEMID
        {
            get {
                if (CurrentCompany.CompanyType == CompanyType.Platform)
                {
                    return Request.QueryString["OemId"];
                }
                else return Guid.Empty.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                Dictionary<OrderStatus, string> ordersStatus = StatusService.GetRoleOrderStatus(OrderRole.OEMOwner);
                Order.BindOrdersStatus(ulOrderStatus, ordersStatus);
                IOrderedEnumerable<ProductType> productValues = (Enum.GetValues(typeof (ProductType)) as ProductType[]).OrderBy(o => (int) o);
                Order.BindProductType(ulProduct, productValues);
                ucPurchaser.SetCompanyType(CompanyType.Purchaser | CompanyType.Supplier | CompanyType.Provider);
                BindCarrair();
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
            }
        }

        private void BindCarrair() {
            Carrier.DataSource = FoundationService.Airlines.Select(c => new
            {
                Text = string.Format("{0}-{1}",c.Code,c.ShortName),
                Value = c.Code
            });
            Carrier.DataTextField = "Text";
            Carrier.DataValueField = "Value";
            Carrier.DataBind();
        }
    }
}