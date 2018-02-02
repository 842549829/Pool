using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order {
    class StatisticHelper {
        public static void Statistic(Order.Domain.Order order) {
            if(order == null || order.IsInterior) return;
            try {
                if(order.Status == OrderStatus.Finished) {
                    if(order.IsSpecial) {
                        if(order.IsThirdRelation) {
                            Service.Statistic.OrderStatisticService.SaveSpecialProductSupplyInfo(order.Supplier.CompanyId, getDeparture(order), getArrival(order), getTicketCount(order), true, (order.SupplyTime ?? order.Purchaser.PayTime).Value);
                            var providerETDZSpeed = (order.ETDZTime.Value - (order.SupplyTime.HasValue ? (order.SupplyTime>order.Purchaser.PayTime?order.SupplyTime:order.Purchaser.PayTime):order.Purchaser.PayTime).Value).TotalSeconds;
                            Service.Statistic.OrderStatisticService.SaveGeneralOrderETDZSpeed(order.Provider.CompanyId, order.Id, (int)providerETDZSpeed, order.ETDZTime.Value, getCarrier(order), getTicketType(order));
                        } else {
                            Service.Statistic.OrderStatisticService.SaveSpecialProductSupplyInfo(order.Provider.CompanyId, getDeparture(order), getArrival(order), getTicketCount(order), true, (order.SupplyTime ?? order.Purchaser.PayTime).Value);
                            var providerETDZSpeed = (order.ETDZTime.Value - order.Purchaser.PayTime.Value).TotalSeconds;
                            Service.Statistic.OrderStatisticService.SaveGeneralOrderETDZSpeed(order.Provider.CompanyId, order.Id, (int)providerETDZSpeed, order.ETDZTime.Value, getCarrier(order), getTicketType(order));
                        }
                    } else {
                        var providerETDZSpeed = (order.ETDZTime.Value - order.Purchaser.PayTime.Value).TotalSeconds;
                        Service.Statistic.OrderStatisticService.SaveGeneralOrderETDZSpeed(order.Provider.CompanyId, order.Id, (int)providerETDZSpeed, order.ETDZTime.Value, getCarrier(order), getTicketType(order));
                    }
                } else if(order.Status == OrderStatus.ConfirmFailed || order.Status == OrderStatus.DeniedWithSupply) {
                    if(order.IsThirdRelation) {
                        Service.Statistic.OrderStatisticService.SaveSpecialProductSupplyInfo(order.Supplier.CompanyId, getDeparture(order), getArrival(order), getTicketCount(order), false, DateTime.Today);
                    } else {
                        Service.Statistic.OrderStatisticService.SaveSpecialProductSupplyInfo(order.Provider.CompanyId, getDeparture(order), getArrival(order), getTicketCount(order), false, DateTime.Today);
                    }
                }
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "保存订单处理速度信息");
            }
        }
        public static void Statistic(Order.Domain.Applyform.RefundOrScrapApplyform applyform) {
            if(applyform == null || applyform.IsInterior || applyform.Status != RefundApplyformStatus.Refunded) return;
            if(!(applyform is Domain.Applyform.RefundApplyform)) return;
            try {
                var refundSpeed = (applyform.ProcessedTime.Value - applyform.AppliedTime).TotalSeconds;
                Service.Statistic.OrderStatisticService.SaveGeneralOrderRefundSpeed(applyform.ProviderId, applyform.Id, (int)refundSpeed, applyform.ProcessedTime.Value, getCarrier(applyform), getTicketType(applyform.Order));
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "保存退废票处理速度信息");
            }
        }
        static int getTicketCount(Order.Domain.Order order) {
            return order.PNRInfos.Sum(pnr => pnr.Passengers.Sum(passenger => passenger.Tickets.Count()));
        }

        static string getDeparture(Order.Domain.Order order) {
            var pnrInfo = order.PNRInfos.First();
            return pnrInfo.Flights.First().Departure.Code;
        }
        static string getArrival(Order.Domain.Order order) {
            var pnrInfo = order.PNRInfos.First();
            return pnrInfo.Flights.First().Arrival.Code;
        }
        static string getCarrier(Order.Domain.Order order) {
            var pnrInfo = order.PNRInfos.First();
            return pnrInfo.Flights.First().Carrier.Code;
        }
        static string getCarrier(Order.Domain.Applyform.RefundOrScrapApplyform applyform) {
            return applyform.OriginalFlights.First().Carrier.Code;
        }
        static Common.Enums.TicketType getTicketType(Order.Domain.Order order) {
            return order.Product.TicketType;
        }
    }
}