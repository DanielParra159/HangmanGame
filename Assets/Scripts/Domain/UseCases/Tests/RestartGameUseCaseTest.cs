using Domain.Services.EventDispatcher;
using Domain.UseCases.RestartGame;
using NSubstitute;
using NUnit.Framework;

namespace Domain.UseCases.Tests
{
    [TestFixture]
    public class RestartGameUseCaseTest
    {
        private StartGame.StartGame _startGame;
        private EventDispatcherService _eventDispatcherService;
        private RestartGameUseCase _restartGameUseCase;

        [SetUp]
        public void SetUo()
        {
            _startGame = Substitute.For<StartGame.StartGame>();
            _eventDispatcherService = Substitute.For<EventDispatcherService>();
            _restartGameUseCase = new RestartGameUseCase(_startGame, _eventDispatcherService);
        }
        
        [Test]
        public void WhenCallToRestart_CallToStartGame()
        {
            _restartGameUseCase.Restart();
            
            _startGame.Received().Start();
        }
        
        [Test]
        public void WhenCallToRestart_DispatchRestartGameSignal()
        {
            _restartGameUseCase.Restart();
            
            _eventDispatcherService
                .Received()
                .Dispatch(Arg.Any<RestartGameSignal>());
            
            
        }
    }
}
