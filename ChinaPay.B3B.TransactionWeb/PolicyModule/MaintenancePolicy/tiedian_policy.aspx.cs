using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class tiedian_policy : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitDataValue();
            }
        }
        private void InitDataValue()
        {
            NormalPolicy normal = PolicyManageService.GetNormalPolicy(Guid.Parse(Request.QueryString["id"].ToString()));
            if (normal != null)
            {
                lblCreator.Text = "<a href='javascript:GoTo(\"" + normal.Owner + "\")' title='查看该供应商的公司基础信息'>" + normal.AbbreviateName + "</a>";
                lblVoyage.Text = normal.VoyageType.GetDescription();
                lblAirline.Text = normal.Airline;
                lblDepartureDate.Text = normal.DepartureDateStart.ToString("yyyy-MM-dd") + "至" + normal.DepartureDateEnd.ToString("yyyy-MM-dd");
                lblDeparture.Text = normal.Departure;
                lblArrival.Text = normal.Arrival;
                lblDepartureFilght.Text =
                    normal.DepartureFlightsFilterType == Common.Enums.LimitType.None ? "不限" :
                    normal.DepartureFlightsFilterType == Common.Enums.LimitType.Include ? "适用:" + normal.DepartureFlightsFilter :
                    normal.DepartureFlightsFilterType == Common.Enums.LimitType.Exclude ? "不适用:" + normal.DepartureFlightsFilter : "";
                lblExceptAirlines.Text = normal.ExceptAirways;

                txtStartTime.Text = normal.DepartureDateStart.ToString("yyyy-MM-dd");
                txtEndTime.Text = normal.DepartureDateEnd.ToString("yyyy-MM-dd");
                hidTime.Value = normal.DepartureDateEnd.ToString("yyyy-MM-dd");
                lblCommission.Text = (normal.ProfessionCommission * 100).TrimInvaidZero();
                chkBunksList.DataSource = normal.Berths.Split(',').ToList();
                chkBunksList.DataBind();
            }

            grvTiedian.DataSource = from item in PolicySetService.QueryNormalPolicySetting(Guid.Parse(Request.QueryString["id"].ToString()), true, null)
                                    orderby item.OperationTime descending
                                    select new
                                    {
                                        item.Berths,
                                        Commission = (item.Commission * 100).TrimInvaidZero() + "%",
                                        item.Creator,
                                        item.Enable,
                                        EndTime = item.EndTime.ToString("yyyy-MM-dd"),
                                        item.FlightsFilter,
                                        item.Id,
                                        item.OperationTime,
                                        item.PolicyId,
                                        item.Remark,
                                        StartTime = item.StartTime.ToString("yyyy-MM-dd"),
                                        item.Type
                                    };
            grvTiedian.DataBind();


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            NormalPolicySetting setting = new NormalPolicySetting();
            setting.Creator = CurrentUser.UserName;
            setting.EndTime = DateTime.Parse(txtEndTime.Text);
            setting.StartTime = DateTime.Parse(txtStartTime.Text);
            setting.PolicyId = Guid.Parse(Request.QueryString["id"].ToString());
            setting.Remark = txtRemark.Text;
            setting.Type = true;
            setting.FlightsFilter = txtAirlines.Text.Trim().ToUpper();
            setting.Commission = decimal.Parse(txtCommission.Text) / 100;
            string berths = "";
            foreach (ListItem item in chkBunksList.Items)
            {
                if (item.Selected)
                {
                    if (berths == "")
                    {
                        berths += item.Value;
                    }
                    else
                    {
                        berths += "," + item.Value;
                    }
                }
            }
            setting.Berths = berths;

            PolicySetService.AddNormalPolicySetting(setting);
            InitDataValue();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("./base_policy_info.aspx?id=" + Request.QueryString["id"], true);
        }

        protected void grvTiedian_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "disEnable")
            {
                var str = e.CommandArgument.ToString();
                string id = str.Split('|')[0];
                string enable = str.Split('|')[1];
                PolicySetService.UpdateNormalPolicySetting(Guid.Parse(id), !bool.Parse(enable), CurrentUser.UserName, true);
                InitDataValue();
            }

        }

    }
}