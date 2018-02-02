using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Repository;
using ChinaPay.Core;
using System.Linq;
using ChinaPay.DataAccess;
using Company = ChinaPay.B3B.Data.DataMapping.Company;
using Relationship = ChinaPay.B3B.Data.DataMapping.Relationship;
using Time = Izual.Time;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    internal class CompanyRepository : SqlServerRepository, ICompanyRepository
    {
        public CompanyRepository(string connectionString)
            : base(connectionString) { }

        #region ICompanyRepository Members

        public IEnumerable<CompanyListInfo> QueryCompanies(CompanyQueryParameter condition, Pagination pagination)
        {
            var result = new List<CompanyListInfo>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (!string.IsNullOrWhiteSpace(condition.AbbreviateName))
                    dbOperator.AddParameter("i_AbbreviateName", condition.AbbreviateName);
                if (condition.Type.HasValue)
                    dbOperator.AddParameter("i_CompanyType", (byte)condition.Type);
                if (condition.AccountType.HasValue)
                    dbOperator.AddParameter("i_AccountType", (byte)condition.AccountType);
                if (!string.IsNullOrWhiteSpace(condition.Contact))
                    dbOperator.AddParameter("i_Contact", condition.Contact);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("i_UserNo", condition.UserNo);
                if (condition.Enabled.HasValue)
                    dbOperator.AddParameter("i_Enabled", condition.Enabled);
                if (condition.CompanyAuditStatus.HasValue)
                    dbOperator.AddParameter("i_CompanyAuditStatus", (byte)condition.CompanyAuditStatus);
                dbOperator.AddParameter("i_PageSize", pagination.PageSize);
                dbOperator.AddParameter("i_PageIndex", pagination.PageIndex);
                DbParameter totalCount = dbOperator.AddParameter("o_RowCount");
                totalCount.Direction = ParameterDirection.Output;
                totalCount.DbType = DbType.Int32;
                using (DbDataReader reader = dbOperator.ExecuteReader("dbo.P_CompanyListPagination", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var companyInfo = new CompanyListInfo();
                        if (!reader.IsDBNull(0))
                            companyInfo.CompanyId = reader.GetGuid(0);
                        if (!reader.IsDBNull(1))
                            companyInfo.UserNo = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            companyInfo.CompanyType = (CompanyType)reader.GetByte(2);
                        if (!reader.IsDBNull(3))
                            companyInfo.AccountType = (AccountBaseType)reader.GetByte(3);
                        if (!reader.IsDBNull(4))
                            companyInfo.AbbreviateName = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            companyInfo.Contact = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            companyInfo.Audited = reader.GetBoolean(6);
                        if (!reader.IsDBNull(7))
                            companyInfo.AuditTime = reader.GetDateTime(7);
                        if (!reader.IsDBNull(8))
                            companyInfo.Enabled = reader.GetBoolean(8);
                        if (!reader.IsDBNull(9))
                            companyInfo.LastLoginTime = reader.GetDateTime(9);
                        companyInfo.RegisterTime = reader.GetDateTime(10);
                        companyInfo.IsOem = reader.GetBoolean(11);
                        result.Add(companyInfo);
                    }
                }
                if (pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public IEnumerable<SpreadingView> QuerySpreadCompanies(SpreadingQueryParameter condition, Pagination pagination)
        {
            var result = new List<SpreadingView>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("i_Initiator", condition.Initiator);
                if (!string.IsNullOrWhiteSpace(condition.AbbreviateName))
                    dbOperator.AddParameter("i_AbbreviateName", condition.AbbreviateName);
                if (condition.Type.HasValue)
                    dbOperator.AddParameter("i_CompanyType", (byte)condition.Type);
                if (condition.AccountType.HasValue)
                    dbOperator.AddParameter("i_AccountType", (byte)condition.AccountType);
                if (!string.IsNullOrWhiteSpace(condition.Contact))
                    dbOperator.AddParameter("i_Contact", condition.Contact);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("i_UserNo", condition.UserNo);
                if (condition.Enabled.HasValue)
                    dbOperator.AddParameter("i_Enabled", condition.Enabled);
                if (!string.IsNullOrWhiteSpace(condition.OperatorAccount))
                    dbOperator.AddParameter("i_OperatorAccount", condition.OperatorAccount);
                if (condition.RegisterTimeStart.HasValue)
                    dbOperator.AddParameter("i_RegisterTimeStart", condition.RegisterTimeStart);
                if (condition.RegisterTimeEnd.HasValue)
                    dbOperator.AddParameter("i_RegisterTimeEnd", condition.RegisterTimeEnd);
                dbOperator.AddParameter("i_PageSize", pagination.PageSize);
                dbOperator.AddParameter("i_PageIndex", pagination.PageIndex);
                DbParameter totalCount = dbOperator.AddParameter("o_RowCount");
                totalCount.DbType = DbType.Int32;
                totalCount.Direction = ParameterDirection.Output;
                using (DbDataReader reader = dbOperator.ExecuteReader("dbo.P_SpreadCompanyListPagination", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var companyInfo = new SpreadingView();
                        companyInfo.Id = reader.GetGuid(0);
                        if (!reader.IsDBNull(1))
                            companyInfo.Admin = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            companyInfo.Type = (CompanyType)reader.GetByte(2);
                        if (!reader.IsDBNull(3))
                            companyInfo.AccountType = (AccountBaseType)reader.GetByte(3);
                        if (!reader.IsDBNull(4))
                            companyInfo.AbbreviateName = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            companyInfo.Contact = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            companyInfo.ContactCellphone = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            companyInfo.Enabled = reader.GetBoolean(7);
                        if (!reader.IsDBNull(8))
                            companyInfo.OperatorAccount = reader.GetString(8);
                        if (!reader.IsDBNull(9))
                            companyInfo.RegisterTime = reader.GetDateTime(9);
                        if (!reader.IsDBNull(10))
                            companyInfo.City = reader.GetString(10);
                        result.Add(companyInfo);
                    }
                }
                if (pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public IEnumerable<SubordinateCompanyListInfo> QuerySuordinateCompanies(SubordinateQueryParameter condition, Pagination pagination)
        {
            var result = new List<SubordinateCompanyListInfo>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("i_Superior", condition.Superior);
                if (!string.IsNullOrWhiteSpace(condition.CompanyName))
                    dbOperator.AddParameter("i_CompanyName", condition.CompanyName);
                if (!string.IsNullOrWhiteSpace(condition.AbbreviateName))
                    dbOperator.AddParameter("i_AbbreviateName", condition.AbbreviateName);
                if (condition.RelationshipType.HasValue)
                    dbOperator.AddParameter("i_RelationType", (byte)condition.RelationshipType);
                if (condition.AccountType.HasValue)
                    dbOperator.AddParameter("i_AccountType", (byte)condition.AccountType);
                if (!string.IsNullOrWhiteSpace(condition.Contact))
                    dbOperator.AddParameter("i_Contact", condition.Contact);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("i_UserNo", condition.UserNo);
                if (condition.Enabled.HasValue)
                    dbOperator.AddParameter("i_Enabled", condition.Enabled);
                if (condition.RegisterTimeStart.HasValue)
                    dbOperator.AddParameter("i_RegisterTimeStart", condition.RegisterTimeStart);
                if (condition.RegisterTimeEnd.HasValue)
                    dbOperator.AddParameter("i_RegisterTimeEnd", condition.RegisterTimeEnd);
                dbOperator.AddParameter("i_PageSize", pagination.PageSize);
                dbOperator.AddParameter("i_PageIndex", pagination.PageIndex);
                DbParameter totalCount = dbOperator.AddParameter("o_RowCount");
                totalCount.DbType = DbType.Int32;
                totalCount.Direction = ParameterDirection.Output;
                using (DbDataReader reader = dbOperator.ExecuteReader("dbo.P_SubordinateCompanyListPagination", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var companyInfo = new SubordinateCompanyListInfo();
                        companyInfo.CompanyId = reader.GetGuid(0);
                        if (!reader.IsDBNull(1))
                            companyInfo.UserNo = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            companyInfo.RelationshipType = (RelationshipType)reader.GetByte(2);
                        if (!reader.IsDBNull(3))
                            companyInfo.AccountType = (AccountBaseType)reader.GetByte(3);
                        if (!reader.IsDBNull(4))
                            companyInfo.AbbreviateName = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            companyInfo.Contact = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            companyInfo.Group = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            companyInfo.Enabled = reader.GetBoolean(7);
                        if (!reader.IsDBNull(8))
                            companyInfo.CompanyName = reader.GetString(8);
                        if (!reader.IsDBNull(9))
                            companyInfo.City = reader.GetString(9);
                        if (!reader.IsDBNull(10))
                            companyInfo.ContactPhone = reader.GetString(10);
                        if (!reader.IsDBNull(11))
                            companyInfo.Audtied = reader.GetBoolean(11);
                        if (!reader.IsDBNull(12))
                            companyInfo.AuditTime = reader.GetDateTime(12);
                        if (!reader.IsDBNull(13))
                            companyInfo.RegisterTime = reader.GetDateTime(13);
                        result.Add(companyInfo);
                    }
                }
                if (pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public IEnumerable<CompanyGroupMemberListInfo> QueryCompanyGroupMember(CompanyGroupMemberParameter condition, Pagination pagination)
        {
            var result = new List<CompanyGroupMemberListInfo>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("i_GroupId", condition.GroupId);
                if (!string.IsNullOrWhiteSpace(condition.CompanyName))
                    dbOperator.AddParameter("i_CompanyName", condition.CompanyName);
                if (!string.IsNullOrWhiteSpace(condition.Contact))
                    dbOperator.AddParameter("i_Contact", condition.Contact);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("i_UserNo", condition.UserNo);
                dbOperator.AddParameter("i_PageSize", pagination.PageSize);
                dbOperator.AddParameter("i_PageIndex", pagination.PageIndex);
                DbParameter totalCount = dbOperator.AddParameter("o_RowCount");
                totalCount.DbType = DbType.Int32;
                totalCount.Direction = ParameterDirection.Output;
                using (DbDataReader reader = dbOperator.ExecuteReader("dbo.P_CompanyGroupMemberListPagination", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var memberInfo = new CompanyGroupMemberListInfo();
                        if (!reader.IsDBNull(0))
                            memberInfo.CompanyId = reader.GetGuid(0);
                        if (!reader.IsDBNull(1))
                            memberInfo.CompanyName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            memberInfo.UserNo = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                            memberInfo.City = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                            memberInfo.Contact = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            memberInfo.ContactPhone = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            memberInfo.Group = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            memberInfo.RegisterTime = reader.GetDateTime(7);
                        result.Add(memberInfo);
                    }
                }
                if (pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public IEnumerable<CompanyGroupMemberListInfo> QueryMemberCanAdd(CompanyGroupMemberParameter condition, Pagination pagination)
        {
            var result = new List<CompanyGroupMemberListInfo>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("i_Superior", condition.Superior);
                if (!string.IsNullOrWhiteSpace(condition.CompanyName))
                    dbOperator.AddParameter("i_CompanyName", condition.CompanyName);
                if (!string.IsNullOrWhiteSpace(condition.Contact))
                    dbOperator.AddParameter("i_Contact", condition.Contact);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("i_UserNo", condition.UserNo);
                dbOperator.AddParameter("i_PageSize", pagination.PageSize);
                dbOperator.AddParameter("i_PageIndex", pagination.PageIndex);
                DbParameter totalCount = dbOperator.AddParameter("o_RowCount");
                totalCount.DbType = DbType.Int32;
                totalCount.Direction = ParameterDirection.Output;
                using (DbDataReader reader = dbOperator.ExecuteReader("dbo.P_GroupMemberCanAddPagination", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var member = new CompanyGroupMemberListInfo();
                        member.CompanyId = reader.GetGuid(0);
                        member.CompanyName = reader.GetString(1);
                        member.UserNo = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                            member.City = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                            member.Contact = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            member.ContactPhone = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            member.RegisterTime = reader.GetDateTime(6);
                        result.Add(member);
                    }
                }
                if (pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public CompanySettingsInfo QueryCompanySettingsInfo(Guid companyId)
        {
            string sql =
                "SELECT parameter.Company,setting.DefaultDeparture,setting.DefaultArrival,setting.AirlineForAdult,setting.AirlineForChild,setting.RebateForChild ,setting.RefundNeedAudit,setting.DefaultOfficeNumber,setting.IsImpower,setting.AirlineForDefault,setting.RebateForDefault,parameter.AutoPrintBSP,parameter.AutoPrintB2B,parameter.CancelPnrBySelf,parameter.CanReleaseVip,parameter.CanHaveSubordinate,parameter.RefundCountLimit,parameter.RefundTimeLimit,parameter.FullRefundTimeLimit,parameter.ProfessionRate,parameter.SubordinateRate,parameter.ValidityStart,parameter.ValidityEnd,parameter.AutoPlatformAudit,parameter.AllowBrotherPurchase,parameter.Singleness,parameter.Disperse,parameter.CostFree,parameter.Bloc,parameter.Business,parameter.SinglenessRate,parameter.DisperseRate,parameter.CostFreeRate,parameter.BlocRate,parameter.BusinessRate,parameter.Creditworthiness,parameter.OtherSpecialRate,parameter.OtherSpecial,[hours].WorkdayWorkStart,[hours].WorkdayWorkEnd,[hours].WorkdayRefundStart,[hours].WorkdayRefundEnd,[hours].RestdayWorkStart,[hours].RestdayWorkEnd,[hours].RestdayRefundStart,[hours].RestdayRefundEnd,parameter.LowToHigh,parameter.LowToHighRate FROM T_CompanyParameter parameter LEFT JOIN T_WorkingSetting setting  ON  parameter.Company = setting.Company LEFT JOIN T_WorkingHours [hours] ON  [hours].Company = parameter.Company WHERE parameter.Company  = @Company;";
            CompanySettingsInfo result = null;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql, CommandType.Text))
                {
                    if (reader.Read())
                    {
                        result = new CompanySettingsInfo();
                        var workingSetting = new WorkingSetting();
                        workingSetting.Company = reader.GetGuid(0);
                        workingSetting.DefaultDeparture = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        workingSetting.DefaultArrival = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        workingSetting.AirlineForAdult = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        workingSetting.AirlineForChild = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        if (!reader.IsDBNull(5)) workingSetting.RebateForChild = reader.GetDecimal(5);
                        workingSetting.RefundNeedAudit = reader.IsDBNull(6) ? false : reader.GetBoolean(6);
                        workingSetting.DefaultOfficeNumber = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        workingSetting.IsImpower = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        workingSetting.AirlineForDefault = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        workingSetting.RebateForDefault = reader.IsDBNull(10) ? 0M : reader.GetDecimal(10);
                        result.WorkingSetting = workingSetting;
                        var parameter = new CompanyParameter();
                        parameter.Company = reader.GetGuid(0);
                        parameter.AutoPrintBSP = reader.GetBoolean(11);
                        parameter.AutoPrintB2B = reader.GetBoolean(12);
                        parameter.CancelPnrBySelf = reader.GetBoolean(13);
                        parameter.CanReleaseVip = reader.GetBoolean(14);
                        parameter.CanHaveSubordinate = reader.GetBoolean(15);
                        parameter.RefundCountLimit = reader.GetInt32(16);
                        parameter.RefundTimeLimit = reader.GetInt32(17);
                        parameter.FullRefundTimeLimit = reader.GetInt32(18);
                        parameter.ProfessionRate = reader.GetDecimal(19);
                        parameter.SubordinateRate = reader.GetDecimal(20);
                        if (!reader.IsDBNull(21)) parameter.ValidityStart = reader.GetDateTime(21);
                        if (!reader.IsDBNull(22)) parameter.ValidityEnd = reader.GetDateTime(22);
                        parameter.AutoPlatformAudit = reader.GetBoolean(23);
                        parameter.AllowBrotherPurchase = reader.GetBoolean(24);
                        parameter.Singleness = reader.GetBoolean(25);
                        parameter.Disperse = reader.GetBoolean(26);
                        parameter.CostFree = reader.GetBoolean(27);
                        parameter.Bloc = reader.GetBoolean(28);
                        parameter.Business = reader.GetBoolean(29);
                        parameter.SinglenessRate = reader.GetDecimal(30);
                        parameter.DisperseRate = reader.GetDecimal(31);
                        parameter.CostFreeRate = reader.GetDecimal(32);
                        parameter.BlocRate = reader.GetDecimal(33);
                        parameter.BusinessRate = reader.GetDecimal(34);
                        parameter.Creditworthiness = reader.GetDecimal(35);
                        parameter.OtherSpecialRate = reader.GetDecimal(36);
                        parameter.OtherSpecial = reader.GetBoolean(37);
                        parameter.LowToHigh = reader.GetBoolean(46);
                        parameter.LowToHighRate = reader.GetDecimal(47);
                        result.Parameter = parameter;
                        var hous = new WorkingHours();
                        hous.Company = reader.GetGuid(0);
                        hous.WorkdayWorkStart = Time.Parse(reader.GetValue(38).ToString());
                        hous.WorkdayWorkEnd = Time.Parse(reader.GetValue(39).ToString());
                        hous.WorkdayRefundStart = Time.Parse(reader.GetValue(40).ToString());
                        hous.WorkdayRefundEnd = Time.Parse(reader.GetValue(41).ToString());
                        hous.RestdayWorkStart = Time.Parse(reader.GetValue(42).ToString());
                        hous.RestdayWorkEnd = Time.Parse(reader.GetValue(43).ToString());
                        hous.RestdayRefundStart = Time.Parse(reader.GetValue(44).ToString());
                        hous.RestdayRefundEnd = Time.Parse(reader.GetValue(45).ToString());
                        result.WorkingHours = hous;
                    }
                }
            }
            return result;
        }


        public IEnumerable<CompanyInitInfo> QueryCompanyInit(CompanyType companyType, bool searchDisabledCompany)
        {
            var result = new List<CompanyInitInfo>();
            string sql =
                @"SELECT company.Id,employee.[Login],company.[Type],company.AbbreviateName
                           FROM dbo.T_Company AS company
                           INNER JOIN dbo.T_Employee AS employee 
                           ON company.[Type]&@CompanyType=company.[Type] AND company.Id = employee.[Owner] AND employee.IsAdministrator =1";
            if (!searchDisabledCompany)
            {
                sql += " and company.Enabled = 1";
            }
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyType", (byte)companyType);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var companyInitInfo = new CompanyInitInfo();
                        companyInitInfo.CompanyId = reader.GetGuid(0);
                        if (!reader.IsDBNull(1))
                            companyInitInfo.UserNo = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            companyInitInfo.CompanyType = (CompanyType)reader.GetInt32(2);
                        if (!reader.IsDBNull(3))
                            companyInitInfo.AbbreviateName = reader.GetString(3);
                        result.Add(companyInitInfo);
                    }
                }
            }
            return result;
        }


        public CompanyParameter QueryCompanyParameter(Guid id)
        {
            CompanyParameter result = null;
            string sql =
                "SELECT [Company],[AutoPrintBSP],[AutoPrintB2B],[CancelPnrBySelf],[CanReleaseVip],[CanHaveSubordinate],[RefundCountLimit],[RefundTimeLimit],[FullRefundTimeLimit],[ProfessionRate],[SubordinateRate],[ValidityStart],[ValidityEnd],[AutoPlatformAudit],[AllowBrotherPurchase],[Singleness],[Disperse],[CostFree],[Bloc],[Business],[SinglenessRate],[DisperseRate],[CostFreeRate],[BlocRate],[BusinessRate],[Creditworthiness],[OtherSpecialRate],[OtherSpecial],[LowToHighRate],[LowToHigh] FROM [dbo].[T_CompanyParameter] WHERE Company =@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", id);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new CompanyParameter();
                        result.Company = reader.GetGuid(0);
                        result.AutoPrintBSP = reader.GetBoolean(1);
                        result.AutoPrintB2B = reader.GetBoolean(2);
                        result.CancelPnrBySelf = reader.GetBoolean(3);
                        result.CanReleaseVip = reader.GetBoolean(4);
                        result.CanHaveSubordinate = reader.GetBoolean(5);
                        result.RefundCountLimit = reader.GetInt32(6);
                        result.RefundTimeLimit = reader.GetInt32(7);
                        result.FullRefundTimeLimit = reader.GetInt32(8);
                        result.ProfessionRate = reader.GetDecimal(9);
                        result.SubordinateRate = reader.GetDecimal(10);
                        if (!reader.IsDBNull(11)) result.ValidityStart = reader.GetDateTime(11);
                        if (!reader.IsDBNull(12)) result.ValidityEnd = reader.GetDateTime(12);
                        result.AutoPlatformAudit = reader.GetBoolean(13);
                        result.AllowBrotherPurchase = reader.GetBoolean(14);
                        result.Singleness = reader.GetBoolean(15);
                        result.Disperse = reader.GetBoolean(16);
                        result.CostFree = reader.GetBoolean(17);
                        result.Bloc = reader.GetBoolean(18);
                        result.Business = reader.GetBoolean(19);
                        result.SinglenessRate = reader.GetDecimal(20);
                        result.DisperseRate = reader.GetDecimal(21);
                        result.CostFreeRate = reader.GetDecimal(22);
                        result.BlocRate = reader.GetDecimal(23);
                        result.BusinessRate = reader.GetDecimal(24);
                        result.Creditworthiness = reader.GetDecimal(25);
                        result.OtherSpecialRate = reader.GetDecimal(26);
                        result.OtherSpecial = reader.GetBoolean(27);
                        result.LowToHighRate = reader.GetDecimal(28);
                        result.LowToHigh = reader.GetBoolean(29);
                    }
                }
            }
            return result;
        }


        public WorkingHours QueryWorkingHours(Guid id)
        {
            WorkingHours result = null;
            string sql =
                "SELECT [Company],[WorkdayWorkStart],[WorkdayWorkEnd],[WorkdayRefundStart],[WorkdayRefundEnd],[RestdayWorkStart],[RestdayWorkEnd],[RestdayRefundStart],[RestdayRefundEnd] FROM [dbo].[T_WorkingHours] WHERE Company = @Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", id);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new WorkingHours();
                        result.Company = reader.GetGuid(0);
                        result.WorkdayWorkStart = Time.Parse(reader.GetValue(1).ToString());
                        result.WorkdayWorkEnd = Time.Parse(reader.GetValue(2).ToString());
                        result.WorkdayRefundStart = Time.Parse(reader.GetValue(3).ToString());
                        result.WorkdayRefundEnd = Time.Parse(reader.GetValue(4).ToString());
                        result.RestdayWorkStart = Time.Parse(reader.GetValue(5).ToString());
                        result.RestdayWorkEnd = Time.Parse(reader.GetValue(6).ToString());
                        result.RestdayRefundStart = Time.Parse(reader.GetValue(7).ToString());
                        result.RestdayRefundEnd = Time.Parse(reader.GetValue(8).ToString());
                    }
                }
            }
            return result;
        }


        public WorkingSetting QueryWorkingSetting(Guid id)
        {
            WorkingSetting result = null;
            string sql =
                "SELECT [Company],[DefaultDeparture],[DefaultArrival],[AirlineForAdult],[AirlineForChild],[RebateForChild],[RefundNeedAudit],[DefaultOfficeNumber] ,[IsImpower],[AirlineForDefault],[RebateForDefault] FROM [dbo].[T_WorkingSetting] WHERE Company = @Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", id);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new WorkingSetting();
                        result.Company = reader.GetGuid(0);
                        result.DefaultDeparture = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        result.DefaultArrival = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        result.AirlineForAdult = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        result.AirlineForChild = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        if (!reader.IsDBNull(5)) result.RebateForChild = reader.GetDecimal(5);
                        result.RefundNeedAudit = reader.GetBoolean(6);
                        result.DefaultOfficeNumber = reader.GetString(7);
                        result.IsImpower = reader.GetBoolean(8);
                        result.AirlineForDefault = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        result.RebateForDefault = reader.IsDBNull(10) ? 0M : reader.GetDecimal(10);
                    }
                }
            }
            return result;
        }

        public bool ExistsCompanyName(string companyName)
        {
            string sql = "SELECT COUNT(0) FROM  T_Company WHERE Name = @Name";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", companyName);
                var result = (int)dbOperator.ExecuteScalar(sql);
                return result >= 1;
            }
        }

        public bool ExistsAbbreviateName(string abberviateName)
        {
            string sql = "SELECT COUNT(0) FROM  T_Company WHERE AbbreviateName = @AbbreviateName";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("AbbreviateName", abberviateName);
                var result = (int)dbOperator.ExecuteScalar(sql);
                return result >= 1;
            }
        }

        public bool ExistsAbbreviateNameOrCompanyName(string abberviateName, string companyName)
        {
            string sql = "SELECT COUNT(0) FROM  T_Company WHERE AbbreviateName = @AbbreviateName or Name = @CompanyName";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("AbbreviateName", abberviateName);
                dbOperator.AddParameter("CompanyName", companyName);
                var result = (int)dbOperator.ExecuteScalar(sql);
                return result >= 1;
            }
        }

        public CompanyGroupDetailInfo GetGroupInfo(Guid id)
        {
            string sql =
                "select CG.Name,CG.Company,CG.LastModifyTime,CG.CreateTime,CG.Creator,CG.[Description],CG.Id,CGL.[Group],CGL.Airlines,CGL.Departures from T_CompanyGroup CG left join T_CompanyGroupLimitation CGL ON CG.Id = CGL.[Group] where CG.Id = @GroupId";
            CompanyGroupDetailInfo result = null;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("GroupId", id);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    var groupLimitations = new List<CompanyGroupLimitation>();
                    if (reader.Read())
                    {
                        result = new CompanyGroupDetailInfo();
                        result.Name = reader.GetString(0);
                        result.Company = reader.GetGuid(1);
                        if (!reader.IsDBNull(2)) result.LastModifyTime = reader.GetDateTime(2);
                        result.CreateTime = reader.GetDateTime(3);
                        result.Creator = reader.GetString(4);
                        result.Description = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        result.Id = reader.GetGuid(6);
                        var groupLimitation = new CompanyGroupLimitation();
                        groupLimitation.Group = reader.IsDBNull(7) ? Guid.Empty : reader.GetGuid(7);
                        groupLimitation.Airlines = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        groupLimitation.Departures = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        groupLimitations.Add(groupLimitation);
                    }
                    while (reader.Read())
                    {
                        var groupLimitation = new CompanyGroupLimitation();
                        groupLimitation.Group = reader.GetGuid(7);
                        groupLimitation.Airlines = reader.GetString(8);
                        groupLimitation.Departures = reader.GetString(9);
                        groupLimitations.Add(groupLimitation);
                    }
                    result.Limitations = groupLimitations;
                }
            }
            return result;
        }


        public IEnumerable<BusinessManager> QueryBusinessManagers(Guid companyId)
        {
            var result = new List<BusinessManager>();
            string sql = "SELECT [Id],[Company],[BusinessName],[Mamanger],[Cellphone],[QQ] FROM [dbo].[T_BusinessManager] WHERE Company=@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var info = new BusinessManager();
                        info.Id = reader.GetGuid(0);
                        info.Company = reader.GetGuid(1);
                        info.BusinessName = reader.GetString(2);
                        info.Mamanger = reader.GetString(3);
                        info.Cellphone = reader.GetString(4);
                        info.QQ = reader.GetString(5);
                        result.Add(info);
                    }
                }
            }
            return result;
        }

        public CompanyGroupLimitationInfo GetGroupLimitation(Guid id)
        {
            string sql =
                "	select CGL.Id,CGL.Airlines,CG.Company,CGL.Departures,CG.Id from t_CompanyGroupLimitation CGL inner join T_CompanyGroup CG on CGL.[Group] = CG.Id where CGL.Id = @LimitationId";
            CompanyGroupLimitationInfo result = null;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("LimitationId", id);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new CompanyGroupLimitationInfo();
                        result.Id = reader.GetGuid(0);
                        result.Airlines = reader.GetString(1);
                        result.Company = reader.GetGuid(2);
                        result.Departures = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        result.Group = reader.GetGuid(4);
                    }
                }
            }
            return result;
        }

        public SettingPolicy QuerytPolicySetting(Guid companyId)
        {
            SettingPolicy result = null;
            string sql =
                "SELECT [Company],[BargainCount],[Departure],[Airlines],[Remark],[AllowChildPolicy],[ChildRebate],[ChildAirlines],[AllowInfantPolicy],[InfantRebate],[InfantAirlines],[SinglenessCount],[DisperseCount],[CostFreeCount],[BlocCount],[BusinessCount],[OtherSpecialCount],LowToHighCount FROM [dbo].[T_SettingPolicy] WHERE Company=@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new SettingPolicy();
                        result.Company = reader.GetGuid(0);
                        result.BargainCount = reader.GetInt32(1);
                        result.Departure = reader.GetString(2);
                        result.Airlines = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        result.Remark = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        result.AllowChildPolicy = reader.GetBoolean(5);
                        result.ChildRebate = reader.GetDecimal(6);
                        result.ChildAirlines = reader.GetString(7);
                        result.AllowInfantPolicy = reader.GetBoolean(8);
                        result.InfantRebate = reader.GetDecimal(9);
                        result.InfantAirlines = reader.GetString(10);
                        result.SinglenessCount = reader.GetInt32(11);
                        result.DisperseCount = reader.GetInt32(12);
                        result.CostFreeCount = reader.GetInt32(13);
                        result.BlocCount = reader.GetInt32(14);
                        result.BusinessCount = reader.GetInt32(15);
                        result.OtherSpecialCount = reader.GetInt32(16);
                        result.LowToHighCount = reader.GetInt32(17);
                    }
                }
            }
            return result;
        }

        public IEnumerable<WorkingHours> QueryWorkingHours()
        {
            var result = new List<WorkingHours>();
            string sql =
                "SELECT [Company],[WorkdayWorkStart],[WorkdayWorkEnd],[WorkdayRefundStart],[WorkdayRefundEnd],[RestdayWorkStart],[RestdayWorkEnd],[RestdayRefundStart],[RestdayRefundEnd] FROM [dbo].[T_WorkingHours]";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var hours = new WorkingHours();
                        hours.Company = reader.GetGuid(0);
                        hours.WorkdayWorkStart = Time.Parse(reader.GetValue(1).ToString());
                        hours.WorkdayWorkEnd = Time.Parse(reader.GetValue(2).ToString());
                        hours.WorkdayRefundStart = Time.Parse(reader.GetValue(3).ToString());
                        hours.WorkdayRefundEnd = Time.Parse(reader.GetValue(4).ToString());
                        hours.RestdayWorkStart = Time.Parse(reader.GetValue(5).ToString());
                        hours.RestdayWorkEnd = Time.Parse(reader.GetValue(6).ToString());
                        hours.RestdayRefundStart = Time.Parse(reader.GetValue(7).ToString());
                        hours.RestdayRefundEnd = Time.Parse(reader.GetValue(8).ToString());
                        result.Add(hours);
                    }
                }
            }
            return result;
        }


        public IEnumerable<WorkingSetting> QueryChildTicketProviders(string ariline)
        {
            var result = new List<WorkingSetting>();
            string sql =
                "SELECT setting.[Company],[DefaultDeparture],[DefaultArrival],[AirlineForAdult],[AirlineForChild],[RebateForChild],[RefundNeedAudit],[DefaultOfficeNumber],[IsImpower],[AirlineForDefault],[RebateForDefault] FROM [dbo].[T_WorkingSetting] setting INNER JOIN [dbo].[T_WorkingHours] [hours] ON setting.Company = [hours].Company WHERE CHARINDEX(@Airline,AirlineForChild) > 0 AND setting.RebateForChild IS NOT NULL";
            DateTime now = DateTime.Now;
            bool isWeekend = now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday;
            if (isWeekend)
            {
                sql += " AND @CurTime BETWEEN RestdayWorkStart AND RestdayWorkEnd";
            }
            else
            {
                sql += " AND @CurTime BETWEEN WorkdayWorkStart AND WorkdayWorkEnd";
            }
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CurTime", now.ToString("HH:mm"));
                dbOperator.AddParameter("Airline", ariline);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var info = new WorkingSetting();
                        info.Company = reader.GetGuid(0);
                        info.DefaultDeparture = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        info.DefaultArrival = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        info.AirlineForAdult = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        info.AirlineForChild = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        if (!reader.IsDBNull(5)) info.RebateForChild = reader.GetDecimal(5);
                        info.RefundNeedAudit = reader.GetBoolean(6);
                        info.DefaultOfficeNumber = reader.GetString(7);
                        info.IsImpower = reader.GetBoolean(8);
                        info.AirlineForDefault = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        info.RebateForDefault = reader.IsDBNull(10) ? 0M : reader.GetDecimal(10);
                        result.Add(info);
                    }
                }
            }
            return result;
        }


        public IEnumerable<CompanyGroupLimitationInfo> GetGroupLimitations(Guid companyId)
        {
            string sql =
                "select CGL.Id,CGL.Airlines,CG.Company,CGL.Departures,CG.Id from T_CompanyGroup CG inner join T_CompanyGroupRelation CGR on CGR.[Group] = CG.Id left join T_CompanyGroupLimitation CGL ON CG.Id = CGL.[Group] where CGR.Company = @CompanyId";
            var result = new List<CompanyGroupLimitationInfo>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var item = new CompanyGroupLimitationInfo();
                        item.Id = reader.GetGuid(0);
                        item.Airlines = reader.GetString(1);
                        item.Company = reader.GetGuid(2);
                        item.Departures = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        item.Group = reader.GetGuid(4);
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public Dictionary<Guid, CompanyParameter> QueryCreditworthiness(IEnumerable<Guid> company)
        {
            var result = new Dictionary<Guid, CompanyParameter>();
            string sql =
                "EXEC('SELECT [Company],[AutoPrintBSP],[AutoPrintB2B],[CancelPnrBySelf],[CanReleaseVip],[CanHaveSubordinate],[RefundCountLimit],[RefundTimeLimit],[FullRefundTimeLimit],[ProfessionRate],[SubordinateRate],[ValidityStart],[ValidityEnd],[AutoPlatformAudit],[AllowBrotherPurchase],[Singleness],[Disperse],[CostFree],[Bloc],[Business],[SinglenessRate],[DisperseRate],[CostFreeRate],[BlocRate],[BusinessRate],[Creditworthiness],[OtherSpecialRate],[OtherSpecial] FROM [dbo].[T_CompanyParameter] WHERE Company IN ('+@Company+')')";
            string sqlWhere = string.Empty;
            foreach (Guid item in company) sqlWhere += string.IsNullOrEmpty(sqlWhere) ? "'" + item + "'" : ",'" + item + "'";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", sqlWhere);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var info = new CompanyParameter();
                        info.Company = reader.GetGuid(0);
                        info.AutoPrintBSP = reader.GetBoolean(1);
                        info.AutoPrintB2B = reader.GetBoolean(2);
                        info.CancelPnrBySelf = reader.GetBoolean(3);
                        info.CanReleaseVip = reader.GetBoolean(4);
                        info.CanHaveSubordinate = reader.GetBoolean(5);
                        info.RefundCountLimit = reader.GetInt32(6);
                        info.RefundTimeLimit = reader.GetInt32(7);
                        info.FullRefundTimeLimit = reader.GetInt32(8);
                        info.ProfessionRate = reader.GetDecimal(9);
                        info.SubordinateRate = reader.GetDecimal(10);
                        if (!reader.IsDBNull(11)) info.ValidityStart = reader.GetDateTime(11);
                        if (!reader.IsDBNull(12)) info.ValidityEnd = reader.GetDateTime(12);
                        info.AutoPlatformAudit = reader.GetBoolean(13);
                        info.AllowBrotherPurchase = reader.GetBoolean(14);
                        info.Singleness = reader.GetBoolean(15);
                        info.Disperse = reader.GetBoolean(16);
                        info.CostFree = reader.GetBoolean(17);
                        info.Bloc = reader.GetBoolean(18);
                        info.Business = reader.GetBoolean(19);
                        info.SinglenessRate = reader.GetDecimal(20);
                        info.DisperseRate = reader.GetDecimal(21);
                        info.CostFreeRate = reader.GetDecimal(22);
                        info.BlocRate = reader.GetDecimal(23);
                        info.BusinessRate = reader.GetDecimal(24);
                        info.Creditworthiness = reader.GetDecimal(25);
                        info.OtherSpecialRate = reader.GetDecimal(26);
                        info.OtherSpecial = reader.GetBoolean(27);
                        result.Add(info.Company, info);
                    }
                }
            }
            return result;
        }

        public RelationInfo QuerySpreader(Guid companyId)
        {
            RelationInfo spreader = null;
            string sql =
                @"SELECT company.[Type],AbbreviateName,addr.City,contact.Name,contact.Cellphone,
                           employee.[Login],company.RegisterTime FROM dbo.T_Company company
                           INNER JOIN dbo.T_Relationship relationShip ON relationShip.[Type] =2 AND relationShip.[Initiator] = company.Id
                           INNER JOIN dbo.T_Employee employee ON company.Id = employee.[Owner] AND employee.IsAdministrator = 1
                           LEFT JOIN dbo.T_Address addr ON company.[Address] =addr.Id
                           LEFT JOIN dbo.T_Contact contact ON company.Contact =contact.Id
                           WHERE Responser =@CompanyId ";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        spreader = new RelationInfo();
                        spreader.CompanyType = (CompanyType)reader.GetInt32(0);
                        spreader.AbbreviateName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            spreader.City = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                            spreader.Contact = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                            spreader.ContactPhone = reader.GetString(4);
                        spreader.UserNo = reader.GetString(5);
                        spreader.RegisterTime = reader.GetDateTime(6);
                    }
                }
            }
            return spreader;
        }

        public Company GetCompanyInfo(Guid companyId)
        {
            string sql =
                @"SELECT Id,[Type],AccountType,Name,AbbreviateName,[Address],[Enabled],Audited,OrginationCode,
                          OfficePhones,Faxes,RegisterTime,AuditTime,EffectTime,LastLoginTime,Manager,Contact,EmergencyContact,
                          IsVip,IsOem,Area,OperatorAccount,IsOpenExternalInterface FROM T_Company where Id = @CompanyId";
            Company result = null;
            using (var dbOpeator = new DbOperator(Provider, ConnectionString))
            {
                dbOpeator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOpeator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new Company
                            {
                                Id = reader.GetGuid(0),
                                Type = (CompanyType)reader.GetInt32(1),
                                AccountType = (AccountBaseType)reader.GetInt32(2),
                                Name = reader.GetString(3),
                                AbbreviateName = reader.GetString(4),
                                Enabled = reader.GetBoolean(6),
                                Audited = reader.GetBoolean(7),
                                RegisterTime = reader.GetDateTime(11),
                                EffectTime = reader.GetDateTime(13),
                                IsVip = reader.GetBoolean(18),
                                IsOem = reader.GetBoolean(19),
                                IsOpenExternalInterface = reader.GetBoolean(22),
                            };
                        result.Faxes = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                        result.OrginationCode = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        if (!reader.IsDBNull(5)) result.Address = reader.GetGuid(5);
                        result.OfficePhones = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        if (!reader.IsDBNull(12)) result.AuditTime = reader.GetDateTime(12);
                        if (!reader.IsDBNull(14)) result.LastLoginTime = reader.GetDateTime(14);
                        if (!reader.IsDBNull(15)) result.Manager = reader.GetGuid(15);
                        if (!reader.IsDBNull(16)) result.Contact = reader.GetGuid(16);
                        if (!reader.IsDBNull(17)) result.EmergencyContact = reader.GetGuid(17);
                        result.Area = reader.IsDBNull(20) ? string.Empty : reader.GetString(20);
                        result.OperatorAccount = reader.IsDBNull(21) ? string.Empty : reader.GetString(21);
                    }
                }
            }
            return result;
        }

        public CompanyDetailInfo GetCompanyDetailInfo(Guid companyId)
        {
            string sql =
                @"SELECT Com.Id,[Type],AccountType,Com.Name,AbbreviateName,Com.[Address],Com.[Enabled],Audited,OrginationCode,
     OfficePhones,Faxes,RegisterTime,AuditTime,EffectTime,Com.LastLoginTime,Manager,Contact,EmergencyContact, 
     IsVip,IsOem,Area,OperatorAccount,IsOpenExternalInterface,												  
     Emp.LastLoginIP,Emp.LastLoginLocation,Emp.[Login] as UserName,Emp.[Password] as UserPassword,				
     Manager.Cellphone,Manager.Email,Manager.MSN,Manager.Name,Manager.QQ,										
     EmC.OfficePhone,EmC.Cellphone,EmC.Name,																	
     Contact.Name,Contact.OfficePhone,Contact.Cellphone,Contact.Email,Contact.MSN,Contact.QQ, Contact.CertNo,	
     Addr.Avenue,Addr.City,Addr.District,Addr.Province,Addr.ZipCode,Emp.LastLoginTime,							
     CP.ValidityStart,CP.ValidityEnd,Com.IsOem
                  FROM T_Company Com
                 left join T_Employee Emp on Emp.IsAdministrator = 1 and Com.Id = Emp.[Owner]
                 left join T_Contact Manager on   Com.Manager = Manager.Id
                 left join T_Contact EmC on  Com.EmergencyContact = EmC.Id
                 left join T_Contact Contact on Com.Contact = Contact.Id
                 left join T_Address Addr on Com.[Address] = Addr.Id
                 left join T_CompanyParameter CP on Com.Id = CP.Company
                 where Com.Id = @CompanyId";
            CompanyDetailInfo result = null;
            using (var dbOpeator = new DbOperator(Provider, ConnectionString))
            {
                dbOpeator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOpeator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new CompanyDetailInfo
                            {
                                CompanyId = reader.GetGuid(0),
                                CompanyType = (CompanyType)reader.GetInt32(1),
                                AccountType = (AccountBaseType)reader.GetInt32(2),
                                CompanyName = reader.GetString(3),
                                AbbreviateName = reader.GetString(4),
                                Enabled = reader.GetBoolean(6),
                                Audited = reader.GetBoolean(7),
                                RegisterTime = reader.GetDateTime(11),
                                IsOpenExternalInterface = reader.GetBoolean(22),
                                Contact = reader.IsDBNull(35) ? string.Empty : reader.GetString(35),
                                ContactPhone = reader.IsDBNull(37) ? string.Empty : reader.GetString(37),
                                ContactEmail = reader.IsDBNull(38) ? string.Empty : reader.GetString(38),
                                ContactMSN = reader.IsDBNull(39) ? string.Empty : reader.GetString(39),
                                ContactQQ = reader.IsDBNull(40) ? String.Empty : reader.GetString(40),
                                CertNo = reader.IsDBNull(41) ? string.Empty : reader.GetString(41),
                                EmergencyCall = reader.IsDBNull(33) ? string.Empty : reader.GetString(33),
                                EmergencyContact = reader.IsDBNull(34) ? string.Empty : reader.GetString(34),
                                ManagerCellphone = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                                ManagerEmail = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                                ManagerMsn = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                                ManagerName = reader.IsDBNull(30) ? string.Empty : reader.GetString(30),
                                ManagerQQ = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                                UserName = reader.GetString(25),
                                UserPassword = reader.GetString(26),
                                OperatorAccount = reader.IsDBNull(21) ? string.Empty : reader.GetString(21)
                            };
                        result.Faxes = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                        result.OrginationCode = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        if (!reader.IsDBNull(12)) result.AuditTime = reader.GetDateTime(12);
                        result.Address = reader.IsDBNull(42) ? string.Empty : reader.GetString(42);
                        result.City = reader.IsDBNull(43) ? string.Empty : reader.GetString(43);
                        result.District = reader.IsDBNull(44) ? string.Empty : reader.GetString(44);
                        result.Province = reader.IsDBNull(45) ? string.Empty : reader.GetString(45);
                        result.ZipCode = reader.IsDBNull(46) ? string.Empty : reader.GetString(46);

                        if (!reader.IsDBNull(23)) result.LastLoginIP = reader.GetString(23);
                        if (!reader.IsDBNull(24)) result.LastLoginLocation = reader.GetString(24);
                        if (!reader.IsDBNull(47)) result.LastLoginTime = reader.GetDateTime(47);


                        if (!reader.IsDBNull(48)) result.PeriodStartOfUse = reader.GetDateTime(48);
                        if (!reader.IsDBNull(49)) result.PeriodEndOfUse = reader.GetDateTime(49);

                        result.OfficePhones = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        if (!reader.IsDBNull(12)) result.AuditTime = reader.GetDateTime(12);
                        if (!reader.IsDBNull(14)) result.LastLoginTime = reader.GetDateTime(14);
                        result.Area = reader.IsDBNull(20) ? string.Empty : reader.GetString(20);
                        result.IsOem = reader.GetBoolean(50);
                    }
                }
            }
            return result;
        }

        //2012-12-17 deng.zhao 因需求变更，新增字段
        public SuperiorInfo QuerySuperiorInfo(Guid companyId)
        {
            SuperiorInfo superiorInfo = null;
            string sql = "SELECT TCOM.Id,TRS.[Type],TCOM.[Enabled],TSP.Airlines,TCOMP.ValidityStart,TCOMP.ValidityEnd" +
                        " FROM dbo.T_Company TCOM" +
                        " INNER JOIN dbo.T_Relationship TRS ON TRS.Responser=@CompanyId AND TRS.[Initiator]=TCOM.Id AND TRS.[Type] IN (0,1)" +
                        " LEFT JOIN dbo.T_CompanyParameter TCOMP ON TCOMP.Company=TCOM.Id" +
                        " LEFT JOIN (SELECT sp.Company, STUFF((SELECT '/'+Airline FROM T_SuspendedPolicy WHERE Company = sp.Company FOR XML PATH('')),1,1, '') AS Airlines" +
                        " FROM T_SuspendedPolicy sp GROUP BY sp.Company) TSP ON TSP.Company=TCOM.Id;";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        superiorInfo = new SuperiorInfo
                                           {
                                               Id = reader.GetGuid(0),
                                               Type = (RelationshipType)reader.GetInt32(1),
                                               Enable = reader.GetBoolean(2),
                                               SuspendedCarirer = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                               Expired = false
                                           };
                        if (!reader.IsDBNull(4))
                        {
                            var effectiveDate = reader.GetDateTime(4);
                            superiorInfo.Expired = effectiveDate.Date > DateTime.Today;
                        }
                        if (!reader.IsDBNull(5) && !superiorInfo.Expired)
                        {
                            var expiredDate = reader.GetDateTime(5);
                            superiorInfo.Expired = expiredDate.Date < DateTime.Today;
                        }
                    }
                }
            }
            return superiorInfo;
        }

        public RelationInfo QuerySupperior(Guid companyId)
        {
            RelationInfo relationInfo = null;
            string sql =
                @"SELECT company.[Type],company.AbbreviateName,addr.City,contact.Name,contact.Cellphone,
                           employee.[Login],company.RegisterTime FROM dbo.T_Relationship relation
                           INNER JOIN dbo.T_Company company ON relation.[Initiator] =company.Id
                           LEFT JOIN dbo.T_Address addr ON company.[Address] = addr.Id
                           INNER JOIN dbo.T_Contact contact ON company.Contact = contact.Id
                           INNER JOIN dbo.T_Employee employee ON employee.IsAdministrator =1 AND company.Id = employee.[Owner]
                           WHERE relation.[Type] IN (0,1)  AND  Responser =@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        relationInfo = new RelationInfo();
                        relationInfo.CompanyType = (CompanyType)reader.GetInt32(0);
                        relationInfo.AbbreviateName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            relationInfo.City = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                            relationInfo.Contact = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                            relationInfo.ContactPhone = reader.GetString(4);
                        relationInfo.UserNo = reader.GetString(5);
                        relationInfo.RegisterTime = reader.GetDateTime(6);
                    }
                }
            }
            return relationInfo;
        }

        public Relationship QueryRelationship(Guid companyId)
        {
            Relationship relationShip = null;
            string sql = "SELECT Id,Initiator,Type,CreateTime FROM dbo.T_Relationship WHERE Responser=@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        relationShip = new Relationship();
                        relationShip.Id = reader.GetGuid(0);
                        relationShip.Initiator = reader.GetGuid(1);
                        relationShip.Type = (RelationshipType)reader.GetInt32(2);
                        relationShip.CreateTime = reader.GetDateTime(3);
                    }
                }
            }
            return relationShip;
        }

        public Contact QueryContact(Guid id)
        {
            Contact contact = null;
            string sql =
                @"SELECT Name,[Gender],[Title],[IsPrincipal],[IsEmergency],[Cellphone],[OfficePhone],[CertNo],[EmergencyCall],
                           [Fax],[Email],[MSN],[QQ],[Address] FROM [dbo].[T_Contact] WHERE [Id]=@Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", id);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        contact = new Contact();
                        contact.Name = reader.GetString(0);
                        contact.Gender = (Gender)reader.GetByte(1);
                        if (!reader.IsDBNull(2))
                            contact.Title = reader.GetString(2);
                        contact.IsPrincipal = reader.GetBoolean(3);
                        contact.IsEmergency = reader.GetBoolean(4);
                        if (!reader.IsDBNull(5))
                            contact.Cellphone = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            contact.OfficePhone = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            contact.CertNo = reader.GetString(7);
                        if (!reader.IsDBNull(8))
                            contact.EmergencyCall = reader.GetString(8);
                        if (!reader.IsDBNull(9))
                            contact.Fax = reader.GetString(9);
                        if (!reader.IsDBNull(10))
                            contact.Email = reader.GetString(10);
                        if (!reader.IsDBNull(11))
                            contact.MSN = reader.GetString(11);
                        if (!reader.IsDBNull(12))
                            contact.QQ = reader.GetString(12);
                        if (!reader.IsDBNull(13))
                            contact.Address = reader.GetGuid(13);
                    }
                }
            }
            return contact;
        }
        //2013-02-25 wangsl 因需求变更，对公司用户控件修改
        public IEnumerable<CompanyListInfo> QueryCompanys()
        {
            var result = new List<CompanyListInfo>();
            string sql = @"SELECT  c.Id,e.[Login],c.[Type],c.AbbreviateName,c.[Enabled] FROM T_Company c
                    INNER JOIN T_Employee e ON c.Id=e.[Owner] AND e.IsAdministrator = 1 ORDER BY e.[Login] ";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                //if (companyType.HasValue)
                //{
                //    sql += " AND c.[Type]&@CompanyType=c.[Type] ";
                //    dbOperator.AddParameter("CompanyType", (byte)companyType);
                //}
                //if (!searchDisabledCompany)
                //{
                //    sql += " and c.Enabled = 1";
                //}
                //sql += " ORDER BY e.[Login]";
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var companyInitInfo = new CompanyListInfo();
                        companyInitInfo.CompanyId = reader.GetGuid(0);
                        if (!reader.IsDBNull(1))
                            companyInitInfo.UserNo = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            companyInitInfo.CompanyType = (CompanyType)reader.GetInt32(2);
                        if (!reader.IsDBNull(3))
                            companyInitInfo.AbbreviateName = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                            companyInitInfo.Enabled = reader.GetBoolean(4);
                        result.Add(companyInitInfo);
                    }
                }
            }
            return result;
        }

        public CompanyDetailInfo GetCompanyDetail(string userNo)
        {
            string sql =
                @"SELECT Com.Id,[Type],AccountType,Com.Name,AbbreviateName,Com.[Address],Com.[Enabled],Audited,OrginationCode,
     OfficePhones,Faxes,RegisterTime,AuditTime,EffectTime,Com.LastLoginTime,Manager,Contact,EmergencyContact, 
     IsVip,IsOem,Area,OperatorAccount,IsOpenExternalInterface,												  
     Emp.LastLoginIP,Emp.LastLoginLocation,Emp.[Login] as UserName,Emp.[Password] as UserPassword,				
     Manager.Cellphone,Manager.Email,Manager.MSN,Manager.Name,Manager.QQ,										
     EmC.OfficePhone,EmC.Cellphone,EmC.Name,																	
     Contact.Name,Contact.OfficePhone,Contact.Cellphone,Contact.Email,Contact.MSN,Contact.QQ, Contact.CertNo,	
     Addr.Avenue,Addr.City,Addr.District,Addr.Province,Addr.ZipCode,Emp.LastLoginTime,							
     CP.ValidityStart,CP.ValidityEnd,Com.IsOem
                  FROM T_Company Com
                 left join T_Employee Emp on Com.Id = Emp.[Owner]
                 left join T_Contact Manager on   Com.Manager = Manager.Id
                 left join T_Contact EmC on  Com.EmergencyContact = EmC.Id
                 left join T_Contact Contact on Com.Contact = Contact.Id
                 left join T_Address Addr on Com.[Address] = Addr.Id
                 left join T_CompanyParameter CP on Com.Id = CP.Company
                 where Emp.[Login] = @UserNo";
            CompanyDetailInfo result = null;
            using (var dbOpeator = new DbOperator(Provider, ConnectionString))
            {
                dbOpeator.AddParameter("UserNo", userNo);
                using (DbDataReader reader = dbOpeator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new CompanyDetailInfo
                        {
                            CompanyId = reader.GetGuid(0),
                            CompanyType = (CompanyType)reader.GetInt32(1),
                            AccountType = (AccountBaseType)reader.GetInt32(2),
                            CompanyName = reader.GetString(3),
                            AbbreviateName = reader.GetString(4),
                            Enabled = reader.GetBoolean(6),
                            Audited = reader.GetBoolean(7),
                            RegisterTime = reader.GetDateTime(11),
                            IsOpenExternalInterface = reader.GetBoolean(22),
                            Contact = reader.IsDBNull(35) ? string.Empty : reader.GetString(35),
                            ContactPhone = reader.IsDBNull(37) ? string.Empty : reader.GetString(37),
                            ContactEmail = reader.IsDBNull(38) ? string.Empty : reader.GetString(38),
                            ContactMSN = reader.IsDBNull(39) ? string.Empty : reader.GetString(39),
                            ContactQQ = reader.IsDBNull(40) ? String.Empty : reader.GetString(40),
                            CertNo = reader.IsDBNull(41) ? string.Empty : reader.GetString(41),
                            EmergencyCall = reader.IsDBNull(33) ? string.Empty : reader.GetString(33),
                            EmergencyContact = reader.IsDBNull(34) ? string.Empty : reader.GetString(34),
                            ManagerCellphone = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                            ManagerEmail = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                            ManagerMsn = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                            ManagerName = reader.IsDBNull(30) ? string.Empty : reader.GetString(30),
                            ManagerQQ = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                            UserName = reader.GetString(25),
                            UserPassword = reader.GetString(26),
                            OperatorAccount = reader.IsDBNull(21) ? string.Empty : reader.GetString(21)
                        };
                        result.Faxes = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                        result.OrginationCode = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        if (!reader.IsDBNull(12)) result.AuditTime = reader.GetDateTime(12);
                        result.Address = reader.IsDBNull(42) ? string.Empty : reader.GetString(42);
                        result.City = reader.IsDBNull(43) ? string.Empty : reader.GetString(43);
                        result.District = reader.IsDBNull(44) ? string.Empty : reader.GetString(44);
                        result.Province = reader.IsDBNull(45) ? string.Empty : reader.GetString(45);
                        result.ZipCode = reader.IsDBNull(46) ? string.Empty : reader.GetString(46);

                        if (!reader.IsDBNull(23)) result.LastLoginIP = reader.GetString(23);
                        if (!reader.IsDBNull(24)) result.LastLoginLocation = reader.GetString(24);
                        if (!reader.IsDBNull(47)) result.LastLoginTime = reader.GetDateTime(47);


                        if (!reader.IsDBNull(48)) result.PeriodStartOfUse = reader.GetDateTime(48);
                        if (!reader.IsDBNull(49)) result.PeriodEndOfUse = reader.GetDateTime(49);

                        result.OfficePhones = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        if (!reader.IsDBNull(12)) result.AuditTime = reader.GetDateTime(12);
                        if (!reader.IsDBNull(14)) result.LastLoginTime = reader.GetDateTime(14);
                        result.Area = reader.IsDBNull(20) ? string.Empty : reader.GetString(20);
                        result.IsOem = reader.GetBoolean(50);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据域名查找OEM
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public OEMInfo QueryOEM(string domain)
        {
            OEMInfo result = null;
            var sql = @"select TOEM.Id,TOEM.Company,TOEM.CompanyName,SiteName,DomainName,ManageEmail,ICPRecord,EmbedCode,
		LoginUrl,TOEM.[Enabled],AllowSelfRegex,UseB3BConfig,TOEM.RegisterTime,TOEM.EffectTime,AuthCashDeposit,
		TOEM.OperatorAccount,Setting,SiteKeyWord,SiteDescription,LogoPath,BGColor,copyrightInfo,
		TC.Id as ContractId,EnterpriseQQ,Fax,ServicePhone,RefundPhone,ScrapPhone,PayServicePhone,
		EmergencyPhone,ComplainPhone,PrintTicketPhone,AllowPlatformContractPurchaser,UseB3BServicePhone,StyleId,LoginUrl
 from T_OEMInfo TOEM
   left join T_OEMSetting TSet on TOEM.Setting = TSet.Id
   left join T_OEMContract TC on TOEM.[Contract] = TC.Id
   where TOEM.DomainName = @Domain";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Domain", domain ?? string.Empty);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = ParseOEM(reader);
                    }
                }
                if (result != null)
                {
                    LoadOEMSettingLinks(result, dbOperator);
                    LoadOEMAirlineConfig(result, dbOperator);
                    LoadOEMStyle(result, dbOperator);
                    //result.IncomeGroupList = loadIncomeGroups(result.CompanyId, dbOperator);
                }
            }
            return result;
        }

        public OEMInfo QueryOEM(Guid companyId)
        {
            OEMInfo result = null;
            var sql = @"select TOEM.Id,TOEM.Company,TOEM.CompanyName,SiteName,DomainName,ManageEmail,ICPRecord,EmbedCode,
		LoginUrl,TOEM.[Enabled],AllowSelfRegex,UseB3BConfig,TOEM.RegisterTime,TOEM.EffectTime,AuthCashDeposit,
		TOEM.OperatorAccount,Setting,SiteKeyWord,SiteDescription,LogoPath,BGColor,copyrightInfo,
		TC.Id as ContractId,EnterpriseQQ,Fax,ServicePhone,RefundPhone,ScrapPhone,PayServicePhone,
		EmergencyPhone,ComplainPhone,PrintTicketPhone,AllowPlatformContractPurchaser,UseB3BServicePhone,StyleId,LoginUrl
 from T_OEMInfo TOEM
   left join T_OEMSetting TSet on TOEM.Setting = TSet.Id
   left join T_OEMContract TC on TOEM.[Contract] = TC.Id
   where TOEM.Company = @CompanyID";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyID", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = ParseOEM(reader);
                    }
                }
                if (result != null)
                {
                    LoadOEMSettingLinks(result, dbOperator);
                    LoadOEMAirlineConfig(result, dbOperator);
                    LoadOEMStyle(result, dbOperator);
                    //result.IncomeGroupList =loadIncomeGroups(result.CompanyId, dbOperator);
                }
            }
            return result;
        }

        public IEnumerable<KeyValuePair<Guid, bool>> QueryOEMContractSettings(IEnumerable<Guid> oemIds)
        {
            IList<KeyValuePair<Guid, bool>> oems = new List<KeyValuePair<Guid, bool>>();
            string sql = string.Format("  SELECT oemInfo.Id,oemContract.AllowPlatformContractPurchaser FROM T_OEMInfo oemInfo LEFT JOIN T_OEMContract oemContract ON oemContract.Id = oemInfo.[Contract] WHERE oemInfo.Id IN('{0}');", string.Join(",", oemIds).Replace("'", "").Replace(",", "','"));
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        oems.Add(new KeyValuePair<Guid, bool>(reader.GetGuid(0), reader.IsDBNull(1) ? true : reader.GetBoolean(1)));
                    }
                }
            }
            return oems;
        }

        /// <summary>
        ///  通过OEMID查找OEM信息
        /// </summary>
        /// <param name="oemId"></param>
        /// <returns></returns>
        public OEMInfo QueryOEMById(Guid oemId)
        {
            OEMInfo result = null;
            var sql = @"select TOEM.Id,TOEM.Company,TOEM.CompanyName,SiteName,DomainName,ManageEmail,ICPRecord,EmbedCode,
		LoginUrl,TOEM.[Enabled],AllowSelfRegex,UseB3BConfig,TOEM.RegisterTime,TOEM.EffectTime,AuthCashDeposit,
		TOEM.OperatorAccount,Setting,SiteKeyWord,SiteDescription,LogoPath,BGColor,copyrightInfo,
		TC.Id as ContractId,EnterpriseQQ,Fax,ServicePhone,RefundPhone,ScrapPhone,PayServicePhone,
		EmergencyPhone,ComplainPhone,PrintTicketPhone,AllowPlatformContractPurchaser,UseB3BServicePhone,StyleId,LoginUrl
 from T_OEMInfo TOEM
   left join T_OEMSetting TSet on TOEM.Setting = TSet.Id
   left join T_OEMContract TC on TOEM.[Contract] = TC.Id
   where TOEM.Id = @OEMId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("OEMId", oemId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = ParseOEM(reader);
                    }
                }
                if (result != null)
                {
                    LoadOEMSettingLinks(result, dbOperator);
                    LoadOEMAirlineConfig(result, dbOperator);
                    LoadOEMStyle(result, dbOperator);
                    //result.IncomeGroupList = loadIncomeGroups(result.CompanyId, dbOperator);
                }
            }
            return result;
        }

        private void LoadOEMAirlineConfig(OEMInfo oemInfo, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            string sql = "select ConfigId,OEMId,ConfigUseType,UserName,OfficeNO  from T_OEMAirlineConfig where OEMId = @OEMID";
            dbOperator.AddParameter("OEMID", oemInfo.Id);
            using (var reader = dbOperator.ExecuteReader(sql))
            {
                oemInfo.AirlineConfig = new Dictionary<ConfigUseType, Tuple<string, string>>();
                while (reader.Read())
                {
                    oemInfo.AirlineConfig.Add((ConfigUseType)reader.GetByte(2),
                        new Tuple<string, string>(
                        reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                        ));
                }
            }
        }

        private void LoadOEMSettingLinks(OEMInfo oem, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            var headerLinks = new List<Links>();
            var footerLinks = new List<Links>();
            if (oem.Setting.Id != Guid.Empty)
            {
                string sql = "select LinkType,LinkName,URL,Remark from T_OEMLinks where SettingId = @SettingId";
                dbOperator.AddParameter("SettingId", oem.Setting.Id);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0) && reader.GetByte(0) == (byte)OEMLinkType.Header)
                        {
                            headerLinks.Add(new Links
                            {
                                LinkName = reader.GetString(1),
                                URL = reader.GetString(2),
                                Remark = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                            });
                        }
                        else
                        {
                            footerLinks.Add(new Links
                            {
                                LinkName = reader.GetString(1),
                                URL = reader.GetString(2),
                                Remark = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                            });

                        }
                    }
                }
            }
            oem.Setting.HeaderLinks = headerLinks;
            oem.Setting.FooterLinks = footerLinks;
        }

        private void LoadOEMStyle(OEMInfo oemInfo, DbOperator dbOperator)
        {
            dbOperator.ClearParameters();
            const string sql = "SELECT Id,StyleName,ThumbnailPicture,Remark,Sort,Enable,TemplatePath FROM  dbo.T_OEMStyle WHERE Id = @Id";
            if (oemInfo.OEMStyle.Id == Guid.Empty)
            {
                oemInfo.OEMStyle = null;
            }
            else
            {
                dbOperator.AddParameter("Id", oemInfo.OEMStyle.Id);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        OEMStyle style = new Domain.OEMStyle();
                        style.Id = reader.GetGuid(0);
                        style.StyleName = reader.GetString(1);
                        style.ThumbnailPicture = reader.GetString(2);
                        style.Remark = reader.GetString(3);
                        style.Sort = reader.GetInt32(4);
                        style.Enable = reader.GetBoolean(5);
                        style.StylePath = new List<string>();
                        style.TemplatePath = reader.GetString(6);
                        oemInfo.OEMStyle = style;
                    }
                }
                const string queryFiles = "select FilePath from T_OEMStyleFile where OEMStyleId = @Id";
                using (var reader = dbOperator.ExecuteReader(queryFiles))
                {
                    while (reader.Read())
                    {
                        oemInfo.OEMStyle.StylePath.Add(reader.GetString(0));
                    }
                }
            }
        }

        //private List<IncomeGroup> loadIncomeGroups(Guid company, DbOperator dbOperator)
        //{
        //    dbOperator.ClearParameters();
        //    var sql = "SELECT tGroup.Id,tGroup.Name,tGroup.[Description],tGroup.Creator,tGroup.CreateTime,tSetting.CompanyId,IsGlobal,Price,Remark,[Type]," +
        //            "StartPeriod,EndPeriod,Period,tSetting.Id " +
        //            "FROM dbo.T_IncomeGroup tGroup " +
        //            "INNER JOIN T_OEMIncomeGroupDeductSetting tSetting ON tGroup.Id=tSetting.IncomeGroupId " +
        //            "INNER JOIN T_OEMIncomeGroupPeriod tDetail ON tSetting.Id=tDetail.DeductId " +
        //            "WHERE Company=@Company ORDER BY tGroup.Id";
        //    dbOperator.AddParameter("Company", company);
        //    var groups = new List<IncomeGroup>();
        //    IncomeGroup group = null;
        //    IncomeGroupDeductGlobal groupSetting = null;
        //    using (var reader = dbOperator.ExecuteReader(sql))
        //    {
        //        while(reader.Read())
        //        {
        //            var currentGroupId = reader.GetGuid(0);
        //            if (group == null || group.Id != currentGroupId)
        //            {
        //                group = new IncomeGroup
        //                            {
        //                                Id = currentGroupId,
        //                                Company = company,
        //                                Name = reader.GetString(1),
        //                                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
        //                                Creator = reader.GetString(3),
        //                                CreateTime = reader.GetDateTime(4),
        //                                Setting = new List<IncomeGroupDeductGlobal>()
        //                            };
        //                groups.Add(group);
        //                groupSetting = new IncomeGroupDeductGlobal
        //                                   {
        //                                       Id = reader.GetGuid(13),
        //                                       IncomeGroupId = currentGroupId,
        //                                       CompanyId = reader.GetGuid(5),
        //                                       IsGlobal = reader.GetBoolean(6),
        //                                       Price = reader.GetInt32(7),
        //                                       Remark = reader.GetString(8),
        //                                       Type = (Common.Enums.PeriodType) reader.GetByte(9),
        //                                       Period = new List<IncomeGroupPeriod>()
        //                                   };
        //                group.Setting.Add(groupSetting);
        //            }
        //            var period = new IncomeGroupPeriod
        //                             {
        //                                 DeductId = reader.GetGuid(13),
        //                                 StartPeriod = reader.GetDecimal(10),
        //                                 EndPeriod = reader.GetDecimal(11),
        //                                 Period = reader.GetDecimal(12)
        //                             };
        //            groupSetting.Period.Add(period);
        //        }
        //    }
        //    return groups;
        //}

        private OEMInfo ParseOEM(DbDataReader reader)
        {
            OEMInfo oem = new OEMInfo();
            oem.Id = reader.GetGuid(0);
            oem.CompanyId = reader.GetGuid(1);
            oem.SiteName = reader.GetString(3);
            oem.DomainName = reader.GetString(4);
            oem.ManageEmail = reader.GetString(5);
            oem.ICPRecord = reader.GetString(6);
            oem.EmbedCode = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
            oem.Enabled = reader.GetBoolean(9);
            oem.AllowSelfRegex = reader.GetBoolean(10);
            oem.UseB3BConfig = reader.GetBoolean(11);
            oem.RegisterTime = reader.GetDateTime(12);
            oem.EffectTime = reader.IsDBNull(13) ? (DateTime?)null : reader.GetDateTime(13);
            oem.AuthCashDeposit = reader.GetDecimal(14);
            oem.OperatorAccount = reader.GetString(15);
            var setting = new OemSetting();
            setting.Id = reader.IsDBNull(16) ? Guid.Empty : reader.GetGuid(16);
            setting.SiteKeyWord = reader.IsDBNull(17) ? string.Empty : reader.GetString(17);
            setting.SiteDescription = reader.IsDBNull(18) ? string.Empty : reader.GetString(18);
            oem.LogoPath = reader.IsDBNull(19) ? string.Empty : reader.GetString(19);
            setting.BGColor = reader.IsDBNull(20) ? string.Empty : reader.GetString(20);
            setting.CopyrightInfo = reader.IsDBNull(21) ? string.Empty : reader.GetString(21);
            var contract = new OEMContract
            {
                Id = reader.IsDBNull(22) ? Guid.Empty : reader.GetGuid(22),
                EnterpriseQQ = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                Fax = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                ServicePhone = reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                RefundPhone = reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                ScrapPhone = reader.IsDBNull(27) ? string.Empty : reader.GetString(27),
                PayServicePhone = reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                EmergencyPhone = reader.IsDBNull(29) ? string.Empty : reader.GetString(29),
                ComplainPhone = reader.IsDBNull(30) ? string.Empty : reader.GetString(30),
                PrintTicketPhone = reader.IsDBNull(31) ? string.Empty : reader.GetString(31),
                AllowPlatformContractPurchaser = reader.IsDBNull(32) || reader.GetBoolean(32),
                UseB3BServicePhone = reader.IsDBNull(33) || reader.GetBoolean(33)
            };
            oem.OEMStyle = new OEMStyle()
            {
                Id = reader.IsDBNull(34) ? Guid.Empty : reader.GetGuid(34)
            };
            oem.LoginUrl = reader.IsDBNull(35) ? string.Empty : reader.GetString(35);
            oem.Contract = contract;
            oem.Setting = setting;
            return oem;
        }

        #endregion

        public IEnumerable<string> QueryOfficeNumbers(Guid id)
        {
            var result = new List<string>();
            string sql = "SELECT [Number] FROM [dbo].[T_OfficeNumber] WHERE Company =@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", id);
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }
            return result;
        }

        public bool SetCompanyOperatorAccount(Guid companyId, string employeeNo)
        {
            string sql = "UPDATE [dbo].[T_Company] SET [OperatorAccount] = @OperatorAccount  WHERE Id = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", companyId);
                dbOperator.AddParameter("OperatorAccount", employeeNo);
                return dbOperator.ExecuteNonQuery(sql) == 1;
            }
        }
        public PurchaseLimitationType QueryGlobalPurchase(Guid companyId)
        {
            PurchaseLimitationType type = PurchaseLimitationType.None;
            string sql = "SELECT PurchaseLimitationType FROM [dbo].[T_Company] WHERE Id = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {

                        type = (PurchaseLimitationType)reader.GetByte(0);
                    }
                }
            }
            return type;
        }
        public IncomeGroupLimitType QueryGlobalIncomeGroup(Guid companyId)
        {
            IncomeGroupLimitType type = IncomeGroupLimitType.None;
            string sql = "SELECT IncomeGroupLimitType FROM [dbo].[T_Company] WHERE Id = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        type = (IncomeGroupLimitType)reader.GetByte(0);
                    }
                }
            }
            return type;
        }


    }
}