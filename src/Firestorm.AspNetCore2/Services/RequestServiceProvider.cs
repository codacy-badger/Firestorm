﻿using System;
using Firestorm.Host;
using Microsoft.AspNetCore.Http;

namespace Firestorm.AspNetCore2
{
    /// <summary>
    /// Uses the <see cref="IHttpContextAccessor"/> to get request-scoped services when given a singleton-scoped service provider.
    /// </summary>
    internal class RequestServiceProvider : IServiceProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Uses the <see cref="IHttpContextAccessor"/> to get request-scoped services from the given <see cref="singletonServiceProvider"/>.
        /// </summary>
        public RequestServiceProvider(IServiceProvider singletonServiceProvider)
        {
            _contextAccessor = singletonServiceProvider.GetService<IHttpContextAccessor>()
                ?? throw new InvalidOperationException("IHttpContextAccessor is not registered.");
        }

        public object GetService(Type serviceType)
        {
            return _contextAccessor.HttpContext.RequestServices.GetService(serviceType);
        }
    }
}