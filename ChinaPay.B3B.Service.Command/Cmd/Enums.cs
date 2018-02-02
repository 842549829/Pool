using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command
{

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
    public enum DETRQeeryType
    {
        /// <summary>
        /// 按编号
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
        /// 按身份证号
        /// </summary>
        NI
    }
    /// <summary>
    /// 命令集执行结果类型
    /// </summary>
    public enum ReturnResultType
    {
        /// <summary>
        /// 返回单条指令执行结果
        /// </summary>
        Single,
        /// <summary>
        /// 返回每条指令执行结果
        /// </summary>
        All
    }

    /// <summary>
    /// 命令集执行方式类型
    /// </summary>
    public enum ExecuteMothodType
    {
        /// <summary>
        /// 逐行执行
        /// </summary>
        LineByLine,
        /// <summary>
        /// 单批次多行；
        /// </summary>
        SingleBatch,
        /// <summary>
        /// 多批次单行；
        /// </summary>
        MultipleBatch
    }
}
