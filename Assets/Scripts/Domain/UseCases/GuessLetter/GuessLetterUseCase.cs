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

        public void Guess(char letter)
        {
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(true));
            _gameService.GuessLetter(letter);
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(false));
        }
    }
}