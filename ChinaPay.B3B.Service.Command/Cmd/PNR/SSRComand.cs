using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 建立特殊服务组的航信指令
    /// </summary>
    public class SSRCommand : Command
    {
        /// <summary>
        /// 建立特殊服务组的航信指令；
        /// </summary>
        /// <param name="airlineCode">航空公司编码</param>
        /// <param name="certificateNumber">证件号</param>
        /// <param name="passengerId">旅客编号</param>
        public SSRCommand(string airlineCode, string certificateNumber, int passengerId)
        {
            this.airlineCode = airlineCode;
            this.certificateNumber = certificateNumber;
            this.passengerId = passengerId;
            Initialize();
        }

        /// <summary>
        /// 建立特殊服务组的航信指令；
        /// </summary>
        /// <param name="airlineCode">航空公司编码</param>
        /// <param name="certificateNumber">证件号</param>
        /// <param name="passengerId">对应旅客编号</param>
        /// <param name="transactionId">事务编号</param>
        public SSRCommand(string airlineCode, string certificateNumber, int passengerId, string transactionId)
        {
            this.airlineCode = airlineCode;
            this.certificateNumber = certificateNumber;
            this.passengerId = passengerId;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRCreation;
            this.commandString = string.Format("SSR: {0} {1} HK/NI{2}/P{3}",
                "FOID", airlineCode, certificateNumber, passengerId);
        }
        
        // 航空公司编号
        private string airlineCode;
        // 证件号
        private string certificateNumber;
        // 对应旅客编号
        private int passengerId;
    }
}
