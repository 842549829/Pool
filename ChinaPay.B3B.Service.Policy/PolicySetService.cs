using System;
using System.Collections.Generic;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Policy.Repository;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.Service.Policy
{
    public static class PolicySetService
    {
        #region"查询"
        public static Domain.SetPolicy QuerySetPolicy(Guid companyid)
        {
            var repository = Factory.CreatePolicySetRepository();
            return repository.QuerySetPolicy(companyid);
        }

        public static Domain.CompanyLimitPolicy QueryLimitPolicy(Guid companyid)
        {
            var repository = Factory.CreatePolicySetRepository();
            return repository.QueryLimitPolicy(companyid);
        }

        public static IEnumerable<string> QueryAirlines(Guid companyid)
        {
            var repository = Factory.CreatePolicySetRepository();
            return repository.QueryAirlines(companyid);
        }
        #endregion
        #region"新增/修改"
        public static void Save(Domain.SetPolicy policy, string operatorAccount)
        {
            var model = PolicySetService.QuerySetPolicy(policy.Company);
            var reposity = Factory.CreatePolicySetRepository();
            reposity.Save(policy);
            // 记录日志
            string newContent = string.Format("公司Id:{0},可发布特价政策条数:{1},可发布单程控位政策条数:{2},可发布散冲团政策条数:{3},可发布免票政策条数:{4},可发布集团票政策条数:{5},可发布商旅卡政策条数:{6},可发布其他特殊政策{9},可发布低打高返政策条数{10},航空公司:{7},出港城市:{8}",
                                                policy.Company, policy.PromotionCount, policy.SinglenessCount, policy.DisperseCount, policy.CostFreeCount, policy.BlocCount, policy.BusinessCount, policy.Airlines.Join(","), policy.Departure.Join(","), policy.OtherSpecialCount, policy.LowToHighCount);
            if (model != null)
            {
                string originalContent = string.Format("公司Id:{0},可发布特价政策条数:{1},可发布单程控位政策条数:{2},可发布散冲团政策条数:{3},可发布免票政策条数:{4},可发布集团票政策条数:{5},可发布商旅卡政策条数:{6},可发布其他特殊政策{9},可发布低打高返政策条数{10},航空公司:{7},出港城市:{8}",
                                                 policy.Company, model.PromotionCount, model.SinglenessCount, model.DisperseCount, model.CostFreeCount, model.BlocCount, model.BusinessCount, model.Airlines.Join(","), model.Departure.Join(","), policy.OtherSpecialCount, policy.LowToHighCount);
                saveUpdateLog("政策设置", originalContent, newContent, OperatorRole.Platform, policy.Company.ToString(), operatorAccount);
            }
            else
            {
                saveAddLog("政策设置", newContent, OperatorRole.Platform, policy.Company.ToString(), operatorAccount);
            }
        }

        public static void Save(Domain.CompanyLimitPolicy policy, string operatorAccount)
        {
            var model = PolicySetService.QueryLimitPolicy(policy.Company);
            var repository = Factory.CreatePolicySetRepository();
            repository.Save(policy);
            // 记录日志
            string originalContent = "";
            string newContent = string.Format("返点:{0},可出票航空公司:{1}", policy.Child.Rebate, policy.Child.Airlines.Join(","));
            if (model != null)
            {
                originalContent = string.Format("返点:{0},可出票航空公司:{1}", model.Child.Rebate, model.Child.Airlines.Join(","));
                saveUpdateLog("公司设置政策", originalContent, newContent, OperatorRole.Provider, policy.Company.ToString(), operatorAccount);
            }
            else
            {
                saveAddLog("公司设置政策", newContent, OperatorRole.Provider, policy.Company.ToString(), operatorAccount);
            }

        }
        #endregion
        #region "日志"
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
            var log = new Log.Domain.OperationLog(OperationModule.政策, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion

        #region  普通政策单程设置贴扣点信息
        public static bool AddNormalPolicySetting(NormalPolicySetting view)
        {
            using (var cmd = Factory.CreateCommand())
            {
                bool falg = false;
                cmd.BeginTransaction();
                try
                {
                    var repository = Factory.CreateNormalPolicySettingRepository(cmd);
                    repository.AddNormalPolicySetting(view);
                    cmd.CommitTransaction();
                    falg = true;
                }
                catch
                {
                    cmd.RollbackTransaction();
                    falg = false;
                    throw;
                }
                string newContent = string.Format("政策编号：{0}，{1}航线：{2}，适用舱位{3}，{1}数值：{4}，{1}开始时间：{5}，{1}结束时间：{6}，备注：{7}，操作者：{8}"
                    , view.PolicyId, view.Type ? "贴点" : "扣点", view.FlightsFilter, view.Berths, view.Commission * 100, view.StartTime, view.EndTime, view.Remark, view.Creator);
                saveAddLog("政策设置贴/扣点", newContent, OperatorRole.Platform, view.PolicyId.ToString(), view.Creator);
                return falg;
            }
        }

        public static bool UpdateNormalPolicySetting(Guid Id, bool Enable, string creator, bool type)
        {
            using (var cmd = Factory.CreateCommand())
            {
                bool falg = false;
                cmd.BeginTransaction();
                try
                {
                    var repository = Factory.CreateNormalPolicySettingRepository(cmd);
                    repository.UpdateNormalPolicySetting(Id, Enable);
                    cmd.CommitTransaction();
                    falg = true;
                }
                catch
                {
                    cmd.RollbackTransaction();
                    falg = false;
                    throw;
                }
                string newContent = string.Format("设置编号：{0}，状态：{1}，操作者：{2}"
                    , Id, type ? "贴点" : "扣点", Enable ? "启用" : "禁用", creator);
                string originalContent = string.Format("设置编号：{0}，状态：{1}，操作者：{2}"
                    , Id, type ? "贴点" : "扣点", !Enable ? "启用" : "禁用", creator);
                saveUpdateLog("政策设置贴/扣点", originalContent, newContent, OperatorRole.Provider, Id.ToString(), creator);
                return falg;
            }
        }
        /// <summary>
        /// 查询政策设置信息
        /// </summary>
        /// <param name="policyId">政策编号</param>
        /// <param name="Type">null查询当前政策的下的贴扣点所有有效信息,是否贴扣点</param>
        /// <param name="Enable">是否启用</param>
        /// <returns></returns>
        public static IEnumerable<DataTransferObject.Policy.NormalPolicySetting> QueryNormalPolicySetting(Guid policyId, bool? Type, bool? Enable)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var repository = Factory.CreateNormalPolicySettingRepository(cmd);
                return repository.QueryNormalPolicySetting(policyId, Type, Enable, "", "", null);
            }
        }

        /// <summary>
        /// 查询政策设置信息
        /// </summary>
        /// <param name="policyId">政策编号</param>
        /// <param name="Type">是否贴点</param>
        /// <param name="FlightsFilter">航段信息</param>
        /// <param name="Berths">舱位列表</param>
        /// <param name="flightDate">航班日期</param>
        /// <returns>普通单程政策设置信息列表</returns>
        /// <remarks>
        /// 航段信息格式：KMGCTU
        /// 传入的是舱位列表，给出的分解后的单个舱位的信息；
        /// </remarks>
        public static IEnumerable<DataTransferObject.Policy.NormalPolicySetting> QueryNormalPolicySetting(Guid? policyId, bool? Type, string FlightsFilter, string Berths, DateTime? flightDate)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var repository = Factory.CreateNormalPolicySettingRepository(cmd);
                return repository.QueryNormalPolicySetting(policyId, Type, true, FlightsFilter, Berths, flightDate);
            }
        }

        public static IEnumerable<NormalPolicySetting> QueryAllValidNormalPolicySetting()
        {
            using (var cmd = Factory.CreateCommand())
            {
                var repository = Factory.CreateNormalPolicySettingRepository(cmd);
                return repository.QueryNormalPolicySetting(null, null, true, null, null, null);
            }
        }

        #endregion
    }
}
