using System;
using Application.Services;
using Application.Services.EventDispatcher;
using Application.Services.Parsers;
using Application.Services.Repositories;
using Application.Services.Web;
using Domain.Repositories;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;
using UnityEngine;
using Views;

namespace Installers
{
    public class MainMenuInstaller : MonoBehaviour
    {
        public MainMenuView MainMenuViewPrefab;

        private void Start()
        {
            var mainMenuViewInstance = Instantiate(MainMenuViewPrefab); // TODO: extract to a service
            var mainMenuViewModel = new MainMenuViewModel();
            mainMenuViewInstance.SetModel(mainMenuViewModel);

            // TODO: these services should be unique, instantiate it in a previous step
            var gameServerService = new GameServerService
            (
                new RestRestClientAdapter(new JsonUtilityAdapter()),
                new GameRepositoryImpl()
            );
            var startGameController = new StartGameController(mainMenuViewModel,
                new StartGameUseCase(gameServerService, new EventDispatcherServiceImpl())
            );
        }
    }
}