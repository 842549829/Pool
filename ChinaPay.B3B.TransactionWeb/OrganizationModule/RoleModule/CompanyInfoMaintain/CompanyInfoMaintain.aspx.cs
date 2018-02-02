using System;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain
{
    public partial class CompanyInfoMaintain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var companyInfo = CompanyService.GetCompanyDetail(this.CurrentCompany.CompanyId);
                if (companyInfo.AccountType == AccountBaseType.Enterprise)
                {
                    this.hfdAccountType.Value = "enterprise";
                }
                else
                {
                    this.hfdAccountType.Value = "individual";
                }
                if (companyInfo.AccountType == AccountBaseType.Individual)
                {
                    this.enterpriseName.Visible = false;
                    this.enterprisePhone.Visible = false;
                    this.enterpriseManager.Visible = false;
                    this.enterpriseEmergency.Visible = false;
                    this.fixedPhoneTitle.Visible = true;
                    this.fixedPhoneValue.Visible = true;
                }
                else
                {
                    this.individualName.Visible = false;
                }
                //获取公司工作信息
                getCompanyInfo(companyInfo);
                getUpgradeInfo(companyInfo);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (valiateCompnayInfo())
            {
                try
                {
                    if (this.CurrentCompany.AccountType == AccountBaseType.Individual)
                    {
                        AccountCombineService.AddPurchaseInfo(this.CurrentCompany.CompanyId, getIndividualInfo(),this.CurrentUser.UserName);
                    }
                    else
                    {
                        AccountCombineService.AddPurchaseInfo(this.CurrentCompany.CompanyId, getEnterpriseInfo(),this.CurrentUser.UserName);
                    }
                    ShowMessage("修改成功");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "保存公司信息");
                }
            }
        }

        private void getCompanyInfo(CompanyDetailInfo companyInfo)
        {
            this.lblUserName.Text = companyInfo.UserName;
            this.lblCurrentCompanyType.Text = this.lblCompanyType.Text = companyInfo.CompanyType.GetDescription();
          this.lblContactName.Text = this.hfdContactName.Value = this.txtContactName.Text = companyInfo.Contact;
           this.hfdContactPhone.Value = this.txtContactPhone.Text = companyInfo.ContactPhone;
           this.lblIsExternalInterface.Text = companyInfo.IsOpenExternalInterface ? "已启用" : "未启用";
            this.lblCompanyName.Text = companyInfo.CompanyName;
            this.lblAbbreviateName.Text = companyInfo.AbbreviateName;
            this.lblName.Text = companyInfo.CompanyName;
            this.lblCurrentAccountType.Text = this.lblAccountType.Text = companyInfo.AccountType.GetDescription();
            
            if (companyInfo.AccountType == Common.Enums.AccountBaseType.Enterprise)
            {
                this.hfdCompanyPhone.Value= this.txtOfficePhone.Text = this.txtCompanyPhone.Text = companyInfo.OfficePhones;
                this.txtManagerName.Text = this.txtManager.Text = companyInfo.ManagerName;
                this.hfdManager.Value = companyInfo.ManagerName;
                this.txtManagerMobile.Text = this.txtManagerPhone.Text = companyInfo.ManagerCellphone;
                this.hfdManagerPhone.Value = companyInfo.ManagerCellphone;
                this.txtEmergencyName.Text = this.txtEmergency.Text = companyInfo.EmergencyContact;
                this.hfdEmergency.Value = companyInfo.EmergencyContact;
                this.txtEmergecyMobile.Text = this.txtEmergencyPhone.Text = companyInfo.EmergencyCall;
                this.hfdEmergencyPhone.Value = companyInfo.EmergencyCall;
                this.txtOrgnationCode.Text = this.lblOrganationCode.Text = companyInfo.OrginationCode;
                this.txtCompanyName.Text = companyInfo.CompanyName;
                this.txtCompanyAbbreaviateName.Text = companyInfo.AbbreviateName;
                this.lblContactName.Visible = false;
            }
            else
            {
                this.lblCerNo.Text = companyInfo.CertNo;
                this.txtContactName.Visible = false;
                this.txtFixedPhone.Text = companyInfo.OfficePhones;
            }
            if (!string.IsNullOrWhiteSpace(companyInfo.Province))
            {
                this.hfldAddressCode.Value = AddressShow.GetAddressJson("", companyInfo.Province, "", "");
            }
            if (!string.IsNullOrWhiteSpace(companyInfo.City))
            {
                this.hfldAddressCode.Value = AddressShow.GetAddressJson("", companyInfo.Province, companyInfo.City, "");
            }

            if (!string.IsNullOrWhiteSpace(companyInfo.District))
            {
                this.hfldAddressCode.Value = AddressShow.GetAddressJson("", companyInfo.Province, companyInfo.City, companyInfo.District);
            }
            if (!string.IsNullOrWhiteSpace(companyInfo.Address))
            {
                this.hfdAddress.Value = companyInfo.Address;
                this.txtAddress.Text = companyInfo.Address;
            }
            if (!string.IsNullOrWhiteSpace(companyInfo.ZipCode))
            {
                this.txtPostCode.Text = companyInfo.ZipCode;
                this.hfdPostCode.Value = companyInfo.ZipCode;
            }
            if (!string.IsNullOrWhiteSpace(companyInfo.ContactEmail))
            {
                this.txtEmail.Text = companyInfo.ContactEmail;
                this.hfdEmail.Value = companyInfo.ContactEmail;
            }
            if (!string.IsNullOrWhiteSpace(companyInfo.Faxes))
            {
                this.txtFax.Text = companyInfo.Faxes;
                this.hfdFax.Value = companyInfo.Faxes;
            }
            if (!string.IsNullOrWhiteSpace(companyInfo.ContactQQ))
            {
                this.txtQQ.Text = companyInfo.ContactQQ;
                this.hfdQQ.Value = companyInfo.ContactQQ;
            }
        }

        private PurchaseIndividualInfo getIndividualInfo()
        {
            PurchaseIndividualInfo individual = new PurchaseIndividualInfo();
            var addressInfo = AddressShow.GetAddressBaseInfo(this.hfldAddressCode.Value);
            if (!string.IsNullOrWhiteSpace(this.hfdContactPhone.Value))
                individual.ContactPhone = this.hfdContactPhone.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdAddress.Value))
                individual.Address = this.hfdAddress.Value;
            if (!string.IsNullOrWhiteSpace(addressInfo.ProvinceCode))
                individual.Province = addressInfo.ProvinceCode;
            if (!string.IsNullOrWhiteSpace(addressInfo.CityCode))
                individual.City = addressInfo.CityCode;
            if (!string.IsNullOrWhiteSpace(addressInfo.CountyCode))
                individual.District = addressInfo.CountyCode;
            if (!string.IsNullOrWhiteSpace(this.hfdEmail.Value))
                individual.Email = this.hfdEmail.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdFax.Value))
                individual.Faxes = this.hfdFax.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdQQ.Value))
                individual.QQ = this.hfdQQ.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdPostCode.Value))
                individual.ZipCode = this.hfdPostCode.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdFixedPhone.Value))
                individual.OfficePhone = this.hfdFixedPhone.Value;
            return individual;
        }

        private PurchaseEnterpriseInfo getEnterpriseInfo()
        {
            PurchaseEnterpriseInfo enterprise = new PurchaseEnterpriseInfo();
            var addressInfo = AddressShow.GetAddressBaseInfo(this.hfldAddressCode.Value);
            if (!string.IsNullOrWhiteSpace(this.hfdContactName.Value))
                enterprise.ContactName = this.hfdContactName.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdContactPhone.Value))
                enterprise.ContactPhone = this.hfdContactPhone.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdCompanyPhone.Value))
                enterprise.CompanyPhone = this.hfdCompanyPhone.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdAddress.Value))
                enterprise.Address = this.hfdAddress.Value;
            if (!string.IsNullOrWhiteSpace(addressInfo.CityCode))
                enterprise.City = addressInfo.CityCode;
            if (!string.IsNullOrWhiteSpace(addressInfo.CountyCode))
                enterprise.District = addressInfo.CountyCode;
            if (!string.IsNullOrWhiteSpace(this.hfdEmail.Value))
                enterprise.Email = this.hfdEmail.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdEmergencyPhone.Value))
                enterprise.EmergencyCall = this.hfdEmergencyPhone.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdEmergency.Value))
                enterprise.EmergencyContact = this.hfdEmergency.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdFax.Value))
                enterprise.Faxes = this.hfdFax.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdManagerPhone.Value))
                enterprise.ManagerCellphone = this.hfdManagerPhone.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdManager.Value))
                enterprise.ManagerName = this.hfdManager.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdQQ.Value))
                enterprise.QQ = this.hfdQQ.Value;
            if (!string.IsNullOrWhiteSpace(this.hfdPostCode.Value))
                enterprise.ZipCode = this.hfdPostCode.Value;
            if (!string.IsNullOrWhiteSpace(addressInfo.ProvinceCode))
                enterprise.Province = addressInfo.ProvinceCode;
            return enterprise;
        }

        private bool valiateCompnayInfo()
        {
            if (!Regex.IsMatch(this.hfdContactName.Value, @"^[a-zA-z\u4e00-\uf95a]{1,8}$"))
            {
                ShowMessage("联系人格式错误！");
                return false;
            }
            if (!Regex.IsMatch(this.hfdContactPhone.Value, @"^1[3458]\d{9}$"))
            {
                ShowMessage("联系人电话格式错误！");
                return false;
            }
            if (this.CurrentCompany.AccountType == AccountBaseType.Enterprise)
            {
                if (!Regex.IsMatch(this.hfdCompanyPhone.Value, @"^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$"))
                {
                    ShowMessage("企业电话格式错误！");
                    return false;
                }
                if (!Regex.IsMatch(this.hfdManager.Value, @"^[a-zA-z\u4e00-\uf95a]{1,8}$"))
                {
                    ShowMessage("负责人格式错误！");
                    return false;
                }
                if (!Regex.IsMatch(this.hfdManagerPhone.Value, @"^1[3458]\d{9}$"))
                {
                    ShowMessage("负责人电话格式错误！");
                    return false;
                }
                if (!Regex.IsMatch(this.hfdEmergency.Value, @"^[a-zA-z\u4e00-\uf95a]{1,8}$"))
                {
                    ShowMessage("紧急联系人格式错误！");
                    return false;
                }

                if (!Regex.IsMatch(this.hfdEmergencyPhone.Value, @"^1[3458]\d{9}$"))
                {
                    ShowMessage("紧急联系人电话格式错误！");
                    return false;
                }
            }
            if (this.hfdAddress.Value.Length > 25 || this.hfdAddress.Value.Length <= 0)
            {
                ShowMessage("地址格式错误！");
                return false;
            }
            if (!Regex.IsMatch(this.hfdEmail.Value, @"^\w+@\w+(\.\w{2,4}){1,2}$"))
            {
                ShowMessage("邮箱格式错误！");
                return false;
            }
            if (!string.IsNullOrEmpty(this.hfdFax.Value) && !Regex.IsMatch(this.hfdFax.Value, @"^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$"))
            {
                ShowMessage("传真格式错误！");
                return false;
            }
            if (!Regex.IsMatch(this.hfdPostCode.Value, @"^[1-9]{1}\d{5}$"))
            {
                ShowMessage("邮编格式错误！");
                return false;
            }
            if (!string.IsNullOrEmpty(this.hfdQQ.Value) && !Regex.IsMatch(this.hfdQQ.Value, @"^\d{5,12}$"))
            {
                ShowMessage("QQ格式错误！");
                return false;
            }
            if (this.CurrentCompany.AccountType == AccountBaseType.Individual)
            {
                if (!string.IsNullOrWhiteSpace(this.hfdFixedPhone.Value) && !Regex.IsMatch(this.hfdFixedPhone.Value, @"^((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}$"))
                {
                    ShowMessage("固定电话格式错误！");
                    return false;
                }
            }
            return true;
        }

        private void getUpgradeInfo(CompanyDetailInfo companyInfo)
        {
            if (CompanyUpgradeService.IsValid(companyInfo) && BasePage.SuperiorOEM == null)
            {
                this.typeUpgradeApply.Visible = true;
                if (companyInfo.CompanyType == Common.Enums.CompanyType.Purchaser && companyInfo.AccountType == AccountBaseType.Individual)
                {
                    this.rbnSupplierIndividual.Visible = true;
                    this.rbnProviderEnterprise.Visible = true;
                    this.rbnSupplierIndividual.Checked = true;
                }
                if (companyInfo.CompanyType == Common.Enums.CompanyType.Purchaser && companyInfo.AccountType == AccountBaseType.Enterprise)
                {
                    this.rbnSupplierEnterprise.Visible = true;
                    this.rbnProviderEnterprise.Visible = true;
                    this.rbnSupplierEnterprise.Checked = true;
                }
                if (companyInfo.CompanyType == Common.Enums.CompanyType.Supplier && companyInfo.Audited == true && companyInfo.AuditTime.HasValue)
                {
                    this.rbnProviderEnterprise.Visible = true;
                    this.rbnProviderEnterprise.Checked = true;
                }
                if (companyInfo.CompanyType == Common.Enums.CompanyType.Supplier && companyInfo.Audited == false && companyInfo.AuditTime.HasValue)
                {
                    this.rbnProviderEnterprise.Visible = true;
                    if (companyInfo.AccountType == AccountBaseType.Individual)
                    {
                        this.rbnSupplierIndividual.Visible = true;
                        this.rbnSupplierIndividual.Checked = true;
                    }
                    else
                    {
                        this.rbnSupplierEnterprise.Visible = true;
                        this.rbnSupplierEnterprise.Checked = true;
                    }
                }
                if (companyInfo.CompanyType == Common.Enums.CompanyType.Provider && companyInfo.Audited == false && companyInfo.AuditTime.HasValue)
                {
                    this.rbnProviderEnterprise.Visible = true;
                    this.rbnSupplierEnterprise.Visible = true;
                    this.rbnSupplierEnterprise.Checked = true;
                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (valiateUpgradeInfo())
            {
                try
                {
                    CompanyUpgradeService.Save(getCompanyUpgrade(), this.CurrentUser.UserName);
                    ShowMessage("账号变更申请成功！");
                    this.typeUpgradeApply.Visible = false;
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                }
            }
        }

        private bool valiateUpgradeInfo()
        {
            if (this.hfdValid.Value=="true")
            {
                if (!Regex.IsMatch(this.txtOrgnationCode.Text, @"^\d{8}-[\dxX]{1}$"))
                {
                    ShowMessage("组织机构代码格式错误！");
                    return false;
                }
                if (!Regex.IsMatch(this.txtOfficePhone.Text, @"((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}"))
                {
                    ShowMessage("公司电话格式错误！");
                    return false;
                }
                if (!Regex.IsMatch(this.txtManagerName.Text, @"^[a-zA-z\u4e00-\uf95a]{1,8}$"))
                {
                    ShowMessage("负责人格式错误！");
                    return false;
                }
                if (!Regex.IsMatch(this.txtManagerMobile.Text, @"^1[3458]\d{9}$"))
                {
                    ShowMessage("负责人电话格式错误！");
                    return false;
                }
                if (!Regex.IsMatch(this.txtEmergencyName.Text, @"^[a-zA-z\u4e00-\uf95a]{1,8}$"))
                {
                    ShowMessage("紧急联系人格式错误！");
                    return false;
                }

                if (!Regex.IsMatch(this.txtEmergecyMobile.Text, @"^1[3458]\d{9}$"))
                {
                    ShowMessage("紧急联系人电话格式错误！");
                    return false;
                }
            }
            if (!this.chkProtocol.Checked)
                return false;
            return true;
        }

        private CompanyUpgrade getCompanyUpgrade()
        {
            var companyUpgrade = new CompanyUpgrade();
            if (this.hfdValid.Value == "true")
            {
                companyUpgrade.Name = this.txtCompanyName.Text;
                companyUpgrade.AbbreviateName = this.txtCompanyAbbreaviateName.Text;
                companyUpgrade.OfficePhones = this.txtOfficePhone.Text;
                companyUpgrade.EmergencyName = this.txtEmergencyName.Text;
                companyUpgrade.EmergencyPhone = this.txtEmergecyMobile.Text;
                companyUpgrade.ManagerName = this.txtManagerName.Text;
                companyUpgrade.ManagerPhone = this.txtManagerMobile.Text;
                companyUpgrade.OrginationCode = this.txtOrgnationCode.Text;
            }
            companyUpgrade.ApplyTime = DateTime.Now;
            companyUpgrade.Audited = false;
            companyUpgrade.Company = this.CurrentCompany.CompanyId;
            companyUpgrade.UserNo = this.CurrentCompany.UserName;
            if (rbnSupplierIndividual.Checked)
            {
                companyUpgrade.AccountType = AccountBaseType.Individual;
                companyUpgrade.Type = Common.Enums.CompanyType.Supplier;
            }
            if (rbnSupplierEnterprise.Checked)
            {
                companyUpgrade.AccountType = AccountBaseType.Enterprise;
                companyUpgrade.Type = Common.Enums.CompanyType.Supplier;
            }
            if (rbnProviderEnterprise.Checked)
            {
                companyUpgrade.AccountType = AccountBaseType.Enterprise;
                companyUpgrade.Type = Common.Enums.CompanyType.Provider;
            }
            return companyUpgrade;
        }
    }

}