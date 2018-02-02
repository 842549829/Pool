using System;
using ChinaPay.B3B.Service.Organization;
using PoolPay.DataTransferObject;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Account
{
    public partial class RegisterCollectionAccount :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Register();
                Response.Redirect("./AccountInformation.aspx");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "注册");
            }
        }
        private void Register() {
            if (rdoIndividual.Checked)
            {
                AccountCombineService.AddReciveAccount(this.CurrentCompany.CompanyId, CreatePerson(),CurrentUser.UserName);
            }
            else 
            {
                AccountCombineService.AddReciveAccount(this.CurrentCompany.CompanyId, CreateCompany(),CurrentUser.UserName);
            }
        }
        private  AccountDTO CreatePerson()
        {
            return new AccountDTO{ 
                 AccountNo = this.txtAccount.Text.Trim(),
                 LoginPassword = this.txtPassword.Text.Trim(),
                 PayPassword = this.txtPayPassowrd.Text.Trim(),
                 AdministorName = this.txtName.Text.Trim(),
                 AdministorCertId  = this.txtIDCard.Text.Trim(),
                 ContactPhone = this.txtCellPhone.Text.Trim(),
                 IsVip = true
            };
        }
        private EnterpriseAccountDTO CreateCompany()
        {
            return new EnterpriseAccountDTO
            {
                AccountNo = this.txtAccount.Text.Trim(),
                LoginPassword = this.txtPassword.Text.Trim(),
                PayPassword = this.txtPayPassowrd.Text.Trim(),
                CompanyName = this.txtCompanyName.Text.Trim(),
                OrganizationCode = this.txtOrganizationCode.Text.Trim(),
                LegalContactPhone = this.txtCompanyPhone.Text.Trim(),
                LegalPersonName = this.txtLegalPersonName.Text.Trim(),
                ContactPhone = txtCellPhone.Text.Trim(),
                IsVip = true
            };
        }
    }
}