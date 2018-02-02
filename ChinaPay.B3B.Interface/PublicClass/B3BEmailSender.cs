using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.MailService;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.MailService.Realize;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using System.Text;

namespace ChinaPay.B3B.Interface.PublicClass
{

    public static class B3BEmailSender
    {
        public static void SendFareError(FareErrorLog fare, decimal price)
        {
            try
            {
                IMailServiceProvider mail = new MailServiceProvider();
                string receptionMailAddress = SystemParamService.QueryString(SystemParamType.SystemReceptionMailAddress);
                string serviceMailAdderess = SystemParamService.QueryString(SystemParamType.SystemServiceMailAddress);
                string serviceMailPassword = SystemParamService.QueryString(SystemParamType.SystemServiceMailPassword);
                string title = "记录价格变动信息";
                string content = B3BEmailSender.GetFareCOnetnt(fare, price);
                MailMessages message = new MailMessages("B3b系统邮件", receptionMailAddress, title, content, serviceMailAdderess, serviceMailPassword, "QQ");
                mail.SendMessage(message);
            }
            catch (Exception ex)
            {
                Service.LogService.SaveExceptionLog(ex, "发送邮件异常");
            }
        }

        /// <summary>
        /// 支付代扣超时的时候发送有条件通知相关人员
        /// </summary>
        public static void SendPayTimeOutMessage()
        {

        }


        private static string GetFareCOnetnt(FareErrorLog fare, decimal price)
        {
            StringBuilder builder = new StringBuilder(100);
            builder.AppendFormat("出发地：{0}，到达地：{1}，承运人：{2}，航班日期：{3}，舱位：{4}，票面价：{5}，PAT价格：{6}，发送日期:{7}",
                fare.Departure,
                fare.Arrival,
                fare.Carrier,
                fare.FlightDate.ToString("yyyy-MM-dd"),
                fare.Bunk,
                price.ToString("C"),
                fare.Fare.ToString("C"),
                DateTime.Now.ToString("yyyy-MM-dd"));
            return builder.ToString();
        }
    }
}