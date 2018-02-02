using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule
{
    public partial class policy_coordination_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                txtTimeStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTimeEnd.Text = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                var query_list = from item in ChinaPay.B3B.Service.FoundationService.Airlines
                                 select new
                                 {
                                     Code = item.Code,
                                     Name = item.Code + "-" + item.ShortName
                                 };
                ddlAirline.DataSource = query_list;
                ddlAirline.DataTextField = "Name";
                ddlAirline.DataValueField = "Code";
                ddlAirline.DataBind();
                ddlAirline.Items.Insert(0, new ListItem { Value = "", Text = "-请选择-" });

            }
        }

        private bool GetCheckBoxValue()
        {
            if (hidIds.Value == "")
            {
                ShowMessage("没有选中任何行,执行被取消");
                return false;
            }
            return true;
        }

        private void QueryBackPolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
            hidIds.Value = "";
        }
        void QueryPolicy(Pagination pagination)
        {
            try
            {
                var list_query = PolicyManageService.QueryPolicyHarmonies(GetCondition(), pagination);
                var list = from item in list_query
                           select new
                           {
                               Id = item.Id,
                               AirLine = item.Airlines,
                               Departure = item.Departure,
                               Arrival = item.Arrival,
                               HarmonyValue = (item.HarmonyValue * 100).TrimInvaidZero(),
                               PolicyType = GetPolicyInfo(item.PolicyType),
                               CommissionType = item.DeductionType == Common.Enums.DeductionType.Profession ? "同行返佣" : "下级返佣",
                               TimeInfo = item.EffectiveLowerDate.ToString("yyyy-MM-dd") + "<br />" + item.EffectiveUpperDate.ToString("yyyy-MM-dd"),
                               CreateName = item.Account,
                               CreateTime = item.CreateTime,
                               LastModifyTime = item.LastModifyTime,
                               LastModifyName = item.LastModifyName,
                               Remark = item.Remark
                           };
                this.grv_back.DataSource = list;
                this.grv_back.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    showempty.Visible = false;
                    grv_back.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    this.pager.Visible = false;
                    showempty.Visible = true;
                }

            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }
        private string GetLimitCity(string limitCity)
        {
            string str = "";
            foreach (var item in limitCity.Split('/'))
            {
                if (str != "")
                {
                    str += ",";
                }
                str += ChinaPay.B3B.Service.FoundationService.QueryCity(item).Name;
            }
            return str;
        }

        private PolicyHarmonyQueryParameter GetCondition()
        {
            Common.Enums.PolicyType policyType = Common.Enums.PolicyType.Normal;
            if (ddlPolicyType.SelectedValue == "0")
                policyType = Common.Enums.PolicyType.Normal;

            if (ddlPolicyType.SelectedValue == "1")
                policyType = Common.Enums.PolicyType.Bargain;


            PolicyHarmonyQueryParameter parameter = new PolicyHarmonyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                EffectTimeStart = txtTimeStart.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtTimeStart.Text),
                EffectTimeEnd = txtTimeEnd.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtTimeEnd.Text)
            };
            if (ddlPolicyType.SelectedIndex != 0)
            {
                parameter.PolicyType = policyType;
            }
            return parameter;
        }
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QueryPolicy(new Pagination { GetRowCount = true, PageSize = pager.PageSize, PageIndex = 1 });
            pager.CurrentPageIndex = 1;
        }

        protected void grv_back_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                PolicyManageService.Delete(this.CurrentUser.UserName,Guid.Parse(e.CommandArgument.ToString()));
                QueryBackPolicy(grv_back.PageIndex + 1);
            }
        }
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            Response.Redirect("./policy_coordination_addModify.aspx");
        }
        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var list_ids = hidIds.Value.Split(',').Select(item => Guid.Parse(item)).ToArray();
                PolicyManageService.Delete(this.CurrentUser.UserName,list_ids);
                QueryBackPolicy(grv_back.PageIndex + 1);
            }
        }

        private string GetPolicyInfo(Common.Enums.PolicyType arg)
        {
            string str = "";
            if ((arg & Common.Enums.PolicyType.Bargain) == Common.Enums.PolicyType.Bargain)
                str += "特价政策<br />";
            if ((arg & Common.Enums.PolicyType.Normal) == Common.Enums.PolicyType.Normal)
                str += "普通政策<br />";
            //if ((arg & Common.Enums.PolicyType.RoundTrip) == Common.Enums.PolicyType.RoundTrip)
            //    str += "往返政策<br />";

            return str;
        }
    }
}