using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Order.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Exception;
using System.Linq;
using RefundFlight = ChinaPay.B3B.Service.Order.Domain.Applyform.RefundFlight;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 申请单处理服务
    /// </summary>
    public static class ApplyformProcessService {
        #region "退/废票"
        /// <summary>
        /// 平台处理退票
        /// </summary>
        /// <param name="refundApplyformId">退票申请单号</param>
        /// <param name="refundable">是否可以退票</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void PlatformProcessRefundApplyform(decimal refundApplyformId, bool refundable, string operatorAccount) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 平台处理退票
        /// </summary>
        /// <param name="refundApplyformId">退票申请单号</param>
        /// <param name="refundViews">处理信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="increasing"> </param>
        public static void PlatformProcessRefundApplyform(decimal refundApplyformId, IEnumerable<PlatformProcessRefundView> refundViews, decimal increasing,string remark, string operatorAccount) {
            Checker.CheckApplyformLocked(refundApplyformId, operatorAccount);
            var refundApplyform = ApplyformQueryService.QueryRefundApplyform(refundApplyformId);
            if(refundApplyform == null) throw new NotFoundException(refundApplyformId.ToString(), "退票申请不存在");
            refundApplyform.ProcessByPlatform(refundViews);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(refundApplyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SavePlatformProcessRefundOrScrapApplyformLog(refundApplyform, refundViews, increasing, remark, operatorAccount);
            Order.RemindHelper.RemindApplyform(refundApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 出票方同意退/废票并退歀 或 提交财务审核
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票申请单号</param>
        /// <param name="processView">退票手续费信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="operatorName">申请处理操作员 </param>
        public static void AgreeRefundOrScrapByProvider(decimal refundOrScrapApplyformId, RefundProcessView processView, string operatorAccount, string operatorName) {
            Checker.CheckApplyformLocked(refundOrScrapApplyformId, operatorAccount);
            var refundOrScrapApplyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundOrScrapApplyformId);
            if(refundOrScrapApplyform == null) throw new NotFoundException(refundOrScrapApplyformId.ToString(), "退/废票申请不存在");
            // 在退款之前，原订单必须分润成功   但如果是内部机构的票(因为内部机构勿需支付票款的)，则无此判断
            if(!refundOrScrapApplyform.IsInterior) {
                var originalPayBill = DistributionQueryService.QueryNormalPayBill(refundOrScrapApplyform.OrderId);
                if(!originalPayBill.RoyaltySucceed) throw new CustomException("订单未分润成功，暂时不能退/废票，请稍后再试或联系平台处理");
            }
            var refundFlights = refundOrScrapApplyform.AgreeByProvider(processView, operatorAccount, operatorName);
            // 生成退款账单
            var refundInfo = new Distribution.Domain.Bill.Refund.RefundInfo(refundOrScrapApplyform.OrderId, refundOrScrapApplyform.Id) {
                HasSupplier = refundOrScrapApplyform.Order.IsThirdRelation
            };
            foreach(var flight in refundFlights) {
                refundInfo.AddFlight(flight);
            }
            foreach(var passenger in refundOrScrapApplyform.Passengers) {
                refundInfo.AddPassenger(passenger.Id);
            }
            var refundBill = DistributionProcessService.ProduceNormalRefundBill(refundInfo, refundOrScrapApplyform is ScrapApplyform ? "废票" : "退票");
            // 判断是否需要财务审核
            var workingSetting = Organization.CompanyService.GetWorkingSetting(refundOrScrapApplyform.ProviderId);
            var requireTreasurerAudit = workingSetting != null && workingSetting.RefundNeedAudit;
            if(!requireTreasurerAudit) {
                refundOrScrapApplyform.AgreeReturnMoneyByProviderTreasurer();
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    var orderRepository = Factory.CreateOrderRepository(command);
                    applyformRepository.Update(refundOrScrapApplyform);
                    distributionRepository.SaveRefundBill(refundBill);
                    if(!requireTreasurerAudit) {
                        orderRepository.UpdateOrderForApplyform(refundOrScrapApplyform.Order);
                    }
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveAgreeRefundOrScrapByProviderLog(refundOrScrapApplyform, operatorAccount);
            if (!requireTreasurerAudit)
            {
                Order.LogHelper.SaveAgreeReturnMoneyByProviderTreasurerLog(refundOrScrapApplyform, operatorAccount);
            }
            Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
            notifier.Execute();
            if(!requireTreasurerAudit) {
                // 退款是否成功，不影响业务流程
                // 如果失败，会由其他地方来单独处理
                providerReturnMoney(refundOrScrapApplyform);
                processRefundScore(refundOrScrapApplyform);

                Order.StatisticHelper.Statistic(refundOrScrapApplyform);
            }

        }
        /// <summary>
        /// 出票方拒绝退/废票
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票申请单号</param>
        /// <param name="reason">原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DenyRefundOrScrapByProvider(decimal refundOrScrapApplyformId, string reason, string operatorAccount) {
            Checker.CheckApplyformLocked(refundOrScrapApplyformId, operatorAccount);
            var refundOrScrapApplyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundOrScrapApplyformId);
            if(refundOrScrapApplyform == null)
                throw new NotFoundException(refundOrScrapApplyformId.ToString(), "退/废票申请不存在");
            refundOrScrapApplyform.DenyRefundByProvider(reason);
            var refundBill = DistributionProcessService.DeleteRefundBill(refundOrScrapApplyformId);
            if(refundOrScrapApplyform.IsInterior) {
                refundOrScrapApplyform.Deny(reason);
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(refundOrScrapApplyform);
                    if(refundBill != null) {
                        var distributionRepository = Factory.CreateDistributionRepository(command);
                        distributionRepository.DeleteRefundBill(refundBill);
                    }
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveDenyRefundOrScrapByProviderLog(refundOrScrapApplyform, reason, operatorAccount);
            Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 平台拒绝退/废票
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票申请单号</param>
        /// <param name="reason">原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DenyRefundOrScrapByPlatform(decimal refundOrScrapApplyformId, string reason, string operatorAccount) {
            Checker.CheckApplyformLocked(refundOrScrapApplyformId, operatorAccount);
            var refundOrScrapApplyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundOrScrapApplyformId);
            if(refundOrScrapApplyform == null)
                throw new NotFoundException(refundOrScrapApplyformId.ToString(), "退/废票申请不存在");
            refundOrScrapApplyform.Deny(reason);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(refundOrScrapApplyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            if(!refundOrScrapApplyform.IsInterior) {
                FreezeService.UnFreeze(refundOrScrapApplyform.Id);
            }

            Order.LogHelper.SaveDenyRefundOrScrapByPlatformLog(refundOrScrapApplyform, reason, operatorAccount);
            Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 重新处理退/废票
        /// </summary>
        /// <param name="refundApplyformId">退/废票订单号</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void ReRefund(decimal refundApplyformId, IEnumerable<PlatformProcessRefundView> refundViews, string remark, string operatorAccount) {
            Checker.CheckApplyformLocked(refundApplyformId, operatorAccount);
            var refundApplyform = ApplyformQueryService.QueryRefundApplyform(refundApplyformId);
            if(refundApplyform == null) throw new NotFoundException(refundApplyformId.ToString(), "退/废票申请不存在");
            refundApplyform.ProcessByPlatform(refundViews);
            //refundApplyform.ReProcess();

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(refundApplyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveReRefundLog(refundApplyform, refundViews, remark, operatorAccount);
            Order.RemindHelper.RemindApplyform(refundApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 重新处理退/废票
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票订单号</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void ReRefund(decimal refundOrScrapApplyformId, string operatorAccount) {
            Checker.CheckApplyformLocked(refundOrScrapApplyformId, operatorAccount);
            var refundOrScrapApplyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundOrScrapApplyformId);
            if(refundOrScrapApplyform == null) throw new NotFoundException(refundOrScrapApplyformId.ToString(), "退/废票申请不存在");
            refundOrScrapApplyform.ReProcess();

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(refundOrScrapApplyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveReRefundLog(refundOrScrapApplyform, operatorAccount);
            Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 订座信息已取消，转到出票方退/废票
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票申请单号</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void ReservationCanceled(decimal refundOrScrapApplyformId, string operatorAccount) {
            Checker.CheckApplyformLocked(refundOrScrapApplyformId, operatorAccount);
            var refundOrScrapApplyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundOrScrapApplyformId);
            if(refundOrScrapApplyform == null)
                throw new NotFoundException(refundOrScrapApplyformId.ToString(), "退/废票申请不存在");
            refundOrScrapApplyform.ReservationCanceled();

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(refundOrScrapApplyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveReservationCanceledLog(refundOrScrapApplyform, operatorAccount);
            Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 财务同意退款
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票申请单号</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void AgreeReturnMoneyByProviderTreasurer(decimal refundOrScrapApplyformId, string operatorAccount) {
            Checker.CheckApplyformLocked(refundOrScrapApplyformId, operatorAccount);
            var refundOrScrapApplyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundOrScrapApplyformId);
            if(refundOrScrapApplyform == null) throw new NotFoundException(refundOrScrapApplyformId.ToString(), "退/废票申请不存在");
            // 在退款之前，原订单必须分润成功   但如果是内部机构的票(因为内部机构勿需支付票款的)，则无此判断
            if(!refundOrScrapApplyform.IsInterior) {
                var originalPayBill = Service.DistributionQueryService.QueryNormalPayBill(refundOrScrapApplyform.OrderId);
                if(!originalPayBill.RoyaltySucceed) throw new CustomException("订单未分润成功，暂时不能退票，请稍后再试或联系平台处理");
            }
            refundOrScrapApplyform.AgreeReturnMoneyByProviderTreasurer();

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    var orderRepository = Factory.CreateOrderRepository(command);
                    applyformRepository.Update(refundOrScrapApplyform);
                    orderRepository.UpdateOrderForApplyform(refundOrScrapApplyform.Order);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveAgreeReturnMoneyByProviderTreasurerLog(refundOrScrapApplyform, operatorAccount);
            Order.StatisticHelper.Statistic(refundOrScrapApplyform);
            Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
            notifier.Execute();

            // 退款是否成功，不影响业务流程
            // 如果失败，会由其他地方来单独处理
            providerReturnMoney(refundOrScrapApplyform);
            processRefundScore(refundOrScrapApplyform);

        }

        private static void providerReturnMoney(RefundOrScrapApplyform refundOrScrapApplyform) {
            try {
                if(!refundOrScrapApplyform.IsInterior) {
                    FreezeService.UnFreeze(refundOrScrapApplyform.Id);
                    var refundBill = DistributionQueryService.QueryNormalRefundBill(refundOrScrapApplyform.Id);
                    var refundBusinessType = refundOrScrapApplyform is Order.Domain.Applyform.RefundApplyform
                                                 ? RefundBusinessType.Refund
                                                 : RefundBusinessType.Scrap;
                    var refundResults = Tradement.RefundmentService.Refund(refundBill, refundBusinessType);
                    if(refundResults.Any(item => item.Success)) {
                        var purchaserSucceed = refundBill.Purchaser.Success;
                        DistributionProcessService.NormalRefundSuccess(refundBill, refundResults);
                        using(var command = Factory.CreateCommand()) {
                            var distributionRepository = Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                        }
                        if(!purchaserSucceed && refundBill.Purchaser.Success) {
                            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
                            notifier.RefundSuccess();
                        }
                    }
                }
            } catch(Exception ex) { LogService.SaveExceptionLog(ex); }
        }
        /// <summary>
        /// 退票扣除积分
        /// </summary>
        /// <param name="applyform"></param>
        private static void processRefundScore(RefundOrScrapApplyform applyform) {
            if(applyform.IsInterior) return;
            try {
                var refundFare = applyform.RefundBill.Purchaser.Source.Details.Sum(d => d.Flight.Fare);
                bool isCredit = applyform.RefundBill.Tradement.IsPoolpay && applyform.RefundBill.Tradement.PayAccountType == PayAccountType.Credit;
                Service.Integral.IntegralServer.InsertIntergralByMoney(applyform.ApplierAccount, applyform.PurchaserId,
                                                                       applyform.PurchaserName, refundFare,
                                                                       applyform.RefundBill.Tradement.IsPoolpay, applyform.Id.ToString(), false, isCredit);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex);
            }
        }

        /// <summary>
        /// 财务拒绝退款
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票申请单号</param>
        /// <param name="reason">原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DenyReturnMoneyByProviderTreasurer(decimal refundOrScrapApplyformId, string reason, string operatorAccount) {
            Checker.CheckApplyformLocked(refundOrScrapApplyformId, operatorAccount);
            var refundOrScrapApplyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundOrScrapApplyformId);
            if(refundOrScrapApplyform == null)
                throw new NotFoundException(refundOrScrapApplyformId.ToString(), "退/废票申请不存在");
            refundOrScrapApplyform.DenyReturnMoneyByProviderTreasurer(reason);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(refundOrScrapApplyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveDenyReturnMoneyByProviderTreasurerLog(refundOrScrapApplyform, reason, operatorAccount);
            Order.RemindHelper.RemindApplyform(refundOrScrapApplyform);
            var notifier = new Order.Notify.RefundApplyformNotifier(refundOrScrapApplyform);
            notifier.Execute();
        }
        #endregion

        #region "改期"

        /// <summary>
        /// 同意改期
        /// </summary>
        /// <param name="postponeApplyformId">申请单号</param>
        /// <param name="postponeView">改期信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="operator">改期操作员 </param>
        public static void AgreePostpone(decimal postponeApplyformId, PostponeView postponeView, string operatorAccount, string @operator) {
            Checker.CheckApplyformLocked(postponeApplyformId, operatorAccount);
            var postponeApplyform = ApplyformQueryService.QueryPostponeApplyform(postponeApplyformId);
            if(postponeApplyform == null) throw new NotFoundException(postponeApplyformId.ToString(), "改期申请不存在");

            postponeApplyform.Agree(postponeView, operatorAccount, @operator);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    var orderRepository = Factory.CreateOrderRepository(command);
                    applyformRepository.Update(postponeApplyform);
                    orderRepository.UpdateOrderForApplyform(postponeApplyform.Order);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveAgreePostponeLog(postponeApplyform, postponeView, operatorAccount);
            Order.RemindHelper.RemindApplyform(postponeApplyform);
            var notifier = new Order.Notify.PostponeApplyformNotifier(postponeApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 收取改期费
        /// </summary>
        /// <param name="postponeApplyformId">申请单号</param>
        /// <param name="postponeFeeViews">改期费信息集合</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void AgreePostponeForFee(decimal postponeApplyformId, IEnumerable<PostponeFeeView> postponeFeeViews, string operatorAccount) {
            Checker.CheckApplyformLocked(postponeApplyformId, operatorAccount);
            var postponeApplyform = ApplyformQueryService.QueryPostponeApplyform(postponeApplyformId);
            if(postponeApplyform == null) throw new NotFoundException(postponeApplyformId.ToString(), "改期申请不存在");

            postponeApplyform.ChargePostponeFee(postponeFeeViews);
            var payBill = DistributionProcessService.ProducePayBill(postponeApplyform);
            postponeApplyform.PayBill = payBill;

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    var billRepository = Factory.CreateDistributionRepository(command);
                    applyformRepository.Update(postponeApplyform);
                    billRepository.SavePostponePayBill(payBill);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveAgreePostponeForFeeLog(postponeApplyform, postponeFeeViews, operatorAccount);
            Order.RemindHelper.RemindApplyform(postponeApplyform);
            var notifier = new Order.Notify.PostponeApplyformNotifier(postponeApplyform);
            notifier.Execute();
        }
        /// <summary>
        /// 判断改期收费申请单是否可支付
        /// </summary>
        public static bool Payable(decimal postponeApplyformId, out string message) {
            var applyform = ApplyformQueryService.QueryPostponeApplyform(postponeApplyformId);
            if(applyform == null) throw new NotFoundException(postponeApplyformId.ToString() + "改期申请单不存在");
            return applyform.IsPayable(out message);
        }
        /// <summary>
        /// 拒绝改期
        /// </summary>
        /// <param name="postponeApplyformId">申请单号</param>
        /// <param name="reason">原因</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DenyPostpone(decimal postponeApplyformId, string reason, string operatorAccount) {
            Checker.CheckApplyformLocked(postponeApplyformId, operatorAccount);
            var postponeApplyform = ApplyformQueryService.QueryPostponeApplyform(postponeApplyformId);
            if(postponeApplyform == null) throw new NotFoundException(postponeApplyformId.ToString(), "改期申请不存在");

            postponeApplyform.Deny(reason);
            Distribution.Domain.Bill.Refund.PostponeRefundBill refundBill = null;
            if(postponeApplyform.PayBill != null) {
                refundBill = DistributionProcessService.ProducePostponeRefundBill(postponeApplyformId);
            }

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(postponeApplyform);
                    if(refundBill != null) {
                        var distributionRepository = Factory.CreateDistributionRepository(command);
                        distributionRepository.SaveRefundBill(refundBill);
                    }
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            Order.LogHelper.SaveDenyPostponeLog(postponeApplyform, reason, operatorAccount);
            Order.RemindHelper.RemindApplyform(postponeApplyform);
            var notifier = new Order.Notify.PostponeApplyformNotifier(postponeApplyform);
            notifier.Execute();
            if(refundBill != null) {
                try {
                    var refundResult = Tradement.RefundmentService.Refund(postponeApplyform.OrderId, refundBill);
                    // 退款是否成功，不影响业务流程
                    // 如果失败，会由其他地方来单独处理
                    if(refundResult.Success) {
                        DistributionProcessService.PostponeRefundSuccess(refundBill, refundResult.RefundTime.Value);
                        using(var command = Factory.CreateCommand()) {
                            var distributionRepository = Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                        }
                        //发拒绝改期退款的通知
                        notifier.SendRefundApplySuccessNotify();
                    }
                } catch(Exception ex) { LogService.SaveExceptionLog(ex); }
            }

        }
        /// <summary>
        /// 改期费支付成功
        /// </summary>
        /// <param name="postponeApplyformId">申请单号</param>
        /// <param name="payAccount">支付账号</param>
        /// <param name="payTradeNo">流水号</param>
        /// <param name="payTime">支付时间</param>
        /// <param name="payInterface">支付接口</param>
        /// <param name="payAccountType">支付账号类型</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static decimal PostponeFeePaySuccess(decimal postponeApplyformId, string payAccount, string payTradeNo,string channelTradeNo, DateTime payTime, PayInterface payInterface, PayAccountType payAccountType, string operatorAccount) {
            var postponeApplyform = ApplyformQueryService.QueryPostponeApplyform(postponeApplyformId);
            if(postponeApplyform == null) throw new NotFoundException(postponeApplyformId.ToString(), "改期申请不存在");

            if(postponeApplyform.Status != PostponeApplyformStatus.Agreed) return postponeApplyform.OrderId;
            else if(postponeApplyform.Status == PostponeApplyformStatus.Cancelled) {
                processTimeoutPaySuccess(postponeApplyform, payAccount, payTradeNo, channelTradeNo, payInterface, payAccountType, payTime);
                return postponeApplyform.OrderId;
            }

            postponeApplyform.PaySuccess();
            var payBill = DistributionProcessService.PurchaserPaySuccessForPostpone(postponeApplyformId, payAccount, payTradeNo, payInterface, payAccountType, payTime,channelTradeNo);

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(postponeApplyform);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    distributionRepository.UpdatePayBillForPurchaserPayPostponeFeeSuccess(payBill);
                    var autoPayRepository = Factory.CreateAutoPayRepository(command);
                    //修改代扣状态 2013-4-01 wangsl
                    autoPayRepository.UpdateSuccess(postponeApplyform.Id);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            LockService.UnLockForcibly(postponeApplyformId.ToString());
            Order.LogHelper.SavePostponeFeePaySuccessLog(postponeApplyform, payAccount, payTradeNo, payTime, operatorAccount);
            Order.RemindHelper.RemindApplyform(postponeApplyform);
            var notifier = new Order.Notify.PostponeApplyformNotifier(postponeApplyform);
            notifier.Execute();
            return postponeApplyform.OrderId;
        }
        /// <summary>
        /// 取消改期
        /// </summary>
        /// <param name="postponeApplyformId">申请单号</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void CancelPostpone(decimal postponeApplyformId, string operatorAccount) {
            Checker.CheckApplyformLocked(postponeApplyformId, operatorAccount);
            var postponeApplyform = ApplyformQueryService.QueryPostponeApplyform(postponeApplyformId);
            if(postponeApplyform == null)
                throw new NotFoundException(postponeApplyformId.ToString(), "改期申请不存在");
            postponeApplyform.Cancel();

            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.Update(postponeApplyform);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            Order.LogHelper.SaveCancelPostponeLog(postponeApplyform, operatorAccount);
            Order.RemindHelper.RemindApplyform(postponeApplyform);
            var notifier = new Order.Notify.PostponeApplyformNotifier(postponeApplyform);
            notifier.Execute();
        }

        private static void processTimeoutPaySuccess(Order.Domain.Applyform.PostponeApplyform applyform, string payAccount, string payTradeNo,string channelTradeNo, PayInterface payInterface, PayAccountType payAccountType, DateTime payTime) {
            Distribution.Domain.Bill.Refund.PostponeRefundBill refundBill;
            using(var command = Factory.CreateCommand()) {
                command.BeginTransaction();
                try {
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    // 写支付账单
                    var payBill = DistributionProcessService.PurchaserPaySuccessForPostpone(applyform.Id, payAccount, payTradeNo, payInterface, payAccountType, payTime,channelTradeNo);
                    distributionRepository.UpdatePayBillForPurchaserPayPostponeFeeSuccess(payBill);
                    // 写退款账单
                    refundBill = DistributionProcessService.ProducePostponeRefundBill(payBill);
                    distributionRepository.SaveRefundBill(refundBill);
                    command.CommitTransaction();
                } catch(System.Data.Common.DbException ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                } catch(Exception ex) {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }

            // 提交退款
            try {
                var refundResult = Tradement.RefundmentService.Refund(applyform.OrderId, refundBill);
                // 退款是否成功，不影响业务流程
                // 如果失败，会由其他地方来单独处理
                if(refundResult.Success) {
                    DistributionProcessService.PostponeRefundSuccess(refundBill, refundResult.RefundTime.Value);
                    using(var command = Factory.CreateCommand()) {
                        var distributionRepository = Factory.CreateDistributionRepository(command);
                        distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                    }
                }
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex);
            }
        }
        #endregion

        #region  差额退款

        /// <summary>
        /// 申请差额退款
        /// </summary>
        /// <param name="applyformId">申请单号</param>
        /// <param name="Remark">差错备注</param>
        /// <param name="purchaerId">采购Id</param>
        /// <param name="applyerAccount">申请人帐号</param>
        /// <param name="applyerName"> 申请人姓名</param>
        //public static void ApplyBalanceRefund(decimal applyformId, string Remark,Guid purchaerId,string applyerAccount,string applyerName) {
        //    var applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyformId);
        //    if (applyform == null) throw new ArgumentNullException("申请单不存在");
        //    var balanceRefundApplyform = BalanceRefundApplyform.NewBalanceRefundApplyform(applyform, Remark, applyerAccount, applyerName);
        //    applyform.ApplyBalanceRefundApplyform();
        //    using (var command = Factory.CreateCommand())
        //    {
        //        command.BeginTransaction();
        //        try
        //        {
        //            var applyformRepository = Factory.CreateApplyformRepository(command);
        //            applyformRepository.InsertBalanceRefundApplyform(balanceRefundApplyform);
        //            applyformRepository.UpdateFlag(applyformId);
        //            command.CommitTransaction();
        //        }
        //        catch (System.Data.Common.DbException ex)
        //        {
        //            command.RollbackTransaction();
        //            LogService.SaveExceptionLog(ex);
        //            throw new Exception("系统错误", ex);
        //        }
        //        catch (Exception ex)
        //        {
        //            command.RollbackTransaction();
        //            LogService.SaveExceptionLog(ex);
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// 平台处理差额退款
        /// </summary>
        /// <param name="banlanceRefundId">差额退款申请单号</param>
        /// <param name="fee">差额费用信息</param>
        /// <param name="operatorAccount">处理人帐号</param>
        /// <param name="operatorName">处理人姓名 </param>
        public static void PlatformAgreeBalanceRefund(decimal banlanceRefundId, IEnumerable<BalanceRefundFeeView> fee, string operatorAccount,string operatorName) {
            var applyform = ApplyformQueryService.QueryBalanceRefundApplyform(banlanceRefundId);
            if (applyform == null) throw new ArgumentException("差额退款申请单不存在");
            if (fee == null || !fee.Any()) throw new ArgumentException("缺少差额费用信息");
            var flights = applyform.Applyform.Flights;
            foreach (BalanceRefundFeeView refundFeeView in fee)
            {
                var flight = flights.FirstOrDefault(f => refundFeeView.Voyage == f.OriginalFlight.Id);
                if (flight == null) throw new CustomException("航班信息不存在");
                if (refundFeeView.Rate.HasValue && refundFeeView.Rate.Value > 0)
                {
                    var ticketFee = flight.OriginalFlight.ReleaseFare == 0 ? flight.OriginalFlight.Bunk.Fare : flight.OriginalFlight.ReleaseFare;

                    flight.BanlanceFare = ticketFee * refundFeeView.Rate/100;
                }
                else if (refundFeeView.Fee.HasValue && refundFeeView.Fee.Value>0)
                {
                    flight.BanlanceFare = refundFeeView.Fee;
                }
                else
                {
                    throw new CustomException("缺少差额费用信息");
                }
            }
            var aviableAmount = applyform.Applyform.RefundBill.PayBill.Purchaser.Source.Details.Where(b => flights.Any(f => f.OriginalFlight.ReservateFlight == b.Flight.Id)).Sum(b=>b.Amount);
            if (flights.Sum(f => f.BanlanceFare) + Math.Abs(applyform.Applyform.RefundBill.Purchaser.Amount) > aviableAmount)
            {
                throw new CustomException("差额退款价格错误,总退款金额不能超过支付金额");
            }
            applyform.PlatformAgreeRefund(operatorAccount, operatorName);
            var balanceRefundBill = BuildErrorRefundBill(flights, applyform);
            using (var command = Factory.CreateCommand())
            {
                command.BeginTransaction();
                try
                {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.SaveBalanceRefundProcessStatus(applyform);
                    applyformRepository.UpdateFlightBalanceRefunfFee(applyform.AssociateApplyformId,flights);
                    var distributionRepository = Factory.CreateDistributionRepository(command);
                    distributionRepository.SaveRefundBill(balanceRefundBill);
                    command.CommitTransaction();
                }
                catch (System.Data.Common.DbException ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// 平台拒绝差额退款
        /// </summary>
        /// <param name="banlanceRefundId">差额退款申请单号</param>
        /// <param name="operatorAccount">处理人帐号</param>
        /// <param name="operatorName">处理人姓名 </param>
        public static void PlatformNotAgreeBalanceRefund(decimal banlanceRefundId, string operatorAccount, string operatorName,string reason)
        {
            var applyform = ApplyformQueryService.QueryBalanceRefundApplyform(banlanceRefundId);
            if (applyform == null) throw new ArgumentException("差额退款申请单不存在");
            applyform.PlatformNotAgreeRefund(operatorAccount, operatorName,reason);
            using (var command = Factory.CreateCommand())
            {
                command.BeginTransaction();
                try
                {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.SaveBalanceRefundProcessStatus(applyform);
                    command.CommitTransaction();
                }
                catch (System.Data.Common.DbException ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            if (!applyform.IsInterior)
            {
                FreezeService.UnFreeze(applyform.Id);
            }
        }

        /// <summary>
        /// 出票业务同意差额退款
        /// </summary>
        /// <param name="balanceRefundId">申请单号</param>
        /// <param name="operatorAccount">操作员帐号</param>
        /// <param name="operatorName">操作员名称 </param>
        public static void ProviderBusinessAgreeBalanceRefund(decimal balanceRefundId, string operatorAccount, string operatorName)
        {
            var applyform = ApplyformQueryService.QueryBalanceRefundApplyform(balanceRefundId);
            if (applyform == null) throw new ArgumentException("差额退款申请单不存在");
            applyform.ProviderBusinessAgreeRefund(operatorAccount, operatorName);
            // 判断是否需要财务审核
            var workingSetting = Organization.CompanyService.GetWorkingSetting(applyform.ProviderId);
            var requireTreasurerAudit = workingSetting != null && workingSetting.RefundNeedAudit;
            if (!requireTreasurerAudit)
            {
                applyform.ProviderTreasurerAgreeRefund(operatorAccount, operatorName);
            }
            using (var command = Factory.CreateCommand())
            {
                command.BeginTransaction();
                try
                {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.SaveBalanceRefundProcessStatus(applyform);
                    command.CommitTransaction();
                }
                catch (System.Data.Common.DbException ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// 出票业务拒绝差额退款
        /// </summary>
        /// <param name="balanceRefundId">申请单号</param>
        /// <param name="operatorAccount">操作员帐号</param>
        /// <param name="operatorName">操作员名称 </param>
        public static void ProviderBusinessNotAgreeBalanceRefund(decimal balanceRefundId, string operatorAccount, string operatorName,string reason)
        {
            var applyform = ApplyformQueryService.QueryBalanceRefundApplyform(balanceRefundId);
            if (applyform == null) throw new ArgumentException("差额退款申请单不存在");
            applyform.ProviderBusinessNotAgreeRefund(operatorAccount, operatorName,reason);
            using (var command = Factory.CreateCommand())
            {
                command.BeginTransaction();
                try
                {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.SaveBalanceRefundProcessStatus(applyform);
                    command.CommitTransaction();
                }
                catch (System.Data.Common.DbException ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// 出票方财务同意差额退款
        /// </summary>
        /// <param name="balanceRefundId"></param>
        /// <param name="operatorAccount"></param>
        /// <param name="operatorName"> </param>
        public static void ProviderTreasurerAgreeBalanceRefund(decimal balanceRefundId, string operatorAccount, string operatorName)
        {
            var applyform = ApplyformQueryService.QueryBalanceRefundApplyform(balanceRefundId);
            if (applyform == null) throw new ArgumentException("差额退款申请单不存在");
            applyform.ProviderTreasurerAgreeRefund(operatorAccount,operatorName);
            using (var command = Factory.CreateCommand())
            {
                command.BeginTransaction();
                try
                {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.SaveBalanceRefundProcessStatus(applyform);
                    command.CommitTransaction();
                }
                catch (System.Data.Common.DbException ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
            providerReturnMoney(applyform);
        }

        /// <summary>
        /// 出票财务拒绝差额退款
        /// </summary>
        /// <param name="balanceRefundId">申请单号</param>
        /// <param name="reason"> </param>
        /// <param name="operatorAccount">操作员帐号</param>
        /// <param name="operatorName">操作员名称 </param>
        public static void ProviderTreasurerNotAgreeBalanceRefund(decimal balanceRefundId, string reason, string operatorAccount, string operatorName)
        {
            var applyform = ApplyformQueryService.QueryBalanceRefundApplyform(balanceRefundId);
            if (applyform == null) throw new ArgumentException("差额退款申请单不存在");
            applyform.ProviderTreasurerNotAgreeRefund(operatorAccount, operatorName,reason);
            using (var command = Factory.CreateCommand())
            {
                command.BeginTransaction();
                try
                {
                    var applyformRepository = Factory.CreateApplyformRepository(command);
                    applyformRepository.SaveBalanceRefundProcessStatus(applyform);
                    command.CommitTransaction();
                }
                catch (System.Data.Common.DbException ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    command.RollbackTransaction();
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// 构建差错退款账单
        /// </summary>
        /// <param name="flights"></param>
        /// <param name="applyform"></param>
        /// <returns></returns>
        private static NormalRefundBill BuildErrorRefundBill(IEnumerable<RefundFlight> flights, BalanceRefundApplyform applyform)
        {
            var errorRefundInfo = new ErrorRefundInfo(applyform.OrderId, applyform.Id);
            foreach (Passenger passenger in applyform.Applyform.Passengers)
            {
                errorRefundInfo.AddPassenger(passenger.Id);
            }
            foreach (RefundFlight flight in flights)
            {
                if (flight.BanlanceFare != null)
                    errorRefundInfo.AddFlight(new ErrorRefundFlight(flight.OriginalFlight.Id)
                        {
                            Amount = flight.BanlanceFare.Value
                        });
            }
            var bill = DistributionProcessService.ProduceErrorRefundBill(errorRefundInfo);
            return bill;
        }
        /// <summary>
        /// 差额退款退钱
        /// </summary>
        /// <param name="balanceRefundApplyform"></param>
        private static void providerReturnMoney(BalanceRefundApplyform balanceRefundApplyform)
        {
            try
            {
                if (!balanceRefundApplyform.Applyform.IsInterior)
                {
                    var refundBill = DistributionQueryService.QueryNormalRefundBill(balanceRefundApplyform.Id);

                    var refundResults = Tradement.RefundmentService.Refund(refundBill, RefundBusinessType.BalanceRefund);
                    if (refundResults.Any(item => item.Success))
                    {
                        var purchaserSucceed = refundBill.Purchaser.Success;
                        DistributionProcessService.NormalRefundSuccess(refundBill, refundResults);
                        using (var command = Factory.CreateCommand())
                        {
                            var distributionRepository = Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                        }
                        if (!purchaserSucceed && refundBill.Purchaser.Success)
                        {
                            var notifier = new Order.Notify.BalanceRefundApplyformNotifier(balanceRefundApplyform);
                            notifier.RefundSuccess();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
            }
        }
        /// <summary>
        /// 差额退款扣减积分
        /// </summary>
        /// <param name="applyform"></param>
        private static void processRefundScore(BalanceRefundApplyform applyform)
        {
            if (applyform.Applyform.IsInterior) return;
            try
            {
                var refundFare = applyform.RefundBill.Purchaser.Source.Details.Sum(d => d.Flight.Fare);
                bool isCredit = applyform.RefundBill.Tradement.IsPoolpay && applyform.RefundBill.Tradement.PayAccountType == PayAccountType.Credit;
                Service.Integral.IntegralServer.InsertIntergralByMoney(applyform.ApplierAccount, applyform.PurchaserId,
                                                                       applyform.PurchaserName, refundFare,
                                                                       applyform.RefundBill.Tradement.IsPoolpay, applyform.Id.ToString(), false, isCredit);
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
            }
        }

        #endregion
    }
}