using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer {
    using Common.Enums;

    class AccountRepository : SqlServerRepository, IAccountRepository {
        public AccountRepository(string connectionString)
            : base(connectionString) {
        }

        public int Save(Guid companyId, Account item) {
            string sql = "IF EXISTS(SELECT NULL FROM [T_Account] WHERE [Company]=@COMPANY AND [Type]=@TYPE)" +
                " UPDATE [T_Account] SET [Account]=@ACCOUNT,[Valid]=@VALID,[Time]=@TIME WHERE [Company]=@COMPANY AND [Type]=@TYPE;ELSE " +
                        "INSERT INTO [T_Account]([Company],[Account],[Type],[Valid],[Time]) VALUES(@COMPANY,@ACCOUNT,@TYPE,@VALID,@TIME);";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ACCOUNT", item.No);
                dbOperator.AddParameter("COMPANY", companyId);
                dbOperator.AddParameter("TYPE", item.Type);
                dbOperator.AddParameter("VALID", item.Valid);
                dbOperator.AddParameter("TIME", item.Time);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateStatus(Guid companyId, AccountType accountType, bool enabled) {
            string sql = "UPDATE [T_Account] SET [Valid]=@VALID WHERE [Company]=@COMPANY AND [Type]=@TYPE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("VALID", enabled);
                dbOperator.AddParameter("COMPANY", companyId);
                dbOperator.AddParameter("TYPE", accountType);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Account Query(Guid companyId, AccountType accountType) {
            Account result = null;
            string sql = "SELECT [Account],[Valid],[Time] FROM [T_Account] WHERE [Company]=@COMPANY AND [Type]=@TYPE";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("COMPANY", companyId);
                dbOperator.AddParameter("TYPE", accountType);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result = new Account(reader.GetString(0), accountType, reader.GetBoolean(1), reader.GetDateTime(2));
                    }
                }
            }
            return result;
        }

        public IEnumerable<Account> Query(Guid companyId) {
            var result = new List<Account>();
            string sql = "SELECT [Account],[Valid],[Type],[Time] FROM [T_Account] WHERE [Company]=@COMPANY";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("COMPANY", companyId);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var account = new Account(reader.GetString(0), (AccountType)reader.GetInt32(2), reader.GetBoolean(1), reader.GetDateTime(3));
                        result.Add(account);
                    }
                }
            }
            return result;
        }

        public IEnumerable<Account> GetAllValidAccount(AccountType accountType)
        {
            var sql = "SELECT  [Company],[Account],[Valid],[Type],[Time] FROM T_Account WHERE Valid=1 AND Type=@Type";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Type", accountType);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        yield return new Account(reader.GetGuid(0), reader.GetString(1), (AccountType)reader.GetInt32(3), reader.GetBoolean(2), reader.GetDateTime(4));
                    }
                }
            }
        }


        public IEnumerable<Account> Query(string accountNo)
        {
            var result = new List<Account>();
            string sql = "SELECT [Account],[Valid],[Type],[Time],[Company] FROM [T_Account] WHERE [Account]=@Account";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Account", accountNo);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var account = new Account(reader.GetGuid(4),reader.GetString(0), (AccountType)reader.GetInt32(2), reader.GetBoolean(1), reader.GetDateTime(3));
                        result.Add(account);
                    }
                }
            }
            return result;
        }


        public IEnumerable<DataTransferObject.Organization.AccountDetailInfo> QueryAccount(Pagination pagination, DataTransferObject.Organization.PaymentsAccountQueryCondition condition)
        {
            IList<DataTransferObject.Organization.AccountDetailInfo> result = new List<DataTransferObject.Organization.AccountDetailInfo>();
            using (var dbOperator = new DbOperator(Provider,ConnectionString))
            {
                if(!string.IsNullOrEmpty(condition.AbbreviateName)) dbOperator.AddParameter("@iAbbreviateName", condition.AbbreviateName);
                if(!string.IsNullOrEmpty(condition.PaymentAccount)) dbOperator.AddParameter("@iPaymentAccount", condition.PaymentAccount);
                if(condition.Enabled.HasValue) dbOperator.AddParameter("@iEnabled", condition.Enabled.Value);
                if(!string.IsNullOrEmpty(condition.UserName)) dbOperator.AddParameter("@iAdministrator", condition.UserName);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                    dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.[P_QueryAccounts]", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var info = new DataTransferObject.Organization.AccountDetailInfo();
                        info.CompanyId = reader.GetGuid(0);
                        info.Account = reader.GetString(1);
                        info.CreateTime = reader.GetDateTime(2);
                        info.AccountType = (AccountType)reader.GetInt32(3);
                        info.Enabled = reader.GetBoolean(4);
                        info.AbbreviateName = reader.GetString(5);
                        info.CompanyType = (Common.Enums.CompanyType)reader.GetInt32(6);
                        info.Administrator = reader.GetString(7);
                        result.Add(info);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }
        /*缓存代码
        public int Delete(Account value)
        {
            throw new NotImplementedException();
        }

        public int Insert(Account value)
        {
            return Save(value.Company, value);
        }

        public IEnumerable<KeyValuePair<string, Account>> Query()
        {
            string sql = "SELECT  [Company],[Account],[Valid],[Type],[Time] FROM T_Account WHERE Valid=1";
            using (DbOperator dbOperator = new DbOperator(Provider,ConnectionString))
            {
                using (System.Data.Common.DbDataReader  reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        Guid companyId = reader.GetGuid(0);
                        string accountNo = reader.GetString(1);
                        bool valid = reader.GetBoolean(2);
                        AccountType type= (AccountType)reader.GetInt32(3);
                        DateTime time = reader.GetDateTime(4);
                        Account account = new Account(companyId,accountNo,type,valid,time);
                        yield return new KeyValuePair<string, Account>(companyId.ToString() + type.GetHashCode(),account);
                    }
                }
            }
        }

        public int Update(Account value)
        {
            return Save(value.Company, value);
        }*/


        public void SetWithholdingInfo(DataTransferObject.Organization.WithholdingView withholdingView)
        {
            string sql = "IF EXISTS(SELECT NULL FROM T_Withholding WHERE AccountType=@AccountType AND Company =@Company ) UPDATE T_Withholding SET [Time] = @Time,[Status]=@Status,Amount =@Amount,AccountNo=@AccountNo WHERE  AccountType=@AccountType AND Company =@Company ELSE INSERT INTO T_Withholding(AccountNo,AccountType,[Time],[Status],Amount,Company) Values(@AccountNo,@AccountType,@Time,@Status,@Amount,@Company) ";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("AccountNo", withholdingView.AccountNo);
                dbOperator.AddParameter("AccountType", withholdingView.AccountType);
                dbOperator.AddParameter("Time", withholdingView.Time);
                dbOperator.AddParameter("Status", withholdingView.Status);
                dbOperator.AddParameter("Amount", withholdingView.Amount);
                dbOperator.AddParameter("Company", withholdingView.Company);
                dbOperator.ExecuteNonQuery(sql);
            }
        }


        public DataTransferObject.Organization.WithholdingView GetWithholding(WithholdingAccountType accountType, Guid company)
        {
            DataTransferObject.Organization.WithholdingView withholding= null;
            //[AccountNo] = @AccountNo AND
            string sql = "SELECT [AccountNo],[AccountType],[Time],[Status],[Amount],[Company] FROM [T_Withholding] WHERE [AccountType]=@AccountType AND [Company] = @Company ";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString)) {
                //dbOperator.AddParameter("AccountNo", withholdingView.AccountNo);
                dbOperator.AddParameter("AccountType", accountType);
                dbOperator.AddParameter("Company", company);
                using (System.Data.Common.DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        withholding = new DataTransferObject.Organization.WithholdingView();
                        withholding.AccountNo = reader.GetString(0);
                        withholding.AccountType = (WithholdingAccountType)reader.GetByte(1);
                        withholding.Time = reader.GetDateTime(2);
                        withholding.Status = (WithholdingProtocolStatus)reader.GetByte(3);
                        withholding.Amount = reader.GetDecimal(4);
                        withholding.Company = reader.GetGuid(5);
                    }
                }
            }
            return withholding;
        }
    }
}
