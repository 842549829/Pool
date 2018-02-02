using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.Policy.Domain;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class PolicySet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");

            if (!IsPostBack)
            {
                var companyId = Request.QueryString["CompanyId"];
                if (companyId != null)
                {
                    Guid id = Guid.Parse(companyId);
                    this.BindPloicCountg(id);
                    this.BindAriline(id);
                    this.BindAirport(id);
                }
            }
        }
        private List<string> GetAirport(Guid id)
        {
            var policy = CompanyService.GetPolicySetting(id);
            List<string> list = new List<string>();
            if (policy == null) return null;
            string[] airports = policy.Departure.Split(',');
            for (int i = 0; i < airports.Length; i++)
            {
                list.Add(airports[i]);
            }
            return policy == null ? null : list;
        }
        private void BindAirport(Guid id)
        {
            this.ucMultipleAirport.InitData(FoundationService.Airports);
            var citys = this.GetAirport(id);
            if (citys != null)
            {
                this.ucMultipleAirport.AirportsCode = citys;
            }
        }
        private void BindAriline(Guid company)
        {
            var airlines = PolicySetService.QueryAirlines(company);
            foreach (Airline item in FoundationService.Airlines)
            {
                if (item.Valid)
                {
                    ListItem listItem = new ListItem(item.Code.Value);
                    listItem.Selected = airlines.Contains(item.Code.Value);
                    this.chklAirline.Items.Add(listItem);
                }
            }
        }
        private void BindPloicCountg(Guid id)
        {
            var policy = CompanyService.GetPolicySetting(id);
            if (policy != null)
            {
                this.txtPromotionCount.Text = policy.BargainCount.ToString();
                this.txtSinglenessCount.Text = policy.SinglenessCount.ToString();
                this.txtBusinessCount.Text = policy.BusinessCount.ToString();
                this.txtCostFreeCount.Text = policy.CostFreeCount.ToString();
                this.txtDisperseCount.Text = policy.DisperseCount.ToString();
                this.txtBlocCount.Text = policy.BlocCount.ToString();
                txtOtherSpecialCount.Text = policy.OtherSpecialCount.ToString();
                txtLowToHighCount.Text = policy.LowToHighCount.ToString();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                PolicySetService.Save(this.GetPolicy(), this.CurrentUser.UserName);
                Response.Redirect("./CompanyList.aspx?Search=Back", false);
            }
            catch (Exception ex) { ShowExceptionMessage(ex, "设置失败"); }
        }
        private SetPolicy GetPolicy()
        {
            return new SetPolicy(Guid.Parse(Request.QueryString["CompanyId"]))
            {
                PromotionCount = int.Parse(this.txtPromotionCount.Text.Trim()),
                BlocCount = int.Parse(this.txtBlocCount.Text.Trim()),
                BusinessCount = int.Parse(this.txtBusinessCount.Text.Trim()),
                CostFreeCount = int.Parse(this.txtCostFreeCount.Text.Trim()),
                DisperseCount = int.Parse(this.txtDisperseCount.Text.Trim()),
                SinglenessCount = int.Parse(this.txtSinglenessCount.Text.Trim()),
                OtherSpecialCount = int.Parse(txtOtherSpecialCount.Text.Trim()),
                LowToHighCount = int.Parse(txtLowToHighCount.Text.Trim()),
                Airlines = this.GetAirlines(),
                Departure = this.GetDeparture()
            };
        }
        private IEnumerable<string> GetDeparture()
        {
            return this.ucMultipleAirport.AirportsCode;
        }
        private IEnumerable<string> GetAirlines()
        {
            List<string> lists = new List<string>();
            for (int i = 0; i < this.chklAirline.Items.Count; i++)
                if (this.chklAirline.Items[i].Selected)
                    lists.Add(this.chklAirline.Items[i].Text);
            return lists;
        }
    }
}