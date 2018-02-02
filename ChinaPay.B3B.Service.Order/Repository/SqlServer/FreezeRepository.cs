using System;
using System.Collections.Generic;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer {
    class FreezeRepository : SqlServerRepository, IFreezeRepository {
        public FreezeRepository(string connectionString)
            : base(connectionString) {
        }

        public void Save(FreezeInfo info) {
            var sql = "INSERT INTO dbo.T_OrderFreeze(Id,OrderId,ApplyformId,TradeNo,Account,Amount,FreezeNo," +
            "RequestTime,Success,ProcessTime,[Type],Remark) VALUES (@Id,@OrderId,@ApplyformId,@TradeNo,@Account,@Amount,@FreezeNo," +
            "@RequestTime,@Success,@ProcessTime,@Type,@Remark)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", info.Id);
                dbOperator.AddParameter("OrderId", info.OrderId);
                dbOperator.AddParameter("ApplyformId", info.ApplyformId);
                dbOperator.AddParameter("TradeNo", info.TradeNo);
                dbOperator.AddParameter("Account", info.Account);
                dbOperator.AddParameter("Amount", info.Amount);
                if(string.IsNullOrWhiteSpace(info.No)) {
                    dbOperator.AddParameter("FreezeNo", DBNull.Value);
                } else {
                    dbOperator.AddParameter("FreezeNo", info.No);
                }
                dbOperator.AddParameter("RequestTime", info.RequestTime);
                dbOperator.AddParameter("Success", info.Success);
                if(info.ProcessedTime.HasValue) {
                    dbOperator.AddParameter("ProcessTime", info.ProcessedTime.Value);
                } else {
                    dbOperator.AddParameter("ProcessTime", DBNull.Value);
                }
                dbOperator.AddParameter("Type", (byte)FreezeType.Freeze);
                dbOperator.AddParameter("Remark", info.Remark ?? string.Empty);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Save(UnfreezeInfo info) {
            var sql = "INSERT INTO dbo.T_OrderFreeze(Id,OrderId,ApplyformId,Account,Amount,FreezeNo,UnfreezeNo," +
            "RequestTime,Success,ProcessTime,[Type],Remark) VALUES (@Id,@OrderId,@ApplyformId,@Account,@Amount,@FreezeNo,@UnfreezeNo," +
            "@RequestTime,@Success,@ProcessTime,@Type,@Remark)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", info.Id);
                dbOperator.AddParameter("OrderId", info.OrderId);
                dbOperator.AddParameter("ApplyformId", info.ApplyformId);
                dbOperator.AddParameter("Account", info.Account);
                dbOperator.AddParameter("Amount", info.Amount);
                if(string.IsNullOrWhiteSpace(info.FreezeNo)) {
                    dbOperator.AddParameter("FreezeNo", DBNull.Value);
                } else {
                    dbOperator.AddParameter("FreezeNo", info.FreezeNo);
                }
                if(string.IsNullOrWhiteSpace(info.No)) {
                    dbOperator.AddParameter("UnfreezeNo", DBNull.Value);
                } else {
                    dbOperator.AddParameter("UnfreezeNo", info.No);
                }
                dbOperator.AddParameter("RequestTime", info.RequestTime);
                dbOperator.AddParameter("Success", info.Success);
                if(info.ProcessedTime.HasValue) {
                    dbOperator.AddParameter("ProcessTime", info.ProcessedTime.Value);
                } else {
                    dbOperator.AddParameter("ProcessTime", DBNull.Value);
                }
                dbOperator.AddParameter("Type", (byte)FreezeType.Unfreeze);
                dbOperator.AddParameter("Remark", info.Remark ?? string.Empty);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(UnfreezeInfo info) {
            var sql = "UPDATE dbo.T_OrderFreeze SET UnfreezeNo=@UnfreezeNo,Success=@Success,ProcessTime=@ProcessTime,Remark=@Remark WHERE Id=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(string.IsNullOrWhiteSpace(info.No)) {
                    dbOperator.AddParameter("UnfreezeNo", DBNull.Value);
                } else {
                    dbOperator.AddParameter("UnfreezeNo", info.No);
                }
                dbOperator.AddParameter("Success", info.Success);
                if(info.ProcessedTime.HasValue) {
                    dbOperator.AddParameter("ProcessTime", info.ProcessedTime.Value);
                } else {
                    dbOperator.AddParameter("ProcessTime", DBNull.Value);
                }
                dbOperator.AddParameter("Remark", info.Remark ?? string.Empty);
                dbOperator.AddParameter("Id", info.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public FreezeInfo QueryFreezeInfo(decimal applyformId) {
            FreezeInfo result = null;
            var sql = "SELECT Id,OrderId,TradeNo,Account,Amount,FreezeNo,RequestTime,Success,ProcessTime,Remark FROM dbo.T_OrderFreeze"
                + " WHERE ApplyformId=@ApplyformId AND [Type]=@Type";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ApplyformId", applyformId);
                dbOperator.AddParameter("Type", (byte)FreezeType.Freeze);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        result = new FreezeInfo(reader.GetGuid(0)) {
                            OrderId = reader.GetDecimal(1),
                            ApplyformId = applyformId,
                            TradeNo = reader.GetString(2),
                            Account = reader.GetString(3),
                            Amount = reader.GetDecimal(4),
                            No = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            RequestTime = reader.GetDateTime(6),
                            Success = reader.GetBoolean(7),
                            Remark = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
                        };
                        if(!reader.IsDBNull(8)) {
                            result.ProcessedTime = reader.GetDateTime(8);
                        }
                    }
                }
            }
            return result;
        }

        public UnfreezeInfo QueryUnfreezeInfo(string freezeNo) {
            UnfreezeInfo result = null;
            var sql = "SELECT Id,OrderId,ApplyformId,Account,Amount,UnfreezeNo,RequestTime,Success,ProcessTime,Remark FROM dbo.T_OrderFreeze"
                + " WHERE FreezeNo=@FreezeNo AND [Type]=@Type";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("FreezeNo", freezeNo);
                dbOperator.AddParameter("Type", (byte)FreezeType.Unfreeze);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        result = new UnfreezeInfo(reader.GetGuid(0)) {
                            OrderId = reader.GetDecimal(1),
                            ApplyformId = reader.GetDecimal(2),
                            Account = reader.GetString(3),
                            Amount = reader.GetDecimal(4),
                            FreezeNo = freezeNo,
                            No = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            RequestTime = reader.GetDateTime(6),
                            Success = reader.GetBoolean(7),
                            Remark = reader.IsDBNull(9) ? string.Empty : reader.GetString(9)
                        };
                        if(!reader.IsDBNull(8)) {
                            result.ProcessedTime = reader.GetDateTime(8);
                        }
                    }
                }
            }
            return result;
        }

        public IEnumerable<FreezeBaseInfo> Query(FreezeQueryCondition condition, Pagination pagination) {
            var result = new List<FreezeBaseInfo>();
            var fields = "[Type],Id,OrderId,ApplyformId,TradeNo,Account,Amount,FreezeNo,UnfreezeNo,RequestTime,Success,ProcessTime,Remark";
            var catelog = "dbo.T_OrderFreeze";
            var orderbyFiled = "ApplyformId DESC,RequestTime";
            var where = new StringBuilder();
            if(condition.OrderId.HasValue) {
                where.AppendFormat("OrderId={0} AND ", condition.OrderId.Value);
            }
            if(condition.ApplyformId.HasValue) {
                where.AppendFormat("ApplyformId={0} AND ", condition.ApplyformId.Value);
            }
            if(condition.Type.HasValue) {
                where.AppendFormat("[Type]={0} AND ", (byte)condition.Type.Value);
            }
            if(condition.Success.HasValue) {
                where.AppendFormat("Success={0} AND ", condition.Success.Value?1:0);
            }
            where.AppendFormat("RequestTime BETWEEN '{0}' AND '{1}'", condition.RequestDate.Lower.Date, condition.RequestDate.Upper.Date.AddDays(1).AddTicks(-1));
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
                        FreezeBaseInfo item = null;
                        var type = (FreezeType)reader.GetByte(0);
                        if(type == FreezeType.Freeze) {
                            item = new FreezeInfo(reader.GetGuid(1)) {
                                TradeNo = getString(reader["TradeNo"]),
                                No = getString(reader["FreezeNo"])
                            };
                        } else {
                            item = new UnfreezeInfo(reader.GetGuid(1)) {
                                FreezeNo = getString(reader["FreezeNo"]),
                                No = getString(reader["UnfreezeNo"])
                            };
                        }
                        item.OrderId = reader.GetDecimal(2);
                        item.ApplyformId = reader.GetDecimal(3);
                        item.Account = reader.GetString(5);
                        item.Amount = reader.GetDecimal(6);
                        item.RequestTime = reader.GetDateTime(9);
                        item.Success = reader.GetBoolean(10);
                        if(!reader.IsDBNull(11)) {
                            item.ProcessedTime = reader.GetDateTime(11);
                        }
                        item.Remark = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                        result.Add(item);
                    }
                }
                if(pagination.GetRowCount) {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }
        private string getString(object obj) {
            return obj == null ? string.Empty : obj.ToString();
        }
    }
}