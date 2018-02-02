using System;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    class OsiCommand : Command
    {
        /// <summary>
        /// 建立其他服务信息组的航信指令
        /// </summary>
        /// <param name="airlineCode">航空公司编码</param>
        /// <param name="passageInformaition">旅客信息</param>
        /// <param name="passengerId">旅客标识</param>
        /// <param name="otherServiceInformationType">服务类型</param>
        public OsiCommand(string airlineCode, string passengeInformaition, int passengerId, OtherServiceInformationType otherServiceInformationType)
        {
            this.airlineCode = airlineCode;
            this.otherServiceInformationType = otherServiceInformationType;
            this.passengeInformaition = passengeInformaition;
            this.passengerId = passengerId;
            Initialize();
        }

        private void Initialize()
        {
            // 暂时只有CTCT的操作，其他的格式暂时没有写进来；
            if (airlineCode == "MU" || airlineCode == "FM" || airlineCode == "KN" || airlineCode == "KY")
            {
                CommandString = string.Format("OSI: {0} {1}{2}",
                                              airlineCode,
                                              Enum.GetName(typeof (OtherServiceInformationType),
                                                           otherServiceInformationType), passengeInformaition);
            }
            else
            {
                CommandString = string.Format("OSI: {0} {1} {2}/P{3}",
                                                  airlineCode,
                                                  Enum.GetName(typeof (OtherServiceInformationType),
                                                               otherServiceInformationType), passengeInformaition,
                                                  passengerId);
            }
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
