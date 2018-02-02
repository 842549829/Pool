using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate {
    public partial class EditCredentialsSearch : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtDateStart.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                txtDateEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
                btnQuery_Click(this, e);
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage) {
            var pagination = new Pagination {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            QueryApplyForm(pagination);
        }


        private void QueryApplyForm(Pagination pagination) {
            try {
                var orderInfos = OrderQueryService.QueryCredentialsUpdateInfos(getCondition(), pagination);
                dataList.DataSource = orderInfos.Select(order =>
                                                        new
                                                            {
                                                                order.OrderId,
                                                                PNR = order.PNR == null ? string.Empty : order.PNR.ToListString(),
                                                                AirportPair = order.Flights.Join("<br />", f => f.DepartureCity + "-" + f.ArrivalCity),
                                                                FlightInfo = order.Flights.Join("<br />",
                                                                    f => string.Format(
                                                                        "{0}{1}<br />{2} / {3}",
                                                                        f.Carrier,
                                                                        f.FlightNo,
                                                                        string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk,
                                                                        getDiscountText(f.Discount))),
                                                                TakeoffTime = order.Flights.Join("<br />", f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                                                                order.Passenger,
                                                                order.PassengerName,
                                                                CommitTime = order.CommitTime.ToString("yyyy-MM-dd<br />HH:mm"),
                                                                order.PurchaserName,
                                                                order.OriginalCredentials,
                                                                order.NewCredentials,
                                                                Status = order.Success ? "成功" : "失败",
                                                                IsFail = !order.Success,
                                                                order.Id
                                                            });
                dataList.DataBind();
                if (pagination.RowCount>0)
                {
                    pager.Visible = true;
                    if(pagination.GetRowCount) {
                        pager.RowCount = pagination.RowCount;
                    }
                } else {
                    pager.Visible = false;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private CredentialsUpdateInfoQueryCondition getCondition() {
            var parameter = new CredentialsUpdateInfoQueryCondition {
                PNR = txtPNR.Text.Trim(),
                Passenger = txtPassenger.Text.Trim(),
                CommitDateRange =
                    new Range<DateTime>(DateTime.Parse(txtDateStart.Text.Trim()),
                        DateTime.Parse(txtDateEnd.Text.Trim()).AddDays(1).AddTicks(-1)),
            };
            if(!String.IsNullOrEmpty(txtOrderformId.Text.Trim()) && Regex.IsMatch(txtOrderformId.Text.Trim(), "\\d+")) {
                parameter.OrderId = decimal.Parse(txtOrderformId.Text.Trim());
            }
            return parameter;
        }


        protected void btnQuery_Click(object sender, EventArgs e) {
            if(pager.CurrentPageIndex == 1) {
                var pagination = new Pagination {
                    PageSize = pager.PageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                QueryApplyForm(pagination);
            } else {
                pager.CurrentPageIndex = 1;
            }
        }

        private string getDiscountText(decimal? discount) {
            if(discount.HasValue) {
                return (discount.Value * 100).TrimInvaidZero();
            }
            return "-";
        }

        protected void DealOrder(object source, RepeaterCommandEventArgs e) {
            try {
                var orderId = Guid.Parse(e.CommandArgument.ToString());
                OrderProcessService.HandlingCredentialsForUpdateFailed(orderId, CurrentUser.UserName, BasePage.OwnerOEMId);
                ShowMessage("处理完成！");
                var pagination = new Pagination {
                    PageSize = pager.PageSize,
                    PageIndex = pager.CurrentPageIndex,
                    GetRowCount = true
                };
                QueryApplyForm(pagination);
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
            btnQuery_Click(this, null);
        }
    }
}