using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using ChinaPay.Core;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Report;
using System.Data;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class PurchaseFinancialReport : BasePage
    {
        public string totalTradeAmount = "￥0.00";
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            this.dataList.Visible = false;
            if (!IsPostBack)
            {
                initData();
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageIndex = newPage,
                GetRowCount = true,
                PageSize = pager.PageSize
            };
            queryPurchaseTicket(pagination);
        }

        private void initData()
        {
            seniorCondition.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (DateTime.Now.CompareTo(DateTime.Parse("18:00")) > 0)
            {
                this.txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                this.txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            this.hfdPurchaseCompanyId.Value = this.CurrentCompany.CompanyId.ToString();
            this.ddlAirlines.DataSource = from item in FoundationService.Airlines
                                          select new
                                          {
                                              Name = item.Code + "-" + item.ShortName,
                                              Code = item.Code
                                          };
            this.ddlAirlines.DataTextField = "Name";
            this.ddlAirlines.DataValueField = "Code";
            this.ddlAirlines.DataBind();
            this.ddlAirlines.Items.Insert(0, new ListItem("全部", ""));
            //机票状态
            var ticketStatus = Enum.GetValues(typeof(TicketState)) as TicketState[];
            foreach (var item in ticketStatus)
            {
                this.ddlTicketStatus.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlTicketStatus.Items.Insert(0, new ListItem("全部", ""));
            //机票类型
            var ticketType = Enum.GetValues(typeof(TicketType)) as TicketType[];
            foreach (var item in ticketType)
            {
                this.ddlTiketType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlTiketType.Items.Insert(0, new ListItem("全部", ""));
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.hfdSeniorCondition.Value == "show")
            {
                this.seniorCondition.Style.Add(HtmlTextWriterStyle.Display, "");
                this.btnSeniorCondition.Value = "简化条件";
            }
            else
            {
                this.seniorCondition.Style.Add(HtmlTextWriterStyle.Display, "none");
                this.btnSeniorCondition.Value = "更多条件";
            }
            if (valiate())
            {
                if (this.pager.CurrentPageIndex == 1)
                {
                    var pagination = new Pagination()
                    {
                        PageIndex = 1,
                        PageSize = pager.PageSize,
                        GetRowCount = true
                    };
                    queryPurchaseTicket(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        private void queryPurchaseTicket(Pagination pagination)
        {
            try
            {
                decimal totaltradeAmount;
                DataTable list = ReportService.QueryPurchaseFinancial(pagination, getCondition(), out  totaltradeAmount);
                var tradeAmount = list.Compute("Sum(TradeAmount)", "");
                if (tradeAmount != DBNull.Value)
                {
                    totalTradeAmount = "￥" + tradeAmount.ToString();
                }
                this.dataList.DataSource = list;
                this.dataList.DataBind();
                if (list.Rows.Count > 0)
                {
                    counts.Visible = true;
                    this.pager.Visible = true;
                    this.dataList.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    lblTradeAmount.Text = "￥" + totaltradeAmount + "元";
                }
                else
                {
                    counts.Visible = false;
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private DataTransferObject.Report.PurchaseTicketView getCondition()
        {
            var view = new DataTransferObject.Report.PurchaseTicketView();
            view.CompanyId = this.CurrentCompany.CompanyId;
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                view.FinishBeginDate = DateTime.Parse(this.txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                view.FinishEndDate = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPayStartDate.Text))
            {
                view.PayBeginDate = DateTime.Parse(this.txtPayStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPayEndDate.Text))
            {
                view.PayEndDate = DateTime.Parse(this.txtPayEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }

            if (!string.IsNullOrWhiteSpace(this.txtTakeOffStartDate.Text))
            {
                view.TakeoffBeginDate = DateTime.Parse(this.txtTakeOffStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtTakeOffEndDate.Text))
            {
                view.TakeoffEndDate = DateTime.Parse(this.txtTakeOffEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }

            if (!string.IsNullOrWhiteSpace(this.txtTicketNo.Text))
            {
                view.TicketNo = this.txtTicketNo.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(this.txtPNR.Text))
            {
                view.PNR = this.txtPNR.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(this.txtPassenger.Text))
            {
                view.Passenger = this.txtPassenger.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(this.txtDeparture.Code))
            {
                view.Departure = this.txtDeparture.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.txtArrivals.Code))
            {
                view.Arrival = this.txtArrivals.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlTicketStatus.SelectedValue))
            {
                view.TicketState = (TicketState)int.Parse(this.ddlTicketStatus.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPolicyType.SelectedValue))
            {
                view.PolicyType = Convert.ToByte(this.ddlPolicyType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                view.Airline = this.ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlTiketType.SelectedValue))
            {
                view.TicketType = (TicketType)int.Parse(this.ddlTiketType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtOrderId.Text))
            {
                view.OrderId = decimal.Parse(this.txtOrderId.Text.Trim());
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPayType.SelectedValue))
            {
                view.PayType = this.ddlPayType.SelectedValue == "1";
            }
            return view;
        }

        private bool valiate()
        {
            if (this.txtOrderId.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtOrderId.Text.Trim(), @"^\d{1,13}$"))
            {
                ShowMessage("订单号格式错误！");
                return false;
            }
            if (this.txtPassenger.Text.Trim().Length > 25)
            {
                ShowMessage("乘机人位数不能超过25位！");
                return false;
            }
            if (this.txtTicketNo.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtTicketNo.Text.Trim(), @"^\d{10}$"))
            {
                ShowMessage("票号格式错误！");
                return false;
            }
            if (this.txtPNR.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtPNR.Text.Trim(), @"^\w{6}$"))
            {
                ShowMessage("PNR编码格式错误！");
                return false;
            }
            return true;
        }
    }
}