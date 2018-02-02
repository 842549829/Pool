using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.SystemSetting;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule
{
    public partial class SuggestList : BasePage
    {
        private const string DateFromat = "yyyy-MM-dd";

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtAppliedDateStart.Text = DateTime.Now.AddMonths(-1).ToString(DateFromat);
                txtAppliedDateEnd.Text = DateTime.Now.ToString(DateFromat);
                initData();
            }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void initData()
        {
            ddlSuggestType.DataSource = from SuggestCategory c in Enum.GetValues(typeof(SuggestCategory))
                                        select new
                                            {
                                                Text = c.GetDescription(),
                                                Value = (int)c
                                            };
            ddlSuggestType.DataValueField = "Value";
            ddlSuggestType.DataTextField = "Text";
            ddlSuggestType.DataBind();
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage)
        {
            var pagination = new Pagination
                {
                    PageSize = pager.PageSize,
                    PageIndex = newPage,
                    GetRowCount = true
                };
            queryCompanys(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                    {
                        PageSize = pager.PageSize,
                        PageIndex = 1,
                        GetRowCount = true
                    };
                queryCompanys(pagination);
            }
            else
            {
                pager.CurrentPageIndex = 1;
            }
        }

        private void queryCompanys(Pagination pagination)
        {
            try
            {
                var suggests = SuggestService.GetSuggest(string.IsNullOrWhiteSpace(txtAppliedDateStart.Text) ? (DateTime?)null : DateTime.Parse(txtAppliedDateStart.Text),
                    string.IsNullOrWhiteSpace(txtAppliedDateEnd.Text) ? (DateTime?)null : DateTime.Parse(txtAppliedDateEnd.Text).AddDays(1).AddSeconds(-1),
                    ddlSuggestType.SelectedValue == string.Empty ? null : (SuggestCategory?)int.Parse(ddlSuggestType.SelectedValue)
                    , pagination);
                datalist.DataSource = suggests.Select(s => new
                    {
                        SuggestCategory = s.SuggestCategory.GetDescription(),
                        s.SuggestContent,
                        CreateTime = s.CreateTime.ToString(DateFromat),
                        s.CreatorName,
                        s.Creator,
                        s.ContractInformation,
                        Employee = s.EmployeeId.HasValue ? "<a href='./EmployeeInfo.aspx?employeeId=" + s.EmployeeId.Value + "' >" + s.CreatorName + "</a>" : s.CreatorName
                    });
                datalist.DataBind();
                if (pagination.RowCount > 0)
                {
                    pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        pager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    pager.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }
    }
}