using System;
using System.Data;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Report.Repository
{
    public interface IReportRepository
    {
        DataTable QueryPurchaseTicketReport(Pagination paination, PurchaseTicketView view, out decimal totaltradeAmount);
        DataTable QueryNewPurchaseTicketReport(Pagination paination, PurchaseTicketView view, out decimal totaltradeAmount);
        DataTable QueryPurchaseFinancialReport(Pagination paination, PurchaseTicketView view, out decimal totaltradeAmount);
        DataTable QueryProvideTicketReport(Pagination paination, ProvideTicketView view, out decimal totaltradeAmount);
        DataTable QueryProvideFinancialReport(Pagination paination, ProvideTicketView view, out decimal totaltradeAmount);
        DataTable QuerySupplyTicketReport(Pagination paination, SupplyTicketView view, out decimal totalTradeAmount);
        DataTable QueryPlatformTicketReport(Pagination paination, PlatformTicketView view, out decimal totalPurchaserAmount, out decimal totalProviderAmount, out decimal totalSupplierAmount, out decimal totalRoyaltyAmount, out decimal totalPostponeFee, out decimal totalPlatformCommission, out decimal totalPlatformPremium, out decimal totalPlatformProfit);
        DataTable QuerySpreadTicketReport(Pagination paination, SpreadTicketView view, out decimal tradeAmount, out decimal amount);
        DataTable QueryPurchaseStatisticReport(Pagination pagination, PurchaseStatisticView view, out int orderCount, out int ticketCount);
        DataTable QueryProviderStatisticReport(Pagination pagination, ProviderStatisticSearchCondition condition, out int orderCount, out int ticketCount);
        DataTable QueryEmployeeSpreadStatisticReport(Pagination paination, DateTime startTime, DateTime endTime, CompanyType? type, string EmployeeName, out decimal totalPurchaseAmount, out decimal totalSupplyAmount, out decimal totalProvideAmount, out int totalPurchaseCount, out int totalSupplyCount, out int totalProvideCount);
        DataTable QueryTodayProvideStatisticReport(TodayProvideStatisticQueryCondition condition, GroupInfo groupInfo, Pagination pagination, out int totalTicketCount, out decimal totalAmount);
        DataTable QueryProviderETDZSpeedStatistics(ETDZSpeedStatCondition condition, Pagination pagination);
        DataTable QueryPlatformExternalOrder(Pagination paination, PlatformExternalOrderView view, out decimal totalReceiveAmount, out decimal totalPaymentAmount, out decimal totalProfitAmount);
        DataTable QueryRoyaltyProfit(Pagination pagination, RoyaltyProfitCondition condition,out decimal totalTradeFee,out decimal totalRoyalty,out decimal totalAmount);
        DataTable QueryPlatformErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition);
        DataTable QueryProviderErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition);
        DataTable QueryPurchaseErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition);
        DataTable QueryPurchaseErrorRefundFinancial(Pagination pagination, ErrorRefundQueryCondition condition);
        DataTable QueryProviderErrorRefundFinancial(Pagination paginantion, ErrorRefundQueryCondition condition);
    }
}
