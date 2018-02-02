#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.DefaultMapping.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using Izual.Data.Common;
using Izual.Data.Common.Translation;
using Izual.Data.Mapping;
using Izual.Linq;

namespace Izual.Data {
    /// <summary>
    /// 映射器
    /// </summary>
    public class DefaultMapping : QueryMapping {
        private readonly Dictionary<Type, EntryMapping> dic = new Dictionary<Type, EntryMapping>();
        private readonly QueryTypeResolver typeResolver;
        private XmlDocument doc;

        /// <summary>
        /// 初始化 Mapper 类型的新实例。
        /// </summary>
        public DefaultMapping() : this(null, null) { }

        /// <summary>
        /// 用指定的配置，初始化 Mapper 类型的新实例。
        /// </summary>
        /// <param name="settingsFilename"> </param>
        public DefaultMapping(string settingsFilename) : this(null, settingsFilename) { }

        public DefaultMapping(QueryTypeResolver typeResolver) : this(typeResolver, null) { }

        /// <summary>
        /// 用指定的配置，指定的数据类型解析器， 初始化 Mapper 类型的新实例。
        /// </summary>
        /// <param name="typeResolver"> </param>
        /// <param name="settingsFilename"> </param>
        public DefaultMapping(QueryTypeResolver typeResolver, string settingsFilename) {
            this.typeResolver = typeResolver ?? new DbTypeResolver();
            if (!string.IsNullOrWhiteSpace(settingsFilename)) {
                LoadSettings(settingsFilename);
            }
        }

        /// <summary>
        /// 从属性信息中生成 MemberMapping 类型的实例。
        /// </summary>
        /// <param name="member"> 属性信息 </param>
        /// <returns> 返回生成的映射信息 </returns>
        public MemberMapping CreateMemberMapping(MemberInfo member) {
            if (member == null) throw new ArgumentNullException("member");
            if (!(member.MemberType == MemberTypes.Property || member.MemberType == MemberTypes.Field)) {
                throw new InvalidOperationException("无效的成员类型。");
            }
            QueryType queryType = typeResolver.GetColumnType(member.GetMemberType());
            var mapping = new MemberMapping(member, queryType) {
                Identifier = member.IsDefined(typeof(IdentifierAttribute), true),
                IsGenerated = member.IsDefined(typeof(GeneratedAttribute), true),
                IsAggregation = member.IsDefined(typeof(IsAggregationAttribute), true)
            };

            MappingAttribute mappingAttr = member.GetAttributes<MappingAttribute>().LastOrDefault();
            //mapping.Name = member.Name;
            mapping.Name = mapping.Alias = mappingAttr == null ? member.Name : mappingAttr.Name;

            PersistenceAttribute persistenceAttr = member.GetAttributes<PersistenceAttribute>().LastOrDefault();
            mapping.Persistence = persistenceAttr == null ? PersistenceMode.Normal : persistenceAttr.Mode;

            return mapping;
        }

        /// <summary>
        /// 添加一个条目映射信息
        /// </summary>
        /// <typeparam name="T"> 生成条目映射的类型 </typeparam>
        /// <returns> 生成的条目映射信息 </returns>
        public EntryMapping CreateEntryMapping<T>() {
            return CreateEntryMapping(typeof(T));
        }

        /// <summary>
        /// 生成一个条目映射信息
        /// </summary>
        /// <param name="type"> 生成条目映射的类型 </param>
        /// <returns> 生成的条目映射信息 </returns>
        public EntryMapping CreateEntryMapping(Type type) {
            if (type == null || type.Equals(Type.EmptyTypes)) throw new ArgumentNullException("type");
            if (dic.ContainsKey(type)) {
                return dic[type];
            }

            lock (dic) {
                if (dic.ContainsKey(type)) {
                    return dic[type];
                }

                var entry = new EntryMapping(type);

                MemberInfo[] memberInfos = type.GetMembers();
                entry.Members.AddRange(from m in memberInfos
                                       where m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property
                                       select CreateMemberMapping(m));

                if (doc != null) {
                    XmlNode entrySetting = doc.SelectSingleNode(string.Format("/entries/entry[@type='{0}']", entry.Type.FullName));
                    if (entrySetting != null) {
                        CombineEntrySetting(entry, entrySetting);
                    }
                }
                dic.Add(type, entry);
                return entry;
            }
        }

        private static void CombineEntrySetting(EntryMapping entry, XmlNode node) {
            if (entry == null) {
                throw new ArgumentNullException("entry");
            }
            if (node == null) {
                throw new ArgumentNullException("node");
            }
            if (node.Attributes == null || node.Attributes.Count == 0) return;

            XmlAttribute typeAttr = node.Attributes["type"];
            if (typeAttr == null || typeAttr.Value != entry.Type.FullName) {
                throw new InvalidOperationException("配置中的类型与映射中的类型不匹配。");
            }

            XmlAttribute nameAttr = node.Attributes["mapping"];
            entry.Name = nameAttr == null ? entry.Name : nameAttr.Value;

            XmlAttribute aliasAttr = node.Attributes["alias"];
            entry.Alias = aliasAttr == null ? entry.Alias : aliasAttr.Value;
        }

        private static void CombineMemberSetting(MemberMapping member, XmlNode node) {
            if (member == null) throw new ArgumentNullException("member");
            if (node == null) throw new ArgumentNullException("node");
            if (node.Attributes == null || node.Attributes.Count == 0) return;

            XmlAttribute nameAttr = node.Attributes["name"];
            if (nameAttr == null) throw new InvalidOperationException("配置缺少名称");
            if (nameAttr.Value != member.MemberInfo.Name) throw new InvalidOperationException("配置中的名称与映射中的名称不匹配。");

            XmlAttribute mappingAttr = node.Attributes["mapping"];
            member.Name = mappingAttr == null ? member.Name : mappingAttr.Value;

            XmlAttribute aliasAttr = node.Attributes["alias"];
            member.Alias = aliasAttr == null ? member.Alias : aliasAttr.Value;

            XmlAttribute generatedAttr = node.Attributes["generated"];
            bool generated;
            if (bool.TryParse(generatedAttr.Value, out generated)) {
                member.IsGenerated = generated;
            }

            XmlAttribute identifierAttr = node.Attributes["identifier"];
            bool identifier;
            if (bool.TryParse(identifierAttr.Value, out identifier)) {
                member.Identifier = identifier;
            }

            XmlAttribute persistenceAttr = node.Attributes["persistence"];
            PersistenceMode persistence;
            if (Enum.TryParse(persistenceAttr.Value, out persistence)) {
                member.Persistence = persistence;
            }
        }

        /// <summary>
        /// 加载配置文件中的映射配置信息
        /// </summary>
        /// <param name="filename"> 配置文件名称 </param>
        public void LoadSettings(string filename) {
            if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentNullException(filename);
            doc = new XmlDocument();
            doc.Load(filename);
            foreach (var key in dic.Keys) {
                EntryMapping entry = dic[key];
                XmlNode entrySetting = doc.SelectSingleNode(string.Format("/entries/entry[@type='{0}']", entry.Type.FullName));
                if (entrySetting == null) continue;
                CombineEntrySetting(entry, entrySetting);
                foreach (MemberMapping member in entry.Members) {
                    XmlNode memberSetting = entrySetting.SelectSingleNode(string.Format("member[@name='{0}']", member.Name));
                    if (memberSetting != null) CombineMemberSetting(member, memberSetting);
                }
            }
        }

        public bool IsRelationshipSource(EntryMapping entity, MemberInfo member) {
            if (IsAssociationRelationship(entity, member)) {
                if (typeof(IEnumerable).IsAssignableFrom(TypeHelper.GetMemberType(member)))
                    return false;

                // is source of relationship if relatedKeyMembers are the related entity's primary keys
                EntryMapping entity2 = GetRelatedEntity(entity, member);
                var relatedPKs = new HashSet<string>(GetPrimaryKeyMembers(entity2).Select(m => m.Name));
                var relatedKeyMembers = new HashSet<string>(GetAssociationRelatedKeyMembers(entity, member).Select(m => m.Name));
                return relatedPKs.IsSubsetOf(relatedKeyMembers) && relatedKeyMembers.IsSubsetOf(relatedPKs);
            }
            return false;
        }

        public bool IsAssociationRelationship(EntryMapping entity, MemberInfo member) {
            //if (IsMapped(entity, member) && !IsColumn(entity, member)) {
            //    Type otherType = TypeHelper.GetElementType(TypeHelper.GetMemberType(member));
            //    return !this.IsScalar(otherType);
            //}
            return false;
        }

        public bool IsRelationshipTarget(EntryMapping entry, MemberInfo member) {
            if (IsAssociationRelationship(entry, member)) {
                if (typeof(IEnumerable).IsAssignableFrom(TypeHelper.GetMemberType(member)))
                    return true;

                // is target of relationship if the assoctions keys are the same as this entities primary key
                var pks = new HashSet<string>(GetPrimaryKeyMembers(entry).Select(m => m.Name));
                var keys = new HashSet<string>(GetAssociationKeyMembers(entry, member).Select(m => m.Name));
                return keys.IsSubsetOf(pks) && pks.IsSubsetOf(keys);
            }
            return false;
        }

        public IEnumerable<MemberInfo> GetAssociationKeyMembers(EntryMapping entity, MemberInfo member) {
            List<MemberInfo> keyMembers;
            List<MemberInfo> relatedKeyMembers;
            GetAssociationKeys(entity, member, out keyMembers, out relatedKeyMembers);
            return keyMembers;
        }

        public IEnumerable<MemberInfo> GetAssociationRelatedKeyMembers(EntryMapping entity, MemberInfo member) {
            List<MemberInfo> keyMembers;
            List<MemberInfo> relatedKeyMembers;
            GetAssociationKeys(entity, member, out keyMembers, out relatedKeyMembers);
            return relatedKeyMembers;
        }

        private void GetAssociationKeys(EntryMapping entity, MemberInfo member, out List<MemberInfo> keyMembers, out List<MemberInfo> relatedKeyMembers) {
            EntryMapping entity2 = GetRelatedEntity(entity, member);

            // find all members in common (same name)
            //var map1 = this.GetMappedMembers(entity).Where(m => this.IsColumn(entity, m)).ToDictionary(m => m.Name);
            //var map2 = this.GetMappedMembers(entity2).Where(m => this.IsColumn(entity2, m)).ToDictionary(m => m.Name);
            Dictionary<string, MemberInfo> map1 = GetMappedMembers(entity).ToDictionary(m => m.Name);
            Dictionary<string, MemberInfo> map2 = GetMappedMembers(entity2).ToDictionary(m => m.Name);
            IOrderedEnumerable<string> commonNames = map1.Keys.Intersect(map2.Keys).OrderBy(k => k);
            keyMembers = new List<MemberInfo>();
            relatedKeyMembers = new List<MemberInfo>();
            foreach (string name in commonNames) {
                keyMembers.Add(map1[name]);
                relatedKeyMembers.Add(map2[name]);
            }
        }

        public virtual EntryMapping GetRelatedEntity(EntryMapping entry, MemberInfo member) {
            Type relatedType = TypeHelper.GetElementType(member.GetMemberType());
            return GetEntry(relatedType);
        }

        internal bool IsUpdatable(EntryMapping e, MemberInfo m) {
            return !(e.Members[m.Name].Identifier || e.Members[m.Name].IsGenerated);
        }

        internal QueryTypeResolver TypeResolver { get { return typeResolver; } }

        #region Overrides of QueryMapping

        /// <summary>
        /// Get the meta entity directly corresponding to the CLR type
        /// </summary>
        /// <param name="type"> </param>
        /// <returns> </returns>
        public override EntryMapping GetEntry(Type type) {
            return CreateEntryMapping(type);
        }

        /// <summary>
        /// Get the meta entity represented by the IQueryable context member
        /// </summary>
        /// <param name="contextMember"> </param>
        /// <returns> </returns>
        public override EntryMapping GetEntry(MemberInfo contextMember) {
            return CreateEntryMapping(contextMember.GetMemberType());
        }

        public override IEnumerable<MemberInfo> GetMappedMembers(EntryMapping entry) {
            return entry.Members.Select(m => m.MemberInfo);
        }

        public override bool IsPrimaryKey(EntryMapping entry, MemberInfo member) {
            return entry.Members[member.Name].Identifier;
        }

        /// <summary>
        /// Determines if a property is mapped as a relationship
        /// </summary>
        /// <param name="entity"> </param>
        /// <param name="member"> </param>
        /// <returns> </returns>
        public override bool IsRelationship(EntryMapping entity, MemberInfo member) {
            return false;
        }

        public override object GetPrimaryKey(EntryMapping entry, object instance) {
            IEnumerable<object> keys = GetPrimaryKeyMembers(entry).Select(m => instance.Get(m.Name));
            return keys.Count() > 1 ? new CompoundKey(keys) : keys.FirstOrDefault();
        }

        public override Expression GetPrimaryKeyQuery(EntryMapping entry, Expression source, Expression[] keys) {
            // make predicate
            ParameterExpression p = Expression.Parameter(entry.Type, "p");
            Expression pred = null;
            List<MemberInfo> idMembers = GetPrimaryKeyMembers(entry).ToList();
            if (idMembers.Count != keys.Length) {
                throw new InvalidOperationException("Incorrect number of primary key values");
            }
            for (int i = 0, n = keys.Length; i < n; i++) {
                MemberInfo mem = idMembers[i];
                Type memberType = TypeHelper.GetMemberType(mem);
                if (keys[i] != null && TypeHelper.GetNonNullableType(keys[i].Type) != TypeHelper.GetNonNullableType(memberType)) {
                    throw new InvalidOperationException("Primary key value is wrong type");
                }
                Expression eq = Expression.MakeMemberAccess(p, mem).Equal(keys[i]);
                pred = (pred == null) ? eq : pred.And(eq);
            }
            LambdaExpression predLambda = Expression.Lambda(pred, p);

            return Expression.Call(typeof(Queryable), "SingleOrDefault", new[] { entry.Type }, source, predLambda);
        }

        public override IEnumerable<EntryInfo> GetDependentEntities(EntryMapping entry, object instance) {
            foreach (MemberInfo mi in GetMappedMembers(entry)) {
                if (IsRelationship(entry, mi) && IsRelationshipSource(entry, mi)) {
                    EntryMapping relatedEntity = GetRelatedEntity(entry, mi);
                    object value = mi.GetValue(instance);
                    if (value != null) {
                        var list = value as IList;
                        if (list != null) {
                            foreach (object item in list) {
                                if (item != null) {
                                    yield return new EntryInfo(item, relatedEntity);
                                }
                            }
                        }
                        else {
                            yield return new EntryInfo(value, relatedEntity);
                        }
                    }
                }
            }
        }

        public override IEnumerable<EntryInfo> GetDependingEntities(EntryMapping entity, object instance) {
            yield break;
        }

        public override object CloneEntity(EntryMapping entity, object instance) {
            return null;
        }

        public override bool IsModified(EntryMapping entity, object instance, object original) {
            return false;
        }

        public override QueryMapper CreateMapper(QueryTranslator translator) {
            return new DefaultMapper(this, translator);
        }

        #endregion
    }

    /// <summary>
    /// 条目映射信息
    /// </summary>
    public sealed class EntryMapping : MappingInfo {
        private readonly MemberMappingCollection members = new MemberMappingCollection();
        private readonly Type type;

        /// <summary>
        /// 根据指定的类型，初始化 EntryMapping 类型的新实例
        /// </summary>
        /// <param name="type"> </param>
        public EntryMapping(Type type) {
            this.type = type;

            MappingAttribute mappingAttr = type.GetAttributes<MappingAttribute>().LastOrDefault();
            Alias = Name = mappingAttr == null ? type.Name : mappingAttr.Name;
            PersistenceAttribute persistenceAttr = type.GetAttributes<PersistenceAttribute>().LastOrDefault();
            Persistence = persistenceAttr == null ? PersistenceMode.Normal : persistenceAttr.Mode;
        }

        /// <summary>
        /// 获取成员映射信息
        /// </summary>
        public MemberMappingCollection Members {
            get { return members; }
        }

        /// <summary>
        /// 获取生成当前映射信息的原始类型
        /// </summary>
        public Type Type {
            get { return type; }
        }

        public Type ElementType {
            get { return null; }
        }

        ///// <summary>
        /////  根据类型初始化条目映射信息
        ///// </summary>
        ///// <param name="type"> 用于初始化映射信息的类型 </param>
        //private void InitializeByType(Type type) {
        //    if (type == null) throw new ArgumentNullException("type");

        //    MappingAttribute mappingAttr = type.GetAttributes<MappingAttribute>().LastOrDefault();
        //    Alias = Name = mappingAttr == null ? type.Name : mappingAttr.Name;

        //    PersistenceAttribute persistenceAttr = type.GetAttributes<PersistenceAttribute>().LastOrDefault();
        //    Persistence = persistenceAttr == null ? PersistenceMode.Normal : persistenceAttr.Mode;

        //    MemberInfo[] memberInfos = type.GetMembers();
        //    members.AddRange(from m in memberInfos
        //                     where m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property
        //                     select MemberMapping.FromMember(m));
        //}
    }

    /// <summary>
    /// 成员映射信息
    /// </summary>
    public class MemberMapping : MappingInfo {
        private readonly MemberInfo member;
        private readonly QueryType queryType;

        internal MemberMapping(MemberInfo member, QueryType queryType) {
            this.member = member;
            this.queryType = queryType;
        }

        /// <summary>
        /// 获取或设置一个值，用以指示当前成员是否可用于标识一个条目
        /// </summary>
        public bool Identifier { get; set; }

        /// <summary>
        /// 获取或设置一个值，用以指示当前成员是否由仓储方生成
        /// </summary>
        public bool IsGenerated { get; set; }

        /// <summary>
        /// 获取或设置一个值，用以指示当前成员是否是一个聚合属性
        /// </summary>
        public bool IsAggregation { get; set; }

        ///<summary>
        ///    获取生成当前映射信息的原始成员信息
        ///</summary>
        public MemberInfo MemberInfo {
            get { return member; }
        }

        public QueryType QueryType {
            get { return queryType; }
        }

    }

    public class MemberMappingCollection : IEnumerable<MemberMapping> {
        private readonly IDictionary<string, MemberMapping> mappings;

        public MemberMappingCollection() {
            mappings = new Dictionary<string, MemberMapping>();
        }

        public MemberMappingCollection(IDictionary<string, MemberMapping> mappings) {
            this.mappings = mappings;
        }

        public MemberMappingCollection(int capacity) {
            mappings = new Dictionary<string, MemberMapping>(capacity);
        }

        public bool IsReadonly {
            get { return mappings.IsReadOnly; }
        }

        public int Count {
            get { return mappings.Count; }
        }

        public string[] Names {
            get { return mappings.Keys.ToArray(); }
        }

        public MemberMapping this[string name] {
            get { return mappings[name]; }
        }

        #region Implementation of IEnumerable

        public IEnumerator<MemberMapping> GetEnumerator() {
            return mappings.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return mappings.Values.GetEnumerator();
        }

        #endregion

        public void Add(MemberMapping member) {
            if (member == null) throw new ArgumentNullException("member");
            mappings.Add(member.MemberInfo.Name, member);
        }

        public void AddRange(IEnumerable<MemberMapping> members) {
            if (members == null) throw new ArgumentNullException("members");
            members.ForEach(Add);
        }

        public void Remove(string name) {
            mappings.Remove(name);
        }

        public void Remove(MemberMapping member) {
            if (member == null) throw new ArgumentNullException("member");
            mappings.Remove(member.Name);
        }

        public void Clear() {
            mappings.Clear();
        }

        public bool TryGetValue(string name, out MemberMapping value) {
            return mappings.TryGetValue(name, out value);
        }
    }

    /// <summary>
    /// 映射信息
    /// </summary>
    public abstract class MappingInfo {
        /// <summary>
        /// 获取映射对象的名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 获取或设置映射对象的别名
        /// </summary>
        public virtual string Alias { get; set; }

        /// <summary>
        /// 获取或设置存储方式
        /// </summary>
        public virtual PersistenceMode Persistence { get; set; }
    }

    public class DefaultMapper : QueryMapper {
        private readonly DefaultMapping mapping;
        private readonly QueryTranslator translator;

        public DefaultMapper(DefaultMapping mapping, QueryTranslator translator) {
            this.mapping = mapping;
            this.translator = translator;
        }

        public override QueryMapping Mapping {
            get { return mapping; }
        }

        public override QueryTranslator Translator {
            get { return translator; }
        }

        /// <summary>
        /// The query language specific type for the column
        /// </summary>
        /// <param name="member"> </param>
        /// <returns> </returns>
        public virtual QueryType GetColumnType(EntryMapping entry, MemberInfo member) {
            return entry.Members[member.Name].QueryType;
            //if (dbType != null) {
            //    return translator.Linguist.Language.TypeSystem.Parse(dbType);
            //}
            //return translator.Linguist.Language.TypeSystem.GetColumnType(TypeHelper.GetMemberType(member));
        }

        public override ProjectionExpression GetQueryExpression(EntryMapping entry) {
            var tableAlias = new TableAlias();
            var selectAlias = new TableAlias();
            var table = new TableExpression(tableAlias, entry, entry.Name);

            Expression projector = GetEntityExpression(table, entry);
            ProjectedColumns pc = ColumnProjector.ProjectColumns(translator.Linguist.Language, projector, null, selectAlias, tableAlias);

            var proj = new ProjectionExpression(new SelectExpression(selectAlias, pc.Columns, table, null), pc.Projector);

            return (ProjectionExpression)Translator.Police.ApplyPolicy(proj, entry.Type);
        }

        public override EntryExpression GetEntityExpression(Expression root, EntryMapping entry) {
            // must be some complex type constructed from multiple columns
            var assignments = new List<EntityAssignment>();
            foreach (MemberInfo mi in mapping.GetMappedMembers(entry)) {
                if (!mapping.IsAssociationRelationship(entry, mi)) {
                    Expression me = GetMemberExpression(root, entry, mi);
                    if (me != null) {
                        assignments.Add(new EntityAssignment(mi, me));
                    }
                }
            }

            return new EntryExpression(entry, BuildEntityExpression(entry, assignments));
        }

        protected virtual Expression BuildEntityExpression(EntryMapping entry, IList<EntityAssignment> assignments) {
            NewExpression newExpression;

            // handle cases where members are not directly assignable
            EntityAssignment[] readonlyMembers = assignments.Where(b => TypeHelper.IsReadOnly(b.Member)).ToArray();
            ConstructorInfo[] cons = entry.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            bool hasNoArgConstructor = cons.Any(c => c.GetParameters().Length == 0);

            if (readonlyMembers.Length > 0 || !hasNoArgConstructor) {
                // find all the constructors that bind all the read-only members
                List<ConstructorBindResult> consThatApply = cons.Select(c => BindConstructor(c, readonlyMembers)).Where(cbr => cbr != null && cbr.Remaining.Count == 0).ToList();
                if (consThatApply.Count == 0) {
                    throw new InvalidOperationException(string.Format("Cannot construct type '{0}' with all mapped includedMembers.", entry.Type));
                }
                // just use the first one... (Note: need better algorithm. :-)
                if (readonlyMembers.Length == assignments.Count) {
                    return consThatApply[0].Expression;
                }
                ConstructorBindResult r = BindConstructor(consThatApply[0].Expression.Constructor, assignments);

                newExpression = r.Expression;
                assignments = r.Remaining;
            }
            else {
                newExpression = Expression.New(entry.Type);
            }

            Expression result;
            if (assignments.Count > 0) {
                if (entry.Type.IsInterface) {
                    assignments = MapAssignments(assignments, entry.Type).ToList();
                }
                result = Expression.MemberInit(newExpression, assignments.Select(a => Expression.Bind(a.Member, a.Expression)).ToArray());
            }
            else {
                result = newExpression;
            }

            //if (entry.ElementType != entry.EntityType) {
            //    result = Expression.Convert(result, entry.ElementType);
            //}

            return result;
        }

        private IEnumerable<EntityAssignment> MapAssignments(IEnumerable<EntityAssignment> assignments, Type entityType) {
            foreach (EntityAssignment assign in assignments) {
                MemberInfo[] members = entityType.GetMember(assign.Member.Name, BindingFlags.Instance | BindingFlags.Public);
                if (members.Length > 0) {
                    yield return new EntityAssignment(members[0], assign.Expression);
                }
                else {
                    yield return assign;
                }
            }
        }

        protected virtual ConstructorBindResult BindConstructor(ConstructorInfo cons, IList<EntityAssignment> assignments) {
            ParameterInfo[] ps = cons.GetParameters();
            var args = new Expression[ps.Length];
            var mis = new MemberInfo[ps.Length];
            var members = new HashSet<EntityAssignment>(assignments);
            var used = new HashSet<EntityAssignment>();

            for (int i = 0, n = ps.Length; i < n; i++) {
                ParameterInfo p = ps[i];
                EntityAssignment assignment = members.FirstOrDefault(a => p.Name == a.Member.Name && p.ParameterType.IsAssignableFrom(a.Expression.Type));
                if (assignment == null) {
                    assignment = members.FirstOrDefault(a => string.Compare(p.Name, a.Member.Name, true) == 0 && p.ParameterType.IsAssignableFrom(a.Expression.Type));
                }
                if (assignment != null) {
                    args[i] = assignment.Expression;
                    mis[i] = assignment.Member;
                    used.Add(assignment);
                }
                else {
                    MemberInfo[] mems = cons.DeclaringType.GetMember(p.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                    if (mems.Length > 0) {
                        args[i] = Expression.Constant(TypeHelper.GetDefault(p.ParameterType), p.ParameterType);
                        mis[i] = mems[0];
                    }
                    else {
                        // unknown parameter, does not match any member
                        return null;
                    }
                }
            }

            members.ExceptWith(used);

            return new ConstructorBindResult(Expression.New(cons, args, mis), members);
        }

        public override bool HasIncludedMembers(EntryExpression entry) {
            QueryPolicy policy = translator.Police.Policy;
            foreach (MemberInfo mi in mapping.GetMappedMembers(entry.Entry)) {
                if (policy.IsIncluded(mi))
                    return true;
            }
            return false;
        }

        public override EntryExpression IncludeMembers(EntryExpression entry, Func<MemberInfo, bool> fnIsIncluded) {
            Dictionary<string, EntityAssignment> assignments = GetAssignments(entry.Expression).ToDictionary(ma => ma.Member.Name);
            bool anyAdded = false;
            foreach (MemberInfo mi in mapping.GetMappedMembers(entry.Entry)) {
                EntityAssignment ea;
                bool okayToInclude = !assignments.TryGetValue(mi.Name, out ea) || IsNullRelationshipAssignment(entry.Entry, ea);
                if (okayToInclude && fnIsIncluded(mi)) {
                    ea = new EntityAssignment(mi, GetMemberExpression(entry.Expression, entry.Entry, mi));
                    assignments[mi.Name] = ea;
                    anyAdded = true;
                }
            }
            if (anyAdded) {
                return new EntryExpression(entry.Entry, BuildEntityExpression(entry.Entry, assignments.Values.ToList()));
            }
            return entry;
        }

        private bool IsNullRelationshipAssignment(EntryMapping entry, EntityAssignment assignment) {
            if (mapping.IsRelationship(entry, assignment.Member)) {
                var cex = assignment.Expression as ConstantExpression;
                if (cex != null && cex.Value == null)
                    return true;
            }
            return false;
        }

        private IEnumerable<EntityAssignment> GetAssignments(Expression newOrMemberInit) {
            var assignments = new List<EntityAssignment>();
            var minit = newOrMemberInit as MemberInitExpression;
            if (minit != null) {
                assignments.AddRange(minit.Bindings.OfType<MemberAssignment>().Select(a => new EntityAssignment(a.Member, a.Expression)));
                newOrMemberInit = minit.NewExpression;
            }
            var nex = newOrMemberInit as NewExpression;
            if (nex != null) {
                assignments.AddRange(Enumerable.Range(0, nex.Arguments.Count).Where(i => nex.Members[i] != null).Select(i => new EntityAssignment(nex.Members[i], nex.Arguments[i])));
            }
            return assignments;
        }

        public override Expression GetMemberExpression(Expression root, EntryMapping entry, MemberInfo member) {
            //if (mapping.IsAssociationRelationship(entry, member)) {
            if (entry.Members[member.Name].IsAggregation) {
                EntryMapping relatedEntity = mapping.GetRelatedEntity(entry, member);
                ProjectionExpression projection = GetQueryExpression(relatedEntity);

                // make where clause for joining back to 'root'
                List<MemberInfo> declaredTypeMembers = mapping.GetAssociationKeyMembers(entry, member).ToList();
                List<MemberInfo> associatedMembers = mapping.GetAssociationRelatedKeyMembers(entry, member).ToList();

                Expression where = null;
                for (int i = 0, n = associatedMembers.Count; i < n; i++) {
                    Expression equal = GetMemberExpression(projection.Projector, relatedEntity, associatedMembers[i]).Equal(GetMemberExpression(root, entry, declaredTypeMembers[i]));
                    where = (where != null) ? where.And(equal) : equal;
                }

                var newAlias = new TableAlias();
                ProjectedColumns pc = ColumnProjector.ProjectColumns(translator.Linguist.Language, projection.Projector, null, newAlias, projection.Select.Alias);

                LambdaExpression aggregator = Aggregator.GetAggregator(TypeHelper.GetMemberType(member), typeof(IEnumerable<>).MakeGenericType(pc.Projector.Type));
                var result = new ProjectionExpression(new SelectExpression(newAlias, pc.Columns, projection.Select, where), pc.Projector, aggregator);

                return translator.Police.ApplyPolicy(result, member);
            }
            else {
                var aliasedRoot = root as AliasedExpression;
                //if (aliasedRoot != null && mapping.IsColumn(entry, member)) {
                if (aliasedRoot != null) {
                    QueryType ctype = GetColumnType(entry, member);
                    if (ctype == null)
                        return null;
                    return new ColumnExpression(TypeHelper.GetMemberType(member), ctype, aliasedRoot.Alias, entry.Members[member.Name].Name);
                }
                return QueryBinder.BindMember(root, member);
            }
        }

        public override Expression GetInsertExpression(EntryMapping entry, Expression instance, LambdaExpression selector) {
            var tableAlias = new TableAlias();
            var table = new TableExpression(tableAlias, entry, entry.Name);
            IEnumerable<ColumnAssignment> assignments = GetColumnAssignments(table, instance, entry, (m) => !m.IsGenerated);

            if (selector != null) {
                return new BlockCommand(new InsertCommand(table, assignments), GetInsertResult(entry, instance, selector, null));
            }

            return new InsertCommand(table, assignments);
        }
        private object GetPersistValue(object value) {
            if (value == null) return null;
            var type = value.GetType();
            if (type.IsSimpleType()) {
                return value;
            }
            if (type.IsNullableType()) {
                return GetPersistValue(value.Get("Value"));
            }
            try {
                var key = type.GetMembers().SingleOrDefault(m => (m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field) && m.HasAttribute<IdentifierAttribute>());
                if (key == null)
                    throw new InvalidOperationException("无法确定引用的键值。");

                return value.Get(key.Name);
            }
            catch (InvalidOperationException ioe) {
                throw new InvalidOperationException(string.Format("无法确定引用类型：\"{0}\" 的键值。", type.FullName), ioe);
            }
        }

        private IEnumerable<ColumnAssignment> GetColumnAssignments(Expression table, Expression instance, EntryMapping entry, Func<MemberMapping, bool> fnIncludeColumn) {
            if (table == null) throw new ArgumentNullException("table");
            if (table as TableExpression == null) throw new InvalidOperationException("table is not a valid expression.");
            if (instance == null) throw new ArgumentNullException("instance");
            if (entry == null) throw new ArgumentNullException("entry");

            foreach (var m in entry.Members) {
                if (fnIncludeColumn == null || !fnIncludeColumn(m)) continue;

                var mval = instance.NodeType == ExpressionType.Constant ? ((ConstantExpression)instance).Value : null;
                mval = mval == null ? null : mval.Get(m.MemberInfo.Name);
                var mtype = mval == null ? null : mval.GetType();
                if (mtype != null && mtype.IsSimpleType()) {
                    var persistValue = GetPersistValue(mval);
                    if (persistValue == null) continue;
                    yield return new ColumnAssignment(new ColumnExpression(m.MemberInfo.GetMemberType(), mapping.TypeResolver.GetColumnType(persistValue.GetType()), (table as TableExpression).Alias, m.Name), Expression.Constant(persistValue, persistValue.GetType()));
                }
                else {
                    if (instance.NodeType != ExpressionType.Constant)
                        yield return new ColumnAssignment((ColumnExpression)GetMemberExpression(table, entry, m.MemberInfo), Expression.MakeMemberAccess(instance, m.MemberInfo));
                }
                #region refactored
                // TODO:修改，将非简单类型的属性作为聚合
                //if (fnIncludeColumn != null && fnIncludeColumn(m)) {
                //    if (instance.NodeType == ExpressionType.Constant) {
                //        var memberValue = ((ConstantExpression)instance).Value.Get(m.MemberInfo.Name);
                //        if (memberValue == null) continue;

                //        var type = m.MemberInfo.GetMemberType();
                //        if (!type.IsSimpleType()) {
                //            var persistValue = GetPersistValue(memberValue);
                //            if (persistValue == null) continue;

                //            yield return new ColumnAssignment(
                //                new ColumnExpression(
                //                    m.MemberInfo.GetMemberType(),
                //                    mapping.TypeResolver.GetColumnType(persistValue.GetType()),
                //                    (table as TableExpression).Alias, m.Name),
                //                Expression.Constant(persistValue, persistValue.GetType()));
                //        }
                //        else {
                //            yield return new ColumnAssignment((ColumnExpression)GetMemberExpression(table, entry, m.MemberInfo), Expression.MakeMemberAccess(instance, m.MemberInfo));
                //        }
                //    }
                //    else {
                //        yield return new ColumnAssignment((ColumnExpression)GetMemberExpression(table, entry, m.MemberInfo), Expression.MakeMemberAccess(instance, m.MemberInfo));
                //    }
                //}
                #endregion
            }
        }

        protected virtual Expression GetInsertResult(EntryMapping entry, Expression instance, LambdaExpression selector, Dictionary<MemberInfo, Expression> map) {
            var tableAlias = new TableAlias();
            var tex = new TableExpression(tableAlias, entry, entry.Name);
            LambdaExpression aggregator = Aggregator.GetAggregator(selector.Body.Type, typeof(IEnumerable<>).MakeGenericType(selector.Body.Type));

            Expression where;
            DeclarationCommand genIdCommand = null;
            List<MemberInfo> generatedIds = mapping.GetMappedMembers(entry).Where(m => mapping.IsPrimaryKey(entry, m) && entry.Members[m.Name].IsGenerated).ToList();
            if (generatedIds.Count > 0) {
                if (map == null || !generatedIds.Any(m => map.ContainsKey(m))) {
                    var localMap = new Dictionary<MemberInfo, Expression>();
                    genIdCommand = GetGeneratedIdCommand(entry, generatedIds.ToList(), localMap);
                    map = localMap;
                }

                // is this just a retrieval of one generated id member?
                var mex = selector.Body as MemberExpression;
                if (mex != null && mapping.IsPrimaryKey(entry, mex.Member) && entry.Members[mex.Member.Name].IsGenerated) {
                    if (genIdCommand != null) {
                        // just use the select from the genIdCommand
                        return new ProjectionExpression(genIdCommand.Source,
                                                        new ColumnExpression(mex.Type, genIdCommand.Variables[0].QueryType, genIdCommand.Source.Alias, genIdCommand.Source.Columns[0].Name), aggregator);
                    }
                    var alias = new TableAlias();
                    QueryType colType = GetColumnType(entry, mex.Member);
                    return new ProjectionExpression(new SelectExpression(alias, new[] { new ColumnDeclaration("", map[mex.Member], colType) }, null, null),
                                                    new ColumnExpression(TypeHelper.GetMemberType(mex.Member), colType, alias, ""), aggregator);
                }

                where = generatedIds.Select((m, i) => GetMemberExpression(tex, entry, m).Equal(map[m])).Aggregate((x, y) => x.And(y));
            }
            else {
                where = GetIdentityCheck(tex, entry, instance);
            }

            Expression typeProjector = GetEntityExpression(tex, entry);
            Expression selection = DbExpressionReplacer.Replace(selector.Body, selector.Parameters[0], typeProjector);
            var newAlias = new TableAlias();
            ProjectedColumns pc = ColumnProjector.ProjectColumns(translator.Linguist.Language, selection, null, newAlias, tableAlias);
            var pe = new ProjectionExpression(new SelectExpression(newAlias, pc.Columns, tex, where), pc.Projector, aggregator);

            if (genIdCommand != null) {
                return new BlockCommand(genIdCommand, pe);
            }
            return pe;
        }

        protected virtual DeclarationCommand GetGeneratedIdCommand(EntryMapping entry, List<MemberInfo> members, Dictionary<MemberInfo, Expression> map) {
            var columns = new List<ColumnDeclaration>();
            var decls = new List<VariableDeclaration>();
            var alias = new TableAlias();
            foreach (MemberInfo member in members) {
                Expression genId = translator.Linguist.Language.GetGeneratedIdExpression(member);
                string name = member.Name;
                QueryType colType = GetColumnType(entry, member);
                columns.Add(new ColumnDeclaration(member.Name, genId, colType));
                decls.Add(new VariableDeclaration(member.Name, colType, new ColumnExpression(genId.Type, colType, alias, member.Name)));
                if (map != null) {
                    var vex = new VariableExpression(member.Name, TypeHelper.GetMemberType(member), colType);
                    map.Add(member, vex);
                }
            }
            var select = new SelectExpression(alias, columns, null, null);
            return new DeclarationCommand(decls, select);
        }

        protected virtual Expression GetIdentityCheck(Expression root, EntryMapping entry, Expression instance) {
            return mapping.GetMappedMembers(entry)
                .Where(m => mapping.IsPrimaryKey(entry, m))
                .Select(m => GetMemberExpression(root, entry, m).Equal(Expression.MakeMemberAccess(instance, m)))
                .Aggregate((x, y) => x.And(y));
        }

        protected virtual Expression GetEntityExistsTest(EntryMapping entry, Expression instance) {
            ProjectionExpression tq = GetQueryExpression(entry);
            Expression where = GetIdentityCheck(tq.Select, entry, instance);
            return new ExistsExpression(new SelectExpression(new TableAlias(), null, tq.Select, where));
        }

        protected virtual Expression GetEntityStateTest(EntryMapping entry, Expression instance, LambdaExpression updateCheck) {
            ProjectionExpression tq = GetQueryExpression(entry);
            Expression where = GetIdentityCheck(tq.Select, entry, instance);
            Expression check = DbExpressionReplacer.Replace(updateCheck.Body, updateCheck.Parameters[0], tq.Projector);
            where = where.And(check);
            return new ExistsExpression(new SelectExpression(new TableAlias(), null, tq.Select, where));
        }

        public override Expression GetUpdateExpression(EntryMapping entry, Expression instance, Expression updateCheck, LambdaExpression selector, Expression @else) {
            var tableAlias = new TableAlias();
            var table = new TableExpression(tableAlias, entry, entry.Name);

            var check = GetWhere(updateCheck);
            var where = DbExpressionReplacer.Replace(check.Body, check.Parameters[0], GetEntityExpression(table, entry));
            var assignments = GetAssignments(entry, tableAlias, instance);
            //Expression where = GetIdentityCheck(table, entry, instance);
            //if (updateCheck != null) {
            //    Expression typeProjector = GetEntityExpression(table, entry);
            //    Expression pred = DbExpressionReplacer.Replace(updateCheck.Body, updateCheck.Parameters[0], typeProjector);
            //    where = where.And(pred);
            //}

            //IEnumerable<ColumnAssignment> assignments = GetColumnAssignments(table, instance, entry, (m) => (m.IsGenerated || m.Identifier));

            Expression update = new UpdateCommand(table, where, assignments);

            if (selector != null) {
                return new BlockCommand(update,
                                        new IFCommand(translator.Linguist.Language.GetRowsAffectedExpression(update).GreaterThan(Expression.Constant(0)), GetUpdateResult(entry, instance, selector),
                                                      @else));
            }
            return @else != null ? new BlockCommand(update, new IFCommand(translator.Linguist.Language.GetRowsAffectedExpression(update).LessThanOrEqual(Expression.Constant(0)), @else, null)) : update;
        }
        protected virtual Expression GetUpdateResult(EntryMapping entry, Expression instance, LambdaExpression selector) {
            ProjectionExpression tq = GetQueryExpression(entry);
            Expression where = GetIdentityCheck(tq.Select, entry, instance);
            Expression selection = DbExpressionReplacer.Replace(selector.Body, selector.Parameters[0], tq.Projector);
            var newAlias = new TableAlias();
            ProjectedColumns pc = ColumnProjector.ProjectColumns(translator.Linguist.Language, selection, null, newAlias, tq.Select.Alias);
            return new ProjectionExpression(new SelectExpression(newAlias, pc.Columns, tq.Select, where), pc.Projector,
                                            Aggregator.GetAggregator(selector.Body.Type, typeof(IEnumerable<>).MakeGenericType(selector.Body.Type)));
        }

        public override Expression GetInsertOrUpdateExpression(EntryMapping entry, Expression instance, LambdaExpression updateCheck, LambdaExpression resultSelector) {
            if (updateCheck != null) {
                var insert = GetInsertExpression(entry, instance, resultSelector);
                var update = GetUpdateExpression(entry, instance, updateCheck, resultSelector, null);
                var check = GetEntityExistsTest(entry, instance);
                return new IFCommand(check, update, insert);
            }
            else {
                var insert = GetInsertExpression(entry, instance, resultSelector);
                var update = GetUpdateExpression(entry, instance, null, resultSelector, insert);
                return update;
            }
        }

        public override Expression GetDeleteExpression(EntryMapping entry, Expression instance, LambdaExpression deleteCheck) {
            var table = new TableExpression(new TableAlias(), entry, entry.Name);
            Expression where = null;

            if (instance != null) {
                where = GetIdentityCheck(table, entry, instance);
            }

            if (deleteCheck != null) {
                Expression row = GetEntityExpression(table, entry);
                Expression pred = DbExpressionReplacer.Replace(deleteCheck.Body, deleteCheck.Parameters[0], row);
                where = (where != null) ? where.And(pred) : pred;
            }

            return new DeleteCommand(table, where);
        }

        private IEnumerable<ColumnAssignment> GetAssignments(EntryMapping entry, TableAlias tableAlias, Expression action) {
            switch (action.NodeType) {
                case ExpressionType.Constant:
                    return GetAssignmentsByInstance(entry, tableAlias, action as ConstantExpression);
                case ExpressionType.Lambda:
                    return GetAssignmentsByLambda(entry, tableAlias, action as LambdaExpression);
                case ExpressionType.Quote:
                    return GetAssignments(entry, tableAlias, StripQuotes(action));
            }
            throw new InvalidOperationException("");
        }
        private IEnumerable<ColumnAssignment> GetAssignmentsByInstance(EntryMapping entry, TableAlias tableAlias, ConstantExpression instance) {
            return (from member in entry.Members.Where(m => !m.IsAggregation && !m.IsGenerated && (!m.MemberInfo.HasAttribute<PersistenceAttribute>() || m.MemberInfo.GetAttributes<PersistenceAttribute>().All(p => p.Mode != PersistenceMode.None)))
                    let column = new ColumnExpression(member.MemberInfo.GetMemberType(), member.QueryType, tableAlias, member.Name)
                    let value = GetPersistValue(instance.Value.Get(member.MemberInfo.Name))
                    select new ColumnAssignment(column, Expression.Constant(value))).ToList();
        }
        private IEnumerable<ColumnAssignment> GetAssignmentsByLambda(EntryMapping entry, TableAlias tableAlias, LambdaExpression lambda) {
            var instance = lambda.Body as ConstantExpression;
            var members = instance.Value.GetType().GetMembers().Where(m =>
                (m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                && (!m.HasAttribute<PersistenceAttribute>() || m.GetAttributes<PersistenceAttribute>().All(p => p.Mode != PersistenceMode.None)));
            foreach (var member in members) {
                MemberMapping mapping;
                if (entry.Members.TryGetValue(member.Name, out mapping)) {
                    var column = new ColumnExpression(member.GetMemberType(), entry.Members[member.Name].QueryType, tableAlias, mapping.Name);
                    var value = instance.Value.Get(member.Name);
                    yield return new ColumnAssignment(column, Expression.Constant(value));
                }
                else {
                    throw new ArgumentNullException(string.Format("表 \"{0}\" 中不包含字段 \"{1}\"。", entry.Name, member.Name));
                }
            }
        }

        private LambdaExpression GetWhere(Expression where) {
            where = StripQuotes(where);
            if (where == null)
                return null;

            if (where is LambdaExpression)
                return (LambdaExpression)where;

            if (where is ConstantExpression) {
                Type type = where.Type;
                object value = (where as ConstantExpression).Value;
                ParameterExpression arg = Expression.Parameter(type, "arg");

                IEnumerable<MemberMapping> keys = mapping.GetEntry(type).Members.Where(m => m.Identifier);
                if (!keys.Any()) {
                    keys = mapping.GetEntry(type).Members;
                }

                // arg.prop[key1]==c.prop[val1] && c.prop[key2]==c.prop[val2]
                List<BinaryExpression> equalList = (from key in keys
                                                    let prop = Expression.MakeMemberAccess(arg, key.MemberInfo)
                                                    let val = Expression.Constant(value.Get(key.MemberInfo.Name))
                                                    select Expression.MakeBinary(ExpressionType.Equal, prop, val)).ToList();
                BinaryExpression equals = equalList[0];
                for (int i = 1; i < equalList.Count; i++) {
                    equals = Expression.MakeBinary(ExpressionType.AndAlso, equals, equalList[i]);
                }

                // 
                return Expression.Lambda(equals, arg);
            }
            throw new InvalidOperationException("无法处理的 where 条件表达式类型。");
        }
        private static Expression StripQuotes(Expression e) {
            while (e != null && e.NodeType == ExpressionType.Quote) {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }
        #region Nested type: ConstructorBindResult

        protected class ConstructorBindResult {
            public ConstructorBindResult(NewExpression expression, IEnumerable<EntityAssignment> remaining) {
                Expression = expression;
                Remaining = remaining.ToReadOnly();
            }

            public NewExpression Expression { get; private set; }
            public ReadOnlyCollection<EntityAssignment> Remaining { get; private set; }
        }

        #endregion

        #region Nested type: EntityAssignment

        public class EntityAssignment {
            public EntityAssignment(MemberInfo member, Expression expression) {
                Member = member;
                //Debug.Assert(expression != null);
                Expression = expression;
            }

            public MemberInfo Member { get; private set; }
            public Expression Expression { get; private set; }
        }

        #endregion
    }
}