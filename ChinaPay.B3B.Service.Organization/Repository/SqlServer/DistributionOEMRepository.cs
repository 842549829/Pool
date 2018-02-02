using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core;
using System.Data.Common;
using System.Data;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class DistributionOEMRepository : SqlServerRepository, IDistributionOEMRepository
    {
        public DistributionOEMRepository(string connectionString)
            : base(connectionString)
        {

        }
        public void Insert(Domain.DistributionOEM oem, string abbreviateName)
        {
            string sql = @"INSERT INTO [dbo].[T_OEMInfo] ([Id],[Company],[CompanyName],[SiteName],[DomainName],[ManageEmail],[ICPRecord],[EmbedCode]
           ,[Enabled],[AllowSelfRegex],[UseB3BConfig],[RegisterTime] ,[EffectTime],[AuthCashDeposit],[OperatorAccount],[LoginUrl])
            VALUES (@Id,@Company,@CompanyName,@SiteName,@DomainName,@ManageEmail,@ICPRecord,@EmbedCode,@Enabled,@AllowSelfRegex,@UseB3BConfig,@RegisterTime
           ,@EffectTime,@AuthCashDeposit,@OperatorAccount,@LoginUrl)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", oem.Id);
                dbOperator.AddParameter("Company", oem.CompanyId);
                dbOperator.AddParameter("CompanyName", abbreviateName);
                dbOperator.AddParameter("SiteName", oem.SiteName);
                dbOperator.AddParameter("DomainName", oem.DomainName);

                if (string.IsNullOrWhiteSpace(oem.ManageEmail))
                {
                    dbOperator.AddParameter("ManageEmail", string.Empty);
                }
                else
                {
                    dbOperator.AddParameter("ManageEmail", oem.ManageEmail);
                }
                if (string.IsNullOrWhiteSpace(oem.ICPRecord))
                {
                    dbOperator.AddParameter("ICPRecord", string.Empty);
                }
                else
                {
                    dbOperator.AddParameter("ICPRecord", oem.ICPRecord);
                }
                if (string.IsNullOrWhiteSpace(oem.EmbedCode))
                {
                    dbOperator.AddParameter("EmbedCode", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("EmbedCode", oem.EmbedCode);
                }
                dbOperator.AddParameter("Enabled", oem.Enabled);
                dbOperator.AddParameter("AllowSelfRegex", oem.AllowSelfRegex);
                dbOperator.AddParameter("UseB3BConfig", oem.UseB3BConfig);
                dbOperator.AddParameter("RegisterTime", oem.RegisterTime);
                dbOperator.AddParameter("EffectTime", oem.EffectTime);
                dbOperator.AddParameter("AuthCashDeposit", oem.AuthCashDeposit);
                dbOperator.AddParameter("OperatorAccount", oem.OperatorAccount);
                dbOperator.AddParameter("LoginUrl", oem.LoginUrl);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(Domain.DistributionOEM oem)
        {
            string sql = @"UPDATE [dbo].[T_OEMInfo] SET [Company] = @Company ,[CompanyName] = @CompanyName ,[SiteName] = @SiteName ,[DomainName] = @DomainName ,[ManageEmail] = @ManageEmail ,[ICPRecord] = @ICPRecord ,[EmbedCode] = @EmbedCode ,[LoginUrl] = @LoginUrl ,[Enabled] = @Enabled ,[AllowSelfRegex] = @AllowSelfRegex ,[UseB3BConfig] = @UseB3BConfig ,[Contract] = @Contract ,[Setting] = @Setting ,[RegisterTime] = @RegisterTime ,[EffectTime] = @EffectTime ,[AuthCashDeposit] = @AuthCashDeposit ,[OperatorAccount] = @OperatorAccount,LogoPath=@LogoPath WHERE [Id] = @Id ";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Company", oem.CompanyId);
                dbOperator.AddParameter("@CompanyName", oem.Company.AbbreviateName);
                dbOperator.AddParameter("@SiteName", oem.SiteName);
                dbOperator.AddParameter("@DomainName", oem.DomainName);
                dbOperator.AddParameter("@ManageEmail", oem.ManageEmail);
                dbOperator.AddParameter("@ICPRecord", oem.ICPRecord);
                if (string.IsNullOrWhiteSpace(oem.EmbedCode))
                {
                    dbOperator.AddParameter("@EmbedCode", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@EmbedCode", oem.EmbedCode);
                }
                dbOperator.AddParameter("@LoginUrl", oem.LoginUrl);
                dbOperator.AddParameter("@Enabled", oem.Enabled);
                dbOperator.AddParameter("@AllowSelfRegex", oem.AllowSelfRegex);
                dbOperator.AddParameter("@UseB3BConfig", oem.UseB3BConfig);
                dbOperator.AddParameter("@Contract", oem.Contract.Id);
                dbOperator.AddParameter("@Setting", oem.Setting.Id);
                dbOperator.AddParameter("@RegisterTime", oem.RegisterTime);
                dbOperator.AddParameter("@EffectTime", oem.EffectTime);
                dbOperator.AddParameter("@AuthCashDeposit", oem.AuthCashDeposit);
                dbOperator.AddParameter("@OperatorAccount", oem.OperatorAccount);
                dbOperator.AddParameter("@LogoPath", oem.LogoPath);
                dbOperator.AddParameter("@Id", oem.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Domain.DistributionOEM QueryDistributionOEM(Guid companyId)
        {
            throw new NotImplementedException();
        }


        public DataTable QueryDistributionOEMList(DistributionOEMCondition condition, Pagination pagination)
        {
            System.Data.DataTable result = null;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (condition.RegisterBeginTime.HasValue)
                    dbOperator.AddParameter("i_RegisterBeginTime", condition.RegisterBeginTime);
                if (condition.RegisterEndTime.HasValue)
                    dbOperator.AddParameter("i_RegisterEndTime", condition.RegisterEndTime);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("i_UserNo", condition.UserNo);
                if (condition.AutorizationStatus.HasValue)
                    dbOperator.AddParameter("i_AuthorizationStatus", condition.AutorizationStatus);
                if (!string.IsNullOrWhiteSpace(condition.DomainName))
                    dbOperator.AddParameter("i_DomainName", condition.DomainName);
                if (!string.IsNullOrWhiteSpace(condition.AbbreviateName))
                    dbOperator.AddParameter("i_AbbreviateName", condition.AbbreviateName);
                if (!string.IsNullOrWhiteSpace(condition.SiteName))
                    dbOperator.AddParameter("i_SiteName", condition.SiteName);
                if (pagination != null)
                {
                    dbOperator.AddParameter("i_PageSize", pagination.PageSize);
                    dbOperator.AddParameter("i_PageIndex", pagination.PageIndex);
                }
                DbParameter totalCount = dbOperator.AddParameter("o_RowCount");
                totalCount.Direction = ParameterDirection.Output;
                totalCount.DbType = DbType.Int32;
                result = dbOperator.ExecuteTable("dbo.P_DistributionListPagination", CommandType.StoredProcedure);
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public void UpdateSetting(Domain.OemSetting setting)
        {
            string sql = @"UPDATE [dbo].[T_OEMSetting] SET SiteKeyWord=@SiteKeyWord ,SiteDescription=@SiteDescription,BGColor=@BGColor,copyrightInfo=@copyrightInfo WHERE ID = @Id;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@SiteKeyWord", setting.SiteKeyWord);
                dbOperator.AddParameter("@SiteDescription", setting.SiteDescription);
                //dbOperator.AddParameter("@LogoPath", setting.LogoPath);
                dbOperator.AddParameter("@BGColor", setting.BGColor);
                dbOperator.AddParameter("@copyrightInfo", setting.CopyrightInfo);
                dbOperator.AddParameter("@Id", setting.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void UpdateOemInfo(Domain.DistributionOEM oem)
        {
            string sql = @"UPDATE [dbo].[T_OEMInfo] SET [SiteName] = @SiteName,[DomainName] = @DomainName,[UseB3BConfig] = @UseB3BConfig ,[EffectTime] = @EffectTime,[AuthCashDeposit] = @AuthCashDeposit,LoginUrl=@LoginUrl
                           WHERE Id = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", oem.Id);
                dbOperator.AddParameter("SiteName", oem.SiteName);
                dbOperator.AddParameter("DomainName", oem.DomainName);
                dbOperator.AddParameter("UseB3BConfig", oem.UseB3BConfig);
                dbOperator.AddParameter("EffectTime", oem.EffectTime);
                dbOperator.AddParameter("AuthCashDeposit", oem.AuthCashDeposit);
                dbOperator.AddParameter("LoginUrl", oem.LoginUrl);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void InsertSetting(Domain.OemSetting setting, Guid oemid)
        {
            string sql = @"INSERT INTO [dbo].[T_OEMSetting] ([Id],[SiteKeyWord],[SiteDescription] ,[BGColor],[copyrightInfo]) VALUES (@Id ,@SiteKeyWord ,@SiteDescription ,@BGColor ,@copyrightInfo);UPDATE [dbo].[T_OEMInfo] SET [Setting] = @settingId WHERE [Id] = @oemId ;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Id", setting.Id);
                dbOperator.AddParameter("@settingId", setting.Id);
                dbOperator.AddParameter("@oemId", oemid);
                dbOperator.AddParameter("@SiteKeyWord", setting.SiteKeyWord);
                dbOperator.AddParameter("@SiteDescription", setting.SiteDescription);
                //dbOperator.AddParameter("@LogoPath", setting.LogoPath);
                dbOperator.AddParameter("@BGColor", setting.BGColor);
                dbOperator.AddParameter("@copyrightInfo", setting.CopyrightInfo);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Domain.OemSetting QuerySetting(Guid id)
        {
            string sql = @"SELECT [Id],[SiteKeyWord],[SiteDescription] ,[BGColor],[copyrightInfo] FROM T_OEMSetting WHERE Id=@Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Id", id);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Domain.OemSetting setting = null;
                    if (reader.Read())
                    {
                        setting = new Domain.OemSetting();
                        setting.Id = reader.GetGuid(0);
                        setting.SiteKeyWord = reader.GetString(1);
                        setting.SiteDescription = reader.GetString(2);
                        //setting.LogoPath = reader.GetString(3);
                        setting.BGColor = reader.GetString(3);
                        setting.CopyrightInfo = reader.GetString(4);
                    }
                    return setting;
                }
            }
        }

        public IEnumerable<DistributionOEMUserListView> QueryDistributionOEMUserList(DistributionOEMUserCondition condition, Pagination pagination)
        {
            var result = new List<DistributionOEMUserListView>();
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_Owner", condition.CompanyId);
                if (condition.IsOwnerAll.HasValue)
                {
                    dbOperator.AddParameter("@i_IsOwnerAll", condition.IsOwnerAll);
                }
                else
                {
                    dbOperator.AddParameter("@i_IsOwnerAll", true);
                }
                dbOperator.AddParameter("@i_RegisterBeginTime", condition.RegisterBeginTime.HasValue ? condition.RegisterBeginTime.Value : (DateTime?)null);
                dbOperator.AddParameter("@i_RegisterEndTime", condition.RegisterEndTime.HasValue ? condition.RegisterEndTime.Value : (DateTime?)null);
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("@i_UserNo", condition.UserNo);
                if (condition.Enable.HasValue)
                    dbOperator.AddParameter("@i_Enable", condition.Enable);
                if (!string.IsNullOrWhiteSpace(condition.ContactName))
                    dbOperator.AddParameter("@i_Contact", condition.ContactName);
                if (condition.IncomeGroup.HasValue)
                    dbOperator.AddParameter("@i_IncomeGroup", condition.IncomeGroup);
                if (!string.IsNullOrWhiteSpace(condition.AbbreviateName))
                {
                    dbOperator.AddParameter("@i_AbbreviateName", condition.AbbreviateName);
                }
                if (pagination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", pagination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_RowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("[dbo].[P_DistributionUserListPagination]", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var user = new DistributionOEMUserListView();
                        user.RegisterTime = reader.GetDateTime(0);
                        user.AbbreviateName = reader.GetString(1);
                        user.Type = (CompanyType)reader.GetInt32(2);
                        user.AccountType = (AccountBaseType)reader.GetInt32(3);
                        user.ContactName = reader.GetString(4);
                        user.CompanyId = reader.GetGuid(5);
                        user.Enabled = reader.GetBoolean(6);
                        user.Login = reader.GetString(7);
                        user.IncomeGroupName = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        if (!reader.IsDBNull(9))
                            user.IncomeGroupId = reader.GetGuid(9);
                        result.Add(user);
                    }
                }
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public DistribtionOEMUserCompanyDetailInfo QueryDistributionOEMUserDetail(Guid companyId)
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
     CP.ValidityStart,CP.ValidityEnd,Com.IsOem,TIncome.Name,TIncome.Id
                  FROM T_Company Com
                 LEFT JOIN dbo.T_IncomeGroupRelation TRel ON TRel.Company =Com.Id
                 LEFT JOIN dbo.T_IncomeGroup TIncome ON TRel.IncomeGroup = TIncome.Id
                 LEFT JOIN T_Employee Emp on Emp.IsAdministrator = 1 and Com.Id = Emp.[Owner]
                 LEFT JOIN T_Contact Manager on   Com.Manager = Manager.Id
                 LEFT JOIN T_Contact EmC on  Com.EmergencyContact = EmC.Id
                 LEFT JOIN T_Contact Contact on Com.Contact = Contact.Id
                 LEFT JOIN T_Address Addr on Com.[Address] = Addr.Id
                 LEFT JOIN T_CompanyParameter CP on Com.Id = CP.Company
                 WHERE Com.Id = @CompanyId";
            DistribtionOEMUserCompanyDetailInfo result = null;
            using (var dbOpeator = new DbOperator(Provider, ConnectionString))
            {
                dbOpeator.AddParameter("CompanyId", companyId);
                using (DbDataReader reader = dbOpeator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        result = new DistribtionOEMUserCompanyDetailInfo
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
                        if (!reader.IsDBNull(51))
                            result.IncomeGroupName = reader.GetString(51);
                        if (!reader.IsDBNull(52))
                            result.IncomeGroupId = reader.GetGuid(52);
                    }
                }
            }
            return result;
        }


        public void InsertOEMLinks(Guid settingId, IEnumerable<Domain.Links> headerLinks)
        {
            string sql = "DELETE FROM T_OEMLinks WHERE SettingId = @SettingId AND LinkType = @LinkType;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@SettingId", settingId);
                dbOperator.AddParameter("@LinkType", (byte)headerLinks.FirstOrDefault().LinkType);
                int i = 0;
                foreach (var item in headerLinks)
                {
                    if (item.LinkName.Trim() != "")
                    {
                        i++;
                        sql += "INSERT INTO [dbo].[T_OEMLinks] ([SettingId],[LinkType],[LinkName],[URL],[Remark])VALUES(@SettingId" + i + " ,@LinkType" + i + " ,@LinkName" + i + " ,@URL" + i + " ,@Remark" + i + ");";
                        dbOperator.AddParameter("@SettingId" + i, settingId);
                        dbOperator.AddParameter("@LinkName" + i, item.LinkName);
                        dbOperator.AddParameter("@LinkType" + i, (byte)item.LinkType);
                        dbOperator.AddParameter("@URL" + i, item.URL);
                        dbOperator.AddParameter("@Remark" + i, item.Remark);
                    }
                }
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void SvaeOEMContract(Domain.OEMInfo oemInfo)
        {
            string sql = "IF EXISTS(SELECT NULL FROM T_OEMContract where Id = @ContractId) UPDATE T_OEMContract set EnterpriseQQ = @EnterpriseQQ,Fax = @Fax,ServicePhone = @ServicePhone,RefundPhone = @RefundPhone,ScrapPhone = @ScrapPhone,PayServicePhone = @PayServicePhone,EmergencyPhone = @EmergencyPhone,ComplainPhone = @ComplainPhone,PrintTicketPhone = @PrintTicketPhone,AllowPlatformContractPurchaser= @AllowPlatformContractPurchaser ,UseB3BServicePhone = @UseB3BServicePhone WHERE Id=@ContractId ELSE INSERT INTO T_OEMContract(Id,EnterpriseQQ,Fax,ServicePhone,RefundPhone,ScrapPhone,PayServicePhone,EmergencyPhone,ComplainPhone,PrintTicketPhone,AllowPlatformContractPurchaser,UseB3BServicePhone) VALUES(@ContractId,@EnterpriseQQ,@Fax,@ServicePhone,@RefundPhone,@ScrapPhone,@PayServicePhone,@EmergencyPhone,@ComplainPhone,@PrintTicketPhone,@AllowPlatformContractPurchaser,@UseB3BServicePhone);";
            sql += "UPDATE T_OEMInfo SET [Contract] = @ContractId WHERE Id =@Id";
            using (DbOperator dboperator = new DbOperator(Provider, ConnectionString))
            {
                dboperator.AddParameter("Id", oemInfo.Id);
                dboperator.AddParameter("ContractId", oemInfo.Contract.Id == Guid.Empty ? Guid.NewGuid() : oemInfo.Contract.Id);
                dboperator.AddParameter("EnterpriseQQ", oemInfo.Contract.EnterpriseQQ);
                dboperator.AddParameter("Fax", oemInfo.Contract.Fax);
                dboperator.AddParameter("ServicePhone", oemInfo.Contract.ServicePhone);
                dboperator.AddParameter("RefundPhone", oemInfo.Contract.RefundPhone);
                dboperator.AddParameter("ScrapPhone", oemInfo.Contract.ScrapPhone);
                dboperator.AddParameter("PayServicePhone", oemInfo.Contract.PayServicePhone);
                dboperator.AddParameter("EmergencyPhone", oemInfo.Contract.EmergencyPhone);
                dboperator.AddParameter("ComplainPhone", oemInfo.Contract.ComplainPhone);
                dboperator.AddParameter("PrintTicketPhone", oemInfo.Contract.PrintTicketPhone);
                dboperator.AddParameter("AllowPlatformContractPurchaser", oemInfo.Contract.AllowPlatformContractPurchaser);
                dboperator.AddParameter("UseB3BServicePhone", oemInfo.Contract.UseB3BServicePhone);
                dboperator.ExecuteNonQuery(sql);
            }
        }


        public bool CheckExsistDomainName(string domainName)
        {
            bool IsExsist = false;
            string sql = "SELECT * FROM dbo.T_OEMInfo WHERE DomainName=@DomainName";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("DomainName", domainName);
                var result = dbOperator.ExecuteNonQuery(sql);
                if (result > 0)
                    IsExsist = true;
            }
            return IsExsist;
        }


        public bool CheckInitiatorIsOem(Guid companyId)
        {
            var result = false;
            string sql = @"SELECT TINit.IsOem FROM dbo.T_Relationship TRel
INNER JOIN dbo.T_Company TCom ON TRel.Responser=TCom.Id
INNER JOIN dbo.T_Company TINit ON TRel.Initiator =TINit.Id
WHERE TCom.Id =@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result = reader.GetBoolean(0);
                    }
                }
            }
            return result;
        }


        public IEnumerable<Guid> QueryOrginationCompany()
        {
            var result = new List<Guid>();
            string sql = @"SELECT Responser FROM dbo.T_Relationship WHERE [Type] =@Type";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Type", RelationshipType.Organization);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetGuid(0));
                    }
                }
            }
            return result;
        }


        public void ChooiceStyle(Guid oemId, Guid styleId)
        {
            string sql = "UPDATE [dbo].[T_OEMInfo] SET [StyleId] = @StyleId WHERE [Id] = @oemId ";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("oemId", oemId);
                dbOperator.AddParameter("StyleId", styleId);
                dbOperator.ExecuteNonQuery(sql);
            }
        }


        public IEnumerable<DistributionOEMView> QueryDistributionOEM()
        {
            var result = new List<DistributionOEMView>();
            string sql = @"SELECT 
	    Company,CompanyName,[Login]
	    FROM T_OEMInfo 
		INNER JOIN dbo.T_Employee ON T_OEMInfo.Company = T_Employee.[Owner] AND T_Employee.IsAdministrator =1";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var distributionOemView = new DistributionOEMView();
                        distributionOemView.CompanyId = reader.GetGuid(0);
                        distributionOemView.AbbreivateName = reader.GetString(1);
                        distributionOemView.UserNo = reader.GetString(2);
                        result.Add(distributionOemView);
                    }
                }
            }
            return result;
        }
    }
}
