using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Distribution.Repository {
    interface IDistributionRepository {
        /// <summary>
        /// 插入支付账单所有信息
        /// </summary>
        void SaveNormalPayBill(Domain.Bill.Pay.NormalPayBill payBill);
        /// <summary>
        /// 重新插入支付账单所有信息
        /// </summary>
        void ReSaveNormalPayBill(Domain.Bill.Pay.NormalPayBill payBill);
        /// <summary>
        /// 插入支付账单中分润方信息
        /// </summary>
        void SaveRoyaltiesPayBill(Domain.Bill.Pay.NormalPayBill payBill);
        /// <summary>
        /// 插入改期支付账单
        /// </summary>
        void SavePostponePayBill(Domain.Bill.Pay.PostponePayBill payBill);
        /// <summary>
        /// 更新支付账单中买家支付订单金额成功信息
        /// </summary>
        void UpdatePayBillForPurchaserPaySuccess(Domain.Bill.Pay.NormalPayBill payBill);
        /// <summary>
        /// 更新支付账单信息中的价格信息
        /// </summary>
        void UpdatePayBillPriceInfo(Domain.Bill.Pay.NormalPayBill payBill);
        /// <summary>
        /// 更新支付账单信息中的真实票面价
        /// </summary>
        void UpdatePayBillFare(Domain.Bill.Pay.NormalPayBill payBill);
        /// <summary>
        /// 更新支付账单中买家支付改期费成功信息
        /// </summary>
        void UpdatePayBillForPurchaserPayPostponeFeeSuccess(Domain.Bill.Pay.PostponePayBill payBill);
        /// <summary>
        /// 更新支付账单中分润方分润成功信息
        /// </summary>
        void UpdatePayBillForRoyaltiesTradeSuccess(Domain.Bill.Pay.NormalPayBill payBill);
        /// <summary>
        /// 插入退款账单
        /// </summary>
        void SaveRefundBill(Domain.Bill.Refund.NormalRefundBill refundBill);
        /// <summary>
        /// 插入退款账单
        /// </summary>
        void SaveRefundBill(Domain.Bill.Refund.PostponeRefundBill refundBill);
        /// <summary>
        /// 更新退款账单中的退款成功信息
        /// </summary>
        //void UpdateRefundBillForRefundSuccess(decimal refundApplyformId, IEnumerable<Domain.Bill.Refund.Normal.NormalRefundRoleBill> refundSuccessRoles);
        void UpdateRefundBillForRefundSuccess(Domain.Bill.Refund.NormalRefundBill refundBill);
        /// <summary>
        /// 更新退款账单中的退款成功信息
        /// </summary>
        void UpdateRefundBillForRefundSuccess(Domain.Bill.Refund.PostponeRefundBill refundBill);
        /// <summary>
        /// 删除退款账单信息
        /// </summary>
        void DeleteRefundBill(Domain.Bill.Refund.NormalRefundBill refundBill);
        /// <summary>
        /// 查询支付账单
        /// </summary>
        /// <param name="orderId">订单号</param>
        Domain.Bill.Pay.NormalPayBill QueryNormalPayBill(decimal orderId);
        /// <summary>
        /// 查询所有改期支付账单
        /// </summary>
        /// <param name="orderId">订单号</param>
        IEnumerable<Domain.Bill.Pay.PostponePayBill> QueryPostponePayBills(decimal orderId);
        /// <summary>
        /// 查询改期支付账单
        /// </summary>
        Domain.Bill.Pay.PostponePayBill QueryPostponePayBill(decimal postponeApplyformId);
        /// <summary>
        /// 查询角色支付账单明细
        /// </summary>
        IEnumerable<Domain.Bill.Pay.Normal.NormalPayDetailBill> QueryNormalPayDetailBills(Guid normalPayRoleBillId);
        /// <summary>
        /// 查询角色支付账单明细
        /// </summary>
        IEnumerable<Domain.Bill.Pay.Postpone.PostponePayDetailBill> QueryPostponePayDetailBills(Guid postponePayRoleBillId);
        /// <summary>
        /// 查询退款账单
        /// </summary>
        Domain.Bill.Refund.NormalRefundBill QueryNormalRefundBill(decimal normalRefundApplyformId);
        /// <summary>
        /// 查询所有退款账单
        /// </summary>
        IEnumerable<Domain.Bill.Refund.NormalRefundBill> QueryNormalRefundBills(decimal orderId);
        /// <summary>
        /// 查询退款账单
        /// </summary>
        Domain.Bill.Refund.PostponeRefundBill QueryPostponeRefundBill(decimal postponeRefundApplyformId);
        /// <summary>
        /// 查询角色退款账单明细
        /// </summary>
        IEnumerable<Domain.Bill.Refund.Normal.NormalRefundDetailBill> QueryNormalRefundDetailBills(Guid normalRefundRoleBillId);
        /// <summary>
        /// 查询角色退款账单明细
        /// </summary>
        IEnumerable<Domain.Bill.Refund.Postpone.PostponeRefundDetailBill> QueryPostponeRefundDetailBills(Guid postponeRefundRoleBillId);
        /// <summary>
        /// 查询支付交易信息
        /// </summary>
        Domain.Tradement.Payment QueryPaymentByRefundTradeNo(string refundTradeNo);
        /// <summary>
        /// 查询角色原支付账单
        /// </summary>
        Domain.Bill.Pay.Normal.NormalPayRoleBill QueryNormalPayRoleBillByRefundRoleBillId(Guid normalRefundRoleBillId);
        /// <summary>
        /// 查询角色原支付账单
        /// </summary>
        Domain.Bill.Pay.Postpone.PostponePayRoleBill QueryPostponePayRoleBillByRefundRoleBillId(Guid postponeRefundRoleBillId);
    }
}