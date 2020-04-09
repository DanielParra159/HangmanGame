using Domain.UseCases.StartGame;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class StartGameController
    {
        private readonly MainMenuViewModel _mainMenuViewModel;
        private readonly StartGame _startGame;

        public StartGameController(MainMenuViewModel mainMenuViewModel, StartGame startGame)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _startGame = startGame;

            _mainMenuViewModel
                .OnStartGamePressed
                .Subscribe(_ => _startGame.Start());
        }

    }
}
