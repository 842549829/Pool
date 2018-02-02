using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 订座封口指令
    /// </summary>
    public class EOTCommand: Command
    {
        /// <summary>
        /// 封口指令
        /// </summary>
        public EOTCommand()
        {
            this.commandType = CommandType.PNRCreation;
            Initialize();
	    }

        public EOTCommand(string transactionId)
        {
            this.transactionId = transactionId;
            this.commandType = CommandType.PNRCreation;
            Initialize();
        }

        private void Initialize()
        {
            this.commandString = string.Format("@");
        }
    }
}