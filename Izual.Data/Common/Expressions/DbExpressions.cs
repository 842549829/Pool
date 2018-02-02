#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.DbExpressions.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Izual.Data.Common {
    /// <summary>
    /// Extended node types for custom expressions
    /// </summary>
    public enum DbExpressionType {
        Table = 1000, // make sure these don't overlap with ExpressionType
        ClientJoin=1001,
        Column = 1002,
        Select = 1003,
        Projection = 1004,
        Entity = 1005,
        Join = 1006,
        Aggregate = 1007,
        Scalar = 1008,
        Exists = 1009,
        In = 1010,
        Grouping = 1011,
        AggregateSubquery = 1012,
        IsNull = 1013,
        Between = 1014,
        RowCount = 1015,
        NamedValue = 1016,
        OuterJoined = 1017,
        Insert = 1018,
        Update = 1019,
        Delete = 1020,
        Batch = 1021,
        Function = 1022,
        Block = 1023,
        If = 1024,
        Declaration = 1025,
        Variable = 1026
    }

    public static class DbExpressionTypeExtensions {
        public static bool IsDbExpression(this ExpressionType et) {
            return ((int)et) >= 1000;
        }
    }

    public abstract class DbExpression : Expression {
        protected DbExpression(DbExpressionType eType, Type type) : base((ExpressionType)eType, type) {}

        public override string ToString() {
            return DbExpressionWriter.WriteToString(this);
        }
    }

    public abstract class AliasedExpression : DbExpression {
        private readonly TableAlias alias;

        protected AliasedExpression(DbExpressionType nodeType, Type type, TableAlias alias) : base(nodeType, type) {
            this.alias = alias;
        }

        public TableAlias Alias {
            get { return alias; }
        }
    }

    /// <summary>
    /// A custom expression node that represents a table reference in a SQL query
    /// </summary>
    public class TableExpression : AliasedExpression {
        private readonly EntryMapping entry;
        private readonly string name;

        public TableExpression(TableAlias alias, EntryMapping entry, string name) : base(DbExpressionType.Table, typeof(void), alias) {
            this.entry = entry;
            this.name = name;
        }

        public EntryMapping Entity {
            get { return entry; }
        }

        public string Name {
            get { return name; }
        }

        public override string ToString() {
            return "T(" + Name + ")";
        }
    }

    public class EntryExpression : DbExpression {
        private readonly EntryMapping entry;
        private readonly Expression expression;

        public EntryExpression(EntryMapping entry, Expression expression) : base(DbExpressionType.Entity, expression.Type) {
            this.entry = entry;
            this.expression = expression;
        }

        public EntryMapping Entry {
            get { return entry; }
        }

        public Expression Expression {
            get { return expression; }
        }
    }

    /// <summary>
    /// A custom expression node that represents a reference to a column in a SQL query
    /// </summary>
    public class ColumnExpression : DbExpression, IEquatable<ColumnExpression> {
        private readonly TableAlias alias;
        private readonly string name;
        private readonly QueryType queryType;

        public ColumnExpression(Type type, QueryType queryType, TableAlias alias, string name) : base(DbExpressionType.Column, type) {
            if(queryType == null)
                throw new ArgumentNullException("queryType");
            if(name == null)
                throw new ArgumentNullException("name");
            this.alias = alias;
            this.name = name;
            this.queryType = queryType;
        }

        public TableAlias Alias {
            get { return alias; }
        }

        public string Name {
            get { return name; }
        }

        public QueryType QueryType {
            get { return queryType; }
        }

        #region IEquatable<ColumnExpression> Members

        public bool Equals(ColumnExpression other) {
            return other != null && (this) == other || (alias == other.alias && name == other.Name);
        }

        #endregion

        public override string ToString() {
            return Alias + ".C(" + name + ")";
        }

        public override int GetHashCode() {
            return alias.GetHashCode() + name.GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as ColumnExpression);
        }
    }

    public class TableAlias {
        public override string ToString() {
            return "A:" + GetHashCode();
        }
    }

    /// <summary>
    /// A declaration of a column in a SQL SELECT expression
    /// </summary>
    public class ColumnDeclaration {
        private readonly Expression expression;
        private readonly string name;
        private readonly QueryType queryType;

        public ColumnDeclaration(string name, Expression expression, QueryType queryType) {
            if(name == null)
                throw new ArgumentNullException("name");
            if(expression == null)
                throw new ArgumentNullException("expression");
            if(queryType == null)
                throw new ArgumentNullException("queryType");
            this.name = name;
            this.expression = expression;
            this.queryType = queryType;
        }

        public string Name {
            get { return name; }
        }

        public Expression Expression {
            get { return expression; }
        }

        public QueryType QueryType {
            get { return queryType; }
        }
    }

    /// <summary>
    /// An SQL OrderBy order type
    /// </summary>
    public enum OrderType {
        Ascending,
        Descending
    }

    /// <summary>
    /// A pairing of an expression and an order type for use in a SQL Order By clause
    /// </summary>
    public class OrderExpression {
        private readonly Expression expression;
        private readonly OrderType orderType;

        public OrderExpression(OrderType orderType, Expression expression) {
            this.orderType = orderType;
            this.expression = expression;
        }

        public OrderType OrderType {
            get { return orderType; }
        }

        public Expression Expression {
            get { return expression; }
        }
    }

    /// <summary>
    /// A custom expression node used to represent a SQL SELECT expression
    /// </summary>
    public class SelectExpression : AliasedExpression {
        private readonly ReadOnlyCollection<ColumnDeclaration> columns;
        private readonly Expression from;
        private readonly ReadOnlyCollection<Expression> groupBy;
        private readonly bool isDistinct;
        private readonly ReadOnlyCollection<OrderExpression> orderBy;
        private readonly bool reverse;
        private readonly Expression skip;
        private readonly Expression take;
        private readonly Expression where;

        public SelectExpression(TableAlias alias, IEnumerable<ColumnDeclaration> columns, Expression from, Expression where, IEnumerable<OrderExpression> orderBy, IEnumerable<Expression> groupBy,
                                bool isDistinct, Expression skip, Expression take, bool reverse) : base(DbExpressionType.Select, typeof(void), alias) {
            this.columns = columns.ToReadOnly();
            this.isDistinct = isDistinct;
            this.from = from;
            this.where = where;
            this.orderBy = orderBy.ToReadOnly();
            this.groupBy = groupBy.ToReadOnly();
            this.take = take;
            this.skip = skip;
            this.reverse = reverse;
        }

        public SelectExpression(TableAlias alias, IEnumerable<ColumnDeclaration> columns, Expression from, Expression where, IEnumerable<OrderExpression> orderBy, IEnumerable<Expression> groupBy)
            : this(alias, columns, from, where, orderBy, groupBy, false, null, null, false) {}

        public SelectExpression(TableAlias alias, IEnumerable<ColumnDeclaration> columns, Expression from, Expression where) : this(alias, columns, from, where, null, null) {}

        public ReadOnlyCollection<ColumnDeclaration> Columns {
            get { return columns; }
        }

        public Expression From {
            get { return @from; }
        }

        public Expression Where {
            get { return @where; }
        }

        public ReadOnlyCollection<OrderExpression> OrderBy {
            get { return orderBy; }
        }

        public ReadOnlyCollection<Expression> GroupBy {
            get { return groupBy; }
        }

        public bool IsDistinct {
            get { return isDistinct; }
        }

        public Expression Skip {
            get { return skip; }
        }

        public Expression Take {
            get { return take; }
        }

        public bool IsReverse {
            get { return reverse; }
        }

        public string QueryText {
            get { return SqlFormatter.Format(this, true); }
        }
    }

    /// <summary>
    /// A kind of SQL join
    /// </summary>
    public enum JoinType {
        CrossJoin,
        InnerJoin,
        CrossApply,
        OuterApply,
        LeftOuter,
        SingletonLeftOuter
    }

    /// <summary>
    /// A custom expression node representing a SQL join clause
    /// </summary>
    public class JoinExpression : DbExpression {
        private readonly Expression condition;
        private readonly JoinType joinType;
        private readonly Expression left;
        private readonly Expression right;

        public JoinExpression(JoinType joinType, Expression left, Expression right, Expression condition) : base(DbExpressionType.Join, typeof(void)) {
            this.joinType = joinType;
            this.left = left;
            this.right = right;
            this.condition = condition;
        }

        public JoinType Join {
            get { return joinType; }
        }

        public Expression Left {
            get { return left; }
        }

        public Expression Right {
            get { return right; }
        }

        public new Expression Condition {
            get { return condition; }
        }
    }

    public class OuterJoinedExpression : DbExpression {
        private readonly Expression expression;
        private readonly Expression test;

        public OuterJoinedExpression(Expression test, Expression expression) : base(DbExpressionType.OuterJoined, expression.Type) {
            this.test = test;
            this.expression = expression;
        }

        public Expression Test {
            get { return test; }
        }

        public Expression Expression {
            get { return expression; }
        }
    }

    public abstract class SubqueryExpression : DbExpression {
        private readonly SelectExpression select;

        protected SubqueryExpression(DbExpressionType eType, Type type, SelectExpression select) : base(eType, type) {
            //Debug.Assert(eType == DbExpressionType.Scalar || eType == DbExpressionType.Exists || eType == DbExpressionType.In);
            this.select = select;
        }

        public SelectExpression Select {
            get { return @select; }
        }
    }

    public class ScalarExpression : SubqueryExpression {
        public ScalarExpression(Type type, SelectExpression select) : base(DbExpressionType.Scalar, type, select) {}
    }

    public class ExistsExpression : SubqueryExpression {
        public ExistsExpression(SelectExpression select) : base(DbExpressionType.Exists, typeof(bool), select) {}
    }

    public class InExpression : SubqueryExpression {
        private readonly Expression expression;
        private readonly ReadOnlyCollection<Expression> values; // either select or expressions are assigned

        public InExpression(Expression expression, SelectExpression select) : base(DbExpressionType.In, typeof(bool), select) {
            this.expression = expression;
        }

        public InExpression(Expression expression, IEnumerable<Expression> values) : base(DbExpressionType.In, typeof(bool), null) {
            this.expression = expression;
            this.values = values.ToReadOnly();
        }

        public Expression Expression {
            get { return expression; }
        }

        public ReadOnlyCollection<Expression> Values {
            get { return values; }
        }
    }

    public class AggregateExpression : DbExpression {
        private readonly string aggregateName;
        private readonly Expression argument;
        private readonly bool isDistinct;

        public AggregateExpression(Type type, string aggregateName, Expression argument, bool isDistinct) : base(DbExpressionType.Aggregate, type) {
            this.aggregateName = aggregateName;
            this.argument = argument;
            this.isDistinct = isDistinct;
        }

        public string AggregateName {
            get { return aggregateName; }
        }

        public Expression Argument {
            get { return argument; }
        }

        public bool IsDistinct {
            get { return isDistinct; }
        }
    }

    public class AggregateSubqueryExpression : DbExpression {
        private readonly ScalarExpression aggregateAsSubquery;
        private readonly Expression aggregateInGroupSelect;
        private readonly TableAlias groupByAlias;

        public AggregateSubqueryExpression(TableAlias groupByAlias, Expression aggregateInGroupSelect, ScalarExpression aggregateAsSubquery)
            : base(DbExpressionType.AggregateSubquery, aggregateAsSubquery.Type) {
            this.aggregateInGroupSelect = aggregateInGroupSelect;
            this.groupByAlias = groupByAlias;
            this.aggregateAsSubquery = aggregateAsSubquery;
        }

        public TableAlias GroupByAlias {
            get { return groupByAlias; }
        }

        public Expression AggregateInGroupSelect {
            get { return aggregateInGroupSelect; }
        }

        public ScalarExpression AggregateAsSubquery {
            get { return aggregateAsSubquery; }
        }
    }

    /// <summary>
    /// Allows is-null tests against value-types like int and float
    /// </summary>
    public class IsNullExpression : DbExpression {
        private readonly Expression expression;

        public IsNullExpression(Expression expression) : base(DbExpressionType.IsNull, typeof(bool)) {
            this.expression = expression;
        }

        public Expression Expression {
            get { return expression; }
        }
    }

    public class BetweenExpression : DbExpression {
        private readonly Expression expression;
        private readonly Expression lower;
        private readonly Expression upper;

        public BetweenExpression(Expression expression, Expression lower, Expression upper) : base(DbExpressionType.Between, expression.Type) {
            this.expression = expression;
            this.lower = lower;
            this.upper = upper;
        }

        public Expression Expression {
            get { return expression; }
        }

        public Expression Lower {
            get { return lower; }
        }

        public Expression Upper {
            get { return upper; }
        }
    }

    public class RowNumberExpression : DbExpression {
        private readonly ReadOnlyCollection<OrderExpression> orderBy;

        public RowNumberExpression(IEnumerable<OrderExpression> orderBy) : base(DbExpressionType.RowCount, typeof(int)) {
            this.orderBy = orderBy.ToReadOnly();
        }

        public ReadOnlyCollection<OrderExpression> OrderBy {
            get { return orderBy; }
        }
    }

    public class NamedValueExpression : DbExpression {
        private readonly string name;
        private readonly QueryType queryType;
        private readonly Expression value;

        public NamedValueExpression(string name, QueryType queryType, Expression value) : base(DbExpressionType.NamedValue, value.Type) {
            if(name == null)
                throw new ArgumentNullException("name");
            //if (queryType == null)
            //throw new ArgumentNullException("queryType");
            if(value == null)
                throw new ArgumentNullException("value");
            this.name = name;
            this.queryType = queryType;
            this.value = value;
        }

        public string Name {
            get { return name; }
        }

        public QueryType QueryType {
            get { return queryType; }
        }

        public Expression Value {
            get { return value; }
        }
    }

    /// <summary>
    /// A custom expression representing the construction of one or more result objects from a SQL select expression
    /// </summary>
    public class ProjectionExpression : DbExpression {
        private readonly LambdaExpression aggregator;
        private readonly Expression projector;
        private readonly SelectExpression select;
        public ProjectionExpression(SelectExpression source, Expression projector) : this(source, projector, null) {}

        public ProjectionExpression(SelectExpression source, Expression projector, LambdaExpression aggregator)
            : base(DbExpressionType.Projection, aggregator != null ? aggregator.Body.Type : typeof(IEnumerable<>).MakeGenericType(projector.Type)) {
            @select = source;
            this.projector = projector;
            this.aggregator = aggregator;
        }

        public SelectExpression Select {
            get { return @select; }
        }

        public Expression Projector {
            get { return projector; }
        }

        public LambdaExpression Aggregator {
            get { return aggregator; }
        }

        public bool IsSingleton {
            get { return aggregator != null && aggregator.Body.Type == projector.Type; }
        }

        public string QueryText {
            get { return SqlFormatter.Format(select, true); }
        }

        public override string ToString() {
            return DbExpressionWriter.WriteToString(this);
        }
    }

    public class ClientJoinExpression : DbExpression {
        private readonly ReadOnlyCollection<Expression> innerKey;
        private readonly ReadOnlyCollection<Expression> outerKey;
        private readonly ProjectionExpression projection;

        public ClientJoinExpression(ProjectionExpression projection, IEnumerable<Expression> outerKey, IEnumerable<Expression> innerKey) : base(DbExpressionType.ClientJoin, projection.Type) {
            this.outerKey = outerKey.ToReadOnly();
            this.innerKey = innerKey.ToReadOnly();
            this.projection = projection;
        }

        public ReadOnlyCollection<Expression> OuterKey {
            get { return outerKey; }
        }

        public ReadOnlyCollection<Expression> InnerKey {
            get { return innerKey; }
        }

        public ProjectionExpression Projection {
            get { return projection; }
        }
    }

    public class BatchExpression : Expression {
        private readonly Expression batchSize;
        private readonly Expression input;
        private readonly LambdaExpression operation;
        private readonly Expression stream;

        public BatchExpression(Expression input, LambdaExpression operation, Expression batchSize, Expression stream)
            : base((ExpressionType)DbExpressionType.Batch, typeof(IEnumerable<>).MakeGenericType(operation.Body.Type)) {
            this.input = input;
            this.operation = operation;
            this.batchSize = batchSize;
            this.stream = stream;
        }

        public Expression Input {
            get { return input; }
        }

        public LambdaExpression Operation {
            get { return operation; }
        }

        public Expression BatchSize {
            get { return batchSize; }
        }

        public Expression Stream {
            get { return stream; }
        }
    }

    public class FunctionExpression : DbExpression {
        private readonly ReadOnlyCollection<Expression> arguments;
        private readonly string name;

        public FunctionExpression(Type type, string name, IEnumerable<Expression> arguments) : base(DbExpressionType.Function, type) {
            this.name = name;
            this.arguments = arguments.ToReadOnly();
        }

        public string Name {
            get { return name; }
        }

        public ReadOnlyCollection<Expression> Arguments {
            get { return arguments; }
        }
    }

    public abstract class CommandExpression : DbExpression {
        protected CommandExpression(DbExpressionType eType, Type type) : base(eType, type) {}
    }

    public class InsertCommand : CommandExpression {
        private readonly ReadOnlyCollection<ColumnAssignment> assignments;
        private readonly TableExpression table;

        public InsertCommand(TableExpression table, IEnumerable<ColumnAssignment> assignments) : base(DbExpressionType.Insert, typeof(int)) {
            this.table = table;
            this.assignments = assignments.ToReadOnly();
        }

        public TableExpression Table {
            get { return table; }
        }

        public ReadOnlyCollection<ColumnAssignment> Assignments {
            get { return assignments; }
        }
    }

    public class ColumnAssignment {
        private readonly ColumnExpression column;
        private readonly Expression expression;

        public ColumnAssignment(ColumnExpression column, Expression expression) {
            this.column = column;
            this.expression = expression;
        }

        public ColumnExpression Column {
            get { return column; }
        }

        public Expression Expression {
            get { return expression; }
        }
    }

    public class UpdateCommand : CommandExpression {
        private readonly ReadOnlyCollection<ColumnAssignment> assignments;
        private readonly TableExpression table;
        private readonly Expression where;

        public UpdateCommand(TableExpression table, Expression where, IEnumerable<ColumnAssignment> assignments) : base(DbExpressionType.Update, typeof(int)) {
            this.table = table;
            this.where = where;
            this.assignments = assignments.ToReadOnly();
        }

        public TableExpression Table {
            get { return table; }
        }

        public Expression Where {
            get { return @where; }
        }

        public ReadOnlyCollection<ColumnAssignment> Assignments {
            get { return assignments; }
        }
    }

    public class DeleteCommand : CommandExpression {
        private readonly TableExpression table;
        private readonly Expression where;

        public DeleteCommand(TableExpression table, Expression where) : base(DbExpressionType.Delete, typeof(int)) {
            this.table = table;
            this.where = where;
        }

        public TableExpression Table {
            get { return table; }
        }

        public Expression Where {
            get { return @where; }
        }
    }

    public class IFCommand : CommandExpression {
        private readonly Expression check;
        private readonly Expression ifFalse;
        private readonly Expression ifTrue;

        public IFCommand(Expression check, Expression ifTrue, Expression ifFalse) : base(DbExpressionType.If, ifTrue.Type) {
            this.check = check;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }

        public Expression Check {
            get { return check; }
        }

        public Expression IfTrue {
            get { return ifTrue; }
        }

        public Expression IfFalse {
            get { return ifFalse; }
        }
    }

    public class BlockCommand : CommandExpression {
        private readonly ReadOnlyCollection<Expression> commands;

        public BlockCommand(IList<Expression> commands) : base(DbExpressionType.Block, commands[commands.Count - 1].Type) {
            this.commands = commands.ToReadOnly();
        }

        public BlockCommand(params Expression[] commands) : this((IList<Expression>)commands) {}

        public ReadOnlyCollection<Expression> Commands {
            get { return commands; }
        }
    }

    public class DeclarationCommand : CommandExpression {
        private readonly SelectExpression source;
        private readonly ReadOnlyCollection<VariableDeclaration> variables;

        public DeclarationCommand(IEnumerable<VariableDeclaration> variables, SelectExpression source) : base(DbExpressionType.Declaration, typeof(void)) {
            this.variables = variables.ToReadOnly();
            this.source = source;
        }

        public ReadOnlyCollection<VariableDeclaration> Variables {
            get { return variables; }
        }

        public SelectExpression Source {
            get { return source; }
        }
    }

    public class VariableDeclaration {
        private readonly Expression expression;
        private readonly string name;
        private readonly QueryType type;

        public VariableDeclaration(string name, QueryType type, Expression expression) {
            this.name = name;
            this.type = type;
            this.expression = expression;
        }

        public string Name {
            get { return name; }
        }

        public QueryType QueryType {
            get { return type; }
        }

        public Expression Expression {
            get { return expression; }
        }
    }

    public class VariableExpression : Expression {
        private readonly string name;
        private readonly QueryType queryType;

        public VariableExpression(string name, Type type, QueryType queryType) : base((ExpressionType)DbExpressionType.Variable, type) {
            this.name = name;
            this.queryType = queryType;
        }

        public string Name {
            get { return name; }
        }

        public QueryType QueryType {
            get { return queryType; }
        }
    }
}