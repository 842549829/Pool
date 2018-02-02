using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule
{
    public partial class DistributionOEMSiteSetChooiceStyle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var oem = OEMService.QueryOEM(CurrentCompany.CompanyId);
                LoadStyle(oem);
            }
        }

        private void LoadStyle(OEMInfo oem)
        {
            string str = "";
            int i = 0;
            foreach (var item in OEMStyleService.QueryOEMVailStyles())
            {
                if (oem.OEMStyle != null && item.Id == oem.OEMStyle.Id)
                {
                    hidValue.Value = item.Id.ToString();
                }
                str += "<li for='s" + i + "'><img  title='" + item.Remark + "' src='" + item.ThumbnailPicture
                       + "' for='s" + i + "' /><p class='bor'>" + item.StyleName + "</p><p><input type='radio' " +
                       "name='radChooice' value='" + item.Id + "' id='s" + i + "' " + (oem.OEMStyle != null && item.Id == oem.OEMStyle.Id
                                                                                           ? "checked='checked'"
                                                                                           : "") + " /><label for='s" + i + "'>设为默认</label></p></li>";
                i++;
            }
            ulHtml.InnerHtml = str;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var oem = OEMService.QueryOEM(CurrentCompany.CompanyId);
                Guid styleId = Guid.Parse(hidValue.Value);
                DistributionOEMService.ChooiceStyle(oem.Id, styleId, CurrentUser.UserName);
                //刷新缓存
                FlushRequester.TriggerOEMFlusher(oem.Id);
                oem = OEMService.QueryOEM(oem.DomainName);
                LoadStyle(oem);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "选择风格");
            }

            ShowMessage("选择风格成功，刷新页面后生效！");
        }
    }
}