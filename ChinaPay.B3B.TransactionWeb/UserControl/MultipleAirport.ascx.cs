using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.UserControl {
    public partial class MultipleAirport : System.Web.UI.UserControl {
        private IEnumerable<Service.Foundation.Domain.Airport> m_source;
        private IEnumerable<string> m_airports;
        private bool m_requireInclude;
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                bindAirports();
            }
        }
        public void InitData(IEnumerable<Service.Foundation.Domain.Airport> source) {
            InitData(false, source);
        }
        public void InitData(bool requireInclude, IEnumerable<Service.Foundation.Domain.Airport> source) {
            this.m_requireInclude = requireInclude;
            this.m_source = source;
        }
        public void InitData(bool requireInclude, List<string> source) {
            this.m_requireInclude = requireInclude;
            this.m_airports = source;
        }
        private void bindAirports() {
            if(m_source == null) m_source = Service.FoundationService.Airports.Where(item => item.Valid);
            this.lbSource.DataSource = from item in m_source
                                       select new
                                                  {
                                                      Value = item.Code.Value,
                                                      Text = item.Code.Value + '-' + item.ShortName
                                                  };
            this.lbSource.DataTextField = "Text";
            this.lbSource.DataValueField = "Value";
            this.lbSource.DataBind();
            this.txtAirports.Text = this.m_airports == null ? string.Empty : this.m_airports.Join("/");
            this.divInclude.Visible = this.m_requireInclude;
        }

        public IEnumerable<string> AirportsCode {
            get {
                if(this.m_airports == null) {
                    this.m_airports = getAirports(!this.divInclude.Visible || this.rbInclude.Checked, this.txtAirports.Text);
                }
                return this.m_airports;
            }
            set { this.m_airports = value; }
        }
        private IEnumerable<string> getAirports(bool include, string airportCodes) {
            var inputAirports = airportCodes.Split('/');
            if(include) {
                return inputAirports;
            } else {
                return this.lbSource.Items.Cast<object>()
                    .Select((t, i) => this.lbSource.Items[i].Value)
                    .Where(item => !inputAirports.Contains(item));
            }
        }
    }
}