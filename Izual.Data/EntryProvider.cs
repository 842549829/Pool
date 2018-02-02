#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.EntryProvider.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Izual.Data.Common;
using Izual.Linq;

namespace Izual.Data {
    /// <summary>
    /// A LINQ IQueryable query provider that executes database queries over a DbConnection
    /// </summary>
    public abstract class EntryProvider : QueryProvider, IEntryProvider, ICreateExecutor {
        private readonly QueryLanguage language;
        private readonly QueryMapping mapping;
        private readonly Dictionary<EntryMapping, IEntry> entries;
        private QueryCache cache;
        private TextWriter log;
        private QueryPolicy policy;

        public EntryProvider(QueryLanguage language, QueryMapping mapping, QueryPolicy policy) {
            if(language == null)
                throw new InvalidOperationException("Language not specified");
            if(mapping == null)
                throw new InvalidOperationException("Mapping not specified");
            if(policy == null)
                throw new InvalidOperationException("Policy not specified");
            this.language = language;
            this.mapping = mapping;
            this.policy = policy;
            entries = new Dictionary<EntryMapping, IEntry>();
        }

        public QueryMapping Mapping {
            get { return mapping; }
        }

        public QueryLanguage Language {
            get { return language; }
        }

        public QueryPolicy Policy {
            get { return policy; }

            set {
                if(value == null) {
                    policy = QueryPolicy.Default;
                }
                else {
                    policy = value;
                }
            }
        }

        public TextWriter Log {
            get { return log; }
            set { log = value; }
        }

        public QueryCache Cache {
            get { return cache; }
            set { cache = value; }
        }

        #region ICreateExecutor Members

        QueryExecutor ICreateExecutor.CreateExecutor() {
            return CreateExecutor();
        }

        #endregion

        #region IEntityProvider Members

        public bool CanBeEvaluatedLocally(Expression expression) {
            return Mapping.CanBeEvaluatedLocally(expression);
        }

        public virtual bool CanBeParameter(Expression expression) {
            Type type = TypeHelper.GetNonNullableType(expression.Type);
            switch(Type.GetTypeCode(type)) {
                case TypeCode.Object:
                    if(expression.Type == typeof(Byte[]) || expression.Type == typeof(Char[]))
                        return true;
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Execute the query expression (does translation, etc.)
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public override object Execute(Expression expression) {
            var lambda = expression as LambdaExpression;

            if(lambda == null && cache != null && expression.NodeType != ExpressionType.Constant) {
                return cache.Execute(expression);
            }

            Expression plan = GetExecutionPlan(expression);

            if(lambda != null) {
                // compile & return the execution plan so it can be used multiple times
                LambdaExpression fn = Expression.Lambda(lambda.Type, plan, lambda.Parameters);
                return fn.Compile();
            }
            else {
                // compile the execution plan and invoke it
                Expression<Func<object>> efn = Expression.Lambda<Func<object>>(Expression.Convert(plan, typeof(object)));
                Func<object> fn = efn.Compile();
                return fn();
            }
        }

        public virtual IEntry GetEntry(Type type) {
            return CreateEntry(type);
        }
        public virtual IEntry<T> GetEntry<T>() {
            return (IEntry<T>)GetEntry(typeof(T));
        }

        #endregion

        protected virtual IEntry CreateEntry(Type type) {
            return (IEntry)ObjectCreator.Create(typeof(DbEntry<>).MakeGenericType(type),this,mapping.GetEntry(type));
        }

        protected abstract QueryExecutor CreateExecutor();

        public override string GetQueryText(Expression expression) {
            Expression plan = GetExecutionPlan(expression);
            string[] commands = CommandGatherer.Gather(plan).Select(c => c.CommandText).ToArray();
            return string.Join("\n\n", commands);
        }

        public string GetQueryPlan(Expression expression) {
            Expression plan = GetExecutionPlan(expression);
            return DbExpressionWriter.WriteToString(Language, plan);
        }

        protected virtual QueryTranslator CreateTranslator() {
            return new QueryTranslator(language, mapping, policy);
        }

        public abstract void DoTransacted(Action action);
        public abstract void DoConnected(Action action);
        public abstract int ExecuteCommand(string commandText);

        /// <summary>
        /// Convert the query expression into an execution plan
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public virtual Expression GetExecutionPlan(Expression expression) {
            // strip off lambda for now
            var lambda = expression as LambdaExpression;
            if(lambda != null)
                expression = lambda.Body;

            QueryTranslator translator = CreateTranslator();

            // translate query into client & server parts
            Expression translation = translator.Translate(expression);

            ReadOnlyCollection<ParameterExpression> parameters = lambda != null ? lambda.Parameters : null;
            Expression provider = Find(expression, parameters, typeof(EntryProvider));
            if(provider == null) {
                Expression rootQueryable = Find(expression, parameters, typeof(IQueryable));
                provider = Expression.Property(rootQueryable, typeof(IQueryable).GetProperty("Provider"));
            }

            return translator.Police.BuildExecutionPlan(translation, provider);
        }

        private Expression Find(Expression expression, IList<ParameterExpression> parameters, Type type) {
            if(parameters != null) {
                Expression found = parameters.FirstOrDefault(p => type.IsAssignableFrom(p.Type));
                if(found != null)
                    return found;
            }
            return TypedSubtreeFinder.Find(expression, type);
        }

        public static QueryMapping GetMapping(string settingsFilename) {
            return new DefaultMapping(settingsFilename);
        }

        public static Type GetProviderType(string providerName) {
            if(!string.IsNullOrEmpty(providerName)) {
                Type type = FindInstancesIn(typeof(EntryProvider), providerName).FirstOrDefault();
                if(type != null)
                    return type;
            }
            return null;
        }

        private static Type FindLoadedType(string typeName) {
            foreach(Assembly assem in AppDomain.CurrentDomain.GetAssemblies()) {
                Type type = assem.GetType(typeName, false, true);
                if(type != null)
                    return type;
            }
            return null;
        }

        private static IEnumerable<Type> FindInstancesIn(Type type, string assemblyName) {
            Assembly assembly = GetAssemblyForNamespace(assemblyName);
            if(assembly != null) {
                foreach(Type atype in assembly.GetTypes()) {
                    if(string.Compare(atype.Namespace, assemblyName, true) == 0 && type.IsAssignableFrom(atype)) {
                        yield return atype;
                    }
                }
            }
        }

        private static Assembly GetAssemblyForNamespace(string nspace) {
            foreach(Assembly assem in AppDomain.CurrentDomain.GetAssemblies()) {
                if(assem.FullName.Contains(nspace)) {
                    return assem;
                }
            }

            return Load(nspace + ".dll");
        }

        private static Assembly Load(string name) {
            // try to load it.
            try {
                return Assembly.LoadFrom(name);
            }
            catch {}
            return null;
        }

        #region Nested type: CommandGatherer

        private class CommandGatherer : DbExpressionVisitor {
            private readonly List<QueryCommand> commands = new List<QueryCommand>();

            public static ReadOnlyCollection<QueryCommand> Gather(Expression expression) {
                var gatherer = new CommandGatherer();
                gatherer.Visit(expression);
                return gatherer.commands.AsReadOnly();
            }

            protected override Expression VisitConstant(ConstantExpression c) {
                var qc = c.Value as QueryCommand;
                if(qc != null) {
                    commands.Add(qc);
                }
                return c;
            }
        }

        #endregion

        #region Nested type: EntityTable

        public class DbEntry<T> : Query<T>, IEntry<T>, IMapped {
            private readonly EntryMapping entity;
            private readonly EntryProvider provider;

            public DbEntry(EntryProvider provider, EntryMapping entity) : base(provider, typeof(IEntry<T>)) {
                this.provider = provider;
                this.entity = entity;
            }

            public Type EntityType {
                get { return Entry.Type; }
            }

            #region IEntityTable<T> Members

            public new IEntryProvider Provider {
                get { return provider; }
            }

            public string EntryId {
                get { return entity.Name; }
            }

            public int Insert(T instance) {
                return Updatable.Insert(this, instance);
            }

            int IEntry.Insert(object instance) {
                return Insert((T)instance);
            }

            public int Delete(T instance) {
                return Updatable.Delete(this, instance);
            }

            int IEntry.Delete(object instance) {
                return Delete((T)instance);
            }

            public int Update(T instance) {
                return Updatable.Update(this, instance);
            }

            int IEntry.Update(object instance) {
                return Update((T)instance);
            }

            public int InsertOrUpdate(T instance) {
                return Updatable.InsertOrUpdate(this, instance);
            }

            int IEntry.InsertOrUpdate(object instance) {
                return InsertOrUpdate((T)instance);
            }

            #endregion

            #region IMapped Members

            public EntryMapping Entry {
                get { return entity; }
            }

            #endregion
        }

        #endregion
    }
}