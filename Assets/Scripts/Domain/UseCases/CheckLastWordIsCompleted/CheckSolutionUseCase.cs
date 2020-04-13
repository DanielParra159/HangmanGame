using System;
using Domain.Repositories;
using Domain.Services.EventDispatcher;
using Domain.Services.Game;

namespace Domain.UseCases.CheckLastWordIsCompleted
{
    public class CheckSolutionUseCase : ICheckSolution
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly IGameRepository _gameRepository;
        private readonly IGameService _gameService;

        public CheckSolutionUseCase(
            IGameService gameService, 
            IGameRepository gameRepository,
            IEventDispatcherService eventDispatcherService
            )
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
                if (_gameRepository.LastGuess.IsCorrect)
                {
                    return;
                }
                _gameRepository.RemainingLives -= 1;
                if (_gameRepository.RemainingLives == 0)
                {
                    _eventDispatcherService.Dispatch(new GameOverSignal());
                }

                return;
            }

            var (word, token) = await _gameService.GetSolution();
            _gameRepository.Word = word;
            _gameRepository.GameToken = token;
            if (word.Equals(currentWord))
            {
                _eventDispatcherService.Dispatch(new WordCompletedSignal());
            }
        }
    }
}
