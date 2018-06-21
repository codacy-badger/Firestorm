using Firestorm.Engine;
using Firestorm.Engine.Identifiers;
using Firestorm.Engine.Subs.Context;
using Firestorm.Stems.Fuel.Authorization;
using Firestorm.Stems.Fuel.Fields;
using Firestorm.Stems.Fuel.Identifiers;
using JetBrains.Annotations;

namespace Firestorm.Stems.Fuel
{
    /// <summary>
    /// Wraps a <see cref="Stem{TItem}"/> and provides API field information for the Firestorm Engine.
    /// Basically a <see cref="FullEngineContext{TItem}"/> without repository info.
    /// </summary>
    public class StemsEngineSubContext<TItem> : IEngineSubContext<TItem>
        where TItem : class
    {
        private readonly Stem<TItem> _stem;

        public StemsEngineSubContext([NotNull] Stem<TItem> stem)
        {
            _stem = stem;

            Identifiers = new AttributeIdentifierProvider<TItem>(stem); //new IdConventionIdentifierInfo<TItem>();
            Fields = new AttributeFieldProvider<TItem>(stem);
            AuthorizationChecker = new StemAuthorizationChecker<TItem>(stem);
        }

        public IIdentifierProvider<TItem> Identifiers { get; }

        public ILocatableFieldProvider<TItem> Fields { get; }

        public IAuthorizationChecker<TItem> AuthorizationChecker { get; }
    }
}