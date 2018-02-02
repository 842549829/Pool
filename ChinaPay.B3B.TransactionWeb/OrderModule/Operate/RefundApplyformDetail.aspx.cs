using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using RefundFlight = ChinaPay.B3B.Service.Order.Domain.Applyform.RefundFlight;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class RefundApplyformDetail : BasePage
    {
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    var applyform = Service.ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    if (applyform == null)
                    {
                        showErrorMessage("退/废票申请单不存在");
                    }
                    else
                    {
                        bindAttachment(applyform);
                        bindData(applyform);
                        setButtons(applyform);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindData(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            bindHeader(applyform);
            bindVoyages(applyform);
            bindPassengers(applyform);
            bindApplyAndProcessInfo(applyform);
            bindBill(applyform);
        }
        private void bindHeader(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            RefundApplyform refundForm = applyform as RefundApplyform;
            this.lblApplyformId.Text = applyform.Id.ToString();
            this.linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + System.Web.HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            this.linkOrderId.InnerText = applyform.OrderId.ToString();
            this.lblApplyType.Text = string.Format("{0} {1}", applyform,
                refundForm != null ? (string.Format("({0})", refundForm.RefundType.GetDescription())) : string.Empty);
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
            this.lblStatus.Text = Service.Order.StatusService.GetRefundApplyformStatus(applyform.Status, DataTransferObject.Order.OrderRole.Platform);
            if (applyform.Order.Provider != null && applyform.Order.Provider.Product is Service.Order.Domain.CommonProductInfo)
            {
                this.lblTicketType.Text = (applyform.Order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            }
            else
            {
                this.lblTicketType.Text = "-";
            }
            lblPNR.Text = AppendPNR(applyform.NewPNR, string.Empty);
            lblPNR.Text += AppendPNR(applyform.OriginalPNR, string.IsNullOrWhiteSpace(lblPNR.Text) ? string.Empty : "原编码：");

            //lblPNR.Text = string.Format("{0} {1} &nbsp; 原编码：{2} {3}",
            //    string.IsNullOrWhiteSpace(applyform.NewPNR.PNR) ? "" : string.Format(PNRFORMAT, applyform.NewPNR.PNR,"小"),
            //    string.IsNullOrWhiteSpace(applyform.NewPNR.BPNR) ? "" : string.Format(PNRFORMAT, applyform.NewPNR.PNR,"大"),
            //    string.IsNullOrWhiteSpace(applyform.OriginalPNR.PNR) ? "" : string.Format(PNRFORMAT, applyform.NewPNR.PNR, "小"),
            //    string.IsNullOrWhiteSpace(applyform.OriginalPNR.BPNR) ? "" : string.Format(PNRFORMAT, applyform.NewPNR.PNR,"大")
            //    );




            this.linkPurchaser.InnerText = applyform.PurchaserName;
            this.linkPurchaser.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + applyform.PurchaserId.ToString();
            this.linkProvider.InnerText = applyform.ProviderName;
            this.linkProvider.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + applyform.ProviderId.ToString();
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


        private void bindVoyages(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.voyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight));
        }
        private void bindPassengers(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.passengers.InitData(applyform.Order, applyform.Passengers, applyform.Flights.Select(f=>f.OriginalFlight));
        }
        private void bindApplyAndProcessInfo(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            this.lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm");
            this.lblAppliedReason.Text = applyform.ApplyRemark;
            if (applyform.ProcessedTime.HasValue)
            {
                this.lblProcessedTime.Text = applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm");
                this.lblProcessedResult.Text = StatusService.GetRefundApplyformStatus(applyform.Status, GetOrderRole(applyform.Order)) + " " + applyform.ProcessedFailedReason;
            }
            if (applyform.Status == DataTransferObject.Order.RefundApplyformStatus.Refunded)
            {
                var refundInfoHTML = new StringBuilder();
                refundInfoHTML.Append("<table><tr><th>航段</th><th>手续费率</th><th>手续费</th><th>退款金额</th></tr>");
                var index = 0;
                var bill = CompanyType.Platform == CurrentCompany.CompanyType ? applyform.RefundBill.Provider : getUserRoleBill(applyform.RefundBill);
                var flightRefundFees = applyform.OriginalFlights.Join(bill.Source.Details, f => f.ReservateFlight, f => f.Flight.Id, (f1, f2) => new
                    {
                        flight = f1,
                        fee = f2
                    });
                foreach(var item in flightRefundFees) {
                    refundInfoHTML.Append("<tr>");
                    refundInfoHTML.AppendFormat("<td>{0} - {1}</td>", item.flight.Departure.City, item.flight.Arrival.City);
                    refundInfoHTML.AppendFormat("<td>{0}%</td>", (item.fee.RefundRate * 100).TrimInvaidZero());
                    refundInfoHTML.AppendFormat("<td>{0}</td>", Math.Abs(item.fee.RefundFee).TrimInvaidZero());
                    if(index == 0) {
                        refundInfoHTML.AppendFormat("<td rowspan='{0}'>{1}</td>", flightRefundFees.Count(), bill.Source.Details.Sum(p => p.Anticipation).TrimInvaidZero());
                    }
                    refundInfoHTML.Append("</tr>");
                    index++;
                }
                refundInfoHTML.Append("</table>");
                this.divRefundFeeInfo.InnerHtml = refundInfoHTML.ToString();
            }
        }

        private NormalRefundRoleBill getUserRoleBill(NormalRefundBill bill)
        {
            if (bill.Purchaser.Owner.Id == BasePage.LogonCompany.CompanyId)
            {
                return bill.Purchaser;
            }
            else if (bill.Provider != null && bill.Provider.Owner.Id == BasePage.LogonCompany.CompanyId)
            {
                return bill.Provider;
            }
            else if (bill.Supplier != null && bill.Supplier.Owner.Id == BasePage.LogonCompany.CompanyId)
            {
                return bill.Supplier;
            }
            return null;
        }

        private void bindBill(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            if (applyform.Status == ChinaPay.B3B.DataTransferObject.Order.RefundApplyformStatus.Refunded)
            {
                this.bill.InitData(applyform.RefundBill);
            }
            else
            {
                this.bill.Visible = false;
            }
        }
        private void setButtons(Service.Order.Domain.Applyform.RefundOrScrapApplyform applyform)
        {
            switch (applyform.Status)
            {
                case ChinaPay.B3B.DataTransferObject.Order.RefundApplyformStatus.AppliedForPlatform:
                case ChinaPay.B3B.DataTransferObject.Order.RefundApplyformStatus.AppliedForCancelReservation:
                case ChinaPay.B3B.DataTransferObject.Order.RefundApplyformStatus.DeniedByProviderBusiness:
                    this.btnProcess.Visible = true;
                    var processPage = applyform.RequireRevisePrice ? "ProcessRevisePrice.aspx" : "ProcessRefund.aspx";
                    this.btnProcess.Attributes.Add("onclick", "window.location.href='" + processPage + "?id=" + applyform.Id
                        + "&returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery) + "';return false;");
                    break;
                default:
                    this.btnProcess.Visible = false;
                    break;
            }
        }
        private void setBackButton()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "ApplyformList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void showErrorMessage(string message)
        {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }
        private void bindAttachment(RefundOrScrapApplyform applyform)
        {
            RefundApplyform refundApplyform = applyform as RefundApplyform;
            if (refundApplyform != null && (refundApplyform.RefundType == RefundType.Involuntary || refundApplyform.RefundType == RefundType.SpecialReason))
            {
                var attachment = ApplyformQueryService.QueryApplyAttachmentView(refundApplyform.Id);
                UserControl.OutPutImage outPutImage = LoadControl(ResolveUrl("~/UserControl/OutPutImage.ascx")) as UserControl.OutPutImage;
                outPutImage.ApplyAttachment = attachment;
                outPutImage.IsPlatform = refundApplyform.Status != RefundApplyformStatus.Refunded && refundApplyform.Status != RefundApplyformStatus.DeniedByProviderTreasurer && refundApplyform.Status != RefundApplyformStatus.Denied;
                outPutImage.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                outPutImage.EnableTheming = false;
                outPutImage.ApplyformId = refundApplyform.Id;
                string content = outPutImage.Binddata();
                divApplyAttachment.InnerHtml = string.IsNullOrEmpty(content) ? string.Empty : string.Format("<h3 class=\"titleBg\">附件</h3><div id=\"divOutPutImage\" class=\"clearfix\">{0}</div>", outPutImage.Binddata());
            }
        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            decimal attachmentId;
            string id = Request.QueryString["id"];
            if (decimal.TryParse(id, out attachmentId))
            {
                try
                {
                    string filePath = FileService.Upload(fileAttachment, "RefundApplyformView", "(jpg)|(bmp)|(png)", 600 * 1024);
                    List<ApplyAttachmentView> list = new List<ApplyAttachmentView>();
                    var bytes = ChinaPay.B3B.Service.FileService.GetFileBytes(filePath);
                    Thumbnail thumbnail = new Thumbnail();
                    list.Add(new ApplyAttachmentView
                    {
                        Id = Guid.NewGuid(),
                        ApplyformId = attachmentId,
                        FilePath = filePath,
                        Thumbnail = thumbnail.MakeThumb(100, bytes),
                        Time = DateTime.Now
                    });
                    ApplyformQueryService.AddApplyAttachmentView(list, CurrentUser.UserName);
                    var applyform = Service.ApplyformQueryService.QueryRefundOrScrapApplyform(attachmentId);
                    bindAttachment(applyform);
                    ShowMessage("上传成功");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "上传");
                }
            }
        }
    }
}