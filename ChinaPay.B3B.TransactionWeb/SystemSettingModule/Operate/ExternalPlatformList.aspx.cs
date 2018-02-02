using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.ExternalPlatform;
using System.Linq;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class ExternalPlatformList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                bindDataSource();
            }
        }

        protected void grdExternalPlatform_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "enable")
            {

                ManageService.UpdateStatus((Common.Enums.PlatformType)int.Parse(e.CommandArgument.ToString()), true,this.CurrentUser.UserName);
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "success", "alert('启用成功');", true);
            }
            if (e.CommandName == "disable")
            {
                ManageService.UpdateStatus((Common.Enums.PlatformType)int.Parse(e.CommandArgument.ToString()), false,this.CurrentUser.UserName);
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "success", "alert('禁用成功');", true);
            }

            bindDataSource();
        }

        private void bindDataSource()
        {
            this.grdExternalPlatform.DataSource = from item in ManageService.QuerySettings()
                                                  select new
                                                  {
                                                      Platform = item.Platform.GetDescription(),
                                                      PlatformValue = (byte)item.Platform,
                                                      Enabled = item.Enabled ? "启用" : "禁用",
                                                      Deduct = (item.Deduct*100).TrimInvaidZero(),
                                                      PayInterfaceText = item.StrPayInterface,
                                                      RebateBalance = (item.RebateBalance*100).TrimInvaidZero(),
                                                      ProviderAccount = item.ProviderAccount
                                                  };
            this.grdExternalPlatform.DataBind();
        }

        protected void grdExternalPlatform_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnkEnable = e.Row.Cells[6].FindControl("lnkEnable") as LinkButton;
                var lnkDisable = e.Row.Cells[6].FindControl("lnkDisable") as LinkButton;
                if (e.Row.Cells[1].Text == "启用")
                {
                    lnkEnable.Visible = false;
                    lnkDisable.Visible = true;
                }
                else
                {
                    lnkEnable.Visible = true;
                    lnkDisable.Visible = false;
                }
                
            }
        }
    }
}