using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.CustomerModule {
    public partial class CustomerList : BasePage {

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");

            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage) {
            var pagination = new Pagination() {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryCustomerList(pagination);
        }

        void queryCustomerList(Pagination pagination) {
            try {
                var customer = from item in Service.Organization.CustomerService.Query(getCondition(), pagination)
                               select new
                               {
                                   Id = item.Id,
                                   Name = item.Name,
                                   Sex = item.Sex.HasValue ? item.Sex.Value.GetDescription() : string.Empty,
                                   PassengerType = item.PassengerType.GetDescription(),
                                   CredentialsType = item.CredentialsType.ToString(),
                                   Credentials = item.Credentials,
                                   Mobile = item.Mobile,
                               };
                this.dataSource.DataSource = customer;
                this.dataSource.DataBind();
                if(customer.Any()) {
                    this.pager.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if(pagination.GetRowCount) {
                        this.pager.RowCount = pagination.RowCount;
                    }
                } else {
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private CustomerQueryCondition getCondition() {
            var condition = new CustomerQueryCondition() {
                Company = this.CurrentCompany.CompanyId,
                Credentials = this.txtCertId.Text,
                Mobile = this.txtPhone.Text,
                Name = this.txtName.Text
            };
            return condition;
        }

        protected void btnQuery_Click(object sender, EventArgs e) {
            if(this.pager.CurrentPageIndex == 1) {
                var pagination = new Pagination() {
                    PageSize = pager.PageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                queryCustomerList(pagination);
            } else {
                this.pager.CurrentPageIndex = 1;
            }
        }

        protected void dataSource_RowCommand(object sender, GridViewCommandEventArgs e) {
            if(e.CommandName == "del") {
                try {
                    CustomerService.Delete(this.CurrentCompany.CompanyId, Guid.Parse(e.CommandArgument.ToString()), this.CurrentUser.Name);
                    btnQuery_Click(sender, e);
                } catch(Exception) { }
            }
        }

        private bool Valiate() {
            if(this.txtName.Text.Trim().Length > 50) {
                ShowMessage("姓名格式错误！");
                return false;
            }
            if(this.txtCertId.Text.Trim().Length > 50) {
                ShowMessage("证件号格式错误！");
                return false;
            }
            if(!string.IsNullOrWhiteSpace(this.txtPhone.Text)) {
                if(!Regex.IsMatch(this.txtPhone.Text.Trim(), "^1[3458]\\d{9}$")) {
                    ShowMessage("手机号码格式错误！");
                    return false;
                }
            }
            return true;
        }
    }
}