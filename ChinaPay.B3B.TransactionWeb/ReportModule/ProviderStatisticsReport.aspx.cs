using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Report;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using Airport = ChinaPay.B3B.TransactionWeb.UserControl.Airport;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class ProviderStatisticsReport : BasePage
    {
        public string totalOrderCount = "0";
        public string totalTicketCount = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            dataList.Visible = false;
            if (!IsPostBack)
            {
                initData();
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
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
                int orderCounts, ticketCounts;
                DataTable list = ReportService.QueryProviderStatistics(pagination, getCondition(), out orderCounts, out ticketCounts);
                object orderCount = list.Compute("Sum(OrderCount)", "");
                object ticketCount = list.Compute("Sum(TicketCount)", "");
                if (orderCount != DBNull.Value)
                {
                    totalOrderCount = orderCount.ToString();
                }
                if (ticketCount != DBNull.Value)
                {
                    totalTicketCount = ticketCount.ToString();
                }
                dataList.DataSource = list;
                dataList.DataBind();
                if (list.Rows.Count > 0)
                {
                    counts.Visible = true;
                    pager.Visible = true;
                    dataList.Visible = true;
                    emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        pager.RowCount = pagination.RowCount;
                    }
                    lblOrderCount.Text = orderCounts + "张";
                    lblPiaoCount.Text = ticketCounts + "张";
                }
                else
                {
                    counts.Visible = false;
                    pager.Visible = false;
                    emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private ProviderStatisticSearchCondition getCondition()
        {
            var condition = new ProviderStatisticSearchCondition();
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
            {
                condition.ReportStartDate = DateTime.Parse(txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                condition.ReportEndDate = DateTime.Parse(txtEndDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtDeparture.Code))
            {
                condition.Departure = txtDeparture.Code;
            }
            if (!string.IsNullOrWhiteSpace(ddlAirlines.SelectedValue))
            {
                condition.Carrier = ddlAirlines.SelectedValue;
            }
            condition.IsHasTrade = chkHasTrade.Checked;
            if (ProviderCompany.CompanyId.HasValue)
            {
                condition.Provider = ProviderCompany.CompanyId;
            }
            if (!string.IsNullOrEmpty(CityArrival.Code))
            {
                condition.Arrival = CityArrival.Code;
            }
            if (!string.IsNullOrWhiteSpace(ddlProductType.SelectedValue))
            {
                condition.ProductType = (ProductType) int.Parse(ddlProductType.SelectedValue);
                if (condition.ProductType.Value == ProductType.Special && !string.IsNullOrWhiteSpace(ddlSpecialTickType.SelectedValue))
                {
                    condition.SpecialProductType = (SpecialProductType) int.Parse(ddlSpecialTickType.SelectedValue);
                }
            }
            if (!String.IsNullOrWhiteSpace(ddlSaleRelation.SelectedValue))
            {
                condition.SaleRelation = (RelationType) int.Parse(ddlSaleRelation.SelectedValue);
            }
            return condition;
        }

        private void initData()
        {
            txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            BindAriline(CurrentCompany.CompanyId);
            BindCity(CurrentCompany.CompanyId, txtDeparture);
            BindCity(CurrentCompany.CompanyId, CityArrival);
            BindSaleRelation();
            BindProductType();
            //IEnumerable<CompanyInitInfo> companies = CompanyService.GetCompanies(CompanyType.Provider, true);
            ProviderCompany.SetCompanyType(CompanyType.Provider, true);
        }

        private void BindProductType()
        {
            var products = from ProductType t in Enum.GetValues(typeof (ProductType))
                           select new
                               {
                                   Text = t.GetDescription(),
                                   Value = (int) t
                               };
            var specialProcut = from SpecialProductType sp in Enum.GetValues(typeof (SpecialProductType))
                                select new
                                    {
                                        Text = sp.GetDescription(),
                                        Value = (int) sp
                                    };
            ddlProductType.DataSource = products;
            ddlProductType.DataTextField = "Text";
            ddlProductType.DataValueField = "Value";
            ddlProductType.DataBind();
            ddlSpecialTickType.DataSource = specialProcut;
            ddlSpecialTickType.DataTextField = "Text";
            ddlSpecialTickType.DataValueField = "Value";
            ddlSpecialTickType.DataBind();
        }

        private void BindSaleRelation()
        {
            var relations = from RelationType r in Enum.GetValues(typeof (RelationType))
                            select new
                                {
                                    Text = r.GetDescription(),
                                    Value = (int) r
                                };
            ddlSaleRelation.DataSource = relations;
            ddlSaleRelation.DataTextField = "Text";
            ddlSaleRelation.DataValueField = "Value";
            ddlSaleRelation.DataBind();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                    {
                        PageIndex = 1,
                        GetRowCount = true,
                        PageSize = pager.PageSize
                    };
                queryPurchaseStatisticsReport(pagination);
            }
            else
            {
                pager.CurrentPageIndex = 1;
            }
        }

        private void BindAriline(Guid company)
        {
            IEnumerable<Airline> allAirlines = FoundationService.Airlines;
            foreach (Airline item in allAirlines)
            {
                if (item.Valid)
                {
                    var listItem = new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value);
                    ddlAirlines.Items.Add(listItem);
                }
            }
            ddlAirlines.Items.Insert(0, new ListItem("全部", ""));
        }

        private void BindCity(Guid company, Airport city)
        {
            IEnumerable<Service.Foundation.Domain.Airport> allAirports = FoundationService.Airports;
            List<Service.Foundation.Domain.Airport> result = allAirports.Where(item => item.Valid).ToList();
            city.InitData(result);
        }
    }
}