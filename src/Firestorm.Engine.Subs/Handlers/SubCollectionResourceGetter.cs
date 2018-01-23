using System.Collections.Generic;
using Firestorm.Data;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubCollectionResourceGetter<TItem, TCollection, TNav> : IFieldResourceGetter<TItem>
        where TItem : class
        where TNav : class, new()
        where TCollection : class, IEnumerable<TNav>
    {
        private readonly SubWriterTools<TItem, TCollection, TNav> _navTools;
        private readonly IEngineSubContext<TNav> _subContext;

        public SubCollectionResourceGetter(SubWriterTools<TItem, TCollection, TNav> navTools, IEngineSubContext<TNav> subContext)
        {
            _navTools = navTools;
            _subContext = subContext;
        }

        public IRestResource GetFullResource(IDeferredItem<TItem> item, IDataTransaction dataTransaction)
        {
            IEngineRepository<TNav> navRepository = new NavigationCollectionRepository<TItem, TCollection, TNav>(item, _navTools);

            var context = new FullEngineContext<TNav>(dataTransaction, navRepository, _subContext);

            return new EngineRestCollection<TNav>(context);
        }
    }
}