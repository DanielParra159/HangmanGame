using Domain.UseCases.StartGame;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Controllers.Tests
{
    public class StartGameControllerTest
    {
        private StartGame _startGame;
        private MainMenuViewModel _mainMenuViewModel;
        private StartGameController _startGameController;

        [SetUp]
        public void SetUp()
        {
            _startGame = Substitute.For<StartGame>();
            _mainMenuViewModel = new MainMenuViewModel();
            _startGameController = new StartGameController(_mainMenuViewModel, _startGame);
        }
        
        [Test]
        public void StartGameControllerTestSimplePasses()
        {
            _mainMenuViewModel.OnStartGamePressed.Execute();
            
            _startGame.Received().Start();
        }
    }
}
