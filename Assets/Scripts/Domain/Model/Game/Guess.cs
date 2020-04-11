namespace Domain.Model.Game
{
    public sealed class Guess
    {
        public readonly Word UpdatedWord;
        public readonly bool IsCorrect;

        public Guess(Word updatedWord, bool isCorrect)
        {
            UpdatedWord = updatedWord;
            IsCorrect = isCorrect;
        }

        private bool Equals(Guess other)
        {
            return Equals(UpdatedWord, other.UpdatedWord) && IsCorrect == other.IsCorrect;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Guess) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UpdatedWord != null ? UpdatedWord.GetHashCode() : 0) * 397) ^ IsCorrect.GetHashCode();
            }
        }
    }
}
