using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Order.Repository;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 申请单查询服务
    /// </summary>
    public static class ApplyformQueryService {
        /// <summary>
        /// 获取申请单
        /// </summary>
        /// <param name="applyformId">申请单号</param>
        public static Order.Domain.Applyform.BaseApplyform QueryApplyform(decimal applyformId) {
            try
            {
                using (var command = Factory.CreateCommand())
                {
                    var repository = Factory.CreateApplyformRepository(command);
                    return repository.QueryApplyform(applyformId);
                }
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 查询退/废票申请单
        /// </summary>
        /// <param name="refundOrScrapApplyformId">退/废票申请单号</param>
        public static Order.Domain.Applyform.RefundOrScrapApplyform QueryRefundOrScrapApplyform(decimal refundOrScrapApplyformId) {
            var applyform = QueryApplyform(refundOrScrapApplyformId);
            if(applyform != null) {
                return applyform as Order.Domain.Applyform.RefundOrScrapApplyform;
            }
            return null;
        }
        /// <summary>
        /// 查询退票申请单
        /// </summary>
        /// <param name="refundApplyformId">退票申请单号</param>
        public static Order.Domain.Applyform.RefundApplyform QueryRefundApplyform(decimal refundApplyformId) {
            var applyform = QueryApplyform(refundApplyformId);
            if(applyform != null) {
                return applyform as Order.Domain.Applyform.RefundApplyform;
            }
            return null;
        }
        /// <summary>
        /// 查询废票申请单
        /// </summary>
        /// <param name="scrapApplyformId">废票申请单号</param>
        public static Order.Domain.Applyform.ScrapApplyform QueryScrapApplyform(decimal scrapApplyformId) {
            var applyform = QueryApplyform(scrapApplyformId);
            if(applyform != null) {
                return applyform as Order.Domain.Applyform.ScrapApplyform;
            }
            return null;
        }
        /// <summary>
        /// 查询改期申请单
        /// </summary>
        /// <param name="postponeApplyformId">改期申请单号</param>
        public static Order.Domain.Applyform.PostponeApplyform QueryPostponeApplyform(decimal postponeApplyformId) {
            var applyform = QueryApplyform(postponeApplyformId);
            if(applyform != null) {
                return applyform as Order.Domain.Applyform.PostponeApplyform;
            }
            return null;
        }
        /// <summary>
        /// 查询申请单附件信息
        /// </summary>
        /// <returns>附件信息</returns>
        public static List<ApplyAttachmentView> QueryApplyAttachmentView(decimal applyformId) 
        {
            using(DataAccess.DbOperator commad = Factory.CreateCommand())
	        {
                IApplyformRepository repository = Factory.CreateApplyformRepository(commad);
                return repository.QueryApplyAttachmentView(applyformId);
	        }
        }
        /// <summary>
        /// 查询申请单附件信息
        /// </summary>
        /// <param name="applyAttachmentId">申请单Id</param>
        /// <returns></returns>
        public static ApplyAttachmentView QueryApplyAttachmentView(Guid applyAttachmentId) 
        {
            using (DataAccess.DbOperator commad = Factory.CreateCommand())
            {
                IApplyformRepository repository = Factory.CreateApplyformRepository(commad);
                return repository.QueryApplyAttachmentView(applyAttachmentId);
            }
        }
        /// <summary>
        /// 删除附件 
        /// </summary>
        /// <param name="applyAttachmentId"></param>
        /// <returns></returns>
        public static bool DeleteApplyAttachmentView(Guid applyAttachmentId, string operators)
        {
            using (DataAccess.DbOperator commad = Factory.CreateCommand())
            {
                //记录操作日志
                ApplyAttachmentView apply = Service.ApplyformQueryService.QueryApplyAttachmentView(applyAttachmentId);
                bool isSuccess = false;
                IApplyformRepository repository = Factory.CreateApplyformRepository(commad);
                try
                {
                    repository.DeleteApplyAttachmentView(applyAttachmentId);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex);
                    isSuccess = false;   
                }
                if (isSuccess && apply != null)
                {
                    saveLog(OperationType.Delete, string.Format("删除:退票附件。申请单号：{0},附件Id：{1},源文件相对路径:{2},时间:{3},操作员:{4}",
                         apply.ApplyformId, apply.Id, apply.FilePath, apply.Time, operators),
                        OperatorRole.Platform, apply.ApplyformId.ToString(), operators);
                }
                return isSuccess;
            }
        }
        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="applyAttachmentViews"></param>
        /// <returns></returns>
        public static bool AddApplyAttachmentView(List<ApplyAttachmentView> applyAttachmentViews,string operators) {
            using (DataAccess.DbOperator commad = Factory.CreateCommand())
            {
                bool isSuccess = false;
                IApplyformRepository repository = Factory.CreateApplyformRepository(commad);
                try
                {
                    repository.AddApplyAttachmentView(applyAttachmentViews);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex);
                    isSuccess = false;
                }
                //记录操作日志
                ApplyAttachmentView apply = applyAttachmentViews.FirstOrDefault();
                if (isSuccess && apply != null)
                {
                    saveLog(OperationType.Insert, string.Format("添加:退票附件。申请单号：{0},附件Id：{1},源文件相对路径:{2},时间:{3},操作员：{4}",
                         apply.ApplyformId, apply.Id, apply.FilePath, apply.Time, operators),
                        OperatorRole.Platform, apply.ApplyformId.ToString(), operators);
                }
                return isSuccess;
            }
        }
        /// <summary>
        /// 获取订单下的申请单
        /// </summary>
        /// <param name="orderId">订单号</param>
        public static IEnumerable<Order.Domain.Applyform.BaseApplyform> QueryApplyforms(decimal orderId) {
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateApplyformRepository(command);
                return repository.QueryApplyforms(orderId);
            }
        }
        /// <summary>
        /// 获取申请单列表信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        public static IEnumerable<ApplyformListView> QueryApplyforms(ApplyformQueryCondition condition, Pagination pagination) {
            if(condition == null) throw new ArgumentNullException("condition");
            if(pagination == null) throw new ArgumentNullException("pagination");
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateApplyformRepository(command);
                return repository.QueryApplyforms(condition, pagination);
            }
        }
        /// <summary>
        /// 获取申请单列表信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        public static IEnumerable<ApplyformListView> QueryApplyformsNew(ApplyformQueryCondition condition, Pagination pagination) {
            if(condition == null) throw new ArgumentNullException("condition");
            if(pagination == null) throw new ArgumentNullException("pagination");
            using(var command = Factory.CreateCommand()) {
                var repository = Factory.CreateApplyformRepository(command);
                return repository.QueryApplyformsNew(condition, pagination);
            }
        }
        /// <summary>
        /// 平台查询待处理的申请单
        /// </summary>
        public static IEnumerable<ApplyformListView> PlatformQueryApplyformsForProcessNew(ApplyformQueryCondition condition, Pagination pagination)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            condition.RefundStatuses = RefundApplyformStatus.AppliedForPlatform | RefundApplyformStatus.AppliedForCancelReservation | RefundApplyformStatus.DeniedByProviderBusiness;
            condition.PostponeStatuses = PostponeApplyformStatus.Applied | PostponeApplyformStatus.Paid;
            condition.RequireRevisePrice = true;
            condition.IsStatusAndRequireRevisePrice = false;
            return QueryApplyformsNew(condition, pagination);
        }
        /// <summary>
        /// 平台查询待处理的申请单
        /// </summary>
        public static IEnumerable<ApplyformListView> PlatformQueryApplyformsForProcess(ApplyformQueryCondition condition, Pagination pagination) {
            if(condition == null) throw new ArgumentNullException("condition");
            condition.RefundStatuses = RefundApplyformStatus.AppliedForPlatform | RefundApplyformStatus.AppliedForCancelReservation | RefundApplyformStatus.DeniedByProviderBusiness;
            condition.PostponeStatuses = PostponeApplyformStatus.Applied | PostponeApplyformStatus.Paid;
            condition.RequireRevisePrice = true;
            condition.IsStatusAndRequireRevisePrice = false;
            return QueryApplyforms(condition, pagination);
        }
        /// <summary>
        /// 出票方查询申请单
        /// </summary>
        public static IEnumerable<ApplyformListView> ProviderQueryApplyforms(ApplyformQueryCondition condition, Pagination pagination) {
            if(condition == null) throw new ArgumentNullException("condition");
            condition.RequireRevisePrice = false;
            condition.IsStatusAndRequireRevisePrice = true;

            if (condition.ApplyformType.HasValue)
            {
                condition.ApplyformType = ExcludeApplyformType(condition.ApplyformType.Value, ApplyformType.Postpone);
            }
            else
            {
                condition.ApplyformType = ApplyformType.Refund | ApplyformType.Scrap | ApplyformType.BlanceRefund;
            }
            if ((condition.ApplyformType.Value & ApplyformType.Refund) == ApplyformType.Refund ||
                (condition.ApplyformType.Value & ApplyformType.Scrap) == ApplyformType.Scrap)
            {
                if (!condition.RefundStatuses.HasValue)
                {
                    condition.RefundStatuses = GetAllRefundStatus();
                }
                condition.RefundStatuses = ExcludeRefundStatus(condition.RefundStatuses.Value,
                    RefundApplyformStatus.AppliedForPlatform, RefundApplyformStatus.AppliedForCancelReservation);
            }
            if (!condition.BalanceRefundProcessStatus.HasValue)
            {
                condition.BalanceRefundProcessStatus = GetAllBalanceRefundStatus();
            }
            return QueryApplyforms(condition, pagination);
        }

        private static BalanceRefundProcessStatus? GetAllBalanceRefundStatus() {
            var statuses = Enum.GetValues(typeof(BalanceRefundProcessStatus)) as BalanceRefundProcessStatus[];
            return statuses.Aggregate<BalanceRefundProcessStatus, BalanceRefundProcessStatus>(0, (current, status) => current | status);
        }

        /// <summary>
        /// 出票方查询申请单
        /// </summary>
        public static IEnumerable<ApplyformListView> ProviderQueryApplyformsNew(ApplyformQueryCondition condition, Pagination pagination)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            condition.RequireRevisePrice = false;
            condition.IsStatusAndRequireRevisePrice = true;

            if (condition.ApplyformType.HasValue)
            {
                condition.ApplyformType = ExcludeApplyformType(condition.ApplyformType.Value, ApplyformType.Postpone);
            }
            else
            {
                condition.ApplyformType = ApplyformType.Refund | ApplyformType.Scrap | ApplyformType.BlanceRefund;
            }
            if ((condition.ApplyformType.Value & ApplyformType.Refund) == ApplyformType.Refund ||
                (condition.ApplyformType.Value & ApplyformType.Scrap) == ApplyformType.Scrap)
            {
                if (!condition.RefundStatuses.HasValue)
                {
                    condition.RefundStatuses = GetAllRefundStatus();
                }
                condition.RefundStatuses = ExcludeRefundStatus(condition.RefundStatuses.Value,
                    RefundApplyformStatus.AppliedForPlatform, RefundApplyformStatus.AppliedForCancelReservation);
            }

            return QueryApplyformsNew(condition, pagination);
        }
        /// <summary>
        /// 出票方查询待处理的申请单
        /// 只能查退/票
        /// </summary>
        public static IEnumerable<ApplyformListView> ProviderQueryApplyformsForProcess(ApplyformQueryCondition condition, Pagination pagination) {
            if(condition == null) throw new ArgumentNullException("condition");
            condition.RefundStatuses = RefundApplyformStatus.AppliedForProvider | RefundApplyformStatus.DeniedByProviderTreasurer;
            return ProviderQueryApplyforms(condition, pagination);
        }

        /// <summary>
        /// 出票方查询待处理的申请单
        /// 只能查退/票
        /// </summary>
        public static IEnumerable<ApplyformListView> ProviderQueryApplyformsForProcessNew(ApplyformQueryCondition condition, Pagination pagination)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            condition.RefundStatuses = RefundApplyformStatus.AppliedForProvider | RefundApplyformStatus.DeniedByProviderTreasurer;
            return ProviderQueryApplyformsNew(condition, pagination);
        }
        /// <summary>
        /// 出票方的财务查询待退款的申请单
        /// 只能查退/票
        /// </summary>
        public static IEnumerable<ApplyformListView> ProviderQueryApplyformsForReturnMoney(ApplyformQueryCondition condition, Pagination pagination) {
            if(condition == null) throw new ArgumentNullException("condition");
            condition.RefundStatuses = RefundApplyformStatus.AgreedByProviderBusiness;
            condition.ApplyformType = ApplyformType.Refund|ApplyformType.Scrap;
            return ProviderQueryApplyforms(condition, pagination);
        }
        /// <summary>
        /// 出票方的财务查询待差错退款的申请单
        /// 只能查退/票
        /// </summary>
        public static IEnumerable<ApplyformListView> ProviderQueryBalanceRefundForReturnMoney(ApplyformQueryCondition condition, Pagination pagination)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            condition.BalanceRefundProcessStatus = BalanceRefundProcessStatus.AgreedByProviderBusiness;
            condition.ApplyformType = ApplyformType.BlanceRefund;
            return ProviderQueryApplyforms(condition, pagination);
        }

        /// <summary>
        /// 查询差额退款申请单
        /// </summary>
        /// <param name="applyformId">申请单号</param>
        /// <returns></returns>
        public static BalanceRefundApplyform QueryBalanceRefundApplyform(decimal applyformId) {
            try
            {
                using (var command = Factory.CreateCommand())
                {
                    var repository = Factory.CreateApplyformRepository(command);
                    return repository.QueryBalanceRefundApplyform(applyformId);
                }
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                return null;
            }
        }
        private static RefundApplyformStatus GetAllRefundStatus() {
            var statuses = Enum.GetValues(typeof(RefundApplyformStatus)) as RefundApplyformStatus[];
            return statuses.Aggregate<RefundApplyformStatus, RefundApplyformStatus>(0, (current, status) => current | status);
        }
        private static RefundApplyformStatus ExcludeRefundStatus(RefundApplyformStatus originalStatuses, params RefundApplyformStatus[] statuses) {
            return statuses.Aggregate(originalStatuses, (current, status) => {
                if((current & status) == status) {
                    return current ^ status;
                } else {
                    return current;
                }
            });
        }
        private static ApplyformType ExcludeApplyformType(ApplyformType originalApplforms, params ApplyformType[] types)
        {
            return types.Aggregate(originalApplforms, (current, type) =>
            {
                if ((current & type) == type)
                {
                    return current ^ type;
                }
                else
                {
                    return current;
                }
            });
        }

        private static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            OperationLog log = new OperationLog(OperationModule.其他, operationType,account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch
            {
            }
        }
    }
}