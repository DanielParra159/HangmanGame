using Domain.Repositories;
using Infrastructure.Web;
using NSubstitute;
using NUnit.Framework;

namespace Application.Services.Tests
{
    public class GameServerServiceTest
    {
        private GameRepository _gameRepository;
        private APIRest _apiRest;
        private GameServerService _gameServerService;

        [SetUp]
        public void SetUp()
        {
            _gameRepository = Substitute.For<GameRepository>();
            _apiRest = Substitute.For<APIRest>();
            _gameServerService = new GameServerService(_apiRest, _gameRepository);
        }
        [Test]
        public async void WhenCallToStartNewGame_DoPostRequestWithTheCorrectData()
        {
            const string expectedWord = "_____";
            var newGameResponse = new GameServerService.NewGameResponse{hangman = expectedWord};
            _apiRest.Post<Request, GameServerService.NewGameResponse>("TODO: URL", Arg.Any<Request>()).Returns(newGameResponse);
            

            var word = await _gameServerService.StartNewGame();
            
            Assert.AreEqual(expectedWord, word.CurrentWord);
        }
        
        [Test]
        public async void WhenCallToStartNewGame_StoreWord()
        {
            const string expectedWord = "_____";
            var newGameResponse = new GameServerService.NewGameResponse{hangman = expectedWord};
            _apiRest.Post<Request, GameServerService.NewGameResponse>("TODO: URL", Arg.Any<Request>()).Returns(newGameResponse);

            await _gameServerService.StartNewGame();

            _gameRepository.Received().Word = expectedWord;
        }
        
        [Test]
        public async void WhenCallToStartNewGame_StoreGameTokenId()
        {
            const string expectedToken = "token";
            var newGameResponse = new GameServerService.NewGameResponse{token = expectedToken};
            _apiRest.Post<Request, GameServerService.NewGameResponse>("TODO: URL", Arg.Any<Request>()).Returns(newGameResponse);

            await _gameServerService.StartNewGame();

            _gameRepository.Received().GameToken = expectedToken;
        }
    }
}
