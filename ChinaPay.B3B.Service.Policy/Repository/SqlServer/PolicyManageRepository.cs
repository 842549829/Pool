using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Policy.Repository.SqlServer
{
    class PolicyManageRepository : SqlServerTransaction, IPolicyManageRepository
    {
        public PolicyManageRepository(DbOperator command) : base(command) { }
        public IEnumerable<BargainDefaultPolicyInfo> QueryBargainDefaultList(BargainDefaultPolicyQueryParameter parameter, Pagination pagination)
        {
            if (parameter == null) throw new ArgumentException("parameter");
            if (pagination == null) throw new ArgumentException("pagination");
            var result = new List<BargainDefaultPolicyInfo>();
            ClearParameters();
            AddParameter("@iAirline", parameter.Airline);
            AddParameter("@iProvinceCode", parameter.ProvinceCode);
            if (parameter.AdultProviderId.HasValue)
            {
                AddParameter("@iAdultProviderId", parameter.AdultProviderId);
            }
            else
            {
                AddParameter("@iAdultProviderId", DBNull.Value);
            }
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iPageSize", pagination.PageSize);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("P_QueryBarginDefaultPolicy", System.Data.CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    var item = new BargainDefaultPolicyInfo
                        {
                            AdultCommission = reader.GetDecimal(0),
                            Airline = reader.GetString(1),
                            AdultProviderId = reader.GetGuid(2),
                            ProvinceCode = reader.GetString(3),
                            AdultProviderAbbreviateName = reader.GetString(4),
                            AdultProviderName = reader.GetString(5)
                        };
                    result.Add(item);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }

        public IEnumerable<NormalDefaultPolicyInfo> QueryDefaultList(DefaultPolicyQueryParameter parameter, Pagination pagination)
        {
            if (parameter == null) throw new ArgumentException("parameter");
            if (pagination == null) throw new ArgumentException("pagination");
            var result = new List<NormalDefaultPolicyInfo>();
            ClearParameters();
            AddParameter("@iAirline", parameter.Airline);
            AddParameter("@iAdultProviderName", parameter.AdultProviderName);
            AddParameter("@iChildProviderName", parameter.ChildProviderName);
            if (parameter.AdultProviderId.HasValue)
            {
                AddParameter("@iAdultProviderId", parameter.AdultProviderId.Value);
            }
            else
            {
                AddParameter("@iAdultProviderId", DBNull.Value);
            }
            if (parameter.ChildProviderId.HasValue)
            {
                AddParameter("@iChildProviderId", parameter.ChildProviderId.Value);
            }
            else
            {
                AddParameter("@iChildProviderId", DBNull.Value);
            }
            if (parameter.AdultCommission.HasValue)
            {
                AddParameter("@iAdultCommission", parameter.AdultCommission);
            }
            else
            {
                AddParameter("@iAdultCommission", DBNull.Value);
            }
            if (parameter.ChildCommission.HasValue)
            {
                AddParameter("@iChildCommission", parameter.ChildCommission);
            }
            else
            {
                AddParameter("@iChildCommission", DBNull.Value);
            }
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iPageSize", pagination.PageSize);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("P_QueryDefaultPolicy", System.Data.CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    var item = new NormalDefaultPolicyInfo
                        {
                            Airline = reader.GetString(0),
                            AdultProviderId = reader.GetGuid(1),
                            AdultProviderName = reader.GetString(2),
                            AdultProviderAbbreviateName = reader.GetString(3),
                            AdultCommission = reader.GetDecimal(4),
                            ChildProviderId = reader.GetGuid(5),
                            ChildProviderName = reader.GetString(6),
                            ChildProviderAbbreviateName = reader.GetString(7),
                            ChildCommission = reader.GetDecimal(8)
                        };
                    result.Add(item);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;

        }

        public IEnumerable<PolicySettingInfo> QueryPolicySettingList(PolicySettingQueryParameter parameter, Pagination pagination)
        {
            if (parameter == null) throw new ArgumentException("parameter");
            if (pagination == null) throw new ArgumentException("pagination");
            var result = new List<PolicySettingInfo>();
            ClearParameters();
            if (parameter.Airline != null)
            {
                AddParameter("@iAirline", parameter.Airline);
            }
            else
            {
                AddParameter("@iAirline", DBNull.Value);
            }
            if (parameter.Departure != null)
            {
                AddParameter("@iDeparture", parameter.Departure);
            }
            else
            {
                AddParameter("@iDeparture", DBNull.Value);
            }
            if (parameter.Arrival != null)
            {
                AddParameter("@iArrival", parameter.Arrival);
            }
            else
            {
                AddParameter("@iArrival", DBNull.Value);
            }
            if (parameter.Rebate.HasValue)
            {
                AddParameter("@iRebate", parameter.Rebate);
            }
            else
            {
                AddParameter("@iRebate", DBNull.Value);
            }
            if (parameter.EffectiveTimeStart.HasValue)
            {
                AddParameter("@iEffectiveTimeStart", parameter.EffectiveTimeStart);
            }
            else
            {
                AddParameter("@iEffectiveTimeStart", DBNull.Value);
            }
            if (parameter.EffectiveTimeEnd.HasValue)
            {
                AddParameter("@iEffectiveTimeEnd", parameter.EffectiveTimeEnd);
            }
            else
            {
                AddParameter("@iEffectiveTimeEnd", DBNull.Value);
            }
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iPageSize", pagination.PageSize);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("P_QueryPolicySettings", System.Data.CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    var item = new PolicySettingInfo
                        {
                            Id = reader.GetGuid(0),
                            Airline = reader.GetString(1),
                            Departure = reader.GetString(2),
                            Arrivals = reader.GetString(3),
                            RebateType = reader.GetBoolean(4),
                            Berths = reader.GetString(5),
                            EffectiveTimeStart = reader.GetDateTime(6),
                            EffectiveTimeEnd = reader.GetDateTime(7),
                            Enable = reader.GetBoolean(8),
                            Remark = reader.GetString(9),
                            Creator = reader.GetString(10),
                            CreateTime = reader.GetDateTime(11),
                            LastModifyTime = reader.GetDateTime(12),
                            Periods = new List<PolicySettingPeriod>()
                        };
                    var period = new PolicySettingPeriod()
                        {
                            PolicySetting = reader.GetGuid(13),
                            PeriodStart = reader.GetDecimal(14),
                            PeriodEnd = reader.GetDecimal(15),
                            Rebate = reader.GetDecimal(16)
                        };
                    ((List<PolicySettingPeriod>)item.Periods).Add(period);
                    if (result.Any(p => p.Id == item.Id))
                    {
                        ((List<PolicySettingPeriod>)result.First(p => p.Id == item.Id).Periods).Add(period);
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }

            return result;


        }

        public IEnumerable<SuspendOperation> QuerySuspendOption(SuspendOperationQueryParameter parameter, Pagination pagination)
        {
            if (parameter == null) throw new ArgumentException("parameter");
            if (pagination == null) throw new ArgumentException("pagination");
            var result = new List<SuspendOperation>();
            ClearParameters();
            if (parameter.Company.HasValue)
            {
                AddParameter("@iCompany", parameter.Company);
            }
            else
            {
                AddParameter("@iCompany", DBNull.Value);
            }
            AddParameter("@iStartDate", parameter.OperateDate.Lower);
            AddParameter("@iEndDate", parameter.OperateDate.Upper);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iPageSize", pagination.PageSize);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("P_QuerySuspendOptions", System.Data.CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    var item = new SuspendOperation
                        {
                            Id = reader.GetGuid(0),
                            Owner = reader.GetGuid(1),
                            Airlines = reader.GetString(2),
                            Reason = reader.GetString(3),
                            OperateTime = reader.GetDateTime(4),
                            OperateType = (PolicySuspendOperationType)reader.GetByte(5),
                            Operator = reader.GetString(6),
                            IP = reader.GetString(7),
                            OperatorRoleType = (PublishRole)reader.GetByte(8),
                            CompanyId = reader.GetValue(9) == DBNull.Value ? Guid.Empty : reader.GetGuid(9),
                            CompanyName = reader.GetString(10)
                        };
                    result.Add(item);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }

            return result;
        }
    }
}