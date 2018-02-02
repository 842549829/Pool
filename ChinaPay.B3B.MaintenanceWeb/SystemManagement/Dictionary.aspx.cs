using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;
using System.Linq;

namespace ChinaPay.B3B.MaintenanceWeb.SystemManagement
{
    public partial class Dictionary : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataList();
                Refresh();
            }
        }
        private void Refresh()
        {
            SystemDictionaryType dictionaryType = SystemDictionaryType.FirstOrBusinessBunkDescription;
            if (Request.QueryString["categoryId"] != null)
            {
                string categoryId = Request.QueryString["categoryId"].ToString();
                dictionaryType = (SystemDictionaryType)Enum.Parse(typeof(SystemDictionaryType), categoryId);
            }
            this.lblDictionaryName.Text = dictionaryType.GetDescription();
            this.gvSpecialType.DataSource = SystemDictionaryService.Query(dictionaryType);
            this.gvSpecialType.DataBind();
            this.iType.Value = ((int)dictionaryType).ToString();
        }
        #region 列表绑定
        /// <summary>
        /// 列表绑定
        /// </summary>
        private void DataList()
        {
            List<NewSystemDictionaryType> newSystemDictionaryTypes = new List<NewSystemDictionaryType>();
            NewSystemDictionaryType newSystemDictionaryType = null;
            foreach (SystemDictionaryType item in Enum.GetValues(typeof(SystemDictionaryType)))
            {
                newSystemDictionaryType = new NewSystemDictionaryType() { TypeValue = item.ToString(), TypeName = item.GetDescription() };
                newSystemDictionaryTypes.Add(newSystemDictionaryType);
            }
            this.DirctionaryCate.DataSource = newSystemDictionaryTypes;
            this.DirctionaryCate.DataBind();
        }
        public class NewSystemDictionaryType
        {
            public string TypeName { get; set; }
            public string TypeValue { get; set; }
        }
        #endregion 
        #endregion

        #region 添加
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (iType.Value.ToString() == "0")
            {
                Response.Redirect("Dictionary_new.aspx?action=add&type=FirstOrBusinessBunkDescription");
            }
            else
            {
                Response.Redirect("Dictionary_new.aspx?action=add&type=" + iType.Value.ToString());
            }
        } 
        #endregion

        #region 修改 删除
        protected void gvSpecialType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "update")
            {
                string id = e.CommandArgument.ToString();
                if (iType.Value.ToString() == "0")
                    Response.Redirect("Dictionary_new.aspx?action=update&Id=" + id + "&type=FirstOrBusinessBunkDescription");
                else
                    Response.Redirect("Dictionary_new.aspx?Id=" + id + "&action=update&type=" + iType.Value.ToString());
            }
            else if (e.CommandName == "dictionaryDel")
            {
                if (e.CommandArgument != null)
                {
                    string type = string.Empty;
                    if (iType.Value.ToString() == "0")
                        type = "FirstOrBusinessBunkDescription";
                    else
                       type =iType.Value.ToString();
                    try
                    {
                        SystemDictionaryType systemDictionaryType = (SystemDictionaryType)Enum.Parse(typeof(SystemDictionaryType),type);
                        SystemDictionaryService.DeleteItem(systemDictionaryType,new Guid(e.CommandArgument.ToString()),"");
                        RegisterScript("alert('删除成功！'); window.location.href='Dictionary.aspx';");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "删除");
                    }
                }
            }
        } 
        #endregion

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string refreshAddress = SystemDictionaryService.Query(SystemDictionaryType.SystemRefreshCacheAddress).FirstOrDefault().Value;
                string key = Utility.MD5EncryptorService.MD5FilterZero(System.Configuration.ConfigurationManager.AppSettings["SignKey"], "utf-8");
                ChinaPay.Utility.HttpRequestUtility.GetHttpResult(refreshAddress + "?Action=Flush&Target=DICTIONARY&Key=" + key, 3000);
                ShowMessage("刷新成功");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "刷新");
            }
        }
    }
}
