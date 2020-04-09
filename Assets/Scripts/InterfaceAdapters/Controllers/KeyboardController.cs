using System.Diagnostics;
using Domain.UseCases.GuessLetter;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class KeyboardController
    {
        private readonly InGameViewModel _viewModel;
        private readonly GuessLetter _guessLetter;

        public KeyboardController(InGameViewModel viewModel, GuessLetter guessLetter)
        {
            _viewModel = viewModel;
            _guessLetter = guessLetter;

            _viewModel
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