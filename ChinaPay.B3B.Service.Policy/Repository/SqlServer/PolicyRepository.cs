using System;
using System.Collections.Generic;
using System.Data;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using System.Data.SqlClient;

namespace ChinaPay.B3B.Service.Policy.Repository.SqlServer
{
    class PolicyRepository : SqlServerRepository, IPolicyRepository
    {
        public PolicyRepository(string connectionstring)
            : base(connectionstring)
        {
        }

        //public PolicyRepository(DbOperator dbOperator)
        //    : base(dbOperator)
        //{
        //}

        #region 删除
        /// <summary>
        /// 删除普通政策
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void DeleteNormalPolicy(params Guid[] ids)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string idlist = "";
                foreach (var item in ids)
                {
                    if (idlist == "")
                    {
                        idlist += "'" + item.ToString() + "'";
                    }
                    else
                    {
                        idlist += ",'" + item.ToString() + "'";
                    }
                }
                string sql = "INSERT INTO T_NormalPolicyDelLog SELECT [Id],[Owner],[Airline],[OfficeCode],[CustomCode],[ImpowerOffice],[IsInternal],[IsPeer],[VoyageType],[Departure],[Transit],[Arrival],[DepartureDateStart],[DepartureDateEnd],[DepartureFlightsFilter],[DepartureDateFilter],[DepartureWeekFilter],[DepartureFlightsFilterType],[ReturnFlightsFilter],[ReturnFlightsFilterType],[ExceptAirways],[DrawerCondition],[Remark],[Berths],[InternalCommission],[SubordinateCommission],[ProfessionCommission],[AutoAudit],[ChangePNR],[AutoPrint],[SuitReduce],[TicketType],[StartPrintDate],[Suspended],[Freezed],[Audited],[CreateTime],[AuditTime],[Creator],[MultiSuitReduce],LastDelTime = GETDATE(),AbbreviateName,PrintBeforeTwoHours FROM T_NormalPolicy WHERE Id IN (" + idlist + ");DELETE FROM T_NormalPolicy WHERE Id IN (" + idlist + ");DELETE FROM dbo.T_NormalPolicySetting WHERE PolicyId IN (" + idlist + ")";
                dbOperator.ExecuteNonQuery(sql);
            }
        }
        /// <summary>
        /// 删除特价政策
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void DeleteBargainPolicy(params Guid[] ids)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string idlist = "";
                foreach (var item in ids)
                {
                    if (idlist == "")
                    {
                        idlist += "'" + item.ToString() + "'";
                    }
                    else
                    {
                        idlist += ",'" + item.ToString() + "'";
                    }
                }
                string sql = "INSERT INTO T_BargainPolicyDelLog SELECT [Id],[Owner],[Airline],[OfficeCode],[CustomCode],[ImpowerOffice],[IsInternal],[IsPeer],[VoyageType],[Departure],[Transit],[Arrival],[ExceptAirways],[DepartureDateStart],[DepartureDateEnd],[DepartureFlightsFilter],[DepartureDateFilter],[DepartureWeekFilter],[DepartureFlightsFilterType],[ReturnFlightsFilter],[ReturnFlightsFilterType],[BeforehandDays],[TravelDays],[InvalidRegulation],[ChangeRegulation],[EndorseRegulation],[RefundRegulation],[DrawerCondition],[Remark],[Berths],[InternalCommission],[SubordinateCommission],[ProfessionCommission],[Price],[PriceType],[AutoAudit],[ChangePNR],[TicketType],[StartPrintDate],[Suspended],[Freezed],[Audited],[CreateTime],[AuditTime],[Creator],[MultiSuitReduce],[MostBeforehandDays],LastDelTime = GETDATE(),AbbreviateName,PrintBeforeTwoHours FROM T_BargainPolicy WHERE Id IN (" + idlist + ");DELETE FROM T_BargainPolicy WHERE Id IN (" + idlist + ");";
                dbOperator.ExecuteNonQuery(sql);
            }
        }
        /// <summary>
        /// 删除特殊政策
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void DeleteSpecialPolicy(params Guid[] ids)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string idlist = "";
                foreach (var item in ids)
                {
                    if (idlist == "")
                    {
                        idlist += "'" + item.ToString() + "'";
                    }
                    else
                    {
                        idlist += ",'" + item.ToString() + "'";
                    }
                }

                string sql = "INSERT INTO T_SpecialPolicyDelLog SELECT [Id],[Owner],[Type],[OfficeCode],[CustomCode],[ImpowerOffice],[IsPeer],[IsInternal],[Airline],[VoyageType],[Departure],[Arrival],[ExceptAirways],[DepartureDateStart],[DepartureDateEnd],[DepartureDateFilter],[DepartureWeekFilter],[DepartureFlightsFilter],[DepartureFlightsFilterType],[BeforehandDays],[InvalidRegulation],[ChangeRegulation],[EndorseRegulation],[RefundRegulation],[DrawerCondition],[Remark],[Price],[ProvideDate],[ResourceAmount],[AutoAudit],[ConfirmResource],[Suspended],[Freezed],[Audited],[PlatformAudited],[SynBlackScreen],[Berths],[TicketType],[PriceType],[InternalCommission],[SubordinateCommission],[ProfessionCommission],[CreateTime],[AuditTime],[Creator],[IsBargainBerths],[IsSeat],LastDelTime = GETDATE(),AbbreviateName,PrintBeforeTwoHours,LowNoType,LowNoMaxPrice,LowNoMinPrice FROM T_SpecialPolicy WHERE Id IN (" + idlist + ");DELETE FROM T_SpecialPolicy WHERE Id IN (" + idlist + ");";
                dbOperator.ExecuteNonQuery(sql);
            }
        }
        /// <summary>
        /// 删除团队政策
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public void DeleteTeamPolicy(params Guid[] ids)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string idlist = "";
                foreach (var item in ids)
                {
                    if (idlist == "")
                    {
                        idlist += "'" + item.ToString() + "'";
                    }
                    else
                    {
                        idlist += ",'" + item.ToString() + "'";
                    }
                }

                string sql = "INSERT INTO T_TeamPolicyDelLog SELECT [Id],[Owner],[Airline],[OfficeCode],[CustomCode],[ImpowerOffice],[IsInternal],[IsPeer],[VoyageType],[Departure],[Transit],[Arrival],[DepartureDateStart],[DepartureDateEnd],[DepartureFlightsFilter],[DepartureDateFilter],[DepartureWeekFilter],[DepartureFlightsFilterType],[ReturnFlightsFilter],[ReturnFlightsFilterType],[ExceptAirways],[DrawerCondition],[Remark],[AppointBerths],[Berths],[InternalCommission],[SubordinateCommission],[ProfessionCommission],[AutoAudit],[ChangePNR],[AutoPrint],[SuitReduce],[TicketType],[StartPrintDate],[Suspended],[Freezed],[Audited],[CreateTime],[AuditTime],[Creator],[MultiSuitReduce],LastDelTime = GETDATE(),AbbreviateName,PrintBeforeTwoHours FROM T_TeamPolicy WHERE Id IN (" + idlist + ");DELETE FROM T_TeamPolicy WHERE Id IN (" + idlist + ");";

                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void DeleteNormalPolicyDeparture(IEnumerable<Guid> policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = "";
                int i = 0;
                foreach (var item in policyId)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_NormalPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                    dbOperator.AddParameter("plid" + i, item);
                }
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void DeleteBargainPolicyDeparture(IEnumerable<Guid> policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = "";
                int i = 0;
                foreach (var item in policyId)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_BargainPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                    dbOperator.AddParameter("plid" + i, item);
                }
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void DeleteSpecialPolicyDeparture(IEnumerable<Guid> policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = "";
                int i = 0;
                foreach (var item in policyId)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_SpecialPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                    dbOperator.AddParameter("plid" + i, item);
                }
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void DeleteTeamPolicyDeparture(IEnumerable<Guid> policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = "";
                int i = 0;
                foreach (var item in policyId)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_TeamPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                    dbOperator.AddParameter("plid" + i, item);
                }
                dbOperator.ExecuteNonQuery(sql);
            }
        }
        #endregion

        #region 查询

        public System.Collections.Generic.List<DataTransferObject.Policy.NormalPolicyInfo> QueryNormalPolicies(DataTransferObject.Policy.PolicyQueryParameter parameter, Pagination pagination)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                System.Collections.Generic.List<DataTransferObject.Policy.NormalPolicyInfo> result;
                if (!string.IsNullOrWhiteSpace(parameter.Departure))
                    dbOperator.AddParameter("@iDeparture", parameter.Departure);
                if (!string.IsNullOrWhiteSpace(parameter.Transit))
                    dbOperator.AddParameter("@iTransit", parameter.Transit);
                if (!string.IsNullOrWhiteSpace(parameter.Arrival))
                    dbOperator.AddParameter("@iArrival", parameter.Arrival);
                if (!string.IsNullOrWhiteSpace(parameter.Airline))
                    dbOperator.AddParameter("@iAirline", parameter.Airline);
                if (!string.IsNullOrWhiteSpace(parameter.OfficeCode))
                    dbOperator.AddParameter("@iOfficeNo", parameter.OfficeCode);
                if (!string.IsNullOrWhiteSpace(parameter.Bunks))
                    dbOperator.AddParameter("@iBunks", parameter.Bunks);
                if (!string.IsNullOrWhiteSpace(parameter.Creator))
                    dbOperator.AddParameter("@iCreator", parameter.Creator);
                if (parameter.Audited != null)
                    dbOperator.AddParameter("@iAudited", parameter.Audited);
                if (parameter.Freezed != null)
                    dbOperator.AddParameter("@iFreezed", parameter.Freezed);
                if (parameter.Effective != null)
                    dbOperator.AddParameter("@iEffective", parameter.Effective);
                if (parameter.TicketType != null)
                    dbOperator.AddParameter("@iTicketType", parameter.TicketType);
                if (parameter.VoyageType != null)
                    dbOperator.AddParameter("@iVoyageType", parameter.VoyageType);
                if (parameter.Suspended != null)
                    dbOperator.AddParameter("@iSuspended", parameter.Suspended);
                if (parameter.Owner != null)
                    dbOperator.AddParameter("@iOwner", parameter.Owner);
                if (parameter.DepartureDateStart != null)
                    dbOperator.AddParameter("@iDepartureDateStart", parameter.DepartureDateStart.Value.Date);
                if (parameter.DepartureDateEnd != null)
                    dbOperator.AddParameter("@iDepartureDateEnd", parameter.DepartureDateEnd.Value.Date);
                if (parameter.PubDateStart != null)
                    dbOperator.AddParameter("@iPubDateStart", parameter.PubDateStart.Date);
                if (parameter.PubDateEnd != null)
                    dbOperator.AddParameter("@iPubDateEnd", parameter.PubDateEnd.Date);
                if (parameter.InternalCommissionLower != null)
                    dbOperator.AddParameter("@iInternalCommissionLower", parameter.InternalCommissionLower);
                if (parameter.InternalCommissionUpper != null)
                    dbOperator.AddParameter("@iInternalCommissionUpper", parameter.InternalCommissionUpper);
                if (parameter.SubordinateCommissionLower != null)
                    dbOperator.AddParameter("@iSubordinateCommissionLower", parameter.SubordinateCommissionLower);
                if (parameter.SubordinateCommissionUpper != null)
                    dbOperator.AddParameter("@iSubordinateCommissionUpper", parameter.SubordinateCommissionUpper);
                if (parameter.ProfessionCommissionLower != null)
                    dbOperator.AddParameter("@iProfessionCommissionLower", parameter.ProfessionCommissionLower);
                if (parameter.ProfessionCommissionUpper != null)
                    dbOperator.AddParameter("@iProfessionCommissionUpper", parameter.ProfessionCommissionUpper);
                dbOperator.AddParameter("@iOrderBy", parameter.OrderBy);

                dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);

                var totalCount = dbOperator.AddParameter("@oTotalCount");

                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_QueryNormalPolicy", System.Data.CommandType.StoredProcedure))
                {
                    result = new System.Collections.Generic.List<DataTransferObject.Policy.NormalPolicyInfo>();
                    while (reader.Read())
                    {
                        NormalPolicyInfo view = new NormalPolicyInfo();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.NeedAUTH = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.Departure = reader.GetString(9);
                        view.Transit = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.DepartureDateStart = reader.GetDateTime(12);
                        view.DepartureDateEnd = reader.GetDateTime(13);
                        view.DepartureFlightsFilter = reader.GetString(14);
                        view.DepartureDateFilter = reader.GetString(15);
                        view.DepartureWeekFilter = reader.GetString(16);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(17); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude);
                        view.ReturnFlightsFilter = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                        view.ReturnFlightsFilterType = reader.GetValue(19) == DBNull.Value ? (LimitType?)null : (LimitType)reader.GetByte(19);
                        view.ExceptAirways = reader.GetString(20);

                        //出票条件暂时在列表上不显示
                        //view.DrawerCondition = reader.GetString(21);

                        view.Remark = reader.GetString(22);
                        view.Berths = reader.GetString(23);
                        view.InternalCommission = reader.GetDecimal(24);
                        view.SubordinateCommission = reader.GetDecimal(25);
                        view.ProfessionCommission = reader.GetDecimal(26);
                        //自动审核暂时不在列表上显示
                        //view.AutoAudit = reader.GetString(27);
                        view.ChangePNR = reader.GetBoolean(28);
                        view.AutoPrint = reader.GetBoolean(29);
                        view.SuitReduce = reader.GetBoolean(30);
                        view.TicketType = (TicketType)reader.GetByte(31);
                        view.StartProcessDate = reader.GetDateTime(32);
                        view.Suspended = !reader.IsDBNull(40);
                        view.Freezed = reader.GetBoolean(34);
                        view.Audited = reader.GetBoolean(35);
                        view.CreateTime = reader.GetDateTime(36);
                        view.AuditTime = reader.GetValue(37) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(37);
                        view.Creator = reader.GetString(38);
                        view.MultiConjunctionSuitable = reader.GetBoolean(39);
                        view.SuspendByPlatform = !reader.IsDBNull(40) && reader.GetBoolean(40);
                        view.DiscountPoint = !reader.IsDBNull(41);
                        view.MountPoint = !reader.IsDBNull(42);
                        result.Add(view);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
                return result;
            }
        }

        public System.Collections.Generic.List<DataTransferObject.Policy.SpecialPolicyInfo> QuerySpecialPolicies(DataTransferObject.Policy.PolicyQueryParameter parameter, Pagination pagination)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                System.Collections.Generic.List<DataTransferObject.Policy.SpecialPolicyInfo> result;
                if (!string.IsNullOrWhiteSpace(parameter.Departure))
                    dbOperator.AddParameter("@iDeparture", parameter.Departure);
                if (parameter.SpecialProductType != null)
                    dbOperator.AddParameter("@iType", parameter.SpecialProductType);
                if (!string.IsNullOrWhiteSpace(parameter.Arrival))
                    dbOperator.AddParameter("@iArrival", parameter.Arrival);
                if (!string.IsNullOrWhiteSpace(parameter.Airline))
                    dbOperator.AddParameter("@iAirline", parameter.Airline);
                if (!string.IsNullOrWhiteSpace(parameter.OfficeCode))
                    dbOperator.AddParameter("@iOfficeNo", parameter.OfficeCode);
                if (!string.IsNullOrWhiteSpace(parameter.Bunks))
                    dbOperator.AddParameter("@iBunks", parameter.Bunks);
                if (!string.IsNullOrWhiteSpace(parameter.Creator))
                    dbOperator.AddParameter("@iCreator", parameter.Creator);
                if (parameter.Effective != null)
                    dbOperator.AddParameter("@iEffective", parameter.Effective);
                if (parameter.Suspended != null)
                    dbOperator.AddParameter("@iSuspended", parameter.Suspended);
                if (parameter.Audited != null)
                    dbOperator.AddParameter("@iAudited", parameter.Audited);
                if (parameter.Freezed != null)
                    dbOperator.AddParameter("@iFreezed", parameter.Freezed);
                if (parameter.TicketType != null)
                    dbOperator.AddParameter("@iTicketType", parameter.TicketType);
                if (parameter.VoyageType != null)
                    dbOperator.AddParameter("@iVoyageType", parameter.VoyageType);
                if (parameter.PlatformAudited != null)
                    dbOperator.AddParameter("@iPlatformAudited", parameter.PlatformAudited);
                if (parameter.Owner != null)
                    dbOperator.AddParameter("@iOwner", parameter.Owner);
                if (parameter.DepartureDateStart != null)
                    dbOperator.AddParameter("@iDepartureDateStart", parameter.DepartureDateStart.Value.Date);
                if (parameter.DepartureDateEnd != null)
                    dbOperator.AddParameter("@iDepartureDateEnd", parameter.DepartureDateEnd.Value.Date);
                if (parameter.PubDateStart != null)
                    dbOperator.AddParameter("@iPubDateStart", parameter.PubDateStart.Date);
                if (parameter.PubDateEnd != null)
                    dbOperator.AddParameter("@iPubDateEnd", parameter.PubDateEnd.Date);
                if (parameter.InternalCommissionLower != null)
                    dbOperator.AddParameter("@iInternalCommissionLower", parameter.InternalCommissionLower);
                if (parameter.InternalCommissionUpper != null)
                    dbOperator.AddParameter("@iInternalCommissionUpper", parameter.InternalCommissionUpper);
                if (parameter.SubordinateCommissionLower != null)
                    dbOperator.AddParameter("@iSubordinateCommissionLower", parameter.SubordinateCommissionLower);
                if (parameter.SubordinateCommissionUpper != null)
                    dbOperator.AddParameter("@iSubordinateCommissionUpper", parameter.SubordinateCommissionUpper);
                if (parameter.ProfessionCommissionLower != null)
                    dbOperator.AddParameter("@iProfessionCommissionLower", parameter.ProfessionCommissionLower);
                if (parameter.ProfessionCommissionUpper != null)
                    dbOperator.AddParameter("@iProfessionCommissionUpper", parameter.ProfessionCommissionUpper);
                dbOperator.AddParameter("@iOrderBy", parameter.OrderBy);

                dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);

                var totalCount = dbOperator.AddParameter("@oTotalCount");

                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_QuerySpecialPolicy", System.Data.CommandType.StoredProcedure))
                {
                    result = new System.Collections.Generic.List<DataTransferObject.Policy.SpecialPolicyInfo>();
                    while (reader.Read())
                    {
                        SpecialPolicyInfo view = new SpecialPolicyInfo();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Type = (SpecialProductType)reader.GetByte(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.NeedAUTH = reader.GetBoolean(5);
                        view.IsPeer = reader.GetBoolean(6);
                        view.IsInternal = reader.GetBoolean(7);
                        view.Airline = reader.GetString(8);
                        view.VoyageType = (VoyageType)reader.GetByte(9);
                        view.Departure = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.ExceptAirways = reader.GetValue(12) == DBNull.Value ? "" : reader.GetString(12);
                        view.DepartureDateStart = reader.GetDateTime(13);
                        view.DepartureDateEnd = reader.GetDateTime(14);
                        view.DepartureDateFilter = reader.GetString(15);
                        view.DepartureWeekFilter = reader.GetString(16);
                        view.DepartureFlightsFilter = reader.GetString(17);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(18); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude);
                        //view.ReturnFlightsFilter = reader.GetString(18);
                        //view.ReturnFlightsFilterType = (LimitType)reader.GetByte(19);
                        view.BeforehandDays = reader.GetInt16(19);

                        view.InvalidRegulation = reader.GetString(20);
                        view.ChangeRegulation = reader.GetString(21);
                        view.EndorseRegulation = reader.GetString(22);
                        view.RefundRegulation = reader.GetString(23);
                        //view.DrawerCondition = reader.GetString(24);

                        view.Remark = reader.GetString(25);
                        view.Price = reader.GetDecimal(26);
                        view.StartProcessDate = reader.GetDateTime(27);
                        view.ResourceAmount = reader.GetInt32(28);
                        //view.AutoAudit =  reader.GetInt16(29);
                        view.ConfirmResource = reader.GetBoolean(30);
                        view.Suspended = !reader.IsDBNull(47);
                        view.Freezed = reader.GetBoolean(32);
                        view.Audited = reader.GetBoolean(33);
                        view.PlatformAudited = !reader.IsDBNull(34) && reader.GetBoolean(34);
                        view.SynBlackScreen = reader.GetBoolean(35);
                        view.Berths = reader.GetString(36);
                        view.TicketType = (TicketType)reader.GetByte(37);
                        view.PriceType = (PriceType)reader.GetByte(38);

                        view.InternalCommission = reader.GetDecimal(39);
                        view.SubordinateCommission = reader.GetDecimal(40);
                        view.ProfessionCommission = reader.GetDecimal(41);
                        view.CreateTime = reader.GetDateTime(42);
                        view.AuditTime = reader.GetValue(43) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(43);
                        view.Creator = reader.GetString(44);
                        view.IsBargainBerths = reader.GetBoolean(45);
                        view.IsSeat = reader.GetBoolean(46);
                        view.SuspendByPlatform = reader.GetValue(47) != DBNull.Value && reader.GetBoolean(47);
                        result.Add(view);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
                return result;
            }
        }

        public System.Collections.Generic.List<DataTransferObject.Policy.BargainPolicyInfo> QueryBargainPolicies(DataTransferObject.Policy.PolicyQueryParameter parameter, Pagination pagination)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                System.Collections.Generic.List<DataTransferObject.Policy.BargainPolicyInfo> result;
                if (!string.IsNullOrWhiteSpace(parameter.Departure))
                    dbOperator.AddParameter("@iDeparture", parameter.Departure);
                if (!string.IsNullOrWhiteSpace(parameter.Transit))
                    dbOperator.AddParameter("@iTransit", parameter.Transit);
                if (!string.IsNullOrWhiteSpace(parameter.Arrival))
                    dbOperator.AddParameter("@iArrival", parameter.Arrival);
                if (!string.IsNullOrWhiteSpace(parameter.Airline))
                    dbOperator.AddParameter("@iAirline", parameter.Airline);
                if (!string.IsNullOrWhiteSpace(parameter.OfficeCode))
                    dbOperator.AddParameter("@iOfficeNo", parameter.OfficeCode);
                if (!string.IsNullOrWhiteSpace(parameter.Bunks))
                    dbOperator.AddParameter("@iBunks", parameter.Bunks);
                if (!string.IsNullOrWhiteSpace(parameter.Creator))
                    dbOperator.AddParameter("@iCreator", parameter.Creator);
                if (parameter.Effective != null)
                    dbOperator.AddParameter("@iEffective", parameter.Effective);
                if (parameter.Audited != null)
                    dbOperator.AddParameter("@iAudited", parameter.Audited);
                if (parameter.Freezed != null)
                    dbOperator.AddParameter("@iFreezed", parameter.Freezed);
                if (parameter.TicketType != null)
                    dbOperator.AddParameter("@iTicketType", parameter.TicketType);
                if (parameter.VoyageType != null)
                    dbOperator.AddParameter("@iVoyageType", parameter.VoyageType);
                if (parameter.Suspended != null)
                    dbOperator.AddParameter("@iSuspended", parameter.Suspended);
                if (parameter.Owner != null)
                    dbOperator.AddParameter("@iOwner", parameter.Owner);
                if (parameter.DepartureDateStart != null)
                    dbOperator.AddParameter("@iDepartureDateStart", parameter.DepartureDateStart.Value.Date);
                if (parameter.DepartureDateEnd != null)
                    dbOperator.AddParameter("@iDepartureDateEnd", parameter.DepartureDateEnd.Value.Date);
                if (parameter.PubDateStart != null)
                    dbOperator.AddParameter("@iPubDateStart", parameter.PubDateStart.Date);
                if (parameter.PubDateEnd != null)
                    dbOperator.AddParameter("@iPubDateEnd", parameter.PubDateEnd.Date);
                if (parameter.InternalCommissionLower != null)
                    dbOperator.AddParameter("@iInternalCommissionLower", parameter.InternalCommissionLower);
                if (parameter.InternalCommissionUpper != null)
                    dbOperator.AddParameter("@iInternalCommissionUpper", parameter.InternalCommissionUpper);
                if (parameter.SubordinateCommissionLower != null)
                    dbOperator.AddParameter("@iSubordinateCommissionLower", parameter.SubordinateCommissionLower);
                if (parameter.SubordinateCommissionUpper != null)
                    dbOperator.AddParameter("@iSubordinateCommissionUpper", parameter.SubordinateCommissionUpper);
                if (parameter.ProfessionCommissionLower != null)
                    dbOperator.AddParameter("@iProfessionCommissionLower", parameter.ProfessionCommissionLower);
                if (parameter.ProfessionCommissionUpper != null)
                    dbOperator.AddParameter("@iProfessionCommissionUpper", parameter.ProfessionCommissionUpper);
                dbOperator.AddParameter("@iOrderBy", parameter.OrderBy);

                dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);

                var totalCount = dbOperator.AddParameter("@oTotalCount");

                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_QueryBargainPolicy", System.Data.CommandType.StoredProcedure))
                {
                    result = new System.Collections.Generic.List<DataTransferObject.Policy.BargainPolicyInfo>();
                    while (reader.Read())
                    {
                        BargainPolicyInfo view = new BargainPolicyInfo();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.NeedAUTH = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.Departure = reader.GetString(9);
                        view.Transit = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.ExceptAirways = reader.GetString(12);
                        view.DepartureDateStart = reader.GetDateTime(13);
                        view.DepartureDateEnd = reader.GetDateTime(14);
                        view.DepartureFlightsFilter = reader.GetString(15);
                        view.DepartureDateFilter = reader.GetString(16);
                        view.DepartureWeekFilter = reader.GetString(17);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(18); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude);
                        view.ReturnFlightsFilter = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                        view.ReturnFlightsFilterType = reader.GetValue(20) == DBNull.Value ? (LimitType?)null : (LimitType)reader.GetByte(20);
                        view.BeforehandDays = reader.GetInt16(21);
                        view.TravelDays = reader.GetInt16(22);
                        view.InvalidRegulation = reader.GetString(23);
                        view.ChangeRegulation = reader.GetString(24);
                        view.EndorseRegulation = reader.GetString(25);
                        view.RefundRegulation = reader.GetString(26);
                        //view.DrawerCondition = reader.GetString(27); 
                        view.Remark = reader.GetString(28);
                        view.Berths = reader.GetString(29);
                        view.InternalCommission = reader.GetDecimal(30);
                        view.SubordinateCommission = reader.GetDecimal(31);
                        view.ProfessionCommission = reader.GetDecimal(32);
                        view.Price = reader.GetDecimal(33);
                        view.PriceType = (PriceType)reader.GetByte(34);
                        //自动审核暂时不在列表上显示
                        //view.AutoAudit = reader.GetBoolean(35);

                        view.ChangePNR = reader.GetBoolean(36);
                        view.TicketType = (TicketType)reader.GetByte(37);
                        view.StartProcessDate = reader.GetDateTime(38);
                        view.Suspended = !reader.IsDBNull(47);
                        view.Freezed = reader.GetBoolean(40);
                        view.Audited = reader.GetBoolean(41);
                        view.CreateTime = reader.GetDateTime(42);
                        view.AuditTime = reader.GetValue(43) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(43);
                        view.Creator = reader.GetString(44);
                        view.MultiConjunctionSuitable = reader.GetBoolean(45);
                        view.MaxBeforehandDays = reader.GetInt16(46);
                        view.SuspendByPlatform = !reader.IsDBNull(47) && reader.GetBoolean(47);
                        result.Add(view);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
                return result;
            }
        }

        public List<DataTransferObject.Policy.TeamPolicyInfo> QueryTeamPolicies(DataTransferObject.Policy.PolicyQueryParameter parameter, Pagination pagination)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                List<DataTransferObject.Policy.TeamPolicyInfo> result;
                if (!string.IsNullOrWhiteSpace(parameter.Departure))
                    dbOperator.AddParameter("@iDeparture", parameter.Departure);
                if (!string.IsNullOrWhiteSpace(parameter.Transit))
                    dbOperator.AddParameter("@iTransit", parameter.Transit);
                if (!string.IsNullOrWhiteSpace(parameter.Arrival))
                    dbOperator.AddParameter("@iArrival", parameter.Arrival);
                if (!string.IsNullOrWhiteSpace(parameter.Airline))
                    dbOperator.AddParameter("@iAirline", parameter.Airline);
                if (!string.IsNullOrWhiteSpace(parameter.OfficeCode))
                    dbOperator.AddParameter("@iOfficeNo", parameter.OfficeCode);
                if (!string.IsNullOrWhiteSpace(parameter.Bunks))
                    dbOperator.AddParameter("@iBunks", parameter.Bunks);
                if (!string.IsNullOrWhiteSpace(parameter.Creator))
                    dbOperator.AddParameter("@iCreator", parameter.Creator);
                if (parameter.Audited != null)
                    dbOperator.AddParameter("@iAudited", parameter.Audited);
                if (parameter.Freezed != null)
                    dbOperator.AddParameter("@iFreezed", parameter.Freezed);
                if (parameter.TicketType != null)
                    dbOperator.AddParameter("@iTicketType", parameter.TicketType);
                if (parameter.Suspended != null)
                    dbOperator.AddParameter("@iSuspended", parameter.Suspended);
                if (parameter.Effective != null)
                    dbOperator.AddParameter("@iEffective", parameter.Effective);
                if (parameter.VoyageType != null)
                    dbOperator.AddParameter("@iVoyageType", parameter.VoyageType);
                if (parameter.Owner != null)
                    dbOperator.AddParameter("@iOwner", parameter.Owner);
                if (parameter.DepartureDateStart != null)
                    dbOperator.AddParameter("@iDepartureDateStart", parameter.DepartureDateStart.Value.Date);
                if (parameter.DepartureDateEnd != null)
                    dbOperator.AddParameter("@iDepartureDateEnd", parameter.DepartureDateEnd.Value.Date);
                if (parameter.PubDateStart != null)
                    dbOperator.AddParameter("@iPubDateStart", parameter.PubDateStart.Date);
                if (parameter.PubDateEnd != null)
                    dbOperator.AddParameter("@iPubDateEnd", parameter.PubDateEnd.Date);
                if (parameter.InternalCommissionLower != null)
                    dbOperator.AddParameter("@iInternalCommissionLower", parameter.InternalCommissionLower);
                if (parameter.InternalCommissionUpper != null)
                    dbOperator.AddParameter("@iInternalCommissionUpper", parameter.InternalCommissionUpper);
                if (parameter.SubordinateCommissionLower != null)
                    dbOperator.AddParameter("@iSubordinateCommissionLower", parameter.SubordinateCommissionLower);
                if (parameter.SubordinateCommissionUpper != null)
                    dbOperator.AddParameter("@iSubordinateCommissionUpper", parameter.SubordinateCommissionUpper);
                if (parameter.ProfessionCommissionLower != null)
                    dbOperator.AddParameter("@iProfessionCommissionLower", parameter.ProfessionCommissionLower);
                if (parameter.ProfessionCommissionUpper != null)
                    dbOperator.AddParameter("@iProfessionCommissionUpper", parameter.ProfessionCommissionUpper);
                dbOperator.AddParameter("@iOrderBy", parameter.OrderBy);

                dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);

                var totalCount = dbOperator.AddParameter("@oTotalCount");

                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_QueryTeamPolicy", System.Data.CommandType.StoredProcedure))
                {
                    result = new System.Collections.Generic.List<DataTransferObject.Policy.TeamPolicyInfo>();
                    while (reader.Read())
                    {
                        TeamPolicyInfo view = new TeamPolicyInfo();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.NeedAUTH = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.Departure = reader.GetString(9);
                        view.Transit = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.DepartureDateStart = reader.GetDateTime(12);
                        view.DepartureDateEnd = reader.GetDateTime(13);
                        view.DepartureFlightsFilter = reader.GetString(14);
                        view.DepartureDateFilter = reader.GetString(15);
                        view.DepartureWeekFilter = reader.GetString(16);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(17); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude);
                        view.ReturnFlightsFilter = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                        view.ReturnFlightsFilterType = reader.GetValue(19) == DBNull.Value ? (LimitType?)null : (LimitType)reader.GetByte(19);
                        view.ExceptAirways = reader.GetString(20);

                        //出票条件暂时在列表上不显示
                        //view.DrawerCondition = reader.GetString(21);

                        view.Remark = reader.GetString(22);
                        view.Berths = reader.GetString(23);
                        view.InternalCommission = reader.GetDecimal(24);
                        view.SubordinateCommission = reader.GetDecimal(25);
                        view.ProfessionCommission = reader.GetDecimal(26);
                        //自动审核暂时不在列表上显示
                        //view.AutoAudit = reader.GetString(27);
                        view.ChangePNR = reader.GetBoolean(28);
                        view.AutoPrint = reader.GetBoolean(29);
                        view.SuitReduce = reader.GetBoolean(30);
                        view.TicketType = (TicketType)reader.GetByte(31);
                        view.StartProcessDate = reader.GetDateTime(32);
                        view.Suspended = !reader.IsDBNull(40);
                        view.Freezed = reader.GetBoolean(34);
                        view.Audited = reader.GetBoolean(35);
                        view.CreateTime = reader.GetDateTime(36);
                        view.AuditTime = reader.GetValue(37) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(37);
                        view.Creator = reader.GetString(38);
                        view.MultiConjunctionSuitable = reader.GetBoolean(39);
                        view.SuspendByPlatform = !reader.IsDBNull(40) && reader.GetBoolean(40);
                        //是否是团队舱位
                        //view.AppointBerths = reader.GetBoolean(41);
                        result.Add(view);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
                return result;
            }
        }

        public Dictionary<Guid, bool> QueryPolicyIds(PolicyQueryParameter parameter)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                Dictionary<Guid, bool> result = new Dictionary<Guid, bool>();
                dbOperator.AddParameter("@iType", parameter.PolicyType);
                if (!string.IsNullOrWhiteSpace(parameter.Departure))
                    dbOperator.AddParameter("@iDeparture", parameter.Departure);
                if (!string.IsNullOrWhiteSpace(parameter.Transit))
                    dbOperator.AddParameter("@iTransit", parameter.Transit);
                if (!string.IsNullOrWhiteSpace(parameter.Arrival))
                    dbOperator.AddParameter("@iArrival", parameter.Arrival);
                if (!string.IsNullOrWhiteSpace(parameter.Airline))
                    dbOperator.AddParameter("@iAirline", parameter.Airline);
                if (!string.IsNullOrWhiteSpace(parameter.OfficeCode))
                    dbOperator.AddParameter("@iOfficeNo", parameter.OfficeCode);
                if (!string.IsNullOrWhiteSpace(parameter.Bunks))
                    dbOperator.AddParameter("@iBunks", parameter.Bunks);
                if (!string.IsNullOrWhiteSpace(parameter.Creator))
                    dbOperator.AddParameter("@iCreator", parameter.Creator);
                if (parameter.Audited != null)
                    dbOperator.AddParameter("@iAudited", parameter.Audited);
                if (parameter.Freezed != null)
                    dbOperator.AddParameter("@iFreezed", parameter.Freezed);
                if (parameter.Effective != null)
                    dbOperator.AddParameter("@iEffective", parameter.Effective);
                if (parameter.TicketType != null)
                    dbOperator.AddParameter("@iTicketType", parameter.TicketType);
                if (parameter.VoyageType != null)
                    dbOperator.AddParameter("@iVoyageType", parameter.VoyageType);
                if (parameter.Suspended != null)
                    dbOperator.AddParameter("@iSuspended", parameter.Suspended);
                if (parameter.Owner != null)
                    dbOperator.AddParameter("@iOwner", parameter.Owner);
                if (parameter.DepartureDateStart != null)
                    dbOperator.AddParameter("@iDepartureDateStart", parameter.DepartureDateStart.Value.Date);
                if (parameter.DepartureDateEnd != null)
                    dbOperator.AddParameter("@iDepartureDateEnd", parameter.DepartureDateEnd.Value.Date);
                if (parameter.PubDateStart != null)
                    dbOperator.AddParameter("@iPubDateStart", parameter.PubDateStart.Date);
                if (parameter.PubDateEnd != null)
                    dbOperator.AddParameter("@iPubDateEnd", parameter.PubDateEnd.Date);
                if (parameter.InternalCommissionLower != null)
                    dbOperator.AddParameter("@iInternalCommissionLower", parameter.InternalCommissionLower);
                if (parameter.InternalCommissionUpper != null)
                    dbOperator.AddParameter("@iInternalCommissionUpper", parameter.InternalCommissionUpper);
                if (parameter.SubordinateCommissionLower != null)
                    dbOperator.AddParameter("@iSubordinateCommissionLower", parameter.SubordinateCommissionLower);
                if (parameter.SubordinateCommissionUpper != null)
                    dbOperator.AddParameter("@iSubordinateCommissionUpper", parameter.SubordinateCommissionUpper);
                if (parameter.ProfessionCommissionLower != null)
                    dbOperator.AddParameter("@iProfessionCommissionLower", parameter.ProfessionCommissionLower);
                if (parameter.ProfessionCommissionUpper != null)
                    dbOperator.AddParameter("@iProfessionCommissionUpper", parameter.ProfessionCommissionUpper);

                using (var reader = dbOperator.ExecuteReader("dbo.P_QueryPolicyIds", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetGuid(0), reader.GetBoolean(1));
                        //result.Add(reader.GetGuid(0));
                    }
                }
                return result;
            }
        }
        public IEnumerable<PolicyInfoBase> QueryPolicies(string departure, DateTime flightStartDate, DateTime flightEndDate, VoyageType voyageType, PolicyType policyType, string airline)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var result = new List<PolicyInfoBase>();
                dbOperator.ClearParameters();
                dbOperator.AddParameter("@iDeparture", departure);
                dbOperator.AddParameter("@iFlightStartDate", flightStartDate.Date);
                dbOperator.AddParameter("@iFlightEndDate", flightEndDate.Date);
                dbOperator.AddParameter("@iVoyageType", (byte)voyageType);
                dbOperator.AddParameter("@iPolicyType", (byte)policyType);
                if (!string.IsNullOrWhiteSpace(airline))
                {
                    dbOperator.AddParameter("@iAirline", airline);
                }
                var datas = dbOperator.ExecuteDataSet("P_QueryPolicies", CommandType.StoredProcedure, "normal", "bargain", "speical", "team", "notch");
                // 普通政策
                var normalPolicies = datas.Tables[0];
                if (normalPolicies.Columns.Count > 1)
                {
                    foreach (DataRow normalRow in normalPolicies.Rows)
                    {
                        var policy = new NormalPolicyInfo
                                         {
                                             PolicyType = PolicyType.Normal,
                                             Id = DataHelper.GetGuid(normalRow["Id"]),
                                             Owner = DataHelper.GetGuid(normalRow["Owner"]),
                                             Airline = DataHelper.GetString(normalRow["Airline"]),
                                             VoyageType = DataHelper.GetEnumValue<VoyageType>(normalRow["VoyageType"]),
                                             CustomCode = DataHelper.GetString(normalRow["CustomCode"]),
                                             OfficeCode = DataHelper.GetString(normalRow["OfficeCode"]),
                                             Departure = DataHelper.GetString(normalRow["Departure"]),
                                             Transit = DataHelper.GetString(normalRow["Transit"]),
                                             Arrival = DataHelper.GetString(normalRow["Arrival"]),
                                             ExceptAirways = DataHelper.GetString(normalRow["ExceptAirways"]),
                                             DepartureFlightsFilterType = DataHelper.GetEnumValue<LimitType>(normalRow["DepartureFlightsFilterType"]),
                                             DepartureFlightsFilter = DataHelper.GetString(normalRow["DepartureFlightsFilter"]),
                                             ReturnFlightsFilterType = DataHelper.GetEnumValue<LimitType>(normalRow["ReturnFlightsFilterType"]),
                                             ReturnFlightsFilter = DataHelper.GetString(normalRow["ReturnFlightsFilter"]),
                                             DepartureDateFilter = DataHelper.GetString(normalRow["DepartureDateFilter"]),
                                             DepartureWeekFilter = DataHelper.GetString(normalRow["DepartureWeekFilter"]),
                                             DepartureDateStart = DataHelper.GetDateTime(normalRow["DepartureDateStart"]),
                                             DepartureDateEnd = DataHelper.GetDateTime(normalRow["DepartureDateEnd"]),
                                             StartProcessDate = DataHelper.GetDateTime(normalRow["StartPrintDate"]),
                                             Berths = DataHelper.GetString(normalRow["Berths"]),
                                             TicketType = DataHelper.GetEnumValue<TicketType>(normalRow["TicketType"]),
                                             IsInternal = DataHelper.GetBoolean(normalRow["IsInternal"]),
                                             InternalCommission = DataHelper.GetDecimal(normalRow["InternalCommission"]),
                                             SubordinateCommission = DataHelper.GetDecimal(normalRow["SubordinateCommission"]),
                                             IsPeer = DataHelper.GetBoolean(normalRow["IsPeer"]),
                                             ProfessionCommission = DataHelper.GetDecimal(normalRow["ProfessionCommission"]),
                                             Remark = DataHelper.GetString(normalRow["Remark"]),
                                             Condition = DataHelper.GetString(normalRow["DrawerCondition"]),
                                             ChangePNR = DataHelper.GetBoolean(normalRow["ChangePNR"]),
                                             AutoPrint = DataHelper.GetBoolean(normalRow["AutoPrint"]),
                                             SuitReduce = DataHelper.GetBoolean(normalRow["SuitReduce"]),
                                             NeedAUTH = DataHelper.GetBoolean(normalRow["ImpowerOffice"]),
                                             MultiConjunctionSuitable = DataHelper.GetBoolean(normalRow["MultiSuitReduce"]),
                                             PrintBeforeTwoHours = DataHelper.GetBoolean(normalRow["PrintBeforeTwoHours"]),
                                             Creator = DataHelper.GetString(normalRow["Creator"]),
                                             CreateTime = DataHelper.GetDateTime(normalRow["CreateTime"]),
                                             AuditTime = DataHelper.GetNullableDateTime(normalRow["AuditTime"]),
                                             SuspendByPlatform = false,
                                             Suspended = false,
                                             Freezed = false,
                                             Audited = true,
                                             Enabled = true
                                         };
                        result.Add(policy);
                    }
                }
                // 特价政策
                var bargainPolicies = datas.Tables[1];
                if (bargainPolicies.Columns.Count > 1)
                {
                    foreach (DataRow bargainRow in bargainPolicies.Rows)
                    {
                        var policy = new BargainPolicyInfo
                        {
                            PolicyType = PolicyType.Bargain,
                            Id = DataHelper.GetGuid(bargainRow["Id"]),
                            Owner = DataHelper.GetGuid(bargainRow["Owner"]),
                            Airline = DataHelper.GetString(bargainRow["Airline"]),
                            VoyageType = DataHelper.GetEnumValue<VoyageType>(bargainRow["VoyageType"]),
                            CustomCode = DataHelper.GetString(bargainRow["CustomCode"]),
                            OfficeCode = DataHelper.GetString(bargainRow["OfficeCode"]),
                            Departure = DataHelper.GetString(bargainRow["Departure"]),
                            Transit = DataHelper.GetString(bargainRow["Transit"]),
                            Arrival = DataHelper.GetString(bargainRow["Arrival"]),
                            ExceptAirways = DataHelper.GetString(bargainRow["ExceptAirways"]),
                            DepartureFlightsFilterType = DataHelper.GetEnumValue<LimitType>(bargainRow["DepartureFlightsFilterType"]),
                            DepartureFlightsFilter = DataHelper.GetString(bargainRow["DepartureFlightsFilter"]),
                            ReturnFlightsFilterType = DataHelper.GetEnumValue<LimitType>(bargainRow["ReturnFlightsFilterType"]),
                            ReturnFlightsFilter = DataHelper.GetString(bargainRow["ReturnFlightsFilter"]),
                            DepartureDateFilter = DataHelper.GetString(bargainRow["DepartureDateFilter"]),
                            DepartureWeekFilter = DataHelper.GetString(bargainRow["DepartureWeekFilter"]),
                            DepartureDateStart = DataHelper.GetDateTime(bargainRow["DepartureDateStart"]),
                            DepartureDateEnd = DataHelper.GetDateTime(bargainRow["DepartureDateEnd"]),
                            StartProcessDate = DataHelper.GetDateTime(bargainRow["StartPrintDate"]),
                            Berths = DataHelper.GetString(bargainRow["Berths"]),
                            TicketType = DataHelper.GetEnumValue<TicketType>(bargainRow["TicketType"]),
                            IsInternal = DataHelper.GetBoolean(bargainRow["IsInternal"]),
                            InternalCommission = DataHelper.GetDecimal(bargainRow["InternalCommission"]),
                            SubordinateCommission = DataHelper.GetDecimal(bargainRow["SubordinateCommission"]),
                            IsPeer = DataHelper.GetBoolean(bargainRow["IsPeer"]),
                            ProfessionCommission = DataHelper.GetDecimal(bargainRow["ProfessionCommission"]),
                            Remark = DataHelper.GetString(bargainRow["Remark"]),
                            Condition = DataHelper.GetString(bargainRow["DrawerCondition"]),
                            ChangePNR = DataHelper.GetBoolean(bargainRow["ChangePNR"]),
                            NeedAUTH = DataHelper.GetBoolean(bargainRow["ImpowerOffice"]),
                            MultiConjunctionSuitable = DataHelper.GetBoolean(bargainRow["MultiSuitReduce"]),
                            PrintBeforeTwoHours = DataHelper.GetBoolean(bargainRow["PrintBeforeTwoHours"]),
                            PriceType = DataHelper.GetEnumValue<PriceType>(bargainRow["PriceType"]),
                            Price = DataHelper.GetDecimal(bargainRow["Price"]),
                            ChangeRegulation = DataHelper.GetString(bargainRow["ChangeRegulation"]),
                            EndorseRegulation = DataHelper.GetString(bargainRow["EndorseRegulation"]),
                            RefundRegulation = DataHelper.GetString(bargainRow["RefundRegulation"]),
                            InvalidRegulation = DataHelper.GetString(bargainRow["InvalidRegulation"]),
                            BeforehandDays = DataHelper.GetInteger(bargainRow["BeforehandDays"]),
                            MaxBeforehandDays = DataHelper.GetInteger(bargainRow["MostBeforehandDays"]),
                            TravelDays = DataHelper.GetInteger(bargainRow["TravelDays"]),
                            Creator = DataHelper.GetString(bargainRow["Creator"]),
                            CreateTime = DataHelper.GetDateTime(bargainRow["CreateTime"]),
                            AuditTime = DataHelper.GetNullableDateTime(bargainRow["AuditTime"]),
                            SuspendByPlatform = false,
                            Suspended = false,
                            Freezed = false,
                            Audited = true,
                            Enabled = true
                        };
                        result.Add(policy);
                    }
                }
                // 特殊政策
                var specialPolicies = datas.Tables[2];
                if (specialPolicies.Columns.Count > 1)
                {
                    foreach (DataRow specialRow in specialPolicies.Rows)
                    {
                        var policy = new SpecialPolicyInfo
                        {
                            PolicyType = PolicyType.Special,
                            Id = DataHelper.GetGuid(specialRow["Id"]),
                            Owner = DataHelper.GetGuid(specialRow["Owner"]),
                            Airline = DataHelper.GetString(specialRow["Airline"]),
                            VoyageType = DataHelper.GetEnumValue<VoyageType>(specialRow["VoyageType"]),
                            CustomCode = DataHelper.GetString(specialRow["CustomCode"]),
                            OfficeCode = DataHelper.GetString(specialRow["OfficeCode"]),
                            Departure = DataHelper.GetString(specialRow["Departure"]),
                            Arrival = DataHelper.GetString(specialRow["Arrival"]),
                            ExceptAirways = DataHelper.GetString(specialRow["ExceptAirways"]),
                            DepartureFlightsFilterType = DataHelper.GetEnumValue<LimitType>(specialRow["DepartureFlightsFilterType"]),
                            DepartureFlightsFilter = DataHelper.GetString(specialRow["DepartureFlightsFilter"]),
                            DepartureDateFilter = DataHelper.GetString(specialRow["DepartureDateFilter"]),
                            DepartureWeekFilter = DataHelper.GetString(specialRow["DepartureWeekFilter"]),
                            DepartureDateStart = DataHelper.GetDateTime(specialRow["DepartureDateStart"]),
                            DepartureDateEnd = DataHelper.GetDateTime(specialRow["DepartureDateEnd"]),
                            StartProcessDate = DataHelper.GetDateTime(specialRow["ProvideDate"]),
                            Berths = DataHelper.GetString(specialRow["Berths"]),
                            TicketType = DataHelper.GetEnumValue<TicketType>(specialRow["TicketType"]),
                            Type = DataHelper.GetEnumValue<SpecialProductType>(specialRow["Type"]),
                            PriceType = DataHelper.GetEnumValue<PriceType>(specialRow["PriceType"]),
                            Price = DataHelper.GetDecimal(specialRow["Price"]),
                            IsInternal = DataHelper.GetBoolean(specialRow["IsInternal"]),
                            InternalCommission = DataHelper.GetDecimal(specialRow["InternalCommission"]),
                            SubordinateCommission = DataHelper.GetDecimal(specialRow["SubordinateCommission"]),
                            IsPeer = DataHelper.GetBoolean(specialRow["IsPeer"]),
                            ProfessionCommission = DataHelper.GetDecimal(specialRow["ProfessionCommission"]),
                            Remark = DataHelper.GetString(specialRow["Remark"]),
                            Condition = DataHelper.GetString(specialRow["DrawerCondition"]),
                            NeedAUTH = DataHelper.GetBoolean(specialRow["ImpowerOffice"]),
                            PrintBeforeTwoHours = DataHelper.GetBoolean(specialRow["PrintBeforeTwoHours"]),
                            SynBlackScreen = DataHelper.GetBoolean(specialRow["SynBlackScreen"]),
                            ConfirmResource = DataHelper.GetBoolean(specialRow["ConfirmResource"]),
                            LowNoType = DataHelper.GetEnumValue<LowNoType>(specialRow["LowNoType"]),
                            LowNoMinPrice = DataHelper.GetDecimal(specialRow["LowNoMinPrice"]),
                            LowNoMaxPrice = DataHelper.GetDecimal(specialRow["LowNoMaxPrice"]),
                            IsSeat = DataHelper.GetBoolean(specialRow["IsSeat"]),
                            BeforehandDays = DataHelper.GetInteger(specialRow["BeforehandDays"]),
                            MaxBeforehandDays = -1,
                            ResourceAmount = DataHelper.GetInteger(specialRow["ResourceAmount"]),
                            IsBargainBerths = DataHelper.GetBoolean(specialRow["IsBargainBerths"]),
                            ChangeRegulation = DataHelper.GetString(specialRow["ChangeRegulation"]),
                            EndorseRegulation = DataHelper.GetString(specialRow["EndorseRegulation"]),
                            RefundRegulation = DataHelper.GetString(specialRow["RefundRegulation"]),
                            InvalidRegulation = DataHelper.GetString(specialRow["InvalidRegulation"]),
                            Creator = DataHelper.GetString(specialRow["Creator"]),
                            CreateTime = DataHelper.GetDateTime(specialRow["CreateTime"]),
                            AuditTime = DataHelper.GetNullableDateTime(specialRow["AuditTime"]),
                            SuspendByPlatform = false,
                            Suspended = false,
                            Freezed = false,
                            Audited = true,
                            PlatformAudited = true,
                            Enabled = true
                        };
                        result.Add(policy);
                    }
                }
                // 团队政策
                var teamPolicies = datas.Tables[3];
                if (teamPolicies.Columns.Count > 1)
                {
                    foreach (DataRow teamRow in teamPolicies.Rows)
                    {
                        var policy = new TeamPolicyInfo
                        {
                            PolicyType = PolicyType.Team,
                            Id = DataHelper.GetGuid(teamRow["Id"]),
                            Owner = DataHelper.GetGuid(teamRow["Owner"]),
                            Airline = DataHelper.GetString(teamRow["Airline"]),
                            VoyageType = DataHelper.GetEnumValue<VoyageType>(teamRow["VoyageType"]),
                            CustomCode = DataHelper.GetString(teamRow["CustomCode"]),
                            OfficeCode = DataHelper.GetString(teamRow["OfficeCode"]),
                            Departure = DataHelper.GetString(teamRow["Departure"]),
                            Transit = DataHelper.GetString(teamRow["Transit"]),
                            Arrival = DataHelper.GetString(teamRow["Arrival"]),
                            ExceptAirways = DataHelper.GetString(teamRow["ExceptAirways"]),
                            DepartureFlightsFilterType = DataHelper.GetEnumValue<LimitType>(teamRow["DepartureFlightsFilterType"]),
                            DepartureFlightsFilter = DataHelper.GetString(teamRow["DepartureFlightsFilter"]),
                            ReturnFlightsFilterType = DataHelper.GetEnumValue<LimitType>(teamRow["ReturnFlightsFilterType"]),
                            ReturnFlightsFilter = DataHelper.GetString(teamRow["ReturnFlightsFilter"]),
                            DepartureDateFilter = DataHelper.GetString(teamRow["DepartureDateFilter"]),
                            DepartureWeekFilter = DataHelper.GetString(teamRow["DepartureWeekFilter"]),
                            DepartureDateStart = DataHelper.GetDateTime(teamRow["DepartureDateStart"]),
                            DepartureDateEnd = DataHelper.GetDateTime(teamRow["DepartureDateEnd"]),
                            StartProcessDate = DataHelper.GetDateTime(teamRow["StartPrintDate"]),
                            Berths = DataHelper.GetString(teamRow["Berths"]),
                            TicketType = DataHelper.GetEnumValue<TicketType>(teamRow["TicketType"]),
                            IsInternal = DataHelper.GetBoolean(teamRow["IsInternal"]),
                            InternalCommission = DataHelper.GetDecimal(teamRow["InternalCommission"]),
                            SubordinateCommission = DataHelper.GetDecimal(teamRow["SubordinateCommission"]),
                            IsPeer = DataHelper.GetBoolean(teamRow["IsPeer"]),
                            ProfessionCommission = DataHelper.GetDecimal(teamRow["ProfessionCommission"]),
                            Remark = DataHelper.GetString(teamRow["Remark"]),
                            Condition = DataHelper.GetString(teamRow["DrawerCondition"]),
                            ChangePNR = DataHelper.GetBoolean(teamRow["ChangePNR"]),
                            AutoPrint = DataHelper.GetBoolean(teamRow["AutoPrint"]),
                            SuitReduce = DataHelper.GetBoolean(teamRow["SuitReduce"]),
                            NeedAUTH = DataHelper.GetBoolean(teamRow["ImpowerOffice"]),
                            MultiConjunctionSuitable = DataHelper.GetBoolean(teamRow["MultiSuitReduce"]),
                            PrintBeforeTwoHours = DataHelper.GetBoolean(teamRow["PrintBeforeTwoHours"]),
                            Creator = DataHelper.GetString(teamRow["Creator"]),
                            CreateTime = DataHelper.GetDateTime(teamRow["CreateTime"]),
                            AuditTime = DataHelper.GetNullableDateTime(teamRow["AuditTime"]),
                            SuspendByPlatform = false,
                            Suspended = false,
                            Freezed = false,
                            Audited = true,
                            Enabled = true
                        };
                        result.Add(policy);
                    }
                }
                // 缺口政策
                var notchPolicies = datas.Tables[4];
                if (notchPolicies.Columns.Count > 1)
                {
                    foreach (DataRow notchRow in notchPolicies.Rows)
                    {
                        var policy = new NotchPolicyInfo
                        {
                            PolicyType = PolicyType.Normal,
                            Id = DataHelper.GetGuid(notchRow["Id"]),
                            Owner = DataHelper.GetGuid(notchRow["Owner"]),
                            Airline = DataHelper.GetString(notchRow["Airline"]),
                            VoyageType = DataHelper.GetEnumValue<VoyageType>(notchRow["VoyageType"]),
                            CustomCode = DataHelper.GetString(notchRow["CustomCode"]),
                            OfficeCode = DataHelper.GetString(notchRow["OfficeCode"]),
                            Departure = DataHelper.GetString(notchRow["Departure"]),
                            Arrival = DataHelper.GetString(notchRow["Arrival"]),
                            ExceptAirways = DataHelper.GetString(notchRow["ExceptAirways"]),
                            DepartureFlightsFilterType = DataHelper.GetEnumValue<LimitType>(notchRow["DepartureFlightsFilterType"]),
                            DepartureFlightsFilter = DataHelper.GetString(notchRow["DepartureFlightsFilter"]),
                            DepartureDateFilter = DataHelper.GetString(notchRow["DepartureDateFilter"]),
                            DepartureWeekFilter = DataHelper.GetString(notchRow["DepartureWeekFilter"]),
                            DepartureDateStart = DataHelper.GetDateTime(notchRow["DepartureDateStart"]),
                            DepartureDateEnd = DataHelper.GetDateTime(notchRow["DepartureDateEnd"]),
                            StartProcessDate = DataHelper.GetDateTime(notchRow["StartPrintDate"]),
                            Berths = DataHelper.GetString(notchRow["Berths"]),
                            TicketType = DataHelper.GetEnumValue<TicketType>(notchRow["TicketType"]),
                            IsInternal = DataHelper.GetBoolean(notchRow["IsInternal"]),
                            InternalCommission = DataHelper.GetDecimal(notchRow["InternalCommission"]),
                            SubordinateCommission = DataHelper.GetDecimal(notchRow["SubordinateCommission"]),
                            IsPeer = DataHelper.GetBoolean(notchRow["IsPeer"]),
                            ProfessionCommission = DataHelper.GetDecimal(notchRow["ProfessionCommission"]),
                            Remark = DataHelper.GetString(notchRow["Remark"]),
                            Condition = DataHelper.GetString(notchRow["DrawerCondition"]),
                            ChangePNR = DataHelper.GetBoolean(notchRow["ChangePNR"]),
                            NeedAUTH = DataHelper.GetBoolean(notchRow["ImpowerOffice"]),
                            PrintBeforeTwoHours = DataHelper.GetBoolean(notchRow["PrintBeforeTwoHours"]),
                            Creator = DataHelper.GetString(notchRow["Creator"]),
                            CreateTime = DataHelper.GetDateTime(notchRow["CreateTime"]),
                            AuditTime = DataHelper.GetNullableDateTime(notchRow["AuditTime"]),
                            SuspendByPlatform = false,
                            Suspended = false,
                            Freezed = false,
                            Audited = true,
                            Enabled = true
                        };
                        result.Add(policy);
                    }
                }
                return result;
            }
        }
        public NormalPolicy QueryNormalPolicy(Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"SELECT [Id]
                            ,[Owner]
                            ,[Airline]
                            ,[OfficeCode]
                            ,[CustomCode]
                            ,[ImpowerOffice]
                            ,[IsInternal]
                            ,[IsPeer]
                            ,[VoyageType]
                            ,[Departure]
                            ,[Transit]
                            ,[Arrival]
                            ,[DepartureDateStart]
                            ,[DepartureDateEnd]
                            ,[DepartureFlightsFilter]
                            ,[DepartureDateFilter]
                            ,[DepartureWeekFilter]
                            ,[DepartureFlightsFilterType]
                            ,[ReturnFlightsFilter]
                            ,[ReturnFlightsFilterType]
                            ,[ExceptAirways]
                            ,[DrawerCondition]
                            ,[Remark]
                            ,[Berths]
                            ,[InternalCommission]
                            ,[SubordinateCommission]
                            ,[ProfessionCommission]
                            ,[AutoAudit]
                            ,[ChangePNR]
                            ,[AutoPrint]
                            ,[SuitReduce]
                            ,[TicketType]
                            ,[StartPrintDate]
                            ,[Suspended]
                            ,[Freezed]
                            ,[Audited]
                            ,[CreateTime]
                            ,[AuditTime]
                            ,[Creator]
                            ,[MultiSuitReduce],[AbbreviateName],[PrintBeforeTwoHours]
                        FROM [dbo].[T_NormalPolicy] WHERE ID = @ID";
                dbOperator.AddParameter("@ID", policyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    NormalPolicy view = null;
                    if (reader.Read())
                    {
                        view = new NormalPolicy();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.ImpowerOffice = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.Departure = reader.GetString(9);
                        view.Transit = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.DepartureDateStart = reader.GetDateTime(12);
                        view.DepartureDateEnd = reader.GetDateTime(13);
                        view.DepartureFlightsFilter = reader.GetString(14);
                        view.DepartureDateFilter = reader.GetString(15);
                        view.DepartureWeekFilter = reader.GetString(16);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(17); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude);
                        view.ReturnFlightsFilter = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                        view.ReturnFlightsFilterType = reader.GetValue(19) == DBNull.Value ? LimitType.None : (LimitType)reader.GetByte(19);
                        view.ExceptAirways = reader.GetString(20);
                        view.DrawerCondition = reader.GetString(21);
                        view.Remark = reader.GetString(22);
                        view.Berths = reader.GetString(23);
                        view.InternalCommission = reader.GetDecimal(24);
                        view.SubordinateCommission = reader.GetDecimal(25);
                        view.ProfessionCommission = reader.GetDecimal(26);
                        view.AutoAudit = reader.GetValue(27) == DBNull.Value ? false : reader.GetBoolean(27);
                        view.ChangePNR = reader.GetBoolean(28);
                        view.AutoPrint = reader.GetBoolean(29);
                        view.SuitReduce = reader.GetBoolean(30);
                        view.TicketType = (TicketType)reader.GetByte(31);
                        view.StartPrintDate = reader.GetDateTime(32);
                        view.Suspended = reader.GetBoolean(33);
                        view.Freezed = reader.GetBoolean(34);
                        view.Audited = reader.GetBoolean(35);
                        view.CreateTime = reader.GetDateTime(36);
                        view.AuditTime = reader.GetValue(37) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(37);
                        view.Creator = reader.GetString(38);
                        view.MultiSuitReduce = reader.GetBoolean(39);
                        view.AbbreviateName = reader.GetString(40);
                        view.PrintBeforeTwoHours = reader.GetBoolean(41);
                    }
                    return view;
                }
            }
        }

        public SpecialPolicy QuerySpecialPolicy(Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"SELECT [Id]
                              ,[Owner]
                              ,[Type]
                              ,[OfficeCode]
                              ,[CustomCode]
                              ,[ImpowerOffice]
                              ,[IsPeer]
                              ,[IsInternal]
                              ,[Airline]
                              ,[VoyageType]
                              ,[Departure]
                              ,[Arrival]
                              ,[ExceptAirways]
                              ,[DepartureDateStart]
                              ,[DepartureDateEnd]
                              ,[DepartureDateFilter]
                              ,[DepartureWeekFilter]
                              ,[DepartureFlightsFilter]
                              ,[DepartureFlightsFilterType]
                              ,[BeforehandDays]
                              ,[InvalidRegulation]
                              ,[ChangeRegulation]
                              ,[EndorseRegulation]
                              ,[RefundRegulation]
                              ,[DrawerCondition]
                              ,[Remark]
                              ,[Price]
                              ,[ProvideDate]
                              ,[ResourceAmount]
                              ,[AutoAudit]
                              ,[ConfirmResource]
                              ,[Suspended]
                              ,[Freezed]
                              ,[Audited]
                              ,[PlatformAudited]
                              ,[SynBlackScreen]
                              ,[Berths]
                              ,[TicketType]
                              ,[PriceType]
                              ,[InternalCommission]
                              ,[SubordinateCommission]
                              ,[ProfessionCommission]
                              ,[CreateTime]
                              ,[AuditTime]
                              ,[Creator]
                              ,[IsBargainBerths]
                              ,[IsSeat]
                              ,[AbbreviateName]
                              ,[PrintBeforeTwoHours]
                              ,LowNoType,LowNoMaxPrice,LowNoMinPrice
                          FROM [dbo].[T_SpecialPolicy] WHERE ID = @ID";
                dbOperator.AddParameter("@ID", policyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    SpecialPolicy view = null;
                    if (reader.Read())
                    {
                        view = new SpecialPolicy();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Type = (SpecialProductType)reader.GetByte(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.ImpowerOffice = reader.GetBoolean(5);
                        view.IsPeer = reader.GetBoolean(6);
                        view.IsInternal = reader.GetBoolean(7);
                        view.Airline = reader.GetString(8);
                        view.VoyageType = (VoyageType)reader.GetByte(9);
                        view.Departure = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.ExceptAirways = reader.GetString(12);
                        view.DepartureDateStart = reader.GetDateTime(13);
                        view.DepartureDateEnd = reader.GetDateTime(14);
                        view.DepartureDateFilter = reader.GetString(15);
                        view.DepartureWeekFilter = reader.GetString(16);
                        view.DepartureFlightsFilter = reader.GetString(17);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(18); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude); 
                        view.BeforehandDays = reader.GetInt16(19);

                        view.InvalidRegulation = reader.GetString(20);
                        view.ChangeRegulation = reader.GetString(21);
                        view.EndorseRegulation = reader.GetString(22);
                        view.RefundRegulation = reader.GetString(23);
                        view.DrawerCondition = reader.GetString(24);

                        view.Remark = reader.GetString(25);
                        view.Price = reader.GetDecimal(26);
                        view.ProvideDate = reader.GetDateTime(27);
                        view.ResourceAmount = reader.GetInt32(28);
                        view.AutoAudit = reader.GetValue(29) == DBNull.Value ? false : reader.GetBoolean(29);
                        view.ConfirmResource = reader.GetBoolean(30);
                        view.Suspended = reader.GetBoolean(31);
                        view.Freezed = reader.GetBoolean(32);
                        view.Audited = reader.GetBoolean(33);
                        view.PlatformAudited = reader.GetValue(34) == DBNull.Value ? false : reader.GetBoolean(34);
                        view.SynBlackScreen = reader.GetBoolean(35);
                        view.Berths = reader.GetString(36);
                        view.TicketType = (TicketType)reader.GetByte(37);
                        view.PriceType = (PriceType)reader.GetByte(38);

                        view.InternalCommission = reader.GetDecimal(39);
                        view.SubordinateCommission = reader.GetDecimal(40);
                        view.ProfessionCommission = reader.GetDecimal(41);
                        view.CreateTime = reader.GetDateTime(42);
                        view.AuditTime = reader.GetValue(43) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(43);
                        view.Creator = reader.GetString(44);
                        view.IsBargainBerths = reader.GetBoolean(45);
                        view.IsSeat = reader.GetBoolean(46);
                        view.AbbreviateName = reader.GetString(47);
                        view.PrintBeforeTwoHours = reader.GetBoolean(48);
                        view.LowNoType = (LowNoType)reader.GetByte(49);
                        view.LowNoMaxPrice = reader.GetDecimal(50);
                        view.LowNoMinPrice = reader.GetDecimal(51);
                    }
                    return view;
                }
            }
        }

        public BargainPolicy QueryBargainPolicy(Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"SELECT [Id]
                              ,[Owner]
                              ,[Airline]
                              ,[OfficeCode]
                              ,[CustomCode]
                              ,[ImpowerOffice]
                              ,[IsInternal]
                              ,[IsPeer]
                              ,[VoyageType]
                              ,[Departure]
                              ,[Transit]
                              ,[Arrival]
                              ,[ExceptAirways]
                              ,[DepartureDateStart]
                              ,[DepartureDateEnd]
                              ,[DepartureFlightsFilter]
                              ,[DepartureDateFilter]
                              ,[DepartureWeekFilter]
                              ,[DepartureFlightsFilterType]
                              ,[ReturnFlightsFilter]
                              ,[ReturnFlightsFilterType]
                              ,[BeforehandDays]
                              ,[TravelDays]
                              ,[InvalidRegulation]
                              ,[ChangeRegulation]
                              ,[EndorseRegulation]
                              ,[RefundRegulation]
                              ,[DrawerCondition]
                              ,[Remark]
                              ,[Berths]
                              ,[InternalCommission]
                              ,[SubordinateCommission]
                              ,[ProfessionCommission]
                              ,[Price]
                              ,[PriceType]
                              ,[AutoAudit]
                              ,[ChangePNR]
                              ,[TicketType]
                              ,[StartPrintDate]
                              ,[Suspended]
                              ,[Freezed]
                              ,[Audited]
                              ,[CreateTime]
                              ,[AuditTime]
                              ,[Creator]
                              ,[MultiSuitReduce]
                              ,[MostBeforehandDays],[AbbreviateName],[PrintBeforeTwoHours]
                          FROM [dbo].[T_BargainPolicy] WHERE ID = @ID";
                dbOperator.AddParameter("@ID", policyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    BargainPolicy view = null;
                    if (reader.Read())
                    {
                        view = new BargainPolicy();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.ImpowerOffice = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.Departure = reader.GetString(9);
                        view.Transit = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.ExceptAirways = reader.GetString(12);
                        view.DepartureDateStart = reader.GetDateTime(13);
                        view.DepartureDateEnd = reader.GetDateTime(14);
                        view.DepartureFlightsFilter = reader.GetString(15);
                        view.DepartureDateFilter = reader.GetString(16);
                        view.DepartureWeekFilter = reader.GetString(17);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(18); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude);
                        view.ReturnFlightsFilter = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                        view.ReturnFlightsFilterType = reader.GetValue(20) == DBNull.Value ? LimitType.None : (LimitType)reader.GetByte(20);
                        view.BeforehandDays = reader.GetInt16(21);
                        view.TravelDays = reader.GetInt16(22);
                        view.InvalidRegulation = reader.GetString(23);
                        view.ChangeRegulation = reader.GetString(24);
                        view.EndorseRegulation = reader.GetString(25);
                        view.RefundRegulation = reader.GetString(26);
                        view.DrawerCondition = reader.GetString(27);
                        view.Remark = reader.GetString(28);
                        view.Berths = reader.GetString(29);
                        view.InternalCommission = reader.GetDecimal(30);
                        view.SubordinateCommission = reader.GetDecimal(31);
                        view.ProfessionCommission = reader.GetDecimal(32);
                        view.Price = reader.GetDecimal(33);
                        view.PriceType = (PriceType)reader.GetByte(34);
                        view.AutoAudit = reader.GetBoolean(35);
                        view.ChangePNR = reader.GetBoolean(36);
                        view.TicketType = (TicketType)reader.GetByte(37);
                        view.StartPrintDate = reader.GetDateTime(38);
                        view.Suspended = reader.GetBoolean(39);
                        view.Freezed = reader.GetBoolean(40);
                        view.Audited = reader.GetBoolean(41);
                        view.CreateTime = reader.GetDateTime(42);
                        view.AuditTime = reader.GetValue(43) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(43);
                        view.Creator = reader.GetString(44);
                        view.MultiSuitReduce = reader.GetBoolean(45);
                        view.MostBeforehandDays = reader.GetInt16(46);
                        view.AbbreviateName = reader.GetString(47);
                        view.PrintBeforeTwoHours = reader.GetBoolean(48);
                    }
                    return view;
                }
            }
        }

        public TeamPolicy QueryTeamPolicy(Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"SELECT [Id]
                              ,[Owner]
                              ,[Airline]
                              ,[OfficeCode]
                              ,[CustomCode]
                              ,[ImpowerOffice]
                              ,[IsInternal]
                              ,[IsPeer]
                              ,[VoyageType]
                              ,[Departure]
                              ,[Transit]
                              ,[Arrival]
                              ,[DepartureDateStart]
                              ,[DepartureDateEnd]
                              ,[DepartureFlightsFilter]
                              ,[DepartureDateFilter]
                              ,[DepartureWeekFilter]
                              ,[DepartureFlightsFilterType]
                              ,[ReturnFlightsFilter]
                              ,[ReturnFlightsFilterType]
                              ,[ExceptAirways]
                              ,[DrawerCondition]
                              ,[Remark]
                              ,[Berths]
                              ,[InternalCommission]
                              ,[SubordinateCommission]
                              ,[ProfessionCommission]
                              ,[AutoAudit]
                              ,[ChangePNR]
                              ,[AutoPrint]
                              ,[SuitReduce]
                              ,[TicketType]
                              ,[StartPrintDate]
                              ,[Suspended]
                              ,[Freezed]
                              ,[Audited]
                              ,[CreateTime]
                              ,[AuditTime]
                              ,[Creator]
                              ,[MultiSuitReduce]
                              ,[AppointBerths],[AbbreviateName],[PrintBeforeTwoHours]
                          FROM  [dbo].[T_TeamPolicy] WHERE ID = @ID";
                dbOperator.AddParameter("@ID", policyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    TeamPolicy view = null;
                    if (reader.Read())
                    {
                        view = new TeamPolicy();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.ImpowerOffice = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.Departure = reader.GetString(9);
                        view.Transit = reader.GetString(10);
                        view.Arrival = reader.GetString(11);
                        view.DepartureDateStart = reader.GetDateTime(12);
                        view.DepartureDateEnd = reader.GetDateTime(13);
                        view.DepartureFlightsFilter = reader.GetString(14);
                        view.DepartureDateFilter = reader.GetString(15);
                        view.DepartureWeekFilter = reader.GetString(16);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(17); // reader.GetString(17) == "0" ? LimitType.None : (reader.GetString(17) == "1" ? LimitType.Include : LimitType.Exclude);
                        view.ReturnFlightsFilter = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                        view.ReturnFlightsFilterType = reader.GetValue(19) == DBNull.Value ? LimitType.None : (LimitType)reader.GetByte(19);
                        view.ExceptAirways = reader.GetString(20);
                        view.DrawerCondition = reader.GetString(21);
                        view.Remark = reader.GetString(22);
                        view.Berths = reader.GetString(23);
                        view.InternalCommission = reader.GetDecimal(24);
                        view.SubordinateCommission = reader.GetDecimal(25);
                        view.ProfessionCommission = reader.GetDecimal(26);
                        view.AutoAudit = reader.GetValue(27) == DBNull.Value ? false : reader.GetBoolean(27);
                        view.ChangePNR = reader.GetBoolean(28);
                        view.AutoPrint = reader.GetBoolean(29);
                        view.SuitReduce = reader.GetBoolean(30);
                        view.TicketType = (TicketType)reader.GetByte(31);
                        view.StartPrintDate = reader.GetDateTime(32);
                        view.Suspended = reader.GetBoolean(33);
                        view.Freezed = reader.GetBoolean(34);
                        view.Audited = reader.GetBoolean(35);
                        view.CreateTime = reader.GetDateTime(36);
                        view.AuditTime = reader.GetValue(37) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(37);
                        view.Creator = reader.GetString(38);
                        view.MultiSuitReduce = reader.GetBoolean(39);
                        view.AppointBerths = reader.GetValue(40) != DBNull.Value && reader.GetBoolean(40);
                        view.AbbreviateName = reader.GetString(41);
                        view.PrintBeforeTwoHours = reader.GetBoolean(42);
                    }
                    return view;
                }
            }
        }

        #endregion

        #region 修改

        public void UpdateNormalPolicy(NormalPolicy normal)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"UPDATE  [dbo].[T_NormalPolicy]
                               SET [Owner] = @Owner
                                  ,[Airline] = @Airline
                                  ,[OfficeCode] = @OfficeCode
                                  ,[CustomCode] = @CustomCode
                                  ,[ImpowerOffice] = @ImpowerOffice
                                  ,[IsInternal] = @IsInternal
                                  ,[IsPeer] = @IsPeer
                                  ,[VoyageType] = @VoyageType
                                  ,[Departure] = @Departure
                                  ,[Transit] = @Transit
                                  ,[Arrival] = @Arrival
                                  ,[DepartureDateStart] = @DepartureDateStart
                                  ,[DepartureDateEnd] = @DepartureDateEnd
                                  ,[DepartureFlightsFilter] = @DepartureFlightsFilter
                                  ,[DepartureDateFilter] = @DepartureDateFilter
                                  ,[DepartureWeekFilter] = @DepartureWeekFilter
                                  ,[DepartureFlightsFilterType] = @DepartureFlightsFilterType
                                  ,[ReturnFlightsFilter] = @ReturnFlightsFilter
                                  ,[ReturnFlightsFilterType] = @ReturnFlightsFilterType
                                  ,[ExceptAirways] = @ExceptAirways
                                  ,[DrawerCondition] = @DrawerCondition 
                                  ,[Remark] = @Remark 
                                  ,[Berths] = @Berths
                                  ,[InternalCommission] = @InternalCommission
                                  ,[SubordinateCommission] = @SubordinateCommission
                                  ,[ProfessionCommission] = @ProfessionCommission
                                  ,[AutoAudit] = @AutoAudit
                                  ,[ChangePNR] = @ChangePNR
                                  ,[AutoPrint] = @AutoPrint
                                  ,[SuitReduce] = @SuitReduce
                                  ,[TicketType] = @TicketType
                                  ,[StartPrintDate] = @StartPrintDate
                                  ,[Suspended] = @Suspended
                                  ,[Freezed] = @Freezed
                                  ,[Audited] = @Audited
                                  ,[CreateTime] = @CreateTime
                                  ,[AuditTime] = @AuditTime
                                  ,[Creator] = @Creator 
                                  ,[LastModifyTime] = @LastModifyTime 
                                  ,[MultiSuitReduce] = @MultiSuitReduce
                                  ,[PrintBeforeTwoHours] =@PrintBeforeTwoHours
                             WHERE  [Id] = @id; DELETE FROM dbo.T_NormalPolicySetting WHERE PolicyId = @delPolicyId";
                dbOperator.AddParameter("@Owner", normal.Owner);
                dbOperator.AddParameter("@Airline", normal.Airline);
                dbOperator.AddParameter("@OfficeCode", normal.OfficeCode);
                dbOperator.AddParameter("@CustomCode", normal.CustomCode);
                dbOperator.AddParameter("@ImpowerOffice", normal.ImpowerOffice);
                dbOperator.AddParameter("@IsInternal", normal.IsInternal);
                dbOperator.AddParameter("@IsPeer", normal.IsPeer);
                dbOperator.AddParameter("@VoyageType", normal.VoyageType);
                dbOperator.AddParameter("@Departure", normal.Departure);
                dbOperator.AddParameter("@Transit", normal.Transit);
                dbOperator.AddParameter("@Arrival", normal.Arrival);
                dbOperator.AddParameter("@DepartureDateStart", normal.DepartureDateStart);
                dbOperator.AddParameter("@DepartureDateEnd", normal.DepartureDateEnd);
                dbOperator.AddParameter("@DepartureFlightsFilter", normal.DepartureFlightsFilter);
                dbOperator.AddParameter("@DepartureDateFilter", normal.DepartureDateFilter);
                dbOperator.AddParameter("@DepartureWeekFilter", normal.DepartureWeekFilter);
                dbOperator.AddParameter("@DepartureFlightsFilterType", normal.DepartureFlightsFilterType);
                dbOperator.AddParameter("@ReturnFlightsFilter", string.IsNullOrWhiteSpace(normal.ReturnFlightsFilter) ? "" : normal.ReturnFlightsFilter);
                dbOperator.AddParameter("@ReturnFlightsFilterType", normal.ReturnFlightsFilterType);
                dbOperator.AddParameter("@ExceptAirways", normal.ExceptAirways);
                dbOperator.AddParameter("@DrawerCondition", normal.DrawerCondition);
                dbOperator.AddParameter("@Remark", normal.Remark);
                dbOperator.AddParameter("@Berths", normal.Berths);
                dbOperator.AddParameter("@InternalCommission", normal.InternalCommission);
                dbOperator.AddParameter("@SubordinateCommission", normal.SubordinateCommission);
                dbOperator.AddParameter("@ProfessionCommission", normal.ProfessionCommission);
                dbOperator.AddParameter("@AutoAudit", normal.AutoAudit);
                dbOperator.AddParameter("@ChangePNR", normal.ChangePNR);
                dbOperator.AddParameter("@AutoPrint", normal.AutoPrint);
                dbOperator.AddParameter("@SuitReduce", normal.SuitReduce);
                dbOperator.AddParameter("@TicketType", normal.TicketType);
                dbOperator.AddParameter("@StartPrintDate", normal.StartPrintDate);
                dbOperator.AddParameter("@Suspended", normal.Suspended);
                dbOperator.AddParameter("@Freezed", normal.Freezed);
                dbOperator.AddParameter("@Audited", normal.Audited);
                dbOperator.AddParameter("@CreateTime", normal.CreateTime);
                dbOperator.AddParameter("@LastModifyTime", normal.LastModifyTime);
                dbOperator.AddParameter("@PrintBeforeTwoHours", normal.PrintBeforeTwoHours);
                if (normal.AuditTime.HasValue)
                {
                    dbOperator.AddParameter("@AuditTime", normal.AuditTime);
                }
                else
                {
                    dbOperator.AddParameter("@AuditTime", DBNull.Value);
                }
                dbOperator.AddParameter("@Creator", normal.Creator);
                dbOperator.AddParameter("@MultiSuitReduce", normal.MultiSuitReduce);
                dbOperator.AddParameter("@Id", normal.Id);
                dbOperator.AddParameter("@delPolicyId", normal.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void UpdateBargainPolicy(BargainPolicy bargain)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"UPDATE dbo.T_BargainPolicy
                                   SET Owner = @Owner
                                      ,Airline = @Airline
                                      ,OfficeCode = @OfficeCode
                                      ,CustomCode = @CustomCode
                                      ,ImpowerOffice = @ImpowerOffice
                                      ,IsInternal = @IsInternal
                                      ,IsPeer = @IsPeer
                                      ,VoyageType = @VoyageType
                                      ,Departure = @Departure
                                      ,Transit = @Transit
                                      ,Arrival = @Arrival
                                      ,ExceptAirways = @ExceptAirways
                                      ,DepartureDateStart = @DepartureDateStart
                                      ,DepartureDateEnd = @DepartureDateEnd
                                      ,DepartureFlightsFilter = @DepartureFlightsFilter
                                      ,DepartureDateFilter = @DepartureDateFilter
                                      ,DepartureWeekFilter = @DepartureWeekFilter
                                      ,DepartureFlightsFilterType = @DepartureFlightsFilterType
                                      ,ReturnFlightsFilter = @ReturnFlightsFilter
                                      ,ReturnFlightsFilterType = @ReturnFlightsFilterType
                                      ,BeforehandDays = @BeforehandDays
                                      ,TravelDays = @TravelDays
                                      ,InvalidRegulation = @InvalidRegulation
                                      ,ChangeRegulation = @ChangeRegulation
                                      ,EndorseRegulation = @EndorseRegulation
                                      ,RefundRegulation = @RefundRegulation
                                      ,DrawerCondition = @DrawerCondition
                                      ,Remark = @Remark
                                      ,Berths = @Berths
                                      ,InternalCommission = @InternalCommission
                                      ,SubordinateCommission = @SubordinateCommission
                                      ,ProfessionCommission = @ProfessionCommission
                                      ,Price = @Price
                                      ,PriceType = @PriceType
                                      ,AutoAudit = @AutoAudit
                                      ,ChangePNR = @ChangePNR
                                      ,TicketType = @TicketType
                                      ,StartPrintDate = @StartPrintDate
                                      ,Suspended = @Suspended
                                      ,Freezed = @Freezed
                                      ,Audited = @Audited
                                      ,CreateTime = @CreateTime
                                      ,LastModifyTime = @LastModifyTime
                                      ,AuditTime = @AuditTime
                                      ,Creator = @Creator
                                      ,MultiSuitReduce = @MultiSuitReduce
                                      ,MostBeforehandDays = @MostBeforehandDays
                                      ,PrintBeforeTwoHours =@PrintBeforeTwoHours
                                 WHERE Id = @Id";
                dbOperator.AddParameter("@Owner", bargain.Owner);
                dbOperator.AddParameter("@Airline", bargain.Airline);
                dbOperator.AddParameter("@OfficeCode", bargain.OfficeCode);
                dbOperator.AddParameter("@CustomCode", bargain.CustomCode);
                dbOperator.AddParameter("@ImpowerOffice", bargain.ImpowerOffice);
                dbOperator.AddParameter("@IsInternal", bargain.IsInternal);
                dbOperator.AddParameter("@IsPeer", bargain.IsPeer);
                dbOperator.AddParameter("@VoyageType", bargain.VoyageType);
                dbOperator.AddParameter("@Departure", bargain.Departure);
                dbOperator.AddParameter("@Transit", bargain.Transit);
                dbOperator.AddParameter("@Arrival", bargain.Arrival);
                dbOperator.AddParameter("@ExceptAirways", bargain.ExceptAirways);
                dbOperator.AddParameter("@DepartureDateStart", bargain.DepartureDateStart);
                dbOperator.AddParameter("@DepartureDateEnd", bargain.DepartureDateEnd);
                dbOperator.AddParameter("@DepartureFlightsFilter", bargain.DepartureFlightsFilter);
                dbOperator.AddParameter("@DepartureDateFilter", bargain.DepartureDateFilter);
                dbOperator.AddParameter("@DepartureWeekFilter", bargain.DepartureWeekFilter);
                dbOperator.AddParameter("@DepartureFlightsFilterType", bargain.DepartureFlightsFilterType);
                dbOperator.AddParameter("@ReturnFlightsFilter", string.IsNullOrWhiteSpace(bargain.ReturnFlightsFilter) ? "" : bargain.ReturnFlightsFilter);
                dbOperator.AddParameter("@ReturnFlightsFilterType", bargain.ReturnFlightsFilterType);
                dbOperator.AddParameter("@BeforehandDays", bargain.BeforehandDays);
                dbOperator.AddParameter("@TravelDays", bargain.TravelDays);
                dbOperator.AddParameter("@InvalidRegulation", bargain.InvalidRegulation);
                dbOperator.AddParameter("@ChangeRegulation", bargain.ChangeRegulation);
                dbOperator.AddParameter("@EndorseRegulation", bargain.EndorseRegulation);
                dbOperator.AddParameter("@RefundRegulation", bargain.RefundRegulation);
                dbOperator.AddParameter("@DrawerCondition", bargain.DrawerCondition);
                dbOperator.AddParameter("@Remark", bargain.Remark);
                dbOperator.AddParameter("@Berths", bargain.Berths);
                dbOperator.AddParameter("@InternalCommission", bargain.InternalCommission);
                dbOperator.AddParameter("@SubordinateCommission", bargain.SubordinateCommission);
                dbOperator.AddParameter("@ProfessionCommission", bargain.ProfessionCommission);
                dbOperator.AddParameter("@Price", bargain.Price);
                dbOperator.AddParameter("@PriceType", bargain.PriceType);
                dbOperator.AddParameter("@AutoAudit", bargain.AutoAudit);
                dbOperator.AddParameter("@ChangePNR", bargain.ChangePNR);
                dbOperator.AddParameter("@TicketType", bargain.TicketType);
                dbOperator.AddParameter("@StartPrintDate", bargain.StartPrintDate);
                dbOperator.AddParameter("@Suspended", bargain.Suspended);
                dbOperator.AddParameter("@Freezed", bargain.Freezed);
                dbOperator.AddParameter("@Audited", bargain.Audited);
                dbOperator.AddParameter("@CreateTime", bargain.CreateTime);
                dbOperator.AddParameter("@LastModifyTime", bargain.LastModifyTime);
                dbOperator.AddParameter("@PrintBeforeTwoHours", bargain.PrintBeforeTwoHours);
                if (bargain.AuditTime.HasValue)
                {
                    dbOperator.AddParameter("@AuditTime", bargain.AuditTime);
                }
                else
                {
                    dbOperator.AddParameter("@AuditTime", DBNull.Value);
                }
                dbOperator.AddParameter("@Creator", bargain.Creator);
                dbOperator.AddParameter("@MultiSuitReduce", bargain.MultiSuitReduce);
                dbOperator.AddParameter("@MostBeforehandDays", bargain.MostBeforehandDays);
                dbOperator.AddParameter("@Id", bargain.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void UpdateSpecialPolicy(SpecialPolicy special)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"UPDATE  dbo.T_SpecialPolicy 
                           SET Owner  = @Owner
                              ,Type  = @Type
                              ,OfficeCode  = @OfficeCode
                              ,CustomCode  = @CustomCode
                              ,ImpowerOffice  = @ImpowerOffice
                              ,IsPeer  = @IsPeer
                              ,IsInternal  = @IsInternal
                              ,Airline  = @Airline
                              ,VoyageType  = @VoyageType
                              ,Departure  = @Departure
                              ,Arrival  = @Arrival
                              ,ExceptAirways  = @ExceptAirways
                              ,DepartureDateStart  = @DepartureDateStart
                              ,DepartureDateEnd  = @DepartureDateEnd
                              ,DepartureDateFilter  = @DepartureDateFilter
                              ,DepartureWeekFilter  = @DepartureWeekFilter
                              ,DepartureFlightsFilter  = @DepartureFlightsFilter
                              ,DepartureFlightsFilterType  = @DepartureFlightsFilterType
                              ,BeforehandDays  = @BeforehandDays
                              ,InvalidRegulation  = @InvalidRegulation
                              ,ChangeRegulation  = @ChangeRegulation
                              ,EndorseRegulation  = @EndorseRegulation
                              ,RefundRegulation  = @RefundRegulation
                              ,DrawerCondition  = @DrawerCondition
                              ,Remark  = @Remark
                              ,Price  = @Price
                              ,ProvideDate  = @ProvideDate
                              ,ResourceAmount  = @ResourceAmount
                              ,AutoAudit  = @AutoAudit
                              ,ConfirmResource  = @ConfirmResource
                              ,Suspended  = @Suspended
                              ,Freezed  = @Freezed
                              ,Audited  = @Audited
                              ,PlatformAudited  = @PlatformAudited
                              ,SynBlackScreen  = @SynBlackScreen
                              ,Berths  = @Berths
                              ,TicketType  = @TicketType
                              ,PriceType  = @PriceType
                              ,InternalCommission  = @InternalCommission
                              ,SubordinateCommission  = @SubordinateCommission
                              ,ProfessionCommission  = @ProfessionCommission
                              ,CreateTime  = @CreateTime
                              ,AuditTime  = @AuditTime
                              ,Creator  = @Creator
                              ,LastModifyTime  = @LastModifyTime
                              ,IsBargainBerths  = @IsBargainBerths
                              ,IsSeat  = @IsSeat
                              ,PrintBeforeTwoHours =@PrintBeforeTwoHours
                              ,LowNoType =@LowNoType
                              ,LowNoMaxPrice =@LowNoMaxPrice
                              ,LowNoMinPrice =@LowNoMinPrice
                         WHERE Id = @Id";
                dbOperator.AddParameter("@Owner", special.Owner);
                dbOperator.AddParameter("@Type", special.Type);
                dbOperator.AddParameter("@OfficeCode", special.OfficeCode);
                dbOperator.AddParameter("@CustomCode", special.CustomCode);
                dbOperator.AddParameter("@ImpowerOffice", special.ImpowerOffice);
                dbOperator.AddParameter("@IsPeer", special.IsPeer);
                dbOperator.AddParameter("@IsInternal", special.IsInternal);
                dbOperator.AddParameter("@Airline", special.Airline);
                dbOperator.AddParameter("@VoyageType", special.VoyageType);
                dbOperator.AddParameter("@Departure", special.Departure);
                dbOperator.AddParameter("@Arrival", special.Arrival);
                dbOperator.AddParameter("@ExceptAirways", special.ExceptAirways);
                dbOperator.AddParameter("@DepartureDateStart", special.DepartureDateStart);
                dbOperator.AddParameter("@DepartureDateEnd", special.DepartureDateEnd);
                dbOperator.AddParameter("@DepartureDateFilter", special.DepartureDateFilter);
                dbOperator.AddParameter("@DepartureWeekFilter", special.DepartureWeekFilter);
                dbOperator.AddParameter("@DepartureFlightsFilter", special.DepartureFlightsFilter);
                dbOperator.AddParameter("@DepartureFlightsFilterType", special.DepartureFlightsFilterType);
                dbOperator.AddParameter("@BeforehandDays", special.BeforehandDays);
                dbOperator.AddParameter("@InvalidRegulation", special.InvalidRegulation);
                dbOperator.AddParameter("@ChangeRegulation", special.ChangeRegulation);
                dbOperator.AddParameter("@EndorseRegulation", special.EndorseRegulation);
                dbOperator.AddParameter("@RefundRegulation", special.RefundRegulation);
                dbOperator.AddParameter("@DrawerCondition", special.DrawerCondition);
                dbOperator.AddParameter("@Remark", special.Remark);
                dbOperator.AddParameter("@Price", special.Price);
                dbOperator.AddParameter("@ProvideDate", special.ProvideDate);
                dbOperator.AddParameter("@ResourceAmount", special.ResourceAmount);
                dbOperator.AddParameter("@AutoAudit ", special.AutoAudit);
                dbOperator.AddParameter("@ConfirmResource", special.ConfirmResource);
                dbOperator.AddParameter("@Suspended ", special.Suspended);
                dbOperator.AddParameter("@Freezed", special.Freezed);
                dbOperator.AddParameter("@Audited", special.Audited);
                dbOperator.AddParameter("@PlatformAudited", special.PlatformAudited);
                dbOperator.AddParameter("@SynBlackScreen", special.SynBlackScreen);
                dbOperator.AddParameter("@Berths", special.Berths);
                dbOperator.AddParameter("@TicketType", special.TicketType);
                dbOperator.AddParameter("@PriceType", special.PriceType);
                dbOperator.AddParameter("@InternalCommission", special.InternalCommission);
                dbOperator.AddParameter("@SubordinateCommission", special.SubordinateCommission);
                dbOperator.AddParameter("@ProfessionCommission", special.ProfessionCommission);
                dbOperator.AddParameter("@CreateTime", special.CreateTime);
                dbOperator.AddParameter("@LastModifyTime", special.LastModifyTime);
                dbOperator.AddParameter("@PrintBeforeTwoHours", special.PrintBeforeTwoHours);
                if (special.AuditTime.HasValue)
                {
                    dbOperator.AddParameter("@AuditTime", special.AuditTime);
                }
                else
                {
                    dbOperator.AddParameter("@AuditTime", DBNull.Value);
                }
                dbOperator.AddParameter("@Creator", special.Creator);
                dbOperator.AddParameter("@IsBargainBerths", special.IsBargainBerths);
                dbOperator.AddParameter("@IsSeat", special.IsSeat);
                dbOperator.AddParameter("@Id", special.Id);
                dbOperator.AddParameter("@LowNoType", special.LowNoType);
                dbOperator.AddParameter("@LowNoMaxPrice", special.LowNoMaxPrice);
                dbOperator.AddParameter("@LowNoMinPrice", special.LowNoMinPrice);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void UpdateTeamPolicy(TeamPolicy team)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"UPDATE dbo.T_TeamPolicy
                               SET Owner = @Owner
                                  ,Airline = @Airline
                                  ,OfficeCode = @OfficeCode
                                  ,CustomCode = @CustomCode
                                  ,ImpowerOffice = @ImpowerOffice
                                  ,IsInternal = @IsInternal
                                  ,IsPeer = @IsPeer
                                  ,VoyageType = @VoyageType
                                  ,Departure = @Departure
                                  ,Transit = @Transit
                                  ,Arrival = @Arrival
                                  ,DepartureDateStart = @DepartureDateStart
                                  ,DepartureDateEnd = @DepartureDateEnd
                                  ,DepartureFlightsFilter = @DepartureFlightsFilter
                                  ,DepartureDateFilter = @DepartureDateFilter
                                  ,DepartureWeekFilter = @DepartureWeekFilter
                                  ,DepartureFlightsFilterType = @DepartureFlightsFilterType
                                  ,ReturnFlightsFilter = @ReturnFlightsFilter
                                  ,ReturnFlightsFilterType = @ReturnFlightsFilterType
                                  ,ExceptAirways = @ExceptAirways
                                  ,DrawerCondition = @DrawerCondition
                                  ,Remark = @Remark
                                  ,AppointBerths = @AppointBerths
                                  ,Berths = @Berths
                                  ,InternalCommission = @InternalCommission
                                  ,SubordinateCommission = @SubordinateCommission
                                  ,ProfessionCommission = @ProfessionCommission
                                  ,AutoAudit = @AutoAudit
                                  ,ChangePNR = @ChangePNR
                                  ,AutoPrint = @AutoPrint
                                  ,SuitReduce = @SuitReduce
                                  ,TicketType = @TicketType
                                  ,StartPrintDate = @StartPrintDate
                                  ,Suspended = @Suspended
                                  ,Freezed = @Freezed
                                  ,Audited = @Audited
                                  ,CreateTime = @CreateTime
                                  ,AuditTime = @AuditTime
                                  ,Creator = @Creator
                                  ,LastModifyTime = @LastModifyTime
                                  ,MultiSuitReduce = @MultiSuitReduce
                                  ,PrintBeforeTwoHours =@PrintBeforeTwoHours
                             WHERE Id = @Id";
                dbOperator.AddParameter("@Owner", team.Owner);
                dbOperator.AddParameter("@Airline", team.Airline);
                dbOperator.AddParameter("@OfficeCode", team.OfficeCode);
                dbOperator.AddParameter("@CustomCode", team.CustomCode);
                dbOperator.AddParameter("@ImpowerOffice", team.ImpowerOffice);
                dbOperator.AddParameter("@IsInternal", team.IsInternal);
                dbOperator.AddParameter("@IsPeer", team.IsPeer);
                dbOperator.AddParameter("@VoyageType", team.VoyageType);
                dbOperator.AddParameter("@Departure", team.Departure);
                dbOperator.AddParameter("@Transit", team.Transit);
                dbOperator.AddParameter("@Arrival", team.Arrival);
                dbOperator.AddParameter("@DepartureDateStart", team.DepartureDateStart);
                dbOperator.AddParameter("@DepartureDateEnd", team.DepartureDateEnd);
                dbOperator.AddParameter("@DepartureFlightsFilter", team.DepartureFlightsFilter);
                dbOperator.AddParameter("@DepartureDateFilter", team.DepartureDateFilter);
                dbOperator.AddParameter("@DepartureWeekFilter", team.DepartureWeekFilter);
                dbOperator.AddParameter("@DepartureFlightsFilterType", team.DepartureFlightsFilterType);
                dbOperator.AddParameter("@ReturnFlightsFilter", string.IsNullOrWhiteSpace(team.ReturnFlightsFilter) ? "" : team.ReturnFlightsFilter);
                dbOperator.AddParameter("@ReturnFlightsFilterType", team.ReturnFlightsFilterType);
                dbOperator.AddParameter("@ExceptAirways", team.ExceptAirways);
                dbOperator.AddParameter("@DrawerCondition", team.DrawerCondition);
                dbOperator.AddParameter("@Remark", team.Remark);
                dbOperator.AddParameter("@AppointBerths", team.AppointBerths);
                dbOperator.AddParameter("@Berths", team.Berths);
                dbOperator.AddParameter("@InternalCommission", team.InternalCommission);
                dbOperator.AddParameter("@SubordinateCommission", team.SubordinateCommission);
                dbOperator.AddParameter("@ProfessionCommission", team.ProfessionCommission);
                dbOperator.AddParameter("@AutoAudit", team.AutoAudit);
                dbOperator.AddParameter("@ChangePNR", team.ChangePNR);
                dbOperator.AddParameter("@AutoPrint", team.AutoPrint);
                dbOperator.AddParameter("@SuitReduce", team.SuitReduce);
                dbOperator.AddParameter("@TicketType", team.TicketType);
                dbOperator.AddParameter("@StartPrintDate", team.StartPrintDate);
                dbOperator.AddParameter("@Suspended", team.Suspended);
                dbOperator.AddParameter("@Freezed", team.Freezed);
                dbOperator.AddParameter("@Audited", team.Audited);
                dbOperator.AddParameter("@CreateTime", team.CreateTime);
                dbOperator.AddParameter("@LastModifyTime", team.LastModifyTime);
                dbOperator.AddParameter("@PrintBeforeTwoHours", team.PrintBeforeTwoHours);
                if (team.AuditTime.HasValue)
                {
                    dbOperator.AddParameter("@AuditTime", team.AuditTime);
                }
                else
                {
                    dbOperator.AddParameter("@AuditTime", DBNull.Value);
                }
                dbOperator.AddParameter("@Creator", team.Creator);
                dbOperator.AddParameter("@MultiSuitReduce", team.MultiSuitReduce);
                dbOperator.AddParameter("@Id", team.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateSpecialPolicy(Guid id, int num)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"UPDATE  dbo.T_SpecialPolicy 
                           SET ResourceAmount = ResourceAmount - @ResourceAmount
                         WHERE Id = @Id AND ResourceAmount - @num >= 0 ";
                dbOperator.AddParameter("@Id", id);
                dbOperator.AddParameter("@num", num);
                dbOperator.AddParameter("@ResourceAmount", num);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void UpdateNormalPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                int i = 0;
                string sql = "";
                foreach (var item in delDeparture)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_NormalPolicyDeparture WHERE [PolicyId] = @plid" + i + " AND Code=@delDeparture" + i + ";";
                    dbOperator.AddParameter("plid" + i, policyId);
                    dbOperator.AddParameter("delDeparture" + i, item);
                }
                foreach (var item in addDeparture)
                {
                    i++;
                    sql += "INSERT INTO [dbo].[T_NormalPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                    dbOperator.AddParameter("Code" + i, item);
                    dbOperator.AddParameter("PolicyId" + i, policyId);
                }
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    dbOperator.ExecuteNonQuery(sql);
                }
                ////foreach (var policy in policies)
                ////{
                //sql = "DELETE FROM dbo.T_NormalPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                //dbOperator.AddParameter("plid" + i, policy.Id);
                ////policy.Departure.Split('/').ToList().Except( new List<string>());
                //foreach (var item in policy.Departure.Split('/'))
                //{
                //    i++;
                //    sql += "INSERT INTO [dbo].[T_NormalPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                //    dbOperator.AddParameter("Code" + i, item);
                //    dbOperator.AddParameter("PolicyId" + i, policy.Id);
                //}
            }
        }
        public void UpdateBargainPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                int i = 0;
                string sql = "";

                foreach (var item in delDeparture)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_BargainPolicyDeparture WHERE [PolicyId] = @plid" + i + " AND Code=@delDeparture" + i + ";";
                    dbOperator.AddParameter("plid" + i, policyId);
                    dbOperator.AddParameter("delDeparture" + i, item);
                }
                foreach (var item in addDeparture)
                {
                    i++;
                    sql += "INSERT INTO [dbo].[T_BargainPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                    dbOperator.AddParameter("Code" + i, item);
                    dbOperator.AddParameter("PolicyId" + i, policyId);
                }
                ////foreach (var policy in policies)
                ////{
                //sql += "DELETE FROM dbo.T_BargainPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                //dbOperator.AddParameter("plid" + i, policy.Id);
                //foreach (var item in policy.Departure.Split('/'))
                //{
                //    i++;
                //    sql += "INSERT INTO [dbo].[T_BargainPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                //    dbOperator.AddParameter("Code" + i, item);
                //    dbOperator.AddParameter("PolicyId" + i, policy.Id);
                //}
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    dbOperator.ExecuteNonQuery(sql);
                    sql = "";
                }
                //}
            }
        }
        public void UpdateSpecialPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                int i = 0;
                string sql = "";

                foreach (var item in delDeparture)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_SpecialPolicyDeparture WHERE [PolicyId] = @plid" + i + " AND Code=@delDeparture" + i + ";";
                    dbOperator.AddParameter("plid" + i, policyId);
                    dbOperator.AddParameter("delDeparture" + i, item);
                }
                foreach (var item in addDeparture)
                {
                    i++;
                    sql += "INSERT INTO [dbo].[T_SpecialPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                    dbOperator.AddParameter("Code" + i, item);
                    dbOperator.AddParameter("PolicyId" + i, policyId);
                }
                //foreach (var policy in policies)
                //{
                //sql += "DELETE FROM dbo.T_SpecialPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                //dbOperator.AddParameter("plid" + i, policy.Id);
                //foreach (var item in policy.Departure.Split('/'))
                //{
                //    i++;
                //    sql += "INSERT INTO [dbo].[T_SpecialPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                //    dbOperator.AddParameter("Code" + i, item);
                //    dbOperator.AddParameter("PolicyId" + i, policy.Id);
                //}
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    dbOperator.ExecuteNonQuery(sql);
                    sql = "";
                }
                //}
            }
        }
        public void UpdateTeamPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                int i = 0;
                string sql = "";

                foreach (var item in delDeparture)
                {
                    i++;
                    sql += "DELETE FROM dbo.T_TeamPolicyDeparture WHERE [PolicyId] = @plid" + i + " AND Code=@delDeparture" + i + ";";
                    dbOperator.AddParameter("plid" + i, policyId);
                    dbOperator.AddParameter("delDeparture" + i, item);
                }
                foreach (var item in addDeparture)
                {
                    i++;
                    sql += "INSERT INTO [dbo].[T_TeamPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                    dbOperator.AddParameter("Code" + i, item);
                    dbOperator.AddParameter("PolicyId" + i, policyId);
                }
                //foreach (var policy in policies)
                //{
                //sql += "DELETE FROM dbo.T_TeamPolicyDeparture WHERE [PolicyId] = @plid" + i + ";";
                //dbOperator.AddParameter("plid" + i, policy.Id);
                //foreach (var item in policy.Departure.Split('/'))
                //{
                //    i++;
                //    sql += "INSERT INTO [dbo].[T_TeamPolicyDeparture] ([Code] ,[PolicyId]) VALUES (@Code" + i + ",@PolicyId" + i + ");";
                //    dbOperator.AddParameter("Code" + i, item);
                //    dbOperator.AddParameter("PolicyId" + i, policy.Id);
                //}
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    dbOperator.ExecuteNonQuery(sql);
                    sql = "";
                }
                //}
            }
        }
        #endregion

        #region 添加
        public void InsertNormalPolicy(System.Collections.Generic.IEnumerable<NormalPolicy> normalPloicy)
        {
            int rowCount = 0;

            #region 给普通政策表中添加数据
            var dataTableNormal = GetTableNormalPolicy();
            var dataTableNormalDeparture = GetTablePolicyDeparture();

            foreach (var item in normalPloicy)
            {
                DataRow dataRow = dataTableNormal.NewRow();
                dataRow[0] = item.Id;
                dataRow[1] = item.Owner;
                dataRow[2] = item.Airline;
                dataRow[3] = item.OfficeCode;
                dataRow[4] = item.CustomCode;
                dataRow[5] = item.ImpowerOffice;
                dataRow[6] = item.IsInternal;
                dataRow[7] = item.IsPeer;
                dataRow[8] = (byte)item.VoyageType;
                dataRow[9] = item.Departure;
                dataRow[10] = item.Transit;
                dataRow[11] = item.Arrival; ;
                dataRow[12] = item.DepartureDateStart;
                dataRow[13] = item.DepartureDateEnd;
                dataRow[14] = item.DepartureFlightsFilterType == LimitType.None ? "" : item.DepartureFlightsFilter;
                dataRow[15] = item.DepartureDateFilter;
                dataRow[16] = item.DepartureWeekFilter;
                dataRow[17] = (byte)item.DepartureFlightsFilterType;
                dataRow[18] = item.ReturnFlightsFilterType == LimitType.None ? "" : item.ReturnFlightsFilter;
                dataRow[19] = (byte)item.ReturnFlightsFilterType;
                dataRow[20] = item.ExceptAirways.ToUpper();
                dataRow[21] = item.DrawerCondition;
                dataRow[22] = item.Remark;
                dataRow[23] = item.Berths;
                dataRow[24] = item.InternalCommission;
                dataRow[25] = item.SubordinateCommission;
                dataRow[26] = item.ProfessionCommission;
                dataRow[27] = item.AutoAudit;
                dataRow[28] = item.ChangePNR;
                dataRow[29] = item.AutoPrint;
                dataRow[30] = item.SuitReduce;
                dataRow[31] = (byte)item.TicketType;
                dataRow[32] = item.StartPrintDate;
                dataRow[33] = item.Suspended;
                dataRow[34] = item.Freezed;
                dataRow[35] = item.Audited;
                dataRow[36] = item.CreateTime;
                if (item.AuditTime.HasValue)
                {
                    dataRow[37] = item.AuditTime;
                }
                else
                {
                    dataRow[37] = DBNull.Value;
                }
                dataRow[38] = item.Creator;
                dataRow[39] = item.MultiSuitReduce;
                dataRow[40] = item.LastModifyTime;
                dataRow[41] = item.AbbreviateName;
                dataRow[42] = item.PrintBeforeTwoHours;
                dataTableNormal.Rows.Add(dataRow);
                foreach (var d in item.Departure.Split('/'))
                {
                    DataRow dataRowd = dataTableNormalDeparture.NewRow();
                    dataRowd[0] = d;
                    dataRowd[1] = item.Id;
                    dataTableNormalDeparture.Rows.Add(dataRowd);
                }
            }
            #endregion

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlTransaction st = sqlConnection.BeginTransaction();
                try
                {
                    using (SqlBulkCopy sqlBC1 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC1.DestinationTableName = "dbo.T_NormalPolicy";
                        sqlBC1.BatchSize = dataTableNormal.Rows.Count;
                        if (sqlBC1.BatchSize != 0)
                        {
                            sqlBC1.WriteToServer(dataTableNormal);
                            rowCount += sqlBC1.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC2 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC2.DestinationTableName = "dbo.T_NormalPolicyDeparture";
                        sqlBC2.BatchSize = dataTableNormalDeparture.Rows.Count;
                        if (sqlBC2.BatchSize != 0)
                        {
                            sqlBC2.WriteToServer(dataTableNormalDeparture);
                            rowCount += sqlBC2.BatchSize;
                        }
                    }
                    st.Commit();
                }
                catch (Exception)
                {
                    st.Rollback();
                    throw;
                }
            }
            #region
            //            return rowCount;
            //            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            //            {
            //                int i = 0;
            //                var sql = "";
            //                foreach (var normal in normalPloicy)
            //                {
            //                    i++;
            //                    sql += @"INSERT INTO dbo.T_NormalPolicy
            //(Id,Owner,Airline,OfficeCode,CustomCode,ImpowerOffice,IsInternal,IsPeer,VoyageType,Departure,Transit,Arrival,DepartureDateStart,DepartureDateEnd,DepartureFlightsFilter,DepartureDateFilter,DepartureWeekFilter,DepartureFlightsFilterType,ReturnFlightsFilter,ReturnFlightsFilterType,ExceptAirways,DrawerCondition,Remark,Berths,InternalCommission,SubordinateCommission,ProfessionCommission,AutoAudit,ChangePNR,AutoPrint,SuitReduce,TicketType,StartPrintDate,Suspended,Freezed,Audited,CreateTime,AuditTime,Creator,MultiSuitReduce,AbbreviateName,PrintBeforeTwoHours)
            //VALUES(@Id" + i + ",@Owner" + i + ",@Airline" + i + ",@OfficeCode" + i + ",@CustomCode" + i + ",@ImpowerOffice" + i + ",@IsInternal" + i + ",@IsPeer" + i + ",@VoyageType" + i + ",@Departure" + i + ",@Transit" + i + ",@Arrival" + i + ",@DepartureDateStart" + i + ",@DepartureDateEnd" + i + ",@DepartureFlightsFilter" + i + ",@DepartureDateFilter" + i + ",@DepartureWeekFilter" + i + ",@DepartureFlightsFilterType" + i + ",@ReturnFlightsFilter" + i + ",@ReturnFlightsFilterType" + i + ",@ExceptAirways" + i + ",@DrawerCondition" + i + ",@Remark"
            //    + i + ",@Berths" + i + ",@InternalCommission" + i + ",@SubordinateCommission" + i + ",@ProfessionCommission" + i + ",@AutoAudit" + i + ",@ChangePNR"
            //    + i + ",@AutoPrint" + i + ",@SuitReduce" + i + ",@TicketType" + i + ",@StartPrintDate" + i + ",@Suspended" + i + ",@Freezed" + i + ",@Audited"
            //    + i + ",@CreateTime" + i + ",@AuditTime" + i + ",@Creator" + i + ",@MultiSuitReduce" + i + ",@AbbreviateName"
            //    + i + ",@PrintBeforeTwoHours" + i + ");";
            //                    dbOperator.AddParameter("@Owner" + i, normal.Owner);
            //                    dbOperator.AddParameter("@Airline" + i, normal.Airline);
            //                    dbOperator.AddParameter("@OfficeCode" + i, normal.OfficeCode);
            //                    dbOperator.AddParameter("@CustomCode" + i, normal.CustomCode);
            //                    dbOperator.AddParameter("@ImpowerOffice" + i, normal.ImpowerOffice);
            //                    dbOperator.AddParameter("@IsInternal" + i, normal.IsInternal);
            //                    dbOperator.AddParameter("@IsPeer" + i, normal.IsPeer);
            //                    dbOperator.AddParameter("@VoyageType" + i, normal.VoyageType);
            //                    dbOperator.AddParameter("@Departure" + i, normal.Departure);
            //                    dbOperator.AddParameter("@Transit" + i, normal.Transit);
            //                    dbOperator.AddParameter("@Arrival" + i, normal.Arrival);
            //                    dbOperator.AddParameter("@AbbreviateName" + i, normal.AbbreviateName);
            //                    dbOperator.AddParameter("@DepartureDateStart" + i, normal.DepartureDateStart);
            //                    dbOperator.AddParameter("@DepartureDateEnd" + i, normal.DepartureDateEnd);
            //                    dbOperator.AddParameter("@DepartureFlightsFilter" + i, normal.DepartureFlightsFilter);
            //                    dbOperator.AddParameter("@DepartureDateFilter" + i, normal.DepartureDateFilter);
            //                    dbOperator.AddParameter("@DepartureWeekFilter" + i, normal.DepartureWeekFilter);
            //                    dbOperator.AddParameter("@DepartureFlightsFilterType" + i, normal.DepartureFlightsFilterType);
            //                    dbOperator.AddParameter("@ReturnFlightsFilter" + i, string.IsNullOrWhiteSpace(normal.ReturnFlightsFilter) ? "" : normal.ReturnFlightsFilter);
            //                    dbOperator.AddParameter("@ReturnFlightsFilterType" + i, normal.ReturnFlightsFilterType);
            //                    dbOperator.AddParameter("@ExceptAirways" + i, normal.ExceptAirways);
            //                    dbOperator.AddParameter("@DrawerCondition" + i, normal.DrawerCondition);
            //                    dbOperator.AddParameter("@Remark" + i, normal.Remark);
            //                    dbOperator.AddParameter("@Berths" + i, normal.Berths);
            //                    dbOperator.AddParameter("@InternalCommission" + i, normal.InternalCommission);
            //                    dbOperator.AddParameter("@SubordinateCommission" + i, normal.SubordinateCommission);
            //                    dbOperator.AddParameter("@ProfessionCommission" + i, normal.ProfessionCommission);
            //                    dbOperator.AddParameter("@AutoAudit" + i, normal.AutoAudit);
            //                    dbOperator.AddParameter("@ChangePNR" + i, normal.ChangePNR);
            //                    dbOperator.AddParameter("@AutoPrint" + i, normal.AutoPrint);
            //                    dbOperator.AddParameter("@SuitReduce" + i, normal.SuitReduce);
            //                    dbOperator.AddParameter("@TicketType" + i, normal.TicketType);
            //                    dbOperator.AddParameter("@StartPrintDate" + i, normal.StartPrintDate);
            //                    dbOperator.AddParameter("@Suspended" + i, normal.Suspended);
            //                    dbOperator.AddParameter("@Freezed" + i, normal.Freezed);
            //                    dbOperator.AddParameter("@Audited" + i, normal.Audited);
            //                    dbOperator.AddParameter("@CreateTime" + i, normal.CreateTime);
            //                    dbOperator.AddParameter("@PrintBeforeTwoHours" + i, normal.PrintBeforeTwoHours);
            //                    if (normal.AuditTime.HasValue)
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, normal.AuditTime);
            //                    }
            //                    else
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, DBNull.Value);
            //                    }
            //                    dbOperator.AddParameter("@Creator" + i, normal.Creator);
            //                    dbOperator.AddParameter("@MultiSuitReduce" + i, normal.MultiSuitReduce);
            //                    dbOperator.AddParameter("@Id" + i, normal.Id);

            //                    if (!string.IsNullOrWhiteSpace(sql))
            //                    {
            //                        dbOperator.ExecuteNonQuery(sql);
            //                        sql = "";
            //                    }
            //                }
            //                //if (!string.IsNullOrWhiteSpace(sql))
            //                //{
            //                //    ExecuteNonQuery(sql);
            //                //}
            //            }
            #endregion
        }

        public void InsertBargainPolicy(System.Collections.Generic.IEnumerable<BargainPolicy> bargainPolicy)
        {
            int rowCount = 0;

            #region 给政策表中添加数据
            var dataTable = GetTableBargainPolicy();
            var dataTableDeparture = GetTablePolicyDeparture();

            foreach (var item in bargainPolicy)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = item.Id;
                dataRow[1] = item.Owner;
                dataRow[2] = item.Airline;
                dataRow[3] = item.OfficeCode;
                dataRow[4] = item.CustomCode;
                dataRow[5] = item.ImpowerOffice;
                dataRow[6] = item.IsInternal;
                dataRow[7] = item.IsPeer;
                dataRow[8] = (byte)item.VoyageType;
                dataRow[9] = item.Departure;
                dataRow[10] = item.Transit;
                dataRow[11] = item.Arrival;
                dataRow[12] = item.ExceptAirways.ToUpper();
                dataRow[13] = item.DepartureDateStart;
                dataRow[14] = item.DepartureDateEnd;
                dataRow[15] = item.DepartureFlightsFilterType == LimitType.None ? "" : item.DepartureFlightsFilter.ToUpper();
                dataRow[16] = item.DepartureDateFilter;
                dataRow[17] = item.DepartureWeekFilter;
                dataRow[18] = (byte)item.DepartureFlightsFilterType;
                dataRow[19] = item.ReturnFlightsFilterType == LimitType.None ? "" : item.ReturnFlightsFilter.ToUpper();
                dataRow[20] = (byte)item.ReturnFlightsFilterType;
                dataRow[21] = item.BeforehandDays;
                dataRow[22] = item.TravelDays;
                dataRow[23] = item.InvalidRegulation;
                dataRow[24] = item.ChangeRegulation;
                dataRow[25] = item.EndorseRegulation;
                dataRow[26] = item.RefundRegulation;
                dataRow[27] = item.DrawerCondition;
                dataRow[28] = item.Remark;
                dataRow[29] = item.Berths;
                dataRow[30] = item.InternalCommission;
                dataRow[31] = item.SubordinateCommission;
                dataRow[32] = item.ProfessionCommission;
                dataRow[33] = item.Price;
                dataRow[34] = (byte)item.PriceType;
                dataRow[35] = item.AutoAudit;
                dataRow[36] = item.ChangePNR;
                dataRow[37] = (byte)item.TicketType;
                dataRow[38] = item.StartPrintDate;
                dataRow[39] = item.Suspended;
                dataRow[40] = item.Freezed;
                dataRow[41] = item.Audited;
                dataRow[42] = item.CreateTime;
                if (item.AuditTime.HasValue)
                {
                    dataRow[43] = item.AuditTime;
                }
                else
                {
                    dataRow[43] = DBNull.Value;
                }
                dataRow[44] = item.Creator;
                dataRow[45] = item.MultiSuitReduce;
                dataRow[46] = item.MostBeforehandDays;
                dataRow[47] = item.LastModifyTime;
                dataRow[48] = item.AbbreviateName;
                dataRow[49] = item.PrintBeforeTwoHours;

                dataTable.Rows.Add(dataRow);
                foreach (var d in item.Departure.Split('/'))
                {
                    DataRow dataRowd = dataTableDeparture.NewRow();
                    dataRowd[0] = d;
                    dataRowd[1] = item.Id;
                    dataTableDeparture.Rows.Add(dataRowd);
                }
            }
            #endregion

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlTransaction st = sqlConnection.BeginTransaction();
                try
                {
                    using (SqlBulkCopy sqlBC1 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC1.DestinationTableName = "dbo.T_BargainPolicy";
                        sqlBC1.BatchSize = dataTable.Rows.Count;
                        if (sqlBC1.BatchSize != 0)
                        {
                            sqlBC1.WriteToServer(dataTable);
                            rowCount += sqlBC1.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC2 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC2.DestinationTableName = "dbo.T_BargainPolicyDeparture";
                        sqlBC2.BatchSize = dataTableDeparture.Rows.Count;
                        if (sqlBC2.BatchSize != 0)
                        {
                            sqlBC2.WriteToServer(dataTableDeparture);
                            rowCount += sqlBC2.BatchSize;
                        }
                    }
                    st.Commit();
                }
                catch (Exception)
                {
                    st.Rollback();
                    throw;
                }
            }
            #region
            //            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            //            {
            //                int i = 0;
            //                var sql = "";
            //                foreach (var bargain in bargainPolicy)
            //                {
            //                    i++;
            //                    sql += @"INSERT INTO dbo.T_BargainPolicy
            //(Id,Owner,Airline,OfficeCode,CustomCode,ImpowerOffice,IsInternal,IsPeer,VoyageType,Departure,Transit,Arrival,ExceptAirways,DepartureDateStart,DepartureDateEnd,DepartureFlightsFilter,DepartureDateFilter,DepartureWeekFilter,DepartureFlightsFilterType,ReturnFlightsFilter,ReturnFlightsFilterType,BeforehandDays,TravelDays,InvalidRegulation,ChangeRegulation,EndorseRegulation,RefundRegulation,DrawerCondition,Remark,Berths,InternalCommission,SubordinateCommission,ProfessionCommission,Price,PriceType,AutoAudit,ChangePNR,TicketType,StartPrintDate,Suspended,Freezed,Audited,CreateTime,AuditTime,Creator,MultiSuitReduce,MostBeforehandDays,AbbreviateName,PrintBeforeTwoHours)
            //VALUES(@Id" + i + ",@Owner" + i + ",@Airline" + i + ",@OfficeCode" + i + ",@CustomCode" + i + ",@ImpowerOffice" + i + ",@IsInternal" + i
            //                + ",@IsPeer" + i + ",@VoyageType" + i + ",@Departure" + i + ",@Transit" + i + ",@Arrival" + i
            //                + ",@ExceptAirways" + i + ",@DepartureDateStart" + i + ",@DepartureDateEnd" + i + ",@DepartureFlightsFilter" + i
            //                + ",@DepartureDateFilter" + i + ",@DepartureWeekFilter" + i + ",@DepartureFlightsFilterType" + i + ",@ReturnFlightsFilter" + i
            //                + ",@ReturnFlightsFilterType" + i + ",@BeforehandDays" + i + ",@TravelDays" + i + ",@InvalidRegulation" + i
            //                + ",@ChangeRegulation" + i + ",@EndorseRegulation" + i + ",@RefundRegulation" + i + ",@DrawerCondition" + i
            //                + ",@Remark" + i + ",@Berths" + i + ",@InternalCommission" + i + ",@SubordinateCommission" + i
            //                + ",@ProfessionCommission" + i + ",@Price" + i + ",@PriceType" + i + ",@AutoAudit" + i + ",@ChangePNR" + i
            //                + ",@TicketType" + i + ",@StartPrintDate" + i + ",@Suspended" + i + ",@Freezed" + i + ",@Audited" + i
            //                + ",@CreateTime" + i + ",@AuditTime" + i + ",@Creator" + i + ",@MultiSuitReduce" + i + ",@MostBeforehandDays" + i + ",@AbbreviateName" + i
            //                + ",@PrintBeforeTwoHours" + i + ");";

            //                    dbOperator.AddParameter("@Id" + i, bargain.Id);
            //                    dbOperator.AddParameter("@Owner" + i, bargain.Owner);
            //                    dbOperator.AddParameter("@Airline" + i, bargain.Airline);
            //                    dbOperator.AddParameter("@OfficeCode" + i, bargain.OfficeCode);
            //                    dbOperator.AddParameter("@CustomCode" + i, bargain.CustomCode);
            //                    dbOperator.AddParameter("@ImpowerOffice" + i, bargain.ImpowerOffice);
            //                    dbOperator.AddParameter("@AbbreviateName" + i, bargain.AbbreviateName);
            //                    dbOperator.AddParameter("@IsInternal" + i, bargain.IsInternal);
            //                    dbOperator.AddParameter("@IsPeer" + i, bargain.IsPeer);
            //                    dbOperator.AddParameter("@VoyageType" + i, bargain.VoyageType);
            //                    dbOperator.AddParameter("@Departure" + i, bargain.Departure);
            //                    dbOperator.AddParameter("@Transit" + i, bargain.Transit);
            //                    dbOperator.AddParameter("@Arrival" + i, bargain.Arrival);
            //                    dbOperator.AddParameter("@ExceptAirways" + i, bargain.ExceptAirways);
            //                    dbOperator.AddParameter("@DepartureDateStart" + i, bargain.DepartureDateStart);
            //                    dbOperator.AddParameter("@DepartureDateEnd" + i, bargain.DepartureDateEnd);
            //                    dbOperator.AddParameter("@DepartureFlightsFilter" + i, bargain.DepartureFlightsFilter);
            //                    dbOperator.AddParameter("@DepartureDateFilter" + i, bargain.DepartureDateFilter);
            //                    dbOperator.AddParameter("@DepartureWeekFilter" + i, bargain.DepartureWeekFilter);
            //                    dbOperator.AddParameter("@DepartureFlightsFilterType" + i, bargain.DepartureFlightsFilterType);
            //                    dbOperator.AddParameter("@ReturnFlightsFilter" + i, string.IsNullOrWhiteSpace(bargain.ReturnFlightsFilter) ? "" : bargain.ReturnFlightsFilter);
            //                    dbOperator.AddParameter("@ReturnFlightsFilterType" + i, bargain.ReturnFlightsFilterType);
            //                    dbOperator.AddParameter("@BeforehandDays" + i, bargain.BeforehandDays);
            //                    dbOperator.AddParameter("@TravelDays" + i, bargain.TravelDays);
            //                    dbOperator.AddParameter("@InvalidRegulation" + i, bargain.InvalidRegulation);
            //                    dbOperator.AddParameter("@ChangeRegulation" + i, bargain.ChangeRegulation);
            //                    dbOperator.AddParameter("@EndorseRegulation" + i, bargain.EndorseRegulation);
            //                    dbOperator.AddParameter("@RefundRegulation" + i, bargain.RefundRegulation);
            //                    dbOperator.AddParameter("@DrawerCondition" + i, bargain.DrawerCondition);
            //                    dbOperator.AddParameter("@Remark" + i, bargain.Remark);
            //                    dbOperator.AddParameter("@Berths" + i, bargain.Berths);
            //                    dbOperator.AddParameter("@InternalCommission" + i, bargain.InternalCommission);
            //                    dbOperator.AddParameter("@SubordinateCommission" + i, bargain.SubordinateCommission);
            //                    dbOperator.AddParameter("@ProfessionCommission" + i, bargain.ProfessionCommission);
            //                    dbOperator.AddParameter("@Price" + i, bargain.Price);
            //                    dbOperator.AddParameter("@PriceType" + i, bargain.PriceType);
            //                    dbOperator.AddParameter("@AutoAudit" + i, bargain.AutoAudit);
            //                    dbOperator.AddParameter("@ChangePNR" + i, bargain.ChangePNR);
            //                    dbOperator.AddParameter("@TicketType" + i, bargain.TicketType);
            //                    dbOperator.AddParameter("@StartPrintDate" + i, bargain.StartPrintDate);
            //                    dbOperator.AddParameter("@Suspended" + i, bargain.Suspended);
            //                    dbOperator.AddParameter("@Freezed" + i, bargain.Freezed);
            //                    dbOperator.AddParameter("@Audited" + i, bargain.Audited);
            //                    dbOperator.AddParameter("@CreateTime" + i, bargain.CreateTime);
            //                    dbOperator.AddParameter("@Creator" + i, bargain.Creator);
            //                    dbOperator.AddParameter("@MultiSuitReduce" + i, bargain.MultiSuitReduce);
            //                    dbOperator.AddParameter("@MostBeforehandDays" + i, bargain.MostBeforehandDays);
            //                    dbOperator.AddParameter("PrintBeforeTwoHours" + i, bargain.PrintBeforeTwoHours);
            //                    if (bargain.AutoAudit)
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, DateTime.Now);
            //                    }
            //                    else
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, DBNull.Value);
            //                    }

            //                    if (!string.IsNullOrWhiteSpace(sql))
            //                    {
            //                        dbOperator.ExecuteNonQuery(sql);
            //                        sql = "";
            //                    }
            //                }
            //            }
            #endregion
        }

        public void InsertSpecialPolicy(System.Collections.Generic.IEnumerable<SpecialPolicy> specialPolicy)
        {
            int rowCount = 0;

            #region 给政策表中添加数据
            var dataTable = GetTableSpecialPolicy();
            var dataTableDeparture = GetTablePolicyDeparture();

            foreach (var item in specialPolicy)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = item.Id;
                dataRow[1] = item.Owner;
                dataRow[2] = (byte)item.Type;
                dataRow[3] = item.OfficeCode;
                dataRow[4] = item.CustomCode;
                dataRow[5] = item.ImpowerOffice;
                dataRow[6] = item.IsPeer;
                dataRow[7] = item.IsInternal;
                dataRow[8] = item.Airline;
                dataRow[9] = (byte)item.VoyageType;
                dataRow[10] = item.Departure;
                dataRow[11] = item.Arrival;
                dataRow[12] = item.ExceptAirways;
                dataRow[13] = item.DepartureDateStart;
                dataRow[14] = item.DepartureDateEnd;
                dataRow[15] = item.DepartureDateFilter;
                dataRow[16] = item.DepartureWeekFilter;
                dataRow[17] = item.DepartureFlightsFilterType == LimitType.None ? "" : item.DepartureFlightsFilter.ToUpper();
                dataRow[18] = (byte)item.DepartureFlightsFilterType;
                dataRow[19] = item.BeforehandDays;
                dataRow[20] = item.InvalidRegulation;
                dataRow[21] = item.ChangeRegulation;
                dataRow[22] = item.EndorseRegulation;
                dataRow[23] = item.RefundRegulation;
                dataRow[24] = item.DrawerCondition;
                dataRow[25] = item.Remark;
                dataRow[26] = item.Price;
                dataRow[27] = item.ProvideDate;
                dataRow[28] = item.ResourceAmount;
                dataRow[29] = item.AutoAudit;
                dataRow[30] = item.ConfirmResource;
                dataRow[31] = item.Suspended;
                dataRow[32] = item.Freezed;
                dataRow[33] = item.Audited;
                dataRow[34] = item.PlatformAudited;
                dataRow[35] = item.SynBlackScreen;
                dataRow[36] = item.Berths;
                dataRow[37] = (byte)item.TicketType;
                dataRow[38] = (byte)item.PriceType;
                dataRow[39] = item.InternalCommission;
                dataRow[40] = item.SubordinateCommission;
                dataRow[41] = item.ProfessionCommission;
                dataRow[42] = item.CreateTime;
                if (item.AuditTime.HasValue)
                {
                    dataRow[43] = item.AuditTime;
                }
                else
                {
                    dataRow[43] = DBNull.Value;
                }
                dataRow[44] = item.Creator;
                dataRow[45] = item.IsBargainBerths;
                dataRow[46] = item.IsSeat;
                dataRow[47] = item.LastModifyTime;
                dataRow[48] = item.AbbreviateName;
                dataRow[49] = item.PrintBeforeTwoHours;
                dataRow[50] = (byte)item.LowNoType;
                dataRow[51] = item.LowNoMaxPrice;
                dataRow[52] = item.LowNoMinPrice;
                dataTable.Rows.Add(dataRow);
                foreach (var d in item.Departure.Split('/'))
                {
                    DataRow dataRowd = dataTableDeparture.NewRow();
                    dataRowd[0] = d;
                    dataRowd[1] = item.Id;
                    dataTableDeparture.Rows.Add(dataRowd);
                }
            }
            #endregion

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlTransaction st = sqlConnection.BeginTransaction();
                try
                {
                    using (SqlBulkCopy sqlBC1 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC1.DestinationTableName = "dbo.T_SpecialPolicy";
                        sqlBC1.BatchSize = dataTable.Rows.Count;
                        if (sqlBC1.BatchSize != 0)
                        {
                            sqlBC1.WriteToServer(dataTable);
                            rowCount += sqlBC1.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC2 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC2.DestinationTableName = "dbo.T_SpecialPolicyDeparture";
                        sqlBC2.BatchSize = dataTableDeparture.Rows.Count;
                        if (sqlBC2.BatchSize != 0)
                        {
                            sqlBC2.WriteToServer(dataTableDeparture);
                            rowCount += sqlBC2.BatchSize;
                        }
                    }
                    st.Commit();
                }
                catch (Exception)
                {
                    st.Rollback();
                    throw;
                }
            }
            #region
            //            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            //            {
            //                int i = 0;
            //                var sql = "";
            //                foreach (var special in specialPolicy)
            //                {
            //                    i++;
            //                    sql += @"INSERT INTO dbo.T_SpecialPolicy
            //(Id,Owner,Type,OfficeCode,CustomCode,ImpowerOffice,IsPeer,IsInternal,Airline,VoyageType,Departure,Arrival,ExceptAirways,DepartureDateStart,DepartureDateEnd,DepartureDateFilter,DepartureWeekFilter,DepartureFlightsFilter,DepartureFlightsFilterType,BeforehandDays,InvalidRegulation,ChangeRegulation,EndorseRegulation,RefundRegulation,DrawerCondition,Remark,Price,ProvideDate,ResourceAmount,AutoAudit,ConfirmResource,Suspended,Freezed,Audited,PlatformAudited,SynBlackScreen,Berths,TicketType,PriceType,InternalCommission,SubordinateCommission,ProfessionCommission,CreateTime,AuditTime,Creator,IsBargainBerths,IsSeat,AbbreviateName,PrintBeforeTwoHours,LowNoType,LowNoMaxPrice,LowNoMinPrice)
            //VALUES(@Id" + i + ",@Owner" + i + ",@Type" + i + ",@OfficeCode" + i + ",@CustomCode" + i + ",@ImpowerOffice" + i + ",@IsPeer" + i
            //                + ",@IsInternal" + i + ",@Airline" + i + ",@VoyageType" + i + ",@Departure" + i + ",@Arrival" + i + ",@ExceptAirways" + i
            //                + ",@DepartureDateStart" + i + ",@DepartureDateEnd" + i + ",@DepartureDateFilter" + i + ",@DepartureWeekFilter" + i
            //                + ",@DepartureFlightsFilter" + i + ",@DepartureFlightsFilterType" + i + ",@BeforehandDays" + i + ",@InvalidRegulation" + i
            //                + ",@ChangeRegulation" + i + ",@EndorseRegulation" + i + ",@RefundRegulation" + i + ",@DrawerCondition" + i + ",@Remark" + i
            //                + ",@Price" + i + ",@ProvideDate" + i + ",@ResourceAmount" + i + ",@AutoAudit" + i + ",@ConfirmResource" + i + ",@Suspended" + i
            //                + ",@Freezed" + i + ",@Audited" + i + ",@PlatformAudited" + i + ",@SynBlackScreen" + i + ",@Berths" + i + ",@TicketType" + i
            //                + ",@PriceType" + i + ",@InternalCommission" + i + ",@SubordinateCommission" + i + ",@ProfessionCommission" + i + ",@CreateTime" + i
            //                + ",@AuditTime" + i + ",@Creator" + i + ",@IsBargainBerths" + i + ",@IsSeat" + i + ",@AbbreviateName" + i + ",@PrintBeforeTwoHours" + i + ",@LowNoType" + i + ",@LowNoMaxPrice" + i + ",@LowNoMinPrice" + i + ");";
            //                    dbOperator.AddParameter("@Id" + i, special.Id);
            //                    dbOperator.AddParameter("@Owner" + i, special.Owner);
            //                    dbOperator.AddParameter("@Type" + i, special.Type);
            //                    dbOperator.AddParameter("@OfficeCode" + i, special.OfficeCode);
            //                    dbOperator.AddParameter("@CustomCode" + i, special.CustomCode);
            //                    dbOperator.AddParameter("@ImpowerOffice" + i, special.ImpowerOffice);
            //                    dbOperator.AddParameter("@AbbreviateName" + i, special.AbbreviateName);
            //                    dbOperator.AddParameter("@IsPeer" + i, special.IsPeer);
            //                    dbOperator.AddParameter("@IsInternal" + i, special.IsInternal);
            //                    dbOperator.AddParameter("@Airline" + i, special.Airline);
            //                    dbOperator.AddParameter("@VoyageType" + i, special.VoyageType);
            //                    dbOperator.AddParameter("@Departure" + i, special.Departure);
            //                    dbOperator.AddParameter("@Arrival" + i, special.Arrival);
            //                    dbOperator.AddParameter("@ExceptAirways" + i, special.ExceptAirways);
            //                    dbOperator.AddParameter("@DepartureDateStart" + i, special.DepartureDateStart);
            //                    dbOperator.AddParameter("@DepartureDateEnd" + i, special.DepartureDateEnd);
            //                    dbOperator.AddParameter("@DepartureDateFilter" + i, special.DepartureDateFilter);
            //                    dbOperator.AddParameter("@DepartureWeekFilter" + i, special.DepartureWeekFilter);
            //                    dbOperator.AddParameter("@DepartureFlightsFilter" + i, special.DepartureFlightsFilter);
            //                    dbOperator.AddParameter("@DepartureFlightsFilterType" + i, special.DepartureFlightsFilterType);
            //                    dbOperator.AddParameter("@BeforehandDays" + i, special.BeforehandDays);
            //                    dbOperator.AddParameter("@InvalidRegulation" + i, special.InvalidRegulation);
            //                    dbOperator.AddParameter("@ChangeRegulation" + i, special.ChangeRegulation);
            //                    dbOperator.AddParameter("@EndorseRegulation" + i, special.EndorseRegulation);
            //                    dbOperator.AddParameter("@RefundRegulation" + i, special.RefundRegulation);
            //                    dbOperator.AddParameter("@DrawerCondition" + i, special.DrawerCondition);
            //                    dbOperator.AddParameter("@Remark" + i, special.Remark);
            //                    dbOperator.AddParameter("@Price" + i, special.Price);
            //                    dbOperator.AddParameter("@ProvideDate" + i, special.ProvideDate);
            //                    dbOperator.AddParameter("@ResourceAmount" + i, special.ResourceAmount);
            //                    dbOperator.AddParameter("@AutoAudit" + i, special.AutoAudit);
            //                    dbOperator.AddParameter("@ConfirmResource" + i, special.ConfirmResource);
            //                    dbOperator.AddParameter("@Suspended" + i, special.Suspended);
            //                    dbOperator.AddParameter("@Freezed" + i, special.Freezed);
            //                    dbOperator.AddParameter("@Audited" + i, special.Audited);
            //                    dbOperator.AddParameter("@PlatformAudited" + i, special.PlatformAudited);
            //                    dbOperator.AddParameter("@SynBlackScreen" + i, special.SynBlackScreen);
            //                    dbOperator.AddParameter("@Berths" + i, special.Berths);
            //                    dbOperator.AddParameter("@TicketType" + i, special.TicketType);
            //                    dbOperator.AddParameter("@PriceType" + i, special.PriceType);
            //                    dbOperator.AddParameter("@InternalCommission" + i, special.InternalCommission);
            //                    dbOperator.AddParameter("@SubordinateCommission" + i, special.SubordinateCommission);
            //                    dbOperator.AddParameter("@ProfessionCommission" + i, special.ProfessionCommission);
            //                    dbOperator.AddParameter("@CreateTime" + i, special.CreateTime);
            //                    dbOperator.AddParameter("@PrintBeforeTwoHours" + i, special.PrintBeforeTwoHours);
            //                    if (special.AuditTime.HasValue)
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, special.AuditTime);
            //                    }
            //                    else
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, DBNull.Value);
            //                    }
            //                    dbOperator.AddParameter("@Creator" + i, special.Creator);
            //                    dbOperator.AddParameter("@IsBargainBerths" + i, special.IsBargainBerths);
            //                    dbOperator.AddParameter("@IsSeat" + i, special.IsSeat);
            //                    dbOperator.AddParameter("@LowNoType" + i, special.LowNoType);
            //                    dbOperator.AddParameter("@LowNoMaxPrice" + i, special.LowNoMaxPrice);
            //                    dbOperator.AddParameter("@LowNoMinPrice" + i, special.LowNoMinPrice);

            //                    if (!string.IsNullOrWhiteSpace(sql))
            //                    {
            //                        dbOperator.ExecuteNonQuery(sql);
            //                        sql = "";
            //                    }
            //                }
            //            }
            #endregion
        }

        public void InsertTeamPolicy(System.Collections.Generic.IEnumerable<TeamPolicy> teamPolicy)
        {
            int rowCount = 0;

            #region 给政策表中添加数据
            var dataTable = GetTableTeamPolicy();
            var dataTableDeparture = GetTablePolicyDeparture();

            foreach (var item in teamPolicy)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = item.Id;
                dataRow[1] = item.Owner;
                dataRow[2] = item.Airline;
                dataRow[3] = item.OfficeCode;
                dataRow[4] = item.CustomCode;
                dataRow[5] = item.ImpowerOffice;
                dataRow[6] = item.IsInternal;
                dataRow[7] = item.IsPeer;
                dataRow[8] = (byte)item.VoyageType;
                dataRow[9] = item.Departure;
                dataRow[10] = item.Transit;
                dataRow[11] = item.Arrival;
                dataRow[12] = item.DepartureDateStart;
                dataRow[13] = item.DepartureDateEnd;
                dataRow[14] = item.DepartureFlightsFilter;
                dataRow[15] = item.DepartureDateFilter;
                dataRow[16] = item.DepartureWeekFilter;
                dataRow[17] = (byte)item.DepartureFlightsFilterType;
                dataRow[18] = item.ReturnFlightsFilter;
                dataRow[19] = (byte)item.ReturnFlightsFilterType;
                dataRow[20] = item.ExceptAirways;
                dataRow[21] = item.DrawerCondition;
                dataRow[22] = item.Remark;
                dataRow[23] = item.AppointBerths;
                dataRow[24] = item.Berths;
                dataRow[25] = item.InternalCommission;
                dataRow[26] = item.SubordinateCommission;
                dataRow[27] = item.ProfessionCommission;
                dataRow[28] = item.AutoAudit;
                dataRow[29] = item.ChangePNR;
                dataRow[30] = item.AutoPrint;
                dataRow[31] = item.SuitReduce;
                dataRow[32] = (byte)item.TicketType;
                dataRow[33] = item.StartPrintDate;
                dataRow[34] = item.Suspended;
                dataRow[35] = item.Freezed;
                dataRow[36] = item.Audited;
                dataRow[37] = item.CreateTime;
                if (item.AuditTime.HasValue)
                {
                    dataRow[38] = item.AuditTime;
                }
                else
                {
                    dataRow[38] = DBNull.Value;
                }
                dataRow[39] = item.Creator;
                dataRow[40] = item.MultiSuitReduce;
                dataRow[41] = item.LastModifyTime;
                dataRow[42] = item.AbbreviateName;
                dataRow[43] = item.PrintBeforeTwoHours;

                dataTable.Rows.Add(dataRow);
                foreach (var d in item.Departure.Split('/'))
                {
                    DataRow dataRowd = dataTableDeparture.NewRow();
                    dataRowd[0] = d;
                    dataRowd[1] = item.Id;
                    dataTableDeparture.Rows.Add(dataRowd);
                }
            }
            #endregion

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlTransaction st = sqlConnection.BeginTransaction();
                try
                {
                    using (SqlBulkCopy sqlBC1 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC1.DestinationTableName = "dbo.T_TeamPolicy";
                        sqlBC1.BatchSize = dataTable.Rows.Count;
                        if (sqlBC1.BatchSize != 0)
                        {
                            sqlBC1.WriteToServer(dataTable);
                            rowCount += sqlBC1.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC2 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC2.DestinationTableName = "dbo.T_TeamPolicyDeparture";
                        sqlBC2.BatchSize = dataTableDeparture.Rows.Count;
                        if (sqlBC2.BatchSize != 0)
                        {
                            sqlBC2.WriteToServer(dataTableDeparture);
                            rowCount += sqlBC2.BatchSize;
                        }
                    }
                    st.Commit();
                }
                catch (Exception)
                {
                    st.Rollback();
                    throw;
                }
            }
            #region
            //            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            //            {
            //                int i = 0;
            //                var sql = "";
            //                foreach (var team in teamPolicy)
            //                {
            //                    i++;
            //                    sql += @"INSERT INTO dbo.T_TeamPolicy
            //(Id,Owner,Airline,OfficeCode,CustomCode,ImpowerOffice,IsInternal,IsPeer,VoyageType,Departure,Transit,Arrival,DepartureDateStart,DepartureDateEnd,
            //DepartureFlightsFilter,DepartureDateFilter,DepartureWeekFilter,DepartureFlightsFilterType,ReturnFlightsFilter,ReturnFlightsFilterType,ExceptAirways,
            //DrawerCondition,Remark,AppointBerths,Berths,InternalCommission,SubordinateCommission,ProfessionCommission,AutoAudit,ChangePNR,AutoPrint,SuitReduce,
            //TicketType,StartPrintDate,Suspended,Freezed,Audited,CreateTime,AuditTime,Creator,MultiSuitReduce,AbbreviateName,PrintBeforeTwoHours)
            //VALUES(@Id" + i + ",@Owner" + i + ",@Airline" + i + ",@OfficeCode" + i + ",@CustomCode" + i + ",@ImpowerOffice" + i + ",@IsInternal" + i
            //                + ",@IsPeer" + i + ",@VoyageType" + i + ",@Departure" + i + ",@Transit" + i + ",@Arrival" + i + ",@DepartureDateStart" + i
            //                + ",@DepartureDateEnd" + i + ",@DepartureFlightsFilter" + i + ",@DepartureDateFilter" + i + ",@DepartureWeekFilter" + i
            //                + ",@DepartureFlightsFilterType" + i + ",@ReturnFlightsFilter" + i + ",@ReturnFlightsFilterType" + i + ",@ExceptAirways" + i
            //                + ",@DrawerCondition" + i + ",@Remark" + i + ",@AppointBerths" + i + ",@Berths" + i + ",@InternalCommission" + i
            //                + ",@SubordinateCommission" + i + ",@ProfessionCommission" + i + ",@AutoAudit" + i + ",@ChangePNR" + i + ",@AutoPrint" + i
            //                + ",@SuitReduce" + i + ",@TicketType" + i + ",@StartPrintDate" + i + ",@Suspended" + i + ",@Freezed" + i + ",@Audited" + i
            //                + ",@CreateTime" + i + ",@AuditTime" + i + ",@Creator" + i + ",@MultiSuitReduce" + i + ",@AbbreviateName" + i + ",@PrintBeforeTwoHours" + i + ")";
            //                    dbOperator.AddParameter("@Owner" + i, team.Owner);
            //                    dbOperator.AddParameter("@Airline" + i, team.Airline);
            //                    dbOperator.AddParameter("@OfficeCode" + i, team.OfficeCode);
            //                    dbOperator.AddParameter("@CustomCode" + i, team.CustomCode);
            //                    dbOperator.AddParameter("@ImpowerOffice" + i, team.ImpowerOffice);
            //                    dbOperator.AddParameter("@AbbreviateName" + i, team.AbbreviateName);
            //                    dbOperator.AddParameter("@IsInternal" + i, team.IsInternal);
            //                    dbOperator.AddParameter("@IsPeer" + i, team.IsPeer);
            //                    dbOperator.AddParameter("@VoyageType" + i, team.VoyageType);
            //                    dbOperator.AddParameter("@Departure" + i, team.Departure);
            //                    dbOperator.AddParameter("@Transit" + i, team.Transit);
            //                    dbOperator.AddParameter("@Arrival" + i, team.Arrival);
            //                    dbOperator.AddParameter("@DepartureDateStart" + i, team.DepartureDateStart);
            //                    dbOperator.AddParameter("@DepartureDateEnd" + i, team.DepartureDateEnd);
            //                    dbOperator.AddParameter("@DepartureFlightsFilter" + i, team.DepartureFlightsFilter);
            //                    dbOperator.AddParameter("@DepartureDateFilter" + i, team.DepartureDateFilter);
            //                    dbOperator.AddParameter("@DepartureWeekFilter" + i, team.DepartureWeekFilter);
            //                    dbOperator.AddParameter("@DepartureFlightsFilterType" + i, team.DepartureFlightsFilterType);
            //                    dbOperator.AddParameter("@ReturnFlightsFilter" + i, string.IsNullOrWhiteSpace(team.ReturnFlightsFilter) ? "" : team.ReturnFlightsFilter);
            //                    dbOperator.AddParameter("@ReturnFlightsFilterType" + i, team.ReturnFlightsFilterType);
            //                    dbOperator.AddParameter("@ExceptAirways" + i, team.ExceptAirways);
            //                    dbOperator.AddParameter("@DrawerCondition" + i, team.DrawerCondition);
            //                    dbOperator.AddParameter("@Remark" + i, team.Remark);
            //                    dbOperator.AddParameter("@AppointBerths" + i, team.AppointBerths);
            //                    dbOperator.AddParameter("@Berths" + i, team.Berths);
            //                    dbOperator.AddParameter("@InternalCommission" + i, team.InternalCommission);
            //                    dbOperator.AddParameter("@SubordinateCommission" + i, team.SubordinateCommission);
            //                    dbOperator.AddParameter("@ProfessionCommission" + i, team.ProfessionCommission);
            //                    dbOperator.AddParameter("@AutoAudit" + i, team.AutoAudit);
            //                    dbOperator.AddParameter("@ChangePNR" + i, team.ChangePNR);
            //                    dbOperator.AddParameter("@AutoPrint" + i, team.AutoPrint);
            //                    dbOperator.AddParameter("@SuitReduce" + i, team.SuitReduce);
            //                    dbOperator.AddParameter("@TicketType" + i, team.TicketType);
            //                    dbOperator.AddParameter("@StartPrintDate" + i, team.StartPrintDate);
            //                    dbOperator.AddParameter("@Suspended" + i, team.Suspended);
            //                    dbOperator.AddParameter("@Freezed" + i, team.Freezed);
            //                    dbOperator.AddParameter("@Audited" + i, team.Audited);
            //                    dbOperator.AddParameter("@CreateTime" + i, team.CreateTime);
            //                    dbOperator.AddParameter("@PrintBeforeTwoHours" + i, team.PrintBeforeTwoHours);
            //                    if (team.AuditTime.HasValue)
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, team.AuditTime);
            //                    }
            //                    else
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, DBNull.Value);
            //                    }
            //                    dbOperator.AddParameter("@Creator" + i, team.Creator);
            //                    dbOperator.AddParameter("@MultiSuitReduce" + i, team.MultiSuitReduce);
            //                    dbOperator.AddParameter("@Id" + i, team.Id);

            //                    if (!string.IsNullOrWhiteSpace(sql))
            //                    {
            //                        dbOperator.ExecuteNonQuery(sql);
            //                        sql = "";
            //                    }
            //                }
            //            }
            #endregion
        }

        public PolicyType CheckIfHasDefaultPolicy(Guid companyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = "select COUNT(0) from T_DefaultPolicy  where AdultProvider = @CompanyId or ChildProvider = @CompanyId ;select COUNT(0) from T_BargainDefaultPolicy where AdultProvider = @CompanyId";
                dbOperator.AddParameter("CompanyId", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read() && reader.GetInt32(0) != 0) return PolicyType.NormalDefault;
                    if (reader.NextResult() && reader.Read() && reader.GetInt32(0) != 0) return PolicyType.BargainDefault;
                }
                return PolicyType.Unknown;
            }
        }

        public PolicyType CheckIfHasDefaultPolicy(Guid companyId, List<string> airlines)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = "select COUNT(0) from T_DefaultPolicy where (AdultProvider = @CompanyId or ChildProvider = @CompanyId) AND CHARINDEX(Airline,@Airline,0)>0; select COUNT(0) from T_BargainDefaultPolicy where AdultProvider = @CompanyId AND CHARINDEX(Airline,@Airline,0)>0 ";
                dbOperator.AddParameter("CompanyId", companyId);
                dbOperator.AddParameter("Airline", string.Join(",", airlines));
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read() && reader.GetInt32(0) != 0) return PolicyType.NormalDefault;
                    if (reader.NextResult() && reader.NextResult() && reader.Read() && reader.GetInt32(0) != 0) return PolicyType.BargainDefault;
                }
                return PolicyType.Unknown;
            }
        }

        #endregion

        #region 构建政策的表
        //普通政策
        private DataTable GetTableNormalPolicy()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                                 new DataColumn("Id",typeof(Guid)),
                                                 new DataColumn("Owner",typeof(Guid)),
                                                 new DataColumn("Airline",typeof(string)),
                                                 new DataColumn("OfficeCode",typeof(string)),
                                                 new DataColumn("CustomCode",typeof(string)),
                                                 new DataColumn("ImpowerOffice",typeof(bool)),
                                                 new DataColumn("IsInternal",typeof(bool)),
                                                 new DataColumn("IsPeer",typeof(bool)),
                                                 new DataColumn("VoyageType",typeof(byte)),
                                                 new DataColumn("Departure",typeof(string)),
                                                 new DataColumn("Transit",typeof(string)),
                                                 new DataColumn("Arrival",typeof(string)),
                                                 new DataColumn("DepartureDateStart",typeof(DateTime)),
                                                 new DataColumn("DepartureDateEnd",typeof(DateTime)),
                                                 new DataColumn("DepartureFlightsFilter",typeof(string)),
                                                 new DataColumn("DepartureDateFilter",typeof(string)),
                                                 new DataColumn("DepartureWeekFilter",typeof(string)),
                                                 new DataColumn("DepartureFlightsFilterType",typeof(byte)),
                                                 new DataColumn("ReturnFlightsFilter",typeof(string)),
                                                 new DataColumn("ReturnFlightsFilterType",typeof(byte)),
                                                 new DataColumn("ExceptAirways",typeof(string)),
                                                 new DataColumn("DrawerCondition",typeof(string)),
                                                 new DataColumn("Remark",typeof(string)),
                                                 new DataColumn("Berths",typeof(string)),
                                                 new DataColumn("InternalCommission",typeof(decimal)),
                                                 new DataColumn("SubordinateCommission",typeof(decimal)),
                                                 new DataColumn("ProfessionCommission",typeof(decimal)),
                                                 new DataColumn("AutoAudit",typeof(bool)),
                                                 new DataColumn("ChangePNR",typeof(bool)),
                                                 new DataColumn("AutoPrint",typeof(bool)),
                                                 new DataColumn("SuitReduce",typeof(bool)),
                                                 new DataColumn("TicketType",typeof(byte)),
                                                 new DataColumn("StartPrintDate",typeof(DateTime)),
                                                 new DataColumn("Suspended",typeof(bool)),
                                                 new DataColumn("Freezed",typeof(bool)),
                                                 new DataColumn("Audited",typeof(bool)),
                                                 new DataColumn("CreateTime",typeof(DateTime)),
                                                 new DataColumn("AuditTime",typeof(DateTime)),
                                                 new DataColumn("Creator",typeof(string)),
                                                 new DataColumn("MultiSuitReduce",typeof(bool)),
                                                 new DataColumn("LastModifyTime",typeof(DateTime)),
                                                 new DataColumn("AbbreviateName",typeof(string)),
                                                 new DataColumn("PrintBeforeTwoHours",typeof(bool))
                                           });
            return dataTable;
        }

        //特价政策
        private DataTable GetTableBargainPolicy()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                                 new DataColumn("Id",typeof(Guid)),
                                                 new DataColumn("Owner",typeof(Guid)),
                                                 new DataColumn("Airline",typeof(string)),
                                                 new DataColumn("OfficeCode",typeof(string)),
                                                 new DataColumn("CustomCode",typeof(string)),
                                                 new DataColumn("ImpowerOffice",typeof(bool)),
                                                 new DataColumn("IsInternal",typeof(bool)),
                                                 new DataColumn("IsPeer",typeof(bool)),
                                                 new DataColumn("VoyageType",typeof(byte)),
                                                 new DataColumn("Departure",typeof(string)),
                                                 new DataColumn("Transit",typeof(string)),
                                                 new DataColumn("Arrival",typeof(string)),
                                                 new DataColumn("ExceptAirways",typeof(string)),
                                                 new DataColumn("DepartureDateStart",typeof(DateTime)),
                                                 new DataColumn("DepartureDateEnd",typeof(DateTime)),
                                                 new DataColumn("DepartureFlightsFilter",typeof(string)),
                                                 new DataColumn("DepartureDateFilter",typeof(string)),
                                                 new DataColumn("DepartureWeekFilter",typeof(string)),
                                                 new DataColumn("DepartureFlightsFilterType",typeof(byte)),
                                                 new DataColumn("ReturnFlightsFilter",typeof(string)),
                                                 new DataColumn("ReturnFlightsFilterType",typeof(byte)),
                                                 new DataColumn("BeforehandDays",typeof(Int16)),
                                                 new DataColumn("TravelDays",typeof(Int16)),
                                                 new DataColumn("InvalidRegulation",typeof(string)),
                                                 new DataColumn("ChangeRegulation",typeof(string)),
                                                 new DataColumn("EndorseRegulation",typeof(string)),
                                                 new DataColumn("RefundRegulation",typeof(string)),
                                                 new DataColumn("DrawerCondition",typeof(string)),
                                                 new DataColumn("Remark",typeof(string)),
                                                 new DataColumn("Berths",typeof(string)),
                                                 new DataColumn("InternalCommission",typeof(decimal)),
                                                 new DataColumn("SubordinateCommission",typeof(decimal)),
                                                 new DataColumn("ProfessionCommission",typeof(decimal)),
                                                 new DataColumn("Price",typeof(decimal)),
                                                 new DataColumn("PriceType",typeof(byte)),
                                                 new DataColumn("AutoAudit",typeof(bool)),
                                                 new DataColumn("ChangePNR",typeof(bool)),
                                                 new DataColumn("TicketType",typeof(byte)),
                                                 new DataColumn("StartPrintDate",typeof(DateTime)),
                                                 new DataColumn("Suspended",typeof(bool)),
                                                 new DataColumn("Freezed",typeof(bool)),
                                                 new DataColumn("Audited",typeof(bool)),
                                                 new DataColumn("CreateTime",typeof(DateTime)),
                                                 new DataColumn("AuditTime",typeof(DateTime)),
                                                 new DataColumn("Creator",typeof(string)),
                                                 new DataColumn("MultiSuitReduce",typeof(bool)),
                                                 new DataColumn("MostBeforehandDays",typeof(Int16)),
                                                 new DataColumn("LastModifyTime",typeof(DateTime)),
                                                 new DataColumn("AbbreviateName",typeof(string)),
                                                 new DataColumn("PrintBeforeTwoHours",typeof(bool))
                                           });
            return dataTable;
        }

        //特殊政策
        private DataTable GetTableSpecialPolicy()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                                 new DataColumn("Id",typeof(Guid)),
                                                 new DataColumn("Owner",typeof(Guid)),
                                                 new DataColumn("Type",typeof(byte)),
                                                 new DataColumn("OfficeCode",typeof(string)),
                                                 new DataColumn("CustomCode",typeof(string)),
                                                 new DataColumn("ImpowerOffice",typeof(bool)),
                                                 new DataColumn("IsPeer",typeof(bool)),
                                                 new DataColumn("IsInternal",typeof(bool)),
                                                 new DataColumn("Airline",typeof(string)),
                                                 new DataColumn("VoyageType",typeof(byte)),
                                                 new DataColumn("Departure",typeof(string)),
                                                 new DataColumn("Arrival",typeof(string)),
                                                 new DataColumn("ExceptAirways",typeof(string)),
                                                 new DataColumn("DepartureDateStart",typeof(DateTime)),
                                                 new DataColumn("DepartureDateEnd",typeof(DateTime)),
                                                 new DataColumn("DepartureDateFilter",typeof(string)),
                                                 new DataColumn("DepartureWeekFilter",typeof(string)),
                                                 new DataColumn("DepartureFlightsFilter",typeof(string)),
                                                 new DataColumn("DepartureFlightsFilterType",typeof(byte)),
                                                 new DataColumn("BeforehandDays",typeof(Int16)),
                                                 new DataColumn("InvalidRegulation",typeof(string)),
                                                 new DataColumn("ChangeRegulation",typeof(string)),
                                                 new DataColumn("EndorseRegulation",typeof(string)),
                                                 new DataColumn("RefundRegulation",typeof(string)),
                                                 new DataColumn("DrawerCondition",typeof(string)),
                                                 new DataColumn("Remark",typeof(string)),
                                                 new DataColumn("Price",typeof(decimal)),
                                                 new DataColumn("ProvideDate",typeof(DateTime)),
                                                 new DataColumn("ResourceAmount",typeof(int)),
                                                 new DataColumn("AutoAudit",typeof(bool)),
                                                 new DataColumn("ConfirmResource",typeof(bool)),
                                                 new DataColumn("Suspended",typeof(bool)),
                                                 new DataColumn("Freezed",typeof(bool)),
                                                 new DataColumn("Audited",typeof(bool)),
                                                 new DataColumn("PlatformAudited",typeof(bool)),
                                                 new DataColumn("SynBlackScreen",typeof(bool)),
                                                 new DataColumn("Berths",typeof(string)),
                                                 new DataColumn("TicketType",typeof(byte)),
                                                 new DataColumn("PriceType",typeof(byte)),
                                                 new DataColumn("InternalCommission",typeof(decimal)),
                                                 new DataColumn("SubordinateCommission",typeof(decimal)),
                                                 new DataColumn("ProfessionCommission",typeof(decimal)),
                                                 new DataColumn("CreateTime",typeof(DateTime)),
                                                 new DataColumn("AuditTime",typeof(DateTime)),
                                                 new DataColumn("Creator",typeof(string)),
                                                 new DataColumn("IsBargainBerths",typeof(bool)),
                                                 new DataColumn("IsSeat",typeof(bool)),
                                                 new DataColumn("LastModifyTime",typeof(DateTime)),
                                                 new DataColumn("AbbreviateName",typeof(string)),
                                                 new DataColumn("PrintBeforeTwoHours",typeof(bool)),
                                                 new DataColumn("LowNoType",typeof(byte)),
                                                 new DataColumn("LowNoMaxPrice",typeof(decimal)),
                                                 new DataColumn("LowNoMinPrice",typeof(decimal))
                                           });
            return dataTable;
        }

        //团队政策
        private DataTable GetTableTeamPolicy()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                                new DataColumn("Id",typeof(Guid)),
                                                new DataColumn("Owner",typeof(Guid)),
                                                new DataColumn("Airline",typeof(string)),
                                                new DataColumn("OfficeCode",typeof(string)),
                                                new DataColumn("CustomCode",typeof(string)),
                                                new DataColumn("ImpowerOffice",typeof(bool)),
                                                new DataColumn("IsInternal",typeof(bool)),
                                                new DataColumn("IsPeer",typeof(bool)),
                                                new DataColumn("VoyageType",typeof(byte)),
                                                new DataColumn("Departure",typeof(string)),
                                                new DataColumn("Transit",typeof(string)),
                                                new DataColumn("Arrival",typeof(string)),
                                                new DataColumn("DepartureDateStart",typeof(DateTime)),
                                                new DataColumn("DepartureDateEnd",typeof(DateTime)),
                                                new DataColumn("DepartureFlightsFilter",typeof(string)),
                                                new DataColumn("DepartureDateFilter",typeof(string)),
                                                new DataColumn("DepartureWeekFilter",typeof(string)),
                                                new DataColumn("DepartureFlightsFilterType",typeof(byte)),
                                                new DataColumn("ReturnFlightsFilter",typeof(string)),
                                                new DataColumn("ReturnFlightsFilterType",typeof(byte)),
                                                new DataColumn("ExceptAirways",typeof(string)),
                                                new DataColumn("DrawerCondition",typeof(string)),
                                                new DataColumn("Remark",typeof(string)),
                                                new DataColumn("AppointBerths",typeof(bool)),
                                                new DataColumn("Berths",typeof(string)),
                                                new DataColumn("InternalCommission",typeof(decimal)),
                                                new DataColumn("SubordinateCommission",typeof(decimal)),
                                                new DataColumn("ProfessionCommission",typeof(decimal)),
                                                new DataColumn("AutoAudit",typeof(bool)),
                                                new DataColumn("ChangePNR",typeof(bool)),
                                                new DataColumn("AutoPrint",typeof(bool)),
                                                new DataColumn("SuitReduce",typeof(bool)),
                                                new DataColumn("TicketType",typeof(byte)),
                                                new DataColumn("StartPrintDate",typeof(DateTime)),
                                                new DataColumn("Suspended",typeof(bool)),
                                                new DataColumn("Freezed",typeof(bool)),
                                                new DataColumn("Audited",typeof(bool)),
                                                new DataColumn("CreateTime",typeof(DateTime)),
                                                new DataColumn("AuditTime",typeof(DateTime)),
                                                new DataColumn("Creator",typeof(string)),
                                                new DataColumn("MultiSuitReduce",typeof(bool)),
                                                new DataColumn("LastModifyTime",typeof(DateTime)),
                                                new DataColumn("AbbreviateName",typeof(string)),
                                                new DataColumn("PrintBeforeTwoHours",typeof(bool))
                                           });
            return dataTable;
        }

        //政策出发地
        private DataTable GetTablePolicyDeparture()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                               new DataColumn("Code",typeof(string)),
                                               new DataColumn("PolicyId",typeof(Guid))
                                           });
            return dataTable;
        }
        #endregion

        //缺口政策
        private DataTable GetTableNotchPolicy()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
            {
                    new DataColumn("Id",typeof(Guid)),
                    new DataColumn("Owner",typeof(Guid)),
                    new DataColumn("Airline",typeof(string)),
                    new DataColumn("OfficeCode",typeof(string)),
                    new DataColumn("CustomCode",typeof(string)),
                    new DataColumn("ImpowerOffice",typeof(bool)),
                    new DataColumn("IsInternal",typeof(bool)),
                    new DataColumn("IsPeer",typeof(bool)),
                    new DataColumn("VoyageType",typeof(byte)),
                    new DataColumn("DepartureDateStart",typeof(DateTime)),
                    new DataColumn("DepartureDateEnd",typeof(DateTime)),
                    new DataColumn("DepartureDateFilter",typeof(string)),
                    new DataColumn("DepartureWeekFilter",typeof(string)),
                    new DataColumn("DrawerCondition",typeof(string)),
                    new DataColumn("Remark",typeof(string)),
                    new DataColumn("Berths",typeof(string)),
                    new DataColumn("InternalCommission",typeof(decimal)),
                    new DataColumn("SubordinateCommission",typeof(decimal)),
                    new DataColumn("ProfessionCommission",typeof(decimal)),
                    new DataColumn("AutoAudit",typeof(bool)),
                    new DataColumn("ChangePNR",typeof(bool)),
                    new DataColumn("TicketType",typeof(byte)),
                    new DataColumn("StartPrintDate",typeof(DateTime)),
                    new DataColumn("Suspended",typeof(bool)),
                    new DataColumn("Freezed",typeof(bool)),
                    new DataColumn("Audited",typeof(bool)) ,
                    new DataColumn("CreateTime",typeof(DateTime)) ,
                    new DataColumn("AuditTime",typeof(DateTime)) ,
                    new DataColumn("Creator",typeof(string)) ,
                    new DataColumn("LastModifyTime",typeof(DateTime)) ,
                    new DataColumn("AbbreviateName",typeof(string)),
                    new DataColumn("PrintBeforeTwoHours",typeof(bool)),
                    new DataColumn("DepartureFlightsFilter",typeof(string)),
                    new DataColumn("DepartureFlightsFilterType",typeof(byte))
              });
            return dataTable;
        }
        //缺口政策出发到达地
        private DataTable GetTableNotchPolicyDeparture()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                               new DataColumn("ID",typeof(Guid)),
                                               new DataColumn("PolicyId",typeof(Guid)),
                                               new DataColumn("Departure",typeof(string)),
                                               new DataColumn("Arrival",typeof(string )),
                                               new DataColumn("IsAllowable",typeof(bool))
                                           });
            return dataTable;
        }
        private DataTable GetTableVoyageCondition()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                               new DataColumn("Id",typeof(Guid)),
                                               new DataColumn("ProductId",typeof(Guid)),
                                               new DataColumn("IsApplicable",typeof(bool))
                                           });
            return dataTable;
        }
        private DataTable GetTableStopAirportApplicability()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                               new DataColumn("ConditionId",typeof(Guid)),
                                               new DataColumn("AirportCode",typeof(string)),
                                               new DataColumn("PositionNumber",typeof(string))
                                           });
            return dataTable;
        }
        public void DeleteNotchPolicy(params Guid[] ids)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string idlist = "";
                foreach (var item in ids)
                {
                    if (idlist == "")
                    {
                        idlist += "'" + item.ToString() + "'";
                    }
                    else
                    {
                        idlist += ",'" + item.ToString() + "'";
                    }
                }
                string sql = @"INSERT INTO T_NotchPolicyDelLog SELECT Id,Owner,Airline,OfficeCode,CustomCode,ImpowerOffice,IsInternal,IsPeer,VoyageType,
                                    DepartureDateStart,DepartureDateEnd,DepartureDateFilter,DepartureWeekFilter,DrawerCondition,Remark,Berths,
                                    InternalCommission,SubordinateCommission,ProfessionCommission,AutoAudit,ChangePNR,TicketType,StartPrintDate,Suspended,Freezed,
                                    Audited,CreateTime,AuditTime,Creator,LastModifyTime,AbbreviateName,PrintBeforeTwoHours,DepartureFlightsFilter,DepartureFlightsFilterType FROM T_NotchPolicy WHERE Id IN ("
                                    + idlist + ");DELETE FROM T_NotchPolicy WHERE Id IN (" + idlist
                                    + ");DELETE FROM dbo.T_NotchPolicyDepartureArrival WHERE PolicyId IN (" + idlist
                                    + "); DELETE  FROM saa FROM    Product.StopAirportApplicability saa INNER JOIN Product.VoyageCondition vc ON vc.Id = saa.ConditionId WHERE   vc.ProductId  IN (" + idlist
                                    + "); DELETE  FROM Product.VoyageCondition WHERE   ProductId IN (" + idlist + ");";
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public List<NotchPolicyInfo> QueryNotchPolicies(PolicyQueryParameter parameter, Pagination pagination)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                System.Collections.Generic.List<DataTransferObject.Policy.NotchPolicyInfo> result;
                if (!string.IsNullOrWhiteSpace(parameter.Airline))
                    dbOperator.AddParameter("@iAirline", parameter.Airline);
                if (!string.IsNullOrWhiteSpace(parameter.OfficeCode))
                    dbOperator.AddParameter("@iOfficeNo", parameter.OfficeCode);
                if (!string.IsNullOrWhiteSpace(parameter.Bunks))
                    dbOperator.AddParameter("@iBunks", parameter.Bunks);
                if (!string.IsNullOrWhiteSpace(parameter.Creator))
                    dbOperator.AddParameter("@iCreator", parameter.Creator);
                if (parameter.Audited != null)
                    dbOperator.AddParameter("@iAudited", parameter.Audited);
                if (parameter.Freezed != null)
                    dbOperator.AddParameter("@iFreezed", parameter.Freezed);
                if (parameter.Effective != null)
                    dbOperator.AddParameter("@iEffective", parameter.Effective);
                if (parameter.TicketType != null)
                    dbOperator.AddParameter("@iTicketType", parameter.TicketType);
                if (parameter.Suspended != null)
                    dbOperator.AddParameter("@iSuspended", parameter.Suspended);
                if (parameter.Owner != null)
                    dbOperator.AddParameter("@iOwner", parameter.Owner);
                if (parameter.DepartureDateStart != null)
                    dbOperator.AddParameter("@iDepartureDateStart", parameter.DepartureDateStart.Value.Date);
                if (parameter.DepartureDateEnd != null)
                    dbOperator.AddParameter("@iDepartureDateEnd", parameter.DepartureDateEnd.Value.Date);
                if (parameter.PubDateStart != null)
                    dbOperator.AddParameter("@iPubDateStart", parameter.PubDateStart.Date);
                if (parameter.PubDateEnd != null)
                    dbOperator.AddParameter("@iPubDateEnd", parameter.PubDateEnd.Date);
                if (parameter.InternalCommissionLower != null)
                    dbOperator.AddParameter("@iInternalCommissionLower", parameter.InternalCommissionLower);
                if (parameter.InternalCommissionUpper != null)
                    dbOperator.AddParameter("@iInternalCommissionUpper", parameter.InternalCommissionUpper);
                if (parameter.SubordinateCommissionLower != null)
                    dbOperator.AddParameter("@iSubordinateCommissionLower", parameter.SubordinateCommissionLower);
                if (parameter.SubordinateCommissionUpper != null)
                    dbOperator.AddParameter("@iSubordinateCommissionUpper", parameter.SubordinateCommissionUpper);
                if (parameter.ProfessionCommissionLower != null)
                    dbOperator.AddParameter("@iProfessionCommissionLower", parameter.ProfessionCommissionLower);
                if (parameter.ProfessionCommissionUpper != null)
                    dbOperator.AddParameter("@iProfessionCommissionUpper", parameter.ProfessionCommissionUpper);
                dbOperator.AddParameter("@iOrderBy", parameter.OrderBy);

                dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);

                var totalCount = dbOperator.AddParameter("@oTotalCount");

                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_QueryNotchPolicy", System.Data.CommandType.StoredProcedure))
                {
                    result = new System.Collections.Generic.List<DataTransferObject.Policy.NotchPolicyInfo>();
                    while (reader.Read())
                    {
                        NotchPolicyInfo view = new NotchPolicyInfo();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.NeedAUTH = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.DepartureDateStart = reader.GetDateTime(9);
                        view.DepartureDateEnd = reader.GetDateTime(10);
                        view.DepartureDateFilter = reader.GetString(11);
                        view.DepartureWeekFilter = reader.GetString(12);
                        view.DepartureFlightsFilter = reader.GetString(13);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(14);
                        //15 出票条件
                        view.Remark = reader.GetString(16);
                        view.Berths = reader.GetString(17);
                        view.InternalCommission = reader.GetDecimal(18);
                        view.SubordinateCommission = reader.GetDecimal(19);
                        view.ProfessionCommission = reader.GetDecimal(20);
                        //21 自动审核
                        view.ChangePNR = reader.GetBoolean(22);
                        view.TicketType = (TicketType)reader.GetByte(23);
                        view.StartProcessDate = reader.GetDateTime(24);
                        view.Suspended = !reader.IsDBNull(31);
                        view.Freezed = reader.GetBoolean(26);
                        view.Audited = reader.GetBoolean(27);
                        view.CreateTime = reader.GetDateTime(28);
                        view.AuditTime = reader.GetValue(29) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(29);
                        view.Creator = reader.GetString(30);
                        view.SuspendByPlatform = !reader.IsDBNull(31) && reader.GetBoolean(31);
                        result.Add(view);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
                return result;
            }
        }

        public void UpdateNotchPolicy(NotchPolicy item)
        {
            #region
            //using (var dbOperator = new DbOperator(Provider, ConnectionString))
            //{
            var sql = @"UPDATE  [dbo].[T_NotchPolicy] SET [Owner] = '" + item.Owner
                + "',[Airline] = '" + item.Airline
                + "',[OfficeCode] = '" + item.OfficeCode
                + "',[CustomCode] = '" + item.CustomCode
                + "',[ImpowerOffice] = '" + (item.ImpowerOffice ? "1" : "0")
                + "',[IsInternal] = '" + (item.IsInternal ? "1" : "0")
                + "',[IsPeer] = '" + (item.IsPeer ? "1" : "0")
                + "',[VoyageType] = '" + (byte)item.VoyageType
                + "',[DepartureDateStart] = '" + item.DepartureDateStart
                + "',[DepartureDateEnd] = '" + item.DepartureDateEnd
                + "',[DepartureDateFilter] = '" + item.DepartureDateFilter
                + "',[DepartureWeekFilter] = '" + item.DepartureWeekFilter
                + "',[DrawerCondition] = '" + item.DrawerCondition
                + "',[Remark] = '" + item.Remark
                + "',[Berths] = '" + item.Berths
                + "',[InternalCommission] = '" + item.InternalCommission
                + "',[SubordinateCommission] = '" + item.SubordinateCommission
                + "',[ProfessionCommission] = '" + item.ProfessionCommission
                + "',[AutoAudit] = '" + (item.AutoAudit ? "1" : "0")
                + "',[ChangePNR] = '" + (item.ChangePNR ? "1" : "0")
                + "',[TicketType] = '" + (byte)item.TicketType
                + "',[StartPrintDate] = '" + item.StartPrintDate
                + "',[Suspended] = '" + (item.Suspended ? "1" : "0")
                + "',[Freezed] = '" + (item.Freezed ? "1" : "0")
                + "',[Audited] = '" + (item.Audited ? "1" : "0")
                + "',[CreateTime] = '" + item.CreateTime
                + "',[AuditTime] = '" + item.AuditTime
                + "',[Creator] = '" + item.Creator
                + "',[LastModifyTime] = '" + item.LastModifyTime
                + "',[PrintBeforeTwoHours] = '" + (item.PrintBeforeTwoHours ? "1" : "0")
                + "',DepartureFlightsFilter = '" + item.DepartureFlightsFilter
                + "',DepartureFlightsFilterType = '" + (byte)item.DepartureFlightsFilterType
                + "' WHERE id='" + item.Id
                + "'; DELETE FROM dbo.T_NotchPolicyDepartureArrival WHERE PolicyId = '" + item.Id
                + "'; DELETE  FROM saa FROM    Product.StopAirportApplicability saa INNER JOIN Product.VoyageCondition vc ON vc.Id = saa.ConditionId WHERE   vc.ProductId = '" + item.Id
                + "'; DELETE  FROM Product.VoyageCondition WHERE   ProductId = '" + item.Id + "';";

            //int i = 0, j = 0, k = 0;
            //List<string> departureList = new List<string>();
            //List<string> arrivalList = new List<string>();  

            //foreach (var d in item.DepartureArrival)
            //{
            //    i++;
            //    sql += "INSERT INTO [dbo].[T_NotchPolicyDepartureArrival] ([Id],[PolicyId],[Departure],[Arrival],[IsAllowable]) VALUES (@Id" + i + " ,@PolicyId" + i + ",@Departure" + i + ",@Arrival" + i + ",@IsAllowable" + i + ");";
            //    dbOperator.AddParameter("@Id" + i, d.Id);
            //    dbOperator.AddParameter("@PolicyId" + i, item.Id);
            //    dbOperator.AddParameter("@Departure" + i, d.Departure);
            //    dbOperator.AddParameter("@Arrival" + i, d.Arrival);
            //    dbOperator.AddParameter("@IsAllowable" + i, d.IsAllowable);

            //    Guid nid = Guid.NewGuid();

            //    sql += " INSERT INTO [Product].[VoyageCondition] ([Id],[ProductId],[IsApplicable]) VALUES(@IdD" + i + ",@ProductIdD" + i + ",@IsApplicableE" + i + ");";

            //    dbOperator.AddParameter("@IdD" + i, nid);
            //    dbOperator.AddParameter("@ProductIdD" + i, item.Id);
            //    dbOperator.AddParameter("@IsApplicableE" + i, d.IsAllowable);
            //    foreach (var dd in d.Departure.Split('/'))
            //    {
            //        k++;
            //        sql += " INSERT INTO [Product].[StopAirportApplicability] ([ConditionId],[AirportCode],[PositionNumber]) VALUES(@ConditionIdc" + k + ",@AirportCodec" + k + ",1);";
            //        dbOperator.AddParameter("@ConditionIdc" + k, nid);
            //        dbOperator.AddParameter("@AirportCodec" + k, dd);
            //    }
            //    foreach (var a in d.Arrival.Split('/'))
            //    {
            //        j++;
            //        sql += " INSERT INTO [Product].[StopAirportApplicability] ([ConditionId],[AirportCode],[PositionNumber]) VALUES(@ConditionId" + j + ",@AirportCode" + j + ",2);";
            //        dbOperator.AddParameter("@ConditionId" + j, nid);
            //        dbOperator.AddParameter("@AirportCode" + j, a);


            //    }
            //}
            //dbOperator.ExecuteNonQuery(sql);
            //}
            int rowCount = 0;
            #region 给政策表中添加数据
            //var dataTableNotch = GetTableNotchPolicy();
            var n = GetTableNotchPolicyDeparture();
            var contion = GetTableVoyageCondition();
            var stop = GetTableStopAirportApplicability();

            foreach (var notch in item.DepartureArrival)
            {
                Guid nid = Guid.NewGuid();
                DataRow dataRowd = n.NewRow();
                dataRowd[0] = notch.Id;
                dataRowd[1] = item.Id;
                dataRowd[2] = notch.Departure;
                dataRowd[3] = notch.Arrival;
                dataRowd[4] = notch.IsAllowable;
                n.Rows.Add(dataRowd);
                DataRow dataRowC = contion.NewRow();
                dataRowC[0] = nid;
                dataRowC[1] = item.Id;
                dataRowC[2] = notch.IsAllowable;
                contion.Rows.Add(dataRowC);

                foreach (var d in notch.Departure.Split('/'))
                {
                    DataRow dataRowS = stop.NewRow();
                    dataRowS[0] = nid;
                    dataRowS[1] = d;
                    dataRowS[2] = 1;
                    stop.Rows.Add(dataRowS);
                }
                foreach (var a in notch.Arrival.Split('/'))
                {
                    DataRow dataRowS = stop.NewRow();
                    dataRowS[0] = nid;
                    dataRowS[1] = a;
                    dataRowS[2] = 2;
                    stop.Rows.Add(dataRowS);
                }
            }
            #endregion

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                var comd = sqlConnection.CreateCommand();

                SqlTransaction st = sqlConnection.BeginTransaction();

                try
                {
                    comd.Transaction = st;
                    comd.CommandText = sql;
                    comd.ExecuteNonQuery();
                    using (SqlBulkCopy sqlBC2 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC2.DestinationTableName = "dbo.T_NotchPolicyDepartureArrival";
                        sqlBC2.BatchSize = n.Rows.Count;
                        if (sqlBC2.BatchSize != 0)
                        {
                            sqlBC2.WriteToServer(n);
                            rowCount += sqlBC2.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC3 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC3.DestinationTableName = "Product.StopAirportApplicability";
                        sqlBC3.BatchSize = stop.Rows.Count;
                        if (sqlBC3.BatchSize != 0)
                        {
                            sqlBC3.WriteToServer(stop);
                            rowCount += sqlBC3.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC4 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC4.DestinationTableName = "Product.VoyageCondition";
                        sqlBC4.BatchSize = contion.Rows.Count;
                        if (sqlBC4.BatchSize != 0)
                        {
                            sqlBC4.WriteToServer(contion);
                            rowCount += sqlBC4.BatchSize;
                        }
                    }
                    st.Commit();
                }
                catch (Exception)
                {
                    st.Rollback();
                    throw;
                }
            }
            #endregion

        }

        public void InsertNotchPolicy(IEnumerable<NotchPolicy> notchs)
        {
            int rowCount = 0;

            #region 给政策表中添加数据
            var dataTableNotch = GetTableNotchPolicy();
            var n = GetTableNotchPolicyDeparture();
            var contion = GetTableVoyageCondition();
            var stop = GetTableStopAirportApplicability();

            foreach (var item in notchs)
            {
                DataRow dataRow = dataTableNotch.NewRow();
                dataRow[0] = item.Id;
                dataRow[1] = item.Owner;
                dataRow[2] = item.Airline;
                dataRow[3] = item.OfficeCode;
                dataRow[4] = item.CustomCode;
                dataRow[5] = item.ImpowerOffice;
                dataRow[6] = item.IsInternal;
                dataRow[7] = item.IsPeer;
                dataRow[8] = (byte)item.VoyageType;
                dataRow[9] = item.DepartureDateStart;
                dataRow[10] = item.DepartureDateEnd;
                dataRow[11] = item.DepartureDateFilter;
                dataRow[12] = item.DepartureWeekFilter;
                dataRow[13] = item.DrawerCondition;
                dataRow[14] = item.Remark;
                dataRow[15] = item.Berths;
                dataRow[16] = item.InternalCommission;
                dataRow[17] = item.SubordinateCommission;
                dataRow[18] = item.ProfessionCommission;
                dataRow[19] = item.AutoAudit;
                dataRow[20] = item.ChangePNR;
                dataRow[21] = (byte)item.TicketType;
                dataRow[22] = item.StartPrintDate;
                dataRow[23] = item.Suspended;
                dataRow[24] = item.Freezed;
                dataRow[25] = item.Audited;
                dataRow[26] = item.CreateTime;
                if (item.AuditTime.HasValue)
                {
                    dataRow[27] = item.AuditTime;
                }
                else
                {
                    dataRow[27] = DBNull.Value;
                }
                dataRow[28] = item.Creator;
                dataRow[29] = item.LastModifyTime;
                dataRow[30] = item.AbbreviateName;
                dataRow[31] = item.PrintBeforeTwoHours;
                dataRow[32] = item.DepartureFlightsFilter;
                dataRow[33] = (byte)item.DepartureFlightsFilterType;
                dataTableNotch.Rows.Add(dataRow);
                //List<string> departureList = new List<string>();
                //List<string> arrivalList = new List<string>();
                foreach (var notch in item.DepartureArrival)
                {
                    Guid nid = Guid.NewGuid();
                    DataRow dataRowd = n.NewRow();
                    dataRowd[0] = notch.Id;
                    dataRowd[1] = item.Id;
                    dataRowd[2] = notch.Departure;
                    dataRowd[3] = notch.Arrival;
                    dataRowd[4] = notch.IsAllowable;
                    n.Rows.Add(dataRowd);
                    DataRow dataRowC = contion.NewRow();
                    dataRowC[0] = nid;
                    dataRowC[1] = item.Id;
                    dataRowC[2] = notch.IsAllowable;
                    contion.Rows.Add(dataRowC);

                    foreach (var d in notch.Departure.Split('/'))
                    {
                        DataRow dataRowS = stop.NewRow();
                        dataRowS[0] = nid;
                        dataRowS[1] = d;
                        dataRowS[2] = 1;
                        stop.Rows.Add(dataRowS);
                    }
                    foreach (var a in notch.Arrival.Split('/'))
                    {
                        DataRow dataRowS = stop.NewRow();
                        dataRowS[0] = nid;
                        dataRowS[1] = a;
                        dataRowS[2] = 2;
                        stop.Rows.Add(dataRowS);
                    }
                }
                //foreach (var d in item.Departure.Split('/'))
                //{
                //    DataRow dataRowd = dataTableNormalDeparture.NewRow();
                //    dataRowd[0] = d;
                //    dataRowd[1] = item.Id;
                //    dataTableNormalDeparture.Rows.Add(dataRowd);
                //}
            }
            #endregion

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                SqlTransaction st = sqlConnection.BeginTransaction();
                try
                {
                    using (SqlBulkCopy sqlBC1 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC1.DestinationTableName = "dbo.T_NotchPolicy";
                        sqlBC1.BatchSize = dataTableNotch.Rows.Count;
                        if (sqlBC1.BatchSize != 0)
                        {
                            sqlBC1.WriteToServer(dataTableNotch);
                            rowCount += sqlBC1.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC2 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC2.DestinationTableName = "dbo.T_NotchPolicyDepartureArrival";
                        sqlBC2.BatchSize = n.Rows.Count;
                        if (sqlBC2.BatchSize != 0)
                        {
                            sqlBC2.WriteToServer(n);
                            rowCount += sqlBC2.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC4 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC4.DestinationTableName = "Product.VoyageCondition";
                        sqlBC4.BatchSize = contion.Rows.Count;
                        if (sqlBC4.BatchSize != 0)
                        {
                            sqlBC4.WriteToServer(contion);
                            rowCount += sqlBC4.BatchSize;
                        }
                    }
                    using (SqlBulkCopy sqlBC3 = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, st))
                    {
                        sqlBC3.DestinationTableName = "Product.StopAirportApplicability";
                        sqlBC3.BatchSize = stop.Rows.Count;
                        if (sqlBC3.BatchSize != 0)
                        {
                            sqlBC3.WriteToServer(stop);
                            rowCount += sqlBC3.BatchSize;
                        }
                    }
                    st.Commit();
                }
                catch (Exception)
                {
                    st.Rollback();
                    throw;
                }
            }

            #region
            //            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            //            {
            //                int i = 1;
            //                var sql = "";
            //                foreach (var notch in notchs)
            //                {
            //                    sql += @"INSERT INTO [dbo].[T_NotchPolicy] ([Id],[Owner],[Airline],[OfficeCode],[CustomCode],[ImpowerOffice],[IsInternal],[IsPeer],[VoyageType],DepartureDateStart,[DepartureDateEnd],[DepartureDateFilter],[DepartureWeekFilter],[DrawerCondition],[Remark],[Berths],[InternalCommission],[SubordinateCommission],[ProfessionCommission],[AutoAudit],[ChangePNR],[TicketType],[StartPrintDate],[Suspended],[Freezed],[Audited],[CreateTime],[AuditTime],[Creator],[LastModifyTime],[AbbreviateName],[PrintBeforeTwoHours],DepartureFlightsFilter,DepartureFlightsFilterType)
            //                                 VALUES
            //(@Id" + i + ",@Owner" + i + ",@Airline" + i + ",@OfficeCode" + i + ",@CustomCode" + i + ",@ImpowerOffice" + i + ",@IsInternal" + i + ",@IsPeer" + i + ",@VoyageType" + i + ",@DepartureDateStart" + i + ",@DepartureDateEnd" + i + ",@DepartureDateFilter" + i + ",@DepartureWeekFilter" + i + ",@DrawerCondition" + i + ",@Remark" + i + ",@Berths" + i + ",@InternalCommission" + i + ",@SubordinateCommission" + i + ",@ProfessionCommission" + i + ",@AutoAudit" + i + ",@ChangePNR" + i + ",@TicketType" + i + ",@StartPrintDate" + i + ",@Suspended" + i + ",@Freezed" + i + ",@Audited" + i + ",@CreateTime" + i + ",@AuditTime" + i + ",@Creator" + i + ",@LastModifyTime" + i + ",@AbbreviateName" + i + ",@PrintBeforeTwoHours" + i + ",@DepartureFlightsFilter" + i + ",@DepartureFlightsFilterType" + i + ");";
            //                    dbOperator.AddParameter("@Owner" + i, notch.Owner);
            //                    dbOperator.AddParameter("@Airline" + i, notch.Airline);
            //                    dbOperator.AddParameter("@OfficeCode" + i, notch.OfficeCode);
            //                    dbOperator.AddParameter("@CustomCode" + i, notch.CustomCode);
            //                    dbOperator.AddParameter("@ImpowerOffice" + i, notch.ImpowerOffice);
            //                    dbOperator.AddParameter("@IsInternal" + i, notch.IsInternal);
            //                    dbOperator.AddParameter("@IsPeer" + i, notch.IsPeer);
            //                    dbOperator.AddParameter("@VoyageType" + i, notch.VoyageType);
            //                    dbOperator.AddParameter("@DepartureDateStart" + i, notch.DepartureDateStart);
            //                    dbOperator.AddParameter("@DepartureDateEnd" + i, notch.DepartureDateEnd);
            //                    dbOperator.AddParameter("@DepartureDateFilter" + i, notch.DepartureDateFilter);
            //                    dbOperator.AddParameter("@DepartureWeekFilter" + i, notch.DepartureWeekFilter);
            //                    dbOperator.AddParameter("@DrawerCondition" + i, notch.DrawerCondition);
            //                    dbOperator.AddParameter("@Remark" + i, notch.Remark);
            //                    dbOperator.AddParameter("@Berths" + i, notch.Berths);
            //                    dbOperator.AddParameter("@InternalCommission" + i, notch.InternalCommission);
            //                    dbOperator.AddParameter("@SubordinateCommission" + i, notch.SubordinateCommission);
            //                    dbOperator.AddParameter("@ProfessionCommission" + i, notch.ProfessionCommission);
            //                    dbOperator.AddParameter("@AutoAudit" + i, notch.AutoAudit);
            //                    dbOperator.AddParameter("@ChangePNR" + i, notch.ChangePNR);
            //                    dbOperator.AddParameter("@TicketType" + i, notch.TicketType);
            //                    dbOperator.AddParameter("@StartPrintDate" + i, notch.StartPrintDate);
            //                    dbOperator.AddParameter("@Suspended" + i, notch.Suspended);
            //                    dbOperator.AddParameter("@Freezed" + i, notch.Freezed);
            //                    dbOperator.AddParameter("@Audited" + i, notch.Audited);
            //                    dbOperator.AddParameter("@CreateTime" + i, notch.CreateTime);
            //                    dbOperator.AddParameter("@LastModifyTime" + i, notch.LastModifyTime);
            //                    dbOperator.AddParameter("@AbbreviateName" + i, notch.AbbreviateName);
            //                    dbOperator.AddParameter("@PrintBeforeTwoHours" + i, notch.PrintBeforeTwoHours);
            //                    if (notch.AuditTime.HasValue)
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, notch.AuditTime);
            //                    }
            //                    else
            //                    {
            //                        dbOperator.AddParameter("@AuditTime" + i, DBNull.Value);
            //                    }
            //                    dbOperator.AddParameter("@Creator" + i, notch.Creator);
            //                    dbOperator.AddParameter("@DepartureFlightsFilter" + i, notch.DepartureFlightsFilter);
            //                    dbOperator.AddParameter("@DepartureFlightsFilterType" + i, (byte)notch.DepartureFlightsFilterType);
            //                    dbOperator.AddParameter("@Id" + i, notch.Id);
            //                    foreach (var item in notch.DepartureArrival)
            //                    {
            //                        i++;
            //                        sql += "  INSERT INTO [dbo].[T_NotchPolicyDepartureArrival] ([Id],[PolicyId],[Departure],[Arrival],[IsAllowable]) VALUES (@sId" + i + " ,@PolicyId" + i + ",@Departure" + i + ",@Arrival" + i + ",@IsAllowable" + i + ");";
            //                        dbOperator.AddParameter("@sId" + i, item.Id);
            //                        dbOperator.AddParameter("@PolicyId" + i, notch.Id);
            //                        dbOperator.AddParameter("@Departure" + i, item.Departure);
            //                        dbOperator.AddParameter("@Arrival" + i, item.Arrival);
            //                        dbOperator.AddParameter("@IsAllowable" + i, item.IsAllowable);
            //                    }
            //                    //if ( notch.DepartureArrival == null ||  notch.DepartureArrival.Count == 0)
            //                    //{
            //                    //    sql += "  INSERT INTO [dbo].[T_NotchPolicyDepartureArrival] ([Id],[PolicyId],[Departure],[Arrival],[IsAllowable]) VALUES (@sId" + i + " ,@PolicyId" + i + ",@Departure" + i + ",@Arrival" + i + ",@IsAllowable" + i + ");";
            //                    //    dbOperator.AddParameter("@sId" + i, Guid.NewGuid());
            //                    //    dbOperator.AddParameter("@PolicyId" + i, notch.Id);
            //                    //    dbOperator.AddParameter("@Departure" + i, "*");
            //                    //    dbOperator.AddParameter("@Arrival" + i, "*");
            //                    //    dbOperator.AddParameter("@IsAllowable" + i, "1");
            //                    //}
            //                    i++;
            //                }

            //                dbOperator.ExecuteNonQuery(sql);
            //}
            #endregion
        }

        public NotchPolicy QueryNotchPolicy(Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"SELECT [Id],[Owner],[Airline],[OfficeCode],[CustomCode],[ImpowerOffice],[IsInternal],[IsPeer],[VoyageType],DepartureDateStart,[DepartureDateEnd],[DepartureDateFilter],[DepartureWeekFilter],[DrawerCondition],[Remark],[Berths],[InternalCommission],[SubordinateCommission],[ProfessionCommission],[AutoAudit],[ChangePNR],[TicketType],[StartPrintDate],[Suspended],[Freezed],[Audited],[CreateTime],[AuditTime],[Creator],[LastModifyTime],[AbbreviateName],[PrintBeforeTwoHours],DepartureFlightsFilter,DepartureFlightsFilterType FROM [dbo].[T_NotchPolicy] WHERE ID = @ID";
                dbOperator.AddParameter("@ID", policyId);
                NotchPolicy view = null;
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        view = new NotchPolicy();
                        view.Id = reader.GetGuid(0);
                        view.Owner = reader.GetGuid(1);
                        view.Airline = reader.GetString(2);
                        view.OfficeCode = reader.GetString(3);
                        view.CustomCode = reader.GetString(4);
                        view.ImpowerOffice = reader.GetBoolean(5);
                        view.IsInternal = reader.GetBoolean(6);
                        view.IsPeer = reader.GetBoolean(7);
                        view.VoyageType = (VoyageType)reader.GetByte(8);
                        view.DepartureDateStart = reader.GetDateTime(9);
                        view.DepartureDateEnd = reader.GetDateTime(10);
                        view.DepartureDateFilter = reader.GetString(11);
                        view.DepartureWeekFilter = reader.GetString(12);
                        view.DrawerCondition = reader.GetString(13);
                        view.Remark = reader.GetString(14);
                        view.Berths = reader.GetString(15);
                        view.InternalCommission = reader.GetDecimal(16);
                        view.SubordinateCommission = reader.GetDecimal(17);
                        view.ProfessionCommission = reader.GetDecimal(18);
                        view.AutoAudit = reader.GetValue(19) == DBNull.Value ? false : reader.GetBoolean(19);
                        view.ChangePNR = reader.GetBoolean(20);
                        view.TicketType = (TicketType)reader.GetByte(21);
                        view.StartPrintDate = reader.GetDateTime(22);
                        view.Suspended = reader.GetBoolean(23);
                        view.Freezed = reader.GetBoolean(24);
                        view.Audited = reader.GetBoolean(25);
                        view.CreateTime = reader.GetDateTime(26);
                        view.AuditTime = reader.GetValue(27) == DBNull.Value ? (DateTime?)null : reader.GetDateTime(27);
                        view.Creator = reader.GetString(28);
                        view.LastModifyTime = reader.GetDateTime(29);
                        view.AbbreviateName = reader.GetString(30);
                        view.PrintBeforeTwoHours = reader.GetBoolean(31);
                        view.DepartureFlightsFilter = reader.GetString(32);
                        view.DepartureFlightsFilterType = (LimitType)reader.GetByte(33);
                    }
                }
                if (view != null)
                {
                    view.DepartureArrival = QueryNotchPolicyDepartureArrivals(policyId);
                }
                return view;
            }
        }

        private List<Data.DataMapping.NotchPolicyDepartureArrival> QueryNotchPolicyDepartureArrivals(Guid policyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                List<Data.DataMapping.NotchPolicyDepartureArrival> list = new List<Data.DataMapping.NotchPolicyDepartureArrival>();
                var sql = @"SELECT [Id],[PolicyId],[Departure],[Arrival],[IsAllowable] FROM [dbo].[T_NotchPolicyDepartureArrival] WHERE PolicyId = @PolicyId";
                dbOperator.AddParameter("@PolicyId", policyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        Data.DataMapping.NotchPolicyDepartureArrival view = new Data.DataMapping.NotchPolicyDepartureArrival();
                        view.Id = reader.GetGuid(0);
                        view.Departure = reader.GetString(2);
                        view.Arrival = reader.GetString(3);
                        view.IsAllowable = reader.GetBoolean(4);
                        list.Add(view);
                    }
                }
                return list;
            }
        }

        public bool UpdateNotchPolicyCommission(Guid id, decimal @internal, decimal subordinate, decimal profession)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = @"UPDATE  [dbo].[T_NotchPolicy] SET
                                       [InternalCommission] = @InternalCommission 
                                      ,[SubordinateCommission] = @SubordinateCommission
                                      ,[ProfessionCommission] = @ProfessionCommission
                                    WHERE id=@Id; ";
                dbOperator.AddParameter("@InternalCommission", @internal);
                dbOperator.AddParameter("@SubordinateCommission", subordinate);
                dbOperator.AddParameter("@ProfessionCommission", profession);
                dbOperator.AddParameter("@Id", id);
                return dbOperator.ExecuteNonQuery(sql) > 0;
            }
        }

        public bool NotchAudit(bool audit, params Guid[] ids)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string idlist = "";
                foreach (var item in ids)
                {
                    if (idlist == "")
                    {
                        idlist += "'" + item.ToString() + "'";
                    }
                    else
                    {
                        idlist += ",'" + item.ToString() + "'";
                    }
                }
                var sql = @"UPDATE  [dbo].[T_NotchPolicy] SET
                                       [Audited] = " + (audit ? "1" : "0") + ",[AuditTime]=GETDATE()  WHERE id in (" + idlist + ")";
                return dbOperator.ExecuteNonQuery(sql) > 0;
            }
        }

        public void NotchLock(bool @lock, params Guid[] ids)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string idlist = "";
                foreach (var item in ids)
                {
                    if (idlist == "")
                    {
                        idlist += "'" + item.ToString() + "'";
                    }
                    else
                    {
                        idlist += ",'" + item.ToString() + "'";
                    }
                }
                var sql = @"UPDATE  [dbo].[T_NotchPolicy] SET
                                       [Freezed] = " + (@lock ? "1" : "0") + "  WHERE id in (" + idlist + ")";
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<PolicyInfoBase> QueryPolicies(string airLine, DataTable voyages, VoyageType voyageType, PolicyType policyType)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var voyage = command.Parameters.Add("@Voyage", SqlDbType.Structured);
                    voyage.Value = voyages;
                    command.Parameters.AddWithValue("@Airline", airLine);
                    command.CommandText = "Product.ProcQuery";
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<PolicyInfoBase>();
                        while (reader.Read())
                        {
                            var policy = new NotchPolicyInfo
                            {
                                PolicyType = PolicyType.Notch,
                                Id = DataHelper.GetGuid(reader["Id"]),
                                Owner = DataHelper.GetGuid(reader["Owner"]),
                                Airline = DataHelper.GetString(reader["Airline"]),
                                VoyageType = DataHelper.GetEnumValue<VoyageType>(reader["VoyageType"]),
                                CustomCode = DataHelper.GetString(reader["CustomCode"]),
                                OfficeCode = DataHelper.GetString(reader["OfficeCode"]),
                                //Departure = DataHelper.GetString(reader["Departure"]),
                                //Arrival = DataHelper.GetString(reader["Arrival"]),
                                //ExceptAirways = DataHelper.GetString(reader["ExceptAirways"]),
                                DepartureFlightsFilterType = DataHelper.GetEnumValue<LimitType>(reader["DepartureFlightsFilterType"]),
                                DepartureFlightsFilter = DataHelper.GetString(reader["DepartureFlightsFilter"]),
                                DepartureDateFilter = DataHelper.GetString(reader["DepartureDateFilter"]),
                                DepartureWeekFilter = DataHelper.GetString(reader["DepartureWeekFilter"]),
                                DepartureDateStart = DataHelper.GetDateTime(reader["DepartureDateStart"]),
                                DepartureDateEnd = DataHelper.GetDateTime(reader["DepartureDateEnd"]),
                                StartProcessDate = DataHelper.GetDateTime(reader["StartPrintDate"]),
                                Berths = DataHelper.GetString(reader["Berths"]),
                                TicketType = DataHelper.GetEnumValue<TicketType>(reader["TicketType"]),
                                IsInternal = DataHelper.GetBoolean(reader["IsInternal"]),
                                InternalCommission = DataHelper.GetDecimal(reader["InternalCommission"]),
                                SubordinateCommission = DataHelper.GetDecimal(reader["SubordinateCommission"]),
                                IsPeer = DataHelper.GetBoolean(reader["IsPeer"]),
                                ProfessionCommission = DataHelper.GetDecimal(reader["ProfessionCommission"]),
                                Remark = DataHelper.GetString(reader["Remark"]),
                                Condition = DataHelper.GetString(reader["DrawerCondition"]),
                                ChangePNR = DataHelper.GetBoolean(reader["ChangePNR"]),
                                NeedAUTH = DataHelper.GetBoolean(reader["ImpowerOffice"]),
                                PrintBeforeTwoHours = DataHelper.GetBoolean(reader["PrintBeforeTwoHours"]),
                                Creator = DataHelper.GetString(reader["Creator"]),
                                CreateTime = DataHelper.GetDateTime(reader["CreateTime"]),
                                AuditTime = DataHelper.GetNullableDateTime(reader["AuditTime"]),
                                SuspendByPlatform = false,
                                Suspended = false,
                                Freezed = false,
                                Audited = true,
                                Enabled = true
                            };
                            result.Add(policy);
                        }
                        return result;
                    }
                }
            }
        }
    }
}
