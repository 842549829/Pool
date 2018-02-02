using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Organization {
    /// <summary>
    /// 常旅客服务
    /// </summary>
    public static class CustomerService {
        public static void Register(Guid companyId, Customer customer, string operatorAccount) {
            var repository = Factory.CreateCustomerRepository();
            repository.Save(companyId, customer);
            saveAddLog("常旅客", string.Format("公司Id:{0},姓名:{1}",companyId,customer.Name), OperatorRole.User, customer.Id.ToString(), operatorAccount);
        }
        public static void Register(Guid companyId, IEnumerable<Customer> customers, string operatorAccount) {
            var repository = Factory.CreateCustomerRepository();
            repository.Save(companyId, customers);
            string content="";
            string customerIds = "";
            foreach(var item in customers){
                content += string.Format("姓名:{0}",item.Name);
                customerIds += item.Id.ToString()+",";
            }
            customerIds.Remove(customerIds.Length - 1);
            saveAddLog("常旅客",string.Format("公司Id:{0}",companyId)+content,OperatorRole.User,customerIds,operatorAccount);
        }
        public static void Update(Guid companyId, Customer customer, string operatorAccount) {
            var repository = Factory.CreateCustomerRepository();
            repository.Update(companyId, customer);
            saveLog(OperationType.Update, string.Format("修改常旅客名为{0}的信息",customer.Name), OperatorRole.User,customer.Id.ToString() ,operatorAccount);
        }
        public static void Delete(Guid companyId, Guid customerId, string operatorAccount) {
            var repository = Factory.CreateCustomerRepository();
            repository.Delete(companyId, customerId);
            saveDeleteLog("常旅客",string.Format("常旅客Id为:{0}",customerId),OperatorRole.User,customerId.ToString(),operatorAccount);
        }
        public static Customer Query(Guid customerId)
        {
            var repository = Factory.CreateCustomerRepository();
            return repository.Query(customerId);
        }
        public static IEnumerable<Customer> Query(CustomerQueryCondition condition, Pagination pagination) {
            var repository = Factory.CreateCustomerRepository();
            return repository.Query(condition, pagination);
        }

        #region "日志"
        static void saveAddLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Insert, "添加" + itemName + "。" + content, role, key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), role, key, account);
        }
        static void saveDeleteLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + "。" + content, role, key, account);
        }
        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.其他, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion

    }
}