using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Report;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class PlatformTicketReport : BasePage
    {
        public string totalPurchaserAmount = "";
        public string totalProviderAmount = "";
        public string totalSupplierAmount = "";
        public string totalRoyaltyAmount = "";
        public string totalPostponeFee = "";
        public string totalPlatformCommission = "";
        public string totalPlatformPremium = "";
        public string totalPlatformProfit = "";
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
            queryPlatformTicket(pagination);
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
                    queryPlatformTicket(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        private void queryPlatformTicket(Pagination pagination)
        {
            try
            {
                decimal totalPurchaserAmountCount; decimal totalProviderAmountCount; decimal totalSupplierAmountCount; decimal totalRoyaltyAmountCount; decimal totalPostponeFeeCount; decimal totalPlatformCommissionCount; decimal totalPlatformPremiumCount; decimal totalPlatformProfitCount;
                var list = ReportService.QueryPlatformTicket(pagination, getCondition(), out  totalPurchaserAmountCount, out  totalProviderAmountCount, out  totalSupplierAmountCount, out totalRoyaltyAmountCount,out  totalPostponeFeeCount, out  totalPlatformCommissionCount, out totalPlatformPremiumCount,out  totalPlatformProfitCount);

                var purchaserAmount = list.Compute("Sum(PurchaserAmount)", "");
                if (purchaserAmount != DBNull.Value)
                {
                    totalPurchaserAmount = "￥" + purchaserAmount.ToString();
                }
                var providerAmount = list.Compute("Sum(ProviderAmount)", "");
                if (providerAmount != DBNull.Value)
                {
                    totalProviderAmount = "￥" + providerAmount.ToString();
                }
                var supperlierAmount = list.Compute("Sum(SupplierAmount)", "");
                if (supperlierAmount != DBNull.Value)
                {
                    totalSupplierAmount = "￥" + supperlierAmount.ToString();
                }
                var royaltyAmount = list.Compute("Sum(RoyaltyAmount)", "");
                if (royaltyAmount != DBNull.Value)
                {
                    totalRoyaltyAmount = "￥" + royaltyAmount.ToString();
                }
                var postponefee = list.Compute("Sum(PostponeFee)", "");
                if (postponefee != DBNull.Value)
                {
                    totalPostponeFee = "￥" + postponefee.ToString();
                }
                var platformCommission = list.Compute("Sum(PlatformCommission)", "");
                if (platformCommission != DBNull.Value)
                {
                    totalPlatformCommission = "￥" + platformCommission.ToString();
                }
                var platformPremium = list.Compute("Sum(Premium)", "");
                if (platformPremium != DBNull.Value)
                {
                    totalPlatformPremium = "￥" + platformPremium.ToString();
                }
                var platformProfit = list.Compute("Sum(PlatformProfit)", "");
                if (platformProfit != DBNull.Value)
                {
                    totalPlatformProfit = "￥" + platformProfit.ToString();
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
                lblPlatformCommission.Text = "￥" + totalPlatformCommissionCount + "元";
                lblPlatformProfit.Text = "￥" + totalPlatformProfitCount + "元";
                lblPlatformPremium.Text = "￥" + totalPlatformPremiumCount + "元";
                lblPostponeFee.Text = "￥" + totalPostponeFeeCount + "元";
                lblProviderAmount.Text = "￥" + totalProviderAmountCount + "元";
                lblPurchaserAmount.Text = "￥" + totalPurchaserAmountCount + "元";
                lblSupplierAmount.Text = "￥" + totalSupplierAmountCount + "元";
                lblRoyaltyAmount.Text = "￥" + totalRoyaltyAmountCount + "元";
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private PlatformTicketView getCondition()
        {
            var view = new PlatformTicketView();
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                view.FinishBeginTime = DateTime.Parse(this.txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                view.FinishEndTime = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                view.Airline = this.ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlTicketStatus.SelectedValue))
            {
                view.TicketState = (TicketState)int.Parse(this.ddlTicketStatus.SelectedValue);
            }
            if (this.txtProviderCompany.CompanyId.HasValue)
            {
                view.Provider = this.txtProviderCompany.CompanyId;
            }
            if (this.txtProductCompany.CompanyId.HasValue)
            {
                view.Supplier = this.txtProductCompany.CompanyId;
            }
            if (this.txtPurchaseCompany.CompanyId.HasValue)
            {
                view.Purchaser = this.txtPurchaseCompany.CompanyId;
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
            if (!string.IsNullOrWhiteSpace(this.ddlRelationType.SelectedValue))
            {
                view.RelationType = (RelationType)int.Parse(this.ddlRelationType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtOrderId.Text))
            {
                view.OrderId = decimal.Parse(this.txtOrderId.Text.Trim());
            }
            if (!string.IsNullOrWhiteSpace(this.txtTakeOffLowerTime.Text))
            {
                view.TakeoffBeginDate = DateTime.Parse(this.txtTakeOffLowerTime.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtTakeOffUpperTime.Text))
            {
                view.TakeoffEndDate = DateTime.Parse(this.txtTakeOffUpperTime.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPayType.SelectedValue))
            {
                view.PayType = this.ddlPayType.SelectedValue == "1";
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
            this.txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var ticketStatus = Enum.GetValues(typeof(TicketState)) as TicketState[];
            foreach (var item in ticketStatus)
            {
                this.ddlTicketStatus.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlTicketStatus.Items.Insert(0, new ListItem("全部", ""));
            var relationType = Enum.GetValues(typeof(RelationType)) as RelationType[];

            foreach (var item in relationType)
            {
                this.ddlRelationType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlRelationType.Items.Insert(0, new ListItem("全部", ""));
            //var companies = CompanyService.GetCompanies(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
            //txtProviderCompany.InitCompanies(companies.Where(item => item.CompanyType == CompanyType.Provider));
            //txtPurchaseCompany.InitCompanies(companies);
            //txtProductCompany.InitCompanies(companies.Where(item => item.CompanyType == CompanyType.Supplier));
            txtProviderCompany.SetCompanyType(CompanyType.Provider);
            txtPurchaseCompany.SetCompanyType(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
            txtProductCompany.SetCompanyType(CompanyType.Supplier);
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