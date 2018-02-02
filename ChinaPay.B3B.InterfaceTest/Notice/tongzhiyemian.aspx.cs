using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.InterfaceTest.Notice
{
    public partial class tongzhiyemian : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var request = Request.QueryString;
            if (request["type"] == "1")
            {
                Service.LogService.SaveTextLog("确认成功通知:" + request);
            }
            else if (request["type"] == "2")
            {
                Service.LogService.SaveTextLog("确认失败通知:" + request);
            }
            else if (request["type"] == "3")
            {
                Service.LogService.SaveTextLog("订单支付成功通知:" + request);
            }
            else if (request["type"] == "4")
            {
                Service.LogService.SaveTextLog("出票成功通知:" + request);
            }
            else if (request["type"] == "5")
            {
                Service.LogService.SaveTextLog("取消出票通知:" + request);
            }
            else if (request["type"] == "6")
            {
                Service.LogService.SaveTextLog("退废票退款成功通知:" + request);
            }
            else if (request["type"] == "7")
            {
                Service.LogService.SaveTextLog("退废票处理成功通知:" + request);
            }
            else if (request["type"] == "8")
            {
                Service.LogService.SaveTextLog("拒绝退废票通知:" + request);
            }
            else if (request["type"] == "9")
            {
                Service.LogService.SaveTextLog("同意改期通知:" + request);
            }
            else if (request["type"] == "10")
            {
                Service.LogService.SaveTextLog("拒绝改期通知:" + request);
            }
            else if (request["type"] == "11")
            {
                Service.LogService.SaveTextLog("改期支付成功通知:" + request);
            }
            else if (request["type"] == "12")
            {
                Service.LogService.SaveTextLog("改期成功通知:" + request);
            }
            else if (request["type"] == "13")
            {
                Service.LogService.SaveTextLog("取消出票退款成功通知:" + request);
            }
            else if (request["type"] == "14")
            {
                Service.LogService.SaveTextLog("拒绝改期退款成功通知:" + request);
            }
            else
            {
                Service.LogService.SaveTextLog("通知:" + request);
            }
            Response.Write("0");
            Response.End();
            return;
        }
    }
}