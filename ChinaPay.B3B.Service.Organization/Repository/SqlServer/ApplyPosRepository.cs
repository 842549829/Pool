using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
     class ApplyPosRepository : SqlServerRepository,IApplyPosRepository
    {

         public ApplyPosRepository(string connectionString) :
             base(connectionString){

         }

        public int Save(Guid companyId, bool isNeedApply)
        {
            string sql = @"INSERT INTO [dbo].[T_ApplyPos]([Company] ,[IsNeedApply]) VALUES(@Company,@IsNeedApply)";
            using (var dbOperator = new DbOperator(Provider,ConnectionString))
            {
                dbOperator.AddParameter("Company",companyId);
                dbOperator.AddParameter("IsNeedApply",isNeedApply);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
