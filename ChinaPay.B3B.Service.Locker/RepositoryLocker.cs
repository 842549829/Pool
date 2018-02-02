using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.Service.Locker {
    class RepositoryLocker : ILocker {
        Repository.ILockRepository repository = new Repository.SqlServer.LockRepository(ChinaPay.Repository.ConnectionManager.LockConnectionString);

        LockInfo ILocker.Lock(LockInfo lockInfo) {
            return repository.Lock(lockInfo);
        }

        bool ILocker.UnLock(string key, string account) {
            return repository.UnLock(key, account);
        }

        void ILocker.UnLockForcibly(string key) {
            repository.UnLockForcibly(key);
        }

        LockInfo ILocker.Query(string key) {
            return repository.Query(key);
        }

        IEnumerable<LockInfo> ILocker.Query(IEnumerable<string> keys) {
            if (!keys.Any()) return new LockInfo[0];
            return repository.Query(keys);
        }
    }
}
