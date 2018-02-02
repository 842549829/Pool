using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Order.Repository;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 账单查询服务
    /// </summary>
    public static class DistributionQueryService {
        /// <summary>
        /// 查询账单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static Distribution.Domain.OrderBill QueryOrderBill(decimal orderId) {
            return new Distribution.Domain.OrderBill(orderId);
        }
        /// <summary>
        /// 查询支付账单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static Distribution.Domain.Bill.Pay.NormalPayBill QueryNormalPayBill(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryNormalPayBill(orderId);
            }
        }
        /// <summary>
        /// 查询改期支付账单
        /// </summary>
        /// <param name="postponeApplyformId">改期申请单号</param>
        public static Distribution.Domain.Bill.Pay.PostponePayBill QueryPostponePayBill(decimal postponeApplyformId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryPostponePayBill(postponeApplyformId);
            }
        }
        /// <summary>
        /// 查询退款账单
        /// </summary>
        /// <param name="normalRefundApplyformId">退款申请单号</param>
        public static Distribution.Domain.Bill.Refund.NormalRefundBill QueryNormalRefundBill(decimal normalRefundApplyformId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryNormalRefundBill(normalRefundApplyformId);
            }
        }
        /// <summary>
        /// 查询改期退款账单
        /// </summary>
        /// <param name="postponeApplyformId">改期申请单号</param>
        public static Distribution.Domain.Bill.Refund.PostponeRefundBill QueryPostponeRefundBill(decimal postponeApplyformId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryPostponeRefundBill(postponeApplyformId);
            }
        }
        /// <summary>
        /// 查询角色支付账单明细
        /// </summary>
        internal static IEnumerable<Distribution.Domain.Bill.Pay.Normal.NormalPayDetailBill> QueryNormalPayDetailBills(Guid normalPayRoleBillId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryNormalPayDetailBills(normalPayRoleBillId);
            }
        }
        /// <summary>
        /// 查询改期支付角色账单明细
        /// </summary>
        internal static IEnumerable<Distribution.Domain.Bill.Pay.Postpone.PostponePayDetailBill> QueryPostponePayDetailBills(Guid postponePayRoleBillId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryPostponePayDetailBills(postponePayRoleBillId);
            }
        }
        /// <summary>
        /// 查询角色退款账单明细
        /// </summary>
        internal static IEnumerable<Distribution.Domain.Bill.Refund.Normal.NormalRefundDetailBill> QueryNormalRefundDetailBills(Guid normalRefundRoleBillId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryNormalRefundDetailBills(normalRefundRoleBillId);
            }
        }
        /// <summary>
        /// 查询改期退款角色账单明细
        /// </summary>
        internal static IEnumerable<Distribution.Domain.Bill.Refund.Postpone.PostponeRefundDetailBill> QueryPostponeRefundDetailBills(Guid postponeRefundRoleBillId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryPostponeRefundDetailBills(postponeRefundRoleBillId);
            }
        }
        /// <summary>
        /// 查询所有改期支付账单
        /// </summary>
        /// <param name="orderId">订单号</param>
        internal static IEnumerable<Distribution.Domain.Bill.Pay.PostponePayBill> QueryPostponePayBills(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryPostponePayBills(orderId);
            }
        }
        /// <summary>
        /// 查询所有退款账单
        /// </summary>
        /// <param name="orderId">订单号</param>
        internal static IEnumerable<Distribution.Domain.Bill.Refund.NormalRefundBill> QueryNormalRefundBills(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryNormalRefundBills(orderId);
            }
        }
        /// <summary>
        /// 查询原支付交易信息
        /// </summary>
        /// <param name="refundTradeNo">退款流水号</param>
        internal static Distribution.Domain.Tradement.Payment QueryPayment(string refundTradeNo) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryPaymentByRefundTradeNo(refundTradeNo);
            }
        }
        /// <summary>
        /// 查询原支付角色信息
        /// </summary>
        internal static Distribution.Domain.Bill.Pay.Normal.NormalPayRoleBill QueryNormalPayRoleBill(Guid normalRefundRoleBillId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryNormalPayRoleBillByRefundRoleBillId(normalRefundRoleBillId);
            }
        }
        /// <summary>
        /// 查询原改期支付角色信息
        /// </summary>
        internal static Distribution.Domain.Bill.Pay.Postpone.PostponePayRoleBill QueryPostponePayRoleBill(Guid postponeRefundRoleBillId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateDistributionRepository(command);
                return repository.QueryPostponePayRoleBillByRefundRoleBillId(postponeRefundRoleBillId);
            }
        }
    }
}
