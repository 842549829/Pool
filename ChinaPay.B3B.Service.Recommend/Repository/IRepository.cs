using System;
using System.Collections.Generic;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Recommend.Repository {
    interface IFlightLowerFareRepository {
        void Save(Domain.FareInfo fare);
        IEnumerable<Domain.FareInfo> Query();
    }
}