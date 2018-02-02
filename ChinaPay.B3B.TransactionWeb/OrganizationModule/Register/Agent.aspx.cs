﻿using System;
using System.Reflection;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Register
{
    public partial class Agent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindHasClientType();
                this.BindHowToKnow();
                this.BindBusinessType();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyRegistrationInfo info = this.GetProviderInfo();
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
        private void SucceedInfo(CompanyRegistrationInfo info)
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
        private CompanyRegistrationInfo GetProviderInfo()
        {
            AddressInfo address = AddressInfo.GetAddress(this.hidAddress.Value);
            if (address==null) throw new InvalidOperationException("请选择所在地");
            CompanyRegistrationInfo info = new CompanyRegistrationInfo()
            {
                 CompanyType = CompanyType.Provider,
                 CompanyId = Guid.NewGuid(),
                 UserName = this.txtAccountNo.Text.Trim(),
                 UserPassword = this.txtPassWord.Text.Trim(),
                 ConfirmPassword = this.txtConfirmPassWord.Text.Trim(),
                 CompanyName = this.txtCompanyName.Text.Trim(),
                 AbbreviateName = this.txtCompanyShortName.Text.Trim(),
                 Area =address.AreaCode,
                 Province =address.ProvinceCode,
                 City = address.CityCode,
                 District = address.CountyCode,
                 Address = this.txtAddress.Text.Trim(),
                 OfficePhones = this.txtCompanyPhone.Text.Trim(),
                 ManagerName = this.txtPrincipal.Text.Trim(),
                 ManagerCellphone = this.txtPrincipalPhone.Text.Trim(),
                 Contact = this.txtLinkman.Text.Trim(),
                 ContactPhone = this.txtLinkManPhone.Text.Trim(),
                 EmergencyContact = this.txtUrgencyLinkMan.Text.Trim(),
                 EmergencyCall = this.txtUrgencyLinkManPhone.Text.Trim(),
                 ManagerEmail = this.txtEmail.Text.Trim(),
                 Faxes = this.txtFaxes.Text.Trim(),
                 ManagerMsn = this.txtMSN.Text.Trim(),
                 ManagerQQ = this.txtQQ.Text.Trim(),
                 ZipCode = this.txtPostCode.Text.Trim(),
                 Licence = this.txtIATABusinessApprovalNumber.Text.Trim(),
                 IATA = this.txtIATANumber.Text.Trim(),
                 OfficeNumbers = this.txtOFFICENumber.Text.Trim().ToUpper(),
                 Deposit =decimal.Parse(this.txtCaticAssociationSuch.Text.Trim()),
                 HasClientType = (HasClientType)Convert.ToInt32(this.rdolHasClientType.SelectedValue),
                 PeriodStartOfUse = DateTime.Today.Date,
                 PeriodEndOfUse = DateTime.Today.AddYears(ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultUseLimit).Date
            };
            for (int i = 0; i < this.chklBusinessType.Items.Count; i++)
                if (this.chklBusinessType.Items[i].Selected)
                    info.BusinessType |= (BusinessType)Convert.ToInt32(this.chklBusinessType.Items[i].Value);
            info.HowToKnow = (HowToKnow)Convert.ToInt32(this.rdolHowToKnow.SelectedValue);
            if (info.HowToKnow == HowToKnow.Recommend) info.Recommender = this.txtMarket.Text.Trim();
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
        private void BindHowToKnow() {
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
        private void BindBusinessType() {

            FieldInfo[] fieldInfo = typeof(BusinessType).GetFields();
            foreach (var item in fieldInfo)
            {
                if (!item.IsSpecialName)
                {
                    BusinessType obj = (BusinessType)item.GetRawConstantValue();
                    this.chklBusinessType.Items.Add(new ListItem(obj.GetDescription(), item.GetRawConstantValue().ToString()));
                }
            }
            this.chklBusinessType.Items[0].Selected = true;
        }
        private void ExistsVerifyCode() {
            var verifyCode = Session["agent"];
            if (verifyCode == null)throw new ArgumentNullException("验证已经过期,请重刷新页面");
            if ((verifyCode.ToString()).ToUpper() != (this.txtCode.Text.Trim()).ToUpper() )throw new InvalidOperationException("验证码格式错误");
        }
    }
}