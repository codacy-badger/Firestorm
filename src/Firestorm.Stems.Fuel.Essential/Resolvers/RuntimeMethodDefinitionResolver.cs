using System;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Attributes.Definitions;
using Firestorm.Stems.Fuel.Essential.Factories;
using Firestorm.Stems.Fuel.Resolving.Analysis;
using Firestorm.Stems.Fuel.Resolving.Factories;
using Reflectious;

namespace Firestorm.Stems.Fuel.Essential.Resolvers
{
    /// <summary>
    /// Resolves factories using delegates created from methods in the Stem classes.
    /// </summary>
    internal class RuntimeMethodDefinitionResolver : IFieldDefinitionResolver
    {
        public IStemConfiguration Configuration { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public void IncludeDefinition<TItem>(EngineImplementations<TItem> implementations)
            where TItem : class
        {
            if (FieldDefinition.SubstemType != null)
                return; // handled by the SubstemDefinitionResolver instead

            if (FieldDefinition.Getter.GetInstanceMethod != null)
            {
                IFactory<IFieldReader<TItem>, TItem> factory;

                if (FieldDefinition.Getter.Expression != null)
                {
                    var middleExpression = FieldDefinition.Getter.Expression;

                    factory = typeof(InstanceMethodWithExpressionFieldReaderFactory<,,>).Reflect()
                        .MakeGeneric(typeof(TItem), middleExpression.ReturnType, FieldDefinition.FieldType)
                        .CastTo<IFactory<IFieldReader<TItem>, TItem>>()
                        .CreateInstance(middleExpression, FieldDefinition.Getter.GetInstanceMethod);
                }
                else
                {
                    factory = typeof(InstanceMethodFieldReaderFactory<,>).Reflect()
                        .MakeGeneric(typeof(TItem), FieldDefinition.FieldType)
                        .CastTo<IFactory<IFieldReader<TItem>, TItem>>()
                        .CreateInstance(FieldDefinition.Getter.GetInstanceMethod);
                }

                implementations.ReaderFactories.Add(FieldDefinition.FieldName, factory);
                implementations.CollatorFactories.Add(FieldDefinition.FieldName, new BasicCollatorFactory<TItem>(factory));
            }

            if (FieldDefinition.Setter.GetInstanceMethod != null)
            {
                Type setterType = typeof(ActionFieldWriterFactory<,>).MakeGenericType(typeof(TItem), FieldDefinition.FieldType);
                var setter = (IFactory<IFieldWriter<TItem>, TItem>) Activator.CreateInstance(setterType, FieldDefinition.Setter.GetInstanceMethod);
                implementations.WriterFactories.Add(FieldDefinition.FieldName, setter);
            }

            if (FieldDefinition.Locator.GetInstanceMethod != null)
            {
                var locatorFactory = new DelegateItemLocatorFactory<TItem>(FieldDefinition.Locator.GetInstanceMethod);
                implementations.LocatorFactories.Add(FieldDefinition.FieldName, locatorFactory);
            }
        }
    }
}