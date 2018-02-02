using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Check_In : BasePage
    {
        private static readonly int m_pageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCondition("Check_In");
            }
            Pagerl.PageSize = m_pageSize;
            Pagerl.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(Pagerl_CurrentPageChanged);
        }

        void Pagerl_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            refresh(newPage);
        }

        void refresh(int pageIndext)
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = pageIndext,
                GetRowCount = true
            };
            bindDatas(pagination);
        }

        private void bindDatas(Pagination pagination)
        {
            try
            {
                var check_in = from item in FoundationService.Check_Ins
                               where isValidData(item)
                               select item;
                int startRow = pagination.PageSize * (pagination.PageIndex - 1);
                int endRow = pagination.PageSize * pagination.PageIndex;
                var datas = check_in.Take(endRow).Skip(startRow);
                gvCheck_In.DataSource = datas;
                gvCheck_In.DataBind();
                if (datas.Any())
                {
                    Pagerl.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        Pagerl.RowCount = check_in.Count();
                    }
                }
                else {
                    Pagerl.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private bool isValidData(Service.Foundation.Domain.Check_In item)
        {
            if (!string.IsNullOrEmpty(ucAriline.Code) && ucAriline.Code != item.AirlineCode) return false;
            if (!string.IsNullOrEmpty(txtStratDate.Text) && Convert.ToDateTime(txtStratDate.Text) < item.Time) return false; ;
            if (!string.IsNullOrEmpty(txtEndDate.Text) && Convert.ToDateTime(txtEndDate.Text) > item.Time) return false;
            if (!string.IsNullOrEmpty(txtOperator.Text.Trim()) && txtOperator.Text.Trim() != item.Opertor) return false;
            return true;
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            refresh(1);
        }

        protected void gvAirline_RowDeleting(object sender, GridViewDeleteEventArgs e) 
        {
            LinkButton linkButton = gvCheck_In.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if (linkButton != null)
            {
                try
                {
                    Guid id = Guid.Parse(linkButton.CommandArgument);
                    FoundationService.DeleteCheck_In(id, CurrentUser.UserName);
                    RegisterScript("alert('删除成功！'); window.location.href='Check_In.aspx?Search=Back';");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除");
                }
                refresh(1);
            }
        }
    }
}