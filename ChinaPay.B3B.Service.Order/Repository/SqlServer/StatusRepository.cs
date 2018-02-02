using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer {
    class StatusRepository : SqlServerRepository, IStatusRepository {
        public StatusRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<Domain.StatusInfo<DataTransferObject.Order.OrderStatus>> GetOrderStatusInfos() {
            return GetStatusInfos(1, statusCode => (DataTransferObject.Order.OrderStatus)statusCode);
        }

        public IEnumerable<Domain.StatusInfo<DataTransferObject.Order.RefundApplyformStatus>> GetRefundApplyformStatusInfos() {
            return GetStatusInfos(2, statusCode => (DataTransferObject.Order.RefundApplyformStatus)statusCode);
        }


        public IEnumerable<Domain.StatusInfo<DataTransferObject.Order.PostponeApplyformStatus>> GetPostponeApplyformStatusInfos() {
            return GetStatusInfos(3, statusCode => (DataTransferObject.Order.PostponeApplyformStatus)statusCode);
        }

        public IEnumerable<StatusInfo<BalanceRefundProcessStatus>> GetBalanceRefundApplyformStatusInfos()
        {
            return GetStatusInfos(4, statusCode => (DataTransferObject.Order.BalanceRefundProcessStatus)statusCode);
        }

        public IEnumerable<Domain.StatusInfo<TSystemStatus>> GetStatusInfos<TSystemStatus>(byte statusType, Func<int, TSystemStatus> statusAdapter) {
            var result = new List<Domain.StatusInfo<TSystemStatus>>();
            var sql = "SELECT [System],[Platform],Purchaser,Supplier,Provider,DistributionOEM FROM dbo.T_OrderStatus WHERE [Type]=@TYPE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("TYPE", statusType);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new Domain.StatusInfo<TSystemStatus>() {
                            System = statusAdapter(reader.GetInt32(0)),
                            Platform = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Purchaser = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Supplier = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Provider = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            DistributionOEM = reader.IsDBNull(5)?string.Empty:reader.GetString(5)
                        });
                    }
                }
            }
            return result;
        }
    }
}