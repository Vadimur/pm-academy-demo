using System;
using System.Collections.Generic;

namespace DesignPatterns.IoC
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, IServiceDescriptor> _registeredServices =
            new Dictionary<Type, IServiceDescriptor>();
        
        public IServiceCollection AddTransient<T>()
        {
            _registeredServices.Add(typeof(T), new ServiceDescriptor<T>(typeof(T), Lifetime.Transient));
            return this;
        }

        public IServiceCollection AddTransient<T>(Func<T> factory)
        {
            _registeredServices.Add(typeof(T), new ServiceDescriptor<T>(typeof(T), factory, Lifetime.Transient));
            return this;
        }

        public IServiceCollection AddTransient<T>(Func<IServiceProvider, T> factory)
        {
            _registeredServices.Add(typeof(T), new ServiceDescriptor<T>(typeof(T), factory, Lifetime.Transient));
            return this;
        }

        public IServiceCollection AddSingleton<T>()
        {
            _registeredServices.Add(typeof(T), new ServiceDescriptor<T>(typeof(T), Lifetime.Singleton));
            return this;
        }

        public IServiceCollection AddSingleton<T>(T service)
        {
            _registeredServices.Add(typeof(T), new ServiceDescriptor<T>(typeof(T), service));
            return this;
        }

        public IServiceCollection AddSingleton<T>(Func<T> factory)
        {
            _registeredServices.Add(typeof(T), new ServiceDescriptor<T>(typeof(T), factory, Lifetime.Singleton));
            return this;
        }

        public IServiceCollection AddSingleton<T>(Func<IServiceProvider, T> factory)
        {
            
            _registeredServices.Add(typeof(T), new ServiceDescriptor<T>(typeof(T), factory, Lifetime.Singleton));
            return this;
        }

        public IServiceProvider BuildServiceProvider()
        {
            return new ServiceProvider(_registeredServices);
        }
    }
}