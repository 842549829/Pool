using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Policy.Repository.SqlServer
{
    class PolicyHarmoniesRepository : SqlServerRepository, IPolicyHarmoniesRepository
    {
        public PolicyHarmoniesRepository(string connectionString) : base(connectionString) { }

        public IEnumerable<PolicyHarmonyInfo> QueryPolicyHarmonyInfos(Pagination pagination, PolicyHarmonyQueryParameter condition)
        {
            IList<PolicyHarmonyInfo> result = new List<PolicyHarmonyInfo>();
            using (var dbOperator = new DbOperator(Provider,ConnectionString))
            {
                if (!string.IsNullOrEmpty(condition.Departure)) dbOperator.AddParameter("@iDeparture", condition.Departure);
                if (!string.IsNullOrEmpty(condition.Arrival)) dbOperator.AddParameter("@iArrival", condition.Arrival);
                if (!string.IsNullOrEmpty(condition.Airline)) dbOperator.AddParameter("@iAirline", condition.Airline);
                if (condition.PolicyType.HasValue) dbOperator.AddParameter("@iPolicyType", condition.PolicyType.Value);
                if (condition.EffectTimeStart.HasValue) dbOperator.AddParameter("@iEffectiveLowerDate",condition.EffectTimeStart.Value);
                if (condition.EffectTimeEnd.HasValue) dbOperator.AddParameter("@iEffectiveUpperDate", condition.EffectTimeEnd.Value);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                dbOperator.AddParameter("@iPageSize",pagination.PageSize);
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("[dbo].[P_QueryPolicyHarmonys]",System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var info = new PolicyHarmonyInfo();
                        info.Id = reader.GetGuid(0);
                        info.Airlines = reader.GetString(1);
                        info.Departure = reader.GetString(2);
                        info.Arrival = reader.GetString(3);
                        info.PolicyType = (Common.Enums.PolicyType)reader.GetInt32(4);
                        info.EffectiveLowerDate = reader.GetDateTime(5);
                        info.EffectiveUpperDate = reader.GetDateTime(6);
                        info.DeductionType = (Common.Enums.DeductionType)reader.GetInt32(7);
                        info.HarmonyValue = reader.GetDecimal(8);
                        info.Remark = reader.GetString(9);
                        info.Account = reader.GetString(10);
                        info.CreateTime = reader.GetDateTime(11);
                        info.LastModifyTime = reader.GetDateTime(12);
                        info.LastModifyName = reader.GetString(13);
                        result.Add(info);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }
    }
}
