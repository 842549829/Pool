using System;

namespace ChinaPay.B3B.DataTransferObject.Order
{
    /// <summary>
    /// 代扣查询条件类
    /// </summary>
    public class AutoPayCondition
    {
        public decimal? OrderId { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public ChinaPay.B3B.Common.Enums.WithholdingAccountType? PayType { get; set; }
        public bool? isSuccess { get; set; }
        public bool? isProcess { get; set; }
    }
}
