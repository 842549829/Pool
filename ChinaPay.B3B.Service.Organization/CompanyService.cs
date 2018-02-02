using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.Data;
using Izual;
using Izual.Linq;
using Company = ChinaPay.B3B.Data.DataMapping.Company;

namespace ChinaPay.B3B.Service.Organization
{
    /// <summary>
    ///  公司服务
    /// </summary>
    public static class CompanyService
    {

        public static void SetOem(Guid id)
        {
            DataContext.Companies.Update(c => new
                {
                    IsOem = true
                }, c => c.Id == id);
        }

        /// <summary>
        /// 通过对一个公司的审核。
        /// </summary>
        /// <param name="id">被审核的公司的 Id.</param>
        /// <returns>返回审核操作是否成功。</returns>
        public static bool Accept(Guid id)
        {
            return DataContext.Companies.Update(c => new
                {
                    Audited = true,
                    AuditTime = DateTime.Now
                }, c => c.Id == id) > 0;
        }
        /// <summary>
        /// 是否采用全局统一设置采买限制
        /// </summary>
        /// <param name="id">公司Id</param>
        /// <param name="isGlobal">是否全局设置</param>
        /// <param name="operatorAccount">操作员</param>
        public static void SetLimitationType(Guid id, PurchaseLimitationType type, string operatorAccount)
        {
            DataContext.Companies.Update(c => new
                {
                    PurchaseLimitationType = type
                }, c => c.Id == id);
            saveAddLog("是否采用全局统一设置采买限制", string.Format("公司Id:{0},采买限制类型:{1}", id, type.GetDescription()), OperatorRole.User, id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 查询采买限制
        /// </summary>
        /// <param name="companyId">公司Id</param>
        public static PurchaseLimitationType QueryLimitationType(Guid companyId)
        {
            var repository = Factory.CreateCompanyRepository();
            return repository.QueryGlobalPurchase(companyId);
        }
        /// <summary>
        /// 查询收益设置信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        public static IncomeGroupLimitType QueryGlobalPurchaseIncome(Guid companyId)
        {
            var repository = Factory.CreateCompanyRepository();
            return repository.QueryGlobalIncomeGroup(companyId);
        }

        /// <summary>
        /// 拒绝对一个公司的审核。
        /// </summary>
        /// <param name="id">被审核的公司的 Id</param>
        /// <returns></returns>
        public static bool Reject(Guid id, string companyAccount, string reason, string operatorAccount)
        {
            bool isSuccess = false;
            isSuccess = DataContext.Companies.Update(c => new
                {
                    Audited = false,
                    AuditTime = DateTime.Now
                }, c => c.Id == id) > 0;
            saveElseLog("公司正常认证拒绝", string.Format("审核拒绝公司账号为{0}，拒绝原因：{1}", companyAccount, reason), OperatorRole.Platform, companyAccount, operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 禁用一个公司。
        /// </summary>
        /// <param name="id">被禁用的公司的 Id</param>
        /// <returns>返回禁用操作是否成功。</returns>
        public static bool Disable(Guid id, string companyAccount, string reason, string operatorAccount)
        {
            bool isSuccess = false;
            isSuccess = DataContext.Companies.Update(c => new
                {
                    Enabled = false,
                }, c => c.Id == id) > 0;
            saveElseLog("禁用公司账号", string.Format("禁用公司账号为{0}，禁用原因：{1}", companyAccount, reason), OperatorRole.User, companyAccount, operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 启用一个公司。
        /// </summary>
        /// <param name="id">被启用的公司的 Id</param>
        /// <returns>返回启用操作是否成功。</returns>
        public static bool Enable(Guid id, string operatorAccount)
        {
            bool isSuccess = false;
            isSuccess = DataContext.Companies.Update(c => new
                {
                    Enabled = true
                }, c => c.Id == id) > 0;
            saveUpdateLog("账号状态", string.Format("公司Id:{0}的账号状态为禁用", id), string.Format("公司Id:{0}的账号状态改为启用", id), OperatorRole.User, id.ToString(), operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 将指定的公司创建为 OEM
        /// </summary>
        /// <param name="companyInfo">将被创建为 OEM 的公司的 Id</param>
        /// <param name="info">OEM 设置信息。</param>
        /// <returns>返回创建操作是否成功。</returns>
        public static bool CreateOem(CompanyInfo companyInfo, OEMInfo info)
        {
            if (info == null) throw new ArgumentNullException("info");
            Company company = DataContext.Companies.Select(c => new Company
                {
                    Id = c.Id,
                    Type = c.Type
                }).FirstOrDefault(c => c.Id == companyInfo.CompanyId && c.Type == CompanyType.Provider);
            if (company == null)
                throw new InvalidOperationException("指定的公司不存在，或者不是可以成为 OEM 的公司类型。");

            info.CompanyId = companyInfo.CompanyId;
            //return info.InsertOrUpdate();
            return true;
        }

        /// <summary>
        /// 获取指定公司的单位参数
        /// </summary>
        /// <param name="id">公司 Id</param>
        /// <returns>返回该公司的单位参数信息。</returns>
        public static CompanyParameter GetCompanyParameter(Guid id)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryCompanyParameter(id);
        }

        /// <summary>
        /// 设置公司参数
        /// </summary>
        /// <param name="parameter">公司参数信息</param>
        /// <returns>返回设置操作是否成功。</returns>
        public static bool SetCompanyParameter(CompanyParameter parameter, string operatorAccount)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");
            bool isSuccess = false;
            CompanyParameter companyParameter = GetCompanyParameter(parameter.Company);
            //return (Data.GetEntry<CompanyParameter>().Any(p => p.Company == parameter.Company)) ? parameter.Update() : parameter.Insert();
            isSuccess = DataContext.CompanyParameters.InsertOrUpdate(parameter, c => c.Company == parameter.Company) > 0;
            if (companyParameter == null)
            {
                saveAddLog("公司参数设置",
                    string.Format(
                        "公司Id:{0},BSP 自动出票:{1}, B2B 自动出票:{2},自己取消 PNR:{3},可发布 VIP 政策:{4},可开设内部机构:{5},允许同行采购:{6},锁定政策积累退废票数:{7},自愿退票时限:{8},全退时限:{9},同行交易费率:{10},下级交易费率:{11},有效期开始日期:{12},有效期结束日期:{13},平台自动审核特殊政策:{14},单程控位产品:{15},单程控位产品费率:{16},散冲团产品:{17}, 散冲团产品费率:{18},商旅卡特惠产品:{19},商旅卡特惠产品费率:{20},集团票产品:{21},集团票产品费率:{22},商旅卡产品:{23},商旅卡产品费率:{24},其他特殊产品产品:{25},其他特殊产品费率:{26},低打高返产品:{27},低打高返产品费率:{28},信誉评级:{29}",
                        parameter.Company.ToString(), parameter.AutoPrintBSP ? "是" : "否", parameter.AutoPrintB2B ? "是" : "否", parameter.CancelPnrBySelf ? "是" : "否",
                        parameter.CanReleaseVip ? "是" : "否", parameter.CanHaveSubordinate ? "是" : "否", parameter.AllowBrotherPurchase ? "是" : "否", parameter.RefundCountLimit,
                        parameter.RefundTimeLimit, parameter.FullRefundTimeLimit, parameter.ProfessionRate, parameter.SubordinateRate,
                        parameter.ValidityStart.HasValue ? parameter.ValidityStart : DateTime.MinValue, parameter.ValidityEnd.HasValue ? parameter.ValidityEnd : DateTime.MinValue,
                        parameter.AutoPlatformAudit ? "是" : "否", parameter.Singleness ? "是" : "否", parameter.SinglenessRate, parameter.Disperse ? "是" : "否", parameter.DisperseRate,
                        parameter.CostFree ? "是" : "否", parameter.CostFree, parameter.Bloc ? "是" : "否", parameter.BlocRate, parameter.Business ? "是" : "否", parameter.BusinessRate,
                        parameter.OtherSpecial ? "是" : "否", parameter.OtherSpecialRate,
                        parameter.LowToHigh ? "是" : "否", parameter.LowToHighRate,
                        parameter.Creditworthiness.HasValue ? parameter.Creditworthiness : 0M),
                    OperatorRole.Platform,
                    parameter.Company.ToString(),
                    operatorAccount);
            }
            else
            {
                string orginalContent =
                    string.Format(
                        "公司Id:{0},BSP 自动出票:{1}, B2B 自动出票:{2},自己取消 PNR:{3},可发布 VIP 政策:{4},可开设内部机构:{5},允许同行采购:{6},锁定政策积累退废票数:{7},自愿退票时限:{8},全退时限:{9},同行交易费率:{10},下级交易费率:{11},有效期开始日期:{12},有效期结束日期:{13},平台自动审核特殊政策:{14},单程控位产品:{15},单程控位产品费率:{16},散冲团产品:{17}, 散冲团产品费率:{18},商旅卡特惠产品:{19},商旅卡特惠产品费率:{20},集团票产品:{21},集团票产品费率:{22},商旅卡产品:{23},商旅卡产品费率:{24},其他特殊产品产品:{25},其他特殊产品费率:{26},低打高返产品:{27},低打高返产品费率:{28},信誉评级:{29}",
                        companyParameter.Company.ToString(), companyParameter.AutoPrintBSP ? "是" : "否", companyParameter.AutoPrintB2B ? "是" : "否",
                        companyParameter.CancelPnrBySelf ? "是" : "否", companyParameter.CanReleaseVip ? "是" : "否", companyParameter.CanHaveSubordinate ? "是" : "否",
                        companyParameter.AllowBrotherPurchase ? "是" : "否", companyParameter.RefundCountLimit, companyParameter.RefundTimeLimit, companyParameter.FullRefundTimeLimit,
                        companyParameter.ProfessionRate, companyParameter.SubordinateRate,
                        companyParameter.ValidityStart.HasValue ? companyParameter.ValidityStart : DateTime.MinValue,
                        companyParameter.ValidityEnd.HasValue ? companyParameter.ValidityEnd : DateTime.MinValue, companyParameter.AutoPlatformAudit ? "是" : "否",
                        companyParameter.Singleness ? "是" : "否", companyParameter.SinglenessRate, companyParameter.Disperse ? "是" : "否", companyParameter.DisperseRate,
                        companyParameter.CostFree ? "是" : "否", companyParameter.CostFree, companyParameter.Bloc ? "是" : "否", companyParameter.BlocRate,
                        companyParameter.Business ? "是" : "否", companyParameter.BusinessRate,
                        companyParameter.OtherSpecial ? "是" : "否", companyParameter.OtherSpecialRate,
                        companyParameter.LowToHigh ? "是" : "否", companyParameter.LowToHighRate,
                        companyParameter.Creditworthiness.HasValue ? companyParameter.Creditworthiness : 0M);
                string newContent =
                    string.Format(
                        "公司Id:{0},BSP 自动出票:{1}, B2B 自动出票:{2},自己取消 PNR:{3},可发布 VIP 政策:{4},可开设内部机构:{5},允许同行采购:{6},锁定政策积累退废票数:{7},自愿退票时限:{8},全退时限:{9},同行交易费率:{10},下级交易费率:{11},有效期开始日期:{12},有效期结束日期:{13},平台自动审核特殊政策:{14},单程控位产品:{15},单程控位产品费率:{16},散冲团产品:{17}, 散冲团产品费率:{18},商旅卡特惠产品:{19},商旅卡特惠产品费率:{20},集团票产品:{21},集团票产品费率:{22},商旅卡产品:{23},商旅卡产品费率:{24},其他特殊产品产品:{25},其他特殊产品费率:{26},低打高返产品:{27},低打高返产品费率:{28},信誉评级:{29}",
                        parameter.Company.ToString(), parameter.AutoPrintBSP ? "是" : "否", parameter.AutoPrintB2B ? "是" : "否", parameter.CancelPnrBySelf ? "是" : "否",
                        parameter.CanReleaseVip ? "是" : "否", parameter.CanHaveSubordinate ? "是" : "否", parameter.AllowBrotherPurchase ? "是" : "否", parameter.RefundCountLimit,
                        parameter.RefundTimeLimit, parameter.FullRefundTimeLimit, parameter.ProfessionRate, parameter.SubordinateRate,
                        parameter.ValidityStart.HasValue ? parameter.ValidityStart : DateTime.MinValue, parameter.ValidityEnd.HasValue ? parameter.ValidityEnd : DateTime.MinValue,
                        parameter.AutoPlatformAudit ? "是" : "否", parameter.Singleness ? "是" : "否", parameter.SinglenessRate, parameter.Disperse ? "是" : "否", parameter.DisperseRate,
                        parameter.CostFree ? "是" : "否", parameter.CostFree, parameter.Bloc ? "是" : "否", parameter.BlocRate, parameter.Business ? "是" : "否", parameter.BusinessRate,
                        parameter.OtherSpecial ? "是" : "否", parameter.OtherSpecialRate,
                        parameter.LowToHigh ? "是" : "否", parameter.LowToHighRate,
                        parameter.Creditworthiness.HasValue ? parameter.Creditworthiness : 0M);
                saveUpdateLog("公司参数设置", orginalContent, newContent, OperatorRole.Platform, parameter.Company.ToString(), operatorAccount);
            }
            return isSuccess;
        }

        /// <summary>
        /// 获取指定公司的业务负责人
        /// </summary>
        /// <param name="companyId">业务负责人所属公司</param>
        /// <returns>返回该公司的各业务负责人信息</returns>
        public static IEnumerable<BusinessManager> GetBusinessManagers(Guid companyId) { return Factory.CreateCompanyRepository().QueryBusinessManagers(companyId); }

        /// <summary>
        /// 更新指定公司的所有业务负责人信息。
        /// </summary>
        /// <param name="companyId">要更新业务负责人信息的公司的 Id</param>
        /// <param name="mgrs">业务负责人信息列表</param>
        /// <returns>返回更新操作是否成功。</returns>
        public static bool UpdateAllBusinessManager(Guid companyId, IEnumerable<BusinessManager> mgrs, string operatorAccount)
        {
            if (mgrs == null) throw new ArgumentNullException("mgrs");
            using (var trans = new TransactionScope())
            {
                mgrs.ForEach(m =>
                                 {
                                     m.Id = Guid.NewGuid();
                                     m.Company = companyId;
                                 });
                DataContext.BusinessManagers.Delete(m => m.Company == companyId);
                DataContext.BusinessManagers.Batch(mgrs, (set, m) => set.Insert(m));
                trans.Complete();
            }
            saveLog(OperationType.Update, string.Format("修改公司Id为{0}的负责人信息", companyId), OperatorRole.Provider, companyId.ToString(), operatorAccount);
            return true;
        }

        /// <summary>
        /// 设置指定公司的推广人
        /// </summary>
        /// <param name="companyId">要更新业务负责人信息的公司的 Id</param>
        /// <param name="mgrs">业务负责人信息列表</param>
        /// <returns>返回更新操作是否成功。</returns>
        public static bool SetCompanyOperatorAccount(Guid companyId, string employeeNo, string operatorAccount)
        {
            bool falg = Factory.CreateCompanyRepository().SetCompanyOperatorAccount(companyId, employeeNo);
            if (falg)
            {
                saveLog(OperationType.Update, string.Format("设置公司id{0}的推广者是：{1}", companyId, employeeNo), OperatorRole.Platform, companyId.ToString(), operatorAccount);
            }
            return falg;
        }

        /// <summary>
        /// 获取指定公司的政策设置信息
        /// </summary>
        /// <param name="id">公司 Id</param>
        /// <returns>该公司的政策设置信息</returns>
        public static SettingPolicy GetPolicySetting(Guid id) { return Factory.CreateCompanyRepository().QuerytPolicySetting(id); }

        /// <summary>
        /// 查询工作设置时间信息
        /// </summary>
        /// <returns>工作设置时间信息</returns>
        public static IEnumerable<WorkingHours> GetWorkingHours()
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryWorkingHours();
        }

        /// <summary>
        /// 查询工作信息设置
        /// </summary>
        /// <param name="airline">可出票儿童航空公司</param>
        /// <returns>工作信息设置</returns>
        public static IEnumerable<WorkingSetting> GetChildTicketProviders(string airline)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryChildTicketProviders(airline);
        }

        #region "公司组"

        /// <summary>
        /// 根据提供的信息，更新公司组
        /// </summary>
        /// <param name="group">用于更新的公司组信息。</param>
        /// <param name="members">用于更新的成员列表</param>
        /// <param name="limitations">用于更新的购买限制列表</param>
        /// <returns>返回更新操作是否成功。</returns>
        public static bool UpdateCompanyGroup(CompanyGroup group, IEnumerable<CompanyGroupLimitation> limitations, string op)
        {
            if (group == null) throw new ArgumentNullException("group");

            group.LastModifyTime = DateTime.Now;
            using (var tran = new TransactionScope())
            {
                group.Update();
                DataContext.CompanyGroupLimitations.Delete(l => l.Group == group.Id);
                limitations.ForEach(l =>
                                        {
                                            l.Id = (l.Id == Guid.Empty ? Guid.NewGuid() : l.Id);
                                            l.Insert();
                                        });
                tran.Complete();
            }
            string logContent = "修改公司组。" + PrepareCompanyGroupContent(group, limitations);
            LogService.SaveOperationLog(new OperationLog(OperationModule.公司组, OperationType.Update, op, OperatorRole.Provider, group.Id.ToString(), logContent));
            return true;
        }

        private static string PrepareCompanyGroupContent(CompanyGroup group, IEnumerable<CompanyGroupLimitation> limitations)
        {
            string result = string.Format("组名:{0} 不允许采购他人政策:",
                group.Name);
            if (limitations.Any())
            {
                result += " 限制信息:" + limitations.Join(";",
                    l => string.Format("航空公司:{0} 出发城市:{1} ",
                        l.Airlines, l.Departures));
            }
            return result;
        }

        /// <summary>
        /// 公司组查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页条件</param>
        /// <returns>返回结果</returns>
        public static IEnumerable<CompanyGroupInfo> QueryCompanyGroupInfo(CompanyGroupQueryParameter condition, Pagination pagination)
        {
            ICompanyGroupsRepository repository = Factory.CreateCompanyGroupsRepository();
            return repository.QueryCompanyGroups(condition, pagination);
        }

        /// <summary>
        /// 创建公司组
        /// </summary>
        /// <param name="ownerId">公司组所属的单位的 Id。</param>
        /// <param name="group">公司组信息</param>
        /// <param name="members">成员列表</param>
        /// <param name="limitations">购买限制列表</param>
        /// <param name="creator">创建人</param>
        /// <returns>返回创建操作是否成功。</returns>
        public static bool CreateCompanyGroup(Guid ownerId, CompanyGroup group, IEnumerable<CompanyGroupLimitation> limitations, string creator)
        {
            if (group == null) throw new ArgumentNullException("group");

            Company owner = DataContext.Companies.Where(c => c.Id == ownerId && c.Type == CompanyType.Provider).Select(c => new Company
                {
                    Id = c.Id,
                    Type = c.Type
                }).FirstOrDefault();
            if (owner == null)
                throw new InvalidOperationException("指定的公司不存在，或不是拥有公司组类型的公司。");

            //if(DataContext.CompanyGroups.Any(grp=>grp.Name == group.Name))
            //    throw 


            group.Id = Guid.NewGuid();
            group.CreateTime = DateTime.Now;
            group.Creator = creator;
            group.LastModifyTime = DateTime.Now;
            using (var tran = new TransactionScope())
            {
                group.Insert();
                limitations.ForEach(l =>
                                        {
                                            l.Group = group.Id;
                                            l.Id = (l.Id == Guid.Empty ? Guid.NewGuid() : l.Id);
                                            l.Insert();
                                        });
                tran.Complete();
            }
            string logContent = "创建公司组。" + PrepareCompanyGroupContent(group, limitations);
            LogService.SaveOperationLog(new OperationLog(OperationModule.公司组, OperationType.Insert, creator, OperatorRole.Provider, group.Id.ToString(), logContent));
            return true;
        }

        /// <summary>
        /// 获取公司组详细信息
        /// </summary>
        /// <param name="id">公司组 Id</param>
        /// <returns>返回公司组详细信息</returns>
        public static CompanyGroupDetailInfo GetCompanyGroupDetailInfo(Guid id)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.GetGroupInfo(id);
        }

        /// <summary>
        /// 查询公司组成员
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static IEnumerable<CompanyGroupMemberListInfo> QueryCompanyGroupListInfo(CompanyGroupMemberParameter condition, Pagination pagination)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryCompanyGroupMember(condition, pagination);
        }

        /// <summary>
        /// 查询可以添加成公司组成员
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static IEnumerable<CompanyGroupMemberListInfo> QueryGroupMemberCanAdd(CompanyGroupMemberParameter condition, Pagination pagination)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryMemberCanAdd(condition, pagination);
        }

        /// <summary>
        /// 查询公司组限制信息
        /// </summary>
        /// <param name="companyID">公司Id</param>
        /// <returns>公司组限制信息</returns>
        public static IEnumerable<CompanyGroupLimitationInfo> GetGroupLimits(Guid companyID)
        {
            ICompanyRepository groupLimitationRepository = Factory.CreateCompanyRepository();
            return groupLimitationRepository.GetGroupLimitations(companyID);
        }

        /// <summary>
        /// 根据公司组限制编号，得到公司组限制信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CompanyGroupLimitationInfo GetCompanyGroupLimitation(Guid id)
        {
            ICompanyRepository groupLimitationRepository = Factory.CreateCompanyRepository();
            return groupLimitationRepository.GetGroupLimitation(id);
        }

        /// <summary>
        /// 获取指定单位的设置信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CompanySettingsInfo GetCompanySettingsInfo(Guid id)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryCompanySettingsInfo(id);
        }

        /// <summary>
        /// 删除指定的公司组
        /// </summary>
        /// <param name="groupId">公司组 Id</param>
        /// <returns>返回删除操作是否成功</returns>
        public static bool DeleteCompayGroup(Guid groupId, string op)
        {
            CompanyGroupDetailInfo group = GetCompanyGroupDetailInfo(groupId);
            if (group == null) throw new InvalidOperationException("指定的公司组不存在。");
            using (var trans = new TransactionScope())
            {
                DataContext.CompanyGroupLimitations.Delete(l => l.Group == groupId);
                DataContext.CompanyGroupRelations.Delete(r => r.Group == groupId);
                DataContext.CompanyGroups.Delete(g => g.Id == groupId);
                trans.Complete();
            }
            string logContent = "删除公司组。组名:" + group.Name;
            LogService.SaveOperationLog(new OperationLog(OperationModule.公司组, OperationType.Delete, op, OperatorRole.Provider, groupId.ToString(), logContent));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool AddGroupMember(Guid group, Guid member, string op)
        {
            CompanyGroupDetailInfo grp = GetCompanyGroupDetailInfo(group);
            if (grp == null) throw new InvalidOperationException("指定的公司组不存在。");
            Company cp = GetCompanyInfo(member);
            if (cp == null) throw new InvalidOperationException("指定的公司不存在。");

            var relation = new CompanyGroupRelation
                {
                    Group = group,
                    Company = member
                };
            bool result = relation.Insert();
            string logContent = "添加成员。成员:" + cp.AbbreviateName;
            LogService.SaveOperationLog(new OperationLog(OperationModule.公司组, OperationType.Update, op, OperatorRole.Provider, group.ToString(), logContent));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool DeleteGroupMember(Guid group, Guid member, string op)
        {
            CompanyGroupDetailInfo grp = GetCompanyGroupDetailInfo(group);
            if (grp == null) throw new InvalidOperationException("指定的公司组不存在。");
            Company cp = GetCompanyInfo(member);
            if (cp == null) throw new InvalidOperationException("指定的公司不存在。");

            bool result = DataContext.CompanyGroupRelations.Delete(r => r.Group == group && r.Company == member) > 0;

            string logContent = "删除成员。成员:" + cp.AbbreviateName;
            LogService.SaveOperationLog(new OperationLog(OperationModule.公司组, OperationType.Update, op, OperatorRole.Provider, group.ToString(), logContent));
            return result;
        }

        #endregion

        #region "工作信息与工作时间"

        /// <summary>
        /// 设置公司的工作信息。
        /// </summary>
        /// <param name="setting">要设置的工作信息。</param>
        /// <returns>返回设置操作是否成功。</returns>
        public static bool SetWorkingSetting(WorkingSetting setting, string operatorAccount)
        {
            bool isSuccess = false;
            if (setting == null) throw new ArgumentNullException("setting");
            isSuccess = (DataContext.GetEntry<WorkingSetting>().Any(h => h.Company == setting.Company)) ? setting.Update() : setting.Insert();
            saveLog(OperationType.Update, string.Format("修改公司Id:{0}的工作信息", setting.Company), OperatorRole.User, setting.Company.ToString(), operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 获取指定公司的工作信息。
        /// </summary>
        /// <param name="id">要获取工作信息的公司 Id</param>
        /// <returns>返回该公司的工作信息</returns>
        public static WorkingSetting GetWorkingSetting(Guid id)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryWorkingSetting(id);
        }

        /// <summary>
        /// 设置工作时间信息。
        /// </summary>
        /// <param name="hours">要设置的工作时间信息。</param>
        /// <returns>返回设置操作是否成功。</returns>
        public static bool SetWorkinghours(WorkingHours hours, string operatorAccount)
        {
            bool isSuccess = false;
            if (hours == null) throw new ArgumentNullException("hours");
            if (hours.Company.Equals(Guid.Empty))
                throw new InvalidOperationException("缺少所属公司。");

            isSuccess = hours.InsertOrUpdate();
            saveLog(OperationType.Update, string.Format("修改公司Id:{0}的工作时间信息", hours.Company), OperatorRole.User, hours.Company.ToString(), operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 获取指定公司的工作时间信息
        /// </summary>
        /// <param name="id">要获取工作时间信息的公司的 Id</param>
        /// <returns>返回该公司的工作时间信息</returns>
        public static WorkingHours GetWorkinghours(Guid id)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryWorkingHours(id);
            //return DataContext.WorkingHours.SingleOrDefault(hours => hours.Company == id); 
        }

        #endregion

        #region 公司维护自定义出票条件
        /// <summary>
        /// 添加出票条件
        /// </summary>
        /// <param name="dition"></param>
        /// <param name="operatorAccount"></param>
        /// <param name="role"></param>
        public static void InsertCompanyDrawdition(CompanyDrawdition dition, string operatorAccount)
        {
            var repository = Factory.CreateCompanyDrawditionRepository();
            if (repository.QueryByOwerId(dition.OwnerId).Count(item => item.Type == dition.Type) >= 10) throw new Exception("自定义" + (dition.Type == 0 ? "出票条件" : "政策备注") + "不能超过 10 条。添加失败！");
            dition.Id = Guid.NewGuid();
            repository.Insert(dition);
            saveLog(OperationType.Insert, dition.ToString(), OperatorRole.User, dition.Id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 修改出票条件
        /// </summary>
        /// <param name="dition"></param>
        /// <param name="operatorAccount"></param>
        /// <param name="role"></param>
        public static void UpdateCompanyDrawdition(CompanyDrawdition dition, string operatorAccount)
        {
            var repository = Factory.CreateCompanyDrawditionRepository();
            var old = repository.QueryById(dition.Id);
            repository.Update(dition);
            saveLog(OperationType.Update, old.ToString() + " 修改为：" + dition.ToString(), OperatorRole.User, dition.Id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 根据编号删除出票条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void DelDrawditionById(Guid id, string operatorAccount)
        {
            var repository = Factory.CreateCompanyDrawditionRepository();
            var old = repository.QueryById(id);
            repository.Delete(id);
            saveLog(OperationType.Delete, old.ToString(), OperatorRole.User, id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 根据编号查找出票条件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CompanyDrawdition QueryDrawditionById(Guid id)
        {
            var repository = Factory.CreateCompanyDrawditionRepository();
            return repository.QueryById(id);
        }
        /// <summary>
        /// 根据公司编号查询出票条件
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static List<CompanyDrawdition> QueryDrawditionByCompanyId(Guid companyId )
        {
            var repository = Factory.CreateCompanyDrawditionRepository();
            return repository.QueryByOwerId(companyId);
        }


        #endregion

        #region 查询

        //#region CompanyDetailInfo Query

        //private static readonly IQueryable<CompanyDetailInfo> CompanyDetailInfoQuery =
        //    from cmp in DataContext.Companies
        //    join emp in DataContext.Employees on new
        //        {
        //            cmp.Id,
        //            admin = true
        //        } equals new
        //            {
        //                Id = emp.Owner,
        //                admin = emp.IsAdministrator
        //            } into ems
        //    from emp in ems.DefaultIfEmpty()
        //    join mgr in DataContext.Contacts on cmp.Manager equals mgr.Id into mgrs
        //    from mgr in mgrs.DefaultIfEmpty()
        //    join emg in DataContext.Contacts on cmp.EmergencyContact equals emg.Id into emgs
        //    from emg in emgs.DefaultIfEmpty()
        //    join cnt in DataContext.Contacts on cmp.Contact equals cnt.Id into cnts
        //    from cnt in cnts.DefaultIfEmpty()
        //    join addr in DataContext.Addresses on cmp.Address equals addr.Id into addrs
        //    from addr in addrs.DefaultIfEmpty()
        //    join pm in DataContext.CompanyParameters on cmp.Id equals pm.Company into pms
        //    from pm in pms.DefaultIfEmpty()
        //    join set in DataContext.WorkingSettings on cmp.Id equals set.Company into sets
        //    from set in sets.DefaultIfEmpty()
        //    join gi in
        //        (from grn in DataContext.CompanyGroupRelations
        //         join gp in DataContext.CompanyGroups on grn.Group equals gp.Id
        //         select new
        //             {
        //                 GroupId = gp.Id,
        //                 GroupName = gp.Name,
        //                 grn.Company
        //             }) on cmp.Id equals gi.Company into gis
        //    from gi in gis.DefaultIfEmpty()
        //    select new CompanyDetailInfo
        //        {
        //            AbbreviateName = cmp.AbbreviateName,
        //            Address = addr.Avenue,
        //            City = addr.City,
        //            CompanyId = cmp.Id,
        //            IsOpenExternalInterface = cmp.IsOpenExternalInterface,
        //            CompanyName = cmp.Name,
        //            CompanyType = cmp.Type,
        //            AccountType = cmp.AccountType,
        //            OrginationCode = cmp.OrginationCode,
        //            Contact = cnt.Name,
        //            ContactPhone = string.IsNullOrWhiteSpace(cnt.OfficePhone) ? cnt.OfficePhone : cnt.Cellphone,
        //            ContactEmail = cnt.Email,
        //            ContactMSN = cnt.MSN,
        //            ContactQQ = cnt.QQ,
        //            CertNo = cnt.   CertNo,
        //            District = addr.District,
        //            EmergencyCall = string.IsNullOrWhiteSpace(emg.OfficePhone) ? emg.OfficePhone : emg.Cellphone,
        //            EmergencyContact = emg.Name,
        //            Faxes = cmp.Faxes,
        //            ManagerCellphone = mgr.Cellphone,
        //            ManagerEmail = mgr.Email,
        //            ManagerMsn = mgr.MSN,
        //            ManagerName = mgr.Name,
        //            ManagerQQ = mgr.QQ,
        //            OfficePhones = cmp.OfficePhones,
        //            LastLoginTime = cmp.LastLoginTime,
        //            LastLoginIP = emp.LastLoginIP,
        //            LastLoginLocation = emp.LastLoginLocation,
        //            PeriodEndOfUse = pm.ValidityEnd,
        //            PeriodStartOfUse = pm.ValidityStart,
        //            Province = addr.Province,
        //            UserName = emp.Login,
        //            ZipCode = addr.ZipCode,
        //            RegisterTime = cmp.RegisterTime,
        //            AuditTime = cmp.AuditTime,
        //            Audited = cmp.Audited,
        //            Enabled = cmp.Enabled,
        //            Area = cmp.Area,
        //            UserPassword = emp.Password,
        //            CustomNO_On = set.IsImpower,
        //            Group = gi == null ? string.Empty : gi.GroupName
        //        };

        //#endregion

        /// <summary>
        /// 检查公司名称是否存在。
        /// </summary>
        /// <param name="name">要检查的公司名称。</param>
        /// <returns>存在返回 true，否则返回 false</returns>
        public static bool ExistsCompanyName(string name) { return Factory.CreateCompanyRepository().ExistsCompanyName(name); }

        /// <summary>
        /// 检查公司简称是否存在。
        /// </summary>
        /// <param name="name">要检查的公司简称。</param>
        /// <returns>存在返回 true，否则返回 false</returns>
        public static bool ExistsAbbreviateName(string name) { return Factory.CreateCompanyRepository().ExistsAbbreviateName(name); }

        /// <summary>
        /// 检查公司简称是否存在。
        /// </summary>
        /// <param name="name">要检查的公司简称。</param>
        /// <returns>存在返回 true，否则返回 false</returns>
        public static bool ExistsAbbreviateNameOrCompanyName(string abbreviateName, string companyName) { return Factory.CreateCompanyRepository().ExistsAbbreviateNameOrCompanyName(abbreviateName, companyName); }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderMode"></param>
        /// <returns></returns>
        public static IEnumerable<SpreadingView> GetSpreadingList(SpreadingQueryParameter parameter, Pagination pagination)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QuerySpreadCompanies(parameter, pagination);
        }

        /// <summary>
        /// 获取指定公司的推广者
        /// </summary>
        /// <param name="companyId">公司 Id</param>
        /// <returns>返回指定公司的推广者信息</returns>
        public static RelationInfo GetSpreader(Guid companyId)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QuerySpreader(companyId);
        }

        public static Company GetCompanyInfo(Guid id)
        {
            ICompanyRepository companyRepository = Factory.CreateCompanyRepository();
            return companyRepository.GetCompanyInfo(id);
        }

        // 2012-12-17 deng,zhao 新增属性后做相应调整；
        public static SuperiorInfo QuerySuperiorInfo(Guid id)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            SuperiorInfo superior = repository.QuerySuperiorInfo(id);
            return superior ?? new SuperiorInfo { Id = Guid.Empty, Type = RelationshipType.ServiceProvide };
        }

        /// <summary>
        /// 根据 Id 获取公司信息
        /// </summary>
        /// <param name="id">公司 Id</param>
        /// <returns>返回公司信息</returns>
        public static CompanyDetailInfo GetCompanyDetail(Guid id) { return Factory.CreateCompanyRepository().GetCompanyDetailInfo(id); }

        /// <summary>
        /// 根据账号获取公司信息
        /// </summary>
        /// <param name="accountNo">公司账号</param>
        /// <returns></returns>
        public static CompanyDetailInfo GetCompanyDetail(string userNo)
        {
            return Factory.CreateCompanyRepository().GetCompanyDetail(userNo);
        }

        /// <summary>
        /// 获取上级出票方。
        /// </summary>
        /// <param name="companyId">公司 Id</param>
        /// <returns>返回该公司的上级出票方的公司信息。</returns>
        public static RelationInfo QuerySuperior(Guid companyId)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QuerySupperior(companyId);
        }

        /// <summary>
        /// 获取所有满足指定条件的公司信息。
        /// </summary>
        /// <param name="where">查询条件。</param>
        /// <param name="companyType"> </param>
        /// <param name="searchDisabledCompany"> </param>
        /// <returns>返回满足条件的公司信息列表。</returns>
        public static IEnumerable<CompanyInitInfo> GetCompanies(CompanyType companyType, bool searchDisabledCompany = false)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryCompanyInit(companyType, searchDisabledCompany);
        }
        /// <summary>
        /// 获取所有满足指定条件的公司信息
        /// </summary>
        /// <param name="companyName">公司登录账号名</param>
        /// <param name="companyType">公司类型</param>
        /// <param name="pageSize">显示条数</param>
        /// <returns>返回满足条件的公司信息列表</returns>
        public static IEnumerable<CompanyListInfo> QueryCompany()
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            //if (pageSize <= 0) throw new CustomException("显示条数必须大于零！");
            return repository.QueryCompanys();
        }
        /// <summary>
        /// 获取所有满足指定条件的公司信息。
        /// </summary>
        /// <param name="parameter">查询条件参数</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="orderMode">排序方式 </param>
        /// <returns>返回满足条件的公司信息列表。</returns>
        public static IEnumerable<CompanyListInfo> GetCompanies(CompanyQueryParameter parameter, Pagination pagination)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QueryCompanies(parameter, pagination);
        }


        /// <summary>
        /// 查询指定单位所有满足条件的下级单位
        /// </summary>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="parameter">查询参数</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="orderMode">排序方式</param>
        /// <returns>返回由 parameter.Superior 指定的单位的所有满足条件的下级单位信息</returns>
        public static IEnumerable<SubordinateCompanyListInfo> GetAllSubordinates(SubordinateQueryParameter parameter, Pagination pagination)
        {
            ICompanyRepository repository = Factory.CreateCompanyRepository();
            return repository.QuerySuordinateCompanies(parameter, pagination);
        }


        #endregion

        #region "Office号"

        /// <summary>
        /// 获取指定公司的 Office 号
        /// </summary>
        /// <param name="companyId">公司 Id</param>
        /// <returns>返回该公司所有的 Office 号</returns>
        public static IEnumerable<OfficeNumber> QueryOfficeNumbers(Guid companyId)
        {
            IOfficeNumberRepository repository = Factory.CreateOfficeNumberRepository();
            return repository.Query(companyId);
        }

        /// <summary>
        /// 获取默认Office号
        /// </summary>
        /// <param name="company">公司 Id</param>
        public static string QueryDefaultOfficeNumber(Guid company)
        {
            WorkingSetting ws = GetWorkingSetting(company);
            return ws == null ? string.Empty : ws.DefaultOfficeNumber;
        }

        /// <summary>
        /// 修改指定公司的Office号的是否授权
        /// </summary>
        /// <param name="id">Office 号所属的公司 Id</param>
        /// <param name="officeNumber"></param>
        public static void UpdateOfficeNumber(Guid companyId, string officeNumber, bool impower, string operatorAccount)
        {
            OfficeNumber officeNumbers = QueryOfficeNumbers(companyId).FirstOrDefault(o => o.Number == officeNumber);
            DataContext.OfficeNumbers.Update(e => new { Impower = impower }, e => e.Company == companyId && e.Number == officeNumber);
            saveUpdateLog("OFFICE号设置", string.Format("公司Id:{0},OFFICE号:{1},是否需要授权:{2}", companyId.ToString(), officeNumber, officeNumbers.Impower ? "是" : "否"),
                string.Format("公司Id:{0},OFFICE号:{1},是否需要授权:{2}", companyId.ToString(), officeNumber, impower ? "是" : "否"), OperatorRole.User,
                companyId.ToString() + ":" + officeNumber, operatorAccount);
        }

        /// <summary>
        /// 删除单个Office号
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="officeNo"></param>
        public static void DeleteOfficeNumber(Guid companyId, string officeNo, string operatorAccount)
        {
            DataContext.OfficeNumbers.Delete(e => e.Company == companyId && e.Number == officeNo);
            saveDeleteLog("OFFICE号设置", string.Format("公司Id:{0},OFFICE号:{1}", companyId.ToString(), officeNo), OperatorRole.User, "", operatorAccount);
        }

        /// <summary>
        /// 添加OFFICE号
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="officeNo"></param>
        public static void AddOfficeNumber(Guid companyId, string officeNo, bool impower, string operatorAccount)
        {
            IEnumerable<OfficeNumber> officeNos = QueryOfficeNumbers(companyId);
            if (officeNos.Any(o => o.Number == officeNo))
            {
                throw new InvalidOperationException("已经存在此OFFICE号");
            }
            if (officeNos.Count() < 4)
            {
                var office = new OfficeNumber();
                office.Id = Guid.NewGuid();
                office.Number = officeNo;
                office.Company = companyId;
                office.Enabled = true;
                office.Impower = impower;
                DataContext.OfficeNumbers.Insert(office);
            }
            else
            {
                throw new InvalidOperationException("OFFICE号最多可以设置4个。");
            }
            saveAddLog("OFFICE号设置", string.Format("公司Id:{0},OFFICE号:{1},是否需要授权:{2}", companyId, officeNo, impower ? "是" : "否"), OperatorRole.User, companyId.ToString(),
                operatorAccount);
        }

        #endregion

        #region "自定义编号"

        /// <summary>
        /// 设置员工自定义编号
        /// </summary>
        /// <param name="customNumberId">自定义编号Id</param>
        /// <param name="employeeId">员工Id</param>
        public static bool SetEmpowermentCustoms(Guid customNumberId, IEnumerable<Guid> employeeId, string operatorAccount)
        {
            using (var trans = new TransactionScope())
            {
                DataContext.EmpowermentCustoms.Delete(o => o.CustomNumber == customNumberId);
                IEnumerable<EmpowermentCustom> employeeList = employeeId.Select(s => new EmpowermentCustom { Employee = s, CustomNumber = customNumberId });
                DataContext.EmpowermentCustoms.Batch(employeeList, (set, num) => set.Insert(num));
                trans.Complete();
            }
            saveLog(OperationType.Insert, string.Format("将自定义编号Id{0}分配给为员工号为{1}", customNumberId, employeeId.Join(",", item => item.ToString())), OperatorRole.Provider,
                customNumberId.ToString(), operatorAccount);
            return true;
        }

        /// <summary>
        /// 查询自定义编号授权信息
        /// </summary>
        /// <param name="customNumberId">自定义编号Id</param>
        public static IEnumerable<EmpowermentCustom> GetEmpowermentCustoms(Guid customNumberId)
        {
            //TODO 001
            return Factory.CreateCustomNumberRepository().QueryEmpowermentCustoms(customNumberId);
            //return DataContext.EmpowermentCustoms.Where(o => o.CustomNumber == customNumberId);
        }

        /// <summary>
        /// 查询自定义编号
        /// </summary>
        /// <param name="companyId">公司Id</param>
        public static IEnumerable<CustomNumber> QueryCustomNumber(Guid companyId)
        {
            ICustomNumberRepository repository = Factory.CreateCustomNumberRepository();
            return repository.Query(companyId);
        }

        /// <summary>
        /// 删除自定义编号
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="customNumberId">自定义编号Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DeleteCustomNumber(Guid companyId, Guid customNumberId, string operatorAccount)
        {
            using (var trans = new TransactionScope())
            {
                DataContext.EmpowermentCustoms.Delete(e => e.CustomNumber == customNumberId);
                DataContext.CustomNumbers.Delete(e => e.Company == companyId && e.Id == customNumberId);
                trans.Complete();
            }
            saveDeleteLog("自定义编号", string.Format("公司Id为{0},自定义编号Id为{1}", companyId, customNumberId), OperatorRole.Provider, customNumberId.ToString(), operatorAccount);
        }

        /// <summary>
        /// 添加自定义编号
        /// </summary>
        /// <param name="customNumber">自定义编号</param>
        /// <param name="describe">描述</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void AddCustomNumber(string customNumber, string describe, Guid companyId, string operatorAccount)
        {
            IEnumerable<string> customNos = QueryCustomNumber(companyId).Select(q => q.Number);
            if (customNos != null && customNos.Contains(customNumber))
            {
                throw new InvalidOperationException("已经存在此自定义编号");
            }
            if (customNos.Count() < 20)
            {
                var customNumbers = new CustomNumber
                    {
                        Company = companyId,
                        Describe = describe,
                        Enabled = true,
                        Number = customNumber,
                        Id = Guid.NewGuid()
                    };
                DataContext.CustomNumbers.Insert(customNumbers);
            }
            else
            {
                throw new InvalidOperationException("自定义编号最多可以设置20个。");
            }
            saveAddLog("自定义编号", string.Format("公司Id:{0},自定义编号:{1}", companyId, customNumber), OperatorRole.Provider, customNumber, operatorAccount);
        }

        /// <summary>
        /// 查询授权了的员工自定义编号
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        public static IEnumerable<CustomNumber> GetCustomNumberByEmployee(Guid employeeId)
        {
            ICustomNumberRepository repository = Factory.CreateCustomNumberRepository();
            return repository.QueryCustomNumberByEmployee(employeeId);
        }

        #endregion

        #region "日志"

        private static void saveAddLog(string itemName, string content, OperatorRole role, string key, string account) { saveLog(OperationType.Insert, "添加" + itemName + "。" + content, role, key, account); }

        private static void saveUpdateLog(string itemName, string originalContent, string newContent, OperatorRole role, string key, string account) { saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), role, key, account); }

        private static void saveDeleteLog(string itemName, string content, OperatorRole role, string key, string account) { saveLog(OperationType.Delete, "删除" + itemName + "。" + content, role, key, account); }

        static void saveElseLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Else, itemName + "：" + content, role, key, account);
        }

        private static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new OperationLog(OperationModule.单位, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch
            {
            }
        }

        #endregion
    }
}