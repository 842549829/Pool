using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class UpdateProduct : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId)) this.BingInfo(CompanyService.GetCompanyDetail(Guid.Parse(companyId)));
            }
        }
        private void BingInfo(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.txtUserName.Text = info.CompanyName;
            this.txtPetName.Text = info.AbbreviateName;
            this.hidAddress.Value = AddressShow.GetAddressJson(info.Area, info.Province, info.City, info.District);
            this.lblBindLocation.InnerText = AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.txtAddress.Text = info.Address;
            this.txtPostCode.Text = info.ZipCode;
            this.txtFaxes.Text = info.Faxes;
            this.txtLinkman.Text = info.Contact;
            this.txtLinkManPhone.Text = info.ContactPhone;
            this.txtEmail.Text = info.ContactEmail;
           // this.txtMSN.Text = info.ContactMSN;
            this.txtQQ.Text = info.ContactQQ;

        }
        private SupplierCreatureInfo GetProviderCreatureInfo()
        {
            AddressInfo address = AddressInfo.GetAddress(this.hidAddress.Value);
            if (address == null) throw new ArgumentNullException("请选择所在地");
            return new SupplierCreatureInfo
            {
                 CompanyType =  (Common.Enums.CompanyType)int.Parse(Request.QueryString["CompanyType"]),
                ProviderId = Guid.Parse(Request.QueryString["CompanyId"]),
                Name = this.txtUserName.Text.Trim(),
                NickName = this.txtPetName.Text.Trim(),
                Area = address.AreaCode,
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                Address = this.txtAddress.Text.Trim(),
                ZipCode = this.txtPostCode.Text.Trim(),
                Faxes = this.txtFaxes.Text.Trim(),
                Contact = this.txtLinkman.Text.Trim(),
                ContactPhone = this.txtLinkManPhone.Text.Trim(),
                Email = this.txtEmail.Text.Trim(),
              //  MSN = this.txtMSN.Text.Trim(),
                QQ = this.txtQQ.Text.Trim()
            };
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.Update(this.GetProviderCreatureInfo());
                Response.Redirect("./CompanyList.aspx?Search=Back", true);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }
       
    }
}