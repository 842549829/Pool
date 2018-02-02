using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class DistributionOEMUserUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                string incomeGroupId = Request.QueryString["IncomeGroupId"];
                string strAccountType = Request.QueryString["AccountType"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    this.addGroup.HRef = "IncomeGroupAdd.aspx?CompanyId=" + companyId + "&IncomeGroupId=" + incomeGroupId + "&AccountType="+strAccountType;
                    DistribtionOEMUserCompanyDetailInfo info = DistributionOEMService.QueryDistributionOEMUserDetailInfo(Guid.Parse(companyId));
                    if(info != null)
                      BindCompanyInfo(info);
                }
            }
        }
        private void BindCompanyInfo(DistribtionOEMUserCompanyDetailInfo info)
        {
            this.ddlIncomeGroup.DataSource = IncomeGroupService.QueryIncomeGroup(this.CurrentCompany.CompanyId, null);
            this.ddlIncomeGroup.DataTextField = "Name";
            this.ddlIncomeGroup.DataValueField = "Id";
            this.ddlIncomeGroup.DataBind();
            this.ddlIncomeGroup.Items.Insert(0, new ListItem("-请选择-", ""));
            string incomeGroupId = Request.QueryString["IncomeGroupId"];
            if(info.IncomeGroupId.HasValue)
               this.ddlIncomeGroup.SelectedValue = info.IncomeGroupId.Value.ToString();
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
                fixedPhoneTitle.Visible = true;
                fixedPhoneValue.Visible = true;
                this.txtFixedPhone.Text = info.OfficePhones;
                this.txtPresonName.Text = info.CompanyName;
                this.txtIdCard.Text = info.CertNo;
                enterpriseContact.Visible = false;
                enterpriseinfo.Visible = false;
            }
        }
        private DistributionOEMUserIndividualUpdateInfo GetIndividualInfo(AddressInfo address)
        {
            var individual = new DistributionOEMUserIndividualUpdateInfo
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
            string incomeGroupId = Request.QueryString["IncomeGroupId"];
            if (!string.IsNullOrWhiteSpace(incomeGroupId))
                individual.OrginalIncomeGroupId = Guid.Parse(incomeGroupId);
            if (!string.IsNullOrWhiteSpace(this.ddlIncomeGroup.SelectedValue))
                individual.IncomeGroupId = Guid.Parse(this.ddlIncomeGroup.SelectedValue);
            return individual;
        }
        private DistributionOEMUserEnterpriseUpdateInfo GetEnterpriseInfo(AddressInfo address)
        {
            var enterprise = new DistributionOEMUserEnterpriseUpdateInfo
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
            string incomeGroupId = Request.QueryString["IncomeGroupId"];
            if (!string.IsNullOrWhiteSpace(incomeGroupId))
                enterprise.OrginalIncomeGroupId = Guid.Parse(incomeGroupId);
            if (!string.IsNullOrWhiteSpace(this.ddlIncomeGroup.SelectedValue))
                enterprise.IncomeGroupId = Guid.Parse(this.ddlIncomeGroup.SelectedValue);
            return enterprise;
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
                RegisterScript("alert('修改成功！');window.location.href='DistributionOEMUserList.aspx';", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }
    }
}