using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization
{
    public static class IncomeGroupLimitService
    {
        /// <summary>
        /// 新增收益设置
        /// </summary>
        /// <param name="setting"></param>
        public static void InsertIncomeGroupLimit(IncomeGroupLimitGroup setting)
        {
            var repository = Factory.CreateIncomeGroupLimitRespository();
            repository.InsertIncomeGroupLimit(setting);
        }
        /// <summary>
        /// 修改收益设置
        /// </summary>
        /// <param name="setting"></param>
        public static void UpdateIncomeGroupLimit(IncomeGroupLimitGroup setting)
        {
            var repository = Factory.CreateIncomeGroupLimitRespository();
            repository.InsertIncomeGroupLimit(setting);
        }
        /// <summary>
        /// 全局收益设置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="setting"></param>
        public static void InsertIncomeGroupLimitGlobal(IncomeGroupLimitType type, IncomeGroupLimitGroup setting)
        {
            var repository = Factory.CreateIncomeGroupLimitRespository();
            repository.InsertIncomeGroupLimitGlobal(type, setting);
        }
        /// <summary>
        /// 根据用户组编号查询收益信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static IncomeGroupLimitGroup QueryIncomeGroupLimitGroupByGroupId(Guid groupId)
        {
            var repository = Factory.CreateIncomeGroupLimitRespository();
            return repository.QueryIncomeGroupLimitGroupByGroupId(groupId);
        }
        /// <summary>
        /// 根据公司编号查询收益信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static IncomeGroupLimitGroup QueryIncomeGroupLimitGroupByCompanyId(Guid companyId)
        {
            var repository = Factory.CreateIncomeGroupLimitRespository();
            return repository.QueryIncomeGroupLimitGroupByCompanyId(companyId);
        }
        /// <summary>
        /// 查询上级公司的收益设置信息 有可能为空，即没有设置信息
        /// </summary>
        /// <param name="superId">上级编号</param>
        /// <param name="purchseId">当前公司编号</param>
        /// <returns></returns>
        public static IncomeGroupLimitGroup QueryIncomeGroupLimitGroup(Guid superId, Guid purchseId)
        {
            IncomeGroupLimitGroup limit = null;
            PurchaseLimitationType type = CompanyService.QueryLimitationType(superId);
            var repository = Factory.CreateIncomeGroupLimitRespository();
            if (type == PurchaseLimitationType.Global)
            {
                limit = repository.QueryIncomeGroupLimitGroupByCompanyId(superId);
            }
            else if (type == PurchaseLimitationType.Each)
            {
                limit = repository.IncomeGroupLimitGroup(superId, purchseId);
            }
            return limit;
        }
    }
}
