using System;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Order {
    public static class StatusService {
        /// <summary>
        /// 获取订单状态列表
        /// </summary>
        /// <param name="role">订单角色</param>
        public static Dictionary<OrderStatus, string> GetRoleOrderStatus(OrderRole role) {
            return OrderStatusAdapter.Instance.GetRoleStatus(role);
        }
        /// <summary>
        /// 获取改期状态列表
        /// </summary>
        /// <param name="role">订单角色</param>
        public static Dictionary<PostponeApplyformStatus, string> GetRolePostponeApplyformStatus(OrderRole role) {
            return PostponeStatusAdapter.Instance.GetRoleStatus(role);
        }
        /// <summary>
        /// 获取退/废票状态列表
        /// </summary>
        /// <param name="role">订单角色</param>
        public static Dictionary<RefundApplyformStatus, string> GetRoleRefundApplyformStatus(OrderRole role) {
            return RefundStatusAdapter.Instance.GetRoleStatus(role);
        }
        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="status">系统状态</param>
        /// <param name="role">订单角色</param>
        public static string GetOrderStatus(OrderStatus status, OrderRole role) {
            return OrderStatusAdapter.Instance.GetStatus(status, role);
        }
        /// <summary>
        /// 获取改期状态
        /// </summary>
        /// <param name="status">系统状态</param>
        /// <param name="role">订单角色</param>
        public static string GetPostponeApplyformStatus(PostponeApplyformStatus status, OrderRole role) {
            return PostponeStatusAdapter.Instance.GetStatus(status, role);
        }
        /// <summary>
        /// 获取退/废票状态
        /// </summary>
        /// <param name="status">系统状态</param>
        /// <param name="role">订单角色</param>
        public static string GetRefundApplyformStatus(RefundApplyformStatus status, OrderRole role) {
            return RefundStatusAdapter.Instance.GetStatus(status, role);
        }
        /// <summary>
        /// 获取员工在指定的订单状态下
        /// </summary>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="id">员工Id</param>
        /// <returns></returns>
        public static int GetOrderTodoCount(OrderStatus orderStatus, Guid id, Guid providerId) {
            return OrderStatusAdapter.GetOrderTodoCount(orderStatus, id,providerId);
        }

        /// <summary>
        /// 根据角色获取差额退款单的状态
        /// </summary>
        /// <param name="status"> </param>
        /// <param name="role"> </param>
        /// <returns></returns>
        public static string GetBalanceRefundStatus(BalanceRefundProcessStatus status, OrderRole role)
        {
            return BalanceRefundStatusAdapter.Instance.GetStatus(status, role);
        }

        /// <summary>
        /// 根据角色获取差额退款单的状态
        /// </summary>
        /// <param name="role"> </param>
        /// <returns></returns>
        public static Dictionary<BalanceRefundProcessStatus,string> GetBalanceRefundStatus(OrderRole role)
        {
            return BalanceRefundStatusAdapter.Instance.GetRoleStatus(role);
        }
    }
}