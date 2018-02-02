using System;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Integral;
using ChinaPay.B3B.Service.Integral.Domain;
using ChinaPay.B3B.Service.Integral.Repository;
using ChinaPay.B3B.Service.SystemManagement;

namespace ChinaPay.B3B.Service.Integral
{
    public static class IntegralServer
    {
        static object _locker = new object();
        #region 查询
        /// <summary>
        /// 查询用户积分信息列表
        /// </summary>
        /// <param name="time"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        public static IEnumerable<DataTransferObject.Integral.IntegralInfoView> GetIntegralList(Core.Range<DateTime>? time, Guid? companyId, Common.Enums.IntegralWay? way, Pagination pagination)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                return reposity.GetIntegralList(time, companyId, way, pagination);
            }
        }
        /// <summary>
        /// 查询用户消费记录列表
        /// </summary>
        /// <param name="time"></param>
        /// <param name="way"></param> 
        /// <returns></returns>
        public static IEnumerable<IntegralConsumptionView> GetIntegralConsumptionList(Core.Range<DateTime>? time, Guid? companyId, Common.Enums.IntegralWay? way, ExchangeState state, Common.Enums.OEMCommodityState? oEMCommodityState, string falg, Guid? oemId, Pagination pagination)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                return reposity.GetIntegralConsumptionList(time, way, companyId, state, oEMCommodityState, falg, oemId, pagination);
            }
        }
        /// <summary>
        /// 用户有效的总积分，总剩余积分
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static Domain.IntegralCount GetIntegralByAccountIdZong(Guid companyId)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                IntegralCount count = reposity.GetIntegralCount(companyId);
                IntegralParameterView view = reposity.GetIntegralParameter();
                if (count == null)
                {
                    count = new IntegralCount();
                }
                count.IntegralAvailable = count.IntegralSurplus + count.IntegralNotDeduct < 0 ? (count.IntegralSurplus + count.IntegralNotDeduct) : (int)Math.Floor((count.IntegralSurplus + count.IntegralNotDeduct) * view.AvailabilityRatio);
                count.IntegralAvailable = count.IntegralCounts - count.IntegralAvailable > view.MostBuckle ? count.IntegralCounts - view.MostBuckle : count.IntegralAvailable;
                return count;
            }
        }
        /// <summary>
        /// 得到单条的获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTransferObject.Integral.IntegralInfoView GetIntegral(Guid id)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                return reposity.GetIntegral(id);
            }
        }
        /// <summary>
        /// 得到积分参数设置信息
        /// </summary>
        /// <returns></returns>
        public static DataTransferObject.Integral.IntegralParameterView GetIntegralParameter()
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                return reposity.GetIntegralParameter();
            }
        }
        /// <summary>
        /// 得到单条消费积分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTransferObject.Integral.IntegralConsumptionView GetIntegralConsumption(Guid id)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                return reposity.GetIntegralConsumption(id);
            }
        }
        #endregion
        #region 修改
        /// <summary>
        /// 修改兑换状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static void UpdateIntegralConsumption(Guid id, ChinaPay.B3B.Common.Enums.ExchangeState State, string operatorName, string no, string company, string address, string reason)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                cmd.BeginTransaction();
                try
                {
                    reposity.UpdateIntegralConsumption(id, State, no, company, address, reason);
                    //如果是拒绝兑换，将积分返回给用户,商品数量加回去
                    if (State == ExchangeState.Refuse)
                    {
                        IntegralConsumptionView view = reposity.GetIntegralConsumption(id);
                        IntegralCount count = new IntegralCount
                        {
                            CompnayId = view.CompnayId,
                            Integral = view.ConsumptionIntegral
                        };
                        IntegralInfo integral = new IntegralInfo
                        {
                            CompnayId = view.CompnayId,
                            AccessTime = DateTime.Now,
                            IntegralWay = IntegralWay.RefuseExchange,
                            Remark = reason,
                            Integral = view.ConsumptionIntegral
                        };
                        count.Integral = integral.Integral;
                        if (count.Integral != 0)
                        {
                            reposity.InsertIntegralInfo(integral);
                            reposity.UpdateIntegralCount(count);
                            if (view.CommodityId.HasValue)
                                reposity.UpdateShelvesNum(view.CommodityId.Value, view.CommodityCount);
                        }
                    }
                    cmd.CommitTransaction();
                }
                catch (Exception)
                {
                    cmd.RollbackTransaction();
                    throw;
                }

            }
            saveUpdateLog("兑换状态", "处理中", State.GetDescription(), OperatorRole.Platform, id.ToString(), operatorName);
        }
        public static void UpdateIntegralParameter(IntegralParameterView view)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.UpdateIntegralParameter(view);
            }
        }

        #endregion
        #region 新增
        /// <summary>
        /// 用户首次登录获得积分
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="lastTime">最后登录时间</param>
        /// <param name="companyId">账号编号</param>
        /// <param name="userRole">所属操作角色</param>
        public static void InsertIntegralInfo(Guid companyId, string username, DateTime? lastTime, DateTime logonTime)
        {
            using (var cmd = Factory.CreateCommand())
            {
                //lock (_locker)
                //{
                cmd.BeginTransaction();
                var reposity = Factory.CreateIntegralReposity(cmd);
                try
                {
                    NotSignIn(username, lastTime, logonTime, companyId, reposity);
                    FirstInsertIntegral(companyId, reposity);
                    cmd.CommitTransaction();
                }
                catch (Exception)
                {
                    cmd.RollbackTransaction();
                }
                //}
            }
        }

        /// <summary>
        /// 添加一条消费记录
        /// </summary>
        public static void InsertIntegralConsumption(IntegralConsumption view)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.InsertIntegralConsumption(view);
            }
        }

        /// <summary>
        /// OEM修改提交状态
        /// </summary>
        public static void UpdateIntegralConsumption(Guid id, ChinaPay.B3B.Common.Enums.OEMCommodityState State)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                reposity.UpdateIntegralConsumption(id, State);
            }
        }
        private static void NotSignIn(string username, DateTime? lastTime, DateTime logonTime, Guid companyId, IReposity reposity)
        {
            IntegralParameterView parameter = reposity.GetIntegralParameter();
            if (lastTime != null && lastTime.Value.Date != logonTime.AddDays(-1).Date)
            {
                IntegralCount counts = reposity.GetIntegralCount(companyId);
                if (parameter.IsDrop)
                {
                    if (counts == null)
                    {
                        var count = new IntegralCount
                        {
                            CompnayId = companyId,
                            Integral = 0
                        };
                        reposity.InsertIntegralCount(count);
                    }
                    else
                    {
                        TimeSpan day = logonTime.AddDays(-1).Date - lastTime.Value.Date;
                        IntegralConsumption view = new IntegralConsumption
                        {
                            CompnayId = companyId,
                            AccountName = username,
                            AccountNo = "",
                            AccountPhone = "",
                            DeliveryAddress = "",
                            CommodityCount = 0,
                            CommodityId = Guid.Empty,
                            CommodityName = "",
                            ConsumptionIntegral = parameter.SignIntegral,
                            Exchange = ExchangeState.Processing,
                            ExchangeTiem = lastTime.Value.Date,
                            ExpressCompany = "",
                            ExpressDelivery = "",
                            Reason = "",
                            Remark = "未登录减少积分",
                            Way = IntegralWay.NotSignIn
                        };
                        int days = parameter.SignIntegral * day.Days < counts.IntegralSurplus ? day.Days : (int)Math.Ceiling(Convert.ToDouble(counts.IntegralSurplus) / Convert.ToDouble(parameter.SignIntegral));
                        reposity.InsertIntegralConsumption(view, days, parameter.SignIntegral, counts.IntegralSurplus);
                        var count = new IntegralCount
                        {
                            CompnayId = companyId,
                            Integral = -(parameter.SignIntegral * day.Days >= counts.IntegralSurplus ? counts.IntegralSurplus : parameter.SignIntegral * day.Days)
                        };
                        reposity.UpdateIntegralCount(count);
                    }
                }
            }
        }
        public static void InsertIntergralByMoney(string username, Guid companyId, string companyShortName, decimal money, bool IsChinaPay, string orderId, bool IsBuy, bool isCredit) 
        {
            if (!isCredit)
            {
                InsertIntergralByMoney(username, companyId, companyShortName, money, IsChinaPay, orderId, IsBuy);
            }
        }

        /// <summary>
        /// 增加用户购买机票的积分或减少用户退票的积分
        /// </summary>
        /// <param name="IsChinaPay">是否是国付通支付</param>
        /// <param name="IsBuy">是否是购买机票/退票，true买票，false退票</param>
        private static void InsertIntergralByMoney(string username, Guid companyId, string companyShortName, decimal money, bool IsChinaPay, string orderId, bool IsBuy)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                IntegralParameterView prameter = reposity.GetIntegralParameter();
                IntegralCount counts = reposity.GetIntegralCount(companyId);
                IntegralCount count = new IntegralCount
                {
                    CompnayId = companyId,
                    Integral = prameter.SignIntegral
                };
                if (IsBuy)
                {
                    IntegralInfo integral = new IntegralInfo
                    {
                        CompnayId = companyId,
                        AccessTime = DateTime.Now,
                        IntegralWay = IntegralWay.Buy,
                        Remark = orderId,
                        Integral = IsChinaPay ? (int)(Math.Floor(money * ((decimal)prameter.ConsumptionIntegral / 100)) * prameter.Multiple) : (int)Math.Floor(money * ((decimal)prameter.ConsumptionIntegral / 100))
                    };
                    count.Integral = integral.Integral;
                    if (count.Integral != 0)
                    {
                        reposity.InsertIntegralInfo(integral);
                    }
                }
                else
                {
                    IntegralConsumption consumtion = new IntegralConsumption
                    {
                        CompnayId = companyId,
                        AccountName = username,
                        CompanyShortName = companyShortName,
                        AccountNo = "",
                        AccountPhone = "",
                        DeliveryAddress = "",
                        CommodityCount = 0,
                        CommodityId = Guid.Empty,
                        CommodityName = "",
                        Exchange = ExchangeState.Processing,
                        ExchangeTiem = DateTime.Now,
                        ExpressCompany = "",
                        ExpressDelivery = "",
                        Reason = "",
                        Remark = orderId,
                        Way = IntegralWay.TuiPiao
                    };
                    consumtion.ConsumptionIntegral = IsChinaPay ? (int)(Math.Floor(money * ((decimal)prameter.ConsumptionIntegral / 100)) * prameter.Multiple) : (int)Math.Floor(money * ((decimal)prameter.ConsumptionIntegral / 100));
                    count.Integral = 0 - consumtion.ConsumptionIntegral;
                    if (count.Integral != 0)
                    {
                        reposity.InsertIntegralConsumption(consumtion);
                    }
                }
                if (count.Integral == 0) return;
                count.IsNotDeduct = true;
                if (counts == null)
                {
                    reposity.InsertIntegralCount(count);
                }
                else
                {
                    reposity.UpdateIntegralCount(count);
                }
            }
        }

        public static void UpdateIntegralCount(int integral, Guid companyId)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                IntegralCount counts = reposity.GetIntegralCount(companyId);
                IntegralCount count = new IntegralCount
                {
                    CompnayId = companyId,
                    Integral = integral
                };
                if (counts == null)
                {
                    count.Integral = 0;
                    reposity.InsertIntegralCount(count);
                }
                else
                {
                    reposity.UpdateIntegralCount(count);
                }
            }
        }
        /// <summary>
        /// 用户消费积分
        /// </summary>
        /// <param name="integral">消费积分</param>
        /// <param name="companyId">公司Id</param>
        public static void UpdateIntegralCountByConsumption(int integral, Guid companyId)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                IntegralCount orginalCount = reposity.GetIntegralCount(companyId);
                IntegralCount count = new IntegralCount
                {
                    CompnayId = companyId,
                    Integral = integral
                };
                reposity.UpdateIntegralCountByConsumption(orginalCount, count);
            }
        }


        private static void FirstInsertIntegral(Guid companyId, IReposity reposity)
        {
            IntegralParameterView prameter = reposity.GetIntegralParameter();
            if (prameter.IsSignIn)
            {
                IntegralInfo integral = new IntegralInfo
                {
                    CompnayId = companyId,
                    AccessTime = DateTime.Now,
                    Integral = prameter.SignIntegral,
                    IntegralWay = IntegralWay.SignIn,
                    Remark = "登录奖励"
                };
                reposity.InsertIntegralInfo(integral);
                IntegralCount counts = reposity.GetIntegralCount(companyId);
                IntegralCount count = new IntegralCount
                {
                    CompnayId = companyId,
                    Integral = prameter.SignIntegral
                };
                if (counts == null)
                {
                    count.Integral = prameter.SignIntegral;
                    reposity.InsertIntegralCount(count);
                }
                else
                {
                    reposity.UpdateIntegralCount(count);
                }
            }
        }
        /// <summary>
        /// 开户设置积分
        /// </summary>
        /// <param name="companyId"></param>
        public static void OpenAccountIntegral(Guid companyId)
        {
            using (var cmd = Factory.CreateCommand())
            {
                var reposity = Factory.CreateIntegralReposity(cmd);
                IntegralInfo integral = new IntegralInfo
                {
                    CompnayId = companyId,
                    AccessTime = DateTime.Now,
                    Integral = SystemParamService.OpenAccountIntegral,
                    IntegralWay = IntegralWay.OpenAccount,
                    Remark = "开户奖励"
                };
                IntegralCount count = new IntegralCount();
                count.CompnayId = companyId;
                count.Integral = SystemParamService.OpenAccountIntegral;
                reposity.InsertIntegralInfo(integral);
                reposity.InsertIntegralCount(count);
            }
        }
        #endregion
        #region 删除

        #endregion
        #region 日志
        static void saveAddLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Insert, itemName + "。" + content, role, key, account);
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
            var log = new Log.Domain.OperationLog(OperationModule.积分, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion
    }
}
