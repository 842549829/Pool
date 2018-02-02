
namespace Izual.Data {
    public abstract class PagingParameter {
        /// <summary>
        /// 获取或设置当前页码
        /// </summary>
        public virtual int PageIndex { get; set; }
        /// <summary>
        /// 获取或设置每页条目数量
        /// </summary>
        public virtual int PageSize { get; set; }
    }
}
