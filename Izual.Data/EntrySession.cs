#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.EntrySession.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Izual.Data.Common;
using Izual.Linq;

namespace Izual.Data {
    public class EntrySession : IEntitySession {
        private readonly EntryProvider provider;
        private readonly SessionProvider sessionProvider;
        private readonly Dictionary<EntryMapping, ISessionTable> tables;

        public EntrySession(EntryProvider provider) {
            this.provider = provider;
            sessionProvider = new SessionProvider(this, provider);
            tables = new Dictionary<EntryMapping, ISessionTable>();
        }

        public IEntryProvider Provider {
            get { return sessionProvider; }
        }

        #region IEntitySession Members

        IEntryProvider IEntitySession.Provider {
            get { return Provider; }
        }

        public ISessionTable GetTable(Type elementType, string EntryId) {
            //return this.GetTable(this.sessionProvider.Provider.Mapping.GetEntry(elementType, EntryId));
            return GetTable(sessionProvider.Provider.Mapping.GetEntry(elementType));
        }

        public ISessionTable<T> GetTable<T>(string EntryId) {
            return (ISessionTable<T>)GetTable(typeof(T), EntryId);
        }

        public virtual void SubmitChanges() {
            provider.DoTransacted(delegate {
                                      var submitted = new List<TrackedItem>();

                                      // do all submit actions
                                      foreach(TrackedItem item in GetOrderedItems()) {
                                          if(item.Table.SubmitChanges(item)) {
                                              submitted.Add(item);
                                          }
                                      }

                                      // on completion, accept changes
                                      foreach(TrackedItem item in submitted) {
                                          item.Table.AcceptChanges(item);
                                      }
                                  });
        }

        #endregion

        protected IEnumerable<ISessionTable> GetTables() {
            return tables.Values;
        }

        protected ISessionTable GetTable(EntryMapping entity) {
            ISessionTable table;
            if(!tables.TryGetValue(entity, out table)) {
                table = CreateTable(entity);
                tables.Add(entity, table);
            }
            return table;
        }

        private object OnEntityMaterialized(EntryMapping entity, object instance) {
            var table = (IEntitySessionTable)GetTable(entity);
            return table.OnEntityMaterialized(instance);
        }

        protected virtual ISessionTable CreateTable(EntryMapping entry) {
            return (ISessionTable)Activator.CreateInstance(typeof(TrackedTable<>).MakeGenericType(entry.Type), new object[] {this, entry});
        }

        private IEnumerable<TrackedItem> GetOrderedItems() {
            List<TrackedItem> items = (from tab in GetTables()
                                       from ti in ((ITrackedTable)tab).TrackedItems
                                       where ti.State != SubmitAction.None
                                       select ti).ToList();

            // build edge maps to represent all references between entities
            List<Edge> edges = GetEdges(items).Distinct().ToList();
            ILookup<TrackedItem, TrackedItem> stLookup = edges.ToLookup(e => e.Source, e => e.Target);
            ILookup<TrackedItem, TrackedItem> tsLookup = edges.ToLookup(e => e.Target, e => e.Source);

            return items.Sort(item => {
                                  switch(item.State) {
                                      case SubmitAction.Insert:
                                      case SubmitAction.InsertOrUpdate:
                                          // all things this instance depends on must come first
                                          IEnumerable<TrackedItem> beforeMe = stLookup[item];

                                          // if another object exists with same key that is being deleted, then the delete must come before the insert
                                          object cached = item.Table.GetFromCacheById(provider.Mapping.GetPrimaryKey(item.Entity, item.Instance));
                                          if(cached != null && cached != item.Instance) {
                                              TrackedItem ti = item.Table.GetTrackedItem(cached);
                                              if(ti != null && ti.State == SubmitAction.Delete) {
                                                  beforeMe = beforeMe.Concat(new[] {ti});
                                              }
                                          }
                                          return beforeMe;

                                      case SubmitAction.Delete:
                                          // all things that depend on this instance must come first
                                          return tsLookup[item];
                                      default:
                                          return TrackedItem.EmptyList;
                                  }
                              });
        }

        private TrackedItem GetTrackedItem(EntryInfo entity) {
            var table = (ITrackedTable)GetTable(entity.Mapping);
            return table.GetTrackedItem(entity.Instance);
        }

        private IEnumerable<Edge> GetEdges(IEnumerable<TrackedItem> items) {
            foreach(TrackedItem c in items) {
                foreach(EntryInfo d in provider.Mapping.GetDependingEntities(c.Entity, c.Instance)) {
                    TrackedItem dc = GetTrackedItem(d);
                    if(dc != null) {
                        yield return new Edge(dc, c);
                    }
                }
                foreach(EntryInfo d in provider.Mapping.GetDependentEntities(c.Entity, c.Instance)) {
                    TrackedItem dc = GetTrackedItem(d);
                    if(dc != null) {
                        yield return new Edge(c, dc);
                    }
                }
            }
        }

        #region Nested type: Edge

        private class Edge : IEquatable<Edge> {
            private readonly int hash;

            internal Edge(TrackedItem source, TrackedItem target) {
                Source = source;
                Target = target;
                hash = Source.GetHashCode() + Target.GetHashCode();
            }

            internal TrackedItem Source{ get; private set; }
            internal TrackedItem Target{ get; private set; }

            #region IEquatable<Edge> Members

            public bool Equals(Edge edge) {
                return edge != null && Source == edge.Source && Target == edge.Target;
            }

            #endregion

            public override bool Equals(object obj) {
                return Equals(obj as Edge);
            }

            public override int GetHashCode() {
                return hash;
            }
        }

        #endregion

        #region Nested type: IEntitySessionTable

        private interface IEntitySessionTable : ISessionTable {
            EntryMapping Entity{ get; }
            object OnEntityMaterialized(object instance);
        }

        #endregion

        #region Nested type: ITrackedTable

        private interface ITrackedTable : IEntitySessionTable {
            IEnumerable<TrackedItem> TrackedItems{ get; }
            object GetFromCacheById(object key);
            TrackedItem GetTrackedItem(object instance);
            bool SubmitChanges(TrackedItem item);
            void AcceptChanges(TrackedItem item);
        }

        #endregion

        #region Nested type: SessionExecutor

        private class SessionExecutor : QueryExecutor {
            private readonly QueryExecutor executor;
            private readonly EntrySession session;

            public SessionExecutor(EntrySession session, QueryExecutor executor) {
                this.session = session;
                this.executor = executor;
            }

            public override int RowsAffected {
                get { return executor.RowsAffected; }
            }

            public override object Convert(object value, Type type) {
                return executor.Convert(value, type);
            }

            public override IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, EntryMapping entity, object[] paramValues) {
                return executor.Execute(command, Wrap(fnProjector, entity), entity, paramValues);
            }

            public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream) {
                return executor.ExecuteBatch(query, paramSets, batchSize, stream);
            }

            public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, EntryMapping entity, int batchSize, bool stream) {
                return executor.ExecuteBatch(query, paramSets, Wrap(fnProjector, entity), entity, batchSize, stream);
            }

            public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, EntryMapping entity, object[] paramValues) {
                return executor.ExecuteDeferred(query, Wrap(fnProjector, entity), entity, paramValues);
            }

            public override int ExecuteCommand(QueryCommand query, object[] paramValues) {
                return executor.ExecuteCommand(query, paramValues);
            }

            private Func<FieldReader, T> Wrap<T>(Func<FieldReader, T> fnProjector, EntryMapping entity) {
                Func<FieldReader, T> fnWrapped = (fr) => (T)session.OnEntityMaterialized(entity, fnProjector(fr));
                return fnWrapped;
            }
        }

        #endregion

        #region Nested type: SessionProvider

        private class SessionProvider : QueryProvider, IEntryProvider, ICreateExecutor {
            private readonly EntryProvider provider;
            private readonly EntrySession session;

            public SessionProvider(EntrySession session, EntryProvider provider) {
                this.session = session;
                this.provider = provider;
            }

            public EntryProvider Provider {
                get { return provider; }
            }

            #region ICreateExecutor Members

            QueryExecutor ICreateExecutor.CreateExecutor() {
                return new SessionExecutor(session, ((ICreateExecutor)provider).CreateExecutor());
            }

            #endregion

            #region IEntityProvider Members

            public override object Execute(Expression expression) {
                return provider.Execute(expression);
            }

            public IEntry<T> GetEntry<T>() {
                return provider.GetEntry<T>();
            }

            public IEntry GetEntry(Type type) {
                return provider.GetEntry(type);
            }

            public bool CanBeEvaluatedLocally(Expression expression) {
                return provider.Mapping.CanBeEvaluatedLocally(expression);
            }

            public bool CanBeParameter(Expression expression) {
                return provider.CanBeParameter(expression);
            }

            #endregion

            public override string GetQueryText(Expression expression) {
                return provider.GetQueryText(expression);
            }
        }

        #endregion

        #region Nested type: SessionTable

        private abstract class SessionTable<T> : Query<T>, ISessionTable<T>, ISessionTable, IEntitySessionTable {
            private readonly EntryMapping entity;
            private readonly EntrySession session;
            private readonly IEntry<T> underlyingTable;

            public SessionTable(EntrySession session, EntryMapping entity) : base(session.sessionProvider, typeof(ISessionTable<T>)) {
                this.session = session;
                this.entity = entity;
                underlyingTable = this.session.Provider.GetEntry<T>();
            }

            #region IEntitySessionTable Members

            public EntryMapping Entity {
                get { return entity; }
            }

            public virtual object OnEntityMaterialized(object instance) {
                return instance;
            }

            #endregion

            #region ISessionTable<T> Members

            public IEntitySession Session {
                get { return session; }
            }

            public IEntry<T> ProviderTable {
                get { return underlyingTable; }
            }

            IEntry ISessionTable.ProviderTable {
                get { return underlyingTable; }
            }

            public virtual void SetSubmitAction(T instance, SubmitAction action) {
                throw new NotImplementedException();
            }

            void ISessionTable.SetSubmitAction(object instance, SubmitAction action) {
                SetSubmitAction((T)instance, action);
            }

            public virtual SubmitAction GetSubmitAction(T instance) {
                throw new NotImplementedException();
            }

            SubmitAction ISessionTable.GetSubmitAction(object instance) {
                return GetSubmitAction((T)instance);
            }

            #endregion
        }

        #endregion

        #region Nested type: TrackedItem

        private class TrackedItem {
            public static readonly IEnumerable<TrackedItem> EmptyList = new TrackedItem[] {};
            private readonly bool hookedEvent;
            private readonly object instance;
            private readonly object original;
            private readonly SubmitAction state;
            private readonly ITrackedTable table;

            internal TrackedItem(ITrackedTable table, object instance, object original, SubmitAction state, bool hookedEvent) {
                this.table = table;
                this.instance = instance;
                this.original = original;
                this.state = state;
                this.hookedEvent = hookedEvent;
            }

            public ITrackedTable Table {
                get { return table; }
            }

            public EntryMapping Entity {
                get { return table.Entity; }
            }

            public object Instance {
                get { return instance; }
            }

            public object Original {
                get { return original; }
            }

            public SubmitAction State {
                get { return state; }
            }

            public bool HookedEvent {
                get { return hookedEvent; }
            }
        }

        #endregion

        #region Nested type: TrackedTable

        private class TrackedTable<T> : SessionTable<T>, ITrackedTable {
            private readonly Dictionary<object, T> identityCache;
            private readonly Dictionary<T, TrackedItem> tracked;

            public TrackedTable(EntrySession session, EntryMapping entity) : base(session, entity) {
                tracked = new Dictionary<T, TrackedItem>();
                identityCache = new Dictionary<object, T>();
            }

            private QueryMapping Mapping {
                get { return ((EntrySession)Session).provider.Mapping; }
            }

            #region ITrackedTable Members

            IEnumerable<TrackedItem> ITrackedTable.TrackedItems {
                get { return tracked.Values; }
            }

            TrackedItem ITrackedTable.GetTrackedItem(object instance) {
                TrackedItem ti;
                if(tracked.TryGetValue((T)instance, out ti))
                    return ti;
                return null;
            }

            object ITrackedTable.GetFromCacheById(object key) {
                T cached;
                identityCache.TryGetValue(key, out cached);
                return cached;
            }

            bool ITrackedTable.SubmitChanges(TrackedItem item) {
                return SubmitChanges(item);
            }

            void ITrackedTable.AcceptChanges(TrackedItem item) {
                AcceptChanges(item);
            }

            public override object OnEntityMaterialized(object instance) {
                var typedInstance = (T)instance;
                T cached = AddToCache(typedInstance);
                if((object)cached == (object)typedInstance) {
                    AssignAction(typedInstance, SubmitAction.PossibleUpdate);
                }
                return cached;
            }

            #endregion

            private bool SubmitChanges(TrackedItem item) {
                switch(item.State) {
                    case SubmitAction.Delete:
                        ProviderTable.Delete(item.Instance);
                        return true;
                    case SubmitAction.Insert:
                        ProviderTable.Insert(item.Instance);
                        return true;
                    case SubmitAction.InsertOrUpdate:
                        ProviderTable.InsertOrUpdate(item.Instance);
                        return true;
                    case SubmitAction.PossibleUpdate:
                        if(item.Original != null && Mapping.IsModified(item.Entity, item.Instance, item.Original)) {
                            ProviderTable.Update(item.Instance);
                            return true;
                        }
                        break;
                    case SubmitAction.Update:
                        ProviderTable.Update(item.Instance);
                        return true;
                    default:
                        break; // do nothing
                }
                return false;
            }

            private void AcceptChanges(TrackedItem item) {
                switch(item.State) {
                    case SubmitAction.Delete:
                        RemoveFromCache((T)item.Instance);
                        AssignAction((T)item.Instance, SubmitAction.None);
                        break;
                    case SubmitAction.Insert:
                        AddToCache((T)item.Instance);
                        AssignAction((T)item.Instance, SubmitAction.PossibleUpdate);
                        break;
                    case SubmitAction.InsertOrUpdate:
                        AddToCache((T)item.Instance);
                        AssignAction((T)item.Instance, SubmitAction.PossibleUpdate);
                        break;
                    case SubmitAction.PossibleUpdate:
                    case SubmitAction.Update:
                        AssignAction((T)item.Instance, SubmitAction.PossibleUpdate);
                        break;
                    default:
                        break; // do nothing
                }
            }

            public override SubmitAction GetSubmitAction(T instance) {
                TrackedItem ti;
                if(tracked.TryGetValue(instance, out ti)) {
                    if(ti.State == SubmitAction.PossibleUpdate) {
                        if(Mapping.IsModified(ti.Entity, ti.Instance, ti.Original)) {
                            return SubmitAction.Update;
                        }
                        else {
                            return SubmitAction.None;
                        }
                    }
                    return ti.State;
                }
                return SubmitAction.None;
            }

            public override void SetSubmitAction(T instance, SubmitAction action) {
                switch(action) {
                    case SubmitAction.None:
                    case SubmitAction.PossibleUpdate:
                    case SubmitAction.Update:
                    case SubmitAction.Delete:
                        T cached = AddToCache(instance);
                        if((object)cached != (object)instance) {
                            throw new InvalidOperationException("An different instance with the same key is already in the cache.");
                        }
                        break;
                }
                AssignAction(instance, action);
            }

            private T AddToCache(T instance) {
                object key = Mapping.GetPrimaryKey(Entity, instance);
                T cached;
                if(!identityCache.TryGetValue(key, out cached)) {
                    cached = instance;
                    identityCache.Add(key, cached);
                }
                return cached;
            }

            private void RemoveFromCache(T instance) {
                object key = Mapping.GetPrimaryKey(Entity, instance);
                identityCache.Remove(key);
            }

            private void AssignAction(T instance, SubmitAction action) {
                TrackedItem ti;
                tracked.TryGetValue(instance, out ti);

                switch(action) {
                    case SubmitAction.Insert:
                    case SubmitAction.InsertOrUpdate:
                    case SubmitAction.Update:
                    case SubmitAction.Delete:
                    case SubmitAction.None:
                        tracked[instance] = new TrackedItem(this, instance, ti != null ? ti.Original : null, action, ti != null ? ti.HookedEvent : false);
                        break;
                    case SubmitAction.PossibleUpdate:
                        var notify = instance as INotifyPropertyChanging;
                        if(notify != null) {
                            if(!ti.HookedEvent) {
                                notify.PropertyChanging += OnPropertyChanging;
                            }
                            tracked[instance] = new TrackedItem(this, instance, null, SubmitAction.PossibleUpdate, true);
                        }
                        else {
                            object original = Mapping.CloneEntity(Entity, instance);
                            tracked[instance] = new TrackedItem(this, instance, original, SubmitAction.PossibleUpdate, false);
                        }
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown SubmitAction: {0}", action));
                }
            }

            protected virtual void OnPropertyChanging(object sender, PropertyChangingEventArgs args) {
                TrackedItem ti;
                if(tracked.TryGetValue((T)sender, out ti) && ti.State == SubmitAction.PossibleUpdate) {
                    object clone = Mapping.CloneEntity(ti.Entity, ti.Instance);
                    tracked[(T)sender] = new TrackedItem(this, ti.Instance, clone, SubmitAction.Update, true);
                }
            }
        }

        #endregion
    }
}