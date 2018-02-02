using System;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer;
using ChinaPay.B3B.Service.SystemSetting;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class MermberAddOrUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string memberId = Request.QueryString["memberId"];
                if (!string.IsNullOrWhiteSpace(memberId))
                {
                    this.lblAddOrUpdate.Text = "修改";
                    MemberManage view = OnLineCustomerService.QueryMember(Guid.Parse(memberId));
                    this.txtMemberExplain.InnerText = view.Remark;
                    this.hfdQQ.Value = view.QQ.Join(",");
                    this.txtSortLevel.Text = view.SortLevel.ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                if (this.lblAddOrUpdate.Text != "修改")
                {
                    try
                    {
                        MemberView view = new MemberView();
                        SaveInfo(view);
                        string devideGroupId = Request.QueryString["devideGroupId"];
                        OnLineCustomerService.InsertMember(Guid.Parse(devideGroupId), view, IsOEM ? PublishRoles.OEM : PublishRoles.平台, this.CurrentUser.Name);
                        Response.Redirect("MemberManager.aspx?devideGroupId=" + devideGroupId, false);
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        string memberId = Request.QueryString["memberId"];
                        string devideGroupId = Request.QueryString["devideGroupId"];
                        if (!string.IsNullOrWhiteSpace(memberId))
                        {
                            MemberView view = new MemberView(Guid.Parse(memberId));
                            SaveInfo(view);
                            OnLineCustomerService.UpdateMember(view, IsOEM ? PublishRoles.OEM : PublishRoles.平台, this.CurrentUser.Name);
                            Response.Redirect("MemberManager.aspx?devideGroupId=" + devideGroupId, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"修改");
                    }
                }
            }
        }

        private void SaveInfo(MemberView view)
        {
            view.Remark = this.txtMemberExplain.InnerText;
            view.SortLevel = int.Parse(this.txtSortLevel.Text);
            view.QQ = this.hfdQQ.Value.Split(',');
        }

        private bool Valiate()
        {
            if (string.IsNullOrWhiteSpace(this.txtMemberExplain.InnerText))
            {
                ShowMessage("请输入成员说明！");
                return false;
            }
            else
            {
                if (this.txtMemberExplain.InnerText.Trim().Length > 25)
                {
                    ShowMessage("成员说明位数不能超过25位！");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.txtSortLevel.Text))
            {
                ShowMessage("排序字段不能为空！");
                return false;
            }
            else
            {
                if (!Regex.IsMatch(this.txtSortLevel.Text.Trim(), "^[0-9]+$"))
                {
                    ShowMessage("排序字段只能输入数字！");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.hfdQQ.Value))
            {
                ShowMessage("请输入成员QQ！");
                return false;
            }
            else
            {
                if (this.hfdQQ.Value.Trim().Length > 300)
                {
                    ShowMessage("请删除一些QQ号码！");
                    return false;
                }
            }
            return true;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string devideGroupId = Request.QueryString["devideGroupId"];
            Response.Redirect("MemberManager.aspx?devideGroupId="+devideGroupId);
        }
    }
}