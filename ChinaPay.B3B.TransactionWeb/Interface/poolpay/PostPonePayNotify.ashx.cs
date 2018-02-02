using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.Gateway;
using ChinaPay.Gateway.Tradement;

namespace ChinaPay.B3B.TransactionWeb.Interface.poolpay
{
    /// <summary>
    /// PostPonePayNotify 的摘要说明
    /// </summary>
    public class PostPonePayNotify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                PayNotifyProcessor processor = new PayNotifyProcessor(context.Request);
                if (processor.Execute())
                {
                    var result = false;
                    var paramArray = processor.ExtraParams.Split('|');
                    decimal? orderId = null;
                    var request = string.Format("申请单:{0} 支付账号:{1} 流水号:{2} 支付时间:{3} 支付接口:{4} 支付账号类型:{5} 操作员账号:{6}  通道流水号:{7} ",
                        processor.BusinessId, processor.PayAccount, processor.PoolPayTradeNo, processor.PayTime, 
                        processor.PayChannel.GetDescription(), processor.PayAccountType.GetDescription(),
                        processor.ExtraParams, processor.ChannelTradeNo);
                    var response = string.Empty;
                    try
                    {
                        orderId = ApplyformProcessService.PostponeFeePaySuccess(processor.BusinessId, processor.PayAccount,
                            processor.PoolPayTradeNo, processor.ChannelTradeNo, processor.PayTime,
                            processor.PayChannel, processor.PayAccountType, paramArray[2]);
                        result = true;
                        response = "处理成功";
                    }
                    catch (Exception ex)
                    {
                        LogService.SaveExceptionLog(ex, "申请单支付通知 " + request);
                        response = "处理失败 " + ex.Message;
                    }
                    if (orderId.HasValue)
                    {
                        var tradementLog = new TradementLog
                        {
                            OrderId = orderId.Value,
                            ApplyformId = processor.BusinessId,
                            Request = request,
                            Response = response,
                            Time = DateTime.Now,
                            Remark = "支付成功通知"
                        };
                        LogService.SaveTradementLog(tradementLog);
                    }
                    context.Response.Write(string.Format(NotifyProcessor.ResponseFormat, "T", string.Empty));
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(string.Format(NotifyProcessor.ResponseFormat, "F", ex.Message));
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