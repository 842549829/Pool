using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Data;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.B3B.Common.Enums;
namespace ChinaPay.B3B.Service.Organization.Domain
{
    internal class Accounts
    {
        private static Accounts _instance = null;
        private static  object _locker = new object();
        public static Accounts Instance {
            get {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance== null)
                        {
                            _instance = new Accounts();
                        }
                    }
                }
                return _instance;
            }
        }
        RepositoryCache<string, Domain.Account> _repositoryCache;
        const double _interval = 3 * 60 * 1000;
        private Accounts() {
            IAccountRepository repository = Repository.Factory.CreateAccountRepository();
           _repositoryCache = new RepositoryCache<string, Account>(repository,_interval);
        }
        public Account this[string key]{
            get{
                return _repositoryCache[key];
            }
        }
        public IEnumerable<Account> Query()
        {
            return _repositoryCache.Values;
        }
    }
}
