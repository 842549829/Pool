using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using System.Text.RegularExpressions;
using ChinaPay.Core.Extension;
using System.Text;
using System.Web.Script.Serialization;
using ChinaPay.B3B.Service.Organization.Domain;


namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class PurchaseRestrictionSettingGlobal : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                initData();
            }
        }

        private void initData()
        {
            string strAirlines = "";
            int i = 0;
            foreach (var item in FoundationService.Airlines)
            {
                i++;
                strAirlines += "<input type='checkbox' id='checkbox_" + i + "' value='" + item.Code.Value + "' /><label for='checkbox_" + i + "'>" + item.ShortName + "</label>&nbsp;&nbsp;";
                if (i % 15 == 0)
                {
                    strAirlines += "<br />";
                }
            }
            divAirlinelist.InnerHtml = strAirlines;
            txtDepartureAirports.InitData(true, Service.FoundationService.Airports.Where(item => item.Valid));
            var purchaseLimitation = CompanyService.QueryLimitationType(this.CurrentCompany.CompanyId);
            if (purchaseLimitation == Common.Enums.PurchaseLimitationType.Each)
            {
                this.rbnPurchaseEach.Checked = true;
            }
            if (purchaseLimitation == Common.Enums.PurchaseLimitationType.Global)
            {
                this.rbnPurchaseGlobal.Checked = true;
            }
            var global = PurchaseLimitationService.QueryPurchaseLimitationGroup(this.CurrentCompany.CompanyId);
            string strLimitation = "";
            if (global != null)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                strLimitation = jss.Serialize(global.Limitation);
                strLimitation = "var limitation="+strLimitation;
                this.hfdLimitation.Value = strLimitation;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                try
                {
                    var purchaseLimitationType = CompanyService.QueryLimitationType(this.CurrentCompany.CompanyId);
                    CompanyService.SetLimitationType(this.CurrentCompany.CompanyId,this.rbnPurchaseGlobal.Checked? Common.Enums.PurchaseLimitationType.Global:
                        (this.rbnPurchaseEach.Checked?Common.Enums.PurchaseLimitationType.Each:Common.Enums.PurchaseLimitationType.None), this.CurrentUser.UserName);
                    if (this.rbnPurchaseGlobal.Checked)
                    {
                        var view = savaData();
                        
                        var orginalGlobal = PurchaseLimitationService.QueryPurchaseLimitationGroup(this.CurrentCompany.CompanyId);
                        if (orginalGlobal == null)
                        {
                            PurchaseLimitationService.InsertPurchaseLimitationGroup(view, this.CurrentUser.UserName);
                        }
                        else
                        {
                            PurchaseLimitationService.UpdatePurchaseRestrictionSettingGlobal(view, purchaseLimitationType, this.CurrentUser.UserName);
                        }
                    }
                    else
                    {

                        PurchaseLimitationService.UpdatePurchaseLimitationGroup(this.CurrentCompany.CompanyId, purchaseLimitationType, this
                            .rbnPurchaseNone.Checked ? Common.Enums.PurchaseLimitationType.None : Common.Enums.PurchaseLimitationType.Each, this.CurrentUser.UserName);
                    }

                    RegisterScript("alert('保存成功');window.location.href='PurchaseRestrictionSettingGlobal.aspx';", false);
                }
                catch (Exception ex)
                {

                    ShowExceptionMessage(ex, "保存");
                }
            }
        }

        //保存“使用全局统一设置”
        private ChinaPay.B3B.Service.Organization.Domain.PurchaseLimitationGroup savaData()
        {
            var global = new ChinaPay.B3B.Service.Organization.Domain.PurchaseLimitationGroup();
            global.CompanyId = this.CurrentCompany.CompanyId;
            global.IsGlobal = this.rbnPurchaseGlobal.Checked;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var keys = Request.Form.AllKeys;
            List<string> name = new List<string>();
            global.Limitation = new List<PurchaseLimitation>();
            foreach (var item in keys)
            {
                if (Regex.IsMatch(item, "^Data_\\d{1,}$"))
                {
                    name.Add(item);
                    var data = jss.Deserialize<PurchaseLimitation>(Request.Form[item]);
                    data.LimitationGroupId = global.Id;
                    global.Limitation.Add(data);
                }
            }
            global.Limitation = BindId(global.Limitation);
            return global;
        }

        private IList<PurchaseLimitation> BindId(IList<PurchaseLimitation> limitation)
        {
            foreach (var item in limitation)
            {
                item.Id = Guid.NewGuid();
                foreach (var it in item.Rebate)
                {
                    it.LimitationId = item.Id;
                }
            }
            return limitation;
        }

        private bool valiate()
        {
            if (this.rbnPurchaseGlobal.Checked)
            {
                if (this.rbnPurchaseOnlySelfNormalPolicy.Checked)
                {
                    if (!Regex.IsMatch(this.txtDefaultRebateAdultNormalPolicy.Text, "^\\d{1,2}(.\\d{1})?$"))
                    {
                        ShowMessage("下级默认返点（普通政策）格式错误,只支持保留一位小数");
                        return false;
                    }
                }
                if (this.rbnPurchaseOnlySelfBargainPolicy.Checked)
                {
                    if (!Regex.IsMatch(this.txtDefaultRebateAdultBargainPolicy.Text, "^\\d{1,2}(.\\d{1})?$"))
                    {
                        ShowMessage("下级默认返点（特价政策）格式错误,只支持保留一位小数");
                        return false;
                    }
                }
            }
            return true;
        }

    }
}