using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class LoadingViewModel
    {
        public readonly ReactiveProperty<bool> IsVisible;

        public LoadingViewModel()
        {
            IsVisible = new BoolReactiveProperty(false);
        }
    }
}