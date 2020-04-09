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
        public readonly bool Correct;

        public Guess(string currentWord, bool correct)
        {
            CurrentWord = currentWord;
            Correct = correct;
        }
    }
}
