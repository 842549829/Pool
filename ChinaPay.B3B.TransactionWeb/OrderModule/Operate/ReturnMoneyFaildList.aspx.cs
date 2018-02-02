using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class ReturnMoneyFaildList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
             btnQuery_Click(this, e);
           }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void initData()
        {
            Array businesstype = Enum.GetValues(typeof (RefundBusinessType));
            foreach (RefundBusinessType item in businesstype)
            {
                ddlBusinessType.Items.Add(new ListItem(item.GetDescription(), ((int) item).ToString()));
            }
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
                IEnumerable<RefundFailedRecord> orders = OrderQueryService.QueryRefundFailedRecords(getCondition(), pagination);
                dataList.DataSource = orders.Select(item => new
                    {
                        item.OrderId,
                        item.ApplyformId,
                        BusinessType = item.BusinessType.GetDescription(),
                        item.RefundTime,
                        item.RefundFailedInfo
                    });
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
                ShowExceptionMessage(ex, "查询");
            }
        }

        private RefundFailedRecordQueryCondition getCondition()
        {
            var condition = new RefundFailedRecordQueryCondition
                {
                    RefundDateRange =
                        new Range<DateTime>(DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text))
                };
            if (!string.IsNullOrWhiteSpace(ddlBusinessType.SelectedValue))
            {
                condition.BusinessType = (RefundBusinessType) byte.Parse(ddlBusinessType.SelectedValue);
            }
            if (!string.IsNullOrEmpty(txtApplyFormId.Text.Trim()))
            {
                condition.ApplyformId = decimal.Parse(txtApplyFormId.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtOrderId.Text.Trim()))
            {
                condition.ApplyformId = decimal.Parse(txtOrderId.Text.Trim());
            }

            return condition;
        }
    }
}