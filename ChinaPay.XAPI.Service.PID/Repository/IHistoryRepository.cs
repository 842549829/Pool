using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    interface IHistoryRepository
    {
        /// <summary>
        /// 新增命令执行历史记录；
        /// </summary>
        /// <param name="history">旅客订座历史记录</param>
        /// <returns>受影响行数</returns>
        int Insert(ResourceServiceHistory history);

        /// <summary>
        /// 新增旅客订座历史记录；
        /// </summary>
        /// <param name="history">历史记录</param>
        /// <returns>受影响行数</returns>
        int Insert(PNRHistory history);
        
        /// <summary>
        /// 更新旅客订座历史记录；
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        int Update(PNRHistory history);

        /// <summary>
        /// 根据编码查询旅客订座历史记录；
        /// </summary>
        /// <param name="pnrCode">编码</param>
        /// <returns>旅客订座历史记录</returns>
        PNRHistory Query(string pnrCode);

        /// <summary>
        /// 查询全部旅客订座历史记录；
        /// </summary>
        /// <returns>旅客订座历史记录列表</returns>
        IEnumerable<PNRHistory> Query();

        /// <summary>
        /// 查询某段时间内的所有编码；
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>旅客订座历史记录列表</returns>
        IEnumerable<PNRHistory> Query(DateTime startTime, DateTime endTime);
    }
}
