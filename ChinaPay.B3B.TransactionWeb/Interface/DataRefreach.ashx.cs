using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject;
using ChinaPay.B3B.TransactionWeb.FlightHandlers;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.Interface
{
    /// <summary>
    /// DataRefreach 的摘要说明
    /// </summary>
    public class DataRefreach : IHttpHandler
    {
        static object notifyLocker = new object();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["Key"] == Utility.MD5EncryptorService.MD5FilterZero(System.Configuration.ConfigurationManager.AppSettings["SignKey"], "utf-8")
                && context.Request.QueryString["Action"] == "Flush")
            {
                lock (notifyLocker)
                {
                    switch (context.Request.QueryString["Target"])
                    {
                        case "BUNK":
                            RefreshService.FlushBunk();
                            break;
                        case "PRICE":
                            RefreshService.FlushBasePrice();
                            break;
                        case "BAF":
                            RefreshService.FlushBAF();
                            break;
                        case "DICTIONARY":
                            RefreshService.FlushServicePhone();
                            break;
                        case "OEMSETTING":
                            RefreshService.FlushOEM(Guid.Parse(context.Request.QueryString["OEMID"]));
                            break;
                        case "OEMSTYLE":
                            RefreshService.FlushStyles(Guid.Parse(context.Request.QueryString["StyleId"]));
                            break;
                        case "OEMADDED":
                            RefreshService.FlushOEMErrorCache();
                            break;
                    }
                }
            }
        
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}