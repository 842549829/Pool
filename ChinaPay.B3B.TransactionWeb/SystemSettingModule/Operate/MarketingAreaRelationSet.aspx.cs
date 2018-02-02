using System;
using ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea;
using ChinaPay.B3B.Service.SystemSetting;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class MarketingAreaRelationSet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                initData();
                string provinceCode = Request.QueryString["provinceCode"];
                if (!string.IsNullOrWhiteSpace(provinceCode))
                {
                    AreaRelationView view = AreaService.QueryRelation(provinceCode);
                    this.lblProvinceName.Text = view.ProcinceName;
                    this.dropArea.SelectedValue = view.AreaName;
                }
            }
        }

        private void initData()
        {
            AreaQueryConditon condition = new AreaQueryConditon();
            this.dropArea.DataSource = AreaService.Query(condition);
            this.dropArea.DataTextField = "Name";
            this.dropArea.DataValueField = "Name";
            this.dropArea.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string provinceCode = Request.QueryString["provinceCode"];
            if (!string.IsNullOrWhiteSpace(provinceCode))
            {
                try
                {
                    AreaService.InsertAreaRelation(this.dropArea.SelectedValue, provinceCode, this.CurrentUser.Name);
                    RegisterScript("alert('设置成功');window.location.href='AreaRelateList.aspx';", false);
                }
                catch(Exception ex)
                {
                    ShowExceptionMessage(ex,"保存");
                }
            }
        }
    }
}