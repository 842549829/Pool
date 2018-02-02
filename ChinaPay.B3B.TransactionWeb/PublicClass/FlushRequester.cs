using System;

namespace ChinaPay.B3B.TransactionWeb.PublicClass
{
    /// <summary>
    /// 用于发起缓存刷新的请求
    /// </summary>
    public static class FlushRequester
    {
        public static void TriggerOEMFlusher(Guid oemId) {
            var systemDicionary = Service.SystemManagement.SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.SystemRefreshCacheAddress);
            var key = Utility.MD5EncryptorService.MD5FilterZero(System.Configuration.ConfigurationManager.AppSettings["SignKey"], "utf-8");
            foreach (var item in systemDicionary)
            {
                ChinaPay.Utility.HttpRequestUtility.GetHttpResult(item.Value + "?Action=Flush&Target=OEMSETTING&OEMID="+oemId+"&Key=" + key, 3000);
            }
        }

        public static void TriggerOEMAdder()
        {
            var systemDicionary = Service.SystemManagement.SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.SystemRefreshCacheAddress);
            var key = Utility.MD5EncryptorService.MD5FilterZero(System.Configuration.ConfigurationManager.AppSettings["SignKey"], "utf-8");
            foreach (var item in systemDicionary)
            {
                ChinaPay.Utility.HttpRequestUtility.GetHttpResult(item.Value + "?Action=Flush&Target=OEMADDED&Key=" + key, 3000);
            }
        }

        public static void TriggerOEMStyleFlusher(Guid styleId)
        {
            var systemDicionary = Service.SystemManagement.SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.SystemRefreshCacheAddress);
            var key = Utility.MD5EncryptorService.MD5FilterZero(System.Configuration.ConfigurationManager.AppSettings["SignKey"], "utf-8");
            foreach (var item in systemDicionary)
            {
                Utility.HttpRequestUtility.GetHttpResult(item.Value + "?Action=Flush&Target=OEMSTYLE&StyleId=" + styleId + "&Key=" + key, 3000);
            }

        }
    }
}