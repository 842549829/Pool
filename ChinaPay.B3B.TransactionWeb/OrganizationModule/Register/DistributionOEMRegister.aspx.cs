using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Register
{
    public partial class DistributionOEMRegister : System.Web.UI.Page
    {
        private readonly int m_item = 120;
        protected void Page_Load(object sender, EventArgs e)
        {
            SendSMSTime();
        }
        private void SendSMSTime()
        {
            if (Session["phoneTime"] != null)
            {
                var phoneTime = (DateTime)Session["phoneTime"];
                int timeSeconds = (int)(DateTime.Now - phoneTime).TotalSeconds;
                if (timeSeconds <= m_item)
                {
                    BasePage.RegisterJavaScript(this, "window.onload = function(){CountDown(" + (m_item - timeSeconds) + "); };");
                }
            }
        }
        private void RegisterAccount()
        {
            if (person.Checked)
            {
                AccountCombineService.Register(GetAccountInfo(), GetIndividual());
            }
            else
            {
                AccountCombineService.Register(GetAccountInfo(), GetEnterprise());
            }
        }
        private void Succeeed()
        {
            string url = "/Agency.htm?target=" + System.Web.HttpUtility.UrlEncode(string.Format("/OrganizationModule/Register/DistributionOEMRegisterSucceed.aspx?Type={0}&Account={1}&Name={2}&AccounType={3}&pooypayAccount={4}",(int)CompanyType.Purchaser, txtAccount.Text, string.IsNullOrWhiteSpace(txtCompany.Text) ? txtName.Text : txtCompany.Text, person.Checked, txtPoolPayUserName.Text.Trim()));
            Response.Redirect(url, false);
        }
        // 获取账户对象
        private AccountInfo GetAccountInfo()
        {
            return new AccountInfo
            {
                AccountNo = txtAccount.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                ComfirmPassword = txtConfirmPassword.Text.Trim(),
                IsPersonAccountNo = chkIsPersonAccountNo.Checked,
                PoolPayUserName = txtPoolPayUserName.Text.Trim()
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
                IsNeedApply = chkPos.Checked,
                CompanyType = CompanyType.Purchaser,
                OemOwner = Guid.Parse("85DC92F1-04B7-4539-B774-00348C0E120C")
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
                IsNeedApply = chkPos.Checked,
                IDCard = txtCompanyIDCard.Text.Trim(),
                CompanyType = CompanyType.Purchaser,
                OemOwner = Guid.Parse("F5020F89-2BA3-40BA-896D-0050DA3B9978")
            };
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                if (Verification.VerificationCode(txtPhoneCode.Text.Trim(), "phoneValidateCode"))
                {
                    RegisterAccount();
                    Session.Remove("phoneValidateCode");
                    Succeeed();
                }
            }
            catch (Exception ex)
            {
                BasePage.ShowExceptionMessage(this, ex, "注册");
            }
        }
    }
}