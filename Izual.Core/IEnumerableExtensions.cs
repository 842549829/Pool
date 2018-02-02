#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Core.IEnumerableExtensions.cs
// description：
// 
// create by：Izual ,2012/06/11
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Izual {
    /// <summary>
    /// 针对接口 IEnumerable&lt;T>&gt;
    /// </summary>
    public static class EnumerableExtensions {
        /// <summary>
        /// 为所有元素调用指定的方法
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="set"> 被扩展的 IEnumerable&lt;T&gt; </param>
        /// <param name="action"> 要为元素调用的方法 </param>
        public static void ForEach<T>(this IEnumerable<T> set, Action<T> action) {
            if(action == null) return;
            foreach(T item in set) {
                action(item);
            }
        }

        /// <summary>
        /// 为所有元素调用指定的方法，除非它满足排除条件
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="set"> 被扩展的 IEnumerable&lt;T&gt; </param>
        /// <param name="action"> 要为元素调用的方法 </param>
        /// <param name="except"> 排除条件谓词 </param>
        public static void ForEachExcept<T>(this IEnumerable<T> set, Action<T> action, Func<T, bool> except) {
            if(action == null) return;
            foreach(T item in set.Where(item => except == null || !except(item))) {
                action(item);
            }
        }

        /// <summary>
        /// 为所有元素调用指定的方法，在满足指定条件时终止循环
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="set"> 被扩展的 IEnumerable&lt;T&gt; </param>
        /// <param name="action"> 要为元素调用的方法 </param>
        /// <param name="predicate"> 终止循环的条件 </param>
        public static void ForEachUntil<T>(this IEnumerable<T> set, Action<T> action, Func<T, bool> predicate) {
            if(action == null) return;
            if(predicate == null) {
                ForEach(set, action);
            }
            else {
                foreach(T item in set) {
                    if(predicate(item)) {
                        return;
                    }
                    action(item);
                }
            }
        }

        /// <summary>
        /// 向集合中添加元素
        /// </summary>
        /// <typeparam name="T"> 集合中元素的类型 </typeparam>
        /// <param name="set"> 被扩展的属性 </param>
        /// <param name="item"> 要添加的元素 </param>
        /// <returns> 成功添加返回 true,失败返回 false </returns>
        public static bool Add<T>(this ICollection<T> set, T item) {
            try {
                set.Add(item);
            }
            catch {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 向集合中添加多个元素
        /// </summary>
        /// <typeparam name="T"> 集合中元素的类型 </typeparam>
        /// <param name="set"> 被扩展的属性 </param>
        /// <param name="items"> 要添加的元素列表 </param>
        /// <returns> 返回成功添加的元素数量 </returns>
        public static int AddRange<T>(this ICollection<T> set, IEnumerable<T> items) {
            if(set.IsReadOnly) {
                throw new InvalidOperationException("只读集合（属性 IsReadOnly == true），不允许添加元素。");
            }
            return items == null ? 0 : items.Count(item => Add(set, item));
        }

        /// <summary>
        /// 更新所有元素
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="set"> 被扩展的 IEnumerable&lt;T&gt; </param>
        /// <param name="action"> 更新方法 </param>
        /// <returns> 返回更新的元素数量 </returns>
        public static int Update<T>(this IEnumerable<T> set, Action<T> action) {
            return Update(set, action, null);
        }

        /// <summary>
        /// 更新满足指定的条件元素
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="set"> 被扩展的 IEnumerable&lt;T&gt; </param>
        /// <param name="action"> 更新方法 </param>
        /// <param name="predicate"> 条件谓词 </param>
        /// <returns> 返回更新的元素数量 </returns>
        public static int Update<T>(this IEnumerable<T> set, Action<T> action, Func<T, bool> predicate) {
            if(action == null) {
                return 0;
            }
            IEnumerable<T> items = set.ToList().Where(item => predicate != null && predicate(item));
            int count = 0;
            foreach(T item in items) {
                action(item);
                count++;
            }
            return count;
        }

        /// <summary>
        /// 删除满足条件的元素
        /// </summary>
        /// <typeparam name="T"> 元素类型 </typeparam>
        /// <param name="set"> 被扩展的 IList&lt;T&gt; </param>
        /// <param name="predicate"> 条件谓词 </param>
        /// <returns> 返回删除的元素数量 </returns>
        /// <remarks>
        /// 如果条件谓词 predicate 为 null，将删除所有元素
        /// </remarks>
        public static int Delete<T>(this ICollection<T> set, Func<T, bool> predicate) {
            // ICollection 里面的元素没有顺序，不能通过下标访问，所以只能通过 foreach 遍历
            // foreach 在进行迭代的时候不能删除元素（并且 ICollection 接口也不提供删除元素的方法）
            // 所以以下是比较恶心的实现：
            // 1、遍历所有元素，将不满足条件的元素添加到新的列表
            // 2、清空原 set 中的元素
            // 3、将保留下来的元素重新添加回原 set
            if(set.IsReadOnly) {
                throw new InvalidOperationException("只读集合（属性 IsReadOnly == true），不允许删除元素。");
            }
            int count = set.Count;
            ICollection<T> retains = predicate == null ? set : set.Where(item => !predicate(item)).ToList();
            set.Clear();
            return predicate == null ? count : count - set.AddRange(retains);
        }

        /// <summary>
        /// 查找元素在序列中首次出现的位置
        /// </summary>
        /// <typeparam name="T"> 序列中元素的类型 </typeparam>
        /// <param name="set"> 被扩展的序列 </param>
        /// <param name="item"> 要查找的元素 </param>
        /// <returns> 元素在序列中首次出现的位置。如果元素不在序列中，返回 -1. </returns>
        public static int IndexOf<T>(this IEnumerable<T> set, T item) {
            for(int pos = 0; pos < set.Count(); pos++) {
                if(set.ElementAt(pos).Equals(item)) {
                    return pos;
                }
            }
            return -1;
        }

        /// <summary>
        /// 查找元素的匹配项在序列中首次出现的位置
        /// </summary>
        /// <typeparam name="T"> 序列中元素的类型 </typeparam>
        /// <param name="set"> 被扩展的序列 </param>
        /// <param name="item"> 要查找的元素 </param>
        /// <param name="comparer"> 用于比较元素相等的比较器 </param>
        /// <returns> 元素在序列中首次出现的位置。如果元素不在序列中，返回 -1. </returns>
        public static int IndexOf<T>(this IEnumerable<T> set, T item, IEqualityComparer<T> comparer) {
            if(set == null)
                return 0;

            if(comparer == null) {
                return IndexOf(set, item);
            }
            for(int pos = 0; pos < set.Count(); pos++) {
                if(comparer.Equals(set.ElementAt(pos), item)) {
                    return pos;
                }
            }
            return -1;
        }

        /// <summary>
        /// 将序列转换为只读集合
        /// </summary>
        /// <typeparam name="TElement"> 元素类型 </typeparam>
        /// <param name="set"> 被扩展的序列 </param>
        /// <returns> </returns>
        public static ReadOnlyCollection<TElement> ToReadOnly<TElement>(this IEnumerable<TElement> set) {
            var roc = set as ReadOnlyCollection<TElement>;
            if(roc == null) {
                if(set == null) {
                    roc = EmptyReadOnlyCollection<TElement>.Empty;
                }
                else {
                    roc = new List<TElement>(set).AsReadOnly();
                }
            }
            return roc;
        }

        /// <summary>
        /// </summary>
        /// <param name="set"> </param>
        /// <param name="op"> </param>
        /// <returns> </returns>
        public static string Join(this IEnumerable<string> set, string op) {
            return set.ToArray().Join(op);
        }

        /// <summary>
        /// </summary>
        /// <param name="set"> </param>
        /// <param name="op"> </param>
        /// <returns> </returns>
        public static string Join(this string[] set, string op) {
            var sb = new StringBuilder();
            for(int i = 0; i < set.Length; i++) {
                if(i > 0) {
                    sb.Append(op);
                }
                sb.Append(set[i]);
            }
            return sb.ToString();
        }

        #region Nested type: EmptyReadOnlyCollection

        private class EmptyReadOnlyCollection<T> {
            internal static readonly ReadOnlyCollection<T> Empty = new List<T>().AsReadOnly();
        }

        #endregion
    }
}