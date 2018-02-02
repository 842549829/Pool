using System;
using System.Collections.Generic;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.ReleaseNote.Repository
{
    interface IReleaseNoteRepository
    {
        IEnumerable<Domain.ReleaseNote> Query(Pagination paination, DateTime? startTime, DateTime? endTime, ChinaPay.B3B.Common.Enums.CompanyType? type, ChinaPay.B3B.Common.Enums.ReleaseNoteType? releaseType);
        Domain.ReleaseNote Query(Guid id);
        void Update(Domain.ReleaseNote note);
        void Add(Domain.ReleaseNote note);
        void Delete(Guid id);
    }
}
