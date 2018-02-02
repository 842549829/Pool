using System;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;

namespace ChinaPay.B3B.MaintenanceWeb.SystemManagement
{
    public partial class SystemParameter_Update : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["type"]!=null)
                {
                    SystemParamType systemParamType = (SystemParamType)Enum.Parse(typeof(SystemParamType),Request.QueryString["type"].ToString());
                    SystemParam systemParam = SystemParamService.QueryParam(systemParamType);
                    if (systemParam != null) {
                        this.txtType.Text = Request.QueryString["TypeName"].ToString();
                        this.txtValue.Text = systemParam.Value;
                        this.ttRemark.InnerText = systemParam.Remark;
                        this.txtType.Enabled = false;
                        this.ttRemark.Disabled = true;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SystemParamType systemParamType = (SystemParamType)Enum.Parse(typeof(SystemParamType),Request.QueryString["type"].ToString());
            try
            {
                SystemParamService.Update(systemParamType, this.txtValue.Text.ToString(), CurrentUser.UserName);
                RegisterScript("alert('修改成功！'); window.location.href='SystemParameter.aspx';");
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "修改");
            }
        }
    }
}