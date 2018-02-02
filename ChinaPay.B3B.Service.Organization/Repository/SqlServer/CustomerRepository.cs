using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer {
    using Common.Enums;

    class CustomerRepository : SqlServerRepository, ICustomerRepository {
        public CustomerRepository(string connectionString)
            : base(connectionString) {
        }

        public int Save(Guid companyId, Customer customer) {
            var sql = "IF NOT EXISTS(SELECT NULL FROM dbo.T_Customer WHERE Company=@Company AND Name=@Name AND CredentialsType=@CredentialsType) INSERT INTO dbo.T_Customer (Id,Name,Sex,PassengerType,CredentialsType,Credentials,Mobile,Remark,Company,[Time]) " +
                         "VALUES(@Id,@Name,@Sex,@PassengerType,@CredentialsType,@Credentials,@Mobile,@Remark,@Company,@Time)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", companyId);
                dbOperator.AddParameter("Id", customer.Id);
                dbOperator.AddParameter("Name", customer.Name);
                if(customer.Sex.HasValue) {
                    dbOperator.AddParameter("Sex", (byte)customer.Sex);
                } else {
                    dbOperator.AddParameter("Sex", DBNull.Value);
                }
                dbOperator.AddParameter("CredentialsType", (byte)customer.CredentialsType);
                dbOperator.AddParameter("Credentials", customer.Credentials);
                if(string.IsNullOrWhiteSpace(customer.Mobile)) {
                    dbOperator.AddParameter("Mobile", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Mobile", customer.Mobile);
                }
                if(string.IsNullOrWhiteSpace(customer.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", customer.Remark);
                }
                dbOperator.AddParameter("PassengerType", (byte)customer.PassengerType);
                dbOperator.AddParameter("Time", DateTime.Now);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Save(Guid companyId, IEnumerable<Customer> customers) {
            if(!customers.Any()) return 0;
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.BeginTransaction();
                try {
                    foreach(var item in customers) {
                        string sql = "IF NOT EXISTS(SELECT NULL FROM dbo.T_Customer WHERE Company=@Company AND Name=@Name AND CredentialsType=@CredentialsType) " +
                            "INSERT INTO dbo.T_Customer (Id,Name,Sex,PassengerType,CredentialsType,Credentials,Mobile,Remark,Company,[Time]) " +
                            "VALUES(@Id,@Name,@Sex,@PassengerType,@CredentialsType,@Credentials,@Mobile,@Remark,@Company,@Time)";
                        dbOperator.ClearParameters();
                        dbOperator.AddParameter("Company", companyId);
                        dbOperator.AddParameter("Id", item.Id);
                        dbOperator.AddParameter("Name", item.Name);
                        if(item.Sex.HasValue) {
                            dbOperator.AddParameter("Sex", (byte)item.Sex);
                        } else {
                            dbOperator.AddParameter("Sex", DBNull.Value);
                        }
                        dbOperator.AddParameter("CredentialsType", (byte)item.CredentialsType);
                        dbOperator.AddParameter("Credentials", item.Credentials);
                        if(string.IsNullOrWhiteSpace(item.Mobile)) {
                            dbOperator.AddParameter("Mobile", DBNull.Value);
                        } else {
                            dbOperator.AddParameter("Mobile", item.Mobile);
                        }
                        if(string.IsNullOrWhiteSpace(item.Remark)) {
                            dbOperator.AddParameter("Remark", DBNull.Value);
                        } else {
                            dbOperator.AddParameter("Remark", item.Remark);
                        }
                        dbOperator.AddParameter("PassengerType", (byte)item.PassengerType);
                        dbOperator.AddParameter("Time", DateTime.Now);
                        dbOperator.ExecuteNonQuery(sql);
                    }
                    dbOperator.CommitTransaction();
                    return customers.Count();
                } catch {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }

        public int Update(Guid companyId, Customer customer) {
            string sql = "UPDATE dbo.T_Customer SET Name=@Name,Sex=@Sex,PassengerType=@PassengerType,CredentialsType=@CredentialsType,Credentials=@Credentials,Mobile=@Mobile,Remark=@Remark" +
                " WHERE Id=@Id AND Company=@Company";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", companyId);
                dbOperator.AddParameter("Id", customer.Id);
                if(string.IsNullOrWhiteSpace(customer.Name)) {
                    dbOperator.AddParameter("Name", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Name", customer.Name);
                }
                if(customer.Sex.HasValue) {
                    dbOperator.AddParameter("Sex", (byte)customer.Sex);
                } else {
                    dbOperator.AddParameter("Sex", DBNull.Value);
                }
                dbOperator.AddParameter("PassengerType", (byte)customer.PassengerType);
                dbOperator.AddParameter("CredentialsType", (byte)customer.CredentialsType);
                dbOperator.AddParameter("Credentials", customer.Credentials);
                if(string.IsNullOrWhiteSpace(customer.Mobile)) {
                    dbOperator.AddParameter("Mobile", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Mobile", customer.Mobile);
                }
                if(string.IsNullOrWhiteSpace(customer.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", customer.Remark);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(Guid companyId, Guid customer) {
            string sql = "DELETE FROM dbo.T_Customer WHERE Id=@ID AND Company=@COMPANY";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("COMPANY", companyId);
                dbOperator.AddParameter("ID", customer);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<Customer> Query(CustomerQueryCondition condition, Pagination pagination) {
            var result = new List<Customer>();
            var fields = "Id,Name,Sex,PassengerType,CredentialsType,Credentials,Mobile,Remark,Time";
            var catelog = "T_Customer";
            var orderby = "Time DESC";
            var where = new StringBuilder();
            where.AppendFormat("Company='{0}'", condition.Company);
            if(!string.IsNullOrWhiteSpace(condition.Name)) {
                where.AppendFormat(" AND Name LIKE N'%{0}%'", condition.Name.Trim());
            }
            if(!string.IsNullOrWhiteSpace(condition.Mobile)) {
                where.AppendFormat(" AND Mobile=N'{0}'", condition.Mobile.Trim());
            }
            if(condition.CredentialsType.HasValue) {
                where.AppendFormat(" AND CredentialsType={0}", (byte)condition.CredentialsType);
            }
            if(!string.IsNullOrWhiteSpace(condition.Credentials)) {
                where.AppendFormat(" AND Credentials=N'{0}'", condition.Credentials.Trim());
            }
            if(condition.Sex.HasValue) {
                where.AppendFormat(" AND Sex={0}", (byte)condition.Sex);
            }
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("@iField", fields);
                dbOperator.AddParameter("@iCatelog", catelog);
                dbOperator.AddParameter("@iCondition", where.ToString());
                dbOperator.AddParameter("@iOrderBy", orderby);
                dbOperator.AddParameter("@iPagesize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                dbOperator.AddParameter("@iGetCount", pagination.GetRowCount);
                var totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using(var reader = dbOperator.ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure)) {
                    while(reader.Read()) {
                        Customer item = new Customer(reader.GetGuid(0));
                        item.Name = reader.GetString(1);
                        if(!reader.IsDBNull(2)) {
                            item.Sex = (Core.Sex)reader.GetByte(2);
                        }
                        item.PassengerType = (PassengerType)reader.GetByte(3);
                        item.CredentialsType = (CredentialsType)reader.GetByte(4);
                        item.Credentials = reader.GetString(5);
                        item.Mobile = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        result.Add(item);
                    }
                }
                if(pagination.GetRowCount) {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }

        public Customer Query(Guid customerId) {
            Customer customer = null;
            string sql = string.Format(@"SELECT Id,Name,Sex,PassengerType,CredentialsType,Credentials,Mobile,Remark FROM dbo.T_Customer WHERE Id = '{0}'", customerId);
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        customer = new Customer(reader.GetGuid(0));
                        customer.Name = reader.GetString(1);
                        if(!reader.IsDBNull(2)) {
                            customer.Sex = (Core.Sex)reader.GetByte(2);
                        }
                        customer.PassengerType = (PassengerType)reader.GetByte(3);
                        customer.CredentialsType = (CredentialsType)reader.GetByte(4);
                        customer.Credentials = reader.GetString(5);
                        customer.Mobile = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        customer.Remark = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                    }
                }
            }
            return customer;
        }
    }
}