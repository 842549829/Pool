using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.AutoPay;
using System.Collections.Generic; 

namespace ChinaPay.B3B.Service.Order.Repository
{
    interface IAutoPayRepository
    {
        void Insert(AutoPay pay);
        void Update(AutoPay pay);
        /// <summary>
        /// 修改代扣状态(成功)
        /// </summary>
        /// <param name="orderId"></param>
        void UpdateSuccess(decimal orderId);
        /// <summary>
        /// 修改处理状态(已处理)
        /// </summary>
        /// <param name="orderId"></param>
        void UpdateProcess(decimal orderId);
        Domain.AutoPay.AutoPay Query(decimal orderId);
        /// <summary>
        /// 查询当前未处理的代扣的订单
        /// </summary>
        /// <returns></returns>
        List<Domain.AutoPay.AutoPay> QueryNoPorcess();
        List<Domain.AutoPay.AutoPay> Query(AutoPayCondition condition, Pagination pagination);
    }
}
