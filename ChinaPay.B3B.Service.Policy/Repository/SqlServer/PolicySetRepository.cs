using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Policy.Repository.SqlServer
{
    class PolicySetRepository : SqlServerRepository, IPolicySetRepository
    {
        public PolicySetRepository(string connectionstring)
            : base(connectionstring)
        {
        }
        public Domain.SetPolicy QuerySetPolicy(Guid companyid)
        {
            Domain.SetPolicy setPolicy = null;
            string sql = @"SELECT [BargainCount],[SinglenessCount],[DisperseCount],[CostFreeCount],[BlocCount],                  
                          [BusinessCount],[Departure],[Airlines],[Remark],OtherSpecialCount,LowToHighCount FROM [dbo].[T_SettingPolicy] WHERE [Company]=@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyid);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        setPolicy = new Domain.SetPolicy(companyid);
                        setPolicy.PromotionCount = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            setPolicy.SinglenessCount = reader.GetInt32(1);
                        if (!reader.IsDBNull(2))
                            setPolicy.DisperseCount = reader.GetInt32(2);
                        if (!reader.IsDBNull(3))
                            setPolicy.CostFreeCount = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            setPolicy.BlocCount = reader.GetInt32(4);
                        if (!reader.IsDBNull(5))
                            setPolicy.BusinessCount = reader.GetInt32(5);
                        setPolicy.Departure = reader.GetString(6).Split('/');
                        setPolicy.Airlines = reader.GetString(7).Split('/');
                        setPolicy.Remark = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        if (!reader.IsDBNull(9))
                            setPolicy.OtherSpecialCount = reader.GetInt32(9);
                        if (!reader.IsDBNull(10))
                            setPolicy.LowToHighCount = reader.GetInt32(10);
                    }
                }
            }
            return setPolicy;
        }

        public Domain.CompanyLimitPolicy QueryLimitPolicy(Guid companyid)
        {
            Domain.CompanyLimitPolicy policy = null;
            string sql = "SELECT [ChildValid],[ChildRebate],[ChildAirlines] FROM [dbo].[T_SettingPolicy] WHERE [Company]=@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyid);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        policy = new Domain.CompanyLimitPolicy(companyid);
                        if (reader.GetInt32(0) == 1)
                        {
                            var c1 = new Domain.ReturnDeduction();
                            c1.Rebate = reader.GetDecimal(1);
                            c1.Airlines = reader.GetString(2).Split('/');
                            policy.Child = c1;
                        }
                        else
                        {
                            policy.Child = null;
                        }
                    }
                }
            }
            return policy;
        }

        public IEnumerable<string> QueryAirlines(Guid companyid)
        {
            var result = new List<string>();
            string sql = "SELECT [Airlines] FROM [dbo].[T_SettingPolicy] WHERE [Company]=@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyid);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        string[] strAirlines = reader.GetString(0).Split('/');
                        for (int i = 0; i < strAirlines.Length; i++)
                        {
                            result.Add(strAirlines[i]);
                        }
                    }
                }
            }
            return result;
        }

        public int Save(Domain.SetPolicy policy)
        {
            string sql = @"IF EXISTS(SELECT NULL FROM [dbo].[T_SettingPolicy] WHERE [Company]=@Company) 
                            UPDATE [dbo].[T_SettingPolicy] SET BargainCount=@PromotionCount,
                           SinglenessCount=@SinglenessCount,DisperseCount=@DisperseCount,CostFreeCount=@CostFreeCount,
                           BlocCount=@BlocCount,BusinessCount=@BusinessCount,[OtherSpecialCount]=@OtherSpecialCount,[Departure]=@Departure,[Airlines]=@Airlines,
                           [Remark]=@Remark,LowToHighCount=@LowToHighCount WHERE [Company]=@Company;
                            ELSE 
                           INSERT INTO [dbo].[T_SettingPolicy]([Company],[BargainCount],[SinglenessCount],[DisperseCount],
                           [CostFreeCount],[BlocCount],[BusinessCount],[OtherSpecialCount],[Departure],[Airlines],[Remark],LowToHighCount)VALUES(@Company,@PromotionCount,@SinglenessCount,@DisperseCount,@CostFreeCount,@BlocCount,@BusinessCount,@OtherSpecialCount,@Departure,@Airlines,@Remark,@LowToHighCount)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", policy.Company);
                dbOperator.AddParameter("PromotionCount", policy.PromotionCount);
                dbOperator.AddParameter("SinglenessCount", policy.SinglenessCount);
                dbOperator.AddParameter("DisperseCount", policy.DisperseCount);
                dbOperator.AddParameter("CostFreeCount", policy.CostFreeCount);
                dbOperator.AddParameter("BlocCount", policy.BlocCount);
                dbOperator.AddParameter("BusinessCount", policy.BusinessCount);
                dbOperator.AddParameter("OtherSpecialCount", policy.OtherSpecialCount);
                dbOperator.AddParameter("LowToHighCount", policy.LowToHighCount);
                dbOperator.AddParameter("Departure", policy.Departure.Join("/", item => item));
                dbOperator.AddParameter("Airlines", policy.Airlines.Join("/", item => item));
                if (string.IsNullOrWhiteSpace(policy.Remark))
                {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Remark", policy.Remark);
                }
                try
                {
                    dbOperator.ExecuteNonQuery(sql);
                    return 1;
                }
                catch (Exception)
                {
                    //dbOperator.RollbackTransaction();
                    return -1;
                }
            }
        }

        public int Save(Domain.CompanyLimitPolicy policy)
        {
            string sql = "UPDATE [dbo].[T_SettingPolicy] SET [ChildValid]=@ChildValid,[ChildRebate]=@ChildRebate," +
                        "[ChildAirlines]=@ChildAirlines WHERE [Company]=@Company;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", policy.Company);
                if (policy.Child != null)
                {
                    dbOperator.AddParameter("ChildValid", 1);
                    dbOperator.AddParameter("ChildRebate", policy.Child.Rebate);
                    dbOperator.AddParameter("ChildAirlines", policy.Child.Airlines.Join("/", item => item));
                }
                else
                {
                    dbOperator.AddParameter("ChildValid", 0);
                    dbOperator.AddParameter("ChildRebate", DBNull.Value);
                    dbOperator.AddParameter("ChildAirlines", DBNull.Value);
                }
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
