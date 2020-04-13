using Domain.UseCases.StartGame;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class StartGameController
    {
        public StartGameController(MainMenuViewModel mainMenuViewModel, IStartGame startGame)
        {
            mainMenuViewModel
                .OnStartGamePressed
                .Subscribe(_ => startGame.Start());
        }
    }
}
