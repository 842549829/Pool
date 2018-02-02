using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.SMS.Service;

namespace ChinaPay.B3B.TransactionWeb.SMSHandlers
{
    /// <summary>
    /// SMS 的摘要说明
    /// </summary>
    public class SMS : BaseHandler
    {
        public string GetDefaultTemlete(string paramerKey)
        {
            return SMSSendService.GetDefaultTemlete(paramerKey).Replace("DomainName", BasePage.DomainName).Replace("ServicePhone", BasePage.CurrenContract.ServicePhone);
        }
    }
}