﻿using Firestorm.Endpoints.Naming;
using Firestorm.Endpoints.Start;
using Firestorm.Host;
using JetBrains.Annotations;

namespace Firestorm.Endpoints.Web
{
    public class EndpointNavigator
    {
        private readonly IRequestContext _requestContext;
        private readonly IStartResourceFactory _startResourceFactory;
        private readonly RestEndpointConfiguration _configuration;

        public EndpointNavigator(IRequestContext requestContext, FirestormConfiguration configuration)
        {
            _requestContext = requestContext;
            _startResourceFactory = configuration.StartResourceFactory;
            _configuration = configuration.EndpointConfiguration;

        }
        
        public EndpointNavigator(IRequestContext requestContext, IStartResourceFactory startResourceFactory, RestEndpointConfiguration configuration)
        {
            _requestContext = requestContext;
            _startResourceFactory = startResourceFactory;
            _configuration = configuration;
        }

        public IRestEndpoint GetEndpointFromPath(string resourcePath)
        {
            IRestResource startResource = _startResourceFactory.GetStartResource(_requestContext);

            var context = new RestEndpointContext(_requestContext, _configuration);
            IRestEndpoint startEndpoint = Endpoint.GetFromResource(context, startResource);

            var nextAggregator = new EndpointNextAggregator(startEndpoint, _configuration.NamingConventionSwitcher ?? new VoidNamingConventionSwitcher());
            IRestEndpoint endpoint = nextAggregator.AggregateNext(resourcePath);

            //var wrappedEndpoint = new NamingSwitcherEndpointWrapper(endpoint, namingConventionSwitcher);

            return endpoint;
        }
    }

    public class RestEndpointContext : IRestEndpointContext
    {
        public RestEndpointContext(IRequestContext endpointContext, RestEndpointConfiguration configuration)
        {
            Request = endpointContext;
            Configuration = configuration;
        }

        public void Dispose()
        {
            Request.Dispose();
        }

        public RestEndpointConfiguration Configuration { get; }
        public IRequestContext Request { get; }
    }
}