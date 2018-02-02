using System.Collections.Generic;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer {
    class RoyaltyRepository : SqlServerRepository, IRoyaltyRepository {
        public RoyaltyRepository(string connectionString)
            : base(connectionString) {
        }

        public void Save(RoyaltyFailedRecord record) {
            var sql = "IF EXISTS(SELECT NULL FROM dbo.T_RoyaltyFailedInfo WHERE OrderId=@OrderId) "
                      + "UPDATE dbo.T_RoyaltyFailedInfo SET RoyaltyInfo=@RoyaltyInfo,ErrorMsg=@ErrorMsg WHERE OrderId=@OrderId "
                      + "ELSE INSERT INTO dbo.T_RoyaltyFailedInfo (OrderId,PayTime,ETDZTime,TradeAmount,RoyaltyInfo,ErrorMsg) VALUES "
                      + "(@OrderId,@PayTime,@ETDZTime,@TradeAmount,@RoyaltyInfo,@ErrorMsg)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("OrderId", record.OrderId);
                dbOperator.AddParameter("PayTime", record.PayTime);
                dbOperator.AddParameter("ETDZTime", record.ETDZTime);
                dbOperator.AddParameter("TradeAmount", record.TradeAmount);
                dbOperator.AddParameter("RoyaltyInfo", record.RoyaltyInfo);
                dbOperator.AddParameter("ErrorMsg", record.FailedReason);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Delete(decimal orderId) {
            var sql = "DELETE FROM dbo.T_RoyaltyFailedInfo WHERE OrderId=@OrderId";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("OrderId", orderId);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<RoyaltyFailedRecord> Query(RoyaltyFailedRecordQueryCondition condition, Pagination pagination) {
            var result = new List<RoyaltyFailedRecord>();
            var fields = "OrderId,PayTime,ETDZTime,TradeAmount,RoyaltyInfo,ErrorMsg";
            var catelog = "dbo.T_RoyaltyFailedInfo";
            var orderbyFiled = "ETDZTime";
            var where = new StringBuilder();
            if(condition.OrderId.HasValue) {
                where.AppendFormat("OrderId={0} AND ", condition.OrderId.Value);
            }
            where.AppendFormat("ETDZTime BETWEEN '{0}' AND '{1}'", condition.ETDZDateRange.Lower.Date, condition.ETDZDateRange.Upper.Date.AddDays(1).AddTicks(-1));
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("@iField", fields);
                dbOperator.AddParameter("@iCatelog", catelog);
                dbOperator.AddParameter("@iCondition", where.ToString());
                dbOperator.AddParameter("@iOrderBy", orderbyFiled);
                dbOperator.AddParameter("@iPagesize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                dbOperator.AddParameter("@iGetCount", pagination.GetRowCount);
                var totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using(var reader = dbOperator.ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure)) {
                    while(reader.Read()) {
                        result.Add(new RoyaltyFailedRecord() {
                            OrderId = reader.GetDecimal(0),
                            PayTime = reader.GetDateTime(1),
                            ETDZTime = reader.GetDateTime(2),
                            TradeAmount = reader.GetDecimal(3),
                            RoyaltyInfo = reader.GetString(4),
                            FailedReason = reader.GetString(5)
                        });
                    }
                }
                if(pagination.GetRowCount) {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }
        public IEnumerable<decimal> Query() {
            var result = new List<decimal>();
            var sql = "SELECT OrderId FROM dbo.T_RoyaltyFailedInfo";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(reader.GetDecimal(0));
                    }
                }
            }
            return result;
        }
    }
}
