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
            var isSuccess = _registeredServices.TryGetValue(typeof(T), out var descriptor);
            if (!isSuccess)
            {
                throw new ArgumentOutOfRangeException(nameof(T), $"Type {nameof(T)} not registered");
            }
            
            var typedDescriptor = (ServiceDescriptor<T>)Convert.ChangeType(descriptor, typeof(ServiceDescriptor<T>));

            if (typedDescriptor.Instance != null)
            {
                return typedDescriptor.Instance;
            }

            T implementation;
            
            if (typedDescriptor.InstanceFactory != null)
            {
                implementation = typedDescriptor.InstanceFactory.Invoke();
            }
            else if (typedDescriptor.ProviderInstanceFactory != null)
            {
                implementation = typedDescriptor.ProviderInstanceFactory.Invoke(this);
            }
            else
            {
                implementation = (T)Activator.CreateInstance(typeof(T));
            }

            if (typedDescriptor.Lifetime == Lifetime.Singleton)
            {
                typedDescriptor.Instance = implementation;
            }
            
            return implementation;
        }
    }
}