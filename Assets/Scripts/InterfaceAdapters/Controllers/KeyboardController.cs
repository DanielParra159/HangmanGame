using System.Diagnostics;
using Domain.UseCases.GuessLetter;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class KeyboardController
    {
        public KeyboardController(InGameViewModel viewModel, IGuessLetter guessLetter)
        {
            viewModel
                .OnKeyPressedPressed
                .Subscribe(letter =>
                {
                    Debug.Assert(letter.Length == 1,
                        $"Something is wrong configured on KeyboardView, received {letter} and expected a letter of length 1");
                    guessLetter.Guess(letter[0]);
                });
        }
    }
}
