using System;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class RegisterIpRepository :SqlServerRepository,IRegisterIpRepository
    {
        public RegisterIpRepository(string connectionString):
            base(connectionString) {
        }

        public int Insert(Domain.RegisterIP registerIp)
        {
            string sql = @"INSERT INTO [dbo].[T_RegisterIP] ([IP],[Number],[RegisterDate]) VALUES (@IP ,@Number,@RegisterDate)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("IP",registerIp.IP);
                dbOperator.AddParameter("Number",registerIp.Number);
                dbOperator.AddParameter("RegisterDate",registerIp.RegisterDate.Date);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Domain.RegisterIP Query(string ip) {
            var result = new Domain.RegisterIP();
            string sql = @"SELECT [IP],[Number],[RegisterDate] FROM [dbo].[T_RegisterIP] WHERE [IP]=@IP";
            using (var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("IP",ip);
                using (var reader = dbOperator.ExecuteReader(sql)) {
                    while (reader.Read()) {
                        result.IP = ip;
                        result.Number = reader.GetInt32(1);
                        result.RegisterDate = reader.GetDateTime(2);
                    }
                }
            }
            return result;
        }

        public int Update(Domain.RegisterIP registerIp) {
            string sql = @"UPDATE [dbo].[T_RegisterIP] SET [Number] = @Number ,[RegisterDate] = @RegisterDate WHERE [IP] = @IP";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("IP",registerIp.IP);
                dbOperator.AddParameter("Number",registerIp.Number);
                dbOperator.AddParameter("RegisterDate",registerIp.RegisterDate.Date);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
