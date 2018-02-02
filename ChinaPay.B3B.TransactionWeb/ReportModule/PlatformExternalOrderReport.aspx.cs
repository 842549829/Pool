using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.Report;
using ChinaPay.B3B.DataTransferObject.Report;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class PlatformExternalOrderReport : BasePage
    {
        public string totalReceiveAmount = "";
        public string totalPaymentAmount = "";
        public string totalProfitAmount = "";
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
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = newPage
            };
            queryPlatformExternalOrder(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                if (this.pager.CurrentPageIndex == 1)
                {
                    var pagination = new Pagination()
                    {
                        PageIndex = 1,
                        GetRowCount = true,
                        PageSize = pager.PageSize
                    };
                    queryPlatformExternalOrder(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        private void queryPlatformExternalOrder(Pagination pagination)
        {
            try
            {
                decimal totalReceiveAmountCount; decimal totalPaymentAmountCount; decimal totalProfitAmountCount;
                var list = ReportService.QueryPlatformExternalOrder(pagination, getCondition(), out  totalReceiveAmountCount, out  totalPaymentAmountCount, out  totalProfitAmountCount);
                var receivingAmount = list.Compute("Sum(ReceivingAmount)", "");
                if (receivingAmount != DBNull.Value)
                {
                    totalReceiveAmount = "￥" + receivingAmount.ToString();
                }
                var payAmount = list.Compute("Sum(PayAmount)", "");
                if (payAmount != DBNull.Value)
                {
                    totalPaymentAmount = "￥" + payAmount.ToString();
                }
                var profit = list.Compute("Sum(Profit)", "");
                if (profit != DBNull.Value)
                {
                    totalProfitAmount = "￥" + profit.ToString();
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
                }
                else
                {
                    counts.Visible = false;
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
                lblReceiveAmount.Text = "￥" + totalReceiveAmountCount + "元";
                lblPaymentAmount.Text = "￥" + totalPaymentAmountCount + "元";
                lblProfitAmount.Text = "￥" + totalProfitAmountCount + "元";
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private PlatformExternalOrderView getCondition()
        {
            var view = new PlatformExternalOrderView();
            if (!string.IsNullOrWhiteSpace(this.txtPayStartDate.Text))
            {
                view.BeginPayTime = DateTime.Parse(this.txtPayStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPayEndDate.Text))
            {
                view.EndPayTime = DateTime.Parse(this.txtPayEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.txtInternalOrderId.Text))
            {
                view.OrderId = decimal.Parse(this.txtInternalOrderId.Text.Trim());
            }
            if (!string.IsNullOrWhiteSpace(this.txtExternalOrderId.Text))
            {
                view.ExternalOrderId = this.txtExternalOrderId.Text.Trim();
            }
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                view.Airline = this.ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.txtDeparture.Code))
            {
                view.Departure = this.txtDeparture.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPayStatus.SelectedValue))
            {
                view.Payed = this.ddlPayStatus.SelectedValue=="1";
            }
            if (!string.IsNullOrWhiteSpace(this.txtPnr.Text))
            {
                view.PNR = this.txtPnr.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.txtArrival.Code))
            {
                view.Arrival = this.txtArrival.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlOrderSource.SelectedValue))
            {
                view.OrderSource = (PlatformType)int.Parse(this.ddlOrderSource.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPrintStatus.SelectedValue))
            {
                view.ETDZStatus = short.Parse(this.ddlPrintStatus.SelectedValue);
            }
            return view;
        }

        private void initData()
        {
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
            this.txtPayStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.txtPayEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var platformType = Enum.GetValues(typeof(PlatformType)) as PlatformType[];
            foreach (var item in platformType)
            {
                if (item != PlatformType.B3B)
                    this.ddlOrderSource.Items.Add(new ListItem(item.GetDescription(), ((byte)item).ToString()));
            }
            this.ddlOrderSource.Items.Insert(0, new ListItem("全部", ""));
        }

        private bool valiate()
        {
            if (this.txtPnr.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtPnr.Text.Trim(), @"^\w{6}$"))
            {
                ShowMessage("PNR编码格式错误！");
                return false;
            }
            if (this.txtExternalOrderId.Text.Trim().Length > 20)
            {
                ShowMessage("外部订单号格式错误！");
                return false;
            }
            if (this.txtInternalOrderId.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtInternalOrderId.Text.Trim(), @"^\d{1,13}$"))
            {
                ShowMessage("内部订单号格式错误！");
                return false;
            }
           
            return true;
        }
    }
}