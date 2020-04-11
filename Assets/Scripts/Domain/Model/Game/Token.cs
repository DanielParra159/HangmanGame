namespace Domain.Model.Game
{
    public sealed class Token
    {
        public readonly string Value;

        public Token(string value)
        {
            Value = value;
        }

        private bool Equals(Token other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Token other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
