using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Data.DataMapping;
namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain
{
    public partial class AgentMaintain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Guid id = this.CurrentCompany.CompanyId;
                this.BindCompanyInfo(CompanyService.GetCompanyDetail(id));
                this.BindOffice(id);
                this.BindOfficeList(id);
                this.BindAirline(id);
                this.BindCity(id);
                this.BindPerson(id);
            }
        }
        private void BindOffice(Guid id)
        {
            var city = CompanyService.GetWorkingSetting(id);
            foreach (var item in CompanyService.QueryOfficeNumbers(id))
            {
                this.ddlOffice.Items.Add(new ListItem(item.Number, item.Number));
            }
            if (city != null && city.DefaultOfficeNumber != null)
                this.ddlOffice.SelectedValue = city.DefaultOfficeNumber;
            this.ddlOffice.Items.Insert(0, new ListItem("-请选择-", "0"));
        }
        private void BindCompanyInfo(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblCompanyName.Text = info.CompanyName;
            this.lblCompanyShortName.Text = info.AbbreviateName;
            this.lblAddress.Text = AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.lblBeginDeadline.Text = info.PeriodStartOfUse.Value.ToString("yyyy-MM-dd");
            this.lblEndDeadline.Text = info.PeriodEndOfUse.Value.ToString("yyyy-MM-dd");
            this.lblCompanyAddress.Text = info.Address;
            this.txtPostCode.Text = info.ZipCode;
            this.txtCompanyPhone.Text = info.OfficePhones;
            this.txtFaxes.Text = info.Faxes;
            this.lblPrincipal.Text = info.ManagerName;
            this.lblPrincipalPhone.Text = info.ManagerCellphone;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkManPhone.Text = info.ContactPhone;
            this.lblUrgencyLinkMan.Text = info.EmergencyContact;
            this.lblUrgencyLinkManPhone.Text = info.EmergencyCall;
            this.txtEmail.Text = info.ManagerEmail;
            this.txtMSN.Text = info.ManagerMsn;
            this.txtQQ.Text = info.ManagerQQ;
        }
        private void BindCity(Guid id)
        {
            WorkingSetting city = CompanyService.GetWorkingSetting(id);
            if (city != null)
            {
                this.Departure.Code = city.DefaultDeparture;
                this.Arrival.Code = city.DefaultArrival;
                this.chkRefundFinancialAudit.Checked = city.RefundNeedAudit;
                this.txtCholdrenDeduction.Text = city.RebateForChild.HasValue ? (city.RebateForChild.Value * 100).TrimInvaidZero() : string.Empty;
            }
        }
        private void BindOfficeList(Guid id) {
            StringBuilder builder = new StringBuilder(100);
            builder.Append("<table><colgroup><col class='w50'/><col class='w50'/></colgroup><tr><th>序号</th><th>Office</th></tr>");
            var officeNumbers =  CompanyService.QueryOfficeNumbers(id).ToList();
            for (int i = 0; i < officeNumbers.Count(); i++)
               builder.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>",i+1,officeNumbers[i].Number);
            builder.Append("</table>");
            this.divOffice.InnerHtml = builder.ToString();
        }
        private void BindAirline(Guid id)
        {
            var policy = PolicySetService.QueryAirlines(id);
            foreach (string item in policy)
            {
                ListItem listItem = new ListItem(item.ToString());
                listItem.Selected = true;
                this.chklAirlines.Items.Add(listItem);
            }
            this.bindChildernAirline(policy,id);
        }
        private void bindChildernAirline(IEnumerable<string> policy,Guid id)
        {
            var childern = CompanyService.GetWorkingSetting(id);
            this.chkChildern.Checked = childern != null && childern.AirlineForChild.Length > 0;
            foreach (var item in policy)
            {
                ListItem listItem = new ListItem(item.ToString());
                listItem.Selected =  childern != null && childern.AirlineForChild != null && childern.AirlineForChild.Split('/').Contains(item.ToString());
                this.chklCholdrenDeduction.Items.Add(listItem);
            } 
        }
        private void BindPerson(Guid id)
        {
            foreach (var item in CompanyService.GetBusinessManagers(id))
            {
                if (item.BusinessName == "出票") { this.txtDrawerPerson.Text = item.Mamanger; this.txtDrawerCellPhone.Text = item.Cellphone; this.txtDrawerQQ.Text = item.QQ; }
                if (item.BusinessName == "退票") { this.txtRetreatPerson.Text = item.Mamanger; this.txtRetreatCellPhone.Text = item.Cellphone; this.txtRetreatQQ.Text = item.QQ; }
                if (item.BusinessName == "废票") { this.txtWastePerson.Text = item.Mamanger; this.txtWasteCellPhone.Text = item.Cellphone; this.txtWasteQQ.Text = item.QQ; }
                if (item.BusinessName == "改期") { this.txtReschedulingPerson.Text = item.Mamanger; this.txtReschedulingCellPhoen.Text = item.Cellphone; this.txtReschedulingQQ.Text = item.QQ; }
            }
        }
        private CompanyInfo GetCompanyInfo()
        {
            AddressInfo info = AddressShow.GetAddressInfo(this.lblAddress.Text);
            return new CompanyInfo
            {
                CompanyType = ChinaPay.B3B.Common.Enums.CompanyType.Provider,
                Area = info.AreaCode,
                Province = info.ProvinceCode,
                City = info.CityCode,
                District = info.CountyCode,
                CompanyId = this.CurrentCompany.CompanyId,
                ZipCode = this.txtPostCode.Text.Trim(),
                OfficePhones = this.txtCompanyPhone.Text.Trim(),
                Faxes = this.txtFaxes.Text.Trim(),
                ManagerEmail = this.txtEmail.Text.Trim(),
                ManagerMsn = this.txtMSN.Text.Trim(),
                ManagerQQ = this.txtQQ.Text.Trim(),
                Address = lblCompanyAddress.Text,
                PeriodStartOfUse = DateTime.Parse(this.lblBeginDeadline.Text),
                PeriodEndOfUse = DateTime.Parse(this.lblEndDeadline.Text),
                Contact = this.lblPrincipal.Text,
                ContactPhone = this.lblPrincipalPhone.Text,
                EmergencyContact = this.lblUrgencyLinkMan.Text,
                EmergencyCall = this.lblUrgencyLinkManPhone.Text,
                ManagerName = this.lblPrincipal.Text,
                ManagerCellphone = this.lblPrincipalPhone.Text,
                CompanyName = this.lblCompanyName.Text,
                AbbreviateName = this.lblCompanyShortName.Text
            };
        }
        private WorkingSetting GetCitys()
        {
            return new Data.DataMapping.WorkingSetting
            {
                DefaultDeparture = string.IsNullOrEmpty(this.Departure.Code) ? string.Empty :this.Departure.Code,
                DefaultArrival = string.IsNullOrEmpty(this.Arrival.Code)? string.Empty:this.Arrival.Code,
                Company = this.CurrentCompany.CompanyId
            };
        }
        private WorkingSetting GetChilder() {
            WorkingSetting work = this.GetCitys();
            work.RefundNeedAudit = this.chkRefundFinancialAudit.Checked;
            work.DefaultOfficeNumber = this.ddlOffice.SelectedItem.Text;
            if (chkChildern.Checked)
            {
                work.RebateForChild = decimal.Parse(this.txtCholdrenDeduction.Text.Trim()) / 100M;
                var selected = from ListItem item in chklCholdrenDeduction.Items
                               where item.Selected
                               select item.Text;
                work.AirlineForChild = string.Join("/", selected);
            }
            else {
                work.RebateForChild = null;
                work.AirlineForChild = string.Empty;
            }
            return work;
        }
        private IEnumerable<BusinessManager> GteBusinessManager()
        {
            IList<BusinessManager> mamagers = new List<BusinessManager>();
            mamagers.Add(new BusinessManager { BusinessName = "出票", Mamanger = this.txtDrawerPerson.Text.Trim(), Cellphone = this.txtDrawerCellPhone.Text.Trim(), QQ = this.txtDrawerQQ.Text.Trim() });
            mamagers.Add(new BusinessManager { BusinessName = "退票", Mamanger = this.txtRetreatPerson.Text.Trim(), Cellphone = this.txtWasteCellPhone.Text.Trim(), QQ = this.txtRetreatQQ.Text.Trim() });
            mamagers.Add(new BusinessManager { BusinessName = "废票", Mamanger = this.txtWastePerson.Text.Trim(), Cellphone = this.txtWasteCellPhone.Text.Trim(), QQ = this.txtWasteQQ.Text.Trim() });
            mamagers.Add(new BusinessManager { BusinessName = "改期", Mamanger = this.txtReschedulingPerson.Text.Trim(), Cellphone = this.txtReschedulingCellPhoen.Text.Trim(), QQ = this.txtReschedulingQQ.Text.Trim() });
            return mamagers;
        }
        protected void btnSvaeCompanyInfo_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.Update(this.GetCompanyInfo());
                ShowMessage("修改成功");
            }
            catch (InvalidOperationException ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }
        protected void btnSaveChilder_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.SetWorkingSetting(this.GetChilder(),this.CurrentUser.UserName);
                ShowMessage("修改成功");
            }
            catch (Exception ex) { ShowExceptionMessage(ex, "修改"); }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.UpdateAllBusinessManager(this.CurrentCompany.CompanyId, this.GteBusinessManager(),this.CurrentUser.UserName);
                ShowMessage("修改成功");
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex) { ShowExceptionMessage(ex, "修改"); }
        }
    }
}