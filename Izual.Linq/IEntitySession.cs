#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Linq.IEntitySession.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Linq;

namespace Izual.Linq {
    public interface IEntitySession {
        IEntryProvider Provider{ get; }
        ISessionTable<T> GetTable<T>(string EntryId);
        ISessionTable GetTable(Type elementType, string EntryId);
        void SubmitChanges();
    }

    public interface ISessionTable : IQueryable {
        IEntitySession Session{ get; }
        IEntry ProviderTable{ get; }
        void SetSubmitAction(object instance, SubmitAction action);
        SubmitAction GetSubmitAction(object instance);
    }

    public interface ISessionTable<T> : IQueryable<T>, ISessionTable {
        new IEntry<T> ProviderTable{ get; }
        void SetSubmitAction(T instance, SubmitAction action);
        SubmitAction GetSubmitAction(T instance);
    }

    public enum SubmitAction {
        None,
        Update,
        PossibleUpdate,
        Insert,
        InsertOrUpdate,
        Delete
    }

    public static class SessionTableExtensions {
        public static void InsertOnSubmit<T>(this ISessionTable<T> table, T instance) {
            table.SetSubmitAction(instance, SubmitAction.Insert);
        }

        public static void InsertOnSubmit(this ISessionTable table, object instance) {
            table.SetSubmitAction(instance, SubmitAction.Insert);
        }

        public static void InsertOrUpdateOnSubmit<T>(this ISessionTable<T> table, T instance) {
            table.SetSubmitAction(instance, SubmitAction.InsertOrUpdate);
        }

        public static void InsertOrUpdateOnSubmit(this ISessionTable table, object instance) {
            table.SetSubmitAction(instance, SubmitAction.InsertOrUpdate);
        }

        public static void UpdateOnSubmit<T>(this ISessionTable<T> table, T instance) {
            table.SetSubmitAction(instance, SubmitAction.Update);
        }

        public static void UpdateOnSubmit(this ISessionTable table, object instance) {
            table.SetSubmitAction(instance, SubmitAction.Update);
        }

        public static void DeleteOnSubmit<T>(this ISessionTable<T> table, T instance) {
            table.SetSubmitAction(instance, SubmitAction.Delete);
        }

        public static void DeleteOnSubmit(this ISessionTable table, object instance) {
            table.SetSubmitAction(instance, SubmitAction.Delete);
        }
    }
}