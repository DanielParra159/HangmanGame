namespace Domain.Model.Game
{
    public sealed class Word
    {
        public readonly string Value;

        public Word(string value)
        {
            Value = value;
        }

        public bool IsCompleted()
        {
            const string secretCharacter = "_";
            return !Value.Contains(secretCharacter);
        }

        private bool Equals(Word other)
        {
            return Value == other.Value;
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
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
