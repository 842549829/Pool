using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.SystemManagement;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.MaintenanceWeb.SystemManagement
{
    public partial class SpecialProductManagement :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                Datainit();
            }
        }
        private void Datainit() {
            gvProductList.DataSource = SpecialProductService.Query().Select(
                o => new
                { 
                    Id = o.SpecialProductType.GetHashCode(),
                    o.Name,
                    o.Explain,
                    Describe = o.Description,
                    o.Enabled,
                    Remark = o.SpecialProductType.GetDescription()
                }
            );
            gvProductList.DataBind();
        }
        protected void gvProductList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Enabled")
            {
                var id =(SpecialProductType)Convert.ToInt32(e.CommandArgument);
                var air = SpecialProductService.Query(id);
                if (air != null)
                {
                    var view = new SpecialProductView(id)
                    {
                        Description = air.Description,
                        Enabled = !air.Enabled,
                        Explain = air.Explain,
                        Name = air.Name
                    };
                    try
                    {
                        SpecialProductService.Update(view, CurrentUser.UserName);
                        RegisterScript("alert('" + (air.Enabled ? "禁用成功！" : "启用成功！")+ "')");
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, air.Enabled ? "禁用" : "启用");
                        return;
                    }
                    Datainit();
                }
            }
        }
    }
}