using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage
{
    public partial class RegisterProduct : BasePage
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SupplierCreatureInfo info = this.GetProviderCreatureInfo();
                CompanyService.Spread(this.CurrentCompany.CompanyId, info);
                SucceedInfo(info);
            }
            catch (InvalidOperationException ex)
            {
                ShowExceptionMessage(ex, "开户");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "开户");
            }
        }
        /// <summary>
        /// 成功信息
        /// </summary>
        private void SucceedInfo(SupplierCreatureInfo companyInfo)
        {
            Session["Info"] = companyInfo;
            Response.Redirect("./RegisterSucceed.aspx", false);
        }
        /// <summary>
        /// 失败信息
        /// </summary>
        private void FailInfo(string message)
        {
            
        }
        private SupplierCreatureInfo GetProviderCreatureInfo() 
        {
            AddressInfo address = AddressInfo.GetAddress(this.hidAddress.Value);
            if (address == null) throw new ArgumentNullException("请选择所在地");
            return new SupplierCreatureInfo
            {
                ProviderId = Guid.NewGuid(),
                UserName = this.txtAccountNo.Text.Trim(),
                UserPassword = this.txtPassWord.Text.Trim(),
                ConfirmPassword = this.txtConfirmPassWord.Text.Trim(),
                Name = this.txtUserName.Text.Trim(),
                NickName = this.txtPetName.Text.Trim(),
                Area = address.AreaCode,
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                Address = this.txtAddress.Text.Trim(),
                Contact = this.txtLinkman.Text.Trim(),
                ContactPhone = this.txtLinkManPhone.Text.Trim(),
                Email = this.txtEmail.Text.Trim(),
                ZipCode = this.txtPostCode.Text.Trim(),
                Faxes = this.txtFaxes.Text.Trim(),
                MSN = this.txtMSN.Text.Trim(),
                QQ = this.txtQQ.Text.Trim(),
                CompanyType = Common.Enums.CompanyType.Supplier,
                PeriodStartOfUse = DateTime.Today.Date,
                PeriodEndOfUse = DateTime.Today.AddYears(ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultUseLimit).Date
            };
        }
    }
}