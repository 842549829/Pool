using ChinaPay.B3B.Common.Enums;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Command.Domain.PNR
{
    /// <summary>
    /// 旅客
    /// </summary>
    public class Passenger
    {
        public Passenger()
        {
            
        }

        public Passenger(string name, PassengerType type, string certificateNumber, CredentialsType certificateType, string mobilephone)
        {
            Name = name;
            Type = type;
            CertificateNumber = certificateNumber;
            CertificateType = certificateType;
            Mobilephone = mobilephone;
        }

        public Passenger(string name, PassengerType type, string certificateNumber, CredentialsType certificateType, string mobilephone, List<string> ticketNumbers)
        {
            Name = name;
            Type = type;
            CertificateNumber = certificateNumber;
            CertificateType = certificateType;
            Mobilephone = mobilephone;
            TicketNumbers = ticketNumbers;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public CredentialsType CertificateType { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        public string CertificateNumber { get; set; }

        /// <summary>
        /// 旅客类型
        /// </summary>
        public PassengerType Type { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobilephone { get; set; }

        /// <summary>
        /// 机票编号
        /// </summary>
        public List<string> TicketNumbers { get; set; }
    }
}
