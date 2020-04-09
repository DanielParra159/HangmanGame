using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CheckLastWordIsCompleted;
using Domain.UseCases.CommonSignals;

namespace Domain.UseCases.GuessLetter
{
    public class GuessLetterUseCase : GuessLetter
    {
        private readonly CheckSolution _checkSolution;
        private readonly GameService _gameService;
        private readonly EventDispatcherService _eventDispatcherService;

        public GuessLetterUseCase(
            CheckSolution checkSolution,
            GameService gameService,
            EventDispatcherService eventDispatcherService
        )
        {
            _checkSolution = checkSolution;
            _gameService = gameService;
            _eventDispatcherService = eventDispatcherService;
        }

        public async void Guess(char letter)
        {
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(true));
            var guess = await _gameService.GuessLetter(letter);
            _eventDispatcherService.Dispatch(new GuessResultSignal(letter.ToString(), guess.CurrentWord.CurrentWord,
                guess.IsCorrect));

            _checkSolution.Check();
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(false));
        }
    }
}