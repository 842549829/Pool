using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service;


namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class special_policy_edit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                lblServicePhone.Text = CurrenContract.ServicePhone;
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
                var queryAirports = Service.FoundationService.Airports;
                if (queryAirports != null)
                {
                    txtShifaAirports.InitData(true, queryAirports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                    //txtZhongzhuanAirports.InitData(true, queryAirports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                    txtDepartureAirports.InitData(queryAirports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                    //txtArrivalAirports.InitData(queryAirports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                    //txtMuDi.InitData(true, queryAirports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
                }

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

                    SpecialPolicy special = PolicyManageService.GetSpecialPolicy(Guid.Parse(Request.QueryString["Id"]));
                    #region 绑定退改约定
                    if (special.Type == SpecialProductType.Disperse)
                    {
                        //出票条件
                        dropDrawerCondition.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialDisperseDrawerCondition);
                        dropDrawerCondition.DataTextField = "Name";
                        dropDrawerCondition.DataValueField = "Name";
                        dropDrawerCondition.DataBind();

                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialDisperseEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialDisperseInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialDisperseRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialDisperseChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }
                    else if (special.Type == SpecialProductType.Bloc)
                    {
                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBlocEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBlocInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBlocRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBlocChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }
                    else if (special.Type == SpecialProductType.Business)
                    {
                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBusinessEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBusinessInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBusinessRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialBusinessChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }

                    else if (special.Type == SpecialProductType.CostFree)
                    {
                        //出票条件
                        dropDrawerCondition.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialCostFreeDrawerCondition);
                        dropDrawerCondition.DataTextField = "Name";
                        dropDrawerCondition.DataValueField = "Name";
                        dropDrawerCondition.DataBind();

                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialCostFreeEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialCostFreeInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialCostFreeRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialCostFreeChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }

                    else if (special.Type == SpecialProductType.Singleness)
                    {
                        //出票条件
                        dropDrawerCondition.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialSinglenessDrawerCondition);
                        dropDrawerCondition.DataTextField = "Name";
                        dropDrawerCondition.DataValueField = "Name";
                        dropDrawerCondition.DataBind();

                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialSinglenessEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialSinglenessInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialSinglenessRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialSinglenessChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }
                    else if (special.Type == SpecialProductType.OtherSpecial)
                    {
                        //出票条件
                        dropDrawerCondition.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialOtherSpecialDrawerCondition);
                        dropDrawerCondition.DataTextField = "Name";
                        dropDrawerCondition.DataValueField = "Name";
                        dropDrawerCondition.DataBind();

                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialDisperseEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialOtherSpecialInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialOtherSpecialRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialOtherSpecialChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }
                    else if (special.Type == SpecialProductType.LowToHigh)
                    {
                        ////出票条件
                        //dropDrawerCondition.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialOtherSpecialDrawerCondition);
                        //dropDrawerCondition.DataTextField = "Name";
                        //dropDrawerCondition.DataValueField = "Name";
                        //dropDrawerCondition.DataBind();

                        selEndorseRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialLowToHighEndorseRegulation);
                        selEndorseRegulation.DataTextField = "Name";
                        selEndorseRegulation.DataValueField = "Name";
                        selEndorseRegulation.DataBind();

                        selInvalidRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialLowToHighInvalidRegulation);
                        selInvalidRegulation.DataTextField = "Name";
                        selInvalidRegulation.DataValueField = "Name";
                        selInvalidRegulation.DataBind();

                        selRefundRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialLowToHighRefundRegulation);
                        selRefundRegulation.DataTextField = "Name";
                        selRefundRegulation.DataValueField = "Name";
                        selRefundRegulation.DataBind();

                        selChangeRegulation.DataSource = SystemDictionaryService.Query(SystemDictionaryType.SpecialLowToHighChangeRegulation);
                        selChangeRegulation.DataTextField = "Name";
                        selChangeRegulation.DataValueField = "Name";
                        selChangeRegulation.DataBind();
                    }
                    #endregion



                    if (Request.QueryString["Type"].Trim() == "Update")
                    {
                        tip.InnerText = "修改特殊政策";
                        chkAuto.Visible = false;
                        btnCopy.Visible = false;
                        btnModify.Visible = true;
                        lblAirline.Visible = true;
                        ddlAirline.Visible = false;
                    }
                    if (Request.QueryString["Type"].Trim() == "Copy")
                    {
                        tip.InnerText = "复制特殊政策";
                        chkAuto.Visible = true;
                        btnCopy.Visible = true;
                        btnModify.Visible = false;
                        lblAirline.Visible = false;
                        ddlAirline.Visible = true;
                    }

                    var strIds = setting.Airlines.Split('/');
                    ddlAirline.DataSource = from item in Service.FoundationService.Airlines
                                            where item.Valid && strIds.Contains(item.Code.Value)
                                            select new
                                            {
                                                item.Code,
                                                Name = item.Code + "-" + item.ShortName
                                            };
                    ddlAirline.DataTextField = "Name";
                    ddlAirline.DataValueField = "Code";
                    ddlAirline.DataBind();
                    InitDataValue(special);
                    cutomeTh.Visible = cutomeTh.Visible && CurrentCompany.CompanyType == CompanyType.Provider;
                    ddlCustomCode.Visible = ddlCustomCode.Visible && CurrentCompany.CompanyType == CompanyType.Provider;
                    selOfficeTd.Visible = selOfficeTd.Visible && CurrentCompany.CompanyType == CompanyType.Provider;
                }
            }
        }


        void InitDataValue(SpecialPolicy special)
        {
            djbcTxt.Visible = false;
            chkdjbc.Visible = false;
            djbcTip.Visible = false;
            var company = CompanyService.GetCompanySettingsInfo(CurrentCompany.CompanyId);
            cutomeTh.Visible = company != null && company.WorkingSetting != null && company.WorkingSetting.IsImpower;
            ddlCustomCode.Visible = company != null && company.WorkingSetting != null && company.WorkingSetting.IsImpower;
            importantBox.Visible = company != null && company.Parameter != null && !company.Parameter.AutoPlatformAudit;
            var queryList = SpecialProductService.Query(special.Type);
            specialType.Value = (byte)special.Type + "";
            titlePolicy.InnerHtml = special.Type.GetDescription();
            specialtypeSpan.InnerHtml = queryList.Description;
            //航空公司
            this.lblAirline.Text = special.Airline;
            this.ddlAirline.SelectedValue = special.Airline;
            //航班开始时间
            this.txtDepartrueStart.Text = special.DepartureDateStart.ToString("yyyy-MM-dd");
            //航班结束时间
            this.txtDepartrueEnd.Text = special.DepartureDateEnd.ToString("yyyy-MM-dd");
            neibuTh.Visible = company != null && company.WorkingSetting != null && company.Parameter.CanHaveSubordinate;
            neibufanyong.Visible = company != null && company.WorkingSetting != null && company.Parameter.CanHaveSubordinate;
            tong.Visible = company != null && company.WorkingSetting != null && company.Parameter.AllowBrotherPurchase;
            tongTh.Visible = company != null && company.WorkingSetting != null && company.Parameter.AllowBrotherPurchase;
            //航班限制
            this.txtDepartrueFilght.Text = special.DepartureFlightsFilter;
            chkTicket.Text = special.TicketType.GetDescription();

            chkPrintBeforeTwoHours.Visible = special.TicketType == TicketType.B2B;
            //提供资源时间
            this.txtProvideDate.Text = special.ProvideDate.ToString("yyyy-MM-dd");
            txtPriceNeibu.Text = special.InternalCommission == -1 ? "" : special.InternalCommission.TrimInvaidZero();
            txtPriceXiaji.Text = special.SubordinateCommission == -1 ? "" : special.SubordinateCommission.TrimInvaidZero();
            txtPriceTonghang.Text = special.ProfessionCommission == -1 ? "" : special.ProfessionCommission.TrimInvaidZero();
            if (CurrentCompany.CompanyType == CompanyType.Supplier)
            {
                neibuPrice.Visible = false;
                xiajiPrice.Visible = false;
                txtPriceNeibu.Text = "";
                txtPriceXiaji.Text = "";
            }
            if (CurrentCompany.CompanyType == CompanyType.Provider)
            {
                neibuPrice.Visible = company != null && company.Parameter != null && company.Parameter.CanHaveSubordinate;
                tonghangPrice.Visible = company != null && company.Parameter != null && company.Parameter.AllowBrotherPurchase;
                txtPriceNeibu.Text = neibuPrice.Visible ? special.InternalCommission == -1 ? "" : special.InternalCommission.TrimInvaidZero() : "";
                txtPriceTonghang.Text = tonghangPrice.Visible ? special.ProfessionCommission == -1 ? "" : special.ProfessionCommission.TrimInvaidZero() : "";
            }
            //提前天数
            this.txtTiQianDays.Text = special.BeforehandDays == -1 ? "" : special.BeforehandDays + "";
            this.chkAuto.Checked = special.AutoAudit;
            this.chkPrintBeforeTwoHours.Checked = special.PrintBeforeTwoHours;
            txtOutWithFilght.Text = special.ExceptAirways;
            //航班类型(不限，适用以下，不适用以下)
            if (special.DepartureFlightsFilterType == Common.Enums.LimitType.None)
            {
                radBuXian.Checked = true;
            }
            if (special.DepartureFlightsFilterType == Common.Enums.LimitType.Include)
            {
                radYiXia.Checked = true;
            }
            if (special.DepartureFlightsFilterType == Common.Enums.LimitType.Exclude)
            {
                radBuYiXia.Checked = true;
            }
            foreach (var item in special.DepartureWeekFilter.Split(','))
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

            chkConfirmResource.Checked = special.ConfirmResource;
            //价格类型
            if (special.PriceType == PriceType.Price)
            {
                selPrice.SelectedIndex = 0;
                txtPrice.Text = special.Price.TrimInvaidZero();
                price0.Style.Add(HtmlTextWriterStyle.Display, "");
                discount0.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (special.PriceType == PriceType.Subtracting|| special.PriceType == PriceType.Commission)
            {
                selPrice.SelectedIndex = 1;
                txtDiscount.Text = (special.Price * 100).TrimInvaidZero();
                discount0.Style.Add(HtmlTextWriterStyle.Display, "");
                price0.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            //txtPrice.Text = special.PriceType == PriceType.Subtracting ? (special.Price * 100).TrimInvaidZero() : special.Price.TrimInvaidZero();

            txtInternalCommission.Text = special.InternalCommission == -1 || special.InternalCommission == -0.01M ? "" : (special.PriceType == PriceType.Subtracting || special.PriceType == PriceType.Commission ? (special.InternalCommission * 100).TrimInvaidZero() : special.InternalCommission.TrimInvaidZero());
            txtProfessionCommission.Text = special.ProfessionCommission == -1 || special.ProfessionCommission == -0.01M ? "" : (special.PriceType == PriceType.Subtracting || special.PriceType == PriceType.Commission ? (special.ProfessionCommission * 100).TrimInvaidZero() : special.ProfessionCommission.TrimInvaidZero());
            txtSubordinateCommission.Text = special.SubordinateCommission == -1 || special.SubordinateCommission == -0.01M ? "" : (special.PriceType == PriceType.Subtracting || special.PriceType == PriceType.Commission ? (special.SubordinateCommission * 100).TrimInvaidZero() : special.SubordinateCommission.TrimInvaidZero());

            if (special.Type == SpecialProductType.Singleness)
            {
                jiagetishi.Visible = false;
                hidBunks.Visible = false;
                txtDepartureAirports.Code = special.Departure;
                txtArrivalAirports.Code = special.Arrival;
                //diaohuanshifa.Visible = false;
                shifa.Visible = false;
                //wangfanThShifa.Visible = false;
                txtDrawerConditionTh.Visible = false;
                hptbTh.Visible = false;
                btnRef.Visible = false;
                chanpinBunks.Visible = false;
                tableDiv.Visible = false;
                cangweiTh.Visible = false;
                tishi.InnerText = "提示：请输入1-9的纯数字";
                chkPrintBeforeTwoHours.Visible = false;
            }
            else if (special.Type == SpecialProductType.Disperse)
            {
                jiagetishi.Visible = false;
                hidBunks.Visible = false;
                txtDepartureAirports.Code = special.Departure;
                txtArrivalAirports.Code = special.Arrival;
                //diaohuanshifa.Visible = false;
                shifa.Visible = false;
                //wangfanThShifa.Visible = false;
                txtDrawerConditionTh.Visible = false;
                hptbTh.Visible = false;
                btnRef.Visible = false;
                tableDiv.Visible = false;
                cangweiTh.Visible = false;
                chanpinBunks.Visible = false;
                tishi.InnerText = "提示：请输入1-99的纯数字";
                chkPrintBeforeTwoHours.Visible = false;
            }
            else if (special.Type == SpecialProductType.CostFree)
            {
                paichu.Visible = true;
                jiagetishi.Visible = false;
                txtShifaAirports.InitData(true, special.Departure.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, special.Arrival.Split('/').ToList());
                txtDepartureAirports.Visible = false;
                txtArrivalAirports.Visible = false;
                //  diaohuanshifa.Visible = false;
                //wangfanThShifa.Visible = false;
                txtDrawerConditionTh.Visible = false;
                tableDiv.Visible = false;
                danchengThShifa.Visible = false;
                danchengMudifa.Visible = false;
                cangweiTh.Visible = false;
                chanpinBunks.Visible = false;
                if (special.SynBlackScreen)
                {
                    hidBunks.Value = special.Berths.Trim();
                    hptb.Checked = true;
                    btnRef.Style.Add(HtmlTextWriterStyle.Display, "");
                    selBunksSpan.Style.Add(HtmlTextWriterStyle.Display, "");
                    amountDiv.Style.Add(HtmlTextWriterStyle.Display, "none");
                    chkConfirmResource.Style.Add(HtmlTextWriterStyle.Display, "none");
                    youweichupiao.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
                else
                {
                    bhptb.Checked = true;
                    btnRef.Style.Add(HtmlTextWriterStyle.Display, "none");
                    selBunksSpan.Style.Add(HtmlTextWriterStyle.Display, "none");
                    amountDiv.Style.Add(HtmlTextWriterStyle.Display, "");
                }
                youwei.Checked = special.IsSeat;
                wuwei.Checked = !special.IsSeat;
                tishi.InnerText = "提示：请输入1-99的纯数字";
                txtOutWithFilght.Text = special.ExceptAirways;
                chkPrintBeforeTwoHours.Visible = false;
            }
            else if (special.Type == SpecialProductType.Bloc)
            {
                paichu.Visible = true;
                txtShifaAirports.InitData(true, special.Departure.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, special.Arrival.Split('/').ToList());
                chkConfirmResource.Visible = false;

                danchengThShifa.Visible = false;
                danchengMudifa.Visible = false;
                danchengduihuan.Visible = false;
                dropDrawerConditionTh.Visible = false;
                hptbTh.Visible = false;
                //priceDiv.Visible = false;
                neibuPrice.Visible = false;
                xiajiPrice.Visible = false;
                tonghangPrice.Visible = false;
                amountDiv.Visible = false;
                chanpinBunks.Visible = false;
                neibuTh.Visible = company != null && company.Parameter != null && company.Parameter.CanHaveSubordinate;
                neibufanyong.Visible = company != null && company.Parameter != null && company.Parameter.CanHaveSubordinate;
                tongTh.Visible = company != null && company.Parameter != null && company.Parameter.AllowBrotherPurchase;
                tong.Visible = company != null && company.Parameter != null && company.Parameter.AllowBrotherPurchase;
                // kepiao.Visible = false;
                //kepiaoTh.Visible = false;
                chkTicket.Text = special.TicketType.GetDescription();
                if (special.IsBargainBerths)
                {
                    bargain.Checked = true;
                }
                else
                {
                    normal.Checked = true;
                }
                Bunks.InnerHtml = "";
                hidBunks.Value = special.Berths.Trim();
                if (!special.IsBargainBerths)
                {
                    //舱位
                    IEnumerable<string> listBunks = QueryBunks(special.Airline, special.DepartureDateStart, special.DepartureDateEnd, special.ProvideDate);
                    string[] bunks = special.Berths.Split(',');
                    int j = 0;
                    foreach (string item in listBunks)
                    {
                        if (j == 0)
                        {
                            Bunks.InnerHtml += "<input type='radio' value='0' name='1radio' id='1all'  class='choice' /><label for='1all'> 全选</label> <input type='radio' value='1' name='1radio' id='1not' class='choice' /><label for='1not'> 反选</label><br />";
                        }
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
                else
                {
                    Bunks.InnerHtml = "<label class='refBtnBunks btn class3'> 点击获取舱位</label>";
                }
                if (special.PriceType == PriceType.Subtracting)
                {
                    if (special.LowNoType == LowNoType.LowInterval)
                    {
                        chkdjbc.Checked = true;
                        txtdj.Text = special.LowNoMaxPrice.TrimInvaidZero() == "-1" ? "" : special.LowNoMaxPrice.TrimInvaidZero();
                        txtdjbc.Text = special.LowNoMinPrice.TrimInvaidZero();
                        //djbcTxt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                    if (special.LowNoType == LowNoType.None)
                    {
                        djbcTxt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                }
                else
                {
                    djbcTxt.Style.Add(HtmlTextWriterStyle.Display, "none");
                    chkdjbc.Style.Add(HtmlTextWriterStyle.Display, "none");
                    djbcTip.Style.Add(HtmlTextWriterStyle.Display, "none");
                }

                djbcTxt.Visible = true;
                chkdjbc.Visible = true;
                djbcTip.Visible = true;
            }
            else if (special.Type == SpecialProductType.Business)
            {
                paichu.Visible = true;
                txtShifaAirports.InitData(true, special.Departure.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, special.Arrival.Split('/').ToList());
                hidBunks.Value = special.Berths.Trim();
                chkConfirmResource.Visible = false;

                danchengThShifa.Visible = false;

                danchengMudifa.Visible = false;
                danchengduihuan.Visible = false;
                dropDrawerConditionTh.Visible = false;
                kepiao.Visible = false;
                kepiaoTh.Visible = false;
                hptbTh.Visible = false;
                //priceDiv.Visible = false;
                neibuPrice.Visible = false;
                xiajiPrice.Visible = false;
                tonghangPrice.Visible = false;
                amountDiv.Visible = false;
                neibuTh.Visible = company != null && company.Parameter != null && company.Parameter.CanHaveSubordinate;
                neibufanyong.Visible = company != null && company.Parameter != null && company.Parameter.CanHaveSubordinate;
                tongTh.Visible = company != null && company.Parameter != null && company.Parameter.AllowBrotherPurchase;
                tong.Visible = company != null && company.Parameter != null && company.Parameter.AllowBrotherPurchase;
                kepiao.Visible = false;
                kepiaoTh.Visible = false;
                chanpinBunks.Visible = false;
                if (special.IsBargainBerths)
                {
                    bargain.Checked = true;
                }
                else
                {
                    normal.Checked = true;
                }
                Bunks.InnerHtml = "";
                if (!special.IsBargainBerths)
                {
                    //舱位
                    IEnumerable<string> listBunks = QueryBunks(special.Airline, special.DepartureDateStart, special.DepartureDateEnd, special.ProvideDate);
                    string[] bunks = special.Berths.Split(',');
                    int j = 0;
                    foreach (string item in listBunks)
                    {
                        if (j == 0)
                        {
                            Bunks.InnerHtml += "<input type='radio' value='0' name='1radio' id='1all'  class='choice' /><label for='1all'> 全选</label> <input type='radio' value='1' name='1radio' id='1not' class='choice' /><label for='1not'> 反选</label><br />";
                        }
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
            }
            else if (special.Type == SpecialProductType.OtherSpecial)
            {
                paichu.Visible = true;
                txtShifaAirports.InitData(true, special.Departure.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, special.Arrival.Split('/').ToList());
                chkConfirmResource.Visible = false;
                danchengThShifa.Visible = false;
                danchengMudifa.Visible = false;
                danchengduihuan.Visible = false;
                dropDrawerConditionTh.Visible = false;

                txtDrawerConditionTh.Visible = false;
                amountDiv.Visible = false;
                hptbTh.Visible = false;
                btnRef.Visible = false;
                tableDiv.Visible = false;
                cangweiTh.Visible = false;
                hptbTh.Visible = false;
                txtBunks.Text = special.Berths;
                hidBunks.Value = special.Berths;
                chkPrintBeforeTwoHours.Visible = false;
            }

            else if (special.Type == SpecialProductType.LowToHigh)
            {
                paichu.Visible = true;
                txtShifaAirports.InitData(true, special.Departure.Split('/').ToList());
                txtZhongzhuanAirports.InitData(true, special.Arrival.Split('/').ToList());
                hidBunks.Value = special.Berths.Trim();

                danchengThShifa.Visible = false;
                normal.Checked = true;
                danchengMudifa.Visible = false;
                danchengduihuan.Visible = false;
                dropDrawerConditionTh.Visible = false;
                hptbTh.Visible = false;
                //priceDiv.Visible = false;
                neibuPrice.Visible = false;
                xiajiPrice.Visible = false;
                tonghangPrice.Visible = false;
                amountDiv.Visible = false;
                neibuTh.Visible = company != null && company.Parameter != null && company.Parameter.CanHaveSubordinate;
                neibufanyong.Visible = company != null && company.Parameter != null && company.Parameter.CanHaveSubordinate;
                tongTh.Visible = company != null && company.Parameter != null && company.Parameter.AllowBrotherPurchase;
                tong.Visible = company != null && company.Parameter != null && company.Parameter.AllowBrotherPurchase;
                kepiao.Visible = false;
                kepiaoTh.Visible = false;
                chanpinBunks.Visible = false;
                cangweiTh.Style.Add("display", "none");
                kepiao.Visible = false;
                kepiaoTh.Visible = false;
                discountTh.Visible = false;
                discount.Visible = false;
                jiagetishi.Visible = false;
                Bunks.InnerHtml = "";
                if (!special.IsBargainBerths)
                {
                    //舱位
                    IEnumerable<string> listBunks = QueryBunks(special.Airline, special.DepartureDateStart, special.DepartureDateEnd, special.ProvideDate);
                    string[] bunks = special.Berths.Split(',');
                    int j = 0;
                    foreach (string item in listBunks)
                    {
                        if (j == 0)
                        {
                            Bunks.InnerHtml += "<input type='radio' value='0' name='1radio' id='1all'  class='choice' /><label for='1all'> 全选</label> <input type='radio' value='1' name='1radio' id='1not' class='choice' /><label for='1not'> 反选</label><br />";
                        }
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
            }


            txtRemark.Text = special.Remark;
            txtDrawerCondition.Text = special.DrawerCondition;

            dropDrawerCondition.SelectedValue = special.DrawerCondition.Trim();
            selEndorseRegulation.SelectedValue = special.EndorseRegulation.Trim();
            selInvalidRegulation.SelectedValue = special.InvalidRegulation.Trim();
            selRefundRegulation.SelectedValue = special.RefundRegulation.Trim();
            selChangeRegulation.SelectedValue = special.ChangeRegulation.Trim();
            //txtPrice.Text = special.Price == -1 ? "" : special.Price.TrimInvaidZero();
            txtResourceAmount.Text = special.ResourceAmount == -1 ? "" : special.ResourceAmount + "";
            txtPaiChu.Text = special.DepartureDateFilter;
            ddlCustomCode.SelectedValue = special.CustomCode.Trim();
            lblCustomerCode.Text = special.CustomCode.Trim();
            //office号

            for (int i = 0; i < dropOffice.Items.Count; i++)
            {
                if (special.OfficeCode == dropOffice.Items[i].Text.Trim())
                {
                    dropOffice.Items[i].Selected = true;
                    break;
                }
            }
            cutomeTh.Visible = cutomeTh.Visible && CurrentCompany.CompanyType == CompanyType.Provider;
            ddlCustomCode.Visible = ddlCustomCode.Visible && CurrentCompany.CompanyType == CompanyType.Provider;
            selOfficeTd.Visible = selOfficeTd.Visible && CurrentCompany.CompanyType == CompanyType.Provider;
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
            var derpartrueFilghtType = LimitType.None;
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

            var company = CompanyService.GetCompanyParameter(CurrentCompany.CompanyId);
            try
            {
                if (type == "Update")
                {
                    SpecialPolicy specials = PolicyManageService.GetSpecialPolicy(Guid.Parse(Request.QueryString["Id"]));
                    var special = new SpecialPolicy
                                      {
                                          OfficeCode = dropOffice.SelectedItem == null ? "" : hidOfficeNo.Value,
                                          ImpowerOffice = dropOffice.SelectedItem == null ? false : Convert.ToBoolean(dropOffice.SelectedValue),
                                          Airline = lblAirline.Text,
                                          Arrival = txtArrivalAirports.Code,
                                          AutoAudit = chkAuto.Checked,
                                          IsInternal = company.CanHaveSubordinate,
                                          IsPeer = company.AllowBrotherPurchase || CurrentCompany.CompanyType == CompanyType.Supplier,
                                          CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                                          ConfirmResource = chkConfirmResource.Checked,
                                          Departure = txtDepartureAirports.Code,
                                          DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                          DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                          ExceptAirways = txtOutWithFilght.Text,
                                          DepartureFlightsFilter = derpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                                          Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                                          DrawerCondition = txtDrawerCondition.Text.Trim(),
                                          ProvideDate = DateTime.Parse(txtProvideDate.Text),
                                          //DepartureDatesFilter = "",
                                          //DepartureDatesFilterType = DateMode.Date,
                                          ChangeRegulation = selChangeRegulation.Text,
                                          EndorseRegulation = selEndorseRegulation.Text.Trim(),
                                          RefundRegulation = selRefundRegulation.Text.Trim(),
                                          InvalidRegulation = selInvalidRegulation.Text.Trim(),
                                          VoyageType = VoyageType.OneWay,
                                          DepartureWeekFilter = weekStr,
                                          DepartureDateFilter = txtPaiChu.Text,
                                          DepartureFlightsFilterType = derpartrueFilghtType,
                                          Owner = CurrentUser.Owner,
                                          CreateTime = specials.CreateTime,
                                          Suspended = specials.Suspended,
                                          Audited = specials.Audited,
                                          AuditTime = specials.AuditTime,
                                          Freezed = specials.Freezed,
                                          PlatformAudited = company != null && company.AutoPlatformAudit,
                                          Creator = CurrentUser.UserName,
                                          PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked,
                                          Id = Guid.Parse(Request.QueryString["Id"])
                                      };
                    string specialTypes = specialType.Value;
                    if (specialTypes == "0")
                    {
                        special.TicketType = TicketType.B2B;
                        special.Type = SpecialProductType.Singleness;
                        special.Departure = txtDepartureAirports.Code;
                        special.Arrival = txtArrivalAirports.Code;
                        special.BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        special.ResourceAmount = int.Parse(txtResourceAmount.Text);
                        special.Price = -1;
                        special.PriceType = PriceType.Price;
                        special.Berths = "";
                        special.SynBlackScreen = false;
                        special.InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        special.ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        special.SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        special.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        special.IsSeat = false;
                        special.LowNoMaxPrice = -1;
                        special.LowNoMinPrice = -1;
                        special.LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "1")
                    {
                        special.TicketType = TicketType.B2B;
                        special.Type = SpecialProductType.Disperse;
                        special.Departure = txtDepartureAirports.Code;
                        special.Arrival = txtArrivalAirports.Code;
                        special.BeforehandDays = short.Parse(txtTiQianDays.Text.Trim() == "" ? "0" : txtTiQianDays.Text);
                        special.ResourceAmount = int.Parse(txtResourceAmount.Text);
                        special.Price = -1;
                        special.PriceType = PriceType.Price;
                        special.Berths = "";
                        special.SynBlackScreen = false;
                        special.InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        special.ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        special.SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        special.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        special.IsSeat = false;
                        special.LowNoMaxPrice = -1;
                        special.LowNoMinPrice = -1;
                        special.LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "2")
                    {
                        special.TicketType = TicketType.B2B;
                        special.Type = SpecialProductType.CostFree;
                        special.Departure = txtShifaAirports.AirportsCode.Join("/");
                        special.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        special.BeforehandDays = short.Parse(txtTiQianDays.Text.Trim() == "" ? "0" : txtTiQianDays.Text);
                        special.PriceType = PriceType.Price;
                        if (hptb.Checked)
                        {
                            special.Berths = hidBunks.Value;
                            special.SynBlackScreen = true;
                            special.IsSeat = true;
                            special.ConfirmResource = false;
                        }
                        else
                        {
                            special.ResourceAmount = int.Parse(txtResourceAmount.Text == "" ? "0" : txtResourceAmount.Text);
                            special.Berths = "";
                            special.IsSeat = youwei.Checked;
                        }
                        special.Price = -1;
                        special.InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        special.ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        special.SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        special.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        special.LowNoMaxPrice = -1;
                        special.LowNoMinPrice = -1;
                        special.LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "3")
                    {
                        special.Type = SpecialProductType.Bloc;

                        if (chkTicket.Text == "B2B")
                        {
                            special.TicketType = TicketType.B2B;
                        }
                        else
                        {
                            special.TicketType = TicketType.BSP;
                        }
                        special.Departure = txtShifaAirports.AirportsCode.Join("/");
                        special.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        special.BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        special.ResourceAmount = 0;
                        if (selPrice.SelectedIndex == 0)
                        {
                            special.Price = decimal.Parse(txtPrice.Text);
                            special.PriceType = PriceType.Price;
                            special.InternalCommission = decimal.Parse(neibufanyong.Visible ? txtInternalCommission.Text : "-1");
                            special.ProfessionCommission = decimal.Parse(tong.Visible ? txtProfessionCommission.Text : "-1");
                            special.SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                        }
                        else if (!bargain.Checked)
                        {
                            special.Price = decimal.Parse(txtDiscount.Text) / 100;
                            special.PriceType = PriceType.Subtracting;
                            special.InternalCommission = neibufanyong.Visible ? decimal.Parse(txtInternalCommission.Text) / 100 : -1M;
                            special.ProfessionCommission = tong.Visible ? decimal.Parse(txtProfessionCommission.Text) / 100 : -1M;
                            special.SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100;
                        }
                        if (bargain.Checked)
                        {
                            special.Price = decimal.Parse(txtPrice.Text);
                            special.PriceType = PriceType.Price;
                            special.InternalCommission = decimal.Parse(neibufanyong.Visible ? txtInternalCommission.Text : "-1");
                            special.ProfessionCommission = decimal.Parse(tong.Visible ? txtProfessionCommission.Text : "-1");
                            special.SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                        }
                        special.IsBargainBerths = bargain.Checked;
                        special.Berths = hidBunks.Value;
                        special.SynBlackScreen = false;
                        special.IsSeat = false;
                        special.ConfirmResource = false;
                        special.LowNoMinPrice = chkdjbc.Checked ? decimal.Parse(txtdjbc.Text) : -1;
                        special.LowNoMaxPrice = chkdjbc.Checked ? txtdj.Text == "" ? -1 : decimal.Parse(txtdj.Text) : -1;
                        special.LowNoType = chkdjbc.Checked ? LowNoType.LowInterval : LowNoType.None;
                    }
                    if (specialTypes == "4")
                    {
                        if (chkTicket.Text == "B2B")
                        {
                            special.TicketType = TicketType.B2B;
                        }
                        else
                        {
                            special.TicketType = TicketType.BSP;
                        }
                        special.Type = SpecialProductType.Business;

                        special.Departure = txtShifaAirports.AirportsCode.Join("/");
                        special.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        special.BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        special.ResourceAmount = 0;
                        if (selPrice.SelectedIndex == 0)
                        {
                            special.Price = decimal.Parse(txtPrice.Text);
                            special.PriceType = PriceType.Price;
                            special.InternalCommission = decimal.Parse(neibufanyong.Visible ? txtInternalCommission.Text : "-1");
                            special.ProfessionCommission = decimal.Parse(tong.Visible ? txtProfessionCommission.Text : "-1");
                            special.SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                        }
                        else if (!bargain.Checked)
                        {
                            special.Price = decimal.Parse(txtDiscount.Text) / 100;
                            special.PriceType = PriceType.Subtracting;
                            special.InternalCommission = neibufanyong.Visible ? decimal.Parse(txtInternalCommission.Text) / 100 : -1M;
                            special.ProfessionCommission = tong.Visible ? decimal.Parse(txtProfessionCommission.Text) / 100 : -1M;
                            special.SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100;
                        }
                        if (bargain.Checked)
                        {
                            special.Price = decimal.Parse(txtPrice.Text);
                            special.PriceType = PriceType.Price;
                            special.InternalCommission = decimal.Parse(neibufanyong.Visible ? txtInternalCommission.Text : "-1");
                            special.ProfessionCommission = decimal.Parse(tong.Visible ? txtProfessionCommission.Text : "-1");
                            special.SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                        }
                        special.IsBargainBerths = bargain.Checked;
                        special.Berths = hidBunks.Value;
                        special.SynBlackScreen = false;
                        special.IsSeat = false;
                        special.ConfirmResource = false;
                        special.LowNoMaxPrice = -1;
                        special.LowNoMinPrice = -1;
                        special.LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "5")
                    {
                        special.TicketType = TicketType.B2B;
                        special.Type = SpecialProductType.OtherSpecial;
                        special.Departure = txtShifaAirports.AirportsCode.Join("/");
                        special.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        special.BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        special.PriceType = PriceType.Price;
                        special.Berths = txtBunks.Text.ToUpper().Trim();
                        special.SynBlackScreen = true;
                        special.IsBargainBerths = false;
                        special.IsSeat = true;
                        special.Price = -1;
                        special.InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        special.ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        special.SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        special.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        special.LowNoMaxPrice = -1;
                        special.LowNoMinPrice = -1;
                        special.LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "6")
                    {
                        special.TicketType = TicketType.B2B;
                        special.Type = SpecialProductType.LowToHigh;

                        special.Departure = txtShifaAirports.AirportsCode.Join("/");
                        special.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        special.BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        special.ResourceAmount = 0;

                        special.Price = -1;
                        special.PriceType = PriceType.Commission;
                        special.InternalCommission = neibufanyong.Visible ? decimal.Parse(txtInternalCommission.Text) / 100 : -1M;
                        special.ProfessionCommission = tong.Visible ? decimal.Parse(txtProfessionCommission.Text) / 100 : -1M;
                        special.SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text) / 100;

                        special.IsBargainBerths = bargain.Checked;
                        special.Berths = hidBunks.Value;
                        special.SynBlackScreen = true;
                        special.IsSeat = false;
                        special.ConfirmResource = chkConfirmResource.Checked;
                        special.LowNoMaxPrice = -1;
                        special.LowNoMinPrice = -1;
                        special.LowNoType = LowNoType.None;
                    }
                    if (special.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (special.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }

                    special.SubordinateCommission = CurrentCompany.CompanyType == CompanyType.Supplier ? special.ProfessionCommission : special.SubordinateCommission;
                    PolicyManageService.UpdateSpecialPolicy(special, this.CurrentUser.UserName);
                }
                if (type == "Copy")
                {
                    var specialInfo = new SpecialPolicyReleaseInfo
                    {
                        BasicInfo = new SpecialPolicyBasicInfo
                        {
                            IsInternal = company.CanHaveSubordinate,
                            IsPeer = company.AllowBrotherPurchase || CurrentCompany.CompanyType == CompanyType.Supplier,
                            OfficeCode = dropOffice.SelectedItem == null ? "" : hidOfficeNo.Value,
                            ImpowerOffice = dropOffice.SelectedItem == null ? false : Convert.ToBoolean(dropOffice.SelectedValue),
                            CustomCode = ddlCustomCode.Visible ? ddlCustomCode.SelectedValue.Trim() : lblCustomerCode.Text,
                            Airline = ddlAirline.SelectedValue,
                            Arrival = txtArrivalAirports.Code,
                            Departure = txtDepartureAirports.Code,
                            //DepartureDatesFilter = "",
                            //DepartureDatesFilterType = DateMode.Date,
                            DepartureFlightsFilter = derpartrueFilghtType == LimitType.None ? "" : txtDepartrueFilght.Text,
                            DepartureFlightsFilterType = derpartrueFilghtType,
                            Remark = txtRemark.Text.Replace("\r", "").Replace("\n", ""),
                            DrawerCondition = txtDrawerCondition.Text.Trim(),
                            ChangeRegulation = selChangeRegulation.Text.Trim(),
                            EndorseRegulation = selEndorseRegulation.Text.Trim(),
                            RefundRegulation = selRefundRegulation.Text.Trim(),
                            InvalidRegulation = selInvalidRegulation.Text.Trim(),
                            ExceptAirways = txtOutWithFilght.Text,
                            VoyageType = VoyageType.OneWay,
                            //Type = AirportPair,
                            Owner = this.CurrentUser.Owner
                        }
                    };

                    var list = new List<SpecialPolicyRebateInfo>
                                   {
                                       new SpecialPolicyRebateInfo
                                           {
                                               AutoAudit = chkAuto.Checked,
                                               DepartureWeekFilter = weekStr,
                                               DepartureDateFilter = txtPaiChu.Text,
                                               ConfirmResource = chkConfirmResource.Checked,
                                               DepartureDateEnd = DateTime.Parse(txtDepartrueEnd.Text),
                                               DepartureDateStart = DateTime.Parse(txtDepartrueStart.Text),
                                               ProvideDate = DateTime.Parse(txtProvideDate.Text),
                                               ResourceAmount = int.Parse(txtResourceAmount.Text==""?"-1":txtResourceAmount.Text),
                                               BeforehandDays = short.Parse(txtTiQianDays.Text==""?"0":txtTiQianDays.Text),
                                               Price = decimal.Parse(txtPrice.Text==""?"-1":txtPrice.Text),
                                               PrintBeforeTwoHours = chkPrintBeforeTwoHours.Checked
                                           }
                                   };

                    string specialTypes = specialType.Value;
                    if (specialTypes == "0")
                    {
                        list[0].TicketType = TicketType.B2B;
                        specialInfo.BasicInfo.Type = SpecialProductType.Singleness;
                        specialInfo.BasicInfo.Departure = txtDepartureAirports.Code;
                        specialInfo.BasicInfo.Arrival = txtArrivalAirports.Code;
                        list[0].BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        list[0].ResourceAmount = int.Parse(txtResourceAmount.Text);
                        list[0].Price = -1;
                        list[0].PriceType = PriceType.Price;
                        list[0].Berths = "";
                        list[0].SynBlackScreen = false;
                        list[0].InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        list[0].ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        list[0].SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        specialInfo.BasicInfo.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        list[0].IsSeat = false;
                        list[0].LowNoMaxPrice = -1;
                        list[0].LowNoMinPrice = -1;
                        list[0].LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "1")
                    {
                        list[0].TicketType = TicketType.B2B;
                        specialInfo.BasicInfo.Type = SpecialProductType.Disperse;
                        specialInfo.BasicInfo.Departure = txtDepartureAirports.Code;
                        specialInfo.BasicInfo.Arrival = txtArrivalAirports.Code;
                        list[0].BeforehandDays = short.Parse(txtTiQianDays.Text.Trim() == "" ? "0" : txtTiQianDays.Text);
                        list[0].ResourceAmount = int.Parse(txtResourceAmount.Text);
                        list[0].Price = -1;
                        list[0].PriceType = PriceType.Price;
                        list[0].Berths = "";
                        list[0].SynBlackScreen = false;
                        list[0].InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        list[0].ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        list[0].SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        specialInfo.BasicInfo.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        list[0].IsSeat = false;
                        list[0].LowNoMaxPrice = -1;
                        list[0].LowNoMinPrice = -1;
                        list[0].LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "2")
                    {
                        list[0].TicketType = TicketType.B2B;
                        specialInfo.BasicInfo.Type = SpecialProductType.CostFree;
                        specialInfo.BasicInfo.Departure = txtShifaAirports.AirportsCode.Join("/");
                        specialInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        list[0].BeforehandDays = short.Parse(txtTiQianDays.Text.Trim() == "" ? "0" : txtTiQianDays.Text);
                        list[0].Price = -1;
                        list[0].PriceType = PriceType.Price;
                        if (hptb.Checked)
                        {
                            list[0].Berths = hidBunks.Value;
                            list[0].SynBlackScreen = true;
                            list[0].ResourceAmount = -1;
                            list[0].ConfirmResource = false;
                        }
                        else
                        {
                            list[0].ResourceAmount = int.Parse(txtResourceAmount.Text);
                            list[0].Berths = "";
                            list[0].SynBlackScreen = false;
                        }
                        list[0].InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        list[0].ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        list[0].SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        specialInfo.BasicInfo.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        list[0].IsSeat = youwei.Checked;
                        list[0].LowNoMaxPrice = -1;
                        list[0].LowNoMinPrice = -1;
                        list[0].LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "3")
                    {
                        list[0].TicketType = TicketType.B2B;
                        specialInfo.BasicInfo.Type = SpecialProductType.Bloc;
                        specialInfo.BasicInfo.Departure = txtShifaAirports.AirportsCode.Join("/");
                        specialInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        list[0].BeforehandDays = short.Parse(txtTiQianDays.Text.Trim() == "" ? "0" : txtTiQianDays.Text);
                        list[0].ResourceAmount = -1;
                        if (selPrice.SelectedIndex == 0)
                        {
                            list[0].Price = decimal.Parse(txtPrice.Text);
                            list[0].PriceType = PriceType.Price;
                            if (neibuTh.Visible)
                            {
                                list[0].InternalCommission = decimal.Parse(txtInternalCommission.Text);
                            }
                            else
                            {
                                list[0].InternalCommission = -1;
                            }
                            if (tong.Visible)
                            {
                                list[0].ProfessionCommission = decimal.Parse(txtProfessionCommission.Text);
                            }
                            else
                            {
                                list[0].ProfessionCommission = -1;
                            }
                            list[0].SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                            list[0].IsBargainBerths = bargain.Checked;
                        }
                        else if (!bargain.Checked)
                        {
                            list[0].Price = decimal.Parse(txtDiscount.Text);
                            list[0].PriceType = PriceType.Subtracting;
                            if (neibuTh.Visible)
                            {
                                list[0].InternalCommission = decimal.Parse(txtInternalCommission.Text);
                            }
                            else
                            {
                                list[0].InternalCommission = -1;
                            }
                            if (tong.Visible)
                            {
                                list[0].ProfessionCommission = decimal.Parse(txtProfessionCommission.Text);
                            }
                            else
                            {
                                list[0].ProfessionCommission = -1;
                            }
                            list[0].SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                        }
                        if (bargain.Checked)
                        {
                            list[0].Price = decimal.Parse(txtPrice.Text);
                            list[0].PriceType = PriceType.Price;
                            if (neibuTh.Visible)
                            {
                                list[0].InternalCommission = decimal.Parse(txtInternalCommission.Text);
                            }
                            else
                            {
                                list[0].InternalCommission = -1;
                            }
                            if (tong.Visible)
                            {
                                list[0].ProfessionCommission = decimal.Parse(txtProfessionCommission.Text);
                            }
                            else
                            {
                                list[0].ProfessionCommission = -1;
                            }
                            list[0].SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                            list[0].IsBargainBerths = bargain.Checked;
                        }
                        list[0].Berths = hidBunks.Value;
                        list[0].SynBlackScreen = false;
                        list[0].IsSeat = false;
                        list[0].ConfirmResource = false;
                        list[0].LowNoMinPrice = chkdjbc.Checked ? decimal.Parse(txtdjbc.Text) : -1;
                        list[0].LowNoMaxPrice = chkdjbc.Checked ? txtdj.Text == "" ? -1 : decimal.Parse(txtdj.Text) : -1;
                        list[0].LowNoType = chkdjbc.Checked ? LowNoType.LowInterval : LowNoType.None;
                    }
                    if (specialTypes == "4")
                    {
                        if (chkTicket.Text == "B2B")
                        {
                            list[0].TicketType = TicketType.B2B;
                        }
                        else
                        {
                            list[0].TicketType = TicketType.BSP;
                        }
                        specialInfo.BasicInfo.Type = SpecialProductType.Business;
                        specialInfo.BasicInfo.Departure = txtShifaAirports.AirportsCode.Join("/");
                        specialInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        list[0].BeforehandDays = short.Parse(txtTiQianDays.Text.Trim() == "" ? "0" : txtTiQianDays.Text);
                        list[0].ResourceAmount = -1;
                        if (selPrice.SelectedIndex == 0)
                        {
                            list[0].Price = decimal.Parse(txtPrice.Text);
                            list[0].PriceType = PriceType.Price;
                            if (neibuTh.Visible)
                            {
                                list[0].InternalCommission = decimal.Parse(txtInternalCommission.Text);
                            }
                            else
                            {
                                list[0].InternalCommission = -1;
                            }
                            if (tong.Visible)
                            {
                                list[0].ProfessionCommission = decimal.Parse(txtProfessionCommission.Text);
                            }
                            else
                            {
                                list[0].ProfessionCommission = -1;
                            }
                            list[0].SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                            list[0].IsBargainBerths = bargain.Checked;
                        }
                        else if (!bargain.Checked)
                        {
                            list[0].Price = decimal.Parse(txtDiscount.Text);
                            list[0].PriceType = PriceType.Subtracting;
                            if (neibuTh.Visible)
                            {
                                list[0].InternalCommission = decimal.Parse(txtInternalCommission.Text);
                            }
                            else
                            {
                                list[0].InternalCommission = -1;
                            }
                            if (tong.Visible)
                            {
                                list[0].ProfessionCommission = decimal.Parse(txtProfessionCommission.Text);
                            }
                            else
                            {
                                list[0].ProfessionCommission = -1;
                            }
                            list[0].SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                        }

                        if (bargain.Checked)
                        {
                            list[0].Price = decimal.Parse(txtPrice.Text);
                            list[0].PriceType = PriceType.Price;
                            if (neibuTh.Visible)
                            {
                                list[0].InternalCommission = decimal.Parse(txtInternalCommission.Text);
                            }
                            else
                            {
                                list[0].InternalCommission = -1;
                            }
                            if (tong.Visible)
                            {
                                list[0].ProfessionCommission = decimal.Parse(txtProfessionCommission.Text);
                            }
                            else
                            {
                                list[0].ProfessionCommission = -1;
                            }
                            list[0].SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);
                            list[0].IsBargainBerths = bargain.Checked;
                        }
                        list[0].Berths = hidBunks.Value;
                        list[0].SynBlackScreen = false;
                        list[0].IsSeat = false;
                        list[0].ConfirmResource = false;
                        list[0].LowNoMaxPrice = -1;
                        list[0].LowNoMinPrice = -1;
                        list[0].LowNoType = LowNoType.None;
                    }
                    if (specialTypes == "5")
                    {
                        list[0].TicketType = TicketType.B2B;
                        specialInfo.BasicInfo.Type = SpecialProductType.OtherSpecial;
                        specialInfo.BasicInfo.Departure = txtShifaAirports.AirportsCode.Join("/");
                        specialInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        list[0].BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        list[0].PriceType = PriceType.Price;
                        list[0].Berths = txtBunks.Text.ToUpper().Trim();
                        list[0].SynBlackScreen = true;
                        list[0].IsBargainBerths = false;
                        list[0].IsSeat = true;
                        list[0].Price = -1;
                        list[0].InternalCommission = decimal.Parse(txtPriceNeibu.Text == "" ? "-1" : txtPriceNeibu.Text);
                        list[0].ProfessionCommission = decimal.Parse(txtPriceTonghang.Text == "" ? "-1" : txtPriceTonghang.Text);
                        list[0].SubordinateCommission = decimal.Parse(txtPriceXiaji.Text == "" ? "-1" : txtPriceXiaji.Text);
                        specialInfo.BasicInfo.DrawerCondition = dropDrawerCondition.Text.Replace("\r", "").Replace("\n", "");
                        list[0].LowNoMaxPrice = -1;
                        list[0].LowNoMinPrice = -1;
                        list[0].LowNoType = LowNoType.None;
                    } if (specialTypes == "6")
                    {
                        list[0].TicketType = TicketType.B2B;
                        specialInfo.BasicInfo.Type = SpecialProductType.LowToHigh;

                        specialInfo.BasicInfo.Departure = txtShifaAirports.AirportsCode.Join("/");
                        specialInfo.BasicInfo.Arrival = txtZhongzhuanAirports.AirportsCode.Join("/");
                        list[0].BeforehandDays = short.Parse(txtTiQianDays.Text == "" ? "0" : txtTiQianDays.Text);
                        list[0].ResourceAmount = 0;

                        list[0].Price = -1;
                        list[0].PriceType = PriceType.Commission;
                        list[0].InternalCommission = neibufanyong.Visible ? decimal.Parse(txtInternalCommission.Text): -1M;
                        list[0].ProfessionCommission = tong.Visible ? decimal.Parse(txtProfessionCommission.Text) : -1M;
                        list[0].SubordinateCommission = decimal.Parse(txtSubordinateCommission.Text);

                        list[0].IsBargainBerths = bargain.Checked;
                        list[0].Berths = hidBunks.Value;
                        list[0].SynBlackScreen = true;
                        list[0].IsSeat = false;
                        list[0].ConfirmResource = chkConfirmResource.Checked;
                        list[0].LowNoMaxPrice = -1;
                        list[0].LowNoMinPrice = -1;
                        list[0].LowNoType = LowNoType.None;
                    }
                    specialInfo.Rebates = list;
                    if (specialInfo.BasicInfo.Departure.Trim() == "")
                    {
                        RegisterScript("alert('出发地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    if (specialInfo.BasicInfo.Arrival.Trim() == "")
                    {
                        RegisterScript("alert('到达地不能为空，请选择至少一个城市作为出发地!');");
                        return false;
                    }
                    PolicyManageService.ReleaseSpecialPolicies(specialInfo, this.CurrentUser.UserName);
                }
                if (Request.QueryString["Check"] == "view")
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./special_policy_view.aspx'"
                            : "alert('复制成功');window.location.href='./special_policy_view.aspx'", true);
                }
                else
                {
                    RegisterScript(
                        type == "Update"
                            ? "alert('修改成功');window.location.href='./special_policy_manage.aspx'"
                            : "alert('复制成功');window.location.href='./special_policy_manage.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                if (type == "Update")
                {
                    ShowExceptionMessage(ex, "修改特殊政策");
                }
                else
                {
                    ShowExceptionMessage(ex, "复制特殊政策");
                }
            } return true;
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
                                      ? "./special_policy_view.aspx"
                                      : "./special_policy_manage.aspx");
            }
        }

        private IEnumerable<string> QueryBunks(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate)
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

            return (from bunk in result
                    orderby bunk.Value descending
                    select bunk.Key).ToList();

        }
    }
}