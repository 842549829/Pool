using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Repository {
    internal interface IAreaRepository : RepositoryCache<string, Area>.IRepository {
    }
    internal interface IProvinceRepository : RepositoryCache<string, Province>.IRepository {
    }
    internal interface ICityRepository : RepositoryCache<string, City>.IRepository {
    }
    internal interface ICountyRepository : RepositoryCache<string, County>.IRepository {
    }
    internal interface IAirportRepository : RepositoryCache<UpperString, Airport>.IRepository {
    }
    internal interface IAirlineRepository : RepositoryCache<UpperString, Airline>.IRepository {
    }
    internal interface IAirCraftRepository : RepositoryCache<Guid, AirCraft>.IRepository {
    }
    internal interface IBAFRepository : RepositoryCache<Guid, BAF>.IRepository {
    }
    internal interface IBasicPriceRepository : RepositoryCache<Guid, BasicPrice>.IRepository {
        IEnumerable<BasicPriceView> QueryBasicPrice(string airline, string departure, string arrival, Pagination pagination);
        BasicPrice QueryBasicPrice(Guid basicPriceId);
    }
    internal interface IBunkRepository : RepositoryCache<Guid, Bunk>.IRepository {
        IEnumerable<Bunk> QueryBunkListView(BunkQueryCondition condition, Pagination pagination);
        Bunk QueryBunkNew(Guid id);
    }
    internal interface IChildOrderableBunkRepository : RepositoryCache<Guid, ChildOrderableBunk>.IRepository {
    }
    internal interface IRefundAndReschedulingRepository {
        IEnumerable<RefundAndRescheduling> Query();
        RefundAndRescheduling Query(UpperString airlineCode);
        int Insert(RefundAndRescheduling item);
        int Update(RefundAndRescheduling item);
        int Delete(UpperString airlineCode);
    }
    internal interface IRefundAndReschedulingNewRepository {
        IEnumerable<RefundAndReschedulingBase> Query();
        RefundAndReschedulingBase Query(UpperString airlineCode);
        int Insert(RefundAndReschedulingBase item);
        int Update(RefundAndReschedulingBase item);
        int Delete(UpperString airlineCode);
        int Insert(RefundAndReschedulingDetail item);
        int Update(RefundAndReschedulingDetail item);
        int Delete(Guid detailId);
        RefundAndReschedulingDetail Query(Guid detailId);
        IEnumerable<RefundAndReschedulingDetail> Query(string airline);
        IEnumerable<RefundAndReschedulingDetailView> Query(string airline,string bunk);
        IEnumerable<AirlineRulesView> QueryAllRefundAndReschedulings(string airline);
        RefundAndReschedulingBase QueryRefundAndRescheduling(UpperString airline);
    }
    internal interface IFixedNavigationRepository : RepositoryCache<string, ChinaPay.B3B.DataTransferObject.Foundation.FixedNavigationView>.IRepository { 
    }
    internal interface ICheck_InRepository : RepositoryCache<Guid, Check_In>.IRepository
    { 
    
    } 
}