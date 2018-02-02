using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class HistoryRepository : SqlServerRepository, IHistoryRepository
    {
        public HistoryRepository(string connectionString)
            : base(connectionString) { }

        public int Insert(ResourceServiceHistory history)
        {
            string sql = @"INSERT INTO ResourceServiceHistories(ThreadId, GenerateTime, SendMessage, ReceiveMessage, AgentId, OperationId, GroupId) " +
                "VALUES(@ThreadId, @GenerateTime, @SendMessage, @ReceiveMessage, @AgentId, @OperationId, @GroupId)";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("ThreadId", history.ThreadId);
                dbOperator.AddParameter("GenerateTime", history.GenerateTime);
                dbOperator.AddParameter("SendMessage", history.SendMessage);
                dbOperator.AddParameter("ReceiveMessage", history.ReceiveMessage);
                dbOperator.AddParameter("AgentId", history.AgentId);
                dbOperator.AddParameter("OperationId", history.OperationId);
                dbOperator.AddParameter("GroupId", history.GroupId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
        
        public int Insert(PNRHistory history)
        {
            string sql = @"insert into PNRHistories(ThreadId, GenerateTime, PNRCode, OfficeNo, Status)" +
                @"values(@ThreadId, @GenerateTime, @PNRCode, @OfficeNo, 1)";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("ThreadId", history.ThreadId);
                dbOperator.AddParameter("GenerateTime", history.GenerateTime);
                dbOperator.AddParameter("PNRCode", history.PNRCode);
                dbOperator.AddParameter("OfficeNo", history.OfficeNo);
                dbOperator.AddParameter("Status", history.Status);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(PNRHistory history)
        {
            string sql = @"update PNRHistories set Status = @Status where ThreadId = @ThreadId and GenerateTime = @GenerateTime";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Status", history.Status);
                dbOperator.AddParameter("ThreadId", history.ThreadId);
                dbOperator.AddParameter("GenerateTime", history.GenerateTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public PNRHistory Query(string pnrHistoryPNRCode)
        {
            PNRHistory pnrHistory = null;
            // 编码有可能重复，提取时间最近的一个；
            string sql = @"select top 1 ThreadId, GenerateTime, PNRCode, OfficeNo, Status from PNRHistories " +
                    @" where PNRCode = @PNRCode order by GenerateTime desc";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("PNRCode", pnrHistoryPNRCode);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        int threadId = reader.GetInt32(0);
                        DateTime generateTime = reader.GetDateTime(1);
                        string pnrCode = reader.GetString(2);
                        string officeNo = reader.GetString(3);
                        byte status = reader.GetByte(4);
                        pnrHistory = new PNRHistory(threadId, generateTime, pnrCode, officeNo, status);
                    }
                }
            }
            return pnrHistory;
        }

        public IEnumerable<PNRHistory> Query()
        {
            List<PNRHistory> pnrHistories = default(List<PNRHistory>);

            string sql = @"select ThreadId, GenerateTime, PNRCode, OfficeNo, Status from PNRHistories ";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    pnrHistories = new List<PNRHistory>();
                    while (reader.Read())
                    {
                        int threadId = reader.GetInt32(0);
                        DateTime generateTime = reader.GetDateTime(1);
                        string pnrCode = reader.GetString(2);
                        string officeNo = reader.GetString(3);
                        byte status = reader.GetByte(4);
                        PNRHistory pnrHistory = new PNRHistory(threadId, generateTime, pnrCode, officeNo, status);
                        pnrHistories.Add(pnrHistory);
                    }
                }
            }
            return pnrHistories;
        }
        
        IEnumerable<PNRHistory> IHistoryRepository.Query(DateTime startTime, DateTime endTime)
        {
            List<PNRHistory> pnrHistories = default(List<PNRHistory>);

            string sql = @"select ThreadId, GenerateTime, PNRCode, OfficeNo, Status " +
                @" from PNRHistories " +
                @" where GenerateTime between @StartTime and @EndTime";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("StartTime", startTime);
                dbOperator.AddParameter("EndTime", endTime);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    pnrHistories = new List<PNRHistory>();
                    while (reader.Read())
                    {
                        int threadId = reader.GetInt32(0);
                        DateTime generateTime = reader.GetDateTime(1);
                        string pnrCode = reader.GetString(2);
                        string officeNo = reader.GetString(3);
                        byte status = reader.GetByte(4);
                        PNRHistory pnrHistory = new PNRHistory(threadId, generateTime, pnrCode, officeNo, status);
                        pnrHistories.Add(pnrHistory);
                    }
                }
            }
            return pnrHistories;
        }        
    }
}
