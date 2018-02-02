using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddAccount
{
    public partial class AddAccount : BasePage
    {
        private readonly int m_time = 60;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            SendSMSTime();
            //if (!IsPostBack && CurrentCompany.CompanyType == CompanyType.Platform)
            //{
            //    pos.Visible = false;
            //    btnPhoneCode.Visible = false;
            //}
            if (!IsPostBack)
            {
                hfdPlatformName.Value = PlatformName;
            }
        }
        private void SendSMSTime()
        {
            if (Session["phoneTime"] != null)
            {
                var phoneTime = (DateTime)Session["phoneTime"];
                int timeSeconds = (int)(DateTime.Now - phoneTime).TotalSeconds;
                if (timeSeconds <= m_time)
                {
                    BasePage.RegisterJavaScript(this, "window.onload = function(){CountDown(" + (m_time - timeSeconds) + "); };");
                }
            }
        }
        private void RegisterAccount()
        {
            if (CurrentCompany.CompanyType == CompanyType.Platform)
            {
                if (person.Checked)
                {
                    AccountCombineService.Register(GetAccountInfo(), GetIndividual(), DomainName, CurrenContract.ServicePhone,PlatformName);
                }
                else {
                    AccountCombineService.Register(GetAccountInfo(), GetEnterprise(), DomainName, CurrenContract.ServicePhone,PlatformName);
                }
            }
            else
            {
                //if (VerificationIPAddress.VerificationIP() && Verification.VerificationCode(txtPhoneCode.Text, "phoneValidateCode"))
                //{ }
                if (person.Checked)
                {
                   // AccountCombineService.Spread(CurrentCompany.CompanyId, GetAccountInfo(), GetIndividual());
                }
                else
                {
                   // AccountCombineService.Spread(CurrentCompany.CompanyId, GetAccountInfo(), GetEnterprise());
                }
            }
        }
        //获取账户信息
        private AccountInfo GetAccountInfo() {
            return new AccountInfo { 
                AccountNo = txtAccount.Text.Trim(),
                Password = txtPassword.Text,
                ComfirmPassword = txtConfirmPassword.Text
            };
        }
        //获取个人信息
        private AccountBasicIndividual GetIndividual()
        {
            return new AccountBasicIndividual
            {
                AccountName = txtName.Text.Trim(),
                CertNo = txtIDCard.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                IsNeedApply = false,
                CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), hidCpmpanyType.Value)
            };
        }
        //获取企业信息
        private AccountBasicEnterprise GetEnterprise()
        {
            return new AccountBasicEnterprise
            {
                AbbreviateName = txtAbbreviation.Text.Trim(),
                AccountName = txtCompany.Text.Trim(),
                CompanyPhone = txtCompanyPhone.Text.Trim(),
                ContactName = txtContact.Text.Trim(),
                ContactPhone = txtPhone.Text.Trim(),
                OrginationCode = txtOrganizationCode.Text.Trim(),
                IsNeedApply = false,
                CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), hidCpmpanyType.Value)
            };
        }
        private void Succeeed()
        {
            string url = string.Format("./Succeed.aspx?Type={0}&Account={1}&Name={2}&AccounType={3}",
                hidCpmpanyType.Value, txtAccount.Text, string.IsNullOrWhiteSpace(txtCompany.Text) ? txtName.Text : txtCompany.Text, person.Checked);
            Response.Redirect(url, false);
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            { 
                RegisterAccount();
                Session.Remove("phoneValidateCode");
                Succeeed();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "开户");
            }
        }
    }
}