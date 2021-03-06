using System;
using System.Linq;
using Firestorm.Data;
using Firestorm.Engine;
using Firestorm.Engine.Additives.Authorization;
using Firestorm.Engine.Additives.Identifiers;
using Firestorm.Engine.Defaults;
using Firestorm.Engine.Deferring;
using Firestorm.Engine.Fields;
using Firestorm.Engine.Identifiers;
using Firestorm.Testing.Models;
using Firestorm.Testing;

namespace Firestorm.Testing.Http.Models
{
    public class CodedArtistEntityContext : IEngineContext<Artist>
    {
        public CodedArtistEntityContext(IRestUser user)
        {
            Transaction = new VoidTransaction();
            Repository = new ArtistMemoryRepository();
            AuthorizationChecker = new AllowAllAuthorizationChecker<Artist>();
        }

        public IDataTransaction Transaction { get; }

        public IEngineRepository<Artist> Repository { get; }

        public IIdentifierProvider<Artist> Identifiers { get; } = new SingleIdentifierProvider<Artist>(new ExpressionIdentifierInfo<Artist, int>(a => a.ID));

        public IFieldProvider<Artist> Fields => StaticFieldProvider;

        private static IFieldProvider<Artist> StaticFieldProvider { get; } = new FieldDictionary<Artist>
        {
            {"Id", a => a.ID},
            {"Name", a => a.Name}
        };

        public IAuthorizationChecker<Artist> AuthorizationChecker { get; }

        private class ArtistAuthorizationChecker : IAuthorizationChecker<Artist>
        {
            private readonly IRestUser _user;

            public ArtistAuthorizationChecker(IRestUser user)
            {
                _user = user;
            }

            public IQueryable<Artist> ApplyFilter(IQueryable<Artist> items)
            {
                throw new NotImplementedException();
            }

            public bool CanGetItem(IDeferredItem<Artist> itemQuery)
            {
                throw new NotImplementedException();
            }

            public bool CanAddItem()
            {
                throw new NotImplementedException();
            }

            public bool CanEditItem(IDeferredItem<Artist> itemQuery)
            {
                throw new NotImplementedException();
            }

            public bool CanDeleteItem(IDeferredItem<Artist> itemQuery)
            {
                throw new NotImplementedException();
            }

            public bool CanGetField(IDeferredItem<Artist> item, INamedField<Artist> field)
            {
                throw new NotImplementedException();
            }

            public bool CanEditField(IDeferredItem<Artist> item, INamedField<Artist> field)
            {
                throw new NotImplementedException();
            }
        }

        public bool AllowsUpsert { get; }
    }
}