using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.Utility;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class WaitOrderListOld : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
                btnQuery_Click(this, e);
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = newPage,
                    GetRowCount = true
                };
            queryOrders(pagination);
        }



        private void initData()
        {
            var productValues = Enum.GetValues(typeof(ProductType)) as ProductType[];
            foreach (ProductType item in productValues)
            {
                ddlProduct.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }


            ddlFlightCompany.DataTextField = "ShortName";
            ddlFlightCompany.DataValueField = "Code";
            ddlFlightCompany.DataSource = from item in FoundationService.Airlines.Join(PolicySetService.QueryAirlines(CurrentCompany.CompanyId), p => p.Code.Value, p => p, (p, q) => p)
                                          select new
                                          {
                                              ShortName = item.Code +"-"+item.ShortName,
                                              Code= item.Code
                                          };
            ddlFlightCompany.DataBind();

            ddlOfficeNumber.DataSource = CompanyService.QueryOfficeNumbers(CurrentCompany.CompanyId).Select(o=>o.Number);
            ddlOfficeNumber.DataBind();

           LoadCondition("WaitOrderList");
        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
                var pagination = new Pagination
                    {
                        PageSize = pager.PageSize,
                        PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex=1,
                        GetRowCount = true
                    };
                queryOrders(pagination);
        }

      
        private void queryOrders(Pagination pagination)
        {
            try
            {
                List<OrderListView> orders = OrderQueryService.QueryOrders(GetCondition(), pagination).ToList();
                List<OrderListView> SortedOrders = orders.Where(o => o.Flights.Min(f => f.TakeoffTime.Date) == DateTime.Today).ToList();
                SortedOrders.AddRange(orders.Where(o => o.Flights.Min(f => f.TakeoffTime.Date) != DateTime.Today).OrderByDescending(o=>o.IsEmergentOrder).ThenBy(o => o.PayTime));
                var lockInfos = LockService.Query(orders.Select(form => form.OrderId.ToString())).ToList();

                dataList.DataSource = SortedOrders.Select(item =>
                                                        {
                                                            LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == item.OrderId.ToString());
                                                            var isEmergentOrder = Service.Order.StatusService.GetIsEmergentOrder(item.IsEmergentOrder, item.Status);
                                                                            DateTime? startStatTime = null;
                if (CurrentCompany.CompanyType==CompanyType.Supplier)
                {
                }else if (item.IsSpecial && item.Supplier.HasValue)
                {
                    if (item.SupplyTime.HasValue&&item.PayTime.HasValue)
                    {
                        startStatTime = item.SupplyTime.Value > item.PayTime.Value ? item.SupplyTime.Value : item.PayTime.Value;
                    }
                }
                else
                {
                    startStatTime = item.PayTime;
                }
                                                            bool isRelation = item.PurcharseProviderRelation == null || item.PurcharseProviderRelation == RelationType.Brother;

                                                            return new
                                                                {
                                                                    item.OrderId,
                                                                    Product = item.ProductType.GetDescription(),
                                                                    PNR =
                                                                        item.ETDZPNR != null
                                                                            ? item.ETDZPNR.ToListString()
                                                                            : item.ReservationPNR == null ? string.Empty : item.ReservationPNR.ToListString(),
                                                                    AirportPair =
                                                                        item.Flights.Join("<br />",
                                                                            f =>
                                                                            string.Format(
                                                                                "{0}{1} {2} {3}-{4}<br />{5:yyyy-MM-dd HH:mm}",
                                                                                f.Carrier,
                                                                                f.FlightNo,
                                                                                string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk,                                                                             
                                                                                f.DepartureCity,
                                                                                f.ArrivalCity,
                                                                                f.TakeoffTime)),
                                                                    Passenger = item.Passengers.Join("<br />"),
                                                                    Fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero(),
                                                                    AirportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero(),
                                                                    BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero(),
                                                                    SettleAmount = item.SettlementForProvider.Amount.TrimInvaidZero(),
                                                                    Rebate = (item.SettlementForProvider.Rebate * 100).TrimInvaidZero() + "%",
                                                                    Commission = item.SettlementForProvider.Commission.TrimInvaidZero(),
                                                                    Status = StatusService.GetOrderStatus(item.Status, OrderRole.Provider),
                                                                    //EmergentOrderContnt = isEmergentOrder ? string.Format("<a href='javascript:void(0);' class='tips_btn urgent'>紧急</a><div class='tips_box hidden'><div class='tips_bd'><p>{0}</p></div></div>", emergentOrderContnt) : string.Empty,
                                                                    ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                                                                    ProductType = item.ProviderProductType.GetDescription(),
                                                                    LockInfo =lockInfo == null? string.Empty:lockInfo.Name+"<br />",
                                                                    OfficeNum = item.OfficeNo,
                                                                    PurchaseIsBother = isRelation,
                                                                    Relation =  isRelation ? 
                                                                    "平台" : item.PurcharseProviderRelation == RelationType.Interior?"内部":"下级",
                                                                    TodaysFlight = item.Flights.Any(f=>f.TakeoffTime.Date==DateTime.Today),
                                                                    PassengerType = item.PassengerType.GetDescription(),
                                                                    IsChildTicket = item.PassengerType == PassengerType.Child,
                                                                    ETDZTime = (startStatTime.HasValue && item.RefuseETDZTime.HasValue) ? Math.Round((item.RefuseETDZTime.Value - startStatTime.Value).TotalMinutes).ToString() : (startStatTime.HasValue) ? Math.Round(((item.ETDZTime ?? DateTime.Now) - startStatTime.Value).TotalMinutes).ToString() :string.Empty,
                                                                    TicketType = item.IsSpecial ? string.Empty : string.Format("({0})", item.TicketType.ToString()),
                                                                    RemindContent = item.RemindTime.HasValue ? item.RemindContent : string.Empty,
                                                                    RemindIsShow = item.IsNeedReminded
                                                                };
                                                        });
                dataList.DataBind();
                if (orders.Any())
                {
                    pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private OrderQueryCondition GetCondition()
        {
            var condition = new OrderQueryCondition
                {
                    OrderId = StringUtility.ToNullableDecimal(txtOrderId.Text),
                    PNR = txtPNR.Text.Trim(),
                    Passenger = txtPassenger.Text.Trim(),
                    ProducedDateRange = new Range<DateTime>(DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text)),
                    Provider = CurrentCompany.CompanyId
                };
            if (!string.IsNullOrWhiteSpace(ddlProduct.SelectedValue))
            {
                condition.ProviderProductType = (ProductType)int.Parse(ddlProduct.SelectedValue);
            }
            condition.Status = OrderStatus.PaidForETDZ;
            condition.Passenger = txtPassenger.Text.Trim();
            if (!string.IsNullOrEmpty(ddlOfficeNumber.SelectedValue.Trim()))
            {
                condition.OfficeNo = ddlOfficeNumber.SelectedItem.Text.Trim();
            }
            if (!string.IsNullOrEmpty(ddlFlightCompany.SelectedValue.Trim()))
            {
                condition.Carrier = ddlFlightCompany.SelectedValue.Trim();
            }
            var setting = CompanyService.GetWorkingSetting(CurrentCompany.CompanyId);
            if (setting != null && setting.IsImpower)
            {
                condition.CustomNo = CompanyService.GetCustomNumberByEmployee(CurrentUser.Id).Join(",", c => c.Number);
            }
            return condition;
        }
    }
}