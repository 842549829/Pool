using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    internal class AirportRepository : SqlServerRepository, IAirportRepository {
        public AirportRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(Airport value) {
            var sql = "DELETE FROM [dbo].[T_Airport] WHERE [Code]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code.Value);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(Airport value) {
            var sql = "INSERT INTO [dbo].[T_Airport]([Code],[Name],[ShortName],[Valid],[Location],[LocationLevel],[IsMain]) VALUES(@CODE,@NAME,@SHORTNAME,@VALID,@LOCATION,@LOCTIONLEVEL,@ISMAIN)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code.Value);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SHORTNAME", value.ShortName);
                dbOperator.AddParameter("VALID", value.Valid);
                dbOperator.AddParameter("LOCATION", value.LocationCode);
                dbOperator.AddParameter("LOCTIONLEVEL", (int)value.LocationLevel);
                dbOperator.AddParameter("ISMAIN", value.IsMain);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<UpperString, Airport>> Query() {
            var sql = "SELECT [Code],[Name],[ShortName],[Valid],[Location],[LocationLevel],[IsMain] FROM [dbo].[T_Airport]";
            var result = new List<KeyValuePair<UpperString, Airport>>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        Airport item = new Airport(reader.GetString(0));
                        item.Name = reader.GetString(1);
                        item.ShortName = reader.GetString(2);
                        item.Valid = (bool)reader.GetValue(3);
                        item.LocationCode = reader.GetString(4);
                        item.LocationLevel = (DataTransferObject.Foundation.LocationLevel)reader.GetInt32(5);
                        item.IsMain = reader.GetBoolean(6);
                        result.Add(new KeyValuePair<UpperString, Airport>(item.Code, item));
                    }
                }
            }
            return result;
        }

        public int Update(Airport value) {
            var sql = "UPDATE [dbo].[T_Airport] SET [Name]=@NAME,[ShortName]=@SHORTNAME,[Valid]=@VALID,[Location]=@LOCATION,[LocationLevel]=@LOCATIONLEVEL,[IsMain]=@ISMAIN WHERE [Code]=@CODE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CODE", value.Code.Value);
                dbOperator.AddParameter("NAME", value.Name);
                dbOperator.AddParameter("SHORTNAME", value.ShortName);
                dbOperator.AddParameter("VALID", value.Valid);
                dbOperator.AddParameter("LOCATION", value.LocationCode);
                dbOperator.AddParameter("LOCATIONLEVEL", (int)value.LocationLevel);
                dbOperator.AddParameter("ISMAIN", value.IsMain);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
