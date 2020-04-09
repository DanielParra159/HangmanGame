using Domain.UseCases.GuessLetter;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class KeyboardController
    {
        private readonly KeyboardViewModel _keyboardViewModel;
        private readonly GuessLetter _guessLetter;

        public KeyboardController(KeyboardViewModel keyboardViewModel, GuessLetter guessLetter)
        {
            _keyboardViewModel = keyboardViewModel;
            _guessLetter = guessLetter;

            _keyboardViewModel
                .OnKeyPressedPressed
                .Subscribe(letter => _guessLetter.Guess(letter));
        }
    }
}