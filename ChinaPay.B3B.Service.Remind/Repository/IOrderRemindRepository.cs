using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Remind.Model;

namespace ChinaPay.B3B.Service.Remind.Repository {
    interface IOrderRemindRepository {
        IEnumerable<RemindInfo> Query();
        void Save(RemindInfo remindInfo);
        void Delete(decimal id);
        ProviderRemindView QueryProviderRemindInfo(Guid provider);
        SupplierRemindView QuerySupplierRemindInfo(Guid supplier);
    }
}