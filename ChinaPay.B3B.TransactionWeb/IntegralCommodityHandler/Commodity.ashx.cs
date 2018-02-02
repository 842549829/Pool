using System;
using ChinaPay.B3B.Service.Commodity;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodityHandler
{
    /// <summary>
    /// Commodity 的摘要说明
    /// </summary>
    public class Commodity : BaseHandler
    {
        public bool ExChangreCommosity(Guid id, int exChangeNum)
        {
            try
            {
                OEMInfo oem = BasePage.SuperiorOEM;
                CommodityServer.ExChangeCommodity(id, exChangeNum, CurrentCompany, CurrentUser, oem == null ? OEMCommodityState.Success : OEMCommodityState.Processing, oem == null ? "B3B交易平台" : oem.SiteName, oem == null ? (Guid?)null : oem.Id);
                return true;
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("兑换商品发生未知错误，请稍后再试");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}