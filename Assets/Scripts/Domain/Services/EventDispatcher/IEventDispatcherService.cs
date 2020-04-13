namespace Domain.Services.EventDispatcher
{
    public interface IEventDispatcherService
    {
        void Subscribe<T>(SignalDelegate callback) where T : ISignal;
        void Unsubscribe<T>(SignalDelegate callback) where T : ISignal;
        void Dispatch<T>(T signal) where T : ISignal;
    }
}
