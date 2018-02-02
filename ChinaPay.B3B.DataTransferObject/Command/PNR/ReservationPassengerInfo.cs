using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    /// <summary>
    /// 旅客预订时的个人信息
    /// </summary>
    public class ReservationPassengerInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 旅客类型
        /// </summary>
        public PassengerType Type { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string CertificateNumber { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public CredentialsType CertificateType { get; set; }

        /// <summary>
        /// 移动电话号码
        /// </summary>
        public string MobilephoneNumber { get; set; }

        /// <summary>
        /// 生日（用于南航儿童票）
        /// </summary>
        public DateTime? Birthday { get; set; }
    }
}
