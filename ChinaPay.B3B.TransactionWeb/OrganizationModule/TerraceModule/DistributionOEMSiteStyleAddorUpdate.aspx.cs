using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOEMSiteStyleAddorUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var styleId = Request.QueryString["styleId"];
                if (!string.IsNullOrEmpty(styleId)) LoadStyleSet(Guid.Parse(styleId));
                else LoadEmptyStyle();
            }
        }

        private void LoadEmptyStyle() {
            hdStyleId.Value = Guid.Empty.ToString();
            rpStyleFilePath.DataSource = Enumerable.Range(1,5).Select(p=>string.Empty);
            rpStyleFilePath.DataBind();

        }

        private void LoadStyleSet(Guid styleId) {
            var style = OEMStyleService.QueryOEMStyle(styleId);
            if (style!=null)
            {
                hdStyleId.Value = style.Id.ToString();
                txtKeyWord.Value = style.StyleName;
                txtTemplatePath.Value = style.TemplatePath;
                txtThumbnailPicture.Value = style.ThumbnailPicture;
                txtTemplateIndex.Value = style.Sort.ToString();
                txtTemplateDescription.Value = style.Remark;
                if (style.Enable)
                {
                    radTemplateEnabled.Checked = true;
                }
                else
                {
                    radTemplateDisabled.Checked = true;
                }
                style.StylePath.AddRange(Enumerable.Range(1, 3).Select(p => string.Empty));
                rpStyleFilePath.DataSource = style.StylePath;
                rpStyleFilePath.DataBind();
            }
            hdStyleId.Value = styleId.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var style = new OEMStyle();
            style.StyleName = txtKeyWord.Value.Trim() ;
            style.StylePath = new List<string>();
            style.TemplatePath = txtTemplatePath.Value.Trim();
            style.ThumbnailPicture = txtThumbnailPicture.Value.Trim();
            style.Remark = txtTemplateDescription.Value.Trim();
            style.Sort = int.Parse(txtTemplateIndex.Value.Trim());
            style.Enable = radTemplateEnabled.Checked;
            foreach (RepeaterItem item in rpStyleFilePath.Items)
            {
                var pathContainer = item.FindControl("txtStyleFilePath") as HtmlInputText;
                if (pathContainer != null && !string.IsNullOrEmpty(pathContainer.Value)) style.StylePath.Add(pathContainer.Value.Trim());
            }
            var styleId = Request.QueryString["styleId"];
            if (!string.IsNullOrEmpty(styleId))
            {
                style.Id = Guid.Parse(styleId);
                OEMStyleService.UpdateOEMStyle(style, CurrentUser.UserName);
            }
            else
            {
                style.Id = Guid.NewGuid();
                OEMStyleService.InsertOEMStyle(style,CurrentUser.UserName);
            }
            RegisterScript(this, "alert('保存成功！');location.href='OrganizationModule/TerraceModule/DistributionOEMSiteStyleManage.aspx'");
        }
    }
}