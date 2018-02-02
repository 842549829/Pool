using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.SystemSetting.Repository.SqlServer {
    class AreaReposity : SqlServerRepository, IAreaRepository {
        public AreaReposity(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<AreaListView> Query(AreaQueryConditon condition) {
            var result = new List<AreaListView>();
            string sql = "SELECT [Id],[Name],[Remark] FROM [dbo].[T_SellArea]";
            StringBuilder strWhere = new StringBuilder();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(!string.IsNullOrWhiteSpace(condition.Name)) {
                    strWhere.Append(" WHERE [Name]=@Name");
                    dbOperator.AddParameter("Name", condition.Name);
                }
                if(strWhere.Length > 0) {
                    sql += strWhere;
                }
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var view = new AreaListView();
                        view.Id = reader.GetGuid(0);
                        view.Name = reader.GetString(1);
                        view.Remark = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public IEnumerable<AreaRelationListView> Query(AreaRelationQueryCondtion condition) {
            var result = new List<AreaRelationListView>();
            string sql = "SELECT [T_Province].[Code], [T_Province].[Name], [T_SellArea].[Name] FROM [T_SellArea] INNER JOIN [T_SellAreaRelation] ON [T_SellArea].[Id]=[T_SellAreaRelation].[Area] " +
                         "RIGHT JOIN [T_Province] ON [T_SellAreaRelation].[Province]=[T_Province].[Code]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                StringBuilder strWhere = new StringBuilder();
                if(!string.IsNullOrWhiteSpace(condition.AreaName)) {
                    strWhere.Append("[T_SellArea].[Name]=@AreaName");
                    dbOperator.AddParameter("AreaName", condition.AreaName);
                }
                if(!string.IsNullOrWhiteSpace(condition.ProvinceName)) {
                    strWhere.Append(" AND [T_Province].[Name]=@ProvinceName");
                    dbOperator.AddParameter("ProvinceName", condition.ProvinceName);
                }
                if(strWhere.Length > 0) {
                    sql += "WHERE" + strWhere.ToString();
                }
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var view = new AreaRelationListView();
                        view.ProvinceCode = reader.GetString(0);
                        view.ProvinceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        view.AreaName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public IEnumerable<AreaListView> Query(AreaQueryConditon condition, Pagination pagination)
        {
            var result = new List<AreaListView>();
            var fields = "[Id],[Name],[Remark]";
            var catelog = "[dbo].[T_SellArea]";
            var orderbyFiled = "[Id]";
            var where = new StringBuilder();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (!string.IsNullOrWhiteSpace(condition.Name))
                {
                    where.AppendFormat("Name = '{0}'", condition.Name.Trim());
                }
                dbOperator.AddParameter("@iField", fields);
                dbOperator.AddParameter("@iCatelog", catelog);
                dbOperator.AddParameter("@iCondition", where.ToString());
                dbOperator.AddParameter("@iOrderBy", orderbyFiled);
                dbOperator.AddParameter("@iPagesize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                dbOperator.AddParameter("@iGetCount", pagination.GetRowCount);
                var totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        result.Add(new AreaListView(){
                              Id = reader.GetGuid(0),
                              Name = reader.GetString(1),
                              Remark = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                        });
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }

        public IEnumerable<AreaRelationListView> Query(AreaRelationQueryCondtion condition, Pagination pagination)
        {
           var result = new List<AreaRelationListView>();
           var fields = "[T_Province].[Code] AS ProvinceCode, [T_Province].[Name] AS ProvinceName, [T_SellArea].[Name] AS AreaName";
           var catelog = "[T_SellArea] INNER JOIN [T_SellAreaRelation] ON [T_SellArea].[Id]=[T_SellAreaRelation].[Area] RIGHT JOIN [T_Province] ON [T_SellAreaRelation].[Province]=[T_Province].[Code]";
           var orderbyFiled = "[T_Province].[Code]";
           var choiceOrderByFiled = "ProvinceCode";
           var choiceFiled = "ProvinceCode,ProvinceName,AreaName";
           var where = new StringBuilder();
           using (var dbOperator = new DbOperator(Provider, ConnectionString))
           {
               if (!string.IsNullOrWhiteSpace(condition.AreaName))
               {
                   where.AppendFormat("[T_SellArea].[Name]= N'{0}' AND ",condition.AreaName.Trim());
               }
               if (!string.IsNullOrWhiteSpace(condition.ProvinceName))
               {
                   where.AppendFormat("[T_Province].[Name]= N'{0}' AND ",condition.ProvinceName.Trim());
               }
               if (where.Length > 0)
               {
                   where = where.Remove(where.Length - 5, 5);
               }
               dbOperator.AddParameter("@iField", fields);
               dbOperator.AddParameter("@iCatelog", catelog);
               dbOperator.AddParameter("@iChoiceField", choiceFiled);
               dbOperator.AddParameter("@iChoiceOrderBy", choiceOrderByFiled);
               dbOperator.AddParameter("@iCondition", where.ToString());
               dbOperator.AddParameter("@iOrderBy", orderbyFiled);
               dbOperator.AddParameter("@iPagesize", pagination.PageSize);
               dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
               dbOperator.AddParameter("@iGetCount", pagination.GetRowCount);
               var totalCount = dbOperator.AddParameter("@oTotalCount");
               totalCount.DbType = System.Data.DbType.Int32;
               totalCount.Direction = System.Data.ParameterDirection.Output;
               using (var reader = dbOperator.ExecuteReader("dbo.P_AreaRelationPagination", System.Data.CommandType.StoredProcedure))
               {
                   while (reader.Read())
                   {
                       result.Add(new AreaRelationListView()
                       {
                           ProvinceCode = reader.GetString(0),
                           ProvinceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                           AreaName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                       });
                   }
               }
               if (pagination.GetRowCount)
               {
                   pagination.RowCount = (int)totalCount.Value;
               }
           }
           return result;
        }

        public int InsertArea(Domain.SellArea area) {
            string sql = "INSERT INTO [dbo].[T_SellArea]([Id],[Name],[Remark]) VALUES(@Id,@Name,@Remark)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", area.Id);
                dbOperator.AddParameter("Name", area.Name);
                if(string.IsNullOrWhiteSpace(area.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", area.Remark);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteArea(Guid id) {
            string sql = "DELETE FROM [dbo].[T_SellArea] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteArea(IEnumerable<Guid> ids) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(ids.Count() > 0) {
                    string sql = string.Format("DELETE FROM [dbo].[T_SellArea] WHERE [Id] IN ({0})", ids.Join(",", item => "'" + item.ToString() + "'"));
                    return dbOperator.ExecuteNonQuery(sql);
                } else {
                    return 0;
                }
            }
        }

        public int UpdateArea(Domain.SellArea area) {
            string sql = "UPDATE [dbo].[T_SellArea] SET [Name]=@Name,[Remark]=@Remark WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", area.Id);
                dbOperator.AddParameter("Name", area.Name);
                if(string.IsNullOrWhiteSpace(area.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", area.Remark);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int InsertAreaRelation(Guid area, string provinceCode) {
            DeleteAreaRelation(provinceCode);
            string sql = "INSERT INTO [dbo].[T_SellAreaRelation]([Area],[Province]) VALUES(@Area,@Province)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Area", area);
                dbOperator.AddParameter("Province", provinceCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateAreaRelation(Guid area, string provinceCode) {
            string sql = "UPDATE [dbo].[T_SellAreaRelation] SET [Area]=@Area WHERE [Province]=@Province";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Area", area);
                dbOperator.AddParameter("Province", provinceCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteAreaRelation(string provinceCode) {
            string sql = "DELETE FROM [dbo].[T_SellAreaRelation] WHERE [Province]=@Province";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Province", provinceCode);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteAreaRelation(IEnumerable<string> provinceCode) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(provinceCode.Count() > 0) {
                    string sql = string.Format("DELETE FROM [dbo].[T_SellAreaRelation] WHERE [Province] IN ({0})", provinceCode.Join(",", item => "'" + item.ToString() + "'"));
                    return dbOperator.ExecuteNonQuery(sql);
                } else {
                    return 0;
                }
            }
        }

        public Guid QueryAreaCode(string name) {
            Guid code = Guid.NewGuid();
            string sql = "SELECT [Id] FROM [dbo].[T_SellArea] WHERE [Name]=@Name";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Name", name);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        code = reader.GetGuid(0);
                    }
                }
            }
            return code;
        }

        public AreaView Query(Guid id) {
            AreaView view = null;
            string sql = "SELECT [Name],[Remark] FROM [dbo].[T_SellArea] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new AreaView();
                        view.Name = reader.GetString(0);
                        view.Remark = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    }
                }
            }
            return view;
        }

        public AreaRelationView QueryRelation(string provinceCode) {
            AreaRelationView view = null;
            string sql = "SELECT [Province],[ProvinceName],[Name] from [dbo].[V_SellArea] where [Province]=@Province";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Province", provinceCode);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new AreaRelationView();
                        view.Province = reader.GetString(0);
                        view.AreaName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        view.ProcinceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    }
                }
            }
            return view;
        }
    }
}
