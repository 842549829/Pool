#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.EnumerateOnce.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Izual.Linq {
    public class EnumerateOnce<T> : IEnumerable<T>, IEnumerable {
        private IEnumerable<T> enumerable;

        public EnumerateOnce(IEnumerable<T> enumerable) {
            this.enumerable = enumerable;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            IEnumerable<T> en = Interlocked.Exchange(ref enumerable, null);
            if(en != null) {
                return en.GetEnumerator();
            }
            throw new Exception("Enumerated more than once.");
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}