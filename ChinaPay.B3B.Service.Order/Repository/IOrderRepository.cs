using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.Core;
using System.Data;
using ChinaPay.B3B.DataTransferObject.Order.External;

namespace ChinaPay.B3B.Service.Order.Repository {
    interface IOrderRepository {
        void InsertOrder(Order.Domain.Order order);
        void UpdateStatus(decimal orderId, OrderStatus status, string remark);
        void UpdateOrderForSupplyResource(Order.Domain.Order order);
        void UpdateOrderForProviderChanged(Order.Domain.Order order);
        void UpdateOrderForPaySuccess(Order.Domain.Order order);
        void UpdateOrderForETDZ(Order.Domain.Order order);
        void UpdateOrderForReviseReleasedFare(Order.Domain.Order order);
        void UpdateOrderForReviseFare(Order.Domain.Order order);
        void UpdateOrderForApplyform(Order.Domain.Order order);
        void UpdateCredentitals(Guid passengerId, string credentials);
        void UpdateTicketNo(Guid passengerId, int serial, string ticketNo,string settleCode);
        Guid QueryReservationFlightId(Guid flightId);

        Order.Domain.Order QueryOrder(decimal orderId);
        Order.Domain.Order QueryOrder(string pnrCode, DateTime producedTime);
        IEnumerable<OrderListView> QueryOrders(OrderQueryCondition condition, Pagination pagination, bool extendDateTime);
        IEnumerable<OrderListView> QueryWaitOrders(OrderQueryCondition condition, Pagination pagination, bool extendDateTime);
        RefundAndReschedulingProvision QueryProviderRefundAndReschedulingProvision(decimal orderId);
        RefundAndReschedulingProvision QuerySupplierRefundAndReschedulingProvision(decimal orderId);

        void SaveCredentialsUpdateInfo(Order.Domain.Order order, Order.Domain.Passenger passenger, string originalCredentials, string newCredentials, bool success, OperatorRole role, string commitAccount);
        CredentialsUpdateInfo QueryCredentialsUpdateInfo(Guid credentialsUpdateInfoId);
        IEnumerable<CredentialsUpdateInfoListView> QueryCredentialsUpdateInfos(CredentialsUpdateInfoQueryCondition condition, Pagination pagination);
        IEnumerable<CredentialsUpdateRecordView> QueryCredentialsUpdateRecords(decimal orderId, Guid? passengerId);

        /// <summary>
        /// 获取员工在指定订单状态下的订单待办数量
        /// </summary>
        /// <param name="orderstatus"></param>
        /// <param name="employeeId"></param>
        /// <param name="providerId"> </param>
        /// <returns></returns>
        int GetOrderTodoCount(OrderStatus orderstatus, Guid employeeId, Guid providerId);

        /// <summary>
        /// 通过票号获取订单信息
        /// </summary>
        /// <param name="ticketNo"></param>
        /// <returns></returns>
        Domain.Order QueryOrderByTicketNo(string ticketNo);

        IEnumerable<ExternalOrderListView> QueryExternalOrders(ExternalOrderCondition condition,Pagination pagiantion);


        /// <summary>
        /// 保存外平台的生成订单的政策副本
        /// </summary>
        /// <param name="externalPolicy">外平台政策信息</param>
        /// <returns></returns>
        bool SaveExternalPolicyCopy(ExternalPolicyView externalPolicy,decimal orderId);
        /// <summary>
        /// 记录订单已经发送了乘机人出票短信
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool SetPassengerMsgSended(decimal orderId);
        /// <summary>
        /// 记录采购方已发送催单信息
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="remindContent">提醒内容</param>
        bool Reminded(decimal orderId, string remindContent);
        bool UpdateRemindStatus(decimal orderId);

        ExternalOrder QueryExternalOrder(decimal orderId);
        ExternalOrder LoadExternalInfo(ExternalOrder order);
        void PayExternalOrderSuccess(ExternalOrder externalOrderInfo);

        ExternalOrder LoadExternalInfo(string externalOrderId, ExternalOrder order);
        ExternalOrder QueryExternalOrder(string externalOrderId);

        /// <summary>
        /// 验证在指定时间内是否有存在相同编码的订单
        /// </summary>
        /// <param name="pnr"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="companyId"> </param>
        /// <returns></returns>
        decimal ExistsPNR(PNRPair pnr, DateTime timeFrom, DateTime timeTo, Guid companyId);

        ExternalPolicyView QueryExternalPolicy(decimal orderId);
    }
}