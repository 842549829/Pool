using System;
using System.Data;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Report.Repository.SqlServer
{
    public class ReportRepository : SqlServerRepository, IReportRepository
    {
        public ReportRepository(string connectionString)
            : base(connectionString)
        {
        }
        /// <summary>
        /// 买入机票明细
        /// </summary>
        public System.Data.DataTable QueryPurchaseTicketReport(Pagination paination, PurchaseTicketView view, out decimal totaltradeAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_owner", view.CompanyId.HasValue ? view.CompanyId.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_beginFinishTime", view.FinishBeginDate.HasValue ? view.FinishBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endFinishTime", view.FinishEndDate.HasValue ? view.FinishEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginPayDate", view.PayBeginDate.HasValue ? view.PayBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endPayDate", view.PayEndDate.HasValue ? view.PayEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginTakeoffDate", view.TakeoffBeginDate.HasValue ? view.TakeoffBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endTakeoffDate", view.TakeoffEndDate.HasValue ? view.TakeoffEndDate.Value : (Nullable<DateTime>)null);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                if (!string.IsNullOrWhiteSpace(view.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", view.TicketNo);
                if (!string.IsNullOrWhiteSpace(view.Passenger))
                    dbOperator.AddParameter("@i_passenger", view.Passenger);
                if (!string.IsNullOrWhiteSpace(view.Departure))
                    dbOperator.AddParameter("@i_departure", view.Departure);
                if (!string.IsNullOrWhiteSpace(view.Arrival))
                    dbOperator.AddParameter("@i_arrival", view.Arrival);
                dbOperator.AddParameter("@i_type", view.TicketState.HasValue ? (byte)(view.TicketState.Value + 1) : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_productType", view.PolicyType.HasValue ? view.PolicyType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_ticketType", view.TicketType.HasValue ? (byte)view.TicketType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_orderId", view.OrderId.HasValue ? view.OrderId.Value : (Nullable<decimal>)null);
                if (!string.IsNullOrWhiteSpace(view.Airline))
                    dbOperator.AddParameter("@i_carrier", view.Airline);
                if (view.PayType.HasValue)
                    dbOperator.AddParameter("@i_payType", view.PayType);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotaltradeAmounts = dbOperator.AddParameter("@o_TotaltradeAmounts");
                oTotaltradeAmounts.DbType = System.Data.DbType.Decimal;
                (oTotaltradeAmounts as IDbDataParameter).Scale = 2;
                oTotaltradeAmounts.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("P_QueryPurchaseTicketReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;

                if (dt.Rows.Count > 0)
                {
                    totaltradeAmount = oTotaltradeAmounts.Value == DBNull.Value ? 0 : (decimal)oTotaltradeAmounts.Value;
                }
                else
                {
                    totaltradeAmount = 0;
                }
            }
            return dt;
        }
        /// <summary>
        /// 买入机票明细（新）
        /// </summary>
        /// <param name="paination">分页信息</param>
        /// <param name="view">查询条件</param>
        /// <param name="totaltradeAmount">总共的交易金额</param>
        public DataTable QueryNewPurchaseTicketReport(Pagination paination, PurchaseTicketView view, out decimal totaltradeAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_owner", view.CompanyId.HasValue ? view.CompanyId.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_beginFinishTime", view.FinishBeginDate.HasValue ? view.FinishBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endFinishTime", view.FinishEndDate.HasValue ? view.FinishEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginPayDate", view.PayBeginDate.HasValue ? view.PayBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endPayDate", view.PayEndDate.HasValue ? view.PayEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginTakeoffDate", view.TakeoffBeginDate.HasValue ? view.TakeoffBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endTakeoffDate", view.TakeoffEndDate.HasValue ? view.TakeoffEndDate.Value : (Nullable<DateTime>)null);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                if (!string.IsNullOrWhiteSpace(view.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", view.TicketNo);
                if (!string.IsNullOrWhiteSpace(view.Passenger))
                    dbOperator.AddParameter("@i_passenger", view.Passenger);
                if (!string.IsNullOrWhiteSpace(view.Departure))
                    dbOperator.AddParameter("@i_departure", view.Departure);
                if (!string.IsNullOrWhiteSpace(view.Arrival))
                    dbOperator.AddParameter("@i_arrival", view.Arrival);
                dbOperator.AddParameter("@i_type", view.TicketState.HasValue ? (byte)(view.TicketState.Value + 1) : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_productType", view.PolicyType.HasValue ? view.PolicyType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_ticketType", view.TicketType.HasValue ? (byte)view.TicketType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_orderId", view.OrderId.HasValue ? view.OrderId.Value : (Nullable<decimal>)null);
                if (!string.IsNullOrWhiteSpace(view.Airline))
                    dbOperator.AddParameter("@i_carrier", view.Airline);
                if (view.PayType.HasValue)
                    dbOperator.AddParameter("@i_payType", view.PayType);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotaltradeAmounts = dbOperator.AddParameter("@o_TotaltradeAmounts");
                oTotaltradeAmounts.DbType = System.Data.DbType.Decimal;
                (oTotaltradeAmounts as IDbDataParameter).Scale = 2;
                oTotaltradeAmounts.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("P_QueryNewPurchaseTicketReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;

                if (dt.Rows.Count > 0)
                {
                    totaltradeAmount = oTotaltradeAmounts.Value == DBNull.Value ? 0 : (decimal)oTotaltradeAmounts.Value;
                }
                else
                {
                    totaltradeAmount = 0;
                }
            }
            return dt;
        }
        /// <summary>
        /// 查询采购资金报表
        /// </summary>
        /// <param name="paination">分页</param>
        /// <param name="view">查询条件</param>
        /// <param name="totaltradeAmount">总共金额</param>
        public DataTable QueryPurchaseFinancialReport(Pagination paination, PurchaseTicketView view, out decimal totaltradeAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_owner", view.CompanyId.HasValue ? view.CompanyId.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_beginFinishTime", view.FinishBeginDate.HasValue ? view.FinishBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endFinishTime", view.FinishEndDate.HasValue ? view.FinishEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginPayDate", view.PayBeginDate.HasValue ? view.PayBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endPayDate", view.PayEndDate.HasValue ? view.PayEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginTakeoffDate", view.TakeoffBeginDate.HasValue ? view.TakeoffBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endTakeoffDate", view.TakeoffEndDate.HasValue ? view.TakeoffEndDate.Value : (Nullable<DateTime>)null);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                if (!string.IsNullOrWhiteSpace(view.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", view.TicketNo);
                if (!string.IsNullOrWhiteSpace(view.Passenger))
                    dbOperator.AddParameter("@i_passenger", view.Passenger);
                if (!string.IsNullOrWhiteSpace(view.Departure))
                    dbOperator.AddParameter("@i_departure", view.Departure);
                if (!string.IsNullOrWhiteSpace(view.Arrival))
                    dbOperator.AddParameter("@i_arrival", view.Arrival);
                dbOperator.AddParameter("@i_type", view.TicketState.HasValue ? (byte)(view.TicketState.Value + 1) : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_productType", view.PolicyType.HasValue ? view.PolicyType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_ticketType", view.TicketType.HasValue ? (byte)view.TicketType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_orderId", view.OrderId.HasValue ? view.OrderId.Value : (Nullable<decimal>)null);
                if (!string.IsNullOrWhiteSpace(view.Airline))
                    dbOperator.AddParameter("@i_carrier", view.Airline);
                if (view.PayType.HasValue)
                    dbOperator.AddParameter("@i_payType", view.PayType);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotaltradeAmounts = dbOperator.AddParameter("@o_TotaltradeAmounts");
                oTotaltradeAmounts.DbType = System.Data.DbType.Decimal;
                (oTotaltradeAmounts as IDbDataParameter).Scale = 2;
                oTotaltradeAmounts.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("dbo.P_QueryPurchaseFinancialReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;

                if (dt.Rows.Count > 0)
                {
                    totaltradeAmount = oTotaltradeAmounts.Value == DBNull.Value ? 0 : (decimal)oTotaltradeAmounts.Value;
                }
                else
                {
                    totaltradeAmount = 0;
                }
            }
            return dt;
        }
        /// <summary>
        /// 卖出机票明细
        /// </summary>
        public System.Data.DataTable QueryProvideTicketReport(Pagination paination, ProvideTicketView view, out decimal totaltradeAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_owner", view.CompanyId.HasValue ? view.CompanyId.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_beginFinishTime", view.FinishBeginDate.HasValue ? view.FinishBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endFinishTime", view.FinishEndDate.HasValue ? view.FinishEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginPayDate", view.PayBeginDate.HasValue ? view.PayBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endPayDate", view.PayEndDate.HasValue ? view.PayEndDate.Value : (Nullable<DateTime>)null);
                if (view.TakeoffBeginDate.HasValue)
                    dbOperator.AddParameter("@i_beginTakeoffDate", view.TakeoffBeginDate.Value);
                if (view.TakeoffEndDate.HasValue)
                    dbOperator.AddParameter("@i_endTakeoffDate", view.TakeoffEndDate.Value);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                if (!string.IsNullOrWhiteSpace(view.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", view.TicketNo);
                if (!string.IsNullOrWhiteSpace(view.Passenger))
                    dbOperator.AddParameter("@i_passenger", view.Passenger);
                if (!string.IsNullOrWhiteSpace(view.Departure))
                    dbOperator.AddParameter("@i_departure", view.Departure);
                if (!string.IsNullOrWhiteSpace(view.Arrival))
                    dbOperator.AddParameter("@i_arrival", view.Arrival);
                if (view.RelationType.HasValue)
                    dbOperator.AddParameter("@i_relationType", (byte)view.RelationType.Value);
                if (view.Purchase.HasValue)
                    dbOperator.AddParameter("@i_purchase", view.Purchase.Value);
                dbOperator.AddParameter("@i_type", view.TicketState.HasValue ? (byte)(view.TicketState.Value + 1) : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_ticketType", view.TicketType.HasValue ? (byte)view.TicketType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_productType", view.PolicyType.HasValue ? view.PolicyType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_orderId", view.OrderId.HasValue ? view.OrderId.Value : (Nullable<decimal>)null);
                if (!string.IsNullOrWhiteSpace(view.Airline))
                    dbOperator.AddParameter("@i_carrier", view.Airline);
                if (!string.IsNullOrWhiteSpace(view.OfficeNo))
                    dbOperator.AddParameter("@i_officeNo", view.OfficeNo);
                if (!string.IsNullOrWhiteSpace(view.ProcessorAccount))
                    dbOperator.AddParameter("@i_processorAccount", view.ProcessorAccount);
                if (view.SpecialProductType.HasValue)
                    dbOperator.AddParameter("@i_specialProductType", (byte)view.SpecialProductType);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotaltradeAmounts = dbOperator.AddParameter("@o_TotaltradeAmounts");
                oTotaltradeAmounts.DbType = System.Data.DbType.Decimal;
                (oTotaltradeAmounts as IDbDataParameter).Scale = 2;
                oTotaltradeAmounts.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("P_QueryProvideTicketReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    totaltradeAmount = oTotaltradeAmounts.Value == DBNull.Value ? 0 : (decimal)oTotaltradeAmounts.Value;
                }
                else
                {
                    totaltradeAmount = 0;
                }
            }
            return dt;
        }
        /// <summary>
        /// 卖出资金报表
        /// </summary>
        public System.Data.DataTable QueryProvideFinancialReport(Pagination paination, ProvideTicketView view, out decimal totaltradeAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_owner", view.CompanyId.HasValue ? view.CompanyId.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_beginFinishTime", view.FinishBeginDate.HasValue ? view.FinishBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endFinishTime", view.FinishEndDate.HasValue ? view.FinishEndDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_beginPayDate", view.PayBeginDate.HasValue ? view.PayBeginDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endPayDate", view.PayEndDate.HasValue ? view.PayEndDate.Value : (Nullable<DateTime>)null);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                if (!string.IsNullOrWhiteSpace(view.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", view.TicketNo);
                if (!string.IsNullOrWhiteSpace(view.Passenger))
                    dbOperator.AddParameter("@i_passenger", view.Passenger);
                if (!string.IsNullOrWhiteSpace(view.Departure))
                    dbOperator.AddParameter("@i_departure", view.Departure);
                if (!string.IsNullOrWhiteSpace(view.Arrival))
                    dbOperator.AddParameter("@i_arrival", view.Arrival);
                if (view.RelationType.HasValue)
                    dbOperator.AddParameter("@i_relationType", (byte)view.RelationType.Value);
                if (view.Purchase.HasValue)
                    dbOperator.AddParameter("@i_purchase", view.Purchase.Value);
                dbOperator.AddParameter("@i_type", view.TicketState.HasValue ? (byte)(view.TicketState.Value + 1) : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_ticketType", view.TicketType.HasValue ? (byte)view.TicketType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_productType", view.PolicyType.HasValue ? view.PolicyType.Value : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_orderId", view.OrderId.HasValue ? view.OrderId.Value : (Nullable<decimal>)null);
                if (!string.IsNullOrWhiteSpace(view.Airline))
                    dbOperator.AddParameter("@i_carrier", view.Airline);
                if (!string.IsNullOrWhiteSpace(view.OfficeNo))
                    dbOperator.AddParameter("@i_officeNo", view.OfficeNo);
                if (!string.IsNullOrWhiteSpace(view.ProcessorAccount))
                    dbOperator.AddParameter("@i_processorAccount", view.ProcessorAccount);
                if (view.SpecialProductType.HasValue)
                    dbOperator.AddParameter("@i_specialProductType", (byte)view.SpecialProductType);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotaltradeAmounts = dbOperator.AddParameter("@o_TotaltradeAmounts");
                oTotaltradeAmounts.DbType = System.Data.DbType.Decimal;
                (oTotaltradeAmounts as IDbDataParameter).Scale = 2;
                oTotaltradeAmounts.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("P_QueryProvideFinancialReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;

                if (dt.Rows.Count > 0)
                {
                    totaltradeAmount = oTotaltradeAmounts.Value == DBNull.Value ? 0 : (decimal)oTotaltradeAmounts.Value;
                }
                else
                {
                    totaltradeAmount = 0;
                }
            }
            return dt;
        }
        /// <summary>
        /// 提成明细
        /// </summary>
        public System.Data.DataTable QuerySupplyTicketReport(Pagination paination, SupplyTicketView view, out decimal totaltradeAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_owner", view.CompanyId.HasValue ? view.CompanyId.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_beginFinishTime", view.FinishBeginTime.HasValue ? view.FinishBeginTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endFinishTime", view.FinishEndTime.HasValue ? view.FinishEndTime.Value : (Nullable<DateTime>)null);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                dbOperator.AddParameter("@i_orderId", view.OrderId.HasValue ? view.OrderId.Value : (Nullable<decimal>)null);
                dbOperator.AddParameter("@i_type", view.TicketState.HasValue ? (view.TicketState.Value + 1) : (Nullable<ChinaPay.B3B.Common.Enums.TicketState>)null);
                if (!string.IsNullOrWhiteSpace(view.Ariline))
                    dbOperator.AddParameter("@i_carrier", view.Ariline);
                if (view.SpecialProductType.HasValue)
                    dbOperator.AddParameter("@i_specialProductType", (byte)view.SpecialProductType);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotaltradeAmounts = dbOperator.AddParameter("@o_TotaltradeAmounts");
                oTotaltradeAmounts.DbType = System.Data.DbType.Decimal;
                (oTotaltradeAmounts as IDbDataParameter).Scale = 2;
                oTotaltradeAmounts.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("P_QuerySupplyTicketReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    totaltradeAmount = oTotaltradeAmounts.Value == DBNull.Value ? 0 : (decimal)oTotaltradeAmounts.Value;
                }
                else
                {
                    totaltradeAmount = 0;
                }
            }
            return dt;
        }
        /// <summary>
        ///平台机票销售 
        /// </summary>
        public System.Data.DataTable QueryPlatformTicketReport(Pagination paination, PlatformTicketView view, out decimal totalPurchaserAmount,
            out decimal totalProviderAmount, out decimal totalSupplierAmount, out decimal totalRoyaltyAmount, out decimal totalPostponeFee, out decimal totalPlatformCommission, out decimal totalPlatformPremium, out decimal totalPlatformProfit)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginFinishTime", view.FinishBeginTime.HasValue ? view.FinishBeginTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endFinishTime", view.FinishEndTime.HasValue ? view.FinishEndTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_purchaser", view.Purchaser.HasValue ? view.Purchaser.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_provider", view.Provider.HasValue ? view.Provider.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_supplier", view.Supplier.HasValue ? view.Supplier.Value : (Nullable<Guid>)null);
                dbOperator.AddParameter("@i_type", view.TicketState.HasValue ? (byte)(view.TicketState.Value + 1) : (Nullable<byte>)null);
                dbOperator.AddParameter("@i_sellReleation", view.RelationType.HasValue ? (byte)view.RelationType.Value : (Nullable<byte>)null);
                if (view.PayType.HasValue)
                    dbOperator.AddParameter("@i_payType", view.PayType.Value);
                if (view.TakeoffBeginDate.HasValue)
                    dbOperator.AddParameter("@i_beginTakeoffDate", view.TakeoffBeginDate.Value);
                if (view.TakeoffEndDate.HasValue)
                    dbOperator.AddParameter("@i_endTakeoffDate", view.TakeoffEndDate.Value);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                if (!string.IsNullOrWhiteSpace(view.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", view.TicketNo);
                if (!string.IsNullOrWhiteSpace(view.Passenger))
                    dbOperator.AddParameter("@i_passenger", view.Passenger);
                if (!string.IsNullOrWhiteSpace(view.Airline))
                    dbOperator.AddParameter("@i_airline", view.Airline);
                dbOperator.AddParameter("@i_orderId", view.OrderId.HasValue ? view.OrderId.Value : (Nullable<decimal>)null);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalPurchaserAmount = dbOperator.AddParameter("@o_totalPurchaserAmount");
                o_totalPurchaserAmount.DbType = System.Data.DbType.Decimal;
                (o_totalPurchaserAmount as IDbDataParameter).Scale = 2;
                o_totalPurchaserAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter o_totalProviderAmount = dbOperator.AddParameter("@o_totalProviderAmount");
                o_totalProviderAmount.DbType = System.Data.DbType.Decimal;
                (o_totalProviderAmount as IDbDataParameter).Scale = 2;
                o_totalProviderAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter o_totalSupplierAmount = dbOperator.AddParameter("@o_totalSupplierAmount");
                o_totalSupplierAmount.DbType = System.Data.DbType.Decimal;
                (o_totalSupplierAmount as IDbDataParameter).Scale = 2;
                o_totalSupplierAmount.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalRoyaltyAmount = dbOperator.AddParameter("o_totalRoyaltyAmount");
                o_totalRoyaltyAmount.DbType = System.Data.DbType.Decimal;
                (o_totalRoyaltyAmount as IDbDataParameter).Scale = 2;
                o_totalRoyaltyAmount.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalPostponeFee = dbOperator.AddParameter("@o_totalPostponeFee");
                o_totalPostponeFee.DbType = System.Data.DbType.Decimal;
                (o_totalPostponeFee as IDbDataParameter).Scale = 2;
                o_totalPostponeFee.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter o_totalPlatformCommission = dbOperator.AddParameter("@o_totalPlatformCommission");
                o_totalPlatformCommission.DbType = System.Data.DbType.Decimal;
                (o_totalPlatformCommission as IDbDataParameter).Scale = 2;
                o_totalPlatformCommission.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalPlatformPremium = dbOperator.AddParameter("@o_totalPlatformPremium");
                o_totalPlatformPremium.DbType = System.Data.DbType.Decimal;
                (o_totalPlatformPremium as IDbDataParameter).Scale = 2;
                o_totalPlatformPremium.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalPlatformProfit = dbOperator.AddParameter("@o_totalPlatformProfit");
                o_totalPlatformProfit.DbType = System.Data.DbType.Decimal;
                (o_totalPlatformProfit as IDbDataParameter).Scale = 2;
                o_totalPlatformProfit.Direction = System.Data.ParameterDirection.Output;


                dt = dbOperator.ExecuteTable("P_QueryPlatformTicketReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    //总计
                    totalPurchaserAmount = o_totalPurchaserAmount.Value == DBNull.Value ? 0 : (decimal)o_totalPurchaserAmount.Value;
                    totalProviderAmount = o_totalProviderAmount.Value == DBNull.Value ? 0 : (decimal)o_totalProviderAmount.Value;
                    totalSupplierAmount = o_totalSupplierAmount.Value == DBNull.Value ? 0 : (decimal)o_totalSupplierAmount.Value;
                    totalRoyaltyAmount = o_totalRoyaltyAmount.Value == DBNull.Value ? 0 : (decimal)o_totalRoyaltyAmount.Value;
                    totalPostponeFee = o_totalPostponeFee.Value == DBNull.Value ? 0 : (decimal)o_totalPostponeFee.Value;
                    totalPlatformCommission = o_totalPlatformCommission.Value == DBNull.Value ? 0 : (decimal)o_totalPlatformCommission.Value;
                    totalPlatformProfit = o_totalPlatformProfit.Value == DBNull.Value ? 0 : (decimal)o_totalPlatformProfit.Value;
                    totalPlatformPremium = o_totalPlatformPremium.Value == DBNull.Value ? 0 : (decimal)o_totalPlatformPremium.Value;
                }
                else
                {
                    //总计
                    totalPurchaserAmount = 0;
                    totalProviderAmount = 0;
                    totalSupplierAmount = 0;
                    totalRoyaltyAmount = 0;
                    totalPostponeFee = 0;
                    totalPlatformCommission = 0;
                    totalPlatformProfit = 0;
                    totalPlatformPremium = 0;
                }
            }
            return dt;
        }
        /// <summary>
        /// 经纪人报表
        /// </summary>
        /// <param name="paination"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public System.Data.DataTable QuerySpreadTicketReport(Pagination paination, SpreadTicketView view, out decimal tradeAmount, out decimal amount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (view.BeginFinishTime.HasValue)
                    dbOperator.AddParameter("@i_beginFinishTime", view.BeginFinishTime);
                if (view.EndFinishTime.HasValue)
                    dbOperator.AddParameter("@i_endFinishTime", view.EndFinishTime);
                if (view.TicketState.HasValue)
                    dbOperator.AddParameter("@i_type", view.TicketState + 1);
                if (view.BargainType.HasValue)
                    dbOperator.AddParameter("@i_barginerType", view.BargainType);
                if (view.Bargainer.HasValue)
                    dbOperator.AddParameter("@i_bargainer", view.Bargainer);
                if (view.Spreader.HasValue)
                    dbOperator.AddParameter("@i_spreader", view.Spreader);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTradeAmount = dbOperator.AddParameter("@oTradeAmount");
                oTradeAmount.DbType = System.Data.DbType.Decimal;
                (oTradeAmount as IDbDataParameter).Scale = 2;
                oTradeAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oAmount = dbOperator.AddParameter("@oAmount");
                oAmount.DbType = System.Data.DbType.Decimal;
                (oAmount as IDbDataParameter).Scale = 2;
                oAmount.Direction = System.Data.ParameterDirection.Output;

                dt = dbOperator.ExecuteTable("dbo.P_QuerySpreadTicketReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    tradeAmount = oTradeAmount.Value == DBNull.Value ? 0 : (decimal)oTradeAmount.Value;
                    amount = oAmount.Value == DBNull.Value ? 0 : (decimal)oAmount.Value;
                }
                else
                {
                    tradeAmount = 0;
                    amount = 0;
                }
            }
            return dt;
        }
        /// <summary>
        /// 平台查询买家买票排行
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="view">查询条件</param>
        public System.Data.DataTable QueryPurchaseStatisticReport(Pagination pagination, PurchaseStatisticView view, out int orderCount, out int ticketCount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (view.ReportStartDate.HasValue)
                    dbOperator.AddParameter("@iReportStartDate", view.ReportStartDate.Value.Date);
                if (view.ReportEndDate.HasValue)
                    dbOperator.AddParameter("@iReportEndDate", view.ReportEndDate.Value.Date);
                if (view.Purchase.HasValue)
                    dbOperator.AddParameter("@iPurchaser", view.Purchase);
                if (view.IsHasTrade.HasValue)
                    dbOperator.AddParameter("@iHasTrade", view.IsHasTrade);
                if (!(string.IsNullOrWhiteSpace(view.Carrier)))
                    dbOperator.AddParameter("@iCarrier", view.Carrier);
                if (!string.IsNullOrWhiteSpace(view.Departure))
                    dbOperator.AddParameter("@iDeparture", view.Departure);
                if (pagination == null)
                {
                    dbOperator.AddParameter("@iPagination", 0);
                }
                else
                {
                    dbOperator.AddParameter("@iPagination", 1);
                    dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                    dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oRowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalOrderCount = dbOperator.AddParameter("@oTotalOrderCount");
                oTotalOrderCount.DbType = System.Data.DbType.Int32;
                oTotalOrderCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalTicketCount = dbOperator.AddParameter("@oTotalTicketCount");
                oTotalTicketCount.DbType = System.Data.DbType.Int32;
                oTotalTicketCount.Direction = System.Data.ParameterDirection.Output;

                dt = dbOperator.ExecuteTable("dbo.P_QueryPurchaseStatisticReport", System.Data.CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    orderCount = oTotalOrderCount.Value == DBNull.Value ? 0 : (int)oTotalOrderCount.Value;
                    ticketCount = oTotalTicketCount.Value == DBNull.Value ? 0 : (int)oTotalTicketCount.Value;
                }
                else
                {
                    orderCount = 0;
                    ticketCount = 0;
                }
            }
            return dt;
        }

        public DataTable QueryProviderStatisticReport(Pagination pagination, ProviderStatisticSearchCondition condition, out int orderCount, out int ticketCount)
        {
            DataTable dt = null;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (condition.ReportStartDate.HasValue)
                    dbOperator.AddParameter("@iReportStartDate", condition.ReportStartDate.Value.Date);
                if (condition.ReportEndDate.HasValue)
                    dbOperator.AddParameter("@iReportEndDate", condition.ReportEndDate.Value.Date);
                if (condition.Provider.HasValue)
                    dbOperator.AddParameter("@iProvider", condition.Provider);
                if (condition.IsHasTrade.HasValue)
                    dbOperator.AddParameter("@iHasTrade", condition.IsHasTrade);
                if (!(string.IsNullOrWhiteSpace(condition.Carrier)))
                    dbOperator.AddParameter("@iCarrier", condition.Carrier);
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                    dbOperator.AddParameter("@iDeparture", condition.Departure);
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                {
                    dbOperator.AddParameter("@iArrival", condition.Arrival);
                }
                if (condition.SaleRelation.HasValue)
                {
                    dbOperator.AddParameter("@iRelation", condition.SaleRelation.Value);
                }
                if (condition.ProductType.HasValue)
                {
                    dbOperator.AddParameter("@iProduct", (int)condition.ProductType.Value);
                }
                if (condition.SpecialProductType.HasValue)
                {
                    dbOperator.AddParameter("@iSpecialType", (int)condition.SpecialProductType.Value);
                }
                if (pagination == null)
                {
                    dbOperator.AddParameter("@iPagination", 0);
                }
                else
                {
                    dbOperator.AddParameter("@iPagination", 1);
                    dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                    dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oRowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter oTotalOrderCount = dbOperator.AddParameter("@oTotalOrderCount");
                oTotalOrderCount.DbType = System.Data.DbType.Int32;
                oTotalOrderCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalTicketCount = dbOperator.AddParameter("@oTotalTicketCount");
                oTotalTicketCount.DbType = System.Data.DbType.Int32;
                oTotalTicketCount.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("dbo.P_QueryProvideStatisticReport", System.Data.CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    orderCount = oTotalOrderCount.Value == DBNull.Value ? 0 : (int)oTotalOrderCount.Value;
                    ticketCount = oTotalTicketCount.Value == DBNull.Value ? 0 : (int)oTotalTicketCount.Value;
                }
                else
                {
                    orderCount = 0;
                    ticketCount = 0;
                }
            }
            return dt;
        }


        public DataTable QueryEmployeeSpreadStatisticReport(Pagination paination, DateTime startTime, DateTime endTime, Common.Enums.CompanyType? type, string EmployeeName, out decimal totalPurchaseAmount, out decimal totalSupplyAmount, out decimal totalProvideAmount, out int totalPurchaseCount, out int totalSupplyCount, out int totalProvideCount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_employee", string.IsNullOrEmpty(EmployeeName) ? null : EmployeeName);
                dbOperator.AddParameter("@i_beginFinishTime", startTime);
                dbOperator.AddParameter("@i_endFinishTime", endTime);
                dbOperator.AddParameter("@i_companyType", type.HasValue ? type : null);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalPurchaseAmount = dbOperator.AddParameter("@o_totalPurchaseAmount");
                oTotalPurchaseAmount.DbType = System.Data.DbType.Decimal;
                (oTotalPurchaseAmount as IDbDataParameter).Scale = 2;
                oTotalPurchaseAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalSupplyAmount = dbOperator.AddParameter("@o_totalSupplyAmount");
                oTotalSupplyAmount.DbType = System.Data.DbType.Decimal;
                (oTotalSupplyAmount as IDbDataParameter).Scale = 2;
                oTotalSupplyAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalProvideAmount = dbOperator.AddParameter("@o_totalProvideAmount");
                oTotalProvideAmount.DbType = System.Data.DbType.Decimal;
                (oTotalProvideAmount as IDbDataParameter).Scale = 2;
                oTotalProvideAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalPurchaseCount = dbOperator.AddParameter("@o_totalPurchaseCount");
                oTotalPurchaseCount.DbType = System.Data.DbType.Int32;
                oTotalPurchaseCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalSupplyCount = dbOperator.AddParameter("@o_totalSupplyCount");
                oTotalSupplyCount.DbType = System.Data.DbType.Int32;
                oTotalSupplyCount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter oTotalProvideCount = dbOperator.AddParameter("@o_totalProvideCount");
                oTotalProvideCount.DbType = System.Data.DbType.Int32;
                oTotalProvideCount.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("P_QueryEmployeeSpreadStatisticReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;

                if (dt.Rows.Count > 0)
                {
                    totalPurchaseAmount = oTotalPurchaseAmount.Value == DBNull.Value ? 0 : (decimal)oTotalPurchaseAmount.Value;
                    totalProvideAmount = oTotalProvideAmount.Value == DBNull.Value ? 0 : (decimal)oTotalProvideAmount.Value;
                    totalSupplyAmount = oTotalSupplyAmount.Value == DBNull.Value ? 0 : (decimal)oTotalSupplyAmount.Value;
                    totalPurchaseCount = oTotalPurchaseCount.Value == DBNull.Value ? 0 : (int)oTotalPurchaseCount.Value;
                    totalProvideCount = oTotalProvideCount.Value == DBNull.Value ? 0 : (int)oTotalProvideCount.Value;
                    totalSupplyCount = oTotalSupplyCount.Value == DBNull.Value ? 0 : (int)oTotalSupplyCount.Value;
                }
                else
                {
                    totalPurchaseAmount = 0;
                    totalProvideAmount = 0;
                    totalSupplyAmount = 0;
                    totalPurchaseCount = 0;
                    totalProvideCount = 0;
                    totalSupplyCount = 0;
                }
            }
            return dt;
        }

        public DataTable QueryTodayProvideStatisticReport(TodayProvideStatisticQueryCondition condition, GroupInfo groupInfo, Pagination pagination, out int totalTicketCount, out decimal totalAmount)
        {
            var selectCatelog = "";
            var sqlSelect = "";
            var sqlWhere = new StringBuilder();
            var sqlGroup = "";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var reportDate = DateTime.Today;
                if (DateTime.Now.Hour <= 1)
                {
                    reportDate = reportDate.AddDays(-1);
                }
                sqlWhere.Append(" @ReportDate<=ReportDate AND");
                dbOperator.AddParameter("ReportDate", reportDate.Date);
                if (condition.StartHour.HasValue)
                {
                    sqlWhere.Append(" @StartHour<=ReportHour AND");
                    dbOperator.AddParameter("StartHour", condition.StartHour.Value);
                }
                if (condition.EndHour.HasValue)
                {
                    sqlWhere.Append(" ReportHour<=@EndHour AND");
                    dbOperator.AddParameter("EndHour", condition.EndHour.Value);
                }
                if (condition.Provider.HasValue)
                {
                    sqlWhere.Append(" Provider=@Provider AND");
                    dbOperator.AddParameter("Provider", condition.Provider.Value);
                }
                if (!string.IsNullOrWhiteSpace(condition.Carrier))
                {
                    sqlWhere.Append(" Carrier=@Carrier AND");
                    dbOperator.AddParameter("Carrier", condition.Carrier);
                }
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                {
                    sqlWhere.Append(" Departure=@Departure AND");
                    dbOperator.AddParameter("Departure", condition.Departure);
                }
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                {
                    sqlWhere.Append(" Arrival=@Arrival AND");
                    dbOperator.AddParameter("Arrival", condition.Arrival);
                }
                if (!string.IsNullOrWhiteSpace(condition.Bunk))
                {
                    sqlWhere.Append(" Bunk=@Bunk AND");
                    dbOperator.AddParameter("Bunk", condition.Bunk);
                }
                if (condition.ProductType.HasValue)
                {
                    sqlWhere.Append(" Product=@Product AND");
                    dbOperator.AddParameter("Product", (byte)condition.ProductType.Value);
                }
                if (condition.SpecialProductType.HasValue)
                {
                    sqlWhere.Append(" SpecialType=@SpecialType AND");
                    dbOperator.AddParameter("SpecialType", (byte)condition.SpecialProductType.Value);
                }
                if (condition.Relation.HasValue)
                {
                    sqlWhere.Append(" Relation=@Relation AND");
                    dbOperator.AddParameter("Relation", (byte)condition.Relation.Value);
                }
                if (groupInfo.Carrier)
                {
                    sqlGroup += "Carrier,";
                    sqlSelect += "Carrier,";
                    selectCatelog += "Carrier,";
                }
                if (groupInfo.Voyage)
                {
                    sqlGroup += "Departure,Arrival,";
                    sqlSelect += "Departure+'-'+Arrival AS AirportPair,";
                    selectCatelog += "AirportPair,";
                }
                if (groupInfo.FlightNo)
                {
                    sqlGroup += "FlightNo,";
                    sqlSelect += "FlightNo,";
                    selectCatelog += "FlightNo,";
                }
                if (groupInfo.Bunk)
                {
                    sqlGroup += "Bunk,";
                    sqlSelect += "Bunk,";
                    selectCatelog += "Bunk,";
                }
                if (groupInfo.Provider)
                {
                    sqlGroup += "ProviderName,";
                    sqlSelect += "ProviderName,";
                    selectCatelog += "ProviderName,";
                }
                if (groupInfo.Relation)
                {
                    sqlGroup += "Relation,";
                    sqlSelect += "CASE Relation WHEN 1 THEN '同行' WHEN 2 THEN '下级' WHEN 4 THEN '内部机构' END AS Relation,";
                    selectCatelog += "Relation,";
                }
                sqlSelect += "SUM(TicketCount) AS TicketCount,SUM(Amount) AS Amount";
                selectCatelog += "TicketCount,Amount";
                var sql = new StringBuilder();
                sql.Append("SELECT " + sqlSelect + " INTO #todayReportRecord FROM T_TodayProvideStatisticReport");
                if (sqlWhere.Length > 0)
                {
                    sql.Append(" WHERE " + sqlWhere.Remove(sqlWhere.Length - 4, 4));
                }
                if (sqlGroup.Length > 0)
                {
                    sql.Append(" GROUP BY " + sqlGroup.Remove(sqlGroup.Length - 1, 1));
                }

                if (pagination != null && pagination.GetRowCount)
                {
                    var pageIndex = pagination.PageIndex < 1 ? 1 : pagination.PageIndex;
                    var pageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize;
                    var startRow = (pageIndex - 1) * pageSize + 1;
                    var endRow = pageIndex * pageSize;
                    dbOperator.AddParameter("StartRow", startRow);
                    dbOperator.AddParameter("EndRow", endRow);
                    sql.Append(";SELECT ROW_NUMBER() OVER(ORDER BY TicketCount DESC,Amount DESC) AS RowNum,* INTO #todayReportRecord2 FROM #todayReportRecord;");
                    sql.Append("SELECT " + selectCatelog + " FROM #todayReportRecord2 WHERE RowNum BETWEEN @StartRow AND @EndRow;");
                    sql.Append("SELECT ISNULL(SUM(TicketCount),0) AS TicketCount,ISNULL(SUM(Amount),0) AS Amount,ISNULL(SUM(1),0) AS [RowCount] FROM #todayReportRecord2;");
                    sql.Append("DROP TABLE #todayReportRecord2;");
                }
                else
                {
                    sql.Append(";SELECT * FROM #todayReportRecord;");
                    sql.Append("SELECT ISNULL(SUM(TicketCount),0) AS TicketCount,ISNULL(SUM(Amount),0) AS Amount FROM #todayReportRecord;");
                }
                sql.Append("DROP TABLE #todayReportRecord;");

                var datas = dbOperator.ExecuteDataSet(sql.ToString(), "TableData", "TableStatistic");

                var result = datas.Tables[0];
                if (result.Rows.Count == 1 && result.Rows[0].IsNull(0))
                {
                    result.Rows.RemoveAt(0);

                    totalTicketCount = 0;
                    totalAmount = 0;
                    if (pagination != null && pagination.GetRowCount)
                    {
                        pagination.RowCount = 0;
                    }
                }
                else
                {
                    var statisticInfo = datas.Tables[1].Rows[0];
                    totalTicketCount = (int)statisticInfo["TicketCount"];
                    totalAmount = (decimal)statisticInfo["Amount"];
                    if (pagination != null && pagination.GetRowCount)
                    {
                        pagination.RowCount = (int)statisticInfo["RowCount"];
                    }
                }
                return result;
            }
        }

        public DataTable QueryProviderETDZSpeedStatistics(ETDZSpeedStatCondition condition, Pagination pagination)
        {
            var selectCatelog = "";
            var sqlSelect = "";
            var sqlWhere = new StringBuilder();
            var sqlGroup = "";
            var selectColumnName = "";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (condition.StartStatTime.HasValue)
                {
                    sqlWhere.Append(" ProcessDate>=@StartStatTime AND");
                    dbOperator.AddParameter("StartStatTime", condition.StartStatTime.Value);
                }
                if (condition.EndStatTime.HasValue)
                {
                    sqlWhere.Append(" ProcessDate<=@EndStatTime AND");
                    dbOperator.AddParameter("EndStatTime", condition.EndStatTime.Value);
                }
                if (!string.IsNullOrWhiteSpace(condition.Carrier))
                {
                    sqlWhere.Append(" Carrier = @Carrier AND");
                    dbOperator.AddParameter("Carrier", condition.Carrier);
                }
                if (condition.Provider.HasValue)
                {
                    sqlWhere.Append(" TOR.Provider=@Provider AND");
                    dbOperator.AddParameter("Provider", condition.Provider.Value);
                }
                if (condition.TicketType.HasValue)
                {
                    sqlWhere.Append(" TicketType = @TicketType AND");
                    dbOperator.AddParameter("TicketType", condition.TicketType.Value);
                }
                if (condition.StatGroup.GroupByCarrier)
                {
                    sqlGroup += "Carrier,";
                    sqlSelect += "Carrier,";
                    selectColumnName += "Carrier,";
                }
                if (condition.StatGroup.GroupByTicketType)
                {
                    sqlGroup += "TicketType,";
                    sqlSelect += "TicketType,";
                    selectColumnName += "TicketType,";
                }
                if (condition.StatGroup.GroupByProvider && !condition.Provider.HasValue)
                {
                    sqlGroup += "TOR.Provider,";
                    sqlSelect += "TOR.Provider,";
                }
                selectColumnName += "[rowNum],OrderCount,Speed,";
                sqlSelect += " ROW_NUMBER() OVER(ORDER BY AVG(Speed)) AS RowNum,COUNT(0) as OrderCount,ISNULL(AVG(Speed),0) as AVGSpeed,";
                selectCatelog += "T_SpeedStatistic,T_Order TOR";
                var sql = new StringBuilder("declare @TempTable Table([rowNum] int,Speed int,OrderCount int,Carrier varchar(5),Provider uniqueidentifier,TicketType tinyint );");
                sql.Append("insert into @TempTable(" + selectColumnName.Remove(selectColumnName.Length - 1, 1) + ")");
                sql.Append("SELECT " + sqlSelect.Remove(sqlSelect.Length - 1, 1) + " FROM " + selectCatelog);
                sqlWhere.Append(" T_SpeedStatistic.BusinessId = TOR.Id and T_SpeedStatistic.Type = 1 ");
                sql.Append(" WHERE " + sqlWhere);

                var pageIndex = pagination.PageIndex < 1 ? 1 : pagination.PageIndex;
                var pageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize;
                var startRow = (pageIndex - 1) * pageSize + 1;
                var endRow = pageIndex * pageSize;
                dbOperator.AddParameter("StartRow", startRow);
                dbOperator.AddParameter("EndRow", endRow);
                if (sqlGroup.Length > 0)
                {
                    sql.Append(" GROUP BY " + sqlGroup.Remove(sqlGroup.Length - 1, 1));
                }

                sql.Append(";SELECT " + selectColumnName.Remove(selectColumnName.Length - 1, 1).Replace("TicketType", "CASE TicketType WHEN 1 THEN 'BSP' WHEN 0 THEN 'B2B'END AS TicketType") + " FROM @TempTable WHERE rowNum BETWEEN @StartRow AND @EndRow;");

                var datas = dbOperator.ExecuteDataSet(sql.ToString(), "TableData", "TableStatistic");

                var result = datas.Tables[0];
                pagination.RowCount = result.Rows.Count;
                return result;
            }
        }


        public DataTable QueryPlatformExternalOrder(Pagination paination, PlatformExternalOrderView view, out decimal totalReceiveAmount, out decimal totalPaymentAmount, out decimal totalProfitAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginPayTime", view.BeginPayTime.HasValue ? view.BeginPayTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endPayTime", view.EndPayTime.HasValue ? view.EndPayTime.Value : (Nullable<DateTime>)null);
                if (!string.IsNullOrWhiteSpace(view.ExternalOrderId))
                    dbOperator.AddParameter("@i_externalOrderId", view.ExternalOrderId);
                if (view.OrderId.HasValue)
                    dbOperator.AddParameter("@i_orderId", view.OrderId);
                if (!string.IsNullOrWhiteSpace(view.Airline))
                    dbOperator.AddParameter("@i_airline", view.Airline);
                if (!string.IsNullOrWhiteSpace(view.Departure))
                    dbOperator.AddParameter("@i_departure", view.Departure);
                if (!string.IsNullOrWhiteSpace(view.Arrival))
                    dbOperator.AddParameter("@i_arrival", view.Arrival);
                if (view.Payed.HasValue)
                    dbOperator.AddParameter("@i_payed", view.Payed);
                if (!string.IsNullOrWhiteSpace(view.PNR))
                    dbOperator.AddParameter("@i_PNR", view.PNR);
                if (view.OrderSource.HasValue)
                    dbOperator.AddParameter("@i_orderSource", view.OrderSource);
                if (view.ETDZStatus.HasValue)
                    dbOperator.AddParameter("@i_etdzed", view.ETDZStatus);
                if (paination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalReceiveAmount = dbOperator.AddParameter("@o_totalReceiveAmount");
                o_totalReceiveAmount.DbType = System.Data.DbType.Decimal;
                (o_totalReceiveAmount as IDbDataParameter).Scale = 2;
                o_totalReceiveAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter o_totalPaymentAmount = dbOperator.AddParameter("@o_totalPaymentAmount");
                o_totalPaymentAmount.DbType = System.Data.DbType.Decimal;
                (o_totalPaymentAmount as IDbDataParameter).Scale = 2;
                o_totalPaymentAmount.Direction = System.Data.ParameterDirection.Output;
                System.Data.Common.DbParameter o_totalProfitAmount = dbOperator.AddParameter("@o_totalProfitAmount");
                o_totalProfitAmount.DbType = System.Data.DbType.Decimal;
                (o_totalProfitAmount as IDbDataParameter).Scale = 2;
                o_totalProfitAmount.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("dbo.P_QueryPlatformExternalOrderReport", System.Data.CommandType.StoredProcedure);
                if (paination != null && paination.GetRowCount)
                    paination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    //总计
                    totalReceiveAmount = o_totalReceiveAmount.Value == DBNull.Value ? 0 : (decimal)o_totalReceiveAmount.Value;
                    totalPaymentAmount = o_totalPaymentAmount.Value == DBNull.Value ? 0 : (decimal)o_totalPaymentAmount.Value;
                    totalProfitAmount = o_totalProfitAmount.Value == DBNull.Value ? 0 : (decimal)o_totalProfitAmount.Value;
                }
                else
                {
                    //总计
                    totalReceiveAmount = 0;
                    totalPaymentAmount = 0;
                    totalProfitAmount = 0;
                }
            }
            return dt;
        }


        public DataTable QueryRoyaltyProfit(Pagination pagination, RoyaltyProfitCondition condition, out decimal totalTradeFee, out decimal totalRoyalty, out decimal totalAmount)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginETDZTime", condition.ETDZStartDate.HasValue ? condition.ETDZStartDate.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endETDZTime", condition.ETDZEndDate.HasValue ? condition.ETDZEndDate.Value : (Nullable<DateTime>)null);
                if ((condition.Royalty.HasValue))
                    dbOperator.AddParameter("@i_royalty", condition.Royalty);
                if (!string.IsNullOrWhiteSpace(condition.PNR))
                    dbOperator.AddParameter("@i_PNR", condition.PNR);
                if (condition.OrderId.HasValue)
                    dbOperator.AddParameter("@i_orderId", condition.OrderId);
                if (!string.IsNullOrWhiteSpace(condition.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", condition.TicketNo);
                if (condition.PaymentType.HasValue)
                    dbOperator.AddParameter("@i_Type", condition.PaymentType);
                if (condition.IsSuccess.HasValue)
                    dbOperator.AddParameter("@i_payStatus", condition.IsSuccess);
                if (condition.IsPoolPay.HasValue)
                    dbOperator.AddParameter("@i_isPoolPay", condition.IsPoolPay);
                if (condition.PurchaseId.HasValue)
                    dbOperator.AddParameter("@i_purchaseId", condition.PurchaseId);
                if (condition.IncomeGroupId.HasValue)
                    dbOperator.AddParameter("@i_incomeGroupId", condition.IncomeGroupId);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", pagination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalTradeFee = dbOperator.AddParameter("@o_totalTradeFee");
                o_totalTradeFee.DbType = System.Data.DbType.Decimal;
                (o_totalTradeFee as IDbDataParameter).Scale = 2;
                o_totalTradeFee.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalTradeRoyalty = dbOperator.AddParameter("@o_totalTradeRoyalty");
                o_totalTradeRoyalty.DbType = System.Data.DbType.Decimal;
                (o_totalTradeRoyalty as IDbDataParameter).Scale = 2;
                o_totalTradeRoyalty.Direction = System.Data.ParameterDirection.Output;

                System.Data.Common.DbParameter o_totalTradeAmount = dbOperator.AddParameter("@o_totalTradeAmount");
                o_totalTradeAmount.DbType = System.Data.DbType.Decimal;
                (o_totalTradeAmount as IDbDataParameter).Scale = 2;
                o_totalTradeAmount.Direction = System.Data.ParameterDirection.Output;
                dt = dbOperator.ExecuteTable("dbo.P_QueryRoyaltyTicketReport", System.Data.CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
                if (dt.Rows.Count > 0)
                {
                    //总计
                    totalTradeFee = o_totalTradeFee.Value == DBNull.Value ? 0 : (decimal)o_totalTradeFee.Value;
                    totalRoyalty = o_totalTradeRoyalty.Value == DBNull.Value ? 0 : (decimal)o_totalTradeRoyalty.Value;
                    totalAmount = o_totalTradeAmount.Value == DBNull.Value ? 0 : (decimal)o_totalTradeAmount.Value;
                }
                else
                {
                    //总计
                    totalTradeFee = 0;
                    totalRoyalty = 0;
                    totalAmount = 0;
                }
            }
            return dt;
        }


        public DataTable QueryPlatformErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginApplyTime", condition.ApplyStartTime.HasValue ? condition.ApplyStartTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endApplyTime", condition.ApplyEndTime.HasValue ? condition.ApplyEndTime.Value : (Nullable<DateTime>)null);
                if (condition.OrderId.HasValue)
                    dbOperator.AddParameter("@i_orderId", condition.OrderId);
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                    dbOperator.AddParameter("@i_departure", condition.Departure);
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                    dbOperator.AddParameter("@i_arrival", condition.Arrival);
                if (!string.IsNullOrWhiteSpace(condition.Passenger))
                    dbOperator.AddParameter("@i_passenger",condition.Passenger);
                if (!string.IsNullOrWhiteSpace(condition.SettleCode))
                    dbOperator.AddParameter("@i_settleCode",condition.SettleCode);
                if (!string.IsNullOrWhiteSpace(condition.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo",condition.TicketNo);
                if (condition.ApplyformId.HasValue)
                    dbOperator.AddParameter("@i_applyformId", condition.ApplyformId);
                if (!string.IsNullOrWhiteSpace(condition.ApplierAccount))
                    dbOperator.AddParameter("@i_applierAccount", condition.ApplierAccount);
                if (!string.IsNullOrWhiteSpace(condition.ProcessAccount))
                    dbOperator.AddParameter("@i_processorAccount",condition.ProcessAccount);
                if (condition.Purchase.HasValue)
                    dbOperator.AddParameter("@i_purchaser", condition.Purchase);
                if (condition.Provider.HasValue)
                    dbOperator.AddParameter("@i_provider", condition.Provider);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", pagination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                dt = dbOperator.ExecuteTable("dbo.P_QueryPlatformErrorRefundReport", System.Data.CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return dt;
        }


        public DataTable QueryProviderErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginApplyTime", condition.ApplyStartTime.HasValue ? condition.ApplyStartTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endApplyTime", condition.ApplyEndTime.HasValue ? condition.ApplyEndTime.Value : (Nullable<DateTime>)null);
                if (condition.OrderId.HasValue)
                    dbOperator.AddParameter("@i_orderId", condition.OrderId);
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                    dbOperator.AddParameter("@i_departure", condition.Departure);
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                    dbOperator.AddParameter("@i_arrival", condition.Arrival);
                if (!string.IsNullOrWhiteSpace(condition.Passenger))
                    dbOperator.AddParameter("@i_passenger", condition.Passenger);
                if (!string.IsNullOrWhiteSpace(condition.SettleCode))
                    dbOperator.AddParameter("@i_settleCode", condition.SettleCode);
                if (!string.IsNullOrWhiteSpace(condition.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", condition.TicketNo);
                if (condition.ApplyformId.HasValue)
                    dbOperator.AddParameter("@i_applyformId", condition.ApplyformId);
                if (!string.IsNullOrWhiteSpace(condition.ApplierAccount))
                    dbOperator.AddParameter("@i_applierAccount", condition.ApplierAccount);
                if (!string.IsNullOrWhiteSpace(condition.ProcessAccount))
                    dbOperator.AddParameter("@i_processorAccount", condition.ProcessAccount);
                if (condition.Purchase.HasValue)
                    dbOperator.AddParameter("@i_purchaser", condition.Purchase);
                if (condition.Provider.HasValue)
                    dbOperator.AddParameter("@i_provider", condition.Provider);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", pagination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                dt = dbOperator.ExecuteTable("dbo.P_QueryProviderErrorRefundReport", System.Data.CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return dt;
        }

        public DataTable QueryPurchaseErrorRefund(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginApplyTime", condition.ApplyStartTime.HasValue ? condition.ApplyStartTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endApplyTime", condition.ApplyEndTime.HasValue ? condition.ApplyEndTime.Value : (Nullable<DateTime>)null);
                if (condition.OrderId.HasValue)
                    dbOperator.AddParameter("@i_orderId", condition.OrderId);
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                    dbOperator.AddParameter("@i_departure", condition.Departure);
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                    dbOperator.AddParameter("@i_arrival", condition.Arrival);
                if (!string.IsNullOrWhiteSpace(condition.Passenger))
                    dbOperator.AddParameter("@i_passenger", condition.Passenger);
                if (!string.IsNullOrWhiteSpace(condition.SettleCode))
                    dbOperator.AddParameter("@i_settleCode", condition.SettleCode);
                if (!string.IsNullOrWhiteSpace(condition.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", condition.TicketNo);
                if (condition.ApplyformId.HasValue)
                    dbOperator.AddParameter("@i_applyformId", condition.ApplyformId);
                if (!string.IsNullOrWhiteSpace(condition.ApplierAccount))
                    dbOperator.AddParameter("@i_applierAccount", condition.ApplierAccount);
                if (!string.IsNullOrWhiteSpace(condition.ProcessAccount))
                    dbOperator.AddParameter("@i_processorAccount", condition.ProcessAccount);
                if (condition.Purchase.HasValue)
                    dbOperator.AddParameter("@i_purchaser", condition.Purchase);
                if (condition.Provider.HasValue)
                    dbOperator.AddParameter("@i_provider", condition.Provider);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", pagination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                dt = dbOperator.ExecuteTable("dbo.P_QueryPurchaseErrorRefundReport", System.Data.CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return dt;
        }


        public DataTable QueryPurchaseErrorRefundFinancial(Pagination pagination, ErrorRefundQueryCondition condition)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginApplyTime", condition.ApplyStartTime.HasValue ? condition.ApplyStartTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endApplyTime", condition.ApplyEndTime.HasValue ? condition.ApplyEndTime.Value : (Nullable<DateTime>)null);
                if (condition.OrderId.HasValue)
                    dbOperator.AddParameter("@i_orderId", condition.OrderId);
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                    dbOperator.AddParameter("@i_departure", condition.Departure);
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                    dbOperator.AddParameter("@i_arrival", condition.Arrival);
                if (!string.IsNullOrWhiteSpace(condition.Passenger))
                    dbOperator.AddParameter("@i_passenger", condition.Passenger);
                if (!string.IsNullOrWhiteSpace(condition.SettleCode))
                    dbOperator.AddParameter("@i_settleCode", condition.SettleCode);
                if (!string.IsNullOrWhiteSpace(condition.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", condition.TicketNo);
                if (condition.ApplyformId.HasValue)
                    dbOperator.AddParameter("@i_applyformId", condition.ApplyformId);
                if (!string.IsNullOrWhiteSpace(condition.ApplierAccount))
                    dbOperator.AddParameter("@i_applierAccount", condition.ApplierAccount);
                if (!string.IsNullOrWhiteSpace(condition.ProcessAccount))
                    dbOperator.AddParameter("@i_processorAccount", condition.ProcessAccount);
                if (condition.Purchase.HasValue)
                    dbOperator.AddParameter("@i_purchaser", condition.Purchase);
                if (condition.Provider.HasValue)
                    dbOperator.AddParameter("@i_provider", condition.Provider);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", pagination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                dt = dbOperator.ExecuteTable("dbo.P_QueryPurchaseErrorRefundFinancialReport", System.Data.CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return dt;
        }

        public DataTable QueryProviderErrorRefundFinancial(Pagination paginantion, ErrorRefundQueryCondition condition)
        {
            System.Data.DataTable dt = null;
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_beginApplyTime", condition.ApplyStartTime.HasValue ? condition.ApplyStartTime.Value : (Nullable<DateTime>)null);
                dbOperator.AddParameter("@i_endApplyTime", condition.ApplyEndTime.HasValue ? condition.ApplyEndTime.Value : (Nullable<DateTime>)null);
                if (condition.OrderId.HasValue)
                    dbOperator.AddParameter("@i_orderId", condition.OrderId);
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                    dbOperator.AddParameter("@i_departure", condition.Departure);
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                    dbOperator.AddParameter("@i_arrival", condition.Arrival);
                if (!string.IsNullOrWhiteSpace(condition.Passenger))
                    dbOperator.AddParameter("@i_passenger", condition.Passenger);
                if (!string.IsNullOrWhiteSpace(condition.SettleCode))
                    dbOperator.AddParameter("@i_settleCode", condition.SettleCode);
                if (!string.IsNullOrWhiteSpace(condition.TicketNo))
                    dbOperator.AddParameter("@i_ticketNo", condition.TicketNo);
                if (condition.ApplyformId.HasValue)
                    dbOperator.AddParameter("@i_applyformId", condition.ApplyformId);
                if (!string.IsNullOrWhiteSpace(condition.ApplierAccount))
                    dbOperator.AddParameter("@i_applierAccount", condition.ApplierAccount);
                if (!string.IsNullOrWhiteSpace(condition.ProcessAccount))
                    dbOperator.AddParameter("@i_processorAccount", condition.ProcessAccount);
                if (condition.Purchase.HasValue)
                    dbOperator.AddParameter("@i_purchaser", condition.Purchase);
                if (condition.Provider.HasValue)
                    dbOperator.AddParameter("@i_provider", condition.Provider);
                if (paginantion != null)
                {
                    dbOperator.AddParameter("@i_pageSize", paginantion.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", paginantion.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                dt = dbOperator.ExecuteTable("dbo.P_QueryProviderErrorRefundFinancialReport", System.Data.CommandType.StoredProcedure);
                if (paginantion != null && paginantion.GetRowCount)
                    paginantion.RowCount = (int)totalCount.Value;
            }
            return dt;
        }
    }
}