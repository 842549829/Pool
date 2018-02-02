using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.TransactionWeb.UserControl {
    public partial class Company : System.Web.UI.UserControl
    {
        private IEnumerable<DataTransferObject.Organization.CompanyInitInfo> m_companies = null;
        private Guid? _companyId = null;
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                bindDataSource();
            }
        }
        public void InitCompanies(IEnumerable<DataTransferObject.Organization.CompanyInitInfo> companies)
        {
            this.m_companies = companies;
        }
        public Guid? CompanyId {
            get {
                if(!this._companyId.HasValue && this.ddlCompanies != null) {
                    Guid company;
                    if(Guid.TryParse(this.ddlCompanies.SelectedValue, out company)) {
                        return company;
                    }
                }
                return null;
            }set { _companyId = value; }
        }
        private void bindDataSource() {
            this.ddlCompanies.Items.Clear();
            if (m_companies != null)
            {
                this.ddlCompanies.DataSource = from item in m_companies
                                               orderby item.UserNo
                                               select new
                                               {
                                                   Text = item.UserNo + "-" + item.AbbreviateName,
                                                   Value = item.CompanyId
                                               };
                this.ddlCompanies.DataTextField = "Text";
                this.ddlCompanies.DataValueField = "Value";
                this.ddlCompanies.DataBind();
            }
            this.ddlCompanies.Items.Insert(0, string.Empty);
            if(this._companyId.HasValue)
            {
                this.ddlCompanies.SelectedValue = this._companyId.Value.ToString();
            }
        }
    }
}