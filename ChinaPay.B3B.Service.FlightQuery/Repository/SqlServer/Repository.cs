using System;
using ChinaPay.B3B.Service.FlightQuery.Domain;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.FlightQuery.Repository.SqlServer {
    class Repository : ChinaPay.Repository.SqlServerRepository, IRepository {
        public Repository(string connectionString)
            : base(connectionString) {
        }

        public void SaveFlightRecord(Domain.FlightRecord record) {
            var sql = "INSERT INTO dbo.T_FlightRecord (Departure,Arrival,FlightDate,[Content]) VALUES (@Departure,@Arrival,@FlightDate,@Content)";
            using(var command = new DbOperator(Provider, ConnectionString)) {
                command.AddParameter("Departure", record.Departure);
                command.AddParameter("Arrival", record.Arrival);
                command.AddParameter("FlightDate", record.FlightDate.Date);
                command.AddParameter("Content", record.Content);
                command.ExecuteNonQuery(sql);
            }
        }

        public Domain.FlightRecord Query(string departure, string arrival, DateTime flightDate, bool exactQuery) {
            var sql = "SELECT TOP 1 Departure,Arrival,FlightDate,[Content] FROM dbo.T_FlightRecord WHERE Departure=@Departure AND Arrival=@Arrival";
            if(exactQuery) {
                sql += " AND FlightDate=@FlightDate";
            } else {
                sql += " ORDER BY DATEDIFF(DD,FlightDate,@FlightDate)";
            }
            using(var command = new DbOperator(Provider, ConnectionString)) {
                command.AddParameter("Departure", departure);
                command.AddParameter("Arrival", arrival);
                command.AddParameter("FlightDate", flightDate.Date);
                using(var reader = command.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        return new FlightRecord {
                            Departure = reader.GetString(0),
                            Arrival = reader.GetString(1),
                            FlightDate = reader.GetDateTime(2),
                            Content = reader.GetString(3)
                        };
                    }
                }
            }
            return null;
        }
    }
}