using System;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.Interface.Processor
{
    class QueryOrder : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            string _id = Context.GetParameterValue("id");
            decimal id = 0M;
            if (decimal.TryParse(_id, out id))
            {
                var orderInfo = OrderQueryService.QueryOrder(id);
                if (orderInfo == null) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
                if (orderInfo.Purchaser.CompanyId != Company.CompanyId) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
                if (orderInfo.Bill == null) InterfaceInvokeException.ThrowCustomMsgException("暂无账单信息");
                return ReturnStringUtility.GetOrder(orderInfo);
            }
            InterfaceInvokeException.ThrowParameterErrorException("订单号");
            return "";
        }
    }
}