using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    class RefundAndReschedulingRepository : SqlServerRepository, IRefundAndReschedulingRepository {
        public RefundAndReschedulingRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<RefundAndRescheduling> Query() {
            string sql = "SELECT [AIRLINE],[REFUND],[SCRAP],[CHANGE],[AIRLINETEL],[REMARK],[LEVEL] FROM [T_RefundAndRescheduling]";
            var result = new List<RefundAndRescheduling>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        RefundAndRescheduling item = new RefundAndRescheduling(reader.GetString(0));
                        item.Refund = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        item.Scrap = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        item.Change = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        item.AirlineTel = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        item.Remark = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        item.Level = reader.GetInt32(6);
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public RefundAndRescheduling Query(UpperString airline) {
            RefundAndRescheduling result = null;
            string sql = "SELECT [REFUND],[SCRAP],[CHANGE],[AIRLINETEL],[REMARK],[LEVEL] FROM [T_RefundAndRescheduling] WHERE [AIRLINE]=@AIRLINE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("AIRLINE", airline.Value);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result = new RefundAndRescheduling(airline);
                        result.Refund = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        result.Scrap = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        result.Change = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        result.AirlineTel = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        result.Remark = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        result.Level = reader.GetInt32(5);
                    }
                }
            }
            return result;
        }

        public int Insert(RefundAndRescheduling item) {
            string sql = "INSERT INTO [T_RefundAndRescheduling]([AIRLINE],[REFUND],[SCRAP],[CHANGE],[AIRLINETEL],[REMARK],[LEVEL]) VALUES (@AIRLINE,@REFUND,@SCRAP,@CHANGE,@AIRLINETEL,@REMARK,@LEVEL)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("AIRLINE", item.AirlineCode.Value);
                if(string.IsNullOrEmpty(item.Refund)) {
                    dbOperator.AddParameter("REFUND", DBNull.Value);
                } else {
                    dbOperator.AddParameter("REFUND", item.Refund);
                }
                if(string.IsNullOrEmpty(item.Scrap)) {
                    dbOperator.AddParameter("SCRAP", DBNull.Value);
                } else {
                    dbOperator.AddParameter("SCRAP", item.Scrap);
                }
                if(string.IsNullOrEmpty(item.Change)) {
                    dbOperator.AddParameter("CHANGE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("CHANGE", item.Change);
                }
                if(string.IsNullOrEmpty(item.AirlineTel)) {
                    dbOperator.AddParameter("AIRLINETEL", DBNull.Value);
                } else {
                    dbOperator.AddParameter("AIRLINETEL", item.AirlineTel);
                }
                if(string.IsNullOrEmpty(item.Remark)) {
                    dbOperator.AddParameter("REMARK", DBNull.Value);
                } else {
                    dbOperator.AddParameter("REMARK", item.Remark);
                }
                dbOperator.AddParameter("LEVEL", item.Level);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(RefundAndRescheduling item) {
            string sql = "UPDATE [T_RefundAndRescheduling] SET [REFUND]=@REFUND,[SCRAP]=@SCRAP,[CHANGE]=@CHANGE,[AIRLINETEL]=@AIRLINETEL,[REMARK]=@REMARK,[LEVEL]=@LEVEL WHERE [AIRLINE]=@AIRLINE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("AIRLINE", item.AirlineCode.Value);
                if(string.IsNullOrEmpty(item.Refund)) {
                    dbOperator.AddParameter("REFUND", DBNull.Value);
                } else {
                    dbOperator.AddParameter("REFUND", item.Refund);
                }
                if(string.IsNullOrEmpty(item.Scrap)) {
                    dbOperator.AddParameter("SCRAP", DBNull.Value);
                } else {
                    dbOperator.AddParameter("SCRAP", item.Scrap);
                }
                if(string.IsNullOrEmpty(item.Change)) {
                    dbOperator.AddParameter("CHANGE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("CHANGE", item.Change);
                }
                if(string.IsNullOrEmpty(item.AirlineTel)) {
                    dbOperator.AddParameter("AIRLINETEL", DBNull.Value);
                } else {
                    dbOperator.AddParameter("AIRLINETEL", item.AirlineTel);
                }
                if(string.IsNullOrEmpty(item.Remark)) {
                    dbOperator.AddParameter("REMARK", DBNull.Value);
                } else {
                    dbOperator.AddParameter("REMARK", item.Remark);
                }
                dbOperator.AddParameter("LEVEL", item.Level);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(UpperString airlineCode) {
            string sql = "DELETE FROM [T_RefundAndRescheduling] WHERE [AIRLINE]=@AIRLINE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("AIRLINE", airlineCode.Value);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}