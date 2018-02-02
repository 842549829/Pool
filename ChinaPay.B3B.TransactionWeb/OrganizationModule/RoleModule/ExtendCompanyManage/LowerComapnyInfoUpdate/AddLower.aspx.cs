using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate
{
    public partial class AddLower : BasePage
    {
        private readonly int m_item = 60;
        protected void Page_Load(object sender, EventArgs e)
        {
            SendSMSTime();
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = PlatformName;
            }
        }
        private void SendSMSTime()
        {
            if (Session["phoneTime"] != null)
            {
                var phoneTime = (DateTime)Session["phoneTime"];
                int timeSeconds = (int)(DateTime.Now - phoneTime).TotalSeconds;
                if (timeSeconds <= m_item)
                {
                    BasePage.RegisterJavaScript(this, "window.onload = function(){CountDown(" + (m_item -timeSeconds) + "); };");
                }
            }
        }
        private void RegisterAccount() {
            if (string.IsNullOrWhiteSpace(Request.QueryString["Type"]))
            { 
                throw new ArgumentNullException("开户类型不存在"); 
            }
            if (Request.QueryString["Type"] == "Subordinate")
            {
                if (person.Checked)
                {
                   // AccountCombineService.CreateSubordinate(CurrentCompany.CompanyId,GetAccountInfo(), GetIndividual());
                }
                else
                {
                  //  AccountCombineService.CreateSubordinate(CurrentCompany.CompanyId, GetAccountInfo(), GetEnterprise());
                }
            }
            else {
                if (person.Checked)
                {
                   // AccountCombineService.CreatePurchase(CurrentCompany.CompanyId, GetAccountInfo(), GetIndividual());
                }
                else {
                   // AccountCombineService.CreatePurchase(CurrentCompany.CompanyId, GetAccountInfo(), GetEnterprise());
                }
            }
        }
        private void Succeeed() {
            string url = string.Format("../../../CommonContent/AddAccount/Succeed.aspx?Type={0}&Account={1}&Name={2}&AccounType={3}",
                CompanyType.Purchaser, txtAccount.Text, string.IsNullOrWhiteSpace(txtCompany.Text) ? txtName.Text : txtCompany.Text, person.Checked);
            Response.Redirect(url, false);
        }
        // 获取账户对象
        private  AccountInfo GetAccountInfo() {
            return new AccountInfo { AccountNo = txtAccount.Text.Trim(), Password = txtPassword.Text.Trim(), ComfirmPassword = txtConfirmPassword.Text.Trim() };
        }
        //获取个人信息
        private AccountBasicIndividual GetIndividual()
        {
            return new AccountBasicIndividual { 
                 AccountName = txtName.Text.Trim(),
                 CertNo = txtIDCard.Text.Trim(),
                 Phone = txtPhone.Text.Trim(),
                 IsNeedApply = false,
                 CompanyType = CompanyType.Purchaser
            };
        }
        //获取企业信息
        private AccountBasicEnterprise GetEnterprise() {
            return new AccountBasicEnterprise {
                AbbreviateName = txtAbbreviation.Text.Trim(),
                AccountName = txtCompany.Text.Trim(),
                CompanyPhone = txtCompanyPhone.Text.Trim(),
                ContactName = txtContact.Text.Trim(),
                ContactPhone = txtPhone.Text.Trim(),
                OrginationCode = txtOrganizationCode.Text.Trim(),
                IsNeedApply = false,
                CompanyType = CompanyType.Purchaser
            };
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (VerificationIPAddress.VerificationIP() && Verification.VerificationCode(txtPhoneCode.Text.Trim(), "phoneValidateCode"))
                //{ }
                Session.Remove("phoneValidateCode");
                RegisterAccount();
                Succeeed();
            }
            catch (Exception ex)
            {
                BasePage.ShowExceptionMessage(this, ex, "注册");
            }
        }
    }
}