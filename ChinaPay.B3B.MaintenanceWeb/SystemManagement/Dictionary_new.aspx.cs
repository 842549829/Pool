using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.MaintenanceWeb.SystemManagement
{
    public partial class Dictionary_new : BasePage
    {
        #region 数据加载 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SystemDictionaryType systemDictionaryType = (SystemDictionaryType)Enum.Parse(typeof(SystemDictionaryType), Request.QueryString["type"].ToString());
                this.lblDictionaryName.Text = systemDictionaryType.GetDescription();
                if (Request.QueryString["action"] != null && Request.QueryString["action"].ToString() == "update")
                {
                    IEnumerable<SystemDictionaryItem> systemDictionaryItems = SystemDictionaryService.Query(systemDictionaryType);
                    var itemId = new Guid(Request.QueryString["Id"]);
                    var dictionaryItem = systemDictionaryItems.FirstOrDefault(item => item.Id == itemId);
                    if (dictionaryItem == null) return;
                    if (dictionaryItem.Name != null) this.txtName.Text = dictionaryItem.Name;
                    if (dictionaryItem.Value != null) this.txtValue.Text = dictionaryItem.Value;
                    if (dictionaryItem.Remark != null) this.ttRemark.InnerText = dictionaryItem.Remark;
                }
            }
        }
        #endregion

        #region 保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                SystemDictionaryType systemDictionaryType = (SystemDictionaryType)Enum.Parse(typeof(SystemDictionaryType), Request.QueryString["type"].ToString());
                SystemDictionaryItem systemDictionaryItem =null;
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        systemDictionaryItem = new SystemDictionaryItem(this.txtName.Text.Trim(), this.txtValue.Text.Trim(), this.ttRemark.InnerText.Trim());
                        SystemDictionaryService.AddItem(systemDictionaryType, systemDictionaryItem, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='Dictionary.aspx';");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else if (Request.QueryString["action"].ToString() == "update")
                {
                    try
                    {
                        systemDictionaryItem = new SystemDictionaryItem(new Guid(Request.QueryString["Id"]),this.txtName.Text.Trim(),this.txtValue.Text.Trim(),this.ttRemark.InnerText.Trim());
                        SystemDictionaryService.UpdateItem(systemDictionaryType, systemDictionaryItem, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='Dictionary.aspx';");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        }
        #endregion
    }
}