using System;
using System.Globalization;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    /// <summary>
    /// 建立票号组的航信指令
    /// </summary>
    public class TkCommand: Command
    {
        /// <summary>
        /// 建立票号组的航信指令
        /// </summary>
        /// <param name="issueDatetime">出票时限</param>
        /// <param name="officeNo">代理人编号</param>
        public TkCommand(DateTime issueDatetime, string officeNo)
        {
            _issueDatetime = issueDatetime;
            _officeNo = officeNo;
            Initialize();
        }

        private void Initialize()
        {
            CommandString = string.Format("TK:{0}/{1}/{2}/{3}",
                "TL",
                _issueDatetime.ToString("HHmm"),
                _issueDatetime.ToString("ddMMM", CultureInfo.CreateSpecificCulture("en-US")),
                _officeNo
            );
        }
        
        // 出票时限
        private DateTime _issueDatetime;
        // 代理人编号
        private readonly string _officeNo;
    }
}
