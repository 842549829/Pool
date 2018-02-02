using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.Data.DataMapping;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain
{
    public partial class ProviderMaintain :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Guid id = this.CurrentCompany.CompanyId;
                this.BindCompanyInfo(CompanyService.GetCompanyDetail(id));
                this.BindCity(id);
            }
        }
        private void BindCompanyInfo(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblCompanyName.Text = info.CompanyName;
            this.lblCompanyShortName.Text = info.AbbreviateName;
            this.lblAddress.Text = AddressShow.GetAddressText(info.Area,info.Province,info.City,info.District);
            this.lblCompanyAddress.Text = info.Address;
            this.txtPostCode.Text = info.ZipCode;
            this.txtCompanyPhone.Text = info.OfficePhones;
            this.txtFaxes.Text = info.Faxes;
            this.lblPrincipal.Text = info.ManagerName;
            this.lblPrincipalPhone.Text = info.ManagerCellphone;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkManPhone.Text = info.ContactPhone;
            this.lblUrgencyLinkMan.Text = info.EmergencyContact;
            this.lblUrgencyLinkManPhone.Text = info.EmergencyCall;
            this.txtEmail.Text = info.ManagerEmail;
            this.txtMSN.Text = info.ManagerMsn;
            this.txtQQ.Text = info.ManagerQQ;
        }
        private void BindCity(Guid id)
        {
            WorkingSetting city = ReturnWorking(id);
            if (city != null)
            {
                this.Departure.Code = city.DefaultDeparture;
                this.Arrival.Code = city.DefaultArrival;
            }
        }
        private static WorkingSetting ReturnWorking(Guid id)
        {
            return CompanyService.GetWorkingSetting(id);
        }
        private CompanyInfo GetCompanyInfo()
        {
            AddressInfo info = AddressShow.GetAddressInfo(this.lblAddress.Text);
            return new CompanyInfo
            {
                CompanyType = Common.Enums.CompanyType.Purchaser,
                Area = info.AreaCode,
                Province = info.ProvinceCode,
                City = info.CityCode,
                District = info.CountyCode,
                CompanyId = this.CurrentCompany.CompanyId,
                ZipCode = this.txtPostCode.Text.Trim(),
                OfficePhones = this.txtCompanyPhone.Text.Trim(),
                Faxes = this.txtFaxes.Text.Trim(),
                ManagerEmail = this.txtEmail.Text.Trim(),
                ManagerMsn = this.txtMSN.Text.Trim(),
                ManagerQQ = this.txtQQ.Text.Trim(),
                Address = lblCompanyAddress.Text,
                Contact = this.lblPrincipal.Text,
                ContactPhone = this.lblPrincipalPhone.Text,
                EmergencyContact = this.lblUrgencyLinkMan.Text,
                EmergencyCall = this.lblUrgencyLinkManPhone.Text,
                ManagerName = this.lblPrincipal.Text,
                ManagerCellphone = this.lblPrincipalPhone.Text,
                CompanyName = this.lblCompanyName.Text,
                AbbreviateName = this.lblCompanyShortName.Text
            };
        }
        private WorkingSetting GetCitys()
        {
            return new Data.DataMapping.WorkingSetting
            {
                DefaultOfficeNumber = string.Empty,
                DefaultDeparture = this.Departure.Code,
                DefaultArrival = this.Arrival.Code,
                Company = this.CurrentCompany.CompanyId
            };
        }

        protected void btnSvaeCompanyInfo_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.Update(this.GetCompanyInfo());
                ShowMessage("修改成功");
            }
            catch (InvalidOperationException ex)
            {
                ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }

        protected void btnSaveChilder_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.SetWorkingSetting(this.GetCitys(),this.CurrentUser.UserName);
                ShowMessage("修改成功");
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex) { ShowExceptionMessage(ex, "修改"); }
        }
    }
}