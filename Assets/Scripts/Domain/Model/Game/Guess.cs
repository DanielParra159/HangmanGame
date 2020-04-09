namespace Domain.Model.Game
{
    public class Guess
    {
        public readonly Word CurrentWord;
        public readonly bool IsCorrect;

        public Guess(Word currentWord, bool isCorrect)
        {
            CurrentWord = currentWord;
            IsCorrect = isCorrect;
        }
    }
}