using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 建立票号组的航信指令
    /// </summary>
    public class TKCommand: Command
    {
        /// <summary>
        /// 建立票号组的航信指令
        /// </summary>
        /// <param name="issueDatetime">出票时间</param>
        public TKCommand(DateTime issueDatetime)
        {
            this.commandType = CommandType.PNRCreation;
            this.issueDatetime = issueDatetime;
            Initialize();
        }

        /// <summary>
        /// 建立票号组的航信指令
        /// </summary>
        /// <param name="issueDatetime">出票时限</param>
        /// <param name="transactionId">事务编号</param>
        public TKCommand(DateTime issueDatetime, string transactionId)
        {
            this.issueDatetime = issueDatetime;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandString = string.Format("TK:{0}/{1}/{2}/{3}",
                "TL",
                issueDatetime.ToString("HHmm"),
                issueDatetime.ToString("ddMMM", CultureInfo.CreateSpecificCulture("en-US")),
                "OfficeNo"
            );
        }
        
        // 出票时限
        private DateTime issueDatetime;
    }
}
