using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaPay.B3B.Interface.Processor
{
    class QueryApplyform : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            string applyformId = Context.GetParameterValue("applyformId");
            decimal id;
            if (!decimal.TryParse(applyformId, out id)) InterfaceInvokeException.ThrowParameterErrorException("applyformId");
            var obj = Service.ApplyformQueryService.QueryApplyform(id);
            if (obj == null) InterfaceInvokeException.ThrowCustomMsgException("暂无此申请单");
            if (obj.Purchaser.CompanyId != Company.CompanyId) InterfaceInvokeException.ThrowCustomMsgException("暂无此申请单");
            return ReturnStringUtility.GetApplyform(obj);
        }
    }
}