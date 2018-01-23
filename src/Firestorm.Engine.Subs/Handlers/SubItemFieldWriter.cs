using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Firestorm.Data;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Subs.Context;
using Firestorm.Engine.Subs.Repositories;
using JetBrains.Annotations;

namespace Firestorm.Engine.Subs.Handlers
{
    public class SubItemFieldWriter<TItem, TNav> : IFieldWriter<TItem>
        where TItem : class
        where TNav : class, new()
    {
        private readonly SubWriterTools<TItem, TNav, TNav> _navTools;
        private readonly IEngineSubContext<TNav> _subContext;


        public SubItemFieldWriter(SubWriterTools<TItem, TNav, TNav> navTools, IEngineSubContext<TNav> subContext)
        {
            _navTools = navTools;
            _subContext = subContext;
        }

        public async Task SetValueAsync(IDeferredItem<TItem> item, object deserializedValue, IDataTransaction dataTransaction)
        {
            //IQueryableSingle<TNav> navigationQuery = item.Query.Select(_navigationExpression).SingleDefferred();
            //IEngineRepository<TNav> navRepository = new QueryableSingleRepository<TNav>(navigationQuery);
            IEngineRepository<TNav> navRepository = new NavigationItemRepository<TItem, TNav>(item, _navTools);
            
            var itemData = new RestItemData(deserializedValue);

            var navLocatorCreator = new NavigationItemLocatorCreator<TNav>(_subContext);
            DeferredItemBase<TNav> deferredItem = await navLocatorCreator.LocateOrCreateItemAsync(navRepository, itemData, item.LoadAsync);
            //DeferredItemBase<TNav> deferredItem = new RepositoryDeferredItem<TNav>(navSingleRepository);

            IDataTransaction transaction = new VoidTransaction(); // we commit the transaction in the parent. TODO optional save-as-you-go ?
            var navContext = new FullEngineContext<TNav>(transaction, navRepository, _subContext);
            var navEngineItem = new EngineRestItem<TNav>(navContext, deferredItem);
            Acknowledgment acknowledgment = await navEngineItem.EditAsync(itemData);
        }
    }
}