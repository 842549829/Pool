using System;
using System.Collections.Generic;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer {
    class EmployeeRepository : SqlServerRepository, IEmployeeRepository {
        public EmployeeRepository(string connectionString)
            : base(connectionString) {
        }
        public IEnumerable<EmployeeListView> QueryEmployeeInfo(EmployeeQueryParameter condition, Pagination pagination) {
            var result = new List<EmployeeListView>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(!string.IsNullOrEmpty(condition.Name)) dbOperator.AddParameter("@iName", condition.Name);
                if(!string.IsNullOrEmpty(condition.Login)) dbOperator.AddParameter("@iUserName", condition.Login);
                if(condition.Enabled.HasValue) dbOperator.AddParameter("@iEnabled", condition.Enabled.Value);
                if (!string.IsNullOrWhiteSpace(condition.IpLimitation)) dbOperator.AddParameter("@iIpLimitation",condition.IpLimitation);
                dbOperator.AddParameter("@iOwner", condition.Owner);
                dbOperator.AddParameter("@iPageSize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using(var reader = dbOperator.ExecuteReader("[dbo].[P_QueryEmployees]", System.Data.CommandType.StoredProcedure)) {
                    while(reader.Read()) {
                        var info = new EmployeeListView();
                        info.Name = reader.GetString(0);
                        info.Gender = (Common.Enums.Gender)reader.GetByte(1);
                        info.UserName = reader.GetString(2);
                        info.RoleName = reader.GetString(3);
                        info.Email = reader.GetString(4);
                        info.Cellphone = reader.GetString(5);
                        info.Enabled = reader.GetBoolean(6);
                        info.IsAdministrator = reader.GetBoolean(7);
                        info.Id = reader.GetGuid(8);
                        info.Remark = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        info.IpLimitation = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                        result.Add(info);
                    }
                }
                if(pagination.GetRowCount) {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }

        public bool ExistsUserName(string userName) {
            string sql = "SELECT COUNT(0) FROM T_Employee WHERE Login=@UserName";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("UserName", userName);
                var result = (int)dbOperator.ExecuteScalar(sql);
                return result >= 1;
            }
        }

        public EmployeeDetailInfo QueryEmployee(System.Guid id) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                return queryEmployee(dbOperator, "Id=@Id");
            }
        }

        public EmployeeDetailInfo QueryEmployee(string userName) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("UserName", userName);
                return queryEmployee(dbOperator, "[Login]=@UserName");
            }
        }

        public Employee QueryCompanyAdmin(Guid companyId) {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Owner", companyId);
                var employee = queryEmployee(dbOperator, "[Owner]=@Owner and IsAdministrator = 1");
                var result = new Employee
                {
                    Id = employee.Id,
                    Owner = employee.Owner,
                    Name = employee.Name,
                    Gender = employee.Gender,
                    Login = employee.UserName,
                    Password = employee.UserPassword,
                    Cellphone = employee.Cellphone,
                    OfficePhone = employee.OfficePhone,
                    Email = employee.Email,
                    Enabled = employee.Enabled,
                    Remark = employee.Remark,
                    IsAdministrator = employee.IsAdministrator,
                    LastLoginTime = employee.LastLoginTime,
                    LastLoginIP = employee.LastLoginIP,
                    LastLoginLocation = employee.LastLoginLocation,
                    CreateTime = employee.CreateTime
                };
                return result;
            }

        }

        EmployeeDetailInfo queryEmployee(DbOperator dbOperator, string condition) {
            EmployeeDetailInfo result = null;
            var sql = @"SELECT Id,[Owner],Name,[Gender],[Login],[Password],[Cellphone],[OfficePhone],[Email],[Enabled],[Remark],[IsAdministrator]," +
                      "[LastLoginTime],[LastLoginIP],[LastLoginLocation],[CreateTime],[IpLimitation] FROM dbo.T_Employee WHERE " + condition;
            using(var reader = dbOperator.ExecuteReader(sql)) {
                if(reader.Read()) {
                    result = new EmployeeDetailInfo {
                        Id = reader.GetGuid(0),
                        Owner = reader.GetGuid(1),
                        Name = reader.GetString(2),
                        Gender = (Common.Enums.Gender)reader.GetByte(3),
                        UserName = reader.GetString(4),
                        UserPassword = reader.GetString(5),
                        ConfirmPassword = reader.GetString(5),
                        Cellphone = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                        OfficePhone = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                        Email = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                        Enabled = reader.GetBoolean(9),
                        Remark = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                        IsAdministrator = reader.GetBoolean(11),
                        LastLoginIP = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                        LastLoginLocation = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                        CreateTime = reader.GetDateTime(15),
                        IpLimitation = reader.IsDBNull(16)?string.Empty:reader.GetString(16)
                    };
                    if(!reader.IsDBNull(12)) {
                        result.LastLoginTime = reader.GetDateTime(12);
                    }
                }
            }
            return result;
        }


        public int SetAllIpLimitation(Guid companyId,string ipLimitation)
        {
            string sql = "UPDATE dbo.T_Employee SET IpLimitation=@IpLimitation WHERE [Owner]=@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("IpLimitation",ipLimitation);
                dbOperator.AddParameter("CompanyId",companyId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int SetSingleIpLimitation(Guid employeeId, string ipLimitation)
        {
            string sql = "UPDATE dbo.T_Employee SET IpLimitation=@IpLimitation WHERE [Id]=@EmployeeId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("IpLimitation", ipLimitation);
                dbOperator.AddParameter("EmployeeId", employeeId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}