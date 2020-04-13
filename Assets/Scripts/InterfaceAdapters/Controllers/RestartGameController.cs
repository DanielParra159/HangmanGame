using Domain.UseCases.RestartGame;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class RestartGameController
    {
        public RestartGameController(InGameViewModel inGameViewModel, IRestartGame restartGame)
        {
            inGameViewModel.OnRestartGamePressed.Subscribe(_ => restartGame.Restart());
        }
    }
}
