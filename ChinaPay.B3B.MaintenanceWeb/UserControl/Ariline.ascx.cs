using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.MaintenanceWeb.UserControl
{
    public partial class Ariline : System.Web.UI.UserControl
    {
        private IEnumerable<Service.Foundation.Domain.Airline> m_source;
        private string m_defaultAirline = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                bindDataSource();
                setDefaultValue();
            }
        }
        public string Code 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(m_defaultAirline) && ddlAirlins != null) 
                {
                    m_defaultAirline = ddlAirlins.SelectedValue;
                }
                return m_defaultAirline;
            }
            set { m_defaultAirline = value; }
        }
        public void InitData(IEnumerable<Service.Foundation.Domain.Airline> sourcr) 
        {
            this.m_source = sourcr;
        }
        private void bindDataSource() 
        {
            ddlAirlins.Items.Clear();
            ddlAirlins.DataSource = from item in m_source ?? Service.FoundationService.Airlines
                                    where item.Valid
                                    orderby item.Code.Value
                                    select new { 
                                        Text = item.Code.Value+"-"+item.ShortName,
                                        Value = item.Code.Value
                                    };
            ddlAirlins.DataTextField = "Text";
            ddlAirlins.DataValueField = "Value";
            ddlAirlins.DataBind();
            ddlAirlins.Items.Insert(0,string.Empty);
        }
        private void setDefaultValue() {
            if (!string.IsNullOrWhiteSpace(m_defaultAirline))
            {
                txtAriline.Text = ddlAirlins.SelectedValue = m_defaultAirline;
            }
        }
    }
}