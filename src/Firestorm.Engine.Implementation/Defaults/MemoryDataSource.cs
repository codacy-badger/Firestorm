using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Firestorm.Data;

namespace Firestorm.Engine.Defaults
{
    public class MemoryDataSource : IDiscoverableDataSource, IEnumerable<IList>
    {
        private readonly ConcurrentDictionary<Type, IList> _lists = new ConcurrentDictionary<Type, IList>();

        public IDataTransaction CreateTransaction()
        {
            return new VoidTransaction();
        }

        public IEngineRepository<TItem> GetRepository<TItem>(IDataTransaction transaction)
            where TItem : class, new()
        {
            IList<TItem> list = GetList<TItem>();
            return new MemoryRepository<TItem>(list);
        }

        public void Add<TItem>(IEnumerable<TItem> items)
        {
            List<TItem> list = GetList<TItem>();
            list.AddRange(items);
        }

        private List<TEntity> GetList<TEntity>()
        {
            return (List<TEntity>)_lists.GetOrAdd(typeof(TEntity), t => new List<TEntity>());
        }

        public IEnumerable<Type> FindRepositoryTypes()
        {
            return _lists.Keys;
        }

        IEnumerator<IList> IEnumerable<IList>.GetEnumerator()
        {
            return _lists.Values.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _lists.Values.GetEnumerator();
        }
    }
}