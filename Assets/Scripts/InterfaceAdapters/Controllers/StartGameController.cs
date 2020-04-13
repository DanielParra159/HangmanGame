using Domain.UseCases.StartGame;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class StartGameController
    {
        private readonly MainMenuViewModel _mainMenuViewModel;
        private readonly IStartGame _startGame;

        public StartGameController(MainMenuViewModel mainMenuViewModel, IStartGame startGame)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _startGame = startGame;

            _mainMenuViewModel
                .OnStartGamePressed
                .Subscribe(_ => _startGame.Start());
        }
    }
}
