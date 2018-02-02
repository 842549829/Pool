using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    interface IGroupRepository
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        IEnumerable<Group> Query();
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Group Query(int id);

        Group Query(string pnrCode);

        int Insert(Group agent);
        /// <summary>
        /// 通过编号更改数据
        /// </summary> 
        /// <returns>受影响行数</returns>
        int Update(Group agent);
        /// <summary>
        /// 通过编号删除数据
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>受影响行数</returns>
        int Delete(int[] id);
    }
}
