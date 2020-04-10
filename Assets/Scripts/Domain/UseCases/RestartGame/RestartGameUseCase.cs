using Domain.Services.EventDispatcher;

namespace Domain.UseCases.RestartGame
{
    public class RestartGameUseCase : RestartGame
    {
        private readonly StartGame.StartGame _startGame;
        private readonly EventDispatcherService _eventDispatcherService;

        public RestartGameUseCase(StartGame.StartGame startGame, EventDispatcherService eventDispatcherService)
        {
            _startGame = startGame;
            _eventDispatcherService = eventDispatcherService;
        }

        public async void Restart()
        {
            await _startGame.Start();
            _eventDispatcherService.Dispatch(new RestartGameSignal());
        }
    }
}
