using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.PaymentsAccount {
    public partial class AccountList : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
            }
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage) {
            var pagination = new Pagination() {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryAccounts(pagination);
        }


        protected void btnQuery_Click(object sender, EventArgs e) {
            if(this.pager.CurrentPageIndex == 1) {
                var pagination = new Pagination() {
                    PageSize = pager.PageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                queryAccounts(pagination);
            } else {
                this.pager.CurrentPageIndex = 1;
            }
        }

        private void queryAccounts(Pagination pagination) {
            try {
                var accounts = AccountService.GetAccount(GetCondition(pagination), pagination);
                this.dataList.DataSource = accounts.Select(item => new
                {
                    CompanyType = item.CompanyType.GetDescription(),
                    item.AbbreviateName,
                    item.Administrator,
                    AccountType = item.AccountType.GetDescription(),
                    item.Account,
                    Enabled = item.Enabled ? "有效" : "无效",
                    item.CreateTime,
                    item.CompanyId,
                    CanAudit = item.CompanyType != CompanyType.Platform && !string.IsNullOrEmpty(item.Account)
                });
                this.dataList.DataBind();
                if(accounts.Any()) {
                    this.pager.Visible = true;
                    if(pagination.GetRowCount) {
                        this.pager.RowCount = pagination.RowCount;
                    }
                } else {
                    this.pager.Visible = false;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private PaymentsAccountQueryCondition GetCondition(Pagination pagination) {
            var condition = new PaymentsAccountQueryCondition() {
                AbbreviateName = txtAbbreviateName.Text.Trim(),
                PaymentAccount = txtPaymentAccount.Text.Trim(),
                UserName = txtAdministrator.Text.Trim()
            };
            if(!string.IsNullOrEmpty(ddlEnabled.SelectedValue)) {
                condition.Enabled = ddlEnabled.SelectedValue == "1";
            }
            return condition;
        }
    }
}