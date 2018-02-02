using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command
{
    /// <summary>
    /// 命令类型
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// XAPI操作
        /// </summary>
        XapiOperation = 0,
        /// <summary>
        /// 系统操作
        /// </summary>
        SystemOperation = 1,
        /// <summary>
        /// 
        /// </summary>
        Accessibility = 2,
        /// <summary>
        /// 航班查询
        /// </summary>
        FlightQuery = 3,
        /// <summary>
        /// PNR创建
        /// </summary>
        PNRCreation = 4,
        /// <summary>
        /// PNR提取
        /// </summary>
        PNRExtraction = 5,
        /// <summary>
        /// PNR修改
        /// </summary>
        PNRModification = 6,
        /// <summary>
        /// 运价查询
        /// </summary>
        FreightQuery = 7,
        /// <summary>
        /// 电子客票
        /// </summary>
        ETicket = 8,
        /// <summary>
        /// 信箱处理
        /// </summary>
        Queue = 9
    }
}
