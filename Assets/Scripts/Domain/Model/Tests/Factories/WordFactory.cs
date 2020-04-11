using Domain.Model.Game;

namespace Domain.Model.Tests.Factories
{
    public class WordBuilder
    {
        private string _value = ""; // RAMDON

        public WordBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        private Word Build()
        {
            return new Word(_value);
        }

        public static implicit operator Word(WordBuilder wordBuilder) => wordBuilder.Build();
    }

    public static class WordFactory
    {
        public static WordBuilder GetWord => new WordBuilder();
    }
}
