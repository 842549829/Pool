using System;
using System.Data;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service.Report.Repository;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Report {
    /// <summary>
    /// 报表服务类
    /// </summary>
    public static class ReportService {
        #region 查询
        /// <summary>
        /// 买入机票明细
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        public static DataTable QueryPurchaseTicket(Pagination paination, PurchaseTicketView view, out decimal totaltradeAmount) {
            if(view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPurchaseTicketReport(paination, view, out totaltradeAmount);
        }
        /// <summary>
        /// 买入机票明细(新)
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        public static DataTable QueryNewPurchaseTicket(Pagination paination, PurchaseTicketView view, out decimal totalTradeAmount)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryNewPurchaseTicketReport(paination, view,out totalTradeAmount);
        }
        /// <summary>
        /// 买入资金报表
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        /// <param name="totalTradeAmount">交易金额</param>
        public static DataTable QueryPurchaseFinancial(Pagination paination, PurchaseTicketView view, out decimal totalTradeAmount)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPurchaseFinancialReport(paination, view, out totalTradeAmount);
        }
        /// <summary>
        /// 卖出机票明细
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable QueryProvideTicket(Pagination paination, ProvideTicketView view, out decimal totaltradeAmount) {
            if(view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryProvideTicketReport(paination, view, out totaltradeAmount);
        }
        /// <summary>
        /// 卖出资金报表
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="view">查询条件</param>
        public static DataTable QueryProvideFinancial(Pagination pagination, ProvideTicketView view, out decimal totalTradeAmount) {
            if(view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryProvideFinancialReport(pagination, view, out totalTradeAmount);
        }
        /// <summary>
        /// 提成明细
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable QuerySupplyTicket(Pagination pagination, SupplyTicketView view, out decimal totalTradeAmount) {
            if(view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QuerySupplyTicketReport(pagination, view, out totalTradeAmount);
        }
        /// <summary>
        /// 平台机票销售
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable QueryPlatformTicket(Pagination paination, PlatformTicketView view, out decimal totalPurchaserAmount, out decimal totalProviderAmount, out decimal totalSupplierAmount, out decimal totalRoyaltyAmount, out decimal totalPostponeFee, out decimal totalPlatformCommission, out decimal totalPlatformPremium, out decimal totalPlatformProfit)
        {
            if(view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPlatformTicketReport(paination, view, out  totalPurchaserAmount, out  totalProviderAmount, out  totalSupplierAmount, out totalRoyaltyAmount,out  totalPostponeFee, out  totalPlatformCommission, out totalPlatformPremium,out  totalPlatformProfit);
        }
        /// <summary>
        /// 推广用户明细表
        /// </summary>
        /// <param name="paination">分页信息</param> 
        /// <returns>table</returns>
        public static System.Data.DataTable QueryEmployeeSpreadStatisticReport(Pagination paination, DateTime startTime, DateTime endTime, Common.Enums.CompanyType? type, string EmployeeName, out decimal totalPurchaseAmount, out decimal totalSupplyAmount, out decimal totalProvideAmount, out int totalPurchaseCount, out int totalSupplyCount, out int totalProvideCount)
        {
            var repository = Factory.CreateReportRepository();
            return repository.QueryEmployeeSpreadStatisticReport(paination, startTime, endTime, type, EmployeeName, out  totalPurchaseAmount, out  totalSupplyAmount, out  totalProvideAmount, out  totalPurchaseCount, out  totalSupplyCount, out  totalProvideCount);
        }
        /// <summary>
        /// 经纪人报表
        /// </summary>
        /// <param name="paination"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static DataTable QuerySpreadTicket(Pagination paination, SpreadTicketView view, out decimal tradeAmount, out decimal amount) {
            if(view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QuerySpreadTicketReport(paination, view, out tradeAmount, out  amount);
        }
        /// <summary>
        /// 平台查询采购购票排行
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        public static DataTable QueryPurchaseStatistics(Pagination paination, PurchaseStatisticView view, out int orderCount, out int ticketCount) {
            if(view == null)
                throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPurchaseStatisticReport(paination, view, out  orderCount, out  ticketCount);
        }
        /// <summary>
        /// 查询平台出票量统计报表
        /// </summary>
        public static DataTable QueryProviderStatistics(Pagination pagination, ProviderStatisticSearchCondition condition, out int orderCount, out int ticketCount) {
            if(condition == null)
                throw new ArgumentNullException("condition");
            var repository = Factory.CreateReportRepository();
            return repository.QueryProviderStatisticReport(pagination, condition, out  orderCount, out  ticketCount);
        }
        /// <summary>
        /// 查询当日出票量统计信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="groupInfo">分组信息</param>
        /// <param name="pagination">分页信息</param>
        /// <param name="totalTicketCount">总票量</param>
        /// <param name="totalAmount">总交易金额</param>
        public static DataTable QueryTodayProvideStatistics(TodayProvideStatisticQueryCondition condition, GroupInfo groupInfo, Pagination pagination, out int totalTicketCount, out decimal totalAmount) {
            if(condition == null) throw new ArgumentNullException("condition");
            if(groupInfo == null) throw new ArgumentNullException("groupInfo");
            var repository = Factory.CreateReportRepository();
            return repository.QueryTodayProvideStatisticReport(condition, groupInfo, pagination, out totalTicketCount, out totalAmount);
        }
        /// <summary>
        /// 外部订单详细表
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        /// <param name="totalReceiveAmount">收款总额</param>
        /// <param name="totalPaymentAmount">付款总额</param>
        /// <param name="totalProfitAmount">利润总额</param>
        public static DataTable QueryPlatformExternalOrder(Pagination paination, PlatformExternalOrderView view, out decimal totalReceiveAmount, out decimal totalPaymentAmount, out decimal totalProfitAmount)
        {
            if (view == null) throw new ArgumentNullException("view");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPlatformExternalOrder(paination, view,out totalReceiveAmount, out totalPaymentAmount,out totalProfitAmount);
        }
        /// <summary>
        /// 统计出票效率
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static DataTable QueryProviderETDZSpeedStatistics(ETDZSpeedStatCondition condition, Pagination pagination)
        {

            if (condition == null) throw new ArgumentNullException("condition");
            if (condition.StatGroup == null) throw new ArgumentNullException("StatGroup");
            var repository = Factory.CreateReportRepository();
            return repository.QueryProviderETDZSpeedStatistics(condition, pagination);
        }
        /// <summary>
        /// 查询OEM扣点利润明细
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="condition">查询条件</param>
        public static DataTable QueryRoyaltyProfit(Pagination pagination,RoyaltyProfitCondition condition,out decimal totalTradeFee,out decimal totalRoyalty,out decimal totalAmount)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            var repository = Factory.CreateReportRepository();
            return repository.QueryRoyaltyProfit(pagination, condition,out totalTradeFee,out totalRoyalty,out totalAmount);
        }
        /// <summary>
        /// 采购查询差额退款统计
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="condition">查询条件</param>
        public static DataTable QueryPurchaseErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPurchaseErrorRefund(pagination, condition);
        }
        /// <summary>
        /// 出票方查询差额退款统计
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="condition">查询条件</param>
        public static DataTable QueryProviderErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            var repository = Factory.CreateReportRepository();
            return repository.QueryProviderErrorRefund(pagination, condition);
        }
        /// <summary>
        /// 平台查询差额退款统计
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="condition">查询条件</param>
        public static DataTable QueryPlatformErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPlatformErrorRefund(pagination, condition);
        }
        /// <summary>
        /// 查询采购差额退款资金报表
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="condition">查询条件</param>
        public static DataTable QueryPurchaseErrorRefundFinancial(Pagination pagination,ErrorRefundQueryCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            var repository = Factory.CreateReportRepository();
            return repository.QueryPurchaseErrorRefundFinancial(pagination, condition);
        }
        /// <summary>
        /// 查询出票差额退款资金报表
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="condition">查询条件</param>
        public static DataTable QueryProviderErrorRefundFinancial(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            var repository = Factory.CreateReportRepository();
            return repository.QueryProviderErrorRefundFinancial(pagination, condition);
        }
        #endregion

        #region 下载
        /// <summary>
        /// 买入机票明细
        /// </summary>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable DownloadPurchaseTicket(PurchaseTicketView view) {
            decimal totaltradeAmount;
            return ReportService.QueryPurchaseTicket(null, view, out  totaltradeAmount);
        }
        /// <summary>
        /// 买入机票明细(新)
        /// </summary>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable DownloadNewPurchaseTicket(PurchaseTicketView view)
        {
            decimal totaltradeAmount;
            return ReportService.QueryNewPurchaseTicket(null, view, out  totaltradeAmount);
        }
        /// <summary>
        /// 买入资金报表
        /// </summary>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable DownloadPurchaseFinancial(PurchaseTicketView view)
        {
            decimal totaltradeAmount;
            return ReportService.QueryPurchaseFinancial(null,view,out totaltradeAmount);
        }
        /// <summary>
        /// 卖出明细
        /// </summary>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable DownloadProvideTicket(ProvideTicketView view) {
            decimal totaltradeAmount;
            return ReportService.QueryProvideTicket(null, view, out  totaltradeAmount);
        }
        /// <summary>
        /// 卖出资金报表
        /// </summary>
        /// <param name="view">查询条件</param>
        /// <returns></returns>
        public static DataTable DownloadProvideFinancial(ProvideTicketView view) {
            decimal totalTradeAmount;
            return ReportService.QueryProvideFinancial(null, view, out totalTradeAmount);
        }
        /// <summary>
        /// 推广用户明细表
        /// </summary>
        /// <param name="paination">分页信息</param> 
        /// <returns>table</returns>
        public static System.Data.DataTable DownloadEmployeeSpreadStatisticReport(DateTime startTime, DateTime endTime, Common.Enums.CompanyType? type, string EmployeeName, out decimal totalPurchaseAmount, out decimal totalSupplyAmount, out decimal totalProvideAmount, out int totalPurchaseCount, out int totalSupplyCount, out int totalProvideCount)
        {
            var repository = Factory.CreateReportRepository();
            return repository.QueryEmployeeSpreadStatisticReport(null, startTime, endTime, type, EmployeeName, out  totalPurchaseAmount, out  totalSupplyAmount, out  totalProvideAmount, out  totalPurchaseCount, out  totalSupplyCount, out  totalProvideCount);
        }
        /// <summary>
        /// 提成明细
        /// </summary>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable DownloadSupplyTicket(SupplyTicketView view) {
            decimal totalTradeAmount;
            return ReportService.QuerySupplyTicket(null, view, out totalTradeAmount);
        }
        /// <summary>
        ///   平台销售
        /// </summary>
        /// <param name="view">查询条件</param>
        /// <returns>table</returns>
        public static DataTable DownloadPlatformTicket(PlatformTicketView view) {
            decimal totalPurchaserAmount; decimal totalProviderAmount; decimal totalSupplierAmount; decimal totalRoyaltyAmount; decimal totalPostponeFee; decimal totalPlatformCommission; decimal totalPlatformPremium; decimal totalPlatformProfit;
            return ReportService.QueryPlatformTicket(null, view, out  totalPurchaserAmount, out  totalProviderAmount, out  totalSupplierAmount,out totalRoyaltyAmount,out  totalPostponeFee, out  totalPlatformCommission, out totalPlatformPremium,out  totalPlatformProfit);
        }
        /// <summary>
        /// 经纪人报表
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static DataTable DownloadSpreadTicket(SpreadTicketView view) {
            decimal tradeAmount, amount;
            return ReportService.QuerySpreadTicket(null, view, out  tradeAmount, out  amount);
        }
        /// <summary>
        /// 下载平台采购票统计报表
        /// </summary>
        public static DataTable DownloadPurchaseStatistics(PurchaseStatisticView view) {
            int totalOrderCount, totalTicketCount;
            return QueryPurchaseStatistics(null, view, out  totalOrderCount, out  totalTicketCount);
        }
        /// <summary>
        /// 下载平台出票量统计报表
        /// </summary>
        public static DataTable DownloadProviderStatistics(ProviderStatisticSearchCondition searchCondition) {
            int orderCount, ticketCount;
            return QueryProviderStatistics(null, searchCondition, out  orderCount, out  ticketCount);
        }
        /// <summary>
        /// 下载外部订单详细明细表
        /// </summary>
        /// <param name="view">查询条件</param>
        public static DataTable DownloadPlatformExternalOrder(PlatformExternalOrderView view)
        {
            decimal totalRecieveAmount, totalPaymentAmount, totalProfitAmount;
            return QueryPlatformExternalOrder(null, view,out totalRecieveAmount,out totalPaymentAmount,out totalProfitAmount);
        }
        /// <summary>
        /// 下载OEM分润方扣点明细
        /// </summary>
        /// <param name="condition">查询条件</param>
        public static DataTable DownloadRoyaltyProfit(RoyaltyProfitCondition condition)
        {
            decimal totalTradeFee, toalTradeRoyalty, totalAmount;
            return QueryRoyaltyProfit(null, condition, out totalTradeFee, out toalTradeRoyalty, out totalAmount);
        }
        /// <summary>
        /// 平台下载差额退款统计
        /// </summary>
        /// <param name="condition">查询条件</param>
        public static DataTable DownloadPurchaseErrorRefund(ErrorRefundQueryCondition condition)
        {
            return QueryPurchaseErrorRefund(null, condition);
        }
        /// <summary>
        /// 平台下载差额退款统计
        /// </summary>
        /// <param name="condition">查询条件</param>
        public static DataTable DownloadProviderErrorRefund(ErrorRefundQueryCondition condition)
        {
            return QueryProviderErrorRefund(null, condition);
        }
        /// <summary>
        /// 平台下载差额退款统计
        /// </summary>
        /// <param name="condition">查询条件</param>
        public static DataTable DownloadPlatformErrorRefund(ErrorRefundQueryCondition condition)
        {
            return QueryPlatformErrorRefund(null, condition);
        }
        /// <summary>
        /// 下载采购差额退款报表
        /// </summary>
        /// <param name="condition">查询条件</param>
        public static DataTable DownloadPurchaseErrorRefundFinancial(ErrorRefundQueryCondition condition)
        {
            return QueryPurchaseErrorRefundFinancial(null, condition);
        }
        /// <summary>
        /// 下载出票差额资金报表
        /// </summary>
        /// <param name="condition">条件</param>
        public static DataTable DownloadProviderErrorRefundFinancial(ErrorRefundQueryCondition condition)
        {
            return QueryProviderErrorRefundFinancial(null, condition);
        }
        #endregion


    }
}