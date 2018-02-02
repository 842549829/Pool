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
    public partial class CompanyGroupMemberList : BasePage
    {
        protected Guid GroupId
        {
            get
            {
                var groupId = Request.QueryString["GroupId"];
                return groupId == null ? Guid.Empty : new Guid(groupId);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                btnQuery_Click(sender, e);
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
            SetPageNoCache();
        }

        private void LoadMembers(Pagination pagination)
        {
            try
            {
                AddMember.PostBackUrl = "AddGroupMember.aspx?GroupId=" + GroupId;
                var condition = new CompanyGroupMemberParameter();
                condition.GroupId = GroupId;
                if(!string.IsNullOrWhiteSpace(txtName.Text))
                    condition.CompanyName = txtName.Text.Trim();
                if(!string.IsNullOrWhiteSpace(txtCompanyAccount.Text))
                    condition.UserNo = txtCompanyAccount.Text.Trim();
                if(!string.IsNullOrWhiteSpace(txtContract.Text))
                    condition.Contact = txtContract.Text.Trim();
                var members = CompanyService.QueryCompanyGroupListInfo( condition,pagination);
                var companyDetailInfos = (from company in members
                                          join city in FoundationService.Cities on company.City equals city.Code
                                          into memberCompanys
                                          from member in memberCompanys.DefaultIfEmpty()
                                          select new
                                              {
                                                  City = member != null ? member.Name : string.Empty,
                                                  company.CompanyId,
                                                  company.UserNo,
                                                  company.CompanyName,
                                                  company.Contact,
                                                  company.ContactPhone,
                                                  company.Group,
                                                  company.RegisterTime
                                              });
                dataList.DataSource = companyDetailInfos;
                dataList.DataBind();
                if (companyDetailInfos.Any())
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
                SetPageNoCache();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"查询");
            }
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage) {
            var pagination = new Pagination
            {
                 PageIndex = newPage,
                  PageSize = pager.PageSize,
                   GetRowCount = true
            };
            LoadMembers(pagination); }


        protected void btnQuery_Click(object sender, EventArgs e) {
            var pagination = new Pagination
            {
                 GetRowCount = true,
                  PageSize = pager.PageSize,
                   PageIndex =1
            };
            LoadMembers(pagination); }


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
                if (ids.Any())
                {
                    ids.ForEach(id=>CompanyService.DeleteGroupMember(GroupId,id,CurrentUser.UserName));
                    ShowMessage("移除成功!");
                    btnQuery_Click(sender, e);
                }

            }
            catch (Exception ex)
            {

                ShowExceptionMessage(ex, "删除公司组成员");
            }
        }

        internal static Data.DataMapping.CompanyGroup ConvertToGroup(CompanyGroupDetailInfo companyGroup)
        {
            return new Data.DataMapping.CompanyGroup()
                {
                    Id = companyGroup.Id,
                    Name = companyGroup.Name,
                    Description = companyGroup.Description,
                  //  AllowExternalPurchase = companyGroup.AllowExternalPurchase,
                    Company = companyGroup.Company,
                    Creator = companyGroup.Creator,
                    CreateTime = companyGroup.CreateTime,
                    LastModifyTime = DateTime.Now
                };
        }

        protected void btnBack_Click(object sender, EventArgs e) { Response.Redirect("CompanyGroupList.aspx?groupId=" + GroupId); }
    }
}