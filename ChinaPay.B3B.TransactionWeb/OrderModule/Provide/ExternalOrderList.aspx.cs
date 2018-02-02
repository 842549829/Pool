using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class ExternalOrderList : BasePage
    {
        protected bool IsFirstLoad;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                if (Request.QueryString["Search"] == "Back") IsFirstLoad = true;
                initData();
                btnQuery_Click(sender, e);
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = newPage
            };
            queryExternalOrderList(pagination);
        }

        private void queryExternalOrderList(Pagination pagination)
        {
            var isPlatform = CurrentCompany.CompanyType == CompanyType.Platform;
            try
            {
                var list = OrderQueryService.QueryExternalOrders(getCondition(), pagination).Select(item =>
                    {
                        var fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero();
                        var airportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero();
                        var BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero();
                        var settleAmount = item.SettlementForPurchaser.Amount.TrimInvaidZero();
                        var rebateAndCommission = string.Format("{0}%/{1}", (item.SettlementForPurchaser.Rebate * 100).TrimInvaidZero(),
                        item.SettlementForPurchaser.Commission.TrimInvaidZero());
                        return new
                        {
                            InternalOrderId = item.OrderId,
                            PlatformType = item.PlatformType.GetDescription(),
                            PlatformTypeValue =(byte)item.PlatformType,
                            OrderId = "<a href=\"ExternalOrderDetail.aspx?id=" + item.OrderId + "\">" + item.ExternalOrderId + "(外部)</a><br />" +
                                       "<a href=\"" + (isPlatform ? "/OrderModule/Operate/OrderDetail.aspx?id=" : "OrderDetail.aspx?id=")+item.OrderId+ "\">" +
                                        item.OrderId + "(内部)</a>",
                            PNR = item.ETDZPNR != null ? item.ETDZPNR.ToListString() : item.ReservationPNR == null ? string.Empty : item.ReservationPNR.ToListString(),
                            FlightInfo = string.Join("<br/>", item.Flights.Select(f => string.Format("{0}{1} {2} {3}-{4}<br/>{5}",
                                f.Carrier, f.FlightNo, f.Bunk, f.DepartureCity, f.ArrivalCity, f.TakeoffTime.ToString("yyyy-MM-dd HH:mm")))),
                            Passengers = string.Join("<br/>", item.Passengers),
                            Price = fare + "</br>" + airportFee + "/" + BAF,
                            Commission = settleAmount + "<br/>" + rebateAndCommission,
                            ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                            PayStatus =item.PayStatus == PayStatus.NoPay?item.PayStatus.GetDescription() :item.PayStatus.GetDescription() + (item.IsAutoPay ? "(自动)" : "(手工)"),
                            Status = item.Status == OrderStatus.Finished ? "已出票" : "未出票",
                            InternalPayStatus = Service.Order.StatusService.GetOrderStatus(item.Status, DataTransferObject.Order.OrderRole.Provider)
                        };
                    });
                this.dataList.DataSource = list;
                this.dataList.DataBind();
                if (list.Count() > 0)
                {
                    this.dataList.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.dataList.Visible = false;
                    this.emptyDataInfo.Visible = true;
                    this.pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private DataTransferObject.Order.External.ExternalOrderCondition getCondition()
        {
            var condition = new DataTransferObject.Order.External.ExternalOrderCondition();
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                condition.StartTime = DateTime.Parse(this.txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                condition.EndTime = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPlatfromType.SelectedValue))
            {
                condition.PlatformType = (PlatformType)int.Parse(this.ddlPlatfromType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPnr.Text))
            {
                condition.Pnr = this.txtPnr.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.txtDepartureCity.Code))
            {
                condition.Departure = this.txtDepartureCity.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.txtArrivalCity.Code))
            {
                condition.Arrival = this.txtArrivalCity.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.txtPassenger.Text.Trim()))
            {
                condition.Passenger = this.txtPassenger.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPayStatus.SelectedValue))
            {
                condition.PayStatus = (PayStatus)int.Parse(this.ddlPayStatus.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtInternalOrderId.Text))
            {
                condition.InternalOrderId = decimal.Parse(this.txtInternalOrderId.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtExternalOrderId.Text))
            {
                condition.ExternalOrderId = this.txtExternalOrderId.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPrintStatus.SelectedValue))
            {
                condition.IsEtdzed = this.ddlPrintStatus.SelectedValue == "1" ? true : false;
            }
            if (CurrentCompany.CompanyType != CompanyType.Platform)
            {
                condition.ProviderId = this.CurrentCompany.CompanyId;
            }
            return condition;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                PageSize = pager.PageSize,
                GetRowCount = true
            };
            queryExternalOrderList(pagination);
        }

        void initData()
        {
            txtStartDate.Text = txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            var platformType = Enum.GetValues(typeof(PlatformType)) as PlatformType[];
            foreach (var item in platformType)
            {
                if (item != PlatformType.B3B)
                    this.ddlPlatfromType.Items.Add(new ListItem(item.GetDescription(), ((byte)item).ToString()));
            }
            this.ddlPlatfromType.Items.Insert(0, new ListItem("全部", ""));
            var payStatus = Enum.GetValues(typeof(PayStatus)) as PayStatus[];
            foreach (var item in payStatus)
            {
                this.ddlPayStatus.Items.Add(new ListItem(item.GetDescription(), ((byte)item).ToString()));
            }
            this.ddlPayStatus.Items.Insert(0, new ListItem("全部", ""));
            LoadCondition("ExternalOrderList");
        }
    }
}