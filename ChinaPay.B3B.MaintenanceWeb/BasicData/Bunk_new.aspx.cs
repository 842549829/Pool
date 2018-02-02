using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Bunk_new : BasePage
    {
        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBindArea();
                if (!string.IsNullOrEmpty(Request.QueryString["action"]) && Request.QueryString["action"] == "update")
                {
                    Refresh();
                    this.hiddUpdate.Value = "update";
                }
            }
        }
        #region 数据绑定
        private void Refresh()
        {
            ChinaPay.B3B.Service.Foundation.Domain.Bunk bunk = FoundationService.QueryBunkNew(new Guid(Request.QueryString["Id"].ToString()));
            ddlAirline.SelectedValue = bunk.AirlineCode.Value;
            txtHBStartDate.Text = bunk.FlightBeginDate.ToString("yyyy-MM-dd");
            if (bunk.FlightEndDate != null) txtHBStopDate.Text = Convert.ToDateTime(bunk.FlightEndDate).ToString("yyyy-MM-dd");
            txtCpStartDate.Text = bunk.ETDZDate.ToString("yyyy-MM-dd");
            txtRefundRegulation.Text = bunk.RefundRegulation;
            txtChangeRegulation.Text = bunk.ChangeRegulation;
            txtEndorseRegulation.Text = bunk.EndorseRegulation;
            txtRemarks.Text = bunk.Remarks;
            radseatlist.SelectedIndex = bunk.Valid == true ? 0 : 1;
            txtCwCode.Text = bunk.Code.Value;
            //舱位类型  
            if (bunk is EconomicBunk)//经济舱位
            {
                radiolist.SelectedValue = ((int)BunkType.Economic).ToString();
                EconomicBunk economicBunk = bunk as EconomicBunk;
                this.ddlDepartCity.SelectedValue = economicBunk.DepartureCode.Value;
                this.ddlArriveCity.SelectedValue = economicBunk.ArrivalCode.Value;
                this.txtDiscount.Text = (economicBunk.Discount * 100).ToString();
                this.txtCwCode.Text = economicBunk.Code.Value;
                if (economicBunk.Extended.Count() > 0)
                    UpdateData(economicBunk.Extended);
            }
            else if (bunk is FirstBusinessBunk)
            {
                radiolist.SelectedValue = ((int)BunkType.FirstOrBusiness).ToString();
                FirstBusinessBunk firstBusinessBunk = bunk as FirstBusinessBunk;
                this.ddlDepartCity.SelectedValue = firstBusinessBunk.DepartureCode.Value;
                this.ddlArriveCity.SelectedValue = firstBusinessBunk.ArrivalCode.Value;
                setDropDownListSelectItemByText(this.ddltdType, firstBusinessBunk.Description);
                this.txtCwCode.Text = firstBusinessBunk.Code.Value;
                this.txtDiscount.Text = (firstBusinessBunk.Discount * 100).ToString();
                if (firstBusinessBunk.Extended.Count() > 0)
                    UpdateData(firstBusinessBunk.Extended);
            }
            else if (bunk is PromotionBunk)
            {
                radiolist.SelectedValue = ((int)BunkType.Promotion).ToString();
                PromotionBunk promotionBunk = bunk as PromotionBunk;
                setDropDownListSelectItemByText(this.ddlTJType, promotionBunk.Description);
                this.txtCwCode.Text = promotionBunk.Code.Value;
                if (promotionBunk.Extended.Count() > 0)
                {
                    int index = 0;
                    StringBuilder str = new StringBuilder();
                    foreach (string cal in promotionBunk.Extended)
                    {
                        str.Append("(tr class='tr')(th height='23')扩展：(/th)(td colspan='3')(input type='text' class='input2' id='txtCodeExtend" + index + "'  value='" + cal + "' /)(/tr)");
                        index++;
                    }
                    hiddindex.Value = index.ToString();
                    hiddtr.Value = str.ToString();
                }
            }
            else if (bunk is ProductionBunk)
            {
                radiolist.SelectedValue = ((int)BunkType.Production).ToString();
            }
            else if (bunk is TransferBunk)
            {
                radiolist.SelectedValue = ((int)BunkType.Transfer).ToString();
            }
            else if (bunk is FreeBunk)
            {
                radiolist.SelectedValue = ((int)BunkType.Free).ToString();
                FreeBunk promotionBunk = bunk as FreeBunk;
                setDropDownListSelectItemByText(this.dropMpType, promotionBunk.Description);
            }
            else if (bunk is TeamBunk)
            {
                radiolist.SelectedValue = ((int)BunkType.Team).ToString();
            }
            ddlAirline.Enabled = false;
            ddlDepartCity.Enabled = false;
            ddlArriveCity.Enabled = false;
            txtCwCode.Enabled = false;
            radiolist.Enabled = false;
        }
        #endregion
        /// <summary>
        /// 构造明折明扣子舱位
        /// </summary>
        /// <param name="code">舱位代码</param>
        public void UpdateData(IEnumerable<ExtendedWithDiscountBunk> code)
        {
            int index = 0;
            StringBuilder str = new StringBuilder();
            foreach (ExtendedWithDiscountBunk cal in code)
            {
                str.Append("(tr class='tr')(th height='23')扩展：(/th)(td)(input type='text' class='input2' id='txtCodeExtend" + index + "' name='txtCodeExtend" + index + "' value='" + cal.Code + "' /)(/td)(th height='23')折扣：(/th)(td)(input type='text' class='input2 zhekou' id='txtRateExtend" + index + "' name='txtRateExtend" + index + "' value='" + (cal.Discount * 100).ToString() + "\')(/td)(/tr)");
                index++;
            }
            hiddindex.Value = index.ToString();
            hiddtr.Value = str.ToString();
        }

        #region 列表绑定
        /// <summary>
        /// 列表绑定
        /// </summary>
        private void DataBindArea()
        {
            //舱位类型 
            System.Reflection.FieldInfo[] classType = typeof(BunkType).GetFields();
            foreach (var item in classType)
            {
                if (!item.IsSpecialName)//反射出第一个Field为特殊Field
                {
                    BunkType obj = (BunkType)item.GetRawConstantValue();//对应的文章
                    string text = obj.GetDescription();
                    string value = item.GetRawConstantValue().ToString();//对应的值
                    radiolist.Items.Add(new ListItem(text, value));
                }
            }
            this.radiolist.Items[0].Selected = true;
            //航空公司
            foreach (var item in FoundationService.Airlines)
            {
                this.ddlAirline.Items.Add(new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value));
            }
            this.ddlAirline.Items.Insert(0, new ListItem("-请选择-", "0"));
            //到达机场 出发机场

            foreach (var item in FoundationService.Airports)
            {
                string name = item.Code.Value + "-" + item.ShortName;
                this.ddlDepartCity.Items.Add(new ListItem(name, item.Code.Value));
                this.ddlArriveCity.Items.Add(new ListItem(name, item.Code.Value));
            }
            this.ddlDepartCity.Items.Insert(0, new ListItem("-所有-", "0"));
            this.ddlArriveCity.Items.Insert(0, new ListItem("-所有-", "0"));

            //头等/公务舱描述
            this.ddltdType.Items.Clear();
            this.ddltdType.DataSource = SystemDictionaryService.Query(SystemDictionaryType.FirstOrBusinessBunkDescription);
            this.ddltdType.DataTextField = "Name";
            this.ddltdType.DataValueField = "Id";
            this.ddltdType.DataBind();
            this.ddltdType.Items.Insert(0, new ListItem("-请选择-", "0"));

            //特价舱描述
            this.ddlTJType.Items.Clear();
            this.ddlTJType.DataSource = SystemDictionaryService.Query(SystemDictionaryType.PromotionBunkDescription);
            this.ddlTJType.DataTextField = "Name";
            this.ddlTJType.DataValueField = "Id";
            this.ddlTJType.DataBind();
            this.ddlTJType.Items.Insert(0, new ListItem("-请选择-", "0"));

            //免票描述
            dropMpType.Items.Clear();
            dropMpType.DataSource = SystemDictionaryService.Query(SystemDictionaryType.FreeDescription);
            dropMpType.DataTextField = "Name";
            dropMpType.DataValueField = "Id";
            dropMpType.DataBind();
            dropMpType.Items.Insert(0, new ListItem("-请选择-", "0"));
            Service.Foundation.Domain.Bunk bunk = string.IsNullOrEmpty(Request.QueryString["Id"]) ? null : FoundationService.QueryBunkNew(new Guid(Request.QueryString["Id"]));
            //适用行程
            var voyageTypes = (Enum.GetValues(typeof(VoyageTypeValue)) as VoyageTypeValue[]).Select(item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
            foreach (var item in voyageTypes)
            {
                var listIetm = new ListItem();
                if (bunk != null && (bunk.VoyageType & (VoyageTypeValue)item.Key) == (VoyageTypeValue)item.Key)
                {
                    listIetm.Selected = true;
                }
                listIetm.Value = item.Key.ToString();
                listIetm.Text = item.Value;
                chklVoyageType.Items.Add(listIetm);
            }
            //旅行类型
            var travelTypes = (Enum.GetValues(typeof(TravelTypeValue)) as TravelTypeValue[]).Select(item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
            foreach (var item in travelTypes)
            {
                var listItem = new ListItem();
                if (bunk != null && (bunk.TravelType & (TravelTypeValue)item.Key) == (TravelTypeValue)item.Key)
                {
                    listItem.Selected = true;
                }
                listItem.Value = item.Key.ToString();
                listItem.Text = item.Value;
                chklTravelType.Items.Add(listItem);
            }
            //旅客类型
            var passengerTypes = (Enum.GetValues(typeof(PassengerTypeValue)) as PassengerTypeValue[]).Select(item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
            foreach (var item in passengerTypes)
            {
                var listItem = new ListItem();
                if (bunk != null && (bunk.PassengerType & (PassengerTypeValue)item.Key) == (PassengerTypeValue)item.Key)
                {
                    listItem.Selected = true;
                }
                listItem.Text = item.Value;
                listItem.Value = item.Key.ToString();
                chklPassengerType.Items.Add(listItem);
            }
        }
        #endregion
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {

            //舱位类型
            var bunkType = (BunkType)int.Parse(this.radiolist.SelectedValue);
            string AirlineCode = this.ddlAirline.SelectedValue;//航空公司代码 
            DateTime FlightBeginDate = Convert.ToDateTime(this.txtHBStartDate.Text);//航班开始日期
            string FlightEndDate = string.Empty;
            if (!string.IsNullOrEmpty(this.txtHBStopDate.Text)) FlightEndDate = this.txtHBStopDate.Text;//航班截止时间
            DateTime ETDZDate = Convert.ToDateTime(this.txtCpStartDate.Text);//出票时间
            string Code = this.txtCwCode.Text.Trim();//代码
            bool Valid = this.radseatlist.SelectedValue == "T" ? true : false;//状态
            //折扣
            decimal Discount = 0;
            if (bunkType == BunkType.Economic || bunkType == BunkType.FirstOrBusiness)
            {
                Discount = Convert.ToDecimal(this.txtDiscount.Text.Trim()) / 100;
            }
            //描述
            string Description = string.Empty;
            if (bunkType == BunkType.FirstOrBusiness) Description = this.ddltdType.SelectedItem.Text;
            if (bunkType == BunkType.Promotion) Description = this.ddlTJType.SelectedItem.Text;
            if (bunkType == BunkType.Free) Description = dropMpType.SelectedItem.Text;
            //子舱位
            string strSeatName = seatName.Value;
            //创建舱位
            BunkView bunkView = null;
            try
            {
                //验证
                ExistsValidate();
                switch (bunkType)
                {
                    case BunkType.Economic://经济舱位
                        var econmicBunkVie = new EconomicBunkView()
                        {
                            Discount = Discount
                        };
                        if (this.ddlDepartCity.SelectedValue != "0")
                        {
                            econmicBunkVie.Departure = this.ddlDepartCity.SelectedValue;
                        }
                        if (this.ddlArriveCity.SelectedValue != "0")
                        {
                            econmicBunkVie.Arrival = this.ddlArriveCity.SelectedValue;
                        }
                        if (!string.IsNullOrEmpty(strSeatName))
                        {
                            string[] strseatlist = strSeatName.Split('|');
                            for (int i = 0; i < strseatlist.Length; i++)
                            {
                                ExtendedWithDiscountBunkView ewbv = new ExtendedWithDiscountBunkView();
                                ewbv.Code = strseatlist[i].Split(',')[0];
                                ewbv.Discount = decimal.Parse(strseatlist[i].Split(',')[1]) / 100;
                                econmicBunkVie.AddExtended(ewbv);
                            }
                        }
                        bunkView = econmicBunkVie;
                        break;
                    case BunkType.FirstOrBusiness://头等公务舱
                        var firstBusinessBunkView = new FirstBusinessBunkView()
                        {
                            Description = Description,
                            Discount = Discount
                        };
                        if (this.ddlDepartCity.SelectedValue != "0")
                        {
                            firstBusinessBunkView.Departure = this.ddlDepartCity.SelectedValue;
                        }
                        if (this.ddlArriveCity.SelectedValue != "0")
                        {
                            firstBusinessBunkView.Arrival = this.ddlArriveCity.SelectedValue;
                        }
                        if (!string.IsNullOrEmpty(strSeatName))
                        {
                            string[] strseatlist = strSeatName.Split('|');

                            for (int i = 0; i < strseatlist.Length; i++)
                            {
                                ExtendedWithDiscountBunkView ewbv = new ExtendedWithDiscountBunkView();
                                ewbv.Code = strseatlist[i].Split(',')[0];
                                ewbv.Discount = decimal.Parse(strseatlist[i].Split(',')[1]) / 100;
                                firstBusinessBunkView.AddExtended(ewbv);
                            }
                        }
                        bunkView = firstBusinessBunkView;
                        break;
                    case BunkType.Promotion://特价舱位
                        var promotionBunkView = new PromotionBunkView()
                        {
                            Description = Description
                        };
                        string strSeatTJ = this.seatTJ.Value;
                        if (!string.IsNullOrEmpty(strSeatTJ))
                        {
                            string[] strseatlist = strSeatName.Split('|');
                            for (int i = 0; i < strseatlist.Length; i++)
                                promotionBunkView.AddExtended(strseatlist[i].Split(',')[0]);
                        }
                        bunkView = promotionBunkView;
                        break;
                    case BunkType.Production://往返产品舱
                        bunkView = new ProductionBunkView();
                        break;
                    case BunkType.Transfer://中转联程舱
                        bunkView = new TransferBunkView();
                        break;
                    case BunkType.Free://免票
                        bunkView = new FreeBunkView()
                        {
                            Description = Description
                        };
                        break;
                    case BunkType.Team://团队
                        bunkView = new TeamBunkView();
                        break;
                }
                bunkView.Airline = AirlineCode;
                bunkView.Code = Code;
                bunkView.RefundRegulation = txtRefundRegulation.Text.Trim();
                bunkView.ChangeRegulation = txtChangeRegulation.Text.Trim();
                bunkView.EndorseRegulation = txtEndorseRegulation.Text.Trim();
                bunkView.Remarks = txtRemarks.Text.Trim();
                bunkView.ETDZDate = ETDZDate;
                bunkView.Valid = Valid;
                if (!string.IsNullOrEmpty(FlightEndDate)) bunkView.FlightEndDate = Convert.ToDateTime(FlightEndDate);
                bunkView.FlightBeginDate = FlightBeginDate;
                for (int i = 0; i < chklVoyageType.Items.Count; i++)
                {
                    if (chklVoyageType.Items[i].Selected)
                    {
                        bunkView.VoyageType |= (VoyageTypeValue)(int.Parse(chklVoyageType.Items[i].Value));
                    }
                }
                for (int i = 0; i < chklTravelType.Items.Count; i++)
                {
                    if (chklTravelType.Items[i].Selected)
                    {
                        bunkView.TravelType |= (TravelTypeValue)(int.Parse(chklTravelType.Items[i].Value));
                    }
                }
                for (int i = 0; i < chklPassengerType.Items.Count; i++)
                {
                    if (chklPassengerType.Items[i].Selected)
                    {
                        bunkView.PassengerType |= (PassengerTypeValue)(int.Parse(chklPassengerType.Items[i].Value));
                    }
                }
                //添加
                if (Request.QueryString["action"] != null && Request.QueryString["action"].ToString() == "add")
                {
                    try
                    {
                        FoundationService.AddBunk(bunkView, CurrentUser.UserName);
                        RegisterScript("alert('添加成功！'); window.location.href='Bunk.aspx';");
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else//修改 
                {
                    try
                    {
                        FoundationService.UpdateBunk(new Guid(Request.QueryString["Id"].ToString()), bunkView, CurrentUser.UserName);
                        RegisterScript("alert('修改成功！'); window.location.href='Bunk.aspx?Search=Back';");

                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
            catch (Exception exw)
            {
                ShowExceptionMessage(exw, "操作");
            }
        }

        void setDropDownListSelectItemByText(DropDownList control, string text)
        {
            for (int i = 0; i < control.Items.Count; i++)
            {
                var item = control.Items[i];
                if (item.Text == text)
                {
                    control.SelectedIndex = i;
                    break;
                }
            }
        }

        void ExistsValidate()
        {
            if (!this.chklVoyageType.SelectedValue.Any())
            {
                throw new ChinaPay.Core.CustomException("请选择适用行程类型");
            }
            if (!this.chklTravelType.SelectedValue.Any())
            {
                throw new ChinaPay.Core.CustomException("请选择适用旅行类型");
            }
            if (!this.chklPassengerType.SelectedValue.Any())
            {
                throw new ChinaPay.Core.CustomException("请选择适用旅客类型");
            }
            if (string.IsNullOrEmpty(txtRefundRegulation.Text.Trim()))
            {
                throw new ChinaPay.Core.CustomException("退票条件不能为空");
            }
            if (string.IsNullOrEmpty(txtChangeRegulation.Text.Trim()))
            {
                throw new ChinaPay.Core.CustomException("更改规定不能为空");
            }
            if (string.IsNullOrEmpty(txtEndorseRegulation.Text.Trim()))
            {
                throw new ChinaPay.Core.CustomException("签转规定不能为空");
            }
        }
        #endregion
    }
}
