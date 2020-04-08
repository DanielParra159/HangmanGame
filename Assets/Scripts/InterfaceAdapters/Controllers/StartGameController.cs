using Domain.UseCases.StartGame;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class StartGameController
    {
        private MainMenuViewModel _mainMenuViewModel;
        private StartGame _startGame;

        public StartGameController(MainMenuViewModel mainMenuViewModel, StartGame startGame)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _startGame = startGame;

            _mainMenuViewModel
                .StartGamePressed
                .Subscribe(_ => _startGame.Start());
        }

    }
}
