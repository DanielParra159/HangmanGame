using Domain.Model.Game;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using NSubstitute;
using NUnit.Framework;

namespace Domain.UseCases.StartGame.Tests
{
    public class StartGameUseCaseTest
    {
        private GameService _gameService;
        private EventDispatcherService _eventDispatcherService;
        private StartGameUseCase _startGameUseCase;

        [SetUp]
        public void SetUp()
        {
            _gameService = Substitute.For<GameService>();
            _gameService.StartNewGame().Returns(info => new Word("word"));
            _eventDispatcherService = Substitute.For<EventDispatcherService>();
            _startGameUseCase = new StartGameUseCase(_gameService, _eventDispatcherService);
        }

        [Test]
        public void WhenCallToStart_CallToGameServiceStartNewGame()
        {
            _startGameUseCase.Start();

            _gameService.Received().StartNewGame();
        }

        [Test]
        public void WhenCallToStart_DispatchSignalWithTheNewWord()
        {
            _startGameUseCase.Start();

            _eventDispatcherService
                .Received()
                .Dispatch(Arg.Is<NewWordSignal>(signal => signal.NewWord == "word"));
        }
        
        [Test]
        public void WhenCallToStart_DispatchSignalToUpdateTheLoadingScreen()
        {
            _startGameUseCase.Start();

            Received.InOrder(() =>
            {
                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => signal.IsVisible));

                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => !signal.IsVisible));
            });
        }
    }
}