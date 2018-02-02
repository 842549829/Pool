using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.DataAccess;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Order.Domain;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer {
    class CoordinationRepository : SqlServerRepository, ICoordinationRepository {
        public CoordinationRepository(string connectionString)
            : base(connectionString) {
        }

        #region ICoordinationRepository 成员

        public void Save(decimal orderId, Domain.Coordination coordination)
        {
            var sql = "INSERT INTO [dbo].[T_Coordination] ([OrderId],[BusinessType],[CotactMode],[Content],[Result],[Coordinator],[CoordinateTime])" +
                " VALUES (@ORDERID,@BUSINESSTYPE,@COTACTMODE,@CONTENT,@RESULT,@COORDINATOR,@COORDINATETIME)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ORDERID", orderId);
                dbOperator.AddParameter("BUSINESSTYPE", (byte)coordination.Type);
                dbOperator.AddParameter("COTACTMODE", (byte)coordination.Mode);
                dbOperator.AddParameter("CONTENT", coordination.Content ?? string.Empty);
                dbOperator.AddParameter("RESULT", coordination.Result ?? string.Empty);
                dbOperator.AddParameter("COORDINATOR", coordination.Account ?? string.Empty);
                dbOperator.AddParameter("COORDINATETIME", coordination.Time);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Save(decimal orderId, decimal applyformId, Domain.Coordination coordination) {
            var sql = "INSERT INTO [dbo].[T_Coordination] ([OrderId],[ApplyformId],[BusinessType],[CotactMode],[Content],[Result],[Coordinator],[CoordinateTime])" +
                " VALUES (@ORDERID,@APPLYFORMID,@BUSINESSTYPE,@COTACTMODE,@CONTENT,@RESULT,@COORDINATOR,@COORDINATETIME)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ORDERID", orderId);
                dbOperator.AddParameter("APPLYFORMID", applyformId);
                dbOperator.AddParameter("BUSINESSTYPE", (byte)coordination.Type);
                dbOperator.AddParameter("COTACTMODE", (byte)coordination.Mode);
                dbOperator.AddParameter("CONTENT", coordination.Content ?? string.Empty);
                dbOperator.AddParameter("RESULT", coordination.Result ?? string.Empty);
                dbOperator.AddParameter("COORDINATOR", coordination.Account ?? string.Empty);
                dbOperator.AddParameter("COORDINATETIME", coordination.Time);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<Domain.Coordination> QueryByOrderId(decimal orderId) {
            var result = new List<Domain.Coordination>();
            string sql = "SELECT [BusinessType],[CotactMode],[Content],[Result],[Coordinator],[CoordinateTime] FROM [dbo].[T_Coordination] WHERE [OrderId]=@ORDERID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ORDERID", orderId);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new Domain.Coordination(
                            reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            (Domain.BusinessType)reader.GetByte(0),
                            (Domain.ContactMode)reader.GetByte(1),
                            reader.GetDateTime(5),OrderRole.Platform
                            ));
                    }
                }
            }
            return result;
        }

        public IEnumerable<Domain.Coordination> QueryByApplyformId(decimal applyformId) {
            var result = new List<Domain.Coordination>();
            string sql = "SELECT [BusinessType],[CotactMode],[Content],[Result],[Coordinator],[CoordinateTime] FROM [dbo].[T_Coordination] WHERE [ApplyformId]=@APPLYFORMID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("APPLYFORMID", applyformId);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new Domain.Coordination(
                            reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            (Domain.BusinessType)reader.GetByte(0),
                            (Domain.ContactMode)reader.GetByte(1),
                            reader.GetDateTime(5),OrderRole.Platform
                            ));
                    }
                }
            }
            return result;
        }

        #endregion

        public EmergentOrder QueryEmergentOrder(decimal id, DataTransferObject.Order.OrderStatus status)
        {
            EmergentOrder emergentOrder = new EmergentOrder();
            string sql = "select Id,[Type],Content,[Time],Account from T_EmergentOrder WHERE Id =@Id AND [Type] = @Type";
            using (DbOperator dboperator = new DbOperator(Provider,ConnectionString))
            {
                dboperator.AddParameter("Id",id);
                dboperator.AddParameter("Type",status);
                using (System.Data.Common.DbDataReader reader = dboperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        emergentOrder.Id = reader.GetDecimal(0);
                        emergentOrder.Type = (DataTransferObject.Order.OrderStatus)reader.GetInt32(1);
                        emergentOrder.Content = reader.GetString(2);
                        emergentOrder.Time = reader.GetDateTime(3);
                        emergentOrder.Account = reader.GetString(4);
                    }
                }
            }
            return emergentOrder;
        }


        public void SvaeEmergentOrder(EmergentOrder emergentOrder)
        {
            string sql = "IF EXISTS(SELECT NULL FROM T_EmergentOrder WHERE Id=@Id AND [Type]=@Type)" +
               " UPDATE T_EmergentOrder SET Content = @Text, [Time] = @Time, Account=@Account,[OrderIdType] =@OrderIdType WHERE Id=@Id AND [Type]=@Type;ELSE" +
               " INSERT INTO T_EmergentOrder (Id,[Type],Content,[Time],Account,[OrderIdType]) VALUES(@Id,@Type,@Text,@Time,@Account,@OrderIdType);"+ 
               "UPDATE T_Order SET [IsEmergentOrder] = 1 WHERE Id=@OrderId;";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", emergentOrder.Id);
                dbOperator.AddParameter("Type", emergentOrder.Type);
                dbOperator.AddParameter("Text", emergentOrder.Content);
                dbOperator.AddParameter("Time", emergentOrder.Time);
                dbOperator.AddParameter("Account",emergentOrder.Account);
                dbOperator.AddParameter("OrderIdType", emergentOrder.OrderIdTypeValue);
                dbOperator.AddParameter("OrderId", emergentOrder.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Dictionary<string, string> QueryAccountNames(IEnumerable<string> accounts) {
            string sql = string.Format("select [login],name from T_Employee where [Login] in ('{0}');",string.Join("','",accounts));
            var result = new Dictionary<string, string>();
            using (DbOperator dboperator = new DbOperator(Provider, ConnectionString))
            {
                using (System.Data.Common.DbDataReader reader = dboperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0), reader.GetString(1));
                    }
                }
            }
            return result;
        }


        public EmergentOrder QueryEmergentOrder(decimal id,  OrderIdType orderIdType)
        {
            string sql = "SELECT Id,[Type],Content,[Time],Account,OrderIdType FROM T_EmergentOrder WHERE Id=@Id AND OrderIdType =@OrderIdType";
            EmergentOrder result = null;
            using (DbOperator dboperator = new DbOperator(Provider, ConnectionString))
            {
                dboperator.AddParameter("Id",id);
                dboperator.AddParameter("OrderIdType", orderIdType);
                using (System.Data.Common.DbDataReader reader = dboperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new EmergentOrder();
                        result.Id = reader.GetDecimal(0);
                        result.Type = (OrderStatus)reader.GetInt32(1);
                        result.Content = reader.GetString(2);
                        result.Time = reader.GetDateTime(3);
                        result.Account = reader.GetString(4);
                        result.OrderIdTypeValue = (OrderIdType)reader.GetByte(5);
                    }
                }
            }
            return result;
        }
    }
}
