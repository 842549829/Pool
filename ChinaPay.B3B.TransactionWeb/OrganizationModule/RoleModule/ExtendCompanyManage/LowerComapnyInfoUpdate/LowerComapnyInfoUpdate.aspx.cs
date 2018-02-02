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

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate
{
    public partial class LowerComapnyInfoUpdate :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    this.BindCompanyInfo(CompanyService.GetCompanyDetail(Guid.Parse(companyId)));
                }
            }
        }
        private void BindCompanyInfo(CompanyDetailInfo info) 
        {
            string type  = Request.QueryString["Type"];
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = !string.IsNullOrWhiteSpace(type) && type == "Organization" ? "内部机构" : "下级采购";
            this.lblCompanyName.Text = info.CompanyName;
            this.lblCompanyShortName.Text = info.AbbreviateName;
            this.lblAddress.Text = info.Address;
            this.lblLoaction.Text = AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.lblCompanyPhone.Text = info.OfficePhones;
            this.lblPrincipal.Text = info.ManagerName;
            this.lblPrincipalPhone.Text = info.ManagerCellphone;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkmanPhone.Text = info.ContactPhone;
            this.lblUrgencyLinkMan.Text = info.EmergencyContact;
            this.lblUrgencyLinkManPhone.Text = info.EmergencyCall;
            if (Request.QueryString["Editable"] != null && Request.QueryString["Editable"] == "True")
                BindUpdate(info);
            else
                BindLookUp(info);
        }

        private void BindLookUp(CompanyDetailInfo info)
        {
            this.tbUpdate.Visible = false;
            this.txtBtnSave.Visible = false;
            this.lbllPostCode.Text = info.ZipCode;
            this.lbllMsn.Text = info.ManagerMsn;
            this.lbllFasex.Text = info.Faxes;
            this.lbllQQ.Text = info.ManagerQQ;
            this.lbllEmail.Text = info.ManagerEmail;
        }

        private void BindUpdate(CompanyDetailInfo info)
        {
            this.tbLookUp.Visible = false;
            this.txtFaxes.Text = info.Faxes;
            this.txtPostCode.Text = info.ZipCode;
            this.lblEmial.Text = info.ManagerEmail;
            this.txtQQ.Text = info.ManagerQQ;
            this.txtMSN.Text = info.ManagerMsn;
        }
        private CompanyInfo GetCompanyInfo() {
            AddressInfo address = AddressShow.GetAddressInfo(this.lblLoaction.Text);
            Guid company = Guid.Empty;
            string companyId = Request.QueryString["CompanyId"];
            if (!string.IsNullOrWhiteSpace(companyId))
            {
               company = Guid.Parse(companyId);
            }
            return new CompanyInfo {
                 CompanyType = Common.Enums.CompanyType.Purchaser,
                 CompanyId = company,
                 CompanyName = this.lblCompanyName.Text,
                 AbbreviateName = this.lblCompanyShortName.Text,
                 Area = address.AreaCode,
                 Province = address.ProvinceCode,
                 City = address.CityCode,
                 District = address.CountyCode,
                 Address = this.lblAddress.Text,
                 ZipCode = this.txtPostCode.Text,
                 OfficePhones = this.lblCompanyPhone.Text,
                 Faxes = this.txtFaxes.Text,
                 ManagerName = this.lblPrincipal.Text,
                 ManagerCellphone = this.lblPrincipalPhone.Text,
                 Contact = this.lblLinkman.Text,
                 ContactPhone = this.lblLinkmanPhone.Text,
                 EmergencyContact = this.lblUrgencyLinkMan.Text,
                 EmergencyCall = this.lblUrgencyLinkManPhone.Text,
                 ManagerEmail = this.lblEmial.Text,
                 ManagerQQ = this.txtQQ.Text,
                 ManagerMsn = this.txtMSN.Text
            };
        }

        protected void txtBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.Update(this.GetCompanyInfo());
                ShowMessage("修改成功");
                Response.Redirect("./Lower_manage.aspx", false);
            }
            catch (InvalidOperationException ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }
    }
}