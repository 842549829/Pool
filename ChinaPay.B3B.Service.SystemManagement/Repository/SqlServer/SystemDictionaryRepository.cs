using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.SystemManagement.Repository.SqlServer {
    class SystemDictionaryRepository : SqlServerRepository, ISystemDictionaryRepository {
        public SystemDictionaryRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<SystemDictionary> Query() {
            string sql = "SELECT [TYPE],[ID],[NAME],[VALUE],[REMARK] FROM [T_SystemDictionary] ORDER BY TYPE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    var result = new List<SystemDictionary>();
                    SystemDictionary dictionary = null;
                    while(reader.Read()) {
                        SystemDictionaryType currentType = (SystemDictionaryType)reader.GetInt32(0);
                        if(null == dictionary || dictionary.Type != currentType) {
                            dictionary = new SystemDictionary(currentType);
                            result.Add(dictionary);
                        }
                        dictionary.AddItem(new SystemDictionaryItem(
                            reader.GetGuid(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            reader.IsDBNull(4) ? string.Empty : reader.GetString(4)));
                    }
                    return result;
                }
            }
        }

        public int UpdateItem(SystemDictionaryType type, SystemDictionaryItem item) {
            string sql = "UPDATE [T_SystemDictionary] SET [NAME]=@NAME,[VALUE]=@VALUE,[REMARK]=@REMARK WHERE [TYPE]=@TYPE AND [ID]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("NAME", item.Name);
                dbOperator.AddParameter("VALUE", item.Value);
                dbOperator.AddParameter("REMARK", item.Remark);
                dbOperator.AddParameter("TYPE", (int)type);
                dbOperator.AddParameter("ID", item.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int InsertItem(SystemDictionaryType type, SystemDictionaryItem item) {
            string sql = "INSERT INTO [T_SystemDictionary] ([TYPE],[ID],[NAME],[VALUE],[REMARK]) VALUES (@TYPE,@ID,@NAME,@VALUE,@REMARK)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("TYPE", (int)type);
                dbOperator.AddParameter("ID", item.Id);
                dbOperator.AddParameter("NAME", item.Name);
                dbOperator.AddParameter("VALUE", item.Value);
                dbOperator.AddParameter("REMARK", item.Remark);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteItem(SystemDictionaryType type, Guid id) {
            string sql = "DELETE FROM [T_SystemDictionary] WHERE [TYPE]=@TYPE AND [ID]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("TYPE", (int)type);
                dbOperator.AddParameter("ID", id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}