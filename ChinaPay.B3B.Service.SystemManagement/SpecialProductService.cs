using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Repository;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.SystemManagement
{
    /// <summary>
    /// 特殊产品服务类
    /// </summary>
    public static class SpecialProductService
    {
        /// <summary>
        /// 查询特殊产品
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SpecialProductView> Query() 
        {
            return SpecialProducts.Instance.Query();
        }
        public static SpecialProductView Query(SpecialProductType specialProductType) 
        {
            return SpecialProducts.Instance[specialProductType];
        }
        /// <summary>
        /// 修改特殊产品
        /// </summary>
        /// <param name="view"></param>
        public static void Update(SpecialProductView view,string account) 
        {
            SpecialProducts.Instance.Update(view);
            // 记录日志
            var content = string.Format("将特殊产品 [{0}] 修改为 状态:{1} 名称:{2} 说明:{3} 描述:{4} ", view.SpecialProductType.GetDescription(),view.Enabled,view.Name,view.Explain,view.Description);
            var log = new Service.Log.Domain.OperationLog(OperationModule.特殊产品管理, OperationType.Update, account, OperatorRole.Platform, ((int)view.SpecialProductType).ToString(), content, DateTime.Now);
            Service.LogService.SaveOperationLog(log);
        }
    }
}
