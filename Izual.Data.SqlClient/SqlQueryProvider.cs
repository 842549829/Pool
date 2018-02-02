#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.SqlClient.SqlQueryProvider.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Izual.Data.Common;
using Izual.Linq;

namespace Izual.Data.SqlClient {
    public class SqlQueryProvider : DbEntityProvider {
        private bool? allowMulitpleActiveResultSets;

        public SqlQueryProvider(SqlConnection connection, QueryMapping mapping, QueryPolicy policy) : base(connection, TSqlLanguage.Default, mapping, policy) {}
        public SqlQueryProvider(string connectionString) : base(new SqlConnection(connectionString),TSqlLanguage.Default,new DefaultMapping(),QueryPolicy.Default) {}

        public bool AllowsMultipleActiveResultSets {
            get {
                if(allowMulitpleActiveResultSets == null) {
                    var builder = new SqlConnectionStringBuilder(Connection.ConnectionString);
                    object result = builder["MultipleActiveResultSets"];
                    allowMulitpleActiveResultSets = (result != null && result.GetType() == typeof(bool) && (bool)result);
                }
                return (bool)allowMulitpleActiveResultSets;
            }
        }

        public static string GetConnectionString(string databaseFile) {
            return string.Format(@"Data Source=.\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;User Instance=True;MultipleActiveResultSets=true;AttachDbFilename='{0}'", databaseFile);
        }

        protected override QueryExecutor CreateExecutor() {
            return new Executor(this);
        }

        #region Nested type: Executor

        private new class Executor : DbEntityProvider.Executor {
            private readonly SqlQueryProvider provider;

            public Executor(SqlQueryProvider provider) : base(provider) {
                this.provider = provider;
            }

            protected override bool BufferResultRows {
                get { return !provider.AllowsMultipleActiveResultSets; }
            }

            protected override void AddParameter(DbCommand command, QueryParameter parameter, object value) {
                var sqlType = (DbQueryType)parameter.QueryType;
                if(sqlType == null)
                    sqlType = (DbQueryType)Provider.Language.TypeSystem.GetColumnType(parameter.Type);
                int len = sqlType.Length;
                if(len == 0 && DbTypeResolver.IsVariableLength(sqlType.SqlDbType)) {
                    len = Int32.MaxValue;
                }
                SqlParameter p = ((SqlCommand)command).Parameters.Add("@" + parameter.Name, sqlType.SqlDbType, len);
                if(sqlType.Precision != 0)
                    p.Precision = (byte)sqlType.Precision;
                if(sqlType.Scale != 0)
                    p.Scale = (byte)sqlType.Scale;
                p.Value = value ?? DBNull.Value;
            }

            public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream) {
                StartUsingConnection();
                try {
                    IEnumerable<int> result = ExecuteBatch(query, paramSets, batchSize);
                    if(!stream || ActionOpenedConnection) {
                        return result.ToList();
                    }
                    else {
                        return new EnumerateOnce<int>(result);
                    }
                }
                finally {
                    StopUsingConnection();
                }
            }

            private IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize) {
                var cmd = (SqlCommand)GetCommand(query, null);
                var dataTable = new DataTable();
                for(int i = 0, n = query.Parameters.Count; i < n; i++) {
                    QueryParameter qp = query.Parameters[i];
                    cmd.Parameters[i].SourceColumn = qp.Name;
                    dataTable.Columns.Add(qp.Name, TypeHelper.GetNonNullableType(qp.Type));
                }
                var dataAdapter = new SqlDataAdapter();
                dataAdapter.InsertCommand = cmd;
                dataAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                dataAdapter.UpdateBatchSize = batchSize;

                LogMessage("-- Start SQL Batching --");
                LogMessage("");
                LogCommand(query, null);

                IEnumerator<object[]> en = paramSets.GetEnumerator();
                using(en) {
                    bool hasNext = true;
                    while(hasNext) {
                        int count = 0;
                        for(; count < dataAdapter.UpdateBatchSize && (hasNext = en.MoveNext()); count++) {
                            object[] paramValues = en.Current;
                            dataTable.Rows.Add(paramValues);
                            LogParameters(query, paramValues);
                            LogMessage("");
                        }
                        if(count > 0) {
                            int n = dataAdapter.Update(dataTable);
                            for(int i = 0; i < count; i++) {
                                yield return (i < n) ? 1 : 0;
                            }
                            dataTable.Rows.Clear();
                        }
                    }
                }

                LogMessage(string.Format("-- End SQL Batching --"));
                LogMessage("");
            }
        }

        #endregion
    }
}