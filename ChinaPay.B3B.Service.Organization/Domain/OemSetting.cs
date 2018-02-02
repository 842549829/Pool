using System;
using System.Collections.Generic;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class OemSetting
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string SiteKeyWord { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string SiteDescription { get; set; }
        ///// <summary>
        ///// Logo路径 移到了设置信息里面
        ///// </summary>
        //public string LogoPath { get; set; }
        /// <summary>
        /// 页面头链接
        /// </summary>
        public IEnumerable<Links> HeaderLinks
        {
            get;
            set;
        }

        /// <summary>
        /// 页面尾链接
        /// </summary>
        public IEnumerable<Links> FooterLinks
        {
            get;
            set;
        }
        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BGColor { get; set; }
        /// <summary>
        /// 版权信息
        /// </summary>
        public string CopyrightInfo { get; set; }
        public override string ToString()
        {
            return string.Format("编号{0}，关键字{1}，描述{2}，页面头链接{3}，页面尾链接{4}，背景颜色{5}，版权信息{6}", Id, SiteKeyWord, SiteDescription, HeaderLinks.Join(",", item => item.ToString()), FooterLinks.Join(",", item => item.ToString()), BGColor, CopyrightInfo);
        }
    }
}