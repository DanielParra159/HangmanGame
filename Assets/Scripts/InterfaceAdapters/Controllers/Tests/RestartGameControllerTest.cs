using Domain.UseCases.RestartGame;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Controllers.Tests
{
    [TestFixture]
    public class RestartGameControllerTest
    {
        [Test]
        public void WhenReceiveCommandOnRestartGamePressed_CallToRestart()
        {
            var inGameViewModel = new InGameViewModel();
            var restartGame = Substitute.For<RestartGame>();

            var startGameController = new RestartGameController(inGameViewModel, restartGame);
            inGameViewModel.OnRestartGamePressed.Execute();

            restartGame.Received().Restart();
        }
    }
}
