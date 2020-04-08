namespace Domain.Services.ServiceLocator
{
    public interface ServiceLocatorService
    {
        T Get<T>();
    }
    
    public interface ServiceLocatorRegisterService
    {
        void Register<T>(T service);
    }
}