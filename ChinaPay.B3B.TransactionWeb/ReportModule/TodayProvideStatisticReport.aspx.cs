using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Report;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.ExportExcel;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class TodayProvideStatisticReport : BasePage
    {
        public string totalPlatformCommission = "";
        public string totalPlatformProfit = "";
        public string totalPostponeFee = "";
        public string totalProviderAmount = "";
        public string totalPurchaserAmount = "";
        public string totalSupplierAmount = "";

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
            queryStatData(pagination);
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
                queryStatData(pagination);
            }
            else
            {
                pager.CurrentPageIndex = 1;
            }
        }

        private void queryStatData(Pagination pagination)
        {
            try
            {
                int totalTicketCount;
                decimal totalAmount;
                DataTable statInfos = ReportService.QueryTodayProvideStatistics(getCondition(), GetSelectGroupInfo(), pagination, out totalTicketCount, out totalAmount);
                
                ChangeColumnName(statInfos, "Carrier", "承运人");
                ChangeColumnName(statInfos, "AirportPair", "航线");
                ChangeColumnName(statInfos, "FlightNo", "航班号");
                ChangeColumnName(statInfos, "Bunk", "舱位");
                ChangeColumnName(statInfos, "TicketCount", "票量");
                ChangeColumnName(statInfos, "Amount", "交易金额");
                ChangeColumnName(statInfos, "ProviderName", "出票方");
                ChangeColumnName(statInfos, "Relation", "销售关系");



                dataList.DataSource = statInfos;
                dataList.DataBind();
                if (pagination.RowCount > 0)
                {
                    counts.Visible = true;
                    pager.Visible = true;
                    dataList.Visible = true;
                    emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    //counts.Visible = false;
                    pager.Visible = false;
                    emptyDataInfo.Visible = true;
                }
                lblPostponeFee.Text = totalTicketCount + "张";
                lblProviderAmount.Text = "￥" + totalAmount + "元";
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private void BindSaleRelation()
        {
            var relations = from RelationType r in Enum.GetValues(typeof(RelationType))
                            select new
                            {
                                Text = r.GetDescription(),
                                Value = (int)r
                            };
            ddlRelation.DataSource = relations;
            ddlRelation.DataTextField = "Text";
            ddlRelation.DataValueField = "Value";
            ddlRelation.DataBind();
        }

        private GroupInfo GetSelectGroupInfo() {
            var result = new GroupInfo
            {
                Carrier = ByCarriar.Checked,
                Voyage = ByVoyage.Checked,
                FlightNo = ByFlightNo.Checked,
                Bunk = ByBunk.Checked,
                Provider = ByProvider.Checked,
                Relation = ByRelation.Checked
            };
            return result;
        }

        private TodayProvideStatisticQueryCondition getCondition()
        {
            var condition = new TodayProvideStatisticQueryCondition();
            if (!string.IsNullOrEmpty(TimeStart.SelectedValue))
            {
                condition.StartHour = int.Parse(TimeStart.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(TimeEnd.SelectedValue))
            {
                condition.EndHour = int.Parse(TimeEnd.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(ddlAirlines.SelectedValue))
            {
                condition.Carrier = ddlAirlines.SelectedValue;
            }
            if (txtProviderCompany.CompanyId.HasValue)
            {
                condition.Provider = txtProviderCompany.CompanyId;
            }
            if (!string.IsNullOrEmpty(txtDeparture.Code))
            {
                condition.Departure = txtDeparture.Code;
            }
            if (!string.IsNullOrEmpty(txtArrival.Code))
            {
                condition.Arrival = txtArrival.Code;
            }
            if (!string.IsNullOrEmpty(ddlRelation.SelectedValue))
            {
                condition.Relation = (RelationType) int.Parse(ddlRelation.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(ddlProductType.SelectedValue))
            {
                condition.ProductType = (ProductType) int.Parse(ddlProductType.SelectedValue);
                if (ddlProductType.SelectedValue == ((int)ProductType.Special).ToString() && !string.IsNullOrEmpty(ddlSpecialTickType.SelectedValue))
                {
                    condition.SpecialProductType = (SpecialProductType?) int.Parse(ddlSpecialTickType.SelectedValue);
                }
            }
            return condition;
        }

        private void initData()
        {
            ddlAirlines.DataSource = from item in FoundationService.Airlines
                                     select new
                                         {
                                             Name = item.Code + "-" + item.ShortName,
                                             item.Code
                                         };
            ddlAirlines.DataTextField = "Name";
            ddlAirlines.DataValueField = "Code";
            ddlAirlines.DataBind();
            ddlAirlines.Items.Insert(0, new ListItem("全部", ""));

            txtArrival.InitData(FoundationService.Airports);
            txtDeparture.InitData(FoundationService.Airports);

            TimeStart.Text = DateTime.Today.ToString("HH:mm");
            TimeEnd.Text = DateTime.Today.AddDays(1).AddMinutes(-1).ToString("HH:mm");
            //IEnumerable<CompanyInitInfo> companies = CompanyService.GetCompanies(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
            txtProviderCompany.SetCompanyType(CompanyType.Provider,true);

            BindProductType();

            InitHours();
        }

        private void InitHours() {
            for (int i = 0; i < 24; i++)
            {
                TimeStart.Items.Add(i.ToString("00"));
                TimeEnd.Items.Add(i.ToString("00"));
            }
            TimeStart.SelectedIndex = 1;
            TimeEnd.SelectedIndex = 24;

            BindSaleRelation();
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

        private bool ChangeColumnName(DataTable dt, string oName, string nName) {
            var col = dt.Columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName == oName);
            if (col != null)
            {
                col.ColumnName = nName;
                return true;
            }
            return false;
        }

        protected void DownData(object sender, EventArgs e) {
            int totalTicketCount;
            decimal totalAmount;
            DataTable statInfos = ReportService.QueryTodayProvideStatistics(getCondition(), GetSelectGroupInfo(), null, out totalTicketCount, out totalAmount);

            ChangeColumnName(statInfos, "Carrier", "承运人");
            ChangeColumnName(statInfos, "AirportPair", "航线");
            ChangeColumnName(statInfos, "FlightNo", "航班号");
            ChangeColumnName(statInfos, "Bunk", "舱位");
            ChangeColumnName(statInfos, "TicketCount", "票量");
            ChangeColumnName(statInfos, "Amount", "交易金额");
            ChangeColumnName(statInfos, "ProviderName", "出票方");
            ChangeColumnName(statInfos, "Relation", "销售关系");

            DataRow dr = statInfos.NewRow();
            if (totalTicketCount != 0)
            {
                dr["票量"] = totalTicketCount;
            }
            if (totalAmount != 0)
            {
                dr["交易金额"] = totalAmount;
            }
            for (int i = 0; i < statInfos.Columns.Count; i++)
            {
                statInfos.Columns[i].AllowDBNull = true;
            }
            statInfos.Rows.Add(dr);
            NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(statInfos, downloadFileName("当日票量分析报表") );
        }

        private string downloadFileName(string fileName)
        {
            string downloadFileName = fileName;
            string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {//非火狐浏览器
                downloadFileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            }
            return downloadFileName;
        }

    }
}