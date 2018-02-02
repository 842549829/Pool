using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class Links
    {
        public Guid SettingId { get; set; }
        /// <summary>
        /// 链接名称
        /// </summary>
        public string LinkName { get; set; }
        public string URL { get; set; }
        public string Remark { get; set; }
        public OEMLinkType LinkType { get; set; }
        public override string ToString()
        {
            return string.Format("链接名称{0}，连接地址{1}，备注{2}，连接类型{3}", LinkName, URL, Remark, LinkType == OEMLinkType.Footer ? "头部连接" : "尾部连接");
        }
    }
}