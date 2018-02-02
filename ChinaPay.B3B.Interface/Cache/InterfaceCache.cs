using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Interface.Cache {
    internal class InterfaceCache<TKey, TValue> {
        private Dictionary<TKey, CacheItem<TValue>> _items;
        private object _locker = new object();

        public InterfaceCache() {
            _items = new Dictionary<TKey, CacheItem<TValue>>();
        }

        public int Timeout { get; set; }

        public TValue this[TKey key] {
            get {
                CacheItem<TValue> result;
                if(_items.TryGetValue(key, out result)) {
                    if(result.ExpiredTime > DateTime.Now) {
                        return result.Value;
                    }
                    lock(_locker) {
                        _items.Remove(key);
                    }
                }
                return default(TValue);
            }
        }

        public void Save(TKey key, TValue value) {
            lock(_locker) {
                if(_items.ContainsKey(key)) {
                    _items[key] = new CacheItem<TValue>(value, Timeout);
                } else {
                    _items.Add(key, new CacheItem<TValue>(value, Timeout));
                }
            }
        }

        public void Remove(TKey key) {
            lock(_locker) {
                if(_items.ContainsKey(key)) {
                    _items.Remove(key);
                }
            }
        }

        class CacheItem<T> {
            public CacheItem(T value, int timeout) {
                Value = value;
                ExpiredTime = DateTime.Now.AddMinutes(timeout);
            }
            public T Value { get; private set; }
            public DateTime ExpiredTime { get; private set; }
        }
    }
}