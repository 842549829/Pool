using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ChinaPay.B3B.Service.Command
{
    /// <summary>
    /// 产生1-9999间的事务编号
    /// </summary>
    class TransactionUtil
    {
        public static int TransactionId 
        {
            get
            {
                if (transactionId == 9999)
                {
                    Interlocked.Exchange(ref transactionId, 0);
                }

                return Interlocked.Increment(ref transactionId);
            }
        }

        private static int transactionId = 0;
    }
}
