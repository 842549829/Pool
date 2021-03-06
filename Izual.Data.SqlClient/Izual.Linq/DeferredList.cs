﻿#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.DeferredList.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace Izual.Linq {
    /// <summary>
    /// Common interface for controlling defer-loadable types
    /// </summary>
    public interface IDeferLoadable {
        bool IsLoaded{ get; }
        void Load();
    }

    public interface IDeferredList : IList, IDeferLoadable {}

    public interface IDeferredList<T> : IList<T>, IDeferredList {}

    /// <summary>
    /// A list implementation that is loaded the first time the contents are examined
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class DeferredList<T> : IDeferredList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, IDeferLoadable {
        private readonly IEnumerable<T> source;
        private List<T> values;

        public DeferredList(IEnumerable<T> source) {
            this.source = source;
        }

        #region IDeferredList<T> Members

        public void Load() {
            values = new List<T>(source);
        }

        public bool IsLoaded {
            get { return values != null; }
        }

        public int IndexOf(T item) {
            Check();
            return values.IndexOf(item);
        }

        public void Insert(int index, T item) {
            Check();
            values.Insert(index, item);
        }

        public void RemoveAt(int index) {
            Check();
            values.RemoveAt(index);
        }

        public T this[int index] {
            get {
                Check();
                return values[index];
            }
            set {
                Check();
                values[index] = value;
            }
        }

        public void Add(T item) {
            Check();
            values.Add(item);
        }

        public void Clear() {
            Check();
            values.Clear();
        }

        public bool Contains(T item) {
            Check();
            return values.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            Check();
            values.CopyTo(array, arrayIndex);
        }

        public int Count {
            get {
                Check();
                return values.Count;
            }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(T item) {
            Check();
            return values.Remove(item);
        }

        public IEnumerator<T> GetEnumerator() {
            Check();
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int Add(object value) {
            Check();
            return ((IList)values).Add(value);
        }

        public bool Contains(object value) {
            Check();
            return ((IList)values).Contains(value);
        }

        public int IndexOf(object value) {
            Check();
            return ((IList)values).IndexOf(value);
        }

        public void Insert(int index, object value) {
            Check();
            ((IList)values).Insert(index, value);
        }

        public bool IsFixedSize {
            get { return false; }
        }

        public void Remove(object value) {
            Check();
            ((IList)values).Remove(value);
        }

        object IList.this[int index] {
            get {
                Check();
                return ((IList)values)[index];
            }
            set {
                Check();
                ((IList)values)[index] = value;
            }
        }

        public void CopyTo(Array array, int index) {
            Check();
            ((IList)values).CopyTo(array, index);
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { return null; }
        }

        #endregion

        private void Check() {
            if(!IsLoaded) {
                Load();
            }
        }
    }
}