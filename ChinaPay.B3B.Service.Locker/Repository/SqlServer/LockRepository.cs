using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Locker.Repository.SqlServer {
    class LockRepository : SqlServerRepository, ILockRepository {
        public LockRepository(string connectionString)
            : base(connectionString) {
        }

        LockInfo ILockRepository.Lock(LockInfo lockInfo) {
            LockInfo result = null;
            var sql = "dbo.P_Lock";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("@pKey", lockInfo.Key);
                dbOperator.AddParameter("@pRole", (byte)lockInfo.LockRole);
                dbOperator.AddParameter("@pCompany", lockInfo.Company);
                dbOperator.AddParameter("@pCompanyName", Utility.StringUtility.Trim(lockInfo.CompanyName) ?? string.Empty);
                dbOperator.AddParameter("@pAccount", Utility.StringUtility.Trim(lockInfo.Account) ?? string.Empty);
                dbOperator.AddParameter("@pRemark", Utility.StringUtility.Trim(lockInfo.Remark) ?? string.Empty);
                dbOperator.AddParameter("@pTime", lockInfo.Time);
                dbOperator.AddParameter("@pName", Utility.StringUtility.Trim(lockInfo.Name) ?? string.Empty);
                using(var reader = dbOperator.ExecuteReader(sql, System.Data.CommandType.StoredProcedure)) {
                    if(reader.Read()) {
                        result = new LockInfo(reader.GetString(0)) {
                            LockRole = (LockRole)reader.GetByte(1),
                            Company = reader.GetGuid(2),
                            CompanyName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Account = reader.GetString(4),
                            Remark = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            Time = reader.GetDateTime(6),
                            Name = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                        };
                    }
                }
            }
            return result;
        }

        bool ILockRepository.UnLock(string key, string account) {
            var sql = "dbo.P_UnLock";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("@pKey", key);
                dbOperator.AddParameter("@pAccount", Utility.StringUtility.Trim(account));
                var flg = dbOperator.AddParameter("@pFlg");
                flg.Direction = System.Data.ParameterDirection.Output;
                flg.DbType = System.Data.DbType.Boolean;
                dbOperator.ExecuteNonQuery(sql, System.Data.CommandType.StoredProcedure);
                return (bool)flg.Value;
            }
        }

        void ILockRepository.UnLockForcibly(string key) {
            var sql = "DELETE FROM dbo.T_LockInfo WHERE [Key]=@KEY";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("KEY", key);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        LockInfo ILockRepository.Query(string key) {
            LockInfo result = null;
            var sql = "SELECT [Key],[Role],[Company],[CompanyName],[Account],[Remark],[Time],[Name] FROM dbo.T_LockInfo WHERE [Key]=@KEY";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("KEY", key);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        result = new LockInfo(reader.GetString(0)) {
                            LockRole = (LockRole)reader.GetByte(1),
                            Company = reader.GetGuid(2),
                            CompanyName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Account = reader.GetString(4),
                            Remark = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            Time = reader.GetDateTime(6),
                            Name = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
                        };
                    }
                }
            }
            return result;
        }

        IEnumerable<LockInfo> ILockRepository.Query(IEnumerable<string> keys) {
            var result = new List<LockInfo>();
            var sql = "SELECT [Key],[Role],[Company],[CompanyName],[Account],[Remark],[Time],[Name] FROM dbo.T_LockInfo"
                +" WHERE [Key] IN (" + keys.Join(",", item => "'" + item + "'") + ")";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new LockInfo(reader.GetString(0)) {
                            LockRole = (LockRole)reader.GetByte(1),
                            Company = reader.GetGuid(2),
                            CompanyName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Account = reader.GetString(4),
                            Remark = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            Time = reader.GetDateTime(6),
                            Name = reader.GetString(7)
                        });
                    }
                }
            }
            return result;
        }
    }
}