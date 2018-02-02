using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    class ChildOrderableBunkRepository : SqlServerRepository, IChildOrderableBunkRepository {
        public ChildOrderableBunkRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(ChildOrderableBunk value) {
            string sql = "DELETE FROM [T_ChildOrderableBunk] WHERE [ID]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(ChildOrderableBunk value) {
            string sql = "INSERT INTO [T_ChildOrderableBunk]([ID],[AIRLINE],[BUNK],[Discount]) VALUES(@ID,@AIRLINE,@BUNK,@DISCOUNT)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                dbOperator.AddParameter("AIRLINE", value.AirlineCode.Value);
                dbOperator.AddParameter("BUNK", value.BunkCode.Value);
                dbOperator.AddParameter("DISCOUNT", value.Discount);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<Guid, ChildOrderableBunk>> Query() {
            string sql = "SELECT [Id],[Airline],[Bunk],[Discount] FROM [dbo].[T_ChildOrderableBunk]";
            var result = new List<KeyValuePair<Guid, ChildOrderableBunk>>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        ChildOrderableBunk item = new ChildOrderableBunk(reader.GetGuid(0));
                        item.AirlineCode = reader.GetString(1);
                        item.BunkCode = reader.GetString(2);
                        item.Discount = reader.GetDecimal(3);
                        result.Add(new KeyValuePair<Guid, ChildOrderableBunk>(item.Id, item));
                    }
                }
            }
            return result;
        }

        public int Update(ChildOrderableBunk value) {
            string sql = "UPDATE [dbo].[T_ChildOrderableBunk] SET [Airline]=@AIRLINE,[Bunk]=@BUNK,[Discount]=@DISCOUNT WHERE [Id]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                dbOperator.AddParameter("AIRLINE", value.AirlineCode.Value);
                dbOperator.AddParameter("BUNK", value.BunkCode.Value);
                dbOperator.AddParameter("DISCOUNT", value.Discount);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}