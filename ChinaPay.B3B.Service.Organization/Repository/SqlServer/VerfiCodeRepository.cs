using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class VerfiCodeRepository :SqlServerRepository,IVerfiCodeRepository
    {
        public VerfiCodeRepository(string connectionString)
            :base(connectionString)
        {
        }

        public int Insert(Domain.VerfiCode verfiCode)
        {
            string sql = @"INSERT INTO [dbo].[T_VerifyCode] ([Id],[AccountNo],[IP],[CellPhone],[Type],[Code],[SendTime])
                         VALUES(@Id,@AccountNo,@IP ,@CellPhone,@Type,@Code,@SendTime)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id",verfiCode.Id);
                dbOperator.AddParameter("AccountNo",verfiCode.AccountNo);
                dbOperator.AddParameter("IP",verfiCode.IP);
                dbOperator.AddParameter("CellPhone",verfiCode.CellPhone);
                dbOperator.AddParameter("Type",(byte)verfiCode.Type);
                dbOperator.AddParameter("Code",verfiCode.Code);
                dbOperator.AddParameter("SendTime",verfiCode.SendTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
