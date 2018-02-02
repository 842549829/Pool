using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.FlightTransfer;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.SMS.Service;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class FlightChangeNotice : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = lblPlatformName1.Text = lblPlatformName2.Text = PlatformName;
                initData();
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            queryTransferDetail(newPage);
        }

        private void queryTransferDetail(int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = newPage
            };
            queryTransferDetailByPurchase(pagination);
        }

        private void queryTransferDetailByPurchase(Pagination pagination)
        {
            var data = from item in QSService.QueryTransferDetailByPurchase(pagination, this.CurrentCompany.CompanyId)
                       select new
                       {
                           PNR = item.PNR.ToString(),
                           OrderId = item.OrderId,
                           OriginalCarrierName = item.OriginalCarrierName,
                           CarrierName = item.CarrierName,
                           TransferType = item.TransferType.GetDescription(),
                           TransferTypeValue = (byte)item.TransferType,
                           OriginalFlightNo = item.OriginalFlightNo,
                           FlightNo = item.FlightNo,
                           OriginalTakeoffTime = item.OriginalTakeoffTime,
                           TakeoffTime = item.TakeoffTime.HasValue?item.TakeoffTime.ToString():string.Empty,
                           OriginalArrivalTime =item.OriginalArrivalTime,
                           ArrivalTime = item.ArrivalTime.HasValue?item.ArrivalTime.ToString():string.Empty,
                           TransferId = item.TransferId,
                           IsBeyondThreeDay =item.OriginalTakeoffTime.Date<=DateTime.Now.AddDays(2).Date
                       };
            this.rptNotify.DataSource = data;
            this.rptNotify.DataBind();
            if (data.Count() > 0)
            {
                rptNotify.Visible = true;
                pager.Visible = true;
                emptyDataInfo.Visible = false;
                if (pagination.GetRowCount)
                {
                    pager.RowCount = pagination.RowCount;
                }
            }
            else
            {
                rptNotify.Visible = false;
                pager.Visible = false;
                emptyDataInfo.Visible = true;
            }
        }

        private void initData()
        {
            var change = QSService.QueryPurchaseFlightStaticInfo(this.CurrentCompany.CompanyId);
            if (change == null || change.OrderCount ==0)
            {
                flightChangeWithoutData.Visible = true;
                flightChangeHasData.Visible = false;
                this.emptyDataInfo.Visible = true;
            }
            else
            {
                lblFlightChangeTime.Text = change.LastQSTime.ToString();
                lblFlightCount.Text = change.FlightCount.ToString();
                lblOrderCount.Text = change.OrderCount.ToString();
                queryTransferDetail(1);
            }

            if (!string.IsNullOrWhiteSpace(this.CurrentCompany.OfficePhones))
            {
                this.hfdPurchasePhone.Value = this.CurrentCompany.OfficePhones;
            }
            else
            {
                this.hfdPurchasePhone.Value = this.CurrentCompany.ContactPhone;
            }
            var query = SMSAccountService.QueryAccount(CurrentCompany.CompanyId);
            if (query != null)
            {
                this.lblMessageCount.Text = query.Balance.ToString();
            }
            else
            {
                this.lblMessageCount.Text = "0";
            }
        }
    }
}