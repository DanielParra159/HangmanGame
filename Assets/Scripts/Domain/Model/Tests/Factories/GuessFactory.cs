using Domain.Model.Game;

namespace Domain.Model.Tests.Factories
{
    public class GuessBuilder
    {
        private Word _word = WordFactory.GetWord;
        private bool _isCorrect = false;

        public GuessBuilder WithWord(string word)
        {
            _word = WordFactory.GetWord.WithValue(word);
            return this;
        }

        public GuessBuilder WithWord(Word word)
        {
            _word = word;
            return this;
        }

        public GuessBuilder IsCorrect(bool isCorrect)
        {
            _isCorrect = isCorrect;
            return this;
        }

        public static implicit operator Guess(GuessBuilder guessBuilder) => guessBuilder.Build();

        private Guess Build()
        {
            return new Guess(_word, _isCorrect);
        }
    }

    public static class GuessFactory
    {
        public static GuessBuilder GetGuess => new GuessBuilder();
    }
}
