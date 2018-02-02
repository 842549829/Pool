using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain
{
    public partial class ProductMaintain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindCompanyInfo(CompanyService.GetCompanyDetail(this.CurrentCompany.CompanyId));
                this.BindAirline();
                this.BindCity();
            }
        }
        private void BindCompanyInfo(CompanyDetailInfo info) 
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblUserName.Text = info.CompanyName;
            this.lblPetName.Text = info.AbbreviateName;
            this.lblAddress.Text =AddressShow.GetAddressText(info.Area,info.Province,info.City,info.District);
            this.lblUserAddress.Text = info.Address;
            this.txtPostCode.Text = info.ZipCode;
            this.txtFaxes.Text = info.Faxes;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkmanPhone.Text = info.ContactPhone;
            this.txtEmail.Text = info.ContactEmail;
            this.txtQQ.Text = info.ContactQQ;
            this.txtMSN.Text = info.ContactMSN;
            this.lblBeginDeadline.Text = info.PeriodStartOfUse.HasValue?info.PeriodStartOfUse.Value.ToString("yyyy-MM-dd"):string.Empty;
            this.lblEndDeadline.Text = info.PeriodEndOfUse.HasValue?info.PeriodEndOfUse.Value.ToString("yyyy-MM-dd"):string.Empty;
        }
        /// <summary>
        /// 获取可提供资源的航空公司
        /// </summary>
        private void BindAirline()
        {
            foreach (string item in PolicySetService.QueryAirlines(this.CurrentCompany.CompanyId))
            {
                ListItem lists = new ListItem(item.ToString());
                lists.Selected = true;
                this.chklAirline.Items.Add(lists);
            }
        }
        /// <summary>
        ///默认出发到达城市
        /// </summary>
        private void BindCity()
        {
            WorkingSetting city = CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId);
            if (city != null)
            {
                this.Departure.Code = city.DefaultDeparture;
                this.Arrival.Code = city.DefaultArrival;
            }
        }
        private WorkingSetting GetWorkingSetting() {
            return new WorkingSetting {
                DefaultOfficeNumber = string.Empty, 
                Company = this.CurrentCompany.CompanyId,
                DefaultDeparture = this.Departure.Code,
                DefaultArrival = this.Arrival.Code
            };
        }
        private SupplierCreatureInfo GetProviderCreatureInfo()
        {
            AddressInfo info = AddressShow.GetAddressInfo(this.lblAddress.Text);
            return new  SupplierCreatureInfo {
                 CompanyType = Common.Enums.CompanyType.Supplier,
                 Area = info.AreaCode,
                 Province = info.ProvinceCode,
                 City = info.CityCode,
                 District= info.CountyCode,
                 Address = this.lblUserAddress.Text,
                 Name = this.lblUserName.Text,
                 NickName = this.lblPetName.Text,
                 ZipCode = this.txtPostCode.Text.Trim(),
                 QQ = this.txtQQ.Text.Trim(),
                 MSN = this.txtMSN.Text.Trim(),
                 Faxes = this.txtFaxes.Text.Trim(),
                 Email = this.txtEmail.Text.Trim(),
                 Contact = this.lblLinkman.Text,
                 ContactPhone = this.lblLinkmanPhone.Text,
                 ProviderId = this.CurrentCompany.CompanyId
            };
        }
        protected void btnSaveCompanyInfo_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.Update(this.GetProviderCreatureInfo());
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
                CompanyService.SetWorkingSetting(this.GetWorkingSetting(),this.CurrentUser.UserName);
                ShowMessage("修改成功");
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }
    }
}