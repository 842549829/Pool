using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup
{
    public partial class CompanyGroupList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                //txtLower.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                //txtUpper.Text = DateTime.Today.ToString("yyyy-MM-dd");
                LoadCondition("CompanyGroupList");
                btnQuery_Click(this, e);
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;

        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryGroups(pagination);

        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                    {
                        PageSize = pager.PageSize,
                        PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                        GetRowCount = true
                    };
                queryGroups(pagination);
            }
            else {
                pager.CurrentPageIndex = 1;
            }
        }

        private void queryGroups(Pagination pagination)
        {
            try
            {
                var group = CompanyService.QueryCompanyGroupInfo(getCondition(),pagination);
                dataList.DataSource = group.Select(item => new
                {
                    item.Id,
                    item.Name,
                    item.Description,
                    item.AllowExternalPurchase,
                    item.CreateTime,
                    item.Creator,
                    item.MemberCount,
                    LastModifyTime = item.LastModifyTime.HasValue ? item.LastModifyTime.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty,
                    Editable = item.Owner == CurrentCompany.CompanyId

                });
                dataList.DataBind();
                if (group.Any())
                {
                    pager.Visible = true;
                    pager.RowCount = pagination.RowCount;
                }
                else
                {
                    pager.Visible = false;
                }
                SetPageNoCache();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private CompanyGroupQueryParameter getCondition()
        {
            var condition = new CompanyGroupQueryParameter
            {
                Name = txtName.Text.Trim(),
                Creator = txtRegisterAccount.Text.Trim(),
                Owner = CurrentCompany.CompanyId

            };
           if(!string.IsNullOrEmpty(txtLower.Text)) condition.CreateTimeStart = DateTime.Parse(txtLower.Text);
           if (!string.IsNullOrEmpty(txtUpper.Text)) condition.CreateTimeEnd = DateTime.Parse(txtUpper.Text).AddDays(1).AddTicks(-1);
           return condition;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var ids = new List<Guid>();
                foreach (RepeaterItem item in dataList.Items)
                {
                    var check = item.FindControl("checkboxone") as CheckBox;
                    var hdID = item.FindControl("hdID") as HiddenField;
                    if (check == null || hdID == null) continue;
                    if (check.Checked) ids.Add(Guid.Parse(hdID.Value));
                }
                ids.ForEach(id => CompanyService.DeleteCompayGroup(id, CurrentUser.UserName));
                ShowMessage("删除成功！");
                var pagination = new Pagination
               {
                   PageSize = pager.PageSize,
                   PageIndex = pager.CurrentPageIndex,
                   GetRowCount = true
               };
                queryGroups(pagination);
                SetPageNoCache();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "删除公司组");
            }

        }

        protected void dataList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    var id = new Guid(e.CommandArgument.ToString());
                    var result = CompanyService.DeleteCompayGroup(id, CurrentUser.UserName);
                    ShowMessage(string.Format("删除{0}", result ? "成功" : "失败"));
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除公司组");
                }
                var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = pager.CurrentPageIndex,
                    GetRowCount = true
                };
                queryGroups(pagination);
                SetPageNoCache();
            }
        }
    }
}