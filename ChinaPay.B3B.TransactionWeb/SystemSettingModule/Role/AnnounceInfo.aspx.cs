using System;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.Service.Announce;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class AnnounceInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string announceId = Request.QueryString["Id"];
                if(!string.IsNullOrWhiteSpace(announceId))
                {
                   AnnounceView view = AnnounceService.Query(Guid.Parse(announceId));
                   this.lblTitle.Text = view.Title;
                   this.lblPublishTime.Text = view.PublishTime.Date.ToShortDateString();
                   this.lblContent.InnerHtml = view.Content;
                }
            }
        }
    }
}