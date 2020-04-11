using Domain.Model.Game;

namespace Domain.Model.Tests.Factories
{
    public class TokenBuilder
    {
        private string _token = ""; // RAMDON

        public TokenBuilder WithValue(string value)
        {
            _token = value;
            return this;
        }

        private Token Build()
        {
            return new Token(_token);
        }

        public static implicit operator Token(TokenBuilder wordBuilder) => wordBuilder.Build();
    }

    public static class TokenFactory
    {
        public static TokenBuilder GetToken => new TokenBuilder();
    }
}
