using System;
using System.Collections.Generic;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Policy.Repository.SqlServer
{
    class SuspendInfoRepository : SqlServerTransaction, ISuspendInfoRepository
    {
        public SuspendInfoRepository(ChinaPay.DataAccess.DbOperator dbOperator)
            : base(dbOperator)
        {
        }
        public DataTransferObject.Policy.SuspendInfo GetSuspendInfo(Guid companyId)
        {
            string sql = "SELECT [Company] ,[Airline] ,[SuspendedByPlatform] FROM [dbo].[T_SuspendedPolicy] WHERE Company='" + companyId + "'" ;
            using (var reader = ExecuteReader(sql))
            { 
                List<string> SuspendByCompany = new List<string>();
                List<string> SuspendByPlatform = new List<string>();
                while (reader.Read())
                {
                    if (reader.GetBoolean(2))
                    {
                        SuspendByPlatform.Add(reader.GetString(1));
                    }
                    if (!reader.GetBoolean(2))
                    {
                        SuspendByCompany.Add(reader.GetString(1));
                    }
                }
                return new DataTransferObject.Policy.SuspendInfo { SuspendByCompany = SuspendByCompany, SuspendByPlatform = SuspendByPlatform };
            } 
        }
    }
}
