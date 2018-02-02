using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.Report;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class SupplyTicketReport : BasePage
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
            querySupplyTicket(pagination);
        }

        private void querySupplyTicket(Pagination pagination)
        {
            try
            {
                decimal totalTradeAmounts;
                var list = ReportService.QuerySupplyTicket(pagination, getCondition(), out  totalTradeAmounts);
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
                    this.dataList.Visible = true;
                    this.pager.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    lblTradeAmount.Text = "￥" + totalTradeAmounts + "元";
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

        private DataTransferObject.Report.SupplyTicketView getCondition()
        {
            var view = new DataTransferObject.Report.SupplyTicketView();
            view.CompanyId = this.CurrentCompany.CompanyId;
            if (!string.IsNullOrWhiteSpace(this.txtFinishStartDate.Text))
            {
                view.FinishBeginTime = DateTime.Parse(this.txtFinishStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtFinishEndDate.Text))
            {
                view.FinishEndTime = DateTime.Parse(this.txtFinishEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.txtOrderId.Text))
            {
                view.OrderId = decimal.Parse(this.txtOrderId.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPNR.Text))
            {
                view.PNR = this.txtPNR.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                view.Ariline = this.ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlTicketStatus.SelectedValue))
            {
                view.TicketState = (TicketState)int.Parse(this.ddlTicketStatus.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlSpecialType.SelectedValue))
                view.SpecialProductType = (SpecialProductType)int.Parse(this.ddlSpecialType.SelectedValue);
            return view;
        }

        private void initData()
        {
            if (DateTime.Now.CompareTo(DateTime.Parse("18:00")) > 0)
            {
                this.txtFinishStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtFinishEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                this.txtFinishStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.txtFinishEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            BindAriline(this.CurrentCompany.CompanyId);
            this.hfdSupplyCompanyId.Value = this.CurrentCompany.CompanyId.ToString();
            //机票状态
            var ticketStatus = Enum.GetValues(typeof(TicketState)) as TicketState[];
            foreach (var item in ticketStatus)
            {
                this.ddlTicketStatus.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlTicketStatus.Items.Insert(0, new ListItem("全部", ""));
            var companyParameter = CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId);
            //特殊票类型
            if (companyParameter.Singleness)
                this.ddlSpecialType.Items.Add(new ListItem("单程控位产品", ((int)SpecialProductType.Singleness).ToString()));
            if (companyParameter.Disperse)
                this.ddlSpecialType.Items.Add(new ListItem("散冲团产品", ((int)SpecialProductType.Disperse).ToString()));
            if (companyParameter.CostFree)
                this.ddlSpecialType.Items.Add(new ListItem("免票产品", ((int)SpecialProductType.CostFree).ToString()));
            if (companyParameter.Bloc)
                this.ddlSpecialType.Items.Add(new ListItem("集团票产品", ((int)SpecialProductType.Bloc).ToString()));
            if (companyParameter.Business)
                this.ddlSpecialType.Items.Add(new ListItem("商旅卡产品", ((int)SpecialProductType.Business).ToString()));
            this.ddlSpecialType.Items.Insert(0, new ListItem("全部", ""));
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
                    querySupplyTicket(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        private bool valiate()
        {
            if (this.txtOrderId.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtOrderId.Text.Trim(), @"^\d{1,13}$"))
            {
                ShowMessage("订单号格式错误！");
                return false;
            }
            if (this.txtPNR.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtPNR.Text.Trim(), @"^\w{6}$"))
            {
                ShowMessage("PNR编码格式错误！");
                return false;
            }
            return true;
        }

        private void BindAriline(Guid company)
        {
            var airlines = PolicySetService.QueryAirlines(company);
            var allAirlines = FoundationService.Airlines;
            foreach (Airline item in allAirlines)
            {
                if (item.Valid && airlines.Contains(item.Code.Value))
                {
                    ListItem listItem = new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value);
                    this.ddlAirlines.Items.Add(listItem);
                }
            }
            this.ddlAirlines.Items.Insert(0, new ListItem("全部", ""));
        }

    }
}