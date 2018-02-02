using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide {
    public partial class ETDZ : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal orderId;
                if(decimal.TryParse(Request.QueryString["id"], out orderId)) {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if(order == null) {
                        showErrorMessage("订单不存在");
                    } else {
                        if(order.Status == OrderStatus.PaidForETDZ) {
                            string lockErrorMsg;
                            if(BasePage.Lock(orderId, Service.Locker.LockRole.Provider, "出票", out lockErrorMsg)) {
                                bindOrder(order);
                                setButtons(order);
                            } else {
                                showErrorMessage("锁定订单失败。原因:" + lockErrorMsg);
                            }
                        } else {
                            showErrorMessage("仅已支付待出票的订单可出票");
                        }
                    }
                } else {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindOrder(Service.Order.Domain.Order order) {
            bindOrderHeader(order);
            bindOrderBills(order);
            bindPolicyRemark(order);
            bindPNRInfos(order);
            bindOfficeNo(order);
        }

        private void bindOfficeNo(Order order) {
            var currentNO = order.IsThirdRelation ? order.Supplier.Product.OfficeNo : order.Provider.Product.OfficeNo;
            ddlOfficeNo.DataSource = CompanyService.QueryOfficeNumbers(CurrentCompany.CompanyId).Select(o => o.Number);
            ddlOfficeNo.DataBind();
            var current = ddlOfficeNo.Items.FindByText(currentNO);
            if(current!=null) current.Selected = true;
            var commonProductInfo = order.Provider.Product as CommonProductInfo;
            if (commonProductInfo != null)
            {
                var ticketType = commonProductInfo.TicketType;
                if (ticketType == TicketType.BSP)
                {
                    TicketType1.Checked = true;
                }
                else
                {
                    TicketType2.Checked = true;
                }
            }
            else
            {
                TicketTypeContainer.Visible = false;
            }
        }

        private void bindOrderHeader(Service.Order.Domain.Order order) {
            this.lblOrderId.Text = order.Id.ToString();
            this.lblStatus.Text = Service.Order.StatusService.GetOrderStatus(order.Status, DataTransferObject.Order.OrderRole.Provider);
            this.lblAmount.Text = order.Provider.Amount.ToString("F2");
            //var product = order.IsThirdRelation ? order.Supplier.Product : order.Provider.Product;
            var product = order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = product.ProductType.GetDescription();
            }
            if(order.Provider != null && order.Provider.Product is Service.Order.Domain.CommonProductInfo) {
                this.lblTicketType.Text = (order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            } else {
                this.lblTicketType.Text = "-";
            }
            this.lblOriginalOrderId.Text = order.AssociateOrderId.HasValue ? order.AssociateOrderId.Value.ToString() : "-";
            this.lblProducedTime.Text = order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblPayTime.Text = order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
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
        }
        private void bindOrderBills(Service.Order.Domain.Order order) {
            this.bill.InitData(order.Bill);
        }
        private void bindPolicyRemark(Service.Order.Domain.Order order) {
            this.divPolicyRemarkContent.InnerHtml = string.Format("{0} <span class='systemEndFix'>{1}</span>", order.Provider.Product.Remark, SystemParamService.PolicyRemark);
            var requireChangePNR = false;
            var commonProductInfo = order.Provider.Product as CommonProductInfo;
            if(commonProductInfo != null)
            {
                requireChangePNR = commonProductInfo.RequireChangePNR;
            }
            ClientScript.RegisterClientScriptBlock(GetType(),"parameter","var requireChangePNR="+(requireChangePNR?1:0),true);
        }
        private void bindPNRInfos(Service.Order.Domain.Order order) {
            var pnrInfo = order.PNRInfos.First();
            this.lblPNRCode.Text = pnrInfo.Code != null ? pnrInfo.Code.PNR : string.Empty;
            this.lblBPNRCode.Text = pnrInfo.Code != null ? pnrInfo.Code.BPNR : string.Empty;
            this.pnrGroups.InitData(order, pnrInfo, UserControls.Mode.ETDZ);
            this.pnrGroups.Visible = true;
            this.divNewPNRInfo.Visible = true;
            if (order.Choise != AuthenticationChoise.NoNeedAUTH)
            {
                Tips.Visible = true;
                Tips.InnerHtml = "提示：采购该机票的用户已选择了：" + order.Choise.GetDescription();
            }
            divNewPNRInfo.Visible = !order.ForbidChangPNR;
            NOChangePNR.Visible = order.ForbidChangPNR;
            lbOriginalPNR.Text = pnrInfo.Code.ToListString(" ");
        }
        private void setButtons(Service.Order.Domain.Order order) {
            this.btnETDZ.Visible = true;
            this.btnDeny.Visible = true;
            this.btnReleaseLockAndBack.Visible = true;
        }
        private void setBackButton() {
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back")==-1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.hidReturnUrl.Value = returnUrl;
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void showErrorMessage(string message) {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }
        protected void btnReleaseLockAndBack_Click(object sender, EventArgs e) {
            Service.LockService.UnLock(Request.QueryString["id"], CurrentUser.UserName);
            Response.Redirect(this.hidReturnUrl.Value, true);
        }
    }
}