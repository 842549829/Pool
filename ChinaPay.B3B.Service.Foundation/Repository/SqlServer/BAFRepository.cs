using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    class BAFRepository : SqlServerRepository, IBAFRepository {
        public BAFRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(BAF value) {
            string sql = "DELETE FROM [T_BAF] WHERE [ID]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(BAF value) {
            string sql = "INSERT INTO [T_BAF]([ID],[AIRLINE],[MILEAGE],[ADULT],[CHILD],[EFFECTIVEDATE],[EXPIREDDATE]) VALUES(@ID,@AIRLINE,@MILEAGE,@ADULT,@CHILD,@EFFECTIVEDATE,@EXPIREDDATE)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                if(value.AirlineCode.IsNullOrEmpty()) {
                    dbOperator.AddParameter("AIRLINE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("AIRLINE", value.AirlineCode.Value);
                }
                dbOperator.AddParameter("MILEAGE", value.Mileage);
                dbOperator.AddParameter("ADULT", value.Adult);
                dbOperator.AddParameter("CHILD", value.Child);
                dbOperator.AddParameter("EFFECTIVEDATE", value.EffectiveDate);
                if(value.ExpiredDate.HasValue) {
                    dbOperator.AddParameter("EXPIREDDATE", value.ExpiredDate.Value);
                } else {
                    dbOperator.AddParameter("EXPIREDDATE", DBNull.Value);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<Guid, BAF>> Query() {
            string sql = "SELECT [ID],[AIRLINE],[MILEAGE],[ADULT],[CHILD],[EFFECTIVEDATE],[EXPIREDDATE] FROM [T_BAF]";
            var result = new List<KeyValuePair<Guid, BAF>>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        BAF item = new BAF(reader.GetGuid(0));
                        item.AirlineCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        item.Mileage = reader.GetDecimal(2);
                        item.Adult = reader.GetDecimal(3);
                        item.Child = reader.GetDecimal(4);
                        item.EffectiveDate = reader.GetDateTime(5);
                        if(!reader.IsDBNull(6)) {
                            item.ExpiredDate = reader.GetDateTime(6);
                        }
                        result.Add(new KeyValuePair<Guid, BAF>(item.Id, item));
                    }
                }
            }
            return result;
        }

        public int Update(BAF value) {
            string sql = "UPDATE [T_BAF] SET [AIRLINE]=@AIRLINE,[MILEAGE]=@MILEAGE,[ADULT]=@ADULT,[CHILD]=@CHILD,[EFFECTIVEDATE]=@EFFECTIVEDATE,[EXPIREDDATE]=@EXPIREDDATE WHERE ID=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(value.AirlineCode.IsNullOrEmpty()) {
                    dbOperator.AddParameter("AIRLINE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("AIRLINE", value.AirlineCode.Value);
                }
                dbOperator.AddParameter("MILEAGE", value.Mileage);
                dbOperator.AddParameter("ADULT", value.Adult);
                dbOperator.AddParameter("CHILD", value.Child);
                dbOperator.AddParameter("EFFECTIVEDATE", value.EffectiveDate);
                dbOperator.AddParameter("ID", value.Id);
                if(value.ExpiredDate.HasValue) {
                    dbOperator.AddParameter("EXPIREDDATE", value.ExpiredDate.Value);
                } else {
                    dbOperator.AddParameter("EXPIREDDATE", DBNull.Value);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}