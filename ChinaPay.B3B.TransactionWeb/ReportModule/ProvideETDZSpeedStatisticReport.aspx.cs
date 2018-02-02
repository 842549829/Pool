using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Report;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class ProvideETDZSpeedStatisticReport : BasePage
    {

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
                DataTable statInfos = ReportService.QueryProviderETDZSpeedStatistics(getCondition(),  
pagination);
                statInfos.Columns.Remove("rowNum");
                var Carriers = FoundationService.Airlines.ToList();
                    foreach (DataRow dr in statInfos.Rows)
                    {
                        if (ByCarriar.Checked)
                        {
                            var carrierCode = dr.Field<string>("carrier");
                            var carrier = Carriers.FirstOrDefault(c => c.Code.Value == carrierCode);
                            if (carrier != null)
                            {
                                dr["carrier"] = carrier.ShortName;
                            }
                         }
                        dr["Speed"] = Math.Round(dr.Field<int>("Speed")/60.0,0);
                    }

                ChangeColumnName(statInfos, "Carrier", "航空公司");
                ChangeColumnName(statInfos, "OrderCount", "订单数");
                ChangeColumnName(statInfos, "Speed", "平均效率(分钟)");
                ChangeColumnName(statInfos, "TicketType", "客票类型");



                dataList.DataSource = statInfos;
                dataList.DataBind();
                if (pagination.RowCount > 0)
                {
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
                    pager.Visible = false;
                    emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private void BindTicketType()
        {
            var relations = from TicketType r in Enum.GetValues(typeof(TicketType))
                            select new
                            {
                                Text = r.GetDescription(),
                                Value = (int)r
                            };
            ddlTicketType.DataSource = relations;
            ddlTicketType.DataTextField = "Text";
            ddlTicketType.DataValueField = "Value";
            ddlTicketType.DataBind();
        }

        private ETDZSpeedStatCondition getCondition()
        {
            var condition = new ETDZSpeedStatCondition();
            if (!string.IsNullOrEmpty(txtStatFrom.Text))
            {
                condition.StartStatTime = DateTime.Parse(txtStatFrom.Text.Trim());
            }
            if (!string.IsNullOrWhiteSpace(txtStatTo.Text))
            {
                condition.EndStatTime = DateTime.Parse(txtStatTo.Text.Trim()).AddDays(1).AddMilliseconds(-1);
            }
            if (!string.IsNullOrWhiteSpace(ddlAirlines.SelectedValue))
            {
                condition.Carrier = ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(ddlTicketType.SelectedValue))
            {
                condition.TicketType = (TicketType) int.Parse(ddlTicketType.SelectedValue);
            }
            condition.Provider = CurrentCompany.CompanyId;
            condition.StatGroup = new SpeedStatGroup
            {
                GroupByCarrier = ByCarriar.Checked,
                GroupByTicketType = ByTicketType.Checked,
            };
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
            txtStatFrom.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
            txtStatTo.Text = DateTime.Today.AddDays(1).AddMinutes(-1).ToString("yyyy-MM-dd");
            BindTicketType();
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
    }
}