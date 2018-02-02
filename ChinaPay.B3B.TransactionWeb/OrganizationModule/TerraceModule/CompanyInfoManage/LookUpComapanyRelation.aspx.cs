using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class LookUpComapanyRelation : BasePage
    {
        private int m_pageIndex = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                setBackButton();
                hfdType.Value = string.IsNullOrEmpty(Request.QueryString["IsOem"]) ? "Spreading" : "Purchases";
                this.PagerExtend.Visible = false;
                BindSuperior();
                QueryList(m_pageIndex);
                queryEmploee(m_pageIndex);
            }
            PagerExtend.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(PagerExtend_CurrentPageChanged);
            empoyeePager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(empoyeePager_CurrentPageChanged);
        }

        private void setBackButton()
        {
            string returnUrl = Request.QueryString["returnUrl"] ?? Request.UrlReferrer.AbsoluteUri;
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "CompanyList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            btnGoBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';");
        }
       
        void empoyeePager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            queryEmploee(newPage);
        }

        private void queryEmploee(int newPage)
        {
            var pagination = new Pagination
            {
                GetRowCount = true,
                PageIndex = newPage,
                PageSize = empoyeePager.PageSize
            };
            queryEmployeeList(pagination);
        }

        private void queryEmployeeList(Pagination pagination)
        {

            string companyId = Request.QueryString["CompanyId"];
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                var condition = new EmployeeQueryParameter();
                condition.Owner = Guid.Parse(companyId);
                var staffInfos = EmployeeService.QueryEmployees(condition, pagination);
                rptEmployees.DataSource = staffInfos.Select(p => new
                {
                    p.Name,
                    p.UserName,
                    Gender = p.Gender.GetDescription(),
                    UserRoles = p.RoleName,
                    p.Email,
                    p.Cellphone,
                    p.Enabled,
                    p.Remark,
                    p.Id,
                    p.IsAdministrator
                });
                rptEmployees.DataBind();
                if (staffInfos.Any())
                {
                    empoyeePager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        empoyeePager.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    empoyeePager.Visible = false;
                }
            }
        }
        private void BindSuperior()
        {
            string companyId = Request.QueryString["CompanyId"];
            if (!string.IsNullOrEmpty(companyId))
            {
                Guid id = Guid.Parse(companyId);
                RelationInfo info = CompanyService.QuerySuperior(id);
                superiors.InnerText = "上级用户";
                if (info == null)
                {
                    info = CompanyService.GetSpreader(id);
                    superiors.InnerText = "推广者";
                }
                if (info == null)
                {
                    liSuperiors.Visible = divSuperior.Visible = false;
                }
                else
                {
                    this.lblCompanyType.Text = info.CompanyType.GetDescription();
                    this.lblCompanyShortName.Text = info.AbbreviateName;
                    this.lblLoaction.Text = AddressShow.GetCity(info.City);
                    this.lblContact.Text = info.Contact;
                    this.lblContactPhone.Text = info.ContactPhone;
                    this.lblLoginAccount.Text = info.UserNo;
                    this.lblOpentOnAccountTime.Text = info.RegisterTime.ToString("yyyy-MM-dd");
                }
                var companyInfo = CompanyService.GetCompanyInfo(id);
                if (companyInfo.Type != CompanyType.Provider&& !companyInfo.IsOem)
                {
                    Purchases.Visible  = false;
                }
                var companyParameter = CompanyService.GetCompanyParameter(id);
                if(companyParameter == null||!companyParameter.CanHaveSubordinate)
                {
                    Subordinate.Visible = false;
                }
            }
        }
        void PagerExtend_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            QueryList(newPage);
        }
        private void QueryList(int newPage)
        {
            var pagination = new Pagination
            {
                PageIndex = newPage,
                PageSize = PagerExtend.PageSize,
                GetRowCount = true
            };
            QueryList(pagination);
        }
        private List<DataList> SpreadingList(Pagination pagination, Guid id)
        {
            var condition = new SpreadingQueryParameter();
            condition.Initiator = id;
            if (!string.IsNullOrWhiteSpace(txtUserName.Text))
                condition.UserNo = this.txtUserName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtAbberviateName.Text))
                condition.AbbreviateName = this.txtAbberviateName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
                condition.RegisterTimeStart = DateTime.Parse(this.txtStartDate.Text);
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
                condition.RegisterTimeEnd = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            var spreadingList = CompanyService.GetSpreadingList(condition, pagination).Select(item => new DataList
            {
                Type = item.Type.GetDescription(),
                AbbreviateName = item.AbbreviateName,
                City = AddressShow.GetCity(item.City),
                Contact = item.Contact,
                ContactCellphone = item.ContactCellphone,
                Admin = item.Admin,
                RegisterTime = item.RegisterTime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
            return spreadingList;
        }
        private List<DataList> PurchasesList(Pagination pagination, Guid id)
        {
            var condition = new SubordinateQueryParameter();
            condition.Superior = id;
            condition.RelationshipType = RelationshipType.Distribution;
            if (!string.IsNullOrWhiteSpace(txtUserName.Text))
                condition.UserNo = this.txtUserName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtAbberviateName.Text))
                condition.AbbreviateName = this.txtAbberviateName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
                condition.RegisterTimeStart = DateTime.Parse(this.txtStartDate.Text);
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
                condition.RegisterTimeEnd = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            var spreadingList = CompanyService.GetAllSubordinates(condition, pagination).Select(item => new DataList
            {
                Type = "下级公司",
                AbbreviateName = item.AbbreviateName,
                City = AddressShow.GetCity(item.City),
                Contact = item.Contact,
                ContactCellphone = item.ContactPhone,
                Admin = item.UserNo,
                RegisterTime = item.RegisterTime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
            return spreadingList;
        }
        private List<DataList> SubordinateList(Pagination pagination, Guid id)
        {
            var condition = new SubordinateQueryParameter();
            condition.Superior = id;
            condition.RelationshipType = RelationshipType.Organization;
            if (!string.IsNullOrWhiteSpace(txtUserName.Text))
                condition.UserNo = this.txtUserName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtAbberviateName.Text))
                condition.AbbreviateName = this.txtAbberviateName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
                condition.RegisterTimeStart = DateTime.Parse(this.txtStartDate.Text);
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
                condition.RegisterTimeEnd = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            var spreadingList = CompanyService.GetAllSubordinates(condition, pagination).Select(item => new DataList
            {
                Type = "内部机构",
                AbbreviateName = item.AbbreviateName,
                City = AddressShow.GetCity(item.City),
                Contact = item.Contact,
                ContactCellphone = item.ContactPhone,
                Admin = item.UserNo,
                RegisterTime = item.RegisterTime.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
            return spreadingList;
        }
        private void QueryList(Pagination pagination)
        {
            try
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrEmpty(companyId))
                {
                    Guid id = Guid.Parse(companyId);
                    var starRow = pagination.PageSize * (pagination.PageIndex - 1);
                    var endRow = pagination.PageSize * pagination.PageIndex;
                    List<DataList> datalist = null;
                    if (hfdType.Value == "Spreading")
                    {
                        datalist = SpreadingList(pagination, id);
                    }
                    else if (hfdType.Value == "Purchases")
                    {
                        datalist = PurchasesList(pagination, id);
                    }
                    else if (hfdType.Value == "Subordinate")
                    {
                        datalist = SubordinateList(pagination, id);
                    }
                    reperList.DataSource = datalist;
                    reperList.DataBind();
                    if (datalist.Any())
                    {
                        this.PagerExtend.Visible = true;
                        if (pagination.GetRowCount)
                        {
                            this.PagerExtend.RowCount = pagination.RowCount;
                        }
                    }
                    else
                    {
                        this.PagerExtend.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查看信息");
            }
        }
        protected void btnSerach_Click(object sender, EventArgs e)
        {
            if (PagerExtend.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                {
                    GetRowCount = true,
                    PageIndex = m_pageIndex,
                    PageSize = PagerExtend.PageSize
                };
                QueryList(pagination);
            }
            else
            {
                PagerExtend.CurrentPageIndex = 1;
            }
        }
    }
    public class DataList
    {
        public string Type { get; set; }
        public string AbbreviateName { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string ContactCellphone { get; set; }
        public string Admin { get; set; }
        public string RegisterTime { get; set; }
    }
}