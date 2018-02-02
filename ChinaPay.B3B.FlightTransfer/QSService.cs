using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.FlightTransfer;
using ChinaPay.B3B.Service.FlightTransfer.Repository;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core;
using ChinaPay.SMS.DataTransferObject;
using ChinaPay.SMS.Service.Domain;

namespace ChinaPay.B3B.Service.FlightTransfer
{
    public class QSService
    {
        /// <summary>
        /// 查询采购的航班变动通知信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static IEnumerable<TransferInformation> QueryTransferInformation(Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QueryTransferInformation(pagination);
            }
        }

        /// <summary>
        /// 查询采购的航班变动通知信息
        /// </summary>
        /// <param name="purchaserId"></param>
        /// <returns></returns>
        public static TransferInformation QuerySingleTransferInfomation(Guid purchaserId)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QuerySingleTransferInfomation(purchaserId);
            }

        }

        /// <summary>
        /// 查询采购的航班变动详细信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="purchaserId">采购Id</param>
        /// <returns></returns>
        public static IEnumerable<TransferDetail> QueryTransferDetails(Pagination pagination, Guid purchaserId)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QueryTransferDetails(pagination, purchaserId);
            }
        }

        /// <summary>
        /// 查询清Q通知结果
        /// </summary>
        /// <param name="conditoin"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static IEnumerable<InformRecord> QueryInformRecords(InfomrRecordSearchConditoin conditoin, Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QueryInformRecords(conditoin, pagination);
            }

        }

        /// <summary>
        /// 查询清Q通知结果
        /// </summary>
        /// <param name="conditoin"></param>
        /// <returns></returns>
        public static DataTable QueryInformRecords(InfomrRecordSearchConditoin conditoin)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QueryInformRecords(conditoin);
            }

        }

        /// <summary>
        /// 保存通知记录
        /// </summary>
        /// <param name="purchaerId"> </param>
        /// <param name="transferIds"></param>
        /// <param name="inform"></param>
        /// <param name="result"></param>
        /// <param name="remark"></param>
        /// <param name="operatorAccount"> </param>
        /// <param name="operatorName"> </param>
        /// <returns></returns>
        public static bool InformPurchaser(Guid purchaerId, List<Guid> transferIds, InformType inform, InformResult result, string remark, string operatorAccount, string operatorName)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.InformPurchaser(purchaerId, transferIds, inform, result, remark, operatorAccount, operatorName);
            }
        }

        /// <summary>
        /// 发送短信通知采购
        /// </summary>
        /// <param name="purchaerId"> </param>
        /// <param name="transferIds"></param>
        /// <param name="messageContent"></param>
        /// <param name="operatorAccount"> </param>
        /// <param name="operatorName"> </param>
        /// <returns></returns>
        public static bool MessageInformPurchaser(Guid purchaerId, List<Guid> transferIds, string messageContent, string operatorAccount, string operatorName)
        {
            return InformPurchaser(purchaerId, transferIds, InformType.Message, InformResult.Informed, messageContent, operatorAccount, operatorName);
        }

        /// <summary>
        /// 发送短信将所有通知通知采购
        /// </summary>
        /// <param name="purchaerId"> </param>
        /// <param name="messageContent"></param>
        /// <returns></returns>
        public static bool MessageInformPurchaser(Guid purchaerId, string messageContent, string operatorAccount, string operatorName)
        {
            return InformPurchaser(purchaerId, null, InformType.Message, InformResult.Informed, messageContent, operatorAccount, operatorName);
        }


        /// <summary>
        /// 保存某采购的所有通知记录
        /// </summary>
        /// <param name="purchaerId"> </param>
        /// <param name="inform"></param>
        /// <param name="result"></param>
        /// <param name="remark"></param>
        /// <param name="operatorAccount"> </param>
        /// <param name="operatorName"> </param>
        /// <returns></returns>
        public static bool InformPurchaser(Guid purchaerId, InformType inform, InformResult result, string remark, string operatorAccount, string operatorName)
        {
            return InformPurchaser(purchaerId, null, inform, result, remark, operatorAccount, operatorName);
        }

        /// <summary>
        /// 清Q统计
        /// </summary>
        /// <returns></returns>
        public static FlightTransferStatInfo QueryFlightTransferStatInfo()
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QueryFlightTransferStatInfo();
            }
        }

        public static bool AutoInformPuchaser()
        {
            var newInformations = new List<TransferDetail>();
            try
            {
                using (var command = Factory.CreateCommand())
                {
                    var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                    newInformations = transferInfomationRepository.QueryCurrentBatInformation();
                }
                var purchaserInformation = newInformations.GroupBy(i => i.PurchaserPhone);
                foreach (IGrouping<string, TransferDetail> info in purchaserInformation)
                {
                    var purchaserId = info.First().PurchaserId;
                    var autoSendMsgSeted = ChinaPay.SMS.Service.SMSCompanySmsParamService.Query(AccountType.Payment, purchaserId);
                    if ((autoSendMsgSeted.B3BReceiveSms & CompanyB3BReceiveSms.FlightChanges) == 0) continue;
                    var servicePhone = GetServicePhone();
                    ChinaPay.SMS.Service.SMSSendService.SendB3BFlightDelay(info.Key, new Account(purchaserId, "系统自动"),
                        string.Join(",", info.Select(f => f.Carrier + f.FlightNo).Distinct()),
                        string.Join(",", info.Select(i => i.PNR.PNR).Distinct()), servicePhone);
                    InformPurchaser(purchaserId, InformType.Message, InformResult.Informed, "系统自动通知", string.Empty, "平台");
                }
            }
            catch (Exception ex)
            {
                LogService.SaveTextLog("自动发送航班变动通知短信" + ex.Message + ex.Source + ex.StackTrace);
                return false;
            }
            return true;
        }

        private static string GetServicePhone() {
            IEnumerable<SystemDictionaryItem> oemContract = SystemDictionaryService.Query(SystemDictionaryType.OEMContract);
            SystemDictionaryItem result = oemContract.FirstOrDefault(r => r.Name == "ServicePhone");
            return result != null && !string.IsNullOrEmpty(result.Value) ? result.Value : string.Empty;
        
        }

        /// <summary>
        /// 对每个采购的清Q单独统计
        /// </summary>
        /// <returns></returns>
        public static PurchaseFlightTransferInfo QueryPurchaseFlightStaticInfo(Guid purchaseId)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QueryPurchaseFlightStaticInfo(purchaseId);
            }
        }
        /// <summary>
        /// 采购查询航班变动列表
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="purchaseId">采购Id</param>
        public static IEnumerable<TransferDetail> QueryTransferDetailByPurchase(Pagination pagination, Guid purchaseId)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInfomationRepository = Factory.CreateTransfeRepository(command);
                return transferInfomationRepository.QueryTransferDetailByPurchase(pagination, purchaseId);
            }
        }
        /// <summary>
        /// 采购查看航班变动信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PurchaseTransferInformation> QueryTransferInformationByPurchase(FlightTransferCondition condition,Pagination pagination)
        {
            using (var command = Factory.CreateCommand())
            {
                var transferInformationRepository = Factory.CreateTransfeRepository(command);
                return transferInformationRepository.QueryTransferInformationByPurchase(condition,pagination);
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        public static void UpdatePassengerMsgSended(Guid transferId)
        {
            using(var command = Factory.CreateCommand()){
                var trangferRepository = Factory.CreateTransfeRepository(command);
                trangferRepository.UpdatePassengerMsgSended(transferId);
            }
        }
    }
}