using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate {
    public partial class PostponeApplyformDetail : BasePage {
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal applyformId;
                if(decimal.TryParse(Request.QueryString["id"], out applyformId)) {
                    var applyform = Service.ApplyformQueryService.QueryPostponeApplyform(applyformId);
                    if(applyform == null) {
                        showErrorMessage("改期申请单不存在");
                    } else {
                        bindData(applyform);
                        setButtons(applyform);
                    }
                } else {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindData(Service.Order.Domain.Applyform.PostponeApplyform applyform) {
            bindHeader(applyform);
            bindVoyages(applyform);
            bindPassengers(applyform);
            bindBills(applyform);
        }
        private void bindHeader(Service.Order.Domain.Applyform.PostponeApplyform applyform) {
            this.lblApplyformId.Text = applyform.Id.ToString();
            this.linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            this.linkOrderId.InnerText = applyform.OrderId.ToString();
            this.lblApplyType.Text = applyform.ToString();
            var product = applyform.Order.IsThirdRelation ? applyform.Order.Supplier.Product : applyform.Order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = applyform.Order.Product.ProductType.GetDescription();
            }
            this.lblStatus.Text = Service.Order.StatusService.GetPostponeApplyformStatus(applyform.Status, DataTransferObject.Order.OrderRole.Platform);
            if (applyform.Order.Provider != null && applyform.Order.Provider.Product is Service.Order.Domain.CommonProductInfo)
            {
                this.lblTicketType.Text = (applyform.Order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            }
            else
            {
                this.lblTicketType.Text = "-";
            }

            lblPNR.Text = AppendPNR(applyform.NewPNR, string.Empty);
            lblPNR.Text += AppendPNR(applyform.OriginalPNR, string.IsNullOrWhiteSpace(lblPNR.Text)?string.Empty:"原编码：");
            this.lblAppliedReason.Text = applyform.ApplyRemark;
            this.lblDeniedReason.Text = applyform.ProcessedFailedReason;
            this.lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblPostponeFee.Text = applyform.PostponeFee.TrimInvaidZero();
            this.lblProcessedTime.Text = applyform.ProcessedTime.HasValue ? applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";

            this.linkPurchaser.InnerText = applyform.PurchaserName;
            this.linkPurchaser.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + applyform.PurchaserId.ToString();
            this.linkProvider.InnerText = applyform.ProviderName;
            this.linkProvider.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + applyform.ProviderId.ToString();
        }
        private void bindVoyages(Service.Order.Domain.Applyform.PostponeApplyform applyform) {
            this.originalVoyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight));
            this.originalVoyages.Tip = "原航班信息";
            this.newVoyages.InitData(applyform.Order, applyform.Flights.Select(item => item.NewFlight));
            this.newVoyages.Tip = "改期申请新航班信息";
        }
        private void bindPassengers(Service.Order.Domain.Applyform.PostponeApplyform applyform) {
            this.originalPassengers.InitData(applyform.Order, applyform.Passengers, applyform.Flights.Select(p=>p.OriginalFlight));
            this.originalPassengers.Tip = "原机票信息";
            this.applyInfos.DataSource = from item in applyform.Passengers
                                         select new
                                         {
                                             item.Name,
                                             Type = item.PassengerType.GetDescription(),
                                             item.Credentials
                                         };
            this.applyInfos.DataBind();
        }
        string AppendPNR(PNRPair pnr, string tip)
        {
            if (PNRPair.IsNullOrEmpty(pnr)) return string.Empty;
            if (RenderedPNR.Any(pnr.Equals)) return string.Empty;
            var result = new StringBuilder(" ");
            result.Append(tip);
            if (!string.IsNullOrWhiteSpace(pnr.PNR)) result.AppendFormat(PNRFORMAT, pnr.PNR.ToUpper(), "小");
            result.Append(" ");
            if (!string.IsNullOrWhiteSpace(pnr.BPNR)) result.AppendFormat(PNRFORMAT, pnr.BPNR.ToUpper(), "大");
            RenderedPNR.Add(pnr);
            return result.ToString();
        }
        List<PNRPair> RenderedPNR = new List<PNRPair>();

        private void bindBills(Service.Order.Domain.Applyform.PostponeApplyform applyform) {
            switch(applyform.Status) {
                case ChinaPay.B3B.DataTransferObject.Order.PostponeApplyformStatus.Paid:
                    this.bill.InitData(applyform.PayBill);
                    break;
                case ChinaPay.B3B.DataTransferObject.Order.PostponeApplyformStatus.Denied:
                case ChinaPay.B3B.DataTransferObject.Order.PostponeApplyformStatus.Postponed:
                    if(applyform.PostponeFee > 0) {
                        this.bill.InitData(applyform.PayBill);
                    }
                    break;
                default:
                    this.bill.Visible = false;
                    break;
            }
        }
        private void setButtons(Service.Order.Domain.Applyform.PostponeApplyform applyform) {
            switch(applyform.Status) {
                case ChinaPay.B3B.DataTransferObject.Order.PostponeApplyformStatus.Applied:
                case ChinaPay.B3B.DataTransferObject.Order.PostponeApplyformStatus.Paid:
                    this.btnProcess.Visible = true;
                    this.btnProcess.Attributes.Add("onclick", string.Format("window.location.href='ProcessPostpone.aspx?id={0}&returnUrl={1}';return false;",
                        applyform.Id, HttpUtility.UrlEncode(Request.Url.PathAndQuery)));
                    break;
            }
        }
        private void setBackButton() {
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = "ApplyformList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void showErrorMessage(string message) {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }
    }
}