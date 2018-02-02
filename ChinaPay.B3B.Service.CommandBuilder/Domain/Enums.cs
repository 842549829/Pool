namespace ChinaPay.B3B.Service.CommandBuilder.Domain
{
    public enum SpecialServiceRequirementType
    {
        /// <summary>
        /// 证件
        /// </summary>
        FOID,
        /// <summary>
        /// 儿童信息
        /// </summary>
        CHLD
    }


    public enum OtherServiceInformationType
    {
        /// <summary>
        /// 儿童
        /// </summary>
        CHD,
        /// <summary>
        /// 信使
        /// </summary>
        COUR,
        /// <summary>
        /// 联系方式
        /// </summary>
        CTC,
        /// <summary>
        /// 联系地址
        /// </summary>
        CTCA,
        /// <summary>
        /// 联系电话
        /// </summary>
        CTCP,
        /// <summary>
        /// 联系移动电话
        /// </summary>
        CTCT,
        /// <summary>
        /// 婴儿
        /// </summary>
        INF,
        /// <summary>
        /// 海员
        /// </summary>
        SEMN,
        /// <summary>
        /// 特殊旅客
        /// </summary>
        SPON,
        /// <summary>
        /// 完整团体人数
        /// </summary>
        TCP,
        /// <summary>
        /// 重要旅客
        /// </summary>
        VIP,
        /// <summary>
        /// 票号
        /// </summary>
        TKNO
    }
    
    /// <summary>
    /// 电子客票查询方式
    /// </summary>
    public enum DetrQeeryType
    {
        /// <summary>
        /// 航空公司系统（Inventory Control System）编号
        /// </summary>
        CN,
        /// <summary>
        /// 按票号
        /// </summary>
        TN,
        /// <summary>
        /// 按名称
        /// </summary>
        NM,
        /// <summary>
        /// 按证件号
        /// </summary>
        NI
    }
}
