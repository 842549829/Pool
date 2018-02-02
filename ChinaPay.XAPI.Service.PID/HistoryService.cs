using System;
using System.Collections.Generic;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid.Repository;

namespace ChinaPay.XAPI.Service.Pid
{
    public static class HistoryService
    {
        public static int Save(PNRHistory pnrHistory)
        {
            var repository = Factory.CreateHistoryRepository();
            return repository.Insert(pnrHistory);
        }

        public static PNRHistory Query(string pnrCode)
        {
            var repository = Factory.CreateHistoryRepository();
            return repository.Query(pnrCode);
        }

        public static int Update(PNRHistory history)
        {
            IHistoryRepository repository = Factory.CreateHistoryRepository();
            return repository.Update(history);
        }

        /// <summary>
        /// 查询从给定日期起的未取消的订座记录；
        /// </summary>
        /// <param name="startTime">起始日期</param>
        /// <returns>订座历史记录</returns>
        public static IEnumerable<PNRHistory> QueryBooking(DateTime startTime)
        {
            IHistoryRepository repository = Factory.CreateHistoryRepository();
            return repository.Query(startTime, DateTime.Now);
        }

        public static IEnumerable<PNRHistory> QueryAll()
        {
            IHistoryRepository repository = Factory.CreateHistoryRepository();
            return repository.Query();
        }
    }
}