using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class Links
    {
        public Guid SettingId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string LinkName { get; set; }
        public string URL { get; set; }
        public string Remark { get; set; }
        public OEMLinkType LinkType { get; set; }
        public override string ToString()
        {
            return string.Format("��������{0}�����ӵ�ַ{1}����ע{2}����������{3}", LinkName, URL, Remark, LinkType == OEMLinkType.Footer ? "ͷ������" : "β������");
        }
    }
}