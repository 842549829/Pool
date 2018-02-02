using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    internal class AirlineRepository : SqlServerRepository, IAirlineRepository {
        public AirlineRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(Airline value) {
            string sql = "DELETE FROM [T_Airline] WHERE [CODE]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code.Value);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(Airline value) {
            string sql = "INSERT INTO [T_Airline]([CODE],[NAME],[SHORTNAME],[SETTLECODE],[VALID]) VALUES (@CODE,@NAME,@SHORTNAME,@SETTLECODE,@VALID) ";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code.Value);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SHORTNAME", value.ShortName);
                dbOperator.AddParameter("SETTLECODE", value.SettleCode);
                dbOperator.AddParameter("VALID", value.Valid);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<UpperString, Airline>> Query() {
            string sql = "SELECT [CODE],[NAME],[SHORTNAME],[SETTLECODE],[VALID] FROM [T_Airline] ORDER BY CODE";
            var result = new List<KeyValuePair<UpperString, Airline>>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        Airline item = new Airline(reader.GetString(0));
                        item.Name = reader.GetString(1);
                        item.ShortName = reader.GetString(2);
                        item.SettleCode = reader.GetString(3);
                        item.Valid = (bool)reader.GetValue(4);
                        result.Add(new KeyValuePair<UpperString, Airline>(item.Code, item));
                    }
                }
            }
            return result;
        }

        public int Update(Airline value) {
            string sql = "UPDATE [T_Airline] SET [NAME]=@NAME,[SHORTNAME]=@SHORTNAME,[SETTLECODE]=@SETTLECODE,[VALID]=@VALID WHERE [CODE]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SHORTNAME", value.ShortName);
                dbOperator.AddParameter("SETTLECODE", value.SettleCode);
                dbOperator.AddParameter("VALID", value.Valid);
                dbOperator.AddParameter("CODE", value.Code.Value);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
