namespace Izual.Data {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    /// <summary>
    /// 排序方式
    /// </summary>
    public enum OrderMode {
        /// <summary>
        /// 升序
        /// </summary>
        Ascending = 0,
        /// <summary>
        /// 降序
        /// </summary>
        Descending
    }

    public class Pager<T, TKey> {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public Expression<Func<T, TKey>> OrderBy { get; set; }
        public OrderMode OrderMode { get; set; }

        public PagedResult<T> Paging(IQueryable<T> source) {
            if (source == null) throw new ArgumentNullException("source");
            source = OrderMode == OrderMode.Ascending ? source.OrderBy(OrderBy) : source.OrderByDescending(OrderBy);

            var result = PageIndex == 1
                ? (from item in source
                   let count = source.Count()
                   select new { Count = count, Data = source.Take(PageSize) }).FirstOrDefault()
                : (from item in source
                   select source.Skip(PageSize * PageIndex - 1) into tmp
                   let count = source.Count()
                   select new { Count = count, Data = tmp.Take(PageSize) }).FirstOrDefault();
            return result == null ? new PagedResult<T>(0, PageSize, PageIndex, new T[] { }) : new PagedResult<T>(result.Count, PageSize, PageIndex, result.Data);
        }
    }
    public class PagedResult<T> : IEnumerable<T> {
        internal PagedResult(int rowCount, int pageSize, int pageIndex, IEnumerable<T> data) {
            this.rowCount = rowCount;
            this.pageSize = pageSize;
            this.pageIndex = pageIndex;
            this.data = data;
        }

        private readonly int rowCount;
        private readonly int pageSize;
        private readonly int pageIndex;
        private readonly IEnumerable<T> data;

        /// <summary>
        /// 记录总数
        /// </summary>
        public int RowCount {
            get { return rowCount; }
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize {
            get { return pageSize; }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex {
            get { return pageIndex; }
        }

        public int PageCount {
            get { return pageSize > 0 ? (rowCount + pageSize - 1) / pageSize : 0; }
        }
        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator() {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}