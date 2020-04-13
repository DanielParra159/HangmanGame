using Domain.Services.EventDispatcher;

namespace Domain.UseCases.RestartGame
{
    public class RestartGameUseCase : IRestartGame
    {
        private readonly StartGame.IStartGame _startGame;
        private readonly IEventDispatcherService _eventDispatcherService;

        public RestartGameUseCase(StartGame.IStartGame startGame, IEventDispatcherService eventDispatcherService)
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
