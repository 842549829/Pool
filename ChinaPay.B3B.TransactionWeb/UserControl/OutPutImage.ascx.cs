using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Order;
using System.Text;

namespace ChinaPay.B3B.TransactionWeb.UserControl
{
    public partial class OutPutImage : System.Web.UI.UserControl
    {
        private static string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && ApplyAttachment != null)
            {
                divOutPutImage.InnerHtml = Binddata();
            }
        }
        public string Binddata()
        {
            StringBuilder builder = new StringBuilder();
            int counter = 0;
            foreach (var item in ApplyAttachment.OrderBy(o => o.Time))
            {
                counter++;
                builder.AppendFormat("<span id='span{0}'><a style='display:none;' id='a{0}' data=\"{{type:'pop',id:'divLayerImage'}}\"></a><lable> <img class='buttonImg' dataType='{0}' FilePath='{1}' src='/OrganizationHandlers/OutPutImage.ashx/OutPutApplyAttachment?ApplyAttachmentId={0}' /></lable>",
                 item.Id,
                 FileWeb + item.FilePath
                 );
                if (IsPlatform)
                {
                    builder.Append("</br>");
                    builder.AppendFormat("<input type='button' value='删除' id='{0}' class='btn class2'/>", item.Id);
                }
                builder.Append("</span>");
            }
            if (IsPlatform && counter == ApplyAttachment.Count && counter < 5)
            {
                builder.AppendFormat("<span><input type='button' value='新增' id='btnAddApplyAttachment' AttachmentId={0}  class='btn class1'/></span>",ApplyformId);
            }
            return builder.ToString();
        }
        public decimal ApplyformId { get; set; }
        public bool IsPlatform
        {
            get;
            set;
        }
        public List<ApplyAttachmentView> ApplyAttachment
        {
            get;
            set;
        }
    }
}