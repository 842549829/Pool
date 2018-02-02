#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.DbEntityProvider.cs
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Izual.Data.Common;
using Izual.Linq;

namespace Izual.Data {
    public class DbEntityProvider : EntryProvider {
        private readonly DbConnection connection;
        private bool actionOpenedConnection;
        private IsolationLevel isolation = IsolationLevel.ReadCommitted;

        private int nConnectedActions;
        private DbTransaction transaction;

        public DbEntityProvider(DbConnection connection, QueryLanguage language, QueryMapping mapping, QueryPolicy policy) : base(language, mapping, policy) {
            if(connection == null)
                throw new InvalidOperationException("Connection not specified");
            this.connection = connection;
        }

        public virtual DbConnection Connection {
            get { return connection; }
        }

        public virtual DbTransaction Transaction {
            get { return transaction; }
            set {
                if(value != null && value.Connection != connection)
                    throw new InvalidOperationException("Transaction does not match connection.");
                transaction = value;
            }
        }

        public IsolationLevel Isolation {
            get { return isolation; }
            set { isolation = value; }
        }

        protected bool ActionOpenedConnection {
            get { return actionOpenedConnection; }
        }

        public static DbEntityProvider From(string connectionString, string mappingId) {
            return From(connectionString, mappingId, QueryPolicy.Default);
        }

        public static DbEntityProvider From(string connectionString, string mappingId, QueryPolicy policy) {
            return From(null, connectionString, mappingId, policy);
        }

        public static DbEntityProvider From(string connectionString, QueryMapping mapping, QueryPolicy policy) {
            return From((string)null, connectionString, mapping, policy);
        }

        public static DbEntityProvider From(string provider, string connectionString, string mappingId) {
            return From(provider, connectionString, mappingId, QueryPolicy.Default);
        }

        public static DbEntityProvider From(string provider, string connectionString, string mappingId, QueryPolicy policy) {
            return From(provider, connectionString, GetMapping(mappingId), policy);
        }

        public static DbEntityProvider From(string provider, string connectionString, QueryMapping mapping, QueryPolicy policy) {
            if(provider == null) {
                string clower = connectionString.ToLower();
                // try sniffing connection to figure out provider
                if(clower.Contains(".mdb") || clower.Contains(".accdb")) {
                    provider = "Izual.Data.Access";
                }
                else if(clower.Contains(".sdf")) {
                    provider = "Izual.Data.SqlServerCe";
                }
                else if(clower.Contains(".sl3") || clower.Contains(".db3")) {
                    provider = "Izual.Data.SQLite";
                }
                else if(clower.Contains(".mdf")) {
                    provider = "Izual.Data.SqlClient";
                }
                else {
                    throw new InvalidOperationException(string.Format("Query provider not specified and cannot be inferred."));
                }
            }

            Type providerType = GetProviderType(provider);
            if(providerType == null)
                throw new InvalidOperationException(string.Format("Unable to find query provider '{0}'", provider));

            return From(providerType, connectionString, mapping, policy);
        }

        public static DbEntityProvider From(Type providerType, string connectionString, QueryMapping mapping, QueryPolicy policy) {
            Type adoConnectionType = GetAdoConnectionType(providerType);
            if(adoConnectionType == null)
                throw new InvalidOperationException(string.Format("Unable to deduce ADO provider for '{0}'", providerType.Name));
            var connection = (DbConnection)Activator.CreateInstance(adoConnectionType);

            // is the connection string just a filename?
            if(!connectionString.Contains('=')) {
                MethodInfo gcs = providerType.GetMethod("GetConnectionString", BindingFlags.Static | BindingFlags.Public, null, new[] {typeof(string)}, null);
                if(gcs != null) {
                    connectionString = (string)gcs.Invoke(null, new object[] {connectionString});
                }
            }

            connection.ConnectionString = connectionString;

            return (DbEntityProvider)Activator.CreateInstance(providerType, new object[] {connection, mapping, policy});
        }

        private static Type GetAdoConnectionType(Type providerType) {
            // sniff constructors 
            foreach(ConstructorInfo con in providerType.GetConstructors()) {
                foreach(ParameterInfo arg in con.GetParameters()) {
                    if(arg.ParameterType.IsSubclassOf(typeof(DbConnection)))
                        return arg.ParameterType;
                }
            }
            return null;
        }

        protected void StartUsingConnection() {
            if(connection.State == ConnectionState.Closed) {
                connection.Open();
                actionOpenedConnection = true;
            }
            nConnectedActions++;
        }

        protected void StopUsingConnection() {
            //Debug.Assert(nConnectedActions > 0);
            nConnectedActions--;
            if(nConnectedActions == 0 && actionOpenedConnection) {
                connection.Close();
                actionOpenedConnection = false;
            }
        }

        public override void DoConnected(Action action) {
            StartUsingConnection();
            try {
                action();
            }
            finally {
                StopUsingConnection();
            }
        }

        public override void DoTransacted(Action action) {
            StartUsingConnection();
            try {
                if(Transaction == null) {
                    DbTransaction trans = Connection.BeginTransaction(Isolation);
                    try {
                        Transaction = trans;
                        action();
                        trans.Commit();
                    }
                    finally {
                        Transaction = null;
                        trans.Dispose();
                    }
                }
                else {
                    action();
                }
            }
            finally {
                StopUsingConnection();
            }
        }

        public override int ExecuteCommand(string commandText) {
            if(Log != null) {
                Log.WriteLine(commandText);
            }
            StartUsingConnection();
            try {
                DbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = commandText;
                return cmd.ExecuteNonQuery();
            }
            finally {
                StopUsingConnection();
            }
        }

        protected override QueryExecutor CreateExecutor() {
            return new Executor(this);
        }

        #region Nested type: DbFieldReader

        protected class DbFieldReader : FieldReader {
            private readonly QueryExecutor executor;
            private readonly DbDataReader reader;

            public DbFieldReader(QueryExecutor executor, DbDataReader reader) {
                this.executor = executor;
                this.reader = reader;
                Init();
            }

            protected override int FieldCount {
                get { return reader.FieldCount; }
            }

            protected override Type GetFieldType(int ordinal) {
                return reader.GetFieldType(ordinal);
            }

            protected override bool IsDBNull(int ordinal) {
                return reader.IsDBNull(ordinal);
            }

            protected override T GetValue<T>(int ordinal) {
                return (T)executor.Convert(reader.GetValue(ordinal), typeof(T));
            }

            protected override Byte GetByte(int ordinal) {
                return reader.GetByte(ordinal);
            }

            protected override Char GetChar(int ordinal) {
                return reader.GetChar(ordinal);
            }

            protected override DateTime GetDateTime(int ordinal) {
                return reader.GetDateTime(ordinal);
            }

            protected override Decimal GetDecimal(int ordinal) {
                return reader.GetDecimal(ordinal);
            }

            protected override Double GetDouble(int ordinal) {
                return reader.GetDouble(ordinal);
            }

            protected override Single GetSingle(int ordinal) {
                return reader.GetFloat(ordinal);
            }

            protected override Guid GetGuid(int ordinal) {
                return reader.GetGuid(ordinal);
            }

            protected override Int16 GetInt16(int ordinal) {
                return reader.GetInt16(ordinal);
            }

            protected override Int32 GetInt32(int ordinal) {
                return reader.GetInt32(ordinal);
            }

            protected override Int64 GetInt64(int ordinal) {
                return reader.GetInt64(ordinal);
            }

            protected override String GetString(int ordinal) {
                return reader.GetString(ordinal);
            }
        }

        #endregion

        #region Nested type: Executor

        public class Executor : QueryExecutor {
            private readonly DbEntityProvider provider;
            private int rowsAffected;

            public Executor(DbEntityProvider provider) {
                this.provider = provider;
            }

            public DbEntityProvider Provider {
                get { return provider; }
            }

            public override int RowsAffected {
                get { return rowsAffected; }
            }

            protected virtual bool BufferResultRows {
                get { return false; }
            }

            protected bool ActionOpenedConnection {
                get { return provider.actionOpenedConnection; }
            }

            protected void StartUsingConnection() {
                provider.StartUsingConnection();
            }

            protected void StopUsingConnection() {
                provider.StopUsingConnection();
            }

            public override object Convert(object value, Type type) {
                if(value == null) {
                    return TypeHelper.GetDefault(type);
                }
                type = TypeHelper.GetNonNullableType(type);
                if (type == typeof(Time)) {
                    return Time.Parse(value.ToString());
                }
                if(type == typeof(Time2)) {
                    return Time2.Parse(value.ToString());
                }
                Type vtype = value.GetType();
                if(type != vtype) {
                    if(type.IsEnum) {
                        if(vtype == typeof(string)) {
                            return Enum.Parse(type, (string)value);
                        }
                        else {
                            Type utype = Enum.GetUnderlyingType(type);
                            if(utype != vtype) {
                                value = System.Convert.ChangeType(value, utype);
                            }
                            return Enum.ToObject(type, value);
                        }
                    }
                    return System.Convert.ChangeType(value, type);
                }
                return value;
            }

            public override IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, EntryMapping entity, object[] paramValues) {
                LogCommand(command, paramValues);
                StartUsingConnection();
                try {
                    DbCommand cmd = GetCommand(command, paramValues);
                    DbDataReader reader = ExecuteReader(cmd);
                    IEnumerable<T> result = Project(reader, fnProjector, entity, true);
                    if(provider.ActionOpenedConnection) {
                        result = result.ToList();
                    }
                    else {
                        result = new EnumerateOnce<T>(result);
                    }
                    return result;
                }
                finally {
                    StopUsingConnection();
                }
            }

            protected virtual DbDataReader ExecuteReader(DbCommand command) {
                DbDataReader reader = command.ExecuteReader();
                if(BufferResultRows) {
                    // use data table to buffer results
                    var ds = new DataSet();
                    ds.EnforceConstraints = false;
                    var table = new DataTable();
                    ds.Tables.Add(table);
                    ds.EnforceConstraints = false;
                    table.Load(reader);
                    reader = table.CreateDataReader();
                }
                return reader;
            }

            protected virtual IEnumerable<T> Project<T>(DbDataReader reader, Func<FieldReader, T> fnProjector, EntryMapping entity, bool closeReader) {
                var freader = new DbFieldReader(this, reader);
                try {
                    while(reader.Read()) {
                        yield return fnProjector(freader);
                    }
                }
                finally {
                    if(closeReader) {
                        reader.Close();
                    }
                }
            }

            public override int ExecuteCommand(QueryCommand query, object[] paramValues) {
                LogCommand(query, paramValues);
                StartUsingConnection();
                try {
                    DbCommand cmd = GetCommand(query, paramValues);
                    rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
                finally {
                    StopUsingConnection();
                }
            }

            public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream) {
                StartUsingConnection();
                try {
                    IEnumerable<int> result = ExecuteBatch(query, paramSets);
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

            private IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets) {
                LogCommand(query, null);
                DbCommand cmd = GetCommand(query, null);
                foreach(object[] paramValues in paramSets) {
                    LogParameters(query, paramValues);
                    LogMessage("");
                    SetParameterValues(query, cmd, paramValues);
                    rowsAffected = cmd.ExecuteNonQuery();
                    yield return rowsAffected;
                }
            }

            public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, EntryMapping entity, int batchSize, bool stream) {
                StartUsingConnection();
                try {
                    IEnumerable<T> result = ExecuteBatch(query, paramSets, fnProjector, entity);
                    if(!stream || ActionOpenedConnection) {
                        return result.ToList();
                    }
                    else {
                        return new EnumerateOnce<T>(result);
                    }
                }
                finally {
                    StopUsingConnection();
                }
            }

            private IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, EntryMapping entity) {
                LogCommand(query, null);
                DbCommand cmd = GetCommand(query, null);
                cmd.Prepare();
                foreach(object[] paramValues in paramSets) {
                    LogParameters(query, paramValues);
                    LogMessage("");
                    SetParameterValues(query, cmd, paramValues);
                    DbDataReader reader = ExecuteReader(cmd);
                    var freader = new DbFieldReader(this, reader);
                    try {
                        if(reader.HasRows) {
                            reader.Read();
                            yield return fnProjector(freader);
                        }
                        else {
                            yield return default(T);
                        }
                    }
                    finally {
                        reader.Close();
                    }
                }
            }

            public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, EntryMapping entity, object[] paramValues) {
                LogCommand(query, paramValues);
                StartUsingConnection();
                try {
                    DbCommand cmd = GetCommand(query, paramValues);
                    DbDataReader reader = ExecuteReader(cmd);
                    var freader = new DbFieldReader(this, reader);
                    try {
                        while(reader.Read()) {
                            yield return fnProjector(freader);
                        }
                    }
                    finally {
                        reader.Close();
                    }
                }
                finally {
                    StopUsingConnection();
                }
            }

            /// <summary>
            /// Get an ADO command object initialized with the command-text and parameters
            /// </summary>
            /// <param name="commandText"> </param>
            /// <param name="paramNames"> </param>
            /// <param name="paramValues"> </param>
            /// <returns> </returns>
            protected virtual DbCommand GetCommand(QueryCommand query, object[] paramValues) {
                // create command object (and fill in parameters)
                DbCommand cmd = provider.Connection.CreateCommand();
                cmd.CommandText = query.CommandText;
                if(provider.Transaction != null)
                    cmd.Transaction = provider.Transaction;
                SetParameterValues(query, cmd, paramValues);
                return cmd;
            }

            protected virtual void SetParameterValues(QueryCommand query, DbCommand command, object[] paramValues) {
                if(query.Parameters.Count > 0 && command.Parameters.Count == 0) {
                    for(int i = 0, n = query.Parameters.Count; i < n; i++) {
                        AddParameter(command, query.Parameters[i], paramValues != null ? paramValues[i] : null);
                    }
                }
                else if(paramValues != null) {
                    for(int i = 0, n = command.Parameters.Count; i < n; i++) {
                        DbParameter p = command.Parameters[i];
                        if(p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput) {
                            p.Value = paramValues[i] ?? DBNull.Value;
                        }
                    }
                }
            }

            protected virtual void AddParameter(DbCommand command, QueryParameter parameter, object value) {
                DbParameter p = command.CreateParameter();
                p.ParameterName = parameter.Name;
                p.Value = value ?? DBNull.Value;
                command.Parameters.Add(p);
            }

            protected virtual void GetParameterValues(DbCommand command, object[] paramValues) {
                if(paramValues != null) {
                    for(int i = 0, n = command.Parameters.Count; i < n; i++) {
                        if(command.Parameters[i].Direction != ParameterDirection.Input) {
                            object value = command.Parameters[i].Value;
                            if(value == DBNull.Value)
                                value = null;
                            paramValues[i] = value;
                        }
                    }
                }
            }

            protected virtual void LogMessage(string message) {
                if(provider.Log != null) {
                    provider.Log.WriteLine(message);
                }
            }

            /// <summary>
            /// Write a command & parameters to the log
            /// </summary>
            /// <param name="command"> </param>
            /// <param name="paramValues"> </param>
            protected virtual void LogCommand(QueryCommand command, object[] paramValues) {
                if(provider.Log != null) {
                    provider.Log.WriteLine(command.CommandText);
                    if(paramValues != null) {
                        LogParameters(command, paramValues);
                    }
                    provider.Log.WriteLine();
                }
            }

            protected virtual void LogParameters(QueryCommand command, object[] paramValues) {
                if(provider.Log != null && paramValues != null) {
                    for(int i = 0, n = command.Parameters.Count; i < n; i++) {
                        QueryParameter p = command.Parameters[i];
                        object v = paramValues[i];

                        if(v == null || v == DBNull.Value) {
                            provider.Log.WriteLine("-- {0} = NULL", p.Name);
                        }
                        else {
                            provider.Log.WriteLine("-- {0} = [{1}]", p.Name, v);
                        }
                    }
                }
            }
        }

        #endregion
    }
}