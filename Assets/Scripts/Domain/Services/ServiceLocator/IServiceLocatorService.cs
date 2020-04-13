namespace Domain.Services.ServiceLocator
{
    public interface IServiceLocatorService
    {
        T Get<T>();
    }
    
    public interface IServiceLocatorRegisterService
    {
        void Register<T>(T service);
    }
}