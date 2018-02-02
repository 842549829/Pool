namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 预订后的旅客联系信息
    /// </summary>
    public class ReservedContractInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 乘客编号
        /// </summary>
        public int PassengerId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Contract { get; set; }
    }
}
