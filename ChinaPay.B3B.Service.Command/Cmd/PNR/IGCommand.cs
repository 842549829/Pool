using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.PNR
{
    class IGCommand : Command
    {
        /// <summary>
        /// 取消指令；
        /// </summary>
        public IGCommand()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRCreation;
            this.commandString = "IG";
        }
    }
}
