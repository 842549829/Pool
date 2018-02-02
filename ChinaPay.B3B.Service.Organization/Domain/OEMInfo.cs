using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// OEM��Ϣ
    /// </summary>
    public class OEMInfo
    {
        LazyLoader<CompanyInfo> _companyLoader;
        public OEMInfo()
        {
            _companyLoader = new LazyLoader<CompanyInfo>(() => CompanyService.GetCompanyDetail(CompanyId));
        }
        CompanyInfo _company;
        /// <summary>
        /// OEMID
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// ��˾��Ϣ
        /// </summary>
        public CompanyInfo Company
        {
            get
            {
                return _company ?? (_company = _companyLoader.QueryData());
            }
        }
        /// <summary>
        /// ��˾Id
        /// </summary>
        public Guid CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// OEM����
        /// </summary>
        public string SiteName
        {
            get;
            set;
        }
        /// <summary>
        /// ����
        /// </summary>
        public string DomainName
        {
            get;
            set;
        }
        /// <summary>
        /// ����Ա����
        /// </summary>
        public string ManageEmail
        {
            get;
            set;
        }
        /// <summary>
        /// ICP����
        /// </summary>
        public string ICPRecord
        {
            get;
            set;
        }
        /// <summary>
        /// Logo·��
        /// </summary>
        public string LogoPath
        {
            get;
            set;
        }
        /// <summary>
        /// Ƕ�����
        /// </summary>
        public string EmbedCode
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ���
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ���������ע��
        /// </summary>
        public bool AllowSelfRegex
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ�ʹ��B3Bָ������
        /// </summary>
        public bool UseB3BConfig
        {
            get;
            set;
        }
        /// <summary>
        /// ��ͨʱ��
        /// </summary>
        public DateTime RegisterTime
        {
            get;
            set;
        }
        /// <summary>
        /// ��Ȩ��ֹʱ��
        /// </summary>
        public DateTime? EffectTime
        {
            get;
            set;
        }
        /// <summary>
        /// ��Ȩ��֤��
        /// </summary>
        public decimal AuthCashDeposit
        {
            get;
            set;
        }
        /// <summary>
        /// ��ͨOEM���ʺ�
        /// </summary>
        public string OperatorAccount
        {
            get;
            set;
        }
        /// <summary>
        /// ҵ�������ϵ��ʽ
        /// </summary>
        public OEMContract Contract
        {
            get;
            set;
        }
        /// <summary>
        /// վ������
        /// </summary>
        public OemSetting Setting
        {
            get;
            set;
        }
        /// <summary>
        /// oem��Ӧ�ķ��
        /// </summary>
        public OEMStyle OEMStyle
        {
            get;
            set;
        }

        private List<IncomeGroup> _incomeGroupList;

        /// <summary>
        /// ��oem�µ�����������
        /// </summary>
        public List<IncomeGroup> IncomeGroupList
        {
            get
            {
                return Valid ? _incomeGroupList : new List<IncomeGroup>();
            }
            set
            {
                _incomeGroupList = value;
            }
        }
        /// <summary>
        /// ��½��ַ
        /// </summary>
        public string LoginUrl
        {
            get;
            set;
        }


        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        public bool Valid
        {
            get
            {
                return EffectTime.HasValue && DateTime.Now.Date <= EffectTime.Value.Date;
            }
        }
        /// <summary>
        /// ָ������
        /// </summary>
        public Dictionary<ConfigUseType, Tuple<string, string>> AirlineConfig
        {
            get;
            set;
        }
        public override string ToString()
        {
            return string.Format("��˾��ţ�{0},վ�����ƣ�{1},������{2},����Ա���䣺{3},ICP������{4},ͳ�ƴ��룺{5},�Ƿ�����{6},�Ƿ���������ע�᣺{7},ҵ�������ϵ��ʽ��{8},վ�����ã�{9},ָ�����ã�{10},�Ƿ�ʹ��B3Bָ�����ã�{11},��Ȩ��ֹʱ�䣺{12},��Ȩ��֤��{13}"
                , Company.CompanyId
                , SiteName
                , DomainName
                , ManageEmail
                , ICPRecord
                , EmbedCode
                , Enabled ? "��" : "��"
                , AllowSelfRegex ? "��" : "��"
                , Contract
                , Setting
                , AirlineConfig.Aggregate(string.Empty, (result, pair) =>
                                      string.Format("{0}������;��{1},�û���{2};OfficeNO:{3} ", result, pair.Key.GetDescription(), pair.Value.Item1, pair.Value.Item2))
                , UseB3BConfig ? "��" : "��"
                , EffectTime
                , AuthCashDeposit
                );
        }

    }

}