using System;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class ExternalInterfaceList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                LoadCondition("ExternalInterfaceList");
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = newPage
            };
            queryExternalInterface(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = IsLoacCondition ? pager.CurrentPageIndex : pager.CurrentPageIndex = 1,
                    GetRowCount = true
                };
                queryExternalInterface(pagination);
            }
        }

        private ExternalInterfaceQueryCondition getCondition()
        {
            var condition = new ExternalInterfaceQueryCondition();
            if (!string.IsNullOrWhiteSpace(this.txtAccount.Text))
                condition.UserNo = this.txtAccount.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtAbbreviateName.Text))
                condition.AbbreviateName = this.txtAbbreviateName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtOpenTimeStart.Text))
                condition.OpenTimeStart = DateTime.Parse(this.txtOpenTimeStart.Text);
            if (!string.IsNullOrWhiteSpace(this.txtOpenTimeEnd.Text))
                condition.OpenTimeEnd = DateTime.Parse(this.txtOpenTimeEnd.Text).AddDays(1).AddMilliseconds(-3);
            if (!string.IsNullOrWhiteSpace(this.ddlStatus.SelectedValue))
            {
                if (this.ddlStatus.SelectedValue == "1")
                {
                    condition.IsOpenExternalInterface = true;
                }
                else
                {
                    condition.IsOpenExternalInterface = false;
                }
            }
            return condition;
        }

        private void queryExternalInterface(Pagination pagination)
        {
            try
            {
                var dataSource = Service.Organization.ExternalInterfaceService.Query(getCondition(), pagination);
                this.datalist.DataSource = from item in dataSource
                                           select new
                                           {
                                               UserName = item.UserNo,
                                               CompanyTypeText = item.CompanyType.GetDescription(),
                                               AccountType = item.AccountType.GetDescription(),
                                               AbbreviateName = item.AbbreviateName,
                                               IsOpenExternalInterface = item.IsOpenExternalInterface ? "已启用数据接口" : "未启用数据接口",
                                               OpenTime = item.IsOpenExternalInterface ? item.OpenTime.ToString() : "",
                                               CompanyId = item.CompanyId
                                           };
                this.datalist.DataBind();
                if (dataSource.Count() > 0)
                {
                    this.pager.Visible = true;
                    this.datalist.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                    this.datalist.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private bool valiate()
        {
            if (this.txtAccount.Text.Trim().Length > 30 && !Regex.IsMatch(this.txtAccount.Text.Trim(), @"^\d{0,30}$"))
            {
                ShowMessage("用户名格式错误！");
                return false;
            }
            if (this.txtAbbreviateName.Text.Trim().Length > 10 && !Regex.IsMatch(this.txtAbbreviateName.Text.Trim(), @"^[\u4e00-\u9fa5\w][\u4e00-\u9fa5\w\s\.,]*$"))
            {
                ShowMessage("公司简称格式错误！");
                return false;
            }
            return true;
        }
    }
}