using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.B3B.Service.Remind;
using ChinaPay.Core;
using ChinaPay.Core.Exception;
using ChinaPay.Core.Extension;
using System.Linq;
using ChinaPay.Gateway.Tradement;
using ChinaPay.PoolPay.Service;
using ChinaPay.SMS.Service;
using ChinaPay.B3B.Service.ExternalPlatform;
using ChinaPay.B3B.Service.Organization;
using PayStatus = PoolPay.DataTransferObject.PayStatus;
using ChinaPay.B3B.Service.FlightTransfer;
using LogService = ChinaPay.B3B.Service.LogService;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;

namespace ChinaPay.B3B.TransactionWeb.OrderHandlers
{
    /// <summary>
    /// 订单操作
    /// </summary>
    public class Order : BaseHandler
    {
        public static Guid DefaultGuid
        {
            get
            {
                return new Guid("00000000-0000-0000-0000-000000000001");
            }
        }

        /// <summary>
        /// 支付
        /// </summary>
        //public void Pay(decimal id, string type, string account, string password) {
        //    if(type == "2") {
        //        Service.Tradement.PaymentService.PayPostponeFee(id, account, password, CurrentUser.UserName);
        //    } else {
        //        Service.Tradement.PaymentService.PayOrder(id, account, password, CurrentUser.UserName);
        //    }
        //}
        /// <summary>
        /// 取消订单
        /// </summary>
        public void Cancel(decimal orderId)
        {
            lockOrder(orderId, Service.Locker.LockRole.Platform, "取消订单");
            try
            {
                Service.OrderProcessService.CancelOrder(orderId, CurrentUser.UserName);
            }
            finally
            {
                BasePage.ReleaseLock(orderId);
            }
        }
        /// <summary>
        /// 重新出票
        /// </summary>
        public void ReETDZ(decimal orderId)
        {
            lockOrder(orderId, Service.Locker.LockRole.Platform, "重新出票");
            try
            {
                Service.OrderProcessService.ReOutticket(orderId, CurrentUser.UserName);
            }
            finally
            {
                BasePage.ReleaseLock(orderId);
            }
        }
        /// <summary>
        /// 重新提供资源
        /// </summary>
        public void ReSupply(decimal orderId)
        {
            lockOrder(orderId, Service.Locker.LockRole.Platform, "重新提供座位");
            try
            {
                Service.OrderProcessService.ReSupplyResource(orderId, CurrentUser.UserName);
            }
            finally
            {
                BasePage.ReleaseLock(orderId);
            }
        }
        /// <summary>
        /// 修改证件号
        /// </summary>
        public void UpdateCredentials(decimal orderId, string passengerName, string originalCredentials, string newCredentials)
        {
            lockOrder(orderId, Service.Locker.LockRole.Platform, "修改证件号");
            try
            {
                Service.OrderProcessService.UpdateCredentials(orderId, passengerName, originalCredentials, newCredentials, CurrentUser.UserName, true, BasePage.OwnerOEMId);
            }
            catch (Exception)
            {
                throw new CustomException("修改证件号失败！");
            }
            finally
            {
                BasePage.ReleaseLock(orderId);
            }
        }
        /// <summary>
        /// 修改票号
        /// </summary>
        public void UpdateTicketNo(decimal orderId, string originalTicketNo, string[] newTicketNo, bool isPlatform, string settleCode)
        {
            lockOrder(orderId, Service.Locker.LockRole.Platform, "修改证件号");
            try
            {
                Service.OrderProcessService.UpdateTicketNo(orderId, originalTicketNo, newTicketNo, CurrentUser.UserName, isPlatform, settleCode);
            }
            finally
            {
                BasePage.ReleaseLock(orderId);
            }
        }
        /// <summary>
        ///  出票
        /// </summary>
        public void ETDZ(decimal orderId, string newPNRPair, IEnumerable<DataTransferObject.Order.TicketNoView.Item> ticketNos, string NewSettleCode,
            string officeNo, TicketType ticketType)
        {
            var ticketNoView = new DataTransferObject.Order.TicketNoView()
            {
                ETDZPNR = getPNRPair(newPNRPair),
                Mode = ETDZMode.Manual,
                Items = ticketNos,
                NewSettleCode = NewSettleCode,
                OfficeNo = officeNo,
                TicketType = ticketType
            };

            Service.OrderProcessService.OutTicket(orderId, ticketNoView, CurrentUser.UserName, CurrentUser.Name, BasePage.OwnerOEMId);
            BasePage.ReleaseLock(orderId);
        }
        /// <summary>
        /// 拒绝出票
        /// </summary>
        public void DenyETDZ(decimal orderId, string reason)
        {
            Service.OrderProcessService.DenyOutticket(orderId, reason, CurrentUser.UserName);
            BasePage.ReleaseLock(orderId);
        }
        /// <summary>
        /// 拒绝出票(快捷方式)
        /// </summary>
        public object QuicklyDenyETDZ(decimal orderId, string reason)
        {
            string lockErrorMsg = "";
            bool isLocked = BasePage.Lock(orderId, Service.Locker.LockRole.Provider, "出票", out lockErrorMsg);
            if ( isLocked)
            {
                Service.OrderProcessService.DenyOutticket(orderId, reason, CurrentUser.UserName);
                BasePage.ReleaseLock(orderId);
            }
            return new
            {
               LockErrorMsg= lockErrorMsg,
               IsLocked = isLocked
            };
        }
        /// <summary>
        /// 修改发布价格
        /// </summary>
        public void ReviseReleasedFare(decimal orderId, decimal releasedFare)
        {
            Service.OrderProcessService.ReviseReleasedFare(orderId, releasedFare, CurrentUser.UserName);
        }
        /// <summary>
        /// 确认座位失败
        /// </summary>
        public void ConfirmFailed(decimal orderId, string reason)
        {
            Service.OrderProcessService.SupplierConfirmFailed(orderId, reason, CurrentUser.UserName);
            BasePage.ReleaseLock(orderId);
        }
        /// <summary>
        /// 确认并提供座位
        /// </summary>
        public string ConfirmAndSupply(decimal orderId, string pnrPair, decimal newPrice, Guid policyId)
        {
            decimal? NPrice = newPrice;
            if (NPrice == -1) NPrice = null;
            MatchedPolicy matchedPolicy = null;
            if (policyId != DefaultGuid)
            {
                var matchedPolicys = Session["matchedPolicy"] as IEnumerable<MatchedPolicy>;
                matchedPolicy = matchedPolicys.FirstOrDefault(p => p.Id == policyId);
            }
            var order = Session["confirmedOrder"] as Service.Order.Domain.Order ?? OrderQueryService.QueryOrder(orderId);
            var result = Service.OrderProcessService.SupplierConfirmSuccessful(order, getPNRPair(pnrPair), NPrice, CurrentUser.UserName, matchedPolicy, BasePage.OwnerOEMId);
            BasePage.ReleaseLock(orderId);
            Session["confirmedOrder"] = null;
            Session["matchedPolicy"] = null;
            if (result.Length == 6 && order.IsThirdRelation)
            {
                result = string.Empty;
            }
            return result;
        }
        /// <summary>
        /// 拒绝提供座位
        /// </summary>
        public void DenySupply(decimal orderId, string reason)
        {
            Service.OrderProcessService.DenySupplyResource(orderId, reason, CurrentUser.UserName);
            BasePage.ReleaseLock(orderId);
        }
        /// <summary>
        /// 提供座位
        /// </summary>
        public string Supply(decimal orderId, string pnrPair, decimal newPrice, Guid policyId)
        {
            decimal? NPrice = newPrice;
            if (NPrice == -1) NPrice = null;
            MatchedPolicy matchedPolicy = null;
            if (policyId != DefaultGuid)
            {
                var matchedPolicys = Session["matchedPolicy"] as IEnumerable<MatchedPolicy>;
                matchedPolicy = matchedPolicys.FirstOrDefault(p => p.Id == policyId);
            }
            var order = Session["confirmedOrder"] as Service.Order.Domain.Order ?? OrderQueryService.QueryOrder(orderId);
            var result = Service.OrderProcessService.SupplyResource(order, getPNRPair(pnrPair), NPrice, matchedPolicy, CurrentUser.UserName, BasePage.OwnerOEMId);
            BasePage.ReleaseLock(orderId);
            Session["confirmedOrder"] = null;
            Session["matchedPolicy"] = null;
            if (result.Length == 6 && order.IsThirdRelation)
            {
                result = string.Empty;
            }
            return result;
        }
        private DataTransferObject.Common.PNRPair getPNRPair(string pnrPair)
        {
            var codes = pnrPair.Split('|');
            return new DataTransferObject.Common.PNRPair(codes[0], codes[1]);
        }
        private void lockOrder(decimal orderId, Service.Locker.LockRole role, string remark)
        {
            string lockErrorMsg;
            if (!BasePage.Lock(orderId, role, remark, out lockErrorMsg))
            {
                throw new ChinaPay.Core.CustomException("锁定订单失败。原因:" + lockErrorMsg);
            }
        }

        ///// <summary>
        ///// 提供票面价并提供编码
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="pnrPair"></param>
        ///// <param name="newPrice"></param>
        ///// <returns></returns>
        //public string NewPriceAndSupplySource(decimal orderId, string pnrPair, decimal newPrice, string flightNo, DateTime flightDate, Guid policyId)
        //{
        //    MatchedPolicy matchedPolicy = null;
        //    if (policyId != Guid.Empty)
        //    {
        //        var matchedPolicys = Session["matchedPolicy"] as IEnumerable<MatchedPolicy>;
        //        matchedPolicy = matchedPolicys.FirstOrDefault(p => p.Id == policyId);
        //    }

        //    var result = Service.OrderProcessService.SupplyResource(orderId, getPNRPair(pnrPair), newPrice, matchedPolicy, CurrentUser.UserName);
        //    BasePage.ReleaseLock(orderId);
        //    return result;
        //}

        /// <summary>
        /// 修改价格信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="priceViews">价格信息</param>
        public void RevisePrice(decimal orderId, IEnumerable<PriceView> priceViews)
        {
            OrderProcessService.RevisePrice(orderId, priceViews, CurrentUser.UserName);
            BasePage.ReleaseLock(orderId);
        }

        private OrderQueryCondition GetCondition(OrderQueryCondition orderQueryCondition, OrderRole role)
        {
            if (!string.IsNullOrEmpty(orderQueryCondition.OrderStatusText))
            {
                string orderStatusText = orderQueryCondition.OrderStatusText;
                var statuses = Service.Order.StatusService.GetRoleOrderStatus(role)
                    .Where(s => s.Value == orderStatusText).Select(s => s.Key).ToList();
                if (role == OrderRole.OEMOwner)
                {
                    var oem = OEMService.QueryOEM(CurrentCompany.CompanyId);
                    orderQueryCondition.OEMID = oem.Id;
                }
                if (statuses.Any())
                {
                    orderQueryCondition.Status = 0;
                    foreach (var status in statuses)
                    {
                        //if (role == OrderRole.Provider && status == OrderStatus.Ordered) continue;
                        orderQueryCondition.Status |= status;
                    }
                }
            }
            switch (role)
            {
                case OrderRole.Platform:
                    break;
                case OrderRole.Provider:
                    orderQueryCondition.Provider = CurrentCompany.CompanyId;
                    orderQueryCondition.ProviderProductType = orderQueryCondition.ProductType;
                    orderQueryCondition.ProductType = null;
                    break;
                case OrderRole.Purchaser:
                    orderQueryCondition.Purchaser = CurrentCompany.CompanyId;
                    break;
                case OrderRole.Supplier:
                    orderQueryCondition.Supplier = CurrentCompany.CompanyId;
                    break;
            }
            if (orderQueryCondition.RelationBrother || orderQueryCondition.RelationInterior || orderQueryCondition.RelationJunion)
            {
                orderQueryCondition.RelationType = 0;
            }
            if (orderQueryCondition.RelationBrother)
            {
                orderQueryCondition.RelationType |= RelationType.Brother;
            }
            if (orderQueryCondition.RelationInterior)
            {
                orderQueryCondition.RelationType |= RelationType.Interior;
            }
            if (orderQueryCondition.RelationJunion)
            {
                orderQueryCondition.RelationType |= RelationType.Junion;
            }
            return orderQueryCondition;
        }

        public object QueryProvideWaitOrderList(Pagination pagination, OrderQueryCondition orderQueryCondition)
        {
            if (CurrentCompany.CompanyType == Common.Enums.CompanyType.Supplier)
            {
                orderQueryCondition.Supplier = CurrentCompany.CompanyId;
            }
            else if (CurrentCompany.CompanyType == Common.Enums.CompanyType.Provider)
            {
                orderQueryCondition.Provider = CurrentCompany.CompanyId;
                var setting = CompanyService.GetWorkingSetting(CurrentCompany.CompanyId);
                if (setting != null && setting.IsImpower)
                {
                    orderQueryCondition.CustomNo = CompanyService.GetCustomNumberByEmployee(CurrentUser.Id).Join(",", c => c.Number);
                }
            }
            if (!orderQueryCondition.Status.HasValue)
                orderQueryCondition.Status = OrderStatus.PaidForETDZ | OrderStatus.Applied | OrderStatus.PaidForSupply;
            if (!string.IsNullOrEmpty(orderQueryCondition.OrderStatusText))
            {
                string orderStatusText = orderQueryCondition.OrderStatusText;
                var statuses = Service.Order.StatusService.GetRoleOrderStatus(CurrentCompany.CompanyType == Common.Enums.CompanyType.Supplier ? OrderRole.Supplier : OrderRole.Provider)
                    .Where(s => s.Value == orderStatusText).Select(s => s.Key).ToList();
                if (statuses.Any())
                {
                    orderQueryCondition.Status = 0;
                    foreach (var status in statuses)
                    {
                        orderQueryCondition.Status |= status;
                    }
                }
            }

            List<OrderListView> orders = OrderQueryService.QueryWaitOrders(orderQueryCondition, pagination).ToList();
            var lockInfos = LockService.Query(orders.Select(form => form.OrderId.ToString())).ToList();

            var ordersView = orders.Select(item =>
            {
                LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == item.OrderId.ToString());
                DateTime? startStatTime = null;
                if (CurrentCompany.CompanyType == CompanyType.Supplier)
                {
                }
                else if (item.IsSpecial && item.Supplier.HasValue)
                {
                    if (item.SupplyTime.HasValue && item.PayTime.HasValue)
                    {
                        startStatTime = item.SupplyTime.Value > item.PayTime.Value ? item.SupplyTime.Value : item.PayTime.Value;
                    }
                }
                else
                {
                    startStatTime = item.PayTime;
                }
                bool isRelation = item.PurcharseProviderRelation == null || item.PurcharseProviderRelation == RelationType.Brother;

                return new
                {
                    item.OrderId,
                    Product = item.ProductType.GetDescription(),
                    PNR =
                        item.ETDZPNR != null
                            ? item.ETDZPNR.ToListString()
                            : item.ReservationPNR == null ? string.Empty : item.ReservationPNR.ToListString(),
                    AirportPair =
                        item.Flights.Join("<br />",
                            f =>
                            string.Format(
                                "{0}{1} {2} {3}-{4}<br />{5:yyyy-MM-dd HH:mm}",
                                f.Carrier,
                                f.FlightNo,
                                string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk,
                                f.DepartureCity,
                                f.ArrivalCity,
                                f.TakeoffTime)),
                    Passenger = item.Passengers.Join("<br />"),
                    Fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero(),
                    AirportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero(),
                    BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero(),
                    SettleAmount = item.SettlementForProvider.Amount.TrimInvaidZero(),
                    Rebate = (item.SettlementForProvider.Rebate * 100).TrimInvaidZero() + "%",
                    Commission = item.SettlementForProvider.Commission.TrimInvaidZero(),
                    Status = Service.Order.StatusService.GetOrderStatus(item.Status, this.CurrentCompany.CompanyType == CompanyType.Supplier ? OrderRole.Supplier : OrderRole.Provider),
                    IsEmergentOrder = item.IsEmergentOrder,
                    ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                    ProductType = item.ProviderProductType.GetDescription(),
                    LockInfo = lockInfo == null ? string.Empty : lockInfo.Name + "<br />",
                    OfficeNum = item.OfficeNo,
                    PurchaseIsBother = isRelation,
                    Relation = isRelation ?
                    "平台" : item.PurcharseProviderRelation == RelationType.Interior ? "内部" : "下级",
                    TodaysFlight = item.Flights.Any(f => f.TakeoffTime.Date == DateTime.Today),
                    PassengerType = item.PassengerType.GetDescription(),
                    IsChildTicket = item.PassengerType == PassengerType.Child,
                    ETDZTime = (startStatTime.HasValue && item.RefuseETDZTime.HasValue) ? Math.Round((item.RefuseETDZTime.Value - startStatTime.Value).TotalMinutes).ToString() : (startStatTime.HasValue) ? Math.Round(((item.ETDZTime ?? DateTime.Now) - startStatTime.Value).TotalMinutes).ToString() : string.Empty,
                    TicketType = item.IsSpecial ? string.Empty : string.Format("({0})", item.TicketType.ToString()),
                    RemindContent = item.RemindTime.HasValue ? item.RemindContent : string.Empty,
                    RemindIsShow = item.IsNeedReminded,
                    IsPaidForETDZ = item.Status == OrderStatus.PaidForETDZ,
                    PayTime = item.PayTime.HasValue ? item.PayTime.Value.ToString("yyyy-MM-dd<br />HH:mm:ss") : ""
                };
            });
            return new
            {
                Orders = ordersView,
                Pagination = pagination
            };
        }
        public object QueryProvideOrderList(Pagination pagination, OrderQueryCondition orderQueryCondition)
        {
            var searchOrderRole = CompanyType.Provider == CurrentCompany.CompanyType ? OrderRole.Provider : OrderRole.Supplier;
            OrderQueryCondition condition = GetCondition(orderQueryCondition, searchOrderRole);
            condition.OrderRole = searchOrderRole;
            var orders = OrderQueryService.QueryOrders(condition, pagination).ToList().Select(item =>
            {
                var IsSpecial = item.ProductType == ProductType.Special;
                Settlement settlement;
                if (CompanyType.Provider == CurrentCompany.CompanyType)
                {
                    settlement = item.SettlementForProvider;
                }
                else if (IsSpecial && CompanyType.Supplier == CurrentCompany.CompanyType)
                {
                    settlement = item.SettlementForSupplier;
                }
                else
                {
                    settlement = item.SettlementForPurchaser;
                }
                var fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero();
                var airportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero();
                var BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero();
                var settleAmount = settlement.Amount.TrimInvaidZero();
                var rebateAndCommission = IsSpecial ? string.Empty : string.Format("{0}%/{1}",
                    (settlement.Rebate * 100).TrimInvaidZero(), settlement.Commission.TrimInvaidZero());
                DateTime? startStatTime = null;
                if (CurrentCompany.CompanyType == CompanyType.Supplier)
                {
                }
                else if (item.IsSpecial && item.Supplier.HasValue)
                {
                    if (item.SupplyTime.HasValue && item.PayTime.HasValue)
                    {
                        startStatTime = item.SupplyTime.Value > item.PayTime.Value ? item.SupplyTime.Value : item.PayTime.Value;
                    }
                }
                else
                {
                    startStatTime = item.PayTime;
                }
                return new
                {
                    OrderId = item.OrderId,
                    PNR = item.ETDZPNR != null ? item.ETDZPNR.ToListString() : (item.ReservationPNR == null ? string.Empty : item.ReservationPNR.ToListString()),
                    FlightInfo = string.Join("<br/>", item.Flights.Select(f => string.Format("{0}{1} {2} {3}{6}-{4}{7}<br/>{5}",
                        f.Carrier, f.FlightNo, f.Bunk, f.DepartureCity, f.ArrivalCity, f.TakeoffTime.ToString("yyyy-MM-dd HH:mm"), f.ArrivalTeminal, f.ArrivalTeminal))),
                    Passengers = string.Join("<br/>", item.Passengers),
                    Price = fare + "<br/>" + airportFee + "/" + BAF,
                    Commission = settleAmount + "<br/>" + rebateAndCommission,
                    ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = Service.Order.StatusService.GetOrderStatus(item.Status, searchOrderRole),
                    PayTime = startStatTime.HasValue ? startStatTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    IsEmergentOrder = item.IsEmergentOrder,
                    ETDZTime = (startStatTime.HasValue && item.RefuseETDZTime.HasValue) ? Math.Round((item.RefuseETDZTime.Value - startStatTime.Value).TotalMinutes) : (startStatTime.HasValue) ? Math.Round(((item.ETDZTime ?? DateTime.Now) - startStatTime.Value).TotalMinutes) : 0.1,
                    RemindContent = string.IsNullOrWhiteSpace(item.RemindContent) ? string.Empty : item.RemindContent,
                    RemindIsShow = item.IsNeedReminded
                };
            });
            return new
            {
                Orders = orders,
                Pagination = pagination
            };
        }
        public object QueryOperateOrderList(Pagination pagination, OrderQueryCondition orderQueryCondition)
        {
            List<OrderListView> orders = OrderQueryService.QueryOperateOrders(GetCondition(orderQueryCondition, OrderRole.Platform), pagination, false).ToList();
            var lockInfos = LockService.Query(orders.Select(form => form.OrderId.ToString())).ToList();
            return new
            {
                Pagination = pagination,
                Orders = orders.Select(item =>
                {
                    var lockInfo = lockInfos.FirstOrDefault(l => l.Key == item.OrderId.ToString());
                    var fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero();
                    var airportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero();
                    var BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero();
                    var settleAmount = item.SettlementForPurchaser.Amount.TrimInvaidZero();
                    var rebateAndCommission = string.Format("{0}%/{1}", (item.SettlementForPurchaser.Rebate * 100).TrimInvaidZero(),
                    item.SettlementForPurchaser.Commission.TrimInvaidZero());
                    DateTime? startStatTime = null;
                    if (CurrentCompany.CompanyType == CompanyType.Supplier)
                    {
                    }
                    else if (item.IsSpecial && item.Supplier.HasValue)
                    {
                        if (item.SupplyTime.HasValue && item.PayTime.HasValue)
                        {
                            startStatTime = item.SupplyTime.Value > item.PayTime.Value ? item.SupplyTime.Value : item.PayTime.Value;
                        }
                    }
                    else
                    {
                        startStatTime = item.PayTime;
                    }
                    return new
                    {
                        OrderId = item.OrderId,
                        PNR = item.ETDZPNR != null ? item.ETDZPNR.ToListString() : item.ReservationPNR == null ? string.Empty : item.ReservationPNR.ToListString(),
                        FlightInfo = string.Join("<br/>", item.Flights.Select(f => string.Format("{0}{1} {2} {3}{6}-{4}{7}<br/>{5}",
                            f.Carrier, f.FlightNo, f.Bunk, f.DepartureCity, f.ArrivalCity, f.TakeoffTime.ToString("yyyy-MM-dd HH:mm"), f.DepartureTeminal, f.ArrivalTeminal))),
                        Passengers = string.Join("<br/>", item.Passengers),
                        Price = fare + "</br>" + airportFee + "/" + BAF,
                        Commission = settleAmount + "<br/>" + rebateAndCommission,
                        ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                        LockInfo = lockInfo == null ? string.Empty :
                        lockInfo.Company == CurrentCompany.CompanyId ?
                        string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name) :
                        string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(), lockInfo.Account),
                        Status = ChinaPay.B3B.Service.Order.StatusService.GetOrderStatus(item.Status, OrderRole.Platform),
                        PurchaserName = string.IsNullOrEmpty(item.PurchaserName) ? string.Empty : item.PurchaserName,
                        ProviderName = string.IsNullOrEmpty(item.ProviderName) ? string.Empty : item.ProviderName,
                        SupplierName = string.IsNullOrEmpty(item.SupplierName) ? string.Empty : item.SupplierName,
                        Source = string.Format("{0}/{1}", item.ProductType.GetDescription(), item.Source.GetDescription()),
                        RenderSupperUnLock = lockInfo != null,
                        PayTime = startStatTime.HasValue ? startStatTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                        IsEmergentOrder = item.IsEmergentOrder,
                        ETDZTime = (startStatTime.HasValue && item.RefuseETDZTime.HasValue) ? Math.Round((item.RefuseETDZTime.Value - startStatTime.Value).TotalMinutes) : (startStatTime.HasValue) ? Math.Round(((item.ETDZTime ?? DateTime.Now) - startStatTime.Value).TotalMinutes) : 0.1,
                        RelationType = item.PurcharseProviderRelation == null ? "" : (item.PurcharseProviderRelation == RelationType.Brother ? "同行" : (item.PurcharseProviderRelation == RelationType.Interior ? "内部" : "下级")),
                        IsBrother = item.PurcharseProviderRelation != null && item.PurcharseProviderRelation == RelationType.Brother,
                        IsNull = item.PurcharseProviderRelation == null,
                        EnableQueryPaymentInfo = item.Status == OrderStatus.Ordered,
                        RemindContent = string.IsNullOrWhiteSpace(item.RemindContent) ? string.Empty : item.RemindContent,
                        RemindIsShow = item.IsNeedReminded,
                        IsOEM = item.OEMID.HasValue,
                        AllowPlatformContractPurchaser = item.AllowPlatformContractPurchaser
                    };
                })
            };
        }
        public object QueryOEMOrderList(Pagination pagination, OrderQueryCondition orderQueryCondition, Guid oem)
        {
            OrderQueryCondition condition = GetCondition(orderQueryCondition, oem == Guid.Empty ? OrderRole.OEMOwner : OrderRole.Platform);
            if (oem != Guid.Empty) condition.OEMID = oem;
            List<OrderListView> orders = OrderQueryService.QueryOrders(condition, pagination, false).ToList();
            var lockInfos = LockService.Query(orders.Select(form => form.OrderId.ToString())).ToList();
            return new
            {
                Pagination = pagination,
                Orders = orders.Select(item =>
                {
                    var lockInfo = lockInfos.FirstOrDefault(l => l.Key == item.OrderId.ToString());
                    var fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero();
                    var airportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero();
                    var BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero();
                    var settleAmount = item.SettlementForPurchaser.Amount.TrimInvaidZero();
                    var rebateAndCommission = string.Format("{0}%/{1}", (item.SettlementForPurchaser.Rebate * 100).TrimInvaidZero(),
                    item.SettlementForPurchaser.Commission.TrimInvaidZero());
                    DateTime? startStatTime = null;
                    if (CurrentCompany.CompanyType == CompanyType.Supplier)
                    {
                    }
                    else if (item.IsSpecial && item.Supplier.HasValue)
                    {
                        if (item.SupplyTime.HasValue && item.PayTime.HasValue)
                        {
                            startStatTime = item.SupplyTime.Value > item.PayTime.Value ? item.SupplyTime.Value : item.PayTime.Value;
                        }
                    }
                    else
                    {
                        startStatTime = item.PayTime;
                    }
                    return new
                    {
                        OrderId = item.OrderId,
                        PNR = item.ETDZPNR != null ? item.ETDZPNR.ToListString() : item.ReservationPNR == null ? string.Empty : item.ReservationPNR.ToListString(),
                        FlightInfo = string.Join("<br/>", item.Flights.Select(f => string.Format("{0}{1} {2} {3}{6}-{4}{7}<br/>{5}",
                            f.Carrier, f.FlightNo, f.Bunk, f.DepartureCity, f.ArrivalCity, f.TakeoffTime.ToString("yyyy-MM-dd HH:mm"), f.DepartureTeminal, f.ArrivalTeminal))),
                        Passengers = string.Join("<br/>", item.Passengers),
                        Price = fare + "</br>" + airportFee + "/" + BAF,
                        Commission = settleAmount + "<br/>" + rebateAndCommission,
                        ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd<br />HH:mm:ss"),
                        LockInfo = lockInfo == null ? string.Empty :
                        lockInfo.Company == CurrentCompany.CompanyId ?
                        string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name) :
                        lockInfo.LockRole.GetDescription(),
                        Status = ChinaPay.B3B.Service.Order.StatusService.GetOrderStatus(item.Status, OrderRole.OEMOwner),
                        PurchaserName = string.IsNullOrEmpty(item.PurchaserName) ? string.Empty : item.PurchaserName,
                        Source = string.Format("{0}/{1}", item.ProductType.GetDescription(), item.Source.GetDescription()),
                        RenderSupperUnLock = lockInfo != null,
                        PayTime = startStatTime.HasValue ? startStatTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                        IsEmergentOrder = item.IsEmergentOrder,
                        ETDZTime = (startStatTime.HasValue && item.RefuseETDZTime.HasValue) ? Math.Round((item.RefuseETDZTime.Value - startStatTime.Value).TotalMinutes) : (startStatTime.HasValue) ? Math.Round(((item.ETDZTime ?? DateTime.Now) - startStatTime.Value).TotalMinutes) : 0.1,
                        IsBrother = item.PurcharseProviderRelation != null && item.PurcharseProviderRelation == RelationType.Brother,
                        IsNull = item.PurcharseProviderRelation == null,
                        EnableQueryPaymentInfo = item.Status == OrderStatus.Ordered,
                        RemindContent = string.IsNullOrWhiteSpace(item.RemindContent) ? string.Empty : item.RemindContent,
                        RemindIsShow = item.IsNeedReminded
                    };
                })
            };
        }
        public object QueryPurchaseOrderList(Pagination pagination, OrderQueryCondition orderQueryCondition)
        {
            var orders = OrderQueryService.QueryOrders(GetCondition(orderQueryCondition, OrderRole.Purchaser), pagination).ToList().Select(item =>
            {
                var ISPNRImport = item.Source == OrderSource.CodeImport || item.Source == OrderSource.ContentImport || item.Source == OrderSource.InterfaceOrder;
                var IsShowPNR = ISPNRImport || (item.Status > OrderStatus.PaidForSupply && item.Status != OrderStatus.Canceled);
                var isShowAuthOffice = ISPNRImport && (item.Status > OrderStatus.PaidForSupply && item.Status != OrderStatus.Canceled) && item.NeedAUTH && item.Choise != AuthenticationChoise.NoAUTHandArgee;
                var fare = item.Flights.Sum(f => f.Fare).TrimInvaidZero();
                var airportFee = item.Flights.Sum(f => f.AirportFee).TrimInvaidZero();
                var BAF = item.Flights.Sum(f => f.BAF).TrimInvaidZero();
                var settleAmount = item.SettlementForPurchaser.Amount.TrimInvaidZero();
                var rebateAndCommission = item.ProductType == ProductType.Special ? string.Empty : string.Format("{0}%/{1}",
                    (item.SettlementForPurchaser.Rebate * 100).TrimInvaidZero(), item.SettlementForPurchaser.Commission.TrimInvaidZero());
                var pnr = item.ETDZPNR ?? item.ReservationPNR;
                return new
                {
                    OrderId = item.OrderId,
                    PNR = IsShowPNR && pnr != null ?
                    (string.Format("<span class='obvious'>{0}</span>", pnr.ToListString())
                    + (isShowAuthOffice
                    ? string.Format("<br/><a href='javascript:void(0);' class='tips_btn'>需授权</a><div class='tips_box hidden'><div class='tips_bd'><p>您已同意对供应商的Office号进行授权，请在黑屏内输入以下内容<br /><span class='obvious pad-r'>RMK TJ AUTH {0}</span><a href=\"javascript:copyToClipboard('RMK TJ AUTH {0}')\" class='obvious-a'>复制</a></p></div></div>", item.OfficeNo) : string.Empty))
                    : string.Empty,
                    FlightInfo = string.Join("<br/>", item.Flights.Select(f => string.Format("{0}{1} {2} {3}{6}-{4}{7}<br/>{5}",
                        f.Carrier, f.FlightNo, f.Bunk, f.DepartureCity, f.ArrivalCity, f.TakeoffTime.ToString("yyyy-MM-dd HH:mm"), f.DepartureTeminal, f.ArrivalTeminal))),
                    Passengers = string.Join("<br/>", item.Passengers),
                    Price = fare + "</br>" + airportFee + "/" + BAF,
                    Commission = settleAmount + "<br/>" + rebateAndCommission,
                    ProducedTime = item.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = Service.Order.StatusService.GetOrderStatus(item.Status, OrderRole.Purchaser),
                    item.ProducedAccountName,
                    PayTime = item.PayTime.HasValue ? item.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    ETDZTime = (item.PayTime.HasValue && item.RefuseETDZTime.HasValue) ? Math.Round((item.RefuseETDZTime.Value - item.PayTime.Value).TotalMinutes) : (item.PayTime.HasValue) ? Math.Round(((item.ETDZTime ?? DateTime.Now) - item.PayTime.Value).TotalMinutes) : 0.1,
                    PassengerMsgSended = item.PassengerMsgSended,
                    IsNeedRemind = item.Status == OrderStatus.PaidForETDZ || item.Status == OrderStatus.PaidForSupply ? (item.RemindTime.HasValue ?
                                    Math.Round((DateTime.Now - item.RemindTime.Value).TotalMinutes) - ChinaPay.B3B.Service.SystemManagement.SystemParamService.ReminderCycle >= 0 :
                                    (item.PayTime.HasValue ? Math.Round((DateTime.Now - item.PayTime.Value).TotalMinutes) : 0.1) -
                                    ChinaPay.B3B.Service.SystemManagement.SystemParamService.PurchaseReminderTime >= 0) : false
                };
            });
            return new
            {
                Pagination = pagination,
                Orders = orders
            };
        }

        public string UnLockData(decimal orderId)
        {
            LockService.UnLockForcibly(orderId.ToString());
            return "OK";
        }

        public object SearchItinerary(string ticketNo)
        {
            var order = OrderQueryService.QueryOrderByTicketNo(ticketNo);
            if (order == null) throw new CustomException("未搜索到票号匹配的订单");
            if (order.Purchaser == null) throw new CustomException("未搜索到票号匹配的订单");
            if (order.Purchaser.CompanyId != CurrentCompany.CompanyId) throw new CustomException("无权限提取此票号行程告知单");
            Passenger passenger = null;
            IEnumerable<Flight> flights = null;
            foreach (PNRInfo pnrInfo in order.PNRInfos)
            {
                if (pnrInfo.Passengers.Any(p => p.Tickets.Any(t => t.No == ticketNo)))
                {
                    passenger = pnrInfo.Passengers.First(p => p.Tickets.Any(t => t.No == ticketNo));
                    flights = pnrInfo.Flights;
                    break;
                }
            }
            if (passenger == null) throw new CustomException("未搜索到票号匹配的订单");
            if (flights == null || !flights.Any()) throw new CustomException("预订的航班已经被取消");
            return new
            {
                Name = passenger.Name,
                Credentials = passenger.Credentials,
                TicketNos = FromatTicket(passenger.Tickets),
                TicketPrice = flights.Sum(f => f.Price.Fare),
                AirPortFee = flights.Sum(f => f.Price.AirportFee),
                BAF = flights.Sum(f => f.Price.BAF),
                inputer = CurrentCompany.CompanyName,
                FillDate = DateTime.Now.ToString("yyyy-MM-dd"),
                Flights = flights.Select(f => new
                {
                    Voyage = string.Format("{0}/{1}", f.Departure.City, f.Arrival.City),
                    Carrier = f.Carrier.Name,
                    FlightNo = f.FlightNo,
                    BunkType = f.Bunk.Type.GetDescription(),
                    BunkCode = f.Bunk.Code,
                    TakeoffTime = f.TakeoffTime.ToString("yyyy-MM-dd HH:mm"),
                    ArrivalTime = f.LandingTime.ToString("yyyy-MM-dd HH:mm"),
                    ValidPeriod = order.ETDZTime.HasValue ? string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", order.ETDZTime.Value, order.ETDZTime.Value.AddYears(1).AddDays(-1)) : string.Empty,
                    XingLing = "20K"
                })
            };
        }
        public object SearchPassengerName(decimal orderId)
        {
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null || order.Purchaser == null) throw new CustomException("未搜索到票号匹配的订单");
            if (order.Purchaser.CompanyId != CurrentCompany.CompanyId) throw new CustomException("无权限提取此票号行程告知单");
            if (order.Status != OrderStatus.Finished) throw new CustomException("该订单还未出票");
            List<Passenger> passengers = new List<Passenger>();
            foreach (var pnr in order.PNRInfos)
            {
                passengers.AddRange(pnr.Passengers);
            }
            return passengers.Select(item => item.Name);
        }
        public object SearchItineraryByOrder(string orderId, string passengerName)
        {
            var id = orderId.ToDecimal();
            if (id == 0) throw new CustomException("订单号错误");
            var order = OrderQueryService.QueryOrder(id);
            if (order == null) throw new CustomException("未搜索到票号匹配的订单");
            if (order.Purchaser.CompanyId != CurrentCompany.CompanyId) throw new CustomException("无权限提取此票号行程告知单");
            if (order.Status != OrderStatus.Finished) throw new CustomException("该订单还未出票");
            Passenger passenger = null;
            IEnumerable<Flight> flights = null;
            foreach (PNRInfo pnrInfo in order.PNRInfos)
            {
                passenger = pnrInfo.Passengers.ToList().FirstOrDefault(p => p.Name == passengerName.Trim());
                flights = pnrInfo.Flights;
                break;
            }
            if (passenger == null) throw new CustomException("未搜索到票号匹配的订单");
            if (flights == null || !flights.Any()) throw new CustomException("预订的航班已经被取消");
            return new
            {
                Name = passenger.Name,
                Credentials = passenger.Credentials,
                TicketNos = FromatTicket(passenger.Tickets),
                TicketPrice = flights.Sum(f => f.Price.Fare),
                AirPortFee = flights.Sum(f => f.Price.AirportFee),
                BAF = flights.Sum(f => f.Price.BAF),
                inputer = CurrentCompany.CompanyName,
                FillDate = DateTime.Now.ToString("yyyy-MM-dd"),
                Flights = flights.Select(f => new
                {
                    Voyage = string.Format("{0}/{1}", f.Departure.City, f.Arrival.City),
                    Carrier = f.Carrier.Name,
                    FlightNo = f.FlightNo,
                    BunkType = order.IsSpecial ? string.Empty : f.Bunk.Type.GetDescription(),
                    BunkCode = f.Bunk.Code,
                    TakeoffTime = f.TakeoffTime.ToString("yyyy-MM-dd HH:mm"),
                    ArrivalTime = f.LandingTime.ToString("yyyy-MM-dd HH:mm"),
                    ValidPeriod = order.ETDZTime.HasValue ? string.Format("{0:yyyy/MM/dd}-{1:yyyy/MM/dd}", order.ETDZTime.Value, order.ETDZTime.Value.AddYears(1).AddDays(-1)) : string.Empty,
                    XingLing = "20K"
                })
            };
        }
        public string GetServerTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        private string FromatTicket(IEnumerable<Ticket> tickets)
        {
            var firstTicket = tickets.First();
            return string.Format("{0}-{1}{2}", firstTicket.SettleCode, firstTicket.No, tickets.Count() > 1 ? "-" + tickets.Last().No.Substring(8) : string.Empty);
        }

        /// <summary>
        /// 采购商发送出票成功短信模版
        /// </summary>
        /// <param name="orderId">订单号</param>
        public object QueryTemplate(decimal orderId)
        {
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null) throw new CustomException("未找到匹配的订单");
            var flightView = new List<ChinaPay.SMS.Service.Templete.FlightView>();
            var ticketNos = new List<string>();
            var passengers = new List<string>();
            var firstPassengerPhone = "";
            var isNeedSend = false;
            var isSended = order.PassengerMsgSended;
            foreach (var pnrInfo in order.PNRInfos)
            {
                if (pnrInfo.Passengers.Any())
                {
                    isNeedSend = true;
                    firstPassengerPhone = pnrInfo.Passengers.FirstOrDefault().Phone;
                    break;
                }
            }
            foreach (var item in order.PNRInfos)
            {
                foreach (var flight in item.Flights)
                {
                    flightView.Add(new SMS.Service.Templete.FlightView()
                        {
                            Airline = flight.Carrier.Code,
                            Arrival = flight.Arrival.City,
                            Bunk = flight.Bunk.Code,
                            Departure = flight.Departure.City,
                            DepartureAirline = flight.Departure.Name,
                            FlightNo = flight.FlightNo,
                            TakeoffTime = flight.TakeoffTime,
                            Terminal = string.IsNullOrWhiteSpace(flight.DepartureTerminal) ? string.Empty : flight.DepartureTerminal + "航站楼",
                        });
                }
                foreach (var passenger in item.Passengers)
                {
                    passengers.Add(passenger.Name);
                    foreach (var ticket in passenger.Tickets)
                    {
                        ticketNos.Add(ticket.SettleCode + "-" + ticket.No);
                    }
                }
            }
            var template = "";
            if (flightView.Count > 0 && passengers.Count > 0 && ticketNos.Count > 0 && order.Purchaser != null && order.Purchaser.Company != null && !string.IsNullOrWhiteSpace(order.Purchaser.Company.ContactPhone))
            {
                template = SMSSendService.GetDefaultTemlete(flightView,
                     passengers,
                     ticketNos, order.Purchaser.Company.ContactPhone);
            }
            return new
            {
                Template = template,
                FirstPassengerPhone = firstPassengerPhone,
                IsNeedSend = isNeedSend,
                IsSended = isSended
            };


        }
        /// <summary>
        /// 采购方发送短信
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="phone">电话号码</param>
        /// <param name="content">发送内容</param>
        public void PurchaseSendMessage(decimal orderId, string phone, string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                    throw new CustomException("电话号码不能为空");
                if (string.IsNullOrWhiteSpace(content))
                    throw new CustomException("内容不能为空");
                var acc = from item in AccountService.Query(CurrentCompany.CompanyId)
                          where item.Type == Common.Enums.AccountType.Payment
                          select new
                          {
                              No = item.No
                          };
                SMSSendService.SendCustomMessage(
                    new ChinaPay.SMS.Service.Domain.Account(CurrentCompany.CompanyId, this.CurrentUser.UserName),
                    phone.Split(','), content);
                OrderProcessService.SendMessageToPassenger(orderId);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        /// <summary>
        /// 获取手动支付地址
        /// </summary>
        /// <param name="platformText">平台文本</param>
        /// <param name="payInterfaceValue">手动支付方式</param>
        /// <param name="internalOrderId">内部订单Id</param>
        /// <param name="externalOrderId">外部订单Id</param>
        /// <param name="orderAmount">订单金额</param>
        /// <returns></returns>
        public string QueryManualPayUrl(string platformText, string payInterfaceValue, string internalOrderId, string externalOrderId, string orderAmount)
        {
            var result = "";
            var platformType = Enum.GetValues(typeof(PlatformType)) as PlatformType[];
            var newPlatform = PlatformType.Yeexing;
            foreach (var item in platformType)
            {
                if (item.GetDescription() == platformText)
                    newPlatform = item;
            }
            var url = OrderService.GetPayUrl(newPlatform,
                    decimal.Parse(internalOrderId),
                    externalOrderId,
                   (DataTransferObject.Common.PayInterface)int.Parse(payInterfaceValue),
                    decimal.Parse(orderAmount));
            if (url.Success)
            {
                result = url.Result;
            }
            return result;
        }
        /// <summary>
        /// 调用自动支付
        /// </summary>
        /// <param name="orderId"></param>
        public object AutoPayExternalOrderId(decimal orderId)
        {
            var order = OrderProcessService.AutoPayExternalOrder(orderId);
            return new
            {
                PayStatusValue = order.PayStatus,
                PayStatusText = order.PayStatus.GetDescription(),
                PayTime = order.PayTime,
                PayTradeNo = order.PayTradNO,
                Reason = order.FaildInfo
            };
        }

        /// <summary>
        /// 查询订单支付状态，补单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string QueryPaymentInfo(decimal orderId)
        {
            bool payResult = false;
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null)
            {
                return "订单不存在";
            }
            if (order.Status != OrderStatus.Ordered || order.Status == OrderStatus.Canceled)
            {
                return "OK";
            }
            try
            {
#if(DEBUG)
                var tradeResult = AccountTradeService.PayTradeQuery(orderId.ToString());
                if (tradeResult == null)
                {
                    return "查询支付信息失败";
                }
                if (tradeResult.Status != PayStatus.PaySuccess)
                {
                    return "没有查询到支付成功信息";
                }
                var paramArray = tradeResult.CustomParameter.Split('|');
                if (paramArray.Length >= 3)
                {
                    var operatorAccount = paramArray[2];
                    ChinaPay.B3B.DataTransferObject.Common.PayInterface payInterface = ChinaPay.B3B.Service.Tradement.NotifyService.ParsePayInterface(tradeResult.payInterface);
                    ChinaPay.B3B.DataTransferObject.Common.PayAccountType payAccountType = ChinaPay.B3B.Service.Tradement.NotifyService.ParsePayAccountType(tradeResult.payAccountType);
                    OrderProcessService.PaySuccess(
                        order.Id,
                        tradeResult.AccountNo,
                        tradeResult.TradeNo,
                        tradeResult.FillChargeId.ToString(),//channelTradeNo
                        tradeResult.PayTime.Value,
                        payInterface,
                        payAccountType,
                        operatorAccount);
                    payResult = true;
                }
                LogService.SaveTradementLog(new TradementLog
                {
                    OrderId = orderId,
                    Type = TradementBusinessType.Pay,
                    Request = string.Format("{0}进行补单,订单支付金额{1:0.00}", CurrentUser.UserName, order.Purchaser.Amount),
                    Response = "补单" + (payResult ? "成功" : "失败"),
                    Time = DateTime.Now,
                    Remark = tradeResult.CustomParameter
                });          
#else
                QueryPaymentProcess queryPaymentProcess = new QueryPaymentProcess(orderId.ToString());
                if (queryPaymentProcess.Execute() && queryPaymentProcess.PaySuccess)
                {
                    var paramArray = queryPaymentProcess.ExtraParams.Split('|');
                    if (paramArray.Length >= 3)
                    {
                        var operatorAccount = paramArray[2];
                        OrderProcessService.PaySuccess(
                            order.Id,
                            queryPaymentProcess.PayAccount,
                            queryPaymentProcess.PayTradeNo,
                            queryPaymentProcess.ChannelTradeNo,
                            queryPaymentProcess.PayTime,
                            queryPaymentProcess.PayChannel,
                            queryPaymentProcess.PayAccountType,
                            operatorAccount);
                        payResult = true;
                    }
                    LogService.SaveTradementLog(new TradementLog
                    {
                        OrderId = orderId,
                        Type = TradementBusinessType.Pay,
                        Request = string.Format("{0}进行补单,订单支付金额{1:0.00}", CurrentUser.UserName, order.Purchaser.Amount),
                        Response = "补单" + (payResult ? "成功" : "失败"),
                        Time = DateTime.Now,
                        Remark = queryPaymentProcess.ExtraParams
                    });
                }
                else
                {
                    return "没有查询到支付成功信息";
                }
#endif
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex, "补单");
            }
            return payResult ? "OK" : "ERROR";
        }

        /// <summary>
        /// 采购发送给乘机人的航班变更默板
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="originalFlightNo">原航班号</param>
        /// <param name="originalTakeoffTime">原航班起飞时间</param>
        /// <param name="transferTypeValue">变更类型的值</param>
        /// <param name="newFlightNo">新航班号</param>
        /// <param name="newTakeOffTimeText">新起飞时间</param>
        /// <param name="purchasePhone">采购电话号码</param>
        public object QueryFlightTransferTemplate(decimal orderId, string originalFlightNo, DateTime originalTakeoffTime, int transferTypeValue, string newFlightNo, string newTakeOffTimeText, string purchasePhone)
        {
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null) throw new CustomException("未找到匹配的订单");
            var flightView = new List<ChinaPay.SMS.Service.Templete.FlightView>();
            var firstPassengerPhone = "";
            var firstPassengerName = "";
            var isNeedSend = false;
            DateTime? newTakeOffTime = null;
            if (!string.IsNullOrWhiteSpace(newTakeOffTimeText))
                newTakeOffTime = DateTime.Parse(newTakeOffTimeText);
            foreach (var pnrInfo in order.PNRInfos)
            {
                if (pnrInfo.Passengers.Any())
                {
                    isNeedSend = true;
                    firstPassengerName = pnrInfo.Passengers.FirstOrDefault().Name;
                    firstPassengerPhone = pnrInfo.Passengers.FirstOrDefault().Phone;
                    break;
                }

            }
            foreach (var item in order.PNRInfos)
            {
                foreach (var flight in item.Flights)
                {
                    if (flight.FlightNo == originalFlightNo && flight.TakeoffTime == originalTakeoffTime)
                    {
                        flightView.Add(new SMS.Service.Templete.FlightView()
                        {
                            Airline = flight.Carrier.Code,
                            Arrival = flight.Arrival.City,
                            Bunk = flight.Bunk.Code,
                            Departure = flight.Departure.City,
                            DepartureAirline = flight.Departure.Name,
                            FlightNo = flight.FlightNo,
                            TakeoffTime = flight.TakeoffTime,
                            Terminal = string.IsNullOrWhiteSpace(flight.DepartureTerminal) ? string.Empty : flight.DepartureTerminal + "航站楼",
                        });
                    }
                }
            }
            var template = "";
            if (transferTypeValue == (byte)TransferType.Change)
            {
                template = SMSSendService.GetFlightChangeTemplteByPurchase(firstPassengerName, flightView, newTakeOffTime.Value, newFlightNo, purchasePhone);
            }
            if (transferTypeValue == (byte)TransferType.Delay)
            {
                template = SMSSendService.GetFlightDelayTemplteByPurchase(firstPassengerName, flightView, newTakeOffTime.Value, purchasePhone);
            }
            if (transferTypeValue == (byte)TransferType.Canceled)
            {
                template = SMSSendService.GetFlightCancelTemplteByPurchase(firstPassengerName, flightView, purchasePhone);
            }
            return new
            {
                Template = template,
                FirstPassengerPhone = firstPassengerPhone,
                IsNeedSend = isNeedSend,
            };
        }

        public void SendFlightTranferByPurchase(Guid transferId, string phone, string content)
        {
            try
            {
                var acc = from item in AccountService.Query(CurrentCompany.CompanyId)
                          where item.Type == Common.Enums.AccountType.Payment
                          select new
                          {
                              No = item.No
                          };
                SMSSendService.SendCustomMessage(
                    new ChinaPay.SMS.Service.Domain.Account(CurrentCompany.CompanyId, this.CurrentUser.UserName),
                    phone.Split(','), content);
                QSService.UpdatePassengerMsgSended(transferId);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        /// <summary>
        /// 采购催单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="remindContent">提醒内容</param>
        public void Reminded(decimal orderId, string remindContent, string operatorAccount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(remindContent))
                    throw new CustomException("催单内容或备注不能为空");
                OrderProcessService.Reminded(orderId, remindContent);
                var logContent = "催单内容或备注:" + remindContent;
                var log = new ChinaPay.B3B.Service.Log.Domain.OrderLog()
                {
                    OrderId = orderId,
                    Keyword = "采购催单",
                    Content = logContent,
                    Company = this.CurrentCompany.CompanyId,
                    Account = operatorAccount,
                    Role = OperatorRole.Purchaser,
                    VisibleRole = OrderRole.Provider | OrderRole.Supplier | OrderRole.Purchaser | OrderRole.Platform | OrderRole.OEMOwner
                };
                LogService.SaveOrderLog(log);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
                throw;
            }
        }

        public object MatchedPolicy(decimal orderId, string pnr)
        {
            var order = OrderQueryService.QueryOrder(orderId);
            if (order == null) throw new NotFoundException(orderId.ToString(), "订单不存在");
            if (order.IsThirdRelation)
            {
                var matchedPolicys = OrderProcessService.matchPolicys(order, pnr, BasePage.OwnerOEMId);
                Session["confirmedOrder"] = order;
                Session["matchedPolicy"] = matchedPolicys;
                var ParValue = OrderProcessService.getReleasedFare(order, order.PNRInfos.First().Passengers.First());
                return new
                {
                    Bunk = order.PNRInfos.First().Flights.First().Bunk.Code,
                    Policys = from item in matchedPolicys
                              let generalPolicy = item.OriginalPolicy as IGeneralPolicy
                              let regulation = item.OriginalPolicy as IHasRegulation
                              select new
                              {
                                  Fare = ParValue.TrimInvaidZero(),
                                  Rebate = (item.Commission * 100).TrimInvaidZero() + "%",
                                  dRebate = item.Commission,
                                  Commission = (ParValue * item.Commission).TrimInvaidZero(),
                                  SettleAmount = (ParValue * (1 - item.Commission)).TrimInvaidZero(),
                                  WorkingTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                  ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                                  ETDZEfficiency = (item.Speed.ETDZ / 60) + "分 ",
                                  RefundEfficiency = (item.Speed.Refund / 60) + "分钟",
                                  TicketType = (item.OriginalPolicy == null ?
                                                   TicketType.BSP : item.OriginalPolicy.TicketType).ToString(),
                                  PolicyId = item.Id,
                                  PolicyOwner = item.Provider,
                                  PolicyType = (int)item.PolicyType,
                                  OfficeNo = item.OriginalPolicy == null ? item.OfficeNumber : item.OriginalPolicy.OfficeCode,

                                  NeedAUTH = item.OriginalPolicy == null ? item.NeedAUTH : item.OriginalPolicy.NeedAUTH,
                                  PolicyTypes = item.PolicyType.GetDescription(),
                                  IsBusy = OrderRemindService.QueryProviderRemindInfo(item.Provider).ETDZ > 5,
                                  item.HasSubsidized,
                                  RelationType = (int)item.RelationType
                              }
                };
            }
            return new
            {
                Bunk = string.Empty,
                Policys = new List<int>()
            };
        }

        private string getTimeRange(Izual.Time start, Izual.Time end)
        {
            return start.ToString("HH:mm") + "-" + end.ToString("HH:mm");
        }

        public object QueryPaidforETDZOrder(decimal orderId)
        {
            string lockErrorMsg = "";
            Service.Order.Domain.Order order = null;
            IEnumerable<string> officeNo = null;
            IList<ETDZPassengerView> Passengers = null;
            string currentOfficeNO = "";
            var requireChangePNR = false;
            string ticketType = "";
            string BPNRCode = "";
            string PNRCode = "";
            bool isLocked = BasePage.Lock(orderId, Service.Locker.LockRole.Provider, "出票", out lockErrorMsg);
            if (isLocked)
            {
                order = Service.OrderQueryService.QueryOrder(orderId);

                var commonProductInfo = order.Provider.Product as CommonProductInfo;
                if (commonProductInfo != null)
                {
                    ticketType = commonProductInfo.TicketType.GetDescription();
                    requireChangePNR = commonProductInfo.RequireChangePNR;
                }
                currentOfficeNO = order.IsThirdRelation ? order.Supplier.Product.OfficeNo : order.Provider.Product.OfficeNo;
                officeNo = CompanyService.QueryOfficeNumbers(CurrentCompany.CompanyId).Select(o => o.Number);

                var pnrInfo = order.PNRInfos.First();
                Passengers = new List<ETDZPassengerView>();
                foreach (var item in pnrInfo.Passengers)
                {
                    Passengers.Add(new ETDZPassengerView
                    {
                        Name = item.Name,
                        PassengerType = item.PassengerType.GetDescription(),
                        SettleCode = item.Tickets.First().SettleCode,
                        TicketNoCount = item.Tickets.Count()
                    });
                }

                BPNRCode = pnrInfo.Code != null ? pnrInfo.Code.BPNR : string.Empty;
                PNRCode = pnrInfo.Code != null ? pnrInfo.Code.PNR : string.Empty;
                return new
                {
                    LockErrorMsg = lockErrorMsg,
                    IsLocked = isLocked,
                    OfficeNo = officeNo,
                    CurrentOfficeNO = currentOfficeNO,
                    RequireChangePNR = requireChangePNR,
                    Passengers = Passengers,
                    TicketType = ticketType,
                    BPNRCode = BPNRCode,
                    PNRCode = PNRCode,
                    ForbidChangPNR = order.ForbidChangPNR
                };
            }
            else
            {
                lockErrorMsg = "锁定订单失败。原因:" + lockErrorMsg;
                return new
                {
                    LockErrorMsg = lockErrorMsg,
                    IsLocked = isLocked
                };
            }
            
        }

        public bool UnLock(string orderId)
        {
            return Service.LockService.UnLock(orderId, CurrentUser.UserName);
        }

        /// <summary>
        /// 锁定订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="reason"> </param>
        /// <returns></returns>
        public string LockOrder(decimal orderId,string reason) {
            string lockErrorMsg;
            bool isLocked = BasePage.Lock(orderId, LockRole.Purchaser, reason, out lockErrorMsg);
            return isLocked ? string.Empty : lockErrorMsg;
        }

        public string QueryEmergentOrder(decimal id)
        {
            var emergentOrder = CoordinationService.QueryEmergentOrder(id);
            return string.Format("<div class='tips_box'><div class='tips_bd'><p>{0}</p></div></div>", emergentOrder != null ? emergentOrder.Content : string.Empty);
        }
    }
}