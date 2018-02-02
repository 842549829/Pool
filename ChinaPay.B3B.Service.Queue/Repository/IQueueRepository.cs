using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Queue.Repository
{
    using Queue = Domain.Queue;
    interface IQueueRepository
    {
        int Add(Queue queue);

        int Add(List<Queue> queues);
    }
}
