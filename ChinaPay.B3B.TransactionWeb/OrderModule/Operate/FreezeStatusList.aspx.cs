using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.Utility;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate
{
    public partial class FreezeStatusList : BasePage
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
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void initData()
        {
            Array freezeTypes = Enum.GetValues(typeof (FreezeType));
            foreach (FreezeType freezeType in freezeTypes)
            {
                ddlFreezeType.Items.Add(new ListItem(freezeType.GetDescription(), ((byte) freezeType).ToString()));
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
                queryRecords(pagination);
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
            queryRecords(pagination);
        }

        private void queryRecords(Pagination pagination)
        {
            try
            {
                IEnumerable<FreezeBaseInfo> orders = FreezeService.Query(getCondition(), pagination);
                dataList.DataSource = orders;
                dataList.DataBind();
                if (orders.Any())
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
                ShowExceptionMessage(ex, "查询账户状态");
            }
        }

        private FreezeQueryCondition getCondition()
        {
            var parmater = new FreezeQueryCondition();
            parmater.OrderId = StringUtility.ToNullableDecimal(txtOrderId.Text.Trim());
            parmater.ApplyformId = StringUtility.ToNullableDecimal(txtApplyFormId.Text.Trim());
            parmater.RequestDate = new Range<DateTime>(DateTime.Parse(txtStartDate.Text.Trim()), DateTime.Parse(txtEndDate.Text.Trim())); 
                //DateTime.Parse(txtEndDate.Text.Trim()).AddDays(1).AddTicks(-1));

            if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
            {
                parmater.Success = ddlStatus.SelectedValue == "1";
            }
            if (!string.IsNullOrWhiteSpace(ddlFreezeType.SelectedValue))
            {
                parmater.Type = (FreezeType) int.Parse(ddlFreezeType.SelectedValue);
            }
            return parmater;
        }

        protected void dataList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                decimal applyFormId = e.CommandArgument.ToString().ToDecimal();
                FreezeService.UnFreeze(applyFormId);
                ShowMessage("解冻成功！");
                var pagination = new Pagination
                    {
                        PageSize = pager.PageSize,
                        PageIndex = pager.CurrentPageIndex,
                        GetRowCount = true
                    };
                queryRecords(pagination);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "解冻账户");
            }
        }
    }
}