#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.Grouping.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Izual.Linq {
    /// <summary>
    /// Simple implementation of the IGrouping <TKey, TElement>interface
    /// </summary>
    /// <typeparam name="TKey"> </typeparam>
    /// <typeparam name="TElement"> </typeparam>
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement> {
        private readonly TKey key;
        private IEnumerable<TElement> group;

        public Grouping(TKey key, IEnumerable<TElement> group) {
            this.key = key;
            this.group = group;
        }

        #region IGrouping<TKey,TElement> Members

        public TKey Key {
            get { return key; }
        }

        public IEnumerator<TElement> GetEnumerator() {
            if(!(group is List<TElement>))
                group = group.ToList();
            return @group.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return @group.GetEnumerator();
        }

        #endregion
    }
}