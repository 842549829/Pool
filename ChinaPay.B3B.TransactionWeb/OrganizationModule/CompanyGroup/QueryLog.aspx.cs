using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup
{
    public partial class QueryLog : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                if (pager.CurrentPageIndex == 1)
                {
                    QueryGroupLog(new Pagination
                    {
                        PageIndex = pager.CurrentPageIndex,
                        PageSize = pager.PageSize,
                        GetRowCount = true
                    });
                }
                else
                {
                    pager.CurrentPageIndex = 1;
                }
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }
        protected void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            Pagination pagination = new Pagination()
            {
                GetRowCount = true,
                PageIndex = newPage,
                PageSize = pager.PageSize
            };
            QueryGroupLog(pagination);
        }
        private void QueryGroupLog(Pagination pagination)
        {
            try
            {
                var groupLog = LogService.QueryOperationLogs(
                    new DataTransferObject.Log.OperationLogQueryCondition
                    {
                        Module = OperationModule.公司组,
                        AssociateKey = Request.QueryString["Id"]
                    }, pagination);
                datalist.DataSource = groupLog.Select(item => new
                {
                    item.Time,
                    item.Account,
                    OperationType = item.OperationType.GetDescription(),
                    item.Content
                });
                datalist.DataBind();
                if (groupLog.Any())
                {
                    pager.Visible = true;
                    if (pagination.GetRowCount)
                        pager.RowCount = pagination.RowCount;
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
    }
}