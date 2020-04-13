using Domain.Services.EventDispatcher;

namespace Domain.UseCases.CommonSignals
{
    public class UpdateLoadingScreenSignal : ISignal
    {
        public readonly bool IsVisible;

        public UpdateLoadingScreenSignal(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}