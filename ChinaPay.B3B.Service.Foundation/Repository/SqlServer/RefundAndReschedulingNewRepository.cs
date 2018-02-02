using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer
{
    class RefundAndReschedulingNewRepository : SqlServerRepository, IRefundAndReschedulingNewRepository
    {
        public RefundAndReschedulingNewRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<RefundAndReschedulingBase> Query()
        {
            string sql = @"SELECT [Airline],[AirlineTel],[Condition],[Scrap],[Upgrade],[Remark],[Level] FROM [dbo].[T_RefundAndReschedulingBase]";
            var result = new List<RefundAndReschedulingBase>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        RefundAndReschedulingBase item = new RefundAndReschedulingBase(reader.GetString(0));
                        item.AirlineTel = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        item.Condition = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        item.Scrap = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        item.Upgrade = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        item.Remark = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        item.Level = reader.GetInt32(6);
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public RefundAndReschedulingBase Query(UpperString airline)
        {
            RefundAndReschedulingBase result = new RefundAndReschedulingBase(airline);
            IList<RefundAndReschedulingDetail> detail = new List<RefundAndReschedulingDetail>();
            string sql = "SELECT base.Airline,base.AirlineTel,base.Condition,base.Scrap,base.Upgrade,base.Remark,base.[Level],detail.Id,detail.Bunks,detail.ScrapBefore,detail.ScrapAfter,detail.ChangeBefore,detail.ChangeAfter,detail.Endorse,detail.Airline FROM T_RefundAndReschedulingBase base LEFT JOIN T_RefundAndReschedulingDetail detail ON base.Airline = detail.Airline WHERE base.Airline =@Airline";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Airline", airline.Value);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    string strAirline = string.Empty;
                    while (reader.Read())
                    {
                        if (strAirline != reader.GetString(0))
                        {
                            result.AirlineTel = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            result.Condition = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            result.Scrap = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                            result.Upgrade = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            result.Remark = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                            result.Level = reader.GetInt32(6);
                            strAirline = reader.GetString(0);
                        }
                        if (!reader.IsDBNull(7))
                        {
                            detail.Add(new RefundAndReschedulingDetail(reader.GetGuid(7))
                            {
                                Bunks = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),

                                ScrapBefore = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                ScrapAfter = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                ChangeBefore = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                ChangeAfter = reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                                Endorse = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                Airline = reader.GetString(14),
                            });
                            result.RefundAndReschedulingDetail = detail;
                        }
                    }
                }
            }
            return result;
        }

        public int Insert(RefundAndReschedulingBase item)
        {
            string sql = @"INSERT INTO [T_RefundAndReschedulingBase]([Airline],[AirlineTel],[Condition],[Scrap],[Upgrade],[Remark],[Level]) VALUES (@Airline,@AirlineTel,@Condition,@Scrap,@Upgrade ,@Remark,@Level)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Airline", item.AirlineCode.Value);
                if (string.IsNullOrEmpty(item.AirlineTel))
                {
                    dbOperator.AddParameter("AirlineTel", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("AirlineTel", item.AirlineTel);
                }
                if (string.IsNullOrEmpty(item.Condition))
                {
                    dbOperator.AddParameter("Condition", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Condition", item.Condition);
                }
                if (string.IsNullOrEmpty(item.Scrap))
                {
                    dbOperator.AddParameter("Scrap", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Scrap", item.Scrap);
                }
                if (string.IsNullOrEmpty(item.Upgrade))
                {
                    dbOperator.AddParameter("Upgrade", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Upgrade", item.Upgrade);
                }
                if (string.IsNullOrEmpty(item.Remark))
                {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Remark", item.Remark);
                }
                dbOperator.AddParameter("Level", item.Level);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(RefundAndReschedulingBase item)
        {
            string sql = @"UPDATE [dbo].[T_RefundAndReschedulingBase] SET [AirlineTel] = @AirlineTel,[Condition] = @Condition,[Scrap] = @Scrap
                         ,[Upgrade] = @Upgrade,[Remark] = @Remark,[Level] = @Level WHERE [Airline] = @Airline";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Airline", item.AirlineCode.Value);
                if (string.IsNullOrEmpty(item.AirlineTel))
                {
                    dbOperator.AddParameter("AirlineTel", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("AirlineTel", item.AirlineTel);
                }
                if (string.IsNullOrEmpty(item.Condition))
                {
                    dbOperator.AddParameter("Condition", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Condition", item.Condition);
                }
                if (string.IsNullOrEmpty(item.Scrap))
                {
                    dbOperator.AddParameter("Scrap", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Scrap", item.Scrap);
                }
                if (string.IsNullOrEmpty(item.Upgrade))
                {
                    dbOperator.AddParameter("Upgrade", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Upgrade", item.Upgrade);
                }

                if (string.IsNullOrEmpty(item.Remark))
                {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Remark", item.Remark);
                }
                dbOperator.AddParameter("Level", item.Level);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(UpperString airlineCode)
        {
            string sql = "DELETE FROM [T_RefundAndReschedulingBase] WHERE [Airline]=@Airline";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Airline", airlineCode.Value);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }


        public int Insert(RefundAndReschedulingDetail item)
        {
            string sql = @"INSERT INTO [B3B].[dbo].[T_RefundAndReschedulingDetail]
           ([Id],[Bunks],[ScrapBefore],[ScrapAfter],[ChangeBefore],[ChangeAfter],[Endorse],[Airline])
           VALUES (@Id,@Bunks,@ScrapBefore,@ScrapAfter,@ChangeBefore,@ChangeAfter,@Endorse,@Airline)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Id", item.Id);
                dbOperator.AddParameter("@Airline", item.Airline);
                if (string.IsNullOrWhiteSpace(item.Bunks))
                {
                    dbOperator.AddParameter("@Bunks", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@Bunks", item.Bunks);
                }
                if (string.IsNullOrWhiteSpace(item.ScrapBefore))
                {
                    dbOperator.AddParameter("@ScrapBefore", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@ScrapBefore", item.ScrapBefore);
                }
                if (string.IsNullOrWhiteSpace(item.ScrapAfter))
                {
                    dbOperator.AddParameter("@ScrapAfter", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@ScrapAfter", item.ScrapAfter);
                }
                if (string.IsNullOrWhiteSpace(item.ChangeBefore))
                {
                    dbOperator.AddParameter("@ChangeBefore", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@ChangeBefore", item.ChangeBefore);
                }
                if (string.IsNullOrWhiteSpace(item.ChangeAfter))
                {
                    dbOperator.AddParameter("@ChangeAfter", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("ChangeAfter", item.ChangeAfter);
                }
                if (string.IsNullOrWhiteSpace(item.Endorse))
                {
                    dbOperator.AddParameter("@Endorse", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@Endorse", item.Endorse);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(RefundAndReschedulingDetail item)
        {
            string sql = @"UPDATE [dbo].[T_RefundAndReschedulingDetail] SET [Bunks] = @Bunks,[ScrapBefore] = @ScrapBefore,[ScrapAfter] = @ScrapAfter
                                 ,[ChangeBefore] = @ChangeBefore,[ChangeAfter] = @ChangeAfter,[Endorse] = @Endorse,[Airline]=@Airline WHERE [Id] = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Id", item.Id);
                dbOperator.AddParameter("@Airline", item.Airline);
                if (string.IsNullOrWhiteSpace(item.Bunks))
                {
                    dbOperator.AddParameter("@Bunks", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@Bunks", item.Bunks);
                }
                if (string.IsNullOrWhiteSpace(item.ScrapBefore))
                {
                    dbOperator.AddParameter("@ScrapBefore", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@ScrapBefore", item.ScrapBefore);
                }
                if (string.IsNullOrWhiteSpace(item.ScrapAfter))
                {
                    dbOperator.AddParameter("@ScrapAfter", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@ScrapAfter", item.ScrapAfter);
                }
                if (string.IsNullOrWhiteSpace(item.ChangeBefore))
                {
                    dbOperator.AddParameter("@ChangeBefore", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@ChangeBefore", item.ChangeBefore);
                }
                if (string.IsNullOrWhiteSpace(item.ChangeAfter))
                {
                    dbOperator.AddParameter("@ChangeAfter", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@ChangeAfter", item.ChangeAfter);
                }
                if (string.IsNullOrWhiteSpace(item.Endorse))
                {
                    dbOperator.AddParameter("@Endorse", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("@Endorse", item.Endorse);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(Guid detailId)
        {
            string sql = @"DELETE FROM [dbo].[T_RefundAndReschedulingDetail] WHERE Id=@Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Id", detailId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public RefundAndReschedulingDetail Query(Guid detailId)
        {
            RefundAndReschedulingDetail result = null;
            string sql = @"SELECT [Bunks],[ScrapBefore],[ScrapAfter],[ChangeBefore],[ChangeAfter],[Endorse],[Airline] FROM [dbo].[T_RefundAndReschedulingDetail] WHERE [Id]=@Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Id", detailId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result = new RefundAndReschedulingDetail(detailId);
                        result.Bunks = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        result.ScrapBefore = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        result.ScrapAfter = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        result.ChangeBefore = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        result.ChangeAfter = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        result.Endorse = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        result.Airline = reader.GetString(6);
                    }
                }
            }
            return result;
        }

        public IEnumerable<RefundAndReschedulingDetail> Query(string airline)
        {
            var result = new List<RefundAndReschedulingDetail>();
            string sql = @"SELECT [Id],[Bunks],[ScrapBefore],[ScrapAfter],[ChangeBefore],[ChangeAfter],[Endorse] FROM [dbo].[T_RefundAndReschedulingDetail] WHERE Airline=@Airline";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Airline", airline);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var item = new RefundAndReschedulingDetail(reader.GetGuid(0));
                        item.Bunks = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        item.ScrapBefore = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        item.ScrapAfter = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        item.ChangeBefore = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        item.ChangeAfter = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        item.Endorse = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        item.Airline = airline;
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        public IEnumerable<AirlineRulesView> QueryAllRefundAndReschedulings(string airline)
        {
            var result = new List<AirlineRulesView>();
            string sql;
            if (string.IsNullOrEmpty(airline))
            {
                sql = @"select ACode.Code,ACode.Name,COUNT(Base.Airline) HasRules,COUNT(Detail.Airline) as RulesCount from T_Airline ACode
	left join T_RefundAndReschedulingBase Base on ACode.Code = Base.Airline
	left join T_RefundAndReschedulingDetail Detail ON ACode.Code = Detail.Airline
	where ACode.Valid = 1
	Group by ACode.Code,ACode.Name";
            }
            else
            {
                sql = @"select ACode.Code,ACode.Name,COUNT(Base.Airline) HasRules,COUNT(Detail.Airline) as RulesCount from T_Airline ACode
	left join T_RefundAndReschedulingBase Base on ACode.Code = Base.Airline
	left join T_RefundAndReschedulingDetail Detail ON ACode.Code = Detail.Airline
	where ACode.Valid = 1 and ACode.Code = @Airline
	Group by ACode.Code,ACode.Name";
            }
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Airline", airline ?? string.Empty);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var item = new AirlineRulesView();
                        item.AirlineCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        item.AirlineName = reader.GetString(1);
                        item.HasRules = reader.GetInt32(2) > 0;
                        item.RulesCount = reader.GetInt32(3);
                        result.Add(item);
                    }
                }
            }
            return result;

        }

        public RefundAndReschedulingBase QueryRefundAndRescheduling(UpperString airline)
        {
            RefundAndReschedulingBase result = null;
            string sql = @"SELECT [AirlineTel],[Condition],[Scrap],[Upgrade],[Remark],[Level] FROM [dbo].[T_RefundAndReschedulingBase] WHERE [Airline]=@Airline";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Airline", airline.Value);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result = new RefundAndReschedulingBase(airline);
                        result.AirlineTel = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        result.Condition = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        result.Scrap = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        result.Upgrade = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        result.Remark = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        result.Level = reader.GetInt32(5);
                    }
                }
            }
            return result;
        }


        public IEnumerable<RefundAndReschedulingDetailView> Query(string airline, string bunk)
        {
            var result = new List<RefundAndReschedulingDetailView>();
            string sql = @"SELECT detail.ScrapBefore,detail.ScrapAfter,
                           detail.ChangeBefore,detail.ChangeAfter,detail.Endorse,
                           base.Condition,base.Scrap,base.Upgrade,base.Remark,detail.Bunks
                           FROM dbo.T_RefundAndReschedulingDetail detail
                           INNER JOIN dbo.T_RefundAndReschedulingBase base ON detail.Airline= base.Airline
                          WHERE detail.Airline=@Airline AND Bunks LIKE @Bunks";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@Airline", airline.ToUpper());
                dbOperator.AddParameter("@Bunks", "%" + bunk + "%");
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var detail = new RefundAndReschedulingDetailView();
                        detail.ScrapBefore = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        detail.ScrapAfter = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        detail.ChangeBefore = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        detail.ChangeAfter = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        detail.Endorse = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        detail.Condition = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        detail.Scrap = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        detail.Upgrade = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        detail.Remark = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        detail.Bunks = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        detail.Airline = airline;
                        result.Add(detail);
                    }
                }
            }
            return result;
        }
    }



}