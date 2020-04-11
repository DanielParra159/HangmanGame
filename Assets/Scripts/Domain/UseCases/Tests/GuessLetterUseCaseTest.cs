using System;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CheckLastWordIsCompleted;
using Domain.UseCases.CommonSignals;
using Domain.UseCases.GuessLetter;
using NSubstitute;
using NUnit.Framework;

namespace Domain.UseCases.Tests
{
    [TestFixture]
    public class GuessLetterUseCaseTest
    {
        private GuessLetterUseCase _guessLetterUseCase;
        private EventDispatcherService _eventDispatcherService;
        private GameService _gameService;
        private CheckSolution _checkSolution;
        private GameRepository _gameRepository;

        [SetUp]
        public void SetUp()
        {
            _gameService = Substitute.For<GameService>();
            _checkSolution = Substitute.For<CheckSolution>();
            _gameRepository = Substitute.For<GameRepository>();
            _gameService.GuessLetter(Arg.Any<char>()).Returns(info =>
                new Tuple<Guess, Token>(new Guess(new Word(""), false), new Token("")));
            _eventDispatcherService = Substitute.For<EventDispatcherService>();
            _guessLetterUseCase =
                new GuessLetterUseCase(_checkSolution, _gameRepository, _gameService, _eventDispatcherService);
        }

        [Test]
        public async void WhenCallToGuess_CallToGameServiceGuessLetter()
        {
            await _guessLetterUseCase.Guess('A');

            await _gameService.Received().GuessLetter('A');
        }

        [Test]
        public void WhenCallToStart_DispatchSignalToUpdateTheLoadingScreen()
        {
            _guessLetterUseCase.Guess('A');

            Received.InOrder(() =>
            {
                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => signal.IsVisible));

                _gameService.Received().GuessLetter(Arg.Any<char>());

                _eventDispatcherService
                    .Received()
                    .Dispatch(Arg.Is<UpdateLoadingScreenSignal>(signal => !signal.IsVisible));
            });
        }

        [Test]
        public async void WhenCallToGuess_DispatchTheResult()
        {
            _gameService.GuessLetter('A').Returns(info =>
                new Tuple<Guess, Token>(new Guess(new Word("___a"), true), Arg.Any<Token>()));
            await _guessLetterUseCase.Guess('A');

            _eventDispatcherService
                .Received()
                .Dispatch(Arg.Is<GuessResultSignal>
                    (
                        signal => signal.CurrentWord == "___a" && signal.IsCorrect
                    )
                );
        }

        [Test]
        public async void WhenCallToGuess_UpdateGameRepository()
        {
            _gameService.GuessLetter('A').Returns(info =>
                new Tuple<Guess, Token>(new Guess(new Word("___a"), true), new Token("token")));

            await _guessLetterUseCase.Guess('A');

            _gameRepository.Received().Word = Arg.Is<Word>(word => word.Value == "___a");
            _gameRepository.Received().LastGuess =
                Arg.Is<Guess>(guess => guess.UpdatedWord.Value == "___a" && guess.IsCorrect);
            _gameRepository.Received().GameToken = Arg.Is<Token>(token => token.Value == "token");
        }


        [Test]
        public void WhenCallToGuess_CallToCheckIfTheLastWordIsCompleted()
        {
            _guessLetterUseCase.Guess('A');

            _checkSolution.Received().Check();
        }
    }
}
