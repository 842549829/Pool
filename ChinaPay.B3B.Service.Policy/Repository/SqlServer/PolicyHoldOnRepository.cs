using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Policy.HoldOn;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Policy.Repository.SqlServer {
    using Common.Enums;

    class PolicyHoldOnRepository : SqlServerRepository, IPolicyHoldOnRepository {
        public PolicyHoldOnRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<HoldOnListView> QueryList(Guid? publisher) {
            var result = new List<HoldOnListView>();
            string sql = "SELECT tHoldOn.[Company],tCompany.[Type],tCompany.[AbbreviateName],tHoldOn.[Airline],tHoldOn.[TicketType],tHoldOn.[Role]" +
                         "FROM [dbo].[T_HoldOnPolicy] tHoldOn INNER JOIN [dbo].[T_Company] tCompany ON tHoldOn.[Company]=tCompany.[Id]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(publisher.HasValue) {
                    sql += " WHERE tHoldOn.[Company]=@COMPANY";
                    dbOperator.AddParameter("COMPANY", publisher.Value);
                }
                sql += " ORDER BY tHoldOn.[Company]";
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    HoldOnListView holdOnView = null;
                    var platformItems = new List<HoldOnItem>();
                    var publisherItems = new List<HoldOnItem>();
                    while(reader.Read()) {
                        Guid currentCompany = reader.GetGuid(0);
                        if(holdOnView == null || holdOnView.Company != currentCompany) {
                            holdOnView = new HoldOnListView() {
                                Company = currentCompany,
                                CompanyType = (CompanyType)reader.GetInt32(1),
                                AbbreviateName = reader.GetString(2),
                                Platform = platformItems = new List<HoldOnItem>(),
                                Publisher = publisherItems = new List<HoldOnItem>()
                            };
                            result.Add(holdOnView);
                        }
                        HoldOnItem holdOnItem = new HoldOnItem() {
                            Airline = reader.GetString(3)
                        };
                        if(!reader.IsDBNull(4)) {
                            holdOnItem.TicketType = (TicketType)reader.GetInt32(4);
                        }
                        PolicyOperatorRole role = (PolicyOperatorRole)reader.GetInt32(5);
                        switch(role) {
                            case PolicyOperatorRole.Provider:
                                publisherItems.Add(holdOnItem);
                                break;
                            case PolicyOperatorRole.Resourcer:
                                publisherItems.Add(holdOnItem);
                                break;
                            case PolicyOperatorRole.Platform:
                                platformItems.Add(holdOnItem);
                                break;
                        }
                    }
                }
            }
            return result;
        }

        public HoldOnView Query(Guid publisher) {
            var platformItems = new List<HoldOnItem>();
            var publisherItems = new List<HoldOnItem>();
            string sql = "SELECT [Airline],[TicketType],[Role] FROM [dbo].[T_HoldOnPolicy] WHERE [Company]=@COMPANY";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("COMPANY", publisher);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        HoldOnItem item = new HoldOnItem() {
                            Airline = reader.GetString(0)
                        };
                        if(!reader.IsDBNull(1)) {
                            item.TicketType = (TicketType)reader.GetInt32(1);
                        }
                        PolicyOperatorRole role = (PolicyOperatorRole)reader.GetInt32(2);
                        switch(role) {
                            case PolicyOperatorRole.Provider:
                                publisherItems.Add(item);
                                break;
                            case PolicyOperatorRole.Resourcer:
                                publisherItems.Add(item);
                                break;
                            case PolicyOperatorRole.Platform:
                                platformItems.Add(item);
                                break;
                        }
                    }
                }
            }
            return new HoldOnView() {
                Platform = platformItems,
                Publisher = publisherItems
            };
        }

        public int HoldOn(Guid publisher, IEnumerable<HoldOnItem> items, PolicyOperatorRole role) {
            if(items == null || items.Count() == 0)
                return 0;
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO [dbo].[T_HoldOnPolicy]([Company],[TicketType],[Airline],[Role])");
            foreach(var item in items) {
                sql.AppendFormat(" SELECT '{0}',{1},'{2}',{3} UNION ALL",
                    publisher,
                    item.TicketType.HasValue ? ((int)item.TicketType.Value).ToString() : "NULL",
                    item.Airline.ToUpper(),
                    (int)role);
            }
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                return dbOperator.ExecuteNonQuery(sql.Remove(sql.Length - 10, 10).ToString());
            }
        }

        public int UnHoldOn(Guid publisher, IEnumerable<HoldOnItem> items, PolicyOperatorRole role) {
            if(items == null || items.Count() == 0)
                return 0;
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("DELETE FROM [dbo].[T_HoldOnPolicy] WHERE [Company]='{0}' AND [Role]='{1}'", publisher, (int)role);
            sql.Append(" AND (");
            foreach(var item in items) {
                if(item.TicketType.HasValue) {
                    sql.AppendFormat(" ([Airline]='{0}' AND [TicketType]='{1}') OR", item.Airline.ToUpper(), (int)item.TicketType.Value);
                } else {
                    sql.AppendFormat(" [Airline]='{0}' OR", item.Airline.ToUpper());
                }
            }
            sql.Remove(sql.Length - 3, 3);
            sql.Append(")");
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                return dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }
    }
}