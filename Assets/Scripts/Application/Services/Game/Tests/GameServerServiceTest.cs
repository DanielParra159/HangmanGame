using Domain.Configuration;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services.Web;
using NSubstitute;
using NUnit.Framework;

namespace Application.Services.Game.Tests
{
    public class GameServerServiceTest
    {
        private GameRepository _gameRepository;
        private RestClient _restClient;
        private GameServerService _gameServerService;

        [SetUp]
        public void SetUp()
        {
            _gameRepository = Substitute.For<GameRepository>();
            _restClient = Substitute.For<RestClient>();
            _gameServerService = new GameServerService(_restClient, _gameRepository);
        }

        [Test]
        public async void WhenCallToStartNewGame_DoPostRequestWithTheCorrectData()
        {
            const string expectedWord = "_____";
            var newGameResponse = new GameServerService.NewGameResponse {hangman = expectedWord};
            _restClient.Post<Request, GameServerService.NewGameResponse>(EndPoints.NewGame, Arg.Any<Request>())
                .Returns(newGameResponse);


            var word = await _gameServerService.StartNewGame();

            Assert.AreEqual(expectedWord, word.CurrentWord);
        }

        [Test]
        public async void WhenCallToStartNewGame_StoreWord()
        {
            const string expectedWord = "_____";
            var newGameResponse = new GameServerService.NewGameResponse {hangman = expectedWord};
            _restClient.Post<Request, GameServerService.NewGameResponse>(EndPoints.NewGame, Arg.Any<Request>())
                .Returns(newGameResponse);

            await _gameServerService.StartNewGame();
            _gameRepository.Received().Word = Arg.Is<Word>(word => word.CurrentWord == expectedWord);
        }

        [Test]
        public async void WhenCallToStartNewGame_StoreGameTokenId()
        {
            const string expectedToken = "token";
            var newGameResponse = new GameServerService.NewGameResponse {token = expectedToken};
            _restClient.Post<Request, GameServerService.NewGameResponse>(EndPoints.NewGame, Arg.Any<Request>())
                .Returns(newGameResponse);

            await _gameServerService.StartNewGame();

            _gameRepository.Received().GameToken = expectedToken;
        }

        [Test]
        public async void WhenCallToGuessLetter_DoPutRequestWithTheCorrectData()
        {
            _restClient
                .PutWithParametersOnUrl<GameServerService.GuessLetterRequest, GameServerService.GuessLetterResponse>(
                    EndPoints.GuessLetter, Arg.Any<GameServerService.GuessLetterRequest>())
                .Returns(info => new GameServerService.GuessLetterResponse
                {
                    hangman = "____" + ((GameServerService.GuessLetterRequest) info.Args()[1]).letter,
                    correct = true, token = "token"
                });


            var word = await _gameServerService.GuessLetter('a');

            Assert.AreEqual("____a", word.CurrentWord.CurrentWord);
            Assert.AreEqual(true, word.IsCorrect);
        }

        [Test]
        public async void WhenCallToGuessLetter_StoreTheUpdatedWord()
        {
            _restClient
                .PutWithParametersOnUrl<GameServerService.GuessLetterRequest, GameServerService.GuessLetterResponse>(
                    EndPoints.GuessLetter, Arg.Any<GameServerService.GuessLetterRequest>())
                .Returns(info => new GameServerService.GuessLetterResponse
                {
                    hangman = "____" + ((GameServerService.GuessLetterRequest) info.Args()[1]).letter,
                    correct = true, token = "token"
                });

            await _gameServerService.GuessLetter('a');

            _gameRepository.Received().Word = Arg.Is<Word>(word => word.CurrentWord == "____a");
        }

        [Test]
        public async void WhenCallToGuessLetter_StoreLastGuess()
        {
            _restClient
                .PutWithParametersOnUrl<GameServerService.GuessLetterRequest, GameServerService.GuessLetterResponse>(
                    EndPoints.GuessLetter, Arg.Any<GameServerService.GuessLetterRequest>())
                .Returns(info => new GameServerService.GuessLetterResponse
                {
                    hangman = "____" + ((GameServerService.GuessLetterRequest) info.Args()[1]).letter,
                    correct = true, token = "token"
                });

            await _gameServerService.GuessLetter('a');

            _gameRepository.Received().LastGuess =
                Arg.Is<Guess>(word => word.CurrentWord.CurrentWord == "____a" && word.IsCorrect);
        }

        [Test]
        public async void WhenCallToGuessLetter_RefreshStoredTokenId()
        {
            _restClient
                .PutWithParametersOnUrl<GameServerService.GuessLetterRequest, GameServerService.GuessLetterResponse>(
                    EndPoints.GuessLetter, Arg.Any<GameServerService.GuessLetterRequest>())
                .Returns(info => new GameServerService.GuessLetterResponse
                {
                    hangman = "____" + ((GameServerService.GuessLetterRequest) info.Args()[1]).letter,
                    correct = true,
                    token = "token"
                });

            await _gameServerService.GuessLetter('a');

            _gameRepository.Received().GameToken = "token";
        }

        [Test]
        public async void WhenCallToGetSolution_DoGetRequestToTheServer()
        {
            _restClient
                .Get<GameServerService.GetSolutionRequest, GameServerService.GetSolutionResponse>(
                    EndPoints.GetSolution, Arg.Any<GameServerService.GetSolutionRequest>())
                .Returns(info => new GameServerService.GetSolutionResponse
                {
                    solution = "word",
                    token = "token"
                });

            var word = await _gameServerService.GetSolution();

            Assert.AreEqual("word", word.CurrentWord);
            _gameRepository.Received().Word = Arg.Is<Word>(storedWord => storedWord.CurrentWord == "word");
            _gameRepository.Received().GameToken = "token";
        }
    }
}
