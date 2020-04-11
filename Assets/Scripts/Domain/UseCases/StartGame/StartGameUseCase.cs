using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CommonSignals;

namespace Domain.UseCases.StartGame
{
    public class StartGameUseCase : StartGame
    {
        private readonly GameService _gameService;
        private readonly EventDispatcherService _eventDispatcherService;
        private readonly GameRepository _gameRepository;
        private readonly ConfigurationGameRepository _configurationGameRepository;

        public StartGameUseCase(
            GameService gameService,
            GameRepository gameRepository,
            ConfigurationGameRepository configurationGameRepository,
            EventDispatcherService eventDispatcherService
        )
        {
            _gameService = gameService;
            _eventDispatcherService = eventDispatcherService;
            _gameRepository = gameRepository;
            _configurationGameRepository = configurationGameRepository;
        }

        public async Task Start()
        {
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(true));
            var newWord = await _gameService.StartNewGame();
            _gameRepository.RemainingLives = _configurationGameRepository.StartLives;
            _eventDispatcherService.Dispatch(new NewWordSignal(newWord.CurrentWord));
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(false));
        }
    }
}
