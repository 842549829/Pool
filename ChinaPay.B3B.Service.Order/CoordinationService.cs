using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Repository;
using System;
using BusinessType = ChinaPay.B3B.Service.Order.Domain.BusinessType;

namespace ChinaPay.B3B.Service.Order {
    /// <summary>
    /// 协调信息服务类
    /// </summary>
    public static class CoordinationService {
        /// <summary>
        /// 保存订单协调信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="coordination">协调信息</param>
        public static void SaveOrderCoordination(decimal orderId,Coordination coordination)
        {
            var repository = Factory.CreateCoordinationRepository();
            repository.Save(orderId,coordination);
        }
        /// <summary>
        /// 保存紧急订单信息
        /// </summary>
        /// <param name="emergentOrder"></param>
        public static void SvaeEmergentOrder(EmergentOrder emergentOrder,Guid companyId){
            ICoordinationRepository repository = Factory.CreateCoordinationRepository();
            repository.SvaeEmergentOrder(emergentOrder);
            if (emergentOrder.OrderIdTypeValue == OrderIdType.Order)
            {
                try
                {
                    var logOrder = new Log.Domain.OrderLog();
                    logOrder.OrderId = emergentOrder.Id;
                    logOrder.Account = emergentOrder.Account;
                    logOrder.Keyword = "设置紧急";
                    logOrder.Content = string.Format("订单：{0} 已经被操作员：{1} 设置为紧急", emergentOrder.Id, emergentOrder.Account);
                    logOrder.Company = companyId;
                    logOrder.Role = Common.Enums.OperatorRole.Platform;
                    logOrder.Time = DateTime.Now;
                    logOrder.VisibleRole = OrderRole.Platform;
                    LogService.SaveOrderLog(logOrder);
                }
                catch { }
            }
        }
        /// <summary>
        /// 查询紧急订单信息
        /// </summary>
        /// <param name="id">单子号</param>
        /// <param name="orderIdType">单子类型</param>
        /// <returns></returns>
        public static EmergentOrder QueryEmergentOrder(decimal id,OrderIdType orderIdType = OrderIdType.Order) 
        {
            ICoordinationRepository repository = Factory.CreateCoordinationRepository();
            return repository.QueryEmergentOrder(id,orderIdType);
        }
        /// <summary>
        /// 保存申请单协调信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="applyformId">申请单号</param>
        /// <param name="coordination">协调信息</param>
        public static void SaveApplyformCoordination(decimal orderId, decimal applyformId, Coordination coordination) {
            var repository = Factory.CreateCoordinationRepository();
            repository.Save(orderId, applyformId, coordination);
        }
        /// <summary>
        /// 查询订单协调信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static IEnumerable<Domain.Coordination> QueryOrderCoordinations(decimal orderId) {
            var repository = Factory.CreateCoordinationRepository();
            var logs = LogService.QueryOrderLog(orderId);
            var result = repository.QueryByOrderId(orderId).ToList();
            result.AddRange(logs.Where(l => l.Keyword == "出票" && (l.VisibleRole & OrderRole.Provider) > 0).Select(ConstructCoordination));
            result.AddRange(logs.Where(l => l.Keyword == "采购催单").Select(ConstructCoordinationCuiDan));
            var accounts = result.Select(c => c.Account).Distinct();
            var names = repository.QueryAccountNames(accounts);
            result.ForEach(c => c.Account = string.Format("{0}({1})",c.Account,names[c.Account]));
            return result.OrderBy(c=>c.Time);
        }
        /// <summary>
        /// 查询申请单协调信息
        /// </summary>
        /// <param name="applyformId">申请单号</param>
        public static IEnumerable<Domain.Coordination> QueryApplyformCoordinations(decimal applyformId) {
            var repository = Factory.CreateCoordinationRepository();
            return repository.QueryByApplyformId(applyformId);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id">单子号</param>
        /// <param name="status">订单状态</param>
        /// <returns>紧急信息</returns>
        public static EmergentOrder GetEmergentOrder(decimal id, DataTransferObject.Order.OrderStatus status)
        {
            var repository = Factory.CreateCoordinationRepository();
            return repository.QueryEmergentOrder(id,status);
        }

        static Regex logTypereg = new Regex("拒绝出票。原因:(?<reason>.*)");
        private static Coordination ConstructCoordination(Log.Domain.OrderLog log) { 
            var matchResult = logTypereg.Match(log.Content);
            var result = new Coordination(log.Account, matchResult.Success ? matchResult.Groups["reason"].Value : "成功出票",
                matchResult.Success ? "拒绝出票" : "出票成功", BusinessType.出票, ContactMode.Telphone, log.Time, OrderRole.Provider);
            return result;
        }
        private static Coordination ConstructCoordinationCuiDan(Log.Domain.OrderLog log) {
            return new Coordination(log.Account, log.Content.Replace("催单内容或备注:", ""),
                "采购催单", BusinessType.出票, ContactMode.Telphone, log.Time, OrderRole.Provider);
        }
    }
}