using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Order {
    internal class LogHelper {
        #region "订单"
        public static void SaveProduceOrderLog(Order.Domain.Order order) {
            var logContent = string.Format("乘机人:{0} 航段:{1}",
                order.PNRInfos.Join(",", pnr => pnr.Passengers.Join(",", passenger => passenger.Name)),
                order.PNRInfos.Join(",", pnr => pnr.Flights.Join(",", flight => flight.Departure.Code + "-" + flight.Arrival.Code)));
            var log = new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = "生成订单",
                Content = logContent,
                Company = order.Purchaser.CompanyId,
                Account = order.Purchaser.OperatorAccount,
                Role = OperatorRole.Purchaser,
                Time = order.Purchaser.ProducedTime,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser|OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
            logContent += string.Format(" 返点:{0}% 出票方名称:#{1}|{2}", (order.IsSpecial ? 0 : order.Provider.Rebate * 100).TrimInvaidZero(), order.IsSpecial ? string.Empty : order.Provider.Company.AbbreviateName, order.IsSpecial?Guid.Empty:order.Provider.Company.CompanyId);
            var log2 = new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = "生成订单",
                Content = logContent,
                Company = order.Purchaser.CompanyId,
                Account = order.Purchaser.OperatorAccount,
                Role = OperatorRole.Purchaser,
                Time = order.Purchaser.ProducedTime,
                VisibleRole = OrderRole.Platform
            };
            LogService.SaveOrderLog(log2);
        }
        public static void SaveSupplierReviseReleasedFareLog(decimal orderId, decimal originalFare, decimal newFare, decimal originalPurchaserAmount, decimal newPurchaserAmount, decimal increasing,decimal totalIncreasing,Guid supplier, bool isThirdRelation, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = orderId,
                Keyword = "修改价格",
                Content = string.Format("产品原价格:{0} 新价格:{1}；订单原金额:{2} 新金额:{3}", originalFare, newFare, originalPurchaserAmount, newPurchaserAmount),
                Company = supplier,
                Account = operatorAccount,
                Role = OperatorRole.Resourcer,
                VisibleRole = (isThirdRelation ? OrderRole.Supplier : OrderRole.Provider)  | OrderRole.Platform 
            };
            LogService.SaveOrderLog(log);

            var log2 = new Log.Domain.OrderLog
            {
                OrderId = orderId,
                Keyword = "修改价格",
                Content = string.Format("产品原价格:{0} 新价格:{1}；订单原金额:{2} 新金额:{3}", originalFare + increasing, newFare + increasing, originalPurchaserAmount + totalIncreasing, newPurchaserAmount + totalIncreasing),
                Company = supplier,
                Account = operatorAccount,
                Role = OperatorRole.Resourcer,
                VisibleRole =  OrderRole.Purchaser | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log2);
        }
        public static void SaveSupplierConfirmSuccessfulLog(Order.Domain.Order order, decimal? patPrice, string operatorAccount) {
            var keyword = "确认座位";
            var logContent = "确认成功，并提供座位。" + order.ReservationPNR.ToString();
            if(patPrice.HasValue) {
                logContent += " PAT价格:" + patPrice.Value;
            }
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = logContent,
                Company = order.IsThirdRelation ? order.Supplier.CompanyId : order.Provider.CompanyId,
                Account = operatorAccount,
                Role = order.IsThirdRelation ? OperatorRole.Resourcer : OperatorRole.Provider,
                VisibleRole = (order.IsThirdRelation ? OrderRole.Supplier : OrderRole.Provider) | OrderRole.Platform
            });
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = "确认成功",
                Company = order.IsThirdRelation ? order.Supplier.CompanyId : order.Provider.CompanyId,
                Account = operatorAccount,
                Role = order.IsThirdRelation ? OperatorRole.Resourcer : OperatorRole.Provider,
                VisibleRole = order.IsThirdRelation ? OrderRole.Provider | OrderRole.Purchaser : OrderRole.Purchaser|OrderRole.OEMOwner
            });
        }
        public static void SaveSupplierConfirmFailedLog(Order.Domain.Order order, string reason, string operatorAccount) {
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = "确认座位失败",
                Content = "确认座位失败。原因:" + reason,
                Company = order.IsThirdRelation ? order.Supplier.CompanyId : order.Provider.CompanyId,
                Account = operatorAccount,
                Role = order.IsThirdRelation ? OperatorRole.Resourcer : OperatorRole.Provider,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform|OrderRole.OEMOwner
            });
        }
        public static void SaveSupplyResourceLog(Order.Domain.Order order, decimal? patPrice, string operatorAccount) {
            var keyword = "提供座位";
            var logContent = "提供座位。" + order.ReservationPNR.ToString();
            if(patPrice.HasValue) {
                logContent += " PAT价格:" + patPrice.Value;
            }
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = logContent,
                Company = order.IsThirdRelation ? order.Supplier.CompanyId : order.Provider.CompanyId,
                Account = operatorAccount,
                Role = order.IsThirdRelation ? OperatorRole.Resourcer : OperatorRole.Provider,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform|OrderRole.OEMOwner
            });
        }
        public static void SaveDenySupplyResourceLog(Order.Domain.Order order, string reason, string operatorAccount) {
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = "提供座位",
                Content = "拒绝提供座位。原因:" + reason,
                Company = order.IsThirdRelation ? order.Supplier.CompanyId : order.Provider.CompanyId,
                Account = operatorAccount,
                Role = order.IsThirdRelation ? OperatorRole.Resourcer : OperatorRole.Provider,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform|OrderRole.OEMOwner
            });
        }
        public static void SavePaySuccessLog(Order.Domain.Order order, string payAccount, string payTradeNo, DateTime payTime, string operatorAccount) {
            var keyword = "预订支付";
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = string.Format("支付成功。账号:{0} 流水号:{1} 时间:{2}", payAccount, payTradeNo, payTime.ToString("yyyy-MM-dd HH:mm:ss")),
                Company = order.Purchaser.CompanyId,
                Account = operatorAccount,
                Role = OperatorRole.Purchaser,
                Time = payTime,
                VisibleRole = OrderRole.Purchaser | OrderRole.Platform|OrderRole.OEMOwner
            });
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = "支付成功",
                Company = order.Purchaser.CompanyId,
                Account = operatorAccount,
                Role = OperatorRole.Purchaser,
                Time = payTime,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier
            });
        }

        public static void SaveOutTicketLog(Order.Domain.Order order, TicketNoView ticketNoView, string operatorAccount, string originalTicketType, string orginalOfficeNo) {
            var keyword = "出票";
            var logContentBase = string.Format("出票。订座编码:{0} 出票编码:{1} {2}",
                                        order.ReservationPNR == null ? string.Empty : order.ReservationPNR.ToString(),
                                        order.ETDZPNR == null ? string.Empty : order.ETDZPNR.ToString(),
                                        ticketNoView.Items.Join(",", xx => string.Format("姓名:{0} 票号:{1}", xx.Name, xx.TicketNos.Join("、"))));
            var logContent = logContentBase + string.Format(",客票类型:{0}->{1},Office号{2}->{3}",
                                            originalTicketType, ticketNoView.TicketType.ToString(),
                                            orginalOfficeNo, ticketNoView.OfficeNo);
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = logContentBase,
                Company = order.Provider.CompanyId,
                Account = operatorAccount,
                Role = OperatorRole.Provider,
                VisibleRole = OrderRole.Supplier | OrderRole.Purchaser
            });
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = logContent,
                Company = order.Provider.CompanyId,
                Account = operatorAccount,
                Role = OperatorRole.Provider,
                VisibleRole = OrderRole.Provider | OrderRole.Platform
            });
        }
        public static void SaveRevisePriceLog(Order.Domain.Order order, string operatorAccount) {
            var logContent = "修改价格。" + order.PNRInfos.Join(" ",
                                                 pnr => pnr.Flights.Join(" ",
                                                                  flight => string.Format("修改价格。航段:{0}-{1} 票面价:{2} 机建:{3} 燃油:{4}",
                                                                      flight.Departure.Code,
                                                                      flight.Arrival.Code,
                                                                      flight.Price.Fare,
                                                                      flight.Price.AirportFee,
                                                                      flight.Price.BAF)));
            var log = new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = "修改价格",
                Content = logContent,
                Company = Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Platform
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveDenyOutticketLog(Order.Domain.Order order, string reason, string operatorAccount) {
            var logContent = "拒绝出票。原因:" + reason;
            var log = new Log.Domain.OrderLog() {
                OrderId = order.Id,
                Keyword = "出票",
                Content = logContent,
                Company = order.Provider.CompanyId,
                Account = operatorAccount,
                Role = OperatorRole.Provider,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform|OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveReSupplyResourceLog(Order.Domain.Order order, string operatorAccount) {
            var logContent = "重新提供座位";
            var log = new Log.Domain.OrderLog() {
                OrderId = order.Id,
                Keyword = "处理拒绝出票",
                Content = logContent,
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform|OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveReOutticketLog(Order.Domain.Order order, string operatorAccount) {
            var logContent = "指向原出票方，重新出票";
            var log = new Log.Domain.OrderLog() {
                OrderId = order.Id,
                Keyword = "处理拒绝出票",
                Content = logContent,
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveChangeProviderLog(Order.Domain.Order order, string originalProvider, string operatorAccount) {
            var keyword = "处理拒绝出票";
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = "换出票方出票。原出票方：" + originalProvider+
                string.Format(" 返点:{0}% 出票方名称:#{1}|{2}", 
                (order.IsSpecial ? 0 : order.Provider.Rebate * 100).TrimInvaidZero(), order.Provider.Company.AbbreviateName, order.Provider.Company.CompanyId),
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Platform
            });
            LogService.SaveOrderLog(new Log.Domain.OrderLog {
                OrderId = order.Id,
                Keyword = keyword,
                Content = "换出票方出票",
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.OEMOwner
            });
        }
        public static void SaveCancelOrderLog(Order.Domain.Order order, string operatorAccount) {
            var log = new Log.Domain.OrderLog() {
                OrderId = order.Id,
                Keyword = "处理拒绝出票",
                Content = "取消订单",
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveOrderTimeoutLog(Order.Domain.Order order) {
            var log = new Log.Domain.OrderLog() {
                OrderId = order.Id,
                Keyword = "订单检查",
                Content = "取消订单。原因：超出支付时限",
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = "系统",
                Role = OperatorRole.System,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveApplyLog(Order.Domain.Order order, BaseApplyform applyform, string operatorAccount) {
            var logContent = string.Format("申请{0}。乘机人:{1} 航段:{2}",
                applyform.ToString(), applyform.Passengers.Join(",", item => item.Name), getAppliedFlights(applyform));
            var log = new Log.Domain.OrderLog() {
                OrderId = order.Id,
                ApplyformId = applyform is UpgradeApplyform ? (applyform as UpgradeApplyform).NewOrderId : applyform.Id,
                Keyword = "退改签申请",
                Content = logContent,
                Role = OperatorRole.Purchaser,
                Company = order.Purchaser.CompanyId,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        private static string getAppliedFlights(BaseApplyform applyform) {
            if(applyform is RefundOrScrapApplyform) {
                return (applyform as RefundOrScrapApplyform).Flights.Join(",", item => item.OriginalFlight.Departure.Name + "-" + item.OriginalFlight.Arrival.Name);
            } else if(applyform is PostponeApplyform) {
                return (applyform as PostponeApplyform).Flights.Join(",", item => item.OriginalFlight.Departure.Name + "-" + item.OriginalFlight.Arrival.Name);
            } else if(applyform is UpgradeApplyform) {
                return (applyform as UpgradeApplyform).Flights.Join(",", item => FoundationService.QueryAirportName(item.Departure) + "-" + FoundationService.QueryAirportName(item.Arrival));
            }
            return string.Empty;
        }
        public static void SaveUpdateTicketNoLog(decimal orderId, List<Passenger> passenger, IEnumerable<Ticket> ticket, string originalTicketNo, string operatorAccount, OperatorRole role = OperatorRole.Platform) {
            var logContent = string.Format("修改票号。乘机人:{0} 客票序号:{1} 原票号:{2} 新票号:{3}", string.Join(",", passenger.Select(p => p.Name)), string.Join(",", ticket.Select(p => p.Serial)), originalTicketNo, string.Join("", ticket.Select(p => p.No))); var log = new Log.Domain.OrderLog() {
                OrderId = orderId,
                Keyword = "修改票号",
                Content = logContent,
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = role,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveUpdateCredentialsLog(Order.Domain.Order order, Passenger passenger, string originalCredentials, bool isPlatform, string operatorAccount) {
            var logContent = string.Format("修改证件号。乘机人:{0} 原证件号:{1} 新证件号:{2}", passenger.Name, originalCredentials, passenger.Credentials);
            var log = new Log.Domain.OrderLog() {
                OrderId = order.Id,
                Keyword = "修改证件号",
                Content = logContent,
                Company = isPlatform ? Service.Organization.Domain.Platform.Instance.Id : order.Purchaser.CompanyId,
                Account = operatorAccount,
                Role = isPlatform ? OperatorRole.Platform : OperatorRole.Purchaser,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        #endregion

        #region "退/废票"
        public static void SavePlatformProcessRefundOrScrapApplyformLog(RefundOrScrapApplyform refundOrScrapApplyform, IEnumerable<PlatformProcessRefundView> refundViews,decimal increasing, string remark, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "平台处理退/废票",
                Content = "退还服务费:" + refundViews.Sum(v => v.ServiceCharge) + "。备注：" + remark,
                Role = OperatorRole.Platform,
                Company = Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                VisibleRole = (refundOrScrapApplyform.Order.IsThirdRelation ? OrderRole.Supplier : OrderRole.Provider) |  OrderRole.Platform 
            };
            LogService.SaveOrderLog(log);
            var log2 = new Log.Domain.OrderLog
            {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "平台处理退/废票",
                Content = "退还服务费:" + (refundViews.Sum(v => v.ServiceCharge)+increasing) + "。备注：" + remark,
                Role = OperatorRole.Platform,
                Company = Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                VisibleRole = OrderRole.Purchaser |  OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log2);
        }
        public static void SaveAgreeRefundOrScrapByProviderLog(RefundOrScrapApplyform refundOrScrapApplyform, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "出票方处理退/废票",
                Content = "同意退/废票",
                Role = OperatorRole.Provider,
                Company = refundOrScrapApplyform.Order.Provider.CompanyId,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Platform
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveDenyRefundOrScrapByProviderLog(RefundOrScrapApplyform refundOrScrapApplyform, string reason, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "出票方处理退/废票",
                Content = "拒绝退/废票。原因：" + reason,
                Role = OperatorRole.Provider,
                Company = refundOrScrapApplyform.Order.Provider.CompanyId,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveDenyRefundOrScrapByPlatformLog(RefundOrScrapApplyform refundOrScrapApplyform, string reason, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "平台处理拒绝退/废票",
                Content = "拒绝退/废票。原因：" + reason,
                Role = OperatorRole.Platform,
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveReRefundLog(RefundOrScrapApplyform refundOrScrapApplyform, IEnumerable<PlatformProcessRefundView> refundViews, string remark, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "平台处理拒绝退/废票",
                Content = "重新处理退/废票。" + "退还服务费:" + refundViews.Sum(v => v.ServiceCharge) + "。备注：" + remark,
                Role = OperatorRole.Platform,
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                VisibleRole = (refundOrScrapApplyform.Order.IsThirdRelation ? OrderRole.Supplier : OrderRole.Provider) | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveReRefundLog(RefundOrScrapApplyform refundOrScrapApplyform, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "平台处理拒绝退/废票",
                Content = "重新处理退/废票",
                Role = OperatorRole.Platform,
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveReservationCanceledLog(RefundOrScrapApplyform refundOrScrapApplyform, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "平台处理退/废票",
                Content = "订座信息已处理，转出票方退/废票",
                Role = OperatorRole.Platform,
                Company = Service.Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Platform
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveAgreeReturnMoneyByProviderTreasurerLog(RefundOrScrapApplyform refundOrScrapApplyform, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "出票方处理退/废票",
                Content = "同意退款",
                Role = OperatorRole.Provider,
                Company = refundOrScrapApplyform.Order.Provider.CompanyId,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveDenyReturnMoneyByProviderTreasurerLog(RefundOrScrapApplyform refundOrScrapApplyform, string reason, string operatorAccount) {
            var log = new Log.Domain.OrderLog {
                OrderId = refundOrScrapApplyform.OrderId,
                ApplyformId = refundOrScrapApplyform.Id,
                Keyword = "出票方处理退/废票",
                Content = "拒绝退款。原因：" + reason,
                Role = OperatorRole.Provider,
                Company = refundOrScrapApplyform.Order.Provider.CompanyId,
                Account = operatorAccount,
                VisibleRole = OrderRole.Provider | OrderRole.Platform
            };
            LogService.SaveOrderLog(log);
        }
        #endregion

        #region "改期"
        public static void SaveAgreePostponeLog(PostponeApplyform postponeApplyform, PostponeView postponeView, string operatorAccount) {
            var log = new Log.Domain.OrderLog() {
                OrderId = postponeApplyform.OrderId,
                ApplyformId = postponeApplyform.Id,
                Keyword = "改期处理",
                Content = "同意改期",
                Company = Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Platform | OrderRole.Purchaser | OrderRole.Supplier | OrderRole.Provider | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveAgreePostponeForFeeLog(PostponeApplyform postponeApplyform, IEnumerable<PostponeFeeView> postponeFeeViews, string operatorAccount) {
            var log = new Log.Domain.OrderLog() {
                OrderId = postponeApplyform.OrderId,
                ApplyformId = postponeApplyform.Id,
                Keyword = "改期处理",
                Content = "收取改期费。" + postponeFeeViews.Join("; ", item => string.Format("航段:[{0}] 改期费:{1}", item.AirportPair.ToString('-'), item.Fee)),
                Company = Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Platform | OrderRole.Purchaser
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveDenyPostponeLog(PostponeApplyform postponeApplyform, string reason, string operatorAccount) {
            var log = new Log.Domain.OrderLog() {
                OrderId = postponeApplyform.OrderId,
                ApplyformId = postponeApplyform.Id,
                Keyword = "改期处理",
                Content = "拒绝改期。原因:" + reason,
                Company = Organization.Domain.Platform.Instance.Id,
                Account = operatorAccount,
                Role = OperatorRole.Platform,
                VisibleRole = OrderRole.Platform | OrderRole.Purchaser | OrderRole.Supplier | OrderRole.Provider | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SavePostponeFeePaySuccessLog(PostponeApplyform postponeApplyform, string payAccount, string payTradeNo, DateTime payTime, string operatorAccount) {
            var logContent = string.Format("支付改期费。账号:{0} 流水号:{1} 时间:{2}", payAccount, payTradeNo, payTime.ToString("yyyy-MM-dd mm:HH:ss"));
            var log = new Log.Domain.OrderLog() {
                OrderId = postponeApplyform.OrderId,
                ApplyformId = postponeApplyform.Id,
                Keyword = "支付改期费",
                Content = logContent,
                Company = postponeApplyform.PurchaserId,
                Account = operatorAccount,
                Role = OperatorRole.Purchaser,
                Time = payTime,
                VisibleRole = OrderRole.Platform | OrderRole.Purchaser | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        public static void SaveCancelPostponeLog(PostponeApplyform postponeApplyform, string operatorAccount) {
            var log = new Log.Domain.OrderLog() {
                OrderId = postponeApplyform.OrderId,
                ApplyformId = postponeApplyform.Id,
                Keyword = "改期处理",
                Content = "取消改期",
                Company = postponeApplyform.PurchaserId,
                Account = operatorAccount,
                Role = OperatorRole.Purchaser,
                VisibleRole = OrderRole.Platform | OrderRole.Purchaser | OrderRole.Supplier | OrderRole.Provider | OrderRole.OEMOwner
            };
            LogService.SaveOrderLog(log);
        }
        #endregion
    }
}