using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Interface.Cache {
    internal class ContextCenter {
        private static ContextCenter _instance;
        private static object _locker = new object();
        private string _timeOut = System.Configuration.ConfigurationManager.AppSettings["PolicyTimeOut"];
        public static ContextCenter Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new ContextCenter();
                        }
                    }
                }
                return _instance;
            }
        }

        private InterfaceCache<string, CustomContext> _cache = null;
        private ContextCenter() {
            _cache = new InterfaceCache<string, CustomContext> {
                Timeout = int.Parse(_timeOut)
            };
        }

        public CustomContext this[string id]
        {
            get {
                return _cache[id];
            }
        }
        public void Save(CustomContext context) {
            _cache.Save(context.Id, context);
        }
        public void Remove(string id) {
            _cache.Remove(id);
        }
    }
    internal class CustomContext {
        private Dictionary<string, object> _items;
        private object _locker;

        private CustomContext() {
            Id = Guid.NewGuid().ToString();
            _items = new Dictionary<string, object>();
            _locker = new object();
        }

        public static CustomContext NewContext() {
            return new CustomContext();
        }

        public string Id { get; private set; }
        public object this[string key] {
            get {
                object value;
                _items.TryGetValue(key, out value);
                return value;
            }
            set {
                lock(_locker) {
                    if(_items.ContainsKey(key)) {
                        _items[key] = value;
                    } else {
                        _items.Add(key, value);
                    }
                }
            }
        }
    }
}