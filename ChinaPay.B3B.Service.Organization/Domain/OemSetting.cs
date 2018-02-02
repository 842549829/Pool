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
        /// �ؼ���
        /// </summary>
        public string SiteKeyWord { get; set; }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string SiteDescription { get; set; }
        ///// <summary>
        ///// Logo·�� �Ƶ���������Ϣ����
        ///// </summary>
        //public string LogoPath { get; set; }
        /// <summary>
        /// ҳ��ͷ����
        /// </summary>
        public IEnumerable<Links> HeaderLinks
        {
            get;
            set;
        }

        /// <summary>
        /// ҳ��β����
        /// </summary>
        public IEnumerable<Links> FooterLinks
        {
            get;
            set;
        }
        /// <summary>
        /// ������ɫ
        /// </summary>
        public string BGColor { get; set; }
        /// <summary>
        /// ��Ȩ��Ϣ
        /// </summary>
        public string CopyrightInfo { get; set; }
        public override string ToString()
        {
            return string.Format("���{0}���ؼ���{1}������{2}��ҳ��ͷ����{3}��ҳ��β����{4}��������ɫ{5}����Ȩ��Ϣ{6}", Id, SiteKeyWord, SiteDescription, HeaderLinks.Join(",", item => item.ToString()), FooterLinks.Join(",", item => item.ToString()), BGColor, CopyrightInfo);
        }
    }
}