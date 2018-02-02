using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer
{
    class BunkRepository : SqlServerRepository, IBunkRepository
    {
        public BunkRepository(string connectionString)
            : base(connectionString)
        {
        }

        public int Delete(Bunk value)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var deleteSql = prepareDelete(dbOperator, value);
                return dbOperator.ExecuteNonQuery(deleteSql);
            }
        }
        string prepareDelete(DbOperator dbOperator, Bunk value)
        {
            dbOperator.AddParameter("DELETEBUNKID", value.Id);
            return "DELETE FROM [T_ExtendedBunk] WHERE [BUNKID]=@DELETEBUNKID;DELETE FROM [T_Bunk] WHERE [ID]=@DELETEBUNKID;";
        }

        public int Insert(Bunk value)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var insertSql = prepareInsert(dbOperator, value);
                if (string.IsNullOrWhiteSpace(insertSql))
                {
                    return -1;
                }
                else
                {
                    return dbOperator.ExecuteNonQuery(insertSql);
                }
            }
        }
        string prepareInsert(DbOperator dbOperator, Bunk value)
        {
            string bunkSql = "INSERT INTO [T_Bunk]([Id],[Airline],[FlightBeginDate],[FlightEndDate],[ETDZDate],[Code],[Discount],[Valid],[Departure]," +
                "[Arrival],[Type],[Description],ModifyTime,[VoyageType],[TravelType],[PassengerType],[RefundRegulation],[ChangeRegulation],[EndorseRegulation],[Remarks]) " +
                "VALUES(@ID,@AIRLINE,@FLIGHTBEGINDATE,@FLIGHTENDDATE,@ETDZDATE,@CODE,@DISCOUNT,@VALID,@DEPARTURE,@ARRIVAL,@TYPE,@DESCRIPTION," +
                "@ModifyTime,@VoyageType,@TravelType,@PassengerType,@RefundRegulation,@ChangeRegulation,@EndorseRegulation,@Remarks)";
            string extendedBunkInsertSql = "INSERT INTO [T_ExtendedBunk] ([BunkId],[Code],[Discount])";
            string extendedBunkValueFormat = " SELECT @ID,@CODE{0},@DISCOUNT{0} UNION ALL";

            var sbSql = new StringBuilder(bunkSql);
            dbOperator.AddParameter("ID", value.Id);
            dbOperator.AddParameter("VoyageType", value.VoyageType);
            dbOperator.AddParameter("TravelType", value.TravelType);
            dbOperator.AddParameter("PassengerType", value.PassengerType);
            dbOperator.AddParameter("RefundRegulation", value.RefundRegulation);
            dbOperator.AddParameter("ChangeRegulation", value.ChangeRegulation);
            dbOperator.AddParameter("EndorseRegulation", value.EndorseRegulation);
            if (string.IsNullOrEmpty(value.Remarks))
            {
                dbOperator.AddParameter("Remarks", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("Remarks", value.Remarks);
            }
            if (value.AirlineCode.IsNullOrEmpty())
            {
                dbOperator.AddParameter("AIRLINE", DBNull.Value);
            }
            else
            {
                dbOperator.AddParameter("AIRLINE", value.AirlineCode.Value);
            }
            dbOperator.AddParameter("FLIGHTBEGINDATE", value.FlightBeginDate);
            if (value.FlightEndDate.HasValue)
            {
                dbOperator.AddParameter("FLIGHTENDDATE", value.FlightEndDate.Value);
            }
            else
            {
                dbOperator.AddParameter("FLIGHTENDDATE", DBNull.Value);
            }
            dbOperator.AddParameter("ETDZDATE", value.ETDZDate);
            dbOperator.AddParameter("CODE", value.Code.Value);
            dbOperator.AddParameter("VALID", value.Valid);

            if (value is GeneralBunk)
            {
                var generalBunk = value as GeneralBunk;
                if (generalBunk.DepartureCode.IsNullOrEmpty())
                {
                    dbOperator.AddParameter("DEPARTURE", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("DEPARTURE", generalBunk.DepartureCode.Value);
                }
                if (generalBunk.ArrivalCode.IsNullOrEmpty())
                {
                    dbOperator.AddParameter("ARRIVAL", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("ARRIVAL", generalBunk.ArrivalCode.Value);
                }
                dbOperator.AddParameter("DISCOUNT", generalBunk.Discount);
                if (value is EconomicBunk)
                {
                    dbOperator.AddParameter("DESCRIPTION", DBNull.Value);
                }
                else if (value is FirstBusinessBunk)
                {
                    dbOperator.AddParameter("DESCRIPTION", (value as FirstBusinessBunk).Description ?? string.Empty);
                }
                if (generalBunk.Extended.Any())
                {
                    sbSql.Append(extendedBunkInsertSql);
                    int index = 0;
                    foreach (var item in generalBunk.Extended)
                    {
                        sbSql.AppendFormat(extendedBunkValueFormat, index);
                        dbOperator.AddParameter("CODE" + index.ToString(), item.Code.Value);
                        dbOperator.AddParameter("DISCOUNT" + index.ToString(), item.Discount);
                        index++;
                    }
                    sbSql.Remove(sbSql.Length - 10, 10);
                }
            }
            else if (value is PromotionBunk)
            {
                var promotionBunk = value as PromotionBunk;
                dbOperator.AddParameter("DEPARTURE", DBNull.Value);
                dbOperator.AddParameter("ARRIVAL", DBNull.Value);
                dbOperator.AddParameter("DISCOUNT", DBNull.Value);
                dbOperator.AddParameter("DESCRIPTION", promotionBunk.Description ?? string.Empty);
                if (promotionBunk.Extended.Any())
                {
                    sbSql.Append(extendedBunkInsertSql);
                    int index = 0;
                    foreach (var item in promotionBunk.Extended)
                    {
                        sbSql.AppendFormat(extendedBunkValueFormat, index);
                        dbOperator.AddParameter("CODE" + index.ToString(), item);
                        dbOperator.AddParameter("DISCOUNT" + index.ToString(), 0);
                        index++;
                    }
                    sbSql.Remove(sbSql.Length - 10, 10);
                }
            }
            else if (value is ProductionBunk)
            {
                dbOperator.AddParameter("DEPARTURE", DBNull.Value);
                dbOperator.AddParameter("ARRIVAL", DBNull.Value);
                dbOperator.AddParameter("DISCOUNT", DBNull.Value);
                dbOperator.AddParameter("DESCRIPTION", DBNull.Value);
            }
            else if (value is TransferBunk)
            {
                dbOperator.AddParameter("DEPARTURE", DBNull.Value);
                dbOperator.AddParameter("ARRIVAL", DBNull.Value);
                dbOperator.AddParameter("DISCOUNT", DBNull.Value);
                dbOperator.AddParameter("DESCRIPTION", DBNull.Value);
            }
            else if (value is FreeBunk)
            {
                dbOperator.AddParameter("DEPARTURE", DBNull.Value);
                dbOperator.AddParameter("ARRIVAL", DBNull.Value);
                dbOperator.AddParameter("DISCOUNT", DBNull.Value);
                dbOperator.AddParameter("DESCRIPTION", (value as FreeBunk).Description ?? string.Empty);
            }
            else if (value is TeamBunk)
            {
                dbOperator.AddParameter("DEPARTURE", DBNull.Value);
                dbOperator.AddParameter("ARRIVAL", DBNull.Value);
                dbOperator.AddParameter("DISCOUNT", DBNull.Value);
                dbOperator.AddParameter("DESCRIPTION", DBNull.Value);
            }
            else
            {
                return null;
            }
            dbOperator.AddParameter("TYPE", (int)value.Type);
            dbOperator.AddParameter("ModifyTime", value.ModifyTime);
            return sbSql.ToString();
        }

        public IEnumerable<KeyValuePair<Guid, Bunk>> Query()
        {
            string sql = "SELECT T1.[Type],T1.[Id],T1.[Airline],T1.[FlightBeginDate],T1.[FlightEndDate],T1.[ETDZDate],T1.[Code]," +
                "T1.[Discount],T1.[RefundRegulation],T1.[Valid],T1.[Departure],T1.[Arrival],T1.[Description],T1.ModifyTime," +
                "T2.[Code],T2.[Discount],T1.[ChangeRegulation],T1.[EndorseRegulation],T1.[Remarks],T1.[VoyageType],T1.[TravelType],T1.[PassengerType]" +
                "FROM [T_Bunk] T1 LEFT JOIN [T_ExtendedBunk] T2 ON T1.ID=T2.BunkId ORDER BY T1.[Id]";
            var result = new List<KeyValuePair<Guid, Bunk>>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Guid? previousId = null;
                    Bunk bunk = null;
                    while (reader.Read())
                    {
                        var currentId = reader.GetGuid(1);
                        if (bunk == null || previousId.Value != currentId)
                        {
                            bunk = null;
                        }
                        var bunkType = (BunkType)reader.GetInt32(0);
                        switch (bunkType)
                        {
                            case BunkType.Economic:
                                bunk = loadEconomicBunk(bunk, reader);
                                break;
                            case BunkType.FirstOrBusiness:
                                bunk = loadFirstBusinessBunk(bunk, reader);
                                break;
                            case BunkType.Promotion:
                                bunk = loadPromotionBunk(bunk, reader);
                                break;
                            case BunkType.Production:
                                bunk = loadProductionBunk(bunk, reader);
                                break;
                            case BunkType.Transfer:
                                bunk = loadTransferBunk(bunk, reader);
                                break;
                            case BunkType.Free:
                                bunk = loadFreeBunk(bunk, reader);
                                break;
                            case BunkType.Team:
                                bunk = loadTeamBunk(bunk, reader);
                                break;
                        }
                        bunk.VoyageType = (VoyageTypeValue)reader.GetByte(19);
                        bunk.TravelType = (TravelTypeValue)reader.GetByte(20);
                        bunk.PassengerType = (PassengerTypeValue)reader.GetByte(21);
                        bunk.RefundRegulation = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        bunk.ChangeRegulation = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
                        bunk.EndorseRegulation = reader.IsDBNull(17) ? string.Empty : reader.GetString(17);
                        bunk.Remarks = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                        if (!previousId.HasValue || previousId.Value != currentId)
                        {
                            result.Add(new KeyValuePair<Guid, Bunk>(bunk.Id, bunk));
                            previousId = currentId;
                        }
                    }
                }
            }
            return result;
        }
        EconomicBunk loadEconomicBunk(Bunk bunk, DbDataReader reader)
        {
            EconomicBunk result = null;
            var id = reader.GetGuid(1);
            if (null == bunk || id != bunk.Id)
            {
                result = new EconomicBunk(reader.GetGuid(1))
                {
                    AirlineCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    FlightBeginDate = reader.GetDateTime(3),
                    ETDZDate = reader.GetDateTime(5),
                    Code = reader.GetString(6),
                    Discount = reader.GetDecimal(7),
                    Valid = reader.GetBoolean(9),
                    DepartureCode = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    ArrivalCode = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                    ModifyTime = reader.GetDateTime(13)
                };
                if (!reader.IsDBNull(4))
                {
                    result.FlightEndDate = reader.GetDateTime(4);
                }
            }
            else
            {
                result = bunk as EconomicBunk;
            }
            var extendedBunk = loadExtendedBunk(reader);
            if (extendedBunk != null)
            {
                result.AddExtended(extendedBunk);
            }
            return result;
        }
        FirstBusinessBunk loadFirstBusinessBunk(Bunk bunk, DbDataReader reader)
        {
            FirstBusinessBunk result = null;
            var id = reader.GetGuid(1);
            if (null == bunk || id != bunk.Id)
            {
                result = new FirstBusinessBunk(reader.GetGuid(1))
                {
                    AirlineCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    FlightBeginDate = reader.GetDateTime(3),
                    ETDZDate = reader.GetDateTime(5),
                    Code = reader.GetString(6),
                    Discount = reader.GetDecimal(7),
                    Valid = reader.GetBoolean(9),
                    DepartureCode = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                    ArrivalCode = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                    Description = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    ModifyTime = reader.GetDateTime(13)
                };
                if (!reader.IsDBNull(4))
                {
                    result.FlightEndDate = reader.GetDateTime(4);
                }
            }
            else
            {
                result = bunk as FirstBusinessBunk;
            }
            var extendedBunk = loadExtendedBunk(reader);
            if (extendedBunk != null)
            {
                result.AddExtended(extendedBunk);
            }
            return result;
        }
        PromotionBunk loadPromotionBunk(Bunk bunk, DbDataReader reader)
        {
            PromotionBunk result = null;
            var id = reader.GetGuid(1);
            if (null == bunk || id != bunk.Id)
            {
                result = new PromotionBunk(reader.GetGuid(1))
                {
                    AirlineCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    FlightBeginDate = reader.GetDateTime(3),
                    ETDZDate = reader.GetDateTime(5),
                    Code = reader.GetString(6),
                    Valid = reader.GetBoolean(9),
                    Description = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    ModifyTime = reader.GetDateTime(13)
                };
                if (!reader.IsDBNull(4))
                {
                    result.FlightEndDate = reader.GetDateTime(4);
                }
            }
            else
            {
                result = bunk as PromotionBunk;
            }
            var extendedBunk = loadWithoutDiscountExtendedBunk(reader);
            if (!string.IsNullOrWhiteSpace(extendedBunk))
            {
                result.AddExtended(extendedBunk);
            }
            return result;
        }
        ProductionBunk loadProductionBunk(Bunk bunk, DbDataReader reader)
        {
            ProductionBunk result = null;
            var id = reader.GetGuid(1);
            if (null == bunk || id != bunk.Id)
            {
                result = new ProductionBunk(reader.GetGuid(1))
                {
                    AirlineCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    FlightBeginDate = reader.GetDateTime(3),
                    ETDZDate = reader.GetDateTime(5),
                    Code = reader.GetString(6),
                    Valid = reader.GetBoolean(9),
                    ModifyTime = reader.GetDateTime(13)
                };
                if (!reader.IsDBNull(4))
                {
                    result.FlightEndDate = reader.GetDateTime(4);
                }
            }
            else
            {
                result = bunk as ProductionBunk;
            }
            return result;
        }
        TransferBunk loadTransferBunk(Bunk bunk, DbDataReader reader)
        {
            TransferBunk result = null;
            var id = reader.GetGuid(1);
            if (null == bunk || id != bunk.Id)
            {
                result = new TransferBunk(reader.GetGuid(1))
                {
                    AirlineCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    FlightBeginDate = reader.GetDateTime(3),
                    ETDZDate = reader.GetDateTime(5),
                    Code = reader.GetString(6),
                    Valid = reader.GetBoolean(9),
                    ModifyTime = reader.GetDateTime(13)
                };
                if (!reader.IsDBNull(4))
                {
                    result.FlightEndDate = reader.GetDateTime(4);
                }
            }
            else
            {
                result = bunk as TransferBunk;
            }
            return result;
        }
        FreeBunk loadFreeBunk(Bunk bunk, DbDataReader reader)
        {
            FreeBunk result = null;
            var id = reader.GetGuid(1);
            if (null == bunk || id != bunk.Id)
            {
                result = new FreeBunk(reader.GetGuid(1))
                {
                    AirlineCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    FlightBeginDate = reader.GetDateTime(3),
                    ETDZDate = reader.GetDateTime(5),
                    Code = reader.GetString(6),
                    Valid = reader.GetBoolean(9),
                    Description = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                    ModifyTime = reader.GetDateTime(13)
                };
                if (!reader.IsDBNull(4))
                {
                    result.FlightEndDate = reader.GetDateTime(4);
                }
            }
            else
            {
                result = bunk as FreeBunk;
            }
            return result;
        }
        TeamBunk loadTeamBunk(Bunk bunk, DbDataReader reader)
        {
            TeamBunk result = null;
            var id = reader.GetGuid(1);
            if (null == bunk || id != bunk.Id)
            {
                result = new TeamBunk(reader.GetGuid(1))
                {
                    AirlineCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    FlightBeginDate = reader.GetDateTime(3),
                    ETDZDate = reader.GetDateTime(5),
                    Code = reader.GetString(6),
                    Valid = reader.GetBoolean(9),
                    ModifyTime = reader.GetDateTime(13)
                };
                if (!reader.IsDBNull(4))
                {
                    result.FlightEndDate = reader.GetDateTime(4);
                }
            }
            else
            {
                result = bunk as TeamBunk;
            }
            return result;
        }
        ExtendedWithDiscountBunk loadExtendedBunk(DbDataReader reader)
        {
            if (!reader.IsDBNull(14))
            {
                return new ExtendedWithDiscountBunk(reader.GetString(14), reader.GetDecimal(15));
            }
            return null;
        }
        string loadWithoutDiscountExtendedBunk(DbDataReader reader)
        {
            if (!reader.IsDBNull(14))
            {
                return reader.GetString(14);
            }
            return null;
        }

        public int Update(Bunk value)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = prepareDelete(dbOperator, value) + ";" + prepareInsert(dbOperator, value);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<Bunk> QueryBunkListView(DataTransferObject.Foundation.BunkQueryCondition condition, Core.Pagination pagination)
        {
            var result = new List<Bunk>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (!string.IsNullOrWhiteSpace(condition.Airline))
                    dbOperator.AddParameter("@iAirline",condition.Airline.Trim());
                if (!string.IsNullOrWhiteSpace(condition.Departure))
                    dbOperator.AddParameter("@iDeparture",condition.Departure.Trim());
                if (!string.IsNullOrWhiteSpace(condition.Arrival))
                    dbOperator.AddParameter("@iArrival",condition.Arrival.Trim());
                if (!string.IsNullOrWhiteSpace(condition.BunkCode))
                    dbOperator.AddParameter("@iBunkCode",condition.BunkCode.Trim());
                if (condition.BunkType.HasValue)
                    dbOperator.AddParameter("@iBunkType",(int)condition.BunkType);
                if (condition.VoyageType.HasValue)
                    dbOperator.AddParameter("@iVoyageType",(byte)condition.VoyageType);
                if (condition.FlightBeginDate.HasValue)
                    dbOperator.AddParameter("@iFlightBeginDate",condition.FlightBeginDate.Value.Date);
                if (condition.FlightEndDate.HasValue)
                    dbOperator.AddParameter("@iFlightEndDate",condition.FlightEndDate.Value.Date);
                if (condition.Status.HasValue)
                    dbOperator.AddParameter("@iStatus",condition.Status);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@iPageSize",pagination.PageSize);
                    dbOperator.AddParameter("@iPageIndex",pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (System.Data.Common.DbDataReader reader = dbOperator.ExecuteReader("dbo.P_QueryBunks", System.Data.CommandType.StoredProcedure))
                {
                    Guid? previousId = null;
                    Bunk bunk = null;
                    while (reader.Read())
                    {
                        var currentId = reader.GetGuid(1);
                        if (bunk == null || previousId.Value != currentId)
                        {
                            bunk = null;
                        }
                        var bunkType = (BunkType)reader.GetInt32(0);
                        switch (bunkType)
                        {
                            case BunkType.Economic:
                                bunk = loadEconomicBunk(bunk, reader);
                                break;
                            case BunkType.FirstOrBusiness:
                                bunk = loadFirstBusinessBunk(bunk, reader);
                                break;
                            case BunkType.Promotion:
                                bunk = loadPromotionBunk(bunk, reader);
                                break;
                            case BunkType.Production:
                                bunk = loadProductionBunk(bunk, reader);
                                break;
                            case BunkType.Transfer:
                                bunk = loadTransferBunk(bunk, reader);
                                break;
                            case BunkType.Free:
                                bunk = loadFreeBunk(bunk, reader);
                                break;
                            case BunkType.Team:
                                bunk = loadTeamBunk(bunk, reader);
                                break;
                        }
                        bunk.VoyageType = (VoyageTypeValue)reader.GetByte(19);
                        bunk.TravelType = (TravelTypeValue)reader.GetByte(20);
                        bunk.PassengerType = (PassengerTypeValue)reader.GetByte(21);
                        bunk.RefundRegulation = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        bunk.ChangeRegulation = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
                        bunk.EndorseRegulation = reader.IsDBNull(17) ? string.Empty : reader.GetString(17);
                        bunk.Remarks = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                        if (!previousId.HasValue || previousId.Value != currentId)
                        {
                            result.Add(bunk);
                            previousId = currentId;
                        }
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }


        public Bunk QueryBunkNew(Guid id)
        {
            string sql = "SELECT T1.[Type],T1.[Id],T1.[Airline],T1.[FlightBeginDate],T1.[FlightEndDate],T1.[ETDZDate],T1.[Code]," +
                 "T1.[Discount],T1.[RefundRegulation],T1.[Valid],T1.[Departure],T1.[Arrival],T1.[Description],T1.ModifyTime," +
                 "T2.[Code],T2.[Discount],T1.[ChangeRegulation],T1.[EndorseRegulation],T1.[Remarks],T1.[VoyageType],T1.[TravelType],T1.[PassengerType]" +
                 "FROM [T_Bunk] T1 LEFT JOIN [T_ExtendedBunk] T2 ON T1.ID=T2.BunkId WHERE T1.[Id]=@Id";
            Bunk bunk = null;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id",id);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var bunkType = (BunkType)reader.GetInt32(0);
                        switch (bunkType)
                        {
                            case BunkType.Economic:
                                bunk = loadEconomicBunk(bunk, reader);
                                break;
                            case BunkType.FirstOrBusiness:
                                bunk = loadFirstBusinessBunk(bunk, reader);
                                break;
                            case BunkType.Promotion:
                                bunk = loadPromotionBunk(bunk, reader);
                                break;
                            case BunkType.Production:
                                bunk = loadProductionBunk(bunk, reader);
                                break;
                            case BunkType.Transfer:
                                bunk = loadTransferBunk(bunk, reader);
                                break;
                            case BunkType.Free:
                                bunk = loadFreeBunk(bunk, reader);
                                break;
                            case BunkType.Team:
                                bunk = loadTeamBunk(bunk, reader);
                                break;
                        }
                        bunk.VoyageType = (VoyageTypeValue)reader.GetByte(19);
                        bunk.TravelType = (TravelTypeValue)reader.GetByte(20);
                        bunk.PassengerType = (PassengerTypeValue)reader.GetByte(21);
                        bunk.RefundRegulation = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        bunk.ChangeRegulation = reader.IsDBNull(16) ? string.Empty : reader.GetString(16);
                        bunk.EndorseRegulation = reader.IsDBNull(17) ? string.Empty : reader.GetString(17);
                        bunk.Remarks = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
                    }
                }
            }
            return bunk;
        }
    }
}