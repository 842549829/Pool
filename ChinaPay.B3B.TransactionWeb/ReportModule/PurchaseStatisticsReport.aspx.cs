using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Report;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class PurchaseStatisticsReport : BasePage
    {
        public string totalOrderCount = "0";
        public string totalTicketCount = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            this.dataList.Visible = false;
            if (!IsPostBack)
            {
                initData();
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = newPage
            };
            queryPurchaseStatisticsReport(pagination);
        }

        private void queryPurchaseStatisticsReport(Pagination pagination)
        {
            try
            {
                int totalOrderCount, totalTicketCount;
                var list = ReportService.QueryPurchaseStatistics(pagination, getCondition(), out  totalOrderCount, out  totalTicketCount);
                var orderCount = list.Compute("Sum(OrderCount)", "");
                var ticketCount = list.Compute("Sum(TicketCount)", "");
                if (orderCount != DBNull.Value)
                {
                    this.totalOrderCount = orderCount.ToString();
                }
                if (ticketCount != DBNull.Value)
                {
                    this.totalTicketCount = ticketCount.ToString();
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
                    lblOrderCount.Text = totalOrderCount + "张";
                    lblPiaoCount.Text = totalTicketCount + "张";
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

        private DataTransferObject.Report.PurchaseStatisticView getCondition()
        {
            var view = new DataTransferObject.Report.PurchaseStatisticView();
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                view.ReportStartDate = DateTime.Parse(this.txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                view.ReportEndDate = DateTime.Parse(this.txtEndDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtDeparture.Code))
            {
                view.Departure = this.txtDeparture.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                view.Carrier = this.ddlAirlines.SelectedValue;
            }
            view.IsHasTrade = this.chkHasTrade.Checked;
            if (PurchaseCompany.CompanyId.HasValue)
            {
                view.Purchase = PurchaseCompany.CompanyId;
            }
            return view;
        }

        private void initData()
        {
            this.txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            BindAriline(this.CurrentCompany.CompanyId);
            BindCity(this.CurrentCompany.CompanyId);
            //var companies = CompanyService.GetCompanies(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
            this.PurchaseCompany.SetCompanyType(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination()
                {
                    PageIndex = 1,
                    GetRowCount = true,
                    PageSize = pager.PageSize
                };
                queryPurchaseStatisticsReport(pagination);
            }
            else
            {
                this.pager.CurrentPageIndex = 1;
            }
        }

        private void BindAriline(Guid company)
        {
            var allAirlines = FoundationService.Airlines;
            foreach (var item in allAirlines)
            {
                if (item.Valid)
                {
                    ListItem listItem = new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value);
                    this.ddlAirlines.Items.Add(listItem);
                }
            }
            this.ddlAirlines.Items.Insert(0, new ListItem("全部", ""));
        }

        private void BindCity(Guid company)
        {
            var result = new List<Airport>();
            var allAirports = FoundationService.Airports;
            foreach (Airport item in allAirports)
            {
                if (item.Valid)
                {
                    result.Add(item);
                }
            }
            this.txtDeparture.InitData(result);
        }
    }
}