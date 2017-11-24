using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Engine;
using Firestorm.Engine.Fields;

namespace Firestorm.Stems.Fuel.Substems.Handlers
{
    public class SubCollectionFieldReader<TItem, TProperty, TNav> : IFieldReader<TItem>
        where TItem : class
        where TNav : class, new()
        where TProperty : IEnumerable<TNav>
    {
        private readonly Expression<Func<TItem, TProperty>> _navigationExpression;
        private readonly IEngineSubContext<TNav> _substemSubContext;

        public SubCollectionFieldReader(Expression<Func<TItem, TProperty>> navigationExpression, IEngineSubContext<TNav> substemSubContext)
        {
            _navigationExpression = navigationExpression;
            _substemSubContext = substemSubContext;
        }

        public Type FieldType => typeof(IEnumerable); // TODO maybe just object ?

        public Expression GetSelectExpression(ParameterExpression itemPram)
        {
            var visitedNavigationExpr = (LambdaExpression) new ParameterReplacerVisitor(_navigationExpression.Parameters[0], itemPram).Visit(_navigationExpression);

            LambdaExpression memberInitLambda = SubUtilities.GetMemberInitLambda(_substemSubContext.FieldProvider);

            Type dynamicType = memberInitLambda.ReturnType;
            MethodCallExpression selectMethodExpr = Expression.Call(typeof(Enumerable), "Select", new[] { typeof(TNav), dynamicType }, visitedNavigationExpr.Body, memberInitLambda);
            return selectMethodExpr;
        }

        public IFieldValueReplacer<TItem> Replacer { get; } = null;

        public Expression GetFilterExpression(ParameterExpression itemPram, FilterComparisonOperator comparisonOperator, string valueString)
        {
            throw new NotSupportedException("Filtering is not supported for sub collections.");
        }

        public LambdaExpression GetSortExpression(ParameterExpression itemPram)
        {
            throw new NotSupportedException("Sorting is not supported for sub collections.");
        }
    }
}