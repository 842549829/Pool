using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    /// <summary>
    /// 业务操作接口
    /// </summary>
    interface IOperationRepository
    {
        /// <summary>
        /// 获取全部数据；
        /// </summary>
        /// <returns>结果集</returns>
        IEnumerable<Operation> Query();
        /// <summary>
        /// 通过编号获取单个数据
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>单个数据</returns>
        Operation Query(int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="agent">对象</param>
        /// <returns>受影响行数</returns>
        int Insert(Operation operation);
        /// <summary>
        /// 通过编号更改数据
        /// </summary> 
        /// <returns>受影响行数</returns>
        int Update(Operation operation);
        /// <summary>
        /// 通过编号删除数据
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>受影响行数</returns>
        int Delete(int[] id);
    }
}
