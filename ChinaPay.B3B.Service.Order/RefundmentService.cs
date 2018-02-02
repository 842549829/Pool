using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.Gateway.Tradement;
using PoolPay.DataTransferObject;
using RefundType = PoolPay.DataTransferObject.RefundType;
using PoolPay.DomainModel.Trade;

namespace ChinaPay.B3B.Service.Tradement {
    /// <summary>
    /// 退款服务
    /// </summary>
    public static class RefundmentService {
        internal static void ProcessTradeRefundemnt(RefundFailedRecord refundInfo) {
            //if(refundInfo.BusinessType == RefundBusinessType.PayTimeout) {
            //    var order = OrderQueryService.QueryOrder(refundInfo.OrderId);
            //    if(order != null) {
            //        TradeRefund(order, refundInfo.PayTradeNo);
            //    }
            //    return;
            //}
            var refundBill = DistributionQueryService.QueryNormalRefundBill(refundInfo.ApplyformId);
            if(refundBill != null) {
                var refundResult = TradeRefund(refundBill, refundInfo.BusinessType);
                if(refundResult != null && refundResult.Success) {
                    DistributionProcessService.NormalRefundSuccess(refundBill, new[] { refundResult });
                    using(var command = Order.Repository.Factory.CreateCommand()) {
                        command.BeginTransaction();
                        try {
                            var distributionRepository = Order.Repository.Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                            command.CommitTransaction();
                        } catch(Exception ex) {
                            command.RollbackTransaction();
                            LogService.SaveExceptionLog(ex, "交易退款");
                            throw;
                        }
                        var order = OrderQueryService.QueryOrder(refundInfo.OrderId); 
                            //发取消出票退款成功通知
                            var notifier = new Order.Notify.OrderNotifier(order);
                            notifier.SendRefundSuccessNotify();
                    }
                }
            }
        }
        internal static void ProcessOrderRefundment(RefundFailedRecord refundInfo) {
            var refundBill = DistributionQueryService.QueryNormalRefundBill(refundInfo.ApplyformId);
            if(refundBill != null) {
                var refundResults = Refund(refundBill, refundInfo.BusinessType);
                if(refundResults.Any(item => item.Success)) {
                    var purchaserSucceed = refundBill.Purchaser.Success;
                    DistributionProcessService.NormalRefundSuccess(refundBill, refundResults);
                    using(var command = Order.Repository.Factory.CreateCommand()) {
                        command.BeginTransaction();
                        try {
                            var distributionRepository = Order.Repository.Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                            command.CommitTransaction();
                        } catch(Exception ex) {
                            command.RollbackTransaction();
                            LogService.SaveExceptionLog(ex, "交易分润退款");
                            throw;
                        }
                    }
                    if(!purchaserSucceed && refundBill.Purchaser.Success) {
                        var applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(refundInfo.ApplyformId);
                        var notifier = new Order.Notify.RefundApplyformNotifier(applyform);
                        notifier.RefundSuccess();
                    }
                }
            }
        }
        internal static void ProcessPostponeRefundment(RefundFailedRecord refundInfo) {
            var refundBill = DistributionQueryService.QueryPostponeRefundBill(refundInfo.ApplyformId);
            if(refundBill != null) {
                var refundResult = Refund(refundInfo.OrderId, refundBill);
                if(refundResult.Success) {
                    DistributionProcessService.PostponeRefundSuccess(refundBill, refundResult.RefundTime.Value);
                    using(var command = Order.Repository.Factory.CreateCommand()) {
                        command.BeginTransaction();
                        try {
                            var distributionRepository = Order.Repository.Factory.CreateDistributionRepository(command);
                            distributionRepository.UpdateRefundBillForRefundSuccess(refundBill);
                            command.CommitTransaction();
                        } catch(Exception ex) {
                            command.RollbackTransaction();
                            LogService.SaveExceptionLog(ex, "申请单退款");
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 交易退款
        /// </summary>
        /// <param name="refundBill">退款账单</param>
        /// <param name="refundBusinessType">退款类型</param>
        internal static RefundResult TradeRefund(Distribution.Domain.Bill.Refund.NormalRefundBill refundBill, RefundBusinessType refundBusinessType) {
            // 构建退款请求信息
            var tradeRefundView = new TradeRefundDTO() {
                TradeNo = refundBill.Tradement.Payment.TradeNo,
                RefundBatchNo = refundBill.Tradement.TradeNo,
                TradeRefundType = RefundType.TradeRefund,
                RefundAmount = refundBill.Purchaser.Amount,
                RefundReason = refundBusinessType.GetDescription()
            };
            RefundResult result = null;
            var failedMessage = string.Empty;
            //RefundBatch refundBatch = null;
            try {
                // 调用退款接口
                //refundBatch = PoolPay.Service.AccountTradeService.TradeRefund(tradeRefundView);
                //result = getRefundResult(refundBatch.TradeRefund, false);
                var refundRequestProcess = new RefundRequestProcess(tradeRefundView.TradeNo, tradeRefundView.RefundBatchNo, tradeRefundView.RefundReason, refundBill.Purchaser.Amount.ToString());
                if (refundRequestProcess.Execute())
                {
                    result = getRefundResult(RefundRequestProcess.ParseRefundInfo(refundRequestProcess.outPayer));
                }
                else
                {
                    failedMessage = refundRequestProcess.Message;
                }
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "提交交易退款");
                failedMessage = ex.Message;
            }
            // 记录交互日志
            var refundRequest = string.Format("支付交易流水号:{0} 退款批次号:{1} 退款类型:{2} 退款金额:{3} 退款原因{4}",
                                              tradeRefundView.TradeNo, tradeRefundView.RefundBatchNo,
                                              tradeRefundView.TradeRefundType, tradeRefundView.RefundAmount,
                                              tradeRefundView.RefundReason);
            Service.LogService.SaveTradementLog(new TradementLog {
                OrderId = refundBill.OrderId,
                ApplyformId = refundBill.ApplyformId,
                Type = TradementBusinessType.Refund,
                Request = refundRequest,
                Response = result == null ? failedMessage : getRefundResponse(result),
                Time = DateTime.Now,
                Remark = refundBusinessType.GetDescription()
            });
            // 如果退款失败，记录退款失败信息
            var repository = Order.Repository.Factory.CreateRefundRepository();
            if(result != null && result.Success) {
                repository.Delete(refundBill.ApplyformId);
            } else {
                repository.Save(new RefundFailedRecord() {
                    OrderId = refundBill.OrderId,
                    ApplyformId = refundBill.ApplyformId,
                    BusinessType = refundBusinessType,
                    PayTradeNo = tradeRefundView.TradeNo,
                    RefundFailedInfo =  failedMessage ,
                    RefundTime = DateTime.Now
                });
            }
            return result;
        }
        /// <summary>
        /// 超出支付时限，订单取消后，才支付成功引起的退款
        /// </summary>
        //internal static void TradeRefund(Order.Domain.Order order, string tradeNo) {
        //    // 构建退款请求信息
        //    var tradeRefundView = new TradeRefundDTO() {
        //        TradeNo = tradeNo,
        //        RefundBatchNo = order.Id.ToString(),
        //        TradeRefundType = RefundType.TradeRefund,
        //        RefundAmount = Math.Abs(order.Bill.PayBill.Purchaser.Amount),
        //        RefundReason = RefundBusinessType.PayTimeout.GetDescription()
        //    };
        //    RefundResult result = null;
        //    var failedMessage = string.Empty;
        //    //RefundBatch refundBatch = null;
        //    try {
        //        // 调用退款接口
        //        //refundBatch = PoolPay.Service.AccountTradeService.TradeRefund(tradeRefundView);
        //        //result = getRefundResult(refundBatch.TradeRefund, false);
        //        var refundRequestProcess = new RefundRequestProcess(tradeRefundView.TradeNo, tradeRefundView.RefundBatchNo, tradeRefundView.RefundReason, Math.Abs(order.Bill.PayBill.Purchaser.Amount).ToString());
        //        if (refundRequestProcess.Execute())
        //        {
        //            result = getRefundResult(RefundRequestProcess.ParseRefundInfo(refundRequestProcess.outPayer));
        //        }
        //        else
        //        {
        //            failedMessage = refundRequestProcess.Message;
        //        }
        //    } catch(Exception ex) {
        //        LogService.SaveExceptionLog(ex, "提交交易退款");
        //        failedMessage = ex.Message;
        //    }
        //    // 记录交互日志
        //    var refundRequest = string.Format("支付交易流水号:{0} 退款批次号:{1} 退款类型:{2} 退款金额:{3} 退款原因{4}",
        //                                      tradeRefundView.TradeNo, tradeRefundView.RefundBatchNo,
        //                                      tradeRefundView.TradeRefundType, tradeRefundView.RefundAmount,
        //                                      tradeRefundView.RefundReason);
        //    Service.LogService.SaveTradementLog(new TradementLog {
        //        OrderId = order.Id,
        //        Type = TradementBusinessType.Refund,
        //        Request = refundRequest,
        //        Response = result == null ? failedMessage : getRefundResponse(result),
        //        Time = DateTime.Now,
        //        Remark = tradeRefundView.RefundReason
        //    });
        //    // 如果退款失败，记录退款失败信息
        //    var repository = Order.Repository.Factory.CreateRefundRepository();
        //    if(result != null && result.Success) {
        //        repository.Delete(order.Id);
        //    } else {
        //        repository.Save(new RefundFailedRecord() {
        //            OrderId = order.Id,
        //            ApplyformId = order.Id,
        //            BusinessType = RefundBusinessType.PayTimeout,
        //            PayTradeNo = tradeNo,
        //            RefundFailedInfo = failedMessage,
        //            RefundTime = DateTime.Now
        //        });
        //    }
        //}
        /// <summary>
        /// 账单退款
        /// </summary>
        internal static List<RefundResult> Refund(Distribution.Domain.Bill.Refund.NormalRefundBill refundBill, RefundBusinessType refundBusinessType) {
            var result = new List<RefundResult>();
            // 构建退款请求信息
            var tradeRefundView = new TradeRefundDTO {
                TradeNo = refundBill.Tradement.Payment.TradeNo,
                RefundBatchNo = refundBill.Tradement.TradeNo,
                RoyaltyRefunds = getRefundRoyaltyViews(refundBill),
                RefundReason = refundBusinessType.GetDescription()
            };
            if(refundBill.Purchaser.Success) {
                tradeRefundView.TradeRefundType = RefundType.RoyaltyRefund;
            } else {
                tradeRefundView.RefundAmount = refundBill.Purchaser.Amount;
                tradeRefundView.TradeRefundType = RefundType.TradeRoayltyRefund;
                if(refundBill.PayBill.RequireSubPay && refundBill.Platform.TotalAmount > 0) {
                    tradeRefundView.TradeSupplRefund = getTradeRoyaltyView(refundBill.Platform.Account, refundBill.Platform.TotalAmount, "退还贴点票款");
                    var payAmount = Math.Abs(refundBill.PayBill.Platform.TotalAmount);
                    var refundedAmount = Math.Abs(refundBill.PayBill.RefundBills.Where(rb => rb.Platform.Success).Sum(rb => rb.Platform.TotalAmount));
                    var refundableAmount = payAmount - refundedAmount;
                    var requireRefundAmount = tradeRefundView.TradeSupplRefund.RoyaltyAmount;
                    tradeRefundView.TradeSupplRefund.RoyaltyAmount = refundableAmount >= requireRefundAmount ? requireRefundAmount : refundableAmount;
                }
            }
            RefundBatch refundBatch = null;
            var failedMessage = string.Empty;
            try {
                // 调用退款接口
                RefundRequestProcess refundRequestProcess = new RefundRequestProcess(tradeRefundView.TradeNo, tradeRefundView.RefundBatchNo, tradeRefundView.RefundReason,
                   refundBill.Purchaser.Success?string.Empty:refundBill.Purchaser.Amount.ToString(),
                    tradeRefundView.TradeSupplRefund != null ? string.Format("{0}|{1}|{2}|{3}", tradeRefundView.TradeSupplRefund.RoyaltyAccountNo, tradeRefundView.TradeSupplRefund.RoyaltyAmount, tradeRefundView.TradeSupplRefund.Note,
                    tradeRefundView.TradeSupplRefund.ExtendParameter) : string.Empty,
                    string.Join("^", tradeRefundView.RoyaltyRefunds.Select(r => string.Format("{0}|{1}|{2}|{3}",r.RoyaltyAccountNo,r.RoyaltyAmount,r.Note,r.ExtendParameter))));
                if (refundRequestProcess.Execute())
                {
                    var tradeRefund = RefundRequestProcess.ParseRefundInfo(refundRequestProcess.outPayer);
                    if (tradeRefund != null)
                    {
                        result.Add(getRefundResult(tradeRefund));
                    }
                    var tradeSubRefund = RefundRequestProcess.ParseRefundInfo(refundRequestProcess.outSubPayment);
                    if (tradeSubRefund != null)
                    {
                        result.Add(getRefundResult(tradeSubRefund));
                    }
                    result.AddRange(refundRequestProcess.outRoyalties.Split('^').Select(item => getRefundResult(RefundRequestProcess.ParseRefundInfo(item))));
                }
                else
                {
                    failedMessage = refundRequestProcess.Message;
                }
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "提交订单退款");
                failedMessage = ex.Message;
            }
            // 记录交互日志
            var refundRequest = string.Format("支付交易流水号:{0} 退款批次号:{1} 退款类型:{2} (买家)退款金额:{3} 退款原因:{4}",
                                              tradeRefundView.TradeNo, tradeRefundView.RefundBatchNo,
                                              tradeRefundView.TradeRefundType, tradeRefundView.RefundAmount,
                                              tradeRefundView.RefundReason);
            refundRequest += tradeRefundView.RoyaltyRefunds.Join(" ", item => string.Format("退款账号:{0} 退款金额:{1} 备注:{2} 扩展参数:{3}",
                item.RoyaltyAccountNo, item.RoyaltyAmount, item.Note, item.ExtendParameter));
            if(tradeRefundView.TradeSupplRefund != null) {
                refundRequest += string.Format("退款账号:{0} 退款金额:{1} 备注:{2} 扩展参数:{3}",
                    tradeRefundView.TradeSupplRefund.RoyaltyAccountNo, tradeRefundView.TradeSupplRefund.RoyaltyAmount, tradeRefundView.TradeSupplRefund.Note, tradeRefundView.TradeSupplRefund.ExtendParameter);
            }
            LogService.SaveTradementLog(new TradementLog {
                OrderId = refundBill.OrderId,
                ApplyformId = refundBill.ApplyformId,
                Type = TradementBusinessType.Refund,
                Request = refundRequest,
                Response = string.IsNullOrEmpty(failedMessage)?"处理成功":failedMessage,
                Time = DateTime.Now,
                Remark = refundBusinessType.GetDescription()
            });
            // 如果退款失败，记录退款失败信息
            var repository = Order.Repository.Factory.CreateRefundRepository();
            if(result.Any() && result.All(item => item.Success)) {
                repository.Delete(refundBill.ApplyformId);
            } else {
                repository.Save(new RefundFailedRecord {
                    OrderId = refundBill.OrderId,
                    ApplyformId = refundBill.ApplyformId,
                    BusinessType = refundBusinessType,
                    PayTradeNo = tradeRefundView.TradeNo,
                    RefundFailedInfo = refundBatch == null ? failedMessage : (getRefundFailedInfo(refundBatch.TradeRefund) + refundBatch.RoyaltyRefunds.Join(" ", getRefundFailedInfo)),
                    RefundTime = DateTime.Now
                });
            }
            return result;
        }
        /// <summary>
        /// 改期退款
        /// </summary>
        /// <param name="orderId">原订单号</param>
        /// <param name="refundBill">退款账单</param>
        internal static RefundResult Refund(decimal orderId, Distribution.Domain.Bill.Refund.PostponeRefundBill refundBill) {
            // 构建退款请求信息
            var tradeRefundView = new TradeRefundDTO() {
                TradeNo = refundBill.Tradement.Payment.TradeNo,
                RefundBatchNo = refundBill.Tradement.TradeNo,
                TradeRefundType = RefundType.TradeRefund,
                RefundAmount = refundBill.Applier.Amount,
                RefundReason = RefundBusinessType.DenyPostpone.GetDescription()
            };
            // 调用退款接口
#if(DEBUG)
            var refundBatch = ChinaPay.PoolPay.Service.AccountTradeService.TradeRefund(tradeRefundView);
            var result = new RefundResult() {
                Success = refundBatch.TradeRefund.Status == RefundStatus.RefundSuccess,
                Account = refundBatch.TradeRefund.IncomeAccount.AccountNo,
                RefundTime = refundBatch.TradeRefund.RefundDate
            };
#else
            RefundRequestProcess refundRequestProcess = new RefundRequestProcess(refundBill.Tradement.Payment.TradeNo,
                refundBill.Tradement.TradeNo, RefundBusinessType.DenyPostpone.GetDescription(),
                refundBill.Applier.Amount.ToString());

            RefundResult result = new RefundResult();
            if (refundRequestProcess.Execute())
            {
                var refundInfo = RefundRequestProcess.ParseRefundInfo(refundRequestProcess.outPayer);
                result = new RefundResult()
                {
                    Success = refundInfo.RefundStatus == Common.Enums.RefundStatus.RefundSuccess,
                    Account = refundInfo.Account,
                    RefundTime = refundInfo.RefundTime
                };
            }
            
#endif
            // 记录交互日志
            var refundRequest = string.Format("支付交易流水号:{0} 退款批次号:{1} 退款类型:{2} 退款金额:{3} 退款原因:{4}",
                                              tradeRefundView.TradeNo, tradeRefundView.RefundBatchNo,
                                              tradeRefundView.TradeRefundType, tradeRefundView.RefundAmount,
                                              tradeRefundView.RefundReason);
            Service.LogService.SaveTradementLog(new TradementLog {
                OrderId = orderId,
                ApplyformId = refundBill.ApplyformId,
                Type = TradementBusinessType.Refund,
                Request = refundRequest,
                Response = getRefundResponse(result),
                Time = DateTime.Now,
                Remark = RefundBusinessType.DenyPostpone.GetDescription()
            });
            // 如果退款失败，记录退款失败信息
            var repository = Order.Repository.Factory.CreateRefundRepository();
            if(result.Success) {
                repository.Delete(refundBill.ApplyformId);
            } else {
                repository.Save(new RefundFailedRecord() {
                    OrderId = orderId,
                    ApplyformId = refundBill.ApplyformId,
                    BusinessType = RefundBusinessType.DenyPostpone,
                    PayTradeNo = tradeRefundView.TradeNo,
                    RefundFailedInfo = getRefundResponse(result),
                    RefundTime = DateTime.Now
                });
            }
            return result;
        }

        private static string getRefundResponse(RefundResult refundInfo) {
            if (refundInfo == null) return string.Empty;
            return string.Format("退款账号:{0} 退款状态:{1} 退款时间:{2:yyyy-MM-dd}",
                                 refundInfo.Account,
                                 refundInfo.Success?"成功":"失败",
                                 refundInfo.RefundTime);
        }

        private static List<TradeRoyaltyDTO> getRefundRoyaltyViews(Distribution.Domain.Bill.Refund.NormalRefundBill refundBill) {
            var result = new List<TradeRoyaltyDTO>();
            // 出票方
            var provider = getTradeRoyaltyView(refundBill.Provider, "退还机票款");
            if(provider != null) {
                result.Add(provider);
            }
            // 产品方
            var supplier = getTradeRoyaltyView(refundBill.Supplier, "退还机票提成");
            if(supplier != null) {
                result.Add(supplier);
            }
            // 分润方
            foreach(var royaltyBill in refundBill.Royalties) {
                var royalty = getTradeRoyaltyView(royaltyBill, "退还机票提成");
                if(royalty != null) {
                    result.Add(royalty);
                }
            }
            // 平台
            if(!refundBill.PayBill.RequireSubPay && refundBill.Platform != null) {
                if(refundBill.Platform.Deduction != null && refundBill.Platform.Deduction.Owner.Account != refundBill.Platform.Account) {
                    if(refundBill.Platform.ProfitAmount < 0 && !refundBill.Platform.Success) {
                        result.Add(getTradeRoyaltyView(refundBill.Platform.Account, refundBill.Platform.ProfitAmount, "退还交易手续费"));
                    }
                    if(!refundBill.Platform.Deduction.Success) {
                        result.Add(getTradeRoyaltyView(refundBill.Platform.Deduction, "退还机票提成"));
                    }
                } else if(refundBill.Platform.TotalAmount < 0 && !refundBill.Platform.Success) {
                    result.Add(getTradeRoyaltyView(refundBill.Platform.Account, refundBill.Platform.TotalAmount, "退还交易利润"));
                }
            }
            return combineRoyaltyInfo(result);
        }
        private static TradeRoyaltyDTO getTradeRoyaltyView(Distribution.Domain.Bill.Refund.Normal.NormalRefundRoleBill bill, string remark) {
            if(bill == null) return null;
            if(bill.Amount >= 0) return null;
            if(bill.Success) return null;
            if(string.IsNullOrWhiteSpace(bill.Owner.Account)) return null;

            return getTradeRoyaltyView(bill.Owner.Account, bill.Amount, remark, bill.Owner.RoleType);
        }
        private static TradeRoyaltyDTO getTradeRoyaltyView(string account, decimal amount, string note) {
            return getTradeRoyaltyView(account, amount, note, Distribution.Domain.Role.TradeRoleType.Platform);
        }
        #region  new

        
        private static string getTradeRoyaltyInfo(Distribution.Domain.Bill.Refund.Normal.NormalRefundRoleBill bill, string remark)
        {
            if (bill == null) return null;
            if (bill.Amount >= 0) return null;
            if (bill.Success) return null;
            if (string.IsNullOrWhiteSpace(bill.Owner.Account)) return null;

            return conbineRoyaltyInfo(bill.Owner.Account, bill.Amount, remark, bill.Owner.RoleType);
        }
        private static string conbineRoyaltyInfo(string account, decimal amount, string note, Distribution.Domain.Role.TradeRoleType role)
        {
            return string.Format("{0}|{1}|{2}|{3}", account, amount, note, role.ToString());
        }
        #endregion

        private static TradeRoyaltyDTO getTradeRoyaltyView(string account, decimal amount, string note, Distribution.Domain.Role.TradeRoleType role) {
            return new TradeRoyaltyDTO {
                RoyaltyAccountNo = account,
                RoyaltyAmount = Math.Abs(amount),
                Note = note,
                ExtendParameter = role.ToString()
            };
        }
        private static string getRefundResponse(RefundInfo refundInfo) {
            if(refundInfo == null) return string.Empty;
            return string.Format("退款账号:{0} 收款账号:{1} 退款金额:{2} 退款状态:{3} 退款时间:{4} 备注:{5}",
                                 refundInfo.RefundAccount.AccountNo,
                                 refundInfo.IncomeAccount.AccountNo,
                                 refundInfo.RefundAmount,
                                 refundInfo.Status,
                                 refundInfo.RefundDate.HasValue ? refundInfo.RefundDate.Value.ToLongTimeString() : string.Empty,
                                 refundInfo.Description);
        }
        private static string getRefundFailedInfo(RefundInfo refundInfo) {
            if(refundInfo == null) return string.Empty;
            return refundInfo.Status == RefundStatus.RefundFailed
                       ? string.Format("退款账号:{0} 收款账号:{1} 金额:{2} 原因:{3}",
                                       refundInfo.RefundAccount.AccountNo,
                                       refundInfo.IncomeAccount.AccountNo,
                                       refundInfo.RefundAmount,
                                       refundInfo.Description)
                       : string.Empty;
        }
        private static List<TradeRoyaltyDTO> combineRoyaltyInfo(IEnumerable<TradeRoyaltyDTO> royaltyViews) {
            var result = new List<TradeRoyaltyDTO>();
            foreach(var royaltyView in royaltyViews) {
                var sameAccountRoyalView = result.FirstOrDefault(i => string.Compare(i.RoyaltyAccountNo, royaltyView.RoyaltyAccountNo, true) == 0);
                if(sameAccountRoyalView == null) {
                    result.Add(royaltyView);
                } else {
                    sameAccountRoyalView.RoyaltyAmount += royaltyView.RoyaltyAmount;
                    sameAccountRoyalView.ExtendParameter += "," + royaltyView.ExtendParameter;
                }
            }
            return result;
        }
        private static RefundResult getRefundResult(RefundInfo refundInfo, bool isRoyaltyBill) {
            var result = new RefundResult {
                Success = refundInfo.Status == RefundStatus.RefundSuccess,
                Account = isRoyaltyBill ? refundInfo.RefundAccount.AccountNo : refundInfo.IncomeAccount.AccountNo,
                RefundTime = refundInfo.RefundDate,
            };
            if(!result.Success) {
                result.ErrorMessage = refundInfo.Description;
            }

            var billRoles = new List<Distribution.Domain.Role.TradeRoleType>();
            if(string.IsNullOrWhiteSpace(refundInfo.ExtendParameter)) {
                billRoles.Add(Distribution.Domain.Role.TradeRoleType.Purchaser);
            } else {
                var roleType = typeof(Distribution.Domain.Role.TradeRoleType);
                var billRoleTexts = refundInfo.ExtendParameter.Split(',');
                foreach(var tradeRole in billRoleTexts) {
                    billRoles.Add((Distribution.Domain.Role.TradeRoleType)Enum.Parse(roleType, tradeRole, true));
                }
            }
            result.Roles = billRoles;
            return result;
        }
        private static RefundResult getRefundResult(RefundRequestProcess.RefundInfo refundInfo)
        {
            var result = new RefundResult
            {
                Success = refundInfo.RefundStatus == B3B.Common.Enums.RefundStatus.RefundSuccess,
                Account = refundInfo.Account,
                RefundTime = refundInfo.RefundTime,
            };
            if (!result.Success)
            {
                result.ErrorMessage = refundInfo.RefundRemark;
            }

            var billRoles = new List<Distribution.Domain.Role.TradeRoleType>();
            if (string.IsNullOrWhiteSpace(refundInfo.RefundRemark))
            {
                billRoles.Add(Distribution.Domain.Role.TradeRoleType.Purchaser);
            }
            else
            {
                var roleType = typeof(Distribution.Domain.Role.TradeRoleType);
                var billRoleTexts = refundInfo.RefundRemark.Split(',');
                foreach (var tradeRole in billRoleTexts)
                {
                    billRoles.Add((Distribution.Domain.Role.TradeRoleType)Enum.Parse(roleType, tradeRole, true));
                }
            }
            result.Roles = billRoles;
            return result;
        }
    }
}