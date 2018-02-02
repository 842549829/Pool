using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Recommend.Repository.SqlServer {
    class FlightLowerFareRepository : SqlServerRepository, IFlightLowerFareRepository {
        public FlightLowerFareRepository(string connectionString)
            : base(connectionString) {
        }

        public void Save(Domain.FareInfo value) {
            var sql = "dbo.P_SaveFlightLowerFare";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("@iDeparture", value.Departure);
                dbOperator.AddParameter("@iArrival", value.Arrival);
                dbOperator.AddParameter("@iFlightDate", value.FlightDate);
                dbOperator.AddParameter("@iFare", value.Fare);
                dbOperator.AddParameter("@iDiscount", value.Discount);
                dbOperator.AddParameter("@iProduct", (byte)value.Product);
                dbOperator.ExecuteNonQuery(sql, System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Domain.FareInfo> Query() {
            var result = new List<Domain.FareInfo>();
            var sql = "SELECT Departure,Arrival,FlightDate,Fare,Discount,Product FROM dbo.T_FlightLowerFare WHERE FlightDate>=@FLIGHTDATE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("FLIGHTDATE", DateTime.Today);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new Domain.FareInfo(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2), 
                            reader.GetDecimal(3), reader.GetDecimal(4), (DataTransferObject.Order.ProductType)reader.GetByte(5), false));
                    }
                }
            }
            return result;
        }
    }
}