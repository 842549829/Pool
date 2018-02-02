using System;
using ChinaPay.B3B.Service.PidManagement.Domain;
using ChinaPay.B3B.Service.PidManagement.Repository;

namespace ChinaPay.B3B.Service.PidManagement
{
    public class PidManagementService
    {
        public static User GetUser()
        {
            return new User("schedual", "schedual");
        }


        /// <summary>
        /// 更新计数器；
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="isUseB3BConfig">是否使用B3B配置</param>
        public static void SaveCounter(Guid userId, bool isUseB3BConfig)
        {
            var repository = Factory.CreatePidManagementRepository();
            repository.Save(userId, isUseB3BConfig);
        }
    }
}
