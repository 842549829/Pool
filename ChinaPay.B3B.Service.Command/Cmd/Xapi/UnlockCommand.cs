using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Xapi
{
    public class UnlockCommand : Command
    {
        public UnlockCommand(CommandType commandType, string transactionId)
        {
            this.commandType = commandType;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandString = "XLOCK";
        }
    }
}
