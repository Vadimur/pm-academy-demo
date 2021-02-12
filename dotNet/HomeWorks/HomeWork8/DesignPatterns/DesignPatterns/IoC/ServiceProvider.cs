using System;
using System.Collections.Generic;
using System.Linq;

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
            var isSuccess = _registeredServices.TryGetValue(typeof(T), out IServiceDescriptor descriptor);
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
                var defaultConstructor = typeof(T).GetConstructors()[0]; 
                var defaultParams = defaultConstructor.GetParameters();
                
                var parameters = defaultParams
                    .Select(param => typeof(ServiceProvider)
                                                .GetMethod(nameof(GetService))
                                                ?.MakeGenericMethod(param.ParameterType)
                                                .Invoke(this, null))
                    .ToArray();
                
                implementation = (T)defaultConstructor.Invoke(parameters);
            }

            if (typedDescriptor.Lifetime == Lifetime.Singleton)
            {
                typedDescriptor.Instance = implementation;
            }
            
            return implementation;
        }
    }
}