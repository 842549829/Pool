using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class WaitOrderListNew : BasePage
    {
        protected bool IsFirstLoad { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                this.speed.Visible = this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Provider;
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
                initData();
            }
        }

        private void initData()
        {
            var productValues = Enum.GetValues(typeof(ProductType)) as ProductType[];
            foreach (ProductType item in productValues)
            {
                ddlProduct.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }


            ddlCarrier.DataTextField = "ShortName";
            ddlCarrier.DataValueField = "Code";
            ddlCarrier.DataSource = from item in FoundationService.Airlines.Join(PolicySetService.QueryAirlines(CurrentCompany.CompanyId), p => p.Code.Value, p => p, (p, q) => p)
                                          select new
                                          {
                                              ShortName = item.Code + "-" + item.ShortName,
                                              Code = item.Code
                                          };
            ddlCarrier.DataBind();

            ddlOfficeNumber.DataSource = CompanyService.QueryOfficeNumbers(CurrentCompany.CompanyId).Select(o => o.Number);
            ddlOfficeNumber.DataBind();

            var orderRole = this.CurrentCompany.CompanyType== Common.Enums.CompanyType.Supplier ? OrderRole.Supplier : OrderRole.Provider;
            this.ddlStatus.Items.Add(new ListItem("全部", string.Empty));
            this.ddlStatus.Items.Add(new ListItem(Service.Order.StatusService.GetOrderStatus(OrderStatus.Applied, orderRole), ((int)OrderStatus.Applied).ToString()));
            this.ddlStatus.Items.Add(new ListItem(Service.Order.StatusService.GetOrderStatus(OrderStatus.PaidForSupply, orderRole), ((int)OrderStatus.PaidForSupply).ToString()));
            this.ddlStatus.Items.Add(new ListItem(Service.Order.StatusService.GetOrderStatus(OrderStatus.PaidForETDZ, orderRole), ((int)OrderStatus.PaidForETDZ).ToString()));
        }
    }
}