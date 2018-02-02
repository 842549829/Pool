#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.ReadOnlyExtensions.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Izual.Linq {
    public static class ReadOnlyExtensions {
        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> collection) {
            var roc = collection as ReadOnlyCollection<T>;
            if(roc == null) {
                if(collection == null) {
                    roc = EmptyReadOnlyCollection<T>.Empty;
                }
                else {
                    roc = new List<T>(collection).AsReadOnly();
                }
            }
            return roc;
        }

        #region Nested type: EmptyReadOnlyCollection

        private class EmptyReadOnlyCollection<T> {
            internal static readonly ReadOnlyCollection<T> Empty = new List<T>().AsReadOnly();
        }

        #endregion
    }
}