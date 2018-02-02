using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// DistributionOEM 的摘要说明
    /// </summary>
    public class DistributionOEM : BaseHandler
    {
        public object GetCompanyInfo(string accountNo)
        {
            var companyDetailInfo = CompanyService.GetCompanyDetail(accountNo);
            if (companyDetailInfo != null)
            {
                return new
                {
                    companyDetailInfo.CompanyId,
                    UserNo = companyDetailInfo.UserName,
                    AccountType = (int)companyDetailInfo.AccountType,
                    CompanyTypeValue = (int)companyDetailInfo.CompanyType,
                    CompanyType = companyDetailInfo.CompanyType.GetDescription() + "(" + companyDetailInfo.AccountType.GetDescription() + ")",
                    Loaction = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressText(companyDetailInfo.Area, companyDetailInfo.Province, companyDetailInfo.City, companyDetailInfo.District),
                    companyDetailInfo.Address,
                    companyDetailInfo.Contact,
                    companyDetailInfo.ContactPhone,
                    RegisterTime = companyDetailInfo.RegisterTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    AuditTime = companyDetailInfo.AuditTime.HasValue ? companyDetailInfo.AuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    companyDetailInfo.CompanyName,
                    companyDetailInfo.AbbreviateName,
                    companyDetailInfo.OfficePhones,
                    companyDetailInfo.ManagerName,
                    companyDetailInfo.ManagerCellphone,
                    companyDetailInfo.OrginationCode,
                    CertNo = companyDetailInfo.CertNo,
                    BeginDeadline = companyDetailInfo.PeriodStartOfUse.HasValue ? companyDetailInfo.PeriodStartOfUse.Value.ToString("yyyy-MM-dd") : string.Empty,
                    EndDeadline = companyDetailInfo.PeriodEndOfUse.HasValue ? companyDetailInfo.PeriodEndOfUse.Value.ToString("yyyy-MM-dd") : string.Empty,
                    IsOem = companyDetailInfo.IsOem,
                    Enabled = companyDetailInfo.Enabled,
                    IsPlatformEmployee = companyDetailInfo.CompanyType==CompanyType.Platform
                };
            }
            else
            {
                return null;
            }
        }

        public object CurrentContract() {
               return BasePage.CurrenContract;
        }

        public void OpenIncomeGroup(string name, string description)
        {
            var incomeGroup = new IncomeGroup();
            incomeGroup.Id = Guid.NewGuid();
            incomeGroup.Company = this.CurrentCompany.CompanyId;
            incomeGroup.CreateTime = DateTime.Now;
            incomeGroup.Creator = this.CurrentUser.UserName;
            incomeGroup.Name = name;
            incomeGroup.Description = description;
            IncomeGroupService.RegisterIncomeGroup(incomeGroup, this.CurrentUser.UserName);
        }

        public IncomeGroupView QueryIncomeGroup(Guid groupId)
        {
            return IncomeGroupService.QueryIncomeGroupView(groupId);
        }

        public IEnumerable<IncomeGroupView> QueryIncomeGroupList(string groupIds)
        {
            var ids = new List<Guid>();
            var strIds = groupIds.Split(',');
            foreach (var item in strIds)
            {
                ids.Add(Guid.Parse(item));
            }
            if (ids.Count > 0)
            {
                return IncomeGroupService.QueryIncomeGroup(ids);
            }
            else
            {
                return new List<IncomeGroupView>();
            }
        }

        public void DeleteIncomeGroupList(string groupIds)
        {
            var ids = new List<Guid>();
            var strIds = groupIds.Split(',');
            foreach (var item in strIds)
            {
                ids.Add(Guid.Parse(item));
            }
            if (ids.Count > 0)
            {
                try
                {
                    IncomeGroupService.DeleteIncomeGroupList(ids, this.CurrentUser.Name);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void DeleteIncomeGroup(Guid groupId)
        {
            IncomeGroupService.DeleteIncomeGroup(groupId, this.CurrentUser.UserName);
        }

        public void UpdateIncomeGroup(Guid groupId, string name, string description)
        {
            var incomeGroup = new IncomeGroup();
            incomeGroup.Id = groupId;
            incomeGroup.Name = name;
            incomeGroup.Description = description;
            IncomeGroupService.UpdateIncomeGroup(incomeGroup, this.CurrentUser.UserName);
        }

        public object QueryDistrutionUserList(Pagination pagination, DistributionOEMUserCondition condition)
        {
            condition.CompanyId = this.CurrentCompany.CompanyId;
            condition.IsOwnerAll = false;
            if (condition.RegisterEndTime.HasValue)
                condition.RegisterEndTime = condition.RegisterEndTime.Value.AddDays(1).AddMilliseconds(-3);
            var distributionOemUser = from item in DistributionOEMService.QueryDistributionOEMUserList(condition, pagination)
                                      select new
                                      {
                                          RegisterTime = item.RegisterTime,
                                          Login = item.Login,
                                          AbbreviateName = item.AbbreviateName,
                                          IncomeGroupName = item.IncomeGroupName,
                                          IncomeGroupId = item.IncomeGroupId,
                                          CompanyId = item.CompanyId
                                      };
            return new
            {
                Pagination = pagination,
                UserList = distributionOemUser
            };
        }

        public void UpdateIncomeGroupRelation(Guid newIncomeGroupId,string companyIds)
        {
            var ids = new List<Guid>();
            var strIds = companyIds.Split(',');
            foreach (var item in strIds)
            {
                ids.Add(Guid.Parse(item));
            }
            if (ids.Count > 0)
            {
                try
                {
                    IncomeGroupService.UpdateIncomeGroupRelation(newIncomeGroupId,ids);
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// 保存OME风格
        /// </summary>
        /// <param name="style"></param>
        /// <param name="oemId"> </param>
        public void SaveOEMStyle(OEMStyle style) {
            style.StylePath = style.StylePath.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            if (style.Id == Guid.Empty)
            {
                style.Id = Guid.NewGuid();
                OEMStyleService.InsertOEMStyle(style, CurrentUser.UserName);
            }
            else
            {
                OEMStyleService.UpdateOEMStyle(style, CurrentUser.UserName);
                FlushRequester.TriggerOEMStyleFlusher(style.Id);
            }
        }

        public void DeleteStyle(Guid styleId) {
            OEMStyleService.DeleteOEMStyle(styleId,CurrentUser.UserName);
        }
    }
}