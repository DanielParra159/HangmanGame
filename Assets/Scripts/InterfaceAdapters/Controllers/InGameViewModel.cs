using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class InGameViewModel
    {
        public ReactiveProperty<string> CurrentWord;

        public InGameViewModel()
        {
            CurrentWord = new StringReactiveProperty();
        }
    }
}