using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide {
    public partial class ResourceList : BasePage {
        private bool m_IsSupplier;
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            m_IsSupplier = CurrentCompany.CompanyType == CompanyType.Supplier;
            if(!IsPostBack) {
                txtStartDate.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
                btnQuery_Click(this, e);
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void initData() {
            var orderRole = m_IsSupplier ? OrderRole.Supplier : OrderRole.Provider;
            this.ddlStatus.Items.Clear();
            this.ddlStatus.Items.Add(new ListItem("全部", string.Empty));
            this.ddlStatus.Items.Add(new ListItem(Service.Order.StatusService.GetOrderStatus(OrderStatus.Applied, orderRole), ((int)OrderStatus.Applied).ToString()));
            this.ddlStatus.Items.Add(new ListItem(Service.Order.StatusService.GetOrderStatus(OrderStatus.PaidForSupply, orderRole), ((int)OrderStatus.PaidForSupply).ToString()));
            if(Request.QueryString["type"] == "Applied") ddlStatus.SelectedIndex = 1;
            else if(Request.QueryString["type"] == "PaidForSupply") ddlStatus.SelectedIndex = 2;
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage) {
            var pagination = new Pagination() {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryOrders(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e) {
            if(this.pager.CurrentPageIndex == 1) {
                var pagination = new Pagination() {
                    PageSize = pager.PageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                queryOrders(pagination);
            } else {
                this.pager.CurrentPageIndex = 1;
            }
        }

        void queryOrders(Pagination pagination) {
            try {
                var orderListViews = OrderQueryService.QueryOrders(getCondition(), pagination).ToList();
                var lockInfos = LockService.Query(orderListViews.Select(form => form.OrderId.ToString())).ToList();
                var orders = from item in orderListViews
                             let lockInfo = lockInfos.FirstOrDefault(l => l.Key == item.OrderId.ToString())
                             select new
                             {
                                 OrderId = item.OrderId,
                                 AirportPair = item.Flights.Join("<br />", f => string.Format("{0}-{1}", f.DepartureCity, f.ArrivalCity)),
                                 FlightInfo = item.Flights.Join("<br />", f => f.Carrier + f.FlightNo),
                                 TakeoffTime = item.Flights.Join("<br />", f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                                 Passenger = item.Passengers.Join("<br />"),
                                 Fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero(),
                                 AirportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero(),
                                 BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero(),
                                 Status = Service.Order.StatusService.GetOrderStatus(item.Status, m_IsSupplier ? OrderRole.Supplier : OrderRole.Provider),
                                 ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                                 LockInfo = lockInfo == null ? string.Empty :  lockInfo.Name+"<br />",
                                 RemindContent = item.RemindTime.HasValue ? item.RemindContent : string.Empty,
                                 RemindIsShow = item.IsNeedReminded
                             };
                this.dataList.DataSource = orders;
                this.dataList.DataBind();
                if(orders.Any()) {
                    this.pager.Visible = true;
                    if(pagination.GetRowCount) {
                        this.pager.RowCount = pagination.RowCount;
                    }
                } else {
                    this.pager.Visible = false;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private OrderQueryCondition getCondition() {
            var condition = new OrderQueryCondition() {
                OrderId = ChinaPay.Utility.StringUtility.ToNullableDecimal(this.txtOrderId.Text),
                Passenger = this.txtPassenger.Text.Trim(),
                ProductType = ProductType.Special,
                ProducedDateRange = new Core.Range<DateTime>(DateTime.Parse(this.txtStartDate.Text), DateTime.Parse(this.txtEndDate.Text))
            };
            if(CurrentCompany.CompanyType == Common.Enums.CompanyType.Supplier) {
                condition.Supplier = CurrentCompany.CompanyId;
            } else if(CurrentCompany.CompanyType == Common.Enums.CompanyType.Provider) {
                condition.Provider = CurrentCompany.CompanyId;
            }
            if(string.IsNullOrWhiteSpace(this.ddlStatus.SelectedValue)) {
                condition.Status = OrderStatus.Applied | OrderStatus.PaidForSupply;
            } else {
                condition.Status = (OrderStatus)int.Parse(this.ddlStatus.SelectedValue);
            }
            return condition;
        }
    }
}