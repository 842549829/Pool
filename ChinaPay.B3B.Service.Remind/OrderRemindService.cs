using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Remind.Model;

namespace ChinaPay.B3B.Service.Remind {
    public static class OrderRemindService {
        public static IEnumerable<RemindInfo> Query() {
            var repository = Repository.Factory.CreateRemindRepository();
            return repository.Query();
        }
        public static void Save(decimal orderId, RemindStatus status, string carrier, Guid acceptor,string customNO = "") {
            var order = new RemindInfo() {
                Id = orderId,
                Status = status,
                Carrier = carrier,
                Acceptor = acceptor,
                CustomNO = customNO
            };  
            var repository = Repository.Factory.CreateRemindRepository();
            repository.Save(order);
        }
        public static void Delete(decimal orderId) {
            var repository = Repository.Factory.CreateRemindRepository();
            repository.Delete(orderId);
        }
        /// <summary>
        /// 获取出票方订单提醒信息
        /// </summary>
        /// <param name="provider">出票方单位Id</param>
        public static ProviderRemindView QueryProviderRemindInfo(Guid provider) {
            var repository = Repository.Factory.CreateRemindRepository();
            return repository.QueryProviderRemindInfo(provider);
        }
        /// <summary>
        /// 获取资源方订单提醒信息
        /// </summary>
        /// <param name="provider">资源方单位Id</param>
        public static SupplierRemindView QuerySupplierRemindInfo(Guid supplier) {
            var repository = Repository.Factory.CreateRemindRepository();
            return repository.QuerySupplierRemindInfo(supplier);
        }
    }
}