using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class MainMenuViewModel
    {
        public readonly ReactiveCommand OnStartGamePressed;
        public readonly ReactiveProperty<bool> IsVisible;

        public MainMenuViewModel()
        {
            OnStartGamePressed = new ReactiveCommand();
            IsVisible = new BoolReactiveProperty(true);
        }
    }
}