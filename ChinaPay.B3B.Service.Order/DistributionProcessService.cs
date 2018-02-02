using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;
using ChinaPay.Core.Exception;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 账单服务类
    /// </summary>
    internal static class DistributionProcessService {
        /// <summary>
        /// 生成采购支付账单
        /// </summary>
        /// <param name="trade">交易信息</param>
        /// <param name="tradeDeduction">交易方信息</param>
        public static Distribution.Domain.Bill.Pay.NormalPayBill ProducePurchasePayBill(Distribution.Domain.TradeInfo trade, Distribution.Domain.TradeDeduction tradeDeduction) {
            if(trade == null) throw new ArgumentNullException("trade");
            if(trade.Passengers == null || !trade.Passengers.Any()) throw new CustomException("缺少乘机人信息");
            if(trade.Flights == null || !trade.Flights.Any()) throw new CustomException("缺少航班信息");
            if(tradeDeduction == null) throw new ArgumentNullException("tradeDeduction");
            if(tradeDeduction.Purchaser == null) throw new CustomException("缺少买家信息");
            var originalPayBill = DistributionQueryService.QueryNormalPayBill(trade.Id);
            if(originalPayBill != null) throw new RepeatedItemException("支付账单已存在");

            var payBill = new Distribution.Domain.Bill.Pay.NormalPayBill(trade.Id) {
                Remark = "预订出票"
            };
            var tradeRolePayBills = new List<Distribution.Domain.Bill.Pay.Normal.NormalPayRoleBill>();
            // 采购方
            var purchaserDeduction = tradeDeduction.Purchaser;
            var purchaser = new Distribution.Domain.Role.Purchaser(purchaserDeduction.Owner);
            payBill.Purchaser = purchaser.MakePayBill(trade, purchaserDeduction.Deduction);
            tradeRolePayBills.Add(payBill.Purchaser);
            // 分润方
            foreach(var royaltyDeduction in tradeDeduction.Royalties) {
                if(royaltyDeduction.Deduction.Rebate != 0 || royaltyDeduction.Deduction.Increasing != 0) {
                    var royalty = new Distribution.Domain.Role.Royalty(royaltyDeduction.Owner);
                    var royaltyPayBill = royalty.MakePayBill(trade, royaltyDeduction.Deduction);
                    payBill.AddRoyalty(royaltyPayBill);
                    tradeRolePayBills.Add(royaltyPayBill);
                }
            }
            // 平台
            var platform = new Distribution.Domain.Role.Platform();
            payBill.Platform = platform.MakePayBill(trade, tradeRolePayBills, tradeDeduction.Platform);
            return payBill;
        }
        /// <summary>
        /// 生成分润方支付账单
        /// </summary>
        /// <param name="trade">交易信息</param>
        /// <param name="tradeDeduction">交易方信息</param>
        /// <param name="reproduceRoyaltyBill">是否重新生成分润方账单</param>
        public static Distribution.Domain.Bill.Pay.NormalPayBill ProduceProvidePayBill(Distribution.Domain.TradeInfo trade, Distribution.Domain.TradeDeduction tradeDeduction, bool reproduceRoyaltyBill) {
            if(trade == null) throw new ArgumentNullException("trade");
            if(tradeDeduction == null) throw new ArgumentNullException("tradeDeduction");

            var payBill = DistributionQueryService.QueryNormalPayBill(trade.Id);
            return ProduceProvidePayBill(trade, tradeDeduction, payBill, reproduceRoyaltyBill);
        }
        /// <summary>
        /// 生成分润方支付账单
        /// </summary>
        /// <param name="trade">交易信息</param>
        /// <param name="tradeDeduction">交易方信息</param>
        /// <param name="payBill">原支付账单</param>
        /// <param name="reproduceRoyaltyBill">是否重新生成分润方账单</param>
        public static Distribution.Domain.Bill.Pay.NormalPayBill ProduceProvidePayBill(Distribution.Domain.TradeInfo trade, Distribution.Domain.TradeDeduction tradeDeduction, Distribution.Domain.Bill.Pay.NormalPayBill payBill, bool reproduceRoyaltyBill) {
            if(trade == null) throw new ArgumentNullException("trade");
            if(trade.Passengers == null || !trade.Passengers.Any()) throw new CustomException("缺少乘机人信息");
            if(trade.Flights == null || !trade.Flights.Any()) throw new CustomException("缺少航班信息");
            if(tradeDeduction == null) throw new ArgumentNullException("tradeDeduction");
            if(tradeDeduction.Provider == null) throw new CustomException("缺少出票方信息");
            if(trade.IsThirdRelation && tradeDeduction.Supplier == null) throw new CustomException("缺少产品方信息");
            if(payBill == null) throw new NotFoundException("支付账单不存在");
            if(payBill.Purchaser == null) throw new CustomException("账单信息数据错误，缺少买家账单信息");

            var tradeRolePayBills = new List<Distribution.Domain.Bill.Pay.Normal.NormalPayRoleBill>();
            // 采购方
            payBill.Purchaser.Source.ReviseParValue(trade.Flights);
            tradeRolePayBills.Add(payBill.Purchaser);
            // 出票方
            var provider = new Distribution.Domain.Role.Provider(tradeDeduction.Provider.Owner);
            var providerPayBill = provider.MakePayBill(trade, tradeDeduction.Provider.Deduction);
            payBill.Provider = providerPayBill;
            tradeRolePayBills.Add(providerPayBill);
            // 资源方
            if(trade.IsThirdRelation && tradeDeduction.Supplier != null) {
                var supplier = new Distribution.Domain.Role.Supplier(tradeDeduction.Supplier.Owner);
                var supplierPayBill = supplier.MakePayBill(trade, tradeDeduction.Supplier.Deduction);
                payBill.Supplier = supplierPayBill;
                tradeRolePayBills.Add(supplierPayBill);
            }
            // 分润方
            if(reproduceRoyaltyBill) {
                payBill.RemoveRoyalties();
                foreach(var royaltyDeduction in tradeDeduction.Royalties) {
                    if(royaltyDeduction.Deduction.Rebate != 0 || royaltyDeduction.Deduction.Increasing != 0) {
                        var royalty = new Distribution.Domain.Role.Royalty(royaltyDeduction.Owner);
                        var royaltyPayBill = royalty.MakePayBill(trade, royaltyDeduction.Deduction);
                        payBill.AddRoyalty(royaltyPayBill);
                        tradeRolePayBills.Add(royaltyPayBill);
                    }
                }
            } else {
                foreach(var royaltyBill in payBill.Royalties) {
                    royaltyBill.Source.ReviseParValue(trade.Flights);
                    tradeRolePayBills.Add(royaltyBill);
                }
            }
            // 平台
            var platform = new Distribution.Domain.Role.Platform();
            payBill.Platform = platform.MakePayBill(trade, tradeRolePayBills, tradeDeduction.Platform);
            return payBill;
        }
        /// <summary>
        /// 修改价格信息
        /// 重新生成支付账单
        /// </summary>
        /// <param name="trade">交易信息</param>
        /// <param name="payBill">原支付账单信息</param>
        public static void RefreshPayBill(Distribution.Domain.TradeInfo trade, Distribution.Domain.Bill.Pay.NormalPayBill payBill) {
            if(payBill == null) throw new ArgumentNullException("payBill");
            payBill.RefreshPrice(trade);
        }
        /// <summary>
        /// 修改票面价
        /// </summary>
        /// <param name="trade">交易信息</param>
        /// <param name="payBill">原支付账单信息</param>
        public static void RefreshFare(Distribution.Domain.TradeInfo trade, Distribution.Domain.Bill.Pay.NormalPayBill payBill) {
            if(payBill == null) throw new ArgumentNullException("payBill");
            payBill.RefreshFare(trade);
        }
        /// <summary>
        /// 修改发布价
        /// </summary>
        /// <param name="trade">交易信息</param>
        public static Distribution.Domain.Bill.Pay.NormalPayBill RefreshReleaseFare(Distribution.Domain.TradeInfo trade) {
            var payBill = DistributionQueryService.QueryNormalPayBill(trade.Id);
            if(payBill == null) throw new NotFoundException("支付账单不存在");
            payBill.RefreshReleaseFare(trade);
            return payBill;
        }
        /// <summary>
        /// 生成支付账单
        /// </summary>
        /// <param name="postponeApplyform">改期申请</param>
        public static Distribution.Domain.Bill.Pay.PostponePayBill ProducePayBill(Order.Domain.Applyform.PostponeApplyform postponeApplyform) {
            if(postponeApplyform == null) throw new ArgumentNullException("postponeApplyform");
            if(!postponeApplyform.Passengers.Any()) throw new CustomException("缺少乘机人信息");
            if(!postponeApplyform.Flights.Any()) throw new CustomException("缺少航段信息");
            if(DistributionQueryService.QueryPostponePayBill(postponeApplyform.Id) != null) throw new RepeatedItemException("改期支付账单已存在");

            var payBill = new Distribution.Domain.Bill.Pay.PostponePayBill(postponeApplyform.OrderId, postponeApplyform.Id)
            {
                Remark = "改期收费"
            };
            // 申请方
            var applier = new Distribution.Domain.Role.Purchaser(postponeApplyform.PurchaserId);
            payBill.Applier = applier.MakePayBill(postponeApplyform.Flights, postponeApplyform.Passengers);
            // 受理方
            var accepter = new Distribution.Domain.Role.Platform();
            payBill.Accepter = accepter.MakePayBill(postponeApplyform.Flights, postponeApplyform.Passengers, new[] { payBill.Applier });
            return payBill;
        }
        /// <summary>
        /// 采购支付订单金额成功
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="account">支付账号</param>
        /// <param name="payTradeNo">支付交易流水号</param>
        /// <param name="payInterface">支付接口</param>
        /// <param name="payAccountType">支付账号类型</param>
        /// <param name="payTime">支付时间</param>
        public static Distribution.Domain.Bill.Pay.NormalPayBill PurchaserPaySuccessForOrder(decimal orderId, string account, string payTradeNo, PayInterface payInterface, PayAccountType payAccountType, DateTime payTime, string channelTradeNo) {
            var payBill = DistributionQueryService.QueryNormalPayBill(orderId);
            if(payBill == null) throw new NotFoundException("无 " + orderId + " 的记录");
            payBill.PaySuccess(account, payTradeNo, payInterface, payAccountType, payTime, channelTradeNo);
            return payBill;
        }
        /// <summary>
        /// 采购支付改期费成功
        /// </summary>
        /// <param name="postponeApplyformId">改期申请单号</param>
        /// <param name="payAccount">支付账号</param>
        /// <param name="payTradeNo">支付交易流水号</param>
        /// <param name="payInterface">支付接口</param>
        /// <param name="payAccountType">支付账号类型</param>
        /// <param name="payTime">支付时间</param>
        public static Distribution.Domain.Bill.Pay.PostponePayBill PurchaserPaySuccessForPostpone(decimal postponeApplyformId, string payAccount, string payTradeNo, PayInterface payInterface, PayAccountType payAccountType, DateTime payTime, string channelTradeNo) {
            var payBill = DistributionQueryService.QueryPostponePayBill(postponeApplyformId);
            if(payBill == null) throw new NotFoundException("无 " + postponeApplyformId + " 的记录");
            payBill.PaySuccess(payAccount, payTradeNo, payInterface, payAccountType, payTime, channelTradeNo);
            return payBill;
        }
        /// <summary>
        /// 子支付
        /// </summary>
        public static void SubPaySuccess(Distribution.Domain.Bill.Pay.NormalPayBill payBill, DateTime payTime) {
            payBill.SubPaySuccess(payTime);
        }
        /// <summary>
        /// 分润方分润成功
        /// </summary>
        public static void RoyaltiesTradeSuccess(Distribution.Domain.Bill.Pay.NormalPayBill payBill, DateTime tradeTime) {
            payBill.RoyaltySuccess(tradeTime);
        }
        /// <summary>
        /// 生成退款账单
        /// 全退原订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static Distribution.Domain.Bill.Refund.NormalRefundBill ProduceNormalRefundBill(decimal orderId) {
            var payBill = DistributionQueryService.QueryNormalPayBill(orderId);
            if(payBill == null) throw new NotFoundException("无 " + orderId + " 的支付账单记录");
            if(DistributionQueryService.QueryNormalRefundBill(orderId) != null) throw new RepeatedItemException("退款账单已存在");
            return ProduceNormalRefundBill(payBill, "出票失败");
        }
        /// <summary>
        /// 生成退款账单
        /// 全退原订单
        /// </summary>
        /// <param name="payBill">原支付账单</param>
        /// <param name="remark">退款备注</param>
        public static Distribution.Domain.Bill.Refund.NormalRefundBill ProduceNormalRefundBill(Distribution.Domain.Bill.Pay.NormalPayBill payBill, string remark) {
            if(payBill == null) throw new ArgumentNullException("payBill");
            return payBill.MakeRefundBill(payBill.OrderId, null, remark);
        }
        /// <summary>
        /// 生成退款账单
        /// </summary>
        /// <param name="refundInfo">退款信息</param>
        /// <param name="remark">退款备注</param>
        public static Distribution.Domain.Bill.Refund.NormalRefundBill ProduceNormalRefundBill(Distribution.Domain.Bill.Refund.RefundInfo refundInfo, string remark) {
            if(refundInfo == null) throw new ArgumentNullException("refundInfo");
            if(!refundInfo.Passengers.Any()) throw new CustomException("缺少乘机人信息");
            if(!refundInfo.Flights.Any()) throw new CustomException("缺少航段信息");
            var payBill = DistributionQueryService.QueryNormalPayBill(refundInfo.OrderId);
            if(payBill == null) throw new NotFoundException("无 " + refundInfo.OrderId + " 的支付账单记录");
            var oldRefundBill = DistributionQueryService.QueryNormalRefundBill(refundInfo.ApplyformId);
            if(oldRefundBill != null && oldRefundBill.Succeed) throw new RepeatedItemException("该单子已经退款");
            return payBill.MakeRefundBill(refundInfo.ApplyformId, refundInfo, remark);
        }
        /// <summary>
        /// 生成差错退款账单
        /// </summary>
        /// <param name="refundInfo">退款信息</param>
        /// <param name="remark">退款备注</param>
        public static Distribution.Domain.Bill.Refund.NormalRefundBill ProduceErrorRefundBill(Distribution.Domain.Bill.Refund.ErrorRefundInfo refundInfo) {
            if(refundInfo == null) throw new ArgumentNullException("refundInfo");
            if(!refundInfo.Passengers.Any()) throw new CustomException("缺少乘机人信息");
            if(!refundInfo.Flights.Any()) throw new CustomException("缺少航段信息");
            var payBill = DistributionQueryService.QueryNormalPayBill(refundInfo.OrderId);
            if(payBill == null) throw new NotFoundException("无 " + refundInfo.OrderId + " 的支付账单记录");
            return payBill.MakeErrorRefundBill(refundInfo, "差错退款");
        }
        /// <summary>
        /// 生成退款账单
        /// </summary>
        /// <param name="postponeApplyformId">改期申请单号</param>
        public static Distribution.Domain.Bill.Refund.PostponeRefundBill ProducePostponeRefundBill(decimal postponeApplyformId) {
            var payBill = DistributionQueryService.QueryPostponePayBill(postponeApplyformId);
            if(payBill == null) throw new NotFoundException("无 " + postponeApplyformId + " 的支付账单记录");
            if(DistributionQueryService.QueryPostponeRefundBill(postponeApplyformId) != null) throw new RepeatedItemException("退款账单已存在");

            return payBill.MakeRefundBill("拒绝改期");
        }
        /// <summary>
        /// 生成退款账单
        /// </summary>
        /// <param name="payBill">改期支付账单</param>
        public static Distribution.Domain.Bill.Refund.PostponeRefundBill ProducePostponeRefundBill(Distribution.Domain.Bill.Pay.PostponePayBill payBill) {
            if(payBill == null) throw new ArgumentNullException("payBill");
            if(DistributionQueryService.QueryPostponeRefundBill(payBill.ApplyformId) != null) throw new RepeatedItemException("退款账单已存在");

            return payBill.MakeRefundBill("拒绝改期");
        }
        /// <summary>
        /// 退款成功
        /// </summary>
        /// <param name="refundBill">退票账单</param>
        /// <param name="refundResults">退款结果信息</param>
        public static void NormalRefundSuccess(Distribution.Domain.Bill.Refund.NormalRefundBill refundBill, IEnumerable<Tradement.RefundResult> refundResults) {
            refundBill.RefundSuccess(refundResults);
        }
        ///// <summary>
        ///// 退款成功
        ///// </summary>
        ///// <param name="postponeApplyformId">改期申请单号</param>
        ///// <param name="refundTime">退款时间</param>
        //public static Distribution.Domain.Bill.Refund.PostponeRefundBill PostponeRefundSuccess(decimal postponeApplyformId, DateTime refundTime) {
        //    var refundBill = DistributionQueryService.QueryPostponeRefundBill(postponeApplyformId);
        //    if(refundBill == null)
        //        throw new NotFoundException("无 " + postponeApplyformId + " 的记录");
        //    PostponeRefundSuccess(refundBill, refundTime);
        //    return refundBill;
        //}
        public static void PostponeRefundSuccess(Distribution.Domain.Bill.Refund.PostponeRefundBill refundBill, DateTime refundTime) {
            refundBill.RefundSuccess(refundTime);
        }
        /// <summary>
        /// 删除退款账单
        /// </summary>
        public static Distribution.Domain.Bill.Refund.NormalRefundBill DeleteRefundBill(decimal refundApplyformId) {
            var refundBill = DistributionQueryService.QueryNormalRefundBill(refundApplyformId);
            if(refundBill != null && refundBill.HasRefunded) throw new InvalidOperationException("该账单已退款");
            return refundBill;
        }
    }
}