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
            _eventDispatcherService = Substitute.For<EventDispatcherService>();
            _guessLetterUseCase = new GuessLetterUseCase(_gameService, _eventDispatcherService);
        }
        [Test]
        public void WhenCallToGuess_CallToGameServiceGuessLetter()
        {
            _guessLetterUseCase.Guess("A");

            _gameService.Received().GuessLetter("A");
        }
        
        [Test]
        public void WhenCallToStart_DispatchSignalToUpdateTheLoadingScreen()
        {
            _guessLetterUseCase.Guess("A");

            Received.InOrder(() =>
            {
                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => signal.IsVisible));

                _gameService.Received().GuessLetter(Arg.Any<string>());
                
                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => !signal.IsVisible));
            });
        }
    }
}
