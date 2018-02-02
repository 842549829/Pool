#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.QueryCommand.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Izual.Data.Common {
    public class QueryCommand {
        private readonly string commandText;
        private readonly ReadOnlyCollection<QueryParameter> parameters;

        public QueryCommand(string commandText, IEnumerable<QueryParameter> parameters) {
            this.commandText = commandText;
            this.parameters = parameters.ToReadOnly();
        }

        public string CommandText {
            get { return commandText; }
        }

        public ReadOnlyCollection<QueryParameter> Parameters {
            get { return parameters; }
        }
    }

    public class QueryParameter {
        private readonly string name;
        private readonly QueryType queryType;
        private readonly Type type;

        public QueryParameter(string name, Type type, QueryType queryType) {
            this.name = name;
            this.type = type;
            this.queryType = queryType;
        }

        public string Name {
            get { return name; }
        }

        public Type Type {
            get { return type; }
        }

        public QueryType QueryType {
            get { return queryType; }
        }
    }
}