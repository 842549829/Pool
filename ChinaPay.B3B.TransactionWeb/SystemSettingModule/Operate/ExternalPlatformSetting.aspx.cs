using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.ExternalPlatform;
using ChinaPay.Core.Extension;
using Yeexing = ChinaPay.B3B.Service.ExternalPlatform.Yeexing;
using Book = ChinaPay.B3B.Service.ExternalPlatform._51book;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.ExternalPlatform.Processor;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class ExternalPlatformSetting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                initData();
                var platform = Request.QueryString["Platform"];
                if (!string.IsNullOrWhiteSpace(platform))
                {
                    var setting = ManageService.QuerySetting((Common.Enums.PlatformType)int.Parse(platform));
                    if (setting != null)
                    {
                        bindData(setting);
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var platform = Request.QueryString["Platform"];
            var setting = saveData();
            if (!string.IsNullOrWhiteSpace(platform))
            {
                ManageService.UpdateSetting(setting, this.CurrentUser.UserName);
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "success", "alert('保存成功');window.location.href='ExternalPlatformList.aspx'", true);
            }
            else
            {
                var oldSetting = ManageService.QuerySetting((PlatformType)int.Parse(platform));
                if (oldSetting == null)
                {
                    ManageService.InsertSetting(setting, this.CurrentUser.UserName);
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "success", "alert('保存成功');window.location.href='ExternalPlatformList.aspx'", true);
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "success", "alert('该平台已设置过接口信息');", true);
                }
            }

        }

        private void bindData(Setting setting)
        {
            this.ddlPlatform.Items.Clear();
            this.ddlPlatform.Items.Add(new ListItem(setting.Platform.GetDescription(), ((byte)setting.Platform).ToString()));
            this.txtDeduct.Text = (setting.Deduct * 100).TrimInvaidZero().ToString();
            this.txtRebateBalance.Text = (setting.RebateBalance * 100).TrimInvaidZero().ToString();
            txtProviderCompany.SetCompanyName(setting.Provider);
            for (var i = 0; i < setting.PayInterface.Count(); i++)
            {
                var ddlYeexing = Page.FindControl("ddlPayInterface_" + i) as DropDownList;
                ddlYeexing.SelectedValue = ((byte)setting.PayInterface[i]).ToString();
            }
            this.rbnEnable.Enabled = false;
            this.rbnDisable.Enabled = false;
            if (setting.Enabled)
            {
                this.rbnEnable.Checked = true;
            }
            else
            {
                this.rbnDisable.Checked = true;
            }
        }

        private void initData()
        {
            var platformValue = Request.QueryString["Platform"];
            var platformType = (Common.Enums.PlatformType)int.Parse(platformValue);
            var platform = PlatformBase.GetPlatform(platformType);
            for (var i = 0; i < platform.AutoPayInterfaces.Count(); i++)
            {
                var ddlPayInterface = new DropDownList();
                foreach (var item in platform.AutoPayInterfaces)
                {
                    ddlPayInterface.Items.Add(new ListItem(item.GetDescription(), ((byte)item).ToString()));
                }
                ddlPayInterface.Items.Insert(0, new ListItem("-请选择-", ""));
                ddlPayInterface.ID = "ddlPayInterface_" + i;
                this.divPayInterface.Controls.Add(ddlPayInterface);
            }
            var companies = CompanyService.GetCompanies(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
            txtProviderCompany.SetCompanyType(CompanyType.Provider);
        }

        private Setting saveData()
        {
            var setting = new Setting();
            setting.Platform = (PlatformType)int.Parse(this.ddlPlatform.SelectedValue);
            setting.Deduct = decimal.Parse(this.txtDeduct.Text) / 100;
            setting.Enabled = this.rbnEnable.Checked ? true : false;
            setting.Provider = this.txtProviderCompany.CompanyId.Value;
            setting.RebateBalance = decimal.Parse(this.txtRebateBalance.Text) / 100;
            var strPayInterface = this.hfdPayInterface.Value;
            var list = new List<ChinaPay.B3B.DataTransferObject.Common.PayInterface>();
            foreach (var item in strPayInterface.Split('|'))
            {

                list.Add((ChinaPay.B3B.DataTransferObject.Common.PayInterface)int.Parse(item));
            }
            setting.PayInterface = list.Distinct().ToArray();
            setting.ProviderAccount = this.hfdCompanyAccount.Value;
            return setting;
        }
    }
}