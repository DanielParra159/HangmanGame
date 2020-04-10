using System;
using Application.Services;
using Application.Services.EventDispatcher;
using Application.Services.Game;
using Application.Services.Parsers;
using Application.Services.Repositories;
using Application.Services.Web;
using Domain.Repositories;
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
        public MainMenuView MainMenuViewPrefab;
        public InGameView InGameViewPrefab;
        public LoadingView LoadingViewPrefab;

        private void Start()
        {
            var mainMenuViewInstance = Instantiate(MainMenuViewPrefab); // TODO: extract to a service
            var mainMenuViewModel = new MainMenuViewModel();
            mainMenuViewInstance.SetModel(mainMenuViewModel);

            var inGameViewInstance = Instantiate(InGameViewPrefab); // TODO: extract to a service
            var inGameViewModel = new InGameViewModel();
            inGameViewInstance.SetModel(inGameViewModel);
            inGameViewInstance
                .GetComponentInChildren<KeyboardView>()
                .SetModel(inGameViewModel); // TODO: consider move this responsability to the parent view

            var loadingViewInstance = Instantiate(LoadingViewPrefab); // TODO: extract to a service
            var loadingViewModel = new LoadingViewModel();
            loadingViewInstance.SetModel(loadingViewModel);

            // TODO: these services should be unique, instantiate it in a previous step
            var gameRepositoryImpl = new GameRepositoryImpl();
            var gameServerService = new GameServerService
            (
                new RestRestClientAdapter(new JsonUtilityAdapter()),
                gameRepositoryImpl
            );
            var eventDispatcherServiceImpl = new EventDispatcherServiceImpl();
            var startGameUseCase = new StartGameUseCase(gameServerService, eventDispatcherServiceImpl);
            var startGameController = new StartGameController(mainMenuViewModel,
                startGameUseCase
            );
            var keyboardController = new KeyboardController(inGameViewModel,
                new GuessLetterUseCase(
                    new CheckSolutionUseCase(
                        gameServerService,
                        gameRepositoryImpl,
                        eventDispatcherServiceImpl
                    ),
                    gameServerService,
                    eventDispatcherServiceImpl
                )
            );
            var restartGameController =
                new RestartGameController(inGameViewModel,
                    new RestartGameUseCase(startGameUseCase, eventDispatcherServiceImpl));

            var updateWordPresenter = new InGamePresenter(inGameViewModel, eventDispatcherServiceImpl);
            var mainMenuPresenter = new MainMenuPresenter(mainMenuViewModel, eventDispatcherServiceImpl);
            var loadingPresenter = new LoadingPresenter(loadingViewModel, eventDispatcherServiceImpl);
        }
    }
}
