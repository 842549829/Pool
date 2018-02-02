using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.UserControl
{
    public partial class MultipleCity : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void InitData(IEnumerable<Service.Foundation.Domain.City> source)
        {
            if (source == null) source = ChinaPay.B3B.Service.FoundationService.Cities;
            lbSourceCity.DataSource = from item in source
                                      select new
                                      {
                                          Code = item.Code,
                                          Text = item.Name
                                      };

            lbSourceCity.DataTextField = "Text";
            lbSourceCity.DataValueField = "Code";
            lbSourceCity.DataBind();
            txtCitys.Text = CityText == null ? string.Empty : CityText;
            hidCityCode.Value = CityCode == null ? string.Empty : CityCode;
        }
        public string CityCode { get { return hidCityCode.Value; } }
        public string CityText { get; set; }
    }
}