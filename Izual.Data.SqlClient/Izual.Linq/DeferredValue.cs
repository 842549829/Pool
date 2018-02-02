#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.DeferredValue.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Linq;

namespace Izual.Linq {
    public struct DeferredValue<T> : IDeferLoadable {
        private bool loaded;
        private IEnumerable<T> source;
        private T value;

        public DeferredValue(T value) {
            this.value = value;
            source = null;
            loaded = true;
        }

        public DeferredValue(IEnumerable<T> source) {
            this.source = source;
            loaded = false;
            value = default(T);
        }

        public bool IsAssigned {
            get { return loaded && source == null; }
        }

        public T Value {
            get {
                Check();
                return value;
            }

            set {
                this.value = value;
                loaded = true;
                source = null;
            }
        }

        #region IDeferLoadable Members

        public void Load() {
            if(source != null) {
                value = source.SingleOrDefault();
                loaded = true;
            }
        }

        public bool IsLoaded {
            get { return loaded; }
        }

        #endregion

        private void Check() {
            if(!IsLoaded) {
                Load();
            }
        }
    }
}