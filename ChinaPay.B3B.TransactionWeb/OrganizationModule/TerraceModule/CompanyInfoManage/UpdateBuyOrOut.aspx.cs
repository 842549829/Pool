using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
using System.Reflection;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class UpdateBuyOrOut : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    CompanyDetailInfo info = CompanyService.GetCompanyDetail(Guid.Parse(companyId));
                    this.BindCompanyInfo(info);
                }
            }
        }
        private void BindCompanyInfo(CompanyInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription() + "(" + info.AccountType.GetDescription() + ")";
            this.txtCompanyName.Text = info.CompanyName;
            this.txtCompanyShortName.Text = info.AbbreviateName;
            this.hidAddress.Value = AddressShow.GetAddressJson(info.Area, info.Province, info.City, info.District);
            this.lblBindLocation.InnerText = AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.txtAddress.Text = info.Address;
            this.txtPostCode.Text = info.ZipCode;
            this.txtCompanyPhone.Text = info.OfficePhones;
            this.txtFaxes.Text = info.Faxes;
            this.txtPrincipal.Text = info.ManagerName;
            this.txtPrincipalPhone.Text = info.ManagerCellphone;
            this.txtLinkman.Text = info.Contact;
            this.txtLinkManPhone.Text = info.ContactPhone;
            this.txtUrgencyLinkMan.Text = info.EmergencyContact;
            this.txtUrgencyLinkManPhone.Text = info.EmergencyCall;
            this.txtEmail.Text = info.ManagerEmail;
            this.txtQQ.Text = info.ManagerQQ;
        }
        private CompanyInfo GetCompanyInfo() 
        {
            AddressInfo address = AddressInfo.GetAddress(this.hidAddress.Value);
            if (address == null || string.IsNullOrEmpty(address.CountyCode)) throw new ArgumentNullException("请选择所地");
            string companyId = Request.QueryString["CompanyId"];
            CompanyType companyType = (CompanyType)byte.Parse(Request.QueryString["CompanyType"]);
            AccountBaseType accountType = (AccountBaseType)byte.Parse(Request.QueryString["AccountType"]);
            return new CompanyInfo
            { 
                CompanyId = Guid.Parse(companyId),
                CompanyType = companyType,
                AccountType = accountType,
                CompanyName = this.txtCompanyName.Text.Trim(),
                Area = address.AreaCode,
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                AbbreviateName = this.txtCompanyShortName.Text.Trim(),
                ZipCode = this.txtPostCode.Text.Trim(),
                Address = this.txtAddress.Text.Trim(),
                Faxes = this.txtFaxes.Text.Trim(),
                OfficePhones = this.txtCompanyPhone.Text.Trim(),
                ManagerEmail = this.txtEmail.Text.Trim(),
                ManagerName = this.txtPrincipal.Text.Trim(),
                ManagerCellphone = this.txtPrincipalPhone.Text.Trim(),
                Contact = this.txtLinkman.Text.Trim(),
                ContactPhone = this.txtLinkManPhone.Text.Trim(),
                EmergencyContact = this.txtUrgencyLinkMan.Text.Trim(),
                EmergencyCall = this.txtUrgencyLinkManPhone.Text.Trim(),
                ManagerQQ = this.txtQQ.Text.Trim(),
            };
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.Update(this.GetCompanyInfo());
                Response.Redirect("./CompanyList.aspx?Search=Back", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "失败");
            }
        }
    }
}