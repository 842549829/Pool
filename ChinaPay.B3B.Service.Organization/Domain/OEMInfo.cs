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
    /// OEM信息
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
        /// 公司信息
        /// </summary>
        public CompanyInfo Company
        {
            get
            {
                return _company ?? (_company = _companyLoader.QueryData());
            }
        }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// OEM名称
        /// </summary>
        public string SiteName
        {
            get;
            set;
        }
        /// <summary>
        /// 域名
        /// </summary>
        public string DomainName
        {
            get;
            set;
        }
        /// <summary>
        /// 管理员邮箱
        /// </summary>
        public string ManageEmail
        {
            get;
            set;
        }
        /// <summary>
        /// ICP备案
        /// </summary>
        public string ICPRecord
        {
            get;
            set;
        }
        /// <summary>
        /// Logo路径
        /// </summary>
        public string LogoPath
        {
            get;
            set;
        }
        /// <summary>
        /// 嵌入代码
        /// </summary>
        public string EmbedCode
        {
            get;
            set;
        }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }
        /// <summary>
        /// 是否允许自助注册
        /// </summary>
        public bool AllowSelfRegex
        {
            get;
            set;
        }
        /// <summary>
        /// 是否使用B3B指令配置
        /// </summary>
        public bool UseB3BConfig
        {
            get;
            set;
        }
        /// <summary>
        /// 开通时间
        /// </summary>
        public DateTime RegisterTime
        {
            get;
            set;
        }
        /// <summary>
        /// 授权截止时间
        /// </summary>
        public DateTime? EffectTime
        {
            get;
            set;
        }
        /// <summary>
        /// 授权保证金
        /// </summary>
        public decimal AuthCashDeposit
        {
            get;
            set;
        }
        /// <summary>
        /// 开通OEM的帐号
        /// </summary>
        public string OperatorAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 业务服务联系方式
        /// </summary>
        public OEMContract Contract
        {
            get;
            set;
        }
        /// <summary>
        /// 站点设置
        /// </summary>
        public OemSetting Setting
        {
            get;
            set;
        }
        /// <summary>
        /// oem对应的风格
        /// </summary>
        public OEMStyle OEMStyle
        {
            get;
            set;
        }

        private List<IncomeGroup> _incomeGroupList;

        /// <summary>
        /// 该oem下的所有收益组
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
        /// 登陆地址
        /// </summary>
        public string LoginUrl
        {
            get;
            set;
        }


        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid
        {
            get
            {
                return EffectTime.HasValue && DateTime.Now.Date <= EffectTime.Value.Date;
            }
        }
        /// <summary>
        /// 指令配置
        /// </summary>
        public Dictionary<ConfigUseType, Tuple<string, string>> AirlineConfig
        {
            get;
            set;
        }
        public override string ToString()
        {
            return string.Format("公司编号：{0},站点名称：{1},域名：{2},管理员邮箱：{3},ICP备案：{4},统计代码：{5},是否开启：{6},是否允许自助注册：{7},业务服务联系方式：{8},站点设置：{9},指令配置：{10},是否使用B3B指令配置：{11},授权截止时间：{12},授权保证金：{13}"
                , Company.CompanyId
                , SiteName
                , DomainName
                , ManageEmail
                , ICPRecord
                , EmbedCode
                , Enabled ? "是" : "否"
                , AllowSelfRegex ? "是" : "否"
                , Contract
                , Setting
                , AirlineConfig.Aggregate(string.Empty, (result, pair) =>
                                      string.Format("{0}配置用途：{1},用户名{2};OfficeNO:{3} ", result, pair.Key.GetDescription(), pair.Value.Item1, pair.Value.Item2))
                , UseB3BConfig ? "是" : "否"
                , EffectTime
                , AuthCashDeposit
                );
        }

    }

}