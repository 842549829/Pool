using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.SystemSetting.VIPHarmony;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.SystemSetting.Repository.SqlServer {
    class VIPHarmonyRepository : SqlServerRepository, IVIPHarmonyRepository {
        public VIPHarmonyRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<VIPHarmonyListView> Query() {
            var result = new List<VIPHarmonyListView>();
            string sql = "SELECT [dbo].[T_VIPHarmony].[Id],[T_SellArea].[Name],[dbo].[T_VIPHarmony].[AirlineLimit],[dbo].[T_VIPHarmony].[CityLimit]," +
                         "[dbo].[T_VIPHarmony].[Account],[dbo].[T_VIPHarmony].[AddTime],[dbo].[T_VIPHarmony].[Remark] FROM [dbo].[T_VIPHarmony]" +
                         " INNER JOIN [T_SellArea] ON [T_VIPHarmony].[Area]=[T_SellArea].[Id]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var view = new VIPHarmonyListView(reader.GetGuid(0));
                        view.Account = reader.GetString(4);
                        view.AirlineLimit = reader.GetString(2);
                        view.CityLimit = reader.GetString(3);
                        view.AreaName = reader.GetString(1);
                        if(!reader.IsDBNull(5)) {
                            view.AddTime = reader.GetDateTime(5);
                        }
                        view.Remark = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public int Insert(Domain.VIPHarmony harmony) {
            string sql = "INSERT INTO [dbo].[T_VIPHarmony]([Id],[Area],[AirlineLimit],[CityLimit],[Account],[Remark],[AddTime]) VALUES(@Id,@Area,@AirlineLimit,@CityLimit,@Account,@Remark,@AddTime)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", harmony.Id);
                dbOperator.AddParameter("Area", harmony.Area);
                dbOperator.AddParameter("AirlineLimit", harmony.AirlineLimit.Join("/"));
                dbOperator.AddParameter("CityLimit", harmony.CityLimit.Join("/"));
                dbOperator.AddParameter("Account", harmony.Account);
                if(string.IsNullOrWhiteSpace(harmony.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", harmony.Remark);
                }
                dbOperator.AddParameter("AddTime", harmony.AddTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(Domain.VIPHarmony harmony) {
            string sql = "UPDATE [dbo].[T_VIPHarmony] SET [Area]=@Area,[AirlineLimit]=@AirlineLimit,[CityLimit]=@CityLimit,[Remark]=@Remark," +
                         "[AddTime]=@AddTime,[Account]=@Account WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", harmony.Id);
                dbOperator.AddParameter("Area", harmony.Area);
                dbOperator.AddParameter("AirlineLimit", harmony.AirlineLimit.Join("/"));
                dbOperator.AddParameter("CityLimit", harmony.CityLimit.Join("/"));
                if(string.IsNullOrWhiteSpace(harmony.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", harmony.Remark);
                }
                dbOperator.AddParameter("AddTime", harmony.AddTime);
                dbOperator.AddParameter("Account", harmony.Account);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(Guid id) {
            string sql = "DELETE FROM [dbo].[T_VIPHarmony] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }


        public int Delete(IEnumerable<Guid> ids) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(ids.Count() > 0) {
                    string sql = string.Format("DELETE FROM [dbo].[T_VIPHarmony] WHERE [Id] IN ({0})", ids.Join(",", item => "'" + item.ToString() + "'"));
                    return dbOperator.ExecuteNonQuery(sql);
                } else {
                    return 0;
                }
            }
        }


        public VIPHarmonyView Query(Guid id) {
            VIPHarmonyView view = null;
            string sql = "SELECT [Area],[AirlineLimit],[CityLimit],[Remark] FROM [dbo].[T_VIPHarmony] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new VIPHarmonyView(id);
                        view.AreaName = reader.GetString(0);
                        view.AirlineLimit = reader.GetString(1).Split(new char[] { '/' });
                        view.CityLimit = reader.GetString(2).Split(new char[] { '/' });
                        view.Remark = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                    }
                }
            }
            return view;
        }
    }
}
