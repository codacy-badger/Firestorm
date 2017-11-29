﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Firestorm.Core;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Strategies;
using Firestorm.Tests.Endpoints.Models;
using Xunit;

namespace Firestorm.Tests.Endpoints
{
    public class AddToCollectionStrategyTests
    {
        public AddToCollectionStrategyTests()
        {
            Collection = new CountingCollection();
        }

        private CountingCollection Collection { get; set; }

        [Fact]
        public async Task ExecuteAdd_IncreasesCount()
        {
            int startCount = Collection.Count;

            var newItemData = new RestItemData();
            var strategy = new AddToCollectionStrategy();
            await strategy.ExecuteAsync(Collection, new TestEndpointContext(), new ItemBody(newItemData));

            int afterCount = Collection.Count;
            Assert.Equal(startCount + 1, afterCount);
        }

        [Fact]
        public async Task ExecuteAddMany_IncreasesCount()
        {
            int startCount = Collection.Count;

            var newItemData = new RestItemData();
            var strategy = new AddToCollectionStrategy();
            var collectionData = new RestCollectionData(new[] { newItemData, newItemData, newItemData });
            await strategy.ExecuteAsync(Collection, new TestEndpointContext(), new CollectionBody(collectionData, null));

            int afterCount = Collection.Count;
            Assert.Equal(startCount + 3, afterCount);
        }

        private class CountingCollection : IRestCollection // TODO mock instead of this
        {
            public int Count { get; private set; } = 0;

            public async Task<CreatedItemAcknowledgment> AddAsync(RestItemData itemData)
            {
                Count++;
                return new CreatedItemAcknowledgment(null);
            }

            public Task<RestCollectionData> QueryDataAsync(IRestCollectionQuery query)
            {
                throw new NotImplementedException();
            }

            public IRestItem GetItem(string identifier, string identifierName = null)
            {
                throw new NotImplementedException();
            }

            public IRestDictionary ToDictionary(string identifierName)
            {
                throw new NotImplementedException();
            }
        }
    }
}
