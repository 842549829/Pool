using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Report;
using ChinaPay.B3B.Service.Organization;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class PurchaseErrorRefundReport : BasePage
    {
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
            queryErrorRefund(pagination);
        }

        private void queryErrorRefund(Pagination pagination)
        {
            try
            {
                var dataSource = ReportService.QueryPurchaseErrorRefund(pagination, getCondition());
                this.dataList.DataSource = dataSource;
                this.dataList.DataBind();
                if (dataSource.Rows.Count > 0)
                {
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
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private DataTransferObject.Report.ErrorRefundQueryCondition getCondition()
        {
            var condition = new DataTransferObject.Report.ErrorRefundQueryCondition();
            if (!string.IsNullOrWhiteSpace(this.txtApplyStartDate.Text))
                condition.ApplyStartTime = DateTime.Parse(this.txtApplyStartDate.Text);
            if (!string.IsNullOrWhiteSpace(this.txtApplyEndDate.Text))
                condition.ApplyEndTime = DateTime.Parse(this.txtApplyEndDate.Text).AddDays(1).AddMilliseconds(-3);
            if (!string.IsNullOrWhiteSpace(this.txtOrderId.Text))
                condition.OrderId = decimal.Parse(this.txtOrderId.Text);
            if (!string.IsNullOrWhiteSpace(this.txtDeparture.Code))
                condition.Departure = this.txtDeparture.Code;
            if (!string.IsNullOrWhiteSpace(this.txtSettleCode.Text))
                condition.SettleCode = this.txtSettleCode.Text;
            if (!string.IsNullOrWhiteSpace(this.txtTicketNo.Text))
                condition.TicketNo = this.txtTicketNo.Text;
            if (!string.IsNullOrWhiteSpace(this.txtApplyformId.Text))
                condition.ApplyformId = decimal.Parse(this.txtApplyformId.Text);
            if (!string.IsNullOrWhiteSpace(this.txtArrivals.Code))
                condition.Arrival = this.txtArrivals.Code;
            if (!string.IsNullOrWhiteSpace(this.txtPassenger.Text))
                condition.Passenger = this.txtPassenger.Text;
            if (!string.IsNullOrWhiteSpace(this.ddlOperator.SelectedValue))
                condition.ApplierAccount = this.ddlOperator.SelectedValue;
            condition.Purchase = this.CurrentCompany.CompanyId;
            return condition;
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
                        PageSize = pager.PageSize,
                        GetRowCount = true
                    };
                    queryErrorRefund(pagination);
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
            return true;
        }

        private void initData()
        {
            this.hfdPurchase.Value = this.CurrentCompany.CompanyId.ToString();
            if (DateTime.Now.CompareTo(DateTime.Parse("18:00")) > 0)
            {
                this.txtApplyStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtApplyEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                this.txtApplyStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.txtApplyEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            if (this.CurrentCompany.CompanyType != Common.Enums.CompanyType.Platform)
            {
                var employees = EmployeeService.QueryEmployees(this.CurrentCompany.CompanyId);
                this.ddlOperator.DataSource = from item in employees
                                              select new
                                              {
                                                  Text = item.UserName + "-" + item.Name,
                                                  Value = item.UserName
                                              };
                this.ddlOperator.DataTextField = "Text";
                this.ddlOperator.DataValueField = "Value";
                this.ddlOperator.DataBind();
                this.ddlOperator.Items.Insert(0, new ListItem("全部", ""));
            }
        }
    }
}