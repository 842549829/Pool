using System;
using System.Collections.Generic;
using System.Data.Common;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.SystemSetting.Repository.SqlServer
{
    class SuggestRepository : SqlServerTransaction, ISuggestRepository
    {
        public SuggestRepository(DbOperator dbOperator)
            : base(dbOperator)
        {
        }

        /// <summary>
        /// 添加用户建议
        /// </summary>
        /// <param name="suggest"></param>
        /// <returns></returns>
        public bool Insert(Suggest suggest)
        {
            var sqlCommand = "INSERT INTO [T_Suggest] ([Id] ,[SuggestCategory] ,[ContractInformation] ,[SuggestContent] ,[CreateTime] ,[Readed] ,[Handled] ,[Creator] ,[CreatorName])     VALUES (@id ,@SuggestCategory ,@ContractInformation ,@SuggestContent ,@CreateTime ,@Readed ,@Handled ,@Creator ,@CreatorName)";
            AddParameter("Id", suggest.Id);
            AddParameter("SuggestCategory", (int)suggest.SuggestCategory);
            AddParameter("ContractInformation", suggest.ContractInformation);
            AddParameter("SuggestContent", suggest.SuggestContent);
            AddParameter("CreateTime", suggest.CreateTime);
            AddParameter("Readed", suggest.Readed);
            AddParameter("Handled", suggest.Handled);
            AddParameter("Creator", suggest.Creator);
            AddParameter("CreatorName", suggest.CreatorName);
            ExecuteNonQuery(sqlCommand);
            return true;
        }

        public IEnumerable<Suggest> Query(DateTime? start, DateTime? end, SuggestCategory? category, Pagination pagination)
        {
            if (pagination == null)
                throw new ArgumentNullException("pagination");
            ClearParameters();
            IEnumerable<Suggest> result = null;
            if (start.HasValue)
            {
                AddParameter("@Start", start.Value);
            }
            else
            {
                AddParameter("@Start", DBNull.Value);
            }
            if (end.HasValue)
            {
                AddParameter("@End", end.Value);
            }
            else
            {
                AddParameter("@End", DBNull.Value);
            }
            if (category.HasValue)
            {
                AddParameter("@Category", category.Value);
            }
            else
            {
                AddParameter("@Category", DBNull.Value);
            }
            var totalCount = AddParameter("@TotalCount");
            AddParameter("@iPagesize", pagination.PageSize);
            AddParameter("@iPageIndex", pagination.PageIndex);
            AddParameter("@iGetCount", pagination.GetRowCount);
            totalCount.DbType = System.Data.DbType.Int32;
            totalCount.Direction = System.Data.ParameterDirection.Output;
            using (var reader = ExecuteReader("dbo.P_QuerySuggest", System.Data.CommandType.StoredProcedure))
            {
                result = ParseSuggest(reader);
            }
            if (pagination.GetRowCount)
            {
                pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }

        private IEnumerable<Suggest> ParseSuggest(DbDataReader reader)
        {
            var result = new List<Suggest>();
            while (reader.Read())
            {
                var suggest = new Suggest();
                suggest.Id = reader.GetGuid(0);
                suggest.SuggestCategory = (SuggestCategory)reader.GetInt32(1);
                suggest.ContractInformation = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                suggest.SuggestContent = reader.GetString(3);
                suggest.CreateTime = reader.GetDateTime(4);
                suggest.Readed = reader.GetBoolean(5);
                suggest.Handled = reader.GetBoolean(6);
                suggest.Creator = reader.GetString(7);
                suggest.CreatorName = reader.GetString(8);
                suggest.EmployeeId = reader.GetValue(9) == DBNull.Value ? (Guid?)null : reader.GetGuid(9);
                result.Add(suggest);
            }
            return result;
        }
    }
}