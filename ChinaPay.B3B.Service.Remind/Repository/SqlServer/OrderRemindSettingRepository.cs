using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Remind.Model;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Remind.Repository.SqlServer {
    class OrderRemindSettingRepository : SqlServerRepository, IOrderRemindSettingRepository {
        public OrderRemindSettingRepository(string connectionString)
            : base(connectionString) {
        }

        public void SaveCarrierSetting(Guid user, IEnumerable<string> carriers) {
            var sql = "IF EXISTS(SELECT NULL FROM dbo.T_OrderRemindSetting WHERE [User]=@USER)" +
                        " UPDATE dbo.T_OrderRemindSetting SET Carrier=@CARRIER WHERE [User]=@USER" +
                      " ELSE"+
                        " INSERT INTO dbo.T_OrderRemindSetting ([User],Carrier) VALUES (@USER,@CARRIER)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("USER", user);
                dbOperator.AddParameter("CARRIER", carriers.Join("|"));
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void SaveStatusSetting(Guid user, IEnumerable<Model.RemindStatus> status) {
            var sql = "IF EXISTS(SELECT NULL FROM dbo.T_OrderRemindSetting WHERE [User]=@USER)" +
                        " UPDATE dbo.T_OrderRemindSetting SET Status=@STATUS WHERE [User]=@USER" +
                      " ELSE" +
                        " INSERT INTO dbo.T_OrderRemindSetting ([User],Status) VALUES (@USER,@STATUS)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("USER", user);
                dbOperator.AddParameter("STATUS", status.Join("|",item=>((byte)item).ToString()));
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Model.RemindSetting QuerySetting(Guid user) {
            var carriers = new List<string>();
            var statuses = new List<Model.RemindStatus>();
            var sql = "SELECT Carrier,Status FROM dbo.T_OrderRemindSetting WHERE [User]=@USER";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("USER", user);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        if(!reader.IsDBNull(0)) {
                            var carrierArray = reader.GetString(0).Split('|');
                            foreach(var item in carrierArray) {
                                carriers.Add(item);
                            }
                        }
                        if(!reader.IsDBNull(1)) {
                            var statusValues = getStatusValues(reader.GetString(1));
                            foreach(var item in statusValues) {
                                statuses.Add((RemindStatus)item);
                            }
                        }
                    }
                }
            }
            
            return new RemindSetting(carriers, statuses);
        }
        private static IEnumerable<int> getStatusValues(string statuses) {
            var result = new List<int>();
            if(!string.IsNullOrWhiteSpace(statuses)) {
                var array = statuses.Split('|');
                foreach(var item in array) {
                    int num;
                    if(int.TryParse(item, out num)) {
                        result.Add(num);
                    }
                }
            }
            return result;
        }
    }
}