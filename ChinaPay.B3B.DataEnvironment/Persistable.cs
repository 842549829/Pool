namespace ChinaPay.B3B.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Transactions;
    using Izual.Linq;
    using Izual;

    public interface IPersistable<T> { }
    public static class Persistable {
        private static Func<bool> GetInsert(object obj) {
            if (obj == null) return null;
            var type = obj.GetType();
            if (!type.Implements(typeof(IPersistable<>))) return null;

            // find Persistable.Insert<T>(IPersistable<T>,bool);
            // Type.GetMethod(name,......) does not work on this case
            // 搜索条件：为泛型方法定义，Name 为 "Insert" ，2个参数，第一个参数的类型为 IPersistable<T>，第二个参数类型为 bool

            var method = (from m in typeof(Persistable).GetMethods()
                          let ps = m.GetParameters()
                          where m.IsGenericMethodDefinition && m.Name == "Insert" && ps.Length == 2 && ps[1].ParameterType == typeof(bool) &&
                          ps[0].ParameterType == typeof(IPersistable<>).MakeGenericType(m.GetGenericArguments()[0])
                          select m).SingleOrDefault();
            return method == null ? null : Expression.Lambda<Func<bool>>(Expression.Call(null, method.MakeGenericMethod(type), Expression.Constant(obj), Expression.Constant(true))).Compile();
        }
        public static bool Insert<T>(this IPersistable<T> obj, bool recursive) {
            var inserts = new List<Func<bool>>();
            if (recursive) {
                var mis = obj.GetType().GetMembers().Where(m => m.CanPreserve() && !m.GetDeclarationType().IsSimpleType()).Select(m => obj.Get(m.Name));
                mis.ForEach(m => {
                    var insert = GetInsert(m);
                    if (insert != null) {
                        inserts.Add(insert);
                    }
                });

                if (inserts.Count > 0) {
                    using (var tran = new TransactionScope()) {
                        inserts.ForEach(insert => insert());
                        DataContext.GetEntry<T>().Insert((T)obj);
                        tran.Complete();
                        return true;
                    }
                }
            }
            return DataContext.GetEntry<T>().Insert((T)obj) > 0;
        }
        public static bool Insert<T>(this IPersistable<T> obj) {
            return Insert(obj, false);
        }
        public static bool Update<T>(this IPersistable<T> obj) {
            return DataContext.GetEntry<T>().Update((T)obj) > 0;
        }

        public static bool InsertOrUpdate<T>(this IPersistable<T> obj) {
            return DataContext.GetEntry<T>().InsertOrUpdate((T)obj) > 0;
        }
        public static bool InsertOrUpdate<T>(this IPersistable<T> obj ,Expression<Func<T,bool>> where) {
            return DataContext.GetEntry<T>().InsertOrUpdate((T)obj, where) > 0;
        }
        public static bool Delete<T>(this IPersistable<T> obj) {
            return DataContext.GetEntry<T>().Delete((T)obj) > 0;
        }

        public static bool CanPreserve(this MemberInfo mi) {
            return mi != null && (mi.MemberType == MemberTypes.Property || mi.MemberType == MemberTypes.Field);
        }
        public static Type GetDeclarationType(this MemberInfo mi) {
            if (mi == null) throw new ArgumentNullException("mi");

            if (mi.MemberType == MemberTypes.Property) return ((PropertyInfo)mi).PropertyType;
            if (mi.MemberType == MemberTypes.Field) return ((FieldInfo)mi).FieldType;
            if (mi.MemberType == MemberTypes.Method) return ((MethodInfo)mi).ReturnType;

            return null;
        }
    }
}
