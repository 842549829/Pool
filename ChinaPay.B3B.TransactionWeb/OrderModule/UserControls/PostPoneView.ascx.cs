using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.UserControls
{
    public partial class PostPoneView : System.Web.UI.UserControl
    {
        protected bool IsShowRefundFee;
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InitPostPoneView(PostponeApplyform applyform, CompanyType companyType)
        {
            lblApplyformId.Text = applyform.Id.ToString();
            lblPassengers.Text = string.Join("&nbsp;", applyform.Passengers.Select(p => p.Name));
            lblPNR.Text = PNRRender(applyform.NewPNR ?? applyform.OriginalPNR);
                //applyform.NewPNR == null
                //              ? applyform.OriginalPNR == null ?
                //              string.Empty
                //                    : string.Format(PNRFORMAT, applyform.OriginalPNR.PNR, applyform.OriginalPNR.BPNR)
                //              : string.Format(PNRFORMAT, applyform.NewPNR.PNR, applyform.NewPNR.BPNR);
            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm:ss");
            lblRefundFee.Text = applyform.PostponeFee == 0m ? "免费" : applyform.PostponeFee.ToString();
            //lblPaydTime.Text = applyform.PayBill.Tradement.
            IsShowRefundFee = applyform.Status == PostponeApplyformStatus.Denied;
            lblStatus.Text = StatusService.GetPostponeApplyformStatus(applyform.Status, BasePage.GetOrderRole(applyform.Order)); 
            lblProcessedTime.Text = applyform.ProcessedTime.HasValue ? applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            lblApplyformId.PostBackUrl = InitDetailUrl(BasePage.GetOrderRole(applyform.Order)) + applyform.Id;


            dataList.DataSource = applyform.Flights.Select(f => new
            {
                Carrier = f.OriginalFlight.Carrier.Name,
                f.OriginalFlight.FlightNo,
                f.OriginalFlight.AirCraft,
                Bank = string.Format("{0}/{1}", string.IsNullOrEmpty(f.OriginalFlight.Bunk.Code) ? "-" : f.OriginalFlight.Bunk.Code, (f.OriginalFlight.Bunk.Discount * 100).TrimInvaidZero()),
                Departure = string.Format("{0}{1}", f.OriginalFlight.Departure.City, f.OriginalFlight.Departure.Name),
                Arrival = string.Format("{0}{1}", f.OriginalFlight.Arrival.City, f.OriginalFlight.Arrival.Name),
                f.OriginalFlight.TakeoffTime,
                f.OriginalFlight.LandingTime,
                f.OriginalFlight.Id
            });
            dataList.DataBind();
            dataListNew.DataSource = applyform.Flights.Select(f => new
            {
                Carrier = f.NewFlight.Carrier.Name,
                f.NewFlight.FlightNo,
                f.NewFlight.AirCraft,
                Bank = string.Format("{0}/{1}", f.NewFlight.Bunk.Code, (f.NewFlight.Bunk.Discount * 100).TrimInvaidZero()),
                Departure = string.Format("{0}{1}", f.NewFlight.Departure.City, f.NewFlight.Departure.Name),
                Arrival = string.Format("{0}{1}", f.NewFlight.Arrival.City, f.NewFlight.Arrival.Name),
                f.NewFlight.TakeoffTime,
                f.NewFlight.LandingTime,
                f.NewFlight.Id
            });
            dataListNew.DataBind();
        }

        private string PNRRender(PNRPair pnrPair)
        {
            string pnrString = string.Empty;
            if (!string.IsNullOrEmpty(pnrPair.PNR)) pnrString += string.Format(PNRFORMAT, pnrPair.PNR, "小");
            if (!string.IsNullOrEmpty(pnrPair.BPNR)) pnrString += string.Format(PNRFORMAT, pnrPair.BPNR, "大");
            return pnrString;
        }

        private string InitDetailUrl(OrderRole companyType)
        {
            switch (companyType)
            {
                case OrderRole.Platform:
                    return "/OrderModule/Operate/PostponeApplyformDetail.aspx?id=";
                case OrderRole.Purchaser:
                    return "/OrderModule/Purchase/PostponeApplyformDetail.aspx?id=";
                default:
                    return "/OrderModule/Provide/PostponeApplyformDetail.aspx?id=";
            }

        }
    }
}