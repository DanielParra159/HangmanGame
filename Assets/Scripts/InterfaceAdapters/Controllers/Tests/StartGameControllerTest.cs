using Domain.UseCases.StartGame;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Controllers.Tests
{
    [TestFixture]
    public class StartGameControllerTest
    {
        private IStartGame _startGame;
        private MainMenuViewModel _mainMenuViewModel;
        private StartGameController _startGameController;

        [SetUp]
        public void SetUp()
        {
            _startGame = Substitute.For<IStartGame>();
            _mainMenuViewModel = new MainMenuViewModel();
            _startGameController = new StartGameController(_mainMenuViewModel, _startGame);
        }
        
        [Test]
        public void WhenReceiveCommandOnStartGamePressed_CallToStart()
        {
            _mainMenuViewModel.OnStartGamePressed.Execute();
            
            _startGame.Received().Start();
        }
    }
}
