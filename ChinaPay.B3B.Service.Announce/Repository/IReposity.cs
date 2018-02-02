using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Announce.Reposity
{
   interface IAnnounceReposity
    {
       IEnumerable<AnnounceListView> QueryAnnounceList(Guid company, AnnounceQueryCondition condition, Pagination pagination);
       IEnumerable<AnnounceListView> QueryAnnounceList(Guid company, AnnounceQueryCondition condition);
       IEnumerable<AnnounceListView> Query(Guid company,bool domainIsOem,bool companyIsOem,Pagination pagination);
       IEnumerable<Guid> QueryEmergencyIds();
       IEnumerable<Guid> QueryEmergencyIdsByOem(Guid company,bool domainIsOem,bool companyIsOem);
       Guid QueryPlatForm();
       AnnounceView QueryAnnounce(Guid id);
       AduiteStatus QueryAduiteStatus(Guid id);
       int Insert(Domain.Announce announce);
       int Update(Domain.Announce announce);
       int UpdateStatus(Guid id, AduiteStatus status);
       int UpdateStatuses(IEnumerable<Guid> ids, AduiteStatus status);
       int Delete(IEnumerable<Guid> ids);
    }
}
