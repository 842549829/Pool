using System;
using System.Reflection;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Register
{
    public partial class Provider : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindHasClientType();
                this.BindHowToKnow();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SupplierRegistrationInfo info = this.GetProviderInfo();
                ExistsVerifyCode();
                CompanyService.Register(info);
                this.SucceedInfo(info);
            }
            catch (ArgumentNullException ex) { this.FailInfo(ex.Message); }
            catch (InvalidOperationException ex) { this.FailInfo(ex.Message); }
            catch (Exception ex) { this.FailInfo(ex.Message);  /*this.FailInfo("系统异常请稍后再试...");*/ }
        }
        /// <summary>
        /// 成功信息
        /// </summary>
        private void SucceedInfo(SupplierRegistrationInfo info)
        {
            Session["Info"] = info;
            Response.Redirect("./Succeed.aspx", false);
        }
        /// <summary>
        /// 失败信息
        /// </summary>
        private void FailInfo(string message)
        {
            this.lblMessage.InnerText = message;
        }
        /// <summary>
        /// 获取数据信息
        /// </summary>
        /// <returns></returns>
        private SupplierRegistrationInfo GetProviderInfo()
        {
            AddressInfo address = AddressInfo.GetAddress(this.hidAddress.Value);
            if (address == null) throw new ArgumentNullException("请选择所在地");
            SupplierRegistrationInfo info = new SupplierRegistrationInfo()
            {
                ProviderId = Guid.NewGuid(),
                CompanyType = Common.Enums.CompanyType.Supplier,
                UserName = this.txtAccountNo.Text.Trim(),
                UserPassword = this.txtPassWord.Text.Trim(),
                ConfirmPassword = this.txtConfirmPassWord.Text.Trim(),
                Name =this.txtUserName.Text.Trim(),
                NickName = this.txtPetName.Text.Trim(),
                Area = address.AreaCode,
                Province = address.ProvinceCode,
                City = address.CityCode,
                District = address.CountyCode,
                Address =this.txtAddress.Text.Trim(),
                Contact = this.txtLinkman.Text.Trim(),
                ContactPhone = this.txtLinkManPhone.Text.Trim(),
                Email = this.txtEmail.Text.Trim(),
                Faxes = this.txtFaxes.Text.Trim(),
                MSN = this.txtMSN.Text.Trim(),
                QQ = this.txtQQ.Text.Trim(),
                ZipCode = this.txtPostCode.Text.Trim(),
                HasClientType = (HasClientType)Convert.ToInt32(this.rdolHasClientType.SelectedValue),
                PeriodStartOfUse = DateTime.Today.Date,
                PeriodEndOfUse = DateTime.Today.AddYears(ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultUseLimit).Date
            };
            HowToKnow howToknow = (HowToKnow)Convert.ToInt32(this.rdolHowToKnow.SelectedValue);
            if (howToknow == HowToKnow.Recommend) info.Recommender = this.txtMarket.Text.Trim();
            return info;
        }
        private void BindHasClientType()
        {
            FieldInfo[] fieldInfo = typeof(HasClientType).GetFields();
            foreach (var item in fieldInfo)
            {
                if (!item.IsSpecialName)
                {
                    HasClientType obj = (HasClientType)item.GetRawConstantValue();
                    this.rdolHasClientType.Items.Add(new ListItem(obj.GetDescription(), item.GetRawConstantValue().ToString()));
                }
            }
            this.rdolHasClientType.Items[0].Selected = true;
        }
        private void BindHowToKnow()
        {
            FieldInfo[] fieldInfo = typeof(HowToKnow).GetFields();
            foreach (var item in fieldInfo)
            {
                if (!item.IsSpecialName)
                {
                    HowToKnow obj = (HowToKnow)item.GetRawConstantValue();
                    this.rdolHowToKnow.Items.Add(new ListItem(obj.GetDescription(), item.GetRawConstantValue().ToString()));
                }
            }
            this.rdolHowToKnow.Items[0].Selected = true;
        }
        private void ExistsVerifyCode()
        {
            var verifyCode = Session["agent"];
            if (verifyCode == null) throw new ArgumentNullException("验证已经过期,请重刷新页面");
            if ((verifyCode.ToString()).ToUpper() != (this.txtCode.Text.Trim()).ToUpper()) throw new InvalidOperationException("验证码格式错误");
        }
    }
}