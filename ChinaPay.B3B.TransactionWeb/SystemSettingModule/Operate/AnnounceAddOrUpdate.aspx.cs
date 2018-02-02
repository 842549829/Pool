using System;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.Service.Announce;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class AnnounceAddOrUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
              this.announceScope.Visible = this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Platform;
              this.hfdIsPlatform.Value = (this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Platform).ToString();
                string announceId = Request.QueryString["announceId"];
                if (!string.IsNullOrWhiteSpace(announceId))
                {
                 this.lblAddOrUpdate.Text = "修改";
                 var announce = AnnounceService.Query(Guid.Parse(announceId));
                 Bind(announce);
                }
            }
        }

        private void Bind(AnnounceView view)
        {
            this.txtTitle.Text = view.Title;
            this.ftbContent.Text = view.Content;
            if (view.AnnounceType == AnnounceLevel.Common)
            {
                this.rbnCommonType.Checked = true;
            }
            if(view.AnnounceType == AnnounceLevel.Important)
            {
                this.rbnImportType.Checked = true;
             }
            if (view.AnnounceType == AnnounceLevel.Emergency)
            {
                this.rbnErgentType.Checked = true;
            }
            if (view.AnnounceScope == Common.Enums.AnnounceScope.B3B)
            {
                this.chkB3b.Checked = true;
            }
            if (view.AnnounceScope == Common.Enums.AnnounceScope.OEM)
            {
                this.chkOem.Checked = true;
            }
            if (view.AnnounceScope == (Common.Enums.AnnounceScope.B3B | Common.Enums.AnnounceScope.OEM))
            {
                this.chkB3b.Checked = true;
                this.chkOem.Checked = true;
            }
        }

        private bool SaveInfo(AnnounceView view)
        {
            if (!string.IsNullOrWhiteSpace(this.txtTitle.Text))
            {
                view.Title = Regex.Replace(this.txtTitle.Text.Trim(), @"script", "", RegexOptions.IgnoreCase);
                view.Title = Regex.Replace(view.Title, @"eval", "", RegexOptions.IgnoreCase);
                view.Title = view.Title.Replace(@"<%","");
                view.Title = Regex.Replace(view.Title, @"&nbsp;", " ", RegexOptions.IgnoreCase);
                if (view.Title .IndexOf('<') != -1 || view.Title .IndexOf('>') != -1)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('标题禁止包含 < > 特殊符号！请重新输入');", true);
                    return false;
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('标题不能为空');", true);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(this.ftbContent.Text.Trim()))
            {
                view.Content = Regex.Replace(ftbContent.Text, @"script", "", RegexOptions.IgnoreCase);
                view.Content = Regex.Replace(view.Content, @"eval", "", RegexOptions.IgnoreCase);
                view.Content = view.Content.Replace(@"<%","");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('内容不能为空');", true);
                return false;
            }
            view.PublishTime = DateTime.Now;
            if (this.rbnCommonType.Checked)
            {
                view.AnnounceType = AnnounceLevel.Common;
            }
            if (this.rbnErgentType.Checked)
            {
                view.AnnounceType = AnnounceLevel.Emergency;
            }
            if (this.rbnImportType.Checked)
            {
                view.AnnounceType = AnnounceLevel.Important;
            }
            if (this.CurrentCompany.CompanyType == CompanyType.Platform)
            {
                if (this.chkB3b.Checked && !this.chkOem.Checked)
                {
                    view.AnnounceScope = Common.Enums.AnnounceScope.B3B;
                }
                if (!this.chkB3b.Checked && this.chkOem.Checked)
                {
                    view.AnnounceScope = Common.Enums.AnnounceScope.OEM;
                }
                if (this.chkB3b.Checked && this.chkOem.Checked)
                    view.AnnounceScope = Common.Enums.AnnounceScope.B3B | Common.Enums.AnnounceScope.OEM;
            }
            else
            {
                view.AnnounceScope = Common.Enums.AnnounceScope.OEM;
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                if (this.lblAddOrUpdate.Text != "修改")
                {
                    try
                    {
                        AnnounceView view = new AnnounceView();
                        if (SaveInfo(view))
                        {
                            if (this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Platform)
                                AnnounceService.InsertPlatform(this.CurrentCompany.CompanyId, view, this.CurrentUser.Name);
                            else
                                AnnounceService.InsertOEM(this.CurrentCompany.CompanyId,view,this.CurrentUser.Name);
                            RegisterScript("alert('添加成功');window.location.href='AnnounceList.aspx';", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"添加");
                    }
                }
                else
                {
                    try
                    {
                        AnnounceView view = new AnnounceView();
                        if (SaveInfo(view))
                        {
                            string announceId = Request.QueryString["announceId"];
                            AnnounceService.Update(Guid.Parse(announceId), view,this.CurrentCompany.CompanyType==CompanyType.Platform? Common.Enums.PublishRole.平台:PublishRole.用户, this.CurrentUser.Name);
                            RegisterScript("alert('修改成功');window.location.href='AnnounceList.aspx';", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"修改");
                    }
                }
            }
        }

        private bool Valiate()
        {
            if (string.IsNullOrWhiteSpace(this.txtTitle.Text))
            {
                if (this.txtTitle.Text.Trim().Length > 50)
                {
                    ShowMessage("公告标题格式错误！");
                    return false;
                }
                else
                {
                    ShowMessage("请输入公告标题！");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.ftbContent.Text))
            {
                ShowMessage("请输入公告内容！");
                return false;
            }
            if (this.CurrentCompany.CompanyType == CompanyType.Platform)
            {
                if (!this.chkB3b.Checked && !this.chkOem.Checked)
                {
                    ShowMessage("请选择公告范围！");
                    return false;
                }
            }
            return true;
        }
    }
}