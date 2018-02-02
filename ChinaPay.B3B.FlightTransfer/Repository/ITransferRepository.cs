using System;
using System.Collections.Generic;
using System.Data;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.FlightTransfer;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.FlightTransfer.Repository
{
    internal interface ITransferRepository
    {
        IEnumerable<TransferInformation> QueryTransferInformation(Pagination pagination);
        TransferInformation QuerySingleTransferInfomation(Guid purchaserId);
        IEnumerable<TransferDetail> QueryTransferDetails(Pagination pagination, Guid purchaserId);
        IEnumerable<InformRecord> QueryInformRecords(InfomrRecordSearchConditoin conditoin, Pagination pagination);

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
        bool InformPurchaser(Guid purchaerId, List<Guid> transferIds, InformType inform, InformResult result, string remark,
            string operatorAccount,string operatorName);

        DataTable QueryInformRecords(InfomrRecordSearchConditoin conditoin);
        FlightTransferStatInfo QueryFlightTransferStatInfo();
        List<TransferDetail> QueryCurrentBatInformation();
        PurchaseFlightTransferInfo QueryPurchaseFlightStaticInfo(Guid purchaserId);
        IEnumerable<TransferDetail> QueryTransferDetailByPurchase(Pagination pagination, Guid purchaseId);
        IEnumerable<PurchaseTransferInformation> QueryTransferInformationByPurchase(FlightTransferCondition condition, Pagination pagination);
        void UpdatePassengerMsgSended(Guid transferId);
    }
}