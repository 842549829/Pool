using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.ReleaseNote;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.MaintenanceWeb.ReleaseNote
{
    public partial class ReleaseNote : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitCompanyType();
                if (Request.QueryString["type"] == "update")
                {
                    var note = ReleaseNoteService.Query(Guid.Parse(Request.QueryString["id"]));
                    txtTime.Text = note.UpdateTime.ToString("yyyy-MM-dd");
                    txtRemark.Text = note.Context.Replace("<br />", "\r\n");
                    txtTitle.Text = note.Title;
                    if (note.ReleaseType == ReleaseNoteType.B3BVisible)
                    {
                        LoadCompanyType(note.Type);
                        ddlCompanyList.Visible = true;
                        ddlPoolList.Visible = false;
                        radRadB3B.Checked = true;
                        radRadPoolpay.Checked = false;
                    }
                    else if (note.ReleaseType == ReleaseNoteType.PoolpayVisible)
                    {
                        LoadPoolpayType(note.Type);
                        ddlCompanyList.Visible = false;
                        ddlPoolList.Visible = true;
                        radRadPoolpay.Checked = true;
                        radRadB3B.Checked = false;
                    }
                    radRadPoolpay.Enabled = false;
                    radRadB3B.Enabled = false;
                }
                else
                {
                    txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        private void LoadCompanyType(CompanyType? type)
        {
            if (type.HasValue)
            {
                foreach (ListItem item in ddlCompanyList.Items)
                {
                    var itemValue = int.Parse(item.Value);
                    if (((int)type.Value & itemValue) > 0)
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        private void LoadPoolpayType(CompanyType? type)
        {
            if (type.HasValue)
            {
                foreach (ListItem item in ddlPoolList.Items)
                {
                    var itemValue = int.Parse(item.Value);
                    if (((int)type.Value & itemValue) > 0)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void InitCompanyType()
        {
            ddlCompanyList.DataSource = Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().Select(e => new
            {
                Text = e.GetDescription(),
                Value = (int)e
            });
            ddlCompanyList.DataTextField = "Text";
            ddlCompanyList.DataValueField = "Value";
            ddlCompanyList.DataBind();
            ddlPoolList.DataSource = Enum.GetValues(typeof(PoolpayNoteType)).Cast<PoolpayNoteType>().Select(e => new
            {
                Text = e.GetDescription(),
                Value = (int)e
            });
            ddlPoolList.DataTextField = "Text";
            ddlPoolList.DataValueField = "Value";
            ddlPoolList.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Service.ReleaseNote.Domain.ReleaseNote note = new Service.ReleaseNote.Domain.ReleaseNote();
            note.Title = txtTitle.Text;
            note.Context = txtRemark.Text.Replace("\r\n", "<br />");
            if (radRadB3B.Checked)
            {
                note.Type = CollectCompanyType(ddlCompanyList);
                note.ReleaseType = ReleaseNoteType.B3BVisible;
            }
            else if (radRadPoolpay.Checked)
            {
                note.Type = CollectCompanyType(ddlPoolList);
                note.ReleaseType = ReleaseNoteType.PoolpayVisible;
            }
            note.Creator = CurrentUser.UserName;
            note.UpdateTime = DateTime.Parse(txtTime.Text);
            string str = "";
            if (Request.QueryString["type"] == "update")
            {
                try
                {
                    note.Id = Guid.Parse(Request.QueryString["id"]);
                    ReleaseNoteService.Update(note, CurrentUser.UserName);
                    str = "修改成功";
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "修改更新日志");
                }
            }
            else
            {
                try
                {
                    ReleaseNoteService.Add(note, CurrentUser.UserName);
                    str = "添加成功";
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "添加更新日志");
                }
            }
            RegisterScript("alert('" + str + "');window.location.href='./ReleaseNoteManagement.aspx';", true);
        }

        private CompanyType? CollectCompanyType(CheckBoxList companyList)
        {
            var compnayTypes = (from ListItem li in companyList.Items
                                where li.Selected
                                select int.Parse(li.Value)).ToList();
            if (compnayTypes.Any())
            {
                var defaultType = compnayTypes.First();
                for (int i = 1; i < compnayTypes.Count(); i++)
                {
                    defaultType |= compnayTypes.ElementAt(i);
                }
                return (CompanyType)defaultType;
            }
            return null;
        }
    }
}