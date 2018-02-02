using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.UserControl
{
    public partial class CompanyC : System.Web.UI.UserControl
    {
        //private Guid? _companyId;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public Guid? CompanyId
        {
            get
            {
                Guid company;
                if (Guid.TryParse(hidCompanyId.Value, out company))
                    return company;
                return null;
            }
            //set { _companyId = value; }
        }

        public void SetCompanyType(ChinaPay.B3B.Common.Enums.CompanyType? type,bool showDisable = false)
        {
            if (type.HasValue)
            {
                hidCompanyType.Value = ((byte)type).ToString();
            }
            else
            {
                hidCompanyType.Value = "0";
            }
            if (showDisable)
            {
                hidIsShowDisable.Value = "1";
            }
            else
            {
                hidIsShowDisable.Value = "0";
            }
        }
        public void SetCompanyName(Guid companyId)
        {
            hidCompanyId.Value = companyId.ToString();
            var e = EmployeeService.QueryCompanyAdmin(companyId);
            var c = CompanyService.GetCompanyInfo(companyId);
            txtCompanyName.Text = e.Login + "-" + c.AbbreviateName;
        }
    }
}