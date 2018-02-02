using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.Gateway.Tradement;
using ChinaPay.PoolPay.Service;
using LogService = ChinaPay.B3B.Service.LogService;
using PayStatus = PoolPay.DataTransferObject.PayStatus;

namespace ChinaPay.B3B.TransactionWeb.OrderHandlers
{
    /// <summary>
    /// Applyform 的摘要说明
    /// </summary>
    public class Applyform : BaseHandler
    {
        public object PurchaseQueryApplyform(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(OrderRole.Purchaser).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                else if (condition.ApplyformType == ApplyformType.Postpone)
                {
                    if (!string.IsNullOrWhiteSpace(condition.PostponeStatusText) && condition.PostponeStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRolePostponeApplyformStatus(OrderRole.Purchaser).Where(s => s.Value == condition.PostponeStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                condition.Purchaser = CurrentCompany.CompanyId;
                var applyform = from item in Service.ApplyformQueryService.QueryApplyforms(condition, pagination)
                                select new
                                                 {
                                                     ApplyformId = item.ApplyformId,
                                                     ProductType = item.ProductType.GetDescription(),
                                                     PNR = item.OriginalPNR.ToListString(),
                                                     Voyage = item.Flights.Join("<br />", f => string.Format("{0}-{1}", f.DepartureCity, f.ArrivalCity)),
                                                     FlightInfo = item.Flights.Join("<br />", f => string.Format("{0}{1}<br />{2} / {3}", f.Carrier, f.FlightNo, string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk, getDiscountText(f.Discount))),
                                                     TakeoffTime = item.Flights.Join("<br />", f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                                                     Passenger = item.Passengers.Join("<br />"),
                                                     ApplyType = item.ApplyformType,
                                                     ApplyTypeText = item.ApplyformType.GetDescription(),
                                                     Status = GetProcessStatus(item,OrderRole.Purchaser),
                                                     Applier = item.ApplierAccount,
                                                     AppliedTime = item.AppliedTime.ToString("yyyy-MM-dd<br />HH:mm:ss")
                                                 };
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }

        public object ProviderQueryApplyform(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                if (this.CurrentCompany.CompanyType == CompanyType.Provider) condition.Provider = CurrentCompany.CompanyId;
                else condition.Supplier = CurrentCompany.CompanyId;
                var applyform = Service.ApplyformQueryService.ProviderQueryApplyforms(condition, pagination).Select(form => new
                {
                    form.ApplyformId,
                    PNR = form.OriginalPNR == null ? string.Empty : form.OriginalPNR.ToListString(),
                    Voyage = form.Flights.Join("<br />",
                    f => string.Format("{0}-{1}", f.DepartureCity, f.ArrivalCity)),
                    FlightInfo = form.Flights.Join("<br />",
                      f => string.Format(
                                    "{0}{1}<br />{2} / {3}",
                                    f.Carrier,
                                    f.FlightNo,
                                    string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk,
                                    getDiscountText
                                        (f.Discount))),
                    TakeoffTime = form.Flights.Join("<br />",
               f => f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                    Passenger = form.Passengers.Join("<br />"),
                    ApplyType = form.ApplyformType.GetDescription(),
                    form.ApplyformType,
                    ProcessStatus = GetProcessStatus(form,
                        CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier),
                    AppliedTime = form.AppliedTime.ToString("yyyy-MM-dd<br />HH:mm"),
                    form.ApplierAccount,
                    ProductType = form.ProductType.GetDescription(),
                    NeedProcess = form.ApplyformType == ApplyformType.BlanceRefund && ((BalanceRefundProcessStatus)form.ApplyDetailStatus == BalanceRefundProcessStatus.AppliedForProvider || (BalanceRefundProcessStatus)form.ApplyDetailStatus==BalanceRefundProcessStatus.DeniedByProviderTreasurer) 
                });
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }

        public object ProvideProcessApplyform(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                condition.Provider = CurrentCompany.CompanyId;

                var forms = Service.ApplyformQueryService.ProviderQueryApplyformsForProcess(condition, pagination).ToList();
                var lockInfos = LockService.Query(forms.Select(form => form.ApplyformId.ToString())).ToList();
                var applyform = forms.Select(form =>
                 {
                     LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == form.ApplyformId.ToString());
                     return new
                     {
                         form.ApplyformId,
                         PNR = form.OriginalPNR.ToListString(),
                         Voyage = form.Flights.Join("<br />",
                             f =>
                             string.Format(
                                 "{0}-{1}",
                                 f.DepartureCity,
                                 f.ArrivalCity)),
                         FlightInfo = form.Flights.Join("<br />",
                             f => string.Format(
                                 "{0}{1}<br />{2} / {3}",
                                 f.Carrier,
                                 f.FlightNo,
                                 string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk,
                                 getDiscountText
                                      (f.
                                      Discount))),
                         TakeoffTime = form.Flights.Join("<br />",
                             f =>
                             f.TakeoffTime.ToString(
                                 "yyyy-MM-dd<br />HH:mm")),
                         Passengers = string.Join("<br />", form.Passengers),
                         ApplyType = form.ApplyformType.GetDescription(),
                         ApplyTypeValue = form.ApplyformType,
                         ProcessStatus = form.ApplyformType == ApplyformType.Postpone ?
                        Service.Order.StatusService.GetPostponeApplyformStatus((PostponeApplyformStatus)form.ApplyDetailStatus,
                      this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier) :
                     Service.Order.StatusService.GetRefundApplyformStatus((RefundApplyformStatus)form.ApplyDetailStatus,
                       this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier),
                         PStatus = form.ProcessStatus,
                         AppliedTime =
                             form.AppliedTime.ToString("yyyy-MM-dd<br />HH:mm"),
                         form.ApplierAccount,
                         ProductType = form.ProductType.GetDescription(),
                         form.ApplyformType,
                         RefundType = form.RefundType.HasValue ? form.RefundType.GetDescription() : "",
                         LockInfo = lockInfo == null
                                        ? string.Empty
                                        : lockInfo.Company ==
                                          CurrentCompany.CompanyId
                                              ? string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name)
                                              : string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(), lockInfo.Account)
                     };
                 });
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }

        public object ProvideProcessApplyformNew(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                condition.Provider = CurrentCompany.CompanyId;

                var forms = Service.ApplyformQueryService.ProviderQueryApplyformsForProcessNew(condition, pagination).ToList();
                var lockInfos = LockService.Query(forms.Select(form => form.ApplyformId.ToString())).ToList();
                var date = DateTime.Now.ToString("yyMMdd");
                var applyform = forms.Select(form =>
                {
                    LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == form.ApplyformId.ToString());
                    return new
                    {
                        form.ApplyformId,
                        PNR = form.OriginalPNR.ToListString(),
                        Voyage = form.Flights.Join("<br />",
                            f =>
                            string.Format(
                                "{0}-{1}",
                                f.DepartureCity,
                                f.ArrivalCity)),
                        FlightInfo = form.Flights.Join("<br />",
                            f => string.Format(
                                "{0}{1}<br />{2} / {3}",
                                f.Carrier,
                                f.FlightNo,
                                string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk,
                                getDiscountText
                                     (f.
                                     Discount))),
                        TakeoffTime = form.Flights.Join("<br />",
                            f =>
                            f.TakeoffTime.ToString(
                                "yyyy-MM-dd<br />HH:mm")),
                        TakeoffTimeIsToday = form.Flights.Join("|", f => f.TakeoffTime.ToString("yyMMdd") == date ? "1" : "0"),
                        Passengers = string.Join("<br />", form.Passengers),
                        ApplyType = form.ApplyformType.GetDescription(),
                        ApplyTypeValue = form.ApplyformType,
                        ProcessStatus = form.ApplyformType == ApplyformType.Postpone ?
                       Service.Order.StatusService.GetPostponeApplyformStatus((PostponeApplyformStatus)form.ApplyDetailStatus,
                     this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier) :
                    Service.Order.StatusService.GetRefundApplyformStatus((RefundApplyformStatus)form.ApplyDetailStatus,
                      this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier),
                        PStatus = form.ProcessStatus,
                        AppliedTime =
                            form.AppliedTime.ToString("yyyy-MM-dd<br />HH:mm"),
                        form.ApplierAccount,
                        ProductType = form.ProductType.GetDescription(),
                        form.ApplyformType,
                        RefundType = form.RefundType.HasValue ? form.RefundType.GetDescription() : "",
                        LockInfo = lockInfo == null
                                       ? string.Empty
                                       : lockInfo.Company ==
                                         CurrentCompany.CompanyId
                                             ? string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name)
                                             : string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(), lockInfo.Account)
                    };
                });
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }

        public object PlatformQueryApplyform(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(OrderRole.Platform).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                else if (condition.ApplyformType == ApplyformType.Postpone)
                {
                    if (!string.IsNullOrWhiteSpace(condition.PostponeStatusText) && condition.PostponeStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRolePostponeApplyformStatus(OrderRole.Platform).Where(s => s.Value == condition.PostponeStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                List<ApplyformListView> forms = ApplyformQueryService.QueryApplyforms(condition, pagination).ToList();
                var lockInfos = LockService.Query(forms.Select(form => form.ApplyformId.ToString())).ToList();
                var applyform = forms.Select(form =>
                                                     {
                                                         LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == form.ApplyformId.ToString());
                                                         return new
                                                             {
                                                                 form.ApplyformId,
                                                                 PNR = form.OriginalPNR == null ? string.Empty : form.OriginalPNR.ToListString(),
                                                                 Voyage = IEnumerableExtension.Join<FlightListView>(form.Flights, "<br />",
                                                                     f =>
                                                                     string.Format(
                                                                         "{0}{1}-{2}{3}",
                                                                         f.DepartureCity,
                                                                         f.DepartureAirport,
                                                                         f.ArrivalCity,
                                                                         f.ArrivalAirport)),
                                                                 FlightInfo = form.Flights.Join("<br />", f => string.Format(
                                                                     "{0}{1}<br />{2} / {3}",
                                                                     f.Carrier,
                                                                     f.FlightNo,
                                                                     string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk, getDiscountText
                                                                                                                 (f.Discount))),
                                                                 TakeoffTime = form.Flights.Join("<br />", f =>
                                                                                                         f.TakeoffTime.
                                                                                                             ToString(
                                                                                                                 "yyyy-MM-dd<br />HH:mm")),
                                                                 Passengers = String.Join("<br />", (IEnumerable<string>)form.Passengers),
                                                                 ApplyTypeDesc = form.ApplyformType.GetDescription(),
                                                                 ApplyType = form.ApplyformType,
                                                                 ProcessStatus = GetProcessStatus(form,OrderRole.Platform),
                                                                 form.AppliedTime,
                                                                 form.ApplierAccount,
                                                                 ProductType = form.ProductType.GetDescription(),
                                                                 LockInfo = lockInfo == null
                                                                                ? string.Empty
                                                                                : lockInfo.Company ==
                                                                                  CurrentCompany.CompanyId
                                                                                      ? string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name)
                                                                                      : string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(), lockInfo.Account),
                                                                 RenderSupperUnLock = lockInfo != null,
                                                                 RefundType = form.RefundType.HasValue ? form.RefundType.GetDescription() : string.Empty,
                                                                 IsRefund = form.ApplyformType == ApplyformType.Refund,
                                                                 EnableQueryPaymentInfo = form.ApplyformType == ApplyformType.Postpone && (PostponeApplyformStatus)form.ApplyDetailStatus == PostponeApplyformStatus.Agreed,
                                                                 NeedProcess = form.ApplyformType == ApplyformType.BlanceRefund && ((BalanceRefundProcessStatus)form.ApplyDetailStatus == BalanceRefundProcessStatus.AppliedForPlatform || (BalanceRefundProcessStatus)form.ApplyDetailStatus == BalanceRefundProcessStatus.DeniedByProviderBusiness) 
                                                             };
                                                     });
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }

        public object PlatformProcessApplyform(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(OrderRole.Platform).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                else if (condition.ApplyformType == ApplyformType.Postpone)
                {
                    if (!string.IsNullOrWhiteSpace(condition.PostponeStatusText) && condition.PostponeStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRolePostponeApplyformStatus(OrderRole.Platform).Where(s => s.Value == condition.PostponeStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                List<ApplyformListView> forms = ApplyformQueryService.PlatformQueryApplyformsForProcess(condition, pagination).ToList();
                var lockInfos = LockService.Query(forms.Select(form => form.ApplyformId.ToString())).ToList();
                var applyform = forms.Select(form =>
                {
                    LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == form.ApplyformId.ToString());
                    return new
                    {
                        form.ApplyformId,
                        PNR = form.OriginalPNR == null ? string.Empty : form.OriginalPNR.ToListString(),
                        Voyage = form.Flights.Join("<br />",
                            f =>
                            string.Format(
                                "{0}-{1}",
                                f.DepartureCity,
                                f.ArrivalCity)),
                        FlightInfo = form.Flights.Join("<br />",
                            f => string.Format(
                                "{0}{1}<br />{2} / {3}",
                                f.Carrier,
                                f.FlightNo,
                                string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk, getDiscountText
                                     (f.Discount))),
                        TakeoffTime = form.Flights.Join("<br />",
                            f =>
                            f.TakeoffTime.ToString(
                                "yyyy-MM-dd<br />HH:mm")),
                        Passengers = string.Join("<br />", form.Passengers),
                        ApplyType = form.ApplyformType.GetDescription(),
                        ProcessStatus = form.ApplyformType == ApplyformType.Postpone ?
                       Service.Order.StatusService.GetPostponeApplyformStatus((PostponeApplyformStatus)form.ApplyDetailStatus, OrderRole.Platform) :
                       Service.Order.StatusService.GetRefundApplyformStatus((RefundApplyformStatus)form.ApplyDetailStatus, OrderRole.Platform),
                        AppliedTime =
                            form.AppliedTime.ToString("yyyy-MM-dd<br />HH:mm"),
                        form.ApplierAccount,
                        form.ApplyformType,
                        ProductType = form.ProductType.GetDescription(),
                        RefundType = form.RefundType.HasValue ? form.RefundType.GetDescription() : "",
                        form.RequireRevisePrice,
                        LockInfo = lockInfo == null
                                       ? string.Empty
                                       : lockInfo.Company ==
                                         CurrentCompany.CompanyId
                                             ? string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name)
                                             : string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(),
                                                 lockInfo.Account)
                    };
                });
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }

        public object PlatformProcessApplyformNew(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(OrderRole.Platform).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                else if (condition.ApplyformType == ApplyformType.Postpone)
                {
                    if (!string.IsNullOrWhiteSpace(condition.PostponeStatusText) && condition.PostponeStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRolePostponeApplyformStatus(OrderRole.Platform).Where(s => s.Value == condition.PostponeStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                List<ApplyformListView> forms = ApplyformQueryService.PlatformQueryApplyformsForProcessNew(condition, pagination).ToList();
                var lockInfos = LockService.Query(forms.Select(form => form.ApplyformId.ToString())).ToList();
                var date = DateTime.Now.ToString("yyMMdd");
                var applyform = forms.Select(form =>
                {
                    LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == form.ApplyformId.ToString());
                    return new
                    {
                        form.ApplyformId,
                        PNR = form.OriginalPNR == null ? string.Empty : form.OriginalPNR.ToListString(),
                        Voyage = form.Flights.Join("<br />",
                            f =>
                            string.Format(
                                "{0}-{1}",
                                f.DepartureCity,
                                f.ArrivalCity)),
                        FlightInfo = form.Flights.Join("<br />",
                            f => string.Format(
                                "{0}{1}<br />{2} / {3}",
                                f.Carrier,
                                f.FlightNo,
                                string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk, getDiscountText
                                     (f.Discount))),
                        TakeoffTime = form.Flights.Join("<br />",
                            f =>
                            f.TakeoffTime.ToString(
                                "yyyy-MM-dd<br />HH:mm")),
                        TakeoffTimeIsToday = form.Flights.Join("|", f => f.TakeoffTime.ToString("yyMMdd") == date ? "1" : "0"),
                        Passengers = string.Join("<br />", form.Passengers),
                        ApplyType = form.ApplyformType.GetDescription(),
                        ProcessStatus = form.ApplyformType == ApplyformType.Postpone ?
                       Service.Order.StatusService.GetPostponeApplyformStatus((PostponeApplyformStatus)form.ApplyDetailStatus, OrderRole.Platform) :
                       Service.Order.StatusService.GetRefundApplyformStatus((RefundApplyformStatus)form.ApplyDetailStatus, OrderRole.Platform),
                        AppliedTime =
                            form.AppliedTime.ToString("yyyy-MM-dd<br />HH:mm"),
                        form.ApplierAccount,
                        form.ApplyformType,
                        ProductType = form.ProductType.GetDescription(),
                        RefundType = form.RefundType.HasValue ? form.RefundType.GetDescription() : "",
                        form.RequireRevisePrice,
                        LockInfo = lockInfo == null
                                       ? string.Empty
                                       : lockInfo.Company ==
                                         CurrentCompany.CompanyId
                                             ? string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name)
                                             : string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(),
                                                 lockInfo.Account)
                    };
                });
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }
        private string getDiscountText(decimal? discount)
        {
            if (discount.HasValue)
            {
                return (discount.Value * 100).TrimInvaidZero();
            }
            return "-";
        }


        /// <summary>
        /// 查询申请单支付状态，补单
        /// </summary>
        /// <param name="applyformId"></param>
        /// <returns></returns>
        public string QueryPaymentInfo(decimal applyformId)
        {
            bool paySuccess = false;
            var applyform = ApplyformQueryService.QueryApplyform(applyformId) as PostponeApplyform;
            if (applyform == null)
            {
                return "改期申请单不存在";
            }
            if (applyform.Status != PostponeApplyformStatus.Agreed)
            {
                return "OK";
            }
            try
            {
#if(DEBUG)
                var tradeResult = AccountTradeService.PayTradeQuery(applyformId.ToString());
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
                    ApplyformProcessService.PostponeFeePaySuccess(
                        applyform.Id,
                        tradeResult.AccountNo,
                        tradeResult.TradeNo,
                        tradeResult.FillChargeId.ToString(),//channelTradeNo
                        tradeResult.PayTime.Value,
                        payInterface,
                        payAccountType,
                        operatorAccount);
                    paySuccess = true;
                }
                LogService.SaveTradementLog(new TradementLog()
                {
                    OrderId = applyform.OrderId,
                    ApplyformId = applyformId,
                    Type = TradementBusinessType.SubPay,
                    Request = string.Empty,
                    Response = string.Empty,
                    Remark =  "改期支付补单",
                    Time = DateTime.Now
                });
#else
                QueryPaymentProcess queryPayment = new QueryPaymentProcess(applyformId.ToString());
                if (queryPayment.Execute() && queryPayment.PaySuccess)
                {
                    var paramArray = queryPayment.ExtraParams.Split('|');
                    if (paramArray.Length >= 3)
                    {
                        var operatorAccount = paramArray[2];
                        ApplyformProcessService.PostponeFeePaySuccess(
                            applyform.Id,
                            queryPayment.PayAccount,
                            queryPayment.PayTradeNo,
                            queryPayment.ChannelTradeNo,
                            queryPayment.PayTime,
                            queryPayment.PayChannel,
                            queryPayment.PayAccountType,
                            operatorAccount);
                        paySuccess = true;
                    }
                    LogService.SaveTradementLog(new TradementLog()
                    {
                        OrderId = applyform.OrderId,
                        ApplyformId = applyformId,
                        Type = TradementBusinessType.SubPay,
                        Request = string.Empty,
                        Response = queryPayment.ExtraParams,
                        Remark = "改期支付补单",
                        Time = DateTime.Now
                    });
                }
                else
                {
                    return "查询支付信息失败";
                }
#endif
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex, "补单");
                paySuccess = false;
            }
            return paySuccess?"OK":"ERROR";
        }

        public object QueryOEMApplyform(ApplyformQueryCondition condition, Pagination pagination)
        {
            try
            {
                var oem = ChinaPay.B3B.Service.Organization.OEMService.QueryOEM(CurrentCompany.CompanyId);
                if (oem == null)
                    throw new Exception("没有找到对应的oem信息");
                condition.OEMID = oem.Id;
                if (condition.ApplyformType == ApplyformType.Refund || condition.ApplyformType == ApplyformType.Scrap)
                {
                    if (!string.IsNullOrWhiteSpace(condition.RefundStatusText) && condition.RefundStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRoleRefundApplyformStatus(OrderRole.OEMOwner).Where(s => s.Value == condition.RefundStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                else if (condition.ApplyformType == ApplyformType.Postpone)
                {
                    if (!string.IsNullOrWhiteSpace(condition.PostponeStatusText) && condition.PostponeStatusText != "全部")
                    {
                        var statues = Service.Order.StatusService.GetRolePostponeApplyformStatus(OrderRole.OEMOwner).Where(s => s.Value == condition.PostponeStatusText).Select(s => s.Key);
                        if (statues.Any())
                        {
                            condition.ApplyDetailStatus = 0;
                            foreach (var status in statues)
                            {
                                condition.ApplyDetailStatus |= (byte)status;
                            }
                        }
                    }
                }
                List<ApplyformListView> forms = ApplyformQueryService.QueryApplyforms(condition, pagination).ToList();
                var lockInfos = LockService.Query(forms.Select(form => form.ApplyformId.ToString())).ToList();
                var applyform = forms.Select(form =>
                {
                    LockInfo lockInfo = lockInfos.FirstOrDefault(l => l.Key == form.ApplyformId.ToString());
                    return new
                    {
                        form.ApplyformId,
                        PNR = form.OriginalPNR == null ? string.Empty : form.OriginalPNR.ToListString(),
                        Voyage = IEnumerableExtension.Join<FlightListView>(form.Flights, "<br />",
                            f =>
                            string.Format(
                                "{0}{1}-{2}{3}",
                                f.DepartureCity,
                                f.DepartureAirport,
                                f.ArrivalCity,
                                f.ArrivalAirport)),
                        FlightInfo = form.Flights.Join("<br />", f => string.Format(
                            "{0}{1}<br />{2} / {3}",
                            f.Carrier,
                            f.FlightNo,
                            string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk, getDiscountText
                                                                        (f.Discount))),
                        TakeoffTime = form.Flights.Join("<br />", f =>
                                                                f.TakeoffTime.
                                                                    ToString(
                                                                        "yyyy-MM-dd<br />HH:mm")),
                        Passengers = String.Join("<br />", (IEnumerable<string>)form.Passengers),
                        ApplyTypeDesc = form.ApplyformType.GetDescription(),
                        ApplyType = form.ApplyformType,
                        ProcessStatus = GetProcessStatus(form, OrderRole.OEMOwner),
                        form.AppliedTime,
                        form.ApplierAccount,
                        ProductType = form.ProductType.GetDescription(),
                        LockInfo = lockInfo == null
                                       ? string.Empty
                                       : lockInfo.Company ==
                                         CurrentCompany.CompanyId
                                             ? string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name)
                                             : string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(), lockInfo.Account),
                        RenderSupperUnLock = lockInfo != null,
                        RefundType = form.RefundType.HasValue ? form.RefundType.GetDescription() : string.Empty,
                        IsRefund = form.ApplyformType == ApplyformType.Refund,
                        EnableQueryPaymentInfo = form.ApplyformType == ApplyformType.Postpone && (PostponeApplyformStatus)form.ApplyDetailStatus == PostponeApplyformStatus.Agreed
                    };
                });
                return new { Applyforms = applyform, Pagination = pagination };
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw;
            }
        }

        private static string GetProcessStatus(ApplyformListView form, OrderRole orderRole)
        {
            if (form.ApplyformType == ApplyformType.Postpone)
                return Service.Order.StatusService.GetPostponeApplyformStatus((PostponeApplyformStatus)form.ApplyDetailStatus, orderRole);
            else if (form.ApplyformType == ApplyformType.BlanceRefund) return Service.Order.StatusService.GetBalanceRefundStatus((BalanceRefundProcessStatus)form.ApplyDetailStatus, orderRole);
            else return Service.Order.StatusService.GetRefundApplyformStatus((RefundApplyformStatus)form.ApplyDetailStatus, orderRole);
        }
    }
}