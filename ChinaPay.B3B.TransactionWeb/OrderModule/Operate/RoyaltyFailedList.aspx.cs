using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class RoyaltyFailedList : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            btnQuery_Click(this, e);
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                    {
                        PageSize = pager.PageSize,
                        PageIndex = 1,
                        GetRowCount = true
                    };
                queryOrders(pagination);
            }
            else
            {
                pager.CurrentPageIndex = 1;
            }
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = newPage,
                    GetRowCount = true
                };
            queryOrders(pagination);
        }

        private void queryOrders(Pagination pagination)
        {
            try
            {
                IEnumerable<RoyaltyFailedRecord> orders = OrderQueryService.QueryRoyaltyFailedRecords(getCondition(), pagination);
                dataList.DataSource = orders;
                dataList.DataBind();
                if (pagination.RowCount > 0)
                {
                    pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "分润失败订单查询");
            }
        }

        private RoyaltyFailedRecordQueryCondition getCondition()
        {
            var condition = new RoyaltyFailedRecordQueryCondition
                {
                    ETDZDateRange =
                        new Range<DateTime>(DateTime.Parse(txtStartDate.Text),DateTime.Parse(txtEndDate.Text.Trim()))
                };
            if(!string.IsNullOrEmpty(txtOrderId.Text.Trim()))
            {
                condition.OrderId = decimal.Parse(txtOrderId.Text.Trim());
            }
            return condition;
        }
    }
}