#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.QueryTypeSystem.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;

namespace Izual.Data.Common {
    public abstract class QueryType {
        public abstract bool NotNull{ get; }
        public abstract int Length{ get; }
        public abstract short Precision{ get; }
        public abstract short Scale{ get; }
    }

    public abstract class QueryTypeResolver {
        public abstract QueryType Parse(string typeDeclaration);
        public abstract QueryType GetColumnType(Type type);
        public abstract string GetVariableDeclaration(QueryType type, bool suppressSize);
    }
}