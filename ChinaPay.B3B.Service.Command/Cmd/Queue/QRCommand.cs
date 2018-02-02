using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Queue
{
    /// <summary>
    /// 重新显示当前邮箱的航信指令
    /// </summary>
    class QRCommand : Command
    {
        public QRCommand()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.Queue;
            this.commandString = string.Format("QR:");
        }
    }
}
