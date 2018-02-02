using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.AirlineConfig;
using ChinaPay.B3B.Service.AirlineConfig.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class AirlineConfigSettings : BasePage
    {
        private Guid? PlatformId
        {
            get
            {
                var paramId = Request.QueryString["OemId"];
                if (!string.IsNullOrEmpty(paramId))
                {
                    return Guid.Parse(paramId);
                }
                else
                {
                    if (CurrentCompany.CompanyType==CompanyType.Platform)
                    {
                        back.Visible = false;
                        return Guid.Empty;
                    }
                    Response.Write("无权限改配置信息！");
                    Response.End();
                    return null;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                LoadOEMConfigs(PlatformId);
            }
        }

        private void LoadOEMConfigs(Guid? oemId)
        {
            if (oemId==null)
            {
                return;
            }
            var config = OEMAirlineConfigService.QueryConfig(oemId) ?? new OEMAirlineConfig()
            {
                Config = new Dictionary<ConfigUseType, Tuple<string, string>>()
            };
            var configUserTypes = from ConfigUseType u in Enum.GetValues(typeof(ConfigUseType))
                                  select new
                                  {
                                      UseType = u.GetDescription(),
                                      UseTypeValue = (byte)u,
                                      UserName = config.Config.ContainsKey(u) ?
                                        config.Config[u].Item1 : string.Empty,
                                      OfficeNO = config.Config.ContainsKey(u) ?
                                          config.Config[u].Item2 : string.Empty
                                  };
            SettingList.DataSource = configUserTypes;
            SettingList.DataBind();
        }


        protected void btnSaveConfig_Click(object sender, EventArgs e)
        {
            var config = new Dictionary<ConfigUseType, Tuple<string, string>>();
            foreach (RepeaterItem item in SettingList.Items)
            {
                var type = item.FindControl("hdUserTypeValue") as HiddenField;
                var userName = item.FindControl("txtUserName") as TextBox;
                var officeNO = item.FindControl("txtOfficeNO") as TextBox;
                if (type != null)
                {
                    config.Add((ConfigUseType)int.Parse(type.Value.Trim()), new Tuple<string, string>(userName.Text.Trim(), officeNO.Text.Trim()));
                }
            }
            var success = OEMAirlineConfigService.SaveConfig(PlatformId.Value, config,CurrentUser.UserName);
            ClientScript.RegisterStartupScript(GetType(), "successTip", "alert('保存成功！')",true);
        }
    }
}