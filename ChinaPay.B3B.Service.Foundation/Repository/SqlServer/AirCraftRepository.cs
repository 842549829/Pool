using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer {
    class AirCraftRepository : SqlServerRepository, IAirCraftRepository {
        public AirCraftRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(AirCraft value) {
            string sql = "DELETE FROM [T_AirCraft] WHERE [ID]=@ID ";
            using(var dbOperate = new DbOperator(Provider, ConnectionString)) {
                dbOperate.AddParameter("ID", value.Id);
                return dbOperate.ExecuteNonQuery(sql);
            }
        }

        public int Insert(AirCraft value) {
            string sql = "INSERT INTO [T_AirCraft]([ID],[CODE],[NAME],[AIRPORTFEE],[MANUFACTURER],[DESCRIPTION],[VALID]) VALUES (@ID,@CODE,@NAME,@AIRPORTFEE,@MANUFACTURER,@DESCRIPTION,@VALID)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                if(value.Code.IsNullOrEmpty()) {
                    dbOperator.AddParameter("CODE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("CODE", value.Code.Value);
                }
                if(string.IsNullOrEmpty(value.Name)) {
                    dbOperator.AddParameter("NAME", DBNull.Value);
                } else {
                    dbOperator.AddParameter("NAME", value.Name);
                }
                dbOperator.AddParameter("AIRPORTFEE", value.AirportFee);
                if(string.IsNullOrEmpty(value.Manufacturer)) {
                    dbOperator.AddParameter("MANUFACTURER", DBNull.Value);
                } else {
                    dbOperator.AddParameter("MANUFACTURER", value.Manufacturer);
                }
                if(string.IsNullOrEmpty(value.Description)) {
                    dbOperator.AddParameter("DESCRIPTION", DBNull.Value);
                } else {
                    dbOperator.AddParameter("DESCRIPTION", value.Description);
                }
                dbOperator.AddParameter("VALID", value.Valid);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<Guid, AirCraft>> Query() {
            string sql = "SELECT [ID],[CODE],[NAME],[AIRPORTFEE],[MANUFACTURER],[DESCRIPTION],[VALID] FROM [T_AirCraft]";
            var result = new List<KeyValuePair<Guid, AirCraft>>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        AirCraft item = new AirCraft(reader.GetGuid(0));
                        item.Code = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        item.Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        item.AirportFee = reader.GetDecimal(3);
                        item.Manufacturer = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        item.Description = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        item.Valid = (bool)reader.GetValue(6);
                        result.Add(new KeyValuePair<Guid, AirCraft>(item.Id, item));
                    }
                }
            }
            return result;
        }

        public int Update(AirCraft value) {
            string sql = "UPDATE [T_AirCraft] SET [CODE]=@CODE,[NAME]=@NAME,[AIRPORTFEE]=@AIRPORTFEE,[MANUFACTURER]=@MANUFACTURER,[DESCRIPTION]=@DESCRIPTION,[VALID]=@VALID WHERE [ID]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", value.Id);
                if(value.Code.IsNullOrEmpty()) {
                    dbOperator.AddParameter("CODE", DBNull.Value);
                } else {
                    dbOperator.AddParameter("CODE", value.Code.Value);
                }
                if(string.IsNullOrEmpty(value.Name)) {
                    dbOperator.AddParameter("NAME", DBNull.Value);
                } else {
                    dbOperator.AddParameter("NAME", value.Name);
                }
                dbOperator.AddParameter("AIRPORTFEE", value.AirportFee);
                if(string.IsNullOrEmpty(value.Manufacturer)) {
                    dbOperator.AddParameter("MANUFACTURER", DBNull.Value);
                } else {
                    dbOperator.AddParameter("MANUFACTURER", value.Manufacturer);
                }
                if(string.IsNullOrEmpty(value.Description)) {
                    dbOperator.AddParameter("DESCRIPTION", DBNull.Value);
                } else {
                    dbOperator.AddParameter("DESCRIPTION", value.Description);
                }
                dbOperator.AddParameter("VALID", value.Valid);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}