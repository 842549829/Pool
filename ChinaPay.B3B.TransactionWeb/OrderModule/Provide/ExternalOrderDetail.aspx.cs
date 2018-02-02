using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.ExternalPlatform;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.ExternalPlatform.Yeexing;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class ExternalOrderDetail : BasePage
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
                    var order = Service.OrderQueryService.QueryExternalOrder(orderId);
                    if (order == null)
                    {
                        showErrorMessage("订单不存在");
                    }
                    else
                    {
                        bindOrder(order);
                        initData(order);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void initData(Service.Order.Domain.ExternalOrder order)
        {
            var yeexingPlatform = Platform.GetPlatform(order.Platform);
            foreach (var item in yeexingPlatform.ManualPayInterfaces)
            {
                this.ddlYeexingPlatform.Items.Add(new ListItem(item.GetDescription(), ((byte)item).ToString()));
            }
            this.ddlYeexingPlatform.Items.Insert(0, new ListItem("-请选择-", ""));
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
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';");
        }

        private void bindOrder(Service.Order.Domain.ExternalOrder order)
        {
            bindOrderHeader(order);
            bindPNRGroups(order);
        }

        private void bindOrderHeader(Service.Order.Domain.ExternalOrder order)
        {
            var isPlatform = CurrentCompany.CompanyType == CompanyType.Platform;
            this.hfdPlatformValue.Value =((byte)order.Platform).ToString();
            lnkInternalOrderId.HRef = isPlatform ? "/OrderModule/Operate/OrderDetail.aspx?id=" + order.Id.ToString() : "OrderDetail.aspx?id=" + order.Id.ToString();
            this.lblExtenalOrderId.Text = order.ExternalOrderId;
            this.lblPlatformType.Text = order.Platform.GetDescription();
            this.lblProduceTime.Text = order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss");
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
            if (order.Provider == null)
            {
                lblPurchaseAmount.Visible = false;
            }
            else
            {
                lblPurchaseAmount.Text = "（收到采购金额" + order.Provider.Amount.TrimInvaidZero().ToString() + "元）";
            }
            this.lblInternalOrderId.Text = order.Id.ToString();
            this.lblPrintStatus.Text = order.Status == DataTransferObject.Order.OrderStatus.Finished ? "已出票" : "未出票";
            this.lblExternalCommission.Text = (order.ECommission * 100).TrimInvaidZero().ToString() + "%";
            this.lblOrderAmount.Text = order.Amount.ToString();
            this.lblTicketType.Text = order.Product.TicketType.GetDescription();
            this.lblProductType.Text = order.Product.ProductType.GetDescription();
            this.lblPayStatus.Text = order.PayStatus == PayStatus.NoPay ? order.PayStatus.GetDescription() : order.PayStatus.GetDescription() + "（" + (order.IsAutoPay ? "自动" : "手工") + "）";
            this.lblInternalPayStatus.Text = Service.Order.StatusService.GetOrderStatus(order.Status, DataTransferObject.Order.OrderRole.Provider);
            if (order.Status == OrderStatus.PaidForSupply || order.Status == OrderStatus.PaidForETDZ || order.Status == OrderStatus.Finished)
            {
                if (order.PayStatus == Common.Enums.PayStatus.Paied)
                {
                    btnPayOrder.Visible = false;
                    btnGetPayInfo.Visible = false;
                    if (order.Status != DataTransferObject.Order.OrderStatus.Finished)
                    {
                        btnGetTicketNos.Visible = isPlatform;
                    }
                    else
                    {
                        btnGetTicketNos.Visible = false;
                    }
                }
                else
                {
                    failedReasonTitle.Visible = true;
                    failedReasonValue.Visible = true;
                    this.lblFailedReason.Text = order.FaildInfo;
                    btnPayOrder.Visible = isPlatform;
                    btnGetPayInfo.Visible = isPlatform;
                }
            }
            else
            {
                btnPayOrder.Visible = false;
                btnGetPayInfo.Visible = false;
            }
            if (order.PayStatus == Common.Enums.PayStatus.Paied)
            {
                this.lblPayTime.Text = order.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                this.lblPayTradeNo.Text = order.PayTradNO;
            }
            else
            {
                payTimeTitle.Visible = false;
                payTimeValue.Visible = false;
                payTradeNoTitle.Visible = false;
                payTradeNoValue.Visible = false;
            }
        }

        private void bindPNRGroups(Service.Order.Domain.Order order)
        {
            this.divPNRGroups.Visible = true;
            divPNRGroups.InnerHtml = "";
            foreach (var item in order.PNRInfos)
            {
                var pnrInfo = LoadControl(ResolveUrl("~/OrderModule/UserControls/PNRInfo.ascx")) as OrderModule.UserControls.PNRInfo;
                pnrInfo.InitData(order, item);
                this.divPNRGroups.Controls.Add(pnrInfo);
            }
        }

        private void showErrorMessage(string message)
        {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        protected void btnGetPayInfo_Click(object sender, EventArgs e)
        {
            var order = OrderProcessService.QueryExternalPlatformPaymentStatus(decimal.Parse(this.lblInternalOrderId.Text));
            if (order != null)
            {
                bindOrder(order);
            }
        }

        protected void btnGetTicketNos_Click(object sender, EventArgs e)
        {
            try
            {
                var order = OrderQueryService.QueryExternalOrderTicket(decimal.Parse(this.lblInternalOrderId.Text), BasePage.OwnerOEMId);
                if (order != null)
                {
                    bindOrder(order);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "获取票号");
            }
        }

    }
}