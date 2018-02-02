using System;

namespace ChinaPay.B3B.DataTransferObject.Permission
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuView
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 排序顺序
        /// </summary>
        public int SortLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// 是否有效
        /// </summary>
        public bool Valid
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display
        {
            get;
            set;
        }

        public virtual void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                throw new ArgumentNullException("name");
        }
    }
}