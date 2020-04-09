using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CommonSignals;

namespace Domain.UseCases.GuessLetter
{
    public class GuessLetterUseCase : GuessLetter
    {
        private readonly EventDispatcherService _eventDispatcherService;
        private readonly GameService _gameService;

        public GuessLetterUseCase(GameService gameService, EventDispatcherService eventDispatcherService)
        {
            _gameService = gameService;
            _eventDispatcherService = eventDispatcherService;
        }

        public async void Guess(char letter)
        {
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(true));
            var guess = await _gameService.GuessLetter(letter);
            _eventDispatcherService.Dispatch(new GuessResultSignal(letter.ToString(), guess.CurrentWord, guess.IsCorrect));
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(false));
        }
    }
}