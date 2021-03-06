using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Firestorm.Stems.Analysis;
using Firestorm.Stems.Definitions;
using Reflectious;

namespace Firestorm.Stems.Essentials.Resolvers
{
    /// <summary>
    /// Adds information about a Stem member with the <see cref="GetAttribute"/> to the <see cref="FieldDefinition"/>.
    /// </summary>
    internal class GetterAttributeResolver : FieldAttributeResolverBase
    {
        private readonly string _argumentMemberName;
        private Display? _display;

        public GetterAttributeResolver(string argumentMemberName, Display? display)
        {
            _argumentMemberName = argumentMemberName;
            _display = display;
        }

        public override void IncludeMember(MemberInfo member)
        {
            SetDisplayFromSugarAttribute(member);
            
            base.IncludeMember(member);
        }

        private void SetDisplayFromSugarAttribute(MemberInfo member)
        {
            var sugarAttributes = member.GetCustomAttributes<NestedAttribute>().ToArray();
            if (sugarAttributes.Length == 0)
                return;
            
            if(sugarAttributes.Length > 1)
                throw new StemAttributeSetupException("Cannot set multiple NestedAttributes on the same field.");
            
            if(_display.HasValue)
                throw new StemAttributeSetupException("Cannot use a NestedAttribute on a field that is already setting the Display property on the GetAttribute.");

            _display = (Display)sugarAttributes[0].NestingLevel;
        }

        protected override void AddExpressionToDefinition(LambdaExpression expression)
        {
            if (expression.Parameters.Count != 1)
                throw new StemAttributeSetupException("The '" + _argumentMemberName + "' expression must only have one parameter for the item.");

            FieldDefinition.Display = _display;

            FieldDefinition.Getter.Expression = expression;
        }

        protected override Type AddMethodToDefinition(MethodInfo method)
        {
            FieldDefinition.Display = _display;

            ParameterInfo[] parameters = method.GetParameters();
            Type returnType = method.ReturnType;

            if (_argumentMemberName != null)
            {
                PropertyInfo argumentProperty = method.ReflectedType.GetProperty(_argumentMemberName);
                if (argumentProperty == null)
                    throw new StemAttributeSetupException("Cannot find a property in this Stem with the name '" + _argumentMemberName + "'");
                
                LambdaExpression expression = GetExpressionFromProperty(argumentProperty);

                FieldDefinition.Getter.Expression = expression;

                Type delegateType = typeof(Func<,>).MakeGenericType(expression.ReturnType, returnType);
                AddMethodToHandlerPart(FieldDefinition.Getter, method, delegateType);

                if (expression.ReturnType == returnType)
                    return expression.ReturnType;

                // we must return a type that both db query type and replacement type derive from.
                // we could find a common ancestor between them
                return typeof(object);
            }

            if (parameters.Length == 0)
            {
                if (!returnType.IsSubclassOfGeneric(typeof(Expression<>)))
                    throw new StemAttributeSetupException("A getter method with no parameters must return a strongly typed Expression<>.");

                Type delegateType = typeof(Func<>).MakeGenericType(returnType);
                AddMethodToHandlerPart(FieldDefinition.Getter, method, delegateType);

                Type propertyValueType = ResolverTypeUtility.GetPropertyLambdaReturnType(ItemType, returnType);
                return propertyValueType;
            }
            else
            {
                if (parameters.Length != 1)
                    throw new StemAttributeSetupException("A getter method that does not return an expression must have one parameter for the item.");

                if (parameters[0].ParameterType != ItemType)
                    throw new StemAttributeSetupException("A getter method's first parameter must be of type " + ItemType.Name);

                Type delegateType = typeof(Func<,>).MakeGenericType(ItemType, returnType);
                AddMethodToHandlerPart(FieldDefinition.Getter, method, delegateType);
                return method.ReturnType;
            }
        }
    }
}