using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class ChildTicketClass_Add : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"] == "update")
                        Refresh(Request.QueryString["Id"]);
                }
            }
        }
        private void Refresh(string id) 
        {
            ChinaPay.B3B.Service.Foundation.Domain.ChildOrderableBunk childOrderableBunk = FoundationService.QueryChildOrderableBunk(new Guid(id));
            if (childOrderableBunk == null) return;
            this.dropairlinecode.SelectedValue = childOrderableBunk.AirlineCode.Value;
            this.iptClass.Value = childOrderableBunk.BunkCode.Value;
            this.txtDiscount.Value = (childOrderableBunk.Discount*100).ToString();
        }
        private void DropList()
        {
            foreach (var item in FoundationService.Airlines)
            {
                this.dropairlinecode.Items.Add(new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value));
            }
            this.dropairlinecode.Items.Insert(0, new ListItem("-所有-", "-所有-"));
        } 
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                ChildOrderableBunkView childOrderableBunkView = new ChildOrderableBunkView()
                {
                    Airline = this.dropairlinecode.SelectedValue,
                    Bunk = this.iptClass.Value.Trim(),
                    Discount = Convert.ToDecimal(this.txtDiscount.Value.Trim())/100
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddChildrenOrderableBunk(childOrderableBunkView, CurrentUser.UserName);
                        ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, "alert('添加成功！');window.location.href='ChildTicketMaintain.aspx';", true);
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateChildrenOrderableBunk(new Guid(Request.QueryString["Id"].ToString()), childOrderableBunkView, CurrentUser.UserName);
                        ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, "alert('修改成功！'); window.location.href='ChildTicketMaintain.aspx'", true);
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion
    }
}
