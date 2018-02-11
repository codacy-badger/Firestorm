﻿using System;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Core.Web.Options;
using Firestorm.Endpoints.Responses;

namespace Firestorm.Endpoints.Start
{
    /// <summary>
    /// Invokes the request from the given <see cref="IHttpRequestHandler"/> onto the given <see cref="IRestEndpoint"/> and builds the response using the <see cref="ResponseBuilder"/>.
    /// </summary>
    internal class EndpointInvoker
    {
        private readonly IHttpRequestReader _requestReader;
        private readonly IRestEndpoint _endpoint;
        private readonly ResponseBuilder _responseBuilder;

        public EndpointInvoker(IHttpRequestReader requestReader, IRestEndpoint endpoint, ResponseBuilder responseBuilder)
        {
            _requestReader = requestReader;
            _responseBuilder = responseBuilder;
            _endpoint = endpoint;
        }

        public Task InvokeAsync()
        {
            switch (_requestReader.RequestMethod)
            {
                case "GET":
                    return InvokeGetAsync();

                case "OPTIONS":
                    return InvokeOptionsAsync();

                case "POST":
                case "PUT":
                case "PATCH":
                case "DELETE":
                    return InvokeUnsafeAsync();

                default:
                    _responseBuilder.SetStatusCode(HttpStatusCode.MethodNotAllowed);
                    return Task.FromResult(false);
            }
        }

        private async Task InvokeGetAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestReader.GetPreconditions()))
            {
                _responseBuilder.SetStatusCode(HttpStatusCode.NotModified);
                return;
            }

            ResourceBody resourceBody = await _endpoint.GetAsync();

            _responseBuilder.AddResource(resourceBody);
        }

        private async Task InvokeOptionsAsync()
        {
            Options options = await _endpoint.OptionsAsync();

            _responseBuilder.AddOptions(options);
        }

        private async Task InvokeUnsafeAsync()
        {
            if (!_endpoint.EvaluatePreconditions(_requestReader.GetPreconditions()))
            {
                _responseBuilder.SetStatusCode(HttpStatusCode.PreconditionFailed);
                return;
            }

            var method = (UnsafeMethod)Enum.Parse(typeof(UnsafeMethod), _requestReader.RequestMethod, true);
            ResourceBody postBody = _requestReader.GetRequestBodyObject();

            Feedback feedback = await _endpoint.UnsafeAsync(method, postBody);

            _responseBuilder.AddFeedback(feedback);
        }
    }
}