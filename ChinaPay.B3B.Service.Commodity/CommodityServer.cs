using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Commodity
{
    using System.Collections.Generic;
    using Common.Enums;
    using DataTransferObject.Commodity;
    using DataTransferObject.Integral;
    using DataTransferObject.Organization;
    using Integral;
    using Integral.Domain;
    using Repository;
    using ChinaPay.B3B.Service.Organization;
    using System.Linq;
    using ChinaPay.SMS.Service;
    public static class CommodityServer
    {
        #region 查询
        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <param name="falg">是true的时候就是查询展示商品</param>
        /// <returns></returns>
        public static IEnumerable<CommodityView> GetCommodityList(bool falg, Pagination pagination)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                return reposity.GetCommodityList(falg, pagination);
            }
        }
        /// <summary>
        /// 得到单条商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTransferObject.Commodity.CommodityView GetCommodity(Guid id)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                return reposity.GetCommodity(id);
            }
        }
        #endregion
        #region 修改

        /// <summary>
        /// 修改单件商品信息
        /// </summary>
        public static void UpdateCommodity(CommodityView view, string operatorName)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.Update(view);
                saveUpdateLog(operatorName, view.CommodityName, OperatorRole.Platform, view.ID.ToString(), operatorName);
            }
        }
        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="State">true启用/false禁用</param>
        /// <returns></returns>
        public static void UpdateState(Guid id, bool State, string operatorName)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.UpdateState(id, State);
                saveUpdateLog(operatorName, id.ToString(), OperatorRole.Platform, id.ToString(), operatorName);
            }
        }
        /// <summary>
        /// 修改上架数量
        /// </summary> 
        public static void UpdateShelvesNum(Guid id, int StockNumber, string operatorName)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.UpdateShelvesNum(id, StockNumber);
                saveUpdateLog(operatorName, id.ToString(), OperatorRole.Platform, id.ToString(), operatorName);
            }
        }
        /// <summary>
        /// 更新用户购买数量到数据库
        /// </summary> 
        public static void UpdateBuyNum(Guid id, int Number, string operatorName)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.UpdateBuyNum(id, Number);
                saveUpdateLog(operatorName, id.ToString(), OperatorRole.Platform, id.ToString(), operatorName);
            }
        }
        /// <summary>
        /// 兑换商品
        /// </summary>
        public static void ExChangeCommodity(Guid id, int exChangeNum, CompanyDetailInfo compnay, EmployeeDetailInfo accountinfo, OEMCommodityState stata, string domainName, Guid? oemid)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                CommodityView commodityview = reposity.GetCommodity(id);
                if (commodityview.ValidityTime.Date < DateTime.Now.Date) throw new Exception("该商品上架时间已经到期，不能兑换！");
                if (!commodityview.State) throw new Exception("该商品未启用，不能兑换！");
                if (commodityview.SurplusNumber == 0) throw new Exception("该商品已经兑换完毕，不能兑换！");
                if (commodityview.SurplusNumber < exChangeNum) throw new Exception("该商品库存不足，不能兑换！");
                if (commodityview.NeedIntegral > 0)
                {
                    IntegralParameterView paraemter = IntegralServer.GetIntegralParameter();
                    IntegralCount counts = IntegralServer.GetIntegralByAccountIdZong(compnay.CompanyId);
                    if (counts == null) throw new Exception("积分不够，暂不能兑换该商品！");
                    if (counts.IntegralAvailable < commodityview.NeedIntegral * exChangeNum) throw new Exception("积分不够，暂不能兑换该商品！");
                }
                commodityview.SurplusNumber = commodityview.SurplusNumber - exChangeNum;
                commodityview.ExchangeNumber = commodityview.ExchangeNumber + exChangeNum;
                cmd.BeginTransaction();
                try
                {
                    reposity.Update(commodityview);
                    var consumtion = new IntegralConsumption
                                         {
                                             CompnayId = compnay.CompanyId,
                                             CompanyShortName = compnay.AbbreviateName,
                                             AccountName = accountinfo.Name,
                                             AccountNo = accountinfo.UserName,
                                             AccountPhone = compnay.ContactPhone,
                                             DeliveryAddress = commodityview.Type == CommodityType.Entity ? compnay.Address : "",
                                             CommodityCount = exChangeNum,
                                             CommodityId = commodityview.ID,
                                             CommodityName = commodityview.CommodityName,
                                             Exchange = commodityview.Type == CommodityType.Entity ? ExchangeState.Processing : ExchangeState.Success,
                                             ExchangeTiem = DateTime.Now,
                                             ExpressCompany = "",
                                             ExpressDelivery = "",
                                             Reason = "",
                                             Remark = commodityview.CommodityName,
                                             Way = commodityview.Type == CommodityType.Entity ? IntegralWay.Exchange : IntegralWay.ExchangeSms,
                                             ConsumptionIntegral = commodityview.NeedIntegral * exChangeNum,
                                             OEMCommodityState = commodityview.Type == CommodityType.SMS ? OEMCommodityState.Success : stata,
                                             OEMName = domainName,
                                             OEMID = oemid
                                         };
                    IntegralServer.InsertIntegralConsumption(consumtion);
                    IntegralServer.UpdateIntegralCountByConsumption(0 - consumtion.ConsumptionIntegral, accountinfo.Owner);
                    cmd.CommitTransaction();
                }
                catch (Exception)
                {
                    cmd.RollbackTransaction();
                    throw;
                }
                if (commodityview.Type == CommodityType.SMS)
                {
                    var acc = from item in AccountService.Query(compnay.CompanyId)
                              where item.Type == Common.Enums.AccountType.Payment
                              select new { No = item.No };
                    SMSOrderService.ExChangeSms(compnay.CompanyId, commodityview.NeedIntegral, exChangeNum, commodityview.ExchangSmsNumber, acc.First().No);
                }
            }
        }

        #endregion
        #region 添加
        /// <summary>
        /// 添加单件商品信息
        /// </summary>
        public static void InsertCommodity(CommodityView view, string operatorName)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.Insert(view);
                saveAddLog(operatorName, view.CommodityName, OperatorRole.Platform, view.ID.ToString(), operatorName);
            }
        }

        #endregion
        #region 删除
        /// <summary>
        /// 删除单件商品信息
        /// </summary>
        public static void DeleteCommodity(CommodityView view, string operatorName)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.Delete(view);
                saveDeleteLog(operatorName, view.CommodityName, OperatorRole.Platform, view.ID.ToString(), operatorName);
            }
        }
        #endregion
        #region 日志
        static void saveAddLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Insert, itemName + "添加商品：" + content, role, key, account);
        }
        static void saveUpdateLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("{0}修改商品：{1}", itemName, content), role, key, account);
        }

        static void saveUpdateLog(string itemName, int exChange, string commdityName, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("{0} 兑换{1}件，{2}商品", itemName, exChange, commdityName), role, key, account);
        }
        static void saveDeleteLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + "。" + content, role, key, account);
        }
        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.商品, operationType, account, role, key, content);
            LogService.SaveOperationLog(log);
        }
        #endregion
    }
}
