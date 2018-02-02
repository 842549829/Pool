using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    internal class CityRepository : SqlServerRepository, ICityRepository {
        public CityRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(City value) {
            string sql = "DELETE FROM [T_City] WHERE [CODE]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(City value) {
            string sql = "INSERT INTO [T_City]([CODE],[NAME],[SPELLING],[SHORTSPELLING],[HOTLEVEL],[PROVINCE]) VALUES(@CODE,@NAME,@SPELLING,@SHORTSPELLING,@HOTLEVEL,@PROVINCE)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SPELLING", value.Spelling ?? string.Empty);
                dbOperator.AddParameter("SHORTSPELLING", value.ShortSpelling ?? string.Empty);
                dbOperator.AddParameter("HOTLEVEL", value.HotLevel);
                dbOperator.AddParameter("PROVINCE", value.ProvinceCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<string, City>> Query() {
            var result = new List<KeyValuePair<string, City>>();
            string sql = "SELECT [CODE],[NAME],[SPELLING],[SHORTSPELLING],[HOTLEVEL],[PROVINCE] FROM [T_City]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        City item = new City(reader.GetString(0).Trim());
                        item.Name = reader.GetString(1);
                        item.Spelling = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        item.ShortSpelling = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        item.HotLevel = reader.IsDBNull(4) ? 0: reader.GetInt32(4);
                        item.ProvinceCode = reader.GetString(5);
                        result.Add(new KeyValuePair<string, City>(item.Code, item));
                    }
                }
            }
            return result;
        }

        public int Update(City value) {
            string sql = "UPDATE T_City SET [NAME]=@NAME,[SPELLING]=@SPELLING,[SHORTSPELLING]=@SHORTSPELLING,[HOTLEVEL]=@HOTLEVEL,[PROVINCE]=@PROVINCE WHERE [CODE]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SPELLING", value.Spelling ?? string.Empty);
                dbOperator.AddParameter("SHORTSPELLING", value.ShortSpelling ?? string.Empty);
                dbOperator.AddParameter("HOTLEVEL", value.HotLevel);
                dbOperator.AddParameter("PROVINCE", value.ProvinceCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}