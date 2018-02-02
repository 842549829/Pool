using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 提取机票价格的航信指令。
    /// </summary>
    public class PATCommad : Command
    {
        /// <summary>
        /// 提取机票价格的航信指令
        /// </summary>
        public PATCommad(PassengerType passengerType)
        {
            this.passengerType = passengerType;
            Initialize();
        }
        
        /// <summary>
        /// 提取机票价格的航信指令
        /// </summary>
        /// <param name="transactionId"></param>
        public PATCommad(PassengerType passengerType, string transactionId)
        {
            this.passengerType = passengerType;
            this.commandType = CommandType.PNRExtraction;
            this.transactionId = transactionId;
            Initialize();
        }
        
        private void Initialize()
        {
            switch (passengerType)
            {
                case PassengerType.Adult:
                    this.commandString = "PAT:A";
                    break;
                case PassengerType.Child:
                    this.commandString = "PAT:A*CH";
                    break;
                default:
                    break;
            }            
        }

        private PassengerType passengerType;
    }
}
