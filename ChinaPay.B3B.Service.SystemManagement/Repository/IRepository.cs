namespace ChinaPay.B3B.Service.SystemManagement.Repository {
    using System.Collections.Generic;
    using ChinaPay.B3B.Service.SystemManagement.Domain;
    using ChinaPay.B3B.DataTransferObject.SystemManagement;

    internal interface ISystemParamRepository : ChinaPay.Data.RepositoryCache<SystemParamType, SystemParam>.IRepository {
        int Update(SystemParamType type, string value);
    }
    internal interface ISystemDictionaryRepository {
        IEnumerable<SystemDictionary> Query();
        int UpdateItem(SystemDictionaryType type, SystemDictionaryItem item);
        int InsertItem(SystemDictionaryType type, SystemDictionaryItem item);
        int DeleteItem(SystemDictionaryType type, System.Guid id);
    }
    internal interface ISpecialProductRepository : ChinaPay.Data.RepositoryCache<ChinaPay.B3B.Common.Enums.SpecialProductType, SpecialProductView>.IRepository {
    }
}
