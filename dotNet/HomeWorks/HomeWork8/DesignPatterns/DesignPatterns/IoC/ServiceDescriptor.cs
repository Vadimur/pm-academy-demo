using System;

namespace DesignPatterns.IoC
{
    public class ServiceDescriptor<T> : IServiceDescriptor
    {
        public Lifetime Lifetime { get; }
        public Type ServiceType { get; }
        public T Instance { get; internal set; }
        public  Func<T> InstanceFactory { get; }
        public Func<IServiceProvider, T> ProviderInstanceFactory { get; }

        public ServiceDescriptor(Type type, Lifetime lifetime)
        {
            ServiceType = type;
            Lifetime = lifetime;
        }

        public ServiceDescriptor(Type serviceType, T instance) 
            : this(serviceType, Lifetime.Singleton)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            
            Instance = instance;
        }
        
        public ServiceDescriptor(Type serviceType, Func<T> instanceFactory, Lifetime lifetime) 
            : this(serviceType, lifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            InstanceFactory = instanceFactory;

        }
        
        public ServiceDescriptor(Type serviceType, Func<IServiceProvider, T> instanceFactory, Lifetime lifetime) 
                : this(serviceType, lifetime)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }
            
            ProviderInstanceFactory = instanceFactory;
        }

        public object GetService(IServiceProvider provider)
        {
            if (Instance != null)
            {
                return Instance;
            }

            T implementation;
            
            if (InstanceFactory != null)
            {
                implementation = InstanceFactory.Invoke();
            }
            else if (ProviderInstanceFactory != null)
            {
                implementation = ProviderInstanceFactory.Invoke(provider);
            }
            else
            {
                implementation = (T)Activator.CreateInstance(ServiceType);
            }

            if (Lifetime == Lifetime.Singleton)
            {
                Instance = implementation;
            }
            
            return implementation;
        }
    }
}