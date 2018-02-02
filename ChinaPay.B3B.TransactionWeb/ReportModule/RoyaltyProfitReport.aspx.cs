using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.Report;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class RoyaltyProfitReport : BasePage
    {
        public string totalTradeFee = "";
        public string totalTradeRoyalty = "";
        public string totalAmount = "";
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
            queryRoyaltyProfit(pagination);
        }

        private void queryRoyaltyProfit(Pagination pagination)
        {
            try
            {
                decimal totalTradeFeeCount; decimal totalTradeRoyaltyCount; decimal totalAmountCount;
                var list = ReportService.QueryRoyaltyProfit(pagination, getCondition(), out totalTradeFeeCount, out totalTradeRoyaltyCount, out totalAmountCount);
                var tradeFee = list.Compute("Sum(TradeFee)", "");
                if (tradeFee != DBNull.Value)
                {
                    totalTradeFee = "￥" + tradeFee.ToString();
                }
                var tradeRoyalty = list.Compute("Sum(Commission)", "");
                if (tradeRoyalty != DBNull.Value)
                {
                    totalTradeRoyalty = "￥" + tradeRoyalty.ToString();
                }
                var tradeAmount = list.Compute("Sum(Anticipation)", "");
                if (tradeAmount != DBNull.Value)
                {
                    totalAmount = "￥" + tradeAmount.ToString();
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
                    lblTradeFee.Text = "￥" + totalTradeFeeCount + "元";
                    lblTradeRoyalty.Text = "￥" + totalTradeRoyaltyCount + "元";
                    lblTradeAmount.Text = "￥" + totalAmountCount + "元";
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

        private DataTransferObject.Report.RoyaltyProfitCondition getCondition()
        {
            var view = new DataTransferObject.Report.RoyaltyProfitCondition();
            
            if (this.CurrentCompany.CompanyType != CompanyType.Platform)
            {
                view.Royalty = this.CurrentCompany.CompanyId;
                if (!string.IsNullOrWhiteSpace(this.ddlIncomeGroup.SelectedValue))
                {
                    view.IncomeGroupId = Guid.Parse(this.ddlIncomeGroup.SelectedValue);
                }
                if (!string.IsNullOrWhiteSpace(this.ddlPurchaseCompany.SelectedValue))
                    view.PurchaseId = Guid.Parse(this.ddlPurchaseCompany.SelectedValue);
            }
            else
            {
                if (this.txtPurchaseCompany.CompanyId.HasValue)
                    view.PurchaseId = this.txtPurchaseCompany.CompanyId;
                if (!string.IsNullOrWhiteSpace(this.ddlRoyaltyCompany.SelectedValue))
                    view.Royalty = Guid.Parse(this.ddlRoyaltyCompany.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                view.ETDZStartDate = DateTime.Parse(this.txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                view.ETDZEndDate = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.txtTicketNo.Text))
            {
                view.TicketNo = this.txtTicketNo.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlType.SelectedValue))
            {
                view.PaymentType = (RoyaltyReportType)(int.Parse(this.ddlType.SelectedValue));
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPayStatus.SelectedValue))
            {
                view.IsSuccess = this.ddlPayStatus.SelectedValue == "1";
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPayType.SelectedValue))
            {
                view.IsPoolPay = this.ddlPayType.SelectedValue == "1";
            }
            if (!string.IsNullOrWhiteSpace(this.txtOrderId.Text))
            {
                view.OrderId = decimal.Parse(this.txtOrderId.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPnr.Text))
            {
                view.PNR = this.txtPnr.Text;
            }

            return view;
        }

        private void initData()
        {
            this.incomeGroup.Visible = false;
            this.hfdCompanyType.Value = this.CurrentCompany.CompanyType.ToString();
            txtPurchaseCompany.SetCompanyType(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
            var purchase = DistributionOEMService.QueryDistributionOEMUserList(new DistributionOEMUserCondition
            {
                CompanyId = this.CurrentCompany.CompanyId
            }, new Pagination
            {
                 PageIndex = 1,
                  PageSize = int.MaxValue
            });
            this.ddlPurchaseCompany.DataSource = from item in purchase
                                                 select new
                                                 {
                                                     Text = item.Login + "-" + item.AbbreviateName,
                                                     Value = item.CompanyId
                                                 };
            this.ddlPurchaseCompany.DataTextField = "Text";
            this.ddlPurchaseCompany.DataValueField = "Value";
            this.ddlPurchaseCompany.DataBind();
            this.ddlPurchaseCompany.Items.Insert(0, new ListItem("全部", ""));
            this.hfdCompanyId.Value = this.CurrentCompany.CompanyId.ToString();
            this.txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            if (this.CurrentCompany.CompanyType == CompanyType.Platform)
            {
                this.royalty.Visible = true;
                this.txtPurchaseCompany.Visible = true;
                this.ddlPurchaseCompany.Visible = false;
                var royaltyCompany = DistributionOEMService.QueryDistributionOEM();
                this.ddlRoyaltyCompany.DataSource = from item in royaltyCompany
                                                    select new
                                                    {
                                                        Text = item.UserNo + "-" + item.AbbreivateName,
                                                        Value = item.CompanyId
                                                    };
                this.ddlRoyaltyCompany.DataTextField = "Text";
                this.ddlRoyaltyCompany.DataValueField = "Value";
                this.ddlRoyaltyCompany.DataBind();
                this.ddlRoyaltyCompany.Items.Insert(0,new ListItem("全部",""));
            }
            else
            {
                this.incomeGroup.Visible = true;
                this.txtPurchaseCompany.Visible = false;
                this.ddlPurchaseCompany.Visible = true;
                var incomeGroup = IncomeGroupService.QueryIncomeGroup(this.CurrentCompany.CompanyId, null);
                this.ddlIncomeGroup.DataSource = incomeGroup;
                this.ddlIncomeGroup.DataTextField = "Name";
                this.ddlIncomeGroup.DataValueField = "Id";
                this.ddlIncomeGroup.DataBind();
                this.ddlIncomeGroup.Items.Insert(0, new ListItem("全部", ""));
                this.royalty.Visible = false;
            }
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
                    queryRoyaltyProfit(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        private bool valiate()
        {
            if (this.txtPnr.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtPnr.Text.Trim(), @"^\w{6}$"))
            {
                ShowMessage("PNR编码格式错误！");
                return false;
            }
            if (this.txtOrderId.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtOrderId.Text.Trim(), @"^\d{1,13}$"))
            {
                ShowMessage("订单号格式错误！");
                return false;
            }
            if (this.txtTicketNo.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtTicketNo.Text.Trim(), @"^\d{10}$"))
            {
                ShowMessage("票号格式错误！");
                return false;
            }
            return true;
        }
    }
}