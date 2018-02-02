using System;
using System.Collections.Generic;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer
{
    class FixedNavigationRepository:SqlServerRepository, IFixedNavigationRepository {
        public FixedNavigationRepository(string connectionString)
            : base(connectionString){
        }

        public int Delete(DataTransferObject.Foundation.FixedNavigationView value)
        {
            string sql = "DELETE FROM [T_FixedNavigation] WHERE [Departure]=@Departure AND [Arrival]=@Arrival";
            using (var dbOperate = new DbOperator(Provider, ConnectionString))
            {
                dbOperate.AddParameter("Departure", value.Departure);
                dbOperate.AddParameter("Arrival", value.Arrival);
                return dbOperate.ExecuteNonQuery(sql);
            }
        }

        public int Insert(DataTransferObject.Foundation.FixedNavigationView value)
        {
            string sql ="INSERT INTO [T_FixedNavigation]([Departure],[Arrival]) VALUES(@Departure,@Arrival)";
            using (var dbOperator = new DbOperator(Provider,ConnectionString))
            {
                dbOperator.AddParameter("Departure", value.Departure);
                dbOperator.AddParameter("Arrival", value.Arrival);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<string, DataTransferObject.Foundation.FixedNavigationView>> Query()
        {
            string sql = "SELECT [Departure],[Arrival] FROM [T_FixedNavigation]";
            var result = new List<KeyValuePair<string,ChinaPay.B3B.DataTransferObject.Foundation.FixedNavigationView>>();
            using(var dbOperator = new DbOperator(Provider,ConnectionString))
	        {
		        using(var reader = dbOperator.ExecuteReader(sql))
	            {
		            while (reader.Read())
	                {
                        ChinaPay.B3B.DataTransferObject.Foundation.FixedNavigationView item = new DataTransferObject.Foundation.FixedNavigationView();
                        item.Departure = reader.GetString(0);
                        item.Arrival=reader.GetString(1);
                        result.Add(new KeyValuePair<string, ChinaPay.B3B.DataTransferObject.Foundation.FixedNavigationView>((item.Departure + item.Arrival), item));
	                }
	            }
	        }
            return result;
        }

        public int Update(DataTransferObject.Foundation.FixedNavigationView value)
        {
            throw new NotImplementedException();
        }
    }
}
