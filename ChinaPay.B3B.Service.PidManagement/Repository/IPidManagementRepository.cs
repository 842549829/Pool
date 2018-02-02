using System;
using ChinaPay.B3B.Service.PidManagement.Domain;

namespace ChinaPay.B3B.Service.PidManagement.Repository
{
    public interface IPidManagementRepository
    {
        PidUsingInformation Query(Guid userId);
        int Insert(PidUsingInformation pidUsingInfo);
        int Update(PidUsingInformation pidUsingInfo);
        int Save(Guid userId, bool isUseB3BConfig);
    }
}
