using System;
using System.Data.Common;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Order.Repository.SqlServer {
    abstract class SqlServerTransaction {
        DbOperator _dbOperator = null;

        protected SqlServerTransaction(DbOperator dbOperator) {
            this._dbOperator = dbOperator;
        }
        protected void ExecuteNonQuery(string sql) {
            this._dbOperator.ExecuteNonQuery(sql);
        }
        protected void ExecuteNonQuery(string sql, System.Data.CommandType type) {
            this._dbOperator.ExecuteNonQuery(sql, type);
        }
        protected DbDataReader ExecuteReader(string sql) {
            return this._dbOperator.ExecuteReader(sql);
        }
        protected DbDataReader ExecuteReader(string sql, System.Data.CommandType type) {
            return this._dbOperator.ExecuteReader(sql, type);
        }
        protected object ExecuteScalar(string sql) {
            return this._dbOperator.ExecuteScalar(sql);
        }
        protected void ClearParameters() {
            this._dbOperator.ClearParameters();
        }
        protected DbParameter AddParameter(string name) {
            return this._dbOperator.AddParameter(name);
        }
        protected DbParameter AddParameter(string name, object value) {
            return this._dbOperator.AddParameter(name, value);
        }
    }
}
