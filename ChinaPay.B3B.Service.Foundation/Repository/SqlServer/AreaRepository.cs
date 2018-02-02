using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    internal class AreaRepository : SqlServerRepository, IAreaRepository {
        public AreaRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(Area value) {
            string sql = "DELETE FROM dbo.T_Area WHERE Code=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(Area value) {
            string sql = "INSERT INTO dbo.T_Area(Code,Name) VALUES (@CODE,@NAME)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<string, Area>> Query() {
            var result = new List<KeyValuePair<string, Area>>();
            string sql = "SELECT Code,Name FROM dbo.T_Area ORDER BY CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var item = new Area(reader.GetString(0).Trim()) {
                            Name = reader.GetString(1)
                        };
                        result.Add(new KeyValuePair<string, Area>(item.Code, item));
                    }
                }
            }
            return result;
        }

        public int Update(Area value) {
            string sql = "UPDATE dbo.T_Area SET Name=@NAME WHERE Code=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}