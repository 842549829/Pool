using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core;
using ChinaPay.SMS.Service;
using Izual.Linq;
using PoolPay.DataTransferObject;
using Account = ChinaPay.B3B.Data.DataMapping.Account;
using AccountType = ChinaPay.B3B.Common.Enums.AccountType;
using Company = ChinaPay.B3B.Data.DataMapping.Company;
using Time = Izual.Time;

namespace ChinaPay.B3B.Service.Organization
{
    public static class AccountCombineService
    {
        /// <summary>
        /// 验证注册IP是否超过3次
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public static bool ValidateIP(string ip)
        {
            bool isValiate = true;
            var model = QueryRegisterIp(ip);
            RegisterIP registerIP = null;
            if (model == null || model.IP == null)
            {
                registerIP = new RegisterIP();
                registerIP.IP = ip;
                registerIP.Number = 1;
                registerIP.RegisterDate = DateTime.Now;
                InsertRegisterIp(registerIP);
            }
            else
            {
                if (model.Number == 100 && model.RegisterDate.Date == DateTime.Now.Date)
                {
                    isValiate = false;
                }
                if (model.Number < 100 && model.RegisterDate.Date == DateTime.Now.Date)
                {
                    model.Number++;
                    UpdateRegisterIp(model);
                }
                if (model.RegisterDate.Date != DateTime.Now.Date)
                {
                    model.Number = 1;
                    model.RegisterDate = DateTime.Now;
                    UpdateRegisterIp(model);
                }
            }
            return isValiate;
        }

        /// <summary>
        /// 保存验证码
        /// </summary>
        /// <param name="verfiCode"></param>
        public static void SaveVerfiCode(VerfiCode verfiCode)
        {
            var repository = Factory.CreateVerfiCodeRepository();
            repository.Insert(verfiCode);
        }

        /// <summary>
        /// 验证身份证号
        /// </summary>
        public static bool ValidateIdentifyCard(string identifyCardNo)
        {
            var validator = new ChinaPay.IdentityCard.Validator(identifyCardNo);
            validator.Execute();
            return validator.Success;
        }

        /// <summary>
        /// 是否需要POS介绍
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="isNeedApply"></param>
        private static void AddPosApply(Guid companyId, bool isNeedApply)
        {
            var repository = Factory.CreateApplyPosRepository();
            repository.Save(companyId, isNeedApply);
        }

        /// <summary>
        /// 注册个人账户
        /// </summary>
        /// <param name="accountInfo">账户信息</param>
        /// <param name="individual">个人基础信息</param>
        /// <returns></returns>
        public static void Register(AccountInfo accountInfo, AccountBasicIndividual individual, string domainName, string servicePhone, string platformName)
        {
            if (accountInfo == null) throw new ArgumentException("accountInfo");
            if (individual == null) throw new ArgumentException("individual");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");
            if (individual.CompanyType == CompanyType.Provider)
                throw new ArgumentNullException("注册个人账户不能开出票方类型账户");

            var company = CreateCompany(individual);
            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;

            var contact = CreateContact(individual);
            company.Contact = contact.Id;
            company.Manager = contact.Id;
            company.EmergencyContact = contact.Id;

            var admin = CreateAdministrator(accountInfo, individual);
            admin.Owner = company.Id;

            Domain.Relationship relation = null;
            if (individual.OemOwner.HasValue)
            {
                var superior = CompanyService.GetCompanyInfo(individual.OemOwner.Value);
                relation = new Domain.Relationship(superior, company, RelationshipType.Distribution);
            }
            else
            {
                relation = new Domain.Relationship(Platform.Instance, company, RelationshipType.ServiceProvide);
            }

            CompanyParameter companyParameter = null;
            WorkingHours work = null;
            if (individual.CompanyType == CompanyType.Supplier)
            {
                companyParameter = CreateSupplierParameter();
                work = CreateSupplierWorkingHours();
            }
            var accountNo = string.IsNullOrEmpty(accountInfo.PoolPayUserName) ? accountInfo.AccountNo : accountInfo.PoolPayUserName;
            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            AccountDTO account = new AccountDTO()
                {
                    AccountNo = accountNo,
                    AdministorCertId = individual.CertNo,
                    AdministorName = individual.AccountName,
                    ContactPhone = individual.Phone,
                    CreationDate = DateTime.Now,
                    LoginPassword = accountInfo.Password,
                    PayPassword = accountInfo.Password
                };
            ChinaPay.PoolPay.Service.AccountBaseService.B3BPersonAccountOpening(account);
            using (var tran = new TransactionScope())
            {
                contact.Insert();
                company.Insert(true);
                admin.Insert();
                if (companyParameter != null)
                {
                    companyParameter.Company = company.Id;
                    companyParameter.Insert();
                }
                if (work != null)
                {
                    work.Company = company.Id;
                    work.Insert();
                }
                if (relation != null)
                    relation.Save();
                payAccount.Insert();
                AddPosApply(company.Id, individual.IsNeedApply);
                tran.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(individual.Phone, accountInfo.AccountNo, domainName, servicePhone, platformName);
        }

        /// <summary>
        /// 注册企业账户
        /// </summary>
        /// <param name="accountInfo">账户信息</param>
        /// <param name="enterprise">企业基础信息</param>
        /// <returns></returns>
        public static void Register(AccountInfo accountInfo, AccountBasicEnterprise enterprise, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentException("accountInfo");
            if (enterprise == null)
                throw new ArgumentException("enterprise");
            if (CompanyService.ExistsCompanyName(enterprise.AccountName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位名称\"。");
            if (CompanyService.ExistsAbbreviateName(enterprise.AbbreviateName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位简称\"。");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");

            var company = CreateCompany(enterprise);
            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;

            var contact = CreateContact(enterprise);
            company.Contact = contact.Id;
            var manager = CreateManager(enterprise);
            company.Manager = manager.Id;
            var emergerncyContact = CreateEmergencyContact(enterprise);
            company.EmergencyContact = emergerncyContact.Id;


            var admin = CreateAdministrator(accountInfo, enterprise);
            admin.Owner = company.Id;
            Domain.Relationship relation = null;
            if (enterprise.OemOwner.HasValue)
            {
                var superior = CompanyService.GetCompanyInfo(enterprise.OemOwner.Value);
                relation = new Domain.Relationship(superior, company, RelationshipType.Distribution);
            }
            else
            {
                relation = new Domain.Relationship(Platform.Instance, company, RelationshipType.ServiceProvide);
            }

            //var relation = new Domain.Relationship(Platform.Instance, company, RelationshipType.ServiceProvide);

            CompanyParameter companyParameter = null;
            WorkingHours work = null;
            WorkingSetting workSetting = null;
            if (enterprise.CompanyType == CompanyType.Provider)
            {
                companyParameter = CreateProviderParameter();
                work = CreateProvideWorkingHours();
                workSetting = CreateProvideWorkingSetting();
            }
            if (enterprise.CompanyType == CompanyType.Supplier)
            {
                companyParameter = CreateSupplierParameter();
                work = CreateSupplierWorkingHours();
            }
            var accountNo = string.IsNullOrEmpty(accountInfo.PoolPayUserName) ? accountInfo.AccountNo : accountInfo.PoolPayUserName;
            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            if (accountInfo.IsPersonAccountNo)
            {
                AccountDTO account = new AccountDTO()
                {
                    AccountNo = accountNo,
                    AdministorCertId = enterprise.IDCard,
                    AdministorName = enterprise.ContactName,
                    ContactPhone = enterprise.ContactPhone,
                    CreationDate = DateTime.Now,
                    LoginPassword = accountInfo.Password,
                    PayPassword = accountInfo.Password
                };
                ChinaPay.PoolPay.Service.AccountBaseService.B3BPersonAccountOpening(account);
            }
            else
            {
                EnterpriseAccountDTO account = new EnterpriseAccountDTO()
                {
                    AccountNo = accountNo,
                    AdministorName = enterprise.ContactName,
                    LoginPassword = accountInfo.Password,
                    PayPassword = accountInfo.Password,
                    OrganizationCode = enterprise.OrginationCode,
                    CompanyName = enterprise.AccountName,
                    LegalContactPhone = enterprise.ContactPhone,
                    LegalPersonName = enterprise.ContactName
                };
                ChinaPay.PoolPay.Service.AccountBaseService.B3BEnterpriseAccountOpening(account);
            }
            using (var tran = new TransactionScope())
            {
                contact.Insert();
                emergerncyContact.Insert();
                manager.Insert();
                company.Insert(true);
                admin.Insert();
                if (companyParameter != null)
                {
                    companyParameter.Company = company.Id;
                    companyParameter.Insert();
                }
                if (work != null)
                {
                    work.Company = company.Id;
                    work.Insert();
                }
                if (workSetting != null)
                {
                    workSetting.Company = company.Id;
                    workSetting.Insert();
                }
                if (relation != null)
                    relation.Save();
                payAccount.Insert();
                AddPosApply(company.Id, enterprise.IsNeedApply);
                tran.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(enterprise.ContactPhone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 为指定的公司开设下级组织机构
        /// </summary>
        /// <param name="superiorId">要开设下级组织机构的上级公司的 Id</param>
        /// <param name="accountInfo">下级公司账户信息。</param>
        /// <param name="individual">下级公司信息。</param>
        public static void CreateSubordinate(Guid superiorId, AccountInfo accountInfo, AccountIndividual individual, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentNullException("accountInfo");
            if (individual == null)
                throw new ArgumentNullException("individual");
            if (individual.CompanyType != CompanyType.Purchaser)
                throw new ArgumentNullException("注册下级组织机构只能是采购方");
            var sup = DataContext.Companies.Where(c => c.Id == superiorId && c.Type == CompanyType.Provider).Select(c => new Company { Id = c.Id, Type = c.Type }).FirstOrDefault();
            if (sup == null)
                throw new InvalidOperationException("指定的公司不存在或其类型不能添加下级机构。");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");

            var company = CreateCompany(individual);
            var admin = CreateAdministrator(accountInfo, individual);
            var contact = CreateContact(individual);
            var address = CreateAddress(individual);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = contact.Id;
            company.EmergencyContact = contact.Id;
            company.Address = address.Id;
            admin.Owner = company.Id;


            var relation = new Domain.Relationship(sup, company, RelationshipType.Organization);

            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountInfo.AccountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            var addressInfo = QueryAddress(individual.Province, individual.City, individual.District);
            AccountDTO account = new AccountDTO()
            {
                AccountNo = accountInfo.AccountNo,
                AdministorCertId = individual.CertNo,
                AdministorName = individual.AccountName,
                ContactPhone = individual.Phone,
                CreationDate = DateTime.Now,
                LoginPassword = accountInfo.Password,
                PayPassword = accountInfo.Password,
                Email = individual.Email,
                OwnerState = addressInfo.ProvinceName,
                OwnerCity = addressInfo.CityName,
                OwnerZone = addressInfo.CountyName,
                OwnerStreet = individual.Address
            };
            if (!string.IsNullOrWhiteSpace(individual.Address))
                account.OwnerStreet = individual.Address;
            if (!string.IsNullOrWhiteSpace(individual.ZipCode))
                account.PostalCode = individual.ZipCode;

            ChinaPay.PoolPay.Service.AccountBaseService.B3BPersonAccountOpening(account);
            using (var trans = new TransactionScope())
            {
                company.Insert();
                admin.Insert();
                contact.Insert();
                address.Insert();
                relation.Save();
                payAccount.Insert();
                trans.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(individual.Phone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 为指定的公司开设下级组织机构
        /// </summary>
        /// <param name="superiorId">要开设下级组织机构的上级公司的 Id</param>
        /// <param name="accountInfo">下级公司账户信息。</param>
        /// <param name="enterprise">下级公司信息。</param>
        public static void CreateSubordinate(Guid superiorId, AccountInfo accountInfo, AccountEnterprise enterprise, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentNullException("accountInfo");
            if (enterprise == null)
                throw new ArgumentNullException("enterprise");
            if (enterprise.CompanyType != CompanyType.Purchaser)
                throw new ArgumentNullException("注册下级组织机构只能是采购方");
            var sup = DataContext.Companies.Where(c => c.Id == superiorId && c.Type == CompanyType.Provider).Select(c => new Company { Id = c.Id, Type = c.Type }).FirstOrDefault();
            if (sup == null)
                throw new InvalidOperationException("指定的公司不存在或其类型不能添加下级机构。");
            if (CompanyService.ExistsCompanyName(enterprise.AccountName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位名称\"。");
            if (CompanyService.ExistsAbbreviateName(enterprise.AbbreviateName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位简称\"。");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");

            var company = CreateCompany(enterprise);
            var admin = CreateAdministrator(accountInfo, enterprise);
            var contact = CreateContact(enterprise);
            var manager = CreateManager(enterprise);
            var emergencyContact = CreateEmergencyContact(enterprise);
            var address = CreateAddress(enterprise);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = manager.Id;
            company.EmergencyContact = emergencyContact.Id;
            company.Address = address.Id;
            admin.Owner = company.Id;

            var relation = new Domain.Relationship(sup, company, RelationshipType.Organization);

            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountInfo.AccountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            var addressInfo = QueryAddress(enterprise.Province, enterprise.City, enterprise.District);
            EnterpriseAccountDTO account = new EnterpriseAccountDTO()
            {
                AccountNo = accountInfo.AccountNo,
                AdministorName = enterprise.ContactName,
                LoginPassword = accountInfo.Password,
                PayPassword = accountInfo.Password,
                OrganizationCode = enterprise.OrginationCode,
                CompanyName = enterprise.AccountName,
                LegalContactPhone = enterprise.ContactPhone,
                LegalPersonName = enterprise.ContactName,
                AdministorCertId = enterprise.LegalCertNo,
                ContactPhone = enterprise.ContactPhone,
                Email = enterprise.Email,
                LegalPersonCertId = enterprise.LegalCertNo,
                OwnerCity = addressInfo.CityName,
                OwnerState = addressInfo.ProvinceName,
                OwnerZone = addressInfo.CountyName,
                OwnerStreet = enterprise.Address,
                CreationDate = DateTime.Now
            };
            if (!string.IsNullOrWhiteSpace(enterprise.Address))
                account.OwnerStreet = enterprise.Address;
            if (!string.IsNullOrWhiteSpace(enterprise.ZipCode))
                account.PostalCode = enterprise.ZipCode;

            ChinaPay.PoolPay.Service.AccountBaseService.B3BEnterpriseAccountOpening(account);
            using (var trans = new TransactionScope())
            {
                company.Insert();
                admin.Insert();
                contact.Insert();
                manager.Insert();
                emergencyContact.Insert();
                relation.Save();
                address.Insert();
                payAccount.Insert();
                trans.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(enterprise.ContactPhone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 为指定的公司开设下级采购。
        /// </summary>
        /// <param name="superiorId">要开设下级采购的上级公司的 Id</param>
        /// <param name="accountInfo">下级采购的账户信息。</param>
        /// <param name="enterprise">下级采购的信息。</param>
        public static void CreatePurchase(Guid superiorId, AccountInfo accountInfo, AccountIndividual individual, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentNullException("accountInfo");
            if (individual == null)
                throw new ArgumentNullException("individual");
            if (individual.CompanyType != CompanyType.Purchaser)
                throw new ArgumentNullException("注册下级采购只能是采购方");
            var sup = DataContext.Companies.Where(c => c.Id == superiorId && (c.Type == CompanyType.Provider || c.IsOem)).Select(c
                => new Company { Id = c.Id, Type = c.Type }).SingleOrDefault();
            if (sup == null)
                throw new InvalidOperationException("指定公司不存在或其类型不允许开下级采购。");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");

            var company = CreateCompany(individual);
            var admin = CreateAdministrator(accountInfo, individual);
            var contact = CreateContact(individual);
            var address = CreateAddress(individual);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = contact.Id;
            company.EmergencyContact = contact.Id;
            company.Address = address.Id;
            admin.Owner = company.Id;

            var relation = new Domain.Relationship(sup, company, RelationshipType.Distribution);

            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountInfo.AccountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            var addressInfo = QueryAddress(individual.Province, individual.City, individual.District);
            AccountDTO account = new AccountDTO()
            {
                AccountNo = accountInfo.AccountNo,
                AdministorCertId = individual.CertNo,
                AdministorName = individual.AccountName,
                ContactPhone = individual.Phone,
                CreationDate = DateTime.Now,
                LoginPassword = accountInfo.Password,
                PayPassword = accountInfo.Password,
                Email = individual.Email,
                OwnerState = addressInfo.ProvinceName,
                OwnerCity = addressInfo.CityName,
                OwnerZone = addressInfo.CountyName,
                OwnerStreet = individual.Address
            };
            if (!string.IsNullOrWhiteSpace(individual.Address))
                account.OwnerStreet = individual.Address;
            if (!string.IsNullOrWhiteSpace(individual.ZipCode))
                account.PostalCode = individual.ZipCode;

            ChinaPay.PoolPay.Service.AccountBaseService.B3BPersonAccountOpening(account);
            using (var trans = new TransactionScope())
            {
                company.Insert();
                admin.Insert();
                contact.Insert();
                address.Insert();
                relation.Save();
                payAccount.Insert();
                trans.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(individual.Phone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 为指定的公司开设下级采购。
        /// </summary>
        /// <param name="superiorId">要开设下级采购的上级公司的 Id</param>
        /// <param name="accountInfo">下级采购的账户信息。</param>
        /// <param name="enterprise">下级采购的信息。</param>
        public static void CreatePurchase(Guid superiorId, AccountInfo accountInfo, AccountEnterprise enterprise, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentNullException("accountInfo");
            if (enterprise == null)
                throw new ArgumentNullException("enterprise");
            if (enterprise.CompanyType != CompanyType.Purchaser)
                throw new ArgumentNullException("注册下级采购只能是采购方");
            var sup = DataContext.Companies.Where(c => c.Id == superiorId && (c.Type == CompanyType.Provider || c.IsOem)).Select(c
                => new Company { Id = c.Id, Type = c.Type }).SingleOrDefault();
            if (sup == null)
                throw new InvalidOperationException("指定公司不存在或其类型不允许开下级采购。");
            if (CompanyService.ExistsCompanyName(enterprise.AccountName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位名称\"。");
            if (CompanyService.ExistsAbbreviateName(enterprise.AbbreviateName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位简称\"。");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");

            var company = CreateCompany(enterprise);
            var admin = CreateAdministrator(accountInfo, enterprise);
            var contact = CreateContact(enterprise);
            var manager = CreateManager(enterprise);
            var emergencyContact = CreateEmergencyContact(enterprise);
            var address = CreateAddress(enterprise);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = manager.Id;
            company.EmergencyContact = emergencyContact.Id;
            company.Address = address.Id;
            admin.Owner = company.Id;

            var relation = new Domain.Relationship(sup, company, RelationshipType.Distribution);

            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountInfo.AccountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            var addressInfo = QueryAddress(enterprise.Province, enterprise.City, enterprise.District);
            EnterpriseAccountDTO account = new EnterpriseAccountDTO()
            {
                AccountNo = accountInfo.AccountNo,
                AdministorName = enterprise.ContactName,
                LoginPassword = accountInfo.Password,
                PayPassword = accountInfo.Password,
                OrganizationCode = enterprise.OrginationCode,
                CompanyName = enterprise.AccountName,
                LegalContactPhone = enterprise.ContactPhone,
                LegalPersonName = enterprise.ContactName,
                AdministorCertId = enterprise.LegalCertNo,
                ContactPhone = enterprise.ContactPhone,
                Email = enterprise.Email,
                LegalPersonCertId = enterprise.LegalCertNo,
                OwnerCity = addressInfo.CityName,
                OwnerState = addressInfo.ProvinceName,
                OwnerZone = addressInfo.CountyName,
                OwnerStreet = enterprise.Address,
                CreationDate = DateTime.Now
            };
            if (!string.IsNullOrWhiteSpace(enterprise.Address))
                account.OwnerStreet = enterprise.Address;
            if (!string.IsNullOrWhiteSpace(enterprise.ZipCode))
                account.PostalCode = enterprise.ZipCode;

            ChinaPay.PoolPay.Service.AccountBaseService.B3BEnterpriseAccountOpening(account);
            using (var trans = new TransactionScope())
            {
                company.Insert();
                admin.Insert();
                contact.Insert();
                manager.Insert();
                emergencyContact.Insert();
                address.Insert();
                relation.Save();
                payAccount.Insert();
                trans.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(enterprise.ContactPhone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 推广指定的公司成为平台用户。
        /// </summary>
        /// <param name="initiatorId">推广方。</param>
        /// <param name="accountInfo">被推广方的账户信息。</param>
        /// <param name="individual">被推广方的个人信息。</param>
        public static void Spread(Guid initiatorId, AccountInfo accountInfo, AccountIndividual individual, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentNullException("accountInfo");
            if (individual == null)
                throw new ArgumentNullException("individual");
            if (individual.CompanyType == CompanyType.Provider)
                throw new ArgumentNullException("注册个人账户不能开出票方类型账户");
            var initiator = DataContext.Companies.Where(c => c.Id == initiatorId && c.Type != CompanyType.Platform).Select(c => new Company { Id = c.Id, Type = c.Type }).SingleOrDefault();
            if (initiator == null)
                throw new InvalidOperationException("指定的推广发起方不存在。");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");

            var company = CreateCompany(individual);
            var admin = CreateAdministrator(accountInfo, individual);
            var contact = CreateContact(individual);
            var address = CreateAddress(individual);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = contact.Id;
            company.EmergencyContact = contact.Id;
            company.Address = address.Id;
            admin.Owner = company.Id;

            var relationSpread = new Domain.Relationship(initiator, company, RelationshipType.Spread);
            var relationServiceProvide = new Domain.Relationship(Platform.Instance, company, RelationshipType.ServiceProvide);

            CompanyParameter parameter = null;
            WorkingHours work = null;
            if (company.Type == CompanyType.Supplier)
            {
                parameter = CreateSupplierParameter();
                work = CreateSupplierWorkingHours();
            }
            var accountNo = string.IsNullOrEmpty(accountInfo.PoolPayUserName) ? accountInfo.AccountNo : accountInfo.PoolPayUserName;
            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            var addressInfo = QueryAddress(individual.Province, individual.City, individual.District);
            AccountDTO account = new AccountDTO()
            {
                AccountNo = accountNo,
                AdministorCertId = individual.CertNo,
                AdministorName = individual.AccountName,
                ContactPhone = individual.Phone,
                CreationDate = DateTime.Now,
                LoginPassword = accountInfo.Password,
                PayPassword = accountInfo.Password,
                Email = individual.Email,
                OwnerState = addressInfo.ProvinceName,
                OwnerCity = addressInfo.CityName,
                OwnerZone = addressInfo.CountyName,
                OwnerStreet = individual.Address
            };
            if (!string.IsNullOrWhiteSpace(individual.Address))
                account.OwnerStreet = individual.Address;
            if (!string.IsNullOrWhiteSpace(individual.ZipCode))
                account.PostalCode = individual.ZipCode;

            ChinaPay.PoolPay.Service.AccountBaseService.B3BPersonAccountOpening(account);
            using (var trans = new TransactionScope())
            {
                company.Insert();
                admin.Insert();
                contact.Insert();
                address.Insert();
                payAccount.Insert();
                relationSpread.Save();
                relationServiceProvide.Save();

                if (parameter != null)
                {
                    parameter.Company = company.Id;
                    parameter.Insert();
                }
                if (work != null)
                {
                    work.Company = company.Id;
                    work.Insert();
                }
                trans.Complete();
            }
            ChinaPay.B3B.Service.Integral.IntegralServer.OpenAccountIntegral(company.Id);
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(individual.Phone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 推广指定的公司成为平台用户。
        /// </summary>
        /// <param name="initiatorId">推广方。</param>
        /// <param name="accountInfo">被推广方的账户信息。</param>
        /// <param name="individual">被推广方的公司信息。</param>
        public static void Spread(Guid initiatorId, AccountInfo accountInfo, AccountEnterprise enterprise, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentNullException("accountInfo");
            if (enterprise == null)
                throw new ArgumentNullException("enterprise");
            var initiator = DataContext.Companies.Where(c => c.Id == initiatorId && c.Type != CompanyType.Platform).Select(c => new Company { Id = c.Id, Type = c.Type }).SingleOrDefault();
            if (initiator == null)
                throw new InvalidOperationException("指定推广发起方不存在。");
            if (CompanyService.ExistsCompanyName(enterprise.AccountName))
            {
                throw new InvalidOperationException("系统中已存在指定的单位名称。");
            }
            if (CompanyService.ExistsAbbreviateName(enterprise.AbbreviateName))
            {
                throw new InvalidOperationException("系统中已存在指定的单位简称。");
            }
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
            {
                throw new InvalidOperationException("系统中已存在指定的账号。");
            }

            var company = CreateCompany(enterprise);
            var admin = CreateAdministrator(accountInfo, enterprise);
            var contact = CreateContact(enterprise);
            var manager = CreateManager(enterprise);
            if (string.IsNullOrWhiteSpace(manager.Name))
                manager.Name = contact.Name;
            if (string.IsNullOrWhiteSpace(manager.Cellphone))
                manager.Cellphone = contact.Cellphone;
            if (string.IsNullOrWhiteSpace(manager.OfficePhone))
                manager.OfficePhone = contact.OfficePhone;
            if (string.IsNullOrWhiteSpace(manager.Email))
                manager.Email = contact.Email;
            var emergencyContact = CreateEmergencyContact(enterprise);
            if (string.IsNullOrWhiteSpace(emergencyContact.Name))
                emergencyContact.Name = contact.Name;
            if (string.IsNullOrWhiteSpace(emergencyContact.Cellphone))
                emergencyContact.Cellphone = contact.Cellphone;
            if (string.IsNullOrWhiteSpace(emergencyContact.OfficePhone))
                emergencyContact.OfficePhone = contact.OfficePhone;
            if (string.IsNullOrWhiteSpace(emergencyContact.Email))
                emergencyContact.Email = contact.Email;
            Address address = null;
            if (!string.IsNullOrWhiteSpace(enterprise.Province))
                address = CreateAddress(enterprise);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = manager.Id;
            company.EmergencyContact = emergencyContact.Id;
            if (address != null)
                company.Address = address.Id;

            admin.Owner = company.Id;

            var relationSpread = new Domain.Relationship(initiator, company, RelationshipType.Spread);
            var relationServiceProvide = new Domain.Relationship(Platform.Instance, company, RelationshipType.ServiceProvide);

            CompanyParameter parameter = null;
            WorkingHours work = null;
            WorkingSetting workSetting = null;
            if (company.Type == CompanyType.Provider)
            {
                parameter = CreateProviderParameter();
                work = CreateProvideWorkingHours();
                workSetting = CreateProvideWorkingSetting();
            }
            if (company.Type == CompanyType.Supplier)
            {
                parameter = CreateSupplierParameter();
                work = CreateSupplierWorkingHours();
            }
            var accountNo = string.IsNullOrEmpty(accountInfo.PoolPayUserName) ? accountInfo.AccountNo : accountInfo.PoolPayUserName;
            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            if (accountInfo.IsPersonAccountNo)
            {
                AccountDTO account = new AccountDTO()
                {
                    AccountNo = accountNo,
                    AdministorCertId = enterprise.LegalCertNo,
                    AdministorName = enterprise.ContactName,
                    ContactPhone = enterprise.ContactPhone,
                    CreationDate = DateTime.Now,
                    LoginPassword = accountInfo.Password,
                    PayPassword = accountInfo.Password
                };
                ChinaPay.PoolPay.Service.AccountBaseService.B3BPersonAccountOpening(account);
            }
            else
            {


                EnterpriseAccountDTO account = new EnterpriseAccountDTO()
                {
                    AccountNo = accountNo,
                    AdministorName = enterprise.ContactName,
                    LoginPassword = accountInfo.Password,
                    PayPassword = accountInfo.Password,
                    OrganizationCode = enterprise.OrginationCode,
                    CompanyName = enterprise.AccountName,
                    LegalContactPhone = enterprise.ContactPhone,
                    LegalPersonName = enterprise.ContactName,
                    AdministorCertId = enterprise.LegalCertNo,
                    ContactPhone = enterprise.ContactPhone,
                    LegalPersonCertId = enterprise.LegalCertNo,
                    CreationDate = DateTime.Now
                };
                if (!string.IsNullOrWhiteSpace(enterprise.Email))
                    account.Email = enterprise.Email;
                if (!string.IsNullOrWhiteSpace(enterprise.Province) && !string.IsNullOrWhiteSpace(enterprise.City) && !string.IsNullOrWhiteSpace(enterprise.District))
                {
                    var addressInfo = QueryAddress(enterprise.Province, enterprise.City, enterprise.District);
                    account.OwnerState = addressInfo.ProvinceName;
                    account.OwnerCity = addressInfo.CityName;
                    account.OwnerZone = addressInfo.CountyName;
                }
                if (!string.IsNullOrWhiteSpace(enterprise.Address))
                    account.OwnerStreet = enterprise.Address;
                if (!string.IsNullOrWhiteSpace(enterprise.ZipCode))
                    account.PostalCode = enterprise.ZipCode;
                ChinaPay.PoolPay.Service.AccountBaseService.B3BEnterpriseAccountOpening(account);
            }

            using (var tran = new TransactionScope())
            {
                company.Insert();
                contact.Insert();
                manager.Insert();
                emergencyContact.Insert();
                if (address != null)
                    address.Insert();
                admin.Insert();
                if (parameter != null)
                {
                    parameter.Company = company.Id;
                    parameter.Insert();
                }
                if (work != null)
                {
                    work.Company = company.Id;
                    work.Insert();
                }
                if (workSetting != null)
                {
                    workSetting.Company = company.Id;
                    workSetting.Insert();
                }
                relationSpread.Save();
                relationServiceProvide.Save();
                payAccount.Insert();
                tran.Complete();
            }
            ChinaPay.B3B.Service.Integral.IntegralServer.OpenAccountIntegral(company.Id);
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(enterprise.ContactPhone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 平台个人开户
        /// </summary>
        /// <param name="accountInfo">账户信息</param>
        /// <param name="individual">个人账户信息</param>
        public static void Establish(AccountInfo accountInfo, AccountIndividual individual, string domainName, string servicePhone, string plateformName)
        {

            if (accountInfo == null) throw new ArgumentException("accountInfo");
            if (individual == null) throw new ArgumentException("individual");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");
            if (individual.CompanyType == CompanyType.Provider)
                throw new ArgumentNullException("注册个人账户不能开出票方类型账户");

            var company = CreateCompany(individual);
            var contact = CreateContact(individual);
            var admin = CreateAdministrator(accountInfo, individual);
            var address = CreateAddress(individual);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = contact.Id;
            company.EmergencyContact = contact.Id;
            company.Address = address.Id;
            admin.Owner = company.Id;

            var relation = new Domain.Relationship(Platform.Instance, company, RelationshipType.ServiceProvide);

            CompanyParameter companyParameter = null;
            WorkingHours work = null;
            if (individual.CompanyType == CompanyType.Supplier)
            {
                companyParameter = CreateSupplierParameter();
                work = CreateSupplierWorkingHours();
            }

            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountInfo.AccountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            var addressInfo = QueryAddress(individual.Province, individual.City, individual.District);
            AccountDTO account = new AccountDTO()
            {
                AccountNo = accountInfo.AccountNo,
                AdministorCertId = individual.CertNo,
                AdministorName = individual.AccountName,
                ContactPhone = individual.Phone,
                Email = individual.Email,
                OwnerState = addressInfo.ProvinceName,
                OwnerCity = addressInfo.CityName,
                OwnerZone = addressInfo.CountyName,
                OwnerStreet = individual.Address,
                CreationDate = DateTime.Now,
                LoginPassword = accountInfo.Password,
                PayPassword = accountInfo.Password
            };
            if (!string.IsNullOrWhiteSpace(individual.Address))
                account.OwnerStreet = individual.Address;
            if (!string.IsNullOrWhiteSpace(individual.ZipCode))
                account.PostalCode = individual.ZipCode;
            ChinaPay.PoolPay.Service.AccountBaseService.B3BPersonAccountOpening(account);
            using (var tran = new TransactionScope())
            {
                contact.Insert();
                company.Insert();
                admin.Insert();
                address.Insert();
                if (companyParameter != null)
                {
                    companyParameter.Company = company.Id;
                    companyParameter.Insert();
                }
                if (work != null)
                {
                    work.Company = company.Id;
                    work.Insert();
                }
                relation.Save();
                payAccount.Insert();
                tran.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(individual.Phone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 平台企业开户
        /// </summary>
        /// <param name="accountInfo">账户信息</param>
        /// <param name="enterprise">企业账户信息</param>
        public static void Establish(AccountInfo accountInfo, AccountEnterprise enterprise, string domainName, string servicePhone, string plateformName)
        {
            if (accountInfo == null)
                throw new ArgumentException("accountInfo");
            if (enterprise == null)
                throw new ArgumentException("enterprise");
            if (CompanyService.ExistsCompanyName(enterprise.AccountName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位名称\"。");
            if (CompanyService.ExistsAbbreviateName(enterprise.AbbreviateName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位简称\"。");
            if (EmployeeService.ExistsUserName(accountInfo.AccountNo) || ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(accountInfo.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");

            var company = CreateCompany(enterprise);
            var contact = CreateContact(enterprise);
            var manager = CreateManager(enterprise);
            var emergerncyContact = CreateEmergencyContact(enterprise);
            var admin = CreateAdministrator(accountInfo, enterprise);
            var address = CreateAddress(enterprise);

            company.RegisterTime = DateTime.Now;
            company.EffectTime = DateTime.Now;
            company.Contact = contact.Id;
            company.Manager = manager.Id;
            company.EmergencyContact = emergerncyContact.Id;
            company.Address = address.Id;
            admin.Owner = company.Id;
            var relation = new Domain.Relationship(Platform.Instance, company, RelationshipType.ServiceProvide);

            CompanyParameter companyParameter = null;
            WorkingHours work = null;
            WorkingSetting workSetting = null;
            if (enterprise.CompanyType == CompanyType.Provider)
            {
                companyParameter = CreateProviderParameter();
                work = CreateProvideWorkingHours();
                workSetting = CreateProvideWorkingSetting();
            }
            if (enterprise.CompanyType == CompanyType.Supplier)
            {
                companyParameter = CreateSupplierParameter();
                work = CreateSupplierWorkingHours();
            }
            Account payAccount = new Account()
            {
                Company = company.Id,
                No = accountInfo.AccountNo,
                Time = DateTime.Now,
                Type = AccountType.Payment,
                Valid = true
            };
            var addressInfo = QueryAddress(enterprise.Province, enterprise.City, enterprise.District);
            EnterpriseAccountDTO account = new EnterpriseAccountDTO()
            {
                AccountNo = accountInfo.AccountNo,
                AdministorName = enterprise.ContactName,
                AdministorCertId = enterprise.LegalCertNo,
                LoginPassword = accountInfo.Password,
                PayPassword = accountInfo.Password,
                OrganizationCode = enterprise.OrginationCode,
                CompanyName = enterprise.AccountName,
                LegalContactPhone = enterprise.ContactPhone,
                LegalPersonName = enterprise.ContactName,
                LegalPersonCertId = enterprise.LegalCertNo,
                ContactPhone = enterprise.ContactPhone,
                Email = enterprise.Email,
                OwnerCity = addressInfo.CityName,
                OwnerState = addressInfo.ProvinceName,
                OwnerZone = addressInfo.CountyName,
                OwnerStreet = enterprise.Address,
                CreationDate = DateTime.Now
            };
            if (!string.IsNullOrWhiteSpace(enterprise.Address))
                account.OwnerStreet = enterprise.Address;
            if (!string.IsNullOrWhiteSpace(enterprise.ZipCode))
                account.PostalCode = enterprise.ZipCode;
            ChinaPay.PoolPay.Service.AccountBaseService.B3BEnterpriseAccountOpening(account);
            using (var tran = new TransactionScope())
            {
                contact.Insert();
                emergerncyContact.Insert();
                manager.Insert();
                company.Insert();
                address.Insert();
                admin.Insert();
                if (companyParameter != null)
                {
                    companyParameter.Company = company.Id;
                    companyParameter.Insert();
                }
                if (work != null)
                {
                    work.Company = company.Id;
                    work.Insert();
                }
                if (workSetting != null)
                {
                    workSetting.Company = company.Id;
                    workSetting.Insert();
                }
                relation.Save();
                payAccount.Insert();
                tran.Complete();
            }
            var com = SMSCompanySmsParamService.Query(AccountType.Payment, company.Id);
            if (com == null)
            {
                //默认绑定账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = payAccount.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = accountInfo.AccountNo, AccountType = AccountType.Payment });
            }
            SMSSendService.SnedB3bRegisterSuccess(enterprise.ContactPhone, accountInfo.AccountNo, domainName, servicePhone, plateformName);
        }

        /// <summary>
        /// 补填个人信息(基础信息修改也可调用)
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="info">补填的信息</param>
        /// <returns></returns>
        public static void AddPurchaseInfo(Guid companyId, PurchaseIndividualInfo info, string operatorAccount)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(companyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            var upadateCompanyInfo = CreateCompany(company, info);


            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                upadateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                upadateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            upadateCompanyInfo.Contact = company.Contact;
            if (!string.IsNullOrWhiteSpace(info.OfficePhone))
                upadateCompanyInfo.OfficePhones = info.OfficePhone;

            upadateCompanyInfo.Manager = company.Manager;
            upadateCompanyInfo.EmergencyContact = company.EmergencyContact;

            using (var trans = new TransactionScope())
            {
                upadateCompanyInfo.Update();
                address.InsertOrUpdate();
                DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                trans.Complete();
            }
            saveLog(OperationType.Update, string.Format("修改公司Id为{0}的公司基础信息", companyId), OperatorRole.User, companyId.ToString(), operatorAccount);
        }

        /// <summary>
        /// 补填企业信息（基础信息修改）
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="info">补填信息</param>
        /// <returns></returns>
        public static void AddPurchaseInfo(Guid companyId, PurchaseEnterpriseInfo info, string operatorAccount)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(companyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            var updateCompanyInfo = CreateCompany(company, info);

            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                updateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                updateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            updateCompanyInfo.Contact = company.Contact;

            var orginalEmergencyContact = GetContact(company.EmergencyContact.Value);
            if (orginalEmergencyContact == null)
                throw new InvalidOperationException("缺少紧急联系人信息。");
            var emergerncy = CreateEmergencyContact(orginalEmergencyContact, info);
            updateCompanyInfo.EmergencyContact = company.EmergencyContact;

            var orginalManager = GetContact(company.Manager.Value);
            if (orginalManager == null)
                throw new InvalidOperationException("缺少负责人信息。");
            var manager = CreateManager(orginalManager, info);
            updateCompanyInfo.Manager = company.Manager;
            if (!string.IsNullOrWhiteSpace(info.CompanyPhone))
                updateCompanyInfo.OfficePhones = info.CompanyPhone;

            using (var trans = new TransactionScope())
            {
                updateCompanyInfo.Update();
                address.InsertOrUpdate();
                DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                DataContext.Contacts.Update(emergerncy, c => c.Id == company.EmergencyContact);
                DataContext.Contacts.Update(manager, c => c.Id == company.Manager);
                trans.Complete();
            }
            saveLog(OperationType.Update, string.Format("修改公司Id为{0}的公司基础信息", companyId), OperatorRole.User, companyId.ToString(), operatorAccount);
        }

        /// <summary>
        /// 平台修改公司基础信息（个人）
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateIndividualInfo(CompanyIndividualUpdateInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(info.CompanyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            var upadateCompanyInfo = CreateCompany(company, info);
            Employee emp = null;
            if (company.AccountType == AccountBaseType.Individual && company.Name != info.Name)
            {
                emp = EmployeeService.QueryCompanyAdmin(info.CompanyId);
                emp.Name = info.Name;
            }

            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                upadateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                upadateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            upadateCompanyInfo.Contact = company.Contact;

            upadateCompanyInfo.Manager = company.Manager;
            upadateCompanyInfo.EmergencyContact = company.EmergencyContact;

            using (var trans = new TransactionScope())
            {
                upadateCompanyInfo.Update();
                if (emp != null) emp.Update();
                address.InsertOrUpdate();
                DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                trans.Complete();
            }
            saveLog(OperationType.Update, string.Format("修改公司Id为{0}的公司基础信息", info.CompanyId), OperatorRole.Platform, info.CompanyId.ToString(), info.OperatorAccount);
        }

        /// <summary>
        /// 平台修改公司基础信息(企业)
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateEnterpriseInfo(CompanyEnterpriseUpdateInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(info.CompanyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            if (company.Name != info.CompanyName && CompanyService.ExistsCompanyName(info.CompanyName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位名称\"。");
            if (company.AbbreviateName != info.AbbreviateName && CompanyService.ExistsAbbreviateName(info.AbbreviateName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位简称\"。");
            var updateCompanyInfo = CreateCompany(company, info);

            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                updateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                updateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            updateCompanyInfo.Contact = company.Contact;

            var orginalEmergencyContact = GetContact(company.EmergencyContact.Value);
            if (orginalEmergencyContact == null)
                throw new InvalidOperationException("缺少紧急联系人信息。");
            var emergerncy = CreateEmergencyContact(orginalEmergencyContact, info);
            updateCompanyInfo.EmergencyContact = company.EmergencyContact;

            var orginalManager = GetContact(company.Manager.Value);
            if (orginalManager == null)
                throw new InvalidOperationException("缺少负责人信息。");
            var manager = CreateManager(orginalManager, info);
            updateCompanyInfo.Manager = company.Manager;
            if (!string.IsNullOrWhiteSpace(info.CompanyPhone))
                updateCompanyInfo.OfficePhones = info.CompanyPhone;

            using (var trans = new TransactionScope())
            {
                updateCompanyInfo.Update();
                address.InsertOrUpdate();
                DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                DataContext.Contacts.Update(emergerncy, c => c.Id == company.EmergencyContact);
                DataContext.Contacts.Update(manager, c => c.Id == company.Manager);
                trans.Complete();
            }
            saveLog(OperationType.Update, string.Format("修改公司Id为{0}的公司基础信息", info.CompanyId), OperatorRole.Platform, info.CompanyId.ToString(), info.OperatorAccount);
        }

        /// <summary>
        /// 分销OEM修改公司基础信息（个人）
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateIndividualInfo(DistributionOEMUserIndividualUpdateInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(info.CompanyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            var upadateCompanyInfo = CreateCompany(company, info);
            Employee emp = null;
            if (company.AccountType == AccountBaseType.Individual && company.Name != info.Name)
            {
                emp = EmployeeService.QueryCompanyAdmin(info.CompanyId);
                emp.Name = info.Name;
            }

            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                upadateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                upadateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            upadateCompanyInfo.Contact = company.Contact;

            upadateCompanyInfo.Manager = company.Manager;
            upadateCompanyInfo.EmergencyContact = company.EmergencyContact;

            using (var trans = new TransactionScope())
            {
                IncomeGroupService.UpdateIncomeGroupRelation(info.OrginalIncomeGroupId, info.IncomeGroupId, info.CompanyId);
                upadateCompanyInfo.Update();
                if (emp != null) emp.Update();
                address.InsertOrUpdate();
                DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                trans.Complete();
            }
            saveLog(OperationType.Update, string.Format("修改公司Id为{0}的公司基础信息", info.CompanyId), OperatorRole.User, info.CompanyId.ToString(), info.OperatorAccount);
            saveUpdateLog("收益组信息", string.Format("公司Id:{0},收益组Id:{1}", info.CompanyId, info.OrginalIncomeGroupId.HasValue ? info.OrginalIncomeGroupId.ToString() : "未分组"),
                string.Format("公司Id:{0},收益组Id:{1}", info.CompanyId, info.IncomeGroupId.HasValue ? info.IncomeGroupId.ToString() : "未分组状态"),
                OperatorRole.User, info.CompanyId.ToString(), info.OperatorAccount);
        }

        /// <summary>
        /// 分销OEM修改公司基础信息(企业)
        /// </summary>
        /// <param name="info"></param>
        public static void UpdateEnterpriseInfo(DistributionOEMUserEnterpriseUpdateInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(info.CompanyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            if (company.Name != info.CompanyName && CompanyService.ExistsCompanyName(info.CompanyName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位名称\"。");
            if (company.AbbreviateName != info.AbbreviateName && CompanyService.ExistsAbbreviateName(info.AbbreviateName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位简称\"。");
            var updateCompanyInfo = CreateCompany(company, info);

            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                updateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                updateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            updateCompanyInfo.Contact = company.Contact;

            var orginalEmergencyContact = GetContact(company.EmergencyContact.Value);
            if (orginalEmergencyContact == null)
                throw new InvalidOperationException("缺少紧急联系人信息。");
            var emergerncy = CreateEmergencyContact(orginalEmergencyContact, info);
            updateCompanyInfo.EmergencyContact = company.EmergencyContact;

            var orginalManager = GetContact(company.Manager.Value);
            if (orginalManager == null)
                throw new InvalidOperationException("缺少负责人信息。");
            var manager = CreateManager(orginalManager, info);
            updateCompanyInfo.Manager = company.Manager;
            if (!string.IsNullOrWhiteSpace(info.CompanyPhone))
                updateCompanyInfo.OfficePhones = info.CompanyPhone;

            using (var trans = new TransactionScope())
            {
                IncomeGroupService.UpdateIncomeGroupRelation(info.OrginalIncomeGroupId, info.IncomeGroupId, info.CompanyId);
                updateCompanyInfo.Update();
                address.InsertOrUpdate();
                DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                DataContext.Contacts.Update(emergerncy, c => c.Id == company.EmergencyContact);
                DataContext.Contacts.Update(manager, c => c.Id == company.Manager);
                trans.Complete();
            }
            saveLog(OperationType.Update, string.Format("修改公司Id为{0}的公司基础信息", info.CompanyId), OperatorRole.User, info.CompanyId.ToString(), info.OperatorAccount);
            saveUpdateLog("收益组信息", string.Format("公司Id:{0},收益组Id:{1}", info.CompanyId, info.OrginalIncomeGroupId.HasValue ? info.OrginalIncomeGroupId.ToString() : "未分组"),
                string.Format("公司Id:{0},收益组Id:{1}", info.CompanyId, info.IncomeGroupId.HasValue ? info.IncomeGroupId.ToString() : "未分组状态"),
                OperatorRole.User, info.CompanyId.ToString(), info.OperatorAccount);
        }

        /// <summary>
        /// 审核产品方个人（平台）
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="info">产品方个人审核信息</param>
        /// <returns></returns>
        public static void AuditSupplier(Guid companyId, SupplierIndividualAuditInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(companyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");

            var updateCompanyInfo = CreateCompany(company, info);
            WorkingHours workHours = null;
            CompanyParameter companyParameter = null;
            if (info.IsUpgrade)
            {
                updateCompanyInfo.AccountType = AccountBaseType.Individual;
                updateCompanyInfo.Type = CompanyType.Supplier;
                if (company.Type == CompanyType.Purchaser)
                {
                    workHours = CreateSupplierWorkingHours();
                    companyParameter = CreateSupplierParameter();
                    workHours.Company = companyId;
                    companyParameter.Company = companyId;
                }
            }
            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                updateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                updateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            updateCompanyInfo.EmergencyContact = updateCompanyInfo.Manager = updateCompanyInfo.Contact = company.Contact;
            var companyDocument = AccountCombineService.QueryCompanyDocument(companyId);
            if (companyDocument == null)
            {
                companyDocument = new CompanyDocument();
                companyDocument.CertLicense = info.CertLicense;
                companyDocument.IATALicense = info.IATALicense;
                companyDocument.BussinessTime = info.BussinessTime;
                companyDocument.Company = companyId;
            }
            using (var trans = new TransactionScope())
            {
                try
                {
                    if (info.IsUpgrade)
                        CompanyUpgradeService.Enable(companyId, info.OperatorAccount);
                    if (workHours != null)
                        workHours.InsertOrUpdate();
                    if (companyParameter != null)
                        companyParameter.InsertOrUpdate();
                    updateCompanyInfo.Update();
                    address.InsertOrUpdate();
                    CompanyService.Accept(companyId);
                    SaveCompanyDocument(companyDocument);
                    SetEffectTime(companyId, info.EffectBeginTime, info.EffectEndTime);
                    DataContext.Contacts.InsertOrUpdate(contact, c => c.Id == company.Contact);

                    trans.Complete();
                }
                catch (System.Data.Common.DbException ex)
                {
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
                if (info.IsUpgrade)
                    saveUpdateLog("公司升级审核", string.Format("公司Id为{0}的账号的审核状态为未审", companyId), string.Format("公司Id为{0}的账号的审核状态为审核通过", companyId), OperatorRole.Platform, companyId.ToString(), info.OperatorAccount);
            }
        }

        /// <summary>
        /// 审核产品方企业（平台）
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="info">产品方企业审核信息</param>
        /// <returns></returns>
        public static void AuditSupplier(Guid companyId, SupplierEnterpriseAuditInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(companyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            var updateCompanyInfo = CreateCompany(company, info);

            CompanyParameter companyParameter = null;
            WorkingHours workingHours = null;
            if (info.IsUpgrade)
            {
                updateCompanyInfo.AccountType = AccountBaseType.Enterprise;
                updateCompanyInfo.Type = CompanyType.Supplier;
                updateCompanyInfo.Name = info.CompanyName;
                updateCompanyInfo.AbbreviateName = info.AbbreviateName;
                updateCompanyInfo.OrginationCode = info.OrginationCode;
                updateCompanyInfo.OfficePhones = info.OfficePhones;
                if (company.Type == CompanyType.Purchaser)
                {
                    companyParameter = CreateSupplierParameter();
                    workingHours = CreateSupplierWorkingHours();
                    companyParameter.Company = companyId;
                    workingHours.Company = companyId;
                }
                if (company.Type == CompanyType.Provider)
                {
                    companyParameter = CreateSupplierParameter();
                    companyParameter.Company = companyId;
                    var originalCompanyParameter = CompanyService.GetCompanyParameter(companyId);
                    companyParameter.Bloc = originalCompanyParameter.Bloc;
                    companyParameter.BlocRate = originalCompanyParameter.BlocRate;
                    companyParameter.Business = originalCompanyParameter.Business;
                    companyParameter.BusinessRate = originalCompanyParameter.BusinessRate;
                    companyParameter.CostFree = originalCompanyParameter.CostFree;
                    companyParameter.CostFreeRate = originalCompanyParameter.CostFreeRate;
                    companyParameter.Disperse = originalCompanyParameter.Disperse;
                    companyParameter.DisperseRate = originalCompanyParameter.DisperseRate;
                    companyParameter.Singleness = originalCompanyParameter.Singleness;
                    companyParameter.SinglenessRate = originalCompanyParameter.SinglenessRate;
                }
            }
            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                updateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                updateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            updateCompanyInfo.Contact = company.Contact;

            var orginalEmergencyContact = GetContact(company.EmergencyContact.Value);
            if (orginalEmergencyContact == null)
                throw new InvalidOperationException("缺少紧急联系人信息。");
            var emergerncy = CreateEmergencyContact(orginalEmergencyContact, info);
            updateCompanyInfo.EmergencyContact = company.EmergencyContact;

            var orginalManager = GetContact(company.Manager.Value);
            if (orginalManager == null)
                throw new InvalidOperationException("缺少负责人信息。");
            var manager = CreateManager(orginalManager, info);
            updateCompanyInfo.Manager = company.Manager;
            var companyDocument = AccountCombineService.QueryCompanyDocument(companyId);
            if (companyDocument == null)
            {
                companyDocument = new CompanyDocument();
                companyDocument.BussinessLicense = info.BussinessLicense;
                companyDocument.IATALicense = info.IATALicense;
                companyDocument.BussinessTime = info.BussinessTime;
                companyDocument.Company = companyId;
            }
            using (var trans = new TransactionScope())
            {
                try
                {
                    if (workingHours != null)
                        workingHours.InsertOrUpdate();
                    updateCompanyInfo.Update();
                    address.InsertOrUpdate();
                    CompanyService.Accept(companyId);
                    SaveCompanyDocument(companyDocument);
                    SetEffectTime(companyId, info.EffectBeginTime, info.EffectEndTime);
                    DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                    DataContext.Contacts.InsertOrUpdate(emergerncy, c => c.Id == emergerncy.Id);
                    DataContext.Contacts.InsertOrUpdate(manager, c => c.Id == manager.Id);
                    if (info.IsUpgrade)
                        CompanyUpgradeService.Enable(companyId, info.OperatorAccount);
                    if (companyParameter != null)
                        companyParameter.InsertOrUpdate();
                    if (company.Type == CompanyType.Provider)
                        DataContext.WorkingSettings.Delete(c => c.Company == companyId);
                    trans.Complete();
                }
                catch (System.Data.Common.DbException ex)
                {
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
                if (info.IsUpgrade)
                    saveUpdateLog("公司升级审核", string.Format("公司Id为{0}的账号的审核状态为未审", companyId), string.Format("公司Id为{0}的账号的审核状态为审核通过", companyId), OperatorRole.Platform, companyId.ToString(), info.OperatorAccount);
            }
        }

        /// <summary>
        /// 审核出票方（平台）
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="info">出票方审核信息</param>
        /// <returns></returns>
        public static void AuditProviderInfo(Guid companyId, ProviderAuditInfo info)
        {

            if (info == null)
                throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(companyId);
            if (company == null)
                throw new InvalidOperationException("找不到相关单位的信息。");
            var updateCompanyInfo = CreateCompany(company, info);
            CompanyParameter companyParmeter = null;
            WorkingHours workHours = null;
            WorkingSetting workSetting = CreateProvideWorkingSetting();
            workSetting.Company = companyId;
            if (info.IsUpgrade)
            {
                updateCompanyInfo.AccountType = AccountBaseType.Enterprise;
                updateCompanyInfo.Type = CompanyType.Provider;
                updateCompanyInfo.Name = info.CompanyName;
                updateCompanyInfo.AbbreviateName = info.AbbreviateName;
                updateCompanyInfo.OrginationCode = info.OrginationCode;
                updateCompanyInfo.OfficePhones = info.OfficePhones;
                if (company.Type == CompanyType.Purchaser)
                {
                    companyParmeter = CreateProviderParameter();
                    workHours = CreateProvideWorkingHours();
                }
                else
                {
                    if (company.Type == CompanyType.Supplier)
                    {
                        companyParmeter = CompanyService.GetCompanyParameter(companyId);
                        companyParmeter.RefundCountLimit = SystemParamService.DefaultLockPolicyLimit;
                        companyParmeter.FullRefundTimeLimit = SystemParamService.DefaultFullRefundLimit;
                        companyParmeter.RefundTimeLimit = SystemParamService.DefaultVoluntaryRefundLimit;
                        companyParmeter.ProfessionRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForBrother);
                        companyParmeter.SubordinateRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForJunior);
                    }
                }
            }

            var address = CreateAddress(info);
            if (company.Address.HasValue)
            {
                updateCompanyInfo.Address = address.Id = company.Address.Value;
            }
            else
            {
                updateCompanyInfo.Address = company.Address = address.Id;
            }

            var orginalContact = GetContact(company.Contact.Value);
            if (orginalContact == null)
                throw new InvalidOperationException("缺少联系人信息。");
            var contact = CreateContact(orginalContact, info);
            updateCompanyInfo.Contact = company.Contact;

            var orginalEmergencyContact = GetContact(company.EmergencyContact.Value);
            if (orginalEmergencyContact == null)
                throw new InvalidOperationException("缺少紧急联系人信息。");
            var emergerncy = CreateEmergencyContact(orginalEmergencyContact, info);
            updateCompanyInfo.EmergencyContact = company.EmergencyContact;

            var orginalManager = GetContact(company.Manager.Value);
            if (orginalManager == null)
                throw new InvalidOperationException("缺少负责人信息。");
            var manager = CreateManager(orginalManager, info);
            updateCompanyInfo.Manager = company.Manager;

            if (company.Contact == company.Manager)
            {
                updateCompanyInfo.Manager = manager.Id = Guid.NewGuid();
                updateCompanyInfo.EmergencyContact = emergerncy.Id = Guid.NewGuid();
            }

            CompanyDocument companyDocument = QueryCompanyDocument(companyId);
            if (companyDocument == null)
            {
                companyDocument = new CompanyDocument();
                companyDocument.BussinessLicense = info.BusinessLicense;
                companyDocument.IATALicense = info.IATALicense;
                companyDocument.Company = companyId;
            }
            else
            {
                if (info.BusinessLicense != null && info.BusinessLicense.Length > 0)
                    companyDocument.BussinessLicense = info.BusinessLicense;
                if (info.IATALicense != null && info.IATALicense.Length > 0)
                    companyDocument.IATALicense = info.IATALicense;
                companyDocument.CertLicense = null;
                companyDocument.BussinessTime = null;
            }

            using (var trans = new TransactionScope())
            {
                try
                {
                    updateCompanyInfo.Update();
                    address.InsertOrUpdate();
                    workSetting.InsertOrUpdate();
                    if (companyParmeter != null)
                    {
                        companyParmeter.Company = companyId;
                        companyParmeter.InsertOrUpdate();
                    }
                    if (workHours != null)
                    {
                        workHours.Company = companyId;
                        workHours.InsertOrUpdate();
                    }
                    CompanyService.Accept(companyId);
                    SaveCompanyDocument(companyDocument);
                    SetEffectTime(companyId, info.EffectBeginTime, info.EffectEndTime);
                    DataContext.Contacts.Update(contact, c => c.Id == company.Contact);
                    DataContext.Contacts.InsertOrUpdate(emergerncy, c => c.Id == emergerncy.Id);
                    DataContext.Contacts.InsertOrUpdate(manager, c => c.Id == manager.Id);
                    if (info.IsUpgrade)
                        CompanyUpgradeService.Enable(companyId, info.OperatorAccount);
                    trans.Complete();
                }
                catch (System.Data.Common.DbException ex)
                {
                    LogService.SaveExceptionLog(ex);
                    throw new Exception("系统错误", ex);
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex);
                    throw;
                }
                if (info.IsUpgrade)
                    saveUpdateLog("公司升级审核", string.Format("公司Id为{0}的账号的审核状态为未审", companyId), string.Format("公司Id为{0}的账号的审核状态为审核通过", companyId), OperatorRole.Platform, companyId.ToString(), info.OperatorAccount);
            }
        }

        /// <summary>
        /// 注册个人账户的收款账号
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="info">个人账户信息</param>
        /// <returns></returns>
        public static void AddReciveAccount(Guid companyId, AccountDTO info, string operatorAccount)
        {
            if (ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(info.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");
            var receiveAccount = new Service.Organization.Domain.Account(companyId, info.AccountNo, AccountType.Receiving, true, DateTime.Now);
            ChinaPay.PoolPay.Service.AccountBaseService.PersonAccountOpening(info);
            AccountService.Update(receiveAccount.Company, receiveAccount, operatorAccount);
        }

        /// <summary>
        /// 注册企业账户的收款账号
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="info">企业账户信息</param>
        /// <returns></returns>
        public static void AddReciveAccount(Guid companyId, EnterpriseAccountDTO info, string operatorAccount)
        {
            if (ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(info.AccountNo))
                throw new InvalidOperationException("系统中已存在指定的账号。");
            var receiveAccount = new Service.Organization.Domain.Account(companyId, info.AccountNo, AccountType.Receiving, true, DateTime.Now);
            ChinaPay.PoolPay.Service.AccountBaseService.EnterpriseAccountOpening(info);
            AccountService.Update(receiveAccount.Company, receiveAccount, operatorAccount);
        }

        /// <summary>
        /// 设置使用有效时间
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="EffectBeginTime">使用有效开始时间</param>
        /// <param name="EffectEndTime">使用有效结束时间</param>
        public static void SetEffectTime(Guid companyId, DateTime EffectBeginTime, DateTime EffectEndTime)
        {
            DataContext.CompanyParameters.Update(e => new { ValidityStart = EffectBeginTime, ValidityEnd = EffectEndTime }, e => e.Company == companyId);
        }

        /// <summary>
        /// 认证中心列表（2012-11-6）
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static IEnumerable<CompanyAuditInfo> GetNeedAuditCompanies(CompanyAuditQueryCondition condition, Pagination pagination)
        {
            var repository = Factory.CreateCompanyUpgradeRepository();
            return repository.QueryNeedAuditCompanies(condition, pagination);
        }

        /// <summary>
        /// 根据ID查询信誉评级
        /// </summary>
        /// <param name="company">公司ID</param>
        /// <returns></returns>
        public static Dictionary<Guid, CompanyParameter> GetCreditworthiness(IEnumerable<Guid> company)
        {
            var repository = Factory.CreateCompanyRepository();
            return repository.QueryCreditworthiness(company);
        }

        private static WorkingSetting CreateProvideWorkingSetting()
        {
            return new WorkingSetting
            {
                DefaultOfficeNumber = "",
                RefundNeedAudit = false
            };
        }
        private static Company CreateCompany(AccountBasicIndividual individual)
        {
            if (!Regexes.Name.Match(individual.AccountName).Success)
                throw new InvalidOperationException("姓名无效。");
            if (string.IsNullOrWhiteSpace(individual.Phone))
                throw new InvalidOperationException("手机号无效。");
            if (!ValidateIdentifyCard(individual.CertNo))
                throw new InvalidOperationException("身份证号无效。");
            var company = new ChinaPay.B3B.Data.DataMapping.Company
            {
                Id = Guid.NewGuid(),
                Name = individual.AccountName,
                AbbreviateName = individual.AccountName,
                Type = individual.CompanyType,
                AccountType = AccountBaseType.Individual,
                Enabled = true,
                Audited = false,
                IsOpenExternalInterface = false,
            };
            return company;
        }
        private static Company CreateCompany(AccountBasicEnterprise enterprise)
        {
            if (!Regexes.CompanyName.Match(enterprise.AccountName).Success || enterprise.AccountName.Length>25)
                throw new InvalidOperationException("企业名称无效。");
            if (!Regexes.CompanyName.Match(enterprise.AbbreviateName).Success|| enterprise.AbbreviateName.Length>10)
                throw new InvalidOperationException("公司简称无效。");
            if (!Regexes.Phones.Match(enterprise.CompanyPhone).Success || enterprise.CompanyPhone.Length > 100)
                throw new InvalidOperationException("公司电话无效。");
            if (!Regexes.OrginationCode.Match(enterprise.OrginationCode).Success)
                throw new InvalidOperationException("组织机构代码无效。");
            if (!Regexes.Name.Match(enterprise.ContactName).Success)
                throw new InvalidOperationException("联系人姓名无效。");
            if (!Regexes.MobilePhone.Match(enterprise.ContactPhone).Success)
                throw new InvalidOperationException("联系人手机号码无效。");

            var company = new Data.DataMapping.Company
            {
                Id = Guid.NewGuid(),
                Name = enterprise.AccountName,
                AccountType = AccountBaseType.Enterprise,
                AbbreviateName = enterprise.AbbreviateName,
                Type = enterprise.CompanyType,
                OfficePhones = enterprise.CompanyPhone,
                OrginationCode = enterprise.OrginationCode,
                Enabled = true,
                Audited = false,
                IsOpenExternalInterface = false
            };
            return company;
        }
        private static Company CreateCompany(AccountIndividual individual)
        {
            if (!Regexes.Name.Match(individual.AccountName).Success)
                throw new InvalidOperationException("姓名无效。");
            if (string.IsNullOrWhiteSpace(individual.Phone))
                throw new InvalidOperationException("手机号无效。");
            if (!ValidateIdentifyCard(individual.CertNo))
                throw new InvalidOperationException("身份证号无效。");
            if (!individual.IsNotNeedCheck)
            {
                if (string.IsNullOrWhiteSpace(individual.Province))
                    throw new InvalidOperationException("所在省份无效。");
                if (string.IsNullOrWhiteSpace(individual.City))
                    throw new InvalidOperationException("所在城市无效。");
                if (string.IsNullOrWhiteSpace(individual.District))
                    throw new InvalidOperationException("所在区县无效。");
                if (!Regexes.Email.Match(individual.Email).Success)
                    throw new InvalidOperationException("邮箱无效。");
            }
            if (!string.IsNullOrWhiteSpace(individual.Faxes) && !Regexes.Phone.Match(individual.Faxes).Success)
                throw new InvalidOperationException("传真无效。");
            if (!string.IsNullOrWhiteSpace(individual.QQ) && !Regexes.QQ.Match(individual.QQ).Success)
                throw new InvalidOperationException("QQ无效。");
            if (!string.IsNullOrWhiteSpace(individual.ZipCode) && !Regexes.ZipCode.Match(individual.ZipCode).Success)
                throw new InvalidOperationException("邮编无效。");
            var company = new Company();
            company.Id = Guid.NewGuid();
            company.Name = individual.AccountName;
            company.AbbreviateName = individual.AccountName;
            company.Type = individual.CompanyType;
            company.AccountType = AccountBaseType.Individual;
            if (!string.IsNullOrWhiteSpace(individual.Faxes))
                company.Faxes = individual.Faxes;
            if (!string.IsNullOrWhiteSpace(individual.OperatorAccount))
                company.OperatorAccount = individual.OperatorAccount;
            company.Enabled = true;
            company.Audited = false;
            company.IsOpenExternalInterface = false;
            return company;
        }
        private static Company CreateCompany(AccountEnterprise enterprise)
        {
            if (!Regexes.CompanyName.Match(enterprise.AccountName).Success || enterprise.AccountName.Length > 25)
                throw new InvalidOperationException("企业名称无效。");
            if (!Regexes.CompanyName.Match(enterprise.AbbreviateName).Success|| enterprise.AbbreviateName.Length >10)
                throw new InvalidOperationException("公司简称无效。");
            if (!Regexes.Phones.Match(enterprise.CompanyPhone).Success || enterprise.CompanyPhone.Length > 100)
                throw new InvalidOperationException("公司电话无效。");
            if (!Regexes.OrginationCode.Match(enterprise.OrginationCode).Success)
                throw new InvalidOperationException("组织机构代码无效。");
            if (!Regexes.Name.Match(enterprise.ContactName).Success)
                throw new InvalidOperationException("联系人姓名无效。");
            if (!Regexes.MobilePhone.Match(enterprise.ContactPhone).Success)
                throw new InvalidOperationException("联系人手机号码无效。");
            if (!enterprise.IsNotNeedCheck)
            {
                if (string.IsNullOrWhiteSpace(enterprise.Province))
                    throw new InvalidOperationException("所在省份无效。");
                if (string.IsNullOrWhiteSpace(enterprise.City))
                    throw new InvalidOperationException("所在城市无效。");
                if (string.IsNullOrWhiteSpace(enterprise.District))
                    throw new InvalidOperationException("所在区县无效。");
                if (!Regexes.Email.Match(enterprise.Email).Success)
                    throw new InvalidOperationException("邮箱无效。");
                if (!Regexes.Name.Match(enterprise.EmergencyName).Success)
                    throw new InvalidOperationException("紧急联系人无效。");
                if (!Regexes.MobilePhone.Match(enterprise.EmergencyPhone).Success)
                    throw new InvalidOperationException("紧急联系人手机无效。");
                if (!Regexes.Name.Match(enterprise.ManagerName).Success)
                    throw new InvalidOperationException("负责人无效。");
                if (!Regexes.MobilePhone.Match(enterprise.ManagerPhone).Success)
                    throw new InvalidOperationException("负责人手机无效。");
            }
            if (!string.IsNullOrWhiteSpace(enterprise.Faxes) && !Regexes.Phone.Match(enterprise.Faxes).Success)
                throw new InvalidOperationException("传真无效。");
            if (!string.IsNullOrWhiteSpace(enterprise.QQ) && !Regexes.QQ.Match(enterprise.QQ).Success)
                throw new InvalidOperationException("QQ无效。");
            if (!string.IsNullOrWhiteSpace(enterprise.ZipCode) && !Regexes.ZipCode.Match(enterprise.ZipCode).Success)
                throw new InvalidOperationException("邮编无效。");
            var company = new Company();
            company.Id = Guid.NewGuid();
            company.Name = enterprise.AccountName;
            company.AccountType = AccountBaseType.Enterprise;
            company.AbbreviateName = enterprise.AbbreviateName;
            company.Type = enterprise.CompanyType;
            company.OfficePhones = enterprise.CompanyPhone;
            company.OrginationCode = enterprise.OrginationCode;
            if (!string.IsNullOrWhiteSpace(enterprise.Faxes))
                company.Faxes = enterprise.Faxes;
            if (!string.IsNullOrWhiteSpace(enterprise.OperatorAccount))
                company.OperatorAccount = enterprise.OperatorAccount;
            company.Enabled = true;
            company.Audited = false;
            company.IsOpenExternalInterface = false;
            return company;
        }
        private static Company CreateCompany(Company company, PurchaseIndividualInfo info)
        {
            if (string.IsNullOrWhiteSpace(info.Province))
                throw new InvalidOperationException("所在省份无效。");
            if (string.IsNullOrWhiteSpace(info.City))
                throw new InvalidOperationException("所在城市无效。");
            if (string.IsNullOrWhiteSpace(info.District))
                throw new InvalidOperationException("所在区县无效。");
            if (string.IsNullOrWhiteSpace(info.Address))
                throw new InvalidOperationException("所在地无效。");
            if (!string.IsNullOrWhiteSpace(info.OfficePhone) && !Regexes.Phone.Match(info.OfficePhone).Success)
                throw new InvalidOperationException("固定电话无效。");
            if (!string.IsNullOrWhiteSpace(info.Faxes) && !Regexes.Phone.Match(info.Faxes).Success)
                throw new InvalidOperationException("传真无效。");
            if (!Regexes.Email.Match(info.Email).Success)
                throw new InvalidOperationException("邮箱无效。");
            if (!string.IsNullOrWhiteSpace(info.QQ) && !Regexes.QQ.Match(info.QQ).Success)
                throw new InvalidOperationException("QQ无效。");
            if (!Regexes.ZipCode.Match(info.ZipCode).Success)
                throw new InvalidOperationException("邮编无效。");

            return new Company()
            {
                Id = company.Id,
                AccountType = company.AccountType,
                AbbreviateName = company.AbbreviateName,
                Area = info.Area,
                Audited = company.Audited,
                AuditTime = company.AuditTime,
                EffectTime = company.EffectTime,
                Enabled = company.Enabled,
                OfficePhones = info.OfficePhone,
                Faxes = info.Faxes,
                OperatorAccount = company.OperatorAccount,
                Name = company.Name,
                RegisterTime = company.RegisterTime,
                LastLoginTime = company.LastLoginTime,
                Type = company.Type,
                IsOpenExternalInterface = company.IsOpenExternalInterface,
                IsOem = company.IsOem
            };
        }
        private static Company CreateCompany(Company company, PurchaseEnterpriseInfo info)
        {
            if (string.IsNullOrWhiteSpace(info.Province))
                throw new InvalidOperationException("所在省份无效。");
            if (string.IsNullOrWhiteSpace(info.City))
                throw new InvalidOperationException("所在城市无效。");
            if (string.IsNullOrWhiteSpace(info.District))
                throw new InvalidOperationException("所在区县无效。");
            if (string.IsNullOrWhiteSpace(info.Address))
                throw new InvalidOperationException("所在地无效。");
            if (!string.IsNullOrWhiteSpace(info.Faxes) && !Regexes.Phone.Match(info.Faxes).Success)
                throw new InvalidOperationException("传真无效。");
            if (!Regexes.Email.Match(info.Email).Success)
                throw new InvalidOperationException("邮箱无效。");
            if (!string.IsNullOrWhiteSpace(info.QQ) && !Regexes.QQ.Match(info.QQ).Success)
                throw new InvalidOperationException("QQ无效。");
            if (!Regexes.ZipCode.Match(info.ZipCode).Success)
                throw new InvalidOperationException("邮编无效。");
            if (!Regexes.Name.Match(info.EmergencyContact).Success)
                throw new InvalidOperationException("紧急联系人无效。");
            if (!Regexes.MobilePhone.Match(info.EmergencyCall).Success)
                throw new InvalidOperationException("紧急联系人手机无效。");
            if (!Regexes.Name.Match(info.ManagerName).Success)
                throw new InvalidOperationException("负责人无效。");
            if (!Regexes.MobilePhone.Match(info.ManagerCellphone).Success)
                throw new InvalidOperationException("负责人手机无效。");

            return new Company()
            {
                Id = company.Id,
                Type = company.Type,
                RegisterTime = company.RegisterTime,
                AbbreviateName = company.AbbreviateName,
                Name = company.Name,
                Faxes = info.Faxes,
                Enabled = company.Enabled,
                EffectTime = company.EffectTime,
                AuditTime = company.AuditTime,
                Audited = company.Audited,
                LastLoginTime = company.LastLoginTime,
                Area = info.Area,
                OperatorAccount = company.OperatorAccount,
                OrginationCode = company.OrginationCode,
                AccountType = company.AccountType,
                OfficePhones = company.OfficePhones,
                IsOpenExternalInterface = company.IsOpenExternalInterface,
                IsOem = company.IsOem
            };
        }
        private static Company CreateCompany(Company company, CompanyIndividualUpdateInfo info)
        {
            if (!Regexes.Name.Match(info.Name).Success)
                throw new InvalidOperationException("姓名无效。");
            if (!ValidateIdentifyCard(info.CertNo))
                throw new InvalidOperationException("身份证号无效。");
            if (string.IsNullOrWhiteSpace(info.Province))
                throw new InvalidOperationException("所在省份无效。");
            if (!Regexes.Name.Match(info.ContactName).Success)
                throw new InvalidOperationException("联系人姓名无效。");
            if (string.IsNullOrWhiteSpace(info.ContactPhone))
                throw new InvalidOperationException("联系人手机无效。");
            if (string.IsNullOrWhiteSpace(info.City))
                throw new InvalidOperationException("所在城市无效。");
            if (string.IsNullOrWhiteSpace(info.District))
                throw new InvalidOperationException("所在区县无效。");
            if (string.IsNullOrWhiteSpace(info.Address))
                throw new InvalidOperationException("所在地无效。");
            if (!string.IsNullOrWhiteSpace(info.OfficePhone) && !Regexes.Phone.Match(info.OfficePhone).Success)
                throw new InvalidOperationException("固定电话无效。");
            if (!string.IsNullOrWhiteSpace(info.Faxes) && !Regexes.Phone.Match(info.Faxes).Success)
                throw new InvalidOperationException("传真无效。");
            if (!Regexes.Email.Match(info.Email).Success)
                throw new InvalidOperationException("邮箱无效。");
            if (!string.IsNullOrWhiteSpace(info.QQ) && !Regexes.QQ.Match(info.QQ).Success)
                throw new InvalidOperationException("QQ无效。");
            if (!Regexes.ZipCode.Match(info.ZipCode).Success)
                throw new InvalidOperationException("邮编无效。");

            return new Company()
            {
                Id = company.Id,
                AccountType = company.AccountType,
                AbbreviateName = info.Name,
                Area = company.Area,
                Audited = company.Audited,
                AuditTime = company.AuditTime,
                EffectTime = company.EffectTime,
                Enabled = company.Enabled,
                Faxes = info.Faxes,
                OperatorAccount = company.OperatorAccount,
                Name = info.Name,
                RegisterTime = company.RegisterTime,
                LastLoginTime = company.LastLoginTime,
                OfficePhones = info.OfficePhone,
                Type = company.Type,
                IsOpenExternalInterface = company.IsOpenExternalInterface,
                IsOem = company.IsOem
            };
        }
        private static Company CreateCompany(Company company, CompanyEnterpriseUpdateInfo info)
        {
            if (!Regexes.CompanyName.Match(info.CompanyName).Success)
                throw new InvalidOperationException("企业名称无效。");
            if (!Regexes.CompanyName.Match(info.AbbreviateName).Success)
                throw new InvalidOperationException("公司简称无效。");
            if (!Regexes.Phones.Match(info.CompanyPhone).Success || info.CompanyPhone.Length > 100)
                throw new InvalidOperationException("公司电话无效。");
            if (!Regexes.OrginationCode.Match(info.OrginationCode).Success)
                throw new InvalidOperationException("组织机构代码无效。");
            if (!Regexes.Name.Match(info.ContactName).Success)
                throw new InvalidOperationException("联系人姓名无效。");
            if (!Regexes.MobilePhone.Match(info.ContactPhone).Success)
                throw new InvalidOperationException("联系人手机号码无效。");
            if (string.IsNullOrWhiteSpace(info.Province))
                throw new InvalidOperationException("所在省份无效。");
            if (string.IsNullOrWhiteSpace(info.City))
                throw new InvalidOperationException("所在城市无效。");
            if (string.IsNullOrWhiteSpace(info.District))
                throw new InvalidOperationException("所在区县无效。");
            if (!Regexes.Email.Match(info.Email).Success)
                throw new InvalidOperationException("邮箱无效。");
            if (!Regexes.Name.Match(info.EmergencyContact).Success)
                throw new InvalidOperationException("紧急联系人无效。");
            if (!Regexes.MobilePhone.Match(info.EmergencyCall).Success)
                throw new InvalidOperationException("紧急联系人手机无效。");
            if (!Regexes.Name.Match(info.ManagerName).Success)
                throw new InvalidOperationException("负责人无效。");
            if (!Regexes.MobilePhone.Match(info.ManagerCellphone).Success)
                throw new InvalidOperationException("负责人手机无效。");
            if (!string.IsNullOrWhiteSpace(info.Faxes) && !Regexes.Phone.Match(info.Faxes).Success)
                throw new InvalidOperationException("传真无效。");
            if (!string.IsNullOrWhiteSpace(info.QQ) && !Regexes.QQ.Match(info.QQ).Success)
                throw new InvalidOperationException("QQ无效。");
            if (!string.IsNullOrWhiteSpace(info.ZipCode) && !Regexes.ZipCode.Match(info.ZipCode).Success)
                throw new InvalidOperationException("邮编无效。");
            return new Company()
                {
                    Id = company.Id,
                    Name = info.CompanyName,
                    AccountType = AccountBaseType.Enterprise,
                    AbbreviateName = info.AbbreviateName,
                    Type = company.Type,
                    OfficePhones = info.CompanyPhone,
                    OrginationCode = info.OrginationCode,
                    Faxes = info.Faxes,
                    OperatorAccount = company.OperatorAccount,
                    Enabled = company.Enabled,
                    Audited = company.Audited,
                    EffectTime = company.EffectTime,
                    AuditTime = company.AuditTime,
                    LastLoginTime = company.LastLoginTime,
                    IsOpenExternalInterface = company.IsOpenExternalInterface,
                    RegisterTime = company.RegisterTime,
                    IsOem = company.IsOem
                };
        }

        /// <summary>
        /// 根据Id查询联系人
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        private static Contact GetContact(Guid contactId)
        {
            var repository = Factory.CreateCompanyRepository();
            return repository.QueryContact(contactId);
        }
        private static Contact CreateContact(AccountBasicIndividual individual)
        {
            return new Contact { Id = Guid.NewGuid(), Name = individual.AccountName, CertNo = individual.CertNo, Cellphone = individual.Phone, OfficePhone = individual.Phone, IsPrincipal = true, IsEmergency = true };
        }
        private static Contact CreateContact(AccountBasicEnterprise enterprise)
        {
            return new Contact { Id = Guid.NewGuid(), Name = enterprise.ContactName, Cellphone = enterprise.ContactPhone, OfficePhone = enterprise.ContactPhone };
        }
        private static Contact CreateContact(AccountIndividual individual)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = individual.AccountName,
                CertNo = individual.CertNo,
                Cellphone = individual.Phone,
                OfficePhone = individual.Phone,
                Email = individual.Email,
                IsEmergency = true,
                IsPrincipal = true
            };
            if (!string.IsNullOrWhiteSpace(individual.QQ))
                contact.QQ = individual.QQ;
            return contact;
        }
        private static Contact CreateContact(AccountEnterprise enterprise)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = enterprise.ContactName,
                Cellphone = enterprise.ContactPhone,
                OfficePhone = enterprise.ContactPhone,
                Email = enterprise.Email
            };
            if (!string.IsNullOrWhiteSpace(enterprise.QQ))
                contact.QQ = enterprise.QQ;
            return contact;
        }
        private static Contact CreateContact(Contact orginalContact, PurchaseIndividualInfo info)
        {
            return new Contact()
            {
                Id = orginalContact.Id,
                Name = string.IsNullOrWhiteSpace(info.ContactName) ? orginalContact.Name : info.ContactName,
                CertNo = orginalContact.CertNo,
                Cellphone = string.IsNullOrWhiteSpace(info.ContactPhone) ? orginalContact.Cellphone : info.ContactPhone,
                Email = info.Email,
                MSN = info.MSN,
                OfficePhone = orginalContact.OfficePhone,
                QQ = info.QQ,
                IsPrincipal = true,
                IsEmergency = true
            };
        }
        private static Contact CreateContact(Contact orginalContact, PurchaseEnterpriseInfo info)
        {
            return new Contact()
            {
                Id = orginalContact.Id,
                Name = string.IsNullOrWhiteSpace(info.ContactName) ? orginalContact.Name : info.ContactName,
                Cellphone = string.IsNullOrWhiteSpace(info.ContactPhone) ? orginalContact.Cellphone : info.ContactPhone,
                OfficePhone = orginalContact.OfficePhone,
                Email = info.Email,
                MSN = info.MSN,
                QQ = info.QQ
            };
        }
        private static Contact CreateContact(Contact orginalContact, CompanyIndividualUpdateInfo info)
        {
            return new Contact()
            {
                Id = orginalContact.Id,
                Name = string.IsNullOrWhiteSpace(info.ContactName) ? orginalContact.Name : info.ContactName,
                CertNo = info.CertNo,
                Cellphone = string.IsNullOrWhiteSpace(info.ContactPhone) ? orginalContact.Cellphone : info.ContactPhone,
                Email = info.Email,
                OfficePhone = string.IsNullOrWhiteSpace(info.ContactPhone) ? orginalContact.Cellphone : info.ContactPhone,
                QQ = info.QQ,
                MSN = orginalContact.MSN,
                IsPrincipal = true,
                IsEmergency = true
            };
        }
        private static Contact CreateContact(Contact orginalContact, CompanyEnterpriseUpdateInfo info)
        {
            return new Contact()
            {
                Id = orginalContact.Id,
                Name = info.ContactName,
                CertNo = orginalContact.CertNo,
                Cellphone = string.IsNullOrWhiteSpace(info.ContactPhone) ? orginalContact.Cellphone : info.ContactPhone,
                Email = info.Email,
                OfficePhone = string.IsNullOrWhiteSpace(info.ContactPhone) ? orginalContact.Cellphone : info.ContactPhone,
                MSN = orginalContact.MSN,
                QQ = info.QQ,
                IsPrincipal = true,
                IsEmergency = true
            };
        }

        private static Contact CreateManager(AccountBasicEnterprise info)
        {
            return new Contact { Id = Guid.NewGuid(), Name = info.ContactName, Cellphone = info.ContactPhone, OfficePhone = info.ContactPhone, IsPrincipal = true };
        }
        private static Contact CreateManager(AccountEnterprise info)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = info.ManagerName,
                Cellphone = info.ManagerPhone,
                OfficePhone = info.ManagerPhone,
                Email = info.Email,
                IsPrincipal = true
            };
            if (!string.IsNullOrWhiteSpace(info.QQ))
                contact.QQ = info.QQ;
            return contact;
        }
        private static Contact CreateManager(Contact originalManager, PurchaseEnterpriseInfo info)
        {
            return new Contact
            {
                Id = originalManager.Id,
                Name = string.IsNullOrWhiteSpace(info.ManagerName) ? originalManager.Name : info.ManagerName,
                Cellphone = string.IsNullOrWhiteSpace(info.ManagerCellphone) ? originalManager.Cellphone : info.ManagerCellphone,
                OfficePhone = string.IsNullOrWhiteSpace(info.ManagerCellphone) ? originalManager.Cellphone : info.ManagerCellphone,
                Email = info.Email,
                MSN = info.MSN,
                QQ = info.QQ,
                IsPrincipal = true
            };
        }
        private static Contact CreateManager(Contact orginalManager, CompanyEnterpriseUpdateInfo info)
        {
            return new Contact
            {
                Id = orginalManager.Id,
                Name = info.ManagerName,
                Cellphone = string.IsNullOrWhiteSpace(info.ManagerCellphone) ? orginalManager.Cellphone : info.ManagerCellphone,
                OfficePhone = string.IsNullOrWhiteSpace(info.ManagerCellphone) ? orginalManager.Cellphone : info.ManagerCellphone,
                Email = info.Email,
                MSN = orginalManager.MSN,
                QQ = info.QQ,
                IsPrincipal = true
            };
        }

        private static Contact CreateEmergencyContact(AccountBasicEnterprise info)
        {
            return new Contact { Id = Guid.NewGuid(), Name = info.ContactName, Cellphone = info.ContactPhone, OfficePhone = info.ContactPhone, IsEmergency = true };
        }
        private static Contact CreateEmergencyContact(AccountEnterprise info)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = info.EmergencyName,
                Cellphone = info.EmergencyPhone,
                OfficePhone = info.EmergencyPhone,
                Email = info.Email,
                IsEmergency = true
            };
            if (!string.IsNullOrWhiteSpace(info.QQ))
                contact.QQ = info.QQ;
            return contact;
        }
        private static Contact CreateEmergencyContact(Contact orginalEmergencyContact, PurchaseEnterpriseInfo info)
        {
            return new Contact
            {
                Id = orginalEmergencyContact.Id,
                Name = string.IsNullOrWhiteSpace(info.EmergencyContact) ? orginalEmergencyContact.Name : info.EmergencyContact,
                Cellphone = string.IsNullOrWhiteSpace(info.EmergencyCall) ? orginalEmergencyContact.Cellphone : info.EmergencyCall,
                OfficePhone = string.IsNullOrWhiteSpace(info.EmergencyCall) ? orginalEmergencyContact.OfficePhone : info.EmergencyCall,
                Email = info.Email,
                MSN = info.MSN,
                QQ = info.QQ,
                IsEmergency = true
            };
        }
        private static Contact CreateEmergencyContact(Contact orginalEmergencyContact, CompanyEnterpriseUpdateInfo info)
        {
            return new Contact
            {
                Id = orginalEmergencyContact.Id,
                Name = info.EmergencyContact,
                Cellphone = string.IsNullOrWhiteSpace(info.EmergencyCall) ? orginalEmergencyContact.Cellphone : info.EmergencyCall,
                OfficePhone = string.IsNullOrWhiteSpace(info.EmergencyCall) ? orginalEmergencyContact.OfficePhone : info.EmergencyCall,
                Email = info.Email,
                MSN = orginalEmergencyContact.MSN,
                QQ = info.QQ,
                IsEmergency = true
            };
        }

        private static Employee CreateAdministrator(AccountInfo accountInfo, AccountBasicIndividual individual)
        {
            if (!Regexes.UserName.Match(accountInfo.AccountNo).Success)
                throw new InvalidOperationException("用户名无效。");
            if (!Regexes.Password.Match(accountInfo.Password).Success)
                throw new InvalidOperationException("密码无效。");
            if (accountInfo.Password != accountInfo.ComfirmPassword)
                throw new InvalidOperationException("\"密码\" 与 \"确认密码\" 不相同。");

            return new Employee
            {
                Id = Guid.NewGuid(),
                Name = individual.AccountName,
                Login = accountInfo.AccountNo,
                Password = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(accountInfo.Password, "utf-8"),
                Cellphone = individual.Phone,
                Enabled = true,
                IsAdministrator = true,
                CreateTime = DateTime.Now
            };
        }
        private static Employee CreateAdministrator(AccountInfo accountInfo, AccountBasicEnterprise enterprise)
        {
            if (!Regexes.UserName.Match(accountInfo.AccountNo).Success)
                throw new InvalidOperationException("用户名无效。");
            if (!Regexes.Password.Match(accountInfo.Password).Success)
                throw new InvalidOperationException("密码无效。");
            if (accountInfo.Password != accountInfo.ComfirmPassword)
                throw new InvalidOperationException("\"密码\" 与 \"确认密码\" 不相同。");

            return new Employee
            {
                Id = Guid.NewGuid(),
                Name = enterprise.ContactName,
                Login = accountInfo.AccountNo,
                Password = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(accountInfo.Password, "utf-8"),
                Cellphone = enterprise.ContactPhone,
                Enabled = true,
                IsAdministrator = true,
                CreateTime = DateTime.Now
            };
        }
        private static Employee CreateAdministrator(AccountInfo accountInfo, AccountIndividual individual)
        {
            if (!Regexes.UserName.Match(accountInfo.AccountNo).Success)
                throw new InvalidOperationException("用户名无效。");
            if (!Regexes.Password.Match(accountInfo.Password).Success)
                throw new InvalidOperationException("密码无效。");
            if (accountInfo.Password != accountInfo.ComfirmPassword)
                throw new InvalidOperationException("\"密码\" 与 \"确认密码\" 不相同。");
            return new Employee
            {
                Id = Guid.NewGuid(),
                Name = individual.AccountName,
                Login = accountInfo.AccountNo,
                Password = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(accountInfo.Password, "utf-8"),
                Cellphone = individual.Phone,
                Enabled = true,
                IsAdministrator = true,
                CreateTime = DateTime.Now
            };
        }
        private static Employee CreateAdministrator(AccountInfo accountInfo, AccountEnterprise enterprise)
        {
            if (!Regexes.UserName.Match(accountInfo.AccountNo).Success)
                throw new InvalidOperationException("用户名无效。");
            if (!Regexes.Password.Match(accountInfo.Password).Success)
                throw new InvalidOperationException("密码无效。");
            if (accountInfo.Password != accountInfo.ComfirmPassword)
                throw new InvalidOperationException("\"密码\" 与 \"确认密码\" 不相同。");

            return new Employee
            {
                Id = Guid.NewGuid(),
                Name = enterprise.ContactName,
                Login = accountInfo.AccountNo,
                Password = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(accountInfo.Password, "utf-8"),
                Cellphone = enterprise.ContactPhone,
                Enabled = true,
                IsAdministrator = true,
                CreateTime = DateTime.Now
            };
        }

        private static CompanyParameter CreateSupplierParameter()
        {
            return new CompanyParameter
            {
                BusinessRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForSpecial),
                CostFreeRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForSpecial),
                DisperseRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForSpecial),
                SinglenessRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForSpecial),
                BlocRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForSpecial),
                ValidityEnd = DateTime.Today.AddYears(SystemParamService.DefaultUseLimit).Date,
                ValidityStart = DateTime.Today.Date
            };
        }
        private static CompanyParameter CreateProviderParameter()
        {
            CompanyParameter parameter = CreateSupplierParameter();
            parameter.RefundCountLimit = SystemParamService.DefaultLockPolicyLimit;
            parameter.FullRefundTimeLimit = SystemParamService.DefaultFullRefundLimit;
            parameter.RefundTimeLimit = SystemParamService.DefaultVoluntaryRefundLimit;
            parameter.BlocRate = parameter.SinglenessRate = parameter.DisperseRate = parameter.CostFreeRate = parameter.BusinessRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForSpecial);
            parameter.ProfessionRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForBrother);
            parameter.SubordinateRate = Convert.ToDecimal(SystemParamService.DefaultTradeRateForJunior);
            return parameter;
        }

        private static readonly Time StartTime = new Time(0, 0, 0);
        private static readonly Time StopTime = new Time(23, 59, 59);
        private static WorkingHours CreateSupplierWorkingHours()
        {
            return new WorkingHours
            {
                RestdayWorkStart = StartTime,
                RestdayWorkEnd = StopTime,
                WorkdayWorkStart = StartTime,
                WorkdayWorkEnd = StopTime
            };
        }
        private static WorkingHours CreateProvideWorkingHours()
        {
            WorkingHours work = CreateSupplierWorkingHours();
            work.RestdayRefundStart = StartTime;
            work.RestdayRefundEnd = StopTime;
            work.WorkdayRefundStart = StartTime;
            work.WorkdayRefundEnd = StopTime;
            return work;
        }

        /// <summary>
        /// 查询地址
        /// </summary>
        /// <param name="provinceCode">省份代码</param>
        /// <param name="cityCode">城市代码</param>
        /// <param name="countyCode">区域代码</param>
        public static AddressInfo QueryAddress(string provinceCode, string cityCode, string countyCode)
        {
            var addressInfo = new AddressInfo();
            if (!string.IsNullOrWhiteSpace(provinceCode))
                addressInfo.ProvinceName = FoundationService.QueryProvice(provinceCode).Name;
            if (!string.IsNullOrWhiteSpace(cityCode))
                addressInfo.CityName = FoundationService.QueryCity(cityCode).Name;
            if (!string.IsNullOrWhiteSpace(countyCode))
                addressInfo.CountyName = FoundationService.QueryCounty(countyCode).Name;
            return addressInfo;
        }
        private static Address CreateAddress(PurchaseIndividualInfo info)
        {
            return new Address { Id = Guid.NewGuid(), Country = "86", Province = info.Province, City = info.City, District = info.District, Avenue = info.Address, ZipCode = info.ZipCode };
        }
        private static Address CreateAddress(PurchaseEnterpriseInfo info)
        {
            return new Address { Id = Guid.NewGuid(), Country = "86", Province = info.Province, City = info.City, District = info.District, Avenue = info.Address, ZipCode = info.ZipCode };
        }
        private static Address CreateAddress(AccountIndividual info)
        {
            var address = new Address()
                {
                    Id = Guid.NewGuid(),
                    Country = "86",
                    Province = info.Province,
                    City = info.City,
                    District = info.District
                };
            if (!string.IsNullOrWhiteSpace(info.ZipCode))
                address.ZipCode = info.ZipCode;
            if (!string.IsNullOrWhiteSpace(info.Address))
                address.Avenue = info.Address;
            return address;
        }
        private static Address CreateAddress(AccountEnterprise info)
        {
            var address = new Address()
                {
                    Id = Guid.NewGuid(),
                    Country = "86",
                    Province = info.Province,
                    City = info.City,
                    District = info.District
                };
            if (!string.IsNullOrWhiteSpace(info.ZipCode))
                address.ZipCode = info.ZipCode;
            if (!string.IsNullOrWhiteSpace(info.Address))
                address.Avenue = info.Address;
            return address;
        }
        private static Address CreateAddress(CompanyIndividualUpdateInfo info)
        {
            return new Address { Id = Guid.NewGuid(), Country = "86", Province = info.Province, City = info.City, District = info.District, Avenue = info.Address, ZipCode = info.ZipCode };
        }
        private static Address CreateAddress(CompanyEnterpriseUpdateInfo info)
        {
            return new Address { Id = Guid.NewGuid(), Country = "86", Province = info.Province, City = info.City, District = info.District, Avenue = info.Address, ZipCode = info.ZipCode };
        }


        #region 公司附件

        /// <summary>
        /// 保存公司附件
        /// </summary>
        /// <param name="document"></param>
        private static void SaveCompanyDocument(CompanyDocument document)
        {
            var repository = Factory.CreateCompanyDocumentRepository();
            repository.Save(document);
        }

        /// <summary>
        /// 删除公司附件
        /// </summary>
        /// <param name="companyId"></param>
        private static void DeleteCompanyDocument(Guid companyId)
        {
            var repository = Factory.CreateCompanyDocumentRepository();
            repository.Delete(companyId);
        }

        /// <summary>
        /// 查询单个公司的附件
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static CompanyDocument QueryCompanyDocument(Guid companyId)
        {
            var repository = Factory.CreateCompanyDocumentRepository();
            return repository.Query(companyId);
        }

        #endregion

        # region 注册IP地址
        /// <summary>
        /// 插入注册IP信息
        /// </summary>
        /// <param name="registerIp"></param>
        private static void InsertRegisterIp(RegisterIP registerIp)
        {
            var repository = Factory.CreateRegisterIpRepository();
            repository.Insert(registerIp);
        }

        /// <summary>
        /// 修改注册IP信息
        /// </summary>
        /// <param name="registerIp"></param>
        private static void UpdateRegisterIp(RegisterIP registerIp)
        {
            var repository = Factory.CreateRegisterIpRepository();
            repository.Update(registerIp);
        }

        /// <summary>
        /// 根据注册IP查询
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static RegisterIP QueryRegisterIp(string ip)
        {
            var repository = Factory.CreateRegisterIpRepository();
            return repository.Query(ip);
        }

        #endregion

        #region 日志
        static void saveAddLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Insert, "添加" + itemName + "。" + content, role, key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), role, key, account);
        }
        static void saveDeleteLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + "。" + content, role, key, account);
        }

        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.单位, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion

    }
}
