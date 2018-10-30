﻿using System;

namespace Firestorm.AspNetCore2
{
    /// <summary>
    /// An interface that can be extended to inject Firestorm services.
    /// </summary>
    public interface IFirestormServicesBuilder
    {
        IFirestormServicesBuilder Add<TService>(Func<IServiceProvider, TService> implementationFactory) 
            where TService : class;
        
        IFirestormServicesBuilder Add(Type serviceType);
        
        IFirestormServicesBuilder Add<TService>(TService implementationInstance)
            where TService : class;

        IFirestormServicesBuilder Add<TAbstraction, TImplementation>()
            where TImplementation : class, TAbstraction
            where TAbstraction : class;
    }
}