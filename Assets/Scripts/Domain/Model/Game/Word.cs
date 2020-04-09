namespace Domain.Model.Game
{
    public class Word
    {
        public readonly string CurrentWord;

        public Word(string currentWord)
        {
            CurrentWord = currentWord;
        }

        public bool IsCompleted()
        {
            const string secretCharacter = "_";
            return !CurrentWord.Contains(secretCharacter);
        }

        protected bool Equals(Word other)
        {
            return CurrentWord == other.CurrentWord;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Word) obj);
        }

        public override int GetHashCode()
        {
            return (CurrentWord != null ? CurrentWord.GetHashCode() : 0);
        }
    }
}
