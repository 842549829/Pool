using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using ChinaPay.B3B.Service.ReleaseNote;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.MaintenanceWeb.ReleaseNote
{
    public partial class ReleaseNoteManagement : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            this.pager.PageSize = 10;
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            InitData(new Pagination() { GetRowCount = true, PageIndex = newPage, PageSize = 10 });
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            InitData(new Pagination() { GetRowCount = true, PageIndex = 1, PageSize = 10 });
            pager.CurrentPageIndex = 1;
        }
        void InitData(Pagination paination)
        {
            var query = ReleaseNoteService.Query(paination, DateTime.Parse(txtStartTime.Text), DateTime.Parse(txtEndTime.Text), hidFanwei.Value == "1" ? ddlType.SelectedIndex != 0 ? (Common.Enums.CompanyType)int.Parse(ddlType.SelectedValue) : (Common.Enums.CompanyType?)null : (Common.Enums.CompanyType?)null, ddlReleaseType.SelectedIndex != 0 ? (Common.Enums.ReleaseNoteType)int.Parse(ddlReleaseType.SelectedValue) : (Common.Enums.ReleaseNoteType?)null);
            gvRecords.DataSource = from item in query
                                   select new
                                   {
                                       item.Context,
                                       item.UpdateTime,
                                       Creator = item.Creator,
                                       item.Title,
                                       item.Id,
                                       Type = Getstr(item.Type, item.ReleaseType)
                                   };
            gvRecords.DataBind();
            if (query.Any())
            {
                this.pager.Visible = true;
                if (paination.GetRowCount)
                {
                    this.pager.RowCount = paination.RowCount;
                }
                showempty.Visible = false;
                gvRecords.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                this.pager.Visible = false;
                showempty.Visible = true;
            }

        }
        private string Getstr(CompanyType? type, ReleaseNoteType relType)
        {
            if (relType == ReleaseNoteType.B3BVisible)
            {
                return "B3B<br />" + (type.HasValue ? string.Join("<br />", Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>().Where(e => (e & type.Value) > 0).Select(e => e.GetDescription())) : string.Empty);
            }
            else if (relType == ReleaseNoteType.PoolpayVisible)
            {
                string str = "";
                if ((CompanyType.Provider & type.Value) > 0)
                {
                    str += "前台可见";
                }
                if ((CompanyType.Purchaser & type.Value) > 0)
                {
                    if (str != "")
                    {
                        str += "<br />";
                    }
                    str += "后台可见";
                }
                return "国付通<br />" + str;
            }
            return "";
        }
        protected void gvRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                try
                {
                    ReleaseNoteService.Delete(Guid.Parse(e.CommandArgument.ToString()), CurrentUser.UserName);
                    InitData(new Pagination() { GetRowCount = true, PageIndex = 1, PageSize = 10 });
                    pager.CurrentPageIndex = 1;
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除");
                }
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("./ReleaseNote.aspx?type=add", true);
        }
    }
}