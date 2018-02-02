using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Report;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class EmployeeSpreadStatisticReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            this.dataList.Visible = false;
            if (!IsPostBack)
            {
                this.txtStartDate.Text = DateTime.Now.AddDays(-(DateTime.Now.Day - 1)).ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                txtEmpolyeeNo.Value = CurrentUser.UserName;
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
            Query(pagination);
        }
        void Query(Pagination pagination)
        {
            decimal totalPurchaseAmount, totalSupplyAmount, totalProvideAmount;
            int totalPurchaseCount, totalSupplyCount, totalProvideCount;
            var list = ReportService.QueryEmployeeSpreadStatisticReport(pagination, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), selCompanyType.Value == "0" ? null : (CompanyType?)int.Parse(selCompanyType.Value), CurrentUser.UserName, out  totalPurchaseAmount, out  totalSupplyAmount, out  totalProvideAmount, out  totalPurchaseCount, out  totalSupplyCount, out  totalProvideCount);
            dataList.DataSource = list;
            dataList.DataBind();
            if (list.Rows.Count > 0)
            {
                counts.Visible = true;
                pager.Visible = true;
                dataList.Visible = true;
                emptyDataInfo.Visible = false;
                if (pagination.GetRowCount)
                {
                    pager.RowCount = pagination.RowCount;
                }
                lblProvideAmount.Text = totalProvideAmount + "元";
                lblPurchaseAmount.Text = totalPurchaseAmount + "元";
                lblSupplyAmount.Text = totalSupplyAmount + "元";

                lblProvideCount.Text = totalProvideCount + "";
                lblPurchaseCount.Text = totalPurchaseCount + "";
                lblSupplyCount.Text = totalSupplyCount + "";
            }
            else
            {
                counts.Visible = false;
                pager.Visible = false;
                emptyDataInfo.Visible = true;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = 1
            };
            Query(pagination);
        }
    }
}