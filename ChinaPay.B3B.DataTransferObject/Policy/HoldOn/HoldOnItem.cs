using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Policy.HoldOn {
    using B3B.Common.Enums;

    public class HoldOnItem {
        /// <summary>
        /// 航空公司
        /// 二字码
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 客票类型
        /// 如果该项无值，表示所有类型
        /// </summary>
        public TicketType? TicketType { get; set; }
    }
}
