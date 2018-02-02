using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase {
    public partial class PayOrderList : BasePage {

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtStartDate.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
                btnQuery_Click(this, e);
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }
        private void initData() {
            var productValues = Enum.GetValues(typeof(ProductType));
            foreach(ProductType item in productValues) {
                this.ddlProduct.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }

            var employees = Service.Organization.EmployeeService.QueryEmployees(CurrentCompany.CompanyId);
            this.ddlOperator.DataTextField = "UserName";
            this.ddlOperator.DataValueField = "UserName";
            this.ddlOperator.DataSource = employees;
            this.ddlOperator.DataBind();

            LoadCondition("PayOrder");
        }
        protected void btnQuery_Click(object sender, EventArgs e) {
            var pagination = new Pagination() {
                PageSize = pager.PageSize,
                PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                GetRowCount = true
            };
            queryOrders(pagination);
        }
        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage) {
            var pagination = new Pagination() {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryOrders(pagination);
        }

        void queryOrders(Pagination pagination) {
            try {
                var isAdmin = CurrentUser.IsAdministrator;
                IEnumerable<OrderListView> orderListViews = OrderQueryService.QueryOrders(getCondition(), pagination).ToList();
                var lockInfos = LockService.Query(orderListViews.Select(form => form.OrderId.ToString())).ToList();
                var orders = from item in orderListViews
                             let IsShowPNR = item.Source == OrderSource.CodeImport ||
                             item.Source == OrderSource.ContentImport ||
                             item.Source == OrderSource.InterfaceOrder ||
                             (item.Status > OrderStatus.PaidForSupply && item.Status != OrderStatus.Canceled)
                             let lockInfo = lockInfos.FirstOrDefault(l => l.Key == item.OrderId.ToString())
                             select new
                             {
                                 OrderId = item.OrderId,
                                 PNR = item.ETDZPNR != null ? item.ETDZPNR.ToListString() : item.ReservationPNR == null || !IsShowPNR ? string.Empty : item.ReservationPNR.ToListString(),
                                 Product = item.ProductType.GetDescription(),
                                 AirportPair = item.Flights.Join("<br />", f => string.Format("{0}-{1}", f.DepartureCity, f.ArrivalCity)),
                                 FlightInfo = item.Flights.Join("<br />", f => string.Format("{0}{1}<br />{2}/{3}", f.Carrier, f.FlightNo, string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk, getDiscountText(f.Discount))),
                                 TakeoffTime = item.Flights.Join("<br />", f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                                 Passenger = item.Passengers.Join("<br />"),
                                 Fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero(),
                                 AirportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero(),
                                 BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero(),
                                 SettleAmount = item.SettlementForPurchaser.Amount.TrimInvaidZero(),
                                 RebateAndCommission = item.ProductType == ProductType.Special ? string.Empty : string.Format("{0}%/{1}",
                                 (item.SettlementForPurchaser.Rebate * 100).TrimInvaidZero(),
                                 item.SettlementForPurchaser.Commission.TrimInvaidZero()),
                                 item.ProducedAccount,
                                 ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                                 item.ProducedAccountName,
                                 UnLockEnable = isAdmin && lockInfo != null && lockInfo.Account != CurrentUser.UserName
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
                PNR = this.txtPNR.Text.Trim(),
                Passenger = this.txtPassenger.Text.Trim(),
                ProducedDateRange = new Core.Range<DateTime>(DateTime.Parse(this.txtStartDate.Text), DateTime.Parse(this.txtEndDate.Text)),
                Status = OrderStatus.Ordered,
                Purchaser = CurrentCompany.CompanyId
            };
            if(!string.IsNullOrWhiteSpace(this.ddlProduct.SelectedValue)) {
                condition.ProductType = (ProductType)int.Parse(this.ddlProduct.SelectedValue);
            }
            if(!string.IsNullOrWhiteSpace(this.ddlOperator.SelectedValue)) {
                condition.ProducedAccount = this.ddlOperator.SelectedValue;
            }
            return condition;
        }
        private string getDiscountText(decimal? discount) {
            if(discount.HasValue) {
                return (discount.Value * 100).TrimInvaidZero();
            }
            return "-";
        }

        protected void dataList_ItemCommand(object source, RepeaterCommandEventArgs e) {
            try {
                if(e.CommandName == "UnlockAndPay") {
                    LockService.UnLockForcibly(e.CommandArgument.ToString());
                    var error = string.Empty;
                    var lockOk = Lock(e.CommandArgument.ToString().ToDecimal(), LockRole.Purchaser, "代付锁定", out error);
                    if(!lockOk) {
                        ShowMessage(error);
                    } else {
                        Response.Redirect("OrderPay.aspx?returnUrl=PayOrderList.aspx&Id=" + e.CommandArgument + "&Search=Back");
                    }
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "解锁代付");
            }
        }
    }
}