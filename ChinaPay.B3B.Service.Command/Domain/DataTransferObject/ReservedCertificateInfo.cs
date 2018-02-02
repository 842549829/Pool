using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 预订后的旅客证件信息
    /// </summary>
    public class ReservedCertificateInfo
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
        /// 证件号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public CredentialsType CertificateType { get; set; }
    }
}
