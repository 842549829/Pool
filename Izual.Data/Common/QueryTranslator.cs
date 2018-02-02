#region File Comments

// ////////////////////////////////////////////////////////////////////////////////////////////////
// file：Izual.Data.QueryTranslator.cs
// description：
// 
// create by：Izual ,2012/07/03
// last modify：Izual ,2012/07/05
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq.Expressions;
using Izual.Linq;

namespace Izual.Data.Common {
    /// <summary>
    /// </summary>
    public class QueryTranslator {
        private readonly QueryLinguist linguist;
        private readonly QueryMapper mapper;
        private readonly QueryPolice police;

        public QueryTranslator(QueryLanguage language, QueryMapping mapping, QueryPolicy policy) {
            linguist = language.CreateLinguist(this);
            mapper = mapping.CreateMapper(this);
            police = policy.CreatePolice(this);
        }

        public QueryLinguist Linguist {
            get { return linguist; }
        }

        public QueryMapper Mapper {
            get { return mapper; }
        }

        public QueryPolice Police {
            get { return police; }
        }

        public virtual Expression Translate(Expression expression) {
            // pre-evaluate local sub-trees
            expression = PartialEvaluator.Eval(expression, mapper.Mapping.CanBeEvaluatedLocally);

            // apply mapping (binds LINQ operators too)
            expression = mapper.Translate(expression);

            // any policy specific translations or validations
            expression = police.Translate(expression);

            // any language specific translations or validations
            expression = linguist.Translate(expression);

            return expression;
        }
    }
}