using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    public class GroupResource
    {
        public GroupResource(int groupId, int resourceId)
        {
            this.GroupId = groupId;
            this.ResourceId = resourceId;
        }
        public int GroupId { get; set; }
        public int ResourceId { get; set; }
    }
}
