using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Register
{
    public partial class DistributionOEMRegisterSucceed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string type = Request.QueryString["Type"];
                InitData(type);
            }
        }
        private void InitData(string type)
        {
            CompanyType companyType = (CompanyType)int.Parse(type);
            string b3bAccount = Request.QueryString["Account"];
            lblNotProviderAccount.InnerText = b3bAccount;
            string accounType = Request.QueryString["AccounType"], name = Request.QueryString["Name"];
            string poolpayAcccount = Request.QueryString["pooypayAccount"];
            if (!string.IsNullOrEmpty(poolpayAcccount) && b3bAccount != poolpayAcccount)
            {
                lbpoolpayAccount1.InnerText = "您的国付通账号为：" + poolpayAcccount;
                lbpoolpayAccount1.Visible  = true;
                accountTip1.InnerText  = "欢迎使用我们平台购买机票！";
            }
            if (bool.Parse(accounType))
                lblNotProviderCompanyName.InnerText = "您的个人名称为：" + name;
            else
                lblNotProviderCompanyName.InnerText = "您的企业名称为：" + name;
        }
    }
}