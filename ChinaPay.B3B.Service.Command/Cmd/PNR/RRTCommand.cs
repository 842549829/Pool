using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ChinaPay.B3B.Service.Command.PNR
{
    public class RRTCommand : Command
    {
        public RRTCommand(string transactionId)
        {
            this.transactionId = transactionId;
            this.commandString = string.Format("RRT:OK");
            Initialize();
        }
        
        public RRTCommand(string pnrCode, string transactionId)
        {
            this.transactionId = transactionId;
            this.pnrCode = pnrCode;
            this.commandString = string.Format("RRT:{0}/{1}/{2}/{3}", "V", pnrCode, "0000", ".");
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRCreation;
        }

        // 大编码
        private string pnrCode;
        //// 航班号
        //private string flightNumber;
        //// 飞行日期
        //private DateTime flightDate;
    }
}
