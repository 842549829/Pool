using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage
{
    public partial class RegisterPaySucceed : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["accountNo"]!=null)
                {
                    this.lblAccounNo.InnerText = Request.QueryString["accountNo"];
                    if (Session["Info"] != null) Session.Remove("Info");
                    string addAccountType = Session["AddAccountType"].ToString();
                    if (Session["AddAccountType"] != null) Session.Remove("AddAccountType");
                    if (addAccountType == "Extend")
                    {
                        this.btnOk.Attributes.Add("onclick", "window.location.href='../ExtendCompanyList.aspx';");
                    }
                    else{
                        this.btnOk.Attributes.Add("onclick", "window.location.href='../LowerComapnyInfoUpdate/Lower_manage.aspx';");                            
                    }
                }
            }
        }
    }
}