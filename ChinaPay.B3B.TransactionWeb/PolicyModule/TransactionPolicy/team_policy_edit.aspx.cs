using System;
using System.Linq;
using System.Web.UI;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class team_policy_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);

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
                cutomeTh.Visible = company != null && company.WorkingSetting != null && company.WorkingSetting.IsImpower;
                ddlCustomCode.Visible = company != null && company.WorkingSetting != null && company.WorkingSetting.IsImpower;

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
                //this.txtArrivalAirports.InitData(true, FoundationService.Airports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));


                bool allowBrotherPurchases = false;
                if (company != null)
                {
                    allowBrotherPurchases = company.Parameter.AllowBrotherPurchase;
                }

                tonghangTh.Visible = allowBrotherPurchases;
                tonghang.Visible = allowBrotherPurchases;

                dropOffice.DataSource = CompanyService.QueryOfficeNumbers(this.CurrentCompany.CompanyId);
                dropOffice.DataTextField = "Number";
                dropOffice.DataValueField = "Impower";
                dropOffice.DataBind();

                ddlCustomCode.DataSource = CompanyService.GetCustomNumberByEmployee(CurrentUser.Id);
                ddlCustomCode.DataTextField = "Number";
                ddlCustomCode.DataValueField = "Number";
                ddlCustomCode.DataBind();


                if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
                {
                    if (Request.QueryString["Type"].Trim() == "Update")
                    {
                        tip.InnerText = "修改团队政策";
                        chkAuto.Visible = false;
                        btnCopy.Visible = false;
                        btnModify.Visible = true;
                        lblAirline.Visible = true;
                        ddlAirline.Visible = false;
                    }
                    if (Request.QueryString["Type"].Trim() == "Copy")
                    {
                        tip.InnerText = "复制团队政策";
                        chkAuto.Visible = true;
                        btnCopy.Visible = true;
                        btnModify.Visible = false;
                        lblAirline.Visible = false;
                        ddlAirline.Visible = true;
                        string[] strIds = setting.Airlines.Split('/');
                        ddlAirline.DataSource = from item in ChinaPay.B3B.Service.FoundationService.Airlines
                                                where item.Valid && strIds.Contains(item.Code.Value)
                                                select new
                                                {
                                                    Code = item.Code,
                                                    Name = item.Code + "-" + item.ShortName
                                                };
                        ddlAirline.DataTextField = "Name";
                        ddlAirline.DataValueField = "Code";
                        ddlAirline.DataBind();
                    }

                    TeamPolicy team = PolicyManageService.GetTeamPolicy(Guid.Parse(Request.QueryString["Id"]));
                    InitDataValues(team);
                }
            }

        }

        private void InitDataValues(TeamPolicy team)
        {
            chkTicket.Text = team.TicketType == TicketType.B2B ? "B2B" : "BSP";
            //航空公司
            this.lblAirline.Text = team.Airline;
            ddlAirline.SelectedValue = team.Airline;
            //去程航班开始时间
            this.txtDepartrueStart.Text = team.DepartureDateStart.ToString("yyyy-MM-dd");
            //去程航班结束时间
            this.txtDepartrueEnd.Text = team.DepartureDateEnd.ToString("yyyy-MM-dd");

            //去程航班
            this.txtDepartrueFilght.Text = team.DepartureFlightsFilter;
            if (team.VoyageType != VoyageType.OneWay)
            {
                //回程航班
                this.txtReturnFilght.Text = team.ReturnFlightsFilter;
                //回程航班类型
                if (team.ReturnFlightsFilterType == Common.Enums.LimitType.None)
                {
                    radReturnBuXian.Checked = true;
                }
                if (team.ReturnFlightsFilterType == Common.Enums.LimitType.Include)
                {
                    radReturnYiXia.Checked = true;
                }
                if (team.ReturnFlightsFilterType == Common.Enums.LimitType.Exclude)
                {
                    radReturnBuYiXia.Checked = true;
                }
            }
            else
            {
                this.returnFilghtDates.Style.Add(HtmlTextWriterStyle.Display, "none");
                this.wangfan.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            //舱位
            IEnumerable<string> listBunks = null;
            //行程类型
            if (team.VoyageType == Common.Enums.VoyageType.OneWay)
            {
                ddlc.Visible = false;
                this.titlePolicy.InnerText = "单程";
                listBunks = QueryTeamBunks(team.Airline, team.DepartureDateStart, team.DepartureDateEnd, team.StartPrintDate, VoyageTypeValue.OneWay);
            }
            if (team.VoyageType == Common.Enums.VoyageType.RoundTrip)
            {
                ddlc.Visible = false;
                paichutishi.InnerText = "提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。";
                this.titlePolicy.InnerText = "往返";
                quchengFlight.InnerText = "去程航班";
                huichengFlight.InnerText = "回程航班";
                chkRound.Text = "适用于往返降舱政策";
                listBunks = QueryTeamBunks(team.Airline, team.DepartureDateStart, team.DepartureDateEnd, team.StartPrintDate, VoyageTypeValue.RoundTrip);
            }
            //if (team.VoyageType == Common.Enums.VoyageType.OneWayOrRound)
            //{
            //    ddlc.Visible = false;
            //    paichutishi.InnerText = "提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。";
            //    this.titlePolicy.InnerText = "单程·往返";
            //    quchengFlight.InnerText = "去程航班";
            //    huichengFlight.InnerText = "回程航班";
            //    chkRound.Text = "适用于往返降舱政策";
            //    listBunks = QueryTeamBunks(team.Airline, team.DepartureDateStart, team.DepartureDateEnd, team.StartPrintDate, VoyageTypeValue.RoundTrip);
            //}
            if (team.VoyageType == Common.Enums.VoyageType.TransitWay)
            {
                paichutishi.InnerText = "提示： 输入排除航线，多条航线之间用“ / ”隔开，（如：昆明到广州到杭州不适用，填写KMGCANHGH）";
                this.titlePolicy.InnerText = "中转联程";
                zhongzhuandi.InnerText = "中转地";
                quchengFlight.InnerText = "第一程航班";
                huichengFlight.InnerText = "第二程航班";
                chkRound.Text = "适用于联程降舱政策";
                duihuan.Visible = false;
                zhongzhuanTh.Visible = true;
                listBunks = QueryTeamBunks(team.Airline, team.DepartureDateStart, team.DepartureDateEnd, team.StartPrintDate, VoyageTypeValue.TransitWay);
            }
            //开始出票时间
            this.txtProvideDate.Text = team.StartPrintDate.ToString("yyyy-MM-dd");
            for (int i = 0; i < dropOffice.Items.Count; i++)
            {
                if (team.OfficeCode == dropOffice.Items[i].Text)
                {
                    dropOffice.Items[i].Selected = true;
                    break;
                }
            }
            //排除航线
            this.txtExceptAirways.Text = team.ExceptAirways;
            this.chkAuto.Checked = team.AutoAudit;
            this.chkChangePNR.Checked = team.ChangePNR;
            this.chkRound.Checked = team.SuitReduce;
            this.chkPrintBeforeTwoHours.Checked = team.PrintBeforeTwoHours;
            //出票条件
            this.txtDrawerCondition.Text = team.DrawerCondition;
            Bunks.InnerHtml = "";
            hidBunks.Value = team.Berths;
            if (team.AppointBerths)
            {
                zhiding.Checked = true;
            }
            else
            {
                buzhiding.Checked = true;
                string[] bunks = team.Berths.Split(',');
                int j = 0;
                foreach (string item in listBunks)
                {
                    if (j % 4 == 0 && j > 0)
                    {
                        Bunks.InnerHtml += "<br />";
                    }
                    for (int i = 0; i < bunks.Count(); i++)
                    {
                        if (item == bunks[i])
                        {
                            Bunks.InnerHtml += " <input type='checkbox' value='" + bunks[i] + "' checked='checked' />" + bunks[i];
                            break;
                        }
                        if (i + 1 == bunks.Count())
                        {
                            Bunks.InnerHtml += " <input type='checkbox' value='" + item + "' />" + item;
                            break;
                        }
                    }
                    j++;
                }

            }

            //去程航班类型(不限，适用以下，不适用以下)
            if (team.DepartureFlightsFilterType == Common.Enums.LimitType.None)
            {
                radBuXian.Checked = true;
            }
            if (team.DepartureFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radYiXia.Checked = true;
            }
            if (team.DepartureFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radBuYiXia.Checked = true;
            }
            this.txtRemark.Text = team.Remark;
            foreach (var item in team.DepartureWeekFilter.Split(','))
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
            this.txtPaiChu.Text = team.DepartureDateFilter;
            this.txtSubordinateCommission.Text = team.SubordinateCommission == -1 ? "" : (team.SubordinateCommission * 100).TrimInvaidZero();
            this.txtProfessionCommission.Text = team.ProfessionCommission == -1 ? "" : (team.ProfessionCommission * 100).TrimInvaidZero();
            this.txtInternalCommission.Text = team.InternalCommission == -1 ? "" : (team.InternalCommission * 100).TrimInvaidZero();

            ddlCustomCode.SelectedValue = team.CustomCode.Trim();
            lblCustomerCode.Text = team.CustomCode.Trim();
            txtDepartureAirports.InitData(true, team.Departure.Split('/').ToList());
            if (team.VoyageType == VoyageType.TransitWay)
            {
                txtArrivalAirports.InitData(true, team.Transit == null ? null : team.Transit.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, team.Arrival.Split('/').ToList());
            }
            else
            {
                txtArrivalAirports.InitData(true, team.Arrival.Split('/').ToList());
            }
            chkddlc.Checked = team.MultiSuitReduce;
            if (team.TicketType == TicketType.BSP)
                chkPrintBeforeTwoHours.Visible = false;
        }


        protected void btnModify_Click(object sender, EventArgs e)
        {
            if (!GetTeamValue(Request.QueryString["Type"]))
            {
                return;
            }
        }

        private bool GetTeamValue(string type)
        {
            VoyageType voyage = VoyageType.OneWay;
            if (titlePolicy.InnerText == "单程")
            {
                voyage = VoyageType.OneWay;
            }
            else if (titlePolicy.InnerText == "往返")
            {
                voyage = VoyageType.RoundTrip;
            }
            else if (titlePolicy.InnerText == "单程·往返")
            {
                voyage = VoyageType.OneWayOrRound;
            }
            else if (titlePolicy.InnerText == "中转联程")
            {
                voyage = VoyageType.TransitWay;
            }
            LimitType derpartrueFilghtType = LimitType.None;
            if (radYiXia.Checked)
            {
                derpartrueFilghtType = LimitType.Include;
            }
            if (radBuXian.Checked)
            {
                derpartrueFilghtType = LimitType.None;
            }
            if (radBuYiXia.Checked)
            {
                derpartrueFilghtType = LimitType.Exclude;
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

            var teams = PolicyManageService.GetTeamPolicy(Guid.Parse(Request.QueryString["Id"]));
            try
            {
                if (type == "Update")
                {
                    var team = new TeamPolicy
                    {
                        Airline = lblAirline.Text,
                        Arrival = txtArrivalAirports.AirportsCode.ToList().Join("/"),
                        OfficeCode = hidOfficeNo.Value,
                        AutoAudit = chkAuto.Checked,
                        ChangePNR = chkChangePNR.Checked,
                        IsInternal = neibuTh.Visible,
                        IsPeer = tonghang.Visible,
                        CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                        Departure = txtDepartureAirports.AirportsCode.ToList().Join("/"),
                        Transit = "",
                        DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                        DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                        DepartureFlightsFilter = derpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                        DepartureFlightsFilterType = derpartrueFilghtType,
                        Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                        DrawerCondition = txtDrawerCondition.Text.Replace("\r", "").Replace("\n", ""),
                        ReturnFlightsFilter = returnFilghtType == LimitType.None ? "" : txtReturnFilght.Text,
                        ReturnFlightsFilterType = returnFilghtType,
                        SuitReduce = chkRound.Checked,
                        MultiSuitReduce = chkddlc.Checked,
                        StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                        DepartureDateFilter = txtPaiChu.Text,
                        DepartureWeekFilter = weekStr,
                        SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100,
                        ProfessionCommission = tonghang.Visible ? decimal.Parse(txtProfessionCommission.Text) / 100 : -1M,
                        InternalCommission = neibuTh.Visible ? decimal.Parse(txtInternalCommission.Text) / 100 : -1M,
                        ExceptAirways = txtExceptAirways.Text,
                        TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                        Berths = hidBunks.Value,
                        VoyageType = voyage,
                        ImpowerOffice = Convert.ToBoolean(dropOffice.SelectedValue),
                        Audited = teams.Audited,
                        AuditTime = teams.AuditTime,
                        AutoPrint = teams.AutoPrint,
                        CreateTime = teams.CreateTime,
                        Creator = teams.Creator,
                        Freezed = teams.Freezed,
                        Owner = teams.Owner,
                        Suspended = teams.Suspended,
                        Id = Guid.Parse(Request.QueryString["Id"]),
                        AppointBerths = zhiding.Checked,
                        PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked
                    };
                    if (zhongzhuanTh.Visible)
                    {
                        team.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        team.Transit = txtArrivalAirports.AirportsCode.Join("/");
                    }
                    if (team.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (team.VoyageType == VoyageType.TransitWay && team.Transit.Trim() == "")
                    {
                        RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (team.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    PolicyManageService.UpdateTeamPolicy(team, this.CurrentUser.UserName);
                }
                if (type == "Copy")
                {
                    var teamInfo = new TeamPolicyReleaseInfo
                    {
                        BasicInfo = new TeamPolicyBasicInfo
                        {
                            CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                            IsInternal = neibuTh.Visible,
                            IsPeer = tonghang.Visible,
                            Airline = ddlAirline.SelectedValue,
                            Arrival = txtArrivalAirports.AirportsCode.ToList().Join("/"),
                            Transit = "",
                            Departure = txtDepartureAirports.AirportsCode.ToList().Join("/"),
                            DepartureFlightsFilter = derpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                            DepartureFlightsFilterType = derpartrueFilghtType,
                            Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                            DrawerCondition = txtDrawerCondition.Text.Replace("\r", "").Replace("\n", ""),
                            ReturnFlightsFilter = returnFilghtType == LimitType.None ? "" : txtReturnFilght.Text,
                            ReturnFlightsFilterType = returnFilghtType,
                            ExceptAirways = txtExceptAirways.Text,
                            VoyageType = voyage,
                            OfficeCode = hidOfficeNo.Value,
                            ImpowerOffice = Convert.ToBoolean(dropOffice.SelectedValue),
                            Owner = this.CurrentUser.Owner
                        }
                    }; if (zhongzhuanTh.Visible)
                    {
                        teamInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        teamInfo.BasicInfo.Transit = txtArrivalAirports.AirportsCode.Join("/");
                    }
                    var list = new List<TeamPolicyRebateInfo>
                                   {
                                       new TeamPolicyRebateInfo
                                           {
                                               DepartureDateFilter = txtPaiChu.Text,
                                               DepartureWeekFilter = weekStr,
                                               AutoAudit = chkAuto.Checked,
                                               ChangePNR = chkChangePNR.Checked,
                                               DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                               DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                               SuitReduce = chkRound.Checked,
                                               StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                               SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text),
                                               ProfessionCommission = tonghang.Visible ? decimal.Parse(txtProfessionCommission.Text): -1M,
                                               InternalCommission = neibuTh.Visible ? decimal.Parse(txtInternalCommission.Text): -1M,
                                               TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                               Berths = hidBunks.Value,
                                               MultiSuitReduce = chkddlc.Checked,
                                               AppointBerths = zhiding.Checked,
                                                PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked
                                           }
                                   };
                    teamInfo.Rebates = list;
                    if (teamInfo.BasicInfo.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (teamInfo.BasicInfo.VoyageType == VoyageType.TransitWay && teamInfo.BasicInfo.Transit.Trim() == "")
                    {
                        RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (teamInfo.BasicInfo.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    PolicyManageService.ReleaseTeamPolicies(teamInfo, this.CurrentUser.UserName);
                }
                if (Request.QueryString["Check"] == "view")
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./team_policy_view.aspx'"
                            : "alert('复制成功');window.location.href='./team_policy_view.aspx'", true);
                }
                else
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./team_policy_manage.aspx'"
                            : "alert('复制成功');window.location.href='./team_policy_manage.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, type == "Update" ? "修改团队政策" : "复制团队政策");
            }
            return true;
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            if (!GetTeamValue(Request.QueryString["Type"]))
            {
                return;
            }
        }

        //private IEnumerable<string> QueryTeamBunks(string airline, DateTime startTime, DateTime endTime, DateTime startEtdzDate)
        //{
        //    var result = new List<string>();

        //    var list = (from item in FoundationService.Bunks
        //                where item.Valid
        //                    && item.AirlineCode.Value == airline
        //                    && item.FlightBeginDate.Date <= startTime.Date
        //                    && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
        //                    && item.ETDZDate.Date <= startEtdzDate.Date
        //                    && item is TeamBunk
        //                select new
        //                {
        //                    Bunk = item.Code.Value
        //                }).OrderBy(item => item.Bunk);
        //    foreach (var item in list)
        //    {
        //        if (!result.Contains(item.Bunk))
        //        {
        //            result.Add(item.Bunk);
        //        }
        //    }
        //    return result;
        //}
        private IEnumerable<string> QueryTeamBunks(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate, VoyageTypeValue voyage)
        {
            var bunkslist = FoundationService.Bunks;
            if (voyage != VoyageTypeValue.TransitWay)
            {
                var result = new Dictionary<string, decimal>();
                var result1 = new List<string>();
                var bunks = (from item in bunkslist
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & voyage) == voyage)
                                 && ((normalBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                 && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             orderby normalBunk.Discount descending
                             select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                if (voyage == VoyageTypeValue.OneWay)
                {
                    var bunks1 = (from item in bunkslist
                                  let bargainBunk = item as PromotionBunk
                                  where bargainBunk != null
                                      && (bargainBunk.Valid
                                      && bargainBunk.AirlineCode.Value == airline
                                      && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                      && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                      && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                      && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                      && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                                  select bargainBunk);
                    foreach (var item in bunks1)
                    {
                        if (!result1.Contains(item.Code.Value))
                        {
                            result1.Add(item.Code.Value);
                        }
                        foreach (var extended in item.Extended)
                        {
                            if (!result1.Contains(extended))
                            {
                                result1.Add(extended);
                            }
                        }
                    }
                }
                else if (voyage == VoyageTypeValue.RoundTrip)
                {
                    var bunks1 = (from item in bunkslist
                                  let bargainBunk = item as ProductionBunk
                                  where bargainBunk != null
                                      && (bargainBunk.Valid
                                      && bargainBunk.AirlineCode.Value == airline
                                      && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                      && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                      && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                      && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                      && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                                  select bargainBunk);
                    foreach (var item in bunks1)
                    {
                        if (!result1.Contains(item.Code.Value))
                        {
                            result1.Add(item.Code.Value);
                        }
                    }
                }
                var result2 = (from bunk in result
                               orderby bunk.Value descending
                               select bunk.Key).ToList();
                foreach (var item in result1)
                {
                    if (!result2.Contains(item))
                    {
                        result2.Add(item);
                    }
                }
                return result2;
            }
            else
            {
                var result = new Dictionary<string, decimal>();
                var result1 = new List<string>();
                var bunks = (from item in bunkslist
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                 && ((normalBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                 && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             orderby normalBunk.Discount descending
                             select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                bunks = (from item in bunkslist
                         let normalBunk = item as GeneralBunk
                         where normalBunk != null
                             && normalBunk.Valid
                             && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                             && normalBunk.FlightBeginDate.Date <= startTime.Date
                             && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                             && normalBunk.ETDZDate.Date <= startETDZDate.Date
                             && ((normalBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                             && ((normalBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                             && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                         orderby normalBunk.Discount descending
                         select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                var bunks1 = (from item in bunkslist
                              let bargainBunk = item as TransferBunk
                              where bargainBunk != null
                                  && (bargainBunk.Valid
                                  && bargainBunk.AirlineCode.Value == airline
                                  && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                  && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                  && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                  && ((bargainBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                  && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                  && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                              select bargainBunk);
                foreach (var item in bunks1)
                {
                    if (!result1.Contains(item.Code.Value))
                    {
                        result1.Add(item.Code.Value);
                    }
                }
                bunks1 = (from item in bunkslist
                          let bargainBunk = item as TransferBunk
                          where bargainBunk != null
                              && (bargainBunk.Valid
                              && bargainBunk.AirlineCode.Value == airline
                              && bargainBunk.FlightBeginDate.Date <= startTime.Date
                              && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                              && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                              && ((bargainBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                              && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                              && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                          select bargainBunk);
                foreach (var item in bunks1)
                {
                    if (!result1.Contains(item.Code.Value))
                    {
                        result1.Add(item.Code.Value);
                    }
                }

                var result2 = (from bunk in result
                               orderby bunk.Value descending
                               select bunk.Key).ToList();

                foreach (var item in result1)
                {
                    if (!result2.Contains(item))
                    {
                        result2.Add(item);
                    }
                }
                return result2;
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
            {
                Response.Redirect(Request.QueryString["Check"] == "view"
                                      ? "./team_policy_view.aspx"
                                      : "./team_policy_manage.aspx");
            }
        }
    }
}