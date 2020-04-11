using Domain.Model.Game;
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
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            var gameRepository = Substitute.For<GameRepository>();
            var gameService = Substitute.For<GameService>();
            gameService.GetSolution().Returns(new Word("word"));
            gameRepository.Word.Returns(new Word("word"));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameService.Received().GetSolution();
        }

        [Test]
        public void WhenCallToCheckAndIsNotComplete_DoNotCallToServerToCheckIt()
        {
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            var gameRepository = Substitute.For<GameRepository>();
            var gameService = Substitute.For<GameService>();
            var word = new Word("wor_");
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
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            var gameRepository = Substitute.For<GameRepository>();
            var gameService = Substitute.For<GameService>();
            gameRepository.RemainingLives = 2;
            gameRepository.ClearReceivedCalls();
            var word = new Word("wor_");
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
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            var gameRepository = Substitute.For<GameRepository>();
            var gameService = Substitute.For<GameService>();
            gameRepository.RemainingLives = 2;
            var word = new Word("wor_");
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
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            var gameRepository = Substitute.For<GameRepository>();
            var gameService = Substitute.For<GameService>();
            gameRepository.RemainingLives = 1;
            var word = new Word("wor_");
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
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            var gameRepository = Substitute.For<GameRepository>();
            var gameService = Substitute.For<GameService>();
            gameService.GetSolution().Returns(info => new Word("word"));
            gameRepository.Word.Returns(new Word("word"));
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
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            var gameRepository = Substitute.For<GameRepository>();
            var gameService = Substitute.For<GameService>();
            gameService.GetSolution().Returns(info => new Word("wor_"));
            gameRepository.Word.Returns(new Word("word"));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            eventDispatcherService
                .DidNotReceive()
                .Dispatch(Arg.Any<WordCompletedSignal>());
        }
    }
}
