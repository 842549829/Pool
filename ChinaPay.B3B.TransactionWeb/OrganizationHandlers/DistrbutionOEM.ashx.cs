using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// DistrbutionOEM 的摘要说明
    /// </summary>
    public class DistrbutionOEM : BaseHandler
    {
        /// <summary>
        ///  修改页头信息
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public string UpdateHeaderifno(OemSetting setting,string oemid) 
        {
            try
            {
                //var set = DistributionOEMService.QueryDistributionOEMSetting(setting.Id);

                //setting.FooterLinks = set.FooterLinks;
                //setting.BGColor = set.BGColor;
                //setting.copyrightInfo = set.copyrightInfo;

                DistributionOEMService.UpdateDistributionOEMSetting(setting, Guid.Parse(oemid),CurrentUser.UserName);
                //刷新缓存
                FlushRequester.TriggerOEMFlusher(Guid.Parse(oemid));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "1";
        }
        ///// <summary>
        /////  修改页脚信息
        ///// </summary>
        ///// <param name="setting"></param>
        ///// <returns></returns>
        //public bool UpdateFooterifno(OemSetting setting)
        //{
        //    try
        //    {
        //        var set = DistributionOEMService.QueryDistributionOEMSetting(setting.Id);

        //        setting.SiteKeyWord = set.SiteKeyWord;
        //        setting.SiteDescription = set.SiteDescription;
        //        //setting.LogoPath = set.LogoPath;
        //        setting.HeaderLinks = set.HeaderLinks;
        //        DistributionOEMService.UpdateDistributionOEMSetting(setting);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}