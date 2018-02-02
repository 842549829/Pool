using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOemAuthorizationList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                LoadCondition("CompanyAuthorizationList");
                txtBeginTime.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                btnQuery_Click(this, e);
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            Pagination pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryDistributionOEM(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                GetRowCount = true
            };
            queryDistributionOEM(pagination);
        }

        private void queryDistributionOEM(Pagination pagination)
        {
            try
            {
                var distributionOEMList = DistributionOEMService.QueryDistributionOEMList(getCondition(), pagination);
                this.datalist.DataSource = distributionOEMList;
                datalist.DataBind();
                if (distributionOEMList.Rows.Count > 0)
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

        private DataTransferObject.Organization.DistributionOEMCondition getCondition()
        {
            var condition = new DataTransferObject.Organization.DistributionOEMCondition();
            if (!string.IsNullOrWhiteSpace(this.txtBeginTime.Text))
                condition.RegisterBeginTime = DateTime.Parse(this.txtBeginTime.Text);
            if (!string.IsNullOrWhiteSpace(this.txtEndTime.Text))
                condition.RegisterEndTime = DateTime.Parse(this.txtEndTime.Text).AddDays(1).AddTicks(-1);
            if (!string.IsNullOrWhiteSpace(this.txtAccount.Text))
                condition.UserNo = this.txtAccount.Text;
            if (!string.IsNullOrWhiteSpace(this.txtDomainName.Text))
                condition.DomainName = this.txtDomainName.Text;
            if (!string.IsNullOrWhiteSpace(this.txtOemName.Text))
                condition.SiteName = this.txtOemName.Text;
            if (!string.IsNullOrWhiteSpace(this.txtAbbreviateName.Text))
                condition.AbbreviateName = this.txtAbbreviateName.Text;
            if (!string.IsNullOrWhiteSpace(this.ddlAuthorizationStatus.SelectedValue))
                condition.AutorizationStatus = this.ddlAuthorizationStatus.SelectedValue == "1";
            return condition;
        }
    }
}