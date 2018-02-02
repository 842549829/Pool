using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule {
    public partial class ProcessingApplyform : BasePage {
        const string HTMLNewLine = "<br />";

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                var id = Request.QueryString["id"];
                decimal orderId;
                if(decimal.TryParse(id, out orderId)) {
                    this.lblOrderId.Text = id;
                    bindProcessingApplyforms(orderId);
                } else {
                    this.lblOrderId.Text = "订单号错误";
                }
                setBackUrl();
            }
        }
        private void bindProcessingApplyforms(decimal orderId) {
            this.divContent.InnerHtml = "<table cellspacing=\"0\">" + drawTitle() + drawContents(orderId) + "</table>";
        }
        private IEnumerable<Service.Order.Domain.Applyform.BaseApplyform> getApplyforms(decimal orderId) {
            return from item in Service.ApplyformQueryService.QueryApplyforms(orderId)
                   where item.ProcessStatus != DataTransferObject.Order.ApplyformProcessStatus.Finished
                   orderby item.AppliedTime descending
                   select item;
        }
        private string drawTitle() {
            var title = "<tr><th>申请单号</th><th>编码</th><th>航程</th><th>航班号<br />舱位/折扣</th><th>起飞时间</th><th>乘机人</th><th>申请类型</th><th>状态</th>";
            if(isShowApplier()) {
                title += "<th>申请人</th>";
            }
            title += "<th>申请时间</th></tr>";
            return title;
        }
        private string drawContents(decimal orderId) {
            var contents = new StringBuilder();
            var applyforms = getApplyforms(orderId);
            var returnUrl = Request.Url.PathAndQuery;
            foreach(var item in applyforms) {
                contents.Append("<tr>");
                var applyformUrl = getDetailPage(item, returnUrl);
                contents.AppendFormat("<td><a href='{0}' class='obvious-a'>{1}</a></td>", applyformUrl, item.Id);
                contents.AppendFormat("<td>{0}</td>", getPNR(item.OriginalPNR));
                contents.AppendFormat("<td>{0}</td>", getVoyages(item));
                contents.AppendFormat("<td>{0}</td>", getFlights(item));
                contents.AppendFormat("<td>{0}</td>", getTakeoffTimes(item));
                contents.AppendFormat("<td>{0}</td>", item.Passengers.Join(HTMLNewLine, p => p.Name));
                contents.AppendFormat("<td>{0}</td>", getApplyType(item));
                contents.AppendFormat("<td>{0}</td>", getStatusText(item.ProcessStatus));
                if(isShowApplier()) {
                    contents.AppendFormat("<td>{0}</td>", item.ApplierAccount);
                }
                contents.AppendFormat("<td>{0}</td>", item.AppliedTime.ToString("yyyy-MM-dd" + HTMLNewLine + "HH:mm:ss"));
                contents.Append("</tr>");
            }
            return contents.ToString();
        }
        private string getPNR(ChinaPay.B3B.DataTransferObject.Common.PNRPair pnrPair) {
            var result = string.Empty;
            if(!string.IsNullOrWhiteSpace(pnrPair.PNR)) {
                result += pnrPair.PNR + "(小)";
                if(!string.IsNullOrWhiteSpace(pnrPair.BPNR)) {
                    result += HTMLNewLine;
                }
            }
            if(!string.IsNullOrWhiteSpace(pnrPair.BPNR)) {
                result += pnrPair.BPNR + "(大)";
            }
            return result;
        }
        private string getVoyages(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Service.Order.Domain.Applyform.RefundOrScrapApplyform) {
                var refundOrScrapApplyform = applyform as Service.Order.Domain.Applyform.RefundOrScrapApplyform;
                return refundOrScrapApplyform.Flights.Join(HTMLNewLine, item => getVoyage(item.OriginalFlight));
            } else if(applyform is Service.Order.Domain.Applyform.PostponeApplyform) {
                var postponeApplyform = applyform as Service.Order.Domain.Applyform.PostponeApplyform;
                return postponeApplyform.Flights.Join(HTMLNewLine, item => getVoyage(item.OriginalFlight));
            } else {
                return string.Empty;
            }
        }
        private string getVoyage(Service.Order.Domain.Flight flight) {
            return flight.Departure.City + "-" + flight.Arrival.City;
        }
        private string getFlights(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Service.Order.Domain.Applyform.RefundOrScrapApplyform) {
                var refundOrScrapApplyform = applyform as Service.Order.Domain.Applyform.RefundOrScrapApplyform;
                return refundOrScrapApplyform.Flights.Join(HTMLNewLine, item => getFlight(item.OriginalFlight));
            } else if(applyform is Service.Order.Domain.Applyform.PostponeApplyform) {
                var postponeApplyform = applyform as Service.Order.Domain.Applyform.PostponeApplyform;
                return postponeApplyform.Flights.Join(HTMLNewLine, item => getFlight(item.OriginalFlight));
            } else {
                return string.Empty;
            }
        }
        private string getFlight(Service.Order.Domain.Flight flight) {
            return flight.Carrier.Code + flight.FlightNo + HTMLNewLine + flight.Bunk.Code + "/" + (flight.Bunk.Discount * 100).TrimInvaidZero();
        }
        private string getTakeoffTimes(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Service.Order.Domain.Applyform.RefundOrScrapApplyform) {
                var refundOrScrapApplyform = applyform as Service.Order.Domain.Applyform.RefundOrScrapApplyform;
                return refundOrScrapApplyform.Flights.Join(HTMLNewLine, item => item.OriginalFlight.TakeoffTime.ToString("yyyy-MM-dd HH:mm"));
            } else if(applyform is Service.Order.Domain.Applyform.PostponeApplyform) {
                var postponeApplyform = applyform as Service.Order.Domain.Applyform.PostponeApplyform;
                return postponeApplyform.Flights.Join(HTMLNewLine, item => item.OriginalFlight.TakeoffTime.ToString("yyyy-MM-dd HH:mm"));
            } else {
                return string.Empty;
            }
        }
        private string getApplyType(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Service.Order.Domain.Applyform.PostponeApplyform) {
                return "改期";
            } else if(applyform is Service.Order.Domain.Applyform.RefundApplyform) {
                return "退票";
            } else if(applyform is Service.Order.Domain.Applyform.ScrapApplyform) {
                return "废票";
            }
            return string.Empty;
        }
        private string getStatusText(ChinaPay.B3B.DataTransferObject.Order.ApplyformProcessStatus status) {
            switch(status) {
                case ChinaPay.B3B.DataTransferObject.Order.ApplyformProcessStatus.Applied:
                    return "申请中";
                case ChinaPay.B3B.DataTransferObject.Order.ApplyformProcessStatus.Processing:
                    return "处理中";
                default:
                    return string.Empty;
            }
        }
        private void setBackUrl() {
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = Request.UrlReferrer.PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private string getDetailPage(Service.Order.Domain.Applyform.BaseApplyform applyform, string returnUrl) {
            var page = applyform is Service.Order.Domain.Applyform.PostponeApplyform ? "PostponeApplyformDetail.aspx" : "RefundApplyformDetail.aspx";
            return string.Format("{0}/{1}?id={2}&returnUrl={3}", Request.QueryString["role"], page, applyform.Id.ToString(), System.Web.HttpUtility.UrlEncode(returnUrl));
        }
        private bool isShowApplier() {
            return true;
        }
    }
}