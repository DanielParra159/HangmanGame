using System.Threading.Tasks;
using Domain.Model.Game;
using Domain.Services;
using Domain.Services.EventDispatcher;
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
        public void WhenCallToStart_NotifySignalWithTheNewWord()
        {
            _startGameUseCase.Start();

            _eventDispatcherService
                .Received()
                .Notify(Arg.Is<NewWordSignal>(signal => signal.NewWord == "word"));
        }
    }
}