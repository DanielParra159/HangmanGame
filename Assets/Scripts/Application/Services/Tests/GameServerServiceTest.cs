using Infrastructure.Web;
using NSubstitute;
using NUnit.Framework;

namespace Application.Services.Tests
{
    public class GameServerServiceTest
    {
        [Test]
        public async void WhenCallToStartNewGame_DoPostRequestWithTheCorrectData()
        {
            var apiRest = Substitute.For<APIRest>();
            const string expectedWord = "_____";
            var newGameResponse = new GameServerService.NewGameResponse{hangman = expectedWord};
            apiRest.Post<Request, GameServerService.NewGameResponse>("TODO: URL", Arg.Any<Request>()).Returns(newGameResponse);
            var gameServerService = new GameServerService(apiRest);

            var word = await gameServerService.StartNewGame();
            
            Assert.AreEqual(expectedWord, word.CurrentWord);
        }
    }
}
