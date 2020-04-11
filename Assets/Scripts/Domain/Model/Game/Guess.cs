namespace Domain.Model.Game
{
    public class Guess
    {
        public readonly Word UpdatedWord;
        public readonly bool IsCorrect;

        public Guess(Word updatedWord, bool isCorrect)
        {
            UpdatedWord = updatedWord;
            IsCorrect = isCorrect;
        }
    }
}
