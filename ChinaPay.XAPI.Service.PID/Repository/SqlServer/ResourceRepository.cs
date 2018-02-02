using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using ChinaPay.XAPI.Service.Pid.Domain;
using System.Net;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class ResourceRepository : SqlServerRepository, IResourceRepository
    {
        public ResourceRepository(string connectionString)
            : base(connectionString) { }

        public IEnumerable<Resource> Query()
        {
            List<Resource> result = new List<Resource>();
            string sql = @"select Id, Name, Password, officeNo, CertificationType, CertificationAddress,ServerAddress, ServerPort, " +
                    @"ServerLoginString, ConfigurationType, Status, AgentId from Resources";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt16(0);
                        string name = reader.GetString(1);
                        string password = reader.GetString(2);
                        string officeNo = reader.GetString(3);
                        CertificationType certificationType = (CertificationType)reader.GetInt16(4);

                        IPAddress certificationAddress = null;
                        if (!reader.IsDBNull(5))
                        {
                            certificationAddress = IPAddress.Parse(reader.GetString(5));
                        }
                        
                        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(reader.GetString(6)), reader.GetInt16(7));
                        string serverLoginString = reader.GetString(8);
                        string configurationType = reader.GetString(9);
                        byte status = reader.GetByte(10);
                        short agentId = reader.GetInt16(11);
                        Resource resource = new Resource(id, name, password, officeNo, certificationType, certificationAddress, serverAddress, serverLoginString, configurationType, status, agentId);
                        result.Add(resource);
                    }
                }
            }
            return result;
        }

        public Resource Query(int resourceid)
        {
            Resource resource = null;
            string sql = @"select Id, Name, Password, OfficeNo, CertificationType, CertificationAddress,ServerAddress, ServerPort, " +
                    @"ServerLoginString, ConfigurationType, Status, AgentId from Resources where Id = @Id";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", resourceid);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt16(0);
                        string name = reader.GetString(1);
                        string password = reader.GetString(2);
                        string officeNo = reader.GetString(3);
                        CertificationType certificationType = (CertificationType)reader.GetInt16(4);

                        IPAddress certificationAddress = null;
                        if (reader.GetString(5) != null)
                        {
                            certificationAddress = IPAddress.Parse(reader.GetString(5));
                        }

                        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(reader.GetString(6)), reader.GetInt32(7));
                        string serverLoginString = reader.GetString(8);
                        string configurationType = reader.GetString(9);
                        int status = reader.GetInt16(10);
                        int agentId = reader.GetInt32(11);
                        resource = new Resource(id, name, officeNo, password, certificationType, certificationAddress, serverAddress, serverLoginString, configurationType, status, agentId);                        
                    }
                }
            }
            return resource;
        }

        public int Insert(Resource resource)
        {
            string sql = @"insert into Resources(Name, Password, OfficeNo, CertificationType, CertificationAddress,ServerAddress, ServerPort, ServerLoginString, ConfigurationType, Status, AgentId)" +
                        @"values(@Name, @Password, @OfficeNo, @CertificationType, @CertificationAddress, @ServerAddress, @ServerPort, @ServerLoginString, @ConfigurationType, @Status, @AgentId)";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", resource.Name);
                dbOperator.AddParameter("Password", resource.Password);
                dbOperator.AddParameter("OfficeNo", resource.OfficeNo);
                dbOperator.AddParameter("CertificationType", resource.CertificationType);
                if (resource.CertificationAddress != null)
                {
                    dbOperator.AddParameter("CertificationAddress", resource.CertificationAddress.ToString());
                }
                else
                {
                    dbOperator.AddParameter("CertificationAddress", DBNull.Value);
                }

                dbOperator.AddParameter("ServerAddress", resource.ServerAddress.Address.ToString());
                dbOperator.AddParameter("ServerPort", resource.ServerAddress.Port);
                dbOperator.AddParameter("ServerLoginString", resource.ServerLoginString);
                dbOperator.AddParameter("ConfigurationType", resource.ConfigurationType);
                dbOperator.AddParameter("Status", resource.Status);
                dbOperator.AddParameter("AgentId", resource.AgentId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(Resource resource)
        {
            string sql = @"UPDATE Resources set Name=@Name, Password=@Password, OfficeNo=@OfficeNo, CertificationType=@CertificationType, CertificationAddress=@CertificationAddress,   ServerAddress=@ServerAddress, ServerPort=@ServerPort, ServerLoginString=@ServerLoginString, ConfigurationType=@ConfigurationType, Status=@Status,   AgentId=@AgentId  WHERE id=@id";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", resource.Name);
                dbOperator.AddParameter("Password", resource.OfficeNo);
                dbOperator.AddParameter("OfficeNo", resource.OfficeNo);
                dbOperator.AddParameter("CertificationType", resource.CertificationType);
                dbOperator.AddParameter("CertificationAddress", resource.CertificationAddress);
                dbOperator.AddParameter("ServerAddress", resource.ServerAddress.Address);
                dbOperator.AddParameter("ServerPort", resource.ServerAddress.Port);
                dbOperator.AddParameter("ServerLoginString", resource.ServerLoginString);
                dbOperator.AddParameter("ConfigurationType", resource.ConfigurationType);
                dbOperator.AddParameter("Status", resource.Status);
                dbOperator.AddParameter("AgentId", resource.AgentId);
                dbOperator.AddParameter("id", resource.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(int[] id)
        {
            string str_ids = DomainService.GetIds(id);
            string sql = @"DETELE FROM Resources WHERE id in (" + str_ids + " )";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
