using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Firestorm.Engine.Additives.Identifiers
{
    internal static class PredicateUtility
    {
        internal static Expression<Func<TItem, bool>> CombinePredicates<TItem>(IEnumerable<Expression<Func<TItem, bool>>> predicates)
        {
            ParameterExpression paramExpr = null;
            Expression combinedPredicate = null;

            foreach (Expression<Func<TItem, bool>> predicate in predicates)
            {
                if(predicate == null)
                    continue;
                
                Expression predicateBody;

                if (paramExpr == null)
                {
                    paramExpr = predicate.Parameters[0];
                    predicateBody = predicate.Body;
                }
                else
                {
                    var visitor = new ParameterReplacerVisitor(predicate.Parameters[0], paramExpr);
                    predicateBody = visitor.Visit(predicate.Body);
                }

                combinedPredicate = combinedPredicate == null ? predicateBody : Expression.OrElse(combinedPredicate, predicateBody);
            }

            if (combinedPredicate == null)
                return item => false;

            var combinedLambda = Expression.Lambda<Func<TItem, bool>>(combinedPredicate, paramExpr);
            return combinedLambda;
        }

        internal static Expression<Func<TItem, bool>> CombinePredicates<TItem>(params Expression<Func<TItem, bool>>[] predicates)
        {
            return CombinePredicates((IEnumerable<Expression<Func<TItem, bool>>>) predicates);
        }
    }
}