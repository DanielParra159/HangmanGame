using Domain.Services;
using Domain.Services.EventDispatcher;

namespace Domain.UseCases.StartGame
{
    public class StartGameUseCase : StartGame
    {
        private readonly GameService _gameService;
        private readonly EventDispatcherService _eventDispatcherService;

        public StartGameUseCase(GameService gameService, EventDispatcherService eventDispatcherService)
        {
            _gameService = gameService;
            _eventDispatcherService = eventDispatcherService;
        }

        public async void Start()
        {
            var newWord = await _gameService.StartNewGame();
            _eventDispatcherService.Dispatch(new NewWordSignal(newWord.CurrentWord));
        }
    }
}