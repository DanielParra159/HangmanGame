using System;
using Application.Services.Game;
using Domain.Configuration;
using Domain.Model.Game;
using Domain.Model.Tests.Factories;
using Domain.Repositories;
using Domain.Services.Web;
using NSubstitute;
using NUnit.Framework;

namespace Application.Services.Tests.Game
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
            _gameRepository.Word = WordFactory.GetWord;
            _gameRepository.GameToken = TokenFactory.GetToken;
            _restClient = Substitute.For<RestClient>();
            _gameServerService = new GameServerService(_restClient, _gameRepository);
        }

        [Test]
        public async void WhenCallToStartNewGame_DoPostRequestWithTheCorrectData()
        {
            const string expectedWord = "_____";
            const string expectedToken = "token";
            var newGameResponse = new GameServerService.NewGameResponse {hangman = expectedWord, token = expectedToken};
            _restClient.Post<Request, GameServerService.NewGameResponse>(EndPoints.NewGame, Arg.Any<Request>())
                .Returns(newGameResponse);


            var (word, token) = await _gameServerService.StartNewGame();

            Assert.AreEqual(expectedWord, word.Value);
            Assert.AreEqual(expectedToken, token.Value);
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
                    correct = true,
                    token = "token"
                });


            var (guess, token) = await _gameServerService.GuessLetter('a');

            Assert.AreEqual("____a", guess.UpdatedWord.Value);
            Assert.AreEqual(true, guess.IsCorrect);
            Assert.AreEqual("token", token.Value);
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

            var (word, token) = await _gameServerService.GetSolution();

            Assert.AreEqual("word", word.Value);
            Assert.AreEqual("token", token.Value);
        }
    }
}
