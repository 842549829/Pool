namespace ChinaPay.B3B.Service.SystemManagement.Domain {
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 系统字典
    /// </summary>
    public class SystemDictionary {
        ChinaPay.Data.Cache<Guid, SystemDictionaryItem> _itemsCache;
        internal SystemDictionary(SystemDictionaryType type) {
            this.Type = type;
            _itemsCache = new ChinaPay.Data.Cache<Guid, SystemDictionaryItem>();
        }
        public SystemDictionaryType Type {
            get;
            private set;
        }
        /// <summary>
        /// 子项
        /// </summary>
        public IEnumerable<SystemDictionaryItem> Items {
            get { return _itemsCache.Values; }
        }
        internal SystemDictionaryItem this[Guid id] {
            get {
                return _itemsCache[id];
            }
        }
        internal void AddItem(SystemDictionaryItem item) {
            _itemsCache.Add(item.Id, item);
        }
        internal void RemoveItem(Guid id) {
            _itemsCache.Remove(id);
        }
    }
}