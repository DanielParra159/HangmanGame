using System;
using System.Threading.Tasks;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CheckLastWordIsCompleted;
using Domain.UseCases.CommonSignals;

namespace Domain.UseCases.GuessLetter
{
    public class GuessLetterUseCase : IGuessLetter
    {
        private readonly ICheckSolution _checkSolution;
        private readonly IGameService _gameService;
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly IGameRepository _gameRepository;

        public GuessLetterUseCase(
            ICheckSolution checkSolution,
            IGameRepository gameRepository,
            IGameService gameService,
            IEventDispatcherService eventDispatcherService
        )
        {
            _checkSolution = checkSolution;
            _gameRepository = gameRepository;
            _gameService = gameService;
            _eventDispatcherService = eventDispatcherService;
        }

        public async Task Guess(char letter)
        {
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(true));
            var (guess, token) = await _gameService.GuessLetter(letter);

            _gameRepository.Word = guess.UpdatedWord;
            _gameRepository.GameToken = token;
            _gameRepository.LastGuess = guess;

            _eventDispatcherService.Dispatch(
                new GuessResultSignal
                (
                    letter.ToString(),
                    guess.UpdatedWord.Value,
                    guess.IsCorrect
                )
            );

            _checkSolution.Check();
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(false));
        }
    }
}
