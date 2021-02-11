using System;
using System.Collections.Generic;

namespace DesignPatterns.IoC
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, IServiceDescriptor> _registeredServices;

        public ServiceProvider(Dictionary<Type, IServiceDescriptor> registeredServices)
        {
            _registeredServices = registeredServices;
        }
        
        public T GetService<T>()
        {
            var descriptor = _registeredServices[typeof(T)];
            return (T) descriptor.GetService(this);
        }
    }
}