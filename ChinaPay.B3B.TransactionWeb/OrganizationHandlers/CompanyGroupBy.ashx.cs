using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy.Domain;
using ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    ///公司组
    /// </summary>
    public class CompanyGroupBy : BaseHandler
    {
        public object QueryAirlines()
        {
            var workSetting = CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId);
            var airlines = FoundationService.Airlines.Where(air => workSetting.AirlineForDefault.Contains(air.Code.Value) && air.Valid);
                return from item in airlines.OrderBy(item => item.Code.Value)
                       select new
                       {
                           Code = item.Code.Value,
                           Name = item.ShortName
                       };
        }

        public object QueryAirports()
        {
            SetPolicy setPolicy= Service.Policy.PolicySetService.QuerySetPolicy(this.CurrentCompany.CompanyId);
            if (setPolicy.Departure == null && setPolicy.Departure.Count() <= 0)
                return null;
            return from item in Service.FoundationService.Airports.Where(item => setPolicy.Departure.Contains(item.Code.Value)).OrderBy(item => item.Code.Value)
                   select new
                   {
                       Code = item.Code.Value,
                       Name = item.Name
                   };
        }

        private string[] GetAirport(Guid id)
        {
            var policy = ChinaPay.B3B.Service.Organization.CompanyService.GetPolicySetting(id);
            return policy == null ? null : policy.Departure.Split(',');
        }

        public bool CreateComapnyGroup(Group group)
        {
            try
            {
                //group.CompanyGroupList.ForEach(p=>p.DefaultRebate/=100);
                CompanyService.CreateCompanyGroup(this.CurrentCompany.CompanyId, group.CompanyGroup, group.CompanyGroupList, this.CurrentUser.UserName);
            }
            catch (Exception) { return false; }
            return true;
        }
        /// <summary>
        /// 查询公司组 
        /// <returns></returns>
        public object QueryCompanyGroup(string id)
        {
            var companyGroup = CompanyService.GetCompanyGroupDetailInfo(Guid.Parse(id));
            var policySet = Service.Policy.PolicySetService.QuerySetPolicy(this.CurrentCompany.CompanyId);
            var workSetting = CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId);
            var airport = FoundationService.Airports.Where(air => policySet.Departure.Contains(air.Code.Value) && air.Valid);
            return new
                   {
                       companyGroup.Id,
                       companyGroup.Name,
                       companyGroup.Description,
                       Limit = from item in companyGroup.Limitations
                               select new
                               {
                                   item.Id,
                                   //填写文本框
                                   FobiddenAirPort = item.Departures,
                                   //被限制的航空公司
                                   AirlineCode = from result in Service.FoundationService.Airlines.Where(linq => workSetting.AirlineForDefault.Contains(linq.Code.Value) && linq.Valid).OrderBy(linq => linq.Code.Value)
                                                 select new
                                                 {
                                                     Code = result.Code.Value,
                                                     result.Name,
                                                     Valid = (item.Airlines != null &&
                                                         item.Airlines.IndexOf(result.Code.Value, StringComparison.Ordinal) > -1)
                                                 },
                                   //可出港机场
                                   DepartuesCode = item.Departures == null ? airport.Select(p => new
                                       {
                                           Code = p.Code.Value,
                                           Name = p.Name,

                                       }) :
                                       from result in airport.Where(linq => linq.Valid &&
                                                               item.Departures.IndexOf(linq.Code.Value,
                                                               StringComparison.Ordinal) == -1)
                                                           .OrderBy(linq => linq.Code.Value)
                                       select new
                                       {
                                           Code = result.Code.Value,
                                           result.Name,
                                       },
                                   //限制的出港城市
                                   ForbiddenAirPorts = item.Departures == null ? null :
                                       airport.
                                       Where(linq => linq.Valid && item.Departures.IndexOf(linq.Code.Value,
                                           StringComparison.Ordinal) > -1)
                                       .OrderBy(linq => linq.Code.Value).Select(airPort => new
                                           {
                                               Code = airPort.Code.Value,
                                               airPort.Name,
                                           })
                               }
                   };
        }

        /// <summary>
        /// 更新公司组信息
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool UpdateComapnyGroup(Group group)
        {
            var groupId = new Guid(group.Id);
            var groupDetail = CompanyService.GetCompanyGroupDetailInfo(groupId);
            var groupInfo = CompanyGroupMemberList.ConvertToGroup(groupDetail);
            groupInfo.Name = group.CompanyGroup.Name;
            groupInfo.Description = group.CompanyGroup.Description;
            // groupInfo.AllowExternalPurchase = group.CompanyGroup.AllowExternalPurchase;
            group.CompanyGroupList.ForEach(p =>
                      {
                          if (p.Departures == "*")
                          {
                              p.Departures = string.Join("/",
                                  FoundationService.Airports.Select(q => q.Code.Value));
                          }
                          // p.DefaultRebate /= 100;

                      });

            var result = CompanyService.UpdateCompanyGroup(groupInfo, group.CompanyGroupList, CurrentUser.UserName);
            return result;
        }
    }
    public class Group
    {
        public string Id { get; set; }
        public CompanyGroup CompanyGroup { get; set; }
        public List<CompanyGroupLimitation> CompanyGroupList { get; set; }
    }
}