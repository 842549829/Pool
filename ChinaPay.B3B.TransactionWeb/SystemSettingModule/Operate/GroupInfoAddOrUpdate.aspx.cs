using System;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer;
using ChinaPay.B3B.Service.SystemSetting;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class GroupInfoAddOrUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                string divGroupId = Request.QueryString["devideGroupId"];
                if (!string.IsNullOrWhiteSpace(divGroupId))
                {
                    this.lblAddOrUpdate.Text = "修改";
                    DivideGroupView view = OnLineCustomerService.QueryDivideGroup(Guid.Parse(divGroupId));
                    Bind(view);
                }
            }
        }

        private void Bind(DivideGroupView view)
        {
            this.txtGroupName.Text = view.Name;
            this.txtGroupOrder.Text = view.SortLevel.ToString();
            this.txtDescription.InnerText = view.Description;
        }

        private void SaveInfo(DivideGroupView view)
        {
            view.Description = this.txtDescription.InnerText;
            view.Name = this.txtGroupName.Text;
            view.SortLevel = int.Parse(this.txtGroupOrder.Text);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                if (this.lblAddOrUpdate.Text != "修改")
                {
                    DivideGroupView view = new DivideGroupView();
                    SaveInfo(view);
                    try
                    {
                        OnLineCustomerService.InsertDivideGroup(this.CurrentCompany.CompanyId, view, IsOEM ? PublishRoles.OEM : PublishRoles.平台, this.CurrentUser.Name);
                        RegisterScript("alert('添加成功');window.location.href='OnLineServiceSet.aspx';", false);
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"添加");
                    }
                }
                else
                {
                    DivideGroupView view = new DivideGroupView(Guid.Parse(Request.QueryString["devideGroupId"]));
                    SaveInfo(view);
                    try
                    {
                        OnLineCustomerService.UpdateDivideGroup(view, IsOEM ? PublishRoles.OEM : PublishRoles.平台, this.CurrentUser.Name);
                        RegisterScript("alert('修改成功');window.location.href='OnLineServiceSet.aspx';", false);
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"修改");
                    }
                }
            }
        }

        private bool valiate()
        {
            if (string.IsNullOrWhiteSpace(this.txtGroupName.Text))
            {
                ShowMessage("请输入分组名称！");
                return false;
            }
            else
            {
                if (this.txtGroupName.Text.Trim().Length > 25)
                {
                    ShowMessage("分组名称位数不能超过25位！");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.txtGroupOrder.Text))
            {
                ShowMessage("分组排序不能为空！");
                return false;
            }
            else
            {
                if(!Regex.IsMatch(this.txtGroupOrder.Text.Trim(),"^[0-9]+$"))
                {
                    ShowMessage("分组排序只能输入数字！");
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.txtDescription.InnerText)&&(this.txtDescription.InnerText.Trim()).Length > 100)
            {
                ShowMessage("分组描述位数不能超过100位！");
                return false;
            }
            return true;
        }

    }
}