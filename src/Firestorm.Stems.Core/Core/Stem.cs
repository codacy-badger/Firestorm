using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Firestorm.Stems
{
    /// <summary>
    /// The base for Firestorm Stems: a neat, but not a necessary way to write Firestorm Web APIs.
    /// </summary>
    public abstract class Stem<TItem> : Stem
        where TItem : class
    {
        [CanBeNull]
        public virtual Expression<Func<TItem, ItemPermission>> PermissionExpression
        {
            get { return null; }
        }
        
        protected static Expression<Func<TItem, TField>> Expression<TField>(Expression<Func<TItem, TField>> expression)
        {
            return expression;
        }

        public virtual void OnCreating(TItem newItem)
        {
        }

        public virtual void OnUpdating(TItem item)
        {
        }

        public virtual bool MarkDeleted(TItem item)
        {
            return false;
        }

        public virtual Task OnSavingAsync(TItem item)
        {
            return Task.FromResult(false);
        }

        public virtual Task OnSavedAsync(TItem item)
        {
            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// The internal, typeless base for Firestorm Stems.
    /// </summary>
    public abstract class Stem : IAxis
    {
        /// <summary>
        /// Internal constructor prevents creating weakly-typed Stems.
        /// </summary>
        internal Stem()
        { }

        public IRestUser User { get; private set; }

        public IStemsCoreServices Services { get; private set; }

        public void SetParent(IAxis parent)
        {
            User = parent.User;
            Services = parent.Services;
            parent.OnDispose += (sender, args) => Dispose();
        }
        
        public virtual bool CanAddItem()
        {
            return true;
        }

        public event EventHandler OnDispose;

        public virtual void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}