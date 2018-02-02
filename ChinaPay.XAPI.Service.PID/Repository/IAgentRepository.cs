using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    interface IAgentRepository
    {
        IEnumerable<Agent> Query();
        Agent Query(int id);
        int Insert(Agent agent);
        /// <summary>
        /// 通过编号更改数据
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>受影响行数</returns>
        int Update(Agent agent);
        /// <summary>
        /// 通过编号删除数据
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>受影响行数</returns>
        int Delete(int[] ids);
    }
}
