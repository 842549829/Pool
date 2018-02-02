using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Register
{
    public partial class Register : UnAuthBasePage
    {
        private readonly int m_item = 120;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("register.css");
            SendSMSTime();
            if (!IsPostBack)
            {
                lblPlatform.Text = BasePage.PlatformName;
                this.enrol.Visible = this.enrolPenrol.Visible = this.chkReadingProtocol.Visible = this.forReadingProtocol.Visible = !BasePage.IsOEM;
                this.hfdIsOem.Value = BasePage.IsOEM.ToString();
                hfdPlatformName.Value = BasePage.PlatformName;
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
                    BasePage.RegisterJavaScript(this, "window.onload = function(){CountDown(" + (m_item - timeSeconds) + "); };");
                }
            }
        }
        private void RegisterAccount()
        {
            if (person.Checked)
            {
                AccountCombineService.Register(GetAccountInfo(), GetIndividual(), BasePage.DomainName, BasePage.CurrenContract.ServicePhone, BasePage.PlatformName);
            }
            else
            {
                AccountCombineService.Register(GetAccountInfo(), GetEnterprise(), BasePage.DomainName, BasePage.CurrenContract.ServicePhone, BasePage.PlatformName);
            }
        }
        private void Succeeed()
        {
            string url = "/Agency.htm?target=" + System.Web.HttpUtility.UrlEncode(string.Format("/OrganizationModule/Register/RegisterSucceed.aspx?Type={0}&Account={1}&Name={2}&AccounType={3}&pooypayAccount={4}", hidCpmpanyType.Value, txtAccount.Text, string.IsNullOrWhiteSpace(txtCompany.Text) ? txtName.Text : txtCompany.Text, person.Checked, txtPoolPayUserName.Text.Trim()));
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
                CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), hidCpmpanyType.Value),
                OemOwner = BasePage.IsOEM ? BasePage.OEM.CompanyId : (Nullable<Guid>)null
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
                CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), hidCpmpanyType.Value),
                OemOwner = BasePage.IsOEM ? BasePage.OEM.CompanyId : (Nullable<Guid>)null
            };
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                if (Verification.VerificationCode(txtPhoneCode.Text.Trim(), "phoneValidateCode"))
                {
                    string spreadAccount = "";
                    if (Session["Spreader"] != null)
                    {
                        spreadAccount = Session["Spreader"].ToString();
                    }
                    if (string.IsNullOrWhiteSpace(spreadAccount))
                    {
                        RegisterAccount();
                    }
                    else
                    {
                        var empoloyeeInfo = EmployeeService.QueryEmployee(spreadAccount);
                        if (empoloyeeInfo == null)
                        {
                            RegisterAccount();
                        }
                        else
                        {
                            SpreadAccount(empoloyeeInfo, spreadAccount);
                        }
                    }
                    Session.Remove("phoneValidateCode");
                    Succeeed();
                }
            }
            catch (Exception ex)
            {
                BasePage.ShowExceptionMessage(this, ex, "注册");
            }
        }

        private AccountIndividual GetAccountIndividual(string spreadAccount)
        {
            return new AccountIndividual
            {
                AccountName = txtName.Text.Trim(),
                CertNo = txtIDCard.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), hidCpmpanyType.Value),
                OperatorAccount = spreadAccount,
                IsNotNeedCheck = true
            };
        }
        private AccountEnterprise GetAccountEnterprise(string spreadAccount)
        {
            return new AccountEnterprise
            {
                AbbreviateName = txtAbbreviation.Text.Trim(),
                AccountName = txtCompany.Text.Trim(),
                CompanyPhone = txtCompanyPhone.Text.Trim(),
                ContactName = txtContact.Text.Trim(),
                ContactPhone = txtPhone.Text.Trim(),
                OrginationCode = txtOrganizationCode.Text.Trim(),
                LegalCertNo = txtCompanyIDCard.Text.Trim(),
                CompanyType = (CompanyType)Enum.Parse(typeof(CompanyType), hidCpmpanyType.Value),
                OperatorAccount = spreadAccount,
                IsNotNeedCheck = true
            };
        }
        private void SpreadAccount(EmployeeDetailInfo employeeDetailInfo, string spreadAccount)
        {

            if (person.Checked)
            {
                AccountCombineService.Spread(employeeDetailInfo.Owner, GetAccountInfo(), GetAccountIndividual(spreadAccount), BasePage.DomainName, BasePage.CurrenContract.ServicePhone, BasePage.PlatformName);
            }
            else
            {
                AccountCombineService.Spread(employeeDetailInfo.Owner, GetAccountInfo(), GetAccountEnterprise(spreadAccount), BasePage.DomainName, BasePage.CurrenContract.ServicePhone, BasePage.PlatformName);
            }
        }
    }
}