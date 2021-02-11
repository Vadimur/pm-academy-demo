using System;

namespace DesignPatterns.IoC
{
    public interface IServiceDescriptor
    {
        object GetService(IServiceProvider provider);
        Lifetime Lifetime { get; }
        /*Type ServiceType { get; }
        object Instance { get; }
        Func<object> InstanceFactory { get; }
        Func<IServiceProvider, object> ProviderInstanceFactory { get; }*/
    }
}