using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;

namespace Domain.UseCases.CheckLastWordIsCompleted
{
    public class CheckSolutionUseCase : CheckSolution
    {
        private readonly EventDispatcherService _eventDispatcherService;
        private readonly GameRepository _gameRepository;
        private readonly GameService _gameService;

        public CheckSolutionUseCase(GameService gameService, GameRepository gameRepository,
            EventDispatcherService eventDispatcherService)
        {
            _gameService = gameService;
            _gameRepository = gameRepository;
            _eventDispatcherService = eventDispatcherService;
        }

        public async void Check()
        {
            var currentWord = _gameRepository.Word;
            if (!currentWord.IsCompleted())
            {
                return;
            }

            var solution = await _gameService.GetSolution();
            if (solution.Equals(currentWord))
            {
                _eventDispatcherService.Dispatch(new WordCompletedSignal());
            }
        }
    }
}