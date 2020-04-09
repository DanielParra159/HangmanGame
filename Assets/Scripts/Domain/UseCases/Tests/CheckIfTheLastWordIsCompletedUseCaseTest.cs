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
    public class CheckIfTheLastWordIsCompletedUseCaseTest
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
            gameRepository.Word.Returns(new Word("wor_"));
            var checkIfTheLastWordIsCompletedUseCase =
                new CheckSolutionUseCase(gameService, gameRepository, eventDispatcherService);

            checkIfTheLastWordIsCompletedUseCase.Check();

            gameService.DidNotReceive().GetSolution();
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