using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using ChinaPay.Core;
using System.Linq;

namespace ChinaPay.B3B.Service.ReleaseNote.Repository.SqlServer
{
    class ReleaseNoteRepository : SqlServerRepository, IReleaseNoteRepository
    {
        public ReleaseNoteRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<Domain.ReleaseNote> Query(Pagination paination, DateTime? startTime, DateTime? endTime, Common.Enums.CompanyType? type, ChinaPay.B3B.Common.Enums.ReleaseNoteType? releaseType)
        {
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                List<Domain.ReleaseNote> list = new List<Domain.ReleaseNote>();
                if (startTime.HasValue)
                {
                    dbOperator.AddParameter("@iStartTime", startTime);
                } 
                if (endTime.HasValue)
                {
                    dbOperator.AddParameter("@iEndTime", endTime);
                }
                if (type != null)
                {
                    dbOperator.AddParameter("@iType", type);
                }
                if (releaseType != null)
                {
                    dbOperator.AddParameter("@iReleaseType", releaseType);
                }
                dbOperator.AddParameter("@iPageSize", paination.PageSize);
                dbOperator.AddParameter("@iPageIndex", paination.PageIndex);

                var totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;

                using (var reader = dbOperator.ExecuteReader("dbo.P_QueryReleaseNote", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        Domain.ReleaseNote note = new Domain.ReleaseNote();
                        note.Id = reader.GetGuid(0);
                        note.CreateTime = reader.GetDateTime(1);
                        note.Creator = reader.GetString(2);
                        note.UpdateTime = reader.GetDateTime(3);
                        note.Title = reader.GetString(4);
                        note.Context = reader.GetString(5);
                        note.Type = (ChinaPay.B3B.Common.Enums.CompanyType)reader.GetByte(6);
                        note.ReleaseType = (ChinaPay.B3B.Common.Enums.ReleaseNoteType)reader.GetByte(7);
                        list.Add(note);
                    }
                }
                if (list.Any())
                {
                    if (paination.GetRowCount)
                    {
                        paination.RowCount = (int)totalCount.Value;
                    }
                }
                return list;
            }
        }

        public Domain.ReleaseNote Query(Guid id)
        {
            string sql = "SELECT Id,CreateTime,Creator,UpdateTime,Title,Context,Type,ReleaseType FROM dbo.T_ReleaseNote WHERE Id = '" + id + "'";

            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Domain.ReleaseNote note = null;
                    if (reader.Read())
                    {
                        note = new Domain.ReleaseNote();
                        note.Id = reader.GetGuid(0);
                        note.CreateTime = reader.GetDateTime(1);
                        note.Creator = reader.GetString(2);
                        note.UpdateTime = reader.GetDateTime(3);
                        note.Title = reader.GetString(4);
                        note.Context = reader.GetString(5);
                        note.Type = (ChinaPay.B3B.Common.Enums.CompanyType)reader.GetByte(6);
                        note.ReleaseType = (ChinaPay.B3B.Common.Enums.ReleaseNoteType)reader.GetByte(7);
                    }
                    return note;
                }
            }
        }

        public void Update(Domain.ReleaseNote note)
        {
            string sql = "UPDATE dbo.T_ReleaseNote SET CreateTime = @CreateTime ,Creator = @Creator ,UpdateTime = @UpdateTime ,Title = @Title ,Context = @Context ,Type = @Type ,ReleaseType = @ReleaseType WHERE Id = @Id";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CreateTime", note.CreateTime);
                dbOperator.AddParameter("Creator", note.Creator);
                dbOperator.AddParameter("UpdateTime", note.UpdateTime);
                dbOperator.AddParameter("Title", note.Title);
                dbOperator.AddParameter("Context", note.Context);
                dbOperator.AddParameter("Type", note.Type);
                dbOperator.AddParameter("ReleaseType", note.ReleaseType);
                dbOperator.AddParameter("Id", note.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Add(Domain.ReleaseNote note)
        {
            string sql = @"INSERT INTO dbo.T_ReleaseNote(Id,CreateTime,Creator,UpdateTime,Title,Context,Type,ReleaseType)
                           VALUES(@Id,@CreateTime,@Creator,@UpdateTime,@Title,@Context,@Type,@ReleaseType)";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CreateTime", note.CreateTime);
                dbOperator.AddParameter("Creator", note.Creator);
                dbOperator.AddParameter("UpdateTime", note.UpdateTime);
                dbOperator.AddParameter("Title", note.Title);
                dbOperator.AddParameter("Type", note.Type);
                dbOperator.AddParameter("Context", note.Context);
                dbOperator.AddParameter("ReleaseType", note.ReleaseType);
                dbOperator.AddParameter("Id", note.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Delete(Guid id)
        {
            string sql = @"DELETE FROM dbo.T_ReleaseNote WHERE Id=@Id";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
