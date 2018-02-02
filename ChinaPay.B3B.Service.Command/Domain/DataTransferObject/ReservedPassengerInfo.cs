using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 预订后的旅客信息（执行NM指令后）
    /// </summary>
    public class ReservedPassengerInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 旅客类型
        /// </summary>
        public PassengerType Type { get; set; }
    }
}
