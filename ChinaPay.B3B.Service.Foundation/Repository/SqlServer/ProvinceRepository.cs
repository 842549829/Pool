using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    internal class ProvinceRepository : SqlServerRepository, IProvinceRepository {
        public ProvinceRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(Province value) {
            string sql = "DELETE FROM dbo.T_Province WHERE [Code]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(Province value) {
            string sql = "INSERT INTO dbo.T_Province (Code,Name,Area) VALUES (@CODE,@NAME,@AREA)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("AREA", value.AreaCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<string, Province>> Query() {
            var result = new List<KeyValuePair<string, Province>>();
            string sql = "SELECT Code,Name,Area FROM dbo.T_Province ORDER BY Code";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var item = new Province(reader.GetString(0).Trim()){
                            Name = reader.GetString(1),
                            AreaCode = reader.GetString(2)
                        };
                        result.Add(new KeyValuePair<string, Province>(item.Code, item));
                    }
                }
            }
            return result;
        }

        public int Update(Province value) {
            string sql = "UPDATE dbo.T_Province SET Name=@NAME,Area=@AREA WHERE Code=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("AREA", value.AreaCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}