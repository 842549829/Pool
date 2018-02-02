using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.PNR
{
    class OSICommand : Command
    {
        /// <summary>
        /// 建立其他服务信息组的航信指令
        /// </summary>
        /// <param name="airlineCode">航空公司编码</param>
        /// <param name="passageInformaition">旅客信息</param>
        /// <param name="passengerId">旅客标识</param>
        /// <param name="otherServiceInformationType">服务类型</param>
        public OSICommand(string airlineCode, string passengeInformaition, int passengerId, OtherServiceInformationType otherServiceInformationType)
        {
            this.airlineCode = airlineCode;
            this.otherServiceInformationType = otherServiceInformationType;
            this.passengeInformaition = passengeInformaition;
            this.passengerId = passengerId;
            Initialize();
        }

        /// <summary>
        /// 建立其他服务信息组的航信指令
        /// </summary>
        /// <param name="airlineCode">航空公司编码</param>
        /// <param name="passageInformaition">旅客信息</param>
        /// <param name="passengerId">旅客标识</param>
        /// <param name="otherServiceInformationType">服务类型</param>
        /// <param name="transactionId">事务编号</param>
        public OSICommand(string airlineCode, string passengeInformaition, int passengerId, OtherServiceInformationType otherServiceInformationType, string transactionId)
        {
            this.airlineCode = airlineCode;
            this.otherServiceInformationType = otherServiceInformationType;
            this.passengeInformaition = passengeInformaition;
            this.passengerId = passengerId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRCreation;
            this.commandString = string.Format("OSI: {0} {1} {2}/P{3}",
                airlineCode, Enum.GetName(typeof(OtherServiceInformationType), otherServiceInformationType), passengeInformaition, passengerId);
        }

        // 航空公司编号
        private string airlineCode;
        // 证件号
        private string passengeInformaition;
        // 对应旅客编号
        private int passengerId;
        // 服务类型
        private OtherServiceInformationType otherServiceInformationType;
    }
}
