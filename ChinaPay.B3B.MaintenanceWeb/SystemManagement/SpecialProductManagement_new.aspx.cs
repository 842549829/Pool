using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.SystemManagement;

namespace ChinaPay.B3B.MaintenanceWeb.SystemManagement
{
    public partial class SpecialProductManagement_new : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Datainit();
            }
        }
        private void Datainit()
        {
            string id = Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(id))
            {
                var view = SpecialProductService.Query((SpecialProductType)Convert.ToByte(id));
                if (view != null) {
                    lblProductId.Text = view.SpecialProductType.GetHashCode().ToString();
                    txtProductName.Text = view.Name;
                    txtProductExplain.Text = view.Explain;
                    txtProductDescribe.Text = view.Description;
                    if (view.Enabled)
                    {
                        rdoEnabled.Checked = true;
                    }
                    else
                    {
                        rdoDisabled.Checked = true;
                    }
                }
            }
        }
        protected SpecialProductView GetSpecialProductView() {
            return new SpecialProductView((SpecialProductType)Convert.ToByte(lblProductId.Text)) 
            {
                 Name =txtProductName.Text.Trim(),
                 Explain = txtProductExplain.Text.Trim(),
                 Description = txtProductDescribe.Text.Trim(),
                 Enabled = rdoEnabled.Checked
            };
        }
        protected void btnSave_Click(object sender, EventArgs e) 
        {
            try
            {
                SpecialProductService.Update(GetSpecialProductView(), CurrentUser.UserName);
                RegisterScript("alert('修改成功!');window.location.href='./SpecialProductManagement.aspx'");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改失败!");
                return;
            }
        }
    }
}