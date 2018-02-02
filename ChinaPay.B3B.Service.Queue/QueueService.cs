using System.Collections.Generic;
using ChinaPay.B3B.Service.Queue.Repository;

namespace ChinaPay.B3B.Service.Queue
{
    using Queue = Domain.Queue;
    public class QueueService
    {
        public static bool AddQueue(Queue queue)
        {
            var repository = Factory.CreateQueueRepository();
            return repository.Add(queue) > 0;
        }

        public static bool AddQueues(List<Queue> queues)
        {
            var repository = Factory.CreateQueueRepository();
            return repository.Add(queues) > 0;
        }
    }
}
