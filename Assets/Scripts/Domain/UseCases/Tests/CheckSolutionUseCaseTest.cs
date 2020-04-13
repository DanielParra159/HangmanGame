using System;
using Domain.Model.Game;
using Domain.Model.Tests.Factories;
using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CheckLastWordIsCompleted;
using NSubstitute;
using NUnit.Framework;

namespace Domain.UseCases.Tests
{
    [TestFixture]
    public class CheckSolutionUseCaseTest
    {
        [Test]
        public void WhenCallToCheckAndIsComplete_CallToServerToCheckIt()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            gameService.GetSolution().Returns(info =>
                new Tuple<Word, Token>
                (
                    WordFactory.GetWord,
                    TokenFactory.GetToken
                ));
            gameRepository.Word.Returns(WordFactory.GetWord);
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameService.Received().GetSolution();
        }

        [Test]
        public void WhenCallToCheckAndIsNotComplete_DoNotCallToServerToCheckIt()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            var word = WordFactory.GetWord.WithValue("wor_");
            gameRepository.Word.Returns(word);
            gameRepository.LastGuess.Returns(new Guess(word, true));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameService.DidNotReceive().GetSolution();
        }

        [Test]
        public void WhenCallToCheckAndLastGuessWasCorrect_DoNotRemoveLive()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            gameRepository.RemainingLives = 2;
            gameRepository.ClearReceivedCalls();
            var word = WordFactory.GetWord.WithValue("wor_");
            gameRepository.Word.Returns(word);
            gameRepository.LastGuess.Returns(new Guess(word, true));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameRepository.DidNotReceive().RemainingLives = Arg.Any<int>();
        }

        [Test]
        public void WhenCallToCheckAndLastGuessWasNotCorrect_RemoveOneLive()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            gameRepository.RemainingLives = 2;
            var word = WordFactory.GetWord.WithValue("wor_");
            gameRepository.Word.Returns(word);
            gameRepository.LastGuess.Returns(new Guess(word, false));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameRepository.Received().RemainingLives = 1;
            eventDispatcherService
                .DidNotReceive()
                .Dispatch(Arg.Any<GameOverSignal>());
        }

        [Test]
        public void WhenCallToCheckLastGuessWasNotCorrectAndLivesAreOne_RemoveOneLiveAndDispatchGameOverSignal()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            gameRepository.RemainingLives = 1;
            var word = WordFactory.GetWord.WithValue("wor_");
            gameRepository.Word.Returns(word);
            gameRepository.LastGuess.Returns(new Guess(word, false));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameRepository.Received().RemainingLives = 0;
            eventDispatcherService
                .Received()
                .Dispatch(Arg.Any<GameOverSignal>());
        }

        [Test]
        public void WhenCallToCheckAndIsComplete_DispatchASignal()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            gameService.GetSolution().Returns(info =>
                new Tuple<Word, Token>
                (
                    WordFactory.GetWord.WithValue("word"),
                    TokenFactory.GetToken.WithValue("token")
                ));
            gameRepository.Word.Returns(WordFactory.GetWord.WithValue("word"));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            eventDispatcherService
                .Received()
                .Dispatch(Arg.Any<WordCompletedSignal>());
        }

        [Test]
        public void WhenCallToCheckAndServerReturnsIsNotComplete_DoNotDispatchASignal()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            gameService.GetSolution().Returns(info =>
                new Tuple<Word, Token>
                (
                    WordFactory.GetWord.WithValue("wor_"),
                    TokenFactory.GetToken.WithValue("token")
                ));
            gameRepository.Word.Returns(WordFactory.GetWord.WithValue("word"));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            eventDispatcherService
                .DidNotReceive()
                .Dispatch(Arg.Any<WordCompletedSignal>());
        }

        [Test]
        public void WhenCallToCheck_UpdateGameRepo()
        {
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            var expectedWord = WordFactory.GetWord.WithValue("word");
            var expectedToken = TokenFactory.GetToken.WithValue("token");

            gameService.GetSolution().Returns(info =>
                new Tuple<Word, Token>
                (
                    expectedWord,
                    expectedToken
                ));
            gameRepository.Word.Returns(expectedWord);
            gameRepository.ClearReceivedCalls();
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameRepository.Received().Word = expectedWord;
            gameRepository.Received().GameToken = expectedToken;
        }
    }
}
