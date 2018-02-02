using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule
{
    public partial class policy_coordination_addModify : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtTimeStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTimeEnd.Text = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                var list = from item in ChinaPay.B3B.Service.FoundationService.Airlines
                           select new
                           {
                               Code = item.Code.Value
                           };
                chkAirlineList.DataSource = list;
                chkAirlineList.DataTextField = "Code";
                chkAirlineList.DataValueField = "Code";
                chkAirlineList.DataBind();

            }
        }

        private void InitValue()
        {
            if (Request.QueryString["id"] != null)
            {
                PolicyHarmonyInfo hoarmony = PolicyManageService.GetPolicyHarmonyInfo(Guid.Parse(Request.QueryString["id"]));
                if (hoarmony != null)
                {
                    //选择曾经的航空公司
                    foreach (ListItem item in chkAirlineList.Items)
                    {
                        foreach (string str in hoarmony.Airlines.Split(','))
                        {
                            if (item.Value == str)
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }
                    //始发地
                    txtDepartureAirports.AirportsCode = hoarmony.Departure.Split('/');
                    //目的地
                    txtArrivalAirports.AirportsCode = hoarmony.Arrival.Split('/');

                    chkPolicyBargin.Checked = (hoarmony.PolicyType & Common.Enums.PolicyType.Bargain) == Common.Enums.PolicyType.Bargain;
                    chkPolicyNormal.Checked = (hoarmony.PolicyType & Common.Enums.PolicyType.Normal) == Common.Enums.PolicyType.Normal;

                    radSubCommission.Checked = (hoarmony.DeductionType == Common.Enums.DeductionType.Subordinate);
                    radIntCommission.Checked = (hoarmony.DeductionType == Common.Enums.DeductionType.Profession);

                    txtTimeStart.Text = hoarmony.EffectiveLowerDate.Date.ToString("yyyy-MM-dd");
                    txtTimeEnd.Text = hoarmony.EffectiveUpperDate.Date.ToString("yyyy-MM-dd");
                    txtXieTiao.Text = (hoarmony.HarmonyValue * 100).TrimInvaidZero();
                    txtRemark.Text = hoarmony.Remark;
                    hidTime.Value = hoarmony.CreateTime.ToString();
                }
            }
        }
        private string GetLimitCity(string limitCity)
        {
            string str = "";
            foreach (var item in limitCity.Split('/'))
            {
                if (str != "")
                {
                    str += "/";
                }
                str += ChinaPay.B3B.Service.FoundationService.QueryCity(item).Name;
            }
            return str;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PolicyHarmonyInfo Harmony = GetVaule();
                if (Request.QueryString["id"] != null)
                {
                    Harmony.Id = Guid.Parse(Request.QueryString["id"]);
                }
                if (Harmony != null)
                {
                    try
                    {
                        PolicyManageService.SetPolicyHarmony(Harmony);
                        if (Harmony.Id == Guid.Empty)
                        {
                            RegisterScript("alert('添加成功');window.location.href='./policy_coordination_manage.aspx'", true);
                        }
                        else
                        {
                            RegisterScript("alert('修改成功');window.location.href='./policy_coordination_manage.aspx'", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "政策协调添加/修改");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "政策协调修改/添加");
            }
        }

        private PolicyHarmonyInfo GetVaule()
        {
            string airline = "";
            foreach (ListItem item in chkAirlineList.Items)
            {
                if (item.Selected)
                {
                    airline += item.Value + ",";
                }
            }
            if (airline == "")
            {
                ShowMessage("没有选择航空公司，请先选择！");
                return null;
            }
            airline = airline.Substring(0, airline.Length - 1);
            try
            {
                hidTime.Value = hidTime.Value == "" ? DateTime.Now + "" : hidTime.Value;
                PolicyHarmonyInfo Harmony = new PolicyHarmonyInfo
                {
                    Account = this.CurrentUser.UserName,
                    Airlines = airline,
                    Arrival = txtArrivalAirports.AirportsCode.Join("/"),
                    Departure = txtDepartureAirports.AirportsCode.Join("/"),
                    //CityLimit = "",
                    DeductionType = radSubCommission.Checked ? Common.Enums.DeductionType.Subordinate : Common.Enums.DeductionType.Profession,
                    EffectiveLowerDate = DateTime.Parse(txtTimeStart.Text),
                    EffectiveUpperDate = DateTime.Parse(txtTimeEnd.Text),
                    HarmonyValue = decimal.Parse(txtXieTiao.Text) / 100,
                   // IsVIP = false,
                    LastModifyTime = DateTime.Now,
                    LastModifyName = this.CurrentUser.UserName,
                    CreateTime = DateTime.Parse(hidTime.Value),
                    Remark = txtRemark.Text
                };
                if (chkPolicyBargin.Checked)
                {
                    Harmony.PolicyType |= Common.Enums.PolicyType.Bargain;
                }
                if (chkPolicyNormal.Checked)
                {
                    Harmony.PolicyType |= Common.Enums.PolicyType.Normal;
                }
                return Harmony;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "政策协调修改/添加");
            }
            return null;
        }

        protected void chkAirlineList_DataBound(object sender, EventArgs e)
        {
            InitValue();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("./policy_coordination_manage.aspx");
        }

    }
}