using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    class BasicPriceRepository : SqlServerRepository, IBasicPriceRepository {
        public BasicPriceRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(BasicPrice value) {
            string sql = "DELETE FROM [T_BasicPrice] WHERE ID=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(BasicPrice value) {
            string sql = "INSERT INTO [T_BasicPrice]([ID],[AIRLINE],[DEPARTURE],[ARRIVAL],[FLIGHTDATE],[ETDZDATE],[PRICE],[MILEAGE],[ModifyTime]) VALUES(@ID,@AIRLINE,@DEPARTURE,@ARRIVAL,@FLIGHTDATE,@ETDZDATE,@PRICE,@MILEAGE,@MODIFYTIME)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                if(value.AirlineCode.IsNullOrEmpty()) {
                    dbOperator.AddParameter("AIRLINE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("AIRLINE", value.AirlineCode.Value);
                }
                dbOperator.AddParameter("DEPARTURE", value.DepartureCode.Value);
                dbOperator.AddParameter("ARRIVAL", value.ArrivalCode.Value);
                dbOperator.AddParameter("FLIGHTDATE", value.FlightDate);
                dbOperator.AddParameter("ETDZDATE", value.ETDZDate);
                dbOperator.AddParameter("PRICE", value.Price);
                dbOperator.AddParameter("MILEAGE", value.Mileage);
                dbOperator.AddParameter("MODIFYTIME", value.ModifyTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<Guid, BasicPrice>> Query() {
            string sql = "SELECT [ID],[AIRLINE],[DEPARTURE],[ARRIVAL],[FLIGHTDATE],[ETDZDATE],[PRICE],[MILEAGE],[ModifyTime] FROM [T_BasicPrice]";
            var result = new List<KeyValuePair<Guid, BasicPrice>>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        BasicPrice item = new BasicPrice(reader.GetGuid(0));
                        item.AirlineCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        item.DepartureCode = reader.GetString(2);
                        item.ArrivalCode = reader.GetString(3);
                        item.FlightDate = reader.GetDateTime(4);
                        item.ETDZDate = reader.GetDateTime(5);
                        item.Price = reader.GetDecimal(6);
                        item.Mileage = reader.GetDecimal(7);
                        item.ModifyTime = reader.IsDBNull(8) ? new DateTime(2012, 10, 26, 0, 0, 0) : reader.GetDateTime(8);
                        result.Add(new KeyValuePair<Guid, BasicPrice>(item.Id, item));
                    }
                }
            }
            return result;
        }

        public int Update(BasicPrice value) {
            string sql = "UPDATE [T_BasicPrice] SET [AIRLINE]=@AIRLINE,[DEPARTURE]=@DEPARTURE,[ARRIVAL]=@ARRIVAL,[FLIGHTDATE]=@FLIGHTDATE,[ETDZDATE]=@ETDZDATE,[PRICE]=@PRICE,[MILEAGE]=@MILEAGE,ModifyTime=@MODIFYTIME WHERE [ID]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                if(value.AirlineCode.IsNullOrEmpty()) {
                    dbOperator.AddParameter("AIRLINE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("AIRLINE", value.AirlineCode.Value);
                }
                dbOperator.AddParameter("DEPARTURE", value.DepartureCode.Value);
                dbOperator.AddParameter("ARRIVAL", value.ArrivalCode.Value);
                dbOperator.AddParameter("FLIGHTDATE", value.FlightDate);
                dbOperator.AddParameter("ETDZDATE", value.ETDZDate);
                dbOperator.AddParameter("PRICE", value.Price);
                dbOperator.AddParameter("MILEAGE", value.Mileage);
                dbOperator.AddParameter("MODIFYTIME", value.ModifyTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<DataTransferObject.Foundation.BasicPriceView> QueryBasicPrice(string airline, string departure, string arrival, Core.Pagination pagination)
        {
            IList<DataTransferObject.Foundation.BasicPriceView> result = new List<DataTransferObject.Foundation.BasicPriceView>();
            string fields = "[Id],[Airline],[Departure],[Arrival],[FlightDate],[ETDZDate],[Price],[Mileage],ModifyTime";
            string catelog = "[dbo].[T_BasicPrice]";
            string orderbyFiled = "[Airline] DESC,[FlightDate] DESC,[Departure],[Arrival],ModifyTime desc";
            string where = "";
            if (!string.IsNullOrWhiteSpace(airline))
            {
                where += "([Airline] IS NULL OR [Airline] = '" + airline + "') AND ";
            }
            if (!string.IsNullOrWhiteSpace(departure) || !string.IsNullOrWhiteSpace(arrival))
            {
                if (!string.IsNullOrWhiteSpace(departure) && string.IsNullOrWhiteSpace(arrival))
                {
                    where += "([Departure] = '" + departure + "' OR [Arrival] = '" + departure + "')  AND ";
                } if (!string.IsNullOrWhiteSpace(arrival) && string.IsNullOrWhiteSpace(departure))
                {
                    where += "([Departure] = '" + arrival + "' OR [Arrival] = '" + arrival + "')  AND ";
                }
                if (!string.IsNullOrWhiteSpace(arrival) && !string.IsNullOrWhiteSpace(departure))
                {
                    where += "(([Departure] = '" + arrival + "' AND [Arrival] = '" + departure + "') OR ([Departure] = '" + departure + "' AND [Arrival] = '" + arrival + "'))  AND ";
                }
            }
            if (where.Length > 0)
            {
                where = where.Remove(where.Length-5,5);
            }
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@iField", fields);
                dbOperator.AddParameter("@iCatelog", catelog);
                dbOperator.AddParameter("@iCondition", where);
                dbOperator.AddParameter("@iOrderBy", orderbyFiled);
                dbOperator.AddParameter("@iPagesize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                dbOperator.AddParameter("@iGetCount", pagination.GetRowCount);
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (System.Data.Common.DbDataReader reader = dbOperator.ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        DataTransferObject.Foundation.BasicPriceView basicPriceView = new DataTransferObject.Foundation.BasicPriceView();
                        basicPriceView.Id = reader.GetGuid(0);
                        basicPriceView.Airline = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        basicPriceView.Departure = reader.GetString(2);
                        basicPriceView.Arrival = reader.GetString(3);
                        basicPriceView.FlightDate = reader.GetDateTime(4);
                        basicPriceView.ETDZDate = reader.GetDateTime(5);
                        basicPriceView.Price = reader.GetDecimal(6);
                        basicPriceView.Mileage = reader.GetDecimal(7);
                        basicPriceView.ModifyTime = reader.IsDBNull(8) ? new DateTime(2013, 10, 26, 0, 0, 0) : reader.GetDateTime(8);
                        result.Add(basicPriceView);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }


        public BasicPrice QueryBasicPrice(Guid basicPriceId)
        {
            string sql = "SELECT [AIRLINE],[DEPARTURE],[ARRIVAL],[FLIGHTDATE],[ETDZDATE],[PRICE],[MILEAGE],ModifyTime FROM [T_BasicPrice] WHERE [ID]=@Id";
            BasicPrice result =null;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id",basicPriceId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result = new BasicPrice(basicPriceId);
                        result.AirlineCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        result.DepartureCode = reader.GetString(1);
                        result.ArrivalCode = reader.GetString(2);
                        result.FlightDate = reader.GetDateTime(3);
                        result.ETDZDate = reader.GetDateTime(4);
                        result.Price = reader.GetDecimal(5);
                        result.Mileage = reader.GetDecimal(6);
                        result.ModifyTime = reader.IsDBNull(7) ?
                            new DateTime(2012, 10, 26, 0, 0, 0) : reader.GetDateTime(7);
                    }
                }
            }
            return result;
        }
    }
}