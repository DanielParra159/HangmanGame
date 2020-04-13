using System;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;
using Domain.UseCases.CommonSignals;

namespace Domain.UseCases.StartGame
{
    public class StartGameUseCase : IStartGame
    {
        private readonly IGameService _gameService;
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly IGameRepository _gameRepository;
        private readonly IConfigurationGameRepository _configurationGameRepository;

        public StartGameUseCase(
            IGameService gameService,
            IGameRepository gameRepository,
            IConfigurationGameRepository configurationGameRepository,
            IEventDispatcherService eventDispatcherService
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
            var (word, token) = await _gameService.StartNewGame();
            _gameRepository.RemainingLives = _configurationGameRepository.StartLives;
            _gameRepository.Word = word;
            _gameRepository.GameToken = token;
            _eventDispatcherService.Dispatch(new NewWordSignal(word.Value));
            _eventDispatcherService.Dispatch(new UpdateLoadingScreenSignal(false));
        }
    }
}
