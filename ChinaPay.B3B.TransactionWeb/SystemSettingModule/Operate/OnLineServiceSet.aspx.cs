using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer;
using ChinaPay.B3B.Service.SystemSetting;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class OnLineServiceSet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                OnLineCustomerView view = OnLineCustomerService.Query(this.CurrentCompany.CompanyId);
                if (view != null)
                {
                    this.txtTitle.Text = view.Title;
                    this.ftbContent.Text = view.Content;
                }
            }
            BindMember();
        }

        protected void dataSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "del")
            {
                try
                {
                    OnLineCustomerService.DeleteDivideGroup(Guid.Parse(e.CommandArgument.ToString()),PublishRoles.平台, this.CurrentUser.Name);
                    BindMember();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"删除");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string title = "";
            string content = "";
            if (!string.IsNullOrWhiteSpace(this.txtTitle.Text))
            {
                title = this.txtTitle.Text.ToLower().Replace("script", "").Replace("eval", "").Replace("&nbsp;", " ").Trim();
                if (title.IndexOf('<') != -1 || title.IndexOf('>') != -1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('客服服务标题禁止包含 < > 特殊符号！请重新输入');", true);
                    return;
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('客服服务标题不能为空');", true);
                return;
            }
            if (!string.IsNullOrWhiteSpace(this.ftbContent.Text))
            {
                content = ftbContent.Text.ToLower().Replace("script", "").Replace("eval", "").Replace("&nbsp;", " ").Trim();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('客服服务内容不能为空');", true);
                return;
            }
            try
            {
                OnLineCustomerView view = new OnLineCustomerView();
                view.Title = title;
                view.Content = content;
                OnLineCustomerService.SavePlatForm(this.CurrentCompany.CompanyId, view, this.CurrentUser.Name, ChinaPay.B3B.Service.Organization.OEMService.QueryOEM(CurrentCompany.CompanyId) != null ? PublishRoles.OEM : PublishRoles.平台);
                RegisterScript("alert('保存成功');window.location.href='OnLineServiceSet.aspx';", false);
            }catch(Exception ex)
            {
                ShowExceptionMessage(ex,"保存");
            }
        }

        private void BindMember()
        {
            this.dataSource.DataSource = OnLineCustomerService.QueryDivideGroups(this.CurrentCompany.CompanyId);
            this.dataSource.DataBind();
        }

        private bool Valiate()
        {
            if (string.IsNullOrWhiteSpace(this.txtTitle.Text))
            {
                ShowMessage("请输入标题！");
                return false;
            }
            else
            {
                if (this.txtTitle.Text.Trim().Length > 100)
                {
                    ShowMessage("标题字数不能超过100！");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.ftbContent.Text))
            {
                ShowMessage("请输入内容！");
                return false;
            }
            else
            {
                if (this.ftbContent.Text.Trim().Length > 20000)
                {
                    ShowMessage("内容字数不能超过20000！");
                    return false;
                }
            }
            return true;
        }
    }
}