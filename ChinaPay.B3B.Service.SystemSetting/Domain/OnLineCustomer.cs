namespace ChinaPay.B3B.Service.SystemSetting.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    public class OnLineCustomer
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid Company
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
       {
           get;
           set;
       }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
       {
           get;
           set;
       }
        /// <summary>
        /// 发布角色
        /// </summary>
        public PublishRole Role
       {
           get;
           set;
       }
        public IEnumerable<DivideGroupManage> DivideGroupManage
       {
           get;
           set;
       }
    }
   public enum PublishRole
   {
       [Description("平台")]
       平台,
       [Description("OEM")]
       OEM
   }
}
