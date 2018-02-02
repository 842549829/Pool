using System;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent
{
    public partial class AddInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            InitData();
        }
        private void InitData(){
            var info = CompanyService.GetCompanyDetail(CurrentCompany.CompanyId);
            if (info != null)
            {
                txtEmail.Value = info.ContactEmail;
                txtAddress.Value = info.Address;
                txtPostCode.Value = info.ZipCode;
                txtFaexs.Value = info.Faxes;
                txtQQ.Value = info.ContactQQ;
                BindAddress(info);
                BindContact(info);
            }
        }
        private void BindContact(DataTransferObject.Organization.CompanyDetailInfo info)
        {
            if (info.AccountType == Common.Enums.AccountBaseType.Enterprise)
            {
                txtManagerName.Value = info.ManagerName;
                txtManagerCellphone.Value = info.ManagerCellphone;
                txtEmergencyContact.Value = info.EmergencyContact;
                txtEmergencyCall.Value = info.EmergencyCall;
                this.fixedPhone.Visible = false;
            }
            else {
                this.txtFixedPhone.Value = info.OfficePhones;
                enterprise.Visible = false;
            }
        }
        private void BindAddress(DataTransferObject.Organization.CompanyDetailInfo info)
        {
            hfldAddressCode.Value = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressJson(info.Area, info.Province, info.City, info.District);
        }
    }
}