using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase
{
    public partial class EditCredentialsList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtDateStart.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                this.txtDateEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
            btnQuery_Click(this, e);
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            QueryApplyForm(pagination);
        }


        private void QueryApplyForm(Pagination pagination)
        {

            try
            {
            var orderInfos = OrderQueryService.QueryOrders(getCondition(), pagination);
            dataList.DataSource = orderInfos.Select(order =>

                 new
                {
                    order.OrderId,
                    PNR =order.ETDZPNR==null?string.Empty:order.ETDZPNR.ToListString(),
                    AirportPair = order.Flights.Join("<br />",
                                               f =>
                                               string.Format(
                                                             "{0}-{1}",
                                                             f.DepartureCity,
                                                             f.ArrivalCity)),
                    FlightInfo = order.Flights.Join("<br />",
                                                   f => string.Format(
                                                                      "{0}{1}<br />{2} / {3}",
                                                                      f.Carrier,
                                                                      f.FlightNo,
                                                                      string.IsNullOrEmpty(f.Bunk)? "-":f.Bunk,
                                                                      getDiscountText
                                                                          (f.
                                                                               Discount))),
                    TakeoffTime = order.Flights.Join("<br />",
                               f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                    Passengers = string.Join("<br />", order.Passengers),
                    ProducedTime = order.ProducedTime.ToString("yyyy-MM-dd HH:mm"), order.ProducedAccount,
                    allTakeOff = order.Flights.All(item =>item.TakeoffTime < DateTime.Now)
                });
            dataList.DataBind();
            if (pagination.RowCount > 0)
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

        private OrderQueryCondition getCondition()
        {
            var parameter = new OrderQueryCondition()
            {
                PNR = txtPNR.Text.Trim(),
                Passenger = txtPassenger.Text.Trim(),
                ProducedDateRange = new Range<DateTime>(DateTime.Parse(txtDateStart.Text.Trim()), DateTime.Parse(txtDateEnd.Text.Trim()).AddDays(1).AddTicks(-1)),

            };
            if (!String.IsNullOrEmpty(txtOrderformId.Text.Trim()) && Regex.IsMatch(txtOrderformId.Text.Trim(), "\\d+"))
            {
                parameter.OrderId = decimal.Parse(txtOrderformId.Text.Trim());
            }
            parameter.Purchaser = CurrentCompany.CompanyId;
            parameter.Status = OrderStatus.Finished;
            return parameter;
        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination()
                {
                    PageSize = pager.PageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                QueryApplyForm(pagination);
            }
            else
            {
                this.pager.CurrentPageIndex = 1;
            }

        }
        private string getDiscountText(decimal? discount)
        {
            if (discount.HasValue)
            {
                return (discount.Value * 100).TrimInvaidZero();
            }
            return "-";
        }

    }
}