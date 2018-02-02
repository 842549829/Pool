using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.DataAccess;
using System.Data;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class CompanyDocumentRepository:SqlServerRepository,ICompanyDocmentRepository
    {
        public CompanyDocumentRepository(string connectionString)
            :base(connectionString){
        }

        public int Save(CompanyDocument document)
        {
            string sql = @"IF NOT EXISTS(SELECT NULL FROM [dbo].[T_CompanyDocument] WHERE Company =@Company) INSERT INTO [dbo].[T_CompanyDocument]([Company],[BussinessLicense] ,
                          [IATALicense],[CertLicense],[BussinessTime]) VALUES(@Company,@BussinessLicense ,@IATALicense,@CertLicense,@BussinessTime) ELSE UPDATE [dbo].[T_CompanyDocument]
                           SET [BussinessLicense] = @BussinessLicense ,[IATALicense] = @IATALicense,[CertLicense] = @CertLicense,[BussinessTime] = @BussinessTime
                           WHERE  [Company] = @Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company",document.Company);
                if (document.BussinessLicense != null && document.BussinessLicense.Length > 0 ){
                    dbOperator.AddParameter("BussinessLicense",document.BussinessLicense);
                } else{
                    dbOperator.AddParameter("BussinessLicense", DBNull.Value, DbType.Binary);
                }
                if ( document.IATALicense != null && document.IATALicense.Length > 0 ){
                    dbOperator.AddParameter("IATALicense", document.IATALicense);
                } else {
                    dbOperator.AddParameter("IATALicense", DBNull.Value, DbType.Binary);
                }
                if ( document.CertLicense != null && document.CertLicense.Length > 0 ){
                    dbOperator.AddParameter("CertLicense", document.CertLicense);
                } else {
                    dbOperator.AddParameter("CertLicense", DBNull.Value, DbType.Binary);
                }
                if (document.BussinessTime.HasValue) {
                    dbOperator.AddParameter("BussinessTime", document.BussinessTime);
                } else {
                    dbOperator.AddParameter("BussinessTime",DBNull.Value);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public CompanyDocument Query(Guid companyId)
        {
            CompanyDocument document = null;
            string sql = @"SELECT [Company],[BussinessLicense] ,[IATALicense],[CertLicense],[BussinessTime] FROM [dbo].[T_CompanyDocument] WHERE Company = @Company";
            using(var dbOperator = new DbOperator(Provider,ConnectionString)){
                dbOperator.AddParameter("Company",companyId);
                using (var reader = dbOperator.ExecuteReader(sql)) {
                    while (reader.Read()) {
                        document = new CompanyDocument();
                        document.Company = reader.GetGuid(0);
                        if (!reader.IsDBNull(1)){
                            var length = reader.GetBytes(1, 0, null, 0, 0);
                            byte[] buffer = new byte[length];
                            reader.GetBytes(1, 0, buffer, 0, (int)length);
                            document.BussinessLicense = buffer;
                        }
                        if (!reader.IsDBNull(2)){
                            var length = reader.GetBytes(2, 0, null, 0, 0);
                            byte[] buffer = new byte[length];
                            reader.GetBytes(2, 0, buffer, 0, (int)length);
                            document.IATALicense = buffer;
                        }
                        if (!reader.IsDBNull(3)) {
                            var length = reader.GetBytes(3, 0, null, 0, 0);
                            byte[] buffer = new byte[length];
                            reader.GetBytes(3, 0, buffer, 0, (int)length);
                            document.CertLicense = buffer;
                        }
                        if (!reader.IsDBNull(4)){
                            document.BussinessTime = reader.GetInt32(4);
                        }
                    }
                }
            }
            return document;
        }

        public int Delete(Guid companyId)
        {
            string sql = "DELETE FROM dbo.T_CompanyDocument WHERE Company = @Company";
            using (var dbOperator = new DbOperator(Provider,ConnectionString))
            {
                dbOperator.AddParameter("Company",companyId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
