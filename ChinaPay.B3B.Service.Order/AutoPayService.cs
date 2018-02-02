using System;
using ChinaPay.B3B.Service.Order.Repository;
using ChinaPay.B3B.Service.Order.Domain.AutoPay;
using ChinaPay.Core.Extension;
using System.Collections.Generic;
using ChinaPay.PoolPay.Service;
using PoolPay.DataTransferObject;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Order
{
    /// <summary>
    /// 代扣
    /// </summary>
    public static class AutoPayService
    {
        /// <summary>
        /// 新增代扣记录
        /// </summary>
        public static void InsertAutoPay(AutoPay auto)
        {
            using (var command = Factory.CreateCommand())
            {
                var request = string.Format("订单号:{0},代扣账号:{1},代扣方式:{2}",
                    auto.OrderId, auto.PayAccountNo, auto.PayType.GetDescription());
                var response = string.Empty;
                var autoPayRepository = Factory.CreateAutoPayRepository(command);
                try
                {
                    autoPayRepository.Insert(auto);
                    response = "处理成功";
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex, "添加代扣记录： " + request);
                    response = "处理失败 " + ex.Message;
                }
                var tradementLog = new Log.Domain.TradementLog
                {
                    OrderId = auto.OrderId,
                    Request = request,
                    Response = response,
                    Time = DateTime.Now,
                    Remark = "添加代扣记录",
                };
                LogService.SaveTradementLog(tradementLog);
            }
        }

        /// <summary>
        /// 修改处理状态
        /// </summary>
        /// <param name="orderId"></param>
        public static void UpdateProcess(decimal orderId)
        {
            using (var command = Factory.CreateCommand())
            {
                var autoPayRepository = Factory.CreateAutoPayRepository(command);
                autoPayRepository.UpdateProcess(orderId);
            }
        }

        /// <summary>
        /// 查询当前没有处理的代扣订单
        /// </summary>
        public static List<Domain.AutoPay.AutoPay> QueryNoPorcess()
        {
            using (var command = Factory.CreateCommand())
            {
                var autoPayRepository = Factory.CreateAutoPayRepository(command);
                return autoPayRepository.QueryNoPorcess();
            }
        }
        /// <summary>
        /// 代扣
        /// </summary>
        /// <param name="tradeView"></param>
        public static global::PoolPay.DomainModel.Trade.PayTrade AutoPay(AccountTradeDTO tradeView, WithholdingAccountType type)
        {
            global::PoolPay.DomainModel.Trade.PayTrade pay = null;
            string success = "";
            if (type == WithholdingAccountType.Poolpay)
            {
                pay = AccountTradeService.AccountPay(tradeView, "");
                if (pay != null && pay.Status == global::PoolPay.DataTransferObject.PayStatus.PaySuccess) 
                    success = "SUCCESS"; 
                else
                    throw new Exception("支付失败！");
            }
            else if (type == WithholdingAccountType.Alipay)
            {
                success = AccountTradeService.InterfacePay(tradeView);
            }
            if (success != "")
            {
                if (success.ToUpper() == "SUCCESS")
                {
                    using (var command = Factory.CreateCommand())
                    {
                        var autoPayRepository = Factory.CreateAutoPayRepository(command);
                        autoPayRepository.UpdateSuccess(decimal.Parse(tradeView.OutOrderId));
                    }
                }
                else
                {
                    throw new Exception("支付失败！");
                }
                var tradementLog = new Log.Domain.TradementLog
                {
                    OrderId = decimal.Parse(tradeView.OutOrderId),
                    Request = "订单号：" + tradeView.OutOrderId,
                    Response = success,
                    Time = DateTime.Now,
                    Remark = "修改代扣处理状态",
                };
                LogService.SaveTradementLog(tradementLog);
            }
            return pay;
        }
        /// <summary>
        /// 查询代扣记录
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public static AutoPay QueryAuto(decimal orderid)
        {
            using (var command = Factory.CreateCommand())
            {
                var autoPayRepository = Factory.CreateAutoPayRepository(command);
                return autoPayRepository.Query(orderid);
            }
        }
    }
}
