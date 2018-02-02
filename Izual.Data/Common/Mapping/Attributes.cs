#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.Attributes.cs
// description：
// 
// create by：Izual ,2012/07/04
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;

namespace Izual.Data.Mapping {
    /// <summary>
    /// <para>指定对象或对象的属性，在仓储中的映射。</para> <para>可应用于：类，结构，枚举，属性，字段。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MappingAttribute : Attribute {
        private readonly string name = string.Empty;

        /// <summary>
        /// 初始化 MappingAttribute 类型的新实例。
        /// </summary>
        /// <param name="name"> 映射到仓储中的名称。 </param>
        public MappingAttribute(string name) {
            if(string.IsNullOrEmpty(name)) {
                throw new ArgumentException("映射的名称无效。");
            }
            this.name = name;
        }

        /// <summary>
        /// 获取在仓储中映射的名称。
        /// </summary>
        public string Name {
            get { return name; }
        }
    }

    /// <summary>
    /// <para>将属性标识为对象的标识属性。（可通过确定该属性来标识对象）</para>
    /// <para>可应用于：属性、字段</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class IdentifierAttribute : Attribute {}

    /// <summary>
    /// <para>将属性标识为由仓储提供方生成。（持久化实现时，不应将由 GeneratedAttribute 特性的属性或字段，显式提交给仓储提供方。）</para>
    /// <para>可应用于：属性、字段</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class GeneratedAttribute : Attribute {}

    /// <summary>
    /// 持久化配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class PersistenceAttribute : Attribute {
        /// <summary>
        /// 获取或设置持久化方式
        /// </summary>
        public PersistenceMode Mode{ get; set; }
    }

    /// <summary>
    /// <para>将属性标识为聚合。</para>
    /// <para>可应用于：属性、字段</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class IsAggregationAttribute : Attribute { }
    /// <summary>
    /// 持久化方式
    /// </summary>
    /// <remarks>
    /// 如果属性被 GeneratedAttribute 标识，任何持久化方式都不会产生效果。
    /// </remarks>
    public enum PersistenceMode {
        /// <summary>
        /// 不存储
        /// </summary>
        None,

        /// <summary>
        /// 以常规的方式存储（每一个对象实例存储为一条记录）
        /// </summary>
        Normal,

        /// <summary>
        /// <para>作为 PropertyBag 存储</para> <para>将类或结构的每个属性保存为一条记录</para>
        /// </summary>
        PropertyBag,

        /// <summary>
        /// <para>作为一个外部引用存储</para> <para>如果属性是类或结构，保存该类型的标识属性（由 IdentiferAttribute 指定），否则保存该属性的值。</para>
        /// </summary>
        Reference,

        /// <summary>
        /// <para>级联存储</para> <para>如果属性是类或结构，将针对该属性做一次常规方式存储，否则保存该属性的值。</para>
        /// </summary>
        Cascading
    }
}