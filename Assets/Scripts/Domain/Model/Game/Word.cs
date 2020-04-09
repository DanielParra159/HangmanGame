namespace Domain.Model.Game
{
    public class Word
    {
        public readonly string CurrentWord;

        public Word(string currentWord)
        {
            CurrentWord = currentWord;
        }
    }

    public class Guess
    {
        public readonly string CurrentWord;
        public readonly bool IsCorrect;

        public Guess(string currentWord, bool isCorrect)
        {
            CurrentWord = currentWord;
            IsCorrect = isCorrect;
        }
    }
}
