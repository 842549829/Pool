
using ChinaPay.B3B.Service.PolicyMatch.Domain;

namespace ChinaPay.B3B.Service.Policy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Transactions;
    using Common.Enums;
    using Core;
    using Core.Extension;
    using Data;
    using Data.DataMapping;
    using DataTransferObject.Policy;
    using Izual;
    using Izual.Data;
    using Izual.Linq;
    using Organization;
    using DataTransferObject.Log;
    using ChinaPay.B3B.Service;
    using ChinaPay.B3B.Service.Policy.Repository;
    using System.Text;
    using ChinaPay.B3B.DataTransferObject.Common;
    using System.Data;

    public static class PolicyManageService
    {
        /// <summary>
        ///     通过指定政策的审核(作废)
        /// </summary>
        /// <param name="id"> 政策 Id </param>
        /// <param name="type"> 政策类型 </param>
        /// <returns> 返回审核操作是否成功。 </returns>
        public static bool AuditPolicy(Guid id, PolicyType type)
        {
            if (id == null) throw new InvalidOperationException("缺少要审核的政策。");
            bool isAudit = false;
            switch (type)
            {
                case PolicyType.Normal:
                    isAudit = DataContext.NormalPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => p.Id == id) > 0;
                    break;
                case PolicyType.Bargain:
                    isAudit = DataContext.BargainPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => p.Id == id) > 0;
                    break;
                case PolicyType.Special:
                    isAudit = DataContext.SpecialPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => p.Id == id) > 0;
                    break;
                //case PolicyType.RoundTrip:
                //    isAudit= DataContext.RoundTripPolicies.Update(p => new { Audited = true, AuditTime = DateTime.Now }, p => p.Id == id) > 0;
                //    break;
                case PolicyType.Team:
                    isAudit = DataContext.TeamPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => p.Id == id) > 0;
                    break;
                default:
                    throw new InvalidOperationException("未知的政策类型。");
            }
            saveLog(OperationType.Update, string.Format("审核" + type.GetDescription() + "政策" + "{0}。", isAudit ? "成功" : "失败"), OperatorRole.User, id.ToString(), "");
            return isAudit;
        }

        /// <summary>
        ///     通过指定政策的审核
        /// </summary>
        /// <param name="type"> 政策类型 </param>
        /// <param name="operator"> </param>
        /// <param name="ids"> 要通过审核的政策 Id 列表 </param>
        /// <returns> 返回审核操作是否成功。 </returns>
        public static bool Audit(PolicyType type, string @operator, params Guid[] ids)
        {
            if (ids == null || ids.Length == 0) throw new InvalidOperationException("缺少要审核的政策。");
            bool isAudit = false;
            switch (type)
            {
                case PolicyType.Normal:
                    isAudit = DataContext.NormalPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                case PolicyType.Bargain:
                    isAudit = DataContext.BargainPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                case PolicyType.Special:
                    isAudit = DataContext.SpecialPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                //case PolicyType.RoundTrip:
                //    isAudit = DataContext.RoundTripPolicies.Update(p => new { Audited = true, AuditTime = DateTime.Now }, p => ids.Contains(p.Id)) > 0;
                //    break;
                case PolicyType.Team:
                    isAudit = DataContext.TeamPolicies.Update(p => new
                    {
                        Audited = true,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                case PolicyType.Notch:
                    //缺口政策
                    var repository = Factory.CreatePolicyRepository();
                    isAudit = repository.NotchAudit(true, ids);
                    break;
                default:
                    throw new InvalidOperationException("未知的政策类型。");
            }
            foreach (var guid in ids)
            {
                saveLog(OperationType.Update, string.Format("审核" + type.GetDescription() + "政策" + "{0}。", (isAudit ? "成功" : "失败") + guid), OperatorRole.User, guid.ToString(), @operator);
            }
            return isAudit;
        }

        /// <summary>
        ///     取消对指定政策的审核
        /// </summary>
        /// <param name="type"> 政策类型 </param>
        /// <param name="operator"> </param>
        /// <param name="ids"> 要取消审核的政策 Id 列表 </param>
        /// <returns> 返回取消操作是否成功。 </returns>
        public static bool CancelAudit(PolicyType type, string @operator, params Guid[] ids)
        {
            if (ids == null || ids.Length == 0) throw new InvalidOperationException("缺少要取消审核的政策。");
            bool isAudit = false;
            switch (type)
            {
                case PolicyType.Normal:
                    isAudit = DataContext.NormalPolicies.Update(p => new
                    {
                        Audited = false,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                case PolicyType.Bargain:
                    isAudit = DataContext.BargainPolicies.Update(p => new
                    {
                        Audited = false,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                case PolicyType.Special:
                    isAudit = DataContext.SpecialPolicies.Update(p => new
                    {
                        Audited = false,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                //case PolicyType.RoundTrip:
                //    isAudit= DataContext.RoundTripPolicies.Update(p => new { Audited = false, AuditTime = DateTime.Now }, p => ids.Contains(p.Id)) > 0;
                //    break;
                case PolicyType.Team:
                    isAudit = DataContext.TeamPolicies.Update(p => new
                    {
                        Audited = false,
                        AuditTime = DateTime.Now
                    }, p => ids.Contains(p.Id)) > 0;
                    break;
                case PolicyType.Notch:
                    //缺口政策

                    var repository = Factory.CreatePolicyRepository();
                    isAudit = repository.NotchAudit(false, ids);
                    break;
                default:
                    throw new InvalidOperationException("未知的政策类型。");
            }
            foreach (var guid in ids)
            {
                saveLog(OperationType.Update, "取消审核" + type.GetDescription() + "政策。" + (isAudit ? "成功" : "失败") + guid, OperatorRole.User, guid.ToString(), @operator);
            }
            return isAudit;
        }

        /// <summary>
        ///     挂起指定公司的指定航空公司的政策
        /// </summary>
        /// <param name="companyId"> 要挂起政策的公司 </param>
        /// <param name="operator"> 操作者 </param>
        /// <param name="reason"> 挂起政策的原因 </param>
        /// <param name="airlines"> 要挂起的政策所属的航空公司 </param>
        /// <returns> 返回挂起操作是否成功 </returns>
        public static bool SuspendPolicies(Guid companyId, string @operator, string reason, string ip, ChinaPay.B3B.Common.Enums.PublishRole OperatorRoleType, params string[] airlines)
        {
            if (companyId == Guid.Empty) throw new InvalidOperationException("缺少政策所有者。");
            if (String.IsNullOrEmpty(@operator)) throw new InvalidOperationException("缺少挂起的操作者。");
            if (airlines == null || airlines.Length == 0) throw new InvalidOperationException("缺少要挂起的政策所属的航空公司。");
            Company company = (from c in DataContext.Companies
                               join e in DataContext.Employees on c.Id equals e.Owner
                               where e.Login == @operator
                               select c).SingleOrDefault();

            if (company == null)
                throw new InvalidOperationException("指定的操作者不存在，或不属于任何公司。");

            bool suspendedByPlatform = company.Type == CompanyType.Platform;
            List<SuspendedPolicy> sps = airlines.Select(al => new SuspendedPolicy
            {
                Company = companyId,
                Airline = al,
                SuspendedByPlatform = suspendedByPlatform
            }).ToList();

            using (var trans = new TransactionScope())
            {
                DataContext.SuspendedPolicies.Batch(sps, (set, sp) => set.Insert(sp));
                DataContext.SuspendOperations.Insert(new SuspendOperation
                {
                    Airlines = String.Join(",", airlines),
                    Id = Guid.NewGuid(),
                    OperateTime = DateTime.Now,
                    OperateType = PolicySuspendOperationType.Suspend,
                    Owner = companyId,
                    Reason = reason,
                    OperatorRoleType = OperatorRoleType,
                    Operator = @operator,
                    IP = ip,
                    CompanyId = company.Id,
                    CompanyName = company.AbbreviateName
                });
                trans.Complete();
                return true;
            }
        }

        /// <summary>
        ///     解挂指定公司的指定航空公司的政策
        /// </summary>
        /// <param name="companyId"> 要解挂政策的公司 </param>
        /// <param name="operator"> 操作者账号 </param>
        /// <param name="reason"> 解挂政策的原因 </param>
        /// <param name="airlines"> 要解挂的政策所属的航空公司 </param>
        /// <returns> 返回解挂操作是否成功 </returns>
        public static bool UnSuspendPolicies(Guid owner, string @operator, string reason, string ip, ChinaPay.B3B.Common.Enums.PublishRole OperatorRoleType, params string[] airlines)
        {
            if (owner == Guid.Empty) throw new InvalidOperationException("缺少政策所有者。");
            if (String.IsNullOrEmpty(@operator)) throw new InvalidOperationException("缺少解挂的操作者。");
            if (airlines == null || airlines.Length == 0) throw new InvalidOperationException("缺少要解挂的政策所属的航空公司。");
            Company company = (from c in DataContext.Companies
                               join e in DataContext.Employees on c.Id equals e.Owner
                               where e.Login == @operator
                               select c).SingleOrDefault();

            if (company == null)
                throw new InvalidOperationException("指定的操作者不存在，或不属于任何公司。");

            using (var trans = new TransactionScope())
            {
                DataContext.SuspendedPolicies.Delete(sp => sp.Company == owner && airlines.Contains(sp.Airline));
                DataContext.SuspendOperations.Insert(new SuspendOperation
                {
                    Airlines = String.Join(",", airlines),
                    Id = Guid.NewGuid(),
                    Operator = @operator,
                    OperateTime = DateTime.Now,
                    OperatorRoleType = OperatorRoleType,
                    OperateType = PolicySuspendOperationType.Unsuspend,
                    Owner = owner,
                    Reason = reason,
                    IP = ip,
                    CompanyId = company.Id,
                    CompanyName = company.AbbreviateName
                });
                trans.Complete();
                return true;
            }
        }

        private static bool LockOperate(PolicyType type, PolicyFreezeOperationType opType, string @operator, string reason, OperatorRole role, params Guid[] ids)
        {
            if (ids == null || ids.Length == 0) throw new InvalidOperationException("缺少要锁定的政策。");
            bool freezed = opType == PolicyFreezeOperationType.Freeze;
            string idlist = String.Join(",", ids);
            using (var trans = new TransactionScope())
            {
                switch (type)
                {
                    case PolicyType.Normal:
                        DataContext.NormalPolicies.Update(p => new
                        {
                            Freezed = freezed
                        }, p => ids.Contains(p.Id));
                        break;
                    case PolicyType.Bargain:
                        DataContext.BargainPolicies.Update(p => new
                        {
                            Freezed = freezed
                        }, p => ids.Contains(p.Id));
                        break;
                    case PolicyType.Special:
                        DataContext.SpecialPolicies.Update(p => new
                        {
                            Freezed = freezed
                        }, p => ids.Contains(p.Id));
                        break;
                    //case PolicyType.RoundTrip:
                    //    DataContext.RoundTripPolicies.Update(p => new { Freezed = freezed }, p => ids.Contains(p.Id));
                    //    break;
                    case PolicyType.Notch:
                        //缺口政策
                        var repository = Factory.CreatePolicyRepository();
                        repository.NotchLock(freezed, ids);
                        break;
                    case PolicyType.Team:
                        DataContext.TeamPolicies.Update(p => new
                        {
                            Freezed = freezed
                        }, p => ids.Contains(p.Id));
                        break;
                    default:
                        throw new InvalidOperationException("未知的政策类型。");
                }
                DataContext.FreezeOperations.Insert(new FreezeOperation
                {
                    Id = Guid.NewGuid(),
                    OperateTime = DateTime.Now,
                    OperateType = opType,
                    PolicyIds = idlist,
                    Reason = reason
                });
                trans.Complete();
            }
            foreach (var guid in ids)
            {
                saveLog(OperationType.Update, opType.GetDescription() + type.GetDescription() + "政策。编号：" + guid, role, guid.ToString(), @operator);
            }
            return true;
        }

        /// <summary>
        ///     锁定指定政策
        /// </summary>
        /// <param name="type"> 政策类型 </param>
        /// <param name="operator"> </param>
        /// <param name="reason"> 锁定原因 </param>
        /// <param name="ids"> 政策 Id 列表 </param>
        /// <returns> 返回锁定操作是否成功 </returns>
        public static bool LockPolicy(PolicyType type, string @operator, string reason, OperatorRole role, params Guid[] ids)
        {
            return LockOperate(type, PolicyFreezeOperationType.Freeze, @operator, reason, role, ids);
        }

        /// <summary>
        ///     解锁指定政策
        /// </summary>
        /// <param name="type"> 政策类型 </param>
        /// <param name="operator"> </param>
        /// <param name="reason"> 解锁原因 </param>
        /// <param name="ids"> 政策 Id 列表 </param>
        /// <returns> 返回解锁操作是否成功 </returns>
        public static bool UnLockPolicy(PolicyType type, string @operator, string reason, OperatorRole role, params Guid[] ids)
        {
            return LockOperate(type, PolicyFreezeOperationType.Unfreeze, @operator, reason, role, ids);
        }

        /// <summary>
        ///     设置政策协调信息
        /// </summary>
        /// <param name="info"> </param>
        /// <returns> </returns>
        public static bool SetPolicyHarmony(PolicyHarmonyInfo info)
        {
            bool isSuccess = false;
            PolicyHarmonyInfo orginalPolicyInfo = null;
            if (info.Id != Guid.Empty)
            {
                orginalPolicyInfo = GetPolicyHarmonyInfo(info.Id);
            }
            var policyHarmony = new PolicyHarmony
            {
                Id = info.Id == Guid.Empty ? Guid.NewGuid() : info.Id,
                Account = info.Account,
                Arrival = info.Arrival,
                Airlines = info.Airlines,
                // CityLimit = info.CityLimit,
                CreateTime = info.CreateTime == DateTime.MinValue ? DateTime.Now : info.CreateTime,
                DeductionType = info.DeductionType,
                Departure = info.Departure,
                EffectiveLowerDate = info.EffectiveLowerDate,
                EffectiveUpperDate = info.EffectiveUpperDate,
                HarmonyValue = info.HarmonyValue,
                LastModifyTime = info.LastModifyTime,
                // IsVIP = info.IsVIP,
                PolicyType = info.PolicyType,
                LastModifyName = info.LastModifyName,
                Remark = info.Remark
            };
            isSuccess = policyHarmony.InsertOrUpdate();
            if (info.Id == Guid.Empty)
            {
                saveLog(OperationType.Insert,
                       string.Format("添加政策协调信息。协调信息Id:{0},航空公司:{1}, 出发城市:{2},到达城市:{3},政策类型:{4},生效日期(起始):{5},生效日期(结束):{6},返佣类型:{7},协调值:{8},账号:{9},创建时间:{10}",
                                        policyHarmony.Id, policyHarmony.Airlines, policyHarmony.Departure, policyHarmony.Arrival, policyHarmony.PolicyType, policyHarmony.EffectiveLowerDate,
                                        policyHarmony.EffectiveUpperDate, policyHarmony.DeductionType.GetDescription(), policyHarmony.HarmonyValue, policyHarmony.Account, policyHarmony.CreateTime),
                       OperatorRole.Platform, policyHarmony.Id.ToString(), info.Account);
            }
            else
            {
                string orginalContent = string.Format("协调信息Id:{0},航空公司:{1}, 出发城市:{2},到达城市:{3},政策类型:{4},生效日期(起始):{5},生效日期(结束):{6},返佣类型:{7},协调值:{8},账号:{9},创建时间:{10}",
                                        orginalPolicyInfo.Id, orginalPolicyInfo.Airlines, orginalPolicyInfo.Departure, orginalPolicyInfo.Arrival, orginalPolicyInfo.PolicyType, orginalPolicyInfo.EffectiveLowerDate,
                                        orginalPolicyInfo.EffectiveUpperDate, orginalPolicyInfo.DeductionType.GetDescription(), orginalPolicyInfo.HarmonyValue, orginalPolicyInfo.Account, orginalPolicyInfo.CreateTime);
                string newContent = string.Format("协调信息Id:{0},航空公司:{1}, 出发城市:{2},到达城市:{3},政策类型:{4},生效日期(起始):{5},生效日期(结束):{6},返佣类型:{7},协调值:{8},账号:{9},创建时间:{10}",
                                         policyHarmony.Id, policyHarmony.Airlines, policyHarmony.Departure, policyHarmony.Arrival, policyHarmony.PolicyType, policyHarmony.EffectiveLowerDate,
                                         policyHarmony.EffectiveUpperDate, policyHarmony.DeductionType.GetDescription(), policyHarmony.HarmonyValue, policyHarmony.Account, policyHarmony.CreateTime);
                saveLog(OperationType.Update, "修改政策协调信息。由" + orginalContent + "修改为" + newContent, OperatorRole.Platform, policyHarmony.Id.ToString(), info.Account);
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除指定的协调信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool Delete(string operatorAccount, params Guid[] ids)
        {
            bool isSuccess = false;
            isSuccess = ids.Length == 1
                       ? DataContext.PolicyHarmonies.Delete(p => p.Id == ids[0]) > 0
                       : DataContext.PolicyHarmonies.Delete(p => ids.Contains(p.Id)) > 0;
            saveDeleteLog("政策协调信息", OperatorRole.Platform, ids.Join(",", item => item.ToString()), operatorAccount);
            return isSuccess;
        }

        public static PolicyHarmonyInfo GetPolicyHarmonyInfo(Guid id)
        {
            return DataContext.PolicyHarmonies.Where(p => p.Id == id).Select(p => new PolicyHarmonyInfo
            {
                Account = p.Account,
                Airlines = p.Airlines,
                Arrival = p.Arrival,
                //CityLimit = p.CityLimit,
                CreateTime = p.CreateTime,
                DeductionType = p.DeductionType,
                Departure = p.Departure,
                EffectiveLowerDate = p.EffectiveLowerDate,
                EffectiveUpperDate = p.EffectiveUpperDate,
                HarmonyValue = p.HarmonyValue,
                LastModifyTime = p.LastModifyTime,
                LastModifyName = p.LastModifyName,
                Id = p.Id,
                //IsVIP = p.IsVIP,
                PolicyType = p.PolicyType,
                Remark = p.Remark
            }).SingleOrDefault();
        }

        public static IEnumerable<PolicyInfoBase> QueryPolicies(string departure, DateTime flightStartDate, DateTime flightEndDate, VoyageType voyageType, PolicyType policyType, string airline)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryPolicies(departure, flightStartDate, flightEndDate, voyageType, policyType, airline);

        }
        public static IEnumerable<PolicyInfoBase> QueryPolicies(string airLine, DataTable voyages, VoyageType voyageType, PolicyType policyType)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryPolicies(airLine, voyages, voyageType, policyType);

        }

        public static List<NormalPolicyInfo> QueryNormalPolicies(PolicyQueryParameter parameter, Pagination pagination)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryNormalPolicies(parameter, pagination);

        }

        public static List<TeamPolicyInfo> QueryTeamPolicies(PolicyQueryParameter parameter, Pagination pagination)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryTeamPolicies(parameter, pagination);

        }

        public static List<BargainPolicyInfo> QueryBargainPolicies(PolicyQueryParameter parameter, Pagination pagination)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryBargainPolicies(parameter, pagination);
        }

        public static List<SpecialPolicyInfo> QuerySpecialPolicies(PolicyQueryParameter parameter, Pagination pagination)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QuerySpecialPolicies(parameter, pagination);

        }

        public static List<NotchPolicyInfo> QueryNotchPolicies(PolicyQueryParameter parameter, Pagination pagination)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryNotchPolicies(parameter, pagination);

        }
        /// <summary>
        /// 查询满足条件下的所有政策编号
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Dictionary<Guid, bool> QueryPolicyIds(PolicyQueryParameter parameter)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryPolicyIds(parameter);
        }

        /// <summary>
        ///     查询政策挂起信息
        /// </summary>
        /// <returns> 返回政策挂起信息列表 </returns>
        public static PagedResult<SuspendInfo> GetSuspendInfos<TKey>(SuspendInfoQueryParameter parameter, Expression<Func<SuspendInfo, TKey>> orderBy, OrderMode orderMode = OrderMode.Ascending)
        {
            IQueryable<SuspendInfo> query = from c in DataContext.Companies
                                            where c.Type != CompanyType.Platform && c.Type != CompanyType.Purchaser && (parameter.Company == null || c.Id == parameter.Company)
                                            join cs in DataContext.SuspendedPolicies.Where(s => !s.SuspendedByPlatform) on c.Id equals cs.Company into css
                                            join ps in DataContext.SuspendedPolicies.Where(s => s.SuspendedByPlatform) on c.Id equals ps.Company into pss
                                            select new SuspendInfo
                                            {
                                                AbbreviateName = c.AbbreviateName,
                                                CompanyId = c.Id,
                                                CompanyType = c.Type,
                                                SuspendByCompany = css.Select(ci => ci.Airline),
                                                SuspendByPlatform = pss.Select(ci => ci.Airline)
                                            };
            var pager = new Pager<SuspendInfo, TKey>
            {
                OrderBy = orderBy,
                OrderMode = orderMode,
                PageIndex = parameter.PageIndex,
                PageSize = parameter.PageSize
            };
            return pager.Paging(query);
        }

        public static IEnumerable<SuspendOperation> GetSuspendOperation(SuspendOperationQueryParameter parameter, Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var db = Factory.CreatePolicysRepository(command);
                return db.QuerySuspendOption(parameter, pagination);
            }
        }

        /// <summary>
        ///     查询指定公司的政策挂起信息
        /// </summary>
        /// <param name="companyId"> 被挂起政策的公司 Id </param>
        /// <returns> 返回该公司的政策挂起信息 </returns>
        public static SuspendInfo GetSuspendInfo(Guid companyId)
        {
            using (var command = Factory.CreateCommand())
            {
                var db = Factory.CreateSuspendInfoRepository(command);
                return db.GetSuspendInfo(companyId);
            }
        }

        /// <summary>
        /// 添加默认政策
        /// </summary>
        /// <param name="policy">默认政策信息</param>
        /// <returns>返回添加操作是否成功</returns>
        public static bool AddDefaultPolicy(DefaultPolicy policy)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            return policy.Insert();
        }

        /// <summary>
        /// 修改默认政策
        /// </summary>
        /// <param name="policy">默认政策信息</param>
        /// <returns>返回修改操作是否成功</returns>
        public static bool UpdateDefaultPolicy(DefaultPolicy policy)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            return policy.Update();
        }

        public static bool SaveDefaultPolicy(DefaultPolicy policy, string operatorAccount)
        {
            bool isSuccess = false;
            if (policy == null) throw new ArgumentNullException("policy");
            var orginalPolicy = GetNormalDefaultPolicy(policy.Airline);
            isSuccess = policy.InsertOrUpdate();
            if (orginalPolicy == null)
            {
                string content = string.Format("添加普通默认政策。航空公司:{0},成人默认出票方Id:{1},成人默认佣金:{2},儿童默认出票方:{3},儿童默认佣金:{4}",
                                              policy.Airline, policy.AdultProvider, policy.AdultCommission, policy.ChildProvider, policy.ChildCommission);
                saveLog(OperationType.Insert, content, OperatorRole.Platform, policy.Airline, operatorAccount);
            }
            else
            {
                string orginalContent = string.Format("航空公司:{0},成人默认出票方Id:{1},成人默认佣金:{2},儿童默认出票方:{3},儿童默认佣金:{4}",
                                            orginalPolicy.Airline, orginalPolicy.AdultProviderId, orginalPolicy.AdultCommission, orginalPolicy.ChildProviderId, orginalPolicy.ChildCommission);
                string newContent = string.Format("航空公司:{0},成人默认出票方Id:{1},成人默认佣金:{2},儿童默认出票方:{3},儿童默认佣金:{4}",
                                            policy.Airline, policy.AdultProvider, policy.AdultCommission, policy.ChildProvider, policy.ChildCommission);
                saveLog(OperationType.Update, "修改普通默认政策。由" + orginalContent + "修改为" + newContent, OperatorRole.Platform, policy.Airline, operatorAccount);
            }
            return isSuccess;
        }

        public static IEnumerable<NormalDefaultPolicyInfo> GetDefaultPolicies(DefaultPolicyQueryParameter parameter, Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var db = Factory.CreatePolicysRepository(command);
                return db.QueryDefaultList(parameter, pagination);
            }

        }

        /// <summary>
        /// 保存特价默认政策
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static bool SaveBargainDefaultPolicy(BargainDefaultPolicy policy, string operatorAccount)
        {
            bool isSuccess = false;
            if (policy == null) throw new ArgumentNullException("policy");
            var orginalPolicy = GetBargainDefaultPolicy(policy.Airline, policy.Province);
            isSuccess = DataContext.BargainDefaultPolicies.InsertOrUpdate(policy, c => c.Province == policy.Province && c.Airline == policy.Airline) > 0;
            if (orginalPolicy == null)
            {
                string content = string.Format("添加特价默认政策。航空公司:{0},省份:{1}.成人默认出票方Id:{2},成人默认佣金:{3}",
                                              policy.Airline, policy.Province, policy.AdultProvider, policy.AdultCommission);
                saveLog(OperationType.Insert, content, OperatorRole.Platform, policy.Airline + "," + policy.Province, operatorAccount);
            }
            else
            {
                string orginalContent = string.Format("航空公司:{0},省份:{1}.成人默认出票方Id:{2},成人默认佣金:{3}",
                                            orginalPolicy.Airline, orginalPolicy.ProvinceCode, orginalPolicy.AdultProviderId, orginalPolicy.AdultCommission);
                string newContent = string.Format("航空公司:{0},省份:{1}.成人默认出票方Id:{2},成人默认佣金:{3}",
                                            policy.Airline, policy.Province, policy.AdultProvider, policy.AdultCommission);
                saveLog(OperationType.Update, "修改特价默认政策。由" + orginalContent + "修改为" + newContent, OperatorRole.Platform, policy.Airline + "," + policy.Province, operatorAccount);
            }
            return isSuccess;
        }

        /// <summary>
        /// 根据航空公司代码和所在省份代码，获取特价默认政策
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="provinceCode">省份代码</param>
        /// <returns>特价默认政策</returns>
        public static BargainDefaultPolicyInfo GetBargainDefaultPolicy(string airline, string provinceCode)
        {
            if (string.IsNullOrEmpty(airline))
                throw new ArgumentNullException("airline");
            if (string.IsNullOrWhiteSpace(provinceCode))
                throw new ArgumentNullException("provinceCode");

            var bargainDefaultPolicyInfo = (from dp in DataContext.BargainDefaultPolicies
                                            join acp in DataContext.Companies on dp.AdultProvider equals acp.Id
                                            where dp.Airline == airline && dp.Province == provinceCode
                                            select new BargainDefaultPolicyInfo
                                            {
                                                AdultCommission = dp.AdultCommission,
                                                AdultProviderId = acp.Id,
                                                AdultProviderAbbreviateName = acp.AbbreviateName,
                                                AdultProviderName = acp.Name,
                                                Airline = dp.Airline,
                                                ProvinceCode = dp.Province
                                            }).SingleOrDefault();
            return bargainDefaultPolicyInfo;
        }

        /// <summary>
        /// 删除特价默认政策
        /// </summary>
        /// <param name="airline"></param>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public static bool DeleteBargainDefaultPolicy(string airline, string provinceCode, string operatorAccount)
        {
            bool isSuccess = false;
            isSuccess = DataContext.BargainDefaultPolicies.Delete(bp => bp.Airline == airline && bp.Province == provinceCode) > 0;
            saveDeleteLog("特价默认政策", OperatorRole.Platform, airline + "," + provinceCode, operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 根据公司组限制，获取所有者默认政策（暂时是只对成人适用）
        /// </summary>
        /// <param name="limit">公司组限制</param>
        /// <returns>所有者默认政策</returns>
        /// <remarks>
        /// 2012-11-06 deng.zhao
        /// </remarks>
        [Obsolete]
        public static OwnerDefaultPolicyInfo GetOwnerDefaultPolicy(CompanyGroupLimitation limit)
        {
            if (limit == null)
                throw new ArgumentNullException("limit");

            return (from l in DataContext.CompanyGroupLimitations
                    join g in DataContext.CompanyGroups on l.Group equals g.Id
                    join c in DataContext.Companies on g.Company equals c.Id
                    join w in DataContext.WorkingSettings on c.Id equals w.Company
                    where l.Id == limit.Id && w.RebateForDefault != null
                    select new OwnerDefaultPolicyInfo
                    {
                        LimitationId = l.Id,
                        AdultCommission = w.RebateForDefault.Value,
                        AdultProviderAbbreviateName = c.AbbreviateName,
                        AdultProviderName = c.Name,
                        AdultProviderId = c.Id,
                        Airline = l.Airlines,
                        ChildCommission = w.RebateForChild.HasValue ? w.RebateForChild.Value : 0,
                        ChildProviderAbbreviateName = c.AbbreviateName,
                        ChildProviderName = c.Name,
                        ChildProviderId = c.Id
                    }).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limitation"></param>
        /// <param name="superiorId"></param>
        /// <returns></returns>
        /// <remarks>
        /// 用于替代同名方法，只对成人。
        /// </remarks>
        public static OwnerDefaultPolicyInfo GetOwnerDefaultPolicy(PurchaseLimitationInfo limitation, Guid superiorId)
        {
            if (limitation == null)
                throw new ArgumentNullException("limitation");

            var company = (from c in DataContext.Companies
                           where c.Id == superiorId
                           select c).First();

            return new OwnerDefaultPolicyInfo
                       {
                           LimitationId = limitation.LimitationId,
                           AdultCommission = limitation.Debate,
                           AdultProviderAbbreviateName = company.AbbreviateName,
                           AdultProviderName = company.Name,
                           AdultProviderId = company.Id,
                       };
        }


        public static IEnumerable<BargainDefaultPolicyInfo> GetBargainDefaultPolicies(BargainDefaultPolicyQueryParameter parameter, Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var db = Factory.CreatePolicysRepository(command);
                return db.QueryBargainDefaultList(parameter, pagination);
            }
        }

        /// <summary>
        /// 删除指定航空公司的默认政策
        /// </summary>
        /// <param name="airline">航空公司编码</param>
        /// <returns>返回删除操作是否成功。</returns>
        public static bool DeleteDefaultPolicy(string airline)
        {
            return DataContext.DefaultPolicies.Delete(dp => dp.Airline == airline) > 0;
        }

        /// <summary>
        /// 根据航空公司代码，获取普通默认政策
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <returns>普通默认政策</returns>
        public static NormalDefaultPolicyInfo GetNormalDefaultPolicy(string airline)
        {
            if (String.IsNullOrEmpty(airline))
                throw new ArgumentNullException("airline");

            return (from dp in DataContext.DefaultPolicies
                    join acp in DataContext.Companies on dp.AdultProvider equals acp.Id
                    join ccp in DataContext.Companies on dp.ChildProvider equals ccp.Id
                    where dp.Airline == airline
                    select new NormalDefaultPolicyInfo
                    {
                        AdultCommission = dp.AdultCommission,
                        AdultProviderId = acp.Id,
                        AdultProviderAbbreviateName = acp.AbbreviateName,
                        AdultProviderName = acp.Name,
                        Airline = dp.Airline,
                        ChildCommission = dp.ChildCommission,
                        ChildProviderId = ccp.Id,
                        ChildProviderAbbreviateName = ccp.AbbreviateName,
                        ChildProviderName = ccp.Name
                    }).SingleOrDefault();
        }

        public static IEnumerable<NormalDefaultPolicyInfo> GetAllDefaultPolicies()
        {
            var query = from dp in DataContext.DefaultPolicies
                        join acp in DataContext.Companies on dp.AdultProvider equals acp.Id
                        join ccp in DataContext.Companies on dp.ChildProvider equals ccp.Id
                        select new NormalDefaultPolicyInfo
                        {
                            AdultCommission = dp.AdultCommission,
                            AdultProviderId = acp.Id,
                            AdultProviderAbbreviateName = acp.AbbreviateName,
                            AdultProviderName = acp.Name,
                            Airline = dp.Airline,
                            ChildCommission = dp.ChildCommission,
                            ChildProviderId = ccp.Id,
                            ChildProviderAbbreviateName = ccp.AbbreviateName,
                            ChildProviderName = ccp.Name
                        };
            return query.ToList();
        }

        public static IEnumerable<PolicyHarmonyInfo> GetAllPolicyHarmonies()
        {
            return from ph in DataContext.PolicyHarmonies
                   select new PolicyHarmonyInfo
                   {
                       Account = ph.Account,
                       Airlines = ph.Airlines,
                       Arrival = ph.Arrival,
                       // CityLimit = ph.CityLimit,
                       DeductionType = ph.DeductionType,
                       Departure = ph.Departure,
                       EffectiveLowerDate = ph.EffectiveLowerDate,
                       EffectiveUpperDate = ph.EffectiveUpperDate,
                       HarmonyValue = ph.HarmonyValue,
                       LastModifyTime = ph.LastModifyTime,
                       LastModifyName = ph.LastModifyName,
                       Id = ph.Id,
                       // IsVIP = ph.IsVIP,
                       PolicyType = ph.PolicyType,
                       Remark = ph.Remark,
                       CreateTime = ph.CreateTime
                   };
        }

        public static IEnumerable<PolicySettingInfo> GetAllPolicySettings()
        {
            return from ps in DataContext.PolicySettings
                   join pr in DataContext.PolicySettingPeriods on ps.Id equals pr.PolicySetting into prs
                   select new PolicySettingInfo
                   {
                       Id = ps.Id,
                       Departure = ps.Departure,
                       Airline = ps.Airline,
                       Arrivals = ps.Arrivals,
                       Berths = ps.Berths,
                       EffectiveTimeStart = ps.EffectiveTimeStart,
                       EffectiveTimeEnd = ps.EffectiveTimeEnd,
                       Periods = prs,
                       Remark = ps.Remark,
                       // VoyageType = ps.VoyageType,
                       CreateTime = ps.CreateTime,
                       Creator = ps.Creator,
                       LastModifyTime = ps.LastModifyTime
                   };
        }

        private static PolicySetting GetPolicySetting(PolicySettingInfo setting)
        {
            return new PolicySetting
            {
                Airline = setting.Airline,
                Arrivals = setting.Arrivals,
                Berths = setting.Berths,
                CreateTime = DateTime.Now,
                Departure = setting.Departure,
                EffectiveTimeEnd = setting.EffectiveTimeEnd,
                EffectiveTimeStart = setting.EffectiveTimeStart,
                Id = setting.Id == Guid.Empty ? Guid.NewGuid() : setting.Id,
                Remark = setting.Remark,
                Enable = setting.Enable,
                RebateType = setting.RebateType
                //MountStart = setting.MountStart,
                //MountEnd = setting.MountEnd,
                // VoyageType = setting.VoyageType
            };
        }

        public static bool AddPolicySetting(PolicySettingInfo setting, string creator)
        {
            if (setting == null) throw new ArgumentNullException("setting");
            var ps = GetPolicySetting(setting);
            ps.Creator = creator;
            ps.LastModifyTime = DateTime.Now;
            setting.Periods.ForEach(pr =>
            {
                pr.Id = Guid.NewGuid();
                pr.PolicySetting = ps.Id;
            });

            using (var trans = new TransactionScope())
            {
                DataContext.PolicySettings.Insert(ps);
                DataContext.PolicySettingPeriods.Batch(setting.Periods, (set, pr) => set.Insert(pr));
                trans.Complete();
            }
            string content = string.Format("政策设置Id:{0},航空公司:{1},出发城市:{2},到达城市:{3},适用舱位:{4},生效起始时间:{5},生效结束时间:{6},是否启用:{7},创建者:{8},创建时间:{9}",
               ps.Id, ps.Airline, ps.Departure, ps.Arrivals, ps.Berths, ps.EffectiveTimeStart, ps.EffectiveTimeEnd, ps.Enable ? "是" : "否", ps.Creator, ps.CreateTime);
            foreach (var item in setting.Periods)
            {
                content += string.Format("扣点贴点区域信息Id:{0},区间所属的政策设置 Id:{1},扣点/贴点:{2},区域起始:{3},区域结束:{4}", item.Id, item.PolicySetting, (item.Rebate > 0 ? "扣点" : "贴点") + item.Rebate, item.PeriodStart, item.PeriodEnd);
            }
            saveLog(OperationType.Insert, "添加政策设置。" + content, OperatorRole.Platform, ps.Id.ToString(), creator);
            return true;
        }

        public static bool UpdatePolicySetting(PolicySettingInfo setting, string operatorAccount)
        {
            if (setting == null) throw new ArgumentNullException("setting");
            var originalSetting = GetPolicySetting(setting.Id);
            if (originalSetting == null) throw new CustomException("原政策设置信息不存在");
            var ps = GetPolicySetting(setting);
            ps.Creator = originalSetting.Creator;
            ps.CreateTime = originalSetting.CreateTime;
            ps.LastModifyTime = DateTime.Now;
            setting.Periods.ForEach(pr =>
            {
                pr.Id = Guid.NewGuid();
                pr.PolicySetting = ps.Id;
            });
            using (var trans = new TransactionScope())
            {
                DataContext.PolicySettingPeriods.Delete(pr => pr.PolicySetting == ps.Id);
                DataContext.PolicySettingPeriods.Batch(setting.Periods, (set, pr) => set.Insert(pr));
                DataContext.PolicySettings.Update(ps);

                trans.Complete();
            }
            string orginalContent = string.Format("政策设置Id:{0},航空公司:{1},出发城市:{2},到达城市:{3},适用舱位:{4},生效起始时间:{5},生效结束时间:{6},是否启用:{7},创建者:{8},创建时间:{9}",
                                    originalSetting.Id, originalSetting.Airline, originalSetting.Departure, originalSetting.Arrivals, originalSetting.Berths, originalSetting.EffectiveTimeStart, originalSetting.EffectiveTimeEnd, originalSetting.Enable ? "是" : "否", originalSetting.Creator, originalSetting.CreateTime);
            foreach (var item in originalSetting.Periods)
            {
                orginalContent += string.Format("扣点贴点区域信息Id:{0},区间所属的政策设置 Id:{1},扣点/贴点:{2},区域起始:{3},区域结束:{4}", item.Id, item.PolicySetting, (item.Rebate > 0 ? "扣点" : "贴点") + item.Rebate, item.PeriodStart, item.PeriodEnd);
            }
            string newContent = string.Format("政策设置Id:{0},航空公司:{1},出发城市:{2},到达城市:{3},适用舱位:{4},生效起始时间:{5},生效结束时间:{6},是否启用:{7},创建者:{8},创建时间:{9}",
              ps.Id, ps.Airline, ps.Departure, ps.Arrivals, ps.Berths, ps.EffectiveTimeStart, ps.EffectiveTimeEnd, ps.Enable ? "是" : "否", ps.Creator, ps.CreateTime);
            foreach (var item in setting.Periods)
            {
                newContent += string.Format("扣点贴点区域信息Id:{0},区间所属的政策设置 Id:{1},扣点/贴点:{2},区域起始:{3},区域结束:{4},", item.Id, item.PolicySetting, (item.Rebate > 0 ? "扣点" : "贴点") + item.Rebate, item.PeriodStart, item.PeriodEnd);
            }
            saveLog(OperationType.Update, "修改政策设置。由" + orginalContent + "修改为" + newContent, OperatorRole.Platform, setting.Id.ToString(), operatorAccount);
            return true;
        }

        public static bool DeletePolicySetting(string operatorAccount, params Guid[] ids)
        {
            using (var trans = new TransactionScope())
            {
                if (ids.Length == 1)
                {
                    DataContext.PolicySettingPeriods.Delete(pr => pr.PolicySetting == ids[0]);
                    DataContext.PolicySettings.Delete(ps => ps.Id == ids[0]);
                }
                else
                {
                    DataContext.PolicySettingPeriods.Delete(pr => ids.Contains(pr.PolicySetting));
                    DataContext.PolicySettings.Delete(ps => ids.Contains(ps.Id));
                }
                trans.Complete();
            }
            saveDeleteLog("政策设置", OperatorRole.Platform, ids.Join(",", item => item.ToString()), operatorAccount);
            return true;
        }

        public static IEnumerable<PolicySettingInfo> GetPolicySettings(PolicySettingQueryParameter parameter, Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var db = Factory.CreatePolicysRepository(command);
                return db.QueryPolicySettingList(parameter, pagination);
            }

        }

        public static PolicySettingInfo GetPolicySetting(Guid id)
        {
            return (from ps in DataContext.PolicySettings
                    join pr in DataContext.PolicySettingPeriods on ps.Id equals pr.PolicySetting into prs
                    where ps.Id == id
                    select new PolicySettingInfo
                    {
                        Id = ps.Id,
                        Departure = ps.Departure,
                        Airline = ps.Airline,
                        Arrivals = ps.Arrivals,
                        Berths = ps.Berths,
                        EffectiveTimeStart = ps.EffectiveTimeStart,
                        EffectiveTimeEnd = ps.EffectiveTimeEnd,
                        Periods = prs,
                        Remark = ps.Remark,
                        Enable = ps.Enable,
                        // VoyageType = ps.VoyageType,
                        //MountStart = ps.MountStart,
                        //MountEnd = ps.MountEnd,
                        CreateTime = ps.CreateTime,
                        Creator = ps.Creator,
                        LastModifyTime = ps.LastModifyTime
                    }).SingleOrDefault();
        }

        public static int Increase(Guid policyId, int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("数量不能小于 0.");
            var policy = GetSpecialPolicy(policyId);
            if (policy == null)
                throw new InvalidOperationException("指定政策不存在。");

            policy.ResourceAmount += amount;
            if (!UpdateSpecialPolicy(policyId, amount)) throw new CustomException("该特殊票已售罄或资源数不足");
            return policy.ResourceAmount;

        }

        public static int Decrease(Guid policyId, int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("数量不能小于 0.");
            var policy = GetSpecialPolicy(policyId);
            if (policy == null)
                throw new InvalidOperationException("指定政策不存在。");

            if (policy.ResourceAmount < amount)
                throw new InvalidOperationException("资源数量不足。");

            if (!UpdateSpecialPolicy(policyId, amount)) throw new CustomException("该特殊票已售罄或资源数不足");
            return policy.ResourceAmount;
        }


        #region Normal Policy

        public static bool ReleaseNormalPolicies(NormalPolicyReleaseInfo info, string creator)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (String.IsNullOrEmpty(creator)) throw new ArgumentNullException("creator");
            if (info.BasicInfo == null) throw new InvalidOperationException("缺少政策基本信息。");
            if (info.Rebates.Count == 0) throw new InvalidOperationException("缺少政策返点信息。");

            if (info.BasicInfo.Owner == Guid.Empty)
                throw new InvalidOperationException("缺少所有者。");

            Company company = DataContext.Companies.SingleOrDefault(c => c.Id == info.BasicInfo.Owner);
            if (company == null)
                throw new InvalidOperationException("政策所属的公司不存在。");
            if (company.Type != CompanyType.Provider)
                throw new InvalidOperationException("指定的公司不允许发布普通政策。");
            if (!company.Enabled)
                throw new InvalidOperationException("指定的公司已被禁用，不允许发布普通政策。");
            if (!company.Audited)
                throw new InvalidOperationException("指定的公司未通过审核，不允许发布普通政策。");

            SettingPolicy setting = DataContext.SettingPolicies.SingleOrDefault(s => s.Company == info.BasicInfo.Owner);
            if (setting == null)
                throw new InvalidOperationException("缺少指定公司的政策设置信息。");
            if (!setting.Airlines.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Contains(info.BasicInfo.Airline))
                throw new InvalidOperationException(String.Format("指定公司不允许发布航空公司：\"{0}\" 的普通政策。", info.BasicInfo.Airline));
            if (setting.Departure != "*")
            {
                string[] settingArray = setting.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string[] policyArray = info.BasicInfo.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                Array.ForEach(policyArray, s =>
                {
                    if (!settingArray.Contains(s))
                        throw new InvalidOperationException(String.Format("指定的公司不允许发布 \"{0}\" 出港的普通政策。", s));
                });
            }
            var companyParameters = CompanyService.GetCompanyParameter(company.Id);
            IEnumerable<NormalPolicy> policies = info.Rebates.Select(r => new NormalPolicy
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                Airline = info.BasicInfo.Airline,
                CustomCode = info.BasicInfo.CustomCode,
                IsInternal = companyParameters != null && companyParameters.CanHaveSubordinate,
                IsPeer = companyParameters != null && companyParameters.AllowBrotherPurchase,
                Transit = info.BasicInfo.Transit,
                Arrival = info.BasicInfo.Arrival,
                Departure = info.BasicInfo.Departure,
                //DepartureDatesFilter = info.BasicInfo.DepartureDatesFilter,
                //DepartureDatesFilterType = info.BasicInfo.DepartureDatesFilterType,
                DepartureFlightsFilter = info.BasicInfo.DepartureFlightsFilterType == LimitType.None ? "" : info.BasicInfo.DepartureFlightsFilter,
                DepartureFlightsFilterType = info.BasicInfo.DepartureFlightsFilterType,
                ExceptAirways = info.BasicInfo.ExceptAirways.ToUpper(),
                OfficeCode = info.BasicInfo.OfficeCode,
                ImpowerOffice = info.BasicInfo.ImpowerOffice,
                Owner = info.BasicInfo.Owner,
                Remark = info.BasicInfo.Remark,
                AbbreviateName = company.AbbreviateName,
                //ReturnDatesFilter = info.BasicInfo.ReturnDatesFilter,
                //ReturnDatesFilterType = info.BasicInfo.ReturnDatesFilterType,
                ReturnFlightsFilter = info.BasicInfo.ReturnFlightsFilterType == LimitType.None ? "" : info.BasicInfo.ReturnFlightsFilter,
                ReturnFlightsFilterType = info.BasicInfo.ReturnFlightsFilterType,
                //TravelDays = info.BasicInfo.TravelDays,
                VoyageType = info.BasicInfo.VoyageType,
                DrawerCondition = info.BasicInfo.DrawerCondition,
                Audited = r.AutoAudit,
                AutoAudit = r.AutoAudit,
                AutoPrint = r.AutoPrint,
                Berths = r.Berths,
                ChangePNR = r.ChangePNR,
                Freezed = false,
                DepartureDateFilter = r.DepartureDateFilter,
                DepartureWeekFilter = r.DepartureWeekFilter,
                InternalCommission = r.InternalCommission / 100,
                ProfessionCommission = r.ProfessionCommission / 100,
                DepartureDateStart = r.DepartureDateStart,
                DepartureDateEnd = r.DepartureDateEnd,
                //ReturnDateStart = r.ReturnDateStart,
                //ReturnDateEnd = r.ReturnDateEnd,
                StartPrintDate = r.StartPrintDate,
                SubordinateCommission = r.SubordinateCommission / 100,
                SuitReduce = r.SuitReduce,
                MultiSuitReduce = r.MultiSuitReduce,
                Suspended = false,
                TicketType = r.TicketType,
                //Vip = r.Vip,
                LastModifyTime = DateTime.Now,
                Creator = creator,
                PrintBeforeTwoHours = r.PrintBeforeTwoHours
            }).ToList();

            try
            {

                var repository = Factory.CreatePolicyRepository();
                repository.InsertNormalPolicy(policies);
                //repository.InsertNormalPolicyDeparture(policies);
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw new Exception("添加政策失败，请稍后重试！");
            }
            #region 日志
            foreach (var p in policies)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("航空公司：{0}，", p.Airline);
                str.AppendFormat("自定义编码：{0}，", p.CustomCode);
                str.AppendFormat("OFFICE号：{0}，", p.OfficeCode);
                str.AppendFormat("出发地：{0}，", p.Departure);
                if (p.VoyageType == VoyageType.TransitWay)
                    str.AppendFormat("中转地：{0}，", p.Transit);
                str.AppendFormat("到达地：{0}，", p.Arrival);
                str.AppendFormat("排除航段：{0}，", p.ExceptAirways);
                if (p.VoyageType == VoyageType.OneWay)
                {
                    str.AppendFormat("航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.OneWayOrRound || p.VoyageType == VoyageType.RoundTrip)
                {
                    str.AppendFormat("去程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("回程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.TransitWay)
                {
                    str.AppendFormat("第一程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("第二程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                str.AppendFormat("出票条件：{0}，", p.DrawerCondition);
                str.AppendFormat("政策备注：{0}，", p.Remark);
                str.AppendFormat("适用航班日期：{0}，", p.DepartureDateStart + "至" + p.DepartureDateEnd);
                str.AppendFormat("开始出票日期：{0}，", p.StartPrintDate);
                str.AppendFormat("航班排除日期：{0}，", p.DepartureDateFilter);
                str.AppendFormat("适用班期：{0}，", p.DepartureWeekFilter);
                str.AppendFormat("舱位：{0}，", p.Berths);
                str.AppendFormat("客票类型：{0}", p.TicketType.GetDescription());
                if (p.IsInternal)
                    str.AppendFormat("内部返点：{0}，", p.InternalCommission);
                if (p.IsPeer)
                    str.AppendFormat("同行返点：{0}，", p.ProfessionCommission);
                str.AppendFormat("下级返点：{0}，", p.SubordinateCommission);
                str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", p.PrintBeforeTwoHours ? "是" : "否");
                str.AppendFormat("是否适用降舱：{0}，", p.SuitReduce ? "是" : "否");


                saveAddLog("普通政策 [ " + p.VoyageType.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, p.Id.ToString(), creator);
            }
            #endregion
            return true;
            // DataContext.NormalPolicies.Batch(policies, (set, p) => set.Insert(p));

        }

        public static bool UpdateNormalPolicy(NormalPolicy policy, string creator)
        {
            if (policy == null) throw new ArgumentNullException("policy");

            StringBuilder str = new StringBuilder();
            str.AppendFormat("航空公司：{0}，", policy.Airline);
            str.AppendFormat("自定义编码：{0}，", policy.CustomCode);
            str.AppendFormat("OFFICE号：{0}，", policy.OfficeCode);
            str.AppendFormat("出发地：{0}，", policy.Departure);
            if (policy.VoyageType == VoyageType.TransitWay)
                str.AppendFormat("中转地：{0}，", policy.Transit);
            str.AppendFormat("到达地：{0}，", policy.Arrival);
            str.AppendFormat("排除航段：{0}，", policy.ExceptAirways);
            if (policy.VoyageType == VoyageType.OneWay)
            {
                str.AppendFormat("航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            if (policy.VoyageType == VoyageType.OneWayOrRound || policy.VoyageType == VoyageType.RoundTrip)
            {
                str.AppendFormat("去程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
                str.AppendFormat("回程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            if (policy.VoyageType == VoyageType.TransitWay)
            {
                str.AppendFormat("第一程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
                str.AppendFormat("第二程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            str.AppendFormat("出票条件：{0}，", policy.DrawerCondition);
            str.AppendFormat("政策备注：{0}，", policy.Remark);
            str.AppendFormat("适用航班日期：{0}，", policy.DepartureDateStart + "至" + policy.DepartureDateEnd);
            str.AppendFormat("开始出票日期：{0}，", policy.StartPrintDate);
            str.AppendFormat("航班排除日期：{0}，", policy.DepartureDateFilter);
            str.AppendFormat("适用班期：{0}，", policy.DepartureWeekFilter);
            str.AppendFormat("舱位：{0}，", policy.Berths);
            str.AppendFormat("客票类型：{0}", policy.TicketType.GetDescription());
            if (policy.IsInternal)
                str.AppendFormat("内部返点：{0}，", policy.InternalCommission * 100);
            if (policy.IsPeer)
                str.AppendFormat("同行返点：{0}，", policy.ProfessionCommission * 100);
            str.AppendFormat("下级返点：{0}，", policy.SubordinateCommission * 100);
            str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", policy.PrintBeforeTwoHours ? "是" : "否");
            str.AppendFormat("是否适用降舱：{0}，", policy.SuitReduce ? "是" : "否");
            policy.LastModifyTime = DateTime.Now;
            bool falg = false;
            try
            {
                var repository = Factory.CreatePolicyRepository();
                var oldpolicy = repository.QueryNormalPolicy(policy.Id);
                repository.UpdateNormalPolicy(policy);
                if (policy.Departure.CompareTo(oldpolicy.Departure) != 0)
                {
                    var newD = policy.Departure.Split('/').ToList();
                    var oldD = oldpolicy.Departure.Split('/').ToList();
                    var addDeparture = newD.Except(oldD);
                    var delDeparture = oldD.Except(newD);
                    repository.UpdateNormalPolicyDeparture(delDeparture, addDeparture, policy.Id);
                }
                //falg = policy.Update();
                falg = true;
            }
            catch (Exception)
            {
                throw;
            }
            saveUpdateLog("普通政策 [ " + policy.VoyageType.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, policy.Id.ToString(), creator);
            return falg;
        }

        public static bool DeleteNormalPolicy(string creator, params Guid[] ids)
        {
            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.DeleteNormalPolicy(ids);
                repository.DeleteNormalPolicyDeparture(ids);
                foreach (var guid in ids)
                {
                    saveDeleteLog("普通政策", OperatorRole.User, guid.ToString(), creator);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 导入政策添加
        /// </summary>
        /// <param name="list"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static bool ReleaseNormalImportPolicies(List<NormalPolicy> policies, Guid publisherId, string creator)
        {
            if (policies == null || !policies.Any()) throw new ArgumentNullException("policies");
            if (String.IsNullOrEmpty(creator)) throw new ArgumentNullException("creator");
            if (publisherId == Guid.Empty) throw new InvalidOperationException("缺少发布者");

            var publisher = DataContext.Companies.SingleOrDefault(c => c.Id == publisherId);
            if (publisher == null) throw new InvalidOperationException("政策所属的公司不存在。");
            if (publisher.Type != CompanyType.Provider) throw new InvalidOperationException("指定的公司不允许发布普通政策。");
            if (!publisher.Enabled) throw new InvalidOperationException("指定的公司已被禁用，不允许发布普通政策。");
            if (!publisher.Audited) throw new InvalidOperationException("指定的公司未通过审核，不允许发布普通政策。");

            var setting = DataContext.SettingPolicies.SingleOrDefault(s => s.Company == publisherId);
            if (setting == null) throw new InvalidOperationException("缺少指定公司的政策设置信息。");
            var airlines = setting.Airlines.Split('/');
            var departures = setting.Departure.Split('/');
            var companyParameters = CompanyService.GetCompanyParameter(publisher.Id);
            policies.ForEach(item =>
            {
                if (!airlines.Contains(item.Airline))
                {
                    throw new InvalidOperationException(String.Format("指定公司不允许发布航空公司：\"{0}\" 的普通政策。", item.Airline));
                }
                Array.ForEach(item.Departure.Split('/'), s =>
                {
                    if (!departures.Contains(s))
                    {
                        throw new InvalidOperationException(String.Format("指定的公司不允许发布 \"{0}\" 出港的普通政策。", s));
                    }
                });
                item.IsPeer = companyParameters != null && companyParameters.AllowBrotherPurchase;
                item.IsInternal = companyParameters != null && companyParameters.CanHaveSubordinate;
                item.Id = Guid.NewGuid();
                item.Owner = publisherId;
                item.Creator = creator;
                item.CreateTime = DateTime.Now;
                item.LastModifyTime = DateTime.Now;
                item.Freezed = false;
                item.AbbreviateName = publisher.AbbreviateName;
                item.Suspended = false;
            });

            try
            {
                var repository = Factory.CreatePolicyRepository();
                //repository.InsertNormalPolicyDeparture(policies);
                repository.InsertNormalPolicy(policies);
                //cmd.CommitTransaction();
            }
            catch (Exception ex)
            {
                //cmd.RollbackTransaction();
                LogService.SaveExceptionLog(ex); throw new Exception("添加政策失败，请稍后重试！");
            }
            #region 日志
            foreach (var p in policies)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("航空公司：{0}，", p.Airline);
                str.AppendFormat("自定义编码：{0}，", p.CustomCode);
                str.AppendFormat("OFFICE号：{0}，", p.OfficeCode);
                str.AppendFormat("出发地：{0}，", p.Departure);
                if (p.VoyageType == VoyageType.TransitWay)
                    str.AppendFormat("中转地：{0}，", p.Transit);
                str.AppendFormat("到达地：{0}，", p.Arrival);
                str.AppendFormat("排除航段：{0}，", p.ExceptAirways);
                if (p.VoyageType == VoyageType.OneWay)
                {
                    str.AppendFormat("航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.OneWayOrRound || p.VoyageType == VoyageType.RoundTrip)
                {
                    str.AppendFormat("去程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("回程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.TransitWay)
                {
                    str.AppendFormat("第一程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("第二程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                str.AppendFormat("出票条件：{0}，", p.DrawerCondition);
                str.AppendFormat("政策备注：{0}，", p.Remark);
                str.AppendFormat("适用航班日期：{0}，", p.DepartureDateStart + "至" + p.DepartureDateEnd);
                str.AppendFormat("开始出票日期：{0}，", p.StartPrintDate);
                str.AppendFormat("航班排除日期：{0}，", p.DepartureDateFilter);
                str.AppendFormat("适用班期：{0}，", p.DepartureWeekFilter);
                str.AppendFormat("舱位：{0}，", p.Berths);
                str.AppendFormat("客票类型：{0}", p.TicketType.GetDescription());
                if (p.IsInternal)
                    str.AppendFormat("内部返点：{0}，", p.InternalCommission * 100);
                if (p.IsPeer)
                    str.AppendFormat("同行返点：{0}，", p.ProfessionCommission * 100);
                str.AppendFormat("下级返点：{0}，", p.SubordinateCommission * 100);
                str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", p.PrintBeforeTwoHours ? "是" : "否");
                str.AppendFormat("是否适用降舱：{0}，", p.SuitReduce ? "是" : "否");

                saveAddLog("普通政策 [ " + p.VoyageType.GetDescription() + " ] 导入: " + str.ToString(), OperatorRole.User, p.Id.ToString(), creator);
            }
            #endregion
            return true;
            // DataContext.NormalPolicies.Batch(policies, (set, p) => set.Insert(p));
            //  return true;
        }

        public static NormalPolicy GetNormalPolicy(Guid id)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryNormalPolicy(id);
        }

        public static bool UpdateNormalPolicyCommission(Guid id, decimal @internal, decimal subordinate, decimal profession, string creator)
        {
            bool falg = false;
            var policy = GetNormalPolicy(id);
            policy.InternalCommission = @internal;
            policy.SubordinateCommission = subordinate;
            policy.ProfessionCommission = profession;
            policy.LastModifyTime = DateTime.Now;
            var repository = Factory.CreatePolicyRepository();

            try
            {
                repository.UpdateNormalPolicy(policy);
                falg = true;
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex, "普通政策返佣信息");
            }
            saveUpdateLog("普通政策返佣信息 同行返点：" + profession + " 下级返点：" + subordinate + " 内部返点：" + @internal, OperatorRole.User, id.ToString(), creator);
            return falg;
        }

        #endregion

        #region Bargain Policy

        public static bool ReleaseBargainPolicies(BargainPolicyReleaseInfo info, string creator)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (String.IsNullOrEmpty(creator)) throw new ArgumentNullException("creator");
            if (info.BasicInfo == null) throw new InvalidOperationException("缺少政策基本信息。");
            if (info.Rebates.Count == 0) throw new InvalidOperationException("缺少政策返点信息。");

            if (info.BasicInfo.Owner == Guid.Empty)
                throw new InvalidOperationException("缺少所有者。");

            Company company = DataContext.Companies.SingleOrDefault(c => c.Id == info.BasicInfo.Owner);
            if (company == null)
                throw new InvalidOperationException("政策所属的公司不存在。");
            if (company.Type != CompanyType.Provider)
                throw new InvalidOperationException("指定公司不允许发布特价政策。");
            if (!company.Enabled)
                throw new InvalidOperationException("指定公司已被禁用，不允许发布特价政策。");
            if (!company.Audited)
                throw new InvalidOperationException("指定公司未通过审核，不允许发布特价政策。");

            SettingPolicy setting = DataContext.SettingPolicies.SingleOrDefault(sp => sp.Company == info.BasicInfo.Owner);
            if (setting == null)
                throw new InvalidOperationException("缺少指定公司的政策设置信息。");
            int count = DataContext.BargainPolicies.Count(bp => bp.Owner == info.BasicInfo.Owner);
            if (count + info.Rebates.Count > setting.BargainCount)
                throw new InvalidOperationException("超出了指定公司允许发布的特价政策数量。");
            if (!setting.Airlines.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Contains(info.BasicInfo.Airline))
                throw new InvalidOperationException(String.Format("指定公司不允许发布航空公司：\"{0}\" 的特价政策。", info.BasicInfo.Airline));
            if (setting.Departure != "*")
            {
                string[] settingArray = setting.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string[] policyArray = info.BasicInfo.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                Array.ForEach(policyArray, s =>
                {
                    if (!settingArray.Contains(s))
                        throw new InvalidOperationException(String.Format("指定公司不允许发布 \"{0}\" 出港的特价政策。", s));
                });
            }

            var companyParameters = CompanyService.GetCompanyParameter(company.Id);
            IEnumerable<BargainPolicy> policies = info.Rebates.Select(r => new BargainPolicy
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                LastModifyTime = DateTime.Now,
                IsInternal = companyParameters != null && companyParameters.CanHaveSubordinate,
                IsPeer = companyParameters != null && companyParameters.AllowBrotherPurchase,
                ExceptAirways = info.BasicInfo.ExceptAirways.ToUpper(),
                CustomCode = info.BasicInfo.CustomCode,
                Airline = info.BasicInfo.Airline,
                Arrival = info.BasicInfo.Arrival,
                Transit = info.BasicInfo.Transit,
                Departure = info.BasicInfo.Departure,
                AbbreviateName = company.AbbreviateName,
                //DepartureDatesFilter = info.BasicInfo.DepartureDatesFilter,
                //DepartureDatesFilterType = info.BasicInfo.DepartureDatesFilterType,
                DepartureFlightsFilter = info.BasicInfo.DepartureFlightsFilterType == LimitType.None ? "" : info.BasicInfo.DepartureFlightsFilter,
                DepartureFlightsFilterType = info.BasicInfo.DepartureFlightsFilterType,
                ReturnFlightsFilter = info.BasicInfo.ReturnFlightsFilterType == LimitType.None ? "" : info.BasicInfo.ReturnFlightsFilter,
                ReturnFlightsFilterType = info.BasicInfo.ReturnFlightsFilterType,
                OfficeCode = info.BasicInfo.OfficeCode,
                ImpowerOffice = info.BasicInfo.ImpowerOffice,
                Owner = info.BasicInfo.Owner,
                Remark = info.BasicInfo.Remark,
                BeforehandDays = r.BeforehandDays,
                TravelDays = r.TravelDays,
                VoyageType = info.BasicInfo.VoyageType,
                DrawerCondition = info.BasicInfo.DrawerCondition,
                Audited = r.AutoAudit,
                AutoAudit = r.AutoAudit,
                Berths = r.Berths,
                ChangePNR = r.ChangePNR,
                DepartureDateStart = r.DepartureDateStart,
                DepartureDateEnd = r.DepartureDateEnd,
                Freezed = false,
                DepartureDateFilter = r.DepartureDateFilter,
                DepartureWeekFilter = r.DepartureWeekFilter,
                InvalidRegulation = info.BasicInfo.InvalidRegulation,
                RefundRegulation = info.BasicInfo.RefundRegulation,
                ChangeRegulation = info.BasicInfo.ChangeRegulation,
                EndorseRegulation = info.BasicInfo.EndorseRegulation,
                InternalCommission = r.InternalCommission / 100,
                ProfessionCommission = r.ProfessionCommission / 100,
                StartPrintDate = r.StartPrintDate,
                SubordinateCommission = r.SubordinateCommission / 100,
                MultiSuitReduce = r.MultiSuitReduce,
                MostBeforehandDays = r.MostBeforehandDays,
                Suspended = false,
                TicketType = r.TicketType,
                Price = r.PriceType == PriceType.Price ? r.Price : r.Price / 100,
                PriceType = r.PriceType,
                Creator = creator,
                PrintBeforeTwoHours = r.PrintBeforeTwoHours
            }).ToList();
            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.InsertBargainPolicy(policies);
                //repository.InsertBargainPolicyDeparture(policies);
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex); throw new Exception("添加政策失败，请稍后重试！");
            }
            #region 日志
            foreach (var p in policies)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("航空公司：{0}，", p.Airline);
                str.AppendFormat("自定义编码：{0}，", p.CustomCode);
                str.AppendFormat("OFFICE号：{0}，", p.OfficeCode);
                str.AppendFormat("出发地：{0}，", p.Departure);
                if (p.VoyageType == VoyageType.TransitWay)
                    str.AppendFormat("中转地：{0}，", p.Transit);
                str.AppendFormat("到达地：{0}，", p.Arrival);
                str.AppendFormat("排除航段：{0}，", p.ExceptAirways);
                if (p.VoyageType == VoyageType.OneWay)
                {
                    str.AppendFormat("航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.OneWayOrRound || p.VoyageType == VoyageType.RoundTrip)
                {
                    str.AppendFormat("去程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("回程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.TransitWay)
                {
                    str.AppendFormat("第一程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("第二程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                str.AppendFormat("更改规定：{0}，", p.ChangeRegulation);
                str.AppendFormat("作废规定：{0}，", p.InvalidRegulation);
                str.AppendFormat("退票规定：{0}，", p.RefundRegulation);
                str.AppendFormat("签转规定：{0}，", p.EndorseRegulation);
                str.AppendFormat("出票条件：{0}，", p.DrawerCondition);
                str.AppendFormat("政策备注：{0}，", p.Remark);
                str.AppendFormat("适用航班日期：{0}，", p.DepartureDateStart + "至" + p.DepartureDateEnd);
                str.AppendFormat("开始出票日期：{0}，", p.StartPrintDate);
                str.AppendFormat("航班排除日期：{0}，", p.DepartureDateFilter);
                str.AppendFormat("适用班期：{0}，", p.DepartureWeekFilter);
                str.AppendFormat("舱位：{0}，", p.Berths);
                str.AppendFormat("客票类型：{0}", p.TicketType.GetDescription());
                str.AppendFormat("{0}，", p.PriceType == PriceType.Price ? "价格：" + p.Price : (p.PriceType == PriceType.Discount ? "折扣：" + p.Price * 100 : "按返佣"));
                if (p.IsInternal)
                    str.AppendFormat("内部返点：{0}，", p.InternalCommission * 100);
                if (p.IsPeer)
                    str.AppendFormat("同行返点：{0}，", p.ProfessionCommission * 100);
                str.AppendFormat("下级返点：{0}，", p.SubordinateCommission * 100);
                str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", p.PrintBeforeTwoHours ? "是" : "否");
                //str.AppendFormat("是否适用降舱：{0}，", p.SuitReduce ? "是" : "否");

                saveAddLog("特价政策 : [ " + p.VoyageType.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, p.Id.ToString(), creator);
            }
            #endregion
            return true;
            //DataContext.BargainPolicies.Batch(policies, (set, p) => set.Insert(p)); 
        }

        //public static bool AddBargainPolicy(BargainPolicy policy, string creator)
        //{
        //    if (policy == null) throw new ArgumentNullException("policy");
        //    StringBuilder str = new StringBuilder();
        //    str.AppendFormat("航空公司：{0}，", policy.Airline);
        //    str.AppendFormat("自定义编码：{0}，", policy.CustomCode);
        //    str.AppendFormat("OFFICE号：{0}，", policy.OfficeCode);
        //    str.AppendFormat("出发地：{0}，", policy.Departure);
        //    if (policy.VoyageType == VoyageType.TransitWay)
        //        str.AppendFormat("中转地：{0}，", policy.Transit);
        //    str.AppendFormat("到达地：{0}，", policy.Arrival);
        //    str.AppendFormat("排除航段：{0}，", policy.ExceptAirways);
        //    if (policy.VoyageType == VoyageType.OneWay)
        //    {
        //        str.AppendFormat("航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
        //    }
        //    if (policy.VoyageType == VoyageType.OneWayOrRound || policy.VoyageType == VoyageType.RoundTrip)
        //    {
        //        str.AppendFormat("去程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
        //        str.AppendFormat("回程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
        //    }
        //    if (policy.VoyageType == VoyageType.TransitWay)
        //    {
        //        str.AppendFormat("第一程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
        //        str.AppendFormat("第二程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
        //    }
        //    str.AppendFormat("更改规定：{0}，", policy.ChangeRegulation);
        //    str.AppendFormat("作废规定：{0}，", policy.InvalidRegulation);
        //    str.AppendFormat("退票规定：{0}，", policy.RefundRegulation);
        //    str.AppendFormat("签转规定：{0}，", policy.EndorseRegulation);
        //    str.AppendFormat("出票条件：{0}，", policy.DrawerCondition);
        //    str.AppendFormat("政策备注：{0}，", policy.Remark);
        //    str.AppendFormat("适用航班日期：{0}，", policy.DepartureDateStart + "至" + policy.DepartureDateEnd);
        //    str.AppendFormat("开始出票日期：{0}，", policy.StartPrintDate);
        //    str.AppendFormat("航班排除日期：{0}，", policy.DepartureDateFilter);
        //    str.AppendFormat("适用班期：{0}，", policy.DepartureWeekFilter);
        //    str.AppendFormat("舱位：{0}，", policy.Berths);
        //    str.AppendFormat("客票类型：{0}", policy.TicketType.GetDescription());
        //    str.AppendFormat("{0}，", policy.PriceType == PriceType.Price ? "价格：" + policy.Price : (policy.PriceType == PriceType.Discount ? "折扣：" + policy.Price : "按返佣"));
        //    if (policy.IsInternal)
        //        str.AppendFormat("内部返点：{0}，", policy.InternalCommission);
        //    if (policy.IsPeer)
        //        str.AppendFormat("同行返点：{0}，", policy.ProfessionCommission);
        //    str.AppendFormat("下级返点：{0}，", policy.SubordinateCommission);


        //    bool falg = false;
        //    try
        //    {
        //        falg = policy.Insert();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    saveAddLog("特价政策 : [ " + policy.VoyageType.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, policy.Id.ToString(), creator);
        //    return falg;
        //}

        public static bool UpdateBargainPolicy(BargainPolicy policy, string creator)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            StringBuilder str = new StringBuilder();
            str.AppendFormat("航空公司：{0}，", policy.Airline);
            str.AppendFormat("自定义编码：{0}，", policy.CustomCode);
            str.AppendFormat("OFFICE号：{0}，", policy.OfficeCode);
            str.AppendFormat("出发地：{0}，", policy.Departure);
            if (policy.VoyageType == VoyageType.TransitWay)
                str.AppendFormat("中转地：{0}，", policy.Transit);
            str.AppendFormat("到达地：{0}，", policy.Arrival);
            str.AppendFormat("排除航段：{0}，", policy.ExceptAirways);
            if (policy.VoyageType == VoyageType.OneWay)
            {
                str.AppendFormat("航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            if (policy.VoyageType == VoyageType.OneWayOrRound || policy.VoyageType == VoyageType.RoundTrip)
            {
                str.AppendFormat("去程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
                str.AppendFormat("回程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            if (policy.VoyageType == VoyageType.TransitWay)
            {
                str.AppendFormat("第一程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
                str.AppendFormat("第二程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            str.AppendFormat("更改规定：{0}，", policy.ChangeRegulation);
            str.AppendFormat("作废规定：{0}，", policy.InvalidRegulation);
            str.AppendFormat("退票规定：{0}，", policy.RefundRegulation);
            str.AppendFormat("签转规定：{0}，", policy.EndorseRegulation);
            str.AppendFormat("出票条件：{0}，", policy.DrawerCondition);
            str.AppendFormat("政策备注：{0}，", policy.Remark);
            str.AppendFormat("适用航班日期：{0}，", policy.DepartureDateStart + "至" + policy.DepartureDateEnd);
            str.AppendFormat("开始出票日期：{0}，", policy.StartPrintDate);
            str.AppendFormat("航班排除日期：{0}，", policy.DepartureDateFilter);
            str.AppendFormat("适用班期：{0}，", policy.DepartureWeekFilter);
            str.AppendFormat("舱位：{0}，", policy.Berths);
            str.AppendFormat("客票类型：{0}", policy.TicketType.GetDescription());
            str.AppendFormat("{0}，", policy.PriceType == PriceType.Price ? "价格：" + policy.Price : (policy.PriceType == PriceType.Discount ? "折扣：" + policy.Price * 100 : "按返佣"));
            if (policy.IsInternal)
                str.AppendFormat("内部返点：{0}，", policy.InternalCommission);
            if (policy.IsPeer)
                str.AppendFormat("同行返点：{0}，", policy.ProfessionCommission);
            str.AppendFormat("下级返点：{0}，", policy.SubordinateCommission);
            str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", policy.PrintBeforeTwoHours ? "是" : "否");
            str.AppendFormat("适用多段联程：{0}，", policy.MultiSuitReduce ? "是" : "否");
            policy.LastModifyTime = DateTime.Now;
            bool falg = false;

            try
            {
                var repository = Factory.CreatePolicyRepository();
                var oldpolicy = repository.QueryBargainPolicy(policy.Id);
                repository.UpdateBargainPolicy(policy);
                if (policy.Departure != oldpolicy.Departure)
                {
                    var newD = policy.Departure.Split('/').ToList();
                    var oldD = oldpolicy.Departure.Split('/').ToList();
                    var addDeparture = newD.Except(oldD);
                    var delDeparture = oldD.Except(newD);
                    repository.UpdateBargainPolicyDeparture(delDeparture, addDeparture, policy.Id);
                }
                //falg = policy.Update();
                falg = true;
            }
            catch (Exception)
            {
                throw;
            }
            saveUpdateLog("特价政策 : [ " + policy.VoyageType.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, policy.Id.ToString(), creator);

            return falg;
        }

        public static bool DeleteBargainPolicy(string creator, params Guid[] ids)
        {
            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.DeleteBargainPolicy(ids);
                repository.DeleteBargainPolicyDeparture(ids);
                foreach (var guid in ids)
                {
                    saveDeleteLog("特价政策", OperatorRole.User, guid.ToString(), creator);
                }
                return true;
            }
            catch (Exception)
            {
                throw;

            }
        }

        public static BargainPolicy GetBargainPolicy(Guid id)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryBargainPolicy(id);

            //return DataContext.BargainPolicies.SingleOrDefault(p => p.Id == id);
        }

        public static IEnumerable<BargainPolicy> GetBargainPolicies(Expression<Func<BargainPolicy, bool>> where)
        {
            return DataContext.BargainPolicies.Where(@where);
        }

        public static bool SuspendBargainPolicy(Guid id, string creator)
        {
            saveLog(OperationType.Update, "审核特价政策", OperatorRole.User, id.ToString(), creator);
            return DataContext.BargainPolicies.Update(p => new
            {
                Suspended = true
            }, p => p.Id == id) > 0;
        }

        public static bool SuspendBargainPolicies(Expression<Func<BargainPolicy, bool>> where)
        {
            return DataContext.BargainPolicies.Update(p => new
            {
                Suspended = true
            }, @where) > 0;
        }

        public static bool UpdateBargainPolicyCommission(Guid id, decimal price, PriceType type, decimal @internal, decimal subordinate, decimal profession, string creator)
        {
            saveUpdateLog("特价政策返佣信息[" + type.GetDescription() + "] 同行返点：" + profession + " 下级返点：" + subordinate + " 内部返点：" + @internal, OperatorRole.User, id.ToString(), creator);
            return DataContext.BargainPolicies.Update(
                                                      p => new
                                                      {
                                                          Price = price,
                                                          PriceType = type,
                                                          InternalCommission = @internal,
                                                          SubordinateCommission = subordinate,
                                                          ProfessionCommission = profession,
                                                      },
                                                      p => p.Id == id) > 0;
        }

        #endregion

        #region Special Policy

        public static bool ReleaseSpecialPolicies(SpecialPolicyReleaseInfo info, string creator)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (String.IsNullOrEmpty(creator)) throw new ArgumentNullException("creator");
            if (info.BasicInfo == null) throw new InvalidOperationException("缺少政策基本信息。");
            if (info.Rebates.Count == 0) throw new InvalidOperationException("缺少政策返点信息。");

            if (info.BasicInfo.Owner == Guid.Empty)
                throw new InvalidOperationException("缺少所有者。");

            var company = CompanyService.GetCompanyInfo(info.BasicInfo.Owner);
            if (company == null)
                throw new InvalidOperationException("政策所属的公司不存在。");
            if (!(company.Type == CompanyType.Provider || company.Type == CompanyType.Supplier))
                throw new InvalidOperationException("指定公司不允许发布特殊产品。");
            if (!company.Enabled)
                throw new InvalidOperationException("指定公司已被禁用，不允许发布特殊产品。");
            if (!company.Audited)
                throw new InvalidOperationException("指定公司未通过审核，不允许发布特殊产品。");

            var companyParameters = Service.Organization.CompanyService.GetCompanyParameter(company.Id);

            if (info.BasicInfo.Type == SpecialProductType.Singleness && !companyParameters.Singleness)
                throw new InvalidOperationException("指定公司不允许发布单程控位特殊产品。");
            if (info.BasicInfo.Type == SpecialProductType.Bloc && !companyParameters.Bloc)
                throw new InvalidOperationException("指定公司不允许发布集团票特殊产品。");
            if (info.BasicInfo.Type == SpecialProductType.Business && !companyParameters.Business)
                throw new InvalidOperationException("指定公司不允许发布商旅卡特殊产品。");
            if (info.BasicInfo.Type == SpecialProductType.CostFree && !companyParameters.CostFree)
                throw new InvalidOperationException("指定公司不允许发布免票特殊产品。");
            if (info.BasicInfo.Type == SpecialProductType.Disperse && !companyParameters.Disperse)
                throw new InvalidOperationException("指定公司不允许发布散冲团特殊产品。");
            if (info.BasicInfo.Type == SpecialProductType.OtherSpecial && !companyParameters.OtherSpecial)
                throw new InvalidOperationException("指定公司不允许发布其他特殊产品。");
            if (info.BasicInfo.Type == SpecialProductType.LowToHigh && !companyParameters.LowToHigh)
                throw new InvalidOperationException("指定公司不允许发布低打高返特殊产品。");

            SettingPolicy setting = DataContext.SettingPolicies.SingleOrDefault(sp => sp.Company == info.BasicInfo.Owner);
            if (setting == null)
                throw new InvalidOperationException("缺少指定公司的政策设置信息。");
            int count = DataContext.SpecialPolicies.Count(sp => sp.Owner == info.BasicInfo.Owner && sp.Type == info.BasicInfo.Type);

            if (info.BasicInfo.Type == SpecialProductType.CostFree && (count + info.Rebates.Count > setting.CostFreeCount))
                throw new InvalidOperationException("超出了指定公司允许发布的免票特殊产品数量。");
            if (info.BasicInfo.Type == SpecialProductType.Bloc && (count + info.Rebates.Count > setting.BlocCount))
                throw new InvalidOperationException("超出了指定公司允许发布的集团特殊产品数量。");
            if (info.BasicInfo.Type == SpecialProductType.Business && (count + info.Rebates.Count > setting.BusinessCount))
                throw new InvalidOperationException("超出了指定公司允许发布的商旅卡特殊产品数量。");
            if (info.BasicInfo.Type == SpecialProductType.Disperse && (count + info.Rebates.Count > setting.DisperseCount))
                throw new InvalidOperationException("超出了指定公司允许发布的散冲团特殊产品数量。");
            if (info.BasicInfo.Type == SpecialProductType.Singleness && (count + info.Rebates.Count > setting.SinglenessCount))
                throw new InvalidOperationException("超出了指定公司允许发布的单程控位特殊产品数量。");
            if (info.BasicInfo.Type == SpecialProductType.OtherSpecial && (count + info.Rebates.Count > setting.OtherSpecialCount))
                throw new InvalidOperationException("超出了指定公司允许发布的其他特殊产品数量。");
            if (info.BasicInfo.Type == SpecialProductType.LowToHigh && (count + info.Rebates.Count > setting.LowToHighCount))
                throw new InvalidOperationException("超出了指定公司允许发布的低打高返特殊产品数量。");

            //if (count + info.Rebates.Count > setting.SpecialCount)
            //    throw new InvalidOperationException("超出了指定公司允许发布的特殊产品数量。");
            if (!setting.Airlines.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Contains(info.BasicInfo.Airline))
                throw new InvalidOperationException(String.Format("指定公司不允许发布航空公司：\"{0}\" 的特殊产品。", info.BasicInfo.Airline));
            if (setting.Departure != "*")
            {
                string[] settingArray = setting.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string[] policyArray = info.BasicInfo.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                Array.ForEach(policyArray, s =>
                {
                    if (!settingArray.Contains(s))
                        throw new InvalidOperationException(String.Format("指定公司不允许发布 \"{0}\" 出港的特殊产品。", s));
                });
            }
            IEnumerable<SpecialPolicy> policies = info.Rebates.Select(r => new SpecialPolicy
            {
                Id = Guid.NewGuid(),
                AbbreviateName = company.AbbreviateName,
                CreateTime = DateTime.Now,
                LastModifyTime = DateTime.Now,
                OfficeCode = info.BasicInfo.OfficeCode,
                CustomCode = info.BasicInfo.CustomCode,
                ExceptAirways = info.BasicInfo.ExceptAirways.ToUpper(),
                Airline = info.BasicInfo.Airline,
                Arrival = info.BasicInfo.Arrival,
                Departure = info.BasicInfo.Departure,
                //DepartureDatesFilter = info.BasicInfo.DepartureDatesFilter,
                //DepartureDatesFilterType = info.BasicInfo.DepartureDatesFilterType,
                ImpowerOffice = info.BasicInfo.ImpowerOffice,
                DepartureFlightsFilter = info.BasicInfo.DepartureFlightsFilterType == LimitType.None ? "" : info.BasicInfo.DepartureFlightsFilter,
                DepartureFlightsFilterType = info.BasicInfo.DepartureFlightsFilterType,
                Owner = info.BasicInfo.Owner,
                Remark = info.BasicInfo.Remark,
                BeforehandDays = r.BeforehandDays,
                VoyageType = info.BasicInfo.VoyageType,
                DrawerCondition = info.BasicInfo.DrawerCondition,
                Audited = r.AutoAudit,
                Berths = r.Berths.ToUpper(),
                IsBargainBerths = r.IsBargainBerths,
                AutoAudit = r.AutoAudit,
                DepartureDateStart = r.DepartureDateStart,
                DepartureDateEnd = r.DepartureDateEnd,
                Freezed = false,
                Suspended = false,
                SynBlackScreen = r.SynBlackScreen,
                DepartureDateFilter = r.DepartureDateFilter,
                DepartureWeekFilter = r.DepartureWeekFilter,
                Price = r.PriceType == PriceType.Price ? r.Price : r.Price / 100,
                ProvideDate = r.ProvideDate,
                Creator = creator,
                InvalidRegulation = info.BasicInfo.InvalidRegulation,
                RefundRegulation = info.BasicInfo.RefundRegulation,
                ChangeRegulation = info.BasicInfo.ChangeRegulation,
                EndorseRegulation = info.BasicInfo.EndorseRegulation,
                ResourceAmount = r.ResourceAmount,
                InternalCommission = r.PriceType == PriceType.Price ? r.InternalCommission : r.InternalCommission / 100,
                SubordinateCommission = r.PriceType == PriceType.Price ? (company.Type == CompanyType.Supplier ? r.ProfessionCommission : r.SubordinateCommission) : (company.Type == CompanyType.Supplier ? r.ProfessionCommission / 100 : r.SubordinateCommission / 100),
                ProfessionCommission = r.PriceType == PriceType.Price ? r.ProfessionCommission : r.ProfessionCommission / 100,
                IsSeat = r.IsSeat,
                PriceType = r.PriceType,
                Type = info.BasicInfo.Type,
                TicketType = r.TicketType,
                IsInternal = companyParameters != null && companyParameters.CanHaveSubordinate,
                IsPeer = company.Type == CompanyType.Supplier || companyParameters != null && companyParameters.AllowBrotherPurchase,
                PlatformAudited = companyParameters != null && companyParameters.AutoPlatformAudit,
                ConfirmResource = r.ConfirmResource,
                PrintBeforeTwoHours = r.PrintBeforeTwoHours,
                LowNoMaxPrice = r.LowNoMaxPrice,
                LowNoMinPrice = r.LowNoMinPrice,
                LowNoType = r.LowNoType
            }).ToList();


            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.InsertSpecialPolicy(policies);
                //repository.InsertSpecialPolicyDeparture(policies);

            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex); throw new Exception("添加政策失败，请稍后重试！");
            }
            #region 日志
            foreach (var p in policies)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("航空公司：{0}，", p.Airline);
                str.AppendFormat("自定义编码：{0}，", p.CustomCode);
                str.AppendFormat("OFFICE号：{0}，", p.OfficeCode);
                str.AppendFormat("出发地：{0}，", p.Departure);
                str.AppendFormat("到达地：{0}，", p.Arrival);
                str.AppendFormat("排除航段：{0}，", p.ExceptAirways);

                str.AppendFormat("航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));

                str.AppendFormat("更改规定：{0}，", p.ChangeRegulation);
                str.AppendFormat("作废规定：{0}，", p.InvalidRegulation);
                str.AppendFormat("退票规定：{0}，", p.RefundRegulation);
                str.AppendFormat("签转规定：{0}，", p.EndorseRegulation);
                str.AppendFormat("出票条件：{0}，", p.DrawerCondition);
                str.AppendFormat("政策备注：{0}，", p.Remark);
                str.AppendFormat("适用航班日期：{0}，", p.DepartureDateStart + "至" + p.DepartureDateEnd);
                str.AppendFormat("开始出票日期：{0}，", p.ProvideDate);
                str.AppendFormat("航班排除日期：{0}，", p.DepartureDateFilter);
                str.AppendFormat("适用班期：{0}，", p.DepartureWeekFilter);
                str.AppendFormat("舱位：{0}，", p.Berths);
                str.AppendFormat("客票类型：{0}", p.TicketType.GetDescription());
                str.AppendFormat("{0}，", p.PriceType == PriceType.Price ? "价格：" + p.Price : (p.PriceType == PriceType.Subtracting ? "直减：" + p.Price : ""));
                if (p.IsInternal)
                    str.AppendFormat("内部返点：{0}，", p.InternalCommission);
                if (p.IsPeer)
                    str.AppendFormat("同行返点：{0}，", p.ProfessionCommission);
                str.AppendFormat("下级返点：{0}，", p.SubordinateCommission);
                str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", p.PrintBeforeTwoHours ? "是" : "否");
                str.AppendFormat("选择低价类型：{0}，", p.LowNoType.GetDescription());
                str.AppendFormat("直减后的票面价格区间：{0}，", p.LowNoMinPrice + "至" + p.LowNoMaxPrice);
                saveAddLog("特殊政策 : [ " + p.Type.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, p.Id.ToString(), creator);

            }
            #endregion
            return true;
            //DataContext.SpecialPolicies.Batch(policies, (set, p) => set.Insert(p));

        }

        //public static bool AddSpecialPolicy(SpecialPolicy policy)
        //{
        //    if (policy == null) throw new ArgumentNullException("policy");

        //    return policy.Insert();
        //}

        public static bool UpdateSpecialPolicy(SpecialPolicy policy, string creator)
        {
            if (policy == null) throw new ArgumentNullException("policy");

            StringBuilder str = new StringBuilder();
            str.AppendFormat("航空公司：{0}，", policy.Airline);
            str.AppendFormat("自定义编码：{0}，", policy.CustomCode);
            str.AppendFormat("OFFICE号：{0}，", policy.OfficeCode);
            str.AppendFormat("出发地：{0}，", policy.Departure);
            str.AppendFormat("到达地：{0}，", policy.Arrival);
            str.AppendFormat("排除航段：{0}，", policy.ExceptAirways);

            str.AppendFormat("航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));

            str.AppendFormat("更改规定：{0}，", policy.ChangeRegulation);
            str.AppendFormat("作废规定：{0}，", policy.InvalidRegulation);
            str.AppendFormat("退票规定：{0}，", policy.RefundRegulation);
            str.AppendFormat("签转规定：{0}，", policy.EndorseRegulation);
            str.AppendFormat("出票条件：{0}，", policy.DrawerCondition);
            str.AppendFormat("政策备注：{0}，", policy.Remark);
            str.AppendFormat("适用航班日期：{0}，", policy.DepartureDateStart + "至" + policy.DepartureDateEnd);
            str.AppendFormat("开始出票日期：{0}，", policy.ProvideDate);
            str.AppendFormat("航班排除日期：{0}，", policy.DepartureDateFilter);
            str.AppendFormat("适用班期：{0}，", policy.DepartureWeekFilter);
            str.AppendFormat("舱位：{0}，", policy.Berths);
            str.AppendFormat("客票类型：{0}", policy.TicketType.GetDescription());
            str.AppendFormat("{0}，", policy.PriceType == PriceType.Price ? "价格：" + policy.Price : (policy.PriceType == PriceType.Subtracting ? "直减：" + policy.Price * 100 : ""));
            if (policy.IsInternal)
                str.AppendFormat("内部返点：{0}，", policy.PriceType == PriceType.Price ? policy.InternalCommission : policy.InternalCommission * 100);
            if (policy.IsPeer)
                str.AppendFormat("同行返点：{0}，", policy.PriceType == PriceType.Price ? policy.ProfessionCommission : policy.ProfessionCommission * 100);
            str.AppendFormat("下级返点：{0}，", policy.PriceType == PriceType.Price ? policy.SubordinateCommission : policy.SubordinateCommission * 100);
            str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", policy.PrintBeforeTwoHours ? "是" : "否");
            str.AppendFormat("选择低价类型：{0}，", policy.LowNoType.GetDescription());
            str.AppendFormat("直减后的票面价格区间：{0}，", policy.LowNoMinPrice + "至" + policy.LowNoMaxPrice);
            policy.LastModifyTime = DateTime.Now;
            bool flag = false;
            try
            {
                var repository = Factory.CreatePolicyRepository();
                var oldpolicy = repository.QuerySpecialPolicy(policy.Id);
                repository.UpdateSpecialPolicy(policy);
                if (policy.Departure != oldpolicy.Departure)
                {
                    var newD = policy.Departure.Split('/').ToList();
                    var oldD = oldpolicy.Departure.Split('/').ToList();
                    var addDeparture = newD.Except(oldD);
                    var delDeparture = oldD.Except(newD);
                    repository.UpdateSpecialPolicyDeparture(delDeparture, addDeparture, policy.Id);
                }
                //falg = policy.Update();
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            saveUpdateLog("特殊政策 : [ " + policy.Type.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, policy.Id.ToString(), creator);
            return flag;
        }
        /// <summary>
        /// 更新特殊票资源
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool UpdateSpecialPolicy(Guid id, int num)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.UpdateSpecialPolicy(id, num) > 0;

        }

        public static bool DeleteSpecialPolicy(string creator, params Guid[] ids)
        {
            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.DeleteSpecialPolicy(ids);
                repository.DeleteSpecialPolicyDeparture(ids);
                foreach (var guid in ids)
                {
                    saveDeleteLog("特殊政策", OperatorRole.User, guid.ToString(), creator);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static SpecialPolicy GetSpecialPolicy(Guid id)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QuerySpecialPolicy(id);

            //return DataContext.SpecialPolicies.SingleOrDefault(p => p.Id == id);
        }

        public static IEnumerable<SpecialPolicy> GetSpecialPolicies(Expression<Func<SpecialPolicy, bool>> where)
        {
            return DataContext.SpecialPolicies.Where(@where);
        }

        public static bool SuspendSpecialPolicy(Guid id)
        {
            return DataContext.SpecialPolicies.Update(p => new
            {
                Suspended = true
            }, p => p.Id == id) > 0;
        }

        public static bool SuspendSpecialPolicies(Expression<Func<SpecialPolicy, bool>> where)
        {
            return DataContext.SpecialPolicies.Update(p => new
            {
                Suspended = true
            }, @where) > 0;
        }

        public static bool UpdateSpecialPolicyPrice(Guid id, PriceType priceType, decimal price, decimal subordinateCommission, decimal internalCommission, decimal proffessionCommission, string creator)
        {
            saveUpdateLog("特殊政策价格 [" + priceType.GetDescription() + "] 同行返点：" + proffessionCommission + " 下级返点：" + subordinateCommission + " 内部返点：" + internalCommission, OperatorRole.User, id.ToString(), creator);
            return DataContext.SpecialPolicies.Update(p => new
            {
                Price = price,
                PriceType = priceType,
                SubordinateCommission = subordinateCommission,
                InternalCommission = internalCommission,
                ProfessionCommission = proffessionCommission
            }, p => p.Id == id) > 0;
        }

        /// <summary>
        /// 平台审核特殊政策
        /// </summary>
        /// <param name="creator"> </param>
        /// <param name="ids">政策 Id 列表</param>
        /// <returns>返回审核操作是否成功</returns>
        public static bool AuditSpecialPolicy(string creator, params Guid[] ids)
        {
            foreach (var guid in ids)
            {
                saveLog(OperationType.Update, "平台审核特殊政策", OperatorRole.Platform, guid.ToString(), creator);
            }
            return DataContext.SpecialPolicies.Update(
                                                      p => new
                                                      {
                                                          PlatformAudited = true
                                                      },
                                                      p => ids.Contains(p.Id)) > 0;
        }

        /// <summary>
        /// 平台取消审核特殊政策
        /// </summary>
        /// <param name="creator"> </param>
        /// <param name="ids">政策 Id 列表</param>
        /// <returns>返回取消审核操作是否成功</returns>
        public static bool CancelAuditSpecialPolicy(string creator, params Guid[] ids)
        {
            foreach (var guid in ids)
            {
                saveLog(OperationType.Update, "平台取消审核特殊政策", OperatorRole.Platform, guid.ToString(), creator);
            }
            return DataContext.SpecialPolicies.Update(
                                                      p => new
                                                      {
                                                          PlatformAudited = false
                                                      },
                                                      p => ids.Contains(p.Id)) > 0;
        }

        #endregion

        #region Team Policy

        public static bool ReleaseTeamPolicies(TeamPolicyReleaseInfo info, string creator)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (String.IsNullOrEmpty(creator)) throw new ArgumentNullException("creator");
            if (info.BasicInfo == null) throw new InvalidOperationException("缺少政策基本信息。");
            if (info.Rebates.Count == 0) throw new InvalidOperationException("缺少政策返点信息。");

            if (info.BasicInfo.Owner == Guid.Empty)
                throw new InvalidOperationException("缺少所有者。");

            Company company = DataContext.Companies.SingleOrDefault(c => c.Id == info.BasicInfo.Owner);
            if (company == null)
                throw new InvalidOperationException("政策所属的公司不存在。");
            if (company.Type != CompanyType.Provider)
                throw new InvalidOperationException("指定的公司不允许发布团队政策。");
            if (!company.Enabled)
                throw new InvalidOperationException("指定的公司已被禁用，不允许发布团队政策。");
            if (!company.Audited)
                throw new InvalidOperationException("指定的公司未通过审核，不允许发布团队政策。");

            SettingPolicy setting = DataContext.SettingPolicies.SingleOrDefault(s => s.Company == info.BasicInfo.Owner);
            if (setting == null)
                throw new InvalidOperationException("缺少指定公司的政策设置信息。");
            if (!setting.Airlines.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Contains(info.BasicInfo.Airline))
                throw new InvalidOperationException(String.Format("指定公司不允许发布航空公司：\"{0}\" 的团队政策。", info.BasicInfo.Airline));
            if (setting.Departure != "*")
            {
                string[] settingArray = setting.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string[] policyArray = info.BasicInfo.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                Array.ForEach(policyArray, s =>
                {
                    if (!settingArray.Contains(s))
                        throw new InvalidOperationException(String.Format("指定的公司不允许发布 \"{0}\" 出港的普通政策。", s));
                });
            }
            var companyParameters = CompanyService.GetCompanyParameter(company.Id);
            IEnumerable<TeamPolicy> policies = info.Rebates.Select(r => new TeamPolicy
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                LastModifyTime = DateTime.Now,
                Airline = info.BasicInfo.Airline,
                CustomCode = info.BasicInfo.CustomCode,
                AbbreviateName = company.AbbreviateName,
                IsInternal = companyParameters != null && companyParameters.CanHaveSubordinate,
                IsPeer = companyParameters != null && companyParameters.AllowBrotherPurchase,
                Transit = info.BasicInfo.Transit,
                Arrival = info.BasicInfo.Arrival,
                Departure = info.BasicInfo.Departure,
                DepartureFlightsFilter = info.BasicInfo.DepartureFlightsFilterType == LimitType.None ? "" : info.BasicInfo.DepartureFlightsFilter,
                DepartureFlightsFilterType = info.BasicInfo.DepartureFlightsFilterType,
                ExceptAirways = info.BasicInfo.ExceptAirways.ToUpper(),
                OfficeCode = info.BasicInfo.OfficeCode,
                ImpowerOffice = info.BasicInfo.ImpowerOffice,
                Owner = info.BasicInfo.Owner,
                Remark = info.BasicInfo.Remark,
                ReturnFlightsFilter = info.BasicInfo.ReturnFlightsFilterType == LimitType.None ? "" : info.BasicInfo.ReturnFlightsFilter,
                ReturnFlightsFilterType = info.BasicInfo.ReturnFlightsFilterType,
                VoyageType = info.BasicInfo.VoyageType,
                DrawerCondition = info.BasicInfo.DrawerCondition,
                Audited = r.AutoAudit,
                AutoAudit = r.AutoAudit,
                AutoPrint = r.AutoPrint,
                AppointBerths = r.AppointBerths,
                Berths = r.Berths,
                ChangePNR = r.ChangePNR,
                Freezed = false,
                DepartureDateFilter = r.DepartureDateFilter,
                DepartureWeekFilter = r.DepartureWeekFilter,
                InternalCommission = r.InternalCommission / 100,
                ProfessionCommission = r.ProfessionCommission / 100,
                DepartureDateStart = r.DepartureDateStart,
                DepartureDateEnd = r.DepartureDateEnd,
                StartPrintDate = r.StartPrintDate,
                SubordinateCommission = r.SubordinateCommission / 100,
                SuitReduce = r.SuitReduce,
                MultiSuitReduce = r.MultiSuitReduce,
                Suspended = false,
                TicketType = r.TicketType,
                Creator = creator,
                PrintBeforeTwoHours = r.PrintBeforeTwoHours
            }).ToList();
            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.InsertTeamPolicy(policies);
                //repository.InsertTeamPolicyDeparture(policies);
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw new Exception("添加政策失败，请稍后重试！");
            }
            #region 日志
            foreach (var p in policies)
            {
                StringBuilder str = new StringBuilder();
                str.AppendFormat("航空公司：{0}，", p.Airline);
                str.AppendFormat("自定义编码：{0}，", p.CustomCode);
                str.AppendFormat("OFFICE号：{0}，", p.OfficeCode);
                str.AppendFormat("出发地：{0}，", p.Departure);
                if (p.VoyageType == VoyageType.TransitWay)
                    str.AppendFormat("中转地：{0}，", p.Transit);
                str.AppendFormat("到达地：{0}，", p.Arrival);
                str.AppendFormat("排除航段：{0}，", p.ExceptAirways);
                if (p.VoyageType == VoyageType.OneWay)
                {
                    str.AppendFormat("航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.RoundTrip)
                {
                    str.AppendFormat("去程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("回程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                if (p.VoyageType == VoyageType.TransitWay)
                {
                    str.AppendFormat("第一程航班限制：{0}，", p.DepartureFlightsFilterType == LimitType.None ? "不限" : (p.DepartureFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                    str.AppendFormat("第二程航班限制：{0}，", p.ReturnFlightsFilterType == LimitType.None ? "不限" : (p.ReturnFlightsFilterType == LimitType.Include ? "包含" + p.DepartureFlightsFilter : "不包含" + p.DepartureFlightsFilter));
                }
                str.AppendFormat("出票条件：{0}，", p.DrawerCondition);
                str.AppendFormat("政策备注：{0}，", p.Remark);
                str.AppendFormat("适用航班日期：{0}，", p.DepartureDateStart + "至" + p.DepartureDateEnd);
                str.AppendFormat("开始出票日期：{0}，", p.StartPrintDate);
                str.AppendFormat("航班排除日期：{0}，", p.DepartureDateFilter);
                str.AppendFormat("适用班期：{0}，", p.DepartureWeekFilter);
                str.AppendFormat("舱位：{0}，", p.Berths);
                str.AppendFormat("客票类型：{0}", p.TicketType.GetDescription());
                if (p.IsInternal)
                    str.AppendFormat("内部返点：{0}，", p.InternalCommission);
                if (p.IsPeer)
                    str.AppendFormat("同行返点：{0}，", p.ProfessionCommission);
                str.AppendFormat("下级返点：{0}，", p.SubordinateCommission);
                str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", p.PrintBeforeTwoHours ? "是" : "否");
                str.AppendFormat("适用多段联程：{0}，", p.SuitReduce ? "是" : "否");


                saveAddLog("团队政策 [ " + p.VoyageType.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, p.Id.ToString(), creator);
            }
            #endregion
            return true;
            //DataContext.TeamPolicies.Batch(policies, (set, p) => set.Insert(p));

            //return true;
        }

        public static bool UpdateTeamPolicy(TeamPolicy policy, string creator)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            StringBuilder str = new StringBuilder();
            str.AppendFormat("航空公司：{0}，", policy.Airline);
            str.AppendFormat("自定义编码：{0}，", policy.CustomCode);
            str.AppendFormat("OFFICE号：{0}，", policy.OfficeCode);
            str.AppendFormat("出发地：{0}，", policy.Departure);
            if (policy.VoyageType == VoyageType.TransitWay)
                str.AppendFormat("中转地：{0}，", policy.Transit);
            str.AppendFormat("到达地：{0}，", policy.Arrival);
            str.AppendFormat("排除航段：{0}，", policy.ExceptAirways);
            if (policy.VoyageType == VoyageType.OneWay)
            {
                str.AppendFormat("航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            if (policy.VoyageType == VoyageType.RoundTrip)
            {
                str.AppendFormat("去程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
                str.AppendFormat("回程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            if (policy.VoyageType == VoyageType.TransitWay)
            {
                str.AppendFormat("第一程航班限制：{0}，", policy.DepartureFlightsFilterType == LimitType.None ? "不限" : (policy.DepartureFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
                str.AppendFormat("第二程航班限制：{0}，", policy.ReturnFlightsFilterType == LimitType.None ? "不限" : (policy.ReturnFlightsFilterType == LimitType.Include ? "包含" + policy.DepartureFlightsFilter : "不包含" + policy.DepartureFlightsFilter));
            }
            str.AppendFormat("出票条件：{0}，", policy.DrawerCondition);
            str.AppendFormat("政策备注：{0}，", policy.Remark);
            str.AppendFormat("适用航班日期：{0}，", policy.DepartureDateStart + "至" + policy.DepartureDateEnd);
            str.AppendFormat("开始出票日期：{0}，", policy.StartPrintDate);
            str.AppendFormat("航班排除日期：{0}，", policy.DepartureDateFilter);
            str.AppendFormat("适用班期：{0}，", policy.DepartureWeekFilter);
            str.AppendFormat("舱位：{0}，", policy.Berths);
            str.AppendFormat("客票类型：{0}", policy.TicketType.GetDescription());
            if (policy.IsInternal)
                str.AppendFormat("内部返点：{0}，", policy.InternalCommission * 100);
            if (policy.IsPeer)
                str.AppendFormat("同行返点：{0}，", policy.ProfessionCommission * 100);
            str.AppendFormat("下级返点：{0}，", policy.SubordinateCommission * 100);
            str.AppendFormat("是否在起飞前两小时可以用B2B出票：{0}，", policy.PrintBeforeTwoHours ? "是" : "否");
            str.AppendFormat("适用多段联程：{0}，", policy.SuitReduce ? "是" : "否");
            policy.LastModifyTime = DateTime.Now;
            bool flag = false;
            try
            {
                var repository = Factory.CreatePolicyRepository();
                var oldpolicy = repository.QueryTeamPolicy(policy.Id);
                repository.UpdateTeamPolicy(policy);
                if (policy.Departure != oldpolicy.Departure)
                {
                    var newD = policy.Departure.Split('/').ToList();
                    var oldD = oldpolicy.Departure.Split('/').ToList();
                    var addDeparture = newD.Except(oldD);
                    var delDeparture = oldD.Except(newD);
                    repository.UpdateTeamPolicyDeparture(delDeparture, addDeparture, policy.Id);
                }
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            saveUpdateLog("团队政策 [ " + policy.VoyageType.GetDescription() + " ] : " + str.ToString(), OperatorRole.User, policy.Id.ToString(), creator);
            return flag;
        }

        public static bool DeleteTeamPolicy(string creator, params Guid[] ids)
        {
            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.DeleteTeamPolicy(ids);
                repository.DeleteTeamPolicyDeparture(ids);
                foreach (var guid in ids)
                {
                    saveDeleteLog("团队政策", OperatorRole.User, guid.ToString(), creator);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static TeamPolicy GetTeamPolicy(Guid id)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryTeamPolicy(id);

            //return DataContext.TeamPolicies.SingleOrDefault(p => p.Id == id);
        }

        public static bool UpdateTeamPolicyCommission(Guid id, decimal @internal, decimal subordinate, decimal profession, string creator)
        {
            saveUpdateLog("团队政策返佣信息 同行返点：" + profession + " 下级返点：" + subordinate + " 内部返点：" + @internal, OperatorRole.User, id.ToString(), creator);
            return DataContext.TeamPolicies.Update(
                                                     p => new
                                                     {
                                                         InternalCommission = @internal,
                                                         SubordinateCommission = subordinate,
                                                         ProfessionCommission = profession
                                                     }
                                                     , p => p.Id == id) > 0;
        }

        #endregion

        #region Notch Policy

        public static bool ReleaseNotchPolicies(NotchPolicyReleaseInfo info, string creator)
        {
            if (info == null) throw new ArgumentNullException("info");
            if (String.IsNullOrEmpty(creator)) throw new ArgumentNullException("creator");

            if (info.Owner == Guid.Empty)
                throw new InvalidOperationException("缺少所有者。");

            Company company = DataContext.Companies.SingleOrDefault(c => c.Id == info.Owner);
            if (company == null)
                throw new InvalidOperationException("政策所属的公司不存在。");
            if (company.Type != CompanyType.Provider)
                throw new InvalidOperationException("指定的公司不允许发布缺口政策。");
            if (!company.Enabled)
                throw new InvalidOperationException("指定的公司已被禁用，不允许发布缺口政策。");
            if (!company.Audited)
                throw new InvalidOperationException("指定的公司未通过审核，不允许发布缺口政策。");

            SettingPolicy setting = DataContext.SettingPolicies.SingleOrDefault(s => s.Company == info.Owner);
            if (setting == null)
                throw new InvalidOperationException("缺少指定公司的政策设置信息。");
            if (!setting.Airlines.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Contains(info.Airline))
                throw new InvalidOperationException(String.Format("指定公司不允许发布航空公司：\"{0}\" 的缺口政策。", info.Airline));
            //if (setting.Departure != "*")
            //{
            //    string[] settingArray = setting.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            //    string[] policyArray = info.Departure.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            //    Array.ForEach(policyArray, s =>
            //    {
            //        if (!settingArray.Contains(s))
            //            throw new InvalidOperationException(String.Format("指定的公司不允许发布 \"{0}\" 出港的缺口政策。", s));
            //    });
            //}
            var companyParameters = CompanyService.GetCompanyParameter(company.Id);
            IEnumerable<NotchPolicy> policies = info.RebateInfo.Select(r => new NotchPolicy
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.Now,
                LastModifyTime = DateTime.Now,
                Airline = info.Airline,
                CustomCode = info.CustomCode,
                AbbreviateName = company.AbbreviateName,
                IsInternal = companyParameters != null && companyParameters.CanHaveSubordinate,
                IsPeer = companyParameters != null && companyParameters.AllowBrotherPurchase,
                DepartureArrival = info.DepartureArrival.Any() ? info.DepartureArrival.Select(a => new ChinaPay.B3B.Data.DataMapping.NotchPolicyDepartureArrival { Id = Guid.NewGuid(), Arrival = a.Arrival, Departure = a.Departure, IsAllowable = a.IsAllowable }).ToList() : new List<ChinaPay.B3B.Data.DataMapping.NotchPolicyDepartureArrival>() { new ChinaPay.B3B.Data.DataMapping.NotchPolicyDepartureArrival() { Id = Guid.NewGuid(), Arrival = setting.Departure, Departure = setting.Departure, IsAllowable = true } },
                OfficeCode = info.OfficeCode,
                ImpowerOffice = info.ImpowerOffice,
                Owner = info.Owner,
                Remark = info.Remark,
                VoyageType = VoyageType.Notch,
                DrawerCondition = info.DrawerCondition,
                Audited = r.AutoAudit,
                AutoAudit = r.AutoAudit,
                Berths = r.Berths,
                ChangePNR = r.ChangePNR,
                Freezed = false,
                DepartureDateFilter = r.DepartureDateFilter,
                DepartureWeekFilter = r.DepartureWeekFilter,
                InternalCommission = r.InternalCommission / 100,
                ProfessionCommission = r.ProfessionCommission / 100,
                DepartureDateStart = r.DepartureDateStart,
                DepartureDateEnd = r.DepartureDateEnd,
                StartPrintDate = r.StartPrintDate,
                SubordinateCommission = r.SubordinateCommission / 100,
                Suspended = false,
                TicketType = r.TicketType,
                Creator = creator,
                PrintBeforeTwoHours = r.PrintBeforeTwoHours,
                AuditTime = r.AutoAudit ? DateTime.Now : (DateTime?)null,
                DepartureFlightsFilter = info.DepartureFlightsFilter,
                DepartureFlightsFilterType = info.DepartureFlightsFilterType
            }).ToList();
            try
            {
                var repository = Factory.CreatePolicyRepository();
                repository.InsertNotchPolicy(policies);
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                throw new Exception("添加政策失败，请稍后重试！");
            }
            foreach (var item in policies)
            {
                saveAddLog("缺口政策 [ " + item.VoyageType.GetDescription() + " ] : " + item.ToString(), OperatorRole.User, item.Id.ToString(), creator);
            }
            return true;
        }

        public static bool UpdateNotchPolicy(NotchPolicy policy, string creator)
        {
            if (policy == null) throw new ArgumentNullException("policy");

            policy.LastModifyTime = DateTime.Now;
            bool flag = false;
            try
            {
                //仓储层加了事务
                var repository = Factory.CreatePolicyRepository();
                repository.UpdateNotchPolicy(policy);
            }
            catch (Exception)
            {
                throw;
            }
            saveUpdateLog("缺口政策 [ " + policy.VoyageType.GetDescription() + " ] : " + policy.ToString(), OperatorRole.User, policy.Id.ToString(), creator);
            return flag;
        }

        public static bool DeleteNotchPolicy(string creator, params Guid[] ids)
        {
            try
            {
                var repository = Factory.CreatePolicyRepository();
                using (var tran = new TransactionScope())
                {
                    repository.DeleteNotchPolicy(ids);
                    tran.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
            foreach (var guid in ids)
            {
                saveDeleteLog("缺口政策", OperatorRole.User, guid.ToString(), creator);
            }
            return true;
        }


        public static NotchPolicy GetNotchPolicy(Guid id)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.QueryNotchPolicy(id);

            //return DataContext.NotchPolicies.SingleOrDefault(p => p.Id == id);
        }

        public static bool UpdateNotchPolicyCommission(Guid id, decimal @internal, decimal subordinate, decimal profession, string creator)
        {
            var repository = Factory.CreatePolicyRepository();
            var f = repository.UpdateNotchPolicyCommission(id, @internal, subordinate, profession);
            saveUpdateLog("缺口政策返佣信息 同行返点：" + profession + " 下级返点：" + subordinate + " 内部返点：" + @internal, OperatorRole.User, id.ToString(), creator);
            return f;
        }

        #endregion

        /// <summary>
        /// 获得所有贴扣点信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PolicySettingInfo> GetDeductions()
        {
            return from ps in DataContext.PolicySettings
                   join psp in DataContext.PolicySettingPeriods on ps.Id equals psp.PolicySetting into psps
                   select new PolicySettingInfo
                   {
                       Airline = ps.Airline,
                       Arrivals = ps.Arrivals,
                       Berths = ps.Berths,
                       Departure = ps.Departure,
                       EffectiveTimeEnd = ps.EffectiveTimeEnd,
                       EffectiveTimeStart = ps.EffectiveTimeStart,
                       Id = ps.Id,
                       Periods = psps,
                       Enable = ps.Enable,
                       LastModifyTime = ps.LastModifyTime,
                       CreateTime = ps.CreateTime
                   };
        }
        /// <summary>
        /// 取得指定的贴扣点信息；
        /// </summary>
        /// <param name="airline"></param>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="flightDate"></param>
        /// <param name="voyageType"></param>
        /// <returns></returns>
        public static MatchEnvironment GetMatchEnvironment(string airline, string departure, string arrival, DateTime flightDate, VoyageType voyageType)
        {
            var settings = voyageType != VoyageType.OneWay ? EnumerableHelper.GetEmpty<PolicySettingInfo>() :
                           (from ps in DataContext.PolicySettings
                            join psp in DataContext.PolicySettingPeriods on ps.Id equals psp.PolicySetting into psps
                            where (string.IsNullOrWhiteSpace(airline) || ps.Airline == airline) && ps.Departure == departure && ps.Arrivals.Contains(arrival) &&
                                // (ps.VoyageType == voyageType || ps.VoyageType == VoyageType.OneWayOrRound) && 
                                  flightDate >= ps.EffectiveTimeStart && flightDate <= ps.EffectiveTimeEnd
                                  && ps.Enable
                            select new PolicySettingInfo
                            {
                                Airline = ps.Airline,
                                Arrivals = ps.Arrivals,
                                Berths = ps.Berths,
                                Departure = ps.Departure,
                                EffectiveTimeEnd = ps.EffectiveTimeEnd,
                                EffectiveTimeStart = ps.EffectiveTimeStart,
                                Id = ps.Id,
                                Periods = psps,
                                Enable = ps.Enable,
                                CreateTime = ps.CreateTime,
                                LastModifyTime = ps.LastModifyTime
                            }).ToList();
            var harmonies = (from h in DataContext.PolicyHarmonies
                             where
                                 (string.IsNullOrWhiteSpace(airline) || h.Airlines.Contains(airline)) && h.Departure.Contains(departure) && h.Arrival.Contains(arrival) && h.EffectiveLowerDate <= flightDate &&
                                 h.EffectiveUpperDate >= flightDate
                             select new PolicyHarmonyInfo
                             {
                                 Airlines = h.Airlines,
                                 Arrival = h.Arrival,
                                 //CityLimit = h.CityLimit,
                                 DeductionType = h.DeductionType,
                                 Departure = h.Departure,
                                 EffectiveLowerDate = h.EffectiveLowerDate,
                                 EffectiveUpperDate = h.EffectiveUpperDate,
                                 HarmonyValue = h.HarmonyValue,
                                 Id = h.Id,
                                 // IsVIP = h.IsVIP,
                                 PolicyType = h.PolicyType,
                                 CreateTime = h.CreateTime,
                                 LastModifyTime = h.LastModifyTime
                             }).ToList();
            return new MatchEnvironment
            {
                PolicySettings = settings,
                PolicyHarmonies = harmonies,
                WorkingHours = CompanyService.GetWorkingHours()
            };
        }
        /// <summary>
        /// 查询政策协调
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页条件</param>
        /// <returns>查询结果</returns>
        public static IEnumerable<PolicyHarmonyInfo> QueryPolicyHarmonies(PolicyHarmonyQueryParameter condition, Pagination pagination)
        {
            var repository = Factory.CreatePolicyHarmoniesRepository();
            return repository.QueryPolicyHarmonyInfos(pagination, condition);
        }
        #region "日志"
        static void saveAddLog(string itemName, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Insert, "添加" + itemName + "。", role, key, account);
        }
        static void saveUpdateLog(string itemName, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。", itemName), role, key, account);
        }
        static void saveDeleteLog(string itemName, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + "。", role, key, account);
        }
        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.政策, operationType, account, role, key, content);
            LogService.SaveOperationLog(log);
        }
        #endregion

        public static PolicyType CheckIfHasDefaultPolicy(Guid companyId)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.CheckIfHasDefaultPolicy(companyId);
        }

        public static PolicyType CheckIfHasDefaultPolicy(Guid companyId, List<string> airlines)
        {
            var repository = Factory.CreatePolicyRepository();
            return repository.CheckIfHasDefaultPolicy(companyId, airlines);
        }
    }
}