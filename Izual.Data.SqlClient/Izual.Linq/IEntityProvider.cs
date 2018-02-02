#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.IEntityProvider.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Linq;
using System.Linq.Expressions;

namespace Izual.Linq {
    public interface IEntryProvider : IQueryProvider {
        //IEntry<T> GetEntry<T>(string EntryId);
        //IEntry GetEntry(Type type, string EntryId);
        IEntry GetEntry(Type type);
        IEntry<T> GetEntry<T>();
        bool CanBeEvaluatedLocally(Expression expression);
        bool CanBeParameter(Expression expression);
    }

    public interface IEntry : IUpdatable {
        new IEntryProvider Provider{ get; }
        string EntryId{ get; }
        int Insert(object instance);
        int Update(object instance);
        int Delete(object instance);
        int InsertOrUpdate(object instance);
    }

    public interface IEntry<T> : IEntry, IUpdatable<T> {
        int Insert(T instance);
        int Update(T instance);
        int Delete(T instance);
        int InsertOrUpdate(T instance);
    }
}