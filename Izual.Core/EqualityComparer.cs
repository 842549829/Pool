#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Core.EqualityComparer.cs
// description：自定义相等比较器。
// 
// create by：Izual ,2012/06/11
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Izual {
    /// <summary>
    /// 自定义相等比较器
    /// </summary>
    /// <typeparam name="T"> 要比较相等的类型 </typeparam>
    public class EqualityComparer<T> : IEqualityComparer<T> {
        private readonly Func<T, T, bool> equal;
        private readonly Func<T, int> hash;

        /// <summary>
        /// 初始化 EqualityComparer 类型的新实例
        /// </summary>
        public EqualityComparer() {
            equal = CreateDefaultEqualsMethod();
            hash = CreateDefaultGetHashCodeMethod();
        }

        /// <summary>
        /// 初始化 EqualityComparer 类型的新实例，并指定作为相等判断依据的属性。
        /// </summary>
        /// <param name="propertyNames"> 并指定作为相等判断依据的属性 </param>
        public EqualityComparer(params string[] propertyNames) {
            equal = CreateEqualsMethod(propertyNames);
            hash = CreateGetHashCodeMethod(propertyNames);
        }

        /// <summary>
        /// 初始化 EqualityComparer 类型的新实例，并指定作为相等判断依据的属性。
        /// </summary>
        /// <param name="properties"> 并指定作为相等判断依据的属性 </param>
        public EqualityComparer(params PropertyInfo[] properties) {
            equal = CreateEqualsMethod(properties);
            hash = CreateGetHashCodeMethod(properties);
        }

        /// <summary>
        /// 初始化 EqualityComparer 类型的新实例，并指定用于判断两个元素相等的方法，以及计算元素 Hash 值的方法。
        /// </summary>
        /// <param name="equal"> 用于判断元素相等的方法 </param>
        /// <param name="hash"> 用于计算元素 Hash 值的方法 </param>
        public EqualityComparer(Func<T, T, bool> equal, Func<T, int> hash) {
            this.equal = equal ?? CreateDefaultEqualsMethod();
            this.hash = hash ?? CreateDefaultGetHashCodeMethod();
        }

        /// <summary>
        /// 获取计算 Hash 值的方法
        /// </summary>
        public Func<T, int> Hash {
            get { return hash; }
        }

        #region IEqualityComparer<T> Members

        /// <summary>
        /// 调用初始化时传入的委托，判断给定的两个对象是否相等
        /// </summary>
        /// <param name="x"> 要判断相等的第一个 T 类型的对象 </param>
        /// <param name="y"> 要判断相等的第二个 T 类型的对象 </param>
        /// <returns> 如果 x 与 y 相等，返回true，否则返回 false </returns>
        public bool Equals(T x, T y) {
            return equal(x, y);
        }

        /// <summary>
        /// 获取指定对象的
        /// </summary>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public int GetHashCode(T obj) {
            return obj.GetHashCode();
        }

        #endregion

        /// <summary>
        /// 创建默认的相等比较方法
        /// </summary>
        /// <returns> </returns>
        public static Func<T, T, bool> CreateDefaultEqualsMethod() {
            var t = typeof(T);
            // 最终要生成的方法为：(x,y)=>x.Equals(y)

            // x
            var x = Expression.Parameter(typeof(T), "x");
            // y
            var y = Expression.Parameter(typeof(T), "y");
            // x.Equals(y)
            var equals = Expression.Call(x, t.GetMethod("Equals", new[] { typeof(T) }), new Expression[] { y });

            // (x,y)=>x.Equals(y)
            var method = Expression.Lambda<Func<T, T, bool>>(equals, new[] { x, y });
            return method.Compile();
        }

        /// <summary>
        /// 创建相等比较方法
        /// </summary>
        /// <param name="keys"> string 数组，用于指定通过类型上的哪些属性来比较对象是否相等。 </param>
        /// <returns> 返回封装比较两个对象是否相等的方法 </returns>
        public static Func<T, T, bool> CreateEqualsMethod(params string[] keys) {
            if (keys == null || keys.Length == 0) {
                return CreateDefaultEqualsMethod();
            }

            Type t = typeof(T);
            var props = new List<PropertyInfo>();
            keys.ForEach(key => {
                PropertyInfo prop = t.GetProperty(key);
                if (prop == null) {
                    throw new InvalidOperationException(string.Format("类型 \"{0}\" 未定义名为：\"{1}\" 的属性或字段。", t.Name, key));
                }
                props.Add(prop);
            });

            return CreateEqualsMethod(props.ToArray());
        }

        /// <summary>
        /// 创建相等比较方法
        /// </summary>
        /// <param name="props"> PropertyInfo 数组，用于指定通过类型上的哪些属性来比较对象是否相等。 </param>
        /// <returns> 返回封装比较两个对象是否相等的方法 </returns>
        public static Func<T, T, bool> CreateEqualsMethod(params PropertyInfo[] props) {
            // 最终要生成的方法为：
            // 如果没有标识属性：(x,y)=>{x.Equals(y);}
            // 如果有标识属性： (x,y)=>{x.prop1.Equals(y.prop1) && x.prop2.Equals(y.prop2) && ......}

            if (props == null || props.Length == 0) {
                return CreateDefaultEqualsMethod();
            }

            // x
            ParameterExpression x = Expression.Parameter(typeof(T), "x");
            // y
            ParameterExpression y = Expression.Parameter(typeof(T), "y");

            List<MethodCallExpression> equalsList = (from p in props
                                                     let px = Expression.PropertyOrField(x, p.Name)
                                                     let py = Expression.PropertyOrField(y, p.Name)
                                                     select Expression.Call(px, p.PropertyType.GetMethod("Equals", new[] { p.PropertyType }), new Expression[] { py })).ToList();

            Expression equals = equalsList[0];
            for (int i = 1; i < equalsList.Count; i++) {
                // x.prop1.Equals(y.prop1) && x.prop2.Equals(y.prop2) && ......
                equals = Expression.AndAlso(equals, equalsList[i]);
            }

            // (x,y)=>{x.prop1.Equals(y.prop1) && x.prop2.Equals(y.prop2) && ......}
            Expression<Func<T, T, bool>> method = Expression.Lambda<Func<T, T, bool>>(equals, new[] { x, y });
            return method.Compile();
        }

        /// <summary>
        /// 创建默认的计算 Hash 值的方法
        /// </summary>
        /// <returns> 返回调用对象的 GetHashCode 方法的方法 </returns>
        public static Func<T, int> CreateDefaultGetHashCodeMethod() {
            // 最终需要生成的方法：如果没有指定作为计算依据的属性：x=>x.GetHashCode()

            Type t = typeof(T);
            // x
            ParameterExpression x = Expression.Parameter(t, "x");
            // x.GetHashCode
            MethodCallExpression hash = Expression.Call(x, t.GetMethod("GetHashCode", Type.EmptyTypes));

            Expression<Func<T, int>> method = Expression.Lambda<Func<T, int>>(hash, new[] { x });
            return method.Compile();
        }

        /// <summary>
        /// 创建计算 Hash 值的方法
        /// </summary>
        /// <param name="keys"> 作为计算依据的属性名称 </param>
        /// <returns> 返回封装计算对象 Hash 值的方法 </returns>
        public static Func<T, int> CreateGetHashCodeMethod(params string[] keys) {
            if (keys == null || keys.Length == 0) {
                return CreateDefaultGetHashCodeMethod();
            }

            var t = typeof(T);
            var props = new List<PropertyInfo>();
            keys.ForEach(key => {
                var prop = t.GetProperty(key);
                if (prop == null) {
                    throw new InvalidOperationException(string.Format("类型 \"{0}\" 中未定义名为：\"{1}\" 的属性或字段。", t.Name, key));
                }
                props.Add(prop);
            });
            return CreateGetHashCodeMethod(props.ToArray());
        }

        /// <summary>
        /// 创建计算 Hash 值的方法
        /// </summary>
        /// <param name="props"> 作为计算依据的属性列表 </param>
        /// <returns> 返回封装计算对象 Hash 值的方法 </returns>
        public static Func<T, int> CreateGetHashCodeMethod(params PropertyInfo[] props) {
            // 最终需要生成的方法：
            // 如果没有指定作为计算依据的属性：x=>x.GetHashCode()
            // 如果指定了作为计算依据的属性：x=>(x.prop1.ToString()+x.prop2.ToString()+......).GetHashCode()
            if (props == null || props.Length == 0) {
                return CreateDefaultGetHashCodeMethod();
            }

            var t = typeof(T);

            // x
            var x = Expression.Parameter(typeof(T), "x");

            // x.prop1.ToString() + x.prop2.ToString()
            // -_-!  没有为类型“System.String”和“System.String”定义二进制运算符 Add。
            // so : string.Concat(x.prop1,x.prop2,......)
            // =.=! 类型为“System.Int32”的表达式不能用于方法“System.String Concat(System.Object, System.Object)”的类型为“System.Object”的参数
            // and so：string.Concat(x.prop1.ToString(),x.prop2.ToString(),......)
            var pxs = new List<MethodCallExpression>();
            var types = new Type[props.Length];
            for (var i = 0; i < props.Length; i++) {
                // x.prop
                var px = Expression.PropertyOrField(x, props[i].Name);
                // x.prop.ToString
                var px2Str = Expression.Call(px, props[i].PropertyType.GetMethod("ToString", Type.EmptyTypes));
                pxs.Add(px2Str);
                types[i] = typeof(string);
            }
            // string.Concat(x.prop1.ToString(),x.prop2.ToString(),......)
            var concat = Expression.Call(typeof(string).GetMethod("Concat", types), pxs);

            // string.Concat(x.prop1.ToString(),x.prop2.ToString(),......).GetHashCode
            var hashMethod = Expression.Call(concat, t.GetMethod("GetHashCode", Type.EmptyTypes));

            var method = Expression.Lambda<Func<T, int>>(hashMethod, new[] { x });
            return method.Compile();
        }
    }
}