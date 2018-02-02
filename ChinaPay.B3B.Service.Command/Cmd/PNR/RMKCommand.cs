using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 添加备注，现用于授权；
    /// </summary>
    class RMKCommand : Command
    {
        /// <summary>
        /// 添加备注
        /// </summary>
        /// <param name="officeNo">Office编号</param>
        public RMKCommand(string officeNo)
        {
            this.officeNo = officeNo;
            Initialize();
        }

        /// <summary>
        /// 添加备注
        /// </summary>
        /// <param name="officeNo">Office编号</param>
        /// <param name="transactionId">事务编号</param>
        public RMKCommand(string officeNo, string transactionId)
        {
            this.officeNo = officeNo;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRCreation;
            this.commandString = string.Format("RMK:TJ AUTH {0}", this.officeNo);
        }
        
        // Office编号；
        private string officeNo;
    }
}
