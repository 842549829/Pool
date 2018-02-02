#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.TopologicalSort.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;

namespace Izual.Linq {
    public static class TopologicalSorter {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> fnItemsBeforeMe) {
            return Sort(items, fnItemsBeforeMe, null);
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> fnItemsBeforeMe, IEqualityComparer<T> comparer) {
            HashSet<T> seen = comparer != null ? new HashSet<T>(comparer) : new HashSet<T>();
            HashSet<T> done = comparer != null ? new HashSet<T>(comparer) : new HashSet<T>();
            var result = new List<T>();
            foreach(T item in items) {
                SortItem(item, fnItemsBeforeMe, seen, done, result);
            }
            return result;
        }

        private static void SortItem<T>(T item, Func<T, IEnumerable<T>> fnItemsBeforeMe, HashSet<T> seen, HashSet<T> done, List<T> result) {
            if(!done.Contains(item)) {
                if(seen.Contains(item)) {
                    throw new InvalidOperationException("Cycle in topological sort");
                }
                seen.Add(item);
                IEnumerable<T> itemsBefore = fnItemsBeforeMe(item);
                if(itemsBefore != null) {
                    foreach(T itemBefore in itemsBefore) {
                        SortItem(itemBefore, fnItemsBeforeMe, seen, done, result);
                    }
                }
                result.Add(item);
                done.Add(item);
            }
        }
    }
}