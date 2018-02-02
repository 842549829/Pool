using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup
{
    public partial class AddGroupMember : BasePage
    {
        public Guid GroupId
        {
            get
            {
                string groupId = Request.QueryString["GroupId"];
                return groupId == null ? Guid.Empty : new Guid(groupId);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                LoadCondition("AddGroupMember");
             btnQuery_Click(this, e);
           }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void LoadMembers(Pagination pagination)
        {
            try
            {
                var condition = new CompanyGroupMemberParameter();
                condition.Superior = this.CurrentCompany.CompanyId;
                if (!string.IsNullOrWhiteSpace(this.txtName.Text))
                    condition.CompanyName = this.txtName.Text.Trim();
                if (!string.IsNullOrWhiteSpace(this.txtCompanyAccount.Text))
                    condition.UserNo = this.txtCompanyAccount.Text.Trim();
                if (!string.IsNullOrWhiteSpace(this.txtContract.Text))
                    condition.Contact = this.txtContract.Text.Trim();
                var memberInfos = CompanyService.QueryGroupMemberCanAdd(condition,pagination);

                var subCompanies = from p in memberInfos
                                   join city in FoundationService.Cities on p.City equals city.Code
                                   into groupMemeber
                                   from member in groupMemeber.DefaultIfEmpty()
                                   select new SubordinateCompayInfo
                                       {
                                           CompanyId = p.CompanyId,
                                           CompanyName = p.CompanyName,
                                           UserName = p.UserNo,
                                           City = member != null ? member.Name : string.Empty,
                                           Contact = p.Contact,
                                           ContactPhone = p.ContactPhone,
                                           RegisterTime = p.RegisterTime,
                                       };
                dataList.DataSource = subCompanies;
                dataList.DataBind();
                if (subCompanies.Count() > 0)
                {
                    this.pager.Visible = true;
                    this.dataList.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"查询");
            }
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage) {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = newPage
            };
            LoadMembers(pagination); }


        protected void btnQuery_Click(object sender, EventArgs e) {

            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = 1
            };
            LoadMembers(pagination);
        }


        protected void btnChose_Click(object sender, EventArgs e)
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
                if (ids.Any())
                {
                    ids.ForEach(p => CompanyService.AddGroupMember(GroupId, p,CurrentUser.UserName));
                    Response.Redirect("CompanyGroupMemberList.aspx?groupId="+GroupId);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "添加公司组成员");
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("CompanyGroupMemberList.aspx?groupId=" + GroupId);
        }
    }
}