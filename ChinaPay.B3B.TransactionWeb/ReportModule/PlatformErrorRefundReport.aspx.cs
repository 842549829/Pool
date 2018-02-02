﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Report;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class PlatformErrorRefundReport : BasePage
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
                var dataSource = ReportService.QueryPlatformErrorRefund(pagination, getCondition());
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
            if (this.txtPurchase.CompanyId.HasValue)
                condition.Purchase = this.txtPurchase.CompanyId;
            if (this.txtProvider.CompanyId.HasValue)
                condition.Provider = this.txtProvider.CompanyId;
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
            if (this.txtTicketNo.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtTicketNo.Text.Trim(), @"^\d{10}$"))
            {
                ShowMessage("票号格式错误！");
                return false;
            }
            return true;
        }

        private void initData()
        {
            this.txtPurchase.SetCompanyType(Common.Enums.CompanyType.Purchaser | Common.Enums.CompanyType.Supplier | Common.Enums.CompanyType.Provider);
            this.txtProvider.SetCompanyType(Common.Enums.CompanyType.Provider);
            this.txtApplyStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.txtApplyEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        }
    }
}