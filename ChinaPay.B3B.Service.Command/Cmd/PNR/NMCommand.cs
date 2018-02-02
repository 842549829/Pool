using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 建立姓名组的航信指令。
    /// </summary>
    public class NMCommand : Command
    {
        /// <summary>
        /// 建立姓名组的航信指令
        /// </summary>
        /// <param name="names">旅客名称列表</param>
        /// <param name="passengerType">旅客类型</param>
        public NMCommand(List<string> names, PassengerType passengerType)
        {
            this.names = names;
            this.passengerType = passengerType;
            Initialize();
        }

        /// <summary>
        /// 建立姓名组的航信指令
        /// </summary>
        /// <param name="names">旅客名称列表</param>
        /// <param name="passengerType">旅客类型</param>
        /// <param name="transactionId"></param>
        public NMCommand(List<string> names, PassengerType passengerType, string transactionId)
        {
            this.names = names;
            this.passengerType = passengerType;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRCreation;
            StringBuilder cmdStr = new StringBuilder("NM:");
            foreach (string name in names)
            {
                if (passengerType == PassengerType.Child)
                {
                    //如果是中文，直接chd，否则空格；
                    Regex pattern = new Regex("^[\u4e00-\u9fa5]{2,}$");

                    if (pattern.Match(name).Success)
                    {
                        cmdStr.AppendFormat("{0} {1}CHD", 1, name);
                    }
                    else
                    {
                        cmdStr.AppendFormat("{0} {1} CHD", 1, name);
                    }
                }
                else
                {
                    cmdStr.AppendFormat("{0}{1} ", 1, name);
                }
            }
            this.commandString = cmdStr.ToString();
        }

        // 旅客姓名列表；
        private List<string> names;
        // 旅客类型；
        private PassengerType passengerType;
    }
}
