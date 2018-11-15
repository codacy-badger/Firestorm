﻿using System.Net;
using System.Threading.Tasks;
using Firestorm.Endpoints.Web;
using Firestorm.Tests.Unit.Endpoints.Stubs;
using Xunit;

namespace Firestorm.Tests.Unit.Endpoints.Start
{
    public class FirestormMiddlewareHelperTests
    {
        private readonly MockStartResource _startResource;
        private readonly FirestormConfiguration _firestormConfiguration;

        public FirestormMiddlewareHelperTests()
        {
            _startResource = new MockStartResource();

            _firestormConfiguration = new FirestormConfiguration
            {
                StartResourceFactory = new SingletonStartResourceFactory(_startResource)
            };
        }

        [Fact]
        public async Task Middleware_Get_ReturnsStartResourceBody()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "GET",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(HttpStatusCode.OK, handler.ResponseStatusCode);
            // TODO too big-a-test. Maybe test invoker alone?
            //Assert.Equal(_startResource.ObjectValue, handler.ResponseBody);
        }

        [Fact]
        public async Task Middleware_Put_EditsScalar()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "PUT",
                RequestContentReader = new MockJsonReader(@"""New value"""),
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);
            
            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal("New value", _startResource.ObjectValue);
        }

        [Fact]
        public async Task Middleware_Delete_SetsToNull()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "DELETE",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(null, _startResource.ObjectValue);
        }

        [Fact]
        public async Task Middleware_Options_Dunno()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "OPTIONS",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

        }

        [Fact]
        public async Task Middleware_InvalidMethod_405()
        {
            var handler = new MockHttpRequestHandler
            {
                RequestMethod = "NOTMETHOD",
                ResourcePath = ""
            };

            var helper = new FirestormMiddlewareHelper(_firestormConfiguration, handler);

            await helper.InvokeAsync(new TestEndpointContext());

            Assert.Equal(HttpStatusCode.MethodNotAllowed, handler.ResponseStatusCode);
        }

        public class MockStartResource : IRestScalar
        {
            public object ObjectValue { get; set; } = "My value";

            public async Task<object> GetAsync()
            {
                return ObjectValue;
            }

            public async Task<Acknowledgment> EditAsync(object value)
            {
                ObjectValue = value;
                return new Acknowledgment();
            }
        }
    }
}