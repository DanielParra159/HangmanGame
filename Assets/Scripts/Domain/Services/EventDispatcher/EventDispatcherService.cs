namespace Domain.Services.EventDispatcher
{
    public interface EventDispatcherService
    {
        void Subscribe<T>(SignalDelegate callback) where T : Signal;
        void Unsubscribe<T>(SignalDelegate callback) where T : Signal;
        void Notify<T>(T signal) where T : Signal;
    }
}