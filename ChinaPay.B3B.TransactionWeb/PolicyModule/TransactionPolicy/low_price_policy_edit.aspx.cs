using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class low_price_policy_edit : BasePage
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

                bool allowBrotherPurchases = false;
                if (company != null)
                {
                    allowBrotherPurchases = company.Parameter.AllowBrotherPurchase;
                }

                dropOffice.DataSource = CompanyService.QueryOfficeNumbers(this.CurrentCompany.CompanyId);
                dropOffice.DataTextField = "Number";
                dropOffice.DataValueField = "Impower";
                dropOffice.DataBind();

                ddlCustomCode.DataSource = CompanyService.GetCustomNumberByEmployee(CurrentUser.Id);
                ddlCustomCode.DataTextField = "Number";
                ddlCustomCode.DataValueField = "Number";
                ddlCustomCode.DataBind();

                tongTh.Visible = allowBrotherPurchases;
                tong.Visible = allowBrotherPurchases;
                var queryList = FoundationService.Airports;
                this.txtDepartureAirports.InitData(true, queryList.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                //this.txtArrivalAirports.InitData(true, queryList.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                //this.txtZhongzhuanAirports.InitData(true, queryList.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                this.txtShifaAirports.InitData(queryList.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));


                if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
                {
                    BargainPolicy bargain = PolicyManageService.GetBargainPolicy(Guid.Parse(Request.QueryString["Id"]));
                    if (bargain.VoyageType == VoyageType.OneWay)
                    {
                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainOneWayEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainOneWayInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainOneWayRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainOneWayChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }
                    else if (bargain.VoyageType == VoyageType.RoundTrip)
                    {
                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainRoundTripEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainRoundTripInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainRoundTripRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainRoundTripChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }
                    else if (bargain.VoyageType == VoyageType.TransitWay)
                    {
                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainTransitWayEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainTransitWayInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainTransitWayRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.BargainTransitWayChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }



                    if (Request.QueryString["Type"].Trim() == "Update")
                    {
                        tip.InnerText = "修改特价政策";
                        chkAuto.Visible = false;
                        btnCopy.Visible = false;
                        btnModify.Visible = true;
                        lblAirline.Visible = true;
                        ddlAirline.Visible = false;
                    }
                    if (Request.QueryString["Type"].Trim() == "Copy")
                    {
                        tip.InnerText = "复制特价政策";
                        lblAirline.Visible = false;
                        ddlAirline.Visible = true;
                        chkAuto.Visible = true;
                        btnCopy.Visible = true;
                        btnModify.Visible = false;
                        string[] strIds = setting.Airlines.Split('/');
                        ddlAirline.DataSource = from item in FoundationService.Airlines
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
                    //ddlBunks.DataSource = QueryBunks(Bargain.Airline, Bargain.DepartureDateStart, Bargain.DepartureDateEnd);
                    //ddlBunks.DataBind();
                    InitDataValue(bargain);
                }
            }
        }


        void InitDataValue(BargainPolicy bargain)
        {
            chkTicket.Text = bargain.TicketType == TicketType.B2B ? "B2B" : "BSP";
            //航空公司
            this.lblAirline.Text = bargain.Airline;
            ddlAirline.SelectedValue = bargain.Airline;
            //去程航班开始时间
            this.txtDepartrueStart.Text = bargain.DepartureDateStart.ToString("yyyy-MM-dd");
            //去程航班结束时间
            this.txtDepartrueEnd.Text = bargain.DepartureDateEnd.ToString("yyyy-MM-dd");
            //舱位
            // this.txtBunks.Value = Bargain.Berths;

            //去程航班
            this.txtDepartrueFilght.Text = bargain.DepartureFlightsFilter;
            //回程航班
            this.txtReturnFilght.Text = bargain.ReturnFlightsFilter;

            //开始出票时间
            this.txtProvideDate.Text = bargain.StartPrintDate.ToString("yyyy-MM-dd");
            //旅游天数
            this.txtTiQianDays.Text = bargain.BeforehandDays == -1 ? "" : bargain.BeforehandDays + "";
            this.txtMostTiQianDays.Text = bargain.MostBeforehandDays == -1 ? "" : bargain.MostBeforehandDays + "";
            txtOutWithFilght.Text = bargain.ExceptAirways;
            this.chkAuto.Checked = bargain.AutoAudit;
            this.chkChangePNR.Checked = bargain.ChangePNR;
            this.chkPrintBeforeTwoHours.Checked = bargain.PrintBeforeTwoHours;
            hidBunks.Value = bargain.Berths;
            txtChuxing.Text = bargain.TravelDays == -1 ? "" : bargain.TravelDays + "";
            //行程类型
            if (bargain.VoyageType == Common.Enums.VoyageType.OneWay)
            {
                chkddlc.Visible = false;
                paichutishi.InnerText = "提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。";
                titlePolicy.InnerText = "单程";
                qucheng.InnerText = "航班限制";
                dancheng.Visible = true;
                tiqianTh.Visible = true;
                zuiduo.Visible = true;
                shifadi.Visible = false;
                zhongzhuanTh.Visible = false;
                returnFilghtDates.Visible = false;
                chuxingTh.Visible = false;
                txtDepartureAirports.InitData(true, bargain.Departure.Split('/').ToList());
                txtArrivalAirports.InitData(true, bargain.Arrival.Split('/').ToList());
                wangfantishi.Visible = false;
            }
            else if (bargain.VoyageType == Common.Enums.VoyageType.RoundTrip)
            {
                selPrice.Items.Clear();
                selPrice.Items.Add(new System.Web.UI.WebControls.ListItem { Text = "按价格发布", Value = "0" });
                selPrice.Items.Add(new System.Web.UI.WebControls.ListItem { Text = "按返佣发布", Value = "3" });

                chkddlc.Visible = false;
                titlePolicy.InnerText = "往返";
                qucheng.InnerText = "去程航班";
                huicheng.InnerText = "回程航班";
                dancheng.Visible = false;
                duihuan.Visible = false;
                tiqianTh.Visible = true;
                zuiduo.Visible = true;
                shifadi.Visible = true;
                zhongzhuanTh.Visible = true;
                returnFilghtDates.Visible = true;
                chuxingTh.Visible = true;
                if (bargain.Departure.Length > 3)
                {
                    hidShifa.Value = "1";
                    txtShifaAirports.Code = bargain.Arrival;
                    txtZhongzhuanAirports.InitData(true, bargain.Departure.Split('/').ToList());
                }
                else
                {
                    txtShifaAirports.Code = bargain.Departure;
                    txtZhongzhuanAirports.InitData(true, bargain.Arrival.Split('/').ToList());
                }
                //selPrice.Visible = false;
                paichu.Visible = false;
                zejiagetishi.Visible = false;
            }
            else if (bargain.VoyageType == Common.Enums.VoyageType.TransitWay)
            {
                paichutishi.InnerText = "提示： 输入排除航线，多条航线之间用“ / ”隔开，（如：昆明到广州到杭州不适用，填写KMGCANHGH）";
                titlePolicy.InnerText = "中转联程";
                qucheng.InnerText = "第一程航班";
                huicheng.InnerText = "第二程航班";
                zhongzhuandi.InnerText = "中转地";
                dancheng.Visible = true;
                duihuan.Visible = false;
                tiqianTh.Visible = false;
                zuiduo.Visible = false;
                shifadi.Visible = false;
                zhongzhuanTh.Visible = true;
                returnFilghtDates.Visible = true;
                chuxingTh.Visible = false;
                discount.Visible = false;
                discountTh.Visible = false;
                txtDepartureAirports.InitData(true, bargain.Departure.Split('/').ToList());
                txtArrivalAirports.InitData(true, bargain.Transit.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, bargain.Arrival.Split('/').ToList());
                wangfantishi.Visible = false;
                zejiagetishi.Visible = false;
            }
            //去程航班类型(不限，适用以下，不适用以下)
            if (bargain.DepartureFlightsFilterType == Common.Enums.LimitType.None)
            {
                radBuXian.Checked = true;
            }
            if (bargain.DepartureFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radYiXia.Checked = true;
            }
            if (bargain.DepartureFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radBuYiXia.Checked = true;
            }
            //回程航班类型(不限，适用以下，不适用以下)
            if (bargain.ReturnFlightsFilterType == Common.Enums.LimitType.None)
            {
                radReturnBuXian.Checked = true;
            }
            if (bargain.ReturnFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radReturnYiXia.Checked = true;
            }
            if (bargain.ReturnFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radReturnBuYiXia.Checked = true;
            }
            foreach (var item in bargain.DepartureWeekFilter.Split(','))
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
            //价格类型
            if (bargain.PriceType == PriceType.Price)
            {
                selPrice.SelectedIndex = 0;
                txtPrice.Text = bargain.Price == -1 ? "" : bargain.Price.TrimInvaidZero();
                price0.Style.Add(HtmlTextWriterStyle.Display, "");
                discount0.Style.Add(HtmlTextWriterStyle.Display, "none");
                fanyong.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (bargain.PriceType == PriceType.Discount)
            {
                selPrice.SelectedIndex = 1;
                txtDiscount.Text = (bargain.Price * 100).TrimInvaidZero();
                discount0.Style.Add(HtmlTextWriterStyle.Display, "");
                price0.Style.Add(HtmlTextWriterStyle.Display, "none");
                fanyong.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (bargain.VoyageType == VoyageType.OneWay)
            {
                if (bargain.PriceType == PriceType.Commission)
                {
                    selPrice.SelectedIndex = 2;
                    txtDiscount.Text = "";
                    discount0.Style.Add(HtmlTextWriterStyle.Display, "none");
                    price0.Style.Add(HtmlTextWriterStyle.Display, "none");
                    fanyong.Style.Add(HtmlTextWriterStyle.Display, "");
                }
            }
            if (bargain.VoyageType == VoyageType.RoundTrip)
            {
                if (bargain.PriceType == PriceType.Commission)
                {
                    selPrice.SelectedIndex = 1;
                    txtDiscount.Text = "";
                    discount0.Style.Add(HtmlTextWriterStyle.Display, "none");
                    price0.Style.Add(HtmlTextWriterStyle.Display, "none");
                    fanyong.Style.Add(HtmlTextWriterStyle.Display, "");
                }
            }

            this.txtRemark.Text = bargain.Remark;
            this.txtDrawerCondition.Text = bargain.DrawerCondition;
            this.txtSubordinateCommission.Text = bargain.SubordinateCommission == -1 ? "" : (bargain.SubordinateCommission * 100).TrimInvaidZero();
            this.txtProfessionCommission.Text = bargain.ProfessionCommission == -1 ? "" : (bargain.ProfessionCommission * 100).TrimInvaidZero();
            this.txtInternalCommission.Text = bargain.InternalCommission == -1 ? "" : (bargain.InternalCommission * 100).TrimInvaidZero();

            this.selEndorseRegulation.SelectedValue = bargain.EndorseRegulation;
            this.selInvalidRegulation.SelectedValue = bargain.InvalidRegulation;
            this.selRefundRegulation.SelectedValue = bargain.RefundRegulation;
            this.selChangeRegulation.SelectedValue = bargain.ChangeRegulation;

            foreach (var item in bargain.DepartureWeekFilter.Split(','))
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
            this.txtPaiChu.Text = bargain.DepartureDateFilter;
            ddlCustomCode.SelectedValue = bargain.CustomCode.Trim();
            lblCustomerCode.Text = bargain.CustomCode.Trim();
            //office号

            for (int i = 0; i < dropOffice.Items.Count; i++)
            {
                if (bargain.OfficeCode == dropOffice.Items[i].Text.Trim())
                {
                    dropOffice.Items[i].Selected = true;
                    break;
                }
            }
            chkddlc.Checked = bargain.MultiSuitReduce;
            if (bargain.TicketType == TicketType.BSP)
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

            LimitType RetrurnFilghtType = LimitType.None;
            if (radReturnYiXia.Checked)
            {
                RetrurnFilghtType = LimitType.Include;
            }
            if (radReturnBuXian.Checked)
            {
                RetrurnFilghtType = LimitType.None;
            }
            if (radReturnBuYiXia.Checked)
            {
                RetrurnFilghtType = LimitType.Exclude;
            }
            string RetrurnFilght = txtReturnFilght.Text;

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

            try
            {
                if (type == "Update")
                {
                    var bargains = PolicyManageService.GetBargainPolicy(Guid.Parse(Request.QueryString["Id"]));
                    var bargain = new BargainPolicy
                    {
                        OfficeCode = dropOffice.SelectedItem == null ? "" : hidOfficeNo.Value,
                        ImpowerOffice = dropOffice.SelectedItem == null ? false : Convert.ToBoolean(dropOffice.SelectedValue),
                        IsInternal = neibuTh.Visible,
                        IsPeer = tong.Visible,
                        CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                        Airline = lblAirline.Text,
                        Transit = "",
                        //DepartureDatesFilter = "",
                        //DepartureDatesFilterType = DateMode.Date,
                        Arrival = txtArrivalAirports.AirportsCode.Join("/"),
                        AutoAudit = chkAuto.Checked,
                        ChangePNR = chkChangePNR.Checked,
                        Departure = txtDepartureAirports.AirportsCode.Join("/"),
                        DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                        DepartureWeekFilter = weekStr,
                        DepartureDateFilter = txtPaiChu.Text,
                        DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                        DepartureFlightsFilter = DerpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                        DepartureFlightsFilterType = DerpartrueFilghtType,
                        ReturnFlightsFilterType = RetrurnFilghtType,
                        ReturnFlightsFilter = RetrurnFilghtType == LimitType.None ? "" : RetrurnFilght,
                        Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                        ExceptAirways = txtOutWithFilght.Text,
                        DrawerCondition = txtDrawerCondition.Text.Replace("\r", "").Replace("\n", ""),
                        StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                        SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100,
                        ProfessionCommission = tong.Visible ? decimal.Parse(txtProfessionCommission.Text) / 100 : -1M,
                        InternalCommission = neibuTh.Visible ? decimal.Parse(txtInternalCommission.Text) / 100 : -1M,
                        ChangeRegulation = selChangeRegulation.Text,
                        EndorseRegulation = selEndorseRegulation.Text,
                        RefundRegulation = selRefundRegulation.Text,
                        InvalidRegulation = selInvalidRegulation.Text,
                        TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                        Berths = hidBunks.Value,
                        //Price = discount.Visible ? (
                        //    selPrice.SelectedIndex == 0
                        //        ? decimal.Parse(txtPrice.Text == "" ? "-1" : txtPrice.Text)
                        //        : decimal.Parse(txtDiscount.Text == "" ? "-1" : txtDiscount.Text) / 100) : -1,
                        PriceType = discount.Visible && selPrice.Visible ? (PriceType)(int.Parse(selPrice.Value)) : PriceType.Price,
                        BeforehandDays = txtTiQianDays.Text == "" ? (short)0 : short.Parse(txtTiQianDays.Text),
                        MostBeforehandDays = txtMostTiQianDays.Text == "" ? (short)-1 : short.Parse(txtMostTiQianDays.Text),
                        Creator = this.CurrentUser.UserName,
                        CreateTime = bargains.CreateTime,
                        Suspended = bargains.Suspended,
                        AuditTime = bargains.AuditTime,
                        Audited = bargains.Audited,
                        Freezed = bargains.Freezed,
                        Owner = bargains.Owner,
                        MultiSuitReduce = chkddlc.Checked,
                        PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked,
                        Id = Guid.Parse(Request.QueryString["Id"])
                    };
                    if (titlePolicy.InnerText.Trim() == "单程")
                    {
                        bargain.Departure = txtDepartureAirports.AirportsCode.Join("/");
                        bargain.Arrival = txtArrivalAirports.AirportsCode.Join("/");
                        bargain.VoyageType = VoyageType.OneWay;
                        bargain.TravelDays = 0;
                        bargain.Price = selPrice.SelectedIndex == 0 ? decimal.Parse(txtPrice.Text == "" ? "-1" : txtPrice.Text) : (selPrice.SelectedIndex == 1 ? decimal.Parse(txtDiscount.Text == "" ? "-100" : txtDiscount.Text) / 100 : -1);
                    }
                    else if (titlePolicy.InnerText.Trim() == "往返")
                    {
                        if (hidShifa.Value == "" || hidShifa.Value == "2")
                        {
                            bargain.Departure = txtShifaAirports.Code;
                            bargain.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        }
                        else
                        {
                            bargain.Departure = txtZhongzhuanAirports.AirportsCode.Join("/");
                            bargain.Arrival = txtShifaAirports.Code;
                        }
                        bargain.VoyageType = VoyageType.RoundTrip;
                        bargain.TravelDays = short.Parse(txtChuxing.Text == "" ? "0" : txtChuxing.Text);
                        bargain.Price = selPrice.SelectedIndex == 0 ? decimal.Parse(txtPrice.Text == "" ? "-1" : txtPrice.Text) : -1;
                    }
                    else if (titlePolicy.InnerText.Trim() == "中转联程")
                    {
                        bargain.Departure = txtDepartureAirports.AirportsCode.Join("/");
                        bargain.Transit = txtArrivalAirports.AirportsCode.Join("/");
                        bargain.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        bargain.VoyageType = VoyageType.TransitWay;
                        bargain.TravelDays = 0;
                        bargain.Price = -1;
                        bargain.PriceType = PriceType.Commission;
                    }
                    if (bargain.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (bargain.VoyageType == VoyageType.TransitWay && bargain.Transit.Trim() == "")
                    {
                        RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (bargain.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    PolicyManageService.UpdateBargainPolicy(bargain, this.CurrentUser.UserName);
                }
                if (type == "Copy")
                {
                    var bargainInfo = new BargainPolicyReleaseInfo
                    {
                        BasicInfo = new BargainPolicyBasicInfo
                        {
                            OfficeCode = dropOffice.SelectedItem == null ? "" : hidOfficeNo.Value,
                            ImpowerOffice = dropOffice.SelectedItem == null ? false : Convert.ToBoolean(dropOffice.SelectedValue),
                            IsInternal = neibuTh.Visible,
                            IsPeer = tong.Visible,
                            CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                            Airline = ddlAirline.SelectedValue,
                            Arrival = txtArrivalAirports.AirportsCode.Join("/"),
                            Departure = txtDepartureAirports.AirportsCode.Join("/"),
                            //DepartureDatesFilter = "",
                            Transit = "",
                            //DepartureDatesFilterType = DateMode.Date,
                            DepartureFlightsFilter = DerpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                            DepartureFlightsFilterType = DerpartrueFilghtType,
                            ReturnFlightsFilter = RetrurnFilghtType == LimitType.None ? "" : RetrurnFilght,
                            ReturnFlightsFilterType = RetrurnFilghtType,
                            Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                            ExceptAirways = txtOutWithFilght.Text,
                            DrawerCondition = txtDrawerCondition.Text.Replace("\r", "").Replace("\n", ""),
                            ChangeRegulation = selChangeRegulation.Text,
                            EndorseRegulation = selEndorseRegulation.Text,
                            RefundRegulation = selRefundRegulation.Text,
                            InvalidRegulation = selInvalidRegulation.Text,
                            VoyageType = voyage,
                            Owner = this.CurrentUser.Owner
                        }
                    };
                    if (titlePolicy.InnerText.Trim() == "单程")
                    {
                        bargainInfo.BasicInfo.Departure = txtDepartureAirports.AirportsCode.Join("/");
                        bargainInfo.BasicInfo.Arrival = txtArrivalAirports.AirportsCode.Join("/");
                        bargainInfo.BasicInfo.VoyageType = VoyageType.OneWay;
                    }
                    else if (titlePolicy.InnerText.Trim() == "往返")
                    {
                        if (hidShifa.Value == "" || hidShifa.Value == "2")
                        {
                            bargainInfo.BasicInfo.Departure = txtShifaAirports.Code;
                            bargainInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        }
                        else
                        {
                            bargainInfo.BasicInfo.Departure = txtZhongzhuanAirports.AirportsCode.Join("/");
                            bargainInfo.BasicInfo.Arrival = txtShifaAirports.Code;
                        }
                        bargainInfo.BasicInfo.VoyageType = VoyageType.RoundTrip;
                    }
                    else if (titlePolicy.InnerText.Trim() == "中转联程")
                    {
                        bargainInfo.BasicInfo.Departure = txtDepartureAirports.AirportsCode.Join("/");
                        bargainInfo.BasicInfo.Transit = txtArrivalAirports.AirportsCode.Join("/");
                        bargainInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        bargainInfo.BasicInfo.VoyageType = VoyageType.TransitWay;
                    }
                    var list = new List<BargainPolicyRebateInfo>
                                   {
                                       new BargainPolicyRebateInfo
                                           {
                                               BeforehandDays =txtTiQianDays.Text =="" ? (short)0 : short.Parse(txtTiQianDays.Text),
                                               TravelDays = txtChuxing.Text =="" ? (short)0 : short.Parse(txtChuxing.Text),
                                               AutoAudit = chkAuto.Checked,
                                               ChangePNR = chkChangePNR.Checked,
                                               DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                               DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                               StartPrintDate = DateTime.Parse(txtProvideDate.Text),
                                               SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text),
                                               ProfessionCommission = tong.Visible ?  decimal.Parse(txtProfessionCommission.Text):-1M ,
                                               InternalCommission = neibuTh.Visible ? decimal.Parse(txtInternalCommission.Text): -1M ,
                                               TicketType = chkTicket.Text == "B2B" ? TicketType.B2B : TicketType.BSP,
                                               Berths = hidBunks.Value,
                                               DepartureDateFilter = txtPaiChu.Text, 
                                               DepartureWeekFilter = weekStr,
                                               MultiSuitReduce = chkddlc.Checked,
                                               PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked,
                                               MostBeforehandDays = txtMostTiQianDays.Text == "" ? (short)-1 : short.Parse(txtMostTiQianDays.Text),
                                               PriceType = discount.Visible && selPrice.Visible ? (PriceType)(int.Parse(selPrice.Value)) : PriceType.Price,
                                               Price = discount.Visible ? (selPrice.SelectedIndex == 0 ? decimal.Parse(txtPrice.Text == "" ? "-1" : txtPrice.Text) : (selPrice.SelectedIndex == 1 ? decimal.Parse(txtDiscount.Text == "" ? "-100" : txtDiscount.Text): -1)) : -1
                                           }
                                   };
                    bargainInfo.Rebates = list;
                    if (titlePolicy.InnerText.Trim() == "中转联程")
                    {
                        bargainInfo.Rebates[0].TravelDays = 0;
                        bargainInfo.Rebates[0].Price = -1;
                        bargainInfo.Rebates[0].PriceType = PriceType.Commission;
                    }
                    if (bargainInfo.BasicInfo.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (bargainInfo.BasicInfo.VoyageType == VoyageType.TransitWay && bargainInfo.BasicInfo.Transit.Trim() == "")
                    {
                        RegisterScript("alert('中转地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (bargainInfo.BasicInfo.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    PolicyManageService.ReleaseBargainPolicies(bargainInfo, this.CurrentUser.UserName);
                }
                if (Request.QueryString["Check"] == "view")
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./low_price_policy_view.aspx'"
                            : "alert('复制成功');window.location.href='./low_price_policy_view.aspx'", true);
                }

                else
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./low_price_policy_manage.aspx'"
                            : "alert('复制成功');window.location.href='./low_price_policy_manage.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, type == "Update" ? "修改特价政策" : "复制特价政策");
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

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null && Request.QueryString["Type"] != null)
            {
                Response.Redirect(Request.QueryString["Check"] == "view"
                                      ? "./low_price_policy_view.aspx"
                                      : "./low_price_policy_manage.aspx");
            }
        }
    }
}