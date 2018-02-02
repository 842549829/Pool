using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Report;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class SpreadTicketReport : BasePage
    {
        public string totalTradeAmount = "￥0.00";
        public string totalAmount = "￥0.00";
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
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
            querySpreadTicket(pagination);
        }

        private void initData()
        {
            if (DateTime.Now.CompareTo(DateTime.Parse("18:00")) > 0)
            {
                this.txtStartDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                this.txtStartDate.Text = DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            var ticketStatus = Enum.GetValues(typeof(TicketState)) as TicketState[];
            foreach (var item in ticketStatus)
            {
                if (item != TicketState.Change)
                    this.ddlTicketState.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));

            }
            this.ddlTicketState.Items.Insert(0, new ListItem("全部", ""));
            var companyType = Enum.GetValues(typeof(CompanyType)) as CompanyType[];
            foreach (var item in companyType)
            {
                if (item != CompanyType.Platform)
                    this.ddlCompanyType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlCompanyType.Items.Insert(0, new ListItem("全部", ""));
            CompanyQueryParameter parameter = new CompanyQueryParameter();
            //var companies = CompanyService.GetCompanies(CompanyType.Purchaser | CompanyType.Supplier | CompanyType.Provider, true);
            this.SpreadCompany.SetCompanyType(CompanyType.Purchaser | CompanyType.Supplier | CompanyType.Provider, true);
            this.spreadInfo.Visible = true;
            if (this.CurrentCompany.CompanyType != CompanyType.Platform)
            {
                this.spreadInfo.Visible = false;
                this.hfdCompanyType.Value = "roles";
                this.hfdSpreadCompanyId.Value = this.CurrentCompany.CompanyId.ToString();
            }
            if (this.CurrentCompany.CompanyType == CompanyType.Platform)
            {
                this.BargainCompany.SetCompanyType(CompanyType.Purchaser | CompanyType.Supplier | CompanyType.Provider, true);
            }
            else
            {
                var tradeCompanies = CompanyService.GetSpreadingList(new SpreadingQueryParameter
                {
                    Initiator = this.CurrentCompany.CompanyId
                }, new Pagination
                {
                    PageIndex = 1,
                    PageSize = int.MaxValue
                });
                //var companiesInfo = from item in tradeCompanies
                //                    select new CompanyInitInfo
                //                    {
                //                        AbbreviateName = item.AbbreviateName,
                //                        CompanyId = item.Id,
                //                        CompanyType = item.Type,
                //                        UserNo = item.Admin
                //                    };
                this.BargainCompany.SetCompanyType(CompanyType.Purchaser | CompanyType.Supplier | CompanyType.Provider, true);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination()
                {
                    PageIndex = 1,
                    PageSize = pager.PageSize,
                    GetRowCount = true
                };
                querySpreadTicket(pagination);
            }
            else
            {
                this.pager.CurrentPageIndex = 1;
            }
        }

        private void querySpreadTicket(Pagination pagiantion)
        {
            try
            {
                decimal tradeAmounts, amounts;
                var list = ReportService.QuerySpreadTicket(pagiantion, getCondition(), out  tradeAmounts, out  amounts);
                var tradeAmount = list.Compute("Sum(TradeAmount)", "");
                var amount = list.Compute("Sum(Amount)", "");
                if (tradeAmount != DBNull.Value)
                {
                    this.totalTradeAmount = "￥" + tradeAmount.ToString();
                }
                if (amount != DBNull.Value)
                {
                    this.totalAmount = "￥" + amount.ToString();
                }
                this.dataList.DataSource = list;
                this.dataList.DataBind();
                if (list.Rows.Count > 0)
                {
                    counts.Visible = true;
                    this.pager.Visible = true;
                    this.dataList.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if (pagiantion.GetRowCount)
                    {
                        this.pager.RowCount = pagiantion.RowCount;
                    }
                    lblTradeAmount.Text = "￥" + tradeAmounts + "元";
                    lblAmount.Text = "￥" + amounts + "元";
                }
                else
                {
                    counts.Visible = false;
                    this.pager.Visible = false;
                    this.dataList.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private SpreadTicketView getCondition()
        {
            var view = new SpreadTicketView();
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
                view.BeginFinishTime = DateTime.Parse(this.txtStartDate.Text);
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
                view.EndFinishTime = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            if (!string.IsNullOrWhiteSpace(this.ddlCompanyType.SelectedValue))
                view.BargainType = (CompanyType)int.Parse(this.ddlCompanyType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(this.ddlTicketState.SelectedValue))
                view.TicketState = (TicketState)int.Parse(this.ddlTicketState.SelectedValue);
            if (this.CurrentCompany.CompanyType == CompanyType.Platform)
            {
                if (this.SpreadCompany.CompanyId.HasValue)
                {
                    view.Spreader = this.SpreadCompany.CompanyId;
                }
            }
            else
            {
                view.Spreader = Guid.Parse(this.hfdSpreadCompanyId.Value);
            }
            if (this.BargainCompany.CompanyId.HasValue)
                view.Bargainer = this.BargainCompany.CompanyId;
            return view;
        }
    }
}