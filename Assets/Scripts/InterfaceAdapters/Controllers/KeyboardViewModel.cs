using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class KeyboardViewModel
    {
        public readonly ReactiveCommand<string> OnKeyPressedPressed;

        public KeyboardViewModel()
        {
            OnKeyPressedPressed = new ReactiveCommand<string>();
        }
    }
}