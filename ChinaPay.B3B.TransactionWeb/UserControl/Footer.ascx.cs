using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.TransactionWeb.UserControl
{
    public partial class Footer : System.Web.UI.UserControl
    {
        public string EnterpriseQQ;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var contact = BasePage.CurrenContract;
                lblChangeServicePhone.Text = contact.ServicePhone;
                lblOutTicketPhone.Text = contact.ServicePhone; ;
                lblEmergencyPhone.Text = contact.EmergencyPhone;
                lblPayServicePhone.Text = contact.PayServicePhone;
                lblComplainPhone.Text = contact.ComplainPhone;
                lblRefundPhone.Text = contact.RefundPhone;
                EnterpriseQQ = OEMContract.ContractInfoReplace(contact.EnterpriseQQ);
                var oem = BasePage.OEM;
                //加载头部连接
                if (oem != null && oem.Setting != null && oem.Setting.FooterLinks != null)
                {
                    string str = "";
                    foreach (var item in oem.Setting.FooterLinks)
                    {
                        if (str == "")
                        {
                            str += "<a href='" + item.URL + "' title='" + item.Remark + "'class='firstChild'  target='_blank' >" + item.LinkName + "</a>";
                        }
                        else
                        {
                            str += "<a href='" + item.URL + "' title='" + item.Remark + "'  target='_blank' >" + item.LinkName + "</a>";
                        }
                    }
                    links.InnerHtml = str;
                    copyright.InnerHtml = "<span>" + oem.Setting.CopyrightInfo + "</span><span>" + oem.Company.AbbreviateName + "</span><span>" + oem.DomainName + "</span> <span>" + oem.ICPRecord + "</span>" + (oem.EmbedCode == "" ? "" : "<script src='" + oem.EmbedCode + "'  type='text/javascript' language='JavaScript'></script>");
                    //companyName.InnerHtml = oem.Company.AbbreviateName;
                    //domain.InnerHtml = oem.DomainName;
                    //copyright.InnerHtml = oem.Setting.CopyrightInfo;
                    //icp.InnerHtml = oem.ICPRecord;
                    hidValue.Value = oem.Setting.BGColor;
                    linksTypePic.Visible = false;
                }
            }
        }
    }
}