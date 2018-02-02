using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class CertificationAudit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                string id = Request.QueryString["CompanyId"];
                string auditTypeValue = Request.QueryString["AuditType"];
                AuditType auditType = (AuditType)int.Parse(auditTypeValue);
                this.hfdCompanyId.Value = id;
                this.hfdAuditType.Value = auditTypeValue;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    Guid companyId = Guid.Parse(id);
                    if (auditType == AuditType.NormalAudit)
                    {
                        BindInfo(companyId);
                    }
                    else
                    {
                        BindUpgradeInfo(companyId);
                    }
                }
            }
        }

        private void BindUpgradeInfo(Guid companyId)
        {
            var companyUpgrade = CompanyUpgradeService.QueryCompanyUpgrade(companyId);
            var companyDetailInfo = CompanyService.GetCompanyDetail(companyId);
            var companyDocument = AccountCombineService.QueryCompanyDocument(companyId);
            this.lblUpgradeType.Visible = true;
            this.lblCong.Visible = true;
            this.lblChange.Visible = true;
            BindCommenContent(companyDetailInfo);
            this.lblCompanyType.Style.Add(HtmlTextWriterStyle.Color, "blue");
            this.lblUpgradeType.Style.Add(HtmlTextWriterStyle.Color, "blue");
            if (companyUpgrade.AccountType == AccountBaseType.Individual)
            {
                this.hfdAccountType.Value = "individual";
                this.txtFixedPhone.Text = companyDetailInfo.OfficePhones;
                this.lblUpgradeType.Text = CompanyType.Supplier.GetDescription() + "(" + AccountBaseType.Individual.GetDescription() + ")";
                this.lblCompanyInfo.Visible = false;
                this.lblContactInfo.Visible = false;
                lblType.Text = "身份证";
                this.lblTrueName.Text = companyDetailInfo.CompanyName;
                this.lblCertNo.Text = companyDetailInfo.CertNo;
                if (companyDocument != null)
                {
                    if (companyDocument.CertLicense != null && companyDocument.CertLicense.Length > 0)
                    {
                        this.certNoOrBussness.Visible = false;
                        this.iataAccess.Visible = false;
                        this.bussnessTime.Visible = false;
                        this.hfdBussnessValid.Value = "false";
                    }
                }
                else
                {
                    this.hfdValid.Value = "false";
                }
            }
            else
            {
                this.hfdAccountType.Value = "enterprise";
                this.fixedPhoneTitle.Visible = false;
                this.fixedPhoneValue.Visible = false;
                this.lblIndividual.Visible = false;
                if (companyDocument != null)
                {
                    if (companyDocument.BussinessLicense != null)
                    {
                        this.certNoOrBussness.Visible = false;
                        this.hfdBussnessValid.Value = "false";
                        this.access.Visible = true;
                    }
                    if (companyDocument.IATALicense != null)
                    {
                        this.iataAccess.Visible = false;
                        this.access.Visible = false;
                        this.hfdValid.Value = "false";
                    }
                }
                else
                {
                    if (companyUpgrade.Type == CompanyType.Supplier)
                    {
                        this.hfdValid.Value = "false";
                        this.hfdBussnessValid.Value = "false";
                    }
                }
                if (companyUpgrade.Type == CompanyType.Provider)
                {
                    this.bussnessTime.Visible = false;
                    this.lblUpgradeType.Text = CompanyType.Provider.GetDescription() + "(" + AccountBaseType.Enterprise.GetDescription() + ")";
                }
                else
                {
                    this.lblUpgradeType.Text = CompanyType.Supplier.GetDescription() + "(" + AccountBaseType.Enterprise.GetDescription() + ")";
                }
                txtManagerName.Text = string.IsNullOrWhiteSpace(companyUpgrade.ManagerName) ? companyDetailInfo.ManagerName : companyUpgrade.ManagerName;
                txtManagerPhone.Text = string.IsNullOrWhiteSpace(companyUpgrade.ManagerPhone) ? companyDetailInfo.ManagerCellphone : companyUpgrade.ManagerPhone;
                txtEmergencyContact.Text = string.IsNullOrWhiteSpace(companyUpgrade.EmergencyName) ? companyDetailInfo.EmergencyContact : companyUpgrade.EmergencyName;
                txtEmergencyCall.Text = string.IsNullOrWhiteSpace(companyUpgrade.EmergencyPhone) ? companyDetailInfo.EmergencyCall : companyUpgrade.EmergencyPhone;
                lblAbbreviation.Text = string.IsNullOrWhiteSpace(companyUpgrade.AbbreviateName) ? companyDetailInfo.AbbreviateName : companyUpgrade.AbbreviateName;
                lblCompanyPhone.Text = string.IsNullOrWhiteSpace(companyUpgrade.OfficePhones) ? companyDetailInfo.OfficePhones : companyUpgrade.OfficePhones;
                lblIdCode.Text = string.IsNullOrWhiteSpace(companyUpgrade.OrginationCode) ? companyDetailInfo.OrginationCode : companyUpgrade.OrginationCode;
            }
        }

        private void BindInfo(Guid id)
        {
            var info = CompanyService.GetCompanyDetail(id);
            if (info.AccountType == AccountBaseType.Individual)
            {
                this.hfdAccountType.Value = "individual";
            }
            else
            {
                this.hfdAccountType.Value = "enterprise";
            }
            BindCommenContent(info);
            BindCompany(info);
        }

        private void BindCommenContent(CompanyDetailInfo info)
        {
            lblAccountName.Text = info.UserName;
            lblCompanyType.Text = info.CompanyType.GetDescription() + "(" + info.AccountType.GetDescription() + ")";
            lblCompanyName.Text = info.CompanyName;
            lblContact.Text = info.Contact;
            lblContactPhone.Text = info.ContactPhone;
            txtAddress.Text = info.Address;
            txtPostCode.Text = info.ZipCode;
            txtEmail.Text = info.ContactEmail;
            txtFaxes.Text = info.Faxes;
            txtQQ.Text = info.ContactQQ;
            if (info.PeriodStartOfUse.HasValue)
                txtBeginTime.Text = info.PeriodStartOfUse.Value.ToString("yyyy-MM-dd");
            if (info.PeriodEndOfUse.HasValue)
                txtEndTime.Text = info.PeriodEndOfUse.Value.ToString("yyyy-MM-dd");
            BindAddress(info);
        }
        private void BindAddress(DataTransferObject.Organization.CompanyDetailInfo info)
        {
            hfldAddressCode.Value = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressJson(info.Area, info.Province, info.City, info.District);
        }
        private void BindCompany(DataTransferObject.Organization.CompanyDetailInfo info)
        {
            if (info.AccountType == Common.Enums.AccountBaseType.Individual)
            {
                this.lblCompanyInfo.Visible = false;
                this.lblContactInfo.Visible = false;
                lblType.Text = "身份证";
                this.lblTrueName.Text = info.CompanyName;
                this.lblCertNo.Text = info.CertNo;
                this.hfdValid.Value = "false";
                this.txtFixedPhone.Text = info.OfficePhones;
            }
            else
            {
                this.fixedPhoneTitle.Visible = false;
                this.fixedPhoneValue.Visible = false;
                this.lblIndividual.Visible = false;
                if (info.CompanyType == CompanyType.Provider)
                    this.bussnessTime.Visible = false;
                if (info.CompanyType == CompanyType.Supplier)
                    this.hfdValid.Value = "false";
                txtManagerName.Text = info.ManagerName;
                txtManagerPhone.Text = info.ManagerCellphone;
                txtEmergencyContact.Text = info.EmergencyContact;
                txtEmergencyCall.Text = info.EmergencyCall;
                lblAbbreviation.Text = info.AbbreviateName;
                lblCompanyPhone.Text = info.OfficePhones;
                lblIdCode.Text = info.OrginationCode;
            }
        }

        //出票企业对象
        private ProviderAuditInfo GetProviderAuditInfo(AddressInfo address, bool isUpgrade)
        {
            var providerAuditInfo = new ProviderAuditInfo()
                {
                    ManagerName = txtManagerName.Text.Trim(),
                    ManagerCellphone = txtManagerPhone.Text.Trim(),
                    EmergencyContact = txtEmergencyContact.Text.Trim(),
                    EmergencyCall = txtEmergencyCall.Text.Trim(),
                    Province = address.ProvinceCode,
                    City = address.CityCode,
                    District = address.CountyCode,
                    Address = txtAddress.Text.Trim(),
                    ZipCode = txtPostCode.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Faxes = txtFaxes.Text.Trim(),
                    QQ = txtQQ.Text.Trim(),
                    EffectBeginTime = DateTime.Parse(txtBeginTime.Text),
                    EffectEndTime = DateTime.Parse(txtEndTime.Text),
                    IsUpgrade = isUpgrade,
                    CompanyName = lblCompanyName.Text,
                    AbbreviateName = lblAbbreviation.Text,
                    OfficePhones = lblCompanyPhone.Text,
                    OrginationCode = lblIdCode.Text,
                    OperatorAccount = this.CurrentUser.UserName
                };
            if (fupBusinessLicense.HasFile)
                providerAuditInfo.BusinessLicense = GetBytes(fupBusinessLicense);
            if (fupIATA.HasFile)
                providerAuditInfo.IATALicense = GetBytes(fupIATA);
            return providerAuditInfo;
        }
        //产品个人
        private SupplierIndividualAuditInfo GetSupplierIndividualAuditInfo(AddressInfo address, bool isUpgrade)
        {
            var supplierIndividual = new SupplierIndividualAuditInfo();
            supplierIndividual.Address = txtAddress.Text.Trim();
            supplierIndividual.Province = address.ProvinceCode;
            supplierIndividual.City = address.CityCode;
            supplierIndividual.District = address.CountyCode;
            supplierIndividual.Email = txtEmail.Text.Trim();
            supplierIndividual.Faxes = txtFaxes.Text.Trim();
            supplierIndividual.QQ = txtQQ.Text.Trim();
            supplierIndividual.ZipCode = txtPostCode.Text.Trim();
            supplierIndividual.BussinessTime = int.Parse(dropYear.Value);
            supplierIndividual.CertLicense = GetBytes(fupBusinessLicense);
            if (fupIATA.HasFile)
                supplierIndividual.IATALicense = GetBytes(fupIATA);
            supplierIndividual.EffectBeginTime = DateTime.Parse(txtBeginTime.Text);
            supplierIndividual.EffectEndTime = DateTime.Parse(txtEndTime.Text);
            supplierIndividual.IsUpgrade = isUpgrade;
            supplierIndividual.OperatorAccount = this.CurrentUser.UserName;
            supplierIndividual.OfficePhone = this.txtFixedPhone.Text.Trim();
            return supplierIndividual;

        }
        //产品企业
        private SupplierEnterpriseAuditInfo GetSupplierEnterpriseInfo(AddressInfo address, bool isUpgrade)
        {
            var supplierEnterprise = new SupplierEnterpriseAuditInfo();
            supplierEnterprise.ManagerName = txtManagerName.Text.Trim();
            supplierEnterprise.ManagerCellphone = txtManagerPhone.Text.Trim();
            supplierEnterprise.EmergencyContact = txtEmergencyContact.Text.Trim();
            supplierEnterprise.EmergencyCall = txtEmergencyCall.Text.Trim();
            supplierEnterprise.Province = address.ProvinceCode;
            supplierEnterprise.City = address.CityCode;
            supplierEnterprise.District = address.CountyCode;
            supplierEnterprise.Address = txtAddress.Text.Trim();
            supplierEnterprise.ZipCode = txtPostCode.Text.Trim();
            supplierEnterprise.Email = txtEmail.Text.Trim();
            supplierEnterprise.Faxes = txtFaxes.Text.Trim();
            supplierEnterprise.QQ = txtQQ.Text.Trim();
            supplierEnterprise.BussinessTime = int.Parse(dropYear.Value);
            supplierEnterprise.BussinessLicense = GetBytes(fupBusinessLicense);
            if (fupIATA.HasFile)
                supplierEnterprise.IATALicense = GetBytes(fupIATA);
            supplierEnterprise.EffectBeginTime = DateTime.Parse(txtBeginTime.Text);
            supplierEnterprise.EffectEndTime = DateTime.Parse(txtEndTime.Text);
            supplierEnterprise.AbbreviateName = lblAbbreviation.Text;
            supplierEnterprise.CompanyName = lblCompanyName.Text;
            supplierEnterprise.OfficePhones = lblCompanyPhone.Text;
            supplierEnterprise.OperatorAccount = this.CurrentUser.UserName;
            supplierEnterprise.OrginationCode = lblIdCode.Text;
            supplierEnterprise.IsUpgrade = isUpgrade;
            return supplierEnterprise;
        }
        //验证上传的图片
        private bool Verification(FileUpload fileupload)
        {
            bool IsVerfy = true;
            if (fileupload.Visible)
            {
                if (!fileupload.HasFile)
                {
                    IsVerfy = false;
                    throw new ArgumentNullException("请选择上传文件");
                }
                else
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileupload.FileName);
                    string extension = fileInfo.Extension.ToLower();
                    if (extension != ".jpg" && extension != ".png" && extension != ".bmp")
                    {
                        IsVerfy = false;
                        throw new InvalidOperationException("仅支持jpg、png、bmp格式且小于500KB的图片上传");
                    }
                    if (fileupload.PostedFile.ContentLength > 512000)
                    {
                        IsVerfy = false;
                        throw new InvalidProgramException("仅支持jpg、png、bmp格式且小于500KB的图片上传");
                    }
                }
            }
            return IsVerfy;
        }
        //上传图片
        public byte[] GetBytes(FileUpload fileupload)
        {
            int contentLength = fileupload.PostedFile.ContentLength;
            byte[] photo = new byte[contentLength];
            using (System.IO.BinaryReader br = new System.IO.BinaryReader(fileupload.FileContent))
            {
                br.Read(photo, 0, contentLength);
                return photo;
            }
        }
        private void ToExamine()
        {
            Guid id = Guid.Parse(Request.QueryString["CompanyId"]);
            CompanyType type = (CompanyType)byte.Parse(Request.QueryString["CompanyType"]);
            AccountBaseType accounType = (AccountBaseType)byte.Parse(Request.QueryString["AccountType"]);
            AuditType auditType = (AuditType)int.Parse(Request.QueryString["AuditType"]);
            var address = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressBaseInfo(hfldAddressCode.Value);
            bool isUpgrade = false;
            CompanyUpgrade companyUpgrade = null;
            if (auditType == AuditType.ApplyAudit)
            {
                isUpgrade = true;
                companyUpgrade = CompanyUpgradeService.QueryCompanyUpgrade(id);
            }
            if ((type == CompanyType.Provider&& !isUpgrade) || (isUpgrade && companyUpgrade.Type == CompanyType.Provider))
            {
                AccountCombineService.AuditProviderInfo(id, GetProviderAuditInfo(address, isUpgrade));
            }
            else
            {
                if (accounType == AccountBaseType.Individual)
                {
                    AccountCombineService.AuditSupplier(id, GetSupplierIndividualAuditInfo(address, isUpgrade));
                }
                else
                {
                    AccountCombineService.AuditSupplier(id, GetSupplierEnterpriseInfo(address, isUpgrade));
                }
            }
        }
        /// <summary>
        /// 审核
        /// </summary> 
        protected void btnPassed_Click(object sender, EventArgs e)
        {
            try
            {
                if (Verification(fupBusinessLicense))
                {
                    if (this.hfdValid.Value != "false")
                    {
                        if (!Verification(fupIATA))
                        {
                            return;
                        }
                    }
                    ToExamine();
                    RegisterScript(this, "alert('审核通过');window.location.href='./CompanyList.aspx?Search=Back';");
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "审核");
            }
        }
        //protected void btnRefuse_Click(object sender, EventArgs e)
        //{
            //try
            //{
            //    AuditType auditType = (AuditType)int.Parse(Request.QueryString["AuditType"]);
            //    if (auditType == AuditType.NormalAudit)
            //    {
            //        CompanyService.Reject(Guid.Parse(Request.QueryString["CompanyId"]), ,this.CurrentUser.UserName);
            //    }
            //    else
            //    {
            //        CompanyUpgradeService.Disable(Guid.Parse(Request.QueryString["CompanyId"]), this.CurrentUser.UserName);
            //    }
            //    RegisterScript(this, "alert('拒绝审核通过');window.location.href='./CompanyList.aspx?Search=Back';");
            //}
            //catch (Exception ex)
            //{
            //    ShowExceptionMessage(ex, "拒绝审核");
            //}
       // }
    }
}