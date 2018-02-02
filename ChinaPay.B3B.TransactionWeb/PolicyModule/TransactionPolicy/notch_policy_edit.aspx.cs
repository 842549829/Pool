using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;


namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class notch_policy_edit : BasePage
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
                        tip.InnerText = "修改缺口政策";
                        chkAuto.Visible = false;
                        btnCopy.Visible = false;
                        btnModify.Visible = true;
                        lblAirline.Visible = true;
                        ddlAirline.Visible = false;
                    }
                    if (Request.QueryString["Type"].Trim() == "Copy")
                    {
                        tip.InnerText = "复制缺口政策";
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

                    NotchPolicy notch = PolicyManageService.GetNotchPolicy(Guid.Parse(Request.QueryString["Id"]));
                    InitDataValues(notch);
                }
            }

        }

        private void InitDataValues(NotchPolicy notch)
        {
            string str = "", strH = "";

            foreach (var item in notch.DepartureArrival)
            {
                if (str != "")
                {
                    str += ",";
                }
                strH += "<div class='notch_air'><div class='clearfix'><label>" + (item.IsAllowable ? "适用航段：" : "排除航段：") + "</label><p class='text-auto'>" + item.Departure + "</p></div><div class='clearfix'><label>至：</label><p class='text-auto'>" + item.Arrival + "</p></div><p><input type='button' value='删除' class='btn class2 btnShanchu' airports='" + (item.IsAllowable ? "1" : "0") + "|" + item.Departure + "|" + item.Arrival + "' /></p></div>";
                str += (item.IsAllowable ? "1" : "0") + "|" + item.Departure + "|" + item.Arrival;
            }
            showTxt.InnerHtml = strH;
            inputTxtvalue.Value = str;
            ddlCustomCode.SelectedValue = notch.CustomCode.Trim();
            chkTicket.Text = notch.TicketType == TicketType.B2B ? "B2B" : "BSP";
            //航空公司
            this.lblAirline.Text = notch.Airline;
            ddlAirline.SelectedValue = notch.Airline;
            //去程航班开始时间
            this.txtDepartrueStart.Text = notch.DepartureDateStart.ToString("yyyy-MM-dd");
            //去程航班结束时间
            this.txtDepartrueEnd.Text = notch.DepartureDateEnd.ToString("yyyy-MM-dd");

            //去程航班
            this.txtDepartrueFilght.Text = notch.DepartureFlightsFilter;
            //舱位
            IEnumerable<string> listBunks = QueryBunks(notch.Airline, notch.DepartureDateStart, notch.DepartureDateEnd, notch.StartPrintDate);

            //开始出票时间
            this.txtProvideDate.Text = notch.StartPrintDate.ToString("yyyy-MM-dd");
            for (int i = 0; i < dropOffice.Items.Count; i++)
            {
                if (notch.OfficeCode == dropOffice.Items[i].Text)
                {
                    dropOffice.Items[i].Selected = true;
                    break;
                }
            }
            this.chkAuto.Checked = notch.AutoAudit;
            this.chkChangePNR.Checked = notch.ChangePNR;
            this.chkPrintBeforeTwoHours.Checked = notch.PrintBeforeTwoHours;
            //出票条件
            this.txtDrawerCondition.Text = notch.DrawerCondition;
            Bunks.InnerHtml = "";
            hidBunks.Value = notch.Berths;
            string[] bunks = notch.Berths.Split(',');
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
            if (notch.DepartureFlightsFilterType == Common.Enums.LimitType.None)
            {
                radBuXian.Checked = true;
            }
            if (notch.DepartureFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radYiXia.Checked = true;
            }
            if (notch.DepartureFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radBuYiXia.Checked = true;
            }
            this.txtRemark.Text = notch.Remark;
            foreach (var item in notch.DepartureWeekFilter.Split(','))
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
            this.txtPaiChu.Text = notch.DepartureDateFilter;
            this.txtSubordinateCommission.Text = notch.SubordinateCommission == -1 ? "" : (notch.SubordinateCommission * 100).TrimInvaidZero();
            this.txtProfessionCommission.Text = notch.ProfessionCommission == -1 ? "" : (notch.ProfessionCommission * 100).TrimInvaidZero();
            this.txtInternalCommission.Text = notch.InternalCommission == -1 ? "" : (notch.InternalCommission * 100).TrimInvaidZero();
            lblCustomerCode.Text = notch.CustomCode.Trim();

            if (notch.TicketType == TicketType.BSP)
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
            VoyageType voyage = VoyageType.Notch;

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

            var notchs = PolicyManageService.GetNotchPolicy(Guid.Parse(Request.QueryString["Id"]));
            try
            {
                if (type == "Update")
                {
                    var notch = new NotchPolicy
                                    {
                                        CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                                        Airline = lblAirline.Text,
                                        OfficeCode = hidOfficeNo.Value,
                                        //Arrival = txtArrivalAirports.AirportsCode.ToList().Join("/"),
                                        AutoAudit = chkAuto.Checked,
                                        ChangePNR = chkChangePNR.Checked,
                                        IsInternal = neibuTh.Visible,
                                        IsPeer = tonghang.Visible,
                                        //Departure = txtDepartureAirports.AirportsCode.ToList().Join("/"),
                                        //Transit = "",
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
                                        //ReturnFlightsFilter = returnFilghtType == LimitType.None ? "" : txtReturnFilght.Text,
                                        //ReturnFlightsFilterType = returnFilghtType,
                                        //SuitReduce = chkRound.Checked,
                                        StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                        DepartureDateFilter = txtPaiChu.Text,
                                        DepartureWeekFilter = weekStr,
                                        SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100,
                                        ProfessionCommission = decimal.Parse(txtProfessionCommission.Text) / 100,
                                        InternalCommission = decimal.Parse(txtInternalCommission.Text) / 100,
                                        //MultiSuitReduce = chkddlc.Checked,
                                        ////Vip = 0,
                                        //ExceptAirways = txtExceptAirways.Text,
                                        //TravelDays = 0,
                                        TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                        Berths = hidBunks.Value,
                                        VoyageType = voyage,
                                        //ReturnDatesFilterType = DateMode.Date, 
                                        ImpowerOffice = Convert.ToBoolean(dropOffice.SelectedValue),
                                        Audited = notchs.Audited,
                                        AuditTime = notchs.AuditTime,
                                        //AutoPrint = normals.AutoPrint,
                                        CreateTime = notchs.CreateTime,
                                        Creator = notchs.Creator,
                                        Freezed = notchs.Freezed,
                                        Owner = notchs.Owner,
                                        Suspended = notchs.Suspended,
                                        PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked,
                                        Id = Guid.Parse(Request.QueryString["Id"]),
                                        AbbreviateName = CurrentCompany.AbbreviateName
                                    };
                    //if (zhongzhuanTh.Visible)
                    //{
                    //    normal.Transit = txtArrivalAirports.AirportsCode.Join("/");
                    //    normal.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                    //}
                    //if (normal.Departure.Trim() == "")
                    //{
                    //    RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                    //    return false;
                    //}
                    //if (normal.VoyageType == VoyageType.TransitWay && normal.Transit.Trim() == "")
                    //{
                    //    RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                    //    return false;
                    //}
                    //if (normal.Arrival.Trim() == "")
                    //{
                    //    RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                    //    return false;
                    //}
                    SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);
                    var val = inputTxtvalue.Value;
                    notch.DepartureArrival = new List<Data.DataMapping.NotchPolicyDepartureArrival>();
                    if (val.Trim() != "")
                    {
                        var values = val.Split(',');
                        for (var i = 0; i < values.Count(); i++)
                        {
                            var ite = values[i].Split('|');
                            notch.DepartureArrival.Add(new Data.DataMapping.NotchPolicyDepartureArrival { IsAllowable = ite[0] == "1", Departure = ite[1], Arrival = ite[2] });
                        }
                    }
                    if (!notch.DepartureArrival.Any())
                    {
                        notch.DepartureArrival.Add(new Data.DataMapping.NotchPolicyDepartureArrival { IsAllowable = true, Departure = setting.Departure, Arrival = setting.Departure });
                    }
                    PolicyManageService.UpdateNotchPolicy(notch, this.CurrentUser.UserName);
                }
                if (type == "Copy")
                {
                    var notchInfo = new NotchPolicyReleaseInfo
                    {
                        CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,

                        //IsInternal = neibuTh.Visible,
                        //IsPeer = tonghang.Visible,
                        Airline = ddlAirline.SelectedValue,
                        //Arrival = txtArrivalAirports.AirportsCode.ToList().Join("/"),
                        //Transit = "",
                        //Departure = txtDepartureAirports.AirportsCode.ToList().Join("/"),
                        //DepartureDatesFilter = "",
                        //DepartureDatesFilterType = DateMode.Date,
                        DepartureFlightsFilter = derpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                        DepartureFlightsFilterType = derpartrueFilghtType,
                        Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                        DrawerCondition = txtDrawerCondition.Text.Replace("\r", "").Replace("\n", ""),
                        //ReturnDatesFilter = "",
                        //ReturnFlightsFilter = returnFilghtType == LimitType.None ? "" : txtReturnFilght.Text,
                        //ReturnFlightsFilterType = returnFilghtType,
                        //ExceptAirways = txtExceptAirways.Text,
                        //TravelDays = 0,
                        VoyageType = voyage,
                        //ReturnDatesFilterType = DateMode.Date,
                        OfficeCode = hidOfficeNo.Value,
                        ImpowerOffice = Convert.ToBoolean(dropOffice.SelectedValue),
                        Owner = this.CurrentUser.Owner
                    };
                    //if (zhongzhuanTh.Visible)
                    //{
                    //    normalInfo.BasicInfo.Transit = txtArrivalAirports.AirportsCode.Join("/");
                    //    normalInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                    //}
                    var list = new List<NotchPolicyRebateInfo>
                                   {
                                       new NotchPolicyRebateInfo
                                           {
                                               DepartureDateFilter = txtPaiChu.Text,
                                               DepartureWeekFilter = weekStr,
                                               AutoAudit = chkAuto.Checked,
                                               ChangePNR = chkChangePNR.Checked,
                                               DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                               DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                               //ReturnDateStart =  normals.ReturnDateStart,
                                               //ReturnDateEnd =  normals.ReturnDateEnd,
                                               //SuitReduce = chkRound.Checked,
                                               StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                               SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text),
                                               ProfessionCommission = decimal.Parse(txtProfessionCommission.Text),
                                               InternalCommission = decimal.Parse(txtInternalCommission.Text),
                                               //MultiSuitReduce = chkddlc.Checked,
                                               //Vip = 0,
                                               TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                               Berths = hidBunks.Value,
                                               PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked
                                           }
                                   };
                    notchInfo.RebateInfo = list;
                    //if (normalInfo.BasicInfo.Departure.Trim() == "")
                    //{
                    //    RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                    //    return false;
                    //}
                    //if (normalInfo.BasicInfo.VoyageType == VoyageType.TransitWay && normalInfo.BasicInfo.Transit.Trim() == "")
                    //{
                    //    RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                    //    return false;
                    //}
                    //if (normalInfo.BasicInfo.Arrival.Trim() == "")
                    //{
                    //    RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                    //    return false;
                    //}
                    var val = inputTxtvalue.Value;
                    notchInfo.DepartureArrival = new List<DataTransferObject.Policy.NotchPolicyDepartureArrival>();
                    if (val.Trim() != "")
                    {
                        var values = val.Split(',');
                        for (var i = 0; i < values.Count(); i++)
                        {
                            var ite = values[i].Split('|');
                            notchInfo.DepartureArrival.Add(new DataTransferObject.Policy.NotchPolicyDepartureArrival { IsAllowable = ite[0] == "1", Departure = ite[1], Arrival = ite[2] });
                        }
                    }
                    PolicyManageService.ReleaseNotchPolicies(notchInfo, this.CurrentUser.UserName);
                }
                if (Request.QueryString["Check"] == "view")
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./notch_policy_view.aspx'"
                            : "alert('复制成功');window.location.href='./notch_policy_view.aspx'", true);
                }
                else
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./notch_policy_manage.aspx'"
                            : "alert('复制成功');window.location.href='./notch_policy_manage.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, type == "Update" ? "修改缺口政策" : "复制缺口政策");
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

        private IEnumerable<string> QueryBunks(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate)
        {
            var result = new List<string>();

            var bunks = (from item in FoundationService.Bunks
                         where item.Valid
                             && (item.AirlineCode.IsNullOrEmpty() || item.AirlineCode.Value == airline)
                             && item.FlightBeginDate.Date <= startTime.Date
                             && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                             && item.ETDZDate.Date <= startETDZDate.Date
                             && ((item.VoyageType & VoyageTypeValue.Notch) == VoyageTypeValue.Notch)
                         select item).ToList();
            foreach (var bunk in bunks)
            {
                if (!result.Contains(bunk.Code.Value))
                {
                    result.Add(bunk.Code.Value);
                }
                if (bunk is GeneralBunk)
                {
                    var nor = bunk as GeneralBunk;
                    foreach (var extend in nor.Extended)
                    {
                        if (!result.Contains(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value);
                        }
                    }
                }
                else if (bunk is PromotionBunk)
                {
                    var nor = bunk as PromotionBunk;
                    foreach (var extend in nor.Extended)
                    {
                        if (!result.Contains(extend))
                        {
                            result.Add(extend);
                        }
                    }
                }
            }
            return result;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
            {
                Response.Redirect(Request.QueryString["Check"] == "view"
                                      ? "./notch_policy_view.aspx"
                                      : "./notch_policy_manage.aspx");
            }
        }

    }
}