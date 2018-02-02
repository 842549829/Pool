// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.DeferredList.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;

namespace Izual.Linq {
    public class CompoundKey : IEquatable<CompoundKey>, IEnumerable<object>, IEnumerable {
        object[] values;
        int hc;

        public CompoundKey(params object[] values) {
            if (values == null)
                throw new ArgumentNullException("values");

            this.values = values;
            for (int i = 0, n = values.Length; i < n; i++) {
                object value = values[i];
                if (value != null) {
                    hc ^= (value.GetHashCode() + i);
                }
            }
        }

        public override int GetHashCode() {
            return hc;
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public bool Equals(CompoundKey other) {
            if (other == null || other.values.Length != values.Length)
                return false;
            for (int i = 0, n = other.values.Length; i < n; i++) {
                if (!object.Equals(this.values[i], other.values[i]))
                    return false;
            }
            return true;
        }

        public IEnumerator<object> GetEnumerator() {
            return ((IEnumerable<object>)values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}