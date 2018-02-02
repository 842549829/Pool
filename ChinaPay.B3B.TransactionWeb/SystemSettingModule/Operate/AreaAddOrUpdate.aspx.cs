using System;
using ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea;
using ChinaPay.B3B.Service.SystemSetting;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class AreaAddOrUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                        RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                var id = Request.QueryString["Id"];
                if (!string.IsNullOrWhiteSpace(id))
                {
                    this.lblAddOrUpdate.Text = "修改";
                    this.hfdAddOrUpdate.Value = "Update";
                    Guid areaId;
                    if (Guid.TryParse(id, out areaId))
                    {
                        var area = AreaService.Query(areaId);
                        Bind(area);
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                AreaView view = new AreaView();
                if (this.hfdAddOrUpdate.Value != "Update")
                {
                    SaveAreaView(view);
                    try
                    {
                        AreaService.InsertArea(view, this.CurrentUser.Name);
                        RegisterScript("alert('添加成功');window.location.href='MarketingAreaList.aspx';", false);
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"添加");
                        //this.errorMessage.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                        //this.errorMessage.InnerText = "系统繁忙,请稍后......";
                    }
                }
                else
                {
                    SaveAreaView(view);
                    try
                    {
                        var id = Request.QueryString["Id"];
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            Guid areaId = Guid.Parse(id);
                            AreaService.UpdateArea(areaId, view, this.CurrentUser.Name);
                            RegisterScript("alert('修改成功');window.location.href='MarketingAreaList.aspx';", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"修改");
                        //this.errorMessage.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                        //this.errorMessage.InnerText = "系统繁忙,请稍后......";
                    }
                }
            }
        }

        private void Bind(AreaView area)
        {
            this.txtAreaName.Text = area.Name;
            this.txtRemark.InnerText = area.Remark;
        }

        private void SaveAreaView(AreaView view)
        {
            view.Name = this.txtAreaName.Text;
            view.Remark = this.txtRemark.InnerText;
        }

        private bool Valiate()
        {
            if (string.IsNullOrWhiteSpace(this.txtAreaName.Text))
            {
                if (this.txtAreaName.Text.Trim().Length > 25)
                {
                    ShowMessage("区域名称位数不能超过25位！");
                    return false;
                }
                else
                {
                    ShowMessage("请输入区域名称！");
                    return false;
                }
            }
            if (this.txtRemark.InnerText.Trim().Length > 50)
            {
                ShowMessage("区域备注位数不能超过50位！");
                return false;
            }
            return true;
        }
    }
}