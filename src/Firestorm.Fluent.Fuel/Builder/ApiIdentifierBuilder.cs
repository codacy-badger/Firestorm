﻿using System;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class ApiIdentifierBuilder<TItem, TIdentifier> : IApiIdentifierBuilder<TItem, TIdentifier>
        where TItem : class
    {
        private readonly ApiIdentifierModel<TItem> _identifierModel;

        internal ApiIdentifierBuilder(ApiIdentifierModel<TItem> identifierModel)
        {
            _identifierModel = identifierModel;
        }

        public void AddExpression(Expression<Func<TItem, TIdentifier>> expression)
        {
            _identifierModel.IdentifierInfo = new ExpressionIdentifierInfo<TItem, TIdentifier>(expression);
        }

        public IApiIdentifierBuilder<TItem, TIdentifier> HasName(string identifierName)
        {
            _identifierModel.Name = identifierName;
            return this;
        }
    }
}