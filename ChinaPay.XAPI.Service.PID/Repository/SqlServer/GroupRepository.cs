using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.DataAccess;
using System.Net;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class GroupRepository : SqlServerRepository, IGroupRepository
    {
        public GroupRepository(string connectionString)
            : base(connectionString) { }

        public IEnumerable<Group> Query()
        {
            List<Group> result = null;
            string sql = @"select Id, Name, XapiName, XapiPassword, XapiAddress, XapiPort, OfficeNo, IsPublic, Description from Groups ";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    result = new List<Group>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt16(0);
                        string name = reader.GetString(1);
                        string xapiName = reader.GetString(2);
                        string xapiPassword = reader.GetString(3);
                        IPEndPoint xapiAddress = new IPEndPoint(IPAddress.Parse(reader.GetString(4)), reader.GetInt16(5));
                        string officeNo = reader.GetString(6);
                        bool isPublic = reader.GetBoolean(7);
                        string description = reader.GetString(8);
                        Group group = new Group(id, name,officeNo, xapiName, xapiPassword, xapiAddress, isPublic, description);
                        result.Add(group);
                    }
                }
            }
            return result;
        }

        public Group Query(int tid)
        {
            Group group = null;
            string sql = @" select top 1 g.Id, g.Name, g.XapiName, g.XapiPassword, g.XapiAddress, g.XapiPort, g.OfficeNo, g.IsPublic, g.Description " +
                @" from ResourceServiceHistories r " +
                @" inner join Groups g on r.GroupId = g.Id " +
                @" where GenerateTime > DATEADD(SS, -120, GETDATE()) and CHARINDEX('tid=' + CONVERT(varchar, @tid) , sendmessage) > 0";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("tid", tid);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt16(0);
                        string name = reader.GetString(1);
                        string xapiName = reader.GetString(2);
                        string xapiPassword = reader.GetString(3);
                        IPEndPoint xapiAddress = new IPEndPoint(IPAddress.Parse(reader.GetString(4)), reader.GetInt16(5));
                        string officeNo = reader.GetString(6);
                        bool isPublic = reader.GetBoolean(7);
                        string description = reader.GetString(8);
                        group = new Group(id, name, officeNo, xapiName, xapiPassword, xapiAddress, isPublic, description);
                    }
                }
            }
            return group;
        }

        public int Insert(Group group)
        {
            string sql = @"INSERT INTO Groups( Name, OfficeNo, XapiName, XapiPassword, XapiAddress, XapiPort, IsPublic,Description)" +
                @"values( @Name, @OfficeNo, @XapiName, @XapiPassword, @XapiAddress, @XapiPort @IsPublic, @Description);";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", group.Name);
                dbOperator.AddParameter("OfficeNo", group.OfficeNo);
                dbOperator.AddParameter("XapiName", group.XapiName);
                dbOperator.AddParameter("XapiPassword", group.XapiPassword);
                dbOperator.AddParameter("XapiAddress", group.XapiAddress.Address);
                dbOperator.AddParameter("XapiPort", group.XapiAddress.Port);
                dbOperator.AddParameter("IsPublic", group.IsPublic);
                dbOperator.AddParameter("Description", group.Description);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(Domain.Group group)
        {
            string sql = @"UPDATE Groups SET Name=@Name, OfficeNo=@OfficeNo, XapiName=@XapiName, XapiPassword=@XapiPassword, XapiAddress=@XapiAddress, XapiPort = @XapiPort  IsPublic=@IsPublic,Description=@Description WHERE id=@id";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", group.Name);
                dbOperator.AddParameter("OfficeNo", group.OfficeNo);
                dbOperator.AddParameter("XapiName", group.XapiName);
                dbOperator.AddParameter("XapiPassword", group.XapiPassword);
                dbOperator.AddParameter("XapiAddress", group.XapiAddress.Address);
                dbOperator.AddParameter("XapiPort", group.XapiAddress.Port);
                dbOperator.AddParameter("IsPublic", group.IsPublic);
                dbOperator.AddParameter("Description", group.Description);
                dbOperator.AddParameter("id", group.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(int[] ids)
        {
            string str_ids = DomainService.GetIds(ids);
            string sql = @"DETELE FROM Groups WHERE id in (" + str_ids + " )";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
        
        public Group Query(string pnrCode)
        {
            Group group = default(Group);
            string sql = @"select g.Id, g.Name, g.XapiName, g.XapiPassword, g.XapiAddress, g.XapiPort, g.OfficeNo, g.IsPublic, g.Description" +
            @" from PNRHistories pnr " +
            @" inner join ResourceServiceHistories rs on pnr.ThreadId = rs.ThreadId and pnr.GenerateTime = rs.GenerateTime " +
            @" inner join Groups g on g.Id = rs.GroupId " +
            @" where rs.OperationId = 4 and pnr.PNRCode = @PNRCode;";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("PNRCode", pnrCode);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt16(0);
                        string name = reader.GetString(1);
                        string xapiName = reader.GetString(2);
                        string xapiPassword = reader.GetString(3);
                        IPEndPoint xapiAddress = new IPEndPoint(IPAddress.Parse(reader.GetString(4)), reader.GetInt16(5));
                        string officeNo = reader.GetString(6);
                        bool isPublic = reader.GetBoolean(7);
                        string description = reader.GetString(8);
                        group = new Group(id, name, officeNo, xapiName, xapiPassword, xapiAddress, isPublic, description);
                    }
                }
            }
            return group;
        }
    }
}