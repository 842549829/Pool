using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class LookUpCompanyInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("register.css");
            RegisterOEMSkins("form.css");
            setBackButton();
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    Guid id = Guid.Parse(companyId);
                    this.hfdBussniess.Value = "LicenseQuery.aspx?type=bussiness&companyId=" + companyId;
                    this.hfdCertNo.Value = "LicenseQuery.aspx?type=certNo&companyId=" + companyId;
                    this.hfdIATA.Value = "LicenseQuery.aspx?type=iata&companyId=" + companyId;
                    this.BindCompanyInfo(CompanyService.GetCompanyDetail(id));
                    selEmployee.DataSource = from item in EmployeeService.QueryEmployees(CurrentCompany.CompanyId)
                                             where item.Enabled
                                             select new
                                             {
                                                 userName = item.UserName,
                                                 text = item.UserName + "-" + item.Name
                                             };
                    selEmployee.DataTextField = "text";
                    selEmployee.DataValueField = "userName";
                    selEmployee.DataBind();
                    selEmployee.Items.Insert(0, new ListItem()
                    {
                        Text = "",
                        Value = "",
                        Selected = true
                    });
                }
            }
        }
        #region 公司基本信息
        /// <summary>
        /// 绑定公司基本信息
        /// </summary>
        private void BindCompanyInfo(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription() + "(" + info.AccountType.GetDescription() + ")";

            this.lblLocation.Text = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.lblAddress.Text = info.Address;
            this.lblPostCode.Text = info.ZipCode;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkmanPhone.Text = info.ContactPhone;
            this.lblFaxes.Text = info.Faxes;
            this.lblRegisterTime.Text = info.RegisterTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblAudit.Text = info.AuditTime.HasValue ? info.AuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            this.lblIsOpenExternalInterface.Text = info.IsOpenExternalInterface ? "已启用" : "未启用";
            lblTuiguang.Text = info.CompanyType == CompanyType.Platform ? "" : !string.IsNullOrEmpty(info.OperatorAccount) ? info.OperatorAccount : !BasePage.HasPermission("/tuiguang.aspx") ? "" : "<a href='javascript:zhiding();'>业务员指派</a>";
            if (info.AccountType == AccountBaseType.Enterprise)
            {
                this.lblIndividual.Visible = false;
                this.lblCompanyName.Text = info.CompanyName;
                this.lblComapnyShortName.Text = info.AbbreviateName;
                this.lblCompanyPhone.Text = info.OfficePhones;
                this.lblPrincipal.Text = info.ManagerName;
                this.lblPrincipalPhone.Text = info.ManagerCellphone;
                this.lblUrgencyLinkman.Text = info.EmergencyContact;
                this.lblUrgencyLinkmanPhone.Text = info.EmergencyCall;
                this.lblOrginationCode.Text = info.OrginationCode;
            }
            else
            {
                this.fixedPhone.Visible = true;
                this.lblFixedPhone.Text = info.OfficePhones;
                this.lblCompany.Visible = false;
                this.tbBuyorOut.Visible = false;
                this.lblTrueName.Text = info.CompanyName;
                this.lblCertNo.Text = info.CertNo;
            }
            if (info.CompanyType != CompanyType.Purchaser && info.CompanyType != CompanyType.Platform)
            {
                this.exceptPurchase.Visible = true;
                this.lblBeginDeadline.Text = info.PeriodStartOfUse.Value.ToShortDateString();
                this.lblEndDeadline.Text = info.PeriodEndOfUse.Value.ToShortDateString();
                this.BindTime(info);
            }
            this.lblEmail.Text = info.ManagerEmail;
            this.lblQQ.Text = info.ManagerQQ;
            this.BindCompanyInfoByType(info);
        }
        /// <summary>
        /// 判断公司类型
        /// </summary>
        private void BindCompanyInfoByType(CompanyDetailInfo info)
        {
            this.BindCity(info.CompanyId);
            switch (info.CompanyType)
            {
                case CompanyType.Provider:  /*出票*/
                    BindProviderInfo(info);
                    break;
                case CompanyType.Purchaser: /*采购*/
                    BidPuchaserInfo(info);
                    break;
                case CompanyType.Supplier: /*产品*/
                    BindSupplierInfo(info);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 产品
        /// </summary>
        private void BindSupplierInfo(CompanyDetailInfo info)
        {
            if (info.AccountType == AccountBaseType.Individual)
                this.bussinessLicense.Visible = false;
            if (info.AccountType == AccountBaseType.Enterprise)
                this.certNo.Visible = false;
            this.tbRefundTime.Visible = false;
            this.lblEmail.Text = info.ContactEmail;
            this.lblQQ.Text = info.ContactQQ;
            this.hidType.Value = "Supplier";
            this.tbSupplierCompany.Visible = true;
            this.tbProviderCompany.Visible = false;
            this.BindPayAccount(info.CompanyId);
            this.BindPutAccoynt(info.CompanyId);
            this.BindWorkTime(info.CompanyId);
            this.BindSupplierUnitInfo(info.CompanyId);
        }
        /// <summary>
        /// 采购
        /// </summary>
        private void BidPuchaserInfo(CompanyDetailInfo info)
        {
            this.BindPayAccount(info.CompanyId);
            this.hidType.Value = "Purchaser";
            this.tbWorkTime.Visible = false;
            this.tbRefundTime.Visible = false;
            this.tbSupplierCompany.Visible = false;
            this.tbProviderCompany.Visible = false;
        }
        /// <summary>
        /// 出票
        /// </summary>
        private void BindProviderInfo(CompanyDetailInfo info)
        {
            this.tbProviderCompany.Visible = true;
            this.tbSupplierCompany.Visible = false;
            this.certNo.Visible = false;
            this.divOffice.InnerHtml = BindOffice(info.CompanyId);
            this.divPerson.InnerHtml = BindPerson(info.CompanyId);
            this.BindPayAccount(info.CompanyId);
            this.BindPutAccoynt(info.CompanyId);
            this.BindWorkTime(info.CompanyId);
            this.BindCrippledTime(info.CompanyId);
            this.BindProviderUnitInfo(info.CompanyId);
        }
        /// <summary>
        ///有效时间
        /// </summary>
        private void BindTime(CompanyInfo info)
        {
            this.lblBeginDeadline.Text = info.PeriodStartOfUse.HasValue ? info.PeriodStartOfUse.Value.ToString("yyyy-MM-dd") : string.Empty;
            this.lblEndDeadline.Text = info.PeriodEndOfUse.HasValue ? info.PeriodEndOfUse.Value.ToString("yyyy-MM-dd") : string.Empty;
        }
        #endregion
        /// <summary>
        /// 默认出发城市
        /// </summary>
        private void BindCity(Guid id)
        {
            WorkingSetting work = CompanyService.GetWorkingSetting(id);
            if (work != null)
            {
                this.lblDeparture.Text = work.DefaultDeparture;
                this.lblArrival.Text = work.DefaultArrival;
            }
        }
        /// <summary>
        /// 绑定OFFICE号
        /// </summary>
        private string BindOffice(Guid id)
        {
            this.hOffice.InnerText = "Office";
            StringBuilder builder = new StringBuilder(100);
            builder.AppendFormat("<table><tr><th>序号</th><th>OFFICE</th><tr>");
            var item = CompanyService.QueryOfficeNumbers(id).ToList();
            for (int i = 0; i < item.Count(); i++)
                builder.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", i + 1, item[i].Number);
            builder.Append("</table>");
            return builder.ToString();
        }
        /// <summary>
        /// 绑定负责人
        /// </summary>
        private string BindPerson(Guid id)
        {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("<table><colgroup><col class='w15'/><col class='w30'/><col class='w15'/><col class='w30'/></colgroup>");
            builder.Append("<tr><th>负责方向</th><th>负责人</th><th>手机</th><th>QQ</th></tr>");
            foreach (BusinessManager item in CompanyService.GetBusinessManagers(id))
                builder.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                    item.BusinessName, item.Mamanger, item.Cellphone, item.QQ);
            builder.Append("</table>");
            return builder.ToString();
        }
        /// <summary>
        /// 绑定工作时间
        /// </summary>
        private void BindWorkTime(Guid id)
        {
            WorkingHours work = CompanyService.GetWorkinghours(id);
            if (work != null)
            {
                this.lblWorkdayWorkStart.Text = work.WorkdayWorkStart.ToString("HH:mm");
                this.lblWorkdayWorkEnd.Text = work.WorkdayWorkEnd.ToString("HH:mm");
                this.lblRestdayWorkStart.Text = work.RestdayWorkStart.ToString("HH:mm");
                this.lblRestdayWorkEnd.Text = work.RestdayWorkEnd.ToString("HH:mm");
            }
        }
        /// <summary>
        /// 绑定废票时间
        /// </summary>
        private void BindCrippledTime(Guid id)
        {
            WorkingHours work = CompanyService.GetWorkinghours(id);
            if (work != null)
            {
                this.lblWorkdayRefundStart.Text = work.WorkdayRefundStart.ToString("HH:mm");
                this.lblWorkdayRefundEnd.Text = work.WorkdayRefundEnd.ToString("HH:mm");
                this.lblRestdayRefundStart.Text = work.RestdayRefundStart.ToString("HH:mm");
                this.lblRestdayRefundEnd.Text = work.RestdayRefundEnd.ToString("HH:mm");
            }
        }
        /// <summary>
        /// 付款账号
        /// </summary>
        private void BindPayAccount(Guid id)
        {
            var account = AccountService.Query(id, AccountType.Payment);
            if (account != null) this.lblPayAccount.Text = account.No;
        }
        /// <summary>
        /// 收款账号
        /// </summary>
        private void BindPutAccoynt(Guid id)
        {
            var account = AccountService.Query(id, AccountType.Receiving);
            if (account != null) this.lblPutAccount.Text = account.No;
        }
        /// <summary>
        /// 出票公司信息
        /// </summary>
        private void BindProviderUnitInfo(Guid id)
        {
            CompanySettingsInfo parameter = CompanyService.GetCompanySettingsInfo(id);
            if (parameter != null)
            {
                if (parameter.Parameter != null)
                {
                    this.chkAutoPrintBSP.Checked = parameter.Parameter.AutoPrintBSP;
                    this.chkAutoPrintB2B.Checked = parameter.Parameter.AutoPrintB2B;
                    this.chkCanReleaseVip.Checked = parameter.Parameter.CanReleaseVip;
                    this.chkCanHaveSubordinate.Checked = parameter.Parameter.CanHaveSubordinate;
                    this.chkAutoPlatformAudit1.Checked = parameter.Parameter.AutoPlatformAudit;
                    this.chkAllowBrotherPurchase.Checked = parameter.Parameter.AllowBrotherPurchase;

                    this.lblRefundCountLimit.Text = parameter.Parameter.RefundCountLimit.ToString();
                    this.lblRefundTimeLimit.Text = parameter.Parameter.RefundTimeLimit.ToString();
                    this.lblFullRefundTimeLimit.Text = parameter.Parameter.FullRefundTimeLimit.ToString();
                    this.lblProfessionRate.Text = (parameter.Parameter.ProfessionRate * 1000M).TrimInvaidZero();
                    this.lblSubordinateRate.Text = (parameter.Parameter.SubordinateRate * 1000M).TrimInvaidZero();
                    //this.lblSpecialRate.Text = (parameter.Parameter.SpecialRate * 1000M).TrimInvaidZero();

                    BindSpecialProduct(parameter.Parameter);
                }
                if (parameter.WorkingSetting != null)
                {
                    this.chkRefundNeedAudit.Checked = parameter.WorkingSetting.RefundNeedAudit;
                    this.lblOffice.Text = parameter.WorkingSetting.DefaultOfficeNumber;
                    if (parameter.WorkingSetting.RebateForChild.HasValue)
                    {
                        this.chkRebateForChild.Checked = true;
                        this.lblRebateForChild.Text = (parameter.WorkingSetting.RebateForChild.Value * 100M).TrimInvaidZero() + "%";
                        if (!string.IsNullOrEmpty(parameter.WorkingSetting.AirlineForChild))
                        {
                            foreach (string item in parameter.WorkingSetting.AirlineForChild.Split('/'))
                            {
                                ListItem listTiem = new ListItem(item.ToString());
                                listTiem.Selected = true;
                                this.chklAirlineForChild.Items.Add(listTiem);
                            }
                        }
                    }
                    else
                    {
                        chkRebateForChild.Visible = false;
                    }
                    if (!string.IsNullOrWhiteSpace(parameter.WorkingSetting.AirlineForDefault) && parameter.WorkingSetting.RebateForDefault.HasValue)
                    {
                        this.chkRebateForDefault.Checked = true;
                        this.lblRebateForDefault.Text = (parameter.WorkingSetting.RebateForDefault.Value * 100M).TrimInvaidZero() + "%";
                        if (!string.IsNullOrWhiteSpace(parameter.WorkingSetting.AirlineForDefault))
                        {
                            foreach (var item in parameter.WorkingSetting.AirlineForDefault.Split('/'))
                            {
                                ListItem listItem = new ListItem(item.ToString());
                                listItem.Selected = true;
                                this.chkAirlineForDefault.Items.Add(listItem);
                            }
                        }
                    }
                    else
                    {
                        chkRebateForDefault.Visible = false;
                    }
                }
                else
                {
                    chkRebateForChild.Visible = false;
                    chkRebateForDefault.Visible = false;
                }
            }
        }
        private void BindSpecialProduct(CompanyParameter paramter)
        {
            chkSingleness1.Checked = chkSingleness.Checked = paramter.Singleness && SpecialProductService.Query(SpecialProductType.Singleness).Enabled;
            lblSingleness1.Text = lblSingleness.Text = (paramter.SinglenessRate * 1000M).TrimInvaidZero();
            chkDisperse1.Checked = chkDisperse.Checked = paramter.Disperse && SpecialProductService.Query(SpecialProductType.Disperse).Enabled;
            lblDisperse1.Text = lblDisperse.Text = (paramter.DisperseRate * 1000M).TrimInvaidZero();
            chkCostFree.Checked = paramter.CostFree && SpecialProductService.Query(SpecialProductType.CostFree).Enabled;
            lblCostFree.Text = (paramter.CostFreeRate * 1000M).TrimInvaidZero();
            chkBloc1.Checked = chkBloc.Checked = paramter.Bloc && SpecialProductService.Query(SpecialProductType.Bloc).Enabled;
            lblBloc1.Text = lblBloc.Text = (paramter.BlocRate * 1000M).TrimInvaidZero();
            chkBusiness1.Checked = chkBusiness.Checked = paramter.Business && SpecialProductService.Query(SpecialProductType.Business).Enabled;
            lblBusiness1.Text = lblBusiness.Text = (paramter.BusinessRate * 1000M).TrimInvaidZero();
            chkOtherSpecial.Checked = paramter.OtherSpecial && SpecialProductService.Query(SpecialProductType.OtherSpecial).Enabled;
            lblOtherSpecialRate.Text = (paramter.OtherSpecialRate * 1000M).TrimInvaidZero();
            chkLowToHighSpecial.Checked = paramter.LowToHigh && SpecialProductService.Query(SpecialProductType.LowToHigh).Enabled;
            lblLowToHighSpecialRate.Text = (paramter.LowToHighRate * 1000M).TrimInvaidZero();
        }
        /// <summary>
        /// 产品公司信息
        /// </summary>
        private void BindSupplierUnitInfo(Guid id)
        {
            var parameter = CompanyService.GetCompanyParameter(id);
            if (parameter != null)
            {
                chkRebateForChild.Checked = true;
                // lblSpecialRate1.Text = (parameter.SpecialRate * 1000M).TrimInvaidZero();
                chkAutoPlatformAudit.Checked = parameter.AutoPlatformAudit;
                BindSpecialProduct(parameter);
            }
            else
            {
                chkRebateForChild.Visible = false;
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        private void setBackButton()
        {
            string returnUrl = Request.QueryString["returnUrl"] ?? Request.UrlReferrer.AbsoluteUri;
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "CompanyList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            btnGoBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string employee = hidEmployeeNo.Value;
            try
            {
                if (!BasePage.HasPermission("/tuiguang.aspx")) ShowMessage("你还没有没有权限指定推广者！");
                CompanyService.SetCompanyOperatorAccount(Guid.Parse(Request.QueryString["CompanyId"]), employee, CurrentUser.UserName);
                ShowMessage("设置成功！");
                this.BindCompanyInfo(CompanyService.GetCompanyDetail(Guid.Parse(Request.QueryString["CompanyId"])));
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(Page, ex, "设置指定推广人");
            }
        }
    }
}