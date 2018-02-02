using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;
using Izual;
using System.Text;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain
{
    public partial class CompanyWorkInfoMatain : BasePage
    {
        private const string m_BeginTime = "00:00";
        private const string m_EndTime = "23:59";
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var companyInfo = CompanyService.GetCompanyInfo(this.CurrentCompany.CompanyId);
                this.hfdCompanyId.Value = companyInfo.Id.ToString();
                if (!companyInfo.Audited)
                {
                    this.hfdCompanyType.Value = "采购商";
                    GetValue(Common.Enums.CompanyType.Purchaser);
                }
                else
                {
                    this.hfdCompanyType.Value = companyInfo.Type.GetDescription();
                    GetValue(companyInfo.Type);
                }
                BindCity(this.CurrentCompany.CompanyId);
                this.BindWorkingHours();
                BindAirline();
            }
        }

        private void GetValue(Common.Enums.CompanyType companytype)
        {
            if (companytype == Common.Enums.CompanyType.Provider)
            {
                this.customNumber.Visible = true;
                this.officeworkinfo.Visible = true;
                this.fuzerenwoekinfo.Visible = true;
                this.divProviderOffice.Visible = true;
                this.divProviderPerson.Visible = true;
                this.workTimeSet.Visible = true;
                this.divRefundTime.Visible = true;
                this.exceptPurchase.Visible = true;
                this.supplierWorkInfo.Visible = false;
                this.providerWorkInfo.Visible = true;
                //获取负责人信息
                BindPerson(this.CurrentCompany.CompanyId);
                BindAirline(this.CurrentCompany.CompanyId);
                //BindDefaultAirline(this.CurrentCompany.CompanyId);
                BindRefundAduit(this.CurrentCompany.CompanyId);
            }
            else
            {
                this.timeworkinfo.InnerHtml = "工作时间设置";
                this.navTip.InnerHtml = "工作时间设置";
                this.customNumber.Visible = false;
                this.officeworkinfo.Visible = false;
                this.fuzerenwoekinfo.Visible = false;
                this.exceptPurchase.Visible = false;
                this.divRefundTime.Visible = false;
                this.providerWorkInfo.Visible = false;
                this.divProviderOffice.Visible = false;
                this.divProviderPerson.Visible = false;
                this.supplierWorkInfo.Visible = true;
                if (companytype == Common.Enums.CompanyType.Purchaser)
                {
                    this.navTip.InnerHtml = "公司工作信息";
                    this.sel.Visible = false;
                    this.workTime.Visible = false;
                    this.timeworkinfo.Visible = false;
                    this.companyworkinfo.Visible = false;
                    this.supplierWorkInfo.Visible = false;
                    this.workTimeSet.Visible = false;
                    this.tbDrawerCondition.Visible = false;
                }
                this.workTimeSet.Visible = true;
                this.workTimeSet.Style.Add(HtmlTextWriterStyle.Width, "98%");
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

        private void BindAirline(Guid id)
        {
            var policy = PolicySetService.QueryAirlines(id);
            foreach (string item in policy)
            {
                ListItem listItem = new ListItem(item, item);
                listItem.Selected = true;
                this.chklAirlines.Items.Add(listItem);
            }
            this.bindChildernAirline(policy, id);
        }

        /// <summary>
        /// 获取可提供资源的航空公司
        /// </summary>
        private void BindAirline()
        {
            foreach (string item in PolicySetService.QueryAirlines(this.CurrentCompany.CompanyId))
            {
                ListItem lists = new ListItem(item);
                lists.Selected = true;
                this.chklAirline.Items.Add(lists);
            }
        }

        //private void BindDefaultAirline(Guid companyId)
        //{
        //    var workSetting = CompanyService.GetWorkingSetting(companyId);
        //    var policy = PolicySetService.QueryAirlines(companyId);
        //    if (workSetting != null && workSetting.AirlineForDefault != null && workSetting.AirlineForDefault.Length > 0)
        //    {
        //        this.chkDefaultCommission.Checked = true;
        //        this.txtDefaultCommission.Text = (workSetting.RebateForDefault.Value * 100).TrimInvaidZero();
        //    }
        //    string str = "";
        //    bool select = false;
        //    int i = 0;
        //    foreach (var item in policy)
        //    {
        //        i++;
        //        if (workSetting != null && workSetting.AirlineForDefault != null && workSetting.AirlineForDefault.Length > 0 && workSetting.AirlineForDefault.Split('/').Contains(item))
        //            select = true;
        //        if (select)
        //        {
        //            str += "<input type='checkbox' value='" + item + "' checked='true' />" + item;
        //        }
        //        else
        //        {
        //            str += "<input type='checkbox' value='" + item + "' />" + item;
        //        }
        //        if (i % 15 == 0)
        //        {
        //            str += "<br />";
        //        }
        //        select = false;
        //    }
        //    divDefaultAirlines.InnerHtml = str;
        //}

        private void bindChildernAirline(IEnumerable<string> policy, Guid id)
        {
            var childern = CompanyService.GetWorkingSetting(id);
            this.chkChildern.Checked = childern != null && childern.AirlineForChild != null && childern.AirlineForChild.Length > 0;
            string str = "";
            bool select;
            int i = 0;
            foreach (var item in policy)
            {
                i++;
                select = childern != null && childern.AirlineForChild != null &&
                childern.AirlineForChild != null && childern.AirlineForChild.Split('/').Contains(item);
                if (select)
                {
                    str += "<input type='checkbox' value='" + item + "' checked='true' />" + item;
                }
                else
                {
                    str += "<input type='checkbox' value='" + item + "' />" + item;
                }
                if (i % 15 == 0)
                {
                    str += "<br />";
                }
            }
            chklCholdrenDeduction.InnerHtml = str;
        }

        private void BindCity(Guid id)
        {
            WorkingSetting city = CompanyService.GetWorkingSetting(id);
            if (city != null)
            {
                this.Departure.Code = city.DefaultDeparture;
                this.Arrival.Code = city.DefaultArrival;
            }
        }

        private void BindRefundAduit(Guid id)
        {
            WorkingSetting city = CompanyService.GetWorkingSetting(id);
            if (city != null)
            {
                this.chkRefundFinancialAudit.Checked = city.RefundNeedAudit;
                this.chkEmpowermentOffice.Checked = city.IsImpower;
                this.hfdEmpowermentOffice.Value = city.IsImpower.ToString().ToUpper();
                this.chkChildern.Checked = city.RebateForChild.HasValue;
                this.txtCholdrenDeduction.Text = city.RebateForChild.HasValue ? (city.RebateForChild.Value * 100).TrimInvaidZero() : string.Empty;
            }
        }

        /// <summary>
        /// 获取工作时间
        /// </summary>
        private void BindWorkingHours()
        {
            WorkingHours workingHours = CompanyService.GetWorkinghours(this.CurrentCompany.CompanyId) ?? new WorkingHours();
            this.BindWorkingDays(workingHours);
            this.BindRestDays(workingHours);
            this.BindRefundWorkdays(workingHours);
            this.BindRefundRestDays(workingHours);

        }

        /// <summary>
        /// 获取工作日工作时间
        /// </summary>
        private void BindWorkingDays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtWorkdayWorkStart.Text = workingHours.WorkdayWorkStart != Time.MinValue ? workingHours.WorkdayWorkStart.ToString("HH:mm") : m_BeginTime;
            this.txtWorkdayWorkEnd.Text = workingHours.WorkdayWorkEnd != Time.MinValue ? workingHours.WorkdayWorkEnd.ToString("HH:mm") : m_EndTime;
        }

        /// <summary>
        /// 获取周末工作时间
        /// </summary>
        private void BindRestDays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtRestdayWorkStart.Text = workingHours.RestdayWorkStart != Time.MinValue ? workingHours.RestdayWorkStart.ToString("HH:mm") : m_BeginTime;
            this.txtRestdayWorkEnd.Text = workingHours.RestdayWorkEnd != Time.MinValue ? workingHours.RestdayWorkEnd.ToString("HH:mm") : m_EndTime;
        }

        /// <summary>
        /// 获取废票工作日时间
        /// </summary>
        private void BindRefundWorkdays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtWorkdayRefundStart.Text = workingHours.WorkdayRefundStart != Time.MinValue ? workingHours.WorkdayRefundStart.ToString("HH:mm") : m_BeginTime;
            this.txtWorkdayRefundEnd.Text = workingHours.WorkdayRefundEnd != Time.MinValue ? workingHours.WorkdayRefundEnd.ToString("HH:mm") : m_EndTime;
        }

        /// <summary>
        /// 获取废票周末时间
        /// </summary>
        private void BindRefundRestDays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtRestdayRefundStart.Text = workingHours.RestdayRefundStart != Time.MinValue ? workingHours.RestdayRefundStart.ToString("HH:mm") : m_BeginTime;
            this.txtRestdayRefundEnd.Text = workingHours.RestdayRefundEnd != Time.MinValue ? workingHours.RestdayRefundEnd.ToString("HH:mm") : m_EndTime;
        }

        //private void BindDrawCondition()
        //{
        //    var list = CompanyService.QueryDrawditionByCompanyId(CurrentCompany.CompanyId);
        //    StringBuilder str = new StringBuilder();
        //    str.Append("<table>");
        //    str.Append("<tr><th>序号</th><th>标题</th><th>内容</th><th>操作</th></tr>");
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        var dition = list[i];
        //        str.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td> <span class='context'>{3}</span></td></tr>", i + 1, dition.Title, dition.Context, "<a href='javascript:UdpateDition(\'" + dition.Id + "\',\'" + dition.Title + "\',\'" + dition.Context + "\');'>修改</a>&nbsp;&nbsp;&nbsp;<a href='javascript:DelDition(\'" + dition.Id + "\')'>删除</a>");
        //    }
        //    str.Append("</table>");
        //    tbDrawerCondition.InnerHtml = str.ToString();
        //}
    }
}