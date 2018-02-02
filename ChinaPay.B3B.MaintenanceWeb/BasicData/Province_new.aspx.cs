using System;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Province_new : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initArea();
                if (Request.QueryString["action"] != null)
                {
                    if (Request.QueryString["action"].ToString() == "add")
                    {
                        this.txtProvinceCode.Enabled = true;
                    }
                    else
                    {
                        this.txtProvinceCode.Enabled = false;
                        bindProvince(Request.QueryString["code"].ToString());
                    }
                }
            }
        }
        private void initArea() {
            this.ddlArea.DataSource = FoundationService.Areas;
            this.ddlArea.DataTextField = "Name";
            this.ddlArea.DataValueField = "Code";
            this.ddlArea.DataBind();
            this.ddlArea.Items.Insert(0, new ListItem("-选择区域-", ""));
        }
        /// <summary>
        /// 根据省份代码绑定
        /// </summary>
        /// <param name="code">省份代码</param>
        private void bindProvince(string code)
        {
            ChinaPay.B3B.Service.Foundation.Domain.Province province = FoundationService.QueryProvice(code.Trim());
            if (province != null)
            {
                this.txtProvinceCode.Text = province.Code;
                this.txtProvinceName.Text = province.Name;
                this.ddlArea.SelectedValue = province.AreaCode;
            }
        }
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                ProvinceView provinceView = new ProvinceView()
                {
                    Code = this.txtProvinceCode.Text.Trim(),
                    Name = this.txtProvinceName.Text.Trim(),
                    AreaCode = this.ddlArea.SelectedValue
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddProvince(provinceView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='Province.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateProvince(provinceView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='Province.aspx?Search=Back'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        }
        #endregion
    }
}
