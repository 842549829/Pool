using System.Collections.Generic;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.SystemManagement.Repository.SqlServer {
    class SystemParamRepository : SqlServerRepository, ISystemParamRepository {
        public SystemParamRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<KeyValuePair<SystemParamType, SystemParam>> Query() {
            string sql = "SELECT [ID],[VALUE],[REMARK] FROM [T_SystemParam]";
            var result = new List<KeyValuePair<SystemParamType, SystemParam>>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var item = new SystemParam((SystemParamType)reader.GetInt32(0),
                            reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2));
                        result.Add(new KeyValuePair<SystemParamType, SystemParam>(item.Type, item));
                    }
                }
            }
            return result;
        }

        public int Update(SystemParamType type, string value) {
            string sql = "UPDATE [T_SystemParam] SET [VALUE]=@VALUE WHERE [ID]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", (int)type);
                dbOperator.AddParameter("VALUE", value);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(SystemParam value) {
            throw new System.NotImplementedException();
        }

        public int Insert(SystemParam value) {
            throw new System.NotImplementedException();
        }

        public int Update(SystemParam value) {
            return Update(value.Type, value.Value);
        }
    }
}