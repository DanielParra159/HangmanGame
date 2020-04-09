using System.Diagnostics;
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
                .Subscribe(letter =>
                {
                    Debug.Assert(letter.Length == 1,
                        $"Something is wrong configured on KeyboardView, received {letter} and expected a letter of length 1");
                    _guessLetter.Guess(letter[0]);
                });
        }
    }
}