using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea;
using ChinaPay.B3B.Service.SystemSetting;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class MarketingAreaList : BasePage
    {
        //readonly int m_pageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
            }
            //this.pager.PageSize = m_pageSize;
            //this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        //void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        //{
        //    var pagination = new Pagination()
        //    {
        //        PageSize = m_pageSize,
        //        PageIndex = newPage,
        //        GetRowCount = true
        //    };
        //    queryArea(pagination);
        //}

        protected void btnQuery_Click(object sender, EventArgs e)
        {
           // Query();
            if (Valiate())
            {
                queryArea();
            }
        }

        void queryArea()
        {
            try
            {
                var areas = from item in AreaService.Query(getCondition())
                             select new
                             {
                                 Id=item.Id,
                                 Name = item.Name,
                                 Remark = item.Remark
                             };
                this.dataSource.DataSource = areas;
                this.dataSource.DataBind();
                if (!areas.Any())
                {
                    this.emptyDataInfo.Visible = true;
                    // ShowMessage("没有任何符合条件的查询结果");
                }
                else
                {
                    dataSource.HeaderRow.TableSection = TableRowSection.TableHeader;
                    this.emptyDataInfo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private AreaQueryConditon getCondition()
        {
            var condition = new AreaQueryConditon()
            {
                 Name = this.txtAreaName.Text
            };
            return condition;
        }

        //private void Query()
        //{
        //    if (this.pager.CurrentPageIndex == 1)
        //    {
        //        var pagination = new Pagination()
        //        {
        //            PageSize = m_pageSize,
        //            PageIndex = 1,
        //            GetRowCount = true
        //        };
        //        queryArea(pagination);
        //    }
        //    else
        //    {
        //        this.pager.CurrentPageIndex = 1;
        //    }
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AreaAddOrUpdate.aspx");
        }

        protected void btnDetele_Click(object sender, EventArgs e)
        {
            List<Guid> ids = new List<Guid>();
            foreach (GridViewRow gv in this.dataSource.Rows)
            {
                CheckBox chk = (CheckBox)gv.FindControl("chkBox");
                Label lbl = gv.FindControl("lblId") as Label;
                if (chk.Checked)
                {
                    Guid id = Guid.Parse(lbl.Text);
                    ids.Add(id);
                }
            }
            if (ids.Count > 0)
            {
                try
                {
                    AreaService.DeleteArea(ids, this.CurrentUser.Name);
                    ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, "alert('删除成功！ ')", true);
                    btnQuery_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"删除");
                }
            }
        }

        protected void dataSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "del")
            {
                try
                {
                    AreaService.DeleteArea(Guid.Parse(e.CommandArgument.ToString()), this.CurrentUser.Name);
                   // Query();
                    queryArea();
                }
                catch (Exception ex){
                    ShowExceptionMessage(ex,"删除");
                }
            }
        }

        private bool Valiate()
        {
            if (this.txtAreaName.Text.Trim().Length > 25)
            {
                ShowMessage("销售区域名称格式错误！");
                return false;
            }
            return true;
        }
    }
}