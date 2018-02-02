using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// oem风格
    /// </summary>
    public class OEMStyle
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 风格名字
        /// </summary>
        public string StyleName { get; set; }
        /// <summary>
        /// 风格路径
        /// </summary>
        public List<string> StylePath { get; set; }
        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumbnailPicture { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}
