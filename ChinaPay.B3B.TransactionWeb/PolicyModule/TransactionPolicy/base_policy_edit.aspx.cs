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
    public partial class base_policy_edit : BasePage
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
                //this.txtDepartureAirports.InitData(true, ChinaPay.B3B.Service.FoundationService.Airports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                // this.txtArrivalAirports.InitData(true, FoundationService.Airports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));


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
                        tip.InnerText = "修改普通政策";
                        chkAuto.Visible = false;
                        btnCopy.Visible = false;
                        btnModify.Visible = true;
                        lblAirline.Visible = true;
                        ddlAirline.Visible = false;
                    }
                    if (Request.QueryString["Type"].Trim() == "Copy")
                    {
                        tip.InnerText = "复制普通政策";
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

                    NormalPolicy normal = PolicyManageService.GetNormalPolicy(Guid.Parse(Request.QueryString["Id"]));
                    InitDataValues(normal);
                }
            }

        }

        private void InitDataValues(NormalPolicy normal)
        {
            chkTicket.Text = normal.TicketType == TicketType.B2B ? "B2B" : "BSP";
            //航空公司
            this.lblAirline.Text = normal.Airline;
            ddlAirline.SelectedValue = normal.Airline;
            //去程航班开始时间
            this.txtDepartrueStart.Text = normal.DepartureDateStart.ToString("yyyy-MM-dd");
            //去程航班结束时间
            this.txtDepartrueEnd.Text = normal.DepartureDateEnd.ToString("yyyy-MM-dd");

            //去程航班
            this.txtDepartrueFilght.Text = normal.DepartureFlightsFilter;
            //舱位
            IEnumerable<string> listBunks = null;

            if (normal.VoyageType != VoyageType.OneWay)
            {
                ////回程开始时间
                //this.txtReturnStart.Text = ((DateTime)normal.ReturnDateStart).ToString("yyyy-MM-dd");
                ////回程结束时间
                //this.txtReturnEnd.Text = ((DateTime)normal.ReturnDateEnd).ToString("yyyy-MM-dd");
                //回程航班
                this.txtReturnFilght.Text = normal.ReturnFlightsFilter;
                //回程航班类型
                if (normal.ReturnFlightsFilterType == Common.Enums.LimitType.None)
                {
                    radReturnBuXian.Checked = true;
                }
                if (normal.ReturnFlightsFilterType == Common.Enums.LimitType.Include)
                {
                    radReturnYiXia.Checked = true;
                }
                if (normal.ReturnFlightsFilterType == Common.Enums.LimitType.Exclude)
                {
                    radReturnBuYiXia.Checked = true;
                }
            }
            else
            {
                this.returnFilghtDates.Style.Add(HtmlTextWriterStyle.Display, "none");
                this.wangfan.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            //行程类型
            if (normal.VoyageType == Common.Enums.VoyageType.OneWay)
            {
                ddlc.Visible = false;
                this.titlePolicy.InnerText = "单程";
                paichutishi.InnerText = "提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。";
                listBunks = QueryBunks(normal.Airline, normal.DepartureDateStart, normal.DepartureDateEnd, normal.StartPrintDate, VoyageTypeValue.OneWay, true);
            }
            if (normal.VoyageType == Common.Enums.VoyageType.RoundTrip)
            {
                ddlc.Visible = false;
                this.titlePolicy.InnerText = "往返";
                quchengFlight.InnerText = "去程航班";
                huichengFlight.InnerText = "回程航班";
                chkRound.Text = "适用于往返降舱政策";
                paichutishi.InnerText = "提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。";
                listBunks = QueryBunks(normal.Airline, normal.DepartureDateStart, normal.DepartureDateEnd, normal.StartPrintDate, VoyageTypeValue.RoundTrip, true);
            }
            if (normal.VoyageType == Common.Enums.VoyageType.OneWayOrRound)
            {
                ddlc.Visible = false;
                this.titlePolicy.InnerText = "单程·往返";
                quchengFlight.InnerText = "去程航班";
                huichengFlight.InnerText = "回程航班";
                chkRound.Text = "适用于往返降舱政策";
                paichutishi.InnerText = "提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。";
                listBunks = QueryBunks(normal.Airline, normal.DepartureDateStart, normal.DepartureDateEnd, normal.StartPrintDate, VoyageTypeValue.RoundTrip, false);
            }
            if (normal.VoyageType == Common.Enums.VoyageType.TransitWay)
            {
                this.titlePolicy.InnerText = "中转联程";
                zhongzhuandi.InnerText = "中转地";
                quchengFlight.InnerText = "第一程航班";
                huichengFlight.InnerText = "第二程航班";
                chkRound.Text = "适用于联程降舱政策";
                duihuan.Visible = false;
                zhongzhuanTh.Visible = true;
                paichutishi.InnerText = "提示： 输入排除航线，多条航线之间用“ / ”隔开，（如：昆明到广州到杭州不适用，填写KMGCANHGH）";
                listBunks = QueryBunks(normal.Airline, normal.DepartureDateStart, normal.DepartureDateEnd, normal.StartPrintDate, VoyageTypeValue.OneWayOrRound, true);
            }
            //开始出票时间
            this.txtProvideDate.Text = normal.StartPrintDate.ToString("yyyy-MM-dd");
            for (int i = 0; i < dropOffice.Items.Count; i++)
            {
                if (normal.OfficeCode == dropOffice.Items[i].Text)
                {
                    dropOffice.Items[i].Selected = true;
                    break;
                }
            }
            //排除航线
            this.txtExceptAirways.Text = normal.ExceptAirways;
            this.chkAuto.Checked = normal.AutoAudit;
            this.chkChangePNR.Checked = normal.ChangePNR;
            this.chkRound.Checked = normal.SuitReduce;
            this.chkPrintBeforeTwoHours.Checked = normal.PrintBeforeTwoHours;
            //出票条件
            this.txtDrawerCondition.Text = normal.DrawerCondition;
            Bunks.InnerHtml = "";
            hidBunks.Value = normal.Berths;
            string[] bunks = normal.Berths.Split(',');
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
            //去程航班类型(不限，适用以下，不适用以下)
            if (normal.DepartureFlightsFilterType == Common.Enums.LimitType.None)
            {
                radBuXian.Checked = true;
            }
            if (normal.DepartureFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radYiXia.Checked = true;
            }
            if (normal.DepartureFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radBuYiXia.Checked = true;
            }
            this.txtRemark.Text = normal.Remark;
            foreach (var item in normal.DepartureWeekFilter.Split(','))
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
            this.txtPaiChu.Text = normal.DepartureDateFilter;
            this.txtSubordinateCommission.Text = normal.SubordinateCommission == -1 ? "" : (normal.SubordinateCommission * 100).TrimInvaidZero();
            this.txtProfessionCommission.Text = normal.ProfessionCommission == -1 ? "" : (normal.ProfessionCommission * 100).TrimInvaidZero();
            this.txtInternalCommission.Text = normal.InternalCommission == -1 ? "" : (normal.InternalCommission * 100).TrimInvaidZero();
            lblCustomerCode.Text = normal.CustomCode.Trim();
            ddlCustomCode.SelectedValue = normal.CustomCode.Trim();
            txtDepartureAirports.InitData(true,  normal.Departure.Split('/').ToList());
            if (normal.VoyageType == VoyageType.TransitWay)
            {
                txtArrivalAirports.InitData(true, normal.Transit.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, normal.Arrival.Split('/').ToList());
                //txtArrivalAirports.AirportsCode = normal.Transit == null ? null : normal.Transit.Split('/').ToList();
                //txtZhongzhuanAirports.AirportsCode = normal.Arrival.Split('/').ToList();
            }
            else
            {
                txtArrivalAirports.InitData(true, normal.Arrival.Split('/').ToList());
            }
            chkddlc.Checked = normal.MultiSuitReduce;
            if (normal.TicketType == TicketType.BSP)
                chkPrintBeforeTwoHours.Visible = false;
        }


        protected void btnModify_Click(object sender, EventArgs e)
        {
            if (!GetNormalValue(Request.QueryString["Type"]))
            {
                return;
            }
        }

        private bool GetNormalValue(string type)
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

            var normals = PolicyManageService.GetNormalPolicy(Guid.Parse(Request.QueryString["Id"]));
            try
            {
                if (type == "Update")
                {
                    var normal = new NormalPolicy
                                    {
                                        CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                                        Airline = lblAirline.Text,
                                        OfficeCode = hidOfficeNo.Value,
                                        Arrival = txtArrivalAirports.AirportsCode.ToList().Join("/"),
                                        AutoAudit = chkAuto.Checked,
                                        ChangePNR = chkChangePNR.Checked,
                                        IsInternal = neibuTh.Visible,
                                        IsPeer = tonghang.Visible,
                                        Departure = txtDepartureAirports.AirportsCode.ToList().Join("/"),
                                        Transit = "",
                                        DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                        //DepartureDatesFilter = "",
                                        DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                        //DepartureDatesFilterType = DateMode.Date,
                                        DepartureFlightsFilter = derpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                                        DepartureFlightsFilterType = derpartrueFilghtType,
                                        //ReturnDateStart = normals.ReturnDateStart,
                                        //ReturnDateEnd = normals.ReturnDateEnd,
                                        Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                                        DrawerCondition = txtDrawerCondition.Text.Replace("\r", "").Replace("\n", ""),
                                        //ReturnDatesFilter = "",
                                        ReturnFlightsFilter = returnFilghtType == LimitType.None ? "" : txtReturnFilght.Text,
                                        ReturnFlightsFilterType = returnFilghtType,
                                        SuitReduce = chkRound.Checked,
                                        StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                        DepartureDateFilter = txtPaiChu.Text,
                                        DepartureWeekFilter = weekStr,
                                        SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100,
                                        ProfessionCommission = decimal.Parse(txtProfessionCommission.Text) / 100,
                                        InternalCommission = decimal.Parse(txtInternalCommission.Text) / 100,
                                        MultiSuitReduce = chkddlc.Checked,
                                        //Vip = 0,
                                        ExceptAirways = txtExceptAirways.Text,
                                        //TravelDays = 0,
                                        TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                        Berths = hidBunks.Value,
                                        VoyageType = voyage,
                                        //ReturnDatesFilterType = DateMode.Date, 
                                        ImpowerOffice = Convert.ToBoolean(dropOffice.SelectedValue),
                                        Audited = normals.Audited,
                                        AuditTime = normals.AuditTime,
                                        AutoPrint = normals.AutoPrint,
                                        CreateTime = normals.CreateTime,
                                        Creator = normals.Creator,
                                        Freezed = normals.Freezed,
                                        Owner = normals.Owner,
                                        Suspended = normals.Suspended,
                                        PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked,
                                        Id = Guid.Parse(Request.QueryString["Id"])
                                    };
                    if (zhongzhuanTh.Visible)
                    {
                        normal.Transit = txtArrivalAirports.AirportsCode.Join("/");
                        normal.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                    }
                    if (normal.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (normal.VoyageType == VoyageType.TransitWay && normal.Transit.Trim() == "")
                    {
                        RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (normal.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    PolicyManageService.UpdateNormalPolicy(normal, this.CurrentUser.UserName);
                }
                if (type == "Copy")
                {
                    var normalInfo = new NormalPolicyReleaseInfo
                    {
                        BasicInfo = new NormalPolicyBasicInfo
                        {
                            CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                            IsInternal = neibuTh.Visible,
                            IsPeer = tonghang.Visible,
                            Airline = ddlAirline.SelectedValue,
                            Arrival = txtArrivalAirports.AirportsCode.ToList().Join("/"),
                            Transit = "",
                            Departure = txtDepartureAirports.AirportsCode.ToList().Join("/"),
                            //DepartureDatesFilter = "",
                            //DepartureDatesFilterType = DateMode.Date,
                            DepartureFlightsFilter = derpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                            DepartureFlightsFilterType = derpartrueFilghtType,
                            Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                            DrawerCondition = txtDrawerCondition.Text.Replace("\r", "").Replace("\n", ""),
                            //ReturnDatesFilter = "",
                            ReturnFlightsFilter = returnFilghtType == LimitType.None ? "" : txtReturnFilght.Text,
                            ReturnFlightsFilterType = returnFilghtType,
                            ExceptAirways = txtExceptAirways.Text,
                            //TravelDays = 0,
                            VoyageType = voyage,
                            //ReturnDatesFilterType = DateMode.Date,
                            OfficeCode = hidOfficeNo.Value,
                            ImpowerOffice = Convert.ToBoolean(dropOffice.SelectedValue),
                            Owner = this.CurrentUser.Owner
                        }
                    }; if (zhongzhuanTh.Visible)
                    {
                        normalInfo.BasicInfo.Transit = txtArrivalAirports.AirportsCode.Join("/");
                        normalInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                    }
                    var list = new List<NormalPolicyRebateInfo>
                                   {
                                       new NormalPolicyRebateInfo
                                           {
                                               DepartureDateFilter = txtPaiChu.Text,
                                               DepartureWeekFilter = weekStr,
                                               AutoAudit = chkAuto.Checked,
                                               ChangePNR = chkChangePNR.Checked,
                                               DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                               DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                               //ReturnDateStart =  normals.ReturnDateStart,
                                               //ReturnDateEnd =  normals.ReturnDateEnd,
                                               SuitReduce = chkRound.Checked,
                                               StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                               SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text),
                                               ProfessionCommission = decimal.Parse(txtProfessionCommission.Text),
                                               InternalCommission = decimal.Parse(txtInternalCommission.Text),
                                               MultiSuitReduce = chkddlc.Checked,
                                               //Vip = 0,
                                               TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                               Berths = hidBunks.Value,
                                               PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked
                                           }
                                   };
                    normalInfo.Rebates = list;
                    if (normalInfo.BasicInfo.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (normalInfo.BasicInfo.VoyageType == VoyageType.TransitWay && normalInfo.BasicInfo.Transit.Trim() == "")
                    {
                        RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (normalInfo.BasicInfo.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    PolicyManageService.ReleaseNormalPolicies(normalInfo, this.CurrentUser.UserName);
                }
                if (Request.QueryString["Check"] == "view")
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./base_policy_view.aspx'"
                            : "alert('复制成功');window.location.href='./base_policy_view.aspx'", true);
                }
                else
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./base_policy_manage.aspx'"
                            : "alert('复制成功');window.location.href='./base_policy_manage.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, type == "Update" ? "修改普通政策" : "复制普通政策");
            }
            return true;
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            if (!GetNormalValue(Request.QueryString["Type"]))
            {
                return;
            }
        }

        private IEnumerable<string> QueryBunks(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate, VoyageTypeValue voyage, bool isOneWay)
        {
            if (isOneWay && voyage != VoyageTypeValue.OneWayOrRound)
            {
                var result = new Dictionary<string, decimal>();
                var bunks = (from item in FoundationService.Bunks
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & voyage) == voyage)
                                 && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
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
                return (from bunk in result
                        orderby bunk.Value descending
                        select bunk.Key).ToList();



            }
            else if (isOneWay && voyage == VoyageTypeValue.OneWayOrRound)
            {
                var result = new Dictionary<string, decimal>();
                var bunks = (from item in FoundationService.Bunks
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                 && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
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
                bunks = (from item in FoundationService.Bunks
                         let normalBunk = item as GeneralBunk
                         where normalBunk != null
                             && normalBunk.Valid
                             && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                             && normalBunk.FlightBeginDate.Date <= startTime.Date
                             && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                             && normalBunk.ETDZDate.Date <= startETDZDate.Date
                             && ((normalBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                             && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
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
                return (from bunk in result
                        orderby bunk.Value descending
                        select bunk.Key).ToList();
            }
            else
            {
                var result = new Dictionary<string, decimal>();

                var bunks = (from item in FoundationService.Bunks
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & VoyageTypeValue.OneWay) == VoyageTypeValue.OneWay)
                                 && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
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
                bunks = (from item in FoundationService.Bunks
                         let normalBunk = item as GeneralBunk
                         where normalBunk != null
                             && normalBunk.Valid
                             && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                             && normalBunk.FlightBeginDate.Date <= startTime.Date
                             && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                             && normalBunk.ETDZDate.Date <= startETDZDate.Date
                             && ((normalBunk.VoyageType & VoyageTypeValue.RoundTrip) == VoyageTypeValue.RoundTrip)
                             && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
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
                return (from bunk in result
                        orderby bunk.Value descending
                        select bunk.Key).ToList();
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
            {
                Response.Redirect(Request.QueryString["Check"] == "view"
                                      ? "./base_policy_view.aspx"
                                      : "./base_policy_manage.aspx");
            }
        }

    }
}