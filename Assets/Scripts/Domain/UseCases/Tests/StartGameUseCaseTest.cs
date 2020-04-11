﻿using System;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CommonSignals;
using Domain.UseCases.StartGame;
using NSubstitute;
using NUnit.Framework;

namespace Domain.UseCases.Tests
{
    public class StartGameUseCaseTest
    {
        private GameService _gameService;
        private EventDispatcherService _eventDispatcherService;
        private StartGameUseCase _startGameUseCase;
        private GameRepository _gameRepository;
        private ConfigurationGameRepository _configurationGameRepository;

        [SetUp]
        public void SetUp()
        {
            _gameService = Substitute.For<GameService>();
            _gameRepository = Substitute.For<GameRepository>();
            _configurationGameRepository = Substitute.For<ConfigurationGameRepository>();
            _gameService.StartNewGame().Returns(info => new Tuple<Word, Token>(new Word("word"), new Token("token")));
            _eventDispatcherService = Substitute.For<EventDispatcherService>();
            _startGameUseCase = new StartGameUseCase(
                _gameService,
                _gameRepository,
                _configurationGameRepository,
                _eventDispatcherService
            );
        }

        [Test]
        public void WhenCallToStart_CallToGameServiceStartNewGame()
        {
            _startGameUseCase.Start();

            _gameService.Received().StartNewGame();
        }
        
        [Test]
        public async void WhenCallToStartNewGame_StoreWordAndToken()
        {
            await _startGameUseCase.Start();

            _gameRepository.Received().Word = Arg.Is<Word>(word => word.Value == "word");
            _gameRepository.Received().GameToken = Arg.Is<Token>(word => word.Value == "token");
        }

        [Test]
        public void WhenCallToStart_ResetCurrentLives()
        {
            _configurationGameRepository.StartLives.Returns(5);
            _startGameUseCase.Start();

            _gameRepository.Received().RemainingLives = 5;
        }

        [Test]
        public void WhenCallToStart_DispatchSignalWithTheNewWord()
        {
            _startGameUseCase.Start();

            _eventDispatcherService
                .Received()
                .Dispatch(Arg.Is<NewWordSignal>(signal => signal.NewWord == "word"));
        }

        [Test]
        public void WhenCallToStart_DispatchSignalToUpdateTheLoadingScreen()
        {
            _startGameUseCase.Start();

            Received.InOrder(() =>
            {
                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => signal.IsVisible));

                _gameService.Received().StartNewGame();

                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => !signal.IsVisible));
            });
        }
    }
}
