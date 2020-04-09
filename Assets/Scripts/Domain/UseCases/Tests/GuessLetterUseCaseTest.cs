using Domain.Model.Game;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CommonSignals;
using Domain.UseCases.GuessLetter;
using NSubstitute;
using NUnit.Framework;

namespace Domain.UseCases.Tests
{
    public class GuessLetterUseCaseTest
    {
        private GuessLetterUseCase _guessLetterUseCase;
        private EventDispatcherService _eventDispatcherService;
        private GameService _gameService;

        [SetUp]
        public void SetUp()
        {
            _gameService = Substitute.For<GameService>();
            _gameService.GuessLetter(Arg.Any<char>()).Returns(info => new Guess("", false));
            _eventDispatcherService = Substitute.For<EventDispatcherService>();
            _guessLetterUseCase = new GuessLetterUseCase(_gameService, _eventDispatcherService);
        }

        [Test]
        public void WhenCallToGuess_CallToGameServiceGuessLetter()
        {
            _guessLetterUseCase.Guess('A');

            _gameService.Received().GuessLetter('A');
        }

        [Test]
        public void WhenCallToStart_DispatchSignalToUpdateTheLoadingScreen()
        {
            _guessLetterUseCase.Guess('A');

            Received.InOrder(() =>
            {
                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => signal.IsVisible));

                _gameService.Received().GuessLetter(Arg.Any<char>());

                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => !signal.IsVisible));
            });
        }

        [Test]
        public void WhenCallToGuess_DispatchTheResult()
        {
            _gameService.GuessLetter('A').Returns(info => new Guess("___a", true));
            _guessLetterUseCase.Guess('A');

            _eventDispatcherService
                .Received()
                .Dispatch(Arg.Is<GuessResultSignal>
                    (
                        signal => signal.CurrentWord == "___a" && signal.IsCorrect
                    )
                );
        }
    }
}