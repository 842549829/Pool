using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.UserControls
{
    public partial class RefundFormView : System.Web.UI.UserControl
    {
        private const string PNRFORMAT = "{0}({1}) <a href='javascript:copyToClipboard(\"{0}\")'>复制</a>";
        protected bool IsShowRefundFee;

        protected void Page_Load(object sender, EventArgs e) { }

        public void InitRefundFormView(RefundOrScrapApplyform applyform, CompanyType currentCompanyType)
        {
            lblApplyformId.Text = applyform.Id.ToString();
            lblPassengers.Text = string.Join("&nbsp;", applyform.Passengers.Select(p => p.Name));
            lblPNR.Text = PNRRender(applyform.NewPNR?? applyform.OriginalPNR);
            lblAppliedTime.Text = applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (applyform.RefundBill != null)
            {
                lblRefundFee.Text = string.Empty;
                if (currentCompanyType != CompanyType.Platform)
                {

                    NormalRefundRoleBill bill = BasePage.SearchBill(applyform.RefundBill);
                    lblRefundMoney.Text = Math.Abs(bill.Source.Details.Sum(p => p.Anticipation)).TrimInvaidZero();
                }
                else
                {
                    RefundMoney.Text = "&nbsp;";
                }
                IsShowRefundFee = true;
            }
            else
            {
                RefundFee.Text = "&nbsp;";
            }
            //IsShowRefundFee = applyform.Status == RefundApplyformStatus.Denied;
            lblStatus.Text = StatusService.GetRefundApplyformStatus(applyform.Status, BasePage.GetOrderRole(applyform.Order));
            lblProcessedTime.Text = applyform.ProcessedTime.HasValue ?
                applyform.ProcessedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            lblApplyformId.PostBackUrl = InitDetailUrl(BasePage.GetOrderRole(applyform.Order)) + applyform.Id;

            dataList.DataSource = applyform.Flights.Select(f => new
                {
                    f.OriginalFlight.Carrier.Name,
                    f.OriginalFlight.FlightNo,
                    f.OriginalFlight.AirCraft,
                    Bank = string.Format("{0}/{1}", string.IsNullOrEmpty(f.OriginalFlight.Bunk.Code) ? "-" : f.OriginalFlight.Bunk.Code, (f.OriginalFlight.Bunk.Discount*100).TrimInvaidZero()),
                    Departure = string.Format("{0}{1}", f.OriginalFlight.Departure.City, f.OriginalFlight.Departure.Name),
                    Arrival = string.Format("{0}{1}", f.OriginalFlight.Arrival.City, f.OriginalFlight.Arrival.Name),
                    f.OriginalFlight.TakeoffTime,
                    f.OriginalFlight.LandingTime,
                    f.OriginalFlight.Id
                });
            dataList.DataBind();
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
                    return "/OrderModule/Operate/RefundApplyformDetail.aspx?id=";
                case OrderRole.Purchaser:
                    return "/OrderModule/Purchase/RefundApplyformDetail.aspx?id=";
                default:
                    return "/OrderModule/Provide/RefundApplyformDetail.aspx?id=";
            }
        }
    }
}