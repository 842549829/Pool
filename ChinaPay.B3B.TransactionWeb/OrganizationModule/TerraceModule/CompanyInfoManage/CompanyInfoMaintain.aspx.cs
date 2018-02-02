using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class CompanyInfoMaintain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    CompanyDetailInfo info = CompanyService.GetCompanyDetail(Guid.Parse(companyId)); ;
                    BindCompanyInfo(info);
                }
            }
        }
        private void BindCompanyInfo(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription() + "(" + info.AccountType.GetDescription() + ")";
            this.hfldAddressCode.Value = AddressShow.GetAddressJson(info.Area, info.Province, info.City, info.District);
            this.txtAddress.Text = info.Address;
            this.txtPostCode.Text = info.ZipCode;
            this.txtCompanyPhone.Text = info.OfficePhones;
            this.txtFaxes.Text = info.Faxes;
            this.txtLinkman.Text = info.Contact;
            this.txtLinkManPhone.Text = info.ContactPhone;
            this.txtEmail.Text = info.ContactEmail;
            this.txtQQ.Text = info.ContactQQ;
            BindEnterprise(info);
        }
        private void BindEnterprise(CompanyDetailInfo info)
        {
            if (info.AccountType == AccountBaseType.Enterprise)
            {
                this.presonName.Visible = false;
                this.txtCompanyName.Text = info.CompanyName;
                this.txtCompanyShortName.Text = info.AbbreviateName;
                this.txtOrginationCode.Text = info.OrginationCode;
                this.txtCompanyPhone.Text = info.OfficePhones;
                this.txtManagerName.Text = info.ManagerName;
                this.txtManagerCellphone.Text = info.ManagerCellphone;
                this.txtEmergencyContact.Text = info.EmergencyContact;
                this.txtEmergencyCall.Text = info.EmergencyCall;
            }
            else
            {
                fixedPhone.Visible = true;
                this.txtFixedPhone.Text = info.OfficePhones;
                this.txtPresonName.Text = info.CompanyName;
                this.txtIdCard.Text = info.CertNo;
                enterpriseContact.Visible = false;
                enterpriseinfo.Visible = false;
            }
        }
        private CompanyIndividualUpdateInfo GetIndividualInfo(AddressInfo address)
        {
            return new CompanyIndividualUpdateInfo
            {
                ContactName = txtLinkman.Text.Trim(),
                ContactPhone = txtLinkManPhone.Text.Trim(),
                OperatorAccount = this.CurrentUser.UserName,
                CertNo = this.txtIdCard.Text.Trim(),
                CompanyId = Guid.Parse(Request.QueryString["CompanyId"]),
                Name = this.txtPresonName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                Faxes = txtFaxes.Text.Trim(),
                 OfficePhone = txtFixedPhone.Text.Trim(),
                QQ = txtQQ.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                ZipCode = txtPostCode.Text.Trim()
            };
        }
        private CompanyEnterpriseUpdateInfo GetEnterpriseInfo(AddressInfo address)
        {
            return new CompanyEnterpriseUpdateInfo
            {
                CompanyId = Guid.Parse(Request.QueryString["CompanyId"]),
                CompanyName = this.txtCompanyName.Text.Trim(),
                AbbreviateName = this.txtCompanyShortName.Text.Trim(),
                CompanyPhone = this.txtCompanyPhone.Text.Trim(),
                ContactName = this.txtLinkman.Text.Trim(),
                ContactPhone = this.txtLinkManPhone.Text.Trim(),
                OperatorAccount = this.CurrentUser.UserName,
                OrginationCode = this.txtOrginationCode.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                Faxes = txtFaxes.Text.Trim(),
                QQ = txtQQ.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                ZipCode = txtPostCode.Text.Trim(),
                ManagerName = txtManagerName.Text.Trim(),
                ManagerCellphone = txtManagerCellphone.Text.Trim(),
                EmergencyContact = txtEmergencyContact.Text.Trim(),
                EmergencyCall = txtEmergencyCall.Text.Trim()
            };
        }
        private void Update()
        {
            var address = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressBaseInfo(hfldAddressCode.Value);
            if (address == null || string.IsNullOrEmpty(address.CountyCode)) throw new ArgumentNullException("请选择所地");
            string strCompanyId = Request.QueryString["CompanyId"];
            string strAccountType = Request.QueryString["AccountType"];
            if (!string.IsNullOrEmpty(strCompanyId) && !string.IsNullOrEmpty(strAccountType))
            {
                AccountBaseType accountType = (AccountBaseType)byte.Parse(strAccountType);
                Guid id = Guid.Parse(strCompanyId);
                if (accountType == AccountBaseType.Individual)
                {
                    AccountCombineService.UpdateIndividualInfo(GetIndividualInfo(address));
                }
                else
                {
                    AccountCombineService.UpdateEnterpriseInfo(GetEnterpriseInfo(address));
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Update();
                RegisterScript("alert('修改成功！');window.location.href='CompanyList.aspx';", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }
    }
}