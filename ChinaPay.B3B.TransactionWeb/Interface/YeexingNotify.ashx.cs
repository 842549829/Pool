using System;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.ExternalPlatform;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.Interface
{
    /// <summary>
    /// 易行通知接受页面
    /// </summary>
    public class YeexingNotify : IHttpHandler
    {
        #region IHttpHandler Members
        static object notifyLocker = new object();


        public void ProcessRequest(HttpContext context)
        {
            try
            {
                ExternalPlatformNotifyView notifyInfo = NotifyService.YeexingNotify(context);
                lock (notifyLocker)
                {
                    if (notifyInfo is PaySuccessNotifyView)
                    {
                        var paymentResult = notifyInfo as PaySuccessNotifyView;
                        OrderProcessService.ExternalPaySucess(paymentResult);
                    }
                    else if (notifyInfo is ETDZFailedNotifyView)
                    {
                        var etdzResult = notifyInfo as ETDZFailedNotifyView;
                        OrderProcessService.ExternalOrderDenyETDZ(etdzResult.Reason, etdzResult.Id);
                    }
                    else if (notifyInfo is ETDZSuccessNotifyView)
                    {
                        var etdzResult = notifyInfo as ETDZSuccessNotifyView;
                        if (etdzResult.Valid && etdzResult.Ticket.TicketNos.Any())
                        {
                            OrderProcessService.ExternalPlatformETDZ(etdzResult.Ticket.NewPNR, etdzResult.Id, etdzResult.Ticket.TicketNos,
                                etdzResult.Ticket.SettleCode, BasePage.OwnerOEMId);
                        }
                    }
                    else if (notifyInfo is CancelOrderNotifyView)
                    {
                    }
                }
                context.Response.Write(notifyInfo.Response);
            }
            catch (CustomException)
            {
                context.Response.Write("FAILED");
            } 
            catch (Exception ex)
            {
                context.Response.Write("FAILED");
                LogService.SaveExceptionLog(ex);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        #endregion
    }
}