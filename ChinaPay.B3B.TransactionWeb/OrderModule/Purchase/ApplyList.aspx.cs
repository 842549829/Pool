using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase {
    public partial class ApplyList : BasePage {

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartDate.Text = txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
            btnQuery_Click(this, e);
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }
        private void initData() {
            var productValues = Enum.GetValues(typeof(ProductType)) as ProductType[];
            foreach(var item in productValues) {
                this.ddlProduct.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }

            var employees = EmployeeService.QueryEmployees(CurrentCompany.CompanyId);
            this.ddlOperator.DataTextField = "UserName";
            this.ddlOperator.DataValueField = "UserName";
            this.ddlOperator.DataSource = employees;
            this.ddlOperator.DataBind();

            LoadCondition("ApplyList0");
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
                var orders = from item in Service.OrderQueryService.QueryOrders(getCondition(), pagination)
                             select new
                             {
                                 OrderId = item.OrderId,
                                 Product = item.ProductType.GetDescription(),
                                 PNR = item.ETDZPNR == null ? string.Empty : item.ETDZPNR.ToListString(),
                                 AirportPair = item.Flights.Join("<br />", f => string.Format("{0}-{1}", f.DepartureCity, f.ArrivalCity)),
                                 FlightInfo = item.Flights.Join("<br />", f => string.Format("{0}{1}<br />{2} / {3}", f.Carrier,f.FlightNo, string.IsNullOrEmpty(f.Bunk)? "-":f.Bunk, getDiscountText(f.Discount))),
                                 TakeoffTime = item.Flights.Join("<br />", f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                                 Passenger = item.Passengers.Join("<br />"),
                                 Fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero(),
                                 AirportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero(),
                                 BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero(),
                                 SettleAmount = item.SettlementForPurchaser.Amount.TrimInvaidZero(),
                                 RebateAndCommission = item.ProductType==ProductType.Special?string.Empty:string.Format("{0}%/{1}",
                                 (item.SettlementForPurchaser.Rebate * 100).TrimInvaidZero(),
                                 item.SettlementForPurchaser.Commission.TrimInvaidZero()), item.ProducedAccount,
                                 ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss")
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
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }
        private DataTransferObject.Order.OrderQueryCondition getCondition() {
            var condition = new DataTransferObject.Order.OrderQueryCondition() {
                OrderId = ChinaPay.Utility.StringUtility.ToNullableDecimal(this.txtOrderId.Text),
                PNR = this.txtPNR.Text.Trim(),
                Passenger = this.txtPassenger.Text.Trim(),
                ProducedDateRange = new Core.Range<DateTime>(DateTime.Parse(this.txtStartDate.Text), DateTime.Parse(this.txtEndDate.Text)),
                Status = OrderStatus.Finished,
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
    }
}