using Domain.Services.EventDispatcher;

namespace Domain
{
    public class UpdateLoadingScreenSignal : Signal
    {
        public readonly bool IsVisible;

        public UpdateLoadingScreenSignal(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}