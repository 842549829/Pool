using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.Service.Announce;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class AnnounceList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitData();
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void InitData()
        {
            this.txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.hfdDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            var info = Enum.GetValues(typeof(AduiteStatus)) as AduiteStatus[];
            foreach(var item in info) {
                this.dropAuditStatus.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.dropAuditStatus.Items.Insert(0,new ListItem("-请选择-",""));
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryAnnounces(pagination);
        }

        void queryAnnounces(Pagination pagination)
        {
            try
            {
                var announces = from item in Service.Announce.AnnounceService.Query(this.CurrentCompany.CompanyId,getCondition(),pagination)
                             select new
                             {
                                Id=item.Id,
                                Title = item.Title,
                                PublishTime = item.PublishTime,
                                PublishAccount = item.PublishAccount,
                                AnnounceType =item.AnnounceLevel.GetDescription(),
                                AduiteStatus = item.AduiteStatus.GetDescription(),
                                AnnouceScope = getAnnouceScope(item.AnnounceScope),
                                IsPlatform = this.CurrentCompany.CompanyType == CompanyType.Platform
                             };
                this.dataSource.DataSource = announces;
                this.dataSource.DataBind();
                if (announces.Any())
                {
                    this.pager.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    dataSource.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        string getAnnouceScope(AnnounceScope announceScope)
        {
            string result = "";
            if (AnnounceScope.B3B == announceScope)
                result = "B3B可见";
            if (AnnounceScope.OEM == announceScope)
                result = "OEM可见";
            if ((AnnounceScope.B3B| AnnounceScope.OEM )== announceScope)
                result = "B3B可见|OEM可见";
            return result;
        }
        private AnnounceQueryCondition getCondition()
        {
            var condition = new AnnounceQueryCondition()
                {
                     Title = this.txtTitle.Text,
                     PublishAccount = this.txtPublishPerson.Text
                };
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
                {
                    condition.PublishTime = new Core.Range<DateTime?>(DateTime.Parse(this.txtStartDate.Text), DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddTicks(-1));
                }
                else
                {
                    condition.PublishTime = new Core.Range<DateTime?>(DateTime.Parse(this.txtStartDate.Text), null);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
                {
                    condition.PublishTime = new Core.Range<DateTime?>(null,DateTime.Parse(this.txtEndDate.Text));
                }
            }
            
            if (!string.IsNullOrWhiteSpace(this.dropAuditStatus.SelectedValue))
            {
                condition.AduiteStatus = (AduiteStatus)int.Parse(this.dropAuditStatus.SelectedValue);
            }
            return condition;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                if (this.pager.CurrentPageIndex == 1)
                {
                    var pagination = new Pagination()
                    {
                        PageSize = pager.PageSize,
                        PageIndex = 1,
                        GetRowCount = true
                    };
                    queryAnnounces(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        protected void btnAduit_Click(object sender, EventArgs e)
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
                    AnnounceService.UpdateAuditStatuses(ids, AduiteStatus.Audited,this.CurrentCompany.CompanyType== Common.Enums.CompanyType.Platform? Common.Enums.PublishRole.平台:Common.Enums.PublishRole.用户, this.CurrentUser.Name);
                    ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, "alert('审核成功！ ')", true);
                    btnQuery_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"审核");
                }
            }
        }

        protected void btnCancelAduit_Click(object sender, EventArgs e)
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
                    AnnounceService.UpdateAuditStatuses(ids, AduiteStatus.UnAudit, this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Platform ? Common.Enums.PublishRole.平台 : Common.Enums.PublishRole.用户, this.CurrentUser.Name);
                    ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, "alert('取消审核成功！ ')", true);
                    btnQuery_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"取消审核");
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
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
                    AnnounceService.Delete(ids, this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Platform ? Common.Enums.PublishRole.平台 : Common.Enums.PublishRole.用户, this.CurrentUser.Name);
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
                    var ids = new List<Guid>();
                    ids.Add(Guid.Parse(e.CommandArgument.ToString()));
                    AnnounceService.Delete(ids, this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Platform ? Common.Enums.PublishRole.平台 : Common.Enums.PublishRole.用户, this.CurrentUser.Name);
                    btnQuery_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"删除");
                }
            }
        }

        private bool Valiate()
        {
            if (!string.IsNullOrWhiteSpace(this.txtTitle.Text))
            {
                if (this.txtTitle.Text.Trim().Length > 100)
                {
                    ShowMessage("标题格式错误！");
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(this.txtPublishPerson.Text))
            {
                if (this.txtPublishPerson.Text.Trim().Length > 20)
                {
                    ShowMessage("发布人格式错误！");
                    return false;
                }
            }
            return true;
        }
    }
}