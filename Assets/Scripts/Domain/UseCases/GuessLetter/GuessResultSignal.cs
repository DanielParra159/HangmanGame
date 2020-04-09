using Domain.Services.EventDispatcher;

namespace Domain.UseCases.GuessLetter
{
    public class GuessResultSignal : Signal
    {
        public readonly string CurrentWord;
        public readonly bool IsCorrect;

        public GuessResultSignal(string currentWord, bool isCorrect)
        {
            CurrentWord = currentWord;
            IsCorrect = isCorrect;
        }
    }
}