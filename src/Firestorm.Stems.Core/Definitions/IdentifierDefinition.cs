using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Firestorm.Stems.Definitions
{
    public class IdentifierDefinition
    {
        public string IdentifierName { get; set; }
        
        public LambdaExpression GetterExpression { get; set; }

        public MethodInfo PredicateMethod { get; set; }

        public bool IsMultiReference { get; set; }

        // should be Action<TItem, string>
        public Delegate SetterAction { get; set; }

        public MethodInfo ExactGetterMethod { get; set; }
        public string ExactValue { get; set; }

        public MethodInfo GetterMethod { get; set; }
    }
}