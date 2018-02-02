using System;
using ChinaPay.B3B.Service.PidManagement.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.PidManagement.Repository.SqlServer
{
    public class PidManagementRepository : SqlServerRepository, IPidManagementRepository
    {
        public PidManagementRepository(string connectionString)
            : base(connectionString) { }

        public PidUsingInformation Query(Guid userId)
        {
            PidUsingInformation pidUsingInfo = null;
            const string sql = @"SELECT UserId, FromDate , ThruDate, Total FROM T_PidUsingInfo " +
                               @"WHERE UserId = @UserId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("UserId", userId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        var id = reader.GetGuid(0);
                        var fromDate = reader.GetDateTime(1);
                        var thruDate = reader.GetDateTime(2);
                        var total = reader.GetInt64(3);
                        pidUsingInfo = new PidUsingInformation{
                                               UserId = id,
                                               FromDate = fromDate,
                                               ThruDate = thruDate,
                                               Total = total
                                           };
                    }
                }
            }
            return pidUsingInfo;
        }

        public int Insert(PidUsingInformation pidUsingInfo)
        {
            const string sql = @"INSERT INTO dbo.T_PidUsingInfo(UserId, FromDate, ThruDate, Total) " + 
                               "VALUES  (@UserId, @FromDate, @ThruDate, @Total)";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("UserId", pidUsingInfo.UserId);
                dbOperator.AddParameter("FromDate", pidUsingInfo.FromDate);
                dbOperator.AddParameter("ThruDate", pidUsingInfo.ThruDate);
                dbOperator.AddParameter("Total", pidUsingInfo.Total);

                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(PidUsingInformation pidUsingInfo)
        {
            const string sql = @"UPDATE dbo.T_PidUsingInfo " +
                               "SET FromDate = @FromDate, ThruDate = @ThruDate, Total = @Total " +
                               "WHERE UserId = @UserId; ";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("UserId", pidUsingInfo.UserId);
                dbOperator.AddParameter("FromDate", pidUsingInfo.FromDate);
                dbOperator.AddParameter("ThruDate", pidUsingInfo.ThruDate);
                dbOperator.AddParameter("Total", pidUsingInfo.Total);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Save(Guid userId, bool isUseB3BConfig)
        {
            const string sql = @"MERGE dbo.T_PidUsingInfo AS target " +
                               @"USING (VALUES(@UserId, GETDATE(), @IsUseB3BConfig)) AS source (UserId, FromDate, IsUseB3BConfig) " +
                               @"ON (TARGET.UserId = SOURCE.UserId AND TARGET.IsUseB3BConfig = SOURCE.IsUseB3BConfig AND DATEDIFF(MONTH, TARGET.FromDate, SOURCE.FromDate) = 0) " +
                               @"WHEN MATCHED THEN " +
                               @"UPDATE set total = total + 1 " +
                               @"WHEN NOT MATCHED THEN  " +
                               @"INSERT (UserId, FromDate, IsUseB3BConfig, Total) VALUES(UserId, FromDate, IsUseB3BConfig, 1);";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("UserId", userId);
                dbOperator.AddParameter("IsUseB3BConfig", isUseB3BConfig);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
