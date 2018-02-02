using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using System.Data.Common;

namespace ChinaPay.B3B.Service.Foundation.Repository.SqlServer
{
    public class Check_InRepository : SqlServerRepository, ICheck_InRepository
    {
        public Check_InRepository(string connstring) : base(connstring) { }

        public int Delete(Domain.Check_In value)
        {
            string sql = "DELETE FROM T_CheckIn WHERE Id=@Id;";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", value.Id);
                return (int)dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Insert(Domain.Check_In value)
        {
            string sql = "INSERT INTO T_CheckIn(Id,AirlineName,AirlineCode,Remark,OperatingHref,Opertor,[Time]) VALUES(@Id,@AirlineName,@AirlineCode,@Remark,@OperatingHref,@Opertor,@Time);";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", value.Id);
                dbOperator.AddParameter("AirlineName", value.AirlineName);
                dbOperator.AddParameter("AirlineCode", value.AirlineCode);
                if (string.IsNullOrEmpty(value.Remark))
                {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Remark", value.Remark);
                }
                if (string.IsNullOrEmpty(value.OperatingHref))
                {
                    dbOperator.AddParameter("OperatingHref", "#");
                }
                else
                {
                    dbOperator.AddParameter("OperatingHref", value.OperatingHref);
                }
                dbOperator.AddParameter("Opertor", value.Opertor);
                dbOperator.AddParameter("Time", value.Time);
                return (int)dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<Guid, Domain.Check_In>> Query()
        {
            string sql = "SELECT Id,AirlineName,AirlineCode,Remark,OperatingHref,Opertor,[Time] FROM T_CheckIn";
            IList<KeyValuePair<Guid, Domain.Check_In>> result = new List<KeyValuePair<Guid, Domain.Check_In>>();
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (DbDataReader reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new KeyValuePair<Guid, Domain.Check_In>(reader.GetGuid(0),
                            new Domain.Check_In
                            {
                                Id = reader.GetGuid(0),
                                AirlineName = reader.GetString(1),
                                AirlineCode = reader.GetString(2),
                                Remark = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                OperatingHref = reader.GetString(4),
                                Opertor = reader.GetString(5),
                                Time = reader.GetDateTime(6)
                            }));
                    }
                }
            }
            return result;
        }

        public int Update(Domain.Check_In value)
        {
            string sql = "UPDATE T_CheckIn set AirlineName =@AirlineName,AirlineCode=@AirlineCode,Remark=@Remark,OperatingHref=@OperatingHref,Opertor=@Opertor,[Time]=@Time WHERE Id = @Id;";
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", value.Id);
                dbOperator.AddParameter("AirlineName", value.AirlineName);
                dbOperator.AddParameter("AirlineCode", value.AirlineCode);
                dbOperator.AddParameter("Remark", value.Remark);
                dbOperator.AddParameter("OperatingHref", value.OperatingHref);
                dbOperator.AddParameter("Opertor", value.Opertor);
                dbOperator.AddParameter("Time", value.Time);
                return (int)dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
