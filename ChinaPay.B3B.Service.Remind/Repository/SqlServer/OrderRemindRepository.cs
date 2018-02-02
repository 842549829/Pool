using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Remind.Model;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Remind.Repository.SqlServer {
    class OrderRemindRepository : SqlServerRepository, IOrderRemindRepository {
        public OrderRemindRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<RemindInfo> Query() {
            var result = new List<RemindInfo>();
            var sql = "SELECT Id,[Status],Carrier,Acceptor,CustomNO FROM dbo.T_OrderRemind";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("TYPE", 1);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new RemindInfo() {
                            Id = reader.GetDecimal(0),
                            Status = (RemindStatus)reader.GetByte(1),
                            Carrier = reader.GetString(2),
                            Acceptor = reader.GetGuid(3),
                            CustomNO = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        });
                    }
                }
            }
            return result;
        }

        public void Save(RemindInfo remindInfo) {
            var sql = "IF EXISTS(SELECT NULL FROM dbo.T_OrderRemind WHERE Id=@ID)" +
                        " UPDATE dbo.T_OrderRemind SET [Status]=@STATUS,Acceptor=@ACCEPTOR,CustomNO=@CustomNO WHERE Id=@ID"+
                      " ELSE"+
                        " INSERT INTO dbo.T_OrderRemind (Id,[Status],Carrier,Acceptor,CustomNO) VALUES (@ID,@STATUS,@CARRIER,@ACCEPTOR,@CustomNO)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", remindInfo.Id);
                dbOperator.AddParameter("STATUS", (byte)remindInfo.Status);
                dbOperator.AddParameter("CARRIER", remindInfo.Carrier ?? string.Empty);
                dbOperator.AddParameter("ACCEPTOR", remindInfo.Acceptor);
                dbOperator.AddParameter("CustomNO", remindInfo.CustomNO);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Delete(decimal id) {
            var sql = "DELETE FROM dbo.T_OrderRemind WHERE Id=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public ProviderRemindView QueryProviderRemindInfo(Guid provider) {
            var result = new ProviderRemindView();
            var statuses = getRemindStatuses(provider);
            if(statuses.ContainsKey(RemindStatus.AppliedForConfirm)) {
                result.Confirm = statuses[RemindStatus.AppliedForConfirm];
            }
            if(statuses.ContainsKey(RemindStatus.PaidForSupply)) {
                result.Supply = statuses[RemindStatus.PaidForSupply];
            }
            if(statuses.ContainsKey(RemindStatus.PaidForETDZ)) {
                result.ETDZ = statuses[RemindStatus.PaidForETDZ];
            }
            if(statuses.ContainsKey(RemindStatus.AppliedForRefund)) {
                result.Refund = statuses[RemindStatus.AppliedForRefund];
            }
            if(statuses.ContainsKey(RemindStatus.AppliedForScrap)) {
                result.Scrap = statuses[RemindStatus.AppliedForScrap];
            }
            if(statuses.ContainsKey(RemindStatus.AgreedForReturnMoney)) {
                result.ReturnMoney = statuses[RemindStatus.AgreedForReturnMoney];
            }
            if(statuses.ContainsKey(RemindStatus.OrderedForPay)) {
                result.PayOrder = statuses[RemindStatus.OrderedForPay];
            }
            if(statuses.ContainsKey(RemindStatus.AgreedForPostponeFee)) {
                result.PayPostponeFee = statuses[RemindStatus.AgreedForPostponeFee];
            }
            return result;
        }

        public SupplierRemindView QuerySupplierRemindInfo(Guid supplier) {
            var result = new SupplierRemindView();
            var statuses = getRemindStatuses(supplier);
            if(statuses.ContainsKey(RemindStatus.AppliedForConfirm)) {
                result.Confirm = statuses[RemindStatus.AppliedForConfirm];
            }
            if(statuses.ContainsKey(RemindStatus.PaidForSupply)) {
                result.Supply = statuses[RemindStatus.PaidForSupply];
            }
            if(statuses.ContainsKey(RemindStatus.OrderedForPay)) {
                result.PayOrder = statuses[RemindStatus.OrderedForPay];
            }
            if(statuses.ContainsKey(RemindStatus.AgreedForPostponeFee)) {
                result.PayPostponeFee = statuses[RemindStatus.AgreedForPostponeFee];
            }
            return result;
        }

        private Dictionary<RemindStatus, int> getRemindStatuses(Guid acceptor) {
            var result = new Dictionary<RemindStatus, int>();
            var sql = "SELECT [Status],COUNT(0) FROM dbo.T_OrderRemind WHERE Acceptor=@ACCEPTOR GROUP BY [Status]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ACCEPTOR", acceptor);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add((RemindStatus)reader.GetByte(0), reader.GetInt32(1));
                    }
                }
            }
            return result;
        }
    }
}