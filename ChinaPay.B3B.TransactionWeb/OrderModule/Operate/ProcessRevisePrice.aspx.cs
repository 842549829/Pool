using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class ProcessRevisePrice : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    string lockErrorMsg;
                    if (applyform == null)
                    {
                        showErrorMessage("退/废票申请单不存在");
                        return;
                    }
                    if (Lock(applyform.OrderId, LockRole.Platform, "修改价格", out lockErrorMsg))
                    {
                        RefundApplyform refundApplyform = applyform as RefundApplyform;
                        if (refundApplyform != null && (refundApplyform.RefundType == RefundType.Involuntary || refundApplyform.RefundType == RefundType.SpecialReason))
                        {
                            bindAttachment(applyform.Id);
                        }
                        bindData(applyform);
                    }
                    else
                    {
                        showErrorMessage("锁定申请单失败。原因:" + lockErrorMsg);
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }

        private void bindAttachment(decimal applyformId)
        {
            var attachment = ApplyformQueryService.QueryApplyAttachmentView(applyformId);
            UserControl.OutPutImage outPutImage = LoadControl(ResolveUrl("~/UserControl/OutPutImage.ascx")) as UserControl.OutPutImage;
            outPutImage.ApplyAttachment = attachment;
            outPutImage.IsPlatform = true;
            outPutImage.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            outPutImage.EnableTheming = false;
            outPutImage.ApplyformId = applyformId;
            outPutImage.DataBind();
            divApplyAttachment.InnerHtml = string.Format("<h3 class=\"titleBg\">附件</h3><div id=\"divOutPutImage\" class=\"clearfix\">{0}</div>", outPutImage.Binddata());
        }

        private void bindData(RefundOrScrapApplyform applyform)
        {
            bindHeader(applyform);
            bindPassengers(applyform);
            bindVoyage(applyform);
        }

        private void bindVoyage(RefundOrScrapApplyform applyform)
        {
            ListVoyage.DataSource = applyform.Order.PNRInfos.First().Flights;
            ListVoyage.DataBind();
            flightType.InnerHtml = applyform.Order.TripType.GetDescription();
        }

        private void bindHeader(RefundOrScrapApplyform applyform)
        {
            RefundApplyform refundForm = applyform as RefundApplyform;
            var orderRole = OrderRole.Platform;
            lblApplyformId.Text = applyform.Id.ToString();
            linkOrderId.HRef = "OrderDetail.aspx?id=" + applyform.OrderId.ToString() + "&returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery);
            linkOrderId.InnerText = applyform.OrderId.ToString();
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
            lblStatus.Text = StatusService.GetRefundApplyformStatus(applyform.Status, orderRole);
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

        }
        List<PNRPair> RenderedPNR = new List<PNRPair>();
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
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";



        private void bindPassengers(RefundOrScrapApplyform applyform)
        {
            this.passengers.InitData(applyform.Order, applyform.Passengers, applyform.Flights.Select(f => f.OriginalFlight));
        }

        protected void btnReleaseLockAndBack_Click(object sender, EventArgs e)
        {
            decimal orderId;
            if (decimal.TryParse(linkOrderId.InnerText, out orderId))
            {
                ReleaseLock(orderId);
                RegisterScript(this, "location.href='" + ReturnUrl + "'", true);
            }
        }

        protected void RefundInfos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var Passengers = e.Item.FindControl("Passengers") as Repeater;
            if (Passengers != null)
            {
                Passengers.DataSource = DataBinder.Eval(e.Item.DataItem, "Passengers");
                Passengers.DataBind();
            }
        }
        private void setBackButton()
        {
            var returnUrl = ReturnUrl;
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }

        private string ReturnUrl
        {
            get
            {
                var returnUrl = Request.QueryString["returnUrl"];
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = "ChangeProcessList.aspx";
                }
                if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
                return returnUrl;
            }
        }

        private void showErrorMessage(string message)
        {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        protected string GetIndex(int i)
        {
            return CNIndex[i];
        }

        readonly Dictionary<int, string> CNIndex = new Dictionary<int, string>
        {
            {1,"一"},{2,"二"},{3,"三"},{4,"四"},{5,"五"},{6,"六"},{7,"七"},{8,"八"},{9,"九"},{10,"十"},
        };

        /// <summary>
        /// 确认价格信息修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SurePrice(object sender, EventArgs e)
        {
            try
            {
                decimal applyformId;
                if (decimal.TryParse(Request.QueryString["id"], out applyformId))
                {
                    RefundOrScrapApplyform applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
                    var priceView = new List<PriceView>();
                    foreach (RepeaterItem item in ListVoyage.Items)
                    {
                        if (item.ItemType == ListItemType.Header || item.ItemType == ListItemType.Footer) continue;
                        var releasePriceCtl = item.FindControl("ReleasePrice") as TextBox;
                        var airportFeeCtl = item.FindControl("AirPortFee") as TextBox;
                        var Departure = item.FindControl("VoyageStart") as HiddenField;
                        var Arrival = item.FindControl("VoyageEnd") as HiddenField;
                        var ReleasePrice = float.Parse(releasePriceCtl.Text.Trim());
                        var AirPortFee = float.Parse(airportFeeCtl.Text.Trim());
                        priceView.Add(new PriceView
                        {
                            AirportPair = new AirportPair(Departure.Value, Arrival.Value),
                            AirportFee = (decimal)AirPortFee,
                            Fare = (decimal)ReleasePrice
                        });
                    }
                    OrderProcessService.RevisePrice(applyform.OrderId, priceView, CurrentUser.UserName);
                    ReleaseLock(applyform.OrderId);
                    RegisterScript(this, "alert('价格修改成功！');location.href='/OrderModule/Operate/ChangeProcessList.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改价格");
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
                    bindAttachment(attachmentId);
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