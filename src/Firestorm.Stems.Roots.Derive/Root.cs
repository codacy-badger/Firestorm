using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Engine;
using JetBrains.Annotations;

namespace Firestorm.Stems.Roots.Derive
{
    /// <summary>
    /// Provides the data source for a <see cref="Stem{TItem}"/> without a parent.
    /// </summary>
    public abstract class Root<TItem> : Root
        where TItem : class
    {
        public abstract IQueryable<TItem> GetAllItems();

        public virtual Task ForEachAsync<T>(IQueryable<T> query, Action<T> action)
        {
            return ItemQueryHelper.DefaultForEachAsync(query, action);
        }

        public abstract TItem CreateAndAttachItem();

        public abstract void MarkDeleted(TItem item);
    }

    /// <summary>
    /// Provides the data source for a <see cref="Stem"/> without a parent.
    /// </summary>
    [UsedImplicitly]
    public abstract class Root : IAxis
    {
        /// <summary>
        /// Internal constructor prevents creating weakly-typed Roots.
        /// </summary>
        internal Root()
        { }
        

        public abstract Type StartStemType { get; }

        public IRestUser User { get; set; }

        public IStemConfiguration Configuration { get; set; }

        public event EventHandler OnDispose;

        public virtual void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        public abstract Task SaveChangesAsync();
    }
}