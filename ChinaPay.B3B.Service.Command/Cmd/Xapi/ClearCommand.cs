using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Command;

namespace ChinaPay.B3B.Service.Command.Xapi
{
    class ClearCommand : Command
    {
        public ClearCommand()
        {
            this.commandType = CommandType.XapiOperation;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandString = "CLR";
        }
    }
}
