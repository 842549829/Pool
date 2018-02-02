using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class CompanyDrawdition
    {
        public Guid Id { get; set; }
        public byte Type { get; set; }
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public override string ToString()
        {
            return string.Format("编号：{0}，类型：{1}，公司编号：{2}，标题：{3}，内容：{4}", Id, Type == 0 ? "出票条件" : "政策备注", OwnerId, Title, Context);
        }
    }
}
