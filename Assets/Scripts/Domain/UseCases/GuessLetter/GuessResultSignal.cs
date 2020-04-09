using Domain.Services.EventDispatcher;

namespace Domain.UseCases.GuessLetter
{
    public class GuessResultSignal : Signal
    {
        public readonly string Guess;
        public readonly string CurrentWord;
        public readonly bool IsCorrect;

        public GuessResultSignal(string guess, string currentWord, bool isCorrect)
        {
            Guess = guess;
            CurrentWord = currentWord;
            IsCorrect = isCorrect;
        }
    }
}