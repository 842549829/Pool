using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core;
using ChinaPay.Gateway.Tradement;

namespace ChinaPay.B3B.Service {
    public static class FreezeService {
        internal static FreezeInfo QueryFeezeInfo(decimal applyformId) {
            var repository = Order.Repository.Factory.CreateFreezeRepository();
            return repository.QueryFreezeInfo(applyformId);
        }

        public static IEnumerable<FreezeBaseInfo> Query(FreezeQueryCondition condition, Pagination pagination) {
            var repository = Order.Repository.Factory.CreateFreezeRepository();
            return repository.Query(condition, pagination);
        }

        internal static void Freeze(RefundOrScrapApplyform refundOrScrapApplyform) {
            if(!refundOrScrapApplyform.Order.Bill.PayBill.RoyaltySucceed) return;
            var providerPayBill = refundOrScrapApplyform.Order.Bill.PayBill.Provider;
            var tradeNo = refundOrScrapApplyform.Order.Bill.PayBill.Tradement.TradeNo;
            var freezeAccount = providerPayBill.Owner.Account;
            var freezeAmount = getFreezeAmount(refundOrScrapApplyform, providerPayBill);
            var freezeRequest = string.Format("交易流水号:{0} 账号:{1} 金额:{2}", tradeNo, freezeAccount, freezeAmount);
            var freezeResponse = string.Empty;
            string freezeNo = null;
            var freezeSuccess = false;
            try {

                TradeFreezeRequestProcess request = new TradeFreezeRequestProcess(refundOrScrapApplyform.OrderId, refundOrScrapApplyform.Id, tradeNo, providerPayBill.Owner.Account, freezeAmount);
                if (request.Execute())
                {
                    freezeNo = request.FreezeNo;
                    freezeResponse = "冻结号:" + freezeNo;
                    freezeSuccess = true;
                }
                else
                {
                    freezeResponse = "冻结号失败，" + request.Message;
                    freezeSuccess = false;
                }
            } catch(Exception ex) {
                freezeResponse = ex.Message;
            } finally {
                // 记录冻结记录
                var freezeInfo = new FreezeInfo() {
                    OrderId = refundOrScrapApplyform.OrderId,
                    ApplyformId = refundOrScrapApplyform.Id,
                    TradeNo = refundOrScrapApplyform.Order.Bill.PayBill.Tradement.TradeNo,
                    Account = freezeAccount,
                    Amount = freezeAmount,
                    No = freezeNo,
                    RequestTime = DateTime.Now,
                    Success = freezeSuccess
                };
                if(freezeSuccess) {
                    freezeInfo.ProcessedTime = DateTime.Now;
                    freezeInfo.Remark = string.Empty;
                } else {
                    freezeInfo.Remark = freezeResponse;
                }
                var repository = Order.Repository.Factory.CreateFreezeRepository();
                repository.Save(freezeInfo);
                // 记录交互日志
                Service.LogService.SaveTradementLog(new TradementLog() {
                    OrderId = refundOrScrapApplyform.OrderId,
                    ApplyformId = refundOrScrapApplyform.Id,
                    Type = TradementBusinessType.Freeze,
                    Request = freezeRequest,
                    Response = freezeResponse,
                    Time = DateTime.Now,
                    Remark = "冻结出票方票款"
                });
            }
        }

        internal static void Freeze(BalanceRefundApplyform balanceRefundApplyform)
        {
            if (!balanceRefundApplyform.Order.Bill.PayBill.RoyaltySucceed) return;
            if (!balanceRefundApplyform.Applyform.RefundBill.Succeed) return;
            var providerPayBill = balanceRefundApplyform.Order.Bill.PayBill.Provider;
            var tradeNo = balanceRefundApplyform.Order.Bill.PayBill.Tradement.TradeNo;
            var freezeAccount = providerPayBill.Owner.Account;
            var freezeAmount = getFreezeAmount(balanceRefundApplyform, providerPayBill,balanceRefundApplyform.Applyform.RefundBill.Provider);
            var freezeRequest = string.Format("交易流水号:{0} 账号:{1} 金额:{2}", tradeNo, freezeAccount, freezeAmount);
            var freezeResponse = string.Empty;
            string freezeNo = null;
            var freezeSuccess = false;
            try
            {

                TradeFreezeRequestProcess request = new TradeFreezeRequestProcess(balanceRefundApplyform.OrderId, balanceRefundApplyform.Id, tradeNo, providerPayBill.Owner.Account, freezeAmount);
                if (request.Execute())
                {
                    freezeNo = request.FreezeNo;
                    freezeResponse = "冻结号:" + freezeNo;
                    freezeSuccess = true;
                }
                else
                {
                    freezeResponse = "冻结号失败，" + request.Message;
                    freezeSuccess = false;
                }
            }
            catch (Exception ex)
            {
                freezeResponse = ex.Message;
            }
            finally
            {
                // 记录冻结记录
                var freezeInfo = new FreezeInfo()
                {
                    OrderId = balanceRefundApplyform.OrderId,
                    ApplyformId = balanceRefundApplyform.Id,
                    TradeNo = balanceRefundApplyform.Order.Bill.PayBill.Tradement.TradeNo,
                    Account = freezeAccount,
                    Amount = freezeAmount,
                    No = freezeNo,
                    RequestTime = DateTime.Now,
                    Success = freezeSuccess
                };
                if (freezeSuccess)
                {
                    freezeInfo.ProcessedTime = DateTime.Now;
                    freezeInfo.Remark = string.Empty;
                }
                else
                {
                    freezeInfo.Remark = freezeResponse;
                }
                var repository = Order.Repository.Factory.CreateFreezeRepository();
                repository.Save(freezeInfo);
                // 记录交互日志
                Service.LogService.SaveTradementLog(new TradementLog()
                {
                    OrderId = balanceRefundApplyform.OrderId,
                    ApplyformId = balanceRefundApplyform.Id,
                    Type = TradementBusinessType.Freeze,
                    Request = freezeRequest,
                    Response = freezeResponse,
                    Time = DateTime.Now,
                    Remark = "差额退款，冻结出票方票款"
                });
            }
        }

        public static void UnFreeze(decimal refundOrScrapApplyformId) {
            var repository = Order.Repository.Factory.CreateFreezeRepository();
            var freezeInfo = repository.QueryFreezeInfo(refundOrScrapApplyformId);
            if(freezeInfo != null && freezeInfo.Success) {
                var unfreezeInfo = repository.QueryUnfreezeInfo(freezeInfo.No);
                if(unfreezeInfo != null && unfreezeInfo.Success) return;
                var unfreezeNo = string.Empty;
                var unfreezeRequest = string.Format("冻结号:{0} 解冻金额:{1}", freezeInfo.No, freezeInfo.Amount);
                var unfreezeResponse = string.Empty;
                var unfreezeSuccess = false;
                try {


                    TradeUnFreezeRequestProcess request = new TradeUnFreezeRequestProcess(freezeInfo.No, freezeInfo.Amount);
                    if (request.Execute())
                    {
                        unfreezeResponse = "解冻结号:" + unfreezeNo;
                        unfreezeSuccess = true;
                    }
                    else
                    {
                        unfreezeResponse = "解冻失败，原因：" + request.Message;
                        unfreezeSuccess = false;
                    }
                    } catch(Exception ex) {
                    unfreezeResponse = ex.Message;
                } finally {
                    // 记录解冻结记录
                    if(unfreezeInfo == null) {
                        unfreezeInfo = new UnfreezeInfo() {
                            OrderId = freezeInfo.OrderId,
                            ApplyformId = freezeInfo.ApplyformId,
                            FreezeNo = freezeInfo.No,
                            Account = freezeInfo.Account,
                            Amount = freezeInfo.Amount,
                            No = unfreezeNo,
                            RequestTime = DateTime.Now,
                            Success = unfreezeSuccess
                        };
                        if(unfreezeSuccess) {
                            unfreezeInfo.ProcessedTime = DateTime.Now;
                        } else {
                            unfreezeInfo.Remark = unfreezeResponse;
                        }
                        repository.Save(unfreezeInfo);
                    } else {
                        if(unfreezeSuccess) {
                            unfreezeInfo.ProcessedTime = DateTime.Now;
                            unfreezeInfo.Success = true;
                            unfreezeInfo.Remark = string.Empty;
                        } else {
                            unfreezeInfo.Remark = unfreezeResponse;
                        }
                        unfreezeInfo.No = unfreezeNo;
                        repository.Update(unfreezeInfo);
                    }
                    // 记录交互日志
                    Service.LogService.SaveTradementLog(new TradementLog() {
                        OrderId = freezeInfo.OrderId,
                        ApplyformId = freezeInfo.ApplyformId,
                        Type = TradementBusinessType.Unfreeze,
                        Request = unfreezeRequest,
                        Response = unfreezeResponse,
                        Time = DateTime.Now,
                        Remark = "解冻结出票方票款"
                    });
                }
            }
        }

        private static decimal getFreezeAmount(RefundOrScrapApplyform refundOrScrapApplyform, NormalPayRoleBill roleBill) {
            return roleBill.Source.Details.Where(
                item =>
                refundOrScrapApplyform.Passengers.Any(p => p.Id == item.Passenger) &&
                refundOrScrapApplyform.Flights.Any(f => f.OriginalFlight.ReservateFlight == item.Flight.Id)).Sum(item => item.Amount);
        }

        private static decimal getFreezeAmount(BalanceRefundApplyform refundOrScrapApplyform, NormalPayRoleBill roleBill, NormalRefundRoleBill provider)
        {
            var payAmount = roleBill.Source.Details.Where(
                item =>
                refundOrScrapApplyform.Applyform.Passengers.Any(p => p.Id == item.Passenger) &&
                refundOrScrapApplyform.Applyform.Flights.Any(f => f.OriginalFlight.ReservateFlight == item.Flight.Id)).Sum(item => item.Amount);
            return payAmount - Math.Abs(provider.Amount);
        }
    }
}