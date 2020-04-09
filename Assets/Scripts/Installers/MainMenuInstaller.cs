using System;
using Application.Services;
using Application.Services.EventDispatcher;
using Application.Services.Game;
using Application.Services.Parsers;
using Application.Services.Repositories;
using Application.Services.Web;
using Domain.Repositories;
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
            
            var loadingViewInstance = Instantiate(LoadingViewPrefab); // TODO: extract to a service
            var loadingViewModel = new LoadingViewModel();
            loadingViewInstance.SetModel(loadingViewModel);

            // TODO: these services should be unique, instantiate it in a previous step
            var gameServerService = new GameServerService
            (
                new RestRestClientAdapter(new JsonUtilityAdapter()),
                new GameRepositoryImpl()
            );
            var eventDispatcherServiceImpl = new EventDispatcherServiceImpl();
            var startGameController = new StartGameController(mainMenuViewModel,
                new StartGameUseCase(gameServerService, eventDispatcherServiceImpl)
            );

            var updateWordPresenter = new InGamePresenter(inGameViewModel, eventDispatcherServiceImpl);
            var mainMenuPresenter = new MainMenuPresenter(mainMenuViewModel, eventDispatcherServiceImpl);
            var loadingPresenter = new LoadingPresenter(loadingViewModel, eventDispatcherServiceImpl);

        }
    }
}