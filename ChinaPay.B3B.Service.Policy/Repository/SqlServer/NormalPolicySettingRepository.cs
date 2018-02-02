using System;
using System.Collections.Generic;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Policy.Repository.SqlServer
{
    public class NormalPolicySettingRepository : SqlServerTransaction, INormalPolicySettingRepository
    {
        public NormalPolicySettingRepository(ChinaPay.DataAccess.DbOperator dbOperator)
            : base(dbOperator)
        {
        }
        public void AddNormalPolicySetting(DataTransferObject.Policy.NormalPolicySetting view)
        {
            int i = 0;
            string sql = "";
            string sql1 = "";
            foreach (var item in view.Berths.Split(','))
            {
                i++;
                sql += @"DELETE FROM dbo.T_NormalPolicySetting WHERE PolicyId = @delPolicyId" + i + " AND Berths=@delBerths" + i + " AND FlightsFilter=@delFlightsFilter" + i + ";";
                sql1 += @"INSERT INTO [dbo].[T_NormalPolicySetting]
                                   ([Id],[PolicyId],[FlightsFilter],[Berths],[Commission],[StartTime],[EndTime],[Remark],[Type],[Enable],[Creator])
                                   VALUES(@Id" + i + ",@PolicyId" + i + ",@FlightsFilter" + i + ",@Berths" + i + ",@Commission" + i + ", @StartTime" + i + ",@EndTime" + i + ",@Remark" + i + ",@Type" + i + ",@Enable" + i + ",@Creator" + i + ")";

                AddParameter("@delPolicyId" + i, view.PolicyId);
                AddParameter("@delBerths" + i, item);
                AddParameter("@delFlightsFilter" + i, view.FlightsFilter);
                AddParameter("@Id" + i, Guid.NewGuid());
                AddParameter("@PolicyId" + i, view.PolicyId);
                AddParameter("@FlightsFilter" + i, view.FlightsFilter);
                AddParameter("@Berths" + i, item);
                AddParameter("@Commission" + i, view.Commission);
                AddParameter("@StartTime" + i, view.StartTime);
                AddParameter("@EndTime" + i, view.EndTime);
                AddParameter("@Remark" + i, view.Remark);
                AddParameter("@Type" + i, view.Type);
                AddParameter("@Enable" + i, true);
                AddParameter("@Creator" + i, view.Creator);
            }
            if (!string.IsNullOrWhiteSpace(sql + sql1))
            {
                ExecuteNonQuery(sql + sql1);
            }
        }

        public IEnumerable<DataTransferObject.Policy.NormalPolicySetting> QueryNormalPolicySetting(Guid? policyId, bool? Type, bool? Enable, string FlightsFilter, string Berths, DateTime? flightDate)
        {
            List<DataTransferObject.Policy.NormalPolicySetting> list = new List<DataTransferObject.Policy.NormalPolicySetting>();
            string sql = "SELECT [Id],[PolicyId],[FlightsFilter],[Berths],[Commission],[StartTime],[EndTime],[Remark],[Type],[Enable],[Creator],[OperationTime] FROM dbo.T_NormalPolicySetting WHERE 1 = 1 ";
            if (policyId != null)
            {
                sql += " AND  PolicyId = @PolicyId  ";
                AddParameter("@PolicyId", policyId);
            }
            //AddParameter("@PolicyId", policyId);
            if (Type != null)
            {
                sql += " AND Type=@Type";
                AddParameter("@Type", Type);
            }
            if (Enable != null)
            {
                sql += " AND Enable=@Enable";
                AddParameter("@Enable", Enable);
            }
            if (!string.IsNullOrEmpty(FlightsFilter))
            {
                sql += " AND (CHARINDEX(@FlightsFilter,FlightsFilter) > 0 OR CHARINDEX(@FlightsFilter1,FlightsFilter) > 0 OR CHARINDEX(@FlightsFilter2,FlightsFilter) > 0 OR CHARINDEX('**',FlightsFilter) > 0)";
                AddParameter("@FlightsFilter", FlightsFilter);
                AddParameter("@FlightsFilter1", FlightsFilter.Replace(FlightsFilter.Substring(0, 3), "*"));
                AddParameter("@FlightsFilter2", FlightsFilter.Replace(FlightsFilter.Substring(3, 3), "*"));
            }
            if (!string.IsNullOrEmpty(Berths))
            {
                sql += " AND Berths=@Berths";
                AddParameter("@Berths", Berths);
            }
            if (flightDate != null)
            {
                sql += " AND EndTime >= @EndTime AND StartTime<= @StartTime ";
                AddParameter("@EndTime", flightDate.Value.Date);
                AddParameter("@StartTime", flightDate.Value.Date);
            }
            sql += "   ORDER BY OperationTime DESC";
            using (var reader = ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    DataTransferObject.Policy.NormalPolicySetting setting = new DataTransferObject.Policy.NormalPolicySetting();
                    setting.Id = reader.GetGuid(0);
                    setting.PolicyId = reader.GetGuid(1);
                    setting.FlightsFilter = reader.GetString(2);
                    setting.Berths = reader.GetString(3);
                    setting.Commission = reader.GetDecimal(4);
                    setting.StartTime = reader.GetDateTime(5);
                    setting.EndTime = reader.GetDateTime(6);
                    setting.Remark = reader.GetString(7);
                    setting.Type = reader.GetBoolean(8);
                    setting.Enable = reader.GetBoolean(9);
                    setting.Creator = reader.GetString(10);
                    setting.OperationTime = reader.GetDateTime(11);
                    list.Add(setting);
                }

            }
            return list;
        }


        public void UpdateNormalPolicySetting(Guid Id, bool Enable)
        {
            string sql = @"UPDATE dbo.T_NormalPolicySetting SET [Enable] = @Enable WHERE Id = @Id";
            AddParameter("@Enable", Enable);
            AddParameter("@Id", Id);
            ExecuteNonQuery(sql);
        }
    }
}
