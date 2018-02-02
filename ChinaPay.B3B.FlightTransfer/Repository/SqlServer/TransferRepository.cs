using System;
using System.Collections.Generic;
using System.Data;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.FlightTransfer;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.FlightTransfer.Repository.SqlServer
{
    internal class TransferRepository : SqlServerTransaction, ITransferRepository
    {

        public TransferRepository(DbOperator command)
            : base(command)
        {
        }

        /// <summary>
        /// 查看航班变动通知信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IEnumerable<TransferInformation> QueryTransferInformation(Pagination pagination)
        {
            ClearParameters();
            List<TransferInformation> result = null;
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryFlightTransferInformation", System.Data.CommandType.StoredProcedure))
            {
                result = new List<TransferInformation>();
                while (reader.Read())
                {
                    var item = new TransferInformation()
                        {
                            PurchaserName = reader.GetString(3),
                            ContractPhone = reader.GetString(4),
                            PurchaserAccount = reader.GetString(2),
                            OrderCount = reader.GetInt32(0),
                            FlightCount = reader.GetInt32(1),
                            PurchaserId = reader.GetGuid(5)
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

        /// <summary>
        /// 查看单条航班变动通知信息
        /// </summary>
        /// <param name="purchaserId"></param>
        /// <returns></returns>
        public TransferInformation QuerySingleTransferInfomation(Guid purchaserId)
        {
            TransferInformation result = null;
            var sql = @"select OrderCount,FlgitCount,[Login] as PurchaserAccount,
            Com.AbbreviateName as PurchaserName,Contact.Cellphone,PurchaserId
          from (select PurchaserId,Count(1) as OrderCount,Count(1) as FlgitCount
          from T_FlightTransfer where InformStatus = 0 and PurchaserId = @PurchaserID group by PurchaserId
             ) as FlightMain
             join T_Employee Emp on FlightMain.PurchaserId = Emp.[Owner] and IsAdministrator = 1
             join T_Company Com on FlightMain.PurchaserId = Com.Id
             join T_Contact Contact on Com.Contact = Contact.Id";
            AddParameter("@PurchaserId", purchaserId);
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    result = new TransferInformation()
                        {
                            PurchaserName = reader.GetString(3),
                            ContractPhone = reader.GetString(4),
                            PurchaserAccount = reader.GetString(2),
                            OrderCount = reader.GetInt32(0),
                            FlightCount = reader.GetInt32(1),
                            PurchaserId = reader.GetGuid(5)
                        };
                }
            }
            return result;
        }

        public IEnumerable<TransferDetail> QueryTransferDetails(Pagination pagination, Guid purchaserId)
        {
            ClearParameters();
            List<TransferDetail> result = null;
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            AddParameter("@PurchaseId", purchaserId);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryFlightTransferDetail", System.Data.CommandType.StoredProcedure))
            {
                result = new List<TransferDetail>();
                while (reader.Read())
                {
                    var oFlightNO = reader.GetString(4);
                    string nFlightNO = reader.IsDBNull(8)?string.Empty:reader.GetString(8);
                    var item = new TransferDetail()
                        {
                            TransferId = reader.GetGuid(0),
                            PNR = new PNRPair(reader.IsDBNull(1)?string.Empty:reader.GetString(1) ,
                                reader.IsDBNull(2)?string.Empty:reader.GetString(2)),
                            OrderId = reader.GetDecimal(3),
                            OriginalCarrier = oFlightNO.Substring(0, 2),
                            OriginalFlightNo = oFlightNO.Substring(2),
                            OriginalTakeoffTime = reader.GetDateTime(6),
                            OriginalArrivalTime = reader.GetDateTime(7),
                            Carrier = !string.IsNullOrEmpty(nFlightNO) ? nFlightNO.Substring(0, 2) : string.Empty,
                            FlightNo = !string.IsNullOrEmpty(nFlightNO) ? nFlightNO.Substring(2) : string.Empty,
                            TransferType = (TransferType)reader.GetByte(5),
                        };
                    if (!reader.IsDBNull(9))
                    {
                        item.FlightDate = reader.GetDateTime(9);
                        if (!reader.IsDBNull(10))
                        {
                            item.TakeoffTime = reader.GetDateTime(10);
                            item.TakeoffTime = item.FlightDate.Value.AddHours(item.TakeoffTime.Value.Hour)
                                .AddMinutes(item.TakeoffTime.Value.Minute);
                        }
                        if (!reader.IsDBNull(11))
                        {
                            item.ArrivalTime = reader.GetDateTime(11);
                            item.ArrivalTime = item.FlightDate.Value.AddHours(item.ArrivalTime.Value.Hour)
    .AddMinutes(item.ArrivalTime.Value.Minute);

                        }
                    }


                    result.Add(item);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }

        public IEnumerable<InformRecord> QueryInformRecords(InfomrRecordSearchConditoin conditoin, Pagination pagination)
        {
            ClearParameters();
            List<InformRecord> result = null;
            if (!string.IsNullOrWhiteSpace(conditoin.Carrier))
            {
                AddParameter("@Carrier", conditoin.Carrier);
            }
            else
            {
                AddParameter("@Carrier", DBNull.Value);
            }
            if (!string.IsNullOrWhiteSpace(conditoin.FlightNo))
            {
                AddParameter("@FlightNo", conditoin.FlightNo);
            }
            else
            {
                AddParameter("@FlightNo", DBNull.Value);
            }
            if (!string.IsNullOrWhiteSpace(conditoin.Departure))
            {
                AddParameter("@Departure", conditoin.Departure);
            }
            else
            {
                AddParameter("@Departure", DBNull.Value);
            }
            if (conditoin.TransferType.HasValue)
            {
                AddParameter("@TransferType", (byte)conditoin.TransferType.Value);
            }
            else
            {
                AddParameter("@TransferType", DBNull.Value);
            }
            if (!string.IsNullOrWhiteSpace(conditoin.Arrival))
            {
                AddParameter("@Arrival", conditoin.Arrival);
            }
            else
            {
                AddParameter("@Arrival", DBNull.Value);
            }
            if (conditoin.PurchaserId.HasValue)
            {
                AddParameter("@PurchaserId", conditoin.PurchaserId.Value);
            }
            else
            {
                AddParameter("@PurchaserId", DBNull.Value);
            }
            if (conditoin.InformType.HasValue)
            {
                AddParameter("@InformMethod", (byte)conditoin.InformType.Value);
            }
            else
            {
                AddParameter("@InformMethod", DBNull.Value);
            }
            if (conditoin.InformResult.HasValue)
            {
                AddParameter("@InformResult", (byte)conditoin.InformResult.Value);
            }
            else
            {
                AddParameter("@InformResult", DBNull.Value);
            }
            if (conditoin.InformTimeFrom.HasValue)
            {
                AddParameter("@InformTimeFrom", conditoin.InformTimeFrom.Value);
            }
            else
            {
                AddParameter("@InformTimeFrom", DBNull.Value);
            }
            if (conditoin.InformTimeTo.HasValue)
            {
                AddParameter("@InformTimeTo", conditoin.InformTimeTo.Value);
            }
            else
            {
                AddParameter("@InformTimeTo", DBNull.Value);
            }
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryFlightInformDetail", System.Data.CommandType.StoredProcedure))
            {
                result = new List<InformRecord>();
                while (reader.Read())
                {
                    var item = new InformRecord();
                    item.TransferId = reader.GetGuid(0);
                    item.PurchaserId = reader.GetGuid(1);
                    item.PurchaserAccount = reader.GetString(2);
                    item.Carrier = reader.GetString(3);
                    item.FlightNO = reader.GetString(4);
                    item.DepartureCityName = reader.GetString(5);
                    item.DepartureName = reader.GetString(6);
                    //item.Departure = reader.GetString(5);
                    //item.Arrival = reader.GetString(7);
                    item.ArrivalCityName = reader.GetString(7);
                    item.ArrivalName = reader.GetString(8);
                    item.TransferType = (TransferType)reader.GetByte(9);
                    item.InformAccount = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                    item.InfromerName = reader.IsDBNull(14) ? string.Empty : reader.GetString(14);
                    if (!reader.IsDBNull(11))
                    {
                        item.InformType = (InformType)reader.GetByte(11);
                    }
                    if (!reader.IsDBNull(12))
                    {
                        item.InformResult = (InformResult)reader.GetByte(12);
                    }
                    if (!reader.IsDBNull(10))
                    {
                        item.InformTime = reader.GetDateTime(10);
                    }
                    result.Add(item);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }

        /// <summary>
        /// 保存通知记录
        /// </summary>
        /// <param name="purchaerId"> </param>
        /// <param name="transferIds"></param>
        /// <param name="inform"></param>
        /// <param name="result"></param>
        /// <param name="remark"></param>
        /// <param name="operatorAccount"> </param>
        /// <param name="operatorName"> </param>
        /// <returns></returns>
        public bool InformPurchaser(Guid purchaerId, List<Guid> transferIds, InformType inform, InformResult result, string remark,string operatorAccount,string operatorName)
        {
            string sql = string.Empty;
            if (transferIds == null)
            {
                sql = @"UPDATE [T_FlightTransfer]
                   SET [InformStatus] = @InformStatus,InformMethod = @InformMethod,
                    InformRemark = @InformRemark,InformTime=@InformTime,
                    InformerAcount =@InformerAccount,InformerName=@InformerName
                 WHERE PurchaserId = @PurchaserId";
            }
            else
            {
                sql = @"UPDATE [T_FlightTransfer]
                   SET [InformStatus] = @InformStatus,InformMethod = @InformMethod,
                    InformRemark = @InformRemark,InformTime=@InformTime,
                    InformerAcount =@InformerAccount,InformerName=@InformerName
                 WHERE PurchaserId = @PUrchaserId and TransferId in ('";
                sql += string.Join("','", transferIds);
                sql += "')";
            }


            AddParameter("@PurchaserId", purchaerId);
            AddParameter("@InformStatus", (byte)result);
            AddParameter("@InformMethod", (byte)inform);
            AddParameter("@InformTime", DateTime.Now);
            AddParameter("@InformRemark", remark);
            AddParameter("@InformerAccount", operatorAccount);
            AddParameter("@InformerName", operatorName);
            return ExecuteNonQuery(sql) > 0;
        }

        public DataTable QueryInformRecords(InfomrRecordSearchConditoin conditoin) {
            DataTable dt = null;
            using (var op = new DbOperator("System.Data.SqlClient", ConnectionManager.B3BConnectionString))
            {
                ClearParameters();
                if (!string.IsNullOrWhiteSpace(conditoin.Carrier))
                {
                    AddParameter("@Carrier", conditoin.Carrier);
                }
                else
                {
                    AddParameter("@Carrier", DBNull.Value);
                }
                if (!string.IsNullOrWhiteSpace(conditoin.FlightNo))
                {
                    AddParameter("@FlightNo", conditoin.FlightNo);
                }
                else
                {
                    AddParameter("@FlightNo", DBNull.Value);
                }
                if (!string.IsNullOrWhiteSpace(conditoin.Departure))
                {
                    AddParameter("@Departure", conditoin.Departure);
                }
                else
                {
                    AddParameter("@Departure", DBNull.Value);
                }
                if (conditoin.TransferType.HasValue)
                {
                    AddParameter("@TransferType", (byte)conditoin.TransferType.Value);
                }
                else
                {
                    AddParameter("@TransferType", DBNull.Value);
                }
                if (!string.IsNullOrWhiteSpace(conditoin.Arrival))
                {
                    AddParameter("@Arrival", conditoin.Arrival);
                }
                else
                {
                    AddParameter("@Arrival", DBNull.Value);
                }
                if (conditoin.PurchaserId.HasValue)
                {
                    AddParameter("@PurchaserId", conditoin.PurchaserId.Value);
                }
                else
                {
                    AddParameter("@PurchaserId", DBNull.Value);
                }
                if (conditoin.InformType.HasValue)
                {
                    AddParameter("@InformMethod", (byte)conditoin.InformType.Value);
                }
                else
                {
                    AddParameter("@InformMethod", DBNull.Value);
                }
                if (conditoin.InformResult.HasValue)
                {
                    AddParameter("@InformResult", (byte)conditoin.InformResult.Value);
                }
                else
                {
                    AddParameter("@InformResult", DBNull.Value);
                }
                if (conditoin.InformTimeFrom.HasValue)
                {
                    AddParameter("@InformTimeFrom", conditoin.InformTimeFrom.Value);
                }
                else
                {
                    AddParameter("@InformTimeFrom", DBNull.Value);
                }
                if (conditoin.InformTimeTo.HasValue)
                {
                    AddParameter("@InformTimeTo", conditoin.InformTimeTo.Value);
                }
                else
                {
                    AddParameter("@InformTimeTo", DBNull.Value);
                }
                System.Data.Common.DbParameter oTotalTicketCount = op.AddParameter("@oTotalCount");
                oTotalTicketCount.DbType = System.Data.DbType.Int32;
                oTotalTicketCount.Direction = System.Data.ParameterDirection.Output;


                dt = op.ExecuteTable("dbo.P_QueryFlightInformDetail", System.Data.CommandType.StoredProcedure);
            }
            return dt;
        }

        public FlightTransferStatInfo QueryFlightTransferStatInfo() {
            var result = new FlightTransferStatInfo();
            string sql = @" declare @LastQSTime datetime
                            set @LastQSTime = (select top(1) QSTime from T_LastQSTime)
                            declare @ToBeInformCount int
                            set @ToBeInformCount = (select COUNT(distinct PurchaserId)
                                from T_FlightTransfer where InformStatus <> 2)
                            select COUNT(distinct PurchaserId) as PurchaserCount,
			                        COUNT(distinct FlightNO) as FlightCount,
			                        COUNT(0) as OrderCount,
			                        @LastQSTime as LastQSTime,
			                        COUNT(distinct SUBSTRING(FlightNO,0,2)) as CarrierCount,
			                        @ToBeInformCount as ToBeInformCount
                            from T_FlightTransfer
                            where T_FlightTransfer.QSTime = @LastQSTime";
            using (var reader = ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    result.PurchaserCount = reader.GetInt32(0);
                    result.FlightCount = reader.GetInt32(1);
                    result.OrderCount = reader.GetInt32(2);
                    result.LastQSTime = reader.GetDateTime(3);
                    result.CarrierCount = reader.GetInt32(4);
                    result.ToBeInformCount = reader.GetInt32(5);
                }
            }
            return result;
        }

        public List<TransferDetail> QueryCurrentBatInformation() {
            var result = new List<TransferDetail>();
            var sql = @"DECLARE @QSTime DateTime
                    set @QSTime = (select top(1) QSTime from T_LastQSTime)
                    select FT.TransferId,FT.PNR,BPNR,FT.OrderId,FT.FlightNO as OFlightNO,TransferType,FL.TakeoffTime as OTakeOffTime,
			                    FL.LandingTime as OLandingTime,FT.NFlightNO,FT.NFlightDate,FT.NTakeOffTime,FT.NLandingTime,
			                    T_Contact.Cellphone,FT.PurchaserId
			                    from T_FlightTransfer FT
			                    inner join T_Flight FL on FT.OrderId = FL.OrderId and FT.FlightNO = FL.Carrier+FL.FlightNo
			                    inner join T_Company Com on FT.PurchaserId = Com.Id
			                    inner join T_Contact on Com.EmergencyContact = T_Contact.Id
			                    where FT.InformStatus <> 2 and FT.QSTime = @QSTime
		                    order by NTakeOffTime";
            using (var reader = ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    var oFlightNO = reader.GetString(4);
                    string nFlightNO = reader.IsDBNull(8)?null:reader.GetString(8);
                    result.Add(new TransferDetail()
                    {
                        TransferId = reader.GetGuid(0),
                        PNR = new PNRPair(reader.IsDBNull(1)?string.Empty:reader.GetString(1),
                            reader.IsDBNull(2)?string.Empty:reader.GetString(2)),
                        OrderId = reader.GetDecimal(3),
                        OriginalCarrier = oFlightNO.Substring(0, 2),
                        OriginalFlightNo = oFlightNO.Substring(2),
                        OriginalTakeoffTime = reader.GetDateTime(6),
                        OriginalArrivalTime = reader.GetDateTime(7),
                        Carrier = nFlightNO != null ? nFlightNO.Substring(0, 2) : string.Empty,
                        FlightNo = nFlightNO != null ? nFlightNO.Substring(2) : string.Empty,
                        TransferType = (TransferType)reader.GetByte(5),
                        PurchaserPhone = reader.GetString(12),
                        PurchaserId = reader.GetGuid(13)
                    });
                }
            }
            return result;
        }


        public PurchaseFlightTransferInfo QueryPurchaseFlightStaticInfo(Guid purchaserId)
        {
            PurchaseFlightTransferInfo result = null;
            string sql = @"DECLARE @LastQSTime DATETIME
                           SET @LastQSTime = (SELECT QSTime FROM T_LastQSTime)
                           SELECT @LastQSTime AS LastQSTime,OrderCount,FlgitCount,PurchaserId FROM (
                           SELECT PurchaserId,Count(1) AS OrderCount,Count(DISTINCT(flightTransfer.FlightNO)) AS FlgitCount
                           FROM T_FlightTransfer flightTransfer
                           INNER JOIN T_Flight flight ON flightTransfer.FlightNO =flight.Carrier +flight.FlightNo 
                           AND flightTransfer.OrderId = flight.OrderId AND DATEDIFF(DAY,flightTransfer.FlightDate,flight.TakeoffTime) =0
                           WHERE PassengerMsgSended=0 AND ((TransferType =3 AND flight.TakeoffTime > GETDATE()) OR (TransferType !=3 AND 
                           DATEADD ( DAY, DATEDIFF( DAY , 0 , NFlightDate ), CAST (flightTransfer.NTakeOffTime as DATETIME)) >= GETDATE()))
                           AND PurchaserId =@PurchaserId  GROUP BY PurchaserId
                          ) as FlightMain";
            ClearParameters();
            AddParameter("@PurchaserId", purchaserId);
            using (var reader = ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    result = new PurchaseFlightTransferInfo();
                    if (!reader.IsDBNull(0))
                    {
                        result.LastQSTime = reader.GetDateTime(0);
                    }
                    if (!reader.IsDBNull(1))
                    {
                        result.OrderCount = reader.GetInt32(1);
                    }
                    if (!reader.IsDBNull(2))
                    {
                        result.FlightCount = reader.GetInt32(2);
                    }
                    if (!reader.IsDBNull(3))
                    {
                        result.PurchaseId = reader.GetGuid(3);
                    }
                }
            }
            return result;
        }

        public IEnumerable<TransferDetail> QueryTransferDetailByPurchase(Pagination pagination, Guid purchaseId)
        {
            ClearParameters();
            var result = new List<TransferDetail>();
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex",pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            AddParameter("@PurchaseId", purchaseId);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryPurchaseFlightTransferDetail", System.Data.CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    var oFlightNO = reader.GetString(4);
                    string nFlightNO = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                    var item = new TransferDetail()
                    {
                        TransferId = reader.GetGuid(0),
                        PNR = new PNRPair(reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2)),
                        OrderId = reader.GetDecimal(3),
                        OriginalCarrier = oFlightNO.Substring(0, 2),
                        OriginalFlightNo = oFlightNO.Substring(2),
                        OriginalTakeoffTime = reader.GetDateTime(7),
                        OriginalArrivalTime = reader.GetDateTime(8),
                        Carrier = !string.IsNullOrEmpty(nFlightNO) ? nFlightNO.Substring(0, 2) : string.Empty,
                        FlightNo = !string.IsNullOrEmpty(nFlightNO) ? nFlightNO.Substring(2) : string.Empty,
                        TransferType = (TransferType)reader.GetByte(6),
                    };
                    if (!reader.IsDBNull(11))
                    {
                        item.FlightDate = reader.GetDateTime(11);
                        if (!reader.IsDBNull(9))
                        {
                            item.TakeoffTime = reader.GetDateTime(9);
                            item.TakeoffTime = item.FlightDate.Value.AddHours(item.TakeoffTime.Value.Hour)
                                .AddMinutes(item.TakeoffTime.Value.Minute);
                        }
                        if (!reader.IsDBNull(10))
                        {
                            item.ArrivalTime = reader.GetDateTime(10);
                            item.ArrivalTime = item.FlightDate.Value.AddHours(item.ArrivalTime.Value.Hour).AddMinutes(item.ArrivalTime.Value.Minute);
                        }
                    }
                    result.Add(item);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public IEnumerable<PurchaseTransferInformation> QueryTransferInformationByPurchase(FlightTransferCondition condition, Pagination pagination)
        {
            ClearParameters();
            var result = new List<PurchaseTransferInformation>();
            if (!string.IsNullOrWhiteSpace(condition.Carrier))
                AddParameter("@iCarrier",condition.Carrier);
            if (!string.IsNullOrWhiteSpace(condition.OriginalFlightNo))
                AddParameter("@iOriginalFlightNo", condition.OriginalFlightNo);
            if (condition.TransferType.HasValue)
                AddParameter("@iTransferType",(byte)condition.TransferType);
            if (condition.OriginalTakeOffLowerTime.HasValue)
                AddParameter("@iOriginalTakeOffLowerTime",condition.OriginalTakeOffLowerTime);
            if (condition.OriginalTakeOffUpperTime.HasValue)
                AddParameter("@iOriginalTakeOffUpperTime",condition.OriginalTakeOffUpperTime);
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            var totalCount = AddParameter("@oTotalCount");
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QueryPurchaseFlightTransferInformation", System.Data.CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    var oFlightNO = reader.GetString(0);
                    string nFlightNO = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                    var item = new PurchaseTransferInformation()
                    {
                        OriginalCarrier = oFlightNO.Substring(0, 2),
                        OriginalFlightNo = oFlightNO.Substring(2),
                        OriginalTakeoffTime = reader.GetDateTime(1),
                        OriginalArrivalTime = reader.GetDateTime(2),
                        Carrier = !string.IsNullOrEmpty(nFlightNO) ? nFlightNO.Substring(0, 2) : string.Empty,
                        FlightNo = !string.IsNullOrEmpty(nFlightNO) ? nFlightNO.Substring(2) : string.Empty,
                        TransferType = (TransferType)reader.GetByte(8),
                    };
                    if (!reader.IsDBNull(4))
                    {
                        item.FlightDate = reader.GetDateTime(4);
                        item.AddDays = reader.GetInt32(7);
                        if (!reader.IsDBNull(5))
                        {
                            item.TakeoffTime = reader.GetDateTime(5);
                            item.TakeoffTime = item.FlightDate.Value.AddHours(item.TakeoffTime.Value.Hour)
                                .AddMinutes(item.TakeoffTime.Value.Minute);
                        }
                        if (!reader.IsDBNull(6))
                        {
                            item.ArrivalTime = reader.GetDateTime(6);
                            item.ArrivalTime = item.FlightDate.Value.AddDays(item.AddDays).AddHours(item.ArrivalTime.Value.Hour).AddMinutes(item.ArrivalTime.Value.Minute);
                        }
                    }
                    result.Add(item);
                }
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public void UpdatePassengerMsgSended(Guid transferId)
        {
            string sql = "UPDATE dbo.T_FlightTransfer SET PassengerMsgSended =1 WHERE TransferId =@TransferId";
            ClearParameters();
            AddParameter("TransferId",transferId);
            ExecuteNonQuery(sql);
        }
    }
}