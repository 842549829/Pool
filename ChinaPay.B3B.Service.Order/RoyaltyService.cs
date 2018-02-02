using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.Gateway.Tradement;
using PoolPay.DataTransferObject;

namespace ChinaPay.B3B.Service.Tradement {
    internal static class RoyaltyService {
        /// <summary>
        /// 分润
        /// </summary>
        internal static bool Royalty(Order.Domain.Order order, Distribution.Domain.Bill.Pay.NormalPayBill payBill) {
            var royaltyView = getRolyatiesView(payBill);
            var royaltyRequest = "交易流水号:" + royaltyView.TradeNo + " 分润信息:" + royaltyView.Royalties.Join(",",
                    item => string.Format("账号:{0} 金额:{1} 备注:{2}", item.RoyaltyAccountNo, item.RoyaltyAmount, item.Note));
            bool result;
            string royaltyResponse;
            var repository = Order.Repository.Factory.CreateRoyaltyRepository();
            try {
#if(DEBUG)
                PoolPay.Service.AccountTradeService.OrderRoyalty(royaltyView);
                royaltyResponse = "分润成功";
                result = true;
                repository.Delete(order.Id);
#else
                RoyaltyRequestProcess request = new RoyaltyRequestProcess(royaltyView.TradeNo, string.Join("^",royaltyView.Royalties.Select(r=>string.Format("{0}|{1}|{2}",r.RoyaltyAccountNo,r.RoyaltyAmount,r.Note))));
                if (request.Execute())
                {
                    royaltyResponse = "分润成功";
                    result = true;
                    repository.Delete(order.Id);
                }
                else
                {
                    royaltyResponse = "分润失败，原因："+request.Message;
                    result = false;
                }
#endif
                } catch(Exception ex) {
                result = false;
                royaltyResponse = "分润失败。原因:" + ex.Message;
                // 保存分润失败信息
                repository.Save(new RoyaltyFailedRecord {
                    OrderId = order.Id,
                    PayTime = order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value : DateTime.Now,
                    ETDZTime = order.ETDZTime.HasValue ? order.ETDZTime.Value : DateTime.Now,
                    TradeAmount = payBill.Tradement.Amount,
                    RoyaltyInfo = royaltyView.Royalties.Join(",", item => string.Format("账号:{0} 金额:{1}", item.RoyaltyAccountNo, item.RoyaltyAmount)),
                    FailedReason = ex.Message
                });
            }
            // 记录交互日志
            LogService.SaveTradementLog(new TradementLog {
                OrderId = order.Id,
                Type = TradementBusinessType.Royalty,
                Request = royaltyRequest,
                Response = royaltyResponse,
                Time = DateTime.Now,
                Remark = "出票后分润"
            });
            return result;
        }
        /// <summary>
        /// 子交易补款
        /// </summary>
        internal static bool SubPay(Order.Domain.Order order, Distribution.Domain.Bill.Pay.NormalPayBill payBill) {
            var subPayId = order.Id.ToString() + "1";
            var royaltyRequest = string.Format("交易流水号:{0} 补差账号:{1} 补差金额:{2} 补差号:{3}",
                payBill.Tradement.TradeNo, payBill.Platform.Account, payBill.Platform.TotalAmount, subPayId);
            bool result;
            string royaltyResponse = string.Empty;
            var repository = Order.Repository.Factory.CreateRoyaltyRepository();
            try {
#if(DEBUG)
                PoolPay.Service.AccountTradeService.OrderTradeSuppl(decimal.Parse(payBill.Tradement.TradeNo), payBill.Platform.Account, Math.Abs(payBill.Platform.TotalAmount), subPayId, "贴点补差");
                royaltyResponse = "贴点补差成功";
                result = true;
                repository.Delete(order.Id);
#else

                SubPayRequestProcess subPay = new SubPayRequestProcess(payBill.Tradement.TradeNo,
                 subPayId, Math.Abs(payBill.Platform.TotalAmount), payBill.Platform.Account, "贴点补差");
                if (subPay.Execute())
                {
                    royaltyResponse = "贴点补差成功";
                    result = true;
                    repository.Delete(order.Id);
                }
                else
                {
                    royaltyResponse = "贴点补差失败,"+subPay.Message;
                    result = false;
                }
#endif
                } catch(Exception ex) {
                result = false;
                royaltyResponse = "贴点补差失败。原因:" + ex.Message;
                // 保存分润失败信息
                repository.Save(new RoyaltyFailedRecord {
                    OrderId = order.Id,
                    PayTime = order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value : DateTime.Now,
                    ETDZTime = order.ETDZTime.HasValue ? order.ETDZTime.Value : DateTime.Now,
                    TradeAmount = payBill.Tradement.Amount,
                    RoyaltyInfo = "补差金额:" + payBill.Platform.TotalAmount + " 账号:" + payBill.Platform.Account,
                    FailedReason = ex.Message
                });
            }
            // 记录交互日志
            LogService.SaveTradementLog(new TradementLog {
                OrderId = order.Id,
                Type = TradementBusinessType.SubPay,
                Request = royaltyRequest,
                Response = royaltyResponse,
                Time = DateTime.Now,
                Remark = "平台贴点补差"
            });
            return result;
        }

        private static TradeRoyaltiesDTO getRolyatiesView(Distribution.Domain.Bill.Pay.NormalPayBill payBill) {
            var royaltyViews = new List<TradeRoyaltyDTO>();
            // 出票方
            var provider = getRoyaltyView(payBill.Provider, "收机票款");
            if(provider != null) {
                royaltyViews.Add(provider);
            }
            // 产品方
            var supplier = getRoyaltyView(payBill.Supplier, "收机票款");
            if(supplier != null) {
                royaltyViews.Add(supplier);
            }
            // 分润方
            foreach(var royaltyBill in payBill.Royalties) {
                var royalty = getRoyaltyView(royaltyBill, "机票款提成");
                if(royalty != null) {
                    royaltyViews.Add(royalty);
                }
            }
            // 平台
            if(!payBill.RequireSubPay && payBill.Platform != null) {
                if(payBill.Platform.Deduction != null && payBill.Platform.Account != payBill.Platform.Deduction.Owner.Account) {
                    if(payBill.Platform.ProfitAmount > 0) {
                        royaltyViews.Add(getRoyaltyView(payBill.Platform.Account, payBill.Platform.ProfitAmount, "交易手续费"));
                    }
                    if(payBill.Platform.Deduction.Amount > 0) {
                        royaltyViews.Add(getRoyaltyView(payBill.Platform.Deduction.Owner.Account, payBill.Platform.Deduction.Amount, "机票款提成"));
                    }
                } else if(payBill.Platform.TotalAmount > 0) {
                    royaltyViews.Add(getRoyaltyView(payBill.Platform.Account, payBill.Platform.TotalAmount, "交易利润"));
                }
            }
            return new TradeRoyaltiesDTO {
                TradeNo = payBill.Tradement.TradeNo,
                Royalties = combineRoyaltyInfo(royaltyViews)
            };
        }
        private static TradeRoyaltyDTO getRoyaltyView(Distribution.Domain.Bill.Pay.Normal.NormalPayRoleBill bill, string remark) {
            if(bill == null) return null;
            if(bill.Amount <= 0) return null;
            if(string.IsNullOrWhiteSpace(bill.Owner.Account)) return null;

            return getRoyaltyView(bill.Owner.Account, bill.Amount, remark);
        }
        private static TradeRoyaltyDTO getRoyaltyView(string account, decimal amount, string remark) {
            return new TradeRoyaltyDTO {
                RoyaltyAccountNo = account,
                RoyaltyAmount = amount,
                Note = remark
            };
        }
        private static List<TradeRoyaltyDTO> combineRoyaltyInfo(IEnumerable<TradeRoyaltyDTO> royaltyViews) {
            var result = new List<TradeRoyaltyDTO>();
            foreach(var royaltyView in royaltyViews) {
                var sameAccountRoyalView = result.FirstOrDefault(i => string.Compare(i.RoyaltyAccountNo, royaltyView.RoyaltyAccountNo, true) == 0);
                if(sameAccountRoyalView == null) {
                    result.Add(royaltyView);
                } else {
                    sameAccountRoyalView.RoyaltyAmount += royaltyView.RoyaltyAmount;
                }
            }
            return result;
        }
    }
}