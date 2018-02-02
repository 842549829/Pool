using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 订单查询服务
    /// </summary>
    public static class OrderQueryService {
        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static Order.Domain.Order QueryOrder(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryOrder(orderId);
            }
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static Order.Domain.ExternalOrder QueryExternalOrder(decimal orderId)
        {
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryExternalOrder(orderId);
            }
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static Order.Domain.ExternalOrder QueryExternalOrder(string externalOrderId)
        {
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryExternalOrder(externalOrderId);
            }
        }

        public static Order.Domain.ExternalOrder QueryOrderExternalInfo(decimal orderId) {
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                var order = new ExternalOrder(orderId);
                return repository.LoadExternalInfo(order);
            }

        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="pnrCode">编码</param>
        /// <param name="producedTime">编码创建时间</param>
        internal static Order.Domain.Order QueryOrder(string pnrCode, DateTime producedTime) {
            using(var command = Factory.CreateCommand()) {
                var orderRepository = Factory.CreateOrderRepository(command);
                return orderRepository.QueryOrder(pnrCode, producedTime);
            }
        }

        /// <summary>
        /// 获取订单列表信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        /// <param name="extendDateTime">是否进行日期扩展(结束日期扩展时间到23:59) </param>
        public static IEnumerable<OrderListView> QueryOrders(OrderQueryCondition condition, Pagination pagination, bool extendDateTime = true)
        {
            if(condition == null)
                throw new ArgumentNullException("condition");
            if(pagination == null)
                throw new ArgumentNullException("pagination");
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryOrders(condition, pagination, extendDateTime);
            }
        }

        /// <summary>
        /// 获取订单列表信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        /// <param name="extendDateTime">是否进行日期扩展(结束日期扩展时间到23:59) </param>
        public static IEnumerable<OrderListView> QueryWaitOrders(OrderQueryCondition condition, Pagination pagination, bool extendDateTime = true)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (pagination == null)
                throw new ArgumentNullException("pagination");
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryWaitOrders(condition, pagination, extendDateTime);
            }
        }
        /// <summary>
        /// 平台获取订单列表信息(目的为了处理OEM是否能够联系采购)
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        /// <param name="extendDateTime">是否进行日期扩展(结束日期扩展时间到23:59)</param>
        /// <returns></returns>
        public static IEnumerable<OrderListView> QueryOperateOrders(OrderQueryCondition condition, Pagination pagination, bool extendDateTime = true)
        {
            var orders = QueryOrders(condition, pagination, extendDateTime);
            IEnumerable<Guid> oemIds = orders.Where(o => o.OEMID.HasValue).Select(o => o.OEMID.Value).Distinct();
            if (oemIds.Any())
            {
                IEnumerable<KeyValuePair<Guid, bool>> oems = OEMService.QueryOEMContractSettings(oemIds);
                foreach (var item in orders)
                {
                    item.AllowPlatformContractPurchaser = true;
                    if (!item.OEMID.HasValue) continue;
                    var oem = oems.FirstOrDefault(o => o.Key == item.OEMID.Value);
                    item.AllowPlatformContractPurchaser = oem.Key != Guid.Empty && oem.Value;
                }
            }
            return orders;
        }
        /// <summary>
        /// 查询证件号修改日志
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static IEnumerable<CredentialsUpdateRecordView> QueryCredentialsUpdateRecords(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryCredentialsUpdateRecords(orderId, null);
            }
        }
        /// <summary>
        /// 运营方查询证件号修改记录列表
        /// </summary>
        public static IEnumerable<CredentialsUpdateInfoListView> QueryCredentialsUpdateInfos(CredentialsUpdateInfoQueryCondition condition, Pagination pagination) {
            if(condition == null)
                throw new ArgumentNullException("condition");
            if(pagination == null)
                throw new ArgumentNullException("pagination");
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryCredentialsUpdateInfos(condition, pagination);
            }
        }
        /// <summary>
        /// 查询证件号修改日志
        /// </summary>
        public static IEnumerable<CredentialsUpdateRecordView> QueryCredentialsUpdateRecords(decimal orderId, Guid passengerId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryCredentialsUpdateRecords(orderId, passengerId);
            }
        }
        internal static CredentialsUpdateInfo QueryCredentialsUdpateInfo(Guid credentialsUpdateInfoId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryCredentialsUpdateInfo(credentialsUpdateInfoId);
            }
        }

        /// <summary>
        /// 查询分润失败记录
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        public static IEnumerable<RoyaltyFailedRecord> QueryRoyaltyFailedRecords(RoyaltyFailedRecordQueryCondition condition, Pagination pagination) {
            var repository = Factory.CreateRoyaltyRepository();
            return repository.Query(condition, pagination);
        }
        /// <summary>
        /// 查询分润失败记录
        /// </summary>
        public static IEnumerable<decimal> QueryRoyaltyFailedRecords() {
            var repository = Factory.CreateRoyaltyRepository();
            return repository.Query();
        }

        /// <summary>
        /// 查询退款失败记录
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        public static IEnumerable<RefundFailedRecord> QueryRefundFailedRecords(RefundFailedRecordQueryCondition condition, Pagination pagination) {
            var repository = Factory.CreateRefundRepository();
            return repository.Query(condition, pagination);
        }
        /// <summary>
        /// 查询退款失败记录
        /// </summary>
        public static IEnumerable<RefundFailedRecord> QueryRefundFailedRecords() {
            var repository = Factory.CreateRefundRepository();
            return repository.Query();
        }
        internal static DataTransferObject.Policy.RefundAndReschedulingProvision QueryProviderRefundAndReschedulingProvision(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryProviderRefundAndReschedulingProvision(orderId);
            }
        }
        internal static DataTransferObject.Policy.RefundAndReschedulingProvision QuerySupplierRefundAndReschedulingProvision(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QuerySupplierRefundAndReschedulingProvision(orderId);
            }
        }

        public static Order.Domain.Order QueryOrderByTicketNo(string ticketNo)
        {
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryOrderByTicketNo(ticketNo);
            }

        }
        /// <summary>
        /// 查询外部订单列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        public static IEnumerable<ExternalOrderListView> QueryExternalOrders(ExternalOrderCondition condition,Pagination pagination)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                return repository.QueryExternalOrders(condition, pagination);
            }
        }

        public static ExternalOrder QueryOrderExternalInfo(string externalOrderId) {
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                var order = new ExternalOrder(0);
                return repository.LoadExternalInfo(externalOrderId, order);
            }

        }

        public static ExternalOrder QueryExternalOrderTicket(decimal orderId, Guid oemId)
        {
            var extOrder = QueryExternalOrder(orderId);
            var queryResult = ExternalPlatform.OrderService.QueryTicketNo(extOrder.Platform, orderId, extOrder.ExternalOrderId);
            var pnrInfo = extOrder.PNRInfos.FirstOrDefault();
            if (pnrInfo==null)
            {
                throw new CustomException("编码不存在！");
            }
            if (queryResult.Success&&queryResult.Result.TicketNos.Any())
            {
                var ticketNoView = new TicketNoView()
            {
                ETDZPNR = queryResult.Result.NewPNR,
                Mode = ETDZMode.Manual,
                Items = queryResult.Result.TicketNos,
                NewSettleCode = queryResult.Result.SettleCode
            };
                var setting = ExternalPlatform.Processor.PlatformBase.GetPlatform(extOrder.Platform);
                OrderProcessService.ETDZ(ticketNoView, setting.Setting.ProviderAccount, extOrder.Platform.GetDescription(), extOrder,oemId);
            }
            return extOrder;
        }

        /// <summary>
        /// 验证在指定时间内是否有存在相同编码的订单
        /// </summary>
        /// <param name="pnr"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="companyId"> </param>
        /// <returns></returns>
        public static decimal ExistsPNR(PNRPair pnr, DateTime timeFrom, DateTime timeTo, Guid companyId) {
            using (var command = Factory.CreateCommand())
            {
                var repository = Factory.CreateOrderRepository(command);
                return repository.ExistsPNR(pnr, timeFrom, timeTo, companyId);
            }
        }
    }
}