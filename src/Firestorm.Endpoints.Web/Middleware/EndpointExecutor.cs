﻿using System;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Web
{
    /// <summary>
    /// Executes the request from the given <see cref="IHttpRequestHandler"/> onto the given <see cref="IRestEndpoint"/>
    /// and builds the response using the <see cref="ResponseBuilder"/>.
    /// </summary>
    internal class EndpointExecutor
    {
        private readonly IRestEndpoint _endpoint;
        private readonly IRequestReader _requestReader;
        private readonly ResponseBuilder _responseBuilder;

        public EndpointExecutor(IRestEndpoint endpoint, IRequestReader requestReader, ResponseBuilder responseBuilder)
        {
            _endpoint = endpoint;
            _requestReader = requestReader;
            _responseBuilder = responseBuilder;
        }

        public Task ExecuteAsync()
        {
            switch (_requestReader.RequestMethod)
            {
                case "GET":
                    return ExecuteGetAsync();

                case "OPTIONS":
                    return ExecuteOptionsAsync();

                case "POST":
                case "PUT":
                case "PATCH":
                case "DELETE":
                    return ExecuteUnsafeAsync();

                default:
                    _responseBuilder.SetStatusCode(HttpStatusCode.MethodNotAllowed);
                    return Task.FromResult(false);
            }
        }

        private async Task ExecuteGetAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestReader.GetPreconditions()))
            {
                _responseBuilder.SetStatusCode(HttpStatusCode.NotModified);
                return;
            }

            ResourceBody resourceBody = await _endpoint.GetAsync(_requestReader.GetQuery());

            _responseBuilder.AddResource(resourceBody);
        }

        private async Task ExecuteOptionsAsync()
        {
            Options options = await _endpoint.OptionsAsync();

            _responseBuilder.AddOptions(options);
        }

        private async Task ExecuteUnsafeAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestReader.GetPreconditions()))
            {
                _responseBuilder.SetStatusCode(HttpStatusCode.PreconditionFailed);
                return;
            }

            var method =  (UnsafeMethod)Enum.Parse(typeof(UnsafeMethod), _requestReader.RequestMethod, true);
            ResourceBody requestBody = _requestReader.GetRequestBody();
            Feedback feedback = await _endpoint.UnsafeAsync(method, requestBody);

            _responseBuilder.AddFeedback(feedback);
        }
    }
}