using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Core.Extension;
using Izual.Linq;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization
{
  public static  class ConfigurationService
    {
        /// <summary>
        /// 添加配置信息。
        /// </summary>
        /// <param name="configuration">要添加的配置信息。</param>
        /// <returns>返回添加操作是否成功。</returns>
        public static bool AddConfiguration(Configuration configuration, string operatorAccount)
        {
            bool isSuccess = false;
            if (configuration == null) throw new ArgumentNullException("configuration");
            isSuccess = configuration.Insert();
            saveLog(OperationType.Insert, string.Format("为公司Id:{0}添加配置Id为{1}的信息", configuration.Company, configuration.Id), OperatorRole.Provider, configuration.Id.ToString(), operatorAccount);

            return isSuccess;
        }

        /// <summary>
        /// 更新配置信息。
        /// </summary>
        /// <param name="configuration">用于更新的配置信息。</param>
        /// <returns>返回更新操作是否成功。</returns>
        public static bool UpdateConfiguration(Configuration configuration, string operatorAccount)
        {
            bool isSuccess = false;
            if (configuration == null) throw new ArgumentNullException("configuration");
            isSuccess = configuration.Update();
            saveLog(OperationType.Update, string.Format("修改配置Id为{0}的信息", configuration.Id), OperatorRole.Provider, configuration.Id.ToString(), operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 删除指定配置。
        /// </summary>
        /// <param name="ids">要删除的配置的 Id。</param>
        /// <returns>返回删除操作是否成功。</returns>
        public static bool DeleteConfiguration(string operatorAccount, params Guid[] ids)
        {
            bool isSuccess = false;
            isSuccess = (ids.Length == 1
                        ? DataContext.Configurations.Delete(p => p.Id == ids[0])
                        : DataContext.Configurations.Delete(p => ids.Contains(p.Id))) > 0;
            saveLog(OperationType.Delete, string.Format("删除配置Id为{0}的信息", ids.Join(",", item => item.ToString())), OperatorRole.Provider, ids.Join(",", item => item.ToString()), operatorAccount);
            return isSuccess;
        }

        /// <summary>
        /// 获取公司的Id查询所有配置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static IEnumerable<Configuration> QueryConfigurations(Guid companyId) {
            var repository = Factory.CreateConfigurationRepository();
            return repository.QueryConfigurations(companyId);
        }

        /// <summary>
        /// 获取指定 Id 的配置
        /// </summary>
        /// <param name="id">配置 Id</param>
        /// <returns>返回获取到的配置</returns>
        public static Configuration QueryConfiguration(Guid id) {
            var repository = Factory.CreateConfigurationRepository();
            return repository.QueryConfiguration(id);
        }

        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.单位, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
    }
}
