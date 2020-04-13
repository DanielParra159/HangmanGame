using Application.Services.EventDispatcher;
using Application.Services.Game;
using Application.Services.Parsers;
using Application.Services.Repositories;
using Application.Services.Web;
using Domain.UseCases.CheckLastWordIsCompleted;
using Domain.UseCases.GuessLetter;
using Domain.UseCases.RestartGame;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;
using InterfaceAdapters.Presenters;
using UnityEngine;
using Views;

namespace Installers
{
    public class MainMenuInstaller : MonoBehaviour
    {
        [SerializeField] private MainMenuView _mainMenuViewPrefab;
        [SerializeField] private InGameView _inGameViewPrefab;
        [SerializeField] private LoadingView _loadingViewPrefab;

        private void Start()
        {
            var mainMenuViewModel = new MainMenuViewModel();
            var inGameViewModel = new InGameViewModel();
            var loadingViewModel = new LoadingViewModel();

            InstantiateViews(mainMenuViewModel, inGameViewModel, loadingViewModel);

            // TODO: these services should be unique, instantiate it in a previous step
            var gameRepository = new GameRepository();
            var gameServerService = new GameServerService
            (
                new RestRestClientAdapter(new JsonUtilityAdapter()),
                gameRepository
            );
            var eventDispatcherService = new EventDispatcherService();
            var startGameUseCase =
                new StartGameUseCase(gameServerService, gameRepository, new ConfigurationGameRepository(),
                    eventDispatcherService);
            var startGameController = new StartGameController(mainMenuViewModel,
                startGameUseCase
            );
            var keyboardController = new KeyboardController(inGameViewModel,
                new GuessLetterUseCase(
                    new CheckSolutionUseCase(
                        gameServerService,
                        gameRepository,
                        eventDispatcherService
                    ),
                    gameRepository,
                    gameServerService,
                    eventDispatcherService
                )
            );
            var restartGameController =
                new RestartGameController(inGameViewModel,
                    new RestartGameUseCase(startGameUseCase, eventDispatcherService));

            var updateWordPresenter = new InGamePresenter(inGameViewModel, eventDispatcherService);
            var mainMenuPresenter = new MainMenuPresenter(mainMenuViewModel, eventDispatcherService);
            var loadingPresenter = new LoadingPresenter(loadingViewModel, eventDispatcherService);
        }

        private void InstantiateViews(
            MainMenuViewModel mainMenuViewModel,
            InGameViewModel inGameViewModel,
            LoadingViewModel loadingViewModel
        )
        {
            var mainMenuViewInstance = Instantiate(_mainMenuViewPrefab); // TODO: extract to a service
            mainMenuViewInstance.SetModel(mainMenuViewModel);

            var inGameViewInstance = Instantiate(_inGameViewPrefab); // TODO: extract to a service

            inGameViewInstance.SetModel(inGameViewModel);
            inGameViewInstance
                .GetComponentInChildren<KeyboardView>()
                .SetModel(inGameViewModel); // TODO: consider move this responsability to the parent view
            inGameViewInstance
                .GetComponentInChildren<GallowView>()
                .SetModel(inGameViewModel); // TODO: consider move this responsability to the parent view

            var loadingViewInstance = Instantiate(_loadingViewPrefab); // TODO: extract to a service
            loadingViewInstance.SetModel(loadingViewModel);
        }
    }
}
