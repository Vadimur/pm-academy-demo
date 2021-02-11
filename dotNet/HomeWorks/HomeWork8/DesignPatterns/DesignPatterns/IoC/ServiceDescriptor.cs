namespace DesignPatterns.IoC
{
    public class ServiceRegistrationDescriptor<T> : IServiceRegistrationDescriptor
    {
        private T _type;
        private Lifetime _lifetime;

        public ServiceRegistrationDescriptor(T type, Lifetime lifetime)
        {
            _type = type;
            _lifetime = lifetime;
        }
        
        
    }
}