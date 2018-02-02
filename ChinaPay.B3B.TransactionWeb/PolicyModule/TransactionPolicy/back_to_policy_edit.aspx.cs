using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class back_to_policy_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var company = CompanyService.GetCompanySettingsInfo(CurrentCompany.CompanyId);
                if (company != null && company.Parameter.CanHaveSubordinate)
                {
                    neibuTh.Visible = true;
                    neibufanyong.Visible = true;
                }
                else
                {
                    neibuTh.Visible = false;
                    neibufanyong.Visible = false;
                }
                SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);

                if (setting == null)
                {
                    RegisterScript("alert('还未有任何政策设置信息，不能访问本页面！');window.location.href='/Index.aspx';", true);
                    return;
                }
                if (setting.Airlines == "")
                {
                    RegisterScript("alert('还没有设置航空公司，请先设置航空公司！');window.location.href='/Index.aspx';", true);
                    return;
                }
                this.txtDepartureAirports.InitData(true, ChinaPay.B3B.Service.FoundationService.Airports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                this.txtArrivalAirports.InitData(true, ChinaPay.B3B.Service.FoundationService.Airports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));

                IEnumerable<OfficeNumber> list = null;
                var empowermentOffice = CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId) != null && CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId).IsImpower;

                if (empowermentOffice)
                {
                    list = AccountCombineService.GetOfficeNoByEmployee(this.CurrentUser.Id);
                }
                else
                {
                    list = from item in AccountCombineService.GetOfficeNumbers(this.CurrentCompany.CompanyId)
                           select new OfficeNumber
                           {
                               Impower = false,
                               Company = item.Company,
                               Number = item.Number,
                               Enabled = item.Enabled,
                               Id = item.Id
                           };
                }
                if (list != null)
                {
                    dropOffice.DataSource = list;
                    dropOffice.DataTextField = "Number";
                    dropOffice.DataValueField = "Impower";
                    dropOffice.DataBind();
                }

                //selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.EndorseRegulation);
                //selEndorseRegulation.DataTextField = "Name";
                //selEndorseRegulation.DataValueField = "Name";
                //selEndorseRegulation.DataBind();

                //selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.InvalidRegulation);
                //selInvalidRegulation.DataTextField = "Name";
                //selInvalidRegulation.DataValueField = "Name";
                //selInvalidRegulation.DataBind();

                //selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.RefundRegulation);
                //selRefundRegulation.DataTextField = "Name";
                //selRefundRegulation.DataValueField = "Name";
                //selRefundRegulation.DataBind();

                //selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.ChangeRegulation);
                //selChangeRegulation.DataTextField = "Name";
                //selChangeRegulation.DataValueField = "Name";
                //selChangeRegulation.DataBind();


                if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
                {
                    RoundTripPolicy roundTrip = PolicyManageService.GetRoundTripPolicy(Guid.Parse(Request.QueryString["Id"]));
                    if (Request.QueryString["Type"].Trim() == "Update")
                    {
                        tip.InnerText = "修改往返政策";
                        lblAirline.Visible = true;
                        ddlAirline.Visible = false;
                        chkAuto.Visible = false;
                        btnCopy.Visible = false;
                        btnModify.Visible = true;
                    }
                    if (Request.QueryString["Type"].Trim() == "Copy")
                    {
                        tip.InnerText = "复制往返政策";
                        lblAirline.Visible = false;
                        ddlAirline.Visible = true;
                        chkAuto.Visible = true;
                        btnCopy.Visible = true;
                        btnModify.Visible = false;

                        string[] strIds = setting.Airlines.Split('/');
                        ddlAirline.DataSource = from item in ChinaPay.B3B.Service.FoundationService.Airlines
                                                where item.Valid && strIds.Contains(item.Code.Value)
                                                select new
                                                {
                                                    item.Code,
                                                    Name = item.Code + "-" + item.ShortName
                                                };
                        ddlAirline.DataTextField = "Name";
                        ddlAirline.DataValueField = "Code";
                        ddlAirline.DataBind();


                    }

                    InitDataValue(roundTrip);
                }
            }

        }

        void InitDataValue(RoundTripPolicy roundTrip)
        {
            chkTicket.Text = roundTrip.TicketType == TicketType.B2B ? "B2B" : "BSP";
            //航空公司
            this.lblAirline.Text = roundTrip.Airline;
            this.ddlAirline.SelectedValue = roundTrip.Airline;
            //去程航班开始时间
            this.txtDepartrueStart.Text = roundTrip.DepartureDateStart.ToString("yyyy-MM-dd");
            //去程航班结束时间
            this.txtDepartrueEnd.Text = roundTrip.DepartureDateEnd.ToString("yyyy-MM-dd");

            //去程航班
            this.txtDepartrueFilght.Text = roundTrip.DepartureFlightsFilter;
            //回程开始时间
            this.txtReturnStart.Text = ((DateTime)roundTrip.ReturnDateStart).ToString("yyyy-MM-dd");
            //回程结束时间
            this.txtReturnEnd.Text = ((DateTime)roundTrip.ReturnDateEnd).ToString("yyyy-MM-dd");
            //回程航班
            this.txtReturnFilght.Text = roundTrip.ReturnFlightsFilter;
            //回程航班类型
            if (roundTrip.ReturnFlightsFilterType == Common.Enums.LimitType.None)
            {
                radReturnBuXian.Checked = true;
            }
            if (roundTrip.ReturnFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radReturnYiXia.Checked = true;
            }
            if (roundTrip.ReturnFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radReturnBuYiXia.Checked = true;
            }

            //开始出票时间
            this.txtProvideDate.Text = roundTrip.StartPrintDate.ToString("yyyy-MM-dd");
            this.chkAuto.Checked = roundTrip.AutoAudit;
            this.chkChangePNR.Checked = roundTrip.ChangePNR;
            this.hidBunks.Value = roundTrip.Berths;

            this.txtTiQianDays.Text = roundTrip.BeforehandDays + "";
            this.txtPrice.Text = roundTrip.Price.TrimInvaidZero();
            //去程航班类型(不限，适用以下，不适用以下)
            if (roundTrip.DepartureFlightsFilterType == Common.Enums.LimitType.None)
            {
                radBuXian.Checked = true;
            }
            if (roundTrip.DepartureFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radYiXia.Checked = true;
            }
            if (roundTrip.DepartureFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radBuYiXia.Checked = true;
            }
            this.txtDrawerCondition.Text = roundTrip.DrawerCondition;
            this.txtRemark.Text = roundTrip.Remark;
            this.txtSubordinateCommission.Text = (roundTrip.SubordinateCommission * 100).TrimInvaidZero();
            this.txtProfessionCommission.Text = (roundTrip.ProfessionCommission * 100).TrimInvaidZero();
            this.txtInternalCommission.Text = (roundTrip.InternalCommission * 100).TrimInvaidZero();


            ChinaPay.B3B.Service.Policy.Domain.SetPolicy set = PolicySetService.QuerySetPolicy(this.CurrentCompany.CompanyId);
            txtDepartureAirports.InitData(ChinaPay.B3B.Service.FoundationService.Airports.Where(item => set.Departure.Contains(item.Code.Value)));
            txtArrivalAirports.InitData(ChinaPay.B3B.Service.FoundationService.Airports.Where(item => set.Departure.Contains(item.Code.Value)));

            txtDepartureAirports.AirportsCode = roundTrip.Departure.Split('/').ToList();
            txtArrivalAirports.AirportsCode = roundTrip.Arrival.Split('/').ToList();
            hidBunks.Value = roundTrip.Berths;
            foreach (var item in roundTrip.DepartureWeekFilter.Split(','))
            {
                switch (item)
                {
                    case "1":
                        mon.Checked = true;
                        break;
                    case "2":
                        tue.Checked = true;
                        break;
                    case "3":
                        wed.Checked = true;
                        break;
                    case "4":
                        thur.Checked = true;
                        break;
                    case "5":
                        fri.Checked = true;
                        break;
                    case "6":
                        sat.Checked = true;
                        break;
                    case "7":
                        sun.Checked = true;
                        break;
                }
            }
            this.txtPaiChu.Text = roundTrip.DepartureDateFilter;
            //office号

            for (int i = 0; i < dropOffice.Items.Count; i++)
            {
                if (roundTrip.OfficeCode == dropOffice.Items[i].Text)
                {
                    dropOffice.Items[i].Selected = true;
                    break;
                }
            }
        }


        protected void btnModify_Click(object sender, EventArgs e)
        {
            GetNormalValue(Request.QueryString["Type"]);
        }

        private void GetNormalValue(string type)
        {
            if (hidBunks.Value == "")
            {
                ShowMessage("舱位不能为空！");
                return;
            }
            VoyageType voyage = VoyageType.RoundTrip;

            LimitType DerpartrueFilghtType = LimitType.None;
            if (radYiXia.Checked)
            {
                DerpartrueFilghtType = LimitType.Include;
            }
            if (radBuXian.Checked)
            {
                DerpartrueFilghtType = LimitType.None;
            }
            if (radBuYiXia.Checked)
            {
                DerpartrueFilghtType = LimitType.Exclude;
            }

            LimitType returnFilghtType = LimitType.None;
            if (radReturnYiXia.Checked)
            {
                returnFilghtType = LimitType.Include;
            }
            if (radReturnBuXian.Checked)
            {
                returnFilghtType = LimitType.None;
            }
            if (radReturnBuYiXia.Checked)
            {
                returnFilghtType = LimitType.Exclude;
            }
            string weekStr = "";
            if (mon.Checked)
            {
                weekStr += "1,";
            }
            if (tue.Checked)
            {
                weekStr += "2,";
            }
            if (wed.Checked)
            {
                weekStr += "3,";
            }
            if (thur.Checked)
            {
                weekStr += "4,";
            }
            if (fri.Checked)
            {
                weekStr += "5,";
            }
            if (sat.Checked)
            {
                weekStr += "6,";
            }
            if (sun.Checked)
            {
                weekStr += "7,";
            }
            if (weekStr != "")
            {
                weekStr = weekStr.Substring(0, weekStr.Length - 1);
            }

            RoundTripPolicy roundTrips = PolicyManageService.GetRoundTripPolicy(Guid.Parse(Request.QueryString["Id"]));
            try
            {
                if (type == "Update")
                {
                    var roundTrip = new RoundTripPolicy
                                        {
                                            Airline = lblAirline.Text,
                                            Arrival = txtArrivalAirports.AirportsCode.Join("/"),
                                            Departure = txtDepartureAirports.AirportsCode.Join("/"),
                                            AutoAudit = chkAuto.Checked,
                                            ChangePNR = chkChangePNR.Checked,
                                            DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                            DepartureDatesFilter = "",
                                            DepartureWeekFilter = weekStr,
                                            DepartureDateFilter = txtPaiChu.Text,
                                            DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                            DepartureDatesFilterType = DateMode.Date,
                                            DepartureFlightsFilter = txtDepartrueFilght.Text,
                                            DepartureFlightsFilterType = DerpartrueFilghtType,
                                            ReturnDateStart = DateTime.Parse(txtReturnStart.Text),
                                            ReturnDateEnd = DateTime.Parse(txtReturnEnd.Text),
                                            DrawerCondition = txtDrawerCondition.Text,
                                            Remark = txtRemark.Text,
                                            ReturnDatesFilter = "",
                                            ReturnFlightsFilter = txtReturnFilght.Text,
                                            ReturnFlightsFilterType = returnFilghtType,
                                            StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                            SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100,
                                            ProfessionCommission = decimal.Parse(txtProfessionCommission.Text) / 100,
                                            InternalCommission = decimal.Parse(txtInternalCommission.Text) / 100,
                                            TravelDays = 0,
                                            TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                            Berths = hidBunks.Value,
                                            VoyageType = voyage,
                                            ReturnDatesFilterType = DateMode.Date,
                                            OfficeCode = dropOffice.SelectedItem.Text,
                                            BeforehandDays = short.Parse(txtTiQianDays.Text),
                                            ChangeRegulation = selChangeRegulation.Text,
                                            EndorseRegulation = selEndorseRegulation.Text,
                                            InvalidRegulation = selInvalidRegulation.Text,
                                            RefundRegulation = selRefundRegulation.Text,
                                            Price = decimal.Parse(txtPrice.Text),
                                            Audited = roundTrips.Audited,
                                            AuditTime = roundTrips.AuditTime,
                                            CreateTime = roundTrips.CreateTime,
                                            Creator = roundTrips.Creator,
                                            Freezed = roundTrips.Freezed,
                                            Owner = roundTrips.Owner,
                                            Suspended = roundTrips.Suspended,
                                            Id = Guid.Parse(Request.QueryString["Id"])
                                        };
                    PolicyManageService.UpdateRoundTripPolicy(roundTrip, this.CurrentUser.UserName);
                }
                if (type == "Copy")
                {
                    var roundTripInfo = new RoundTripPolicyReleaseInfo
                    {
                        BasicInfo = new RoundTripPolicyBasicInfo
                        {
                            Airline = ddlAirline.SelectedValue,
                            Arrival = txtArrivalAirports.AirportsCode.Join("/"),
                            Departure = txtDepartureAirports.AirportsCode.Join("/"),
                            DepartureDatesFilter = "",
                            DepartureDatesFilterType = DateMode.Date,
                            DepartureFlightsFilter = txtDepartrueFilght.Text,
                            DepartureFlightsFilterType = DerpartrueFilghtType,
                            Remark = txtRemark.Text,
                            DrawerCondition = txtDrawerCondition.Text,
                            ReturnDatesFilter = "",
                            ReturnFlightsFilter = txtReturnFilght.Text,
                            ReturnFlightsFilterType = returnFilghtType,
                            TravelDays = 0,
                            VoyageType = voyage,
                            ReturnDatesFilterType = DateMode.Date,
                            OfficeCode = dropOffice.SelectedItem.Text,
                            Owner = this.CurrentUser.Owner,
                            BeforehandDays = short.Parse(txtTiQianDays.Text),
                            ChangeRegulation = selChangeRegulation.Text,
                            EndorseRegulation = selEndorseRegulation.Text,
                            InvalidRegulation = selInvalidRegulation.Text,
                            RefundRegulation = selRefundRegulation.Text
                        }
                    };
                    var list = new List<RoundTripPolicyRebateInfo>
                                   {
                                       new RoundTripPolicyRebateInfo
                                           {    
                                               DepartureWeekFilter = weekStr,
                                               DepartureDateFilter = txtPaiChu.Text,
                                               AutoAudit = chkAuto.Checked,
                                               ChangePNR = chkChangePNR.Checked,
                                               DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                               DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                               ReturnDateStart = DateTime.Parse(txtReturnStart.Text),
                                               ReturnDateEnd = DateTime.Parse(txtReturnEnd.Text),
                                               StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                               SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text)/100,
                                               ProfessionCommission = decimal.Parse(txtProfessionCommission.Text)/100,
                                               InternalCommission = decimal.Parse(txtInternalCommission.Text)/100,
                                               TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                               Berths = hidBunks.Value,
                                               Price = decimal.Parse(txtPrice.Text)
                                           }
                                   };
                    roundTripInfo.Rebates = list;
                    PolicyManageService.ReleaseRoundTripPolicies(roundTripInfo, this.CurrentUser.UserName);
                }
                if (Request.QueryString["Check"] == "view")
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./back_to_policy_view.aspx'"
                            : "alert('复制成功');window.location.href='./back_to_policy_view.aspx'", true);
                }
                else
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./back_to_policy_manage.aspx'"
                            : "alert('复制成功');window.location.href='./back_to_policy_manage.aspx'", true);
                }
            }
            catch (FormatException)
            {
                ShowMessage("输入字符格式不正确，请重新输入");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, type == "Update" ? "修改往返政策" : "复制往返政策");
            }
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            GetNormalValue(Request.QueryString["Type"]);
        }

        private List<string> QueryBunks(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate)
        {
            var list = (from item in ChinaPay.B3B.Service.FoundationService.Bunks
                        where item.Valid
                            && item.AirlineCode.Value == airline
                            && (item is ProductionBunk)
                            && item.FlightBeginDate.Date <= startTime.Date
                            && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                            && item.ETDZDate.Date <= startETDZDate.Date
                        select new
                        {
                            Bunk = item.Code.Value
                        });
            var result = new List<string>();
            foreach (var item in list)
            {
                if (result == null || result.Count == 0 || !result.Contains(item.Bunk))
                {
                    result.Add(item.Bunk);
                }
            }
            return result;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
            {
                Response.Redirect(Request.QueryString["Check"] == "view"
                                      ? "./back_to_policy_view.aspx"
                                      : "./back_to_policy_manage.aspx");
            }
        }
    }
}