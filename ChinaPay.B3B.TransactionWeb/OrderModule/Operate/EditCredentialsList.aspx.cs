﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using Enumerable = System.Linq.Enumerable;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class EditCredentialsList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                txtDateStart.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                txtDateEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
                LoadCondition("EditNo");
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
            QueryApplyForm(pagination);
        }


        private void QueryApplyForm(Pagination pagination)
        {
            try
            {
                IEnumerable<OrderListView> orderInfos = OrderQueryService.QueryOrders(getCondition(), pagination);
                dataList.DataSource = Enumerable.Select(orderInfos, order =>
                                                                        {
                                                                            return new
                                                                                {
                                                                                    order.OrderId,
                                                                                    PNR = order.ETDZPNR == null ? string.Empty : order.ETDZPNR.ToListString(),
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
                                                                                            string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk, getDiscountText(f.Discount))),
                                                                                    TakeoffTime = order.Flights.Join("<br />",
                                                                                        f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                                                                                    Passengers = string.Join("<br />", order.Passengers),
                                                                                    ProducedTime =
                                                                                        order.ProducedTime.ToString("yyyy-MM-dd HH:mm"),
                                                                                    order.ProducedAccount,
                                                                                    order.PurchaserName,
                                                                                    allTakeOff = order.Flights.All(item=>item.TakeoffTime < DateTime.Now)
                                                                                };
                                                                        });
                dataList.DataBind();
                if (orderInfos.Any())
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
            var parameter = new OrderQueryCondition
                {
                    PNR = txtPNR.Text.Trim(),
                    Passenger = txtPassenger.Text.Trim(),
                    ProducedDateRange =
                        new Range<DateTime>(DateTime.Parse(txtDateStart.Text.Trim()),
                            DateTime.Parse(txtDateEnd.Text.Trim()).AddDays(1).AddTicks(-1)),
                };
            if (!String.IsNullOrEmpty(txtOrderformId.Text.Trim()) && Regex.IsMatch(txtOrderformId.Text.Trim(), "\\d+"))
            {
                parameter.OrderId = decimal.Parse(txtOrderformId.Text.Trim());
            }
            parameter.Status = OrderStatus.Finished;
            return parameter;
        }


        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                    GetRowCount = true
                };
            QueryApplyForm(pagination);
        }

        private string getDiscountText(decimal? discount)
        {
            if (discount.HasValue)
            {
                return (discount.Value*100).TrimInvaidZero();
            }
            return "-";
        }
    }
}