using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class City_new : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                if (Request.QueryString["action"]!=null)
                {
                    if (Request.QueryString["action"].ToString() == "add")
                    {
                        this.txtCityCode.Enabled = true;
                    }
                    else 
                    {
                        this.txtCityCode.Enabled = false;
                        Refresh(Request.QueryString["code"].ToString());
                    }
                }
            }
        }
        private void Refresh(string code)
        {
            ChinaPay.B3B.Service.Foundation.Domain.City city = FoundationService.QueryCity(code.Trim());
            if (city != null)
            {
                this.txtCityCode.Text = city.Code;
                this.txtCityName.Text = city.Name;
                if (city.ProvinceCode != null)
                {
                    this.ddlProvinceName.SelectedValue = city.ProvinceCode;
                }
                if (city.ShortSpelling != null) this.txtShortSpelling.Text = city.ShortSpelling;
                this.txtHotLevel.Text = city.HotLevel.ToString();
                if (city.Spelling != null) this.txtSpelling.Text = city.Spelling;
            }
        }
        /// <summary>
        /// 列表绑定
        /// </summary>
        private void DropList()
        {
            this.ddlProvinceName.DataSource = FoundationService.Provinces;
            this.ddlProvinceName.Items.Clear();
            this.ddlProvinceName.DataTextField = "Name";
            this.ddlProvinceName.DataValueField = "Code";
            this.ddlProvinceName.DataBind();
            this.ddlProvinceName.Items.Insert(0,new ListItem("-请选择-","0"));
        }
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
           if (Request.QueryString["action"] != null)
            {
                string shortSpelling = string.Empty;
                string spelling = string.Empty;
                int hotlevel = 0;
                if (!string.IsNullOrEmpty(this.txtShortSpelling.Text.Trim()))
                    shortSpelling = this.txtShortSpelling.Text.Trim();
                if (!string.IsNullOrEmpty(this.txtSpelling.Text.Trim()))
                    spelling = this.txtSpelling.Text.Trim();
                if (!string.IsNullOrEmpty(this.txtHotLevel.Text.Trim()))
                    hotlevel = int.Parse(this.txtHotLevel.Text.Trim());
                CityView cityView = new CityView()
                {
                    Code = this.txtCityCode.Text.Trim(),
                    Name = this.txtCityName.Text.Trim(),
                    ProvinceCode = this.ddlProvinceName.SelectedValue,
                    ShortSpelling = shortSpelling,
                    Spelling = spelling,
                    HotLevel = hotlevel
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddCity(cityView, "");
                        RegisterScript("alert('添加成功！'); window.location.href='City.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateCity(cityView, "");
                        RegisterScript("alert('修改成功！'); window.location.href='City.aspx?Search=Back'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion
    }
}
