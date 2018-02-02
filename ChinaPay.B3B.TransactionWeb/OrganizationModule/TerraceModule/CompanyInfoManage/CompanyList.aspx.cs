using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.PoolPay.Service;
using PoolPay.DataTransferObject;
using LogService = ChinaPay.B3B.Service.LogService;
using Pager = ChinaPay.B3B.TransactionWeb.UserControl.Pager;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class CompanyList : BasePage
    {
        private const string DateFromat = "yyyy-MM-dd";

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                initData();
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void initData()
        {
            var companyAuditStatus = Enum.GetValues(typeof(CompanyAuditStatus)) as CompanyAuditStatus[];
            foreach (var item in companyAuditStatus)
            {
                this.ddlCompanyAuditStatus.Items.Add(new ListItem(item.GetDescription(),((byte)item).ToString()));
            }
            this.ddlCompanyAuditStatus.Items.Insert(0,new ListItem("全部",""));
            #region LoadCompanyType

            IEnumerable<KeyValuePair<int, string>> companyTypes = GetCompanyTypes();
            ddlCompanyType.DataTextField = "Value";
            ddlCompanyType.DataValueField = "Key";
            ddlCompanyType.DataSource = companyTypes;
            ddlCompanyType.DataBind();

            #endregion

            #region LoadCompanyStatus

            //IEnumerable<KeyValuePair<int, string>> companyStatus = GetCompanyStatuss();
            //ddlStatus.DataTextField = "Value";
            //ddlStatus.DataValueField = "Key";
            //ddlStatus.DataSource = companyStatus;
            //ddlStatus.DataBind();

            #endregion

            LoadCondition("CompanyList");
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = newPage,
                    GetRowCount = true
                };
            queryCompanys(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex=1,
                    GetRowCount = true
                };
            queryCompanys(pagination);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                string companyId = hfdCompanyId.Value;
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    CompanyDetailInfo companyDetail = CompanyService.GetCompanyDetail(Guid.Parse(companyId));
                    if (companyDetail != null)
                    {
                        var merchant = new MerchantDTO
                            {
                                AccountNo = companyDetail.UserName,
                                AdministorName = companyDetail.Contact,
                                ContactPhone = companyDetail.ContactPhone,
                                Email = companyDetail.ContactEmail,
                                LegalPerson = companyDetail.Contact,
                                LegalPhone = companyDetail.ContactPhone,
                                LoginPassword = companyDetail.UserPassword,
                                MerchantNo = companyDetail.UserName,
                                Rate = decimal.Parse(txtPosRate.Text.Trim())/100
                            };
                        if (companyDetail.AccountType == AccountBaseType.Individual)
                        {
                            merchant.CompanyName = companyDetail.Contact + "分销";
                            merchant.OrganizationCode = "12345678-9";
                            merchant.LegalCarID = companyDetail.CertNo;
                        }
                        else
                        {
                            merchant.CompanyName = companyDetail.CompanyName;
                            merchant.OrganizationCode = companyDetail.OrginationCode;
                            merchant.LegalCarID = txtContactCertNo.Text.Trim();
                        }
                        try
                        {
                            MerchantBaseService.MerchantOpening(merchant);
                            var item = new OperationLog(OperationModule.单位,
                                OperationType.Insert,
                                CurrentUser.UserName,
                                OperatorRole.Platform,
                                merchant.AccountNo,
                                string.Format("将账号{0}设为商户", merchant.AccountNo));
                            LogService.SaveOperationLog(item);
                            ShowMessage("设为商户成功");
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessage(ex, "设为商户");
                        }
                    }
                }
            }
        }

        private void queryCompanys(Pagination pagination)
        {
            try
            {
                var  companys = CompanyService.GetCompanies(getCondition(),pagination);
                datalist.DataSource = companys.Select(company => new
                    {
                        company.CompanyType,
                        CompanyTypeValue = (byte) company.CompanyType,
                        AccountTypeValue = (byte) company.AccountType,
                        CompanyTypeText = company.CompanyType.GetDescription(),
                        company.AbbreviateName,
                        company.Contact,
                        company.UserNo,
                        AuditTime = company.AuditTime.HasValue ? company.AuditTime.Value.ToString(DateFromat) : string.Empty,
                        AuditedState = company.CompanyType == CompanyType.Purchaser ? string.Empty : company.AuditTime.HasValue ? company.Audited ? "审核通过" : "审核拒绝" : "未审",
                        company.Enabled,
                        company.CompanyId,
                        AccountType = company.AccountType.GetDescription(),
                        LastLoginTime = company.LastLoginTime.HasValue?company.LastLoginTime.Value.ToString():string.Empty,
                        RegisterTime = company.RegisterTime.ToString(),
                        company.IsOem,
                        SerachCondition = hfdSearchCondition.Value
                    });
                datalist.DataBind();
                if (companys.Any())
                {
                    pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private CompanyQueryParameter getCondition()
        {
            var parameter = new CompanyQueryParameter();
            parameter.AbbreviateName = txtAbbreviateName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(ddlCompanyType.SelectedValue))
                parameter.Type = (CompanyType) byte.Parse(ddlCompanyType.SelectedValue);
            parameter.UserNo = txtAccount.Text.Trim();
            if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
                parameter.Enabled = ddlStatus.SelectedValue == "1";
            if (!string.IsNullOrEmpty(ddlAccountType.SelectedValue))
                parameter.AccountType = (AccountBaseType) byte.Parse(ddlAccountType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(ddlCompanyAuditStatus.SelectedValue))
            {
                parameter.CompanyAuditStatus = (CompanyAuditStatus)int.Parse(ddlCompanyAuditStatus.SelectedValue);
            }
            parameter.Contact = txtContact.Text.Trim();
            return parameter;
        }

        //protected void ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "Disable")
        //    {
        //        try
        //        {
        //            var companyId = new Guid(e.CommandArgument.ToString());
        //            PolicyType policyType = Service.Policy.PolicyManageService.CheckIfHasDefaultPolicy(companyId);
        //            if (PolicyType.Unknown != policyType) throw new CustomException("该供应商在平台存在默认政策指向，请调整后再行操作");
        //            if (CompanyService.Disable(companyId, ,CurrentUser.UserName))
        //            {
        //                ShowMessage("禁用成功");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowExceptionMessage(ex, "禁用");
        //        }
        //    }

        //    if (e.CommandName == "Enable")
        //    {
        //        try
        //        {
        //            var companyId = new Guid(e.CommandArgument.ToString());
        //            if (CompanyService.Enable(companyId, CurrentUser.UserName))
        //            {
        //                ShowMessage("启用成功");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowExceptionMessage(ex, "启用");
        //        }
        //    }

        //    var pagination = new Pagination
        //        {
        //            PageSize = pager.PageSize,
        //            PageIndex = pager.CurrentPageIndex,
        //            GetRowCount = true
        //        };
        //    queryCompanys(pagination);
        //}

        /// <summary>
        /// 获取所有的公司类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetCompanyTypes()
        {
            var companyTypes = Enum.GetValues(typeof (CompanyType)) as CompanyType[];
            if (companyTypes == null) return null;
            return companyTypes.Where(item => item != CompanyType.Platform).Select(item => new KeyValuePair<int, string>((int) item, item.GetDescription()));
        }

        /// <summary>
        /// 获取所有的公司状态
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetCompanyStatuss()
        {
            var companyTypes = Enum.GetValues(typeof (CompanyStatus)) as CompanyStatus[];
            if (companyTypes == null) return null;
            return companyTypes.Select(item => new KeyValuePair<int, string>((int) item, item.GetDescription()));
        }

        private string getAddress(string cityCode)
        {
            if (string.IsNullOrWhiteSpace(cityCode))
            {
                return string.Empty;
            }
            City city = FoundationService.QueryCity(cityCode);
            return city == null ? cityCode : city.Name;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeService.ResetPassword(hdCompanyAccount.Value.Trim(), Reason.Text.Trim(), CurrentUser.UserName);
                ShowMessage(this, "重置密码成功\n\r默认密码" + SystemParamService.DefaultPassword);
                Reason.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "重置密码");
            }
        }

        private bool valiate()
        {
            if (!Regex.IsMatch(txtPosRate.Text.Trim(), "^(0\\.[5-9]\\d?|[1-2]\\.\\d{1,2}|3\\.0{1,2}|[1-3])$"))
            {
                lblWarnRateInfo.Text = "Pos费率格式错误！";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtContactCertNo.Text.Trim()) && !AccountCombineService.ValidateIdentifyCard(txtContactCertNo.Text.Trim()))
            {
                lblWarnCertNoInfo.Text = "联系人身份证号格式错误！";
                return false;
            }
            return true;
        }
    }
}