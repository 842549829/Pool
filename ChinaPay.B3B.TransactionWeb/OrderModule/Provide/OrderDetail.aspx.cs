using System;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;
using System.Web.UI.HtmlControls;
using ChinaPay.B3B.Service.Order.Domain;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class OrderDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                var id = Request.QueryString["id"];
                decimal orderId;
                if (decimal.TryParse(id, out orderId))
                {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if (order != null)
                    {
                        if ((isSupplier() && order.Supplier != null) || (!isSupplier() && order.Provider != null))
                        {
                            bindOrder(order);
                            setButtons(order);
                            return;
                        }
                    }
                }
                this.divError.Visible = true;
                this.divError.InnerHtml = "<h2>订单不存在</h2>";
                form1.Visible = false;
            }
        }
        private void bindOrder(Service.Order.Domain.Order order)
        {
            bindOrderHeader(order);
            bindPNRGroups(order);
            bindPolicyRemark(order);
            bindOrderBills(order);
        }
        private void bindOrderHeader(Service.Order.Domain.Order order)
        {
            this.divHeader.Visible = true;
            this.lblOrderId.Text = order.Id.ToString();
            if (isProvider())
            {
                this.lblStatus.Text = Service.Order.StatusService.GetOrderStatus(order.Status, DataTransferObject.Order.OrderRole.Provider);
                this.lblAmount.Text = order.Provider.Amount.ToString("F2");
                var product = order.IsThirdRelation && GetOrderRole(order) != OrderRole.Provider ? order.Supplier.Product : order.Provider.Product;
                if (product is SpeicalProductInfo)
                {
                    var specialProductInfo = product as SpeicalProductInfo;
                    this.lblProductType.Text = order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
                }
                else
                {
                    this.lblProductType.Text = product.ProductType.GetDescription();
                }
                if (order.Provider != null && order.Provider.Product is Service.Order.Domain.CommonProductInfo)
                {
                    this.lblTicketType.Text = (order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
                }
                else
                {
                    this.lblTicketType.Text = "-";
                }
                this.lblOriginalOrderId.Text = order.AssociateOrderId.HasValue ? order.AssociateOrderId.Value.ToString() : "-";
            }
            else
            {
                this.lblStatus.Text = Service.Order.StatusService.GetOrderStatus(order.Status, OrderRole.Supplier);
            }
            if (order.Provider != null && order.Provider.PurchaserRelationType != Common.Enums.RelationType.Brother)
            {
                lblRelation.Text = order.Provider.PurchaserRelationType.GetDescription() + "-";
                hrefPurchaseName.InnerHtml = order.Purchaser.Company.UserName + "（" + order.Purchaser.Name + "）";
                this.hrefPurchaseName.HRef = "/OrganizationModule/RoleModule/ExtendCompanyManage/LowerComapnyInfoUpdate/LowerCompanyDetailInfo.aspx?CompanyId="
                                            + order.Purchaser.CompanyId.ToString() +
                                            "&Type=" + (order.Provider.PurchaserRelationType == Common.Enums.RelationType.Interior ? "Organization" : "Junion");
            }
            else
            {
                lblRelation.Text = "平台采购";
                hrefPurchaseName.Visible = false;
            }
            if (this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Provider && order.Provider != null && order.IsB3BOrder && order.Provider.Product != null && !order.Provider.Product.IsDefaultPolicy)
            {
                switch (order.Provider.Product.ProductType)
                {
                    case ProductType.General:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/TransactionPolicy/base_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                    case ProductType.Promotion:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/TransactionPolicy/low_price_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                    case ProductType.Special:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/TransactionPolicy/special_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                    case ProductType.Team:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/TransactionPolicy/team_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                }
            }
            else
            {
                this.linkPrividerPolicy.Visible = false;
            }
            this.lblProducedTime.Text = order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblPayTime.Text = order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            this.lblETDZTime.Text = order.ETDZTime.HasValue ? order.ETDZTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            switch (order.Status)
            {
                case OrderStatus.ConfirmFailed:
                case OrderStatus.DeniedWithSupply:
                case OrderStatus.DeniedWithETDZ:
                case OrderStatus.Canceled:
                    this.divFailed.Visible = true;
                    this.lblFailedReason.Text = order.Remark;
                    break;
            }
        }
        private void bindPNRGroups(Service.Order.Domain.Order order)
        {
            this.divPNRGroups.Visible = true;
            foreach (var item in order.PNRInfos)
            {
                var pnrInfo = LoadControl(ResolveUrl("~/OrderModule/UserControls/PNRInfo.ascx")) as OrderModule.UserControls.PNRInfo;
                pnrInfo.InitData(order, item);
                this.divPNRGroups.Controls.Add(pnrInfo);
                hidRenderPrice.Value = item.Flights.Any() && item.Flights.First().Bunk is FreeBunk || item.Flights.Any() && item.Flights.First().Price.Fare != 0 ? "1" : "0";
            }

        }
        private void bindPolicyRemark(Service.Order.Domain.Order order)
        {
            this.divPolicyRemark.Visible = true;
            this.divPolicyRemarkContent.InnerHtml = string.Format("{0} <span class='systemEndFix'>{1}</span>", isProvider() ? order.Provider.Product.Remark : order.Supplier.Product.Remark, SystemParamService.PolicyRemark);
        }
        private void bindOrderBills(Service.Order.Domain.Order order)
        {
            switch (order.Status)
            {
                case OrderStatus.PaidForSupply:
                case OrderStatus.DeniedWithSupply:
                case OrderStatus.PaidForETDZ:
                case OrderStatus.DeniedWithETDZ:
                case OrderStatus.Canceled:
                case OrderStatus.Finished:
                    this.bill.InitData(order.Bill);
                    this.bill.Visible = true;
                    break;
                default:
                    this.bill.Visible = false;
                    break;
            }
        }
        private bool isSupplier()
        {
            return CurrentCompany.CompanyType == Common.Enums.CompanyType.Supplier;
        }
        private bool isProvider()
        {
            return CurrentCompany.CompanyType == Common.Enums.CompanyType.Provider;
        }
        private void setButtons(Service.Order.Domain.Order order)
        {
            if (order.Status == OrderStatus.Applied)
            {
                // 确认座位
                setRequestUrl("Supply.aspx", order.Id, this.btnConfirm);
            }
            else if (order.Status == OrderStatus.PaidForSupply)
            {
                // 提供座位
                if (!order.IsThirdRelation || (order.IsThirdRelation && order.Supplier.CompanyId== this.CurrentCompany.CompanyId))
                    setRequestUrl("Supply.aspx", order.Id, this.btnSupply);
            }
            if (isProvider())
            {
                if (order.Status == OrderStatus.Finished)
                {
                    // 订单历史记录
                    setRequestUrl("../OrderHistoryRecord.aspx", order.Id, this.btnOrderHistory);
                    // 进行中的申请
                    setRequestUrl("../ProcessingApplyform.aspx", order.Id, this.btnProcessingApplyforms);
                }
                else if (order.Status == OrderStatus.PaidForETDZ)
                {
                    var employeeIdCustomNO = CompanyService.GetCustomNumberByEmployee(CurrentUser.Id);

                    // 出票
                    if (string.IsNullOrEmpty(order.CustomNo) || employeeIdCustomNO.Any(c => c.Number == order.CustomNo)) setRequestUrl("ETDZ.aspx", order.Id, this.btnETDZ);
                }
            }
        }
        private void setBackButton()
        {
            // 返回
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void setRequestUrl(string targetPage, decimal orderId, HtmlButton sender)
        {
            var requestUrl = string.Format("{0}?id={1}&returnUrl={2}&role=Provide", targetPage, orderId.ToString(), HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            sender.Attributes.Add("onclick", "window.location.href='" + requestUrl + "';return false;");
            sender.Visible = true;
        }
    }
}