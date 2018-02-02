using System.Collections.Generic;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer {
    class RefundRepository : SqlServerRepository, IRefundRepository {
        public RefundRepository(string connectionString)
            : base(connectionString) {
        }

        public void Save(RefundFailedRecord record) {
            var sql = "IF EXISTS(SELECT NULL FROM dbo.T_RefundFailedInfo WHERE ApplyformId=@ApplyformId) "
                + "UPDATE dbo.T_RefundFailedInfo SET ErrorMsg=@ErrorMsg WHERE ApplyformId=@ApplyformId "
                + "ELSE INSERT INTO dbo.T_RefundFailedInfo (OrderId,ApplyformId,BusinessType,PayTradeNo,RefundTime,ErrorMsg) VALUES "
                + "(@OrderId,@ApplyformId,@BusinessType,@PayTradeNo,@RefundTime,@ErrorMsg)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("OrderId", record.OrderId);
                dbOperator.AddParameter("ApplyformId", record.ApplyformId);
                dbOperator.AddParameter("BusinessType", (byte)record.BusinessType);
                dbOperator.AddParameter("PayTradeNo", record.PayTradeNo);
                dbOperator.AddParameter("RefundTime", record.RefundTime);
                dbOperator.AddParameter("ErrorMsg", record.RefundFailedInfo);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Delete(decimal applyformId) {
            var sql = "DELETE FROM dbo.T_RefundFailedInfo WHERE ApplyformId=@ApplyformId";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ApplyformId", applyformId);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(RefundFailedRecord record) {
            var sql = "INSERT INTO dbo.T_RefundFailedInfo ErrorMsg=@ErrorMsg WHERE ApplyformId=@ApplyformId";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ApplyformId", record.ApplyformId);
                dbOperator.AddParameter("ErrorMsg", record.RefundFailedInfo);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<RefundFailedRecord> Query(RefundFailedRecordQueryCondition condition, Pagination pagination) {
            var result = new List<RefundFailedRecord>();
            var fields = "OrderId,ApplyformId,BusinessType,PayTradeNo,RefundTime,ErrorMsg";
            var catelog = "dbo.T_RefundFailedInfo";
            var orderbyFiled = "RefundTime";
            var where = new StringBuilder();
            if(condition.OrderId.HasValue) {
                where.AppendFormat("OrderId={0} AND ", condition.OrderId.Value);
            }
            if(condition.ApplyformId.HasValue) {
                where.AppendFormat("ApplyformId={0} AND ", condition.ApplyformId.Value);
            }
            if(condition.BusinessType.HasValue) {
                where.AppendFormat("BusinessType={0} AND ", (byte)condition.BusinessType.Value);
            }
            where.AppendFormat("RefundTime BETWEEN '{0}' AND '{1}'", condition.RefundDateRange.Lower.Date, condition.RefundDateRange.Upper.Date.AddDays(1).AddTicks(-1));
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
                        result.Add(new RefundFailedRecord() {
                            OrderId = reader.GetDecimal(0),
                            ApplyformId = reader.GetDecimal(1),
                            BusinessType = (RefundBusinessType)reader.GetByte(2),
                            PayTradeNo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            RefundTime = reader.GetDateTime(4),
                            RefundFailedInfo = reader.GetString(5)
                        });
                    }
                }
                if(pagination.GetRowCount) {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }
        
        public IEnumerable<RefundFailedRecord> Query() {
            var result = new List<RefundFailedRecord>();
            var sql = "SELECT OrderId,ApplyformId,BusinessType,PayTradeNo,RefundTime,ErrorMsg FROM dbo.T_RefundFailedInfo";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new RefundFailedRecord() {
                            OrderId = reader.GetDecimal(0),
                            ApplyformId = reader.GetDecimal(1),
                            BusinessType = (RefundBusinessType)reader.GetByte(2),
                            PayTradeNo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            RefundTime = reader.GetDateTime(4),
                            RefundFailedInfo = reader.GetString(5)
                        });
                    }
                }
            }
            return result;
        }

        public RefundFailedRecord Query(decimal applyformId) {
            var sql = "SELECT OrderId,ApplyformId,BusinessType,PayTradeNo,RefundTime,ErrorMsg FROM dbo.T_RefundFailedInfo WHERE ApplyformId=@ApplyformId";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ApplyformId", applyformId);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        return new RefundFailedRecord() {
                            OrderId = reader.GetDecimal(0),
                            ApplyformId = reader.GetDecimal(1),
                            BusinessType = (RefundBusinessType)reader.GetByte(2),
                            PayTradeNo = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            RefundTime = reader.GetDateTime(4),
                            RefundFailedInfo = reader.GetString(5)
                        };
                    }
                }
            }
            return null;
        }
    }
}
