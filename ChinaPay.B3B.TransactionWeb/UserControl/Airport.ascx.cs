using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.TransactionWeb.UserControl {
    public partial class Airport : System.Web.UI.UserControl {
        private IEnumerable<Service.Foundation.Domain.Airport> m_source;
        private string m_defaultAirport = null;
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                bindDataSource();
                setDefaultValue();
            }
        }

        public string Code {
            get {
                if(string.IsNullOrWhiteSpace(m_defaultAirport) && this.ddlAirports != null) {
                    m_defaultAirport = this.ddlAirports.SelectedValue;
                }
                return m_defaultAirport;
            }
            set { m_defaultAirport = value; }
        }
        public void InitData(IEnumerable<Service.Foundation.Domain.Airport> source)
        {
            this.m_source = source;
        }

        private void bindDataSource() {
            this.ddlAirports.Items.Clear();
            this.ddlAirports.DataSource = from item in m_source ?? Service.FoundationService.Airports
                                          where item.Valid
                                          orderby item.Code.Value
                                          select new
                                          {
                                              Text = item.Code.Value + "-" + item.ShortName,
                                              Value = item.Code.Value
                                          };
            this.ddlAirports.DataTextField = "Text";
            this.ddlAirports.DataValueField = "Value";
            this.ddlAirports.DataBind();
            this.ddlAirports.Items.Insert(0, string.Empty);
        }
        private void setDefaultValue() {
            if(!string.IsNullOrWhiteSpace(m_defaultAirport)) {
                this.txtAirport.Text = this.ddlAirports.SelectedValue = m_defaultAirport;
            }
        }
    }
}