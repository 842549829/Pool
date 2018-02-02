using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.AirlineConfig.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.AirlineConfig.Repository.SqlServer
{
    internal class AirlineConfigRepository : SqlServerRepository, IAirlineConfigRepository
    {
        public AirlineConfigRepository(string connectionString)
            : base(connectionString)
        {
        }

        #region IAirlineConfigRepository Members

        /// <summary>
        /// 查询公司OEM信息
        /// </summary>
        /// <param name="oemId">OEMID</param>
        /// <returns></returns>
        public OEMAirlineConfig QueryConfig(Guid? oemId)
        {

            var result = new OEMAirlineConfig{ Config= new Dictionary<ConfigUseType, Tuple<string,string>>()};
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = "select ConfigId,OEMId,ConfigUseType,UserName,OfficeNO from T_OEMAirlineConfig where OEMId = @OEMID;select UseB3BConfig from T_OEMInfo where Id = @OEMID";
                dbOperator.AddParameter("OEMID", oemId??Guid.Empty);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result.Config.Add((ConfigUseType)reader.GetByte(2),
                           new Tuple<string, string>(reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                           reader.IsDBNull(4) ? string.Empty : reader.GetString(4)));
                        result.OEMID = reader.GetGuid(1);
                    }
                    if (reader.NextResult()&&reader.HasRows&&reader.Read())
                    {
                        result.UserB3bConfig = reader.GetBoolean(0);
                    }
                    else
                    {
                        result.UserB3bConfig = true;
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 保存OEM配置信息
        /// </summary>
        /// <param name="oemId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool SaveConfig(Guid oemId, Dictionary<ConfigUseType, Tuple<string, string>> config)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                StringBuilder sql = new StringBuilder("delete from T_OEMAirlineConfig where OEMId = @OEMID;");
                if (!config.Any()) return false;
                sql.Append("INSERT INTO T_OEMAirlineConfig (OEMId,ConfigUseType,UserName,OfficeNO)");
                int configIndex = 1;
                dbOperator.AddParameter("OEMID", oemId);
                foreach (KeyValuePair<ConfigUseType, Tuple<string, string>> pair in config)
                {
                    sql.AppendFormat("SELECT @OEMID,@ConfigUseType{0},@UserName{0},@OfficeNO{0}  UNION ALL ", configIndex);
                    dbOperator.AddParameter("ConfigUseType" + configIndex, (byte)pair.Key);
                    dbOperator.AddParameter("UserName" + configIndex, pair.Value.Item1);
                    dbOperator.AddParameter("OfficeNO" + configIndex, pair.Value.Item2);
                    configIndex++;
                }
                return dbOperator.ExecuteNonQuery(sql.Remove(sql.Length - 10, 10).ToString()) > 0;
            }
        }

        #endregion
    }
}