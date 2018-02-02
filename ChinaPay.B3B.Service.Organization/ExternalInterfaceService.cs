using System;
using System.Collections.Generic;
using System.Transactions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using Izual.Linq;

namespace ChinaPay.B3B.Service.Organization
{
    public class ExternalInterfaceService
    {


        /// <summary>
        /// 保存外接口信息
        /// </summary>
        /// <param name="setting"></param>
        public static void Save(ExternalInterfaceView view)
        {
            var companyInfo = CompanyService.GetCompanyInfo(view.CompanyId);
            ExternalInterfaceSetting setting = null;
            ExternalInterfaceSetting orginalSetting = null;
            if (view.IsOpenExternalInterface)
            {
                setting = new ExternalInterfaceSetting(view.CompanyId);
                setting.SecurityCode = view.SecurityCode;
                if (!string.IsNullOrWhiteSpace(view.RefuseAddress))
                    setting.RefuseAddress = view.RefuseAddress;
                if (!string.IsNullOrWhiteSpace(view.PaySuccessAddress))
                    setting.PaySuccessAddress = view.PaySuccessAddress;
                setting.OpenTime = DateTime.Now;
                setting.InterfaceInvokeMethod = view.InterfaceInvokeMethod;
                if (!string.IsNullOrWhiteSpace(view.DrawSuccessAddress))
                    setting.DrawSuccessAddress = view.DrawSuccessAddress;
                if (!string.IsNullOrWhiteSpace(view.ConfirmSuccessAddress))
                    setting.ConfirmSuccessAddress = view.ConfirmSuccessAddress;
                if (!string.IsNullOrWhiteSpace(view.ConfirmFailAddress))
                    setting.ConfirmFailAddress = view.ConfirmFailAddress;

                if (!string.IsNullOrWhiteSpace(view.AgreedAddress))
                    setting.AgreedAddress = view.AgreedAddress;
                if (!string.IsNullOrWhiteSpace(view.RefundSuccessAddress))
                    setting.RefundSuccessAddress = view.RefundSuccessAddress;
                if (!string.IsNullOrWhiteSpace(view.ReturnTicketSuccessAddress))
                    setting.ReturnTicketSuccessAddress = view.ReturnTicketSuccessAddress;
                if (!string.IsNullOrWhiteSpace(view.ReschedulingAddress))
                    setting.ReschedulingAddress = view.ReschedulingAddress;
                if (!string.IsNullOrWhiteSpace(view.RefuseTicketAddress))
                    setting.RefuseTicketAddress = view.RefuseTicketAddress;
                if (!string.IsNullOrWhiteSpace(view.ReschPaymentAddress))
                    setting.ReschPaymentAddress = view.ReschPaymentAddress;
                if (!string.IsNullOrWhiteSpace(view.CanceldulingAddress))
                    setting.CanceldulingAddress = view.CanceldulingAddress;
                if (!string.IsNullOrWhiteSpace(view.RefuseChangeAddress))
                    setting.RefuseChangeAddress = view.RefuseChangeAddress;
                if (!string.IsNullOrWhiteSpace(view.RefundApplySuccessAddress))
                    setting.RefundApplySuccessAddress = view.RefundApplySuccessAddress;
                setting.BindIP = view.BindIP;
                setting.PolicyTypes = view.PolicyTypes;
                orginalSetting = Query(view.CompanyId);
                if (orginalSetting!=null)
                {
                    setting.OpenTime = orginalSetting.OpenTime;
                }
            }
            using (var trans = new TransactionScope())
            {
                DataContext.Companies.Update(cmp => new { IsOpenExternalInterface = view.IsOpenExternalInterface }, cmp => cmp.Id == view.CompanyId);
                if (setting != null)
                    Save(setting);
                trans.Complete();
            }
            if (view.IsOpenExternalInterface)
            {
                if (orginalSetting == null)
                {
                    var content = string.Format(@"公司Id:{0},安全码:{1},确认成功通知地址:{2},确认失败通知地址:{3},支付成功通知地址:{4},出票成功通知地址:{5},拒绝出票通知地址:{6},同意改期通知地址:{7}, 退废票退款成功通知地址:{8},退废票处理成功通知地址:{9},改期成功通知地址:{10},拒绝退废票通知地址:{11},改期支付成功通知地址:{12},拒绝改期通知地址:{13},取消出票退款成功:{14},取消出票退款成功:{15},开通时间:{16},可调用方法:{17},指定IP：{18},可使用的政策类型：{19}",
                                      setting.CompanyId, setting.SecurityCode, setting.ConfirmSuccessAddress, setting.ConfirmFailAddress, setting.PaySuccessAddress,
                                      setting.DrawSuccessAddress, setting.RefuseAddress, setting.AgreedAddress, setting.RefundSuccessAddress, setting.ReturnTicketSuccessAddress,
                                      setting.ReschedulingAddress, setting.RefuseTicketAddress, setting.ReschPaymentAddress, setting.RefuseChangeAddress,
                                      setting.CanceldulingAddress, setting.RefundApplySuccessAddress, setting.OpenTime, setting.InterfaceInvokeMethod.Join(","), setting.BindIP, setting.GetPolicyTypesStr);
                    saveAddLog("数据接口信息", content, OperatorRole.Platform, view.CompanyId.ToString(), view.OperatorAccount);
                }
                else
                {
                    var orginalContent = string.Format(@"公司Id:{0},安全码:{1},确认成功通知地址:{2},确认失败通知地址:{3},支付成功通知地址:{4},出票成功通知地址:{5},拒绝出票通知地址:{6},同意改期通知地址:{7}, 退废票退款成功通知地址:{8},退废票处理成功通知地址:{9},改期成功通知地址:{10},拒绝退废票通知地址:{11},改期支付成功通知地址:{12},拒绝改期通知地址:{13},取消出票退款成功:{14},取消出票退款成功:{15},开通时间:{16},可调用方法:{17},指定IP：{18},可使用的政策类型：{19}",
                                      orginalSetting.CompanyId, orginalSetting.SecurityCode, orginalSetting.ConfirmSuccessAddress, orginalSetting.ConfirmFailAddress,
                                       orginalSetting.PaySuccessAddress, orginalSetting.DrawSuccessAddress, orginalSetting.RefuseAddress, orginalSetting.AgreedAddress,
                                       orginalSetting.RefundSuccessAddress, orginalSetting.ReturnTicketSuccessAddress, orginalSetting.ReschedulingAddress,
                                       orginalSetting.RefuseTicketAddress, orginalSetting.ReschPaymentAddress, orginalSetting.RefuseChangeAddress,
                                       orginalSetting.CanceldulingAddress, orginalSetting.RefundApplySuccessAddress, orginalSetting.OpenTime,
                                       orginalSetting.InterfaceInvokeMethod == null ? string.Empty : orginalSetting.InterfaceInvokeMethod.Join(","),
                                       orginalSetting.BindIP, orginalSetting.GetPolicyTypesStr);

                    var newContent = string.Format(@"公司Id:{0},安全码:{1},确认成功通知地址:{2},确认失败通知地址:{3},支付成功通知地址:{4},出票成功通知地址:{5},拒绝出票通知地址:{6},同意改期通知地址:{7}, 退废票退款成功通知地址:{8},退废票处理成功通知地址:{9},改期成功通知地址:{10},拒绝退废票通知地址:{11},改期支付成功通知地址:{12},拒绝改期通知地址:{13},取消出票退款成功:{14},取消出票退款成功:{15},开通时间:{16},可调用方法:{17},指定IP：{18},可使用的政策类型：{19}",
                                      setting.CompanyId, setting.SecurityCode, setting.ConfirmSuccessAddress, setting.ConfirmFailAddress, setting.PaySuccessAddress,
                                      setting.DrawSuccessAddress, setting.RefuseAddress, setting.AgreedAddress, setting.RefundSuccessAddress,
                                      setting.ReturnTicketSuccessAddress, setting.ReschedulingAddress, setting.RefuseTicketAddress, setting.ReschPaymentAddress,
                                      setting.RefuseChangeAddress, setting.CanceldulingAddress, setting.RefundApplySuccessAddress, setting.OpenTime,
                                      setting.InterfaceInvokeMethod == null ? string.Empty : setting.InterfaceInvokeMethod.Join(","),
                                      setting.BindIP, orginalSetting.GetPolicyTypesStr);

                    saveUpdateLog("数据接口信息", orginalContent, newContent, OperatorRole.Platform, view.CompanyId.ToString(), view.OperatorAccount);
                }
            }
            else
            {
                saveUpdateLog("数据接口", string.Format("公司Id:{0},是否启用数据接口:{1}", view.CompanyId, companyInfo.IsOpenExternalInterface ? "已启用" : "未启用"),
                string.Format("公司Id:{0},是否启用数据接口:{1}", view.CompanyId, view.IsOpenExternalInterface ? "已启用" : "未启用"), OperatorRole.Platform, view.CompanyId.ToString(), view.OperatorAccount);
            }
        }
        private static void Save(ExternalInterfaceSetting setting)
        {
            var repository = Factory.CreateExternalInterfaceRepository();
            repository.Save(setting);
        }

        public static ExternalInterfaceSetting Query(Guid companyId)
        {
            var repository = Factory.CreateExternalInterfaceRepository();
            return repository.Query(companyId);
        }
        /// <summary>
        /// 查询接口列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        /// <returns></returns>
        public static IEnumerable<ExternalInterfaceInfo> Query(ExternalInterfaceQueryCondition condition, Pagination pagination)
        {
            var repository = Factory.CreateExternalInterfaceRepository();
            return repository.Query(condition, pagination);
        }

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
