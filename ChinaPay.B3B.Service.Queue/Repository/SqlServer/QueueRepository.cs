using System;
using System.Data;
using System.Data.SqlClient;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Queue.Repository.SqlServer
{
    class QueueRepository : SqlServerRepository, IQueueRepository
    {
        public QueueRepository(string connectionString) : base(connectionString){}
        
        public int Add(Domain.Queue queue)
        {
            const string sql = @"INSERT INTO History.Queues (ProcessTime, InternalNumber, Content, Type, OfficeNumber)" +
                               @"VALUES(@ProcessTime, @InternalNumber, @Content, @Type, @OfficeNumber)";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("ProcessTime", queue.ProcessTime);
                dbOperator.AddParameter("InternalNumber", queue.InternalNumber);
                dbOperator.AddParameter("Content", queue.Content);
                dbOperator.AddParameter("Type", queue.Type);
                dbOperator.AddParameter("OfficeNumber", queue.OfficeNumber);

                return dbOperator.ExecuteNonQuery(sql);
            }
        }


        private DataTable GetTableSchema()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[]
                                           {
                                               new DataColumn("ProcessTime"),
                                               new DataColumn("InternalNumber"),
                                               new DataColumn("Content"),
                                               new DataColumn("Type"),
                                               new DataColumn("OfficeNumber"),
                                           });
            return dataTable;
        }

        public int Add(System.Collections.Generic.List<Domain.Queue> queues)
        {
            int rowCount = 0;
            var dataTable = GetTableSchema();

            foreach (var queue in queues)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = queue.ProcessTime;
                dataRow[1] = queue.InternalNumber;
                dataRow[2] = queue.Content;
                dataRow[3] = queue.Type;
                dataRow[4] = queue.OfficeNumber;
                dataTable.Rows.Add(dataRow);
            }
            
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlBulkCopy = new SqlBulkCopy(sqlConnection))
                {
                    sqlBulkCopy.DestinationTableName = "History.Queues";
                    sqlBulkCopy.BatchSize = dataTable.Rows.Count;
                    if (sqlBulkCopy.BatchSize != 0)
                    {
                        sqlBulkCopy.WriteToServer(dataTable);
                        rowCount = sqlBulkCopy.BatchSize;
                    }
                }
            }

            return rowCount;
        }
    }
}
