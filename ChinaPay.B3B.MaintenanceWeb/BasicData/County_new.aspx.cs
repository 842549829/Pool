using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class County_new : BasePage
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
                        this.txtCountyCode.Enabled = true;
                    }
                    else 
                    {
                        this.txtCountyCode.Enabled = false;
                        Refresh(Request.QueryString["code"].ToString());
                    }
                }
            }
        }
        private void Refresh(string code)
        {
            ChinaPay.B3B.Service.Foundation.Domain.County county = FoundationService.QueryCounty(code.Trim());
            if (county != null)
            {
                this.txtCountyCode.Text = county.Code;
                this.txtChineseName.Text = county.Name;
                if (county.Spelling != null) this.txtSpelling.Text = county.Spelling;
                if(county.ShortSpelling!=null) this.txtShortSpelling.Text = county.ShortSpelling;
                if (county.City!=null) this.ddlCityName.SelectedValue = county.CityCode;
                this.txtHotLevel.Text = county.HotLevel.ToString();
            }
        }
        /// <summary>
        /// 列表绑定
        /// </summary>
        private void DropList()
        {
            this.ddlCityName.DataSource = FoundationService.Cities;
            this.ddlCityName.Items.Clear();
            this.ddlCityName.DataTextField = "Name";
            this.ddlCityName.DataValueField = "Code";
            this.ddlCityName.DataBind();
            this.ddlCityName.Items.Insert(0,new ListItem("-请选择-","0"));
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
                if(!string.IsNullOrEmpty(this.txtShortSpelling.Text.Trim())) 
                    shortSpelling = this.txtShortSpelling.Text.Trim();
                if (!string.IsNullOrEmpty(this.txtSpelling.Text.Trim()))
	                spelling = this.txtSpelling.Text.Trim();
                if(!string.IsNullOrEmpty(this.txtHotLevel.Text.Trim()))
                    hotlevel = int.Parse(this.txtHotLevel.Text.Trim());
                CountyView countyView = new CountyView()
                {
                     Code = this.txtCountyCode.Text.Trim(),
                     Name = this.txtChineseName.Text.Trim(),
                     ShortSpelling = shortSpelling,
                     Spelling = spelling,
                     HotLevel = hotlevel,
                     CityCode = this.ddlCityName.SelectedValue,
                };
                if (Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddCounty(countyView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='County.aspx'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        FoundationService.UpdateCounty(countyView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='County.aspx?Search=Back'");
                    } catch(Exception ex) {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        } 
        #endregion
    }
}
