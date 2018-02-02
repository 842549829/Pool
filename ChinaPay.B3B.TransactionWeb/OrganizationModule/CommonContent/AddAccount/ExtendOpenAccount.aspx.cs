using System;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddAccount
{
    public partial class ExtendOpenAccount : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = PlatformName;
                InitData();
            }
        }
        private void InitData() 
        {
            protorlIsShow.Visible = !this.CurrentCompany.IsOem;
            this.hfdIsOem.Value = this.CurrentCompany.IsOem.ToString();
            string type = Request.QueryString["Type"];
            if (!string.IsNullOrEmpty(type) && type=="Subordinate")
            {
                if (!CompanyService.GetCompanyParameter(CurrentCompany.CompanyId).CanHaveSubordinate)
                {
                    Response.Redirect(ResolveUrl("~/StaticHtml/NoAccess.aspx"), true);
                }
                person.Visible = false;
            }
        }
        private AccountInfo GetAccountInfo()
        {
            return new AccountInfo
            {
                AccountNo = txtAccountNo.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                ComfirmPassword = txtConfirmPassword.Text.Trim()
            };
        }
        private AccountIndividual GetAccountIndividual(AddressInfo address)
        {
            return new AccountIndividual
            {
                AccountName = txtPresonName.Text.Trim(),
                CertNo = txtPresonIDCard.Text.Trim(),
                Phone = txtPresonPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Faxes = txtFaxes.Text.Trim(),
                ZipCode = txtPostCode.Text.Trim(),
                QQ = txtQQ.Text.Trim(),
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                OperatorAccount = CurrentUser.UserName,
                CompanyType = Common.Enums.CompanyType.Purchaser
            };
        }
        private AccountEnterprise GetAccountEnterprise(AddressInfo address)
        {
            return new AccountEnterprise
            {
                AccountName = txtCompany.Text.Trim(),
                AbbreviateName = txtAbbreviation.Text.Trim(),
                OrginationCode = txtOrganizationCode.Text.Trim(),
                CompanyPhone = txtCompanyPhone.Text.Trim(),
                ContactName = txtContact.Text.Trim(),
                ContactPhone = txtContactPhone.Text.Trim(),
                ManagerName = txtMangerName.Text.Trim(),
                ManagerPhone = txtManagerCellphone.Text.Trim(),
                EmergencyName = txtEmergencyContact.Text.Trim(),
                EmergencyPhone = txtEmergencyPhone.Text.Trim(),
                LegalCertNo = txtCompanyIDCard.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Faxes = txtFaxes.Text.Trim(),
                ZipCode = txtPostCode.Text.Trim(),
                QQ = txtQQ.Text.Trim(),
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                OperatorAccount = CurrentUser.UserName,
                CompanyType = Common.Enums.CompanyType.Purchaser
            };
        }
        private void ValidateCode()
        {
            var verifyCode = Session["vate"];
            if (verifyCode == null) 
            { 
                throw new ArgumentNullException("验证已经过期,请重刷新页面"); 
            }
            if ((verifyCode.ToString()).ToUpper() != (this.txtverifyCode.Text.Trim()).ToUpper())
            {
                throw new InvalidOperationException("验证码格式错误");
            }
        }
        private void AddAccount()
        {
            var address = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressBaseInfo(hfldAddressCode.Value);
            string type = Request.QueryString["Type"];
            if (!string.IsNullOrEmpty(type) && type == "Subordinate")
            {
                AccountCombineService.CreateSubordinate(CurrentCompany.CompanyId, GetAccountInfo(), GetAccountEnterprise(address),DomainName,CurrenContract.ServicePhone,PlatformName);
            }
            else 
            {
                if (company.Checked)
                {
                    AccountCombineService.CreatePurchase(CurrentCompany.CompanyId, GetAccountInfo(), GetAccountEnterprise(address),DomainName,CurrenContract.ServicePhone,PlatformName);
                }
                else 
                {
                    AccountCombineService.CreatePurchase(CurrentCompany.CompanyId, GetAccountInfo(), GetAccountIndividual(address),DomainName,CurrenContract.ServicePhone,PlatformName);
                }
            }
        }
        private void Succeeed()
        {
            string url = string.Format("./Succeed.aspx?Type={0}&Account={1}&Name={2}&AccounType={3}",
                hidCpmpanyType.Value, txtAccountNo.Text, string.IsNullOrWhiteSpace(txtCompany.Text) ? txtPresonName.Text : txtCompany.Text, person.Checked);
            Response.Redirect(url, false);
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateCode();
                AddAccount();
                Succeeed();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "开户");
            }
        }
    }
}