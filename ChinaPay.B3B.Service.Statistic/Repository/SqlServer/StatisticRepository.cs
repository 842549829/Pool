using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Statistic.Repository.SqlServer {
    class StatisticRepository : SqlServerRepository, IStatisticRepository {
        public StatisticRepository(string connectionString)
            : base(connectionString) {
        }

        public void SaveSpecialProductSupplyInfo(SpecialProductSupplyInfo supplyInfo) {
            var sql = "INSERT INTO dbo.T_SpecialProductSupplyInfo (Company,Departure,Arrival,Success,TicketCount,SupplyDate) " +
                "VALUES (@Company,@Departure,@Arrival,@Success,@TicketCount,@SupplyDate)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", supplyInfo.Company);
                dbOperator.AddParameter("Departure", supplyInfo.Departure);
                dbOperator.AddParameter("Arrival", supplyInfo.Arrival);
                dbOperator.AddParameter("Success", supplyInfo.Success);
                dbOperator.AddParameter("TicketCount", supplyInfo.TicketCount);
                dbOperator.AddParameter("SupplyDate", supplyInfo.SupplyDate);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void SaveGeneralOrderSpeedInfo(Guid company, int speed, DateTime processDate, string carrier, Common.Enums.TicketType ticketType, decimal businessId, byte type) {
            var sql = "INSERT INTO dbo.T_SpeedStatistic (Company,Speed,ProcessDate,Carrier,TicketType,BusinessId,Type)" + 
                " VALUES (@Company,@Speed,@ProcessDate,@Carrier,@TicketType,@BusinessId,@Type)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                dbOperator.AddParameter("Speed", speed);
                dbOperator.AddParameter("ProcessDate", processDate);
                dbOperator.AddParameter("Carrier", carrier);
                dbOperator.AddParameter("TicketType", (byte)ticketType);
                dbOperator.AddParameter("BusinessId", businessId);
                dbOperator.AddParameter("Type", type);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public List<GeneralProductSpeedStatisticInfo> QuerySpeed(IEnumerable<Guid> companies, string carrier, int statisticDays) {
            var result = new List<GeneralProductSpeedStatisticInfo>();
            var companiesString = companies.Join(",", item => "'" + item.ToString() + "'");
            var sql = "SELECT Company,[TicketType],[Type],AVG(Speed) AS Speed FROM dbo.T_SpeedStatistic WHERE Carrier=@Carrier AND ProcessDate >= @StartDate AND Company IN (" + companiesString + ") GROUP BY Company,TicketType,[Type]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Carrier", carrier);
                dbOperator.AddParameter("StartDate", DateTime.Today.AddDays(-statisticDays));
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new GeneralProductSpeedStatisticInfo {
                            Company = reader.GetGuid(0),
                            TicketType = (Common.Enums.TicketType)reader.GetByte(1),
                            Type = reader.GetByte(2),
                            Speed = reader.GetInt32(3),
                            Carrier = carrier
                        });
                    }
                }
            }
            return result;
        }

        public List<SpecialProductSupplyInfo> QuerySupplyStatisticInfo(IEnumerable<Guid> companies, int statisticDays) {
            var result = new List<SpecialProductSupplyInfo>();
            var companiesString = companies.Join(",", item => "'" + item.ToString() + "'");
            var sql = "SELECT Company,Departure,Arrival,Success,TicketCount,SupplyDate FROM dbo.T_SpecialProductSupplyInfo " +
                "WHERE SupplyDate >= @SupplyDate AND Company IN (" + companiesString + ")";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("SupplyDate", DateTime.Today.AddDays(-statisticDays));
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new SpecialProductSupplyInfo() {
                            Company = reader.GetGuid(0),
                            Departure = reader.GetString(1),
                            Arrival = reader.GetString(2),
                            Success = reader.GetBoolean(3),
                            TicketCount = reader.GetInt32(4),
                            SupplyDate = reader.GetDateTime(5)
                        });
                    }
                }
            }
            return result;
        }
    }
}