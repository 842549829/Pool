using System;
using System.Linq;
using System.Web.UI;
using ChinaPay.B3B.Service.SystemSetting;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class OnLineService : UnAuthBasePage
    {
        private bool isOem() {
            return (BasePage.IsOEM && BasePage.OEM.EffectTime >= DateTime.Today) && 
                (BasePage.OEM.Contract == null || BasePage.OEM.Contract != null && !BasePage.OEM.Contract.UseB3BServicePhone);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("from.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                Guid companyId = isOem()? BasePage.OEM.CompanyId : OnLineCustomerService.QueryPlatForm();
                PublishRoles publishRoles = isOem() ? PublishRoles.OEM : PublishRoles.平台;
                OnLineCustomer onLineCustomer = OnLineCustomerService.Query(companyId, publishRoles);
                if (onLineCustomer != null)
                {
                    this.lblTitle.Text = onLineCustomer.Title;
                    this.lblContent.Text = onLineCustomer.Content;
                    BindDividGroup(onLineCustomer);
                }
            }
        }

        private void BindDividGroup(OnLineCustomer dividGroup)
        {
            var dividGroupHTML = new StringBuilder();
            if (dividGroup.DivideGroupManage != null)
            {
                foreach (var item in dividGroup.DivideGroupManage)
                {
                    dividGroupHTML.Append("<div class='clearfix' style='background-image: url(../Images/sanjiao.png); background-repeat: no-repeat; padding: 20px 0px 0px 20px;'>");
                    dividGroupHTML.AppendFormat("<h2>{0}</h2>", item.Name + "(" + item.Description + ")");
                    var members = OnLineCustomerService.QueryMembers(item.Id);
                    if (members != null)
                    {
                        dividGroupHTML.Append("<ul class='box'>");
                        foreach (var member in members)
                        {
                            dividGroupHTML.AppendFormat("<li>{0}</li>", member.Remark);
                            dividGroupHTML.Append("<li>");
                            if (member.QQ.Any())
                            {
                                foreach (var qq in member.QQ)
                                {
                                    dividGroupHTML.AppendFormat("<a href='http://wpa.qq.com/msgrd?V=1&uin={0}&Site=&Menu=yes' target='_blank'><img src='http://wpa.qq.com/pa?p=2:{0}:41' alt='点击这里给我发消息' /></a>", qq);
                                }
                            }
                            dividGroupHTML.Append("</li>");
                        }
                        dividGroupHTML.Append("</ul>");
                    }
                    dividGroupHTML.Append("</div>");
                }
            }
            this.divDivideGroup.InnerHtml = dividGroupHTML.ToString();
        }
    }
}