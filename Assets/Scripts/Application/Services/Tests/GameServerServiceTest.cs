﻿using Application.Services.Web;
using Domain.Repositories;
using Domain.Services.Web;
using NSubstitute;
using NUnit.Framework;

namespace Application.Services.Tests
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
            var newGameResponse = new GameServerService.NewGameResponse{hangman = expectedWord};
            _restClient.Post<Request, GameServerService.NewGameResponse>("TODO: URL", Arg.Any<Request>()).Returns(newGameResponse);
            

            var word = await _gameServerService.StartNewGame();
            
            Assert.AreEqual(expectedWord, word.CurrentWord);
        }
        
        [Test]
        public async void WhenCallToStartNewGame_StoreWord()
        {
            const string expectedWord = "_____";
            var newGameResponse = new GameServerService.NewGameResponse{hangman = expectedWord};
            _restClient.Post<Request, GameServerService.NewGameResponse>("TODO: URL", Arg.Any<Request>()).Returns(newGameResponse);

            await _gameServerService.StartNewGame();

            _gameRepository.Received().Word = expectedWord;
        }
        
        [Test]
        public async void WhenCallToStartNewGame_StoreGameTokenId()
        {
            const string expectedToken = "token";
            var newGameResponse = new GameServerService.NewGameResponse{token = expectedToken};
            _restClient.Post<Request, GameServerService.NewGameResponse>("TODO: URL", Arg.Any<Request>()).Returns(newGameResponse);

            await _gameServerService.StartNewGame();

            _gameRepository.Received().GameToken = expectedToken;
        }
    }
}
