using ChinaPay.B3B.DataTransferObject;

namespace ChinaPay.B3B.Service.SystemManagement.Domain {
    using System;
    using System.Collections.Generic;
    using System.Timers;
    using ChinaPay.B3B.Service.SystemManagement.Repository;

    internal class SystemDictionarys {
        static SystemDictionarys _instance;
        static object _locker = new object();
        public static SystemDictionarys Instance {
            get {
                if (null == _instance) {
                    lock (_locker) {
                        if (null == _instance) {
                            _instance = new SystemDictionarys();
                        }
                    }
                }
                return _instance;
            }
        }

        Timer _timer;
        ChinaPay.Data.Cache<SystemDictionaryType, SystemDictionary> _datasCache;
        readonly double _interval = 60 * 1000;
        private SystemDictionarys() {
            _datasCache = new ChinaPay.Data.Cache<SystemDictionaryType, SystemDictionary>();
            _datasCache.AddRange(getSystemDictionarys());
            _timer = new Timer();
            _timer.Interval = _interval;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            RefreshService.ServicePhoneChanged += refreshData;
            _timer.Start();
        }
        public SystemDictionary this[SystemDictionaryType type] {
            get {
                return _datasCache[type];
            }
        }
        public void UpdateItem(SystemDictionaryType type, SystemDictionaryItem item) {
            updateItem(type, item);
            _datasCache.Save(type, (dictionaryItem) => {
                dictionaryItem[item.Id].Name = item.Name;
                dictionaryItem[item.Id].Value = item.Value;
                dictionaryItem[item.Id].Remark = item.Remark;
                return true;
            });
        }
        public void DeleteItem(SystemDictionaryType type, Guid item) {
            deleteItem(type, item);
            _datasCache.Save(type, (dictionaryItem) => {
                dictionaryItem.RemoveItem(item);
                return true;
            });
        }
        public void AddItem(SystemDictionaryType type, SystemDictionaryItem item) {
            insertItem(type, item);
            _datasCache.Save(type,
                () => {
                    var data = new SystemDictionary(type);
                    data.AddItem(item);
                    return data;
                },
                (dictionaryItem) => {
                    dictionaryItem.AddItem(item);
                    return true;
                });
        }
        IEnumerable<KeyValuePair<SystemDictionaryType, SystemDictionary>> getSystemDictionarys() {
            var result = new List<KeyValuePair<SystemDictionaryType, SystemDictionary>>();
            var datas = getRepository().Query();
            if (datas != null) {
                foreach (var item in datas) {
                    result.Add(new KeyValuePair<SystemDictionaryType, SystemDictionary>(item.Type, item));
                }
            }
            return result;
        }
        void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            refreshData();
        }
        void refreshData() {
            var datas = getSystemDictionarys();
            _datasCache.Refresh(datas);
        }
        void updateItem(SystemDictionaryType type, SystemDictionaryItem item) {
            var repository = getRepository();
            repository.UpdateItem(type, item);
        }
        void insertItem(SystemDictionaryType type, SystemDictionaryItem item) {
            var repository = getRepository();
            repository.InsertItem(type, item);
        }
        void deleteItem(SystemDictionaryType type, Guid item) {
            var repository = getRepository();
            repository.DeleteItem(type, item);
        }
        ISystemDictionaryRepository getRepository() {
            return Factory.CreateSystemDictionaryRepository();
        }
    }
}