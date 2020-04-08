using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class MainMenuViewModel
    {
        public readonly ReactiveCommand StartGamePressed;

        public MainMenuViewModel()
        {
            StartGamePressed = new ReactiveCommand();
        }
    }
}