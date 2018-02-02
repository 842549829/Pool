using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    public class OEMStyleRepository : SqlServerRepository, IOEMStyleRepository
    {
        public OEMStyleRepository(string ConnectionString)
            : base(ConnectionString)
        {

        }
        public void Insert(Domain.OEMStyle style)
        {
            StringBuilder sql = new StringBuilder("INSERT INTO T_OEMStyle(Id,StyleName,ThumbnailPicture,Remark,Sort,Enable,TemplatePath)VALUES(@Id ,@StyleName ,@ThumbnailPicture ,@Remark ,@Sort ,@Enable,@TemplatePath );");
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {

                dbOperator.AddParameter("Id", style.Id);
                dbOperator.AddParameter("StyleName", style.StyleName);
                dbOperator.AddParameter("ThumbnailPicture", style.ThumbnailPicture);
                dbOperator.AddParameter("Remark", style.Remark);
                dbOperator.AddParameter("Sort", style.Sort);
                dbOperator.AddParameter("Enable", style.Enable);
                dbOperator.AddParameter("TemplatePath", style.TemplatePath);
                int index = 0;
                if (style.StylePath.Any())
                {
                    sql.Append("Insert into T_OEMStyleFile (OEMStyleId,FilePath)");
                    foreach (string path in style.StylePath)
                    {
                        sql.AppendFormat(" Select @Id,@FilePath{0} UNION All", index);
                        dbOperator.AddParameter("@FilePath" + index, path);
                        index++;
                    }
                    dbOperator.ExecuteNonQuery(sql.ToString().Remove(sql.Length - 10, 10));
                }
                else
                {
                    dbOperator.ExecuteNonQuery(sql.ToString());
                }
            }
        }

        public void Delete(Guid styleId)
        {
            const string sql = "DELETE FROM T_OEMStyle WHERE Id=@Id;delete from t_OEMStyleFile where OEMStyleId =@Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", styleId);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(Domain.OEMStyle style)
        {
            StringBuilder sql = new StringBuilder("UPDATE [dbo].[T_OEMStyle] SET [StyleName] = @StyleName ,[ThumbnailPicture] = @ThumbnailPicture ,[Remark] = @Remark ,[Sort] = @Sort ,[Enable] = @Enable,TemplatePath=@TemplatePath WHERE [Id] = @Id ;delete from t_OEMStyleFile where OEMStyleId=@Id;");
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", style.Id);
                dbOperator.AddParameter("StyleName", style.StyleName);
                //dbOperator.AddParameter("StylePath", style.StylePath);
                dbOperator.AddParameter("ThumbnailPicture", style.ThumbnailPicture);
                dbOperator.AddParameter("Remark", style.Remark);
                dbOperator.AddParameter("Sort", style.Sort);
                dbOperator.AddParameter("Enable", style.Enable);
                dbOperator.AddParameter("TemplatePath", style.TemplatePath);
                if (style.StylePath.Any())
                {
                    int index = 0;
                    sql.AppendFormat("Insert into T_OEMStyleFile (OEMStyleId,FilePath)");
                    foreach (string path in style.StylePath)
                    {
                        sql.AppendFormat(" Select @Id,@FilePath{0} UNION All", index);
                        dbOperator.AddParameter("@FilePath" + index, path);
                        index++;
                    }
                    dbOperator.ExecuteNonQuery(sql.ToString().Remove(sql.Length - 10, 10));

                }
                else
                {
                    dbOperator.ExecuteNonQuery(sql.ToString());

                }
            }
        }

        public void UpdateEnable(Guid styleId, bool enable)
        {
            string sql = "UPDATE [dbo].[T_OEMStyle] SET [Enable] = @Enable WHERE [Id] = @Id ";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", styleId);
                dbOperator.AddParameter("Enable", enable);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Domain.OEMStyle Query(Guid styleId)
        {
            Domain.OEMStyle style = null;
            const string sql = "SELECT Id,StyleName,ThumbnailPicture,Remark,Sort,Enable,TemplatePath FROM  dbo.T_OEMStyle WHERE [Id] = @Id ";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", styleId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        style = new Domain.OEMStyle();
                        style.Id = reader.GetGuid(0);
                        style.StyleName = reader.GetString(1);
                        style.ThumbnailPicture = reader.GetString(2);
                        style.Remark = reader.GetString(3);
                        style.Sort = reader.GetInt32(4);
                        style.Enable = reader.GetBoolean(5);
                        style.TemplatePath = reader.GetString(6);
                        style.StylePath = new List<string>();
                    }
                }
                const string queryFiles = "select FilePath from t_OEMStyleFile where OemStyleID =@Id";
                if (style != null)
                {
                    using (var reader = dbOperator.ExecuteReader(queryFiles))
                    {
                        while (reader.Read())
                        {
                            style.StylePath.Add(reader.GetString(0));
                        }
                    }
                }
                return style;
            }
        }

        public IEnumerable<Domain.OEMStyle> Query()
        {
            const string sql = "SELECT Id,StyleName,ThumbnailPicture,Remark,Sort,Enable," +
                               "TemplatePath FROM  dbo.T_OEMStyle Order by Sort";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                List<Domain.OEMStyle> list = new List<Domain.OEMStyle>();
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        Domain.OEMStyle style = new Domain.OEMStyle();
                        style.Id = reader.GetGuid(0);
                        style.StyleName = reader.GetString(1);
                        style.ThumbnailPicture = reader.GetString(2);
                        style.Remark = reader.GetString(3);
                        style.Sort = reader.GetInt32(4);
                        style.Enable = reader.GetBoolean(5);
                        style.TemplatePath = reader.GetString(6);
                        style.StylePath = new List<string>();
                        list.Add(style);
                    }
                }
                var files = new List<Tuple<string, Guid>>();
                const string sqlFileSelector = "select FilePath,OEMStyleId from t_OEMStyleFile";
                using (var reader = dbOperator.ExecuteReader(sqlFileSelector))
                {
                    while (reader.Read())
                    {
                        files.Add(new Tuple<string, Guid>(reader.GetString(0), reader.GetGuid(1)));
                    }
                }
                foreach (OEMStyle oemStyle in list)
                {
                    oemStyle.StylePath.AddRange(files.Where(f => f.Item2 == oemStyle.Id).Select(f => f.Item1));
                }
                return list;
            }
        }
    }
}
