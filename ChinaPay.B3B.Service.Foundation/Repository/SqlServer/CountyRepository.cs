using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    internal class CountyRepository : SqlServerRepository, ICountyRepository {
        public CountyRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(County value) {
            string sql = "DELETE FROM [T_County] WHERE [CODE]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(County value) {
            string sql = "INSERT INTO [T_County]([CODE],[NAME],[SPELLING],[SHORTSPELLING],[HOTLEVEL],[CITY]) VALUES(@CODE,@NAME,@SPELLING,@SHORTSPELLING,@HOTLEVEL,@CITY)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SPELLING", value.Spelling ?? string.Empty);
                dbOperator.AddParameter("SHORTSPELLING", value.ShortSpelling ?? string.Empty);
                dbOperator.AddParameter("HOTLEVEL", value.HotLevel);
                dbOperator.AddParameter("CITY", value.CityCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<string, County>> Query() {
            var result = new List<KeyValuePair<string, County>>();
            string sql = "SELECT [CODE],[NAME],[SPELLING],[SHORTSPELLING],[HOTLEVEL],[CITY] FROM [T_County]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        County item = new County(reader.GetString(0).Trim());
                        item.Name = reader.GetString(1);
                        item.Spelling = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        item.ShortSpelling = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        item.HotLevel = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                        item.CityCode = reader.GetString(5);
                        result.Add(new KeyValuePair<string, County>(item.Code, item));
                    }
                }
            }
            return result;
        }

        public int Update(County value) {
            string sql = "UPDATE [T_County] SET [NAME]=@NAME,[SPELLING]=@SPELLING,[SHORTSPELLING]=@SHORTSPELLING,[HOTLEVEL]=@HOTLEVEL,[CITY]=@CITY WHERE [CODE]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SPELLING", value.Spelling ?? string.Empty);
                dbOperator.AddParameter("SHORTSPELLING", value.ShortSpelling ?? string.Empty);
                dbOperator.AddParameter("HOTLEVEL", value.HotLevel);
                dbOperator.AddParameter("CITY", value.CityCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}